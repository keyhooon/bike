using bike.Models;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
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
    public class HelpViewModel : ViewModel
    {
        private readonly SqliteConnection connection;
        private readonly IContainerExtension _container;


        /// <summary>
        /// Initializes a new instance of the <see cref="HelpViewModel" /> class
        /// </summary>
        public HelpViewModel(IContainerExtension container,SqliteConnection connection)
        {
            _container = container;
            this.connection = connection;
        }
        protected async override Task LoadAsync(INavigationParameters parameters, CancellationToken? cancellation)
        {
            var answerQuestionList = await connection.AnswerQuestions.ToListAsync();
            CategorizedAnswerQuestions = answerQuestionList.GroupBy(
                aq => aq.Category,
                (aqCategory, aq) => ( CategoryName : aqCategory, AnswerQuestions : aq.ToList() ));
        }

        private IEnumerable<(string CategoryName, List<AnswerQuestion> AnswerQuestions)> _categorizedAnswerQuestions;
        public IEnumerable<(string CategoryName, List<AnswerQuestion> AnswerQuestions)> CategorizedAnswerQuestions
        {
            get { return _categorizedAnswerQuestions; }
            set { SetProperty(ref _categorizedAnswerQuestions, value); }
        }

    }


}
