using Xunit;

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
            var auth = account.Authorize("100", "DKK", mpoInfo, null);

            if (!auth.IsSuccess())
            {
                System.Console.WriteLine(auth.status.message);
            }

            Assert.NotNull(auth);
            Assert.True(auth.IsSuccess());
        }

        [Fact]
        public void ItCreatesMobilePayOnlineAuthorizationWithPhoneNumber()
        {
            var account = Util.GetSigningStagingAccount();
            var mpoInfo = Util.GetStagingMPOInfo();
            mpoInfo.phoneNumber = "12445678";
            var auth = account.Authorize("100", "DKK", mpoInfo, null);

            if (!auth.IsSuccess())
            {
                System.Console.WriteLine(auth.status.message);
            }

            Assert.NotNull(auth);
            Assert.True(auth.IsSuccess());
        }

        [Fact(Skip="This is not live yet")]
        public void ItCreatesMobilePayOnlineAuthorizationWithPARes()
        {
            var account = Util.GetSigningStagingAccount();
            var mpoInfo = Util.GetStagingMPOInfo();
            mpoInfo.pares = Util.GetPARes();
            var auth = account.Authorize("100", "DKK", mpoInfo, null);

            if (!auth.IsSuccess())
            {
                System.Console.WriteLine(auth.status.message);
            }

            Assert.NotNull(auth);
            Assert.True(auth.IsSuccess());
        }
    }
}
