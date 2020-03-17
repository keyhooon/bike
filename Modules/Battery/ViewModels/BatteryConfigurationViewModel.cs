using Prism.Mvvm;
using Services;

namespace Battery.ViewModels
{
    public class BatteryConfigurationViewModel : BindableBase
    {
        public BatteryManager BatteryManager { get; }

        public BatteryConfigurationViewModel(BatteryManager batteryManager)
        {
            BatteryManager = batteryManager;            
        }
    }
}
