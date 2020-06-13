﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;
using Infrastructure;
using Prism.Commands;
using Shiny;
using Shiny.Infrastructure;
using Shiny.Integrations.Sqlite;
using Shiny.Logging;
using Shiny.Models;


namespace bike.ViewModels
{
    public class ErrorLogViewModel : AbstractLogViewModel<CommandItem>
    {
        readonly ShinySqliteConnection conn;
        readonly ISerializer serializer;

        public ErrorLogViewModel(ShinySqliteConnection conn,
                                 ISerializer serializer,
                                 IUserDialogs dialogs) : base(dialogs)
        {
            this.conn = conn;
            this.serializer = serializer;

            Log
                .WhenExceptionLogged()
                .Select(x => new CommandItem
                {
                    Text = $"{DateTime.Now:MM/dd/yy hh:mm:ss tt zzz}",
                    PrimaryCommand = new DelegateCommand(() =>
                    {
                        var s = $"{x.Exception}{Environment.NewLine}";
                        foreach (var p in x.Parameters)
                            s += $"{Environment.NewLine}{p.Key}: {p.Value}";

                        this.Dialogs.Alert(s);
                    })
                })
                .Subscribe(this.InsertItem)
                .DisposeWith(this.DeactivateWith);
        }


        protected override Task ClearLogs() => this.conn.DeleteAllAsync<LogStore>();


        protected override async Task<IEnumerable<CommandItem>> LoadLogs()
        {
            var results = await this.conn
                .Logs
                .Where(x => x.IsError)
                .OrderByDescending(x => x.TimestampUtc)
                .ToListAsync();

            return results.Select(x => new CommandItem
            {

                Text = $"{x.TimestampUtc:MM/dd/yy hh:mm:ss tt zzz}",
                Detail = x.Description,
                PrimaryCommand = new DelegateCommand(() =>
                {
                    var s = $"{x.TimestampUtc}{Environment.NewLine}{x.Description}{Environment.NewLine}";
                    if (!x.Parameters.IsEmpty())
                    {
                        var parameters = this.serializer.Deserialize<Tuple<string, string>[]>(x.Parameters);
                        foreach (var p in parameters)
                            s += $"{Environment.NewLine}{p.Item1}: {p.Item2}";
                    }
                    this.Dialogs.Alert(s);
                })
            }); ;
        }
    }
}
