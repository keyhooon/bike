using System;
using System.IO;
using System.Linq;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class CoreSituationPacket : IPacket, IAncestorPacket
    {
        public static byte id = 3;
        private const byte ByteCount = 4;
        public byte Id => id;
        public double Temprature { get; set; }
        public double Voltage { get; set; }


        private const double TempratureBitResolution = 0.1d;
        private const double VoltageBitResolution = 0.002288488210818;


        private const double TempratureBias = 0.0d;
        private const double VoltageBias = 0.0d;

        public override string ToString()
        {
            return $"Temprature : {Temprature}, Voltage : {Voltage} ";
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
                var o = (CoreSituationPacket)packet;
                var value = BitConverter.GetBytes((ushort)((o.Temprature - TempratureBias) / TempratureBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - VoltageBias) / VoltageBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }


            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new CoreSituationPacket
                    {
                        Temprature = BitConverter.ToUInt16(value, 0) * TempratureBitResolution + TempratureBias,
                        Voltage = BitConverter.ToUInt16(value, 2) * VoltageBitResolution + VoltageBias
                    };
                return null;
            }

        }

    }

    public static class CoreSituationEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new CoreSituationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
