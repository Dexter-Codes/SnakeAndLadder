using SnakeAndLadder.Interface;
using SnakeAndLadder.View;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnakeAndLadder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

             MainPage = new NavigationPage(new HomePage());
             CheckVersion();
        }
        async void CheckVersion()
        {
            bool isLatestVersion = await DependencyService.Get<ILatest>().IsUsingLatestVersion();

            if (isLatestVersion)
            {
                bool res = await App.Current.MainPage.DisplayAlert("Hey Mate", "A New version is available for download! Do you want to update it now?", "Yes", "No");
                if (res)
                {
                    await DependencyService.Get<ILatest>().OpenAppInStore();
                }
            }
        }

        protected  override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
          
        }
    }
}
