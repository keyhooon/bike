using SharpCommunication.Codec.Encoding;
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Packets;

namespace Device.Communication.Codec
{
    public class ServoInput : IPacket, IAncestorPacket
    {

        public double Throttle { get; set; }
        public double Pedal { get; set; }
        public double Cruise { get; set; }
        public bool IsBreak { get; set; }
        public bool IsFault { get; set; }



        public ServoInput()
        {

        }
        public override string ToString()
        {

            return $"Servo Input {{ Throttle : {Throttle}, Pedal : {Pedal}, " +
                $"Cruise : {Cruise}, IsBreak : {IsBreak} }} ";
        }
        public class Encoding : EncodingDecorator
        {


            private const double V = 0.0015259021896696d;
            private static readonly byte _byteCount = 7;
            private static readonly double _throttleBitResolution = V;
            private static readonly double _pedalBitResolution = V;
            private static readonly double _cruiseBitResolution = V;
            private static readonly double _throttleBias = 0.0d;
            private static readonly double _pedalBias = 0.0d;
            private static readonly double _cruiseBias = 0.0d;

            public static byte Id => 9;

            public static Type PacketType => typeof(ServoInput);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (ServoInput)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Throttle - _throttleBias) / _throttleBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Pedal - _pedalBias) / _pedalBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Cruise - _cruiseBias) / _cruiseBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = new byte[] {(byte)( (o.IsBreak ? 1 : 0) | (o.IsFault ? 2 : 0) )};
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
                    return new ServoInput()
                    {
                        Throttle = BitConverter.ToUInt16(value.Take(2).ToArray()) * _throttleBitResolution + _throttleBias,
                        Pedal = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * _pedalBitResolution + _pedalBias,
                        Cruise = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * _cruiseBitResolution + _cruiseBias,
                        IsBreak = ((value[6] & 0x01) == 1) ? true : false,
                        IsFault = ((value[6] & 0x02) == 2) ? true : false,

                        //IsFault = (value.Skip(7).First() == 1) ? true : false
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(item => new Encoding(item));
        }
    }

}
