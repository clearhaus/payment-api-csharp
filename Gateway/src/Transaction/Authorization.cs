using Newtonsoft.Json;

namespace Clearhaus.Gateway.Transaction
{
    /// <summary>
    /// Class that represents a completed authorization.
    /// </summary>
    public class Authorization : Base
    {
        /// <summary>
        /// CSC Status
        /// </summary>
        [JsonProperty("csc")]
        public CSCStatus cscStatus {set; get;}
    }

    /// <summary>
    /// Status of CSC for the authorization.
    /// </summary>
    public class CSCStatus
    {
        /// <summary>
        /// Whether the CSC was present in authorization
        /// </summary>
        public bool present {set; get;}

        /// <summary>
        /// Whether the CSC matched
        /// </summary>
        public bool matches {set; get;}
    }
}
