﻿using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Device.Communication.Codec
{
    public class BatteryOutput : IPacket, IAncestorPacket
    {
        [Display(Name = "Battery Current")]
        [Editable(false)]
        public double Current { get; set; }
        [Display(Name = "Battery Voltage")]
        [Editable(false)]
        public double Voltage { get; set; }
        [Display(Name = "Battery Temprature")]
        [Editable(false)]
        public double Temprature { get; set; }

        public BatteryOutput()
        {

        }

        public override string ToString()
        {

            return $"Battery Output {{ Current : {Current}, Voltage : {Voltage}, " +
                $"Temprature : {Temprature} }} ";
        }


        public class Encoding : AncestorPacketEncoding
        {
            public static byte ID => 2;

            private static readonly byte _byteCount = 6;
            private static readonly double _currentBitResolution = 0.125d;
            private static readonly double _voltageBitResolution = 0.25d;
            private static readonly double _tempratureBitResolution = 0.125d;
            private static readonly double _currentBias = 0.0d;
            private static readonly double _voltageBias = 20.0d;
            private static readonly double _tempratureBias = 0.0d;

            public override Type PacketType => typeof(BatteryOutput);

            public override byte Id => ID;

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding(): this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (BatteryOutput)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Current - _currentBias) / _currentBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - _voltageBias) / _voltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Temprature - _tempratureBias) / _tempratureBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(_byteCount);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new BatteryOutput()
                    {
                        Current = BitConverter.ToUInt16(value.Take(2).ToArray()) * _currentBitResolution + _currentBias,
                        Voltage = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * _voltageBitResolution + _voltageBias,
                        Temprature = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * _tempratureBitResolution + _tempratureBias,
                    };
                return null;
            }

            public static PacketEncodingBuilder CreateBuilder() => 
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}