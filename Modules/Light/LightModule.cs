using Prism.Ioc;
using Prism.Modularity;
using Communication.Codec;
using AutoMapper.Configuration;
using DataModels;
using Services;

namespace Module
{
    public class LightModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<LightSettingPacket, LightSetting>().ReverseMap();
            mapperConfigurationExpression.CreateMap<LightStatePacket, LightState>().ReverseMap();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterInstance(LightSettingPacketEncoding.CreateBuilder())
                .RegisterInstance(LightStatePacetEncoding.CreateBuilder())
                .RegisterInstance(LightCommandEncoding.CreateBuilder())
                                .RegisterSingleton<LightManager>()
                ;
        }
    }
}
