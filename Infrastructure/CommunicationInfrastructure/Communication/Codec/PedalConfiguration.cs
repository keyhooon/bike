﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Device.Communication.Codec
{
    public class PedalConfiguration : IPacket, IAncestorPacket
    {
        [Display(Name = "Number of Magnet", Prompt = "Enter Number of Magnet", Description = "Magnet Count that Used in Motor")]
        [Editable(false)]
        public byte MagnetCount { get; set; }

        public override string ToString()
        {

            return $"Pedal Configuration {{ MagnetCount : {MagnetCount} }}";
        }
        public class Encoding : AncestorPacketEncoding
        {
            public static byte ID => 7;

            public override byte Id => ID;

            public override Type PacketType => typeof(PedalConfiguration);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (PedalConfiguration)packet;
                byte crc8 = 0;
                var value = o.MagnetCount;
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
                    return new PedalConfiguration
                    {
                        MagnetCount = value
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));

        }

    }

}

