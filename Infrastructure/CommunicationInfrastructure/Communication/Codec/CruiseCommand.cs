using SharpCommunication.Base.Codec.Packets;
using System;


namespace Communication.Codec
{
    class CruiseCommand : IFunctionPacket
    {
        public bool IsOn { get; set; }
        public byte[] Param
        {
            get => new[] { IsOn ? (byte)0x01 : (byte)0x00 };
            set
            {
                if (value != null && value.Length > 0 && value[0] == 0x01)
                    IsOn = true;
            }
        }

        public override string ToString()
        {

            return $"Cruise : {IsOn}";
        }
        public Action Action => throw new NotImplementedException();

        public static readonly byte ParamByteCount = 1;
        public const byte ID = 3;
        public byte Id => ID;
    }
    public static class CruiseCommandEncoding
    {
        public static PacketEncodingBuilder CreateBuilder(this PacketEncodingBuilder packetEncodingBuilder)
        {
            return PacketEncodingBuilder.CreateDefaultBuilder().WithFunction<CruiseCommand>(CruiseCommand.ParamByteCount, CruiseCommand.ID);
        }
    }
}
