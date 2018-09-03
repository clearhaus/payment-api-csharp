using System;
using System.Net.Http;
using System.Threading.Tasks;

using Clearhaus.Util;
using Clearhaus.MPI.Builder;
using Clearhaus.MPI.Representers;

using Newtonsoft.Json;

namespace Clearhaus.MPI
{
    /// <summary>
    /// MPI is used adding 3D-Secure to a payment transaction flow. Is uses https://3dsecure.io as an MPI service.
    /// </summary>
    /// <example>
    /// <code lang="C#">
    /// using Clearhaus.MPI;
    /// using Clearhaus.MPI.Builder;
    ///
    /// public static void main()
    /// {
    ///         string apiKey = "SOME UUID APIKEY";
    ///
    ///         // Can be disposed by `#Dispose()` or will be GC'd automatically.
    ///         var mpiAccount = new MPI(apiKey);
    ///
    ///         var builder = new EnrollCheckBuilder {
    ///             amount              = "100",
    ///             currency            = "DKK",
    ///             orderID             = "SOME ID",
    ///             cardholderIP        = "1.1.1.1",
    ///             cardNumber          = "SOME PAN",
    ///             cardExpireMonth     = "04",
    ///             cardExpireYear      = "2030",
    ///             merchantAcquirerBin = "SOME BIN",
    ///             merchantCountry     = "DK",
    ///             merchantID          = "SOME ID",
    ///             merchantName        = "MyMerchant",
    ///             merchantUrl         = "http://mymerchant.com"
    ///         };
    ///
    ///         EnrollmentStatus response;
    ///         try
    ///         {
    ///             response = mpiAccount.EnrollCheck(builder);
    ///         }
    ///         catch(ClrhsNetException e)
    ///         {
    ///             // Handle
    ///         }
    ///         catch(ClrhsGatewayException e)
    ///         {
    ///             // Something is wrong on server-side
    ///         }
    ///         catch(ClrhsAuthException e)
    ///         {
    ///             // Invalid APIKey. This should not happen if you have tested your
    ///             // key.
    ///         }
    ///         catch(ClrhsException e)
    ///         {
    ///             // Last effort exception
    ///         }
    ///
    ///         if (response.enrolled == "Y")
    ///         {
    ///             // Continue 3D-Secure procedure
    ///         }
    /// }
    /// </code>
    /// </example>
    public class MPI
    {
        private string apiKey;
        private Uri mpiUrl;

        private HttpClient httpClient;

        private bool disposed;

        /// <summary>The default timespan used for HttpClient, 40s)</summary>
        public readonly TimeSpan timeout = new TimeSpan(0, 0, 40);

        /// <summary>
        /// Temporary documentation
        /// </summary>
        /// <param name="apiKey">UUID representing your 3dsecure.io account</param>
        public MPI(string apiKey)
        {
            this.apiKey = apiKey;
            this.mpiUrl = new Uri(Constants.MPIURL);

            InitializeHttpClient();
        }


        /**** CONSTRUCTORS ****/


        /// <summary>
        /// Temporary documentation
        /// </summary>
        /// <param name="apiKey">UUID representing your 3dsecure.io account</param>
        /// <param name="mpiUrl">URL to use as API mpiUrl</param>
        /// <exception cref="System.ArgumentNullException">If mpiUrl is null</exception>
        /// <exception cref="System.UriFormatException">If mpiUrl is invalid URI</exception>
        public MPI(string apiKey, string mpiUrl)
        {
            this.apiKey = apiKey;
            this.mpiUrl = new Uri(mpiUrl);

            InitializeHttpClient();
        }

        /// <summary>
        /// Override the default 3DSecure mpiUrl.
        ///
        /// <see cref="Constants.MPIURL"/>
        /// </summary>
        /// <param name="mpiUrl">
        /// URL to use as mpiUrl
        /// </param>
        public void SetEndpoint(string mpiUrl)
        {
            this.mpiUrl = new Uri(mpiUrl);

            InitializeHttpClient();
        }


        /**** PRIVATE METHODS ****/


        private void InitializeHttpClient()
        {
            var clientHandler = new HttpClientHandler {
                Credentials = new System.Net.NetworkCredential(this.apiKey, "")
            };

            // Tell it to dispose the HttpClientHandler, so we don't need to.
            this.httpClient = new HttpClient(clientHandler, true) {
                BaseAddress = this.mpiUrl,
            };

            if (this.timeout != null)
            {
                httpClient.Timeout = this.timeout;
            }
        }

        /**** PUBLIC METHODS ****/

        /// <summary>
        /// Query the MPI service, returning <c>PARes</c> and <c>ACSUrl</c> to allow continuing the 3DS flow.
        /// </summary>
        /// <param name="builder">
        /// The information associated with the 3D-Secure flow
        /// </param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public EnrollmentStatus EnrollCheck(EnrollCheckBuilder builder)
        {
            using(var restRequest = new RestRequest(this.httpClient))
            {
                restRequest.SetPath("/enrolled");

                restRequest.AddParameters(builder.GetArgs());

                var response = restRequest.POST();

                var body = response.Content.ReadAsStringAsync().Result;
                var status = JsonConvert.DeserializeObject<EnrollmentStatus>(body);

                return status;
            }
        }

        /// <summary>
        /// Query the MPI service, returning <c>PARes</c> and <c>ACSUrl</c> to allow continuing the 3DS flow.
        /// </summary>
        /// <param name="builder">
        /// The information associated with the 3D-Secure flow
        /// </param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<EnrollmentStatus> EnrollCheckAsync(EnrollCheckBuilder builder)
        {
            using(var restRequest = new RestRequest(this.httpClient))
            {
                restRequest.SetPath("/enrolled");

                restRequest.AddParameters(builder.GetArgs());

                var response = await restRequest.POSTAsync();

                var body = response.Content.ReadAsStringAsync().Result;
                var status = JsonConvert.DeserializeObject<EnrollmentStatus>(body);

                return status;
            }
        }

        /// <summary>
        /// Checks the <c>PARes</c>, returning results.
        /// </summary>
        /// <param name="pares">
        /// The <c>PARes</c> (possibly) returned from the EnrollCheck call.
        /// </param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        public CheckResponse CheckPARes(string pares)
        {
            using(var restRequest = new RestRequest(this.httpClient))
            {
                restRequest.SetPath("/check");
                restRequest.AddParameter("pares", pares);

                var response = restRequest.POST();

                var body = response.Content.ReadAsStringAsync().Result;
                var parsedResponse = JsonConvert.DeserializeObject<CheckResponse>(body);

                return parsedResponse;
            }
        }

        /// <summary>
        /// Checks the <c>PARes</c>, returning results.
        /// </summary>
        /// <param name="pares">
        /// The <c>PARes</c> (possibly) returned from the EnrollCheck call.
        /// </param>
        /// <exception cref="ClrhsNetException">Network error communicating with gateway</exception>
        /// <exception cref="ClrhsAuthException">Thrown if APIKey is invalid</exception>
        /// <exception cref="ClrhsGatewayException">Thrown if gateway responds with internal server error</exception>
        /// <exception cref="ClrhsException">Unexpected connection error</exception>
        async public Task<CheckResponse> CheckPAResAsync(string pares)
        {
            using(var restRequest = new RestRequest(this.httpClient))
            {
                restRequest.SetPath("/check");
                restRequest.AddParameter("pares", pares);

                var response = await restRequest.POSTAsync();
                var body = response.Content.ReadAsStringAsync().Result;
                var parsedResponse = JsonConvert.DeserializeObject<CheckResponse>(body);

                return parsedResponse;
            }
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
        ~MPI()
        {
            Dispose(false);
        }
    }
}
