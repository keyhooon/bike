using System;
using System.IO;
using System.Linq;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class BatteryOutputPacket : IPacket, IAncestorPacket
    {
        public static readonly byte id = 2;
        public static readonly byte byteCount = 6;
        public byte Id => id;
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double Temprature { get; set; }

        private static readonly double CurrentBitResolution = CurrentCalibrationHelper.Gain;
        private static readonly double VoltageBitResolution = VoltageCalibrationHelper.Gain;
        private const double TempratureBitResolution = 1.0d;

        private static readonly double CurrentBias = CurrentCalibrationHelper.Bias;
        private static readonly double VoltageBias = VoltageCalibrationHelper.Bias;
        private const double TempratureBias = 0.0d;

        public override string ToString()
        {

            return $"Current : {Current}, Voltage : {Voltage}, " +
                $"Temprature : {Temprature} ";
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
                var o = (BatteryOutputPacket)packet;
                var value = BitConverter.GetBytes((ushort)((o.Current - CurrentBias) / CurrentBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - VoltageBias) / VoltageBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Temprature - TempratureBias) / TempratureBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(byteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 == reader.ReadByte())
                    return new BatteryOutputPacket
                    {
                        Current = BitConverter.ToUInt16(value, 0) * CurrentBitResolution + CurrentBias,
                        Voltage = BitConverter.ToUInt16(value, 2) * VoltageBitResolution + VoltageBias,
                        Temprature = BitConverter.ToUInt16(value, 4) * TempratureBitResolution + TempratureBias
                    };
                return null;
            }

        }

    }

    public static class BatteryOutputEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new BatteryOutputPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
