using System.IO;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class PedalConfigurationPacket : IPacket, IAncestorPacket
    {
        public readonly static byte id = 7;
        public const byte byteCount = 1;
        public byte Id => id;
        public byte MagnetCount { get; set; }

        public override string ToString()
        {

            return $"MagnetCount : {MagnetCount}";
        }
        public class Encoding : AncestorPacketEncoding
        {

            public Encoding(PacketEncoding encoding) : base(encoding, id)
            {

            }
            public Encoding() : base(null, id)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (PedalConfigurationPacket)packet;
                byte crc8 = 0;
                var value = o.MagnetCount;
                crc8 += value;
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadByte();
                byte crc8 = 0;
                crc8 += value;
                if (crc8 == reader.ReadByte())
                    return new PedalConfigurationPacket
                    {
                        MagnetCount = value
                    };
                return null;
            }
        }

    }

    public static class PedalConfigurationEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new PedalConfigurationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}

