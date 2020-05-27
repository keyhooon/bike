using System.Collections.ObjectModel;
using System.Reflection;
using bike.Models.AboutUs;
using Infrastructure;
using Prism.Mvvm;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.ViewModels.AboutUs
{
    /// <summary>
    /// ViewModel of AboutUs templates.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AboutUsSimpleViewModel : ViewModel
    {
        private readonly IEnvironment environment;
        #region Fields

        private string productDescription;

        private string productVersion;

        private string productBuild;

        private ImageSource productIcon;

        private ImageSource cardsTopImage;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="T:bike.ViewModels.AboutUs.AboutUsViewModel"/> class.
        /// </summary>
        public AboutUsSimpleViewModel(IEnvironment environment)
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
                    Image = ImageSource.FromResource("bike.Assets.Images.1.jpg", typeof(AboutUsSimpleViewModel).GetTypeInfo().Assembly),
                    Designation = "Project Manager"
                },
                new AboutUsModel
                {
                    EmployeeName = "Jessica Park",
                    Image = ImageSource.FromResource("bike.Assets.Images.2.png", typeof(AboutUsSimpleViewModel).GetTypeInfo().Assembly),
                    Designation = "Senior Manager"
                },
                new AboutUsModel
                {
                    EmployeeName = "Lisa",
                    Image = ImageSource.FromResource("bike.Assets.Images.3.png", typeof(AboutUsSimpleViewModel).GetTypeInfo().Assembly),
                    Designation = "Senior Developer"
                },
                new AboutUsModel
                {
                    EmployeeName = "Rebecca",
                    Image = ImageSource.FromResource("bike.Assets.Images.4.jpg", typeof(AboutUsSimpleViewModel).GetTypeInfo().Assembly),
                    Designation = "Senior Designer"
                },
                new AboutUsModel
                {
                    EmployeeName = "Alexander",
                    Image = ImageSource.FromResource("bike.Assets.Images.5.jpg", typeof(AboutUsSimpleViewModel).GetTypeInfo().Assembly),
                    Designation = "Senior Manager"
                },
                new AboutUsModel
                {
                    EmployeeName = "Anthony",
                    Image = ImageSource.FromResource("bike.Assets.Images.6.png", typeof(AboutUsSimpleViewModel).GetTypeInfo().Assembly),
                    Designation = "Senior Developer"
                }
            };

            this.ItemSelectedCommand = new Command(this.ItemSelected);
            this.environment = environment;
            ProductVersion = environment.AppVersion;
            ProductBuild = environment.AppBuild;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the top image source of the About us with cards view.
        /// </summary>
        /// <value>Image source location.</value>
        public ImageSource CardsTopImage
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

        public string ProductBuild
        {
            get
            {
                return this.productBuild;
            }

            set
            {
                SetProperty(ref productBuild, value);
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