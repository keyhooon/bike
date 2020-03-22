using AutoMapper;
using Communication.Codec;
using DataModels;
using Prism.Commands;
using SharpCommunication.Base.Codec.Packets;

namespace Services
{
    public class CoreManager : HardwareService
    {

        public CoreManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration):base (dataTransport, mapperConfiguration)
        {
            CoreVersion = new CoreVersion();
            CoreSituation = new CoreSituation();
        }


        protected override void DataReceivedHandle(IAncestorPacket packet)
        {
            switch (packet)
            {
                case CoreSituationPacket coreSituationPacket:
                    mapper.Map(coreSituationPacket, CoreSituation);
                    break;
                case CoreConfigurationPacket coreConfigurationPacket:
                     mapper.Map(coreConfigurationPacket, CoreVersion);
                    break;
                default:
                    break;
            }
        }

        protected override void IsConnectedChangedHandle()
        {

            if (IsConnect)
                dataTransport.CommandTransmit(new ReadCommand() { DataId = CoreConfigurationPacket.id });
        }

        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = CoreConfigurationPacket.id });
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));

        public CoreSituation CoreSituation
        {
            get ;
        }

        public CoreVersion CoreVersion
        {
            get;
        }
    }
}
