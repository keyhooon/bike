
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class ServoInputPacket : IPacket, IAncestorPacket
    {
        public static readonly byte id = 9;
        private const byte ByteCount = 7;
        public byte Id => id;
        public double Throttle { get; set; }
        public double Pedal { get; set; }
        public double Cruise { get; set; }
        public bool IsBreak { get; set; }
        public bool IsFault { get; set; }


        private const double ThrottleBitResolution = 0.0015259021896696d;
        private const double PedalBitResolution = 0.0015259021896696d;
        private const double CruiseBitResolution = 0.0015259021896696d;


        private const double ThrottleBias = 0.0d;
        private const double PedalBias = 0.0d;
        private const double CruiseBias = 0.0d;

        public override string ToString()
        {

            return $"Servo Input - Throttle : {Throttle}, Pedal : {Pedal}, " +
                $"Cruise : {Cruise}, IsBreak : {IsBreak}, IsFault : {IsFault}, ";
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
                var o = (ServoInputPacket)packet;
                var value = BitConverter.GetBytes((ushort)((o.Throttle - ThrottleBias) / ThrottleBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Pedal - PedalBias) / PedalBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Cruise - CruiseBias) / CruiseBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = new[] { (byte)((o.IsBreak ? 0x01 : 0x00) | (o.IsFault ? 0x02 : 0x00)) };
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                byte crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 == reader.ReadByte())
                    return new ServoInputPacket
                    {
                        Throttle = BitConverter.ToUInt16(value, 0) * ThrottleBitResolution + ThrottleBias,
                        Pedal = BitConverter.ToUInt16(value, 2) * PedalBitResolution + PedalBias,
                        Cruise = BitConverter.ToUInt16(value, 4) * CruiseBitResolution + CruiseBias,
                        IsBreak = (value.Skip(6).First() & 0x01) == 0x01,
                        IsFault = (value.Skip(6).First() & 0x02) == 0x02
                    };
                return null;
            }
        }

    }

    public static class ServoInputEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new ServoInputPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
