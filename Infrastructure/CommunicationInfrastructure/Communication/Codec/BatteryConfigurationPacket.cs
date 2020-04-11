
using System;
using System.IO;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class BatteryConfigurationPacket : IPacket, IAncestorPacket
    {
        public readonly static byte id = 1;
        public const byte byteCount = 10;
        public byte Id => id;
        public double OverCurrent { get; set; }
        public double OverVoltage { get; set; }
        public double UnderVoltage { get; set; }
        public double NominalVoltage { get; set; }
        public double OverTemprature { get; set; }

        private static readonly double overCurrentBitResolution = CurrentCalibrationHelper.Gain;
        private static readonly double overVoltageBitResolution = VoltageCalibrationHelper.Gain;
        private static readonly double underVoltageBitResolution = VoltageCalibrationHelper.Gain;
        private static readonly double nominalVoltageBitResolution = VoltageCalibrationHelper.Gain;
        private static readonly double overTempratureBitResolution = 1;

        private static readonly double overCurrentBias = CurrentCalibrationHelper.Bias;
        private static readonly double overVoltageBias = VoltageCalibrationHelper.Bias;
        private static readonly double underVoltageBias = VoltageCalibrationHelper.Bias;
        private static readonly double nominalVoltageBias = VoltageCalibrationHelper.Bias;
        private static readonly double overTempratureBias = 0.0d;

        public override string ToString()
        {

            return $"OverCurrent : {OverCurrent}, OverVoltage : {OverVoltage}, " +
                $"UnderVoltage : {UnderVoltage}, NominalVoltage : {NominalVoltage}, " +
                $"OverTemprature : {OverTemprature}, ";
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
                var o = (BatteryConfigurationPacket)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.OverCurrent - overCurrentBias) / overCurrentBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverVoltage - overVoltageBias) / overVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.UnderVoltage - underVoltageBias) / underVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.NominalVoltage - nominalVoltageBias) / nominalVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverTemprature - overTempratureBias) / overTempratureBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(byteCount);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new BatteryConfigurationPacket
                    {
                        OverCurrent = BitConverter.ToUInt16(value, 0) * overCurrentBitResolution + overCurrentBias,
                        OverVoltage = BitConverter.ToUInt16(value, 2) * overVoltageBitResolution + overVoltageBias,
                        UnderVoltage = BitConverter.ToUInt16(value, 4) * underVoltageBitResolution + underVoltageBias,
                        NominalVoltage = BitConverter.ToUInt16(value, 6) * nominalVoltageBitResolution + nominalVoltageBias,
                        OverTemprature = BitConverter.ToUInt16(value, 8) * overTempratureBitResolution + overTempratureBias
                    };
                return null;
            }
        }

    }

    public static class BatteryConfigurationEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new BatteryConfigurationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
