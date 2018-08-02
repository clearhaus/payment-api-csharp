namespace Clearhaus.Gateway.Transaction
{
    /// <summary>
    /// Represents a credit transaction
    /// </summary>
    public class Credit : Base
    {
        /// <summary>
        /// Amount of money transferred
        /// </summary>
        public string amount{get; set;}
    }
}
