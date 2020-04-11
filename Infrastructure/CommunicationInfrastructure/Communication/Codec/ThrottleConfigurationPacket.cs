
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class ThrottleConfigurationPacket : IPacket, IAncestorPacket
    {
        public static readonly byte id = 10;
        private const byte ByteCount = 6;
        public byte Id => id;
        public double FaultThreshold { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        private const double FaultThresholdBitResolution = 8.0586080586080586080586080586081e-4;
        private const double MinBitResolution = 8.0586080586080586080586080586081e-4;
        private const double MaxBitResolution = 8.0586080586080586080586080586081e-4;


        private const double FaultThresholdBias = 0.0d;
        private const double MinBias = 0.0d;
        private const double MaxBias = 0.0d;

        public override string ToString()
        {

            return $"Throttle Configuration - FaultThreshold : {FaultThreshold}v, Min : {Min}v, " +
                $"Max : {Max}v";
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
                var o = (ThrottleConfigurationPacket)packet;
                var value = BitConverter.GetBytes((ushort)((o.FaultThreshold - FaultThresholdBias) / FaultThresholdBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Min - MinBias) / MinBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Max - MaxBias) / MaxBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new ThrottleConfigurationPacket
                    {
                        FaultThreshold = BitConverter.ToUInt16(value, 0) * FaultThresholdBitResolution + FaultThresholdBias,
                        Min = BitConverter.ToUInt16(value, 2) * MinBitResolution + MinBias,
                        Max = BitConverter.ToUInt16(value, 4) * MaxBitResolution + MaxBias  
                    };
                return null;
            }
        }

    }

    public static class ThrottleConfigurationEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new ThrottleConfigurationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}