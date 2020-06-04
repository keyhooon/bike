using System;
using System.Net.Sockets;
using Device.Communication.Codec;

namespace Device.Communication.Transport
{
    public class PacketReceivedEventArg : EventArgs
    {
        public PacketReceivedEventArg(Packet packet, Socket socket)
        {
            Packet = packet;
            Socket = socket;
        }
        public Packet Packet { get; }
        public Socket Socket { get; }

    }
}
