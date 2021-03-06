﻿using bike.Models;
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
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace bike.Services
{
    public class ServoDriveService : INotifyPropertyChanged
    {

        private readonly DataTransport<Packet> dataTransport;
        private readonly ISettings settings;
        private readonly SqliteConnection connection;
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

        public event EventHandler<EventArg<(Fault.Kind Kind, bool IsFinished)>> FaultOccured;


        public ServoDriveService(DataTransport<Packet> dataTransport, ISettings settings, SqliteConnection connection)
        {
            this.dataTransport = dataTransport;
            this.settings = settings;
            this.connection = connection;

            _ = Task.Run(TryOpenDataTransport);
            this.dataTransport.IsOpenChanged += (sender, e) =>
            {
                {
                    if (dataTransport.IsOpen)
                    {
                        dataTransport.Channels.First().DataReceived += Item_DataReceived;
                        var packetEncodings = ((PacketCodec)dataTransport.Channels.First().Codec).AncestorPacketEncodings;
                        packetEncodings[typeof(BatteryConfiguration)].DecodeFinished += (o, e1) =>
                        {
                            BatteryConfiguration = (BatteryConfiguration)e1.Packet;
                            OnPropertyChanged(nameof(BatteryConfiguration));
                            BatteryConfigurationChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(BatteryOutput)].DecodeFinished += (o, e1) =>
                        {
                            BatteryOutput = (BatteryOutput)e1.Packet;
                            OnPropertyChanged(nameof(BatteryOutput));
                            BatteryOutputChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(CoreConfiguration)].DecodeFinished += (o, e1) =>
                        {
                            CoreConfiguration = (CoreConfiguration)e1.Packet;
                            OnPropertyChanged(nameof(CoreConfiguration));
                            CoreConfigurationChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(CoreSituation)].DecodeFinished += (o, e1) =>
                        {
                            Core = (CoreSituation)e1.Packet;
                            OnPropertyChanged(nameof(Core));
                            CoreSituationChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(Fault)].DecodeFinished += (o, e1) =>
                        {
                            Fault = (Fault)e1.Packet;
                            OnPropertyChanged(nameof(Fault));
                            FaultChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(LightSetting)].DecodeFinished += (o, e1) =>
                        {
                            settings.Set(nameof(LightSetting), (LightSetting)e1.Packet);
                            OnPropertyChanged(nameof(LightSetting));
                            LightSettingChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(LightState)].DecodeFinished += (o, e1) =>
                        {
                            LightState = (LightState)e1.Packet;
                            OnPropertyChanged(nameof(LightState));
                            LightStateChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(PedalConfiguration)].DecodeFinished += (o, e1) =>
                        {
                            PedalConfiguration = (PedalConfiguration)e1.Packet;
                            OnPropertyChanged(nameof(PedalConfiguration));
                            PedalConfigurationChanged?.Invoke(o, e1);
                        };
                        packetEncodings[typeof(PedalSetting)].DecodeFinished += (o, e1) =>
                        {
                            settings.Set(nameof(PedalSetting), (PedalSetting)e1.Packet);
                            OnPropertyChanged(nameof(PedalSetting));
                            PedalSettingChanged?.Invoke(o, e1);
                        }; 
                        packetEncodings[typeof(ServoInput)].DecodeFinished += (o, e1) =>
                        {
                            ServoInput = (ServoInput)e1.Packet;
                            OnPropertyChanged(nameof(ServoInput));
                            ServoInputChanged?.Invoke(o, e1);
                        }; 
                        packetEncodings[typeof(ServoOutput)].DecodeFinished += (o, e1) =>
                        {
                            ServoOutput = (ServoOutput)e1.Packet;
                            OnPropertyChanged(nameof(ServoOutput));
                            ServoOutputChanged?.Invoke(o, e1);
                        }; 
                        packetEncodings[typeof(ThrottleConfiguration)].DecodeFinished += (o, e1) =>
                        {
                            ThrottleConfiguration = (ThrottleConfiguration)e1.Packet;
                            OnPropertyChanged(nameof(ThrottleConfiguration));
                            ThrottleConfigurationChanged?.Invoke(o, e1);
                        }; 
                        packetEncodings[typeof(ThrottleSetting)].DecodeFinished += (o, e1) =>
                        {
                            settings.Set(nameof(ThrottleSetting), (ThrottleSetting)e1.Packet);
                            //ThrottleSetting = (ThrottleSetting)e.Packet;
                            OnPropertyChanged(nameof(ThrottleSetting));
                            ThrottleSettingChanged?.Invoke(o, e1);
                        }; 

                        RefreshConfiguration();
                    }
                    IsOpenChanged?.Invoke(this, e);
                }
            };
        }
        public void RefreshConfiguration()
        {
            RefreshBatteryConfiguration();
            RefreshCoreConfiguration();
            RefreshPedalConfiguration();
            RefreshThrottleConfiguration();
            RefreshFault();
        }

        private async Task TryOpenDataTransport()
        {
            while (true)
            {
                try
                {

                    if (dataTransport.CanOpen)
                        dataTransport.Open();
                    await Task.Delay(3000);
                }
                catch
                {
                    await Task.Delay(4000);
                }
            }
        }

        private void Item_DataReceived(object sender, DataReceivedEventArg<Packet> e)
        {

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


        public bool IsOpen => dataTransport.IsOpen;

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
                OnPropertyChanged(nameof(LightSetting));
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
                OnPropertyChanged(nameof(PedalSetting));
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
                OnPropertyChanged(nameof(ThrottleSetting));
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
            var old = Fault;
            if (value.OverCurrent && !old.OverCurrent)
                OnFaultOccured(Fault.Kind.OverCurrent, false);
            if (!value.OverCurrent && old.OverCurrent)
                OnFaultOccured(Fault.Kind.OverCurrent, true);
            if (value.OverTemprature && !old.OverTemprature)
                OnFaultOccured(Fault.Kind.OverTemprature, false);
            if (!value.OverTemprature && old.OverTemprature)
                OnFaultOccured(Fault.Kind.OverTemprature, true);
            if (value.PedalSensor && !old.PedalSensor)
                OnFaultOccured(Fault.Kind.PedalSensor, false);
            if (!value.PedalSensor && old.PedalSensor)
                OnFaultOccured(Fault.Kind.PedalSensor, true);
            if (value.Throttle && !old.Throttle)
                OnFaultOccured(Fault.Kind.Throttle, false);
            if (!value.Throttle && old.Throttle)
                OnFaultOccured(Fault.Kind.Throttle, true);
            if (value.OverVoltage && !old.OverVoltage)
                OnFaultOccured(Fault.Kind.OverVoltage, false);
            if (!value.OverVoltage && old.OverVoltage)
                OnFaultOccured(Fault.Kind.OverVoltage, true);
            if (value.UnderVoltage && !old.UnderVoltage)
                OnFaultOccured(Fault.Kind.UnderVoltage, false);
            if (!value.UnderVoltage && old.UnderVoltage)
                OnFaultOccured(Fault.Kind.UnderVoltage, true);
            if (value.Motor && !old.Motor)
                OnFaultOccured(Fault.Kind.Motor, false);
            if (!value.Motor && old.Motor)
                OnFaultOccured(Fault.Kind.Motor, true);
            if (value.Drive && !old.Drive)
                OnFaultOccured(Fault.Kind.Drive, false);
            if (!value.Drive && old.Drive)
                OnFaultOccured(Fault.Kind.Drive, true);


        }
        protected virtual void OnFaultOccured(Fault.Kind kind, bool isFinished)
        {
            if (isFinished)
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
            else
                connection.InsertAsync(new Diagnostic { FaultTypeId = (int)Fault.Kind.Drive, StartTime = DateTime.Now });

            FaultOccured?.Invoke(this, new EventArg<(Fault.Kind kind, bool isFinished)>((kind, isFinished)));
        }


    }
}
