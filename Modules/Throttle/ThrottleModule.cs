using Prism.Ioc;
using Prism.Modularity;
using Throttle.Views;
using Throttle.ViewModels;
using Core;
using Communication.Codec;
using SharpCommunication.Base.Codec;
using AutoMapper.Configuration;
using Throttle.DataModels;

namespace Throttle
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
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(ThrottleConfigurationEncoding.CreateBuilder())
            ;
        }
    }
}
