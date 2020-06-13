using bike.Models;
using bike.Services;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace bike.ViewModels
{
    public class ReportViewModel : ViewModel
    {
        private readonly TripService tripService;

        public ReportViewModel(TripService tripService)
        {
            this.tripService = tripService;
        }

        private List<Trip> _trips;
        public List<Trip> Trips
        {
            get { return _trips; }
            set { SetProperty(ref _trips, value); }
        }

        private DelegateCommand<Trip> _selectTripCommand;
        public DelegateCommand<Trip> SelectTripCommand =>
            _selectTripCommand ?? (_selectTripCommand = new DelegateCommand<Trip>(ExecuteSelectTripCommand));

        void ExecuteSelectTripCommand(Trip trip)
        {
            tripService.LoadTripAsync(trip);
        }
        protected override async Task InitAsync(INavigationParameters parameters, CancellationToken? cancellation) => Trips = await tripService.GetTripListAsync();
    }
}
