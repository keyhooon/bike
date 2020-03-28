using AutoMapper;
using Prism.Mvvm;
using SharpCommunication.Base.Codec.Packets;
using System;

namespace Services
{
    public abstract class HardwareService : BindableBase
    {
        protected readonly DataTransportFacade dataTransport;
        private readonly IConfigurationProvider _mapperConfiguration;
        private IMapper _mapper;
        protected IMapper mapper 
        {
            get
            {
                if (_mapper == null)
                    return (_mapper = _mapperConfiguration.CreateMapper());
                return _mapper;
            }
        }

        public HardwareService(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            _mapperConfiguration = mapperConfiguration;
            this.dataTransport = dataTransport;
            dataTransport.IsOpenChanged += DataTransport_IsOpenChanged;
            dataTransport.DataReceived += DataTransport_DataReceived;
        }

        private void DataTransport_DataReceived(object sender, PacketReceivedEventArg e)
        {
            DataReceivedHandle(e.Packet);
        }
        protected abstract void DataReceivedHandle(IAncestorPacket packet);
        protected abstract void IsConnectedChangedHandle();

        private void DataTransport_IsOpenChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnect));
            IsConnectedChangedHandle();
        }

        public bool IsConnect => dataTransport.IsConnect;


    }
}