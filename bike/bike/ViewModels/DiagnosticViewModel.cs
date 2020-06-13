using Acr.UserDialogs.Forms;
using bike.Models;
using bike.Services;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using ReactiveUI;
using Shiny.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace bike.ViewModels
{
    public class DiagnosticViewModel : AbstractLogViewModel<CommandItem>
    {
        private readonly ISerializer serializer;
        private readonly SqliteConnection connection;
        private readonly ServoDriveService servoDriveService;

        public DiagnosticViewModel(IUserDialogs dialogs,
            ISerializer serializer,
            SqliteConnection connection, ServoDriveService servoDriveService): base(dialogs)
        {
            this.serializer = serializer;
            this.connection = connection;
            this.servoDriveService = servoDriveService;
            servoDriveService.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(servoDriveService.Fault))
                    LoadCommand.Execute();
            };
        }

        protected async override Task ClearLogs()
        {
            await connection.ExecuteAsync("DELETE FROM Diagnostic");
        }

        protected async override Task<IEnumerable<CommandItem>> LoadLogs()
        {
            var faultTypes =await connection.FaultType.ToListAsync().ConfigureAwait(false);
            var query = connection
                .Diagnostics
                .OrderByDescending(o => o.StartTime)
                .Take(100);
            if (IsCurrent)
                query = query.Where(o => o.StopTime == null);
            var diagnostics = await query.ToListAsync().ConfigureAwait(false);
 
            var ret = diagnostics.Select(o => 
            {
                var fault = faultTypes.First(t => t.Id == o.FaultTypeId);
                return new CommandItem
                {
                    Detail = $"{o.StartTime:MM/dd/yy hh:mm:ss tt zzz}",
                    Text = $"{fault.Name}",
                    Data = o.StopTime,
                    PrimaryCommand = new DelegateCommand(() =>
                    {
                        var s = $"{o.StartTime:MM/dd/yy hh:mm:ss tt zzz}{Environment.NewLine}" +
                        $"{o.StopTime:MM/dd/yy hh:mm:ss tt zzz}{Environment.NewLine}" +
                        $"{fault.Description}";
                        Dialogs.Alert(s, fault.Name);
                    })
                };
            });
            return ret;
        }


        private bool _isCurrent = false;
        public bool IsCurrent
        {
            get => _isCurrent;
            set => SetProperty(ref _isCurrent, value, () =>
            {
                if (value)
                    CurrentCommandText = "All";
                else
                    CurrentCommandText = "Current";
            });
        }


        private string _currentCommandText = "Current";
        public string CurrentCommandText
        {
            get => _currentCommandText;
            set => SetProperty(ref _currentCommandText, value);
        }

        private DelegateCommand _toggleCurrentCommand;
        public DelegateCommand ToggleCurrentCommand =>
            _toggleCurrentCommand ?? (_toggleCurrentCommand = new DelegateCommand(async () =>
            {
                IsCurrent = !IsCurrent;
                var logs = await LoadLogs();
                Logs = new ObservableCollection<CommandItem>(logs);
            }));
    }
}
