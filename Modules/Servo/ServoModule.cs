using Prism.Ioc;
using Prism.Modularity;
using Servo.Views;
using Servo.ViewModels;
using Core;
using Servo.DataTrandferPackets;
using Communication.Codec;
using SharpCommunication.Base.Codec;
using AutoMapper.Configuration;
using Servo.DataModels;

namespace Servo
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
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(FaultEncoding.CreateBuilder())
                .RegisterInstance(ServoInputEncoding.CreateBuilder())
                .RegisterInstance(ServoOutputEncoding.CreateBuilder())
            ;
        }
    }
}
