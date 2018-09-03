using System;
using System.Net.Http;
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
    ///     // The `Account` destructor disposes of the HttpClient,
    ///     // it is also possible to call `#Dispose` manually.
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
    ///         myAuth = account.Authorize("100", "DKK", card, null, authOptions);
    ///
    ///         if (!myAuth.IsSuccess())
    ///         {
    ///             // The statuscode returned implies that an error occurred.
    ///             Console.WriteLine(myAuth.status.message);
    ///         }
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
    ///     catch(ClrhsException e)
    ///     {
    ///         // Last effort exception
    ///         System.Console.WriteLine(e.Message);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Requests made with valid parameters will not throw exceptions, unless the invalid parameter is the API key.
    /// Result objects have a <c>status</c> field which contains a <c>code</c> and a <c>message</c>.
    /// These codes/messages can be looked up here https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#transaction-status-codes.
    /// </remarks>
    public class Account : IDisposable
    {
        private string apiKey;

        // API-key used for signing requests, not used for authentication.
        private string signingAPIKey;
        // RSA private key used signing the body of requests, before being sent.
        private AsymmetricCipherKeyPair rsaKeyPair;

        // Used to test if signing credentials were added.
        private bool canSign;

        // If we have been disposed.
        private bool disposed;

        private HttpClient httpClient;

        /// <summary>The default timespan used for HttpClient, 40s)</summary>
        public readonly TimeSpan Timeout = new TimeSpan(0, 0, 40);

        private void InitializeHttpClient()
        {
            var clientHandler = new HttpClientHandler {
                Credentials = new System.Net.NetworkCredential(this.apiKey, "")
            };

            // Tell it to dispose the HttpClientHandler, so we don't need to.
            this.httpClient = new HttpClient(clientHandler, true) {
                BaseAddress = this.gatewayURL,
            };

            if (this.Timeout != null)
            {
                httpClient.Timeout = this.Timeout;
            }
        }

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
            this.gatewayURL = new Uri(Constants.GatewayURL);

            InitializeHttpClient();
        }

        /// <summary>
        /// Creates an account object with associated apiKey, specify alternate gateway address.
        /// </summary>
        /// <param name="apiKey">
        /// The API Key associated with the merchant account on which the transactions are to be performed.
        /// </param>
        /// <param name="timeout">HttpClient timeout property</param>
        /// <exception cref="System.ArgumentNullException">If gatewayUrl is null</exception>
        /// <exception cref="System.UriFormatException">If gatewayUrl is invalid URI</exception>
        public Account(string apiKey, TimeSpan timeout)
        {
            this.apiKey = apiKey;
            this.Timeout = timeout;
            this.gatewayURL = new Uri(Constants.GatewayURL);

            InitializeHttpClient();
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
            this.gatewayURL = new Uri(gatewayURL);

            InitializeHttpClient();
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
        /// <param name="timeout">HttpClient timeout property</param>
        /// <exception cref="System.ArgumentNullException">If gatewayUrl is null</exception>
        /// <exception cref="System.UriFormatException">If gatewayUrl is invalid URI</exception>
        public Account(string apiKey, string gatewayURL, TimeSpan timeout)
        {
            this.apiKey = apiKey;
            this.Timeout = timeout;
            this.gatewayURL = new Uri(gatewayURL);

            InitializeHttpClient();
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
            using(var rest = buildRestRequest(""))
            {
                System.Net.Http.HttpResponseMessage resp;

                try
                {
                    resp = rest.GET();
                }
                catch(ClrhsAuthException)
                {
                    return false;
                }

                // Should always be success, if we get this far.
                if (!resp.IsSuccessStatusCode)
                {
                    throw new ClrhsGatewayException("Invalid response from gateway: " + resp.StatusCode);
                }

                return true;
            }
        }

        /// <summary>
        /// Connects to the Gateway attempts to authorize with the apiKey.
        /// </summary>
        /// <exception cref="Clearhaus.ClrhsNetException">
        /// Thrown if connection to the gateway fails.
        /// </exception>
        async public Task<bool> ValidAPIKeyAsync()
        {
            using(var restRequest = buildRestRequest(""))
            {
                System.Net.Http.HttpResponseMessage resp;

                try
                {
                    resp = await restRequest.GETAsync();
                }
                catch(ClrhsAuthException)
                {
                    return false;
                }

                // Should always be success, if we get this far.
                if (!resp.IsSuccessStatusCode)
                {
                    throw new ClrhsGatewayException("Invalid response from gateway: " + resp.StatusCode);
                }

                return true;
            }
        }

        private RestRequest buildRestRequest(string path, params string[] args)
        {
            var restRequest = new RestRequest(httpClient);
            restRequest.SetPath(path, args);

            return restRequest;
        }

        /*
         * REQUEST DISPATCHERS
         */

        private T GETToObject<T>(RestRequest reqBuilder)
        {
            var response = reqBuilder.GET();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        async private Task<T> GETToObjectAsync<T>(RestRequest req)
        {
            var response = await req.GETAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private T POSTtoObject<T>(RestRequest req)
        {
            if (canSign)
            {
                this.sign(req);
            }

            var response = req.POST();

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        async private Task<T> POSTtoObjectAsync<T>(RestRequest req)
        {
            if (canSign)
            {
                this.sign(req);
            }

            var response = await req.POSTAsync();
            var body = await response.Content.ReadAsStringAsync();

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
            using(var restRequest = buildRestRequest("account/"))
            {
                return GETToObject<AccountInfo>(restRequest);
            }
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
            using(var restRequest = buildRestRequest("account/"))
            {
                return await GETToObjectAsync<AccountInfo>(restRequest);
            }
        }

        /*
         * AUTHORIZATION IMPLEMENTATION
         */

        // Build an authorization request using a credit card.
        private RestRequest buildAuthorizeRequest(
            string amount,
            string currency,
            Card cc,
            string pares,
            AuthorizationRequestOptions opts)
        {
            var restRequest = buildRestRequest("authorizations/");

            restRequest.AddParameter("amount", amount);
            restRequest.AddParameter("currency", currency);
            restRequest.AddParameters(cc.GetArgs());

            if (!string.IsNullOrWhiteSpace(pares))
            {
                restRequest.AddParameter("card[pares]", pares);
            }

            if (opts != null)
            {
                restRequest.AddParameters(opts.GetArgs());
            }

            return restRequest;
        }

        private RestRequest buildAuthorizeRequest(
            string amount,
            string currency,
            ApplePayInfo apInfo,
            AuthorizationRequestOptions opts)
        {
            var restRequest = buildRestRequest("authorizations/");

            restRequest.AddParameter("amount", amount);
            restRequest.AddParameter("currency", currency);
            restRequest.AddParameters(apInfo.GetArgs());

            if (opts != null)
            {
                restRequest.AddParameters(opts.GetArgs());
            }

            return restRequest;
        }

        private RestRequest buildAuthorizeRequest(
            string amount,
            string currency,
            MobilePayOnlineInfo mpoInfo,
			string pares,
            AuthorizationRequestOptions opts)
        {
            var restRequest = buildRestRequest("authorizations/");

            restRequest.AddParameter("amount", amount);
            restRequest.AddParameter("currency", currency);
            restRequest.AddParameters(mpoInfo.GetArgs());

            if (!string.IsNullOrWhiteSpace(pares))
            {
                restRequest.AddParameter("mobilepayonline[pares]", pares);
            }

            if (opts != null)
            {
                restRequest.AddParameters(opts.GetArgs());
            }

            return restRequest;
        }

        /// <summary>
        /// Creates an authorization against the Gateway.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#authentication
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="cc">Card to authorize against. <see cref="Clearhaus.Gateway.Card"/> (Required)</param>
        /// <param name="pares">3D-Secure result (omittable)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public Authorization Authorize(string amount, string currency, Card cc, string pares, AuthorizationRequestOptions opts)
        {
            using(var restRequest = buildAuthorizeRequest(amount, currency, cc, pares, opts))
            {
                return POSTtoObject<Authorization>(restRequest);
            }
        }

        /// <summary>
        /// <see cref="Authorize(string, string, Card, string, AuthorizationRequestOptions)"/>
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="cc">Card to authorize against. <see cref="Clearhaus.Gateway.Card"/> (Required)</param>
        /// <param name="pares">3D-Secure result (omittable)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<Authorization> AuthorizeAsync(string amount, string currency, Card cc, string pares, AuthorizationRequestOptions opts)
        {
            using(var restRequest = buildAuthorizeRequest(amount, currency, cc, pares, opts))
            {
                return await POSTtoObjectAsync<Authorization>(restRequest);
            }
        }

        /// <summary>
        /// Creates an authorization against the Gateway.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#authentication
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="apInfo">Apple Pay payment information (Required)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        /// <remarks>Signing must be enabled for this method to function</remarks>
        public Authorization Authorize(string amount, string currency, ApplePayInfo apInfo, AuthorizationRequestOptions opts)
        {
            using(var restRequest = buildAuthorizeRequest(amount, currency, apInfo, opts))
            {
                return POSTtoObject<Authorization>(restRequest);
            }
        }

        /// <summary>
        /// <see cref="Authorize(string, string, ApplePayInfo, AuthorizationRequestOptions)"/>
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="apInfo">Apple Pay payment information (Required)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        /// <remarks>Signing must be enabled for this method to function</remarks>
        async public Task<Authorization> AuthorizeAsync(string amount, string currency, ApplePayInfo apInfo, AuthorizationRequestOptions opts)
        {
            using(var restRequest = buildAuthorizeRequest(amount, currency, apInfo, opts))
            {
                return await POSTtoObjectAsync<Authorization>(restRequest);
            }
        }

        /// <summary>
        /// Creates an authorization against the Gateway.
        /// See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#authentication
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="mpoInfo">MobilePay Online payment information (Required)</param>
        /// <param name="pares">3D-Secure result (omittable)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        /// <remarks>Signing must be enabled for this method to function</remarks>
        public Authorization Authorize(string amount, string currency, MobilePayOnlineInfo mpoInfo, string pares, AuthorizationRequestOptions opts)
        {
            using(var restRequest = buildAuthorizeRequest(amount, currency, mpoInfo, pares, opts))
            {
                return POSTtoObject<Authorization>(restRequest);
            }
        }

        /// <summary>
        /// <see cref="Authorize(string, string, MobilePayOnlineInfo, string, AuthorizationRequestOptions)"/>
        /// </summary>
        /// <param name="amount">Amount of money to reserve, minor units of <c>currency</c> (Required)</param>
        /// <param name="currency">Currency in which <c>amount</c> is specified (Required)</param>
        /// <param name="mpoInfo">MobilePay Online payment information (Required)</param>
        /// <param name="pares">3D-Secure result (omittable)</param>
        /// <param name="opts">Optional parameters for authorizations or null (Omittable)</param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        /// <remarks>Signing must be enabled for this method to function</remarks>
        async public Task<Authorization> AuthorizeAsync(string amount, string currency, MobilePayOnlineInfo mpoInfo, string pares, AuthorizationRequestOptions opts)
        {
            using(var restRequest = buildAuthorizeRequest(amount, currency, mpoInfo, pares, opts))
            {
                return await POSTtoObjectAsync<Authorization>(restRequest);
            }
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
            using(var restRequest = buildRestRequest("authorizations/{0}/voids", authorizationID))
            {
                return POSTtoObject<Transaction.Void>(restRequest);
            }
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
            using(var restRequest = buildRestRequest("authorizations/{0}/voids", authorizationID))
            {
                return await POSTtoObjectAsync<Transaction.Void>(restRequest);
            }
        }

        /*
         * CAPTURE IMPLEMENTATION
         */

        private RestRequest buildCaptureRequest(string id, string amount, string textOnStatement)
        {
            // Don't dispose restRequest, let caller do that.
            var restRequest = buildRestRequest("authorizations/{0}/captures", id);

            if (!string.IsNullOrWhiteSpace(textOnStatement))
            {
                restRequest.AddParameter("text_on_statement", textOnStatement);
            }

            if (!string.IsNullOrWhiteSpace(amount))
            {
                restRequest.AddParameter("amount", amount);
            }

            return restRequest;
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
            using(var restRequest = buildCaptureRequest(id, amount, textOnStatement))
            {
                return POSTtoObject<Capture>(restRequest);
            }
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
            using(var restRequest = buildCaptureRequest(id, amount, textOnStatement))
            {
                return await POSTtoObjectAsync<Capture>(restRequest);
            }
        }

        /*
         * REFUND IMPLEMENTATION
         */

        private RestRequest buildRefundRequest(string id, string amount, string textOnStatement)
        {
            // Do not dispose restRequest, let caller handle that.
            var restRequest = buildRestRequest( "authorizations/{0}/refunds", id);
            if (!string.IsNullOrWhiteSpace(amount))
            {
                restRequest.AddParameter("amount", amount);
            }

            if (!string.IsNullOrWhiteSpace(textOnStatement))
            {
                restRequest.AddParameter("text_on_statement", textOnStatement);
            }

            return restRequest;
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
            using(var restRequest = buildRefundRequest(id, amount, textOnStatement))
            {
                return POSTtoObject<Refund>(restRequest);
            }
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
            using(var restRequest = buildRefundRequest(id, amount, textOnStatement))
            {
                return await POSTtoObjectAsync<Refund>(restRequest);
            }
        }

        /*
         * CREDIT IMPLEMENTATION
         */

        /*
         * For API reasons, we still need to tokenize the card before we can
         * perform a credit transaction
         */
        private RestRequest buildTokenizeCardRequest(Card cc)
        {
            // Don't dispose restRequest, let caller handle that
            var restRequest = buildRestRequest("/cards");
            restRequest.AddParameters(cc.GetArgs());

            return restRequest;
        }

        private TokenizedCard TokenizeCard(Card cc)
        {
            using(var restRequest = buildTokenizeCardRequest(cc))
            {
                return POSTtoObject<TokenizedCard>(restRequest);
            }
        }

        async private Task<TokenizedCard> TokenizeCardAsync(Card cc)
        {
            using(var restRequest = buildTokenizeCardRequest(cc))
            {
                return await POSTtoObjectAsync<TokenizedCard>(restRequest);
            }
        }

        private RestRequest buildCreditRequest(string amount, string currency, TokenizedCard cc, string textOnStatement, string reference)
        {

            // Do not dispose restRequest, let caller handle that.
            var restRequest = buildRestRequest("cards/{0}/credits", cc.id);
            restRequest.AddParameter("id", cc.id);
            restRequest.AddParameter("amount", amount);
            restRequest.AddParameter("currency", currency);

            if (!string.IsNullOrWhiteSpace(textOnStatement))
            {
                restRequest.AddParameter("text_on_statement", textOnStatement);
            }

            if (!string.IsNullOrWhiteSpace(reference))
            {
                restRequest.AddParameter("reference", reference);
            }

            return restRequest;
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

            using(var restRequest = buildCreditRequest(amount, currency, tokCard, textOnStatement, reference))
            {
                return POSTtoObject<Credit>(restRequest);
            }
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
            var tokCard = await TokenizeCardAsync(cc);

            using(var restRequest = buildCreditRequest(amount, currency, tokCard, textOnStatement, reference))
            {
                return await POSTtoObjectAsync<Credit>(restRequest);
            }
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


        /**** IDisposable implementation ****/

        /// <summary>IDisposable Interface</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>IDisposable Interface</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) {
                return;
            }

            if (disposing) {
            }

            if (httpClient != null) { httpClient.Dispose(); }

            disposed = true;
        }

        /// <summary>Disposes unmanaged objects.</summary>
        ~Account()
        {
            Dispose(false);
        }
    }
}
