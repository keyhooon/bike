using System.IO;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class LightStatePacket : IPacket, IAncestorPacket
    {
        public readonly static byte id = 6;
        public const byte byteCount = 1;
        public byte Id => id;
        public bool Light1 { get; set; }
        public bool Light2 { get; set; }
        public bool Light3 { get; set; }
        public bool Light4 { get; set; }

        public override string ToString()
        {

            return $"Light1 : {Light1}" +
                $", Light2 : {Light2}" +
                $", Light3 : {Light3}" +
                $", Light4 : {Light4}";
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
                var o = (LightSettingPacket)packet;
                byte crc8 = 0;
                byte value;
                value = (byte)((byte)o.Light1 | (byte)o.Light2 << 1 | (byte)o.Light3 << 2 | (byte)o.Light4 << 3);
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
                    return new LightStatePacket
                    {
                        Light1 = (value & 1) == 1,
                        Light2 = (value >> 1 & 1) == 1,
                        Light3 = (value >> 2 & 1) == 1,
                        Light4 = (value >> 3 & 1) == 1
                    };
                return null;
            }
        }

    }

    public static class LightStatePacetEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new LightStatePacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
