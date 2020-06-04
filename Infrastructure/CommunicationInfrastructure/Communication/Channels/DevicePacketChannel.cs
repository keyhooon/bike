using Device.Communication.Codec;
using SharpCommunication.Channels;
using System.IO;


namespace Device.Communication.Channels
{
    public partial class DevicePacketChannel : Channel<Packet>
    {
        public DevicePacketChannel(Stream stream) : base(new DevicePacketCodec(), stream)
        {



        }
    }
}
