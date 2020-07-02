﻿using System;
using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Device.Communication.Codec
{
    public class LightState : IPacket, IAncestorPacket
    {

        public bool Light1 { get; set; }
        public bool Light2 { get; set; }
        public bool Light3 { get; set; }
        public bool Light4 { get; set; }

        public override string ToString()
        {

            return $"Light State {{ Light1 : {Light1}" +
                $", Light2 : {Light2}" +
                $", Light3 : {Light3}" +
                $", Light4 : {Light4} }}";
        }
        public class Encoding : EncodingDecorator
        {


            public static byte Id => 6;

            public static Type PacketType => typeof(LightState);

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
                byte value;
                value = (byte)((byte)o.Light1 | (byte)o.Light2 << 1 | (byte)o.Light3 << 2 | (byte)o.Light4 << 3);
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
                    return new LightState
                    {
                        Light1 = (value & 1) == 1,
                        Light2 = (value >> 1 & 1) == 1,
                        Light3 = (value >> 2 & 1) == 1,
                        Light4 = (value >> 3 & 1) == 1
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new Encoding(o));
        }
    }
}
