using Prism;
using Prism.Ioc;
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
using bike.ViewModels.Settings;
using bike.ViewModels.ContactUs;

using bike.ViewModels.Main;
using bike.Views.Main;
using Xamarin.Forms;
using Communication;
using Device;
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
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            
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
            containerRegistry.RegisterForNavigation<MainPage>("Main");
            containerRegistry.RegisterForNavigation<WelcomePage>("Welcome");
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutUsSimplePage, AboutUsSimpleViewModel>();
            containerRegistry.RegisterForNavigation<ContactUsPage, ContactUsViewModel>("ContactUs");
            containerRegistry.RegisterForNavigation<HelpPage, HelpViewModel>("Help");
            containerRegistry.RegisterForNavigation<SettingPage, SettingViewModel>("Settings");
            containerRegistry.RegisterForNavigation<ConfigurationPage, ConfigurationViewModel>("Configurations");
            containerRegistry.RegisterForNavigation<DashboardPage, DashboardViewModel>("Dashboard");
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>("Map");
            containerRegistry.RegisterForNavigation<LoggingPage>("Logs");
        }
    }
}
