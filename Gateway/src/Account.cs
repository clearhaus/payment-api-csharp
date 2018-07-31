using System;
using System.Text;
using System.Collections.Generic;

using System.Net.Http;
using HttpStatusCode = System.Net.HttpStatusCode;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Utilities.Encoders;

using Newtonsoft.Json;

using Clearhaus.Gateway.Transaction;

namespace Clearhaus.Gateway
{
    /*
     * Class representing an account for the Clearhaus Gateway, identified
     * by an apiKey
     */
    /// <summary>
    /// Represents an account that integrates towards the Clearhaus gateway.
    /// </summary>
    public class Account
    {
        internal string apiKey;
        internal string signingAPIKey;
        internal string rsaPrivateKey;

        /// <summary>
        /// URL address of Clearhaus Gateway. By default <c>Constants.GatewayURL</c>.
        /// <seealso cref="Constants.GatewayTestURL"/>.  </summary>
        public string gatewayURL = Constants.GatewayURL;

        // HTTP Status Codes that indicate success with gateway requests.
        private HttpStatusCode[] httpSuccessCodes = {
            HttpStatusCode.OK,
            HttpStatusCode.Created
        };

        /// <summary>
        /// Creates an account object with associated apiKey.
        /// </summary>
        /// <param name="apiKey">
        /// The API Key associated with the merchant account where transactions
        /// must end up.
        /// </param>
        public Account(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Sets the Clearhaus API key and associated rsaPrivateKey to be
        /// used for signing. When these parameters are set, all requests are
        /// signed.
        /// </summary>
        /// <param name="apiKey">
        /// API key issued to trusted integrator.
        /// </param>
        /// <param name="rsaPrivateKey">
        /// RSA Signing key associated with apiKey.
        /// </param>
        public void SigningKeys(string apiKey, string rsaPrivateKey)
        {
            this.signingAPIKey = apiKey;
            this.rsaPrivateKey = rsaPrivateKey;
        }

        /// <summary>
        /// Connects to the Gateway attempts to authorize with the apiKey.
        /// </summary>
        /// <exception cref="Clearhaus.Gateway.ClrhsNetException">
        /// Thrown if connection to the gateway fails.
        /// </exception>
        public bool ValidAPIKey()
        {
            var builder = newRestBuilder("");
            var req = builder.Ready();
            var resp = req.GET();

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            } else
            {
                return true;
            }

        }

        /// <summary>
        /// Helper to ensure apikey is applied to all rest calls.
        /// </summary>
        /// <param name="path">
        /// URL path with format options like string.Format.
        /// </param>
        /// <param name="args">
        /// String arguments to format string path.
        /// </param>
        private Util.RestRequestBuilder newRestBuilder(string path, params string[] args)
        {
            var builder = new Util.RestRequestBuilder(new Uri(gatewayURL), apiKey, "");
            builder.SetPath(path, args);

            return builder;
        }

        private T GETToObject<T>(Util.RestRequest req)
        {
            var response = req.GET();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        private T POSTtoObject<T>(Util.RestRequest req)
        {
            if (!string.IsNullOrEmpty(rsaPrivateKey) && !string.IsNullOrEmpty(signingAPIKey))
            {
                this.sign(req);
            }

            var response = req.POST();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Fetches account information about the class apiKey.
        /// </summary>
        /// <remarks>
        /// Calls the gateways 'account/' endpoint.
        /// </remarks>
        /// <returns>
        /// An AccountInfo object
        /// </returns>
        public AccountInfo FetchAccountInformation()
        {
            var builder = newRestBuilder("account/");

            return GETToObject<AccountInfo>(builder.Ready());
        }

        /*
         * AUTHORIZATION IMPLEMENTATION
         */

        /// <summary>
        /// <see cref="Authorize(string, string, Card)"/>
        /// </summary>
        public Authorization Authorize(string amount, string currency, Card cc)
        {
            return Authorize(amount, currency, cc, "");
        }

        /// <summary>
        /// Creates an authorization against the Gateway (<a href="https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#authentication">Read More</a>)
        /// </summary>
        /// <param name="amount">
        /// Minor units of <c>currency</c>.
        /// <a href="https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#parameters">
        /// See also Gateway Documentation.
        /// </a>
        /// </param>
        /// <param name="currency">
        /// Currency in which <c>amount</c> is specified.
        /// <a href="https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#parameters">
        /// See also Gateway Documentation.
        /// </a>
        /// </param>
        /// <param name="cc">
        /// Card to authorize against. <see cref="Clearhaus.Gateway.Card"/>.
        /// </param>
        /// <param name="ip">
        /// IPv4/IPv6 address from which the purchase originated. <i>Optional</i>..
        /// <a href="https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#parameters">
        /// See also Gateway Documentation.
        /// </a>
        /// </param>
        public Authorization Authorize(string amount, string currency, Card cc, string ip)
        {
            var builder = newRestBuilder("authorizations/");

            builder.AddParameter("amount", amount);
            builder.AddParameter("currency", currency);
            if (!string.IsNullOrEmpty(ip))
            {
                builder.AddParameter("ip", ip);
            }
            builder.AddParameter("card[pan]", cc.pan);
            builder.AddParameter("card[csc]", cc.csc);
            builder.AddParameter("card[expire_month]", cc.expireMonth);
            builder.AddParameter("card[expire_year]", cc.expireYear);

            return POSTtoObject<Authorization>(builder.Ready());
        }

        /*
         * VOID IMPLEMENTATION
         */

        public Transaction.Void Void(Authorization auth)
        {
            return Void(auth.id);
        }

        public Transaction.Void Void(string authorizationID)
        {
            var builder = newRestBuilder("authorizations/{0}/voids", authorizationID);
            return POSTtoObject<Transaction.Void>(builder.Ready());
        }

        /*
         * CAPTURE IMPLEMENTATION
         */

        public Capture Capture(string id)
        {
            return Capture(id, "");
        }

        public Capture Capture(Authorization auth)
        {
            return Capture(auth.id, "");
        }

        public Capture Capture(Authorization auth, string amount)
        {
            return Capture(auth.id, amount);
        }

        public Capture Capture(string id, string amount)
        {

            var builder = newRestBuilder("authorizations/{0}/captures", id);

            if (!string.IsNullOrEmpty(amount))
            {
                builder.AddParameter("amount", amount);
            }

            return POSTtoObject<Capture>(builder.Ready());
        }

        /*
         * REFUND IMPLEMENTATION
         */

        public Refund Refund(string id, string amount)
        {
            var builder = newRestBuilder( "authorizations/{0}/refunds", id);

            if (!string.IsNullOrEmpty(amount))
            {
                builder.AddParameter("amount", amount);
            }

            return POSTtoObject<Refund>(builder.Ready());
        }

        /*
         * CREDIT IMPLEMENTATION
         */

        private TokenizedCard TokenizeCard(Card cc)
        {
            var builder = newRestBuilder("/cards");
            builder.AddParameter("card[pan]", cc.pan);
            builder.AddParameter("card[expire_month]", cc.expireMonth);
            builder.AddParameter("card[expire_year]", cc.expireYear);
            if (!string.IsNullOrEmpty(cc.csc))
            {
                builder.AddParameter("card[csc]", cc.csc);
            }

            var tokCard = POSTtoObject<TokenizedCard>(builder.Ready());

            return tokCard;
        }

        public Credit Credit(string amount, string currency, Card cc)
        {
            // The API currently needs us to create a `cards/` resource before
            // we can do a credit.
            var tokCard = TokenizeCard(cc);

            var builder = newRestBuilder("cards/{0}/credits", tokCard.id);
            builder.AddParameter("id", tokCard.id);
            builder.AddParameter("amount", amount);
            builder.AddParameter("currency", currency);

            var credit = POSTtoObject<Credit>(builder.Ready());

            return credit;
        }

        /*
         * SIGNING IMPLEMENTATION
         */

        private void sign(Util.RestRequest request)
        {
            var stringreader = new System.IO.StringReader(rsaPrivateKey);
            var reader = new PemReader(stringreader);

            var obj = reader.ReadObject();
            if (obj == null)
            {
                throw new Exception("Invalid private key, remove all spaces");
            }

            if (!(obj is AsymmetricCipherKeyPair))
            {
                throw new Exception("Key was not what we imagined");
            }

            byte[] bodyBytes = request.Body();
            var sha256 = new Sha256Digest();

            AsymmetricCipherKeyPair keypair = (AsymmetricCipherKeyPair)obj;
            var signer = new RsaDigestSigner(sha256);
            signer.Init(true, (ICipherParameters)keypair.Private);

            signer.BlockUpdate(bodyBytes, 0, bodyBytes.Length);

            byte[] signature = signer.GenerateSignature();

            string hexEncoded = Hex.ToHexString(signature);

            var header = string.Format("{0} RS256-hex {1}", signingAPIKey, hexEncoded);
            request.AddHeader("Signature", header);
        }
    }
}
