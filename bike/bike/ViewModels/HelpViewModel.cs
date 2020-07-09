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
    public class HelpViewModel : AbstractItemListViewModel<AnswerQuestion>
    {
        private readonly SqliteConnection conn;


        /// <summary>
        /// Initializes a new instance of the <see cref="HelpViewModel" /> class
        /// </summary>
        public HelpViewModel(SqliteConnection conn, IUserDialogs dialogs) : base(dialogs)
        {
            this.conn = conn;
        }

        protected override Task ClearItemsAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override string DatailText(AnswerQuestion item)
        {
            return $"{item.Answer}{Environment.NewLine}{Environment.NewLine}{item.Detail}";
        }

        protected override string DetailHeader(AnswerQuestion item)
        {
            return item.Question.ToString();
        }

        protected override async Task<IEnumerable<AnswerQuestion>> LoadItemsAsync(INavigationParameters parameters, CancellationToken token)
        {
            return await conn
                .AnswerQuestions
                .ToListAsync();
        }
    }
}
