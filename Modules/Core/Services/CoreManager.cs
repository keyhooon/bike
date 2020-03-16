using AutoMapper;
using Communication.Codec;
using Communication.Service;
using Core.DataModels;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class CoreManager : BindableBase
    {
        private readonly DataTransportFacade dataTransport;
        private readonly IMapper mapper;

        public CoreManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
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
                case CoreSituationPacket coreSituationPacket:
                    CoreSituation = mapper.Map<CoreSituationPacket, CoreSituation>(coreSituationPacket);
                    break;
                case CoreConfigurationPacket coreConfigurationPacket:
                    CoreVersion = mapper.Map<CoreConfigurationPacket, CoreVersion>(coreConfigurationPacket);
                    break;
                default:
                    break;
            }
        }

        private void DataTransport_IsOpenChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnect));
            ConfigurationReceiveCommand.RaiseCanExecuteChanged();
            if (IsConnect)
                dataTransport.CommandTransmit(new ReadCommand() { DataId = CoreConfigurationPacket.id });
        }

        private DelegateCommand _configurationReceiveCommand;
        public DelegateCommand ConfigurationReceiveCommand =>
            _configurationReceiveCommand ?? (_configurationReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = CoreConfigurationPacket.id });
            }, () => dataTransport.IsConnect));


        public bool IsConnect => dataTransport.IsConnect;

        private CoreSituation _coreSituation;
        public CoreSituation CoreSituation
        {
            get { return _coreSituation; }
            protected set { SetProperty(ref _coreSituation, value); }
        }

        private CoreVersion _coreVersion;
        public CoreVersion CoreVersion
        {
            get { return _coreVersion; }
            protected set { SetProperty(ref _coreVersion, value); }
        }
    }
}
