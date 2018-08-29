using System;
using System.Threading.Tasks;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Utilities.Encoders;

using Newtonsoft.Json;

using Clearhaus.Util;
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
    /// <example>
    /// This is an example of how to create an authorization and capture money.
    /// <code lang="C#">
    /// using Clearhaus.Gateway;
    /// using Clearhaus.Gateway.Transaction.Options;
    ///
    /// public static void main()
    /// {
    ///     var apiKey = "My Secret UUID";
    ///     var card = new Card
    ///     {
    ///         pan         = "some PAN",
    ///         expireMonth = "12",
    ///         expireYear  = "2047",
    ///         csc         = "666"
    ///     };
    ///
    ///     var account = new Account(apiKey);
    ///
    ///     var authOptions = new AuthorizationRequestOptions
    ///     {
    ///         recurring = true,
    ///         reference = "sdg7SF12KJHjj"
    ///     };
    ///
    ///     Authorization myAuth;
    ///     try
    ///     {
    ///         myAuth = new Authorize("100", "DKK", card, authOptions);
    ///     }
    ///     catch(ClrhsNetException e)
    ///     {
    ///         // Failure connecting to clearhaus.
    ///         // You should retry this.
    ///         return;
    ///     }
    ///     catch(ClrhsAuthException e)
    ///     {
    ///         // ApiKey was invalid
    ///         // You need to change the apiKey.
    ///         // This can be avoided by checking the key first:
    ///         // account.ValidAPIKey() == true
    ///         return;
    ///     }
    ///     catch(ClrhsGatewayException e)
    ///     {
    ///         // Something was funky with the Clearhaus gateway
    ///         // You could retry this, but maybe give it a few seconds.
    ///         return;
    ///     }
    ///
    ///     if (!myAuth.IsSuccess())
    ///     {
    ///         // The statuscode returned implies that an error occurred.
    ///         Console.WriteLine(auth.status.message);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Requests made with valid parameters will not throw exceptions, unless the invalid parameter is the API key.
    /// Result objects have a <c>status</c> field which contains a <c>code</c> and a <c>message</c>.
    /// These codes/messages can be looked up here https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#transaction-status-codes.
    /// </remarks>
    public class Account
    {
        internal string apiKey;
        internal string signingAPIKey;
        internal AsymmetricCipherKeyPair rsaKeyPair;

        internal bool canSign;

        /// <summary>
        /// Set the timeout for all following requests against the Gateway.
        /// </summary>
        /// <remarks>
        /// Default value is 5 seconds.
        /// This value is passed straight through to a System.Net.HttpClient object without verification.
        /// </remarks>
        public TimeSpan Timeout;

        /// <summary>
        /// URL address of Clearhaus Gateway. By default <c>Constants.GatewayURL</c>.
        /// <seealso cref="Constants.GatewayTestURL"/>.  </summary>
        private Uri gatewayURL;

        /// <summary>
        /// Creates an account object with associated apiKey.
        /// </summary>
        /// <param name="apiKey">
        /// The API Key associated with the merchant account on which the transactions are to be performed.
        /// </param>
        public Account(string apiKey)
        {
            this.apiKey = apiKey;
            this.Timeout = new TimeSpan(0, 0, 5);
            this.gatewayURL = new Uri(Constants.GatewayURL);
        }

        /// <summary>
        /// Creates an account object with associated apiKey, specify alternate gateway address.
        /// </summary>
        /// <param name="apiKey">
        /// The API Key associated with the merchant account on which the transactions are to be performed.
        /// </param>
        /// <param name="gatewayURL">
        /// URL to use as remote Gateway address. Default <c>Constants.GatewayURL</c>.
        /// <seealso cref="Constants.GatewayTestURL"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">If gatewayUrl is null</exception>
        /// <exception cref="System.UriFormatException">If gatewayUrl is invalid URI</exception>
        public Account(string apiKey, string gatewayURL)
        {
            this.apiKey = apiKey;
            this.Timeout = new TimeSpan(0, 0, 5);
            this.gatewayURL = new Uri(gatewayURL);
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
        /// RSA Signing key PEM associated with apiKey. See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#rsa-signature.
        /// </param>
        /// <example>
        /// <code lang="C#">
        /// // This apikey is associated with the account. In general, this
        /// // represents a merchants API key.
        /// var apiKey        = "[My secret UUID]";
        ///
        /// // This APIKey would likely be your key, that is, the key
        /// // belonging to a technical integrator.
        /// var signingApiKey = "[My secret UUID]";
        ///
        /// // This is the private key corresponding to a public key you have
        /// // exchanged with Clearhaus.
        /// var rsaPrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
        ///     MIICWwIBAAKBgQC9KAaGN0y4vHOuXFZLE+GHIgd6Ya8IgL55cWxXzWO8T/AykXyi
        ///     kayr4vy3aTpcJ8JEsZcDWnOkDpyBbaULLjfH7WtTm1Vyt4GrHatv6XtSB921rAIB
        ///     BxKAZUU0BDLdFlztjFgu8qRow2GPxEjltDgiEINwYzhYBbST9EyrowvgcwIBAwKB
        ///     gH4arwQk3dB9onQ9jty3669sBPxBH1sAfvug8uUzl9Lf9XcLqGxhHcfsqHpGJuga
        ///     gYMhD1eRom1fEwDzw1zJeoQjhNuwMmVEajbCrmboT1+wXOZYZdf6UqwgzUJOFCES
        ///     M8cIeStzdnRGLdW56b4q4edohA2Gtb3DV3RslA9xvwCbAkEA3rTybL5hChFAMuiK
        ///     zo5SeSDcHI4MLX1q4TAJ2Dyb4YTE4N8ok2YA8fX+oZOwEDYiM8HfZtVbKKByqOud
        ///     4M7/jQJBANlvF6ZLecbRGMa9Sr518AYxgArbMOIZE1LhRrbYD5mKfh7DRTIMuWgm
        ///     0IvWmGOvJL/7fLJSYDgQ8qiC9peeX/8CQQCUeKGd1ECxYNV3RbHfCYxQwJK9tAge
        ///     U5yWIAaQKGfrrdiV6hsM7qtL+VRrt8q1eWwigT+Z45IbFaHF8mlAif+zAkEAkPS6
        ///     btz72eC7LyjcfvlKrsuqsed17BC3jJYvJJAKZlxUFIIuIV3Q8BngXTm67R9t1VJT
        ///     IYxAJWChxaykZRQ//wJAFM6sXZYIl9SKAcY6iRFElmL1nw3NTFWKU/2/y5fsOn9U
        ///     drtnrCH+i7Iedp+K0qUASitBWAATnHEJ2Q0Pc8LEJQ==
        ///     -----END RSA PRIVATE KEY-----";
        /// var account = new Account(apiKey);
        ///
        /// account.SigningKeys(signingApiKey, rsaPrivateKey);
        ///
        /// // If the key is trusted, API requests against the Clearhaus
        /// // gateway using `account` will now have a signed body.
        /// </code>
        /// </example>
        public void SigningKeys(string apiKey, string rsaPrivateKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ClrhsException("APIKey must be set");
            }

            this.signingAPIKey = apiKey;

            var stringreader = new System.IO.StringReader(rsaPrivateKey);
            var reader = new PemReader(stringreader);

            var obj = reader.ReadObject();
            if (obj == null)
            {
                throw new ClrhsException("Invalid private key. Make sure to remove all spaces.");
            }

            if (!(obj is AsymmetricCipherKeyPair))
            {
                throw new ClrhsException("Key was not what we imagined");
            }

            rsaKeyPair = (AsymmetricCipherKeyPair)obj;

            canSign = true;
        }

        /// <summary>
        /// Connects to the Gateway attempts to authorize with the apiKey.
        /// </summary>
        /// <exception cref="Clearhaus.ClrhsNetException">
        /// Thrown if connection to the gateway fails.
        /// </exception>
        public bool ValidAPIKey()
        {
            var builder = newRestBuilder("");
            var req = builder.Ready();
            System.Net.Http.HttpResponseMessage resp;

            try
            {
                resp = req.GET();
            }
            catch(ClrhsAuthException)
            {
                return false;
            }

            return resp.IsSuccessStatusCode;
        }

        /// <summary>
        /// Connects to the Gateway attempts to authorize with the apiKey.
        /// </summary>
        /// <exception cref="Clearhaus.ClrhsNetException">
        /// Thrown if connection to the gateway fails.
        /// </exception>
        async public Task<bool> ValidAPIKeyAsync()
        {
            var builder = newRestBuilder("");
            var req = builder.Ready();
            System.Net.Http.HttpResponseMessage resp;

            try
            {
                var respTask = req.GETAsync();
                resp = await respTask;
            }
            catch(ClrhsAuthException)
            {
                return false;
            }

            return resp.IsSuccessStatusCode;
        }

        private RestRequestBuilder newRestBuilder(string path, params string[] args)
        {
            var builder = new RestRequestBuilder(gatewayURL, apiKey, "");
            builder.SetPath(path, args);

            if (Timeout != null)
            {
                builder.client.Timeout = Timeout;
            }

            return builder;
        }

        /*
         * REQUEST DISPATCHERS
         */

        private T GETToObject<T>(RestRequest req)
        {
            var response = req.GET();

            req.Dispose();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        async private Task<T> GETToObjectAsync<T>(RestRequest req)
        {
            var responseTask = req.GETAsync();
            var response = await responseTask;

            req.Dispose();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        private T POSTtoObject<T>(RestRequest req)
        {
            if (canSign)
            {
                this.sign(req);
            }

            var response = req.POST();

            req.Dispose();

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        async private Task<T> POSTtoObjectAsync<T>(RestRequest req)
        {
            if (canSign)
            {
                this.sign(req);
            }

            var httpRequestTask = req.POSTAsync();

            var response = await httpRequestTask;

            req.Dispose();

            var bodyReadTask = response.Content.ReadAsStringAsync();

            var body = await bodyReadTask;

            response.Dispose();

            return JsonConvert.DeserializeObject<T>(body);
        }

        /*
         * ACCOUNT INFORMATION IMPLEMENTATION
         */

        /// <summary>
        /// Fetches account information about the associated apiKey.
        /// </summary>
        /// <remarks>
        /// Calls the gateways 'account/' endpoint.
        /// </remarks>
        /// <returns>
        /// An AccountInfo object
        /// </returns>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public AccountInfo FetchAccountInformation()
        {
            var builder = newRestBuilder("account/");

            return GETToObject<AccountInfo>(builder.Ready());
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
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<AccountInfo> FetchAccountInformationAsync()
        {
            var builder = newRestBuilder("account/");

            var task = GETToObjectAsync<AccountInfo>(builder.Ready());
            return await task;
        }

        /*
         * AUTHORIZATION IMPLEMENTATION
         */

        // Build an authorization request using a credit card.
        private RestRequestBuilder buildAuthorizeRequest(
            string amount,
            string currency,
            Card cc,
            string PARes,
            AuthorizationRequestOptions opts)
        {
            var builder = newRestBuilder("authorizations/");

            builder.AddParameter("amount", amount);
            builder.AddParameter("currency", currency);

            builder.AddParameter("card[pan]", cc.pan);

            if (!String.IsNullOrWhiteSpace(cc.csc))
            {
                builder.AddParameter("card[csc]", cc.csc);
            }

            if (!String.IsNullOrWhiteSpace(PARes))
            {
                builder.AddParameter("card[pares]", PARes);
            }

            builder.AddParameter("card[expire_month]", cc.expireMonth);
            builder.AddParameter("card[expire_year]", cc.expireYear);

            if (opts != null)
            {
                builder.AddParameters(opts.GetParameters());
            }

            return builder;
        }

        /// <summary>
        /// Creates an authorization against the Gateway.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#authentication
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="cc">Card to authorize against. <see cref="Clearhaus.Gateway.Card"/> (Omittable, see Clearhaus Documentation)</param>
        /// <param name="PARes">3D-Secure result (omittable)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        /* TODO:
         * - payment-methods
         */
        public Authorization Authorize(string amount, string currency, Card cc, string PARes, AuthorizationRequestOptions opts)
        {

            var builder = buildAuthorizeRequest(amount, currency, cc, PARes, opts);
            return POSTtoObject<Authorization>(builder.Ready());
        }

        /// <summary>
        /// <see cref="Authorize(string, string, Card, string, AuthorizationRequestOptions)"/>
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="cc">Card to authorize against. <see cref="Clearhaus.Gateway.Card"/> (Omittable, see Clearhaus Documentation)</param>
        /// <param name="PARes">3D-Secure result (omittable)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<Authorization> AuthorizeAsync(string amount, string currency, Card cc, string PARes, AuthorizationRequestOptions opts)
        {

            var builder = buildAuthorizeRequest(amount, currency, cc, PARes, opts);
            var POSTTask =  POSTtoObjectAsync<Authorization>(builder.Ready());

            return await POSTTask;
        }

        /*
         * VOID IMPLEMENTATION
         */

        /// <summary>
        /// Void (annul) an authorization
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#voids
        /// </summary>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public Transaction.Void Void(string authorizationID)
        {
            var builder = newRestBuilder("authorizations/{0}/voids", authorizationID);
            return POSTtoObject<Transaction.Void>(builder.Ready());
        }

        /// <summary>
        /// Void (annul) an authorization
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#voids
        /// </summary>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<Transaction.Void> VoidAsync(string authorizationID)
        {
            var builder = newRestBuilder("authorizations/{0}/voids", authorizationID);
            var voidTask = POSTtoObjectAsync<Transaction.Void>(builder.Ready());
            return await voidTask;
        }

        /*
         * CAPTURE IMPLEMENTATION
         */

        private RestRequestBuilder buildCaptureRequest(string id, string amount, string textOnStatement)
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

            return builder;
        }

        /// <summary>
        /// Capture reserved money.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#captures
        /// </summary>
        /// <param name="id">UUID of authorization (Required)</param>
        /// <param name="amount">Amount to capture (Omittable)</param>
        /// <param name="textOnStatement">Text to appear on cardholder bank statement (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public Capture Capture(string id, string amount, string textOnStatement)
        {
            var builder = buildCaptureRequest(id, amount, textOnStatement);
            return POSTtoObject<Capture>(builder.Ready());
        }

        /// <summary>
        /// Capture reserved money.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#captures
        /// </summary>
        /// <param name="id">UUID of authorization (Required)</param>
        /// <param name="amount">Amount to capture (Omittable)</param>
        /// <param name="textOnStatement">Text to appear on cardholder bank statement (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<Capture> CaptureAsync(string id, string amount, string textOnStatement)
        {
            var builder = buildCaptureRequest(id, amount, textOnStatement);
            var objectTask = POSTtoObjectAsync<Capture>(builder.Ready());
            return await objectTask;
        }

        /*
         * REFUND IMPLEMENTATION
         */

        private RestRequestBuilder buildRefundRequest(string id, string amount, string textOnStatement)
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

            return builder;
        }

        /// <summary>
        /// Refund funds captured on an authorization.
        /// https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#refunds
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        /// <param name="amount">Amount to refund or empty string, must be less than captured</param>
        /// <param name="textOnStatement">Overrides text on authorization</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public Refund Refund(string id, string amount, string textOnStatement)
        {
            var builder = buildRefundRequest(id, amount, textOnStatement);
            return POSTtoObject<Refund>(builder.Ready());
        }

        /// <summary>
        /// Refund funds captured on an authorization.
        /// https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#refunds
        /// </summary>
        /// <param name="id">UUID of authorization</param>
        /// <param name="amount">Amount to refund or empty string, must be less than captured</param>
        /// <param name="textOnStatement">Overrides text on authorization</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<Refund> RefundAsync(string id, string amount, string textOnStatement)
        {
            var builder = buildRefundRequest(id, amount, textOnStatement);
            var responseTask = POSTtoObjectAsync<Refund>(builder.Ready());
            return await responseTask;
        }

        /*
         * CREDIT IMPLEMENTATION
         */

        /*
         * For API reasons, we still need to tokenize the card before we can
         * perform a credit transaction
         */
        private RestRequestBuilder buildTokenizeCardRequest(Card cc)
        {
            var builder = newRestBuilder("/cards");
            builder.AddParameter("card[pan]", cc.pan);
            builder.AddParameter("card[expire_month]", cc.expireMonth);
            builder.AddParameter("card[expire_year]", cc.expireYear);
            if (!string.IsNullOrWhiteSpace(cc.csc))
            {
                builder.AddParameter("card[csc]", cc.csc);
            }

            return builder;
        }

        private TokenizedCard TokenizeCard(Card cc)
        {
            var builder = buildTokenizeCardRequest(cc);
            var tokCard = POSTtoObject<TokenizedCard>(builder.Ready());

            return tokCard;
        }

        async private Task<TokenizedCard> TokenizeCardAsync(Card cc)
        {
            var builder = buildTokenizeCardRequest(cc);
            var tokCardTask = POSTtoObjectAsync<TokenizedCard>(builder.Ready());

            return await tokCardTask;
        }

        private RestRequestBuilder buildCreditRequest(string amount, string currency, TokenizedCard cc, string textOnStatement, string reference)
        {

            var builder = newRestBuilder("cards/{0}/credits", cc.id);
            builder.AddParameter("id", cc.id);
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

            return builder;
        }

        /// <summary>
        /// Transfer funds to cardholder account.
        /// </summary>
        /// <param name="amount">Amount to transfer</param>
        /// <param name="currency">Currency to use for transfer</param>
        /// <param name="cc">Card to transfer to</param>
        /// <param name="textOnStatement">Statement on cardholders bank account</param>
        /// <param name="reference">External reference</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public Credit Credit(string amount, string currency, Card cc, string textOnStatement, string reference)
        {
            // The API currently needs us to create a `cards/` resource before
            // we can do a credit.
            var tokCard = TokenizeCard(cc);

            var builder = buildCreditRequest(amount, currency, tokCard, textOnStatement, reference);
            var credit = POSTtoObject<Credit>(builder.Ready());

            return credit;
        }

        /// <summary>
        /// Transfer funds to cardholder account.
        /// </summary>
        /// <param name="amount">Amount to transfer</param>
        /// <param name="currency">Currency to use for transfer</param>
        /// <param name="cc">Card to transfer to</param>
        /// <param name="textOnStatement">Statement on cardholders bank account</param>
        /// <param name="reference">External reference</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<Credit> CreditAsync(string amount, string currency, Card cc, string textOnStatement, string reference)
        {
            // The API currently needs us to create a `cards/` resource before
            // we can do a credit.
            var tokCardTask = TokenizeCardAsync(cc);
            var tokCard = await tokCardTask;

            var builder = buildCreditRequest(amount, currency, tokCard, textOnStatement, reference);
            var creditTask = POSTtoObjectAsync<Credit>(builder.Ready());

            return await creditTask;
        }

        /*
         * SIGNING IMPLEMENTATION
         */

        private void sign(RestRequest request)
        {

            byte[] bodyBytes = request.Body();
            var sha256 = new Sha256Digest();

            var signer = new RsaDigestSigner(sha256);
            signer.Init(true, (ICipherParameters)rsaKeyPair.Private);
            signer.BlockUpdate(bodyBytes, 0, bodyBytes.Length);

            byte[] signature = signer.GenerateSignature();

            string hexEncoded = Hex.ToHexString(signature);

            var header = string.Format("{0} RS256-hex {1}", signingAPIKey, hexEncoded);
            request.AddHeader("Signature", header);
        }
    }
}
