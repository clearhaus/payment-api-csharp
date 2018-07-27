using System;
using Xunit;
using Clearhaus;
using Clearhaus.Gateway;
using Clearhaus.Gateway.Transaction;

namespace Clearhaus.Gateway.Test
{
    public class GatewayTest
    {
        public Card getStagingCard()
        {
            return new Card("4111111111111111", "12", "2020", "584");
        }

        public string getValidAPIKey()
        {
            var apiKey = System.Environment.GetEnvironmentVariable("APIKEY");

            if (apiKey == null) {
                throw new System.ApplicationException("No APIKey found in environment");
            }

            return apiKey;
        }

        [Fact]
        public void ItValidatesAPIKey()
        {
            var account = new Gateway.Account(getValidAPIKey());
            account.gatewayURL = Constants.GatewayTestURL;
            Assert.True(account.ValidAPIKey());
        }

        [Fact]
        public void ItFetchesAccountInformation()
        {
            var account = new Gateway.Account(getValidAPIKey());
            account.gatewayURL = Constants.GatewayTestURL;

            var info = account.FetchAccountInformation();

            Assert.NotNull(info);
        }

        [Fact]
        public void ItCreatesAuthorization()
        {
            var account = new Gateway.Account(getValidAPIKey());
            account.gatewayURL = Constants.GatewayTestURL;
            var card = getStagingCard();

            var auth = account.NewAuthorization("100", "DKK", card);

            Assert.NotNull(auth);
            Assert.True(auth.isSuccess());
        }

        [Fact]
        public void ItVoidsAuthorization()
        {
            var account = new Gateway.Account(getValidAPIKey());
            account.gatewayURL = Constants.GatewayTestURL;
            var card = getStagingCard();

            var auth = account.NewAuthorization("100", "DKK", card);

            var v = account.Void(auth);

            Assert.True(v.isSuccess());
        }

        [Fact]
        public void ItCapturesAuthorization()
        {
            var account = new Gateway.Account(getValidAPIKey());
            account.gatewayURL = Constants.GatewayTestURL;
            var card = getStagingCard();

            var auth = account.NewAuthorization("100", "DKK", card);

            var capture = account.Capture(auth, "100");

            Assert.True(capture.isSuccess());
        }

        [Fact]
        public void ItRefundsAuthorization()
        {
            var account = new Gateway.Account(getValidAPIKey());
            account.gatewayURL = Constants.GatewayTestURL;
            var card = getStagingCard();

            var auth = account.NewAuthorization("100", "DKK", card);
            account.Capture(auth, "100");

            var refund = account.Refund(auth.id, "100");

            Assert.True(refund.isSuccess());
        }
    }
}
