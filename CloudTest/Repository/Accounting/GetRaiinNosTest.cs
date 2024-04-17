using CloudUnitTest.SampleData.AccountingRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.Accounting
{
    public class GetRaiinNosTest : BaseUT
    {
        [Test]
        public void GetRaiinNosTest_001_OyaRaiinNo_Null()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            int hpId = 1; long ptId = 12345; long raiinNo = 1234321; bool getAll = true;
            try
            {
                //Act
                var result = accountingRepository.GetRaiinNos(hpId, ptId, raiinNo, getAll);
                //Assert
                Assert.True(!result.Any());
            }
            finally
            {
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetRaiinNosTest_002_OyaRaiinNo_NotNull()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; long raiinNo = 1234321; bool getAll = true;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            tenant.AddRange(raiinInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetRaiinNos(hpId, ptId, raiinNo, getAll);
                //Assert
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetRaiinNosTest_003_GetAll_Faild()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; long raiinNo = 1234321; bool getAll = false;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            tenant.AddRange(raiinInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetRaiinNos(hpId, ptId, raiinNo, getAll);
                //Assert
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
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
