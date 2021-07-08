using System;
using Foundation;
using AppleGooglePay.iOS.Payment;
using AppleGooglePay.Mobile.Configuration;
using AppleGooglePay.Mobile.Payment;
using PassKit;
using Stripe.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(PaymentService))]
namespace AppleGooglePay.iOS.Payment
{
	public class PaymentService : PKPaymentAuthorizationControllerDelegate, IPaymentService
	{
		public event EventHandler<string> AuthorizationComplete;
		public event EventHandler CanMakePaymentsUpdated;  // handled behind the scenes in iOS

		protected IApiConfiguration ApiConfiguration { get; }

		public PaymentService(IApiConfiguration apiConfiguration)
		{
			ApiConfiguration = apiConfiguration;

			var client = new ApiClient(apiConfiguration.StripePublishableKey);
			client.Configuration.AppleMerchantIdentifier = apiConfiguration.MerchantId;
			ApiClient.SharedClient.PublishableKey = apiConfiguration.StripePublishableKey;
		}

		public bool CanMakePayments => StripeSdk.DeviceSupportsApplePay;

		public void AuthorizePayment(decimal total)
		{
			var request = new PKPaymentRequest
			{
				PaymentSummaryItems = new[] { new PKPaymentSummaryItem { Label = "AppleGooglePay", Amount = new NSDecimalNumber(total), Type = PKPaymentSummaryItemType.Final } },
				CountryCode = "US",
				CurrencyCode = "USD",
				MerchantIdentifier = ApiConfiguration.MerchantId,
				MerchantCapabilities = PKMerchantCapability.ThreeDS,
				SupportedNetworks = new[] { PKPaymentNetwork.Amex, PKPaymentNetwork.MasterCard, PKPaymentNetwork.Visa }
			};

			var authorization = new PKPaymentAuthorizationController(request)
			{
				Delegate = (IPKPaymentAuthorizationControllerDelegate)Self
			};

			authorization.Present(null);
		}

		public override void DidAuthorizePayment(PKPaymentAuthorizationController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
		{
			ApiClient.SharedClient.CreateToken(payment, TokenComplete);

			completion(PKPaymentAuthorizationStatus.Success);
		}

		protected void TokenComplete(Token token, NSError arg1) => AuthorizationComplete.Invoke(this, token.TokenId);

		public override void DidFinish(PKPaymentAuthorizationController controller) => controller.Dismiss(null);
	}
}