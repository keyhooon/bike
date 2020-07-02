using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using bike.Models.ContactUs;
using Infrastructure;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace bike.ViewModels
{
    /// <summary>
    /// ViewModel for contact us page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ContactUsViewModel : ViewModel
    {
        #region Fields
        private readonly SqliteConnection sqliteConnection;
        private List<Building> _buildings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactUsViewModel" /> class.
        /// </summary>
        public ContactUsViewModel(SqliteConnection sqliteConnection)
        {
            _buildings = new List<Building>();
            this.sqliteConnection = sqliteConnection;
        }

        #endregion   

        #region Properties

        /// <summary>
        /// Gets or sets the CustomMarkers collection.
        /// </summary>
        public List<Building> Buildings { get { return _buildings; } set { SetProperty(ref _buildings, value); } }

        private Building _selectedBuilding;
        public Building SelectedBuilding
        {
            get { return _selectedBuilding; }
            set { SetProperty(ref _selectedBuilding, value); }
        }

        #endregion

        #region Methods

        protected override async Task LoadDataAsync(INavigationParameters parameters, CancellationToken? cancellation) => Buildings = await sqliteConnection.Buildings.ToListAsync() ;

        #endregion
    }
}
