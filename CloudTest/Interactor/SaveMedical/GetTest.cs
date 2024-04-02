using Entity.Tenant;
using Helper.Redis;
using Infrastructure.Repositories.SpecialNote;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class GetTest : BaseUT
    {

        private readonly StackExchange.Redis.IDatabase _cache;

        public GetTest()
        {
            string connection = string.Concat("10.2.15.78", ":", "6379");
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        [Test]
        public void TC_001_SaveMedicalInteractor_Test_KeyExists()
        {
            //Arrange
            var mockIConfiguration = new Mock<IConfiguration>();
            var summaryInfRepository = new SummaryInfRepository(TenantProvider, mockIConfiguration.Object);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            long id = 9999999;
            int hpid = 1;
            long ptId = 28032001;
            int seqNo = 99999999;
            string text = "平成28年8月13日にて事故症状固定";
            string finalKey = "SummaryInfGetList_1_28032001";
            _cache.StringAppend(finalKey, string.Empty);

            SummaryInf summaryInf = new SummaryInf()
            {
                Id = id,
                HpId = hpid,
                PtId = ptId,
                SeqNo = seqNo,
                Text = text
            };

            tenantTracking.SummaryInfs.Add(summaryInf);
            try
            {
                tenantTracking.SaveChanges();

                var result = summaryInfRepository.Get(hpid, ptId);

                Assert.That(result.Id != 9999999 && _cache.KeyExists(finalKey));
            }
            finally
            {
                summaryInfRepository.ReleaseResource();
                _cache.KeyDelete(finalKey);
                tenantTracking.SummaryInfs.Remove(summaryInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_002_SaveMedicalInteractor_Test_NoKeyExists()
        {
            //Arrange
            var mockIConfiguration = new Mock<IConfiguration>();
            var summaryInfRepository = new SummaryInfRepository(TenantProvider, mockIConfiguration.Object);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            long id = 9999999;
            int hpid = 1;
            long ptId = 28032001;
            int seqNo = 99999999;
            string text = "平成28年8月13日にて事故症状固定";
            string finalKey = "SummaryInfGetList_1_28032001";

            SummaryInf summaryInf = new SummaryInf()
            {
                Id = id,
                HpId = hpid,
                PtId = ptId,
                SeqNo = seqNo,
                Text = text
            };

            tenantTracking.SummaryInfs.Add(summaryInf);
            try
            {
                tenantTracking.SaveChanges();

                var result = summaryInfRepository.Get(hpid, ptId);

                Assert.That(result.Id == 9999999 && _cache.KeyExists(finalKey));
            }
            finally
            {
                summaryInfRepository.ReleaseResource();
                _cache.KeyDelete(finalKey);
                tenantTracking.SummaryInfs.Remove(summaryInf);
                tenantTracking.SaveChanges();
            }
        }
    }
}
