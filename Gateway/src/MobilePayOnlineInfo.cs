namespace Clearhaus.Gateway
{
    /// <summary>Represents MPO information needed for a transaction</summary>
    public class MobilePayOnlineInfo
    {
        /// <summary>Primary Account Number of card</summary>
        public string pan {get; set;}

        /// <summary>Month of card expiry</summary>
        public string expireMonth {get; set;}

        /// <summary>Year of card expiry</summary>
        public string expireYear {get; set;}

        /// <summary>Phone number (optional)</summary>
        public string phoneNumber {get; set;}

        /// <summary>3D-Secure PARes (optional)</summary>
        public string pares {get; set;}
    }
}
