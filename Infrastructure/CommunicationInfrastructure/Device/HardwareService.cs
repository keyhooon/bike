using AutoMapper;
using System;
using Communication;
namespace Device
{
    public class HardwareService 
    {

        public event EventHandler IsConnectedChanged;
        public event EventHandler DataReceived;
        public event EventHandler CommandReceived;
        protected readonly DataTransportFacade dataTransport;
        private readonly IConfigurationProvider _mapperConfiguration;
        
        public HardwareService(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            _mapperConfiguration = mapperConfiguration;
            this.dataTransport = dataTransport;
            dataTransport.IsOpenChanged += OnIsConnectedChanged;
            dataTransport.DataReceived += OnDataReceived;
            dataTransport.CommandReceived += OnCommandReceived;
        }

        protected virtual void OnCommandReceived(object sender, PacketReceivedEventArg e)
        {
            CommandReceived?.Invoke(sender, e);
        }

        protected virtual void OnIsConnectedChanged(object sender, EventArgs e)
        {
            if (IsConnect)
                mapper = _mapperConfiguration.CreateMapper();
            IsConnectedChanged?.Invoke(sender, e);
        }

        protected virtual void OnDataReceived(object sender, PacketReceivedEventArg e)
        {
            DataReceived?.Invoke(sender, e);
        }

        private IMapper _mapper;
        protected IMapper mapper
        {
            get
            {
                if (_mapper == null)
                    return (_mapper = _mapperConfiguration.CreateMapper());
                return _mapper;
            }
            private set
            {
                _mapper = value;
            }
        }


        public bool IsConnect => dataTransport.IsOpen;


    }
}