namespace Clearhaus.Gateway
{
    /// <summary>Represents the details required for performing a payment using Apple Pay</summary>
    public class ApplePayInfo
    {
        /// <summary>paymentData of Apple Pay PKPaymentToken</summary>
        public string paymentData {get; set;}

        /// <summary>Symmetric key used to decrypt <c>data</c> key of <c>paymentData</c></summary>
        public string symmetricKey {get; set;}
    }
}
