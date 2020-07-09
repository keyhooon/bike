
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Mvvm;
using System.Reactive.Disposables;
using System.Threading;
using System;
using System.Reactive.Linq;

namespace Infrastructure
{
    public abstract class ViewModel : BindableBase
    {
   
        bool _isBusy;
        public bool IsBusy { get => _isBusy; 
            set => SetProperty(ref _isBusy, value); }

        string _title;
        public string Title { get => _title; protected set => SetProperty(ref _title, value); }

    }
}
