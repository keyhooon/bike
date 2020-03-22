using AutoMapper;
using Communication.Codec;
using DataModels;

using Prism.Commands;
using SharpCommunication.Base.Codec.Packets;


namespace Services
{
    public class PedalManager : HardwareService
    {

        public PedalManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration): base(dataTransport, mapperConfiguration)
        {
            PedalConfiguration = new PedalConfiguration();
            PedalSetting = new PedalSetting();
        }

        protected override void DataReceivedHandle(IAncestorPacket packet)
        {
            switch (packet)
            {
                case PedalConfigurationPacket pedalConfigurationPacket:
                    mapper.Map(pedalConfigurationPacket, PedalConfiguration);
                    break;
                case PedalSettingPacket pedalSettingPacket:
                     mapper.Map(pedalSettingPacket, PedalSetting);
                    break;
                default:
                    break;
            }
        }

        protected override void IsConnectedChangedHandle()
        {
            SettingReceiveCommand.RaiseCanExecuteChanged();
            if (IsConnect)
            {
                dataTransport.DataTransmit(mapper.Map<PedalConfiguration, PedalConfigurationPacket>(PedalConfiguration));
                dataTransport.DataTransmit(mapper.Map<PedalSetting, PedalSettingPacket>(PedalSetting));
            }
        }

        private DelegateCommand _configurationSendCommand;
        public DelegateCommand ConfigurationSendCommand =>
            _configurationSendCommand ?? (_configurationSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<PedalConfiguration, PedalConfigurationPacket>(PedalConfiguration));
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = PedalConfigurationPacket.id });
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));

        private DelegateCommand _settingSendCommand;
        public DelegateCommand SettingSendCommand =>
            _settingSendCommand ?? (_settingSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<PedalSetting, PedalSettingPacket>(PedalSetting));
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        private DelegateCommand _settingReceiveCommand;
        public DelegateCommand SettingReceiveCommand =>
            _settingReceiveCommand ?? (_settingReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = PedalSettingPacket.id });
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));

        public PedalConfiguration PedalConfiguration
        {
            get;
        }

        public PedalSetting PedalSetting
        {
            get;
        }
    }
}