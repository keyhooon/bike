using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;
using Infrastructure;
using Prism.Commands;
using ReactiveUI;
using Shiny;
using Shiny.Infrastructure;
using Shiny.Integrations.Sqlite;
using Shiny.Logging;


namespace bike.ViewModels
{
    public class EventsViewModel : AbstractLogViewModel<CommandItem>
    {
        private readonly ShinySqliteConnection connection;
        private readonly ISerializer serializer;

        public EventsViewModel(
            IUserDialogs dialogs,
            ISerializer serializer,
            ShinySqliteConnection connection) : base(dialogs)
        {
            this.connection = connection;
            this.serializer = serializer;

            Log
                .WhenEventLogged()
 //               .ObserveOn(RxApp.MainThreadScheduler)
                .Select(x => new CommandItem
                {
                    Text = $"{x.EventName} ({DateTime.Now:hh:mm:ss tt})",
                    Detail = x.Description,
                    PrimaryCommand = new DelegateCommand(() =>
                    {
                        var s = $"{x.EventName} ({DateTime.Now:hh:mm:ss tt}){Environment.NewLine}{x.Description}";
                        foreach (var p in x.Parameters)
                            s += $"{Environment.NewLine}{p.Key}: {p.Value}";
                        Dialogs.Alert(s);
                    })
                })
                .Subscribe(this.InsertItem)
                .DisposeWith(this.DeactivateWith);

        }


        protected async override Task ClearLogs()
        {
            await connection.Logs.DeleteAsync(o => o.IsError == false);
        }
        protected async override Task<IEnumerable<CommandItem>> LoadLogs()
        {
            var result = await connection
                .Logs
                .OrderByDescending(o => o.TimestampUtc)
                .Where(o => o.IsError == false)
                .Take(100)
                .ToListAsync();
            return result.Select(o=> new CommandItem 
            {
                Text = $"{o.Description} ({o.TimestampUtc:hh:mm:ss tt})",
                Detail = o.Detail,
                PrimaryCommand = new DelegateCommand(() =>
                {
                    var s = $"{o.Description} ({o.TimestampUtc:hh:mm:ss tt}){Environment.NewLine}{o.Detail}{Environment.NewLine}";
                    if (!o.Parameters.IsEmpty())
                    {
                        var parameters = this.serializer.Deserialize<Tuple<string, string>[]>(o.Parameters);
                        foreach (var p in parameters)
                            s += $"{Environment.NewLine}{p.Item1}: {p.Item2}";
                    }
                    Dialogs.Alert(s);
                })
            });
        }
    }
}
