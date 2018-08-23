using System;

namespace Clearhaus
{
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
    }

    public class ClrhsGatewayException : ClrhsException
    {
        public ClrhsGatewayException() : base() {}
        public ClrhsGatewayException(string msg) : base(msg) {}
    }

    public class ClrhsNetException : ClrhsException
    {
        public ClrhsNetException() : base() {}
        public ClrhsNetException(string msg) : base(msg) {}
        public ClrhsNetException(string msg, Exception exc) : base(msg, exc) {}
    }
}
