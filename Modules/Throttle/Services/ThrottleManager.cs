using AutoMapper;
using Communication.Codec;
using DataModels;
using Prism.Commands;
using SharpCommunication.Base.Codec.Packets;

namespace Services
{
    public class ThrottleManager : HardwareService
    {


        public ThrottleManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration) : base(dataTransport, mapperConfiguration)
        {
            ThrottleConfiguration = new ThrottleConfiguration();
            ThrottleSetting = new ThrottleSetting();
        }

        protected override void DataReceivedHandle(IAncestorPacket packet)
        {
            switch (packet)
            {
                case ThrottleConfigurationPacket throttleConfigurationPacket:
                    mapper.Map(throttleConfigurationPacket, ThrottleConfiguration);
                    break;
                default:
                    break;
            }
        }

        protected override void IsConnectedChangedHandle()
        {
            RaisePropertyChanged(nameof(IsConnect));
            if (IsConnect)
            {
                dataTransport.DataTransmit(mapper.Map<ThrottleConfiguration, ThrottleConfigurationPacket>(ThrottleConfiguration));
            }
        }

        private DelegateCommand _configurationSendCommand;
        public DelegateCommand ConfigurationSendCommand =>
            _configurationSendCommand ?? (_configurationSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<ThrottleConfiguration, ThrottleConfigurationPacket>(ThrottleConfiguration));
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = ThrottleConfigurationPacket.id });
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));

        public ThrottleConfiguration ThrottleConfiguration
        {
            get;
        }

        public ThrottleSetting ThrottleSetting
        {
            get;
        }



    }
} 