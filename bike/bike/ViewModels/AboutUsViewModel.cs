using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using bike.Models.AboutUs;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.ViewModels.AboutUs
{
    /// <summary>
    /// ViewModel of AboutUs templates.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AboutUsViewModel : BindableBase
    {
        #region Fields

        private string productDescription;

        private string productVersion;

        private string productIcon;

        private string cardsTopImage;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="T:bike.ViewModels.AboutUs.AboutUsViewModel"/> class.
        /// </summary>
        public AboutUsViewModel()
        {
            this.productDescription =
                "Situated in the heart of Smith-town, Acme Products, Inc., has a long-standing tradition of selling the best products while providing the fastest service on the market. Since 1952, we’ve helped our customers identify their needs, understand their wants, and capture their dreams.";
            this.productIcon = "bike.jpg";
            this.productVersion = "1.0";
            this.cardsTopImage = "bike2.jpg";

            this.EmployeeDetails = new ObservableCollection<AboutUsModel>
            {
                new AboutUsModel
                {
                    EmployeeName = "Alice",
                    Image = "1.jpg",
                    Designation = "Project Manager"
                },
                new AboutUsModel
                {
                    EmployeeName = "Jessica Park",
                    Image = "2.png",
                    Designation = "Senior Manager"
                },
                new AboutUsModel
                {
                    EmployeeName = "Lisa",
                    Image = "3.png",
                    Designation = "Senior Developer"
                },
                new AboutUsModel
                {
                    EmployeeName = "Rebecca",
                    Image = "4.jpg",
                    Designation = "Senior Designer"
                },
                new AboutUsModel
                {
                    EmployeeName = "Alexander",
                    Image = "5.jpg",
                    Designation = "Senior Manager"
                },
                new AboutUsModel
                {
                    EmployeeName = "Anthony",
                    Image = "6.png",
                    Designation = "Senior Developer"
                }
            };

            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the top image source of the About us with cards view.
        /// </summary>
        /// <value>Image source location.</value>
        public string CardsTopImage
        {
            get
            {
                return this.cardsTopImage;
            }

            set
            {
                SetProperty(ref cardsTopImage, value);


            }
        }

        /// <summary>
        /// Gets or sets the description of a product or a company.
        /// </summary>
        /// <value>The product description.</value>
        public string ProductDescription
        {
            get
            {
                return this.productDescription;
            }

            set
            {
                SetProperty(ref productDescription, value);

            }
        }

        /// <summary>
        /// Gets or sets the product icon.
        /// </summary>
        /// <value>The product icon.</value>
        public string ProductIcon
        {
            get
            {
                return this.productIcon;
            }

            set
            {
                SetProperty(ref productIcon, value);
            }
        }

        /// <summary>
        /// Gets or sets the product version.
        /// </summary>
        /// <value>The product version.</value>
        public string ProductVersion
        {
            get
            {
                return this.productVersion;
            }

            set
            {
                SetProperty(ref productVersion, value);
            }
        }

        /// <summary>
        /// Gets or sets the employee details.
        /// </summary>
        /// <value>The employee details.</value>
        public ObservableCollection<AboutUsModel> EmployeeDetails { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            // Do something
        }

        #endregion
    }
}