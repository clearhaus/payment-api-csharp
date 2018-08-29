using System;
using System.Threading.Tasks;

using Clearhaus.Util;
using Clearhaus.MPI.Builder;
using Clearhaus.MPI.Representers;

using Newtonsoft.Json;

namespace Clearhaus.MPI
{
    /// <summary>
    /// MPI is used adding 3D-Secure to a payment transaction flow. Is uses https://3dsecure.io as a MPI service.
    /// </summary>
    /// <example>
    /// <code lang="C#">
    /// using Clearhaus.MPI;
    /// using Clearhaus.MPI.Builder;
    /// using Clearhaus.MPI.Representers;
    ///
    /// string apiKey = "SOME UUID APIKEY";
    ///
    /// var mpiAccount = new MPI(apiKey);
    ///
    /// var builder = new EnrollCheckBuilder {
    ///     amount              = "100",
    ///     currency            = "DKK",
    ///     orderID             = "SOME ID",
    ///     cardholderIP        = "1.1.1.1",
    ///     cardNumber          = "SOME PAN",
    ///     cardExpireMonth     = "04",
    ///     cardExpireYear      = "2030",
    ///     merchantAcquirerBin = "SOME BIN",
    ///     merchantCountry     = "DK",
    ///     merchantID          = "SOME ID",
    ///     merchantName        = "MyMerchant",
    ///     merchantUrl         = "http://mymerchant.com"
    /// };
    ///
    /// EnrollmentStatus response;
    /// try
    /// {
    ///     response = mpiAccount.EnrollCheck(builder);
    /// }
    /// catch(ClrhsNetException e)
    /// {
    ///     // Handle
    /// }
    /// catch(ClrhsGatewayException e)
    /// {
    ///     // Something is wrong on server-side
    /// }
    /// catch(ClrhsAuthException e)
    /// {
    ///     // Invalid APIKey. This should not happen if you have tested your
    ///     // key.
    /// }
    ///
    /// if (response.enrolled == "Y")
    /// {
    ///     // Continue 3D-Secure procedure
    /// }
    /// </code>
    /// </example>
    public class MPI
    {
        private string apikey;
        private Uri mpiUrl;

        /// <summary>
        /// Temporary documentation
        /// </summary>
        /// <param name="apikey">UUID representing your 3dsecure.io account</param>
        public MPI(string apikey)
        {
            this.apikey = apikey;
            this.mpiUrl = new Uri(Constants.MPIURL);
        }

        /// <summary>
        /// Temporary documentation
        /// </summary>
        /// <param name="apikey">UUID representing your 3dsecure.io account</param>
        /// <param name="mpiUrl">URL to use as API mpiUrl</param>
        /// <exception cref="System.ArgumentNullException">If mpiUrl is null</exception>
        /// <exception cref="System.UriFormatException">If mpiUrl is invalid URI</exception>
        public MPI(string apikey, string mpiUrl)
        {
            this.apikey = apikey;
            this.mpiUrl = new Uri(mpiUrl);
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
        public EnrollmentStatus EnrollCheck(EnrollCheckBuilder builder)
        {
            var rrb = new RestRequestBuilder(this.mpiUrl, this.apikey, "");
            rrb.SetPath("/enrolled");

            rrb.AddParameters(builder.GetArgs());

            var rr = rrb.Ready();

            var response = rr.POST();

            var body = response.Content.ReadAsStringAsync().Result;
            var status = JsonConvert.DeserializeObject<EnrollmentStatus>(body);

            return status;
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
            var rrb = new RestRequestBuilder(this.mpiUrl, this.apikey, "");
            rrb.SetPath("/enrolled");

            rrb.AddParameters(builder.GetArgs());

            var rr = rrb.Ready();

            var responseTask = rr.POSTAsync();
            var response = await responseTask;

            var body = response.Content.ReadAsStringAsync().Result;
            var status = JsonConvert.DeserializeObject<EnrollmentStatus>(body);

            return status;
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
            var rrb = new RestRequestBuilder(this.mpiUrl, this.apikey, "");
            rrb.SetPath("/check");
            rrb.AddParameter("pares", pares);

            var rr = rrb.Ready();

            var response = rr.POST();

            var body = response.Content.ReadAsStringAsync().Result;
            var parsedResponse = JsonConvert.DeserializeObject<CheckResponse>(body);

            return parsedResponse;
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
            var rrb = new RestRequestBuilder(this.mpiUrl, this.apikey, "");
            rrb.SetPath("/check");
            rrb.AddParameter("pares", pares);

            var rr = rrb.Ready();

            var responseTask = rr.POSTAsync();
            var response = await responseTask;
            var body = response.Content.ReadAsStringAsync().Result;
            var parsedResponse = JsonConvert.DeserializeObject<CheckResponse>(body);

            return parsedResponse;
        }
    }
}
