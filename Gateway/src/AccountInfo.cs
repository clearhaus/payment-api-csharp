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
}
