using AppleGooglePay.iOS.Renderers;
using AppleGooglePay.Mobile.Controls;
using PassKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PaymentButton), typeof(PaymentButtonRenderer))]
namespace AppleGooglePay.iOS.Renderers
{
	public class PaymentButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement == null)
			{
				var button = new PKPaymentButton(PKPaymentButtonType.Buy, PKPaymentButtonStyle.Black);
				SetNativeControl(button);

				button.PrimaryActionTriggered += Button_PrimaryActionTriggered;
			}
		}

		private void Button_PrimaryActionTriggered(object sender, System.EventArgs e) => ( (IButtonController)Element )?.SendClicked();

		protected override void Dispose(bool disposing)
		{
			if (Control != null)
			{
				Control.PrimaryActionTriggered -= Button_PrimaryActionTriggered;
			}

			base.Dispose(disposing);
		}
	}
}