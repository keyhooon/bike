using System;
using System.Linq;
using System.Threading;
using Android.Bluetooth;
using Device.Communication.Channels;
using Device.Communication.Codec;
using Device.Communication.Transport;
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

        protected override bool IsOpenCore => _socket?.IsConnected ?? false;

        protected override void CloseCore()
        {
            _socket.Close();
            _socket.Dispose();
            _socket = null;
            _adapter.Disable();
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
                throw new Exception("Device not found.");
            _socket = device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString(((BluetoothDataTransportOption)Option).UUID));
            _socket.Connect();
            _channels.Add(ChannelFactory.Create(_socket.InputStream,_socket.OutputStream));
        }
    }
}