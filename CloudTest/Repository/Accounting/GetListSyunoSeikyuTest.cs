using CloudUnitTest.SampleData.AccountingRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.Accounting
{
    public class GetListSyunoSeikyuTest : BaseUT
    {
        [Test]
        public void GetListSyunoSeikyuTest_001_Default()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            int hpId = 1; long ptId = 12345; int sinDate = 20230303; List<long> listRaiinNo = new List<long>(); bool getAll = false;
            try
            {
                //Act
                var result = accountingRepository.GetListSyunoSeikyu(hpId, ptId, sinDate, listRaiinNo, getAll);
                //Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                CleanupResources(accountingRepository);
            }

        }

        [Test]
        public void GetListSyunoSeikyuTest_002_CheckNull_ListHokenPattern()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; List<long> listRaiinNo = new List<long>() { 1234321 }; bool getAll = false;
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetListSyunoSeikyu(hpId, ptId, sinDate, listRaiinNo, getAll);
                //Assert
                Assert.True(result.Count == 1 && result[0].HokenId == 0);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }

        }

        [Test]
        public void GetListSyunoSeikyuTest_003_CheckHokenPId_ListHokenPattern()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; List<long> listRaiinNo = new List<long>() { 1234321 }; bool getAll = false;
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            foreach (var i in raiinInfs)
            {
                i.HokenPid = 10;
            }
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetListSyunoSeikyu(hpId, ptId, sinDate, listRaiinNo, getAll);
                //Assert
                Assert.True(result.Count == 1 && result[0].HokenId == 10);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }

        }

        /// <summary>
        /// CHeck listRaiinNo = raiino table SyunoSeikyus
        /// </summary>
        [Test]
        public void GetListSyunoSeikyuTest_004_GetAll_True()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; List<long> listRaiinNo = new List<long>() { 1234321 }; bool getAll = true;
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetListSyunoSeikyu(hpId, ptId, sinDate, listRaiinNo, getAll);
                //Assert
                Assert.True(result.Count == 0);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// CHeck listRaiinNo != raiino table SyunoSeikyus
        /// </summary>
        [Test]
        public void GetListSyunoSeikyuTest_005_GetAll_True_CheckListRaiinNo()
        {
            // Arrange
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; List<long> listRaiinNo = new List<long>() { 123 }; bool getAll = true;
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = accountingRepository.GetListSyunoSeikyu(hpId, ptId, sinDate, listRaiinNo, getAll);
                //Assert
                Assert.True(result.Count == 1 && result[0].HokenId == 0);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
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
