using Prism.Mvvm;

using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class CoreVersion : BindableBase
    {

        private string _uniqueId;
        [Display(Name = "ID")] 
        [Editable(false)]
        public string UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        private string _firwareVersion;
        [Display(Name = "Firmware Version")]
        [Editable(false)]
        public string FirmwareVersion
        {
            get { return _firwareVersion; }
            set { SetProperty(ref _firwareVersion, value); }
        }
        private string _modelVersion;
        [Display(Name = "Model Number")]
        [Editable(false)]
        public string ModelVersion
        {
            get { return _modelVersion; }
            set { SetProperty(ref _modelVersion, value); }
        }
    }
}
