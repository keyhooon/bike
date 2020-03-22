using AutoMapper;
using Communication.Codec;
using Prism.Commands;
using DataModels;
using SharpCommunication.Base.Codec.Packets;

namespace Services
{
    public class BatteryManager : HardwareService
    {

        public BatteryManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration) : base(dataTransport, mapperConfiguration)
        {
            BatteryConfiguration = new BatteryConfiguration();
            BatteryOutput = new BatteryOutput();
        }

        protected override void DataReceivedHandle(IAncestorPacket packet)
        {
            switch (packet)
            {
                case BatteryConfigurationPacket batteryConfigurationPacket:
                    mapper.Map(batteryConfigurationPacket, BatteryConfiguration);
                    break;
                case BatteryOutputPacket batteryOutputPacket:
                    mapper.Map(batteryOutputPacket, BatteryOutput);
                    break;
                default:
                    break;
            }
        }

        protected override void IsConnectedChangedHandle()
        {
            if (IsConnect)
                dataTransport.DataTransmit(mapper.Map<BatteryConfiguration, BatteryConfigurationPacket>(BatteryConfiguration));
        }

        private DelegateCommand _configurationSendCommand;
        public DelegateCommand ConfigurationSendCommand =>
            _configurationSendCommand ?? (_configurationSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<BatteryConfiguration, BatteryConfigurationPacket>(BatteryConfiguration));
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() {DataId = BatteryConfigurationPacket.id });
            }, () => IsConnect).ObservesProperty(()=>nameof(IsConnect)));
        
        public BatteryOutput BatteryOutput
        {
            get;
        }
        
        public BatteryConfiguration BatteryConfiguration
        {
            get;
        }
    }
}
