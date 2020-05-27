using bike.Models;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Processors;

namespace bike.ViewModels
{
    public class QuestionAnswerViewModel : ViewModel
    {
        public QuestionAnswerViewModel()
        {
            ;
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            SelectedAnswerQuestion = parameters.GetValue<AnswerQuestion>("Question");
        }
        private AnswerQuestion _selectedAnswerQuestion;
        public AnswerQuestion SelectedAnswerQuestion
        {
            get { return _selectedAnswerQuestion; }
            set { SetProperty(ref _selectedAnswerQuestion, value); }
        }




    }
}
