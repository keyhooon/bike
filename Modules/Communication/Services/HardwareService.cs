using AutoMapper;
using Prism.Mvvm;
using SharpCommunication.Base.Codec.Packets;
using System;

namespace Services
{
    public abstract class HardwareService : BindableBase
    {
        protected readonly DataTransportFacade dataTransport;
        protected readonly IMapper mapper;

        public HardwareService(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            this.dataTransport = dataTransport;
            mapper = mapperConfiguration.CreateMapper();
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