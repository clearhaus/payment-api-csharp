using System.Collections.Generic;

namespace Clearhaus.Gateway.Transaction.Options
{
    /// <summary>
    /// Optionals for Authorization transaction, <see href="http://docs.gateway.clearhaus.com/#authorizations"/>.
    /// </summary>
    public class AuthorizationOptions
    {
        /// <summary>Mark authorization as recurring</summary>
        public bool   recurring;

        /// <summary>IPv4/IPv6 address of cardholder initiating authorization</summary>
        public string ip;

        /// <summary>Statement on cardholders bank transaction</summary>
        public string textOnStatement;

        /// <summary>Authorization reference</summary>
        public string reference;

        /// <summary>Proof of 3D-Secure participation</summary>
        /// <remarks>Payer Authentication Response is the message returned by ACS</remarks>
        public string pares;

        /// <summary>
        /// Returns the parameters with correct keys.
        /// </summary>
        public IList<KeyValuePair<string, string>> GetParameters()
        {
            var l = new List<KeyValuePair<string, string>>();
            if (recurring)
            {
                l.Add(new KeyValuePair<string,string>("recurring", "true"));
            }

            if (!string.IsNullOrWhiteSpace(ip))
            {
                l.Add(new KeyValuePair<string,string>("ip", ip));
            }

            if (!string.IsNullOrWhiteSpace(textOnStatement))
            {
                l.Add(new KeyValuePair<string,string>("text_on_statement", textOnStatement));
            }

            if (!string.IsNullOrWhiteSpace(reference))
            {
                l.Add(new KeyValuePair<string,string>("reference", reference));
            }

            if (!string.IsNullOrWhiteSpace(pares))
            {
                l.Add(new KeyValuePair<string,string>("card[pares]", pares));
            }

            return l;
        }
    }
}
