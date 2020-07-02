using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Device.Communication.Codec
{
    public class Packet : IPacket, IDescendantPacket
    {


        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"DevicePacket {{ {DescendantPacket?.ToString()} }}";
        }
        public static class Encoding  
        {
            private static readonly byte[] Header = { 0xAA, 0xAA };

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new HeaderPacketEncoding(o, Header)).AddDecorate(o => new DescendantPacketEncoding<Packet>(o));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<PacketEncodingBuilder> encodingBuileders) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o =>new HeaderPacketEncoding(o, Header)).AddDecorate(o => new DescendantPacketEncoding<Packet>(o, encodingBuileders.Select(o => o.Build()).ToList()));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<EncodingDecorator> encodings) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new HeaderPacketEncoding(o, Header)).AddDecorate(o => new DescendantPacketEncoding<Packet>(o, encodings));
        }

    }
}
