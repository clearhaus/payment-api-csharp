using Newtonsoft.Json;

namespace Clearhaus.Gateway.Transaction
{
    public class Authorization : Base
    {
        [JsonProperty("csc")]
        public CSCStatus cscStatus {set; get;}
    }

    public class CSCStatus
    {
        public bool present {set; get;}
        public bool matches {set; get;}
    }
}
