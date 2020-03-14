
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Communication.Codec;
using Communication.Transport;
using Core;
using Microsoft.Extensions.Configuration;
using Prism.Commands;
using Prism.Mvvm;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec.Packets;
using SharpCommunication.Base.Transport;
using SharpCommunication.Base.Transport.SerialPort;

namespace Communication.Service
{
    public class DataTransportFacade
    {
        private readonly DataTransport<DevicePacket> _dataTransport;

        public event EventHandler<PacketReceivedEventArg> DataReceived;
        public event EventHandler<PacketReceivedEventArg> CommandReceived;
        public event EventHandler IsOpenChanged;
        public event EventHandler CanCloseChanged;
        public event EventHandler CanOpenChanged;

        public DataTransportFacade(DataTransport<DevicePacket> dataTransport, IEnumerable<PacketEncodingBuilder> PacketEncodingBuilderList)
        {
            _dataTransport = dataTransport;
            _dataTransport.IsOpenChanged += IsOpenChanged;
            _dataTransport.CanCloseChanged += CanCloseChanged;
            _dataTransport.CanOpenChanged += CanOpenChanged;
            var packetEncodingGroups = PacketEncodingBuilderList.Select(o => o.Build()).GroupBy((o) => o is IFunctionPacket);
            ((DevicePacketCodec)_dataTransport.ChannelFactory.Codec).RegisterCommand(packetEncodingGroups.FirstOrDefault((o) => o.Key));
            ((DevicePacketCodec)_dataTransport.ChannelFactory.Codec).RegisterData(packetEncodingGroups.FirstOrDefault(o => o.Key));
            ((INotifyCollectionChanged)_dataTransport.Channels).CollectionChanged += OnChannel_CollectionChanged;
        }

        private void OnChannel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems)
                    ((IChannel<DevicePacket>) item).DataReceived += OnChannel_DataReceived;
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


        public bool IsConnect => _dataTransport.IsOpen;

        private DelegateCommand _startCommand;
        public DelegateCommand StartCommand =>
            _startCommand ?? (_startCommand = new DelegateCommand(()=> _dataTransport.Open(),()=> _dataTransport.CanOpen).ObservesProperty(()=>IsConnect));

        private DelegateCommand _stopCommand;
        public DelegateCommand StopCommand =>
            _stopCommand ?? (_stopCommand = new DelegateCommand(()=> _dataTransport.Close(), () => _dataTransport.CanClose).ObservesProperty(() => IsConnect));

        private DelegateCommand<byte?> _refreshDataCommand;
        public DelegateCommand<byte?> RefreshDataCommand =>
            _refreshDataCommand ?? (_refreshDataCommand = new DelegateCommand<byte?>(
                i => {
                    _dataTransport.Channels[0].Transmit(DevicePacket.CreateReadCommand(i.Value));
                },
                i => _dataTransport.IsOpen).ObservesProperty(() => IsConnect));    

    }
}
