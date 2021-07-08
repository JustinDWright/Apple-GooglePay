using System;
using Android.Content;
using Android.Gms.Tasks;
using Android.Gms.Wallet;
using AndroidX.AppCompat.App;
using AppleGooglePay.Mobile.Configuration;
using AppleGooglePay.Mobile.Droid;
using AppleGooglePay.Mobile.Payment;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppleGooglePay.Droid.Payment
{
    public class PaymentService : AppCompatActivity, IPaymentService, IOnCompleteListener
    {
        protected IApiConfiguration ApiConfiguration { get; }
        protected IMessagingCenter MessageCenter { get; }

        public event EventHandler CanMakePaymentsUpdated;
        public event EventHandler<string> AuthorizationComplete;

        protected PaymentsClient PaymentsClient { get; set; }

        public PaymentService(IApiConfiguration apiConfiguration, IMessagingCenter messageCenter)
        {
            ApiConfiguration = apiConfiguration;
            MessageCenter = messageCenter;

            PaymentsClient = WalletClass.GetPaymentsClient(
                 Android.App.Application.Context,
                 new WalletClass.WalletOptions.Builder()
#if DEBUG
                         .SetEnvironment(WalletConstants.EnvironmentTest)
#else
					 .SetEnvironment(WalletConstants.EnvironmentProduction)
#endif
                         .Build());

            var readyToPayRequest = IsReadyToPayRequest.FromJson(GetReadyToPayRequest());
            var task = PaymentsClient.IsReadyToPay(readyToPayRequest);

            task.AddOnCompleteListener(this);

            MessagingCenter.Subscribe<MainActivity, Intent>(this, "AuthorizationComplete", (sender, intent) =>
            {
                var paymentData = PaymentData.GetFromIntent(intent);
                string paymentInfo = paymentData.ToJson();

                if (paymentInfo == null)
                {
                    return;
                }

                var paymentMethodData = (JObject)JsonConvert.DeserializeObject(paymentInfo);
                string tokenData = paymentMethodData.SelectToken("paymentMethodData.tokenizationData.token").ToString();
                var token = JsonConvert.DeserializeObject<GooglePaymentResponseToken>(tokenData);

                AuthorizationComplete.Invoke(this, token.Id);
            });
        }

        public bool CanMakePayments { get; set; }

        public void AuthorizePayment(decimal total) => AutoResolveHelper.ResolveTask(
                    PaymentsClient.LoadPaymentData(CreatePaymentDataRequest(total)),
                    Platform.CurrentActivity,
                    999);

        public void OnComplete(Task completeTask)
        {
            CanMakePayments = completeTask.IsComplete;
            CanMakePaymentsUpdated?.Invoke(this, null);
        }

        public string GetReadyToPayRequest() => JsonConvert.SerializeObject(GetBaseRequest());

        protected GooglePaymentRequest GetBaseRequest() =>
            new GooglePaymentRequest
            {
                ApiVersion = 2,
                ApiVersionMinor = 0,
                MerchantInfo = new MerchantInfo { MerchantName = "AppleGooglePay" },
                AllowedPaymentMethods = new[]
                {
                    new PaymentMethod
                    {
                        Type = "CARD",
                        Parameters = new PaymentParameters
                        {
                            AllowedAuthMethods = new[] { "PAN_ONLY", "CRYPTOGRAM_3DS" },
                            AllowedCardNetworks = new[] { "AMEX", "DISCOVER", "MASTERCARD", "VISA" }
                        },
                        TokenizationSpecification = new TokenizationSpecification
                        {
                            Type = "PAYMENT_GATEWAY",
                            Parameters = new TokenizationSpecificationParameters
                            {
                                Gateway = "stripe",
                                StripeVersion = "2020-03-02",
                                StripeKey = ApiConfiguration.StripePublishableKey
                            }
                        }
                    }
                }
            };

        protected PaymentDataRequest CreatePaymentDataRequest(decimal total)
        {
            var request = GetBaseRequest();

            request.TransactionInfo = new TransactionInfo
            {
                TotalPrice = total.ToString("F"),
                TotalPriceStatus = "FINAL",
                CurrencyCode = "USD"
            };

            return PaymentDataRequest.FromJson(JsonConvert.SerializeObject(request));
        }
    }
}