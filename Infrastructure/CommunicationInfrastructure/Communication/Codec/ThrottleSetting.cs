using Infrastructure.TypeConverters;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel;
using System.IO;

namespace Device.Communication.Codec
{
    public class ThrottleSetting : IPacket, IAncestorPacket
    {
        public ThrottleActivityType ActivityType {get ; set ;}
        public override string ToString()
        {
            return $"Throttle Setting {{ ThrottleMode : {Enum.GetName(typeof(ThrottleActivityType), ActivityType)} }}";
        }
        public class Encoding : EncodingDecorator
        {

            public static byte Id => 30;

            public static Type PacketType => typeof(ThrottleSetting);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (ThrottleSetting)packet;
                byte crc8 = 0;
                var value = BitConverter.GetBytes((byte)(o.ActivityType));
                for (var i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(1);
                byte crc8 = 0;
                for (var i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new ThrottleSetting()
                    {
                        ActivityType = (ThrottleActivityType)(value[0]) 
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
    PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(item => new Encoding(item));

        }
        [Xamarin.Forms.TypeConverter(typeof(EnumDescriptionTypeConverter))]
        public enum ThrottleActivityType : Byte
        {
            [Description("NORMAL")]
            Normal = 0,
            [Description("SPORT")]
            Sport = 1,
            [Description("OFF")]
            Off = 2,
        }
    }



}
