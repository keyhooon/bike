using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;
using Infrastructure;
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI;
using Shiny;
using Shiny.Infrastructure;
using Shiny.Integrations.Sqlite;
using Shiny.Logging;
using Shiny.Models;

namespace bike.ViewModels
{
    public class EventsViewModel : AbstractItemListViewModel<LogStore>
    {
        private readonly ShinySqliteConnection connection;
        private readonly ISerializer serializer;
        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();

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
                .Select(x => new LogStore
                {
                    Description = x.EventName,
                    Detail = x.Description,
                    Parameters = this.serializer.Serialize(x.Parameters),
                    TimestampUtc = DateTime.UtcNow
                })
                .Subscribe(Add)
                .DisposeWith(DestroyWith);

        }

        public override void Destroy()
        {
            base.Destroy();
            DestroyWith.Dispose();
        }

        protected override async Task ClearItemsAsync(CancellationToken token) => await connection.Logs.DeleteAsync(o => o.IsError == false);


        protected override string DatailText(LogStore o)
        {
            var s = $"{o.Description} ({o.TimestampUtc:hh:mm:ss tt}){Environment.NewLine}{o.Detail}{Environment.NewLine}";
            if (!o.Parameters.IsEmpty())
            {
                var parameters = this.serializer.Deserialize<Tuple<string, string>[]>(o.Parameters);
                foreach (var p in parameters)
                    s += $"{Environment.NewLine}{p.Item1}: {p.Item2}";
            }
            return s;
        }

        protected override string DetailHeader(LogStore item)
        {
            return  string.Empty;
        }

        protected override async Task<IEnumerable<LogStore>> LoadItemsAsync(INavigationParameters parameters, CancellationToken token)
        {
            return (await connection
                .Logs
                .OrderByDescending(o => o.TimestampUtc)
                .Where(o => o.IsError == false)
                .Take(100)
                .ToListAsync());
        }
    }
}
