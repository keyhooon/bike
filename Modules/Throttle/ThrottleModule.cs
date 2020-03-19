using Prism.Ioc;
using Prism.Modularity;
using Communication.Codec;
using AutoMapper.Configuration;
using DataModels;
using Services;

namespace Module
{
    public class ThrottleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<ThrottleConfigurationPacket, ThrottleConfiguration>().ReverseMap();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterInstance(ThrottleConfigurationEncoding.CreateBuilder())
                .RegisterSingleton<ThrottleManager>()
            ;
        }
    }
}
