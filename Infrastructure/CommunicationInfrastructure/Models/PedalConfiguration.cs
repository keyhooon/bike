using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class PedalConfiguration : BindableBase
    {

        private int _magnetCount;

        [Display(Name = "Number of Magnet", Prompt = "Enter Number of Magnet", Description = "Magnet Count that Used in Motor")]
        //[Range(6, 60, ErrorMessage = "Magnet count not in range(6 - 60)")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Number of Magnet should be specified ")]
        [Editable(false)]
        public int MagnetCount
        {
            get => _magnetCount;
            set => SetProperty(ref _magnetCount, value);
        }



    }
}
