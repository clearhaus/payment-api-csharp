using Xunit;

namespace Clearhaus.Gateway.Test
{
    public class AuthorizationTests
    {
        [Fact]
        public void CanOmitCSCWhenSignedAndTrusted()
        {
            var account = Util.GetSigningStagingAccount();
            var card = Util.GetStagingCard();

            card.csc = null;

            var auth = account.Authorize("100", "DKK", card, null, null);

            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        [Fact]
        public void CannotOmitCSCWhenNotSigned()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            card.csc = "";

            var auth = account.Authorize("100", "DKK", card, null, null);

            Assert.False(auth.IsSuccess(), "Must sign request");
        }

        [Fact]
        public void CanProvideOptionalArguments()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            var options = new Gateway.AuthorizationRequestOptions {
                ip = "1.1.1.1",
                textOnStatement = "test text",
                reference = "afhAsdgg"
            };

            var auth = account.Authorize("100", "DKK", card, null, options);

            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        [Fact]
        public void CanOmitOptionalArguments()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            var options = new Gateway.AuthorizationRequestOptions {
                ip = "1.1.1.1",
                reference = "afhAsdgg"
            };

            var auth = account.Authorize("100", "DKK", card, null, options);

            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        [Fact]
        public void CanDoRecurring()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            var options = new Gateway.AuthorizationRequestOptions {
                recurring = true
            };

            var auth = account.Authorize("100", "DKK", card, null, options);

            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        /**
         * Tests if failures are handled gracefully
         */

        [Fact]
        public void GracefulAmountFailure()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            var auth = account.Authorize("lal", "DKK", card, null, null);

            Assert.False(auth.IsSuccess(), "Invalid amount should fail");
        }

        [Fact]
        public void GracefulCurrencyFailure()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            var auth = account.Authorize("100", "invalid", card, null, null);

            Assert.False(auth.IsSuccess(), "Invalid currency should fail");
        }

        [Fact]
        public void GracefulTextOnStatementFailure()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            var options = new Gateway.AuthorizationRequestOptions {
                textOnStatement = "This is a really long text on statement"
            };

            var auth = account.Authorize("100", "DKK", card, null, options);

            Assert.False(auth.IsSuccess(), "Invalid currency should fail");
        }

        [Fact]
        public void GracefulCardFailure()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();
            card.expireYear = "Not a year";

            var auth = account.Authorize("100", "DKK", card, null, null);

            Assert.False(auth.IsSuccess(), "Invalid currency should fail");
        }

        [Fact]
        public void GracefulPAResFailure()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            var pares = "This is not a pares";

            var auth = account.Authorize("100", "DKK", card, pares, null);

            Assert.False(auth.IsSuccess(), "Invalid pares should fail");
        }

        [Fact]
        public void GracefulAPIKeyFailure()
        {
            var account = new Gateway.Account("invalid apikey");
            var card = Util.GetStagingCard();

            try
            {
                var auth = account.Authorize("100", "DKK", card, null, null);
            }
            catch(ClrhsAuthException e)
            {
                Assert.NotNull(e);
                return;
            }

            Assert.True(false, "Invalid APIKey did not throw exception");
        }

        [Fact]
        public void GracefulAPIKeyCheckFailure()
        {
            var account = new Gateway.Account("invalid apikey");

            Assert.False(account.ValidAPIKey());
        }

        [Fact]
        public void GracefulNetFailure()
        {
            var account = new Account("Invalid apikey", "https://nonexistant.clearhaus.com");
            var card = Util.GetStagingCard();

            try
            {
                var auth = account.Authorize("100", "DKK", card, null, null);
            }
            catch(ClrhsNetException e)
            {
                Assert.NotNull(e);
                return;
            }

            Assert.True(false, "Inability to connect to gateway did not fail");
        }

        [Fact]
        public void GracefulNetAuthFailure()
        {
            var account = new Account("Invalid apikey", "https://nonexistant.clearhaus.com");

            try
            {
                account.ValidAPIKey();
            }
            catch(ClrhsNetException e)
            {
                Assert.NotNull(e);
                return;
            }

            Assert.True(false, "No exception thrown");
        }
    }
}
