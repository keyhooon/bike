using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Throttle.DataModels
{
    public class ThrottleConfiguration:BindableBase
    {

        private double _min;
        public double Min
        {
            get { return _min; }
            set { SetProperty(ref _min, value); }
        }

        private double _max;
        public double Max
        {
            get { return _max; }
            set { SetProperty(ref _max, value); }
        }

        private double _faultThreshold;
        public double FaultThreshold
        {
            get { return _faultThreshold; }
            set { SetProperty(ref _faultThreshold, value); }
        }

    }
}
