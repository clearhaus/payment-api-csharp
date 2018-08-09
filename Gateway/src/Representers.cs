using Newtonsoft.Json;

namespace Clearhaus.Gateway
{
    public class AccountInfo
    {
        [JsonProperty("merchant_id")]
        public string merchantID { get; set; }
        public string descriptor { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int mcc { get; set; }

        public Acquirer acquirer {get; set;}

        [JsonProperty("transaction_rules")]
        public string transactionRules { get; set; }
    }

    public class Card
    {
        public Card()
        {
        }

        public Card(string pan, string expireMonth, string expireYear, string csc)
        {
            this.pan         = pan;
            this.expireMonth = expireMonth;
            this.expireYear  = expireYear;
            this.csc         = csc;
        }

        public string pan;
        public string expireMonth;
        public string expireYear;
        public string csc;
    }

    public class Acquirer
    {
        [JsonProperty("visa_bin")]
        public string visaBin {get; set;}

        [JsonProperty("mastercard_bin")]
        public string mastercardBin {get; set;}
    }

    // TODO: Add CSC reponse option?
    public class TokenizedCard
    {
        public string id;

        public string last4;

        public string scheme;

        //public string type;
        //public string country;
    }
}
