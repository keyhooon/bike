using System.Collections.Generic;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class DataPacket : IPacket, IDescendantPacket, IAncestorPacket
    {
        public static byte ID = 0;
        public byte Id => ID;

        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"Data \r\n\t\t {DescendantPacket?.ToString()} ";
        }
    }
    public static class DataPacketEncodingHelper
    {
        public static PacketEncodingBuilder CreateDataPacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList)
        {
            return packetEncodingBuilder.WithAncestor(DataPacket.ID).WithDescendant<DataPacket>(encodingBuiledersList);
        }
        public static PacketEncodingBuilder CreateDataPacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncoding> encodingsList)
        {
            return packetEncodingBuilder.WithAncestor(DataPacket.ID).WithDescendant<DataPacket>(encodingsList);
        }
        public static PacketEncodingBuilder CreateDataPacket(this PacketEncodingBuilder packetEncodingBuilder)
        {
            return packetEncodingBuilder.WithAncestor(DataPacket.ID).WithDescendant<DataPacket>();
        }
    }
}
