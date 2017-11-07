using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;

namespace TestADAndroid
{
    [Activity(Label = "TestADAndroid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        TextView _statusTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.connectionButton).Click += connectionButton_Click;
            _statusTextView = FindViewById<TextView>(Resource.Id.statusTextView);
        }

        private async void connectionButton_Click(object sender, System.EventArgs e)
        {
            await LoginAsync();
        }

        public async Task<bool> LoginAsync()
        {
            return (await GetTokenAsync()) != null;
        }

        private async Task<AuthenticationResult> GetTokenAsync()
        {
            try
            {
                var result = await AuthenticateAsync(GlobalConfiguration.Authority, GlobalConfiguration.PrometheeAppId, GlobalConfiguration.ClientId, GlobalConfiguration.RedirectUri);
                if (result != null)
                {
                    _statusTextView.Text = result.UserInfo.ToString();
                    return result;
                }
            }
            catch (Exception) { }

            return null;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string authority, string resource, string clientId, string returnUri)
        {
            var authContext = new AuthenticationContext(authority);
            if (authContext.TokenCache.ReadItems().Any())
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);

            var uri = new Uri(returnUri);
            var activity = (Activity)this;
            var authResult = await authContext.AcquireTokenAsync(resource, clientId, uri, new PlatformParameters(activity));
            return authResult;
        }
    }
}

