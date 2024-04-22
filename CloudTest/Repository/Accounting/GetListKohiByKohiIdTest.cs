using CloudUnitTest.SampleData.AccountingRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.Accounting
{
    public class GetListKohiByKohiIdTest : BaseUT
    {
        [Test]
        public void GetListKohiByKohiIdTest_001_ListPtKohi_Null()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            int hpId = 998; long ptId = 12345; int sinDate = 0; List<int> kohiIds = new List<int>();
            try
            {
                //Act
                var result = accountingRepository.GetListKohiByKohiId(hpId, ptId, sinDate, kohiIds);
                //Assert
                Assert.True(!result.Any());
            }
            finally
            {
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListKohiByKohiIdTest_002_ListPtKohi_Any()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 0; List<int> kohiIds = new List<int>() { 10 };
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(hpInfs);
            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetListKohiByKohiId(hpId, ptId, sinDate, kohiIds);
                //Assert
                Assert.True(result.Count == 1 && !result[0].ConfirmDateList.Any());
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListKohiByKohiIdTest_003_PtHokenCheckRepos_Any()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 0; List<int> kohiIds = new List<int>() { 10 };
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var ptHokenChecks = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenChecks[0].HokenGrp = 2;
            tenant.AddRange(hpInfs);
            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            tenant.AddRange(ptHokenChecks);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetListKohiByKohiId(hpId, ptId, sinDate, kohiIds);
                //Assert
                Assert.True(result.Count == 1 && result[0].ConfirmDateList.Any());
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.RemoveRange(ptHokenChecks);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        private void SetupTestEnvironment(out AccountingRepository accountingRepository)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
        }
        private void CleanupResources(AccountingRepository accountingRepository)
        {
            accountingRepository.ReleaseResource();
        }
    }
}
