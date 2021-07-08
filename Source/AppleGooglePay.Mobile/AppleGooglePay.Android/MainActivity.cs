
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AppleGooglePay.Droid.Payment;
using AppleGooglePay.Mobile.Payment;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace AppleGooglePay.Mobile.Droid
{
    [Activity(Label = "AppleGooglePay", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = AppleGooglePay.Droid.Resource.Layout.Tabbar;
            ToolbarResource = AppleGooglePay.Droid.Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);

            var app = new App();
            var services = new ServiceCollection();

            services.AddScoped<IPaymentService, PaymentService>();

            app.ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            app.Start(provider);

            LoadApplication(app);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 999 && resultCode == Result.Ok)
                MessagingCenter.Send(this, "AuthorizationComplete", data);
        }
    }
}