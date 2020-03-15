using Prism;
using Prism.Ioc;
using bike.ViewModels;
using bike.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AutoMapper;
using AutoMapper.Configuration;
using Prism.Modularity;
using Battery;

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
            Plugin.Iconize.Iconize
                .With(new Plugin.Iconize.Fonts.MaterialModule())
                .With(new Plugin.Iconize.Fonts.MaterialDesignIconsModule());
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<MapperConfigurationExpression>();
            containerRegistry.RegisterSingleton<IConfigurationProvider, MapperConfiguration>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<BatteryModule>();
        }

    }
}
