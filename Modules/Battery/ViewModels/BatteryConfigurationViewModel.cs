using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Battery.Services;
using Battery.DataSettings;

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
