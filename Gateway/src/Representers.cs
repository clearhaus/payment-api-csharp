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
    /// Represents a credit card
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Emtpy constructor
        /// </summary>
        public Card()
        {
        }

        /// <summary>
        /// Construct a new card
        /// </summary>
        /// <param name="pan">Primary Account Number"</param>
        /// <param name="expireMonth">Month of card expiry</param>
        /// <param name="expireYear">Year of card expiry</param>
        /// <param name="csc">CSC</param>
        public Card(string pan, string expireMonth, string expireYear, string csc)
        {
            this.pan         = pan;
            this.expireMonth = expireMonth;
            this.expireYear  = expireYear;
            this.csc         = csc;
        }

        /// <summary>
        /// Primary Account Number
        /// </summary>
        public string pan;

        /// <summary>
        /// Month of card expiry
        /// </summary>
        public string expireMonth;

        /// <summary>
        /// Year of card expiry
        /// </summary>
        public string expireYear;

        /// <summary>
        /// Card security code
        /// </summary>
        public string csc;
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
