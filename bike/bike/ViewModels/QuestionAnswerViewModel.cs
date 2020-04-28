using bike.Models;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bike.ViewModels
{
    public class QuestionAnswerViewModel : ViewModel
    {
        public QuestionAnswerViewModel()
        {

        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            AnswerQuestion = parameters.GetValue<AnswerQuestion>("Question");
        }
        private AnswerQuestion _answerQuestion;
        public AnswerQuestion AnswerQuestion
        {
            get { return _answerQuestion; }
            set { SetProperty(ref _answerQuestion, value); }
        }
    }
}
