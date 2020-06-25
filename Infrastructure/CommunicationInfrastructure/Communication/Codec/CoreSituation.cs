using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Device.Communication.Codec
{
    public class CoreSituation : IPacket, IAncestorPacket
    {
        [Display(Name = "Core Temprature")]
        [Editable(false)]
        public double Temprature { get; set; }
        [Display(Name = "Core Voltage")]
        [Editable(false)]
        public double Voltage { get; set; }

        public override string ToString()
        {
            return $"Core Situation {{ Temprature : {Temprature}, Voltage : {Voltage} }} ";
        }
        public CoreSituation()
        {

        }
        public class Encoding : AncestorPacketEncoding
        {
            public static byte ID => 3;

            private static double _tempratureBitResolution = 0.1d;
            private static double _voltageBitResolution = 0.002288488210818;
            private static double _tempratureBias = 0.0d;
            private static double _voltageBias = 0.0d;


            public override byte Id => ID;

            public override Type PacketType => typeof(CoreSituation);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : base(null)
            {

            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (CoreSituation)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Temprature - _tempratureBias) / _tempratureBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - _voltageBias) / _voltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(4);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new CoreSituation()
                    {
                        Temprature = BitConverter.ToUInt16(value.Take(2).ToArray()) * _tempratureBitResolution + _tempratureBias,
                        Voltage = BitConverter.ToUInt16( value.Skip(2).Take(2).ToArray()) * _voltageBitResolution + _voltageBias,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() => 
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
