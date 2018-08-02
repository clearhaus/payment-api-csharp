using System;
using Xunit;

namespace Clearhaus.Gateway.Test
{
    public class FunctionTests
    {
        private Gateway.Account account;

        public FunctionTests()
        {
            account = new Gateway.Account(getValidAPIKey());
            account.gatewayURL = Constants.GatewayTestURL;
        }

        private string getPrivateKey()
        {
            var signingKey = Environment.GetEnvironmentVariable("RSAKEY");

            if (signingKey == null)
            {
                throw new System.ApplicationException("No RSAKEY in environment");
            }

            return signingKey;
        }

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
            Assert.True(account.ValidAPIKey());
        }

        [Fact]
        public void ItFetchesAccountInformation()
        {
            var info = account.FetchAccountInformation();

            Assert.NotNull(info);
        }

        [Fact]
        public void ItCreatesAuthorization()
        {
            var card = getStagingCard();

            var auth = account.Authorize("100", "DKK", card);

            Assert.NotNull(auth);
            Assert.True(auth.isSuccess());
        }

        [Fact]
        public void ItVoidsAuthorization()
        {
            var card = getStagingCard();

            var auth = account.Authorize("100", "DKK", card);

            var v = account.Void(auth);

            Assert.True(v.isSuccess());
        }

        [Fact]
        public void ItCapturesAuthorization()
        {
            var card = getStagingCard();

            var auth = account.Authorize("100", "DKK", card);

            var capture = account.Capture(auth, "100");

            Assert.True(capture.isSuccess());
        }

        [Fact]
        public void ItRefundsAuthorization()
        {
            var card = getStagingCard();

            var auth = account.Authorize("100", "DKK", card);
            account.Capture(auth, "100");

            var refund = account.Refund(auth.id, "100", "");

            Assert.True(refund.isSuccess());
        }

        [Fact]
        public void ItTokenizesCard()
        {
            var card = getStagingCard();

            var credit = account.Credit("100", "DKK", card, "", "");

            Assert.True(credit.isSuccess());
        }

        [Fact]
        public void CreditWorks()
        {
            account.SigningKeys(getValidAPIKey(), getPrivateKey());
            var card = getStagingCard();

            var credit = account.Credit("100", "DKK", card, "", "");

            Assert.True(credit.isSuccess());
        }
    }
}
