using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Clearhaus.Gateway.Util
{
    /// <summary>
    /// Helper class for building rest request helper.
    /// </summary>
    public class RestRequestBuilder
    {
        private IList<KeyValuePair<string, string>> bodyParameters;
        private string url;
        private FormUrlEncodedContent content;
        private HttpClient client;
        private HttpClientHandler clientHandler;

        public RestRequestBuilder(Uri urlbase, string username, string password)
        {
            bodyParameters = new List<KeyValuePair<string, string>>();

            clientHandler = new HttpClientHandler {
                Credentials = new System.Net.NetworkCredential(username, password)
            };

            client = new HttpClient(clientHandler) {
                BaseAddress = urlbase
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
    public class RestRequest
    {
        private string url;
        private HttpContent content;
        private HttpClient client;

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
            } catch(HttpRequestException e)
            {
                throw new ClrhsNetException(e.Message, e);
            }

            return response;
        }

        public HttpResponseMessage GET()
        {
            HttpResponseMessage response;
            try
            {
                response = client.GetAsync(url).Result;
            } catch(HttpRequestException e)
            {
                throw new ClrhsNetException(e.Message, e);
            }

            return response;
        }

        public byte[] Body()
        {
            var task = content.ReadAsByteArrayAsync();
            return task.Result;
        }
    }
}
