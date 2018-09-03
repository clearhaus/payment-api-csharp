using Clearhaus.Util;

namespace Clearhaus.MPI.Builder
{
    /// <summary>Contains information for performing 3D-Secure flow</summary>
    public class EnrollCheckBuilder : RestParameters
    {
        /// <summary>Amount of currency in transaction</summary>
        public string amount;

        /// <summary>Currency of transaction</summary>
        public string currency;

        /// <summary>Unique transaction identifier</summary>
        [ArgName("order_id")]
        public string orderID;

        /// <summary>IP of cardholder</summary>>
        [ArgName("cardholder_ip")]
        public string cardholderIP;

        /// <summary>PAN</summary>
        [ArgName("card[number]")]
        public string cardNumber;

        /// <summary>Month of card expiry</summary>
        [ArgName("card[expire_month]")]
        public string cardExpireMonth;

        /// <summary>Year of card expiry</summary>
        [ArgName("card[expire_year]")]
        public string cardExpireYear;

        /// <summary>Acquirer BIN for the card scheme</summary>
        [ArgName("merchant[acquirer_bin]")]
        public string merchantAcquirerBin;

        /// <summary>Country of merchant</summary>
        [ArgName("merchant[country]")]
        public string merchantCountry;

        /// <summary>ID of merchant</summary>
        [ArgName("merchant[id]")]
        public string merchantID;

        /// <summary>Merchant Name</summary>
        [ArgName("merchant[name]")]
        public string merchantName;

        /// <summary>URL of merchant page</summary>
        [ArgName("merchant[url]")]
        public string merchantUrl;
    }
}
