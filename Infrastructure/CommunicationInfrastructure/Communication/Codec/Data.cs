using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Device.Communication.Codec
{
    public class Data : IPacket, IDescendantPacket, IAncestorPacket
    {

        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"Data {{ {DescendantPacket?.ToString()} }} ";
        }

        public static class Encoding 
        {

            public static byte Id => 0;

            public static Type PacketType => typeof(Data);
            
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new DescendantPacketEncoding<Data>(o));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<PacketEncodingBuilder> encodingBuileders) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new DescendantPacketEncoding<Data>(o, encodingBuileders.Select(o => o.Build()).ToList()));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<EncodingDecorator> encodings) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o=> new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new DescendantPacketEncoding<Data>(o, encodings));
        }
    }
}
