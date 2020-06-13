using Infrastructure.TypeConverters;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Device.Communication.Codec
{
    public class PedalSetting : IPacket, IAncestorPacket
    {
        [Display(Name = "Level of Assist", Prompt = "Enter Level of Assist for Pedal", Description = "Level of Assist; If you want higher speed when Pedaling set High level for Pedal Assist")]
        public PedalAssistLevelType AssistLevel { get; set; }
        [Display(Name = "Sensivity of Assist", Prompt = "Enter Sensivity of Assist for Pedal", Description = "Sensivity of Pedal Assist.")]
        public PedalActivationTimeType ActivationTime { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }

        public override string ToString()
        {

            return $"Pedal Setting {{ AssistLevel : {Enum.GetName(typeof(PedalAssistLevelType), AssistLevel)}, ActivationTime : {Enum.GetName(typeof(PedalActivationTimeType), ActivationTime)}, " +
                $"LowLimit : {LowLimit}, HighLimit : {HighLimit} }}";
        }
        public class Encoding : AncestorPacketEncoding
        {
            private static readonly byte ByteCount = 5;
            public static byte ID => 8;

            public override byte Id => ID;

            public override Type PacketType => typeof(PedalSetting);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (PedalSetting)packet;
                byte crc8 = 0;
                var value = new byte[] { (byte)((byte)o.AssistLevel | (byte)o.ActivationTime << 3) };
                crc8 += value[0];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)o.LowLimit);
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)o.HighLimit);
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));

                if (crc8 == reader.ReadByte())
                    return new PedalSetting
                    {
                        AssistLevel = (PedalAssistLevelType)((byte)(value.First() & 0b111)),
                        ActivationTime = (PedalActivationTimeType)((byte)(value.First() >> 3 & 0b11)),
                        LowLimit = BitConverter.ToUInt16(value, 1),
                        HighLimit = BitConverter.ToUInt16(value, 3)
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));


        }
        [Xamarin.Forms.TypeConverter(typeof(EnumDescriptionTypeConverter))]
        public enum PedalAssistLevelType : byte
        {
            [Description("OFF  ")]
            Off = 0,
            [Description("25  %")]
            TowentyFive = 1,
            [Description("31.2%")]
            ThrityOnePointTwentyFive = 2,
            [Description("37.5%")]
            ThirtySevenPointFive = 3,
            [Description("50  %")]
            Fifty = 4,
            [Description("62.5%")]
            SixtyTwoPointFive = 5,
            [Description("75  %")]
            SeventyFive = 6,
            [Description("87.5%")]
            EightySevenPointFive = 7,
           
        }
        [Xamarin.Forms.TypeConverter(typeof(EnumDescriptionTypeConverter))]
        public enum PedalActivationTimeType : byte
        {
            [Description("EXTRA")]
            ExteraSensitive = 0,
            [Description("HIGH ")]
            HighSensitive = 1,
            [Description("NORM ")]
            NormalSensitive = 2,
            [Description("LOW  ")]
            LowSensitive = 3,
        }
    }

}

