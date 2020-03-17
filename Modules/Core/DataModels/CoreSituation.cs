using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class CoreSituation : BindableBase
    {
        private double _temprature;
        public double Temprature
        {
            get { return _temprature; }
            set { SetProperty(ref _temprature, value); }
        }

        private double _voltage;
        public double Voltage
        {
            get { return _voltage; }
            set { SetProperty(ref _voltage, value); }
        }

    }
}
