using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Communication;
using Communication.Codec;
using DataModels;

namespace Device
{
    public class ServoDriveService : HardwareService
    {
        private BatteryOutput battery;
        private CoreSituation core;
        private Fault fault;
        private LightState light;
        private ServoInput servoInput;
        private ServoOutput servo;

        private ThrottleSetting throttleSetting;
        private PedalSetting pedalSetting;
        private LightSetting lightSetting;

        public event EventHandler BatteryChanged;
        public event EventHandler CoreChanged;
        public event EventHandler FaultChanged;
        public event EventHandler LightChanged;
        public event EventHandler ServoInputChanged;
        public event EventHandler ServoChanged;
        public event EventHandler ConfigurationChanged;

        public ServoDriveService(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration) : base(dataTransport, mapperConfiguration)
        {
            BatteryConfiguration = new BatteryConfiguration();

            CoreConfiguration = new CoreVersion();

            PedalConfiguration = new PedalConfiguration();

            ThrottleConfiguration = new ThrottleConfiguration();

            ServoConfiguration = new ServoConfiguration();

            PedalSetting = new PedalSetting();

            LightSetting = new LightSetting();

            ThrottleSetting = new ThrottleSetting();

            Core = new CoreSituation();

            Battery = new BatteryOutput();

            Fault = new Fault();

            Light = new LightState();

            Servo = new ServoOutput();

            ServoInput = new ServoInput();

            PedalSetting.PropertyChanged += (sender, e) => dataTransport.DataTransmit(mapper.Map<PedalSettingPacket>(PedalSetting));

            LightSetting.PropertyChanged += (sender, e) => dataTransport.DataTransmit(mapper.Map<LightSettingPacket>(LightSetting));

//            ThrottleSetting.PropertyChanged += (sender, e) => dataTransport.DataTransmit(mapper.Map<ThrottleSettingPacket>(ThrottleSetting));

            Light.PropertyChanged += (sender, e) => dataTransport.DataTransmit(mapper.Map<LightSettingPacket>(Light));
        }

        protected override void OnDataReceived(object sender, PacketReceivedEventArg e)
        {
            base.OnDataReceived(sender, e);
            switch (e.Packet)
            {
                case BatteryOutputPacket batteryOutputPacket:
                    mapper.Map(batteryOutputPacket, Battery);
                    BatteryChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case CoreSituationPacket coreSituationPacket:
                    mapper.Map(coreSituationPacket, Core);
                    CoreChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case FaultPacket faultPacket:
                    mapper.Map(faultPacket, Fault);
                    FaultChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case LightStatePacket lightStatePacket:
                    mapper.Map(lightStatePacket, Light);
                    LightChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ServoInputPacket servoInputPacket:
                    mapper.Map(servoInputPacket, ServoInput);
                    ServoInputChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ServoOutputPacket servoOutputPacket:
                    mapper.Map(servoOutputPacket, Servo);
                    ServoChanged?.Invoke(this, EventArgs.Empty);
                    break;


                case BatteryConfigurationPacket batteryConfigurationPacket:
                    mapper.Map(batteryConfigurationPacket, BatteryConfiguration);
                    ConfigurationChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case CoreConfigurationPacket coreConfigurationPacket:
                    mapper.Map(coreConfigurationPacket, CoreConfiguration);
                    ConfigurationChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case PedalConfigurationPacket pedalconfigurationPacket:
                    mapper.Map(pedalconfigurationPacket, PedalConfiguration);
                    ConfigurationChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ThrottleConfigurationPacket throttleConfigurationPacket:
                    mapper.Map(throttleConfigurationPacket, ThrottleConfiguration);
                    ConfigurationChanged?.Invoke(this, EventArgs.Empty);
                    break;
                //case ServoConfigurationPacket servoConfigurationPacket:
                //    mapper.Map(servoConfigurationPacket, ServoConfiguration);
                //    ConfigurationChanged?.Invoke(this, EventArgs.Empty);
                //    break;


                case LightSettingPacket lightSettingPacket:
                    mapper.Map(lightSettingPacket, LightSetting);
                    break;
                case PedalSettingPacket pedalSettingPacket:
                    mapper.Map(pedalSettingPacket, PedalSetting);
                    break;
                //case ThrottleSettingPacket throttleSettingPacket :
                //    ThrottleSetting = mapper.Map<ThrottleSetting>(throttleSettingPacket);
                //    break;

                default:
                    break;
            }
        }
        protected override void OnIsConnectedChanged(object sender, EventArgs e)
        {
            base.OnIsConnectedChanged(sender, e);
            if (IsConnect)
                IsConnected();

        }
        protected virtual void IsConnected()
        {
            ReceiveConfiguration();
            SendSetting();
        }
        private void ReceiveConfiguration()
        {
            dataTransport.ReadCommandsTransmitAll(new[]{
                BatteryConfigurationPacket.id,
                CoreConfigurationPacket.id,
                PedalConfigurationPacket.id,
                //ServoConfigurationPacket.id,
                ThrottleConfigurationPacket.id }
                );
        }
        private void SendSetting()
        {
            dataTransport.DataTransmit(mapper.Map<LightSettingPacket>(LightSetting));
            dataTransport.DataTransmit(mapper.Map<PedalSettingPacket>(PedalSetting));
            //dataTransport.DataTransmit(mapper.Map<ThrottleSettingPacket>(ThrottleSetting));
        }
        public BatteryConfiguration BatteryConfiguration { get; private set; }

        public CoreVersion CoreConfiguration { get; private set; }

        public PedalConfiguration PedalConfiguration { get; private set; }

        public ServoConfiguration ServoConfiguration { get; private set; }

        public ThrottleConfiguration ThrottleConfiguration { get; private set; }


        public LightSetting LightSetting { get; private set; }

        public PedalSetting PedalSetting { get; private set; }

        public ThrottleSetting ThrottleSetting { get; private set; }


        public BatteryOutput Battery { get; private set; }

        public CoreSituation Core { get; private set; }

        public Fault Fault { get; private set; }

        public LightState Light { get ; private set; }

        public ServoInput ServoInput { get; private set; }

        public ServoOutput Servo { get; private set; }


        public void ActiveCruise() => dataTransport.CommandTransmit(new CruiseCommand() { IsOn = true });

        public void DeactiveCruise() => dataTransport.CommandTransmit(new CruiseCommand() { IsOn = false });

        public void ActiveLight(int lightId) => dataTransport.CommandTransmit(new LightCommand() { IsOn = true, LightId = (byte)lightId });

        public void DeactiveLight(int lightId) => dataTransport.CommandTransmit(new LightCommand() { IsOn = false, LightId = (byte)lightId });


    }
}
