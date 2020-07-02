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
using bike.Services;
using Prism.Mvvm;
using System;
using System.Reflection;
using System.Globalization;


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
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.").Replace("Page", "ViewModel");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewAssemblyName);
                return Type.GetType(viewModelName);
            });
            SyncfusionLicenseProvider.RegisterLicense("NzM3NEAzMTM3MmUzNDJlMzBPRm41TTBEL2hiZ0pjbG93dDZPQ0VocmRCWkJHSXlzWFgrUkxrZVlDaUpzPQ==");
            await NavigationService.NavigateAsync("Main/Nav/Dashboard?createTab=Gauge");
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
            containerRegistry.RegisterForNavigation<MainPage>("Main");
            containerRegistry.RegisterForNavigation<DashboardPage>("Dashboard");
            containerRegistry.RegisterForNavigation<GaugePage>("Gauge");
            //containerRegistry.RegisterForNavigation<MapPage, MapViewModel>("Map");
            containerRegistry.RegisterForNavigation<SettingPage>("Settings");
            containerRegistry.RegisterForNavigation<ReportPage>("Report");
            containerRegistry.RegisterForNavigation<DiagnosticPage>("Diagnostic");
            containerRegistry.RegisterForNavigation<LoggingPage>("Logs");
            containerRegistry.RegisterForNavigation<ErrorLogPage>("Errors");
            containerRegistry.RegisterForNavigation<EventsPage>("Events");
            containerRegistry.RegisterForNavigation<ServoPage>("Servo");


            containerRegistry.RegisterForNavigation<ConfigurationPage, ConfigurationViewModel>("Configurations");
            containerRegistry.RegisterForNavigation<ContactUsPage>("ContactUs");
            containerRegistry.RegisterForNavigation<AboutUsSimplePage, AboutUsSimpleViewModel>("AboutUs");
            containerRegistry.RegisterForNavigation<HelpPage, HelpViewModel>("Help");
            containerRegistry.RegisterForNavigation<QuestionAnswerPage, QuestionAnswerViewModel>("Question");
        }
    }
}
