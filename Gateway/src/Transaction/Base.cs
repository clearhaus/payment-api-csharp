using Newtonsoft.Json;

namespace Clearhaus.Gateway.Transaction
{
    public abstract class Base
    {
        public Status status{get; set;}

        public string id{get; set;}

        [JsonProperty("processed_at")]
        public string processedAt {set; get;}

        public bool isSuccess()
        {
            return status.code == 20000;
        }

    }

    public class Status
    {
        public int code {get; set;}
        public string message {get; set;}
    }
}
