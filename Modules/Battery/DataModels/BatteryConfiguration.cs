using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class BatteryConfiguration : BindableBase
    {

        private static ISettings AppSettings => CrossSettings.Current;

        [Display(Name = "Over Current", Prompt = "Enter Over Current", Description = "Max threshold for Current" )]
        [Range(10, 60, ErrorMessage = "Maximum Threshold for Current not in range(10 - 60)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Over Current should not be empty")]
        public double OverCurrent
        {
            get => AppSettings.GetValueOrDefault(nameof(OverCurrent), 40.0d);
            set
            {
                if (value == OverCurrent)
                    return;
                AppSettings.AddOrUpdateValue(nameof(OverCurrent), value);
                RaisePropertyChanged();
            }
        }

        [Display(Name = "Over Voltage", Prompt = "Enter Over Voltage", Description = "Max threshold for Voltage") ]
        [Range(40, 80, ErrorMessage = "Maximum Threshold for Voltage not in range(40 - 80)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Over Voltage should not be empty")]
        public double OverVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(OverVoltage), 60.0d);
            set
            {
                if (value == OverVoltage)
                    return;
                AppSettings.AddOrUpdateValue(nameof(OverVoltage), value);
                RaisePropertyChanged();
            }
        }

        [Display(Name = "Under Voltage", Prompt = "Enter Under Voltage", Description = "Min threshold for Voltage" )]
        [Range(20, 50, ErrorMessage = "Minimum Threshold for Voltage not in range(20 - 50)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Under Voltage should not be empty")]
        public double UnderVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(UnderVoltage), 30.0d);
            set
            {
                if (value == UnderVoltage)
                    return;
                AppSettings.AddOrUpdateValue(nameof(UnderVoltage), value);
                RaisePropertyChanged();
            }
        }


        [Display(Name = "Nominal Voltage", Prompt = "Enter Nominal Voltage", Description = "Nominal Voltage Value" )]
        [Range(30, 70, ErrorMessage = "Nominal Voltage not in range(30 - 70)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nominal Voltage should not be empty")]
        public double NominalVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(NominalVoltage), 60.0d);
            set
            {
                if (value == NominalVoltage)
                    return;
                AppSettings.AddOrUpdateValue(nameof(NominalVoltage), value);
                RaisePropertyChanged();
            }
        }

        [Display(Name = "Over Temprature", Prompt = "Enter Over Temprature", Description = "Maximum Threshold for Temprature" )]
        [Range(50, 70, ErrorMessage = "Over Temprature not in range(50 - 70)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Over Temprature should not be empty")]
        public double OverTemprature
        {
            get => AppSettings.GetValueOrDefault(nameof(OverTemprature), 60.0d);
            set
            {
                if (value == OverTemprature)
                    return;
                AppSettings.AddOrUpdateValue(nameof(OverTemprature), value);
                RaisePropertyChanged();
            }
        }
    }
}
