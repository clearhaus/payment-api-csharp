using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;

namespace Clearhaus.Util
{
    /// <summary>
    /// Helper class for building rest request helper.
    /// </summary>
    public class RestRequestBuilder
    {
        private IList<KeyValuePair<string, string>> bodyParameters;
        private string url;
        private FormUrlEncodedContent content;
        public HttpClient client;
        private HttpClientHandler clientHandler;

        // Support username/password is for Http Basic auth
        public RestRequestBuilder(Uri urlbase, string username, string password)
        {
            bodyParameters = new List<KeyValuePair<string, string>>();

            clientHandler = new HttpClientHandler {
                Credentials = new System.Net.NetworkCredential(username, password)
            };

            client = new HttpClient(clientHandler) {
                BaseAddress = urlbase,
            };
        }

        public void SetPath(string url, params string[] list)
        {
            this.url = string.Format(url, list);
        }

        public void AddParameter(string key, string val)
        {
            bodyParameters.Add(new KeyValuePair<string, string>(key, val));
        }

        public void AddParameters(IList<KeyValuePair<string, string>> l)
        {
            foreach (var kv in l)
            {
                bodyParameters.Add(kv);
            }
        }

        public RestRequest Ready()
        {
            content = new FormUrlEncodedContent(bodyParameters);
            return new RestRequest(client, content, url);
        }
    }

    /// Help class for building rest requests.
    public class RestRequest : IDisposable
    {
        private string url;
        private HttpContent content;
        private HttpClient client;

        private bool disposed;

        public RestRequest(HttpClient client, HttpContent content , string url)
        {
            this.client = client;

            this.content = content;
            this.url = url;
        }

        public void AddHeader(string key, string val)
        {
            content.Headers.Add(key, val);
        }

        public HttpResponseMessage POST()
        {
            HttpResponseMessage response;
            try
            {
                response = client.PostAsync(url, content).Result;
            }
            catch(HttpRequestException e)
            {
                throw new ClrhsNetException(e.Message, e);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ClrhsAuthException("Invalid API key");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ClrhsGatewayException("The remote server responded with InternalServerError");
            }

            return response;
        }

        public HttpResponseMessage GET()
        {
            HttpResponseMessage response;
            try
            {
                response = client.GetAsync(url).Result;
            }
            catch(HttpRequestException e)
            {
                throw new ClrhsNetException(e.Message, e);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ClrhsAuthException("Invalid API key");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ClrhsGatewayException("The remote server responded with InternalServerError");
            }

            return response;
        }

        public byte[] Body()
        {
            var task = content.ReadAsByteArrayAsync();
            return task.Result;
        }

        /**
         * Implement IDispose interface
         **/
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) {
                return;
            }

            if (disposing) {
                client.Dispose();
                content.Dispose();
            }

            disposed = true;
        }
    }
}

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
