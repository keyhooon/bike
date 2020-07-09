using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace Infrastructure
{
    public abstract class AbstractItemListViewModel<TItem> : ViewModel, IInitialize, IDestructible
    {
        private readonly object syncLock = new object();
        private readonly IUserDialogs _dialogs;
        private readonly CancellationTokenSource tokenSource;
        private ObservableCollection<TItem> innerItems;


        protected AbstractItemListViewModel(IUserDialogs dialogs)
        {
            _dialogs = dialogs;
            tokenSource = new CancellationTokenSource();
            innerItems = new ObservableCollection<TItem>();
            Items = new ReadOnlyObservableCollection<TItem>(innerItems);
        }
        public virtual void Destroy()
        {
            tokenSource.Cancel();
            Clear();
        }
        protected void Add(TItem item)
        {
            lock (syncLock)
            {
                innerItems.Insert(0, item);
            }
            RaisePropertyChanged(nameof(HasItems));
        }
        protected void AddRange(IEnumerable<TItem> items)
        {
            lock (syncLock)
            {
                items.ForEach((item) => innerItems.Insert(0, item));
            }
            RaisePropertyChanged(nameof(HasItems));
        }
        protected void Clear()
        {
            if (!HasItems)
                return; 
            lock (syncLock)
            {
                innerItems.Clear();
            }
            RaisePropertyChanged(nameof(HasItems));
        }


        public ReadOnlyCollection<TItem> Items {get;}
        public bool HasItems => Items?.Any()??false;

        private DelegateCommand _ClearCommand;
        public DelegateCommand ClearCommand =>
            _ClearCommand ??= new DelegateCommand(async () =>
            {
                var confirm = await _dialogs.Confirm("Clear Logs?");
                if (confirm)
                {
                    IsBusy = true;
                    await ClearItemsAsync(tokenSource.Token);
                    Clear();
                    IsBusy = false;
                }
            });
        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand =>
            _loadCommand ??= new DelegateCommand(async () =>
            {
                IsBusy = true;
                Clear();
                AddRange(await LoadItemsAsync(null, tokenSource.Token));
                IsBusy = false;
            });

        private DelegateCommand<TItem> _showDetailCommand;
        public DelegateCommand<TItem> ShowDetailCommand =>
            _showDetailCommand ??= new DelegateCommand<TItem>(async (item) => await _dialogs.Alert(DatailText(item), DetailHeader(item)));


        protected abstract Task<IEnumerable<TItem>> LoadItemsAsync(INavigationParameters parameters, CancellationToken token);
        protected abstract Task ClearItemsAsync(CancellationToken token);
        protected abstract string DatailText(TItem item);
        protected abstract string DetailHeader(TItem item);




        public void Initialize(INavigationParameters parameters)
        {
            Task.Run(async () => {
                IsBusy = true;
                AddRange(await LoadItemsAsync(parameters, tokenSource.Token));
                IsBusy = false;
            });
        }

    }
}
