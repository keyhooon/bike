﻿using System;
using System.Linq;
using System.Threading;
using Android.Bluetooth;
using Device.Communication.Channels;
using Device.Communication.Codec;
using Device.Communication.Transport;
using SharpCommunication.Channels.Decorator;
using SharpCommunication.Transport;

namespace bike.Droid.Services
{
    public class BluetoothPacketDataTransport : DataTransport<Packet>
    {
        private BluetoothAdapter _adapter;
        private BluetoothSocket _socket;


        public BluetoothPacketDataTransport(BluetoothDataTransportOption option) : base(new PacketChannelFactory(), option)
        {
            _adapter = BluetoothAdapter.DefaultAdapter;
        }

        protected override bool IsOpenCore {
            get
            {
                //var lastPacketTime = _channels.FirstOrDefault()?.ToMonitoredChannel()?.LastPacketTime??DateTime.MinValue;
                return _socket != null && _socket.IsConnected /*&& (lastPacketTime > (DateTime.Now - TimeSpan.FromSeconds(2)))*/;
            }
        }

        protected override void CloseCore()
        {
            _channels.Clear();
            _socket?.Close();
            _socket?.Dispose();
            _socket = null;
        }

        protected override void OpenCore()
        {
            if (_adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!_adapter.IsEnabled)
            {
                _adapter.Enable();
                if (!SpinWait.SpinUntil(()=>_adapter.IsEnabled, TimeSpan.FromSeconds(2)))
                    throw new Exception("Bluetooth adapter can not enabled.");
            }
            BluetoothDevice device = (_adapter.BondedDevices.FirstOrDefault((device) => device.Name == ((BluetoothDataTransportOption)Option).DeviceName));
            
            if (device == null)
                throw new Exception($"There is no BlueTooth Device with {Java.Util.UUID.FromString(((BluetoothDataTransportOption)Option).DeviceName)} Name.");
            _socket = device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString(((BluetoothDataTransportOption)Option).UUID));
            _socket.Connect();
            var ch = ChannelFactory.Create(_socket.InputStream, _socket.OutputStream);
            ch.ErrorReceived += (sender, ex) =>
            {
                _socket = null;
            };
            _channels.Add(ch);

        }
    }
}