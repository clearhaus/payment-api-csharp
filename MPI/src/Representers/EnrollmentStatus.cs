using Newtonsoft.Json;

namespace Clearhaus.MPI.Representers
{
    /// <summary>Response of EnrollCheck request</summary>
    public class EnrollmentStatus
    {
        /// <summary>URL of ACS</summary>
        [JsonProperty("acs_url")]
        public string acsUrl;

        /// <summary><c>PAReq</c>to forward to <c>acsUrl</c></summary>
        public string pareq;

        /// <summary>Whether or not card is enrolled for 3D-Secure</summary>
        public string enrolled;

        /// <summary>Status as ECI</summary>
        public string eci;

        /// <summary>Any errors</summary>
        public string error;
    }
}
