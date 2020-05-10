using Mapsui.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace bike.Views.ContactUs
{
    /// <summary>
    /// Page to contact with user name, email and message
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUsPage
    {
        private double frameWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactUsPage" /> class.
        /// </summary>
        public ContactUsPage()
        {
            InitializeComponent();
 //           Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        }

        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
                DefaultStyle(height);
        }

        /// <summary>
        /// This default style method is called when the device is portrait mode.
        /// This method is also called when the android and ios devices are landscape mode
        /// </summary>
        /// <param name="height">The height</param>
        private void DefaultStyle(double height)
        {
                MainStack.Orientation = StackOrientation.Vertical;
                MainFrame.VerticalOptions = LayoutOptions.End;
                MainFrame.Margin = new Thickness(15, -50, 15, 15);
                MainFrameStack.VerticalOptions = LayoutOptions.EndAndExpand;
                MainFrame.CornerRadius = 5;
                MainFrame.HasShadow = true;
                MainFrameStack.Margin = new Thickness(0);
        }
    }
}