using AutoMapper;
using Communication.Codec;
using DataModels;

using Prism.Commands;
using Prism.Mvvm;
using System;


namespace Services
{
    public class PedalManager : BindableBase
    {
        private readonly DataTransportFacade dataTransport;
        private readonly IMapper mapper;

        public PedalManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            this.dataTransport = dataTransport;
            PedalConfiguration = new PedalConfiguration();
            PedalSetting = new PedalSetting();
            mapper = mapperConfiguration.CreateMapper();
            dataTransport.IsOpenChanged += DataTransport_IsOpenChanged;
            dataTransport.DataReceived += DataTransport_DataReceived;
        }

        private void DataTransport_DataReceived(object sender, PacketReceivedEventArg e)
        {
            switch (e.Packet)
            {
                case PedalConfigurationPacket pedalConfigurationPacket:
                    PedalConfiguration = mapper.Map<PedalConfigurationPacket, PedalConfiguration>(pedalConfigurationPacket);
                    break;
                case PedalSettingPacket pedalSettingPacket:
                    PedalSetting = mapper.Map<PedalSettingPacket, PedalSetting>(pedalSettingPacket);
                    break;
                default:
                    break;
            }
        }

        private void DataTransport_IsOpenChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnect));
            ConfigurationSendCommand.RaiseCanExecuteChanged();
            ConfigurationReceiveCommand.RaiseCanExecuteChanged();
            SettingSendCommand.RaiseCanExecuteChanged();
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
            }, () => dataTransport.IsConnect));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = PedalConfigurationPacket.id });
            }, () => dataTransport.IsConnect));

        private DelegateCommand _settingSendCommand;
        public DelegateCommand SettingSendCommand =>
            _settingSendCommand ?? (_settingSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<PedalSetting, PedalSettingPacket>(PedalSetting));
            }, () => dataTransport.IsConnect));


        private DelegateCommand _settingReceiveCommand;
        public DelegateCommand SettingReceiveCommand =>
            _settingReceiveCommand ?? (_settingReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = PedalSettingPacket.id });
            }, () => dataTransport.IsConnect));


        public bool IsConnect => dataTransport.IsConnect;

        private PedalConfiguration _pedalConfiguration;
        public PedalConfiguration PedalConfiguration
        {
            get { return _pedalConfiguration; }
            protected set { SetProperty(ref _pedalConfiguration, value); }
        }

        private PedalSetting _pedalSetting;
        public PedalSetting PedalSetting
        {
            get { return _pedalSetting; }
            set { SetProperty(ref _pedalSetting, value); }
        }
    }
}