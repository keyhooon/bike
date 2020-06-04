using Device.Communication.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using System.IO;

namespace Device.Communication.Channels
{
        public class DevicePacketChannelFactory : ChannelFactory<Packet>
        {

            public DevicePacketChannelFactory() : base(new DevicePacketCodec())
            {

            }

            public override IChannel<Packet> Create(Stream stream)
            {
                return (new CachedChannel<Packet>( new MonitoredChannel<Packet>( new DevicePacketChannel(stream) ) ));
            }

        }
}
