using System;
using System.Collections.ObjectModel;
using System.Globalization;
using bike.Models.ContactUs;
using Prism.Commands;
using Prism.Mvvm;
using Syncfusion.SfMaps.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.ViewModels
{
    /// <summary>
    /// ViewModel for contact us page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ContactUsViewModel : BindableBase
    {
        #region Fields

        private ObservableCollection<MapMarker> customMarkers;

        private Point geoCoordinate;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactUsViewModel" /> class.
        /// </summary>
        public ContactUsViewModel()
        {
            this.CustomMarkers = new ObservableCollection<MapMarker>();
            this.GetPinLocation();
        }

        #endregion   

        #region Commands

        public DelegateCommand _sendCommand;
        /// <summary>
        /// Gets or sets the command that is executed when the Send button is clicked.
        /// </summary>
        public DelegateCommand SendCommand => _sendCommand ?? (_sendCommand = new DelegateCommand(Send));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CustomMarkers collection.
        /// </summary>
        public ObservableCollection<MapMarker> CustomMarkers
        {
            get
            {
                return this.customMarkers;
            }

            set
            {
                SetProperty(ref customMarkers, value);
            }
        }

        /// <summary>
        /// Gets or sets the geo coordinate.
        /// </summary>
        public Point GeoCoordinate
        {
            get
            {
                return this.geoCoordinate;
            }

            set
            {
                SetProperty(ref geoCoordinate, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the send button is clicked.
        /// </summary>
        private void Send()
        {
            // Do something
        }

        /// <summary>
        /// This method is for getting the pin location detail.
        /// </summary>
        private void GetPinLocation()
        {
            this.CustomMarkers.Add(
                new LocationMarker
                {
                    Header = "Sipes Inc",
                    Address = "7654 Cleveland street, Phoenixville, PA 19460",
                    EmailId = "dopuyi@hostguru.info",
                    PhoneNumber = "+1-202-555-0101",
                    Latitude = "40.133808",
                    Longitude = "-75.516279"
                });

            foreach (var marker in this.CustomMarkers)
            {
                this.GeoCoordinate = new Point(Convert.ToDouble(marker.Latitude, CultureInfo.CurrentCulture), Convert.ToDouble(marker.Longitude, CultureInfo.CurrentCulture));
            }
        }

        #endregion
    }
}
