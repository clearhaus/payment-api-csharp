using Newtonsoft.Json;

namespace Clearhaus.MPI.Representers
{
    public class CheckResponse
    {
        public string amount;
        public string currency;
        public string cavv;

        [JsonProperty("cavv_algorithm")]
        public string cavvAlgorithm;
        public string eci;

        [JsonProperty("merchant_id")]
        public string merchantID;
        public string last4;
        public string status;
        public string xid;
    }
}
