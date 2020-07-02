using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;

namespace Device.Communication.Codec
{
    public class LightCommand : IFunctionPacket
    {
        public byte LightId { get; set; }
        public bool IsOn { get; set; }
        public byte[] Param
        {
            get
            {
                return new byte[] { LightId,  IsOn ? (byte)0x01 : (byte)0x00 };
            }
            set
            {
                if (value != null && value.Length > 1)
                {
                    LightId = value[0];
                    IsOn = value[1] == (byte)0x01;
                }
            }
        }
        public override string ToString()
        {
            if (IsOn)
                return $"Light Command {{ Light : {LightId}, State : On }}";
            return $"Light Command {{ Light : {LightId}, State : Off }}";
        }
        public static class Encoding
        {
            public static byte Id => 2;
            public static byte ParameterByteCount => 1;
            public static Type PacketType => typeof(LightCommand);


            public static Action<byte[]> ActionToDo { get => null; set => throw new NotImplementedException(); }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new FunctionPacketEncoding<LightCommand>(o, ActionToDo, ParameterByteCount));
        }

    }
}
