using System;
using System.IO;
using System.Linq;
using SharpCommunication.Base.Codec.Packets;

namespace Servo.DataTrandferPackets
{
    public class ServoOutputPacket : IPacket, IAncestorPacket
    {
        public static readonly byte id = 50;
        private const byte ByteCount = 5;
        public byte Id => id;
        public double ActivityPercent { get; set; }
        public double WheelSpeed { get; set; }


        private const double ActivityPercentBitResolution = 0.0015259021896696d;
        private const double WheelSpeedBitResolution = 0.000244140625 / 60;


        private const double ActivityPercentBias = 0.0d;
        private const double WheelSpeedBias = 0.0d;

        public override string ToString()
        {

            return $"Servo Output - ActivityPercent : {ActivityPercent}, WheelSpeed : {WheelSpeed}, ";
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
                var o = (ServoOutputPacket)packet;
                var value = BitConverter.GetBytes((ushort)((o.ActivityPercent - ActivityPercentBias) / ActivityPercentBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.WheelSpeed - WheelSpeedBias) / WheelSpeedBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                value = new byte[] { 0 };
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 == reader.ReadByte())
                    return new ServoOutputPacket
                    {
                        ActivityPercent = BitConverter.ToUInt16(value, 0) * ActivityPercentBitResolution + ActivityPercentBias,
                        WheelSpeed = 1 / (BitConverter.ToUInt16(value, 2) * WheelSpeedBitResolution + WheelSpeedBias)
                    };
                return null;
            }
        }

    }

    public static class ServoOutputEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new ServoOutputPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
