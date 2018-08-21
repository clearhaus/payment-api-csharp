using System;
using System.Reflection;
using System.Collections.Generic;


namespace Clearhaus.MPI.Builder
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class ArgName : Attribute
    {
        public string name;

        public ArgName(string name)
        {
            this.name = name;
        }
    }

    /// <summary>Contains information for performing 3D-Secure flow</summary>
    public class EnrollCheckBuilder
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

        private string GetFieldName(FieldInfo info)
        {
                var res = (ArgName[])info.GetCustomAttributes(typeof(ArgName), false);
                if (res.Length > 0)
                {
                    var attr = res[0];
                    return attr.name;
                }

                return info.Name;
        }

        /// <summary>Extract object data as a list, for creating request.</summary>
        public IList<KeyValuePair<string, string>> GetArgs()
        {
            var list = new List<KeyValuePair<string, string>>();

            // Iterate through fields, extracting possible argname supplied.
            var fields = this.GetType().GetFields();
            foreach (var info in fields)
            {
                var key = GetFieldName(info);
                var val = Convert.ChangeType(info.GetValue(this), info.FieldType);

                /*
                if (string.IsNullOrWhiteSpace(val))
                {
                    throw new Exception(String.Format("Value '{0}' must be set", info.Name));
                }
                */

                if (val is string && !string.IsNullOrWhiteSpace((string)val))
                {
                    var pair = new KeyValuePair<string, string>(key, (string)val);
                    list.Add(pair);
                }
            }

            return list;
        }
    }
}
