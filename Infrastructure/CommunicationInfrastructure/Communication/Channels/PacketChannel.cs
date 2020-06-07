using Device.Communication.Codec;
using SharpCommunication.Channels;
using System.IO;


namespace Device.Communication.Channels
{
    public partial class PacketChannel : Channel<Packet>
    {
        public PacketChannel(Stream stream) : base(new PacketCodec(), stream)
        {



        }
    }
}
