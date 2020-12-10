using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SnakeAndLadder.Droid;
using SnakeAndLadder.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace SnakeAndLadder.Droid
{
    public class MessageAndroid : IMessage
    {        
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public Task ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();

            return Task.Delay(0);
        }
    }
}