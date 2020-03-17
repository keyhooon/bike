using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class BatteryOutput:BindableBase
    {

        private double _current;
        public double Current
        {
            get { return _current; }
            set { SetProperty(ref _current, value); }
        }

        private double _voltage;
        public double Voltage
        {
            get { return _voltage; }
            set { SetProperty(ref _voltage, value); }
        }

        private double _temprature;
        public double Temprature
        {
            get { return _temprature; }
            set { SetProperty(ref _temprature, value); }
        }

    }
}
