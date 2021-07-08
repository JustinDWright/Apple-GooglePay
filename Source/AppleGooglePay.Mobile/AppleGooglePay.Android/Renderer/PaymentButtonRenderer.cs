using Android.Content;
using AppleGooglePay.Droid.Renderer;
using AppleGooglePay.Mobile.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PaymentButton), typeof(PaymentButtonRenderer))]
namespace AppleGooglePay.Droid.Renderer
{
    public class PaymentButtonRenderer : ViewRenderer
    {
        public PaymentButtonRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    SetNativeControl(Inflate(Context, Resource.Layout.buy_with_googlepay_button, null));
                }

                Control.Click += Control_Click;
            }
        }

        private void Control_Click(object sender, System.EventArgs e) => ((IButtonController)Element).SendClicked();
    }
}