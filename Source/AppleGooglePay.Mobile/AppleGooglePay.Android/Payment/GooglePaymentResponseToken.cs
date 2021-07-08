using Newtonsoft.Json;

namespace AppleGooglePay.Droid.Payment
{
    public class GooglePaymentResponseToken
    {
        public string Id { get; set; }
        public string Object { get; set; }
        [JsonProperty("client_ip")]
        public string ClientIp { get; set; }
        public int Created { get; set; }
        public bool LiveMode { get; set; }
        public string Type { get; set; }
        public bool Used { get; set; }
    }
}

