using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Device.Communication.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Transport;
using Shiny.Settings;

namespace Device
{
    public class ServoDriveService : INotifyPropertyChanged
    {
        private readonly DataTransport<Packet> dataTransport;
        private readonly ISettings settings;

        public event EventHandler IsOpenChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public ServoDriveService(DataTransport<Packet> dataTransport, ISettings settings)
        {
            this.dataTransport = dataTransport;
            this.settings = settings;
            this.dataTransport.IsOpenChanged += (sender, e) => OnIsOpenChanged(e);
            ((INotifyCollectionChanged)this.dataTransport.Channels).CollectionChanged += ServoDriveService_CollectionChanged;
        }


        public virtual void Open()
        {
            dataTransport.Open();
        }

        public virtual void Close()
        {
            dataTransport.Close();
        }


        private void ServoDriveService_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (Channel<Packet> item in e.NewItems)
                {
                    item.DataReceived += Item_DataReceived;
                }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (Channel<Packet> item in e.OldItems)
                {
                    item.DataReceived -= Item_DataReceived;
                }
        }

        private void Item_DataReceived(object sender, DataReceivedEventArg<Packet> e)
        {
            switch (e.Data.DescendantPacket)
            {
                case Data data:
                    switch (data.DescendantPacket)
                    {
                        case BatteryConfiguration batteryConfiguration:
                            BatteryConfiguration = batteryConfiguration;
                            
                            break;
                        case BatteryOutput batteryOutput:
                            BatteryOutput = batteryOutput;
                            break;
                        case CoreConfiguration coreConfiguration:
                            CoreConfiguration = coreConfiguration;
                            break;
                        case CoreSituation coreSituation:
                            Core = coreSituation;
                            break;
                        case LightSetting lightSetting:
                            LightSetting = lightSetting;
                            break;
                        case LightState lightState:
                            LightState = lightState;
                            break;
                        case PedalConfiguration pedalConfiguration:
                            PedalConfiguration = pedalConfiguration;
                            break;
                        case PedalSetting pedalSetting:
                            PedalSetting = pedalSetting;
                            break;
                        case ServoInput servoInput:
                            ServoInput = servoInput;
                            break;
                        case ServoOutput servoOutput:
                            ServoOutput = servoOutput;
                            break;
                        case ThrottleConfiguration throttleConfiguration:
                            ThrottleConfiguration = throttleConfiguration;
                            break;
                        case ThrottleSetting throttleSetting:
                            ThrottleSetting = throttleSetting;
                            break;
                        default:
                            break;
                    }
                    break;
                case Command command:
                    switch (command.DescendantPacket)
                    {
                        case CruiseCommand cruiseCommand:
                            break;
                        case LightCommand lightCommand:
                            break;
                        case ReadCommand readCommand:
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }

        protected virtual void OnIsOpenChanged(EventArgs e)
        {
            if (dataTransport.IsOpen)
            {
                RefreshBatteryConfiguration();
                RefreshCoreConfiguration();
                RefreshPedalConfiguration();
                RefreshThrottleConfiguration();

                SendSetting(settings.Get(nameof(LightSetting), new LightSetting() { Light1 = LightSetting.LightVolume.Low, Light2 = LightSetting.LightVolume.Low, Light3 = LightSetting.LightVolume.Low, Light4 = LightSetting.LightVolume.Low }));
                SendSetting(settings.Get(nameof(PedalSetting), new PedalSetting() { ActivationTime = PedalSetting.PedalActivationTimeType.NormalSensitive, AssistLevel = PedalSetting.PedalAssistLevelType.ThirtySevenPointFive}));
                SendSetting(settings.Get(nameof(ThrottleSetting), new ThrottleSetting() { ActivityType = ThrottleSetting.ThrottleActivityType.Normal }));

            }
            IsOpenChanged(this, e);
        }
        protected virtual void OnProeprtyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }


        public void SendSetting(LightSetting.LightVolume light1, LightSetting.LightVolume light2, LightSetting.LightVolume light3, LightSetting.LightVolume light4) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = new LightSetting() { Light1 = light1, Light2 = light2, Light3 = light3, Light4 = light4 } } });
        
        public void SendSetting(LightSetting lightSetting) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = lightSetting } });

        public void SendSetting(PedalSetting.PedalAssistLevelType assistLevel, PedalSetting.PedalActivationTimeType activationTime) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = new PedalSetting() { AssistLevel = assistLevel, ActivationTime = activationTime } } });

        public void SendSetting(PedalSetting pedalSetting) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = pedalSetting } });

        public void SendSetting(ThrottleSetting.ThrottleActivityType activityType) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = new ThrottleSetting() { ActivityType = activityType } } });
        
        public void SendSetting(ThrottleSetting throttleSetting) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Data() { DescendantPacket = throttleSetting } });

        public void RefreshBatteryConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = BatteryConfiguration.Encoding.ID } } });
        
        public void RefreshCoreConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = CoreConfiguration.Encoding.ID } } });
        
        public void RefreshPedalConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = PedalConfiguration.Encoding.ID } } });
        
        public void RefreshThrottleConfiguration() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = ThrottleConfiguration.Encoding.ID } } });


        public bool IsOpen { get => dataTransport.IsOpen; }

        public BatteryConfiguration BatteryConfiguration { get; private set; }

        public CoreConfiguration CoreConfiguration { get; private set; }

        public PedalConfiguration PedalConfiguration { get; private set; }

        public ThrottleConfiguration ThrottleConfiguration { get; private set; }

        public LightSetting LightSetting { get; private set; }

        public PedalSetting PedalSetting { get; private set; }

        public ThrottleSetting ThrottleSetting { get; private set; }

        public BatteryOutput BatteryOutput { get; private set; }

        public CoreSituation Core { get; private set; }

        public Fault Fault { get; private set; }

        public LightState LightState { get ; private set; }

        public ServoInput ServoInput { get; private set; }

        public ServoOutput ServoOutput { get; private set; }

        public void ActiveCruise() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new CruiseCommand() { IsOn = true } } });

        public void DeactiveCruise() => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new CruiseCommand() { IsOn = false } } });

        public void ActiveLight(int lightId) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new LightCommand() { IsOn = true, LightId = (byte)lightId } } });

        public void DeactiveLight(int lightId) => dataTransport.Channels.FirstOrDefault()?.Transmit(new Packet() { DescendantPacket = new Command() { DescendantPacket = new LightCommand() { IsOn = false, LightId = (byte)lightId } } });
    }
}
