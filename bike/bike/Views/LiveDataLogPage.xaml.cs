using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
namespace bike.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveDataLogPage : ContentPage
    {
        public LiveDataLogPage()
        {
            InitializeComponent();
        }
    }
}
