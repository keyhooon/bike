using AutoMapper;
using Communication.Codec;
using DataModels;
using Prism.Commands;
using SharpCommunication.Base.Codec.Packets;

namespace Services
{
    public class LightManager : HardwareService
    {

        public LightManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration) :base(dataTransport, mapperConfiguration)
        {
            LightState = new LightState();
            LightSetting = new LightSetting();
        }

        protected override void DataReceivedHandle(IAncestorPacket packet)
        {
            switch (packet)
            {
                case LightSettingPacket lightSettingpacket:
                     mapper.Map(lightSettingpacket, LightSetting);
                    break;
                case LightStatePacket lightStatePacket:
                     mapper.Map(lightStatePacket, LightState);
                    break;
                default:
                    break;
            }
        }

        protected override void IsConnectedChangedHandle()
        {
            if (IsConnect)
            {
                dataTransport.DataTransmit(mapper.Map<LightSetting, LightSettingPacket>(LightSetting));
                dataTransport.CommandTransmit(new ReadCommand() { DataId = LightStatePacket.id });
            }
        }

        private DelegateCommand _configurationSendCommand;
        public DelegateCommand ConfigurationSendCommand =>
            _configurationSendCommand ?? (_configurationSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<LightSetting, LightSettingPacket>(LightSetting));
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = LightSettingPacket.id });
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        private DelegateCommand<byte> _toggleLightCommand;
        public DelegateCommand<byte> ToggleLightCommand =>
            _toggleLightCommand ?? (_toggleLightCommand = new DelegateCommand<byte>((o) => { 
                dataTransport.CommandTransmit(new LightCommand() { LightId = o, IsOn = !LightState.Lights[o] });
                dataTransport.CommandTransmit(new ReadCommand() { DataId = LightStatePacket.id });
            }, (o) => IsConnect).ObservesProperty(() => nameof(IsConnect)));



        public LightState LightState
        {
            get;
        }


        public LightSetting LightSetting
        {
            get;
        }
    }
}