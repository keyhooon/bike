using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class PacketReceivedEventArg : EventArgs
    {
        public PacketReceivedEventArg(IAncestorPacket packet)
        {
            Packet = packet;
        }

        public IAncestorPacket Packet { get; }
    }
}
