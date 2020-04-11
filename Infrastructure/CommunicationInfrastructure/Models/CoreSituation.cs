using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class CoreSituation : BindableBase
    {
        private double _temprature;
        [Display(Name = "Core Temprature")]
        [Editable(false)]
        public double Temprature
        {
            get { return _temprature; }
            set { SetProperty(ref _temprature, value); }
        }

        private double _voltage;
        [Display(Name = "Core Voltage")]
        [Editable(false)]
        public double Voltage
        {
            get { return _voltage; }
            set { SetProperty(ref _voltage, value); }
        }

    }
}
