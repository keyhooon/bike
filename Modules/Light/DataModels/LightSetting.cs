using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Light.DataModels
{
    public class LightSetting :BindableBase
    {
        public LightSetting()
        {
            _lights = new ObservableCollection<LightVolume>(new[] { new LightVolume() , new LightVolume() , new LightVolume() , new LightVolume() });
        }

        private ObservableCollection<LightVolume> _lights;
        public ObservableCollection<LightVolume> Lights
        {
            get { return _lights; }
            set { SetProperty(ref _lights, value); }
        }
    }


    public enum LightVolume : byte
    {
        High = 3,
        Normal = 2,
        Low = 1,
        ExtraLow = 0
    }
}
