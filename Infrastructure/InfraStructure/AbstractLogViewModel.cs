using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;


namespace Infrastructure
{
    public abstract class AbstractLogViewModel<TItem> : ViewModel
    {
        readonly object syncLock = new object();


        protected AbstractLogViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            Logs = new ObservableList<TItem>();
        }


        protected IUserDialogs Dialogs { get; }
        public ObservableList<TItem> Logs { get; }
        public bool HasLogs { get; }

        protected async Task LoadAsync(INavigationParameters parameters)
        {
            var logs = await LoadLogs();
            SetLogs(logs);
        }


        private DelegateCommand _doClearCommand;
        public DelegateCommand DoClearCommand =>
            _doClearCommand ?? (_doClearCommand = new DelegateCommand(async () =>
            {
                var confirm = await Dialogs.Confirm("Clear Logs?");
                if (confirm)
                {
                    IsBusy = true;
                    await ClearLogs();
                    var logs = await LoadLogs();
                    SetLogs(logs);
                    IsBusy = false;
                }
            }));
        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(async () =>
            {
                var logs = await LoadLogs();
                SetLogs(logs);
            }));


        protected abstract Task<IEnumerable<TItem>> LoadLogs();

        protected abstract Task ClearLogs();

        protected virtual void InsertItem(TItem item)
        {
            lock (syncLock)
                Logs.Insert(0, item);
        }

        void SetLogs(IEnumerable<TItem> items)
        {
            lock (syncLock)
                Logs.ReplaceAll(items);
        }
    }
}
