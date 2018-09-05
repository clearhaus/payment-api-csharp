using System;
using Xunit;

using Clearhaus.Gateway.Transaction;

namespace Clearhaus.Gateway.Test
{
    public class Basic
    {
        private Gateway.Account account;

        public Basic()
        {
            account = new Gateway.Account(Util.GetValidAPIKey(), Constants.GatewayTestURL);
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
            var card = Util.GetStagingCard();

            var auth = account.Authorize("100", "DKK", card, null, null);

            Assert.NotNull(auth);
            Assert.True(auth.IsSuccess());
        }

        [Fact]
        public void ItVoidsAuthorization()
        {
            var card = Util.GetStagingCard();

            var auth = account.Authorize("100", "DKK", card, null, null);

            var v = account.Void(auth.id);

            Assert.True(v.IsSuccess());
        }

        [Fact]
        public void ItCapturesAuthorization()
        {
            var card = Util.GetStagingCard();

            var auth = account.Authorize("100", "DKK", card, null, null);

            var capture = account.Capture(auth.id, "100", null);

            Assert.True(capture.IsSuccess());
        }

        [Fact]
        public void ItRefundsAuthorization()
        {
            var card = Util.GetStagingCard();

            var auth = account.Authorize("100", "DKK", card, null, null);
            account.Capture(auth.id, "100", null);

            var refund = account.Refund(auth.id, "100", "");

            Assert.True(refund.IsSuccess());
        }

        [Fact]
        public void ItTokenizesCard()
        {
            var card = Util.GetStagingCard();

            var credit = account.Credit("100", "DKK", card, "", "");

            Assert.True(credit.IsSuccess());
        }

        [Fact]
        public void CreditWorks()
        {
            var card = Util.GetStagingCard();

            var credit = account.Credit("100", "DKK", card, "", "");

            Assert.True(credit.IsSuccess());
        }

        /**
         * MobilePayOnline tests
         */
        [Fact]
        public void ItCreatesMobilePayOnlineAuthorization()
        {
            var account = Util.GetSigningStagingAccount();
            var mpoInfo = Util.GetStagingMPOInfo();
            var auth = account.Authorize("100", "DKK", mpoInfo, null, null);

            Assert.NotNull(auth);
            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        [Fact]
        public void ItCreatesMobilePayOnlineAuthorizationWithPhoneNumber()
        {
            var account = Util.GetSigningStagingAccount();
            var mpoInfo = Util.GetStagingMPOInfo();
            mpoInfo.phoneNumber = "12445678";

            Gateway.Transaction.Authorization auth;
            auth = account.Authorize("100", "DKK", mpoInfo, null, null);

            Assert.NotNull(auth);
            Assert.True(auth.IsSuccess(), auth.status.message);
        }

        [Fact]
        public void ItCreatesMobilePayOnlineAuthorizationWithPARes()
        {
            var account = Util.GetSigningStagingAccount();
            var mpoInfo = Util.GetStagingMPOInfo();
            var pares = Util.GetPARes();

            var auth = account.Authorize("100", "DKK", mpoInfo, pares, null);

            Assert.NotNull(auth);
            Assert.True(!auth.IsSuccess());
            Assert.True(auth.status.message == "3-D Secure problem", auth.status.message);
        }


        /**********************************/
        /**** Account Example testcase ****/
        /**********************************/


        [Fact]
        public void TestExample()
        {
            string error = null;
            var apiKey = Util.GetValidAPIKey();
            var card = Util.GetStagingCard();

            // The `Account` destructor disposes of the HttpClient,
            // it is also possible to call `#Dispose` manually.
            var account = new Account(apiKey, Constants.GatewayTestURL);

            var authOptions = new AuthorizationRequestOptions
            {
                recurring = true,
                reference = "sdg7SF12KJHjj"
            };

            Authorization myAuth;
            try
            {
                myAuth = account.Authorize("100", "DKK", card, null, authOptions);

                Assert.True(myAuth.IsSuccess(), myAuth.status.message);
            }
            catch(ClrhsNetException e)
            {
                // Failure connecting to clearhaus.
                // You should retry this.
                error = e.Message;
            }
            catch(ClrhsAuthException e)
            {
                // ApiKey was invalid
                // You need to change the apiKey.
                // This can be avoided by checking the key first:
                // account.ValidAPIKey() == true
                error = e.Message;
            }
            catch(ClrhsGatewayException e)
            {
                // Something was funky with the Clearhaus gateway
                // You could retry this, but maybe give it a few seconds.
                error = e.Message;
            }
            catch(ClrhsException e)
            {
                // Last effort exception
                error = e.Message;
            }

            Assert.True(String.IsNullOrWhiteSpace(error), error);
        }
    }
}
