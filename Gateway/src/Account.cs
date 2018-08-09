using System;

// using System.Net conflictions with Authorization class
using HttpStatusCode = System.Net.HttpStatusCode;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Utilities.Encoders;

using Newtonsoft.Json;

using Clearhaus.Util;
using Clearhaus.Gateway.Transaction;
using Clearhaus.Gateway.Transaction.Options;

namespace Clearhaus.Gateway
{
    /*
     * Class representing an account for the Clearhaus Gateway, identified
     * by an apiKey
     */
    /// <summary>
    /// Represents an account that integrates towards the Clearhaus gateway.
    /// </summary>
    /// <example>
    /// This is an example of how to create an authorization and capture money.
    /// <code lang="csharp">
    /// using Clearhaus.Gateway;
    /// public static void main()
    /// {
    ///     var apiKey = "My Secret UUID";
    /// }
    /// </code>
    /// </example>
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
        /// The API Key associated with the merchant account on which the transactions are to be performed.
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
        /// RSA Signing key associated with apiKey. See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#rsa-signature.
        /// </param>
        public void SigningKeys(string apiKey, string rsaPrivateKey)
        {
            this.signingAPIKey = apiKey;
            this.rsaPrivateKey = rsaPrivateKey;
        }

        /// <summary>
        /// Connects to the Gateway attempts to authorize with the apiKey.
        /// </summary>
        /// <exception cref="Clearhaus.Util.ClrhsNetException">
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
        private RestRequestBuilder newRestBuilder(string path, params string[] args)
        {
            var builder = new RestRequestBuilder(new Uri(gatewayURL), apiKey, "");
            builder.SetPath(path, args);

            return builder;
        }

        private T GETToObject<T>(RestRequest req)
        {
            var response = req.GET();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        private T POSTtoObject<T>(RestRequest req)
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
        /// Fetches account information about the associated apiKey.
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
        /// <see cref="Authorize(string, string, Card, AuthorizationOptions)"/>
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c></param>
        /// <param name="currency">Currency in which <c>amount</c> is specified</param>
        /// <param name="cc">Card to authorize against. <see cref="Clearhaus.Gateway.Card"/></param>
        public Authorization Authorize(string amount, string currency, Card cc)
        {
            return Authorize(amount, currency, cc, null);
        }

        /// <summary>
        /// Creates an authorization against the Gateway.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#authentication
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c></param>
        /// <param name="currency">Currency in which <c>amount</c> is specified</param>
        /// <param name="cc">Card to authorize against. <see cref="Clearhaus.Gateway.Card"/></param>
        /// <param name="opts">Optional parameters for authorizations or null</param>
        /* TODO:
         * - recurring
         * - text_on_statement
         * - reference
         * - payment-methods
         */
        public Authorization Authorize(string amount, string currency, Card cc, AuthorizationOptions opts)
        {
            var builder = newRestBuilder("authorizations/");

            builder.AddParameter("amount", amount);
            builder.AddParameter("currency", currency);

            builder.AddParameter("card[pan]", cc.pan);
            builder.AddParameter("card[csc]", cc.csc);
            builder.AddParameter("card[expire_month]", cc.expireMonth);
            builder.AddParameter("card[expire_year]", cc.expireYear);

            if (opts != null)
            {
                builder.AddParameters(opts.GetParameters());
            }

            return POSTtoObject<Authorization>(builder.Ready());
        }

        /*
         * VOID IMPLEMENTATION
         */

        /// <summary>
        /// <see cref="Void(string)"/>
        /// </summary>
        public Transaction.Void Void(Authorization auth)
        {
            return Void(auth.id);
        }

        /// <summary>
        /// Void (annul) an authorization
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#voids
        /// </summary>
        public Transaction.Void Void(string authorizationID)
        {
            var builder = newRestBuilder("authorizations/{0}/voids", authorizationID);
            return POSTtoObject<Transaction.Void>(builder.Ready());
        }

        /*
         * CAPTURE IMPLEMENTATION
         */

        /// <summary>
        /// <see cref="Capture(string, string, string)"/>
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        public Capture Capture(string id)
        {
            return Capture(id, "", "");
        }

        /// <summary>
        /// <see cref="Capture(string, string, string)"/>
        /// </summary>
        /// <param name="auth">Authorization object on which to perform capture</param>
        public Capture Capture(Authorization auth)
        {
            return Capture(auth.id, "", "");
        }

        /// <summary>
        /// <see cref="Capture(string, string, string)"/>
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        /// <param name="amount">Amount to capture</param>
        public Capture Capture(string id, string amount)
        {
            return Capture(id, amount, "");
        }

        /// <summary>
        /// <see cref="Capture(string, string, string)"/>
        /// </summary>
        /// <param name="auth">Authorization object on which to perform capture</param>
        /// <param name="amount">Amount to capture</param>
        public Capture Capture(Authorization auth, string amount)
        {
            return Capture(auth.id, amount, "");
        }

        /// <summary>
        /// <see cref="Capture(string, string, string)"/>
        /// </summary>
        /// <param name="auth">Authorization object on which to perform capture</param>
        /// <param name="amount">Amount to capture</param>
        /// <param name="textOnStatement">Text to appear on cardholder bank statement</param>
        public Capture Capture(Authorization auth, string amount, string textOnStatement)
        {
            return Capture(auth.id, amount, textOnStatement);
        }

        /// <summary>
        /// Capture reserved money.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#captures
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        /// <param name="amount">Amount to capture</param>
        /// <param name="textOnStatement">Text to appear on cardholder bank statement</param>
        public Capture Capture(string id, string amount, string textOnStatement)
        {

            var builder = newRestBuilder("authorizations/{0}/captures", id);

            if (!string.IsNullOrWhiteSpace(textOnStatement))
            {
                builder.AddParameter("text_on_statement", textOnStatement);
            }

            if (!string.IsNullOrWhiteSpace(amount))
            {
                builder.AddParameter("amount", amount);
            }

            return POSTtoObject<Capture>(builder.Ready());
        }

        /*
         * REFUND IMPLEMENTATION
         */

        /// <summary>
        /// <see cref="Refund(string, string, string)"/>
        /// </summary>
        /// <param name="auth">Authorization to refund</param>
        public Refund Refund(Authorization auth)
        {
            return Refund(auth.id, "", "");
        }

        /// <summary>
        /// <see cref="Refund(string, string, string)"/>
        /// </summary>
        /// <param name="auth">Authorization to refund</param>
        /// <param name="amount">Amount to refund or empty string, must be less than captured</param>
        public Refund Refund(Authorization auth, string amount)
        {
            return Refund(auth.id, amount, "");
        }

        /// <summary>
        /// <see cref="Refund(string, string, string)"/>
        /// </summary>
        /// <param name="auth">Authorization to refund</param>
        /// <param name="amount">Amount to refund or empty string, must be less than captured</param>
        /// <param name="textOnStatement">Overrides text on authorization</param>
        public Refund Refund(Authorization auth, string amount, string textOnStatement)
        {
            return Refund(auth.id, amount, textOnStatement);
        }

        /// <summary>
        /// <see cref="Refund(string, string, string)"/>
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        public Refund Refund(string id)
        {
            return Refund(id, "", "");
        }

        /// <summary>
        /// <see cref="Refund(string, string, string)"/>
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        /// <param name="amount">Amount to refund or empty string, must be less than captured</param>
        public Refund Refund(string id, string amount)
        {
            return Refund(id, amount, "");
        }

        /// <summary>
        /// Refund funds captured on an authorization.
        /// https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#refunds
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        /// <param name="amount">Amount to refund or empty string, must be less than captured</param>
        /// <param name="textOnStatement">Overrides text on authorization</param>
        public Refund Refund(string id, string amount, string textOnStatement)
        {
            var builder = newRestBuilder( "authorizations/{0}/refunds", id);

            if (!string.IsNullOrWhiteSpace(amount))
            {
                builder.AddParameter("amount", amount);
            }

            if (!string.IsNullOrWhiteSpace(textOnStatement))
            {
                builder.AddParameter("text_on_statement", textOnStatement);
            }

            return POSTtoObject<Refund>(builder.Ready());
        }

        /*
         * CREDIT IMPLEMENTATION
         */

        /*
         * For API reasons, we still need to tokenize the card before we can
         * perform a credit transaction
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

        /// <summary>
        /// Transfer funds to cartholder account.
        /// </summary>
        /// <param name="amount">Amount to transfer</param>
        /// <param name="currency">Currency to use for transfer</param>
        /// <param name="cc">Card to transfer to</param>
        /// <param name="textOnStatement">Statement on cardholders bank account</param>
        /// <param name="reference">External reference</param>
        public Credit Credit(string amount, string currency, Card cc, string textOnStatement, string reference)
        {
            // The API currently needs us to create a `cards/` resource before
            // we can do a credit.
            var tokCard = TokenizeCard(cc);

            var builder = newRestBuilder("cards/{0}/credits", tokCard.id);
            builder.AddParameter("id", tokCard.id);
            builder.AddParameter("amount", amount);
            builder.AddParameter("currency", currency);

            if (!string.IsNullOrWhiteSpace(textOnStatement))
            {
                builder.AddParameter("text_on_statement", textOnStatement);
            }

            if (!string.IsNullOrWhiteSpace(reference))
            {
                builder.AddParameter("reference", reference);
            }

            var credit = POSTtoObject<Credit>(builder.Ready());

            return credit;
        }

        /*
         * SIGNING IMPLEMENTATION
         */

        private void sign(RestRequest request)
        {
            var stringreader = new System.IO.StringReader(rsaPrivateKey);
            var reader = new PemReader(stringreader);

            var obj = reader.ReadObject();
            if (obj == null)
            {
                throw new Exception("Invalid private key. Make sure to remove all spaces.");
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
