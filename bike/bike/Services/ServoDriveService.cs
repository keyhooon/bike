using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using bike.Models;
using Device.Communication.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using Shiny.Caching;
using Shiny.Integrations.Sqlite;
using Shiny.Logging;
using Shiny.Models;
using Shiny.Settings;
using SQLite;

namespace bike.Services
{
    public class ServoDriveService : INotifyPropertyChanged
    {
        private readonly DataTransport<Packet> dataTransport;
        private readonly ISettings settings;
        private readonly SqliteConnection connection;
        private readonly ICache cache;
        private readonly ObservableCollection<Diagnostic> currentDiagnostic;
        public event EventHandler IsOpenChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> BatteryConfigurationChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> BatteryOutputChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> CoreConfigurationChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> CoreSituationChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> FaultChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> LightSettingChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> LightStateChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> PedalConfigurationChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> PedalSettingChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> ServoInputChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> ServoOutputChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> ThrottleConfigurationChanged;
        public event EventHandler<EncodingOperationFinishedEventArgs> ThrottleSettingChanged;


        public ServoDriveService(DataTransport<Packet> dataTransport, ISettings settings, SqliteConnection connection, ICache cache)
        {
            this.dataTransport = dataTransport;
            this.settings = settings;
            this.connection = connection;
            this.cache = cache;

            _ = Task.Run(TryOpenDataTransport);
            this.dataTransport.IsOpenChanged += (sender, e) =>
            {
                {
                    if (dataTransport.IsOpen)
                    {
                        dataTransport.Channels.First().DataReceived += Item_DataReceived;
                        var packetEncodings = ((PacketCodec)dataTransport.Channels.First().Codec).AncestorPacketEncodings;
                        packetEncodings[typeof(BatteryConfiguration)].DecodeFinished += (sender, e) =>
                        {
                            BatteryConfiguration = (BatteryConfiguration)e.Packet;
                            OnPropertyChanged(nameof(BatteryConfiguration));
                            BatteryConfigurationChanged?.Invoke(sender, e);
                        };
                        packetEncodings[typeof(BatteryOutput)].DecodeFinished += (sender, e) =>
                        {
                            BatteryOutput = (BatteryOutput)e.Packet;
                            OnPropertyChanged(nameof(BatteryOutput));
                            BatteryOutputChanged?.Invoke(sender, e);
                        };
                        packetEncodings[typeof(CoreConfiguration)].DecodeFinished += (sender, e) =>
                        {
                            CoreConfiguration = (CoreConfiguration)e.Packet;
                            OnPropertyChanged(nameof(CoreConfiguration));
                            CoreConfigurationChanged?.Invoke(sender, e);
                        };
                        packetEncodings[typeof(CoreSituation)].DecodeFinished += (sender, e) =>
                        {
                            Core = (CoreSituation)e.Packet;
                            OnPropertyChanged(nameof(CoreSituation));
                            CoreSituationChanged?.Invoke(sender, e);
                        }; 
                        packetEncodings[typeof(Fault)].DecodeFinished += (sender, e) =>
                        {
                            Fault = (Fault)e.Packet;
                            OnPropertyChanged(nameof(Fault));
                            FaultChanged?.Invoke(sender, e);
                        }; 
                        packetEncodings[typeof(LightSetting)].DecodeFinished += (sender, e) =>
                        {
                            LightSetting = (LightSetting)e.Packet;
                            OnPropertyChanged(nameof(LightSetting));
                            LightSettingChanged?.Invoke(sender, e);
                        }; 
                        packetEncodings[typeof(LightState)].DecodeFinished += (sender, e) =>
                        {
                            LightState = (LightState)e.Packet;
                            OnPropertyChanged(nameof(LightState));
                            LightStateChanged?.Invoke(sender, e);
                        }; 
                        packetEncodings[typeof(PedalConfiguration)].DecodeFinished += (sender, e) =>
                        {
                            PedalConfiguration = (PedalConfiguration)e.Packet;
                            OnPropertyChanged(nameof(PedalConfiguration));
                            PedalConfigurationChanged?.Invoke(sender, e);
                        }; 
                        packetEncodings[typeof(PedalSetting)].DecodeFinished += (sender, e) =>
                        {
                            PedalSetting = (PedalSetting)e.Packet;
                            OnPropertyChanged(nameof(PedalSetting));
                            PedalSettingChanged?.Invoke(sender, e);
                        }; ;
                        packetEncodings[typeof(ServoInput)].DecodeFinished += (sender, e) =>
                        {
                            ServoInput = (ServoInput)e.Packet;
                            OnPropertyChanged(nameof(ServoInput));
                            ServoInputChanged?.Invoke(sender, e);
                        }; ;
                        packetEncodings[typeof(ServoOutput)].DecodeFinished += (sender, e) =>
                        {
                            ServoOutput = (ServoOutput)e.Packet;
                            OnPropertyChanged(nameof(ServoOutput));
                            ServoOutputChanged?.Invoke(sender, e);
                        }; ;
                        packetEncodings[typeof(ThrottleConfiguration)].DecodeFinished += (sender, e) =>
                        {
                            ThrottleConfiguration = (ThrottleConfiguration)e.Packet;
                            OnPropertyChanged(nameof(ThrottleConfiguration));
                            ThrottleConfigurationChanged?.Invoke(sender, e);
                        }; ;
                        packetEncodings[typeof(ThrottleSetting)].DecodeFinished += (sender, e) =>
                        {
                            ThrottleSetting = (ThrottleSetting)e.Packet;
                            OnPropertyChanged(nameof(ThrottleSetting));
                            ThrottleSettingChanged?.Invoke(sender, e);
                        }; ;

                        RefreshBatteryConfiguration();
                        RefreshCoreConfiguration();
                        RefreshPedalConfiguration();
                        RefreshThrottleConfiguration();
                        RefreshFault();
                    }
                    else
                    {
                        _ = Task.Run(TryOpenDataTransport);
                    }
                    IsOpenChanged?.Invoke(this, e);
                }
            };
        }



        private async Task TryOpenDataTransport()
        {
            while (!dataTransport.IsOpen)
            {
                try
                {
                    await Task.Delay(1000);
                    if (dataTransport.CanOpen)
                        dataTransport.Open();
                }
                catch
                {


                }
            }
        }

        private void Item_DataReceived(object sender, DataReceivedEventArg<Packet> e)
        {
            //switch (e.Data.DescendantPacket)
            //{
            //    case Data data:
            //        switch (data.DescendantPacket)
            //        {
            //            case BatteryConfiguration batteryConfiguration:
            //                BatteryConfiguration = batteryConfiguration;
            //                OnPropertyChanged(nameof(BatteryConfiguration));
            //                break;
            //            case BatteryOutput batteryOutput:
            //                BatteryOutput = batteryOutput;
            //                OnPropertyChanged(nameof(BatteryOutput));
            //                break;
            //            case CoreConfiguration coreConfiguration:
            //                CoreConfiguration = coreConfiguration;
            //                OnPropertyChanged(nameof(CoreConfiguration));
            //                break;
            //            case CoreSituation coreSituation:
            //                Core = coreSituation;
            //                OnPropertyChanged(nameof(Core));
            //                break;
            //            case LightSetting lightSetting:
            //                LightSetting = lightSetting;
            //                OnPropertyChanged(nameof(LightSetting));
            //                break;
            //            case Fault fault:
            //                Fault = fault;
            //                OnPropertyChanged(nameof(Fault));
            //                break;
            //            case LightState lightState:
            //                LightState = lightState;
            //                OnPropertyChanged(nameof(LightState));
            //                break;
            //            case PedalConfiguration pedalConfiguration:
            //                PedalConfiguration = pedalConfiguration;
            //                OnPropertyChanged(nameof(PedalConfiguration));
            //                break;
            //            case PedalSetting pedalSetting:
            //                PedalSetting = pedalSetting;
            //                OnPropertyChanged(nameof(PedalSetting));
            //                break;
            //            case ServoInput servoInput:
            //                ServoInput = servoInput;
            //                OnPropertyChanged(nameof(ServoInput));
            //                break;
            //            case ServoOutput servoOutput:
            //                ServoOutput = servoOutput;
            //                OnPropertyChanged(nameof(ServoOutput));
            //                break;
            //            case ThrottleConfiguration throttleConfiguration:
            //                ThrottleConfiguration = throttleConfiguration;
            //                OnPropertyChanged(nameof(ThrottleConfiguration));
            //                break;
            //            case ThrottleSetting throttleSetting:
            //                ThrottleSetting = throttleSetting;
            //                OnPropertyChanged(nameof(ThrottleSetting));
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //    case Command command:
            //        switch (command.DescendantPacket)
            //        {
            //            case CruiseCommand cruiseCommand:
            //                break;
            //            case LightCommand lightCommand:
            //                break;
            //            case ReadCommand readCommand:
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //}

        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void RefreshBatteryConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = BatteryConfiguration.Encoding.Id } } });

        public void RefreshCoreConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = CoreConfiguration.Encoding.Id } } });

        public void RefreshPedalConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = PedalConfiguration.Encoding.Id } } });

        public void RefreshThrottleConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = ThrottleConfiguration.Encoding.Id } } });

        public void RefreshFault() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = Fault.Encoding.Id } } });


        public bool IsOpen { get => dataTransport.IsOpen; }

        public BatteryConfiguration BatteryConfiguration { get; private set; }

        public CoreConfiguration CoreConfiguration { get; private set; }

        public PedalConfiguration PedalConfiguration { get; private set; }

        public ThrottleConfiguration ThrottleConfiguration { get; private set; }

        public LightSetting LightSetting
        {
            get => settings.Get(nameof(LightSetting), new LightSetting());
            set
            {
                if (value == LightSetting)
                    return;
                settings.Set(nameof(LightSetting), value);
                dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = value } });
            }
        }

        public PedalSetting PedalSetting
        {
            get => settings.Get(nameof(PedalSetting), new PedalSetting());
            set
            {
                if (value == PedalSetting)
                    return;
                settings.Set(nameof(PedalSetting), value);
                dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = value } });
            }
        }

        public ThrottleSetting ThrottleSetting
        {
            get => settings.Get(nameof(ThrottleSetting), new ThrottleSetting());
            set
            {
                if (value == ThrottleSetting)
                    return;
                settings.Set(nameof(ThrottleSetting), value);
                dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = value } });
            }
        }

        public BatteryOutput BatteryOutput { get; private set; }

        public CoreSituation Core { get; private set; }

        public Fault Fault
        {
            get => settings.Get(nameof(Fault), new Fault());
            private set
            {
                if (value == Fault)
                    return;
                CheckFault(value);
                settings.Set(nameof(Fault), value);
            }
        }


        public ObservableCollection<Diagnostic> CurrentDiagnostic
        {
            get => currentDiagnostic; 
        }

        public LightState LightState { get; private set; }

        public ServoInput ServoInput { get; private set; }

        public ServoOutput ServoOutput { get; private set; }

        public void ActiveCruise() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new CruiseCommand() { IsOn = true } } });

        public void DeactiveCruise() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new CruiseCommand() { IsOn = false } } });

        public void ActiveLight(int lightId) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new LightCommand() { IsOn = true, LightId = (byte)lightId } } });

        public void DeactiveLight(int lightId) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new LightCommand() { IsOn = false, LightId = (byte)lightId } } });

        private void CheckFault(Fault value)
        {
            if (value.OverCurrent && !Fault.OverCurrent)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.OverCurrent, StartTime = DateTime.Now });
            if (!value.OverCurrent && Fault.OverCurrent)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.OverCurrent && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }
            if (value.OverTemprature && !Fault.OverTemprature)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.OverTemprature, StartTime = DateTime.Now });
            if (!value.OverTemprature && Fault.OverTemprature)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.OverTemprature && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }
            if (value.PedalSensor && !Fault.PedalSensor)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.PedalSensor, StartTime = DateTime.Now });
            if (!value.PedalSensor && Fault.PedalSensor)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.PedalSensor && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }
            if (value.Throttle && !Fault.Throttle)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.Throttle, StartTime = DateTime.Now });
            if (!value.Throttle && Fault.Throttle)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.Throttle && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }
            if (value.OverVoltage && !Fault.OverVoltage)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.OverVoltage, StartTime = DateTime.Now });
            if (!value.OverVoltage && Fault.OverVoltage)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.OverVoltage && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }
            if (value.UnderVoltage && !Fault.UnderVoltage)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.UnderVoltage, StartTime = DateTime.Now });
            if (!value.UnderVoltage && Fault.UnderVoltage)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.UnderVoltage && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }
            if (value.Motor && !Fault.Motor)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.Motor, StartTime = DateTime.Now });
            if (!value.Motor && Fault.Motor)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.Motor && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }
            if (value.Drive && !Fault.Drive)
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.Drive, StartTime = DateTime.Now });
            if (!value.Drive && Fault.Drive)
            {
                Task.Run(async () =>
                {
                    var v = await connection.Diagnostics.FirstOrDefaultAsync((o) => o.FaultTypeId == (int)Fault.Kind.Drive && o.StopTime == null);
                    if (v != null)
                    {
                        v.StopTime = DateTime.Now;
                        await connection.UpdateAsync(v);
                    }
                });
            }

        }


    }
}
