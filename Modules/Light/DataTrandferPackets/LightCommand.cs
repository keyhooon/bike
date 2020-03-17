using Communication.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;


namespace Communication.Codec
{
    class LightCommand : IFunctionPacket
    {
        public byte LightId { get; set; }
        public bool IsOn { get; set; }
        public byte[] Param
        {
            get => new[] { LightId, IsOn ? (byte)0x01 : (byte)0x00 };
            set
            {
                if (value != null && value.Length > 1)
                {
                    LightId = value[0];
                    IsOn = value[1] == 0x01;
                }
            }
        }

        public override string ToString()
        {
            if (IsOn)
                return $"Light : {LightId}, State : On";
            return $"Light : {LightId}, State : Off";
        }
        public Action Action => throw new NotImplementedException();

        public static readonly byte ParamByteCount = 2;
        public static readonly byte ID = 2;
        public byte Id => ID;

    }
    public static class LightCommandEncoding
    {
        public static PacketEncodingBuilder CreateBuilder()
        {
            var packetEncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder().WithFunction<LightCommand>(ReadCommand.ParamByteCount, LightCommand.ID);
            return packetEncodingBuilder;
        }
    }
}
