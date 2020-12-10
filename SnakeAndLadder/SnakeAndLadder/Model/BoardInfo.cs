using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SnakeAndLadder.Model
{
    public class BoardInfo
    {
        public string Row { get; set; }
        public string Col { get; set; }
        public int Value { get; set; }

        public Label Instance{ get; set; }
        
    }
}
