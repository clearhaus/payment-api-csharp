using Clearhaus.Util;

namespace Clearhaus.Gateway
{
    /// <summary>
    /// Optionals for Authorization transaction, <see href="http://docs.gateway.clearhaus.com/#authorizations"/>.
    /// </summary>
    [Optional]
    public class AuthorizationRequestOptions : RestParameters
    {
        /// <summary>Mark authorization as recurring</summary>
        public bool recurring;

        /// <summary>IPv4/IPv6 address of cardholder initiating authorization</summary>
        public string ip;

        /// <summary>Statement on cardholders bank transaction</summary>
        [ArgName("text_on_statement")]
        public string textOnStatement;

        /// <summary>Authorization reference</summary>
        public string reference;
    }
}
