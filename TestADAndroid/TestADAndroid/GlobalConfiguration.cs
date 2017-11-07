using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TestADAndroid
{
    public static class GlobalConfiguration
    {
        public const string ClientId = "26a04582-6e41-4277-8923-1e28253d5054";
        public const string Authority = "https://login.windows.net/common";
        public const string RedirectUri = "https://www.bing.com/";
        public const string PrometheeAppId = "https://www.bing.com";
    }
}