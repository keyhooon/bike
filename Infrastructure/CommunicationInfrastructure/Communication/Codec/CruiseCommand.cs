using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace Device.Communication.Codec
{
    public class CruiseCommand : IFunctionPacket
    {
        public bool IsOn { get; set; }
        public byte[] Param { 
            get {
                return new byte[] { IsOn ? (byte)0x01 : (byte)0x00 };
            } 
            set {
                if (value != null && value.Length > 0 && value[0] == (byte)0x01)
                    IsOn = true;
            }
        }
        public override string ToString()
        {

            return $"Cruise Command {{ Cruise : {IsOn} }}";
        }

        public static class Encoding 
        {
            public static byte Id => 3;
            public static byte ParameterByteCount => 1;
            public static Type PacketType => typeof(CruiseCommand);

            public static Action<byte[]> ActionToDo { get => null; set => throw new NotImplementedException(); }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding(o, Id, PacketType)).AddDecorate(o => new FunctionPacketEncoding<CruiseCommand>(o, ActionToDo, ParameterByteCount));
        }
    }
}
