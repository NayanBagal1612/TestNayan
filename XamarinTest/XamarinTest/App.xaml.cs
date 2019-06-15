using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTest.VIewModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinTest
{
    public partial class App : Application
    {
        private static ViewModelLocator _locator;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
        
        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }       

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
