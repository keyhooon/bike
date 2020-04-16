﻿
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Mvvm;
using System.Reactive.Disposables;
using System.Threading;

namespace Infrastructure
{
    public abstract class ViewModel : BindableBase,
                                      IAutoInitialize,
                                      IInitialize,
                                      IInitializeAsync,
                                      INavigatedAware,
                                      IPageLifecycleAware,
                                      IDestructible,
                                      IConfirmNavigationAsync
    {
        private CompositeDisposable _deactivateWith;
        protected CompositeDisposable DeactivateWith => _deactivateWith ??= new CompositeDisposable();




        private CancellationTokenSource _cancellationTokenSource;
        protected CancellationTokenSource CancellationTokenSource => _cancellationTokenSource ??= new CancellationTokenSource();

        protected virtual void Deactivate() {
            DeactivateWith?.Dispose();
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) => Deactivate();
        public virtual void Initialize(INavigationParameters parameters) { }
        public virtual async Task InitializeAsync(INavigationParameters parameters)
        {
            IsBusy = true;
            await LoadAsync(parameters, CancellationTokenSource?.Token);
            IsBusy = false;
        }
        public virtual void OnNavigatedTo(INavigationParameters parameters) { }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }
        public virtual void Destroy() { DeactivateWith.Dispose(); }
        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);

        bool _isBusy;
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

        string _title;
        public string Title { get => _title; protected set => SetProperty(ref _title, value); }

        protected virtual Task LoadAsync(INavigationParameters parameters, CancellationToken? cancellation) => Task.CompletedTask;
    }
}
