﻿using System;
using Acr.UserDialogs.Forms;
using ReactiveUI;
using Shiny;
using Shiny.Logging;


namespace bike.Infrastructure
{
    public class GlobalExceptionHandler : IObserver<Exception>, IShinyStartupTask
    {
        readonly IUserDialogs dialogs;
        public GlobalExceptionHandler(IUserDialogs dialogs) => this.dialogs = dialogs;


        public void Start() => RxApp.DefaultExceptionHandler = this;
        public void OnCompleted() { }
        public void OnError(Exception error) { }


        public void OnNext(Exception value)
        {
            Log.Write(value);
            dialogs.Alert(value.ToString(), "ERROR");
        }
    }
}