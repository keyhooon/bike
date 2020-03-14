using System.IO;
using Communication.Codec;
using SharpCommunication.Base.Channels;

namespace Communication.Channels
{
    public partial class DevicePacketChannel : Channel<DevicePacket>
    {
        public DevicePacketChannel(Stream stream) : base(new DevicePacketCodec(), stream)
        {



        }
    }
}
