using AutoMapper;
using Communication.Codec;
using Prism.Commands;
using Prism.Mvvm;
using System;
using DataModels;

namespace Services
{
    public class BatteryManager : BindableBase
    {
        private readonly DataTransportFacade dataTransport;
        private readonly IMapper mapper;

        public BatteryManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            this.dataTransport = dataTransport;
            mapper = mapperConfiguration.CreateMapper();
            BatteryConfiguration = new BatteryConfiguration();
            BatteryOutput = new BatteryOutput();
            dataTransport.IsOpenChanged += DataTransport_IsOpenChanged;
            dataTransport.DataReceived += DataTransport_DataReceived;
        }

        private void DataTransport_DataReceived(object sender, PacketReceivedEventArg e)
        {
            switch (e.Packet)
            {
                case BatteryConfigurationPacket batteryConfigurationPacket:
                    BatteryConfiguration = mapper.Map<BatteryConfigurationPacket, BatteryConfiguration>(batteryConfigurationPacket);
                    break;
                case BatteryOutputPacket batteryOutputPacket:
                    BatteryOutput = mapper.Map<BatteryOutputPacket, BatteryOutput>(batteryOutputPacket);
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
                dataTransport.DataTransmit(mapper.Map<BatteryConfiguration, BatteryConfigurationPacket>(BatteryConfiguration));
        }

        private DelegateCommand _configurationSendCommand;
        public DelegateCommand ConfigurationSendCommand =>
            _configurationSendCommand ?? (_configurationSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<BatteryConfiguration, BatteryConfigurationPacket>(BatteryConfiguration));
            }, () => dataTransport.IsConnect));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() {DataId = BatteryConfigurationPacket.id });
            }, () => dataTransport.IsConnect));


        public bool IsConnect => dataTransport.IsConnect;

        private BatteryOutput _batteryOutput;
        public BatteryOutput BatteryOutput
        {
            get { return _batteryOutput; }
            protected set { SetProperty(ref _batteryOutput, value); }
        }

        private BatteryConfiguration _batteryConfiguration;
        public BatteryConfiguration BatteryConfiguration
        {
            get { return _batteryConfiguration; }
            set { SetProperty(ref _batteryConfiguration, value); }
        }
    }
}
