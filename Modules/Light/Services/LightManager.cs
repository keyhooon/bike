using AutoMapper;
using Communication.Codec;
using DataModels;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Services
{
    public class LightManager : BindableBase
    {
        private readonly DataTransportFacade dataTransport;
        private readonly IMapper mapper;

        public LightManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            this.dataTransport = dataTransport;
            LightState = new LightState();
            LightSetting = new LightSetting();
            mapper = mapperConfiguration.CreateMapper();
            dataTransport.IsOpenChanged += DataTransport_IsOpenChanged;
            dataTransport.DataReceived += DataTransport_DataReceived;
        }

        private void DataTransport_DataReceived(object sender, PacketReceivedEventArg e)
        {
            switch (e.Packet)
            {
                case LightSettingPacket lightSettingpacket:
                    LightSetting = mapper.Map<LightSettingPacket, LightSetting>(lightSettingpacket);
                    break;
                case LightStatePacket lightStatePacket:
                    LightState = mapper.Map<LightStatePacket, LightState>(lightStatePacket);
                    break;
                default:
                    break;
            }
        }

        private void DataTransport_IsOpenChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnect));
            ConfigurationReceiveCommand.RaiseCanExecuteChanged();
            ConfigurationSendCommand.RaiseCanExecuteChanged();
            ToggleLightCommand.RaiseCanExecuteChanged();
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
            }, () => dataTransport.IsConnect));


        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = LightSettingPacket.id });
            }, () => dataTransport.IsConnect));


        private DelegateCommand<byte> _toggleLightCommand;
        public DelegateCommand<byte> ToggleLightCommand =>
            _toggleLightCommand ?? (_toggleLightCommand = new DelegateCommand<byte>((o) => { 
                dataTransport.CommandTransmit(new LightCommand() { LightId = o, IsOn = !LightState.Lights[o] });
                dataTransport.CommandTransmit(new ReadCommand() { DataId = LightStatePacket.id });
            }, (o) => dataTransport.IsConnect));

        public bool IsConnect => dataTransport.IsConnect;

        private LightState _lightState;
        public LightState LightState
        {
            get { return _lightState; }
            protected set { SetProperty(ref _lightState, value); }
        }

        private LightSetting _lightSetting;
        public LightSetting LightSetting
        {
            get { return _lightSetting; }
            set { SetProperty(ref _lightSetting, value); }
        }
    }
}