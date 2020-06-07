using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Device.Communication.Codec
{
    public class CoreConfiguration : IPacket, IAncestorPacket
    {
        [Display(Name = "ID")]
        [Editable(false)]
        public string UniqueId { get; set; }
        [Display(Name = "Firmware Version")]
        [Editable(false)] 
        public string FirmwareVersion{ get; set; }
        [Display(Name = "Model Number")]
        [Editable(false)] 
        public string ModelVersion { get; set; }

        public override string ToString()
        {

            return $"Core Configuration {{ UniqueId : {UniqueId}, FirmwareVersion : {FirmwareVersion}, " +
                $"ModelVersion : {ModelVersion} }}";
        }
        public class Encoding : AncestorPacketEncoding
        {

            public static byte ID => 4;



            public override byte Id => ID;

            public override Type PacketType => typeof(CoreConfiguration);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : base(null)
            {

            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (CoreConfiguration)packet;
                byte crc8 = 0;
                byte[] value;
                value = o.UniqueId.ToByteArray();
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = o.FirmwareVersion.ToByteArray();
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = o.ModelVersion.ToByteArray();
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(16);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new CoreConfiguration()
                    {
                        UniqueId = value.Take(12).ToHexString(),
                        FirmwareVersion = value.Skip(12).Take(2).ToHexString(),
                        ModelVersion = value.Skip(14).Take(2).ToHexString(),
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
            
        }

    }
}