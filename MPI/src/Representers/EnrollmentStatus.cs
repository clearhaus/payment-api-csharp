using Newtonsoft.Json;

namespace Clearhaus.MPI.Representers
{
    /// <summary>Error class for  the MPI service</summary>
    public class Error
    {
        /// <summary>No information</summary>
        public string detail;

        /// <summary>Error message</summary>
        public string message;
    }

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
        public Error error;
    }
}
