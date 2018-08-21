using Newtonsoft.Json;

namespace Clearhaus.MPI.Representers
{
    /// <summary>Represents a response of a check call</summary>
    public class CheckResponse
    {
        /// <summary>amount on transaction</summary>
        public string amount;

        /// <summary>currency of the transaction</summary>
        public string currency;

        /// <summary>CAVV (Cardholder Authentication Verification Value)</summary>
        public string cavv;

        /// <summary>The algorithm used for the CAVV algorithm</summary>
        [JsonProperty("cavv_algorithm")]
        public string cavvAlgorithm;

        /// <summary>Electronic Commerce Indicator containing the result</summary>
        public string eci;

        /// <summary>Merchant ID of associated merchant</summary>
        [JsonProperty("merchant_id")]
        public string merchantID;

        /// <summary>Last 4 digits of PAN</summary>
        public string last4;

        /// <summary>Status of the <c>PARes</c></summary>
        /// <remarks>Corresponds to the <c>TX.Status</c> field in the <c>PARes</c> XML</remarks>
        public string status;

        /// <summary>Merchant transaction ID</summary>
        public string xid;
    }
}
