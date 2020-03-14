using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataModels
{
    public class CoreVersion : BindableBase
    {

        private string _uniqueId;
        public string UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        private string _firwareVersion;
        public string PropertyName
        {
            get { return _firwareVersion; }
            set { SetProperty(ref _firwareVersion, value); }
        }
        private string _modelVersion;
        public string ModelVersion
        {
            get { return _modelVersion; }
            set { SetProperty(ref _modelVersion, value); }
        }
    }
}
