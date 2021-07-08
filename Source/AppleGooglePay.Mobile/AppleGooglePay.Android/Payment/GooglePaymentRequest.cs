using Newtonsoft.Json;

namespace AppleGooglePay.Droid.Payment
{
    public class GooglePaymentRequest
    {
        [JsonProperty("apiVersion")]
        public int ApiVersion { get; set; }
        [JsonProperty("apiVersionMinor")]
        public int ApiVersionMinor { get; set; }
        [JsonProperty("merchantInfo")]
        public MerchantInfo MerchantInfo { get; set; }
        [JsonProperty("allowedPaymentMethods")]
        public PaymentMethod[] AllowedPaymentMethods { get; set; }
        [JsonProperty("transactionInfo")]
        public TransactionInfo TransactionInfo { get; set; }
    }

    public class MerchantInfo
    {
        [JsonProperty("merchantName")]
        public string MerchantName { get; set; }
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }
    }

    public class TransactionInfo
    {
        [JsonProperty("totalPriceStatus")]
        public string TotalPriceStatus { get; set; }
        [JsonProperty("totalPrice")]
        public string TotalPrice { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }

    public class PaymentMethod
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("parameters")]
        public PaymentParameters Parameters { get; set; }
        [JsonProperty("tokenizationSpecification")]
        public TokenizationSpecification TokenizationSpecification { get; set; }
    }

    public class PaymentParameters
    {
        [JsonProperty("allowedAuthMethods")]
        public string[] AllowedAuthMethods { get; set; }
        [JsonProperty("allowedCardNetworks")]
        public string[] AllowedCardNetworks { get; set; }
    }

    public class TokenizationSpecification
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("parameters")]
        public TokenizationSpecificationParameters Parameters { get; set; }
    }

    public class TokenizationSpecificationParameters
    {
        [JsonProperty("gateway")]
        public string Gateway { get; set; }
        [JsonProperty("stripe:version")]
        public string StripeVersion { get; set; }
        [JsonProperty("stripe:publishableKey")]
        public string StripeKey { get; set; }
    }
}
