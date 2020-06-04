﻿
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Communication.Codec;
using Device.Communication.Codec;
using Prism.Commands;
using Prism.Ioc;
using Prism.Unity;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using SharpCommunication.Transport.SerialPort;
using Unity;

namespace Communication
{
    public class DataTransportFacade
    {
        private DataTransport<Packet> _dataTransport;
        private UnityContainerExtension _container;
        private readonly SerialPortDataTransportOption serialPortDataTransportOption;

        public event EventHandler<PacketReceivedEventArg> DataReceived;
        public event EventHandler<PacketReceivedEventArg> CommandReceived;
        public event EventHandler IsOpenChanged;
        public event EventHandler CanCloseChanged;
        public event EventHandler CanOpenChanged;

        public DataTransportFacade(UnityContainerExtension container, SerialPortDataTransportOption serialPortDataTransportOption)
        {
            _container = container;
            this.serialPortDataTransportOption = serialPortDataTransportOption;

        }

        private void OnChannel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems)
                    ((IChannel<Packet>)item).DataReceived += OnChannel_DataReceived;
            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems)
                    ((IChannel<Packet>)item).DataReceived -= OnChannel_DataReceived;
        }

        private void OnChannel_DataReceived(object sender, DataReceivedEventArg<Packet> e)
        {
            switch (e.Data.DescendantPacket)
            {
                case DatadataPacket:
                    DataReceived?.Invoke(sender, new PacketReceivedEventArg(data.DescendantPacket));
                    break;
                case CommandcommandPacket:
                    CommandReceived?.Invoke(sender, new PacketReceivedEventArg(command.DescendantPacket));
                    break;
                default:
                    break;
            }
        }

        public void DataTransmit(IAncestorPacket dataPacket)
        {
            foreach (var channel in _dataTransport.Channels)
            {
                channel.Transmit(Packet.CreateDataPacket(dataPacket));
            }
        }

        public void CommandTransmit(IFunctionPacket commandPacket)
        {
            foreach (var channel in _dataTransport.Channels)
            {
                channel.Transmit(DevicePacket.CreateCommandPacket(commandPacket));
            }
        }

        public void ReadCommandTransmit(byte dataId)
        {
            foreach (var channel in _dataTransport.Channels)
            {
                channel.Transmit(DevicePacket.CreateReadCommand(dataId));
            }
        }
        public void ReadCommandsTransmitAll(IEnumerable<byte> dataIds)
        {
            foreach (var dataId in dataIds)
            {
                ReadCommandTransmit(dataId);
            }
        }


        public bool IsOpen => _dataTransport?.IsOpen ?? false;

        private DelegateCommand _startCommand;
        public DelegateCommand StartCommand =>
            _startCommand ?? (_startCommand = new DelegateCommand(() =>
            {
                _dataTransport = _container.Resolve<DataTransport<DevicePacket>>();
                _dataTransport.IsOpenChanged += IsOpenChanged;
                _dataTransport.CanCloseChanged += CanCloseChanged;
                _dataTransport.CanOpenChanged += CanOpenChanged;
                ((INotifyCollectionChanged)_dataTransport.Channels).CollectionChanged += OnChannel_CollectionChanged;
                _dataTransport.Open();
            }, () => _dataTransport.CanOpen).ObservesProperty(() => IsOpen));

        private DelegateCommand _stopCommand;
        public DelegateCommand StopCommand =>
            _stopCommand ?? (_stopCommand = new DelegateCommand(() => _dataTransport.Close(), () => _dataTransport.CanClose).ObservesProperty(() => IsOpen));

        private DelegateCommand<byte?> _refreshDataCommand;
        public DelegateCommand<byte?> RefreshDataCommand =>
            _refreshDataCommand ?? (_refreshDataCommand = new DelegateCommand<byte?>(
                i =>
                {
                    _dataTransport.Channels[0].Transmit(DevicePacket.CreateReadCommand(i.Value));
                },
                i => _dataTransport.IsOpen).ObservesProperty(() => IsOpen));

    }
}
