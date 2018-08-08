using System;
using System.Net;
using System.Text;

using Clearhaus.Util;
using Clearhaus.MPI.Builder;
using Clearhaus.MPI.Representers;

using Newtonsoft.Json;

namespace Clearhaus.MPI
{
    public class MPI
    {
        private string apikey;
        private Uri endpoint;

        /// <summary>
        /// Temporary documentation
        /// </summary>
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
        public void SetEndpoint(string endpoint)
        {
            this.endpoint = new Uri(endpoint);
        }

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
