using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class ThrottleConfiguration:BindableBase
    {
        private static ISettings AppSettings => CrossSettings.Current;

        [Display(Name = "Throttle Low Limit Voltage", Prompt = "Enter Minimum Limit for Throttle Voltage", Description = "Minimum Limit for Throttle Voltage")]
        [Range(0, 5, ErrorMessage = "Low Limit Voltage not in range(0 - 5)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Low Limit Voltage for Throttle cant be empty")]
        public double Min
        {
            get => AppSettings.GetValueOrDefault(nameof(Min), double.Epsilon);
            set
            {
                if (value == Min)
                    return;
                AppSettings.AddOrUpdateValue(nameof(Min), value);
                RaisePropertyChanged();
            }
        }



        [Display(Name = "Throttle High Limit Voltage", Prompt = "Enter Maximum Limit for Throttle Voltage", Description = "Minimum Limit for Throttle Voltage")]
        [Range(0, 5, ErrorMessage = "High Limit Voltage not in range(0 - 5)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "High Limit Voltage for Throttle cant be empty")]
        public double Max
        {
            get => AppSettings.GetValueOrDefault(nameof(Max), 5.0d);
            set
            {
                if (value == Max)
                    return;
                AppSettings.AddOrUpdateValue(nameof(Max), value);
                RaisePropertyChanged();
            }
        }

        [Display(Name = "Throttle Threshold", Prompt = "Enter Threshold Limit for Throttle Voltage", Description = "Threshold Limit for Throttle Voltage")]
        [Range(0, 5, ErrorMessage = "Threshold Limit Voltage not in range(0 - 5)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Threshold Limit Voltage for Throttle cant be empty")]
        public double FaultThreshold
        {
            get => AppSettings.GetValueOrDefault(nameof(FaultThreshold), double.Epsilon);
            set
            {
                if (value == FaultThreshold)
                    return;
                AppSettings.AddOrUpdateValue(nameof(FaultThreshold), value);
                RaisePropertyChanged();
            }
        }

    }
}
