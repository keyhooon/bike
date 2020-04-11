﻿using System.Collections.Generic;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class CommandPacket : IPacket, IDescendantPacket, IAncestorPacket
    {
        public static byte ID = 1;
        public byte Id => ID;

        public IAncestorPacket DescendantPacket { get ; set; }
        public override string ToString()
        {
            return $"Command \r\n\t\t {DescendantPacket?.ToString()} ";
        }
    }
    public static class CommandPacketEncodingHelper
    {
        public static PacketEncodingBuilder CreateCommandPacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList)
        {
            return packetEncodingBuilder.WithAncestor(CommandPacket.ID).WithDescendant<CommandPacket>(encodingBuiledersList);
        }
        public static PacketEncodingBuilder CreateCommandPacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncoding> encodingsList)
        {
            return packetEncodingBuilder.WithAncestor(CommandPacket.ID).WithDescendant<CommandPacket>(encodingsList);
        }
        public static PacketEncodingBuilder CreateCommandPacket(this PacketEncodingBuilder packetEncodingBuilder)
        {
            return packetEncodingBuilder.WithAncestor(CommandPacket.ID).WithDescendant<CommandPacket>();
        }
    }
}
