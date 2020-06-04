﻿using System;
using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Communication.Codec
{
    public class LightSetting : IPacket, IAncestorPacket
    {

        public byte Light1 { get; set; }
        public byte Light2 { get; set; }
        public byte Light3 { get; set; }
        public byte Light4 { get; set; }

        public override string ToString()
        {

            return $"Light Setting {{ Light1 : {Light1}" +
                $", Light2 : {Light2}" +
                $", Light3 : {Light3}" +
                $", Light4 : {Light4} }}";
        }
        public class Encoding : AncestorPacketEncoding
        {
            public override byte Id => 5;

            public override Type PacketType => typeof(LightSetting);

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
                        Light1 = (byte)(value & 0b11),
                        Light2 = (byte)((value >> 2) & 0b11),
                        Light3 = (byte)((value >> 4) & 0b11),
                        Light4 = (byte)((value >> 6) & 0b11)
                    };
                return null;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
        }

    }

}
