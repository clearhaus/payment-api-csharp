using System;

namespace Clearhaus.Gateway
{
    public static class Constants
    {
        public const string GatewayURL = "https://gateway.clearhaus.com";
        public const string GatewayTestURL = "https://gateway.test.clearhaus.com";
    }

    public class ClrhsException : Exception
    {
        public ClrhsException() : base() {}
        public ClrhsException(string msg) : base(msg) {}
        public ClrhsException(string msg, Exception exc) : base(msg, exc) {}
    }

    public class ClrhsAuthException : ClrhsException
    {
        public ClrhsAuthException() : base() {}
        public ClrhsAuthException(string msg) : base(msg) {}
        public ClrhsAuthException(string msg, Exception exc) : base(msg, exc) {}
    }

    public class ClrhsNetException : ClrhsException
    {
        public ClrhsNetException() : base() {}
        public ClrhsNetException(string msg) : base(msg) {}
        public ClrhsNetException(string msg, Exception exc) : base(msg, exc) {}
    }
}
