using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battery.DataSettings
{
    public class BatteryConfiguration : BindableBase
    {

        private static ISettings AppSettings => CrossSettings.Current;

        [Display(Name = "Over Current", Prompt = "Enter Over Current", Description = "Max threshold for Current")]
        [Range(10, 60, ErrorMessage = "Maximum Threshold for Current not in range(10 - 60)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Over Current should not be empty")]
        public double OverCurrent
        {
            get => AppSettings.GetValueOrDefault(nameof(OverCurrent), double.PositiveInfinity);
            set 
            { 
                AppSettings.AddOrUpdateValue(nameof(OverCurrent), value);
                RaisePropertyChanged();
            }
        }

        [Display(Name = "Over Voltage", Prompt = "Enter Over Voltage", Description = "Max threshold for Voltage")]
        [Range(40, 80, ErrorMessage = "Maximum Threshold for Voltage not in range(40 - 80)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Over Voltage should not be empty")]
        public double OverVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(OverVoltage), double.PositiveInfinity);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(OverVoltage), value);
                RaisePropertyChanged();
            }
        }

        public double UnderVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(UnderVoltage), double.NegativeInfinity);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(UnderVoltage), value);
                RaisePropertyChanged();
            }
        }

        public double NominalVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(NominalVoltage), double.Epsilon);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(NominalVoltage), value);
                RaisePropertyChanged();
            }
        }

        public double OverTemprature
        {
            get => AppSettings.GetValueOrDefault(nameof(OverTemprature), double.PositiveInfinity);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(OverTemprature), value);
                RaisePropertyChanged();
            }
        }
    }
}
