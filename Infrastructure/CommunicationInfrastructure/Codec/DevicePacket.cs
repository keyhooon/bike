using System.Collections.Generic;
using SharpCommunication.Base.Codec.Packets;

namespace Communication.Codec
{
    public class DevicePacket : IPacket, IDescendantPacket
    {

        public static readonly byte[] Header = { 0xAA, 0xAA };

        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"DevicePacket \r\n\t {DescendantPacket?.ToString()} ";
        }
        public static DevicePacket CreateReadCommand(int dataId) => new DevicePacket { DescendantPacket = new CommandPacket { DescendantPacket = new ReadCommand { DataId = (byte)dataId } } };

        public static DevicePacket CreateDataPacket<T>(T packet) where T : IAncestorPacket => new DevicePacket { DescendantPacket = new DataPacket { DescendantPacket = packet } };
        public static DevicePacket CreateCommandPacket<T>(T packet) where T : IFunctionPacket=> new DevicePacket { DescendantPacket = new CommandPacket { DescendantPacket = packet } };

    }
    public static class DevicePacketEncodingHelper
    {
        public static PacketEncodingBuilder CreateDevicePacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList)
        {
            return packetEncodingBuilder.WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(encodingBuiledersList);
        }
        public static PacketEncodingBuilder CreateDevicePacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncoding> encodingsList) 
        {
            return packetEncodingBuilder.WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(encodingsList);
        }
        public static PacketEncodingBuilder CreateDevicePacket(this PacketEncodingBuilder packetEncodingBuilder) 
        {
            return packetEncodingBuilder.WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>();
        }
    }
}
