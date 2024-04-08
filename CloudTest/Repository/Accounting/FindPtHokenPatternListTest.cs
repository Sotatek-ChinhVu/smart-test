using CloudUnitTest.SampleData.AccountingRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.Accounting
{
    public class FindPtHokenPatternListTest : BaseUT
    {
        [Test]
        public void FindPtHokenPatternList_001_Check_PtInf()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(!result.Any());
            }
            finally
            {
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void FindPtHokenPatternList_002_Check_PtHokenPattern()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(!result.Any());
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void FindPtHokenPatternList_003_CheckNull_predicateHokenMst()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenId = 30;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(!result.Any());
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void FindPtHokenPatternList_004_Check_predicateHokenMst()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void FindPtHokenPatternList_005_Check_ptHokenCheck()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// Check function private CreatePtKohiModel
        /// </summary>
        [Test]
        public void FindPtHokenPatternList_006_Check_ptHokenCheck_Sinday()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = -1;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void FindPtHokenPatternList_007_Check_ptHokenMst()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = -1;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            //tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                //tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// HokenGrp = 2, HokenId == ptKohi1.HokenId
        /// </summary>
        [Test]
        public void FindPtHokenPatternList_008_Check_ReturnModel()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenCheck[0].HokenGrp = 2;
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// HokenGrp = 2, HokenId == ptKohi2.HokenId
        /// </summary>
        [Test]
        public void FindPtHokenPatternList_009_Check_ReturnModel()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi2Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenCheck[0].HokenGrp = 2;
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// HokenGrp = 2, HokenId == ptKohi3.HokenId
        /// </summary>
        [Test]
        public void FindPtHokenPatternList_010_Check_ReturnModel()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi3Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenCheck[0].HokenGrp = 2;
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// HokenGrp = 2, HokenId == ptKohi3.HokenId
        /// </summary>
        [Test]
        public void FindPtHokenPatternList_011_Check_ReturnModel()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi4Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenCheck[0].HokenGrp = 2;
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// check null hokMstMapped
        /// </summary>
        [Test]
        public void FindPtHokenPatternList_012_CreatePtKohiModel()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDay = 23;
            List<int> listPatternId = new List<int>() { 10 };
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDay, listPatternId);
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
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
