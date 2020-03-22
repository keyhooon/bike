using AutoMapper;
using Communication.Codec;
using DataModels;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Services
{
    public class ThrottleManager : BindableBase
    {
        private readonly DataTransportFacade dataTransport;
        private readonly IMapper mapper;

        public ThrottleManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            this.dataTransport = dataTransport;
            ThrottleConfiguration = new ThrottleConfiguration();
            ThrottleSetting = new ThrottleSetting();
            mapper = mapperConfiguration.CreateMapper();
            dataTransport.IsOpenChanged += DataTransport_IsOpenChanged;
            dataTransport.DataReceived += DataTransport_DataReceived;
        }

        private void DataTransport_DataReceived(object sender, PacketReceivedEventArg e)
        {
            switch (e.Packet)
            {
                case ThrottleConfigurationPacket throttleConfigurationPacket:
                    ThrottleConfiguration = mapper.Map<ThrottleConfigurationPacket, ThrottleConfiguration>(throttleConfigurationPacket);
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
            if (IsConnect)
            {
                dataTransport.DataTransmit(mapper.Map<ThrottleConfiguration, ThrottleConfigurationPacket>(ThrottleConfiguration));
            }
        }

        private DelegateCommand _configurationSendCommand;
        public DelegateCommand ConfigurationSendCommand =>
            _configurationSendCommand ?? (_configurationSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<ThrottleConfiguration, ThrottleConfigurationPacket>(ThrottleConfiguration));
            }, () => dataTransport.IsConnect));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = ThrottleConfigurationPacket.id });
            }, () => dataTransport.IsConnect));


        public bool IsConnect => dataTransport.IsConnect;

        private ThrottleConfiguration _throttleConfiguration;
        public ThrottleConfiguration ThrottleConfiguration
        {
            get { return _throttleConfiguration; }
            protected set { SetProperty(ref _throttleConfiguration, value); }
        }

        private ThrottleSetting _throttleSetting;
        public ThrottleSetting ThrottleSetting
        {
            get { return _throttleSetting; }
            protected set { SetProperty(ref _throttleSetting, value); }
        }



    }
} 