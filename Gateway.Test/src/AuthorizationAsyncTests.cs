using System;
using Xunit;

namespace Clearhaus.Gateway.Test
{
    public class AuthorizationAsyncTests
    {
        [Fact]
        public void CanOmitCSCWhenSignedAndTrusted()
        {
            var account = Util.GetSigningStagingAccount();
            var card = Util.GetStagingCard();

            card.csc = null;

            var auth = account.AuthorizeAsync("100", "DKK", card, null, null).Result;

            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        [Fact]
        public void CannotOmitCSCWhenNotSigned()
        {
            var account = Util.GetStagingAccount();
            var card = Util.GetStagingCard();

            card.csc = "";

            var auth = account.AuthorizeAsync("100", "DKK", card, null, null).Result;

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

            var auth = account.AuthorizeAsync("100", "DKK", card, null, options).Result;

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

            var auth = account.AuthorizeAsync("100", "DKK", card, null, options).Result;

            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        [Fact]
        public void GracefulNetFailure()
        {
            //var account = Util.GetStagingAccount();
            var account = new Account("Invalid apikey", "https://nonexistant.clearhaus.com");
            var card = Util.GetStagingCard();

            try
            {
                var authTask = account.AuthorizeAsync("100", "DKK", card, null, null).Result;
            }
            catch(AggregateException e)
            {
                e.Handle((x) =>
                {
                    return x is ClrhsNetException;
                });
            }
        }
    }
}
