﻿using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Acr.UserDialogs.Forms;
using Shiny;
using Xamarin.Forms;

namespace Infrastructure
{
    public static class Extensions
    {
        public static async Task<bool> RequestAccess(this IUserDialogs dialogs, Func<Task<AccessState>> request)
        {
            var access = await request();
            return await dialogs.AlertAccess(access);
        }


        public static async Task<bool> AlertAccess(this IUserDialogs dialogs, AccessState access)
        {
            switch (access)
            {
                case AccessState.Available:
                    return true;

                case AccessState.Restricted:
                    await dialogs.Alert("WARNING: Access is restricted");
                    return true;

                default:
                    await dialogs.Alert("Invalid Access State: " + access);
                    return false;
            }
        }


        public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext)
            => obs
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(onNext);


        public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext, Action<Exception> onError)
            => obs
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(onNext, onError);


        public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext, Action<Exception> onError, Action onComplete)
            => obs
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(onNext, onError, onComplete);
    }
}
