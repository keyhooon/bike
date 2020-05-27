using Acr.UserDialogs.Forms;
using AiForms.Renderers;
using bike.Models;
using DynamicData;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms.Internals;

namespace bike.ViewModels
{
    /// <summary>
    /// ViewModel for Help page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class HelpViewModel : AbstractLogViewModel<CommandItem>
    {
        private readonly SqliteConnection conn;


        /// <summary>
        /// Initializes a new instance of the <see cref="HelpViewModel" /> class
        /// </summary>
        public HelpViewModel(SqliteConnection conn, IUserDialogs dialogs) : base(dialogs)
        {
            this.conn = conn;
            var results = this.conn
                .AnswerQuestions
                .ToListAsync().Result;

            Logs = new ObservableCollection<CommandItem>(results.Select(x => new CommandItem
            {
                Text = x.Question,
                Detail = x.Answer,
                PrimaryCommand = new DelegateCommand(() =>
                {
                    var s = $"{x.Answer}{Environment.NewLine}{Environment.NewLine}{x.Detail}";
                    Dialogs.Alert(s, x.Question);
                })
            }));
        }

        protected override Task ClearLogs() => throw new NotSupportedException();

    }
}
