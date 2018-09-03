using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clearhaus.Util
{
    /// <summary>
    /// Helper class for building rest request helper.
    /// </summary>
    public class RestRequest: IDisposable
    {
        private bool disposed;

        private IList<KeyValuePair<string, string>> bodyParameters;
        private string url;
        private FormUrlEncodedContent content;
        public HttpClient client;
        private HttpClientHandler clientHandler;
        private bool responsibleForClient = false;

        private static HttpStatusCode[] acceptedResponseCodes = new HttpStatusCode[]{
            HttpStatusCode.OK,
            HttpStatusCode.Created,
            HttpStatusCode.BadRequest,
        };

        public RestRequest(HttpClient client)
        {
            bodyParameters = new List<KeyValuePair<string, string>>();
            this.client = client;
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
            }

            if (responsibleForClient && client != null) { client.Dispose(); }
            if (content != null) { content.Dispose(); }

            disposed = true;
        }

        ~RestRequest()
        {
            Dispose(false);
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

        public void AddHeader(string key, string val)
        {
            content.Headers.Add(key, val);
        }

        public HttpResponseMessage POST()
        {
            if (content == null)
            {
                content = new FormUrlEncodedContent(bodyParameters);
            }

            HttpResponseMessage response;
            try
            {
                response = client.PostAsync(url, content).Result;
            }
            catch(AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is HttpRequestException)
                    {
                       throw new ClrhsNetException(x.Message, x);
                    }

                    return false;
                });

                throw new ClrhsException(ae.Message, ae);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ClrhsAuthException("Invalid API key");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ClrhsGatewayException("The remote server responded with InternalServerError");
            }

            if (!Array.Exists(acceptedResponseCodes, code => code == response.StatusCode))
            {
                throw new ClrhsException("Invalid response code: " + response.StatusCode.ToString());
            }

            return response;
        }

        async public Task<HttpResponseMessage> POSTAsync()
        {
            if (content == null)
            {
                content = new FormUrlEncodedContent(bodyParameters);
            }
            HttpResponseMessage response;
            try
            {
                var t = client.PostAsync(url, content);
                response = await t;
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

            if (!Array.Exists(acceptedResponseCodes, code => code == response.StatusCode))
            {
                throw new ClrhsException("Invalid response code: " + response.StatusCode.ToString());
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
            catch(AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is HttpRequestException)
                    {
                       throw new ClrhsNetException(x.Message, x);
                    }

                    return false;
                });

                throw new ClrhsException(ae.Message, ae);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ClrhsAuthException("Invalid API key");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ClrhsGatewayException("The remote server responded with InternalServerError");
            }

            if (!Array.Exists(acceptedResponseCodes, code => code == response.StatusCode))
            {
                throw new ClrhsException("Invalid response code: " + response.StatusCode.ToString());
            }

            return response;
        }

        async public Task<HttpResponseMessage> GETAsync()
        {
            HttpResponseMessage response;
            try
            {
                var t = client.GetAsync(url);
                response = await t;
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

            if (!Array.Exists(acceptedResponseCodes, code => code == response.StatusCode))
            {
                throw new ClrhsException("Invalid response code: " + response.StatusCode.ToString());
            }

            return response;
        }

        public byte[] Body()
        {
            if (content == null)
            {
                content = new FormUrlEncodedContent(bodyParameters);
            }
            var task = content.ReadAsByteArrayAsync();
            return task.Result;
        }
    }
}
