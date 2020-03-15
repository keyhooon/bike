using Prism.Ioc;
using Prism.Modularity;
using Battery.Views;
using Battery.ViewModels;
using Core;
using Battery.DataTransfterPacket;
using Communication.Codec;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using AutoMapper;
using AutoMapper.Configuration;
using Battery.DataModel;
using Battery.DataSettings;
using Battery.Services;

namespace Battery
{
    public class BatteryModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<BatteryConfigurationPacket, BatteryConfiguration>().ReverseMap();
            mapperConfigurationExpression.CreateMap<BatteryConfiguration, BatteryConfiguration>().ReverseMap();
            mapperConfigurationExpression.CreateMap<BatteryOutputPacket, BatteryOutput>().ReverseMap();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, BatteryOutputViewModel>();
            containerRegistry
                .RegisterInstance(BatteryConfigurationEncoding.CreateBuilder())
                .RegisterInstance(BatteryOutputEncoding.CreateBuilder());
            containerRegistry.RegisterSingleton<BatteryManager>();
        }
        
    }
}
