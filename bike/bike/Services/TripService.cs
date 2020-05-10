using bike.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bike.Services
{
    public class TripService
    {
        private readonly SqliteConnection connection;
        private CancellationTokenSource cancellationTokenSource;

        public event EventHandler SelectedTripChanged;

        public event EventHandler SelectedTripDetailChanged;

        public event EventHandler IsNewTripChanged;

        public TripService(SqliteConnection connection)
        {
            this.connection = connection;
            cancellationTokenSource = new CancellationTokenSource();
        }
        public async Task<List<Trip>> GetTripListAsync(Expression<Func<Trip, bool>> predExpr)
        {
            return await connection.Trips.Where(predExpr).ToListAsync();
        }

        public async Task<List<Trip>> GetTripListAsync()
        {
            return await connection.Trips.ToListAsync();
        }

        public async Task<Trip> LoadTripAsync(Trip trip)
        {
            trip.TripDetails = await connection.TripDetails.Where(o => o.TripId == trip.Id).ToListAsync();
            return trip;
        }
        public async Task<Trip> LoadTripAsync(int tripId)
        {
            var trip = await connection.Trips.FirstAsync(o=> o.Id == tripId);
            trip.TripDetails = await connection.TripDetails.Where(o => o.TripId == trip.Id).ToListAsync();
            return trip;
        }

        public async Task StartTrip()
        {
            var trip = new Trip() { StartTime = DateTime.Now };
            trip.Id = await connection.InsertAsync(trip);
            SelectedTrip = trip;
            IsNewTrip = true;
            cancellationTokenSource = new CancellationTokenSource();
            await Task.Run(async() =>
            {
                while (true)
                {
                    var tripDetail = new TripDetail() { TripId = SelectedTrip.Id, Time = DateTime.Now };
                    tripDetail.Id = await connection.InsertAsync(tripDetail);
                    SelectedTripDetail = tripDetail;
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    if (cancellationTokenSource.IsCancellationRequested)
                        break;
                }
            }).ConfigureAwait(false);
            IsNewTrip = false;
        }

        public void StopTrip()
        {
            cancellationTokenSource.Cancel();
        }

        private Trip _selectedTrip;
        public Trip SelectedTrip
        {
            get => _selectedTrip;
            set
            {
                if (_selectedTrip == value)
                    return;
                _selectedTrip = value;
                OnSelectedtripChanged();
            }
        }

        private TripDetail _selectedTripDetail;
        public TripDetail SelectedTripDetail
        {
            get => _selectedTripDetail;
            set
            {
                if (_selectedTripDetail == value)
                    return;
                _selectedTripDetail = value;
                OnSelectedtripDetailChanged();
            }
        }

        private bool _isNewTrip;
        public bool IsNewTrip
        {
            get => _isNewTrip;
            private set
            {
                if (_isNewTrip == value)
                    return;
                _isNewTrip = value;
                OnIsNewtripChanged();
            }
        }


        private void OnSelectedtripDetailChanged()
        {
            SelectedTripDetailChanged?.Invoke(this, null);
        }

        private void OnSelectedtripChanged()
        {
            SelectedTripChanged?.Invoke(this, null);
        }
        private void OnIsNewtripChanged()
        {
            IsNewTripChanged?.Invoke(this, null);
        }
    }
}
