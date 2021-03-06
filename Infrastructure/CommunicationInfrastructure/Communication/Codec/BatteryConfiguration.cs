﻿
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System.ComponentModel.DataAnnotations;

namespace Device.Communication.Codec
{
    public class BatteryConfiguration: IPacket, IAncestorPacket
    {
        [Display(Name = "Over Current", Prompt = "Enter Over Current", Description = "Max threshold for Current")]
        [Editable(false)]
        public double OverCurrent { get; set; }

        [Display(Name = "Over Voltage", Prompt = "Enter Over Voltage", Description = "Max threshold for Voltage")]
        [Editable(false)] 
        public double OverVoltage { get; set; }
        [Display(Name = "Under Voltage", Prompt = "Enter Under Voltage", Description = "Min threshold for Voltage")]
        [Editable(false)]
        public double UnderVoltage { get; set; }
        [Display(Name = "Nominal Voltage", Prompt = "Enter Nominal Voltage", Description = "Nominal Voltage Value")]
        [Editable(false)]
        public double NominalVoltage { get; set; }
        [Display(Name = "Over Temprature", Prompt = "Enter Over Temprature", Description = "Maximum Threshold for Temprature")]
        [Editable(false)]
        public double OverTemprature { get; set; }

        public BatteryConfiguration()
        {

        }
        public override string ToString()
        {

            return $"Battery Configuration {{ OverCurrent : {OverCurrent}, OverVoltage : {OverVoltage}, " +
                $"UnderVoltage : {UnderVoltage}, NominalVoltage : {NominalVoltage}, " +
                $"OverTemprature : {OverTemprature} }} ";
        }
        public class Encoding : EncodingDecorator
        {
            private static readonly double _overCurrentBitResolution = CurrentCalibrationHelper.Gain;
            private static readonly double _overVoltageBitResolution = VoltageCalibrationHelper.Gain;
            private static readonly double _underVoltageBitResolution = VoltageCalibrationHelper.Gain;
            private static readonly double _nominalVoltageBitResolution = VoltageCalibrationHelper.Gain;
            private static readonly double _overTempratureBitResolution = 1;
            private static readonly double _overCurrentBias = CurrentCalibrationHelper.Bias;
            private static readonly double _overVoltageBias = VoltageCalibrationHelper.Bias;
            private static readonly double _underVoltageBias = VoltageCalibrationHelper.Bias;
            private static readonly double _nominalVoltageBias = VoltageCalibrationHelper.Bias;
            private static readonly double _overTempratureBias = 0;
            private static readonly byte _byteCount = 10;

            public static Type PacketType => typeof(BatteryConfiguration);

            public static byte Id => 1;

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {
  
            }
            public Encoding() : this(null)
            {

            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (BatteryConfiguration)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.OverCurrent - _overCurrentBias) / _overCurrentBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverVoltage - _overVoltageBias) / _overVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.UnderVoltage - _underVoltageBias) / _underVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.NominalVoltage - _nominalVoltageBias) / _nominalVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverTemprature - _overTempratureBias) / _overTempratureBitResolution));
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
                    return new BatteryConfiguration()
                    {
                        OverCurrent = BitConverter.ToUInt16(value.Take(2).ToArray()) * _overCurrentBitResolution + _overCurrentBias,
                        OverVoltage = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * _overVoltageBitResolution + _overVoltageBias,
                        UnderVoltage = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * _underVoltageBitResolution + _underVoltageBias,
                        NominalVoltage = BitConverter.ToUInt16(value.Skip(6).Take(2).ToArray()) * _nominalVoltageBitResolution + _nominalVoltageBias,
                        OverTemprature = BitConverter.ToUInt16(value.Skip(8).Take(2).ToArray()) * _overTempratureBitResolution + _overTempratureBias,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(item => new Encoding(item));
        }
        public static class CurrentCalibrationHelper
        {
            public readonly static double V_Bias = 1.27d;
            public readonly static double ADC_Max = 4095;
            public readonly static double V_Max = 3.3;
            public readonly static double Coeficient = 0.006;
            public static double Gain => -V_Max / (ADC_Max * Coeficient);
            public static double Bias => V_Bias / Coeficient;
        }
        public static class VoltageCalibrationHelper
        {
            public readonly static double ADC_Max = 4095;
            public readonly static double V_Max = 3.3;
            public readonly static double Coeficient = 60 / 3.3;
            public static double Gain => V_Max * Coeficient / ADC_Max;
            public static double Bias => 0;
        }
    }
}
