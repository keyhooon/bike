using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DynamicData;
using System;
using Xamarin.Forms.Internals;

namespace Device.Communication.Codec
{
    public class PacketCodec : Codec<Packet>
    {
        private readonly PacketEncodingBuilder EncodingBuilder;
        private EncodingDecorator encoding;
        private Dictionary<Type, AncestorPacketEncoding> _ancestorPacketEncodings;
        public IReadOnlyDictionary<Type, AncestorPacketEncoding> AncestorPacketEncodings { get; }

        public event EventHandler EncodingCreated;
        private readonly List<PacketEncodingBuilder> _defaultCommandPacketEncodingBuilders = new List<PacketEncodingBuilder>(
            new [] 
            {
                CruiseCommand.Encoding.CreateBuilder(),
                LightCommand.Encoding.CreateBuilder(),
                ReadCommand.Encoding.CreateBuilder()
            });

        private readonly List<PacketEncodingBuilder> _defaultDataPacketEncodingBuilders = new List<PacketEncodingBuilder>(
            new[]
            {
                BatteryConfiguration.Encoding.CreateBuilder(),
                BatteryOutput.Encoding.CreateBuilder(),
                CoreConfiguration.Encoding.CreateBuilder(),
                CoreSituation.Encoding.CreateBuilder(),
                Fault.Encoding.CreateBuilder(),
                LightSetting.Encoding.CreateBuilder(),
                LightState.Encoding.CreateBuilder(),
                PedalConfiguration.Encoding.CreateBuilder(),
                PedalSetting.Encoding.CreateBuilder(),
                ServoInput.Encoding.CreateBuilder(),
                ServoOutput.Encoding.CreateBuilder(),
                ThrottleConfiguration.Encoding.CreateBuilder(),
                ThrottleSetting.Encoding.CreateBuilder()
            });


        public override EncodingDecorator Encoding
        {
            get
            {
                return encoding;
            }
        }

        public PacketCodec(IEnumerable<PacketEncodingBuilder> PacketEncodingBuilderList)
        {
            _ancestorPacketEncodings = new Dictionary<Type, AncestorPacketEncoding>();
            AncestorPacketEncodings = new ReadOnlyDictionary<Type, AncestorPacketEncoding>(_ancestorPacketEncodings);
            _defaultCommandPacketEncodingBuilders.AddRange(PacketEncodingBuilderList.Where(o => o.Build().GetType().BaseType.GetGenericTypeDefinition() == typeof(FunctionPacketEncoding<>)));
            _defaultDataPacketEncodingBuilders.AddRange(PacketEncodingBuilderList.Where(o => o.Build().GetType().BaseType.GetGenericTypeDefinition() == typeof(AncestorPacketEncoding)));

            EncodingBuilder = Packet.Encoding.CreateBuilder(new[] {
                Data.Encoding.CreateBuilder(_defaultDataPacketEncodingBuilders),
                Command.Encoding.CreateBuilder(_defaultCommandPacketEncodingBuilders)
            });
            encoding = EncodingBuilder.Build();
            var PacketEncoding = ((DescendantPacketEncoding<Packet>)encoding.FindDecoratedEncoding<DescendantPacketEncoding<Packet>>());
            var DataEncoding = ((DescendantPacketEncoding<Data>)PacketEncoding.EncodingDictionary[PacketEncoding.IdDictionary[typeof(Data)]].FindDecoratedEncoding<DescendantPacketEncoding<Data>>());
            foreach (var item in DataEncoding.IdDictionary)
            {
                _ancestorPacketEncodings.Add(item.Key,(AncestorPacketEncoding) DataEncoding.EncodingDictionary[item.Value]);
            }
        }
        public PacketCodec() : this(new List<PacketEncodingBuilder>())
        { }

    }
}
