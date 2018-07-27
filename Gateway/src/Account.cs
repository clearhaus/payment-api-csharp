using System;
using System.Net;

using RestSharp;
using RestSharp.Authenticators;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Clearhaus.Gateway
{
    /*
     * Class representing an account for the Clearhaus Gateway, identified
     * by an apiKey
     */
    public class Account
    {
        internal string apiKey;

        private HttpStatusCode[] httpSuccessCodes = {
            HttpStatusCode.OK,
            HttpStatusCode.Created
        };

        public Account(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Connects to the Gateway attempts to authorize with the apiKey.
        /// </summary>
        /// <exception cref="Clearhaus.Gateway.ClrhsNetException">
        /// Thrown if connection to the gateway fails.
        /// </exception>
        public bool ValidAPIKey()
        {
            var request = new RestRequest();
            try
            {
                var restClient = new RestClient {
                    BaseUrl = new Uri(Constants.GatewayTestURL),
                    Authenticator = new HttpBasicAuthenticator(this.apiKey, "")
                };
                var response = restClient.Execute(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized) {
                    return false;
                } else if(response.StatusCode != HttpStatusCode.OK) {
                }
            } catch (Exception e)
            {
                throw new ClrhsNetException("Error connecting to gateway", e);
            }

            return true;
        }

        public AccountInfo FetchAccountInformation()
        {
            var request = new RestRequest
            {
                Method = RestSharp.Method.GET,
                Resource = "account/"
            };

            return performRestRequest<AccountInfo>(request);
        }

        private T performRestRequest<T>(RestRequest req)
        {
            var restClient = new RestClient {
                BaseUrl = new Uri(Constants.GatewayTestURL),
                Authenticator = new HttpBasicAuthenticator(this.apiKey, "")
            };

            var response = restClient.Execute(req);

            var o = JsonConvert.DeserializeObject<T>(response.Content);

            return o;
        }

        public Transaction.Authorization NewAuthorization(string amount, string currency, Card cc)
        {
            return NewAuthorization(amount, currency, cc, "");
        }

        public Transaction.Authorization NewAuthorization(string amount, string currency, Card cc, string ip)
        {
            var request = new RestRequest
            {
                Method = RestSharp.Method.POST,
                Resource = "authorizations/"
            };

            request.AddParameter("amount", amount);
            request.AddParameter("currency", currency);
            if (!string.IsNullOrEmpty(ip))
            {
                request.AddParameter("ip", ip);
            }
            request.AddParameter("card[pan]", cc.pan);
            request.AddParameter("card[csc]", cc.csc);
            request.AddParameter("card[expire_month]", cc.expireMonth);
            request.AddParameter("card[expire_year]", cc.expireYear);

            return performRestRequest<Transaction.Authorization>(request);
        }

        public Transaction.Void Void(Transaction.Authorization auth)
        {
            return Void(auth.id);
        }

        public Transaction.Void Void(string authorizationID)
        {
            var request = new RestRequest
            {
                Method   = Method.POST,
                Resource = "authorizations/{id}/voids"
            };

            request.AddParameter("id", authorizationID, ParameterType.UrlSegment);

            return performRestRequest<Transaction.Void>(request);
        }

        public Transaction.Capture Capture(string id)
        {
            return Capture(id, "");
        }

        public Transaction.Capture Capture(Transaction.Authorization auth)
        {
            return Capture(auth.id, "");
        }

        public Transaction.Capture Capture(Transaction.Authorization auth, string amount)
        {
            return Capture(auth.id, amount);
        }

        public Transaction.Capture Capture(string id, string amount)
        {
            var request = new RestRequest{
                Method   = Method.POST,
                Resource = "authorizations/{id}/captures"
            };

            request.AddParameter("id", id, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(amount))
            {
                request.AddParameter("amount", amount);
            }

            return performRestRequest<Transaction.Capture>(request);
        }

        public Transaction.Refund Refund(string id, string amount)
        {
            var request = new RestRequest{
                Method   = Method.POST,
                Resource = "authorizations/{id}/refunds"
            };

            request.AddParameter("id", id, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(amount))
            {
                request.AddParameter("amount", amount);
            }

            return performRestRequest<Transaction.Refund>(request);
        }
    }

    public class Card
    {
        public Card(string pan, string expireMonth, string expireYear, string csc)
        {
            this.pan = pan;
            this.expireMonth = expireMonth;
            this.expireYear  = expireYear;
            this.csc         = csc;
        }

        public string pan;
        public string expireMonth;
        public string expireYear;
        public string csc;
    }

    public class AccountInfo
    {
        [JsonProperty("merchant_id")]
        public string merchantID { get; set; }
        public string descriptor { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int mcc { get; set; }

        public Acquirer acquirer {get; set;}

        [JsonProperty("transaction_rules")]
        public string transactionRules { get; set; }
    }

    public class Acquirer
    {
        [JsonProperty("visa_bin")]
        public string visaBin {get; set;}

        [JsonProperty("mastercard_bin")]
        public string mastercardBin {get; set;}
    }
}
