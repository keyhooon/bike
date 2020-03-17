using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataModels
{
    public class LightState :BindableBase
    {
        public LightState()
        {
            _lights = new ObservableCollection<bool>(new[] { false, false, false, false });
        }

        private ObservableCollection<bool> _lights;
        public ObservableCollection<bool> Lights
        {
            get { return _lights; }
            set { SetProperty(ref _lights, value); }
        }
    }
}
