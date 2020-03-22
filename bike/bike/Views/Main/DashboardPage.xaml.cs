using Syncfusion.SfPicker.XForms;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace bike.Views.Main
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }
        
        private void SfButton_Clicked(object sender, System.EventArgs e)
        {
            if (((SfButton)sender) == FindByName("PedalAssistLevelButton"))
                PedalAssistLevelPicker.IsOpen = true;
            else if (((SfButton)sender) == FindByName("PedalActivationTimeButton"))
                PedalActivationTimePicker.IsOpen = true;
            else if (((SfButton)sender) == FindByName("ThrottleActivityButton"))
                ThrottleActivityPicker.IsOpen = true;
        }

        private void Picker_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (((SfPicker)sender) == FindByName("PedalAssistLevelPicker"))
                PedalAssistLevelPicker.IsOpen = false;
            else if (((SfPicker)sender) == FindByName("PedalActivationTimePicker"))
                PedalActivationTimePicker.IsOpen = false;
            else if (((SfPicker)sender) == FindByName("ThrottleActivityPicker"))
                ThrottleActivityPicker.IsOpen = false;

        }
    }
}
