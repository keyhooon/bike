using Xamarin.Forms;

namespace bike.Views
{
    public partial class BluetoothPage : ContentPage
    {
        public BluetoothPage()
        {
            InitializeComponent();
            listView.ItemGenerator = new Infrastructure.ItemGeneratorExt(listView);
        }
    }
}
