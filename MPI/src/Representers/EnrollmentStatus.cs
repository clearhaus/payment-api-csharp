using Newtonsoft.Json;

namespace Clearhaus.MPI.Representers
{
    public class EnrollmentStatus
    {
        [JsonProperty("acs_url")]
        public string acsUrl;
        public string pareq;
        public string enrolled;
        public string eci;
        public string error;
    }
}
