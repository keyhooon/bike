using System;
using System.ComponentModel;
using System.IO;
using Infrastructure.TypeConverters;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Device.Communication.Codec
{
    public class LightSetting : IPacket, IAncestorPacket
    {

        public LightVolume Light1 { get; set; }
        public LightVolume Light2 { get; set; }
        public LightVolume Light3 { get; set; }
        public LightVolume Light4 { get; set; }

        public override string ToString()
        {

            return $"Light Setting {{ Light1 : {Enum.GetName(typeof(LightVolume), Light1)}" +
                $", Light2 : {Enum.GetName(typeof(LightVolume), Light2)}" +
                $", Light3 : {Enum.GetName(typeof(LightVolume), Light3)}" +
                $", Light4 : {Enum.GetName(typeof(LightVolume), Light4)} }}";
        }
        public class Encoding : EncodingDecorator
        {

            public static byte Id => 5;

            public static Type PacketType => typeof(LightSetting);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (LightSetting)packet;
                byte crc8 = 0;
                var value = (byte)((byte)o.Light1 | (byte)o.Light2 << 2 | (byte)o.Light3 << 4 | (byte)o.Light4 << 6);
                crc8 += value;
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadByte();
                byte crc8 = 0;
                crc8 += value;
                if (crc8 == reader.ReadByte())
                    return new LightSetting
                    {
                        Light1 = (LightVolume)(value & 0b11),
                        Light2 = (LightVolume)((value >> 2) & 0b11),
                        Light3 = (LightVolume)((value >> 4) & 0b11),
                        Light4 = (LightVolume)((value >> 6) & 0b11)
                    };
                return null;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new Encoding(o));
        }
        [TypeConverter(typeof(EnumDescriptionTypeConverter))]
        public enum LightVolume : byte
        {
            ExtraLow = 0,
            Low = 1,
            Normal = 2,
            High = 3,
        }

    }

}
