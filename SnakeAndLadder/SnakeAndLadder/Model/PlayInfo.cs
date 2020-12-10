using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeAndLadder.Model
{
    public class PlayInfo
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string PlayerName { get; set; }
        public bool PlayerOn { get; set; }

        public string Colour { get; set; }

        public string Pin { get; set; }

        public int Moves { get; set; }

    }
}
