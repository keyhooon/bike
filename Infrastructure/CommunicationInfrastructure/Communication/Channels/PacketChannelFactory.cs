using Device.Communication.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using System.IO;

namespace Device.Communication.Channels
{
        public class PacketChannelFactory : ChannelFactory<Packet>
        {

            public PacketChannelFactory() : base(new PacketCodec())
            {

            }

            public override IChannel<Packet> Create(Stream inputStream, Stream outputStream)
            {
            return (new MonitoredChannel<Packet>(new PacketChannel(inputStream, outputStream)));
        }
        }
}
