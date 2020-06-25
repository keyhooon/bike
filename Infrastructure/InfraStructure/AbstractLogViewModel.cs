using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;
using Prism.Commands;
using Prism.Navigation;

namespace Infrastructure
{
    public abstract class AbstractLogViewModel<TItem> : ViewModel
    {
        readonly object syncLock = new object();


        protected AbstractLogViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
        }
        public override void Destroy()
        {
            base.Destroy();
            lock (syncLock)
                Logs.Clear();
        }

        protected IUserDialogs Dialogs { get; }
        private ObservableCollection<TItem> _logs;
        public ObservableCollection<TItem> Logs {
            get => _logs;
            set => SetProperty(ref _logs, value,()=>RaisePropertyChanged(nameof(HasLogs)));
        }
        public bool HasLogs => Logs?.Any()??false;

        protected override async Task InitAsync(INavigationParameters parameters, CancellationToken? cancellation = null)
        {
            var logs = await LoadLogs();
            if (logs != null)
                Logs = new ObservableCollection<TItem>(logs);

        }


        private DelegateCommand _ClearCommand;
        public DelegateCommand ClearCommand =>
            _ClearCommand ?? (_ClearCommand = new DelegateCommand(async () =>
            {
                var confirm = await Dialogs.Confirm("Clear Logs?");
                if (confirm)
                {
                    IsBusy = true;
                    await ClearLogs();
                    var logs = await LoadLogs();
                    Logs = new ObservableCollection<TItem>(logs);

                    IsBusy = false;
                }
            }));
        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(async () =>
            {
                IsBusy = true;
                var logs = await LoadLogs();
                Logs = new ObservableCollection<TItem>(logs);
               // SetLogs(logs);
                IsBusy = false;
            }));


        protected async virtual Task<IEnumerable<TItem>> LoadLogs() => await Task.FromResult<List<TItem>>(null);

        protected abstract Task ClearLogs();

        protected virtual void InsertItem(TItem item)
        {
            lock (syncLock)
                Logs?.Insert(0, item);
            RaisePropertyChanged(nameof(HasLogs));
        }

        //protected void SetLogs(IEnumerable<TItem> items)
        //{
        //    lock (syncLock)
        //    { 
        //        Logs.Clear();
        //        Logs.AddRange(items);
        //    }
        //}
    }
}
