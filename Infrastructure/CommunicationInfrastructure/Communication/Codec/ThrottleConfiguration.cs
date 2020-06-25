﻿
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Device.Communication.Codec
{
    public class ThrottleConfiguration : IPacket, IAncestorPacket
    {

        [Display(Name = "Throttle Threshold", Prompt = "Enter Threshold Limit for Throttle Voltage", Description = "Threshold Limit for Throttle Voltage")]
        [Editable(false)]
        public double FaultThreshold { get; set; }
        [Display(Name = "Throttle Low Limit Voltage", Prompt = "Enter Minimum Limit for Throttle Voltage", Description = "Minimum Limit for Throttle Voltage")]
        [Editable(false)]
        public double Min { get; set; }
        [Display(Name = "Throttle High Limit Voltage", Prompt = "Enter Maximum Limit for Throttle Voltage", Description = "Minimum Limit for Throttle Voltage")]
        [Editable(false)]
        public double Max { get; set; }



        public ThrottleConfiguration()
        {

        }
        public override string ToString()
        {

            return $"Throttle Configuration {{ FaultThreshold : {FaultThreshold}v, Min : {Min}v, " +
                $"Max : {Max}v }}";
        }
        public class Encoding : AncestorPacketEncoding
        {
            public static byte ID => 10;

            private static readonly double _faultThresholdBitResolution = 8.0586080586080586080586080586081e-4;
            private static readonly double _minBitResolution = 8.0586080586080586080586080586081e-4;
            private static readonly double _maxBitResolution = 8.0586080586080586080586080586081e-4;
            private static readonly double _faultThresholdBias = 0.0d;
            private static readonly double _minBias = 0.0d;
            private static readonly double _maxBias = 0.0d;

            public override byte Id => ID;

            public override Type PacketType => typeof(ThrottleConfiguration);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (ThrottleConfiguration)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.FaultThreshold - _faultThresholdBias) / _faultThresholdBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Min - _minBias) / _minBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Max - _maxBitResolution) / _maxBias));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(6);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new ThrottleConfiguration()
                    {
                        FaultThreshold = BitConverter.ToUInt16(value.Take(2).ToArray()) * _faultThresholdBitResolution + _faultThresholdBias,
                        Min = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * _minBitResolution + _minBias,
                        Max = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * _maxBias + _maxBitResolution,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
    PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));

        }

    }
}