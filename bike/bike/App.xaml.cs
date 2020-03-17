using Prism;
using Prism.Ioc;
using bike.ViewModels;
using bike.Views;
using Xamarin.Forms.Xaml;
using AutoMapper;
using AutoMapper.Configuration;
using Prism.Modularity;
using Syncfusion.Licensing;
using bike.Views.AboutUs;
using bike.ViewModels.AboutUs;
using bike.Views.ContactUs;
using bike.Views.Feedback;
using bike.Views.Settings;
using bike.ViewModels.Settings;
using bike.ViewModels.Feedback;
using bike.ViewModels.ContactUs;
using Services;
using Module;

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
        DataTransportFacade transportFacade;
        protected override async void OnInitialized()
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense("NzM3NEAzMTM3MmUzNDJlMzBPRm41TTBEL2hiZ0pjbG93dDZPQ0VocmRCWkJHSXlzWFgrUkxrZVlDaUpzPQ==");
            transportFacade = Container.Resolve<DataTransportFacade>();
            Plugin.Iconize.Iconize
                .With(new Plugin.Iconize.Fonts.MaterialModule())
                .With(new Plugin.Iconize.Fonts.MaterialDesignIconsModule());
            await NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<MapperConfigurationExpression>();
            containerRegistry.RegisterSingleton<IConfigurationProvider, MapperConfiguration>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutUsSimplePage, AboutUsSimpleViewModel>();
            containerRegistry.RegisterForNavigation<ContactUsPage, ContactUsViewModel>();
            containerRegistry.RegisterForNavigation<FeedbackPage, FeedbackViewModel>();
            containerRegistry.RegisterForNavigation<HelpPage, HelpViewModel>();
            containerRegistry.RegisterForNavigation<SettingPage, SettingViewModel>();
            containerRegistry.RegisterForNavigation<ConfigurationPage, ConfigurationViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<CommunicationModule>();
            moduleCatalog.AddModule<BatteryModule>();
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<PedalModule>();
            moduleCatalog.AddModule<ThrottleModule>();
            moduleCatalog.AddModule<LightModule>();
            moduleCatalog.AddModule<ServoModule>();
        }

    }
}
