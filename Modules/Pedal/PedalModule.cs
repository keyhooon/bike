using Prism.Ioc;
using Prism.Modularity;
using Pedal.Views;
using Pedal.ViewModels;
using Core;
using Pedal.DataTrandferPackets;
using Communication.Codec;
using SharpCommunication.Base.Codec;
using AutoMapper.Configuration;
using Pedal.DataModels;

namespace Pedal
{
    public class PedalModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<PedalConfigurationPacket, PedalConfiguration>().ReverseMap();
            mapperConfigurationExpression.CreateMap<PedalSettingPacket, PedalSetting>().ReverseMap();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(PedalConfigurationEncoding.CreateBuilder())
                .RegisterInstance(PedalSettingEncoding.CreateBuilder())
                ;
        }
    }
}
