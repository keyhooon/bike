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
        private PacketEncodingBuilder[] defaultPacketEncodingBuilder = {  };

        public override PacketEncoding Encoding
        {
            get { return _encoding ?? (_encoding= _encodingBuilder.Build()); }
        }


        public DevicePacketCodec(IEnumerable<PacketEncodingBuilder> PacketEncodingBuilderList)
        {
            var packetEncodingGroups = PacketEncodingBuilderList.Select(o => o.Build()).GroupBy((o) => o is IFunctionPacket);
            _encodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder().WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(new[] {
                PacketEncodingBuilder.CreateDefaultBuilder().CreateDataPacket(Enumerable.Concat(
                    defaultPacketEncodingBuilder.Select(o => o.Build()), packetEncodingGroups.FirstOrDefault(o => o.Key))),
                PacketEncodingBuilder.CreateDefaultBuilder().CreateCommandPacket(Enumerable.Concat(
                    defaultCommandEncodingBuilder.Select(o => o.Build()), packetEncodingGroups.FirstOrDefault((o) => o.Key))
                                )});

            //RegisterCommand(packetEncodingGroups.FirstOrDefault((o) => o.Key));
            //RegisterData(packetEncodingGroups.FirstOrDefault(o => o.Key));
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
