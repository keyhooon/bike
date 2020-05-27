using Prism;
using Prism.Ioc;
using Shiny;
using bike.ViewModels;
using bike.Views;
using Xamarin.Forms.Xaml;
using AutoMapper;
using AutoMapper.Configuration;
using Syncfusion.Licensing;
using bike.Views.AboutUs;
using bike.ViewModels.AboutUs;
using bike.Views.ContactUs;
using bike.Views.Settings;
using Xamarin.Forms;
using Communication;
using Device;
using Prism.Mvvm;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace bike
{
    public partial class App 
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App()
        {

        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = viewType.FullName.Replace("Page", "ViewModel");
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
            SyncfusionLicenseProvider.RegisterLicense("NzM3NEAzMTM3MmUzNDJlMzBPRm41TTBEL2hiZ0pjbG93dDZPQ0VocmRCWkJHSXlzWFgrUkxrZVlDaUpzPQ==");
            await NavigationService.NavigateAsync("Main/Nav/Welcome");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry
                .RegisterSingleton<MapperConfigurationExpression>()
                .RegisterSingleton<IConfigurationProvider, MapperConfiguration>()
                .UseServoDrive()
                ;
            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            containerRegistry.RegisterForNavigation<TabbedPage>("TabbedPage");
            containerRegistry.RegisterForNavigation<WelcomePage>("Welcome");
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>("Main");
            containerRegistry.RegisterForNavigation<DashboardPage, DashboardViewModel>("Dashboard");
            containerRegistry.RegisterForNavigation<GaugePage, GaugeViewModel>("Gauge");
            containerRegistry.RegisterForNavigation<MapPage, MapViewModel>("Map");
            containerRegistry.RegisterForNavigation<SettingPage, SettingViewModel>("Settings");
            containerRegistry.RegisterForNavigation<ReportPage, ReportViewModel>("Report");
            containerRegistry.RegisterForNavigation<DiagnosticPage, DiagnosticViewModel>("Diagnostic");
            containerRegistry.RegisterForNavigation<LoggingPage>("Logs");
            containerRegistry.RegisterForNavigation<ErrorLogPage, ErrorLogViewModel>("Errors");
            containerRegistry.RegisterForNavigation<EventsPage, EventsViewModel>("Events");

            containerRegistry.RegisterForNavigation<ConfigurationPage, ConfigurationViewModel>("Configurations");
            containerRegistry.RegisterForNavigation<ContactUsPage, ContactUsViewModel>("ContactUs");
            containerRegistry.RegisterForNavigation<AboutUsSimplePage, AboutUsSimpleViewModel>("AboutUs");
            containerRegistry.RegisterForNavigation<HelpPage, HelpViewModel>("Help");
            containerRegistry.RegisterForNavigation<QuestionAnswerPage, QuestionAnswerViewModel>("Question");
        }
    }
}
