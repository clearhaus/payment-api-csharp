using Newtonsoft.Json;

namespace Clearhaus.Gateway
{
    /// <summary>
    /// Deserialization of account information supplied by an /account call against the gateway.
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// ID of merchant in Clearhaus system.
        /// </summary>
        [JsonProperty("merchant_id")]
        public string merchantID { get; set; }

        /// <summary>
        /// The default <c>text_on_statement</c>
        /// </summary>
        public string descriptor { get; set; }

        /// <summary>
        /// Merchant name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Merchant name
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Merchant Category Code
        /// </summary>
        public int mcc { get; set; }

        /// <summary>
        /// Acquirer BIN information
        /// </summary>
        public Acquirer acquirer {get; set;}

        /// <summary>
        /// Transaction rules in Clearhaus rule language
        /// </summary>
        [JsonProperty("transaction_rules")]
        public string transactionRules { get; set; }
    }

    /// <summary>
    /// Information about acquirer BIN
    /// </summary>
    public class Acquirer
    {
        /// <summary>
        /// BIN for VISA systems
        /// </summary>
        [JsonProperty("visa_bin")]
        public string visaBin {get; set;}

        /// <summary>
        /// BIN for MasterCard systems
        /// </summary>
        [JsonProperty("mastercard_bin")]
        public string mastercardBin {get; set;}
    }

    /// <summary>
    /// Represents a card tokenized by Clearhaus transaction gateway.
    /// </summary>
    /// <remarks>
    /// Card tokenization is deprecated.
    /// </remarks>
    // TODO: Add CSC response option?
    public class TokenizedCard
    {
        /// <summary>
        /// UUID
        /// </summary>
        public string id;

        /// <summary>
        /// Last 4 digits
        /// </summary>
        public string last4;

        /// <summary>
        /// Card scheme
        /// </summary>
        public string scheme;
    }
}
