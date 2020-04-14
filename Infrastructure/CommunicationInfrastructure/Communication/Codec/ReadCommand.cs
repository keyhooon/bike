using System;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class ReadCommand : IFunctionPacket
    {
        public byte DataId { get; set; }
        public byte[] Param
        {
            get => new[] { DataId };
            set
            {
                if (value != null && value.Length > 0)
                    DataId = value[0];

            }
        }
        public override string ToString()
        {

            return $"Request Data: {DataId}";
        }
        public Action Action => throw new NotImplementedException();

        public const byte ParamByteCount = 1;
        public const byte id = 1;
        public byte Id => id;
    }
    public static class ReadCommandEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            return PacketEncodingBuilder.CreateDefaultBuilder().WithFunction<ReadCommand>(ReadCommand.ParamByteCount, ReadCommand.id);
        }
    }
}
