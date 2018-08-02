namespace Clearhaus.Gateway.Transaction
{
    /// <summary>
    /// Represents a capture transaction
    /// </summary>
    public class Capture : Base
    {
        /// <summary>
        /// The amount captured by this transaction
        /// </summary>
        public string amount {get; set;}
    }
}
