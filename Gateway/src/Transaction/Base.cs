using Newtonsoft.Json;

namespace Clearhaus.Gateway.Transaction
{
    /// <summary>
    /// Base class for a Clearhaus transaction result
    /// </summary>
    public abstract class Base
    {
        /// <summary>
        /// Status of the query
        /// </summary>
        public Status status;

        /// <summary>
        /// UUID identifying the transaction
        /// </summary>
        public string id;

        /// <summary>
        /// Datetime the transaction was processed
        /// </summary>
        [JsonProperty("processed_at")]
        public string processedAt;

        /// <summary>
        /// Helper to check if transaction was a success
        /// </summary>
        public bool IsSuccess()
        {
            return status.code == 20000;
        }
    }


    /// <summary>
    /// Status of a query
    /// </summary>
    public class Status
    {
        /// <summary>
        /// See http://docs.gateway.clearhaus.com/#Transactionstatuscodes
        /// </summary>
        public int code;

        /// <summary>
        /// Message associated with status code
        /// </summary>
        public string message;
    }
}
