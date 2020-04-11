using System.IO;
using System.Linq;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class CoreConfigurationPacket : IPacket, IAncestorPacket
    {
        public static byte id = 4;
        public static byte ByteCount = 16;
        public byte Id => id;
        public string UniqueId { get; set; }
        public string FirmwareVersion{ get; set; }
        public string ModelVersion { get; set; }


        public override string ToString()
        {

            return $"UniqueId : {UniqueId}, FirmwareVersion : {FirmwareVersion}, " +
                $"ModelVersion : {ModelVersion}";
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
                var o = (CoreConfigurationPacket)packet;
                var value = o.UniqueId.ToByteArray().Reverse().ToArray();
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = o.FirmwareVersion.ToByteArray().Reverse().ToArray();
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = o.ModelVersion.ToByteArray().Reverse().ToArray();
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(16);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new CoreConfigurationPacket
                    {
                        UniqueId = value.Take(12).Reverse().ToHexString(),
                        FirmwareVersion = value.Skip(12).Take(2).Reverse().ToHexString(),
                        ModelVersion = value.Skip(14).Take(2).Reverse().ToHexString()
                    };
                return null;
            }
        }

    }

    public static class CoreConfigurationEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder();
            packetEncodingBuilder.SetupActions.Add(item => new CoreConfigurationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}