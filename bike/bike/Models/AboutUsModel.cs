using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.Models.AboutUs
{
    /// <summary>
    /// Model for About us templates.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AboutUsModel : BindableBase
    {
        #region Fields

        private string employeeName;

        private string designation;

        private ImageSource image;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of an employee.
        /// </summary>
        /// <value>The name.</value>
        public string EmployeeName
        {
            get
            {
                return this.employeeName;
            }

            set
            {
                SetProperty(ref employeeName, value);
            }
        }

        /// <summary>
        /// Gets or sets the designation of an employee.
        /// </summary>
        /// <value>The designation.</value>
        public string Designation
        {
            get
            {
                return this.designation;
            }

            set
            {
                SetProperty(ref designation, value);
            }
        }

        /// <summary>
        /// Gets or sets the image of an employee.
        /// </summary>
        /// <value>The image.</value>
        public ImageSource Image
        {
            get
            {
                return this.image;
            }

            set
            {
                SetProperty(ref image, value);
            }
        }

        #endregion
    }
}
