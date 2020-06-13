
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
    public abstract class ViewModel : BindableBase,
                                      IAutoInitialize,
                                      IInitialize,
                                      INavigatedAware,
                                      IPageLifecycleAware,
                                      IDestructible,
                                      IConfirmNavigationAsync
    {
        private CompositeDisposable _deactivateWith;
        protected CompositeDisposable DeactivateWith => _deactivateWith ??= new CompositeDisposable();

        protected EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);



        private CancellationTokenSource _cancellationTokenSource;
        protected CancellationTokenSource CancellationTokenSource => _cancellationTokenSource ??= new CancellationTokenSource();

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }
        public virtual void Initialize(INavigationParameters parameters)
        {
            Task.Run(async () => {
                await LoadAsync(parameters).ConfigureAwait(false);
            });

        }
        public virtual void OnNavigatedTo(INavigationParameters parameters) { }
        public async virtual void OnAppearing()
        {
            waitHandle.Reset();
        }
        public virtual void OnDisappearing() { }
        public virtual void Destroy() { 
            _deactivateWith?.Dispose(); 
            _cancellationTokenSource?.Cancel(); 
            _cancellationTokenSource?.Dispose(); 
        }
        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);

        bool _isBusy;
        public bool IsBusy { get => _isBusy; 
            set => SetProperty(ref _isBusy, value); }

        string _title;
        public string Title { get => _title; protected set => SetProperty(ref _title, value); }

        protected virtual Task InitAsync(INavigationParameters parameters, CancellationToken? cancellation = null) => Task.CompletedTask;

        protected async Task LoadAsync(INavigationParameters parameters = null, CancellationToken? cancellation = null)
        {
            IsBusy = true;
            await InitAsync(parameters, CancellationTokenSource.Token).ConfigureAwait(false);
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            waitHandle.Set();
            IsBusy = false;
        }
    }
}
