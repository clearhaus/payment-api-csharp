using Clearhaus.Util;

namespace Clearhaus.Gateway
{
    /// <summary>Represents the details required for performing a payment using Apple Pay</summary>
    public class ApplePayInfo : RestParameters
    {
        /// <summary>paymentData of Apple Pay PKPaymentToken</summary>
        [ArgName("applepay[payment_token]")]
        public string paymentData;

        /// <summary>Symmetric key used to decrypt <c>data</c> key of <c>paymentData</c></summary>
        [ArgName("applepay[symmetric_key]")]
        public string symmetricKey;
    }
}
