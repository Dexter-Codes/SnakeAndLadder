using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SnakeAndLadder.Droid;
using SnakeAndLadder.Interface;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(LatestVersionCheck))]
namespace SnakeAndLadder.Droid
{
    public class LatestVersionCheck:ILatest
    {
        string _packageName => global::Android.App.Application.Context.PackageName;
        string _versionName => global::Android.App.Application.Context.PackageManager.GetPackageInfo(global::Android.App.Application.Context.PackageName, 0).VersionName;

        public string InstalledVersionNumber
        {
            get => _versionName;
        }

        public async Task<bool> IsUsingLatestVersion()
        {
            bool isLatest = false;
            var latestVersion = string.Empty;

            try
            {
                latestVersion = await GetLatestVersionNumber();

                isLatest =CompareVersionNumbers(Convert.ToDouble(_versionName), Convert.ToDouble(latestVersion));

            }
            catch (Exception e)
            {
                // throw new LatestVersionException($"Error comparing current app version number with latest. Version name={_versionName} and lastest version={latestVersion} .", e);
            }
            return isLatest;
        }
        public bool CompareVersionNumbers(double old, double current)
        {
            if (old < current)
                return true;
            else
                return false;
        }
        public async Task<string> GetLatestVersionNumber()
        {
            return await GetLatestVersionNumber(_packageName);
        }

        public async Task<string> GetLatestVersionNumber(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }

            var version = string.Empty;
            var url = $"https://play.google.com/store/apps/details?id={appName}&hl=en";


            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    using (var handler = new HttpClientHandler())
                    {
                        using (var client = new HttpClient(handler))
                        {
                            using (var responseMsg = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                            {
                                if (!responseMsg.IsSuccessStatusCode)
                                {
                                   // throw new LatestVersionException($"Error connecting to the Play Store. Url={url}.");
                                }

                                try
                                {
                                    var content = responseMsg.Content == null ? null : await responseMsg.Content.ReadAsStringAsync();

                                    var versionMatch = Regex.Match(content, "<div[^>]*>Current Version</div><span[^>]*><div[^>]*><span[^>]*>(.*?)<").Groups[1];

                                    if (versionMatch.Success)
                                    {
                                        version = versionMatch.Value.Trim();
                                    }
                                }
                                catch (Exception e)
                                {
                                  //  throw new LatestVersionException($"Error parsing content from the Play Store. Url={url}.", e);
                                }
                            }
                        }
                    }
                }
            }
         
            return version;
        }

        public Task OpenAppInStore()
        {
            return OpenAppInStore(_packageName);
        }

        public Task OpenAppInStore(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }

            try
            {
                var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse($"market://details?id={appName}"));
                intent.SetPackage("com.android.vending");
                intent.SetFlags(ActivityFlags.NewTask);
                global::Android.App.Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse($"https://play.google.com/store/apps/details?id={appName}"));
                global::Android.App.Application.Context.StartActivity(intent);
            }

            return Task.FromResult(true);
        }
    }
}