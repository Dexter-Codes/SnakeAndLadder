using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnakeAndLadder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameRules : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        INavigation _navigation;
        public GameRules(INavigation navigation)
        {
            InitializeComponent();
            _navigation = navigation;
            AddRulesInfo();
        }      

        public string _textDetails;
        public string TextDetails
        {
            get => _textDetails;
            set { _textDetails = value; OnPropertyChanged(); }
        }
        void AddRulesInfo()
        {
            TextDetails = "1. The Game starts when you roll the dice for 6 \n \n 2. On the first roll of 6, you won't get an additional roll \n \n" +
               "3. On rolling the dice for 3 subsequent sixes, all the 6's rolled become null and only the latest rolled value will be taken into account \n \n" +
               "4. After rolling the dice, to move to the next point, you need to tap on the flashy guy \n \n 5. Once your score reaches 94, then the rolled 6's would not be counted ";
        }

        protected override bool OnBackButtonPressed()
        {
            _navigation.PopPopupAsync();
            return false;
        }
        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            _navigation.PopPopupAsync();
        }
    }
}