using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class BatteryConfiguration : BindableBase
    {

        private double _overCurrent;

        [Display(Name = "Over Current", Prompt = "Enter Over Current", Description = "Max threshold for Current" )]
        //[Range(10, 60, ErrorMessage = "Maximum Threshold for Current not in range(10 - 60)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Over Current should not be empty")]
        [Editable(false)]
        public double OverCurrent
        {
            get => _overCurrent; 
            set => SetProperty(ref _overCurrent, value); 
        }


        private double _overVoltage;

        [Display(Name = "Over Voltage", Prompt = "Enter Over Voltage", Description = "Max threshold for Voltage") ]
        //[Range(40, 80, ErrorMessage = "Maximum Threshold for Voltage not in range(40 - 80)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Over Voltage should not be empty")]
        [Editable(false)]
        public double OverVoltage
        {
                get => _overVoltage; 
                set => SetProperty(ref _overVoltage, value); 
        }


        private double _underVoltage;

        [Display(Name = "Under Voltage", Prompt = "Enter Under Voltage", Description = "Min threshold for Voltage" )]
        //[Range(20, 50, ErrorMessage = "Minimum Threshold for Voltage not in range(20 - 50)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Under Voltage should not be empty")]
        [Editable(false)]
        public double UnderVoltage
        {
            get => _underVoltage;
            set => SetProperty(ref _underVoltage, value);
        }


        private double _nominalVoltage;

        [Display(Name = "Nominal Voltage", Prompt = "Enter Nominal Voltage", Description = "Nominal Voltage Value" )]
        //[Range(30, 70, ErrorMessage = "Nominal Voltage not in range(30 - 70)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Nominal Voltage should not be empty")]
        [Editable(false)]
        public double NominalVoltage
        {
            get => _nominalVoltage;
            set => SetProperty(ref _nominalVoltage, value);
        }


        private double _overTemprature;

        [Display(Name = "Over Temprature", Prompt = "Enter Over Temprature", Description = "Maximum Threshold for Temprature" )]
        //[Range(50, 70, ErrorMessage = "Over Temprature not in range(50 - 70)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Over Temprature should not be empty")]
        [Editable(false)]
        public double OverTemprature
        {
            get => _overTemprature;
            set => SetProperty(ref _overTemprature, value);
        }
    }
}
