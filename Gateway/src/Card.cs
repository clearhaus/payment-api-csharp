namespace Clearhaus.Gateway
{
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
        public Card(string pan, string expireMonth, string expireYear)
        {
            this.pan         = pan;
            this.expireMonth = expireMonth;
            this.expireYear  = expireYear;
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
}
