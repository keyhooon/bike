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
using Shiny;
using Shiny.Infrastructure;
using Shiny.Integrations.Sqlite;
using Shiny.Logging;
using Shiny.Models;


namespace bike.ViewModels
{
    public class ErrorLogViewModel : AbstractItemListViewModel<LogStore>
    {
        private readonly ShinySqliteConnection conn;
        private readonly ISerializer serializer;
        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();
        public ErrorLogViewModel(
            IUserDialogs dialogs,
            ISerializer serializer,
            ShinySqliteConnection connection) : base(dialogs)
        {
            this.conn = connection;
            this.serializer = serializer;

            Log
                .WhenExceptionLogged()
                .Select(x => new LogStore
                {
                    Description = x.ToString(),
                    Detail = String.Empty,
                    Parameters = this.serializer.Serialize(x.Parameters),
                    IsError = true,
                    TimestampUtc = DateTime.UtcNow
                })
                .Subscribe(this.Add)
                .DisposeWith(this.DestroyWith);
        }
        public override void Destroy()
        {
            base.Destroy();
            DestroyWith.Dispose();
        }
        protected override async Task ClearItemsAsync(CancellationToken token) => await conn.DeleteAllAsync<LogStore>();


        protected override string DatailText(LogStore item)
        {
            var s = $"{item.TimestampUtc}{Environment.NewLine}{item.Description}{Environment.NewLine}";
            if (!item.Parameters.IsEmpty())
            {
                var parameters = this.serializer.Deserialize<Tuple<string, string>[]>(item.Parameters);
                foreach (var p in parameters)
                    s += $"{Environment.NewLine}{p.Item1}: {p.Item2}";
            }
            return s;
        }

        protected override string DetailHeader(LogStore item)
        {
            return string.Empty;
        }

        protected override async Task<IEnumerable<LogStore>> LoadItemsAsync(INavigationParameters parameters, CancellationToken token)
        {
            return (await conn
                .Logs
                .Where(x => x.IsError)
                .OrderByDescending(x => x.TimestampUtc)
                .ToListAsync());

        }
    }
}
