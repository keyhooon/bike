﻿using Xamarin.Forms;

namespace bike.Views
{
    public partial class DiagnosticPage : ContentPage
    {
        public DiagnosticPage()
        {
            InitializeComponent();
            listView.ItemGenerator = new Infrastructure.ItemGeneratorExt(listView);
        }
    }
}
