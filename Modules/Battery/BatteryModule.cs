using Prism.Ioc;
using Prism.Modularity;

using Communication.Codec;
using AutoMapper.Configuration;
using DataModels;
using Services;

namespace Module
{
    public class BatteryModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<BatteryConfigurationPacket, BatteryConfiguration>().ReverseMap();
            mapperConfigurationExpression.CreateMap<BatteryConfigurationPacket, BatteryConfiguration>().ReverseMap();
            mapperConfigurationExpression.CreateMap<BatteryOutputPacket, BatteryOutput>().ReverseMap();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry
                .RegisterInstance(BatteryConfigurationEncoding.CreateBuilder())
                .RegisterInstance(BatteryOutputEncoding.CreateBuilder())
                .RegisterSingleton<BatteryManager>(); 

        }
        
    }
}
