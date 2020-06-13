using Device.Communication.Transport;
using Prism.Ioc;
using bike.Services;

namespace bike.Services
{
    public static class ContainerExtension
    {
        public static IContainerRegistry UseServoDrive(this IContainerRegistry containerRegistry)
        {
            return containerRegistry
                .RegisterInstance(new BluetoothDataTransportOption("00001101-0000-1000-8000-00805F9B34FB", "HC-05"))
                .RegisterSingleton<ServoDriveService>(); 
        }
    }
}
