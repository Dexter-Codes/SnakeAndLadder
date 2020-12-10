using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Extensions;
using SnakeAndLadder.Interface;
using SnakeAndLadder.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SnakeAndLadder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Board : ContentPage,INotifyPropertyChanged
    {
        INavigation _navigation;
        public List<BoardInfo> boardInfos { get; set; }
        List<PlayInfo> playInfos { get; set; }
        public bool isBot { get; set; }
        public Board(INavigation navigation,List<PlayInfo> PlayInfos,bool Isbot)
        {
            _navigation = navigation;
            boardInfos = new List<BoardInfo>();
            playInfos = PlayInfos;
            isBot = Isbot;
            InitializeComponent();
            AddBoardView();
            NavigationPage.SetHasNavigationBar(this, false);
            ViewModelMethods();
        }
        protected override void OnAppearing()
        {            
            base.OnAppearing();
        }
        protected  override bool OnBackButtonPressed()
        {
            bool result=true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                var answer = await App.Current.MainPage.DisplayAlert("Hey Mate", "The Game progress would be lost if you go back. Are u sure, u wanna go back?", "Yes", "No I Don't");
                if (answer)
                {
                    await _navigation.PopAsync();
                    await _navigation.PopToRootAsync();
                }
                else
                {
                     return;
                }
                result = answer;
            });
            Task.Delay(1500);
            return result;
            base.OnBackButtonPressed();           
        }
       
        #region BoardViewMethods
        public Color SetColor(int r,int c)
        {
            Color test=Color.Red;
            if (r % 2 == 0)
            {
                if(c % 2 ==0)
                    test=Color.Red;
                else
                    test = Color.White;
            }                
            else
            {
                if (c % 2 == 0)
                    test = Color.White;
                else
                    test = Color.Red;
            }

            return test;

        }
       
        public void AddBoardView()
        {
            
            int num = 100;
            for (int row=0;row<10;row++)
            {
                List<Label> labelInstance = new List<Label>();
                var temp = new List<BoardInfo>();
                for (int col=0;col<10;col++)
                {
                   
                    var label = new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        BackgroundColor = Color.Transparent,
                        FontSize=18.0f,
                        FontAttributes=FontAttributes.None,
                        TextColor= Color.Cornsilk,
                        InputTransparent = true,
                    };

                  
                    var frame = new Frame
                    {
                        BorderColor = Color.Transparent,
                        BackgroundColor = Color.Transparent,
                        CornerRadius = 0,
                        HasShadow = true,
                        Content = label,
                        Margin = new Thickness(0, 0, 0, 0),
                        Padding = new Thickness(0, 0, 0, 0),
                        InputTransparent = true,                                               
                    };


                    BoardGrid.Children.Add(frame, col, row);
                    
                    temp.Add(new BoardInfo
                    {
                        Row = row.ToString(),
                        Col = col.ToString(),
                        Instance = label,                        
                    });
                    labelInstance.Add(label);
                }
                if(row%2==0)
                {
                    int y = labelInstance.Count;
                    for (int x = 0; x < y; x++)
                    {
                        labelInstance[x].Text = num.ToString();
                        temp[x].Value = Convert.ToInt32(labelInstance[x].Text);
                        num--;
                    }

                    boardInfos.AddRange(temp);
                    temp.Clear();
                    labelInstance.Clear();
                }               
                else
                {                  
                    int y = labelInstance.Count;
                    for (int x = y - 1; x >= 0; x--)
                    {
                        labelInstance[x].Text = num.ToString();
                        temp[x].Value = Convert.ToInt32(labelInstance[x].Text);
                        num--;
                    }
                    boardInfos.AddRange(temp);
                    temp.Clear();
                    labelInstance.Clear();
                }
            }
        
        }
       
        #endregion
        void ViewModelMethods()
        {
            Images = new List<string>();
            SixCounts = new List<int>();
            temp = new List<PlayInfo>();
            winners = new List<WinnerBoard>();
       
            AddImages();
            LoadSounds();
            AddDefaultValues();
            SetPointBackground(playInfos[0].Id);
            AddPlayerColors(playInfos);
            PlayStartEffects();
        }
        #region Variables

        private bool _isDice1 = true;
        public bool IsDice1
        {
            get => _isDice1;
            set { _isDice1 = value; OnPropertyChanged(); }
        }
        private bool _rollup;
        public bool Rollup
        {
            get => _rollup;
            set { _rollup = value; OnPropertyChanged(); }
        }
        private bool _botEmpty;
        public bool BotEmpty
        {
            get => _botEmpty;
            set { _botEmpty = value; OnPropertyChanged(); }
        }
        private bool _isDiceAvailable = true;
        public bool IsDiceAvailable
        {
            get => _isDiceAvailable;
            set { _isDiceAvailable = value; OnPropertyChanged(); }
        }
        private string _diceNumber= "countdown.gif";
        public string DiceNumber
        {
            get => _diceNumber;
            set { _diceNumber = value; OnPropertyChanged(); }
        }
        private string _eventItem;
        public string EventItem
        {
            get => _eventItem;
            set { _eventItem = value; OnPropertyChanged(); }
        }
        private bool _isEventItem;
        public bool IsEventItem
        {
            get => _isEventItem;
            set { _isEventItem = value; OnPropertyChanged(); }
        }
        private bool _playGif=true;
        public bool PlayGif
        {
            get => _playGif;
            set { _playGif = value; OnPropertyChanged(); }
        }
        private string _mute="unmute.png";
        public string Mute
        {
            get => _mute;
            set { _mute = value; OnPropertyChanged(); }
        }
        private bool _isMute;
        public bool IsMute
        {
            get => _isMute;
            set { _isMute = value; OnPropertyChanged(); }
        }
        private bool _animationCheck1;
        public bool AnimationCheck1
        {
            get => _animationCheck1;
            set { _animationCheck1 = value; OnPropertyChanged(); }
        }
        private bool _animationCheck2;
        public bool AnimationCheck2
        {
            get => _animationCheck2;
            set { _animationCheck2 = value; OnPropertyChanged(); }
        }
        private bool _animationCheck3;
        public bool AnimationCheck3
        {
            get => _animationCheck3;
            set { _animationCheck3 = value; OnPropertyChanged(); }
        }
        private bool _animationCheck4;
        public bool AnimationCheck4
        {
            get => _animationCheck4;
            set { _animationCheck4 = value; OnPropertyChanged(); }
        }
        private bool _animationOn;
        public bool AnimationOn
        {
            get => _animationOn;
            set { _animationOn = value; OnPropertyChanged(); }
        }
        private bool _isPin1;
        public bool IsPin1
        {
            get => _isPin1;
            set { _isPin1 = value; OnPropertyChanged(); }
        }
        private bool _isPin2;
        public bool IsPin2
        {
            get => _isPin2;
            set { _isPin2 = value; OnPropertyChanged(); }
        }
        private bool _isPin3;
        public bool IsPin3
        {
            get => _isPin3;
            set { _isPin3 = value; OnPropertyChanged(); }
        }
        private bool _isPin4;
        public bool IsPin4
        {
            get => _isPin4;
            set { _isPin4 = value; OnPropertyChanged(); }
        }
        public bool _enableButton = true;
        public bool EnableButton
        {
            get => _enableButton;
            set { _enableButton = value; OnPropertyChanged(); }
        }
        public string _rowNumber1;
        public string RowNumber1
        {
            get => _rowNumber1;
            set { _rowNumber1 = value; OnPropertyChanged(); }
        }
        public string _columnNumber1;
        public string ColumnNumber1
        {
            get => _columnNumber1;
            set { _columnNumber1 = value; OnPropertyChanged(); }
        }
        public string _rowNumber2;
        public string RowNumber2
        {
            get => _rowNumber2;
            set { _rowNumber2 = value; OnPropertyChanged(); }
        }
        public string _columnNumber2;
        public string ColumnNumber2
        {
            get => _columnNumber2;
            set { _columnNumber2 = value; OnPropertyChanged(); }
        }

        public string _rowNumber3;
        public string RowNumber3
        {
            get => _rowNumber3;
            set { _rowNumber3 = value; OnPropertyChanged(); }
        }
        public string _columnNumber3;
        public string ColumnNumber3
        {
            get => _columnNumber3;
            set { _columnNumber3 = value; OnPropertyChanged(); }
        }
        public string _rowNumber4;
        public string RowNumber4
        {
            get => _rowNumber4;
            set { _rowNumber4 = value; OnPropertyChanged(); }
        }
        public string _columnNumber4;
        public string ColumnNumber4
        {
            get => _columnNumber4;
            set { _columnNumber4 = value; OnPropertyChanged(); }
        }
        public string _rollDice;
        public string RollDice
        {
            get => _rollDice;
            set { _rollDice = value; OnPropertyChanged(); }
        }
        public string _player1;
        public string Player1 
        {
            get => _player1;
            set { _player1 = value; OnPropertyChanged(); }
        }
        public string _player2;
        public string Player2
        {
            get => _player2;
            set { _player2 = value; OnPropertyChanged(); }
        }
        public string _player3;
        public string Player3
        {
            get => _player3;
            set { _player3 = value; OnPropertyChanged(); }
        }
        public string _player4;
        public string Player4
        {
            get => _player4;
            set { _player4 = value; OnPropertyChanged(); }
        }
        
        public string _player1Points;
        public string Player1Points
        {
            get => _player1Points;
            set { _player1Points = value; OnPropertyChanged(); }
        }
        public string _player2Points;
        public string Player2Points
        {
            get => _player2Points;
            set { _player2Points = value; OnPropertyChanged(); }
        }
        public string _player3Points;
        public string Player3Points
        {
            get => _player3Points;
            set { _player3Points = value; OnPropertyChanged(); }
        }
        public string _player4Points;
        public string Player4Points
        {
            get => _player4Points;
            set { _player4Points = value; OnPropertyChanged(); }
        }

        private string _player1Color;
        public string Player1Color
        {
            get => _player1Color;
            set { _player1Color = value; OnPropertyChanged(); }
        }
        private string _player2Color;
        public string Player2Color
        {
            get => _player2Color;
            set { _player2Color = value; OnPropertyChanged(); }
        }
        private string _player3Color;
        public string Player3Color
        {
            get => _player3Color;
            set { _player3Color = value; OnPropertyChanged(); }
        }
        private string _player4Color;
        public string Player4Color
        {
            get => _player4Color;
            set { _player4Color = value; OnPropertyChanged(); }
        }
        private Color _player1Background;
        public Color Player1Background
        {
            get => _player1Background;
            set { _player1Background = value; OnPropertyChanged(); }
        }
        private Color _player2Background;
        public Color Player2Background
        {
            get => _player2Background;
            set { _player2Background = value; OnPropertyChanged(); }
        }
        private Color _player3Background;
        public Color Player3Background
        {
            get => _player3Background;
            set { _player3Background = value; OnPropertyChanged(); }
        }
        private Color _player4Background;
        public Color Player4Background
        {
            get => _player4Background;
            set { _player4Background = value; OnPropertyChanged(); }
        }
        private Color _player1TextColor;
        public Color Player1TextColor
        {
            get => _player1TextColor;
            set { _player1TextColor = value; OnPropertyChanged(); }
        }
        private Color _player2TextColor;
        public Color Player2TextColor
        {
            get => _player2TextColor;
            set { _player2TextColor = value; OnPropertyChanged(); }
        }
        private Color _player3TextColor;
        public Color Player3TextColor
        {
            get => _player3TextColor;
            set { _player3TextColor = value; OnPropertyChanged(); }
        }
        private Color _player4TextColor;
        public Color Player4TextColor
        {
            get => _player4TextColor;
            set { _player4TextColor = value; OnPropertyChanged(); }
        }
        private bool _player1Layout=true;
        public bool Player1Layout
        {
            get => _player1Layout;
            set { _player1Layout = value; OnPropertyChanged(); }
        }
        private bool _player2Layout=true;
        public bool Player2Layout
        {
            get => _player2Layout;
            set { _player2Layout = value; OnPropertyChanged(); }
        }
        private bool _player3Layout=true;
        public bool Player3Layout
        {
            get => _player3Layout;
            set { _player3Layout = value; OnPropertyChanged(); }
        }
        private bool _player4Layout=true;
        public bool Player4Layout
        {
            get => _player4Layout;
            set { _player4Layout = value; OnPropertyChanged(); }
        }
        private string _pageImage = "blackbg.png";
        public string PageImage 
        {
            get => _pageImage;
            set { _pageImage = value; OnPropertyChanged(); }
        }
        private bool _swipped;
        public bool Swipped
        {
            get => _swipped;
            set { _swipped = value; OnPropertyChanged(); }
        }
        private string _pin1;
        public string Pin1
        {
            get => _pin1;
            set { _pin1 = value; OnPropertyChanged(); }
        }
        private string _pin2;
        public string Pin2
        {
            get => _pin2;
            set { _pin2 = value; OnPropertyChanged(); }
        }
        private string pin3;
        public string Pin3
        {
            get => pin3;
            set { pin3 = value; OnPropertyChanged(); }
        }
        private string _pin4;
        public string Pin4
        {
            get => _pin4;
            set { _pin4 = value; OnPropertyChanged(); }
        }
        private bool _isCheckRule;
        public bool IsCheckRule
        {
            get => _isCheckRule;
            set { _isCheckRule = value; OnPropertyChanged(); }
        }
        private bool _value;
        public bool Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }
        public List<string> Images { get; set; }
        public List<PlayInfo> temp { get; set; }
        public List<WinnerBoard> winners { get; set; }
        public int DiceValue { get; set; }
        public bool PlayerOn1 { get; set; }
        public bool PlayerOn2 { get; set; }
        public int GamePoints1 { get; set; } = 1;
        public int GamePoints2 { get; set; } = 1;
        public int StarterValue { get; set; } = 0;
        public int AfterSixPoints { get; set; }
        public bool IfSix { get; set; }
        public List<int> SixCounts { get; set; }
        ISimpleAudioPlayer player;
        ISimpleAudioPlayer move;
        ISimpleAudioPlayer gamestart;
        ISimpleAudioPlayer playstart;
        ISimpleAudioPlayer botplaystart;
        ISimpleAudioPlayer hiss;
        ISimpleAudioPlayer climb;
        ISimpleAudioPlayer six;
        ISimpleAudioPlayer sad;
        ISimpleAudioPlayer cheer;
        ISimpleAudioPlayer lost;
        public bool _is1On;
        public bool Is1On
        {
            get => _is1On;
            set { Is1On = value; OnPropertyChanged(); }
        }
        public int nos = 0;
        public int toastcount=0;
        public int Globali = 0;
        public int SixCount = 0;
        public int Counter=0;
        #endregion

        #region Initialise HelperMethods

        public void AddImages()
        {
            Images.Add("roll1.png");
            Images.Add("dice1.png");
            Images.Add("dice2.png");
            Images.Add("dice3.png");
            Images.Add("dice4.png");
            Images.Add("dice5.png");
            Images.Add("dice6.png");
        }

        public void LoadSounds()
        {
            var stream1 = GetStreamFromFile("dicerolling.mp3");
            player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player.Load(stream1);

            var stream = GetStreamFromFile("cheer.mp3");
            playstart = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            playstart.Load(stream);
            
            var stream0 = GetStreamFromFile("start.mp3");
            botplaystart = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            botplaystart.Load(stream0);

            var stream2 = GetStreamFromFile("move.mp3");
            move = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            move.Load(stream2);

            var stream3 = GetStreamFromFile("gamestart.mp3");
            gamestart = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            gamestart.Load(stream3);

            var stream4 = GetStreamFromFile("snakehit.mp3");
            hiss = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            hiss.Load(stream4);

            var stream5 = GetStreamFromFile("sixer.mp3");
            six = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            six.Load(stream5);

            var stream6 = GetStreamFromFile("ladderclimb.mp3");
            climb = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            climb.Load(stream6);

            var stream7 = GetStreamFromFile("cheering.mp3");
            cheer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            cheer.Load(stream7);

            var stream8 = GetStreamFromFile("lose.mp3");
            lost = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            lost.Load(stream8);

            var stream9 = GetStreamFromFile("angry.mp3");
            sad = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            sad.Load(stream9);

        }

        public void AddDefaultValues()
        {
            if(!isBot)
            {
                Player1Points = "-";
                Player2Points = "-";
                Player3Points = "-";
                Player4Points = "-";

                Player1Color = "#E74C3C";
                Player2Color = "#9B59B6";
                Player3Color = "#F1C40F";
                Player4Color = "#2ECC71";

            }           
        }
        public void AddPlayerColors(List<PlayInfo> players)
        {
            var count = players.Count;
            temp.AddRange(players);
            nos = count;
            RollDice = "roll3.png";
            if (isBot)
            {
                Player1Color = players[0].Colour;
                Player1 = players[0].PlayerName;
                Pin1 = players[0].Pin;
                Player1Points = "0";

                Player2Color = players[1].Colour;
                Player2 = players[1].PlayerName;
                Pin2 = players[1].Pin;
                Player2Points = "0";
            }
            else
            {
                if(players.Count>0)
                {
                    var p1 = players.Where(x => x.Id == 0).FirstOrDefault();
                    if (p1 != null)
                    {
                        Player1Color = p1.Colour;
                        Player1 = p1.PlayerName;
                        Pin1 = p1.Pin;
                        Player1Layout = true;
                        Player1Points = "0";
                    }
                    var p2 = players.Where(x => x.Id == 1).FirstOrDefault();
                    if (p2 != null)
                    {
                        Player2Color = p2.Colour;
                        Player2 = p2.PlayerName;
                        Pin2 = p2.Pin;
                        Player2Layout = true;
                        Player2Points = "0";
                    }
                    var p3 = players.Where(x => x.Id == 2).FirstOrDefault();
                    if (p3 != null)
                    {
                        Player3Color = p3.Colour;
                        Player3 = p3.PlayerName;
                        Pin3 = p3.Pin;
                        Player3Layout = true;
                        Player3Points = "0";
                    }
                    var p4 = players.Where(x => x.Id == 3).FirstOrDefault();
                    if (p4 != null)
                    {
                        Player4Color = p4.Colour;
                        Player4 = p4.PlayerName;
                        Pin4 = p4.Pin;
                        Player4Layout = true;
                        Player4Points = "0";
                    }
                }             
            }
        }


        #endregion

        #region UserInteration Methods
        private void RollDice_Tapped(object sender, EventArgs e)
        {
            Rollup = false;
            Task.Run(async () =>
            {
                player.Play();
                Roll(false);
                Counter++;
                for (int i = StarterValue; i < nos; i++)
                {
                    EnableButton = false;
                    playInfos[i].Moves++;
                    Task.Delay(1000).Wait();
                    ShowAfterLoad(false,playInfos[i]);
                    Rollup = true;
                    Task.Delay(1000).Wait();
                    if (playInfos[i].PlayerOn)
                    {
                        if (playInfos[i].Score >= 94)
                        {
                            if (DiceValue != 6)
                            {
                                if (!IfSix)
                                {
                                    var game = CheckDiceValid(playInfos[i].Score, DiceValue);
                                    if (game)
                                    {
                                        goto here;
                                    }
                                }
                                else
                                {
                                    IfSix = false;
                                    SixCounts.Clear();
                                    goto here;
                                }
                            }
                            else
                            {
                                goto here;
                            }
                        }

                        if (DiceValue == 6)
                        {
                            IfSix = true;
                            if (SixCounts.Count < 2)
                            {
                                SixCounts.Add(6);
                                SixCount++;
                            }
                            else
                            {
                                SixCount++;
                                SixCounts.Clear();                                
                            }
                            Task.Delay(200).Wait();
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await DependencyService.Get<IMessage>().ShortAlert(playInfos[i].PlayerName + " has hit 6. Please Roll Again!!");
                                EnableButton = true;                                
                                StarterValue = i;
                                return;
                            });                           
                            return;

                        }
                        else if (IfSix)
                        {
                            IfSix = false;
                            SixCount = 0;
                            int countTotal = 0;
                            for (int j = 0; j < SixCounts.Count; j++)
                            {
                                countTotal = countTotal + SixCounts[j];
                            }
                            SixCounts.Clear();
                            int tempscore = playInfos[i].Score;
                            int temptotal= tempscore + countTotal + DiceValue;
                            if(temptotal > 100)
                            {
                                goto here;
                            }
                            Globali  = playInfos[i].Score;
                            playInfos[i].Score = playInfos[i].Score + countTotal + DiceValue;
                            if (playInfos[i].Score == 100)
                            {
                                goto finish;
                            }                           
                            else
                            {
                                if (isBot)
                                {
                                    GlowMarkedPins(playInfos[i].Id, Swipped, true);
                                    StarterValue = i;
                                    if (!Application.Current.Properties.ContainsKey("ID"))
                                        ShowUserPrompt();
                                    return;
                                }
                                else
                                {
                                    GlowMarkedPins(playInfos[i].Id, Swipped, false);
                                    StarterValue = i;
                                    if (!Application.Current.Properties.ContainsKey("ID"))
                                        ShowUserPrompt();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Globali = playInfos[i].Score;
                            playInfos[i].Score = playInfos[i].Score + DiceValue;

                            if (playInfos[i].Score == 100)
                            {
                                goto finish;
                            }
                            else
                            {
                                if (isBot)
                                {                                   
                                    GlowMarkedPins(playInfos[i].Id, Swipped,true);
                                    StarterValue = i;
                                    if (!Application.Current.Properties.ContainsKey("ID"))
                                        ShowUserPrompt();
                                    return;                                   
                                }
                                else
                                {
                                    GlowMarkedPins(playInfos[i].Id, Swipped,false);
                                    StarterValue = i;
                                    if (!Application.Current.Properties.ContainsKey("ID"))
                                        ShowUserPrompt();
                                    return;
                                }
                            }
                        }
                    }

                    else if (DiceValue == 6 && !playInfos[i].PlayerOn)
                    {                      
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            playInfos[i].PlayerOn = true;
                            playInfos[i].Score = 1;
                            if(!isBot)
                            {
                                if (playInfos[i].Id == 0)
                                    IsPin1 = true;
                                else if (playInfos[i].Id == 1)
                                    IsPin2 = true;
                                else if (playInfos[i].Id == 2)
                                    IsPin3 = true;
                                else if (playInfos[i].Id == 3)
                                    IsPin4 = true;
                            }
                            else
                            {
                                IsPin1 = true;
                            }

                            if (isBot)
                            {
                                BoardUpdate(playInfos[i].Score, i);
                            }
                            else
                                BoardUpdate(playInfos[i].Score, playInfos[i].Id);
                        });
                    }

                    here:
                    if (isBot)
                    {
                        await Task.Delay(500);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            SetPointBackground(playInfos[1].Id);
                            DependencyService.Get<IMessage>().ShortAlert("Bot's Turn!!");
                        });
                        await Task.Delay(3000);
                        DoForBot();                       
                        await Task.Delay(3000);
                        return;
                    }

                    else
                    {
                        await Task.Delay(1000);
                        i++;
                        StarterValue = i;
                        if (StarterValue > nos - 1)
                        {
                            StarterValue = 0;
                        }
                        Task.Delay(100).Wait();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            RollDice = "roll3.png";
                            SetPointBackground(playInfos[StarterValue].Id);
                            if (nos > toastcount)
                            {
                                toastcount++;
                                await DependencyService.Get<IMessage>().ShortAlert(playInfos[StarterValue].PlayerName + "'s" + " Turn");
                            }
                            EnableButton = true;
                            return;
                        });
                        Task.Delay(100).Wait();
                        return;
                    }

                    finish:
                    if (isBot)
                    {
                        BoardUpdate(playInfos[i].Score, i);
                    }
                    else
                        BoardUpdate(playInfos[i].Score, playInfos[i].Id);

                    await Task.Delay(100);


                    if (!isBot)
                    {
                        winners.Add(new WinnerBoard { Name = playInfos[i].PlayerName, Color = playInfos[i].Colour });
                        if (playInfos[i].Id == 0)
                        {
                            Player1Points = "100";
                            IsPin1 = false;
                        }
                        else if (playInfos[i].Id == 1)
                        {
                            Player2Points = "100";
                            IsPin2 = false;
                        }
                        else if (playInfos[i].Id == 2)
                        {
                            Player3Points = "100";
                            IsPin3 = false;
                        }
                        else if (playInfos[i].Id == 3)
                        {
                            Player4Points = "100";
                            IsPin4 = false;
                        }
                    }
                        var name = playInfos[i].PlayerName;
                    if (!isBot)
                    {
                        playInfos.RemoveAt(i);
                        var count = playInfos.Count;
                        nos = count;

                        if (i == 0)
                        {
                            StarterValue = 0;
                        }
                        else if (i == 1)
                        {
                            if (nos > i)
                                StarterValue = 1;
                            else
                                StarterValue = 0;
                        }
                        else if (i == 2)
                        {
                            if (nos > i)
                                StarterValue = 2;
                            else
                                StarterValue = 0;
                        }
                        else if (i == 3)
                        {
                            StarterValue = 0;
                        }
                    }
                    
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        EnableButton = true;
                        if(playInfos.Count>0)
                        SetPointBackground(playInfos[StarterValue].Id);
                        await _navigation.PushPopupAsync(new GameComplete(_navigation, "Winner is " + name, isBot, winners, temp));
                    });
                    return;
                }
            });
        }

        private void DoForBot()
        {
            Rollup = false;
            Task.Run(async () =>
            {
                player.Play();
                Roll(true);
                Task.Delay(1000).Wait();
                ShowAfterLoad(true, playInfos[0]);
                Rollup = true;
                BotEmpty = true;
                Task.Delay(1000).Wait();


                if (playInfos[1].PlayerOn)
                {
                    if (playInfos[1].Score >= 94)
                    {
                        if (DiceValue != 6)
                        {
                            if (!IfSix)
                            {
                                var game = CheckDiceValid(playInfos[1].Score, DiceValue);
                                if (game)
                                {
                                    goto now;
                                }
                            }
                            else
                            {
                                IfSix = false;
                                SixCounts.Clear();
                                goto now;
                            }

                        }
                        else
                        {
                            goto now;
                        }

                    }

                    if (DiceValue == 6)
                    {
                        IfSix = true;
                        if (SixCounts.Count < 2)
                        {
                            SixCounts.Add(6);
                        }
                        else
                        {
                            SixCounts.Clear();
                            SixCounts.Add(6);
                        }
                        Task.Delay(250).Wait();
                        BotEmpty = true;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Bot has rolled 6's");
                        });
                        await Task.Delay(1000);
                        goto Sixspot;
                    }
                    else if (IfSix)
                    {
                        IfSix = false;
                        BotEmpty = true;
                        int countTotal = 0;
                        for (int i = 0; i < SixCounts.Count; i++)
                        {
                            countTotal = countTotal + SixCounts[i];
                        }
                        SixCounts.Clear();
                        int tempscore = playInfos[1].Score;
                        int temptotal = tempscore + countTotal + DiceValue;
                        if (temptotal > 100)
                        {
                            goto now;
                        }
                        playInfos[1].Score = playInfos[1].Score + countTotal + DiceValue;
                        if (playInfos[1].Score == 100)
                        {                            
                            goto botfinish;
                        }
                        else
                            BoardUpdate(playInfos[1].Score, 1);
                    }
                    else
                    {
                        BotEmpty = true;
                        playInfos[1].Score = playInfos[1].Score + DiceValue;
                        if (playInfos[1].Score == 100)
                        {
                            goto botfinish;
                        }
                        else
                            BoardUpdate(playInfos[1].Score, 1);
                    }

                }
                if (DiceValue == 6 && !playInfos[1].PlayerOn)
                {
                    BotStartEffect();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        playInfos[1].PlayerOn = true;
                        playInfos[1].Score = 1;
                        IsPin2 = true;
                        FindCoordinate(playInfos[1].Score, 1);
                    });
                }
                now:
                await Task.Delay(1500);
                Device.BeginInvokeOnMainThread(async () =>
                {
                        StarterValue = 0;
                        SetPointBackground(StarterValue);
                        await DependencyService.Get<IMessage>().ShortAlert("Your Turn");
                        EnableButton = true;
                });
                return;
                
                botfinish:
                Device.BeginInvokeOnMainThread(async () =>
                {
                    BoardUpdate(playInfos[1].Score, 1);
                    winners.Add(new WinnerBoard { Name = playInfos[1].PlayerName, Color = playInfos[1].Colour });
                    await _navigation.PushPopupAsync(new GameComplete(_navigation, "Winner is " + "BOT", isBot, winners, temp));
                });
                await Task.Delay(1500);
                return;

                Sixspot:
                DoForBot();
            });

        }

        private  void Pin_Tapped(object sender, EventArgs e)
        {          
           TapOperations();
        }

        public async void TapOperations()
        {
            Swipped = true;
            Rollup = false;
            RollDice = "roll3.png";
            
            GlowMarkedPins(playInfos[StarterValue].Id, Swipped, isBot);

            if (isBot)
            {
                BoardUpdate(playInfos[StarterValue].Score, StarterValue);

                await Task.Delay(1700);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    SetPointBackground(playInfos[1].Id);
                    await DependencyService.Get<IMessage>().ShortAlert("Bot's Turn!!");
                });
                await Task.Delay(1000);
                DoForBot();
            }
            else
            {
                BoardUpdate(playInfos[StarterValue].Score, playInfos[StarterValue].Id);

                StarterValue++;
                if (StarterValue > nos - 1)
                {
                    StarterValue = 0;
                }
                await Task.Delay(2000);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    SetPointBackground(playInfos[StarterValue].Id);
                    EnableButton = true;
                    if (nos > toastcount)
                    {
                        toastcount++;
                        await DependencyService.Get<IMessage>().ShortAlert(playInfos[StarterValue].PlayerName + "'s" + " Turn");
                    }
                    return;
                });
            }
            Swipped = false;
        }

        private async void InfoBtn_Tapped(object sender, EventArgs e)
        {
            await _navigation.PushPopupAsync(new GameRules(_navigation));
        }
        private async void MuteBtn_Clicked(object sender, EventArgs e)
        {
            if (!IsMute)
            {
                IsMute = true;
                Mute = "mute.png";
                MuteSounds(IsMute);
                await DependencyService.Get<IMessage>().ShortAlert("Muted");
            }
            else
            {
                IsMute = false;
                Mute = "unmute.png";
                MuteSounds(IsMute);
                await DependencyService.Get<IMessage>().ShortAlert("Un-Muted");
            }
        }
        #endregion

        #region HelperMethods
      
        void SetPointBackground(int id)
        {
            if(!isBot)
            {
                if (id == 0)
                {
                    Player1Background = Color.FromHex("#4e4b4b");
                    Player1TextColor = Color.FromHex("#f3f2f2");

                    Player2Background = Color.Transparent;
                    Player3Background = Color.Transparent;
                    Player4Background = Color.Transparent;

                    Player2TextColor = Color.Black;
                    Player3TextColor = Color.Black;
                    Player4TextColor = Color.Black;
                }
                if (id == 1)
                {
                    Player2Background = Color.FromHex("#4e4b4b");
                    Player2TextColor = Color.FromHex("#f3f2f2");

                    Player1Background = Color.Transparent;
                    Player3Background = Color.Transparent;
                    Player4Background = Color.Transparent;

                    Player1TextColor = Color.Black;
                    Player3TextColor = Color.Black;
                    Player4TextColor = Color.Black;
                }
                if (id == 2)
                {
                    Player3Background = Color.FromHex("#4e4b4b");
                    Player3TextColor = Color.FromHex("#f3f2f2");

                    Player2Background = Color.Transparent;
                    Player1Background = Color.Transparent;
                    Player4Background = Color.Transparent;

                    Player1TextColor = Color.Black;
                    Player2TextColor = Color.Black;
                    Player4TextColor = Color.Black;
                }
                if (id == 3)
                {
                    Player4Background = Color.FromHex("#4e4b4b");
                    Player4TextColor = Color.FromHex("#f3f2f2");

                    Player2Background = Color.Transparent;
                    Player3Background = Color.Transparent;
                    Player1Background = Color.Transparent;

                    Player1TextColor = Color.Black;
                    Player2TextColor = Color.Black;
                    Player3TextColor = Color.Black;
                }
            }
            else
            {
                if (id == 0 || id==1 || id==2 || id==3)
                {
                    Player1Background = Color.FromHex("#4e4b4b");
                    Player1TextColor = Color.FromHex("#f3f2f2");

                    Player2Background = Color.Transparent;
                    Player2TextColor = Color.Black;

                }
                if (id == 101)
                {
                    Player2Background = Color.FromHex("#4e4b4b");
                    Player2TextColor = Color.FromHex("#f3f2f2");

                    Player1Background = Color.Transparent;
                    Player1TextColor = Color.Black;
                }
            }
          
          
        }
        public void GlowMarkedPins(int Id, bool animate,bool isBot)
        {
            if (!animate)
            {
                if(!isBot)
                {
                    CheckOverlapping(Id, animate);
                    if (Id == 0)
                    {
                        AnimationCheck1 = true;
                        Pin1 = "runner.gif";
                    }
                    else if (Id == 1)
                    {
                        AnimationCheck2 = true;
                        Pin2 = "runner.gif";
                    }
                    else if (Id == 2)
                    {
                        AnimationCheck3 = true;
                        Pin3 = "runner.gif";
                    }
                    else if (Id == 3)
                    {
                        AnimationCheck4 = true;
                        Pin4 = "runner.gif";
                    }                  
                }
                else
                {
                    CheckOverlapping(Id, animate);
                    AnimationCheck1 = true;
                    Pin1 = "runner.gif";
                }
                
            }
            else
            {
                if (!isBot)
                {
                    CheckOverlapping(Id, animate);
                    if (Id == 0)
                    {
                        Pin1 = "redpin.png";
                        AnimationCheck1 = false;
                    }
                    else if (Id == 1)
                    {
                        Pin2 = "bluepin.png";
                        AnimationCheck2 = false;
                    }
                    else if (Id == 2)
                    {
                        Pin3 = "yellowpin.png";
                        AnimationCheck3 = false;
                    }
                    else if (Id == 3)
                    {
                        Pin4 = "greenpin.png";
                        AnimationCheck4 = false;
                    }                    
                }
                else
                {
                    CheckOverlapping(Id, animate);
                    Pin1 = playInfos[0].Pin;
                    AnimationCheck1 = false;
                }
                    
            }

        }
        public void CheckOverlapping(int id, bool visible)
        {
            if (!visible)
            {               
                    var duplicates = playInfos.Where(x => x.Id != id && x.Score == Globali).ToList();


                if (duplicates.Count() > 0)
                {
                    var pin1=duplicates.Where(x => x.Id == 0).FirstOrDefault();
                    if(pin1!=null)
                        IsPin1 = false;

                    var pin2 = duplicates.Where(x => x.Id == 1).FirstOrDefault();
                    if (pin2 != null)
                        IsPin2 = false;

                    var pin3 = duplicates.Where(x => x.Id == 2).FirstOrDefault();
                    if (pin3 != null)
                        IsPin3 = false;

                    var pin4 = duplicates.Where(x => x.Id == 3).FirstOrDefault();
                    if (pin4 != null)
                        IsPin4 = false;  
                    

                    if(isBot)
                    {
                        var pinb = duplicates.Where(x => x.Id == 101).FirstOrDefault();
                        if (pinb != null)
                            IsPin2 = false;
                    }
                }
            }
            else
            {
                var gameovers = playInfos.Where(x => x.Score != 100 && x.PlayerOn==true).ToList();
                if (gameovers.Count() > 0)
                {
                    var pin1 = gameovers.Where(x => x.Id == 0).FirstOrDefault();
                    if (pin1 != null)
                        IsPin1 = true;

                    var pin2 = gameovers.Where(x => x.Id == 1).FirstOrDefault();
                    if (pin2 != null)
                        IsPin2 = true;

                    var pin3 = gameovers.Where(x => x.Id == 2).FirstOrDefault();
                    if (pin3 != null)
                        IsPin3 = true;

                    var pin4 = gameovers.Where(x => x.Id == 3).FirstOrDefault();
                    if (pin4 != null)
                        IsPin4 = true;

                    if (isBot)
                    {
                        var pinb = gameovers.Where(x => x.Id == 101 && x.PlayerOn == true).FirstOrDefault();
                        if (pinb != null)
                            IsPin2 = true;
                    }
                }               
            }
        }

        public void BoardUpdate(int GamePoints, int playernumber)
        {
            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    FindCoordinate(GamePoints, playernumber);

                });
                move.Play();
                Task.Delay(1000).Wait();
                GameLogic(GamePoints, playernumber);
            });
        }
        public void FindCoordinate(int points, int playernumber)
        {
            for (int i = 0; i < boardInfos.Count; i++)
            {
                if (points == boardInfos[i].Value)
                {
                    if (playernumber == 0)
                    {
                        ColumnNumber1 = boardInfos[i].Col;
                        RowNumber1 = boardInfos[i].Row;
                        Player1Points = points.ToString();
                        break;
                    }
                    else if (playernumber == 1)
                    {
                        ColumnNumber2 = boardInfos[i].Col;
                        RowNumber2 = boardInfos[i].Row;
                        Player2Points = points.ToString();
                        break;
                    }
                    else if (playernumber == 2)
                    {
                        ColumnNumber3 = boardInfos[i].Col;
                        RowNumber3 = boardInfos[i].Row;
                        Player3Points = points.ToString();
                        break;
                    }
                    else
                    {
                        ColumnNumber4 = boardInfos[i].Col;
                        RowNumber4 = boardInfos[i].Row;
                        Player4Points = points.ToString();
                        break;
                    }

                }
            }
        }


        public void AnimateActionCoordinate(int points, int playernumber)
        {
            for (int i = 0; i < boardInfos.Count; i++)
            {
                if (points == boardInfos[i].Value)
                {
                    if (playernumber == 0)
                    {
                        
                        Task.Run(async () =>
                        {
                            EventItem = "asnake.gif";
                            IsEventItem = true;
                            await Task.Delay(1500);
                            IsEventItem = false;
                            SnakeBiteEffect();
                            if(points==22)
                            {
                                await drag1.TranslateTo(10, 60, 300, Easing.Linear);
                                await drag1.TranslateTo(20, 120, 300, Easing.Linear);
                                await drag1.TranslateTo(0, 180, 300, Easing.Linear);
                                await drag1.TranslateTo(10, 240, 300, Easing.Linear);
                                await drag1.TranslateTo(20, 300, 300, Easing.Linear);
                                await drag1.TranslateTo(0, 0, 0, Easing.Linear);
                            }
                            else
                            {
                                await drag1.TranslateTo(10, 30, 300, Easing.Linear);
                                await drag1.TranslateTo(20, 60, 300, Easing.Linear);
                                await drag1.TranslateTo(0, 95, 300, Easing.Linear);
                                await drag1.TranslateTo(10, 120, 300, Easing.Linear);
                                await drag1.TranslateTo(20, 130, 300, Easing.Linear);
                                await drag1.TranslateTo(0, 0, 0, Easing.Linear);
                            }                            
                            
                            ColumnNumber1 = boardInfos[i].Col;
                            RowNumber1 = boardInfos[i].Row;
                            Player1Points = points.ToString();
                        });
                        break;
                    }
                    else if (playernumber == 1)
                    {
                        Task.Run(async () =>
                        {
                            EventItem = "asnake.gif";
                            IsEventItem = true;
                            await Task.Delay(1500);
                            IsEventItem = false;
                            SnakeBiteEffect();
                            if (points == 22)
                            {
                                await drag2.TranslateTo(10, 60, 300, Easing.Linear);
                                await drag2.TranslateTo(20, 120, 300, Easing.Linear);
                                await drag2.TranslateTo(0, 180, 300, Easing.Linear);
                                await drag2.TranslateTo(10, 240, 300, Easing.Linear);
                                await drag2.TranslateTo(20, 300, 300, Easing.Linear);
                                await drag2.TranslateTo(0, 0, 0, Easing.Linear);
                            }
                            else
                            {
                                await drag2.TranslateTo(10, 30, 300, Easing.Linear);
                                await drag2.TranslateTo(20, 60, 300, Easing.Linear);
                                await drag2.TranslateTo(0, 95, 300, Easing.Linear);
                                await drag2.TranslateTo(10, 120, 300, Easing.Linear);
                                await drag2.TranslateTo(20, 130, 300, Easing.Linear);
                                await drag2.TranslateTo(0, 0, 0, Easing.Linear);
                            }

                            ColumnNumber2 = boardInfos[i].Col;
                        RowNumber2 = boardInfos[i].Row;
                        Player2Points = points.ToString();
                        });
                        break;
                    }
                    else if (playernumber == 2)
                    {
                        Task.Run(async () =>
                        {
                            EventItem = "asnake.gif";
                            IsEventItem = true;
                            await Task.Delay(1500);
                            IsEventItem = false;
                            SnakeBiteEffect();
                            if (points == 22)
                            {
                                await drag3.TranslateTo(10, 60, 300, Easing.Linear);
                                await drag3.TranslateTo(20, 120, 300, Easing.Linear);
                                await drag3.TranslateTo(0, 180, 300, Easing.Linear);
                                await drag3.TranslateTo(10, 240, 300, Easing.Linear);
                                await drag3.TranslateTo(20, 300, 300, Easing.Linear);
                                await drag3.TranslateTo(0, 0, 0, Easing.Linear);
                            }
                            else
                            {
                                await drag3.TranslateTo(10, 30, 300, Easing.Linear);
                                await drag3.TranslateTo(20, 60, 300, Easing.Linear);
                                await drag3.TranslateTo(0, 95, 300, Easing.Linear);
                                await drag3.TranslateTo(10, 120, 300, Easing.Linear);
                                await drag3.TranslateTo(20, 130, 300, Easing.Linear);
                                await drag3.TranslateTo(0, 0, 0, Easing.Linear);
                            }
                            ColumnNumber3 = boardInfos[i].Col;
                        RowNumber3 = boardInfos[i].Row;
                        Player3Points = points.ToString();
                    });
                        break;
                    }
                    else
                    {
                        Task.Run(async () =>
                        {
                            EventItem = "asnake.gif";
                            IsEventItem = true;
                            await Task.Delay(1500);
                            IsEventItem = false;
                            SnakeBiteEffect();
                            if (points == 22)
                            {
                                await drag4.TranslateTo(10, 60, 300, Easing.Linear);
                                await drag4.TranslateTo(20, 120, 300, Easing.Linear);
                                await drag4.TranslateTo(0, 180, 300, Easing.Linear);
                                await drag4.TranslateTo(10, 240, 300, Easing.Linear);
                                await drag4.TranslateTo(20, 300, 300, Easing.Linear);
                                await drag4.TranslateTo(0, 0, 0, Easing.Linear);
                            }
                            else
                            {
                                await drag4.TranslateTo(10, 30, 300, Easing.Linear);
                                await drag4.TranslateTo(20, 60, 300, Easing.Linear);
                                await drag4.TranslateTo(0, 95, 300, Easing.Linear);
                                await drag4.TranslateTo(10, 120, 300, Easing.Linear);
                                await drag4.TranslateTo(20, 130, 300, Easing.Linear);
                                await drag4.TranslateTo(0, 0, 0, Easing.Linear);
                            }
                            ColumnNumber4 = boardInfos[i].Col;
                        RowNumber4 = boardInfos[i].Row;
                        Player4Points = points.ToString();
                        });
                        break;
                    }

                }
            }
        }

     
        public void GameLogic(int points, int playernunber)
        {
            var gamepoint = 1;
            switch (points)
            {

                case 98:
                    gamepoint = 22;
                    AddScore(gamepoint, playernunber);
                    AnimateActionCoordinate(gamepoint, playernunber);
                    break;

                case 52:
                    gamepoint = 11;
                    AddScore(gamepoint, playernunber);
                    AnimateActionCoordinate(gamepoint, playernunber);
                    break;

                case 75:
                    gamepoint = 34;
                    AddScore(gamepoint, playernunber);
                    AnimateActionCoordinate(gamepoint, playernunber);
                    break;

                case 36:
                    gamepoint = 6;
                    AddScore(gamepoint, playernunber);
                    AnimateActionCoordinate(gamepoint, playernunber);
                    break;


                case 74:
                    gamepoint = 87;
                    AddScore(gamepoint, playernunber);
                    FindCoordinate(gamepoint, playernunber);
                    ClimbLadderEffect();
                    break;

                case 45:
                    gamepoint = 65;
                    AddScore(gamepoint, playernunber);
                    FindCoordinate(gamepoint, playernunber);
                    ClimbLadderEffect();
                    break;

                case 19:
                    gamepoint = 39;
                    AddScore(gamepoint, playernunber);
                    FindCoordinate(gamepoint, playernunber);
                    ClimbLadderEffect();
                    break;

                case 69:
                    gamepoint = 92;
                    AddScore(gamepoint, playernunber);
                    FindCoordinate(gamepoint, playernunber);
                    ClimbLadderEffect();
                    break;

                case 100:
                    if (playernunber == 1 && isBot)
                    {
                        LostEffect();
                    }
                    else
                    {                        
                        WinnerEffect();
                    }
                    break;
            }
        }

        public void AddScore(int points, int playernumber)
        {
            if (!isBot)
            {
                if (playernumber == 0)
                {

                    playInfos.Where(x => x.Id == 0).ForEach(y => { y.Score = points; });
                    Player1Points = points.ToString();
                }
                else if (playernumber == 1)
                {
                    playInfos.Where(x => x.Id == 1).ForEach(y => { y.Score = points; });
                    Player2Points = points.ToString();
                }
                else if (playernumber == 2)
                {
                    playInfos.Where(x => x.Id == 2).ForEach(y => { y.Score = points; });
                    Player3Points = points.ToString();
                }
                else
                {
                    playInfos.Where(x => x.Id == 3).ForEach(y => { y.Score = points; });
                    Player4Points = points.ToString();
                }
            }
            else
            {
                if (playernumber == 0)
                {

                    playInfos[0].Score=points;
                    Player1Points = points.ToString();
                }
                else if (playernumber == 1)
                {
                    playInfos[1].Score = points;
                    Player2Points = points.ToString();
                }
            }
        }

        #endregion

        #region Effects

        void ShowUserPrompt()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                PageImage = "blackbg.png";
                DiceNumber = "prompt.png";
                PlayGif = false;
                IsDiceAvailable = true;
                Application.Current.Properties["ID"] = "999";
                await Application.Current.SavePropertiesAsync();
                await Task.Delay(1000);
                IsDiceAvailable = false;
                PageImage = "bgsl2.jpg";
                await Task.Delay(500);
                IsCheckRule = true;                
            });
        }
        public async void PlayStartEffects()
        {
              Device.BeginInvokeOnMainThread(async () =>
             {                
                IsDiceAvailable = true;           
                await Task.Delay(500);
                 gamestart.Volume = 0.1;
                 gamestart.Play();
                 await Task.Delay(2000);
                 IsDiceAvailable = false;
                 PageImage = "bgsl2.jpg";
                 await Task.Delay(500);
               
                 if (!isBot)
                 await DependencyService.Get<IMessage>().ShortAlert(playInfos[StarterValue].PlayerName + "'s" + " Turn");
                 else
                 await DependencyService.Get<IMessage>().ShortAlert(playInfos[0].PlayerName + "'s" + " Turn");

             });
        }
        public void StartEffect()
        {
            Task.Delay(1300);
            playstart.Play();
        }     
        public void BotStartEffect()
        {
            botplaystart.Play();
        }
        public void SnakeBiteEffect()
        {            
            hiss.Play();
        }
        public void ZeroEffect()
        {
            Task.Delay(1300);
            sad.Play();
        }
        public void SixerEffect()
        {
            six.Play();
        }
        public void ClimbLadderEffect()
        {         
            climb.Play();
            Thread.Sleep(1500);
            climb.Stop();
        }
        public void WinnerEffect()
        {
            Device.BeginInvokeOnMainThread(async() =>
            {
                cheer.Play();
                RollDice = "roll3.png";              
                await Task.Delay(2000);
            });
        }
        public void LostEffect()
        {            
            lost.Play();
        }
       public void MuteSounds(bool mute)
        {
            if(mute)
            {                
                player.Volume = 0;
                playstart.Volume = 0;
                botplaystart.Volume = 0;
                move.Volume = 0;
                hiss.Volume = 0;
                lost.Volume = 0;
                cheer.Volume = 0;
                climb.Volume = 0;
                six.Volume = 0;
            }
            else
            {
                player.Volume = 0.5;
                playstart.Volume = 0.5;
                botplaystart.Volume = 0.5;
                move.Volume = 0.5;
                hiss.Volume = 0.5;
                lost.Volume = 0.5;
                cheer.Volume = 0.5;
                climb.Volume = 0.5;
                six.Volume = 0.5;
            }
        }
        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("SnakeAndLadder." + filename);

            return stream;
        }
        #endregion

        #region Dice Methods

        public void ShowAfterLoad(bool alien,PlayInfo counInfos)
        {
                DiceValue = 0;
                Random rand = new Random();
                var currentroll = rand.Next(1, 7);

               bool playable = playInfos.Any(x => x.PlayerOn == true);

                if (!alien && !counInfos.PlayerOn)
                {
                    if(isBot && playable && counInfos.Moves >= 8)
                    {
                       DiceValue = 6;
                    }
                    else if(playable && counInfos.Moves >= 13)
                    {
                       DiceValue = 6;
                    }
                    else
                    {
                       DiceValue = currentroll;
                    }
                }
                else
                {
                    DiceValue = currentroll;
                }
                
        }
        void Roll(bool alien)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                while (!Rollup)
                {
                    RollDice = "roll3.png";
                    await image1.RotateTo(90, 500, Easing.Linear);
                    RollDice = "roll4.png";
                    await image1.RotateTo(180, 500, Easing.Linear);
                    RollDice = "roll3.png";
                    await image1.RotateTo(90, 500, Easing.Linear);
                    RollDice = "roll4.png";
                    await image1.RotateTo(180, 500, Easing.Linear);
                    RollDice = "roll3.png";
                    await image1.RotateTo(0, 500, Easing.Linear);
                }
                RollDice = Images[DiceValue];

                if( DiceValue == 6  && playInfos[StarterValue].Score == 1  && SixCounts.Count() == 0 && !alien)
                {
                    IsEventItem = true;
                    EventItem = "yay.gif";
                    StartEffect();
                    await Task.Delay(2700);
                    IsEventItem = false;
                }                
                else if (DiceValue == 6 && SixCount==3 && !alien || DiceValue == 6 && playInfos[StarterValue].Score >= 94 && !alien)
                {
                    IsEventItem = true;
                    EventItem = "ohno.gif";
                    ZeroEffect();
                    await Task.Delay(2000);
                    IsEventItem = false;
                }
                else if (DiceValue == 6 && playInfos[StarterValue].Score < 94 && playInfos[StarterValue].Score!=0 && !alien)
                {
                    SixerEffect();
                    IsEventItem = true;
                    EventItem = "sixer.gif";
                    await Task.Delay(1800);
                    IsEventItem = false;
                }
                await Task.Delay(500);
                if(DiceValue==6)
                {
                    RollDice = "roll3.png";
                }
                if (isBot && BotEmpty)
                {
                    RollDice = "roll3.png";
                    BotEmpty = false;
                }

            });
        }
        public bool CheckDiceValid(int points, int value)
        {
            bool valid = false;
            int counter = 100 - points;
            if (counter >= value)
                valid = false;
            else
                valid = true;

            return valid;
        }
      
        #endregion
       
    }
}