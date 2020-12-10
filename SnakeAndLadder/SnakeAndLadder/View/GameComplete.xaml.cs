using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SnakeAndLadder.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnakeAndLadder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameComplete : Rg.Plugins.Popup.Pages.PopupPage,INotifyPropertyChanged
    {
        INavigation _navigation;
        public string result;
        public bool _isBot;
        public bool GameMode { get; set; } = true;
        public bool IsResume { get; set; } = false;
        public string VerdictDescription { get; set; }
        public List<WinnerBoard> _winnerBoards { get; set; }
        public List<PlayInfo> _playersinfos { get; set; }
        public List<WinnerBoard> LeaderBoard { get; set; }
        private Color _resColor;
        public Color ResColor
        {
            get => _resColor;
            set { _resColor = value; OnPropertyChanged(); }
        }
        private bool _isBalloon;
        public bool IsBalloon
        {
            get => _isBalloon;
            set { _isBalloon = value; OnPropertyChanged(); }
        }
        public GameComplete(INavigation navigation,string res,bool Isbot, List<WinnerBoard> WinnerBoards, List<PlayInfo> Playersinfos)
        {
            VerdictDescription = res;
            _isBot = Isbot;
            _navigation = navigation;
            LeaderBoard = WinnerBoards;
            _playersinfos = Playersinfos;
            InitializeComponent();
            ShowControls();
        }
        public void ShowControls()
        {
            if (_isBot)
            {
                Thread.Sleep(3000);
                SetVerdictColor();
                GameMode = true;
            }
            else
            {
                GameMode = false;
                if (_playersinfos.Count == LeaderBoard.Count)
                {
                    IsResume = false;
                }
                else
                    IsResume = true;
            }
            ShowEffects();
        }
        public void ShowEffects()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBalloon = true;
                await Task.Delay(10000);
                IsBalloon = false;
            });
       }
        public void SetVerdictColor()
        {
            
                Device.BeginInvokeOnMainThread(async () =>
                {
                    ResColor = Color.FromHex("#FF7F76");
                    await Task.Delay(700);
                    ResColor = Color.FromHex("#f1c40f");
                    await Task.Delay(700);
                    ResColor = Color.FromHex("#50f72a");
                    await Task.Delay(700);
                    ResColor = Color.FromHex("#3b53f7");
                    await Task.Delay(700);
                    SetVerdictColor();
                });
        }
        private async void OnClose(object sender, EventArgs e)
        {            
            await _navigation.PopPopupAsync();
            await _navigation.PopToRootAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {          
            return false;
        }

        private async void Home_Clicked(object sender, EventArgs e)
        {
            await _navigation.PopPopupAsync();
            await _navigation.PopToRootAsync();
        }
        private async void Resume_Clicked(object sender, EventArgs e)
        {
            await _navigation.PopPopupAsync();
        }
        private  async void Share_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Launcher.OpenAsync(new Uri("whatsapp://send?text=" + "Hey, guys the Snake and Ladders is absolute fun. Download the App here!" + "\n" + "http://tiny.cc/saapsiri"));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Not Installed", "Whatsapp Not Installed", "ok");
            }
        }
    }
}