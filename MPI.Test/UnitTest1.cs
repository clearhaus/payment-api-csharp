using System;
using Xunit;

using Clearhaus.MPI.Builder;

namespace Clearhaus.MPI.Test
{
    public class UnitTest1
    {
        private string apiKey;
        private string cardNumber;
        private string cardExpireMonth;
        private string cardExpireYear;
        private string merchantID;
        private string merchantName;
        private string merchantAcquirerBin;
        private string merchantCountry;
        private string merchantUrl;
        private string cardholderIP;

        private string pares;

        public UnitTest1()
        {
            this.apiKey = Environment.GetEnvironmentVariable("MPI_KEY");

            this.cardNumber = Environment.GetEnvironmentVariable("CARDNUMBER");
            this.cardExpireMonth = Environment.GetEnvironmentVariable("CARDEXPIREMONTH");
            this.cardExpireYear = Environment.GetEnvironmentVariable("CARDEXPIREYEAR");

            this.merchantAcquirerBin = Environment.GetEnvironmentVariable("ACQUIRER_BIN");
            this.merchantID = Environment.GetEnvironmentVariable("MERCHANT_ID");
            this.merchantName = Environment.GetEnvironmentVariable("MERCHANT_NAME");
            this.merchantUrl = Environment.GetEnvironmentVariable("MERCHANT_URL");

            this.pares = Environment.GetEnvironmentVariable("PARES");
        }

        [Fact]
        public void ItFetchesEnrollmentStatus()
        {
            var mpi = new MPI(apiKey, Constants.MPITestURL);

            var builder = new EnrollCheckBuilder {
                amount               = "100",
                currency             = "DKK",
                orderID              = "1241249814",
                merchantName         = this.merchantName,
                merchantID           = this.merchantID,
                merchantAcquirerBin  = this.merchantAcquirerBin,
                merchantCountry      = "DK",
                merchantUrl          = this.merchantUrl,
                cardholderIP         = "1.1.1.1",
                cardNumber           = this.cardNumber,
                cardExpireMonth      = this.cardExpireMonth,
                cardExpireYear       = this.cardExpireYear
            };

            var status =  mpi.EnrollCheck(builder);
            // Most of the time the testing directory servers are offline.
            Assert.True(status.error?.message == "Directory server timeout or connection error" || status.enrolled == "Y",
                    "Status: '" + status.enrolled + "', Error: " + status.error?.message);
        }

        [Fact]
        public void ItChecksPARes()
        {
            var mpi = new MPI(apiKey, Constants.MPITestURL);

            var checkresponse = mpi.CheckPARes(pares);

            Assert.Equal("Y", checkresponse.status);
        }
    }
}
