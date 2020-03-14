using System;
using System.IO;
using SharpCommunication.Base.Codec.Packets;

namespace Light.DataTrandferPackets
{
    public class LightSettingPacket : IPacket, IAncestorPacket
    {
        public readonly static byte id = 5;
        public const byte byteCount = 1;
        public byte Id => id;
        public byte Light1 { get; set; }
        public byte Light2 { get; set; }
        public byte Light3 { get; set; }
        public byte Light4 { get; set; }

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
                var value = (byte)((byte)o.Light1 | (byte)o.Light2 << 2 | (byte)o.Light3 << 4 | (byte)o.Light4 << 6);
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
                    return new LightSettingPacket
                    {
                        Light1 = (byte)(value & 0b11),
                        Light2 = (byte)((value >> 2) & 0b11),
                        Light3 = (byte)((value >> 4) & 0b11),
                        Light4 = (byte)((value >> 6) & 0b11)
                    };
                return null;
            }
        }


    }

    public static class LightSettingPacketEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new LightSettingPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
