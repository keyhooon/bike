using Acr.UserDialogs.Forms;
using bike.Models;
using bike.Services;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ReactiveUI;
using Shiny.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bike.ViewModels
{
    public class DiagnosticViewModel : AbstractItemListViewModel<Diagnostic>
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
            servoDriveService.FaultChanged += (sender, e) =>
            {
                    LoadCommand.Execute();
            };
        }

        protected async override Task ClearItemsAsync(CancellationToken token)
        {
            await connection.ExecuteAsync("DELETE FROM Diagnostic");
        }

        protected async override Task<IEnumerable<Diagnostic>> LoadItemsAsync(INavigationParameters parameters, CancellationToken token)
        {
            var faultTypes =await connection.FaultType.ToListAsync().ConfigureAwait(false);
            var query = connection
                .Diagnostics
                .OrderByDescending(o => o.StartTime)
                .Take(100);
            if (IsCurrent)
                query = query.Where(o => o.StopTime == null);
            return  (await query.ToListAsync().ConfigureAwait(false)).Join(faultTypes,
                o=>o.FaultTypeId,
                o=>o.Id, 
                (o,o1)=> 
            {
                o.FaultType = o1;
                return o; 
            });
        }

        protected override string DatailText(Diagnostic item)
        {
            if (item.StopTime == null)
            return $"Fault Time:{item.StartTime:MM/dd/yy hh:mm:ss tt zzz}{Environment.NewLine}" +
                $"Not Repaired{Environment.NewLine}" +
                $"{item.FaultType.Description}";
            else return $"Fault Time:{item.StartTime:MM/dd/yy hh:mm:ss tt zzz}{Environment.NewLine}" +
                 $"Repaired Time:{item.StopTime:MM/dd/yy hh:mm:ss tt zzz}{Environment.NewLine}" +
                 $"{item.FaultType.Description}";
        }


        protected override string DetailHeader(Diagnostic item)
        {
            return item.FaultType.Name;
        }

        private bool _isCurrent = false;
        public bool IsCurrent
        {
            get => _isCurrent;
            set => SetProperty(ref _isCurrent, value);
        }

        private DelegateCommand _toggleCurrentCommand;
        public DelegateCommand ToggleCurrentCommand =>
            _toggleCurrentCommand ??= new DelegateCommand(() =>
            {
                IsCurrent = !IsCurrent;
                LoadCommand.Execute();
            });
    }
}
