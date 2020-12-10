using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadder.Interface
{
    public interface IMessage
    {
        void LongAlert(string message);
        Task ShortAlert(string message);
        
    }
}
