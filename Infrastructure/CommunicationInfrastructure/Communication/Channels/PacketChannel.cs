using Device.Communication.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using System.IO;


namespace Device.Communication.Channels
{
    public partial class PacketChannel : Channel<Packet>
    { 
        public PacketChannel(Stream inputstream, Stream outputstream) : base(new PacketCodec(), inputstream, outputstream)
        {

        }
        public MonitoredChannel<Packet> ToMonitoredChannel() 
        {
            return ToMonitoredChannel();
        }
    }
}
