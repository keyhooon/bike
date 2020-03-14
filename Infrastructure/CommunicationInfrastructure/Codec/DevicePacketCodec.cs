using System.Collections.Generic;

using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.Linq;
namespace Communication.Codec
{
    public class DevicePacketCodec : Codec<DevicePacket>
    {
 
        private readonly PacketEncodingBuilder _encodingBuilder;

        private PacketEncoding _encoding;

        private PacketEncodingBuilder[] defaultCommandEncodingBuilder = { PacketEncodingBuilder.CreateDefaultBuilder().CreateReadCommand() };


        public override PacketEncoding Encoding
        {
            get { return _encoding ?? (_encoding= _encodingBuilder.Build()); }
        }


        public DevicePacketCodec()
        {

            _encodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder().WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(new[] {
                PacketEncodingBuilder.CreateDefaultBuilder().CreateDataPacket(),
                PacketEncodingBuilder.CreateDefaultBuilder().CreateCommandPacket(
                                defaultCommandEncodingBuilder.Select(o=>o.Build())
                                )});
        }


        public void RegisterCommand(PacketEncoding enc) 
        {
            var commandEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[CommandPacket.ID].FindDecoratedEncoding<DescendantPacketEncoding<CommandPacket>>();
            commandEncoding.Register(enc);
        }
        public void RegisterCommand(IEnumerable<PacketEncoding> encs) 
        {
            var commandEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[CommandPacket.ID].FindDecoratedEncoding<DescendantPacketEncoding<CommandPacket>>();
            foreach (var enc in encs)
            {
                commandEncoding.Register(enc);
            }
        }

        public void RegisterData(PacketEncoding enc) 
        {
            var dataEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[DataPacket.ID];
            dataEncoding.FindDecoratedEncoding<DescendantPacketEncoding<DataPacket>>().Register(enc);
        }
        public void RegisterData(IEnumerable<PacketEncoding> encs)
        {
            var dataEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[DataPacket.ID]
                .FindDecoratedEncoding<DescendantPacketEncoding<DataPacket>>();
            foreach (var enc in encs)
            {
                dataEncoding.Register(enc);
            }
        }
    }
}
