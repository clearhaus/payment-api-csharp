using System;

namespace Clearhaus.Gateway.Test
{
    public static class Util
    {
        public static Card GetStagingCard()
        {
            // This is not a valid card.
            return new Card("4111111111111111", "12", "2020", "584");
        }

        public static string GetPrivateKey()
        {
            var signingKey = Environment.GetEnvironmentVariable("RSAKEY");

            if (string.IsNullOrWhiteSpace(signingKey))
            {
                throw new System.ApplicationException("No RSAKEY in environment");
            }

            return signingKey;
        }

        public static string GetValidAPIKey()
        {
            var apiKey = System.Environment.GetEnvironmentVariable("APIKEY");

            if (String.IsNullOrWhiteSpace(apiKey)) {
                throw new System.ApplicationException("No APIKey found in environment");
            }

            return apiKey;
        }

        public static Gateway.Account GetStagingAccount()
        {
            var account = new Gateway.Account(Util.GetValidAPIKey(), Constants.GatewayTestURL);

            return account;
        }

        public static Gateway.Account GetSigningStagingAccount()
        {
            var account = new Gateway.Account(Util.GetValidAPIKey(), Constants.GatewayTestURL);
            account.SigningKeys(GetValidAPIKey(), GetPrivateKey());

            return account;
        }
    }
}
