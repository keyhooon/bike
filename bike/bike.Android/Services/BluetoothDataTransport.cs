using System;
using System.Linq;
using System.Threading;
using Android.Bluetooth;
using Android.Locations;
using bike.Services;
using Device.Communication.Channels;
using Device.Communication.Codec;
using Device.Communication.Transport;
using SharpCommunication.Channels.Decorator;
using SharpCommunication.Transport;

namespace bike.Droid.Services
{
    public class BluetoothPacketDataTransport : DataTransport<Packet>, IBlueToothService
    {
        private BluetoothAdapter _adapter;
        private BluetoothSocket _socket;


        public BluetoothPacketDataTransport(BluetoothDataTransportOption option) : base(new PacketChannelFactory(), option)
        {
            _adapter = BluetoothAdapter.DefaultAdapter;
        }

        protected override bool IsOpenCore {
            get => _socket != null && _socket.IsConnected && Channels.Any();
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
            var ch = ((PacketChannelFactory)ChannelFactory).Create(_socket.InputStream, _socket.OutputStream);
            ch.ErrorReceived += (sender, ex) =>
            {
                _socket = null;
            };
            _channels.Add(ch);
            ch.DataReceived += (sender, e) => DataReceivedCount = ch.ToMonitoredChannel().GetDataReceivedCount;
        }
        public int DataReceivedCount { get; set; }
        public (string Name,string Address)[] BluetoothBonded => (_adapter.BondedDevices.Select((device) => ( device.Name, device.Address )).ToArray());
    }
}