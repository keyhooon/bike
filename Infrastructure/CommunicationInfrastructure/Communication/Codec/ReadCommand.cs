using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace Device.Communication.Codec
{
    public class ReadCommand : IFunctionPacket
    {
        public byte DataId { get; set; }


        public byte[] Param
        {
            get
            {
                return new byte[] { DataId };
            }
            set
            {
                if (value != null && value.Length > 0)
                    DataId = value[0];

            }
        }

        public override string ToString()
        {

            return $"Read Command {{ Request Data: {DataId} }}";
        }

        public static class Encoding
        {
            public static byte Id => 1;
            public static byte ParameterByteCount => 1;
            public static Type PacketType => typeof(ReadCommand);
            public static Action<byte[]> ActionToDo { get => null; set => throw new NotImplementedException(); }
            public static PacketEncodingBuilder CreateBuilder()=>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new FunctionPacketEncoding<ReadCommand>(o, ActionToDo, ParameterByteCount));
        }

    }
}
