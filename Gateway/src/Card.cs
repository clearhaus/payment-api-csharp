using Clearhaus.Util;

namespace Clearhaus.Gateway
{
    /// <summary>
    /// Represents a credit card
    /// </summary>
    public class Card : RestParameters
    {
        /// <summary>
        /// Emtpy constructor
        /// </summary>
        public Card() {}

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
        [ArgName("card[pan]")]
        public string pan;

        /// <summary>
        /// Month of card expiry
        /// </summary>
        [ArgName("card[expire_month]")]
        public string expireMonth;

        /// <summary>
        /// Year of card expiry
        /// </summary>
        [ArgName("card[expire_year]")]
        public string expireYear;

        /// <summary>
        /// Card security code (Optional, see documentation)
        /// </summary>
        [Optional]
        [ArgName("card[csc]")]
        public string csc;
    }
}
