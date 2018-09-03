using Clearhaus.Util;

namespace Clearhaus.Gateway
{
    /// <summary>Represents MPO information needed for a transaction</summary>
    public class MobilePayOnlineInfo : RestParameters
    {
        /// <summary>Primary Account Number of card</summary>
        [ArgName("mobilepayonline[pan]")]
        public string pan;

        /// <summary>Month of card expiry</summary>
        [ArgName("mobilepayonline[expire_month]")]
        public string expireMonth;

        /// <summary>Year of card expiry</summary>
        [ArgName("mobilepayonline[expire_year]")]
        public string expireYear;

        /// <summary>Phone number (optional)</summary>
        [Optional]
        [ArgName("mobilepayonline[phone_number]")]
        public string phoneNumber;
    }
}
