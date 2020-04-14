﻿using System.Collections.ObjectModel;
using System.Reflection;
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
    public class AboutUsSimpleViewModel : BindableBase
    {
        #region Fields

        private string productDescription;

        private string productVersion;

        private ImageSource productIcon;

        private ImageSource cardsTopImage;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="T:bike.ViewModels.AboutUs.AboutUsViewModel"/> class.
        /// </summary>
        public AboutUsSimpleViewModel()
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
        public ImageSource ProductIcon
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