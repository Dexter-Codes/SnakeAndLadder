using Plugin.SimpleAudioPlayer;
using SnakeAndLadder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnakeAndLadder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameMode : ContentPage, INotifyPropertyChanged
    {

        #region UI

        INavigation _navigation;
        ISimpleAudioPlayer _player;
        ISimpleAudioPlayer playclick;
        public string result;
        public GameMode(INavigation navigation, ISimpleAudioPlayer player)
        {
            _navigation = navigation;
            _player = player;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ViewModelMethods();            
        }
        protected override void OnAppearing()
        {
            _player.Play();
            _player.Loop = true;
            _player.Volume = 0.1;
            LoadSounds();           
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            _player.Stop();
        }       
        public void LoadSounds()
        {
            var stream1 = GetStreamFromFile("watersplash.mp3");
            playclick = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            playclick.Load(stream1);
            playclick.Volume = 0.1;
        }
        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("SnakeAndLadder." + filename);

            return stream;
        }
        private async void play_btn_Clicked(object sender, EventArgs e)
        {
            play_btn.IsEnabled = false;
            PerformOperations();
        }
        private async void PerformAnimations()
        {
            await play_btn.ScaleTo(2, 500, Easing.SpringOut);
            playclick.Volume = 0.7;
            playclick.Play();
            Device.BeginInvokeOnMainThread(async () =>
            {
                _player.Stop();
                PlayImage = "ws4.png";
                await play_btn.ScaleTo(8, 2000, Easing.SpringOut);
                await Task.Delay(1000);
            });
            Task.Delay(100).Wait();
        }
        public async void PerformOperations()
        {
            if (SingleModeOn)
            {
                checker.Clear();
                AddPlayersForMultiPlay();
                if (checker.Count == 0 || checker.Count > 1)
                {
                    await App.Current.MainPage.DisplayAlert("Hey Mate", "Please select any one color to start your Game!!", "OK");
                    play_btn.IsEnabled = true;
                }
                else
                {
                    AddRelevantPlayers();
                    PerformAnimations();
                    await _navigation.PushAsync(new Board(_navigation, playInfos, true));
                    play_btn.IsEnabled = true;
                }
            }
            else
            {
                checker.Clear();
                
                AddPlayersForMultiPlay();
                var ok=  ValidateEntries();

                if (ok == 1)
                {
                    await App.Current.MainPage.DisplayAlert("Hey Mate", "Please add the minimum number of players for the MultiPlay to start!!", "OK");
                    play_btn.IsEnabled = true;
                }

                else
                {
                    AddRelevantPlayers();
                    PerformAnimations();
                    await _navigation.PushAsync(new Board(_navigation, playInfos, false));
                    play_btn.IsEnabled = true;
                }
            }
        }
        private async void bot_Clicked(object sender, EventArgs e)
        {
            await bot.ScaleTo(1.4, 300, Easing.Linear);
            SingleModeOn = true;
            EntryOn = false;
            Player1on = true;
            Player3on = false;
            Player2on = false;
            Player4on = false;
            PlayMode = "PLAY VS BOT";
            PlayConditions = "Please choose ur relevant color";
            IsModeSelected = false;
            bot.ScaleTo(1, 1, Easing.Linear);
        }

        private async void people_Clicked(object sender, EventArgs e)
        {
            await people.ScaleTo(1.4, 300, Easing.Linear);
            SingleModeOn = false;
            EntryOn = true;
            Player1on = true;
            Player2on = true;
            Player3on = false;
            Player4on = false;
            PlayMode = "MULTIPLAYER";
            PlayConditions = "Minimum 2 players should be selected";
            IsModeSelected = false;
            people.ScaleTo(1, 1, Easing.Linear);
        }

        private void backButton_Clicked(object sender, EventArgs e)
        {
            if (IsModeSelected)
            {
                _navigation.PopAsync();
            }
            else
            {
                if (SingleModeOn)
                {
                    IsModeSelected = true;
                    SingleModeOn = false;
                }
                else if (!SingleModeOn)
                {
                    IsModeSelected = true;
                    SingleModeOn = false;
                }
            }
        }

        #endregion

        #region DataBinding Logic


        public void ViewModelMethods()
        {
            playInfos = new List<PlayInfo>();
            checker = new ObservableCollection<object>();
        }
       
        #region Properties Definition
       
        public string VerdictDescription { get; set; }
        public List<PlayInfo> playInfos { get; set; }
        public ObservableCollection<object> checker { get; set; }
        public bool SingleModeOn { get; set; }

        public string _playMode;
        public string PlayMode
        {
            get => _playMode;
            set { _playMode = value; OnPropertyChanged(); }
        }
        public string _playConditions;
        public string PlayConditions
        {
            get => _playConditions;
            set { _playConditions = value; OnPropertyChanged(); }
        }
        public bool _entryOn = true;
        public bool EntryOn
        {
            get => _entryOn;
            set { _entryOn = value; OnPropertyChanged(); }
        }
        private bool _IsModeSelected = true;
        public bool IsModeSelected
        {
            get => _IsModeSelected;
            set { _IsModeSelected = value; OnPropertyChanged(); }
        }
        private bool _player1on = true;
        public bool Player1on
        {
            get => _player1on;
            set { _player1on = value; OnPropertyChanged();  }
        }
        private bool _player2on = true;
        public bool Player2on
        {
            get => _player2on;
            set { _player2on = value; OnPropertyChanged();  }
        }
        private bool _player3on = true;
        public bool Player3on
        {
            get => _player3on;
            set { _player3on = value; OnPropertyChanged();  }
        }
        private bool _player4on = true;
        public bool Player4on
        {
            get => _player4on;
            set { _player4on = value; OnPropertyChanged();  }
        }
        private string _player1Name;
        public string Player1Name
        {
            get => _player1Name;
            set { _player1Name = value; OnPropertyChanged(); }
        }
        private string _player2Name;
        public string Player2Name
        {
            get => _player2Name;
            set { _player2Name = value; OnPropertyChanged(); }
        }
        private string _player3Name;
        public string Player3Name
        {
            get => _player3Name;
            set { _player3Name = value; OnPropertyChanged(); }
        }
        private string _player4Name;
        public string Player4Name
        {
            get => _player4Name;
            set { _player4Name = value; OnPropertyChanged(); }
        }
        private string _playImage="go.png";
        public string PlayImage
        {
            get => _playImage;
            set { _playImage = value; OnPropertyChanged(); }
        }

        #endregion


        #region Associated Methods

      
        public void AddPlayersForMultiPlay()
        {
            if (Player1on)
                checker.Add(Player1on);
            if (Player2on)
                checker.Add(Player2on);
            if (Player3on)
                checker.Add(Player3on);
            if (Player4on)
                checker.Add(Player4on);
        }
        public void AddRelevantPlayers()
        {
            if (Player1on)
            {
                playInfos.Add(new PlayInfo
                {
                    PlayerName = string.IsNullOrEmpty(Player1Name) ? "KHALEESI" : Player1Name,
                    Id = 0,
                    Colour = "#E74C3C",
                    Pin = "redpin.png",
                    Score = 0,
                    PlayerOn = false
                });
            }
            if (Player2on)
            {
                playInfos.Add(new PlayInfo
                {
                    PlayerName = string.IsNullOrEmpty(Player2Name) ? "SANSA" : Player2Name,
                    Id = 1,
                    Colour = "#9B59B6",
                    Pin = "bluepin.png",
                    Score = 0,
                    PlayerOn = false
                });
            }
            if (Player3on)
            {
                playInfos.Add(new PlayInfo
                {
                    PlayerName = string.IsNullOrEmpty(Player3Name) ? "JON SNOW" : Player3Name,
                    Id = 2,
                    Colour = "#f1c40f",
                    Pin = "yellowpin.png",
                    Score = 0,
                    PlayerOn = false
                });
            }
            if (Player4on)
            {
                playInfos.Add(new PlayInfo
                {
                    PlayerName = string.IsNullOrEmpty(Player4Name) ? "AARYA" : Player4Name,
                    Id = 3,
                    Colour = "#2ECC71",
                    Pin = "greenpin.png",
                    Score = 0,
                    PlayerOn = false
                });
            }

            if (SingleModeOn)
            {

                playInfos.Add(new PlayInfo
                {
                    PlayerName = "BOT",
                    Id = 101,
                    Colour = "#BBB9B9",
                    Pin = "graypin.png",
                    Score = 0,
                    PlayerOn = false
                });
            }
        }
   
        public int ValidateEntries()
        {
            if (checker.Count < 2)
            {
                return 1;
            }
            else
            {
                if (Player1on == true && string.IsNullOrEmpty(Player1Name))
                {
                    return 2;
                }
                if (Player2on == true && string.IsNullOrEmpty(Player2Name))
                {
                    return 2;
                }
                if (Player3on == true && string.IsNullOrEmpty(Player3Name))
                {
                    return 2;
                }
                if (Player4on == true && string.IsNullOrEmpty(Player4Name))
                {
                    return 2;
                }
                return 2;
            }
        }


        #endregion

        #endregion
      
                          
    }
}