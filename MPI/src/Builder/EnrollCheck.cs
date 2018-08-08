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

    public class EnrollCheckBuilder
    {
        public string amount;
        public string currency;

        [ArgName("order_id")]
        public string orderID;

        [ArgName("cardholder_ip")]
        public string cardholderIP;

        [ArgName("card[number]")]
        public string cardNumber;

        [ArgName("card[expire_month]")]
        public string cardExpireMonth;

        [ArgName("card[expire_year]")]
        public string cardExpireYear;

        [ArgName("merchant[acquirer_bin]")]
        public string merchantAcquirerBin;

        [ArgName("merchant[country]")]
        public string merchantCountry;

        [ArgName("merchant[id]")]
        public string merchantID;

        [ArgName("merchant[name]")]
        public string merchantName;

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
