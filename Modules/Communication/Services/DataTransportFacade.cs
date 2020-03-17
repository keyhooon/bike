
using System;
using System.Collections.Specialized;
using Communication.Codec;
using Prism.Commands;
using Prism.Ioc;
using Prism.Unity;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec.Packets;
using SharpCommunication.Base.Transport;
using SharpCommunication.Base.Transport.SerialPort;
using Unity;

namespace Services
{
    public class DataTransportFacade
    {
        private DataTransport<DevicePacket> _dataTransport;
        private readonly UnityContainerExtension _container;
        private readonly SerialPortDataTransportOption serialPortDataTransportOption;

        public event EventHandler<PacketReceivedEventArg> DataReceived;
        public event EventHandler<PacketReceivedEventArg> CommandReceived;
        public event EventHandler IsOpenChanged;
        public event EventHandler CanCloseChanged;
        public event EventHandler CanOpenChanged;

        public DataTransportFacade(UnityContainerExtension containerRegistry, SerialPortDataTransportOption serialPortDataTransportOption)
        {
            _container =  containerRegistry;
            this.serialPortDataTransportOption = serialPortDataTransportOption;
        }

        private void OnChannel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems)
                    ((IChannel<DevicePacket>)item).DataReceived += OnChannel_DataReceived;
            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems)
                    ((IChannel<DevicePacket>)item).DataReceived -= OnChannel_DataReceived;
        }

        private void OnChannel_DataReceived(object sender, DataReceivedEventArg<DevicePacket> e)
        {
            switch (e.Data.DescendantPacket)
            {
                case DataPacket dataPacket:
                    DataReceived?.Invoke(sender, new PacketReceivedEventArg(dataPacket.DescendantPacket));
                    break;
                case CommandPacket commandPacket:
                    CommandReceived?.Invoke(sender, new PacketReceivedEventArg(commandPacket.DescendantPacket));
                    break;
                default:
                    break;
            }
        }

        public void DataTransmit(IAncestorPacket dataPacket)
        {
            new DevicePacket() { DescendantPacket = new DataPacket() { DescendantPacket = dataPacket } };
        }

        public void CommandTransmit(IAncestorPacket commandPacket)
        {
            new DevicePacket() { DescendantPacket = new CommandPacket() { DescendantPacket = commandPacket } };
        }


        public bool IsConnect => _dataTransport?.IsOpen??false ;

        private DelegateCommand _startCommand;
        public DelegateCommand StartCommand =>
            _startCommand ?? (_startCommand = new DelegateCommand(() => {
                _dataTransport = _container.Resolve<DataTransport<DevicePacket>>();
                _dataTransport.IsOpenChanged += IsOpenChanged;
                _dataTransport.CanCloseChanged += CanCloseChanged;
                _dataTransport.CanOpenChanged += CanOpenChanged;
                ((INotifyCollectionChanged)_dataTransport.Channels).CollectionChanged += OnChannel_CollectionChanged;
                _dataTransport.Open(); 
            }, () => _dataTransport.CanOpen).ObservesProperty(() => IsConnect));

        private DelegateCommand _stopCommand;
        public DelegateCommand StopCommand =>
            _stopCommand ?? (_stopCommand = new DelegateCommand(() => _dataTransport.Close(), () => _dataTransport.CanClose).ObservesProperty(() => IsConnect));

        private DelegateCommand<byte?> _refreshDataCommand;
        public DelegateCommand<byte?> RefreshDataCommand =>
            _refreshDataCommand ?? (_refreshDataCommand = new DelegateCommand<byte?>(
                i =>
                {
                    _dataTransport.Channels[0].Transmit(DevicePacket.CreateReadCommand(i.Value));
                },
                i => _dataTransport.IsOpen).ObservesProperty(() => IsConnect));

    }
}
