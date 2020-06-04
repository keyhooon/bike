using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class ThrottleConfiguration:BindableBase
    {

        private double _min;

        [Display(Name = "Throttle Low Limit Voltage", Prompt = "Enter Minimum Limit for Throttle Voltage", Description = "Minimum Limit for Throttle Voltage")]
        //[Range(0, 5, ErrorMessage = "Low Limit Voltage not in range(0 - 5)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Low Limit Voltage for Throttle cant be empty")]
        [Editable(false)]
        public double Min
        {
            get => _min;
            set => SetProperty(ref _min, value);
        }



        private double _max;

        [Display(Name = "Throttle High Limit Voltage", Prompt = "Enter Maximum Limit for Throttle Voltage", Description = "Minimum Limit for Throttle Voltage")]
        //[Range(0, 5, ErrorMessage = "High Limit Voltage not in range(0 - 5)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "High Limit Voltage for Throttle cant be empty")]
        [Editable(false)]
        public double Max
        {
            get => _max;
            set => SetProperty(ref _max, value);
        }


        private double _faultThreshold;

        [Display(Name = "Throttle Threshold", Prompt = "Enter Threshold Limit for Throttle Voltage", Description = "Threshold Limit for Throttle Voltage")]
        //[Range(0, 5, ErrorMessage = "Threshold Limit Voltage not in range(0 - 5)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Threshold Limit Voltage for Throttle cant be empty")]
        [Editable(false)]
        public double FaultThreshold
        {
            get => _faultThreshold;
            set => SetProperty(ref _faultThreshold, value);
        }

    }
}
