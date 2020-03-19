using Prism.Ioc;
using Prism.Modularity;

using Communication.Codec;

using AutoMapper.Configuration;
using DataModels;
using Services;

namespace Module
{
    public class ServoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<FaultPacket, Fault>().ReverseMap();
            mapperConfigurationExpression.CreateMap<ServoInputPacket, ServoInput>().ReverseMap();
            mapperConfigurationExpression.CreateMap<ServoOutputPacket, ServoOutput>().ReverseMap();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterInstance(FaultEncoding.CreateBuilder())
                .RegisterInstance(ServoInputEncoding.CreateBuilder())
                .RegisterInstance(ServoOutputEncoding.CreateBuilder())
                                .RegisterSingleton<ServoManager>()
            ;
        }
    }
}
