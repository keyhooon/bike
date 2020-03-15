using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace bike.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void hamburgerButton_Clicked(object sender, EventArgs e)
        {
           navigationDrawer.ToggleDrawer();
        }
    }
}