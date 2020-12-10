using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnakeAndLadder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        ISimpleAudioPlayer player;
        public HomePage()
        {
            InitializeComponent();           
            LoadSounds();
            ShowAnime();
        }

        private async void ShowAnime()
        {            
            while (1==1)
            {
                await Task.Delay(1500);
                playimage.RotateTo(90, 3000, Easing.Linear);
                await Task.Delay(1000);
                playimage.RotateTo(0, 1500, Easing.Linear);                
            }
        }
        protected override void OnAppearing()
        {
            player.Loop = true;
            player.Play();
            player.Volume = 0.1;
            NavigationPage.SetHasNavigationBar(this, false);
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            player.Volume = 0;
            player.Loop = false;
        }
        public void LoadSounds()
        {
            var stream1 = GetStreamFromFile("startmusic.mp3");
            player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player.Load(stream1);            
        }
        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("SnakeAndLadder." + filename);

            return stream;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            playimage.IsEnabled = false;
            playimage.ScaleTo(10, 1000, Easing.CubicInOut);
            await Task.Delay(500);
            await Navigation.PushAsync(new GameMode(Navigation, player));
            playimage.ScaleTo(1, 1, Easing.CubicInOut);
            playimage.IsEnabled = true;
        }
    }
}