using Mapsui.UI.Forms;
using SQLite;
using Syncfusion.SfMaps.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.Models.ContactUs
{
    public class Building 
    {
        #region Properties

        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public BuildingType Type { get; set; }
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude{ get; set; }

        /// <summary>
        /// Gets or sets the email id.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        #endregion
    }
    public enum BuildingType
    {
        Stock = 1,
        Office = 2,
        Headquarter = 3
    }
}
