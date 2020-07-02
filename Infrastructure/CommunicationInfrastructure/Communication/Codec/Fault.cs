using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;
using System.Linq;

namespace Device.Communication.Codec
{
    public class Fault : IPacket, IAncestorPacket
    {

        public bool OverCurrent { get; set; }
        public bool OverTemprature { get; set; }
        public bool PedalSensor { get; set; }
        public bool Throttle { get; set; }
        public bool OverVoltage { get; set; }
        public bool UnderVoltage { get; set; }
        public bool Motor { get; set; }
        public bool Drive { get; set; }

        public override string ToString()
        {

            return $"Fault {{ OverCurrent : {OverCurrent}, " +
                $"OverTemprature : {OverTemprature}, " +
                $"PedalSensor : {PedalSensor}, " +
                $"Throttle : {Throttle}, " +
                $"OverVoltage : {OverVoltage}, " +
                $"UnderVoltage : {UnderVoltage}, " +
                $"Motor : {Motor}, " +
                $"Drive : {Drive} }} ";
        }
        public enum Kind : int
        {
            OverCurrent = 0,
            OverTemprature = 1,
            PedalSensor = 2,
            Throttle = 3,
            OverVoltage = 4,
            UnderVoltage = 5,
            Motor = 6,
            Drive = 7
        }
        public class Encoding : EncodingDecorator
        {
            private readonly static byte byteCount = 2;
            public static Type PacketType => typeof(Fault);

            public static byte Id => 100;

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : base(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (Fault)packet;
                var value = new byte[2];
                value [0]= ((byte)(
                    (o.OverTemprature ?  0x02 : 0x00) | (o.OverCurrent ? 0x01 : 0x00) |
                    (o.Throttle ? 0x08 : 0x00) | (o.PedalSensor ? 0x04 : 0x00) |
                    (o.UnderVoltage ? 0x20 : 0x00) | (o.OverVoltage ? 0x10 : 0x00) |
                    (o.Drive ? 0x80 : 0x00) | (o.Motor ? 0x40 : 0x00)));
                byte crc8 = (byte)(value[0] + value[1]);
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(byteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 == reader.ReadByte())
                    return new Fault
                    {
                        OverCurrent = (value[0] & 0x01) == 0x01,
                        OverTemprature = (value[0] & 0x02) == 0x02,
                        PedalSensor = (value[0] & 0x04) == 0x04,
                        Throttle = (value[0] & 0x08) == 0x08,
                        OverVoltage = (value[0] & 0x10) == 0x10,
                        UnderVoltage = (value[0] & 0x20) == 0x20,
                        Motor = (value[0] & 0x40) == 0x40,
                        Drive = (value[0] & 0x80) == 0x80
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new Encoding(o));
        }

    }
}
