using AutoMapper;
using Battery.DataSettings;
using Communication.Codec;
using Communication.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using Battery.DataTransfterPacket;
using Battery.DataModel;

namespace Battery.Services
{
    public class BatteryManager : BindableBase
    {
        private readonly DataTransportFacade dataTransport;
        private readonly IMapper mapper;

        public BatteryManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            this.dataTransport = dataTransport;
            mapper = mapperConfiguration.CreateMapper();
            dataTransport.IsOpenChanged += DataTransport_IsOpenChanged;
            dataTransport.DataReceived += DataTransport_DataReceived;
        }

        private void DataTransport_DataReceived(object sender, Core.PacketReceivedEventArg e)
        {
            switch (e.Packet)
            {
                case BatteryConfigurationPacket batteryConfigurationPacket:
                    BatteryConfigurationSetting = mapper.Map<BatteryConfigurationPacket, BatteryConfigurationSetting>(batteryConfigurationPacket);
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
            if (IsConnect)
                dataTransport.DataTransmit(mapper.Map<BatteryConfigurationSetting, BatteryConfigurationPacket>(BatteryConfigurationSetting));
        }

        private DelegateCommand _configurationSendCommand;
        public DelegateCommand ConfigurationSendCommand =>
            _configurationSendCommand ?? (_configurationSendCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(mapper.Map<BatteryConfigurationSetting, BatteryConfigurationPacket>(BatteryConfigurationSetting));
            }, () => dataTransport.IsConnect));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.DataTransmit(new ReadCommand() {DataId = BatteryConfigurationPacket.id });
            }, () => dataTransport.IsConnect));


        public bool IsConnect => dataTransport.IsConnect;

        private BatteryOutput _batteryOutput;
        public BatteryOutput BatteryOutput
        {
            get { return _batteryOutput; }
            set { SetProperty(ref _batteryOutput, value); }
        }

        private BatteryConfigurationSetting _batteryConfigurationSetting;
        public BatteryConfigurationSetting BatteryConfigurationSetting
        {
            get { return _batteryConfigurationSetting; }
            set { SetProperty(ref _batteryConfigurationSetting, value); }
        }
    }
}
