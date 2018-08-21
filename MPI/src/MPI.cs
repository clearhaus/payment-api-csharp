using System;
using System.Net;

using Clearhaus.Util;
using Clearhaus.MPI.Builder;
using Clearhaus.MPI.Representers;

using Newtonsoft.Json;

namespace Clearhaus.MPI
{
    /// <summary>
    /// MPI is used adding 3D-Secure to a payment transaction flow. Is uses https://3dsecure.io as a MPI service.
    /// </summary>
    public class MPI
    {
        private string apikey;
        private Uri endpoint;

        /// <summary>
        /// Temporary documentation
        /// </summary>
        /// <param name="apikey">UUID representing your 3dsecure.io account</param>
        public MPI(string apikey)
        {
            this.apikey = apikey;
            this.endpoint = new Uri(Constants.MPIURL);
        }

        /// <summary>
        /// Override the default 3DSecure endpoint.
        ///
        /// <see cref="Constants.MPIURL"/>
        /// </summary>
        /// <param name="endpoint">
        /// URL to use as endpoint
        /// </param>
        public void SetEndpoint(string endpoint)
        {
            this.endpoint = new Uri(endpoint);
        }

        /// <summary>
        /// Query the MPI service, returning <c>PARes</c> and <c>ACSUrl</c> to allow continuing the 3DS flow.
        /// </summary>
        /// <param name="builder">
        /// The information associated with the 3D-Secure flow
        /// </param>
        public EnrollmentStatus EnrollCheck(EnrollCheckBuilder builder)
        {
            var rrb = new RestRequestBuilder(this.endpoint, this.apikey, "");
            rrb.SetPath("/enrolled");

            rrb.AddParameters(builder.GetArgs());

            var rr = rrb.Ready();

            var response = rr.POST();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                throw new Exception(String.Format("Bad statuscode: {0}", response.StatusCode.ToString()));
            }

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
        public CheckResponse CheckPARes(string pares)
        {
            var rrb = new RestRequestBuilder(this.endpoint, this.apikey, "");
            rrb.SetPath("/check");
            rrb.AddParameter("pares", pares);

            var rr = rrb.Ready();

            var response = rr.POST();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Bad statuscode: {0}", response.StatusCode.ToString()));
            }

            var body = response.Content.ReadAsStringAsync().Result;
            var parsedResponse = JsonConvert.DeserializeObject<CheckResponse>(body);

            return parsedResponse;
        }
    }
}
