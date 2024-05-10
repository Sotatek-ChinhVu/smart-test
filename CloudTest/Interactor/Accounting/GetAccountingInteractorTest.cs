using CloudUnitTest.SampleData.AccountingRepository;
using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.Insurance;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Repositories;
using Interactor.Accounting;
using Microsoft.Extensions.Configuration;
using Moq;
using UseCase.Accounting.GetAccountingInf;

namespace CloudUnitTest.Interactor.Accounting
{
    public class GetAccountingInteractorTest : BaseUT
    {
        #region [test handle]

        [Test]
        public void GetAccountingInteractorTest_001_NoData()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            int hpId = 0;
            long ptId = 0;
            int sinDate = 0;
            long raiinNo = 0;
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);

            //Act
            var result = getAccountingInteractor.Handle(inputData);
            //Assert
            Assert.True(result.GetAccountingStatus == GetAccountingStatus.NoData);
        }


        /// <summary>
        /// Check NyukinKbn = 1
        /// </summary>
        [Test]
        public void GetAccountingInteractorTest_002_Check_NyukinKbnEqual1()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998;
            long ptId = 12345;
            int sinDate = 20180807;
            long raiinNo = 1234321;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            raiinInfs.RemoveAt(1);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.SaveChanges();
            }
        }

        /// <summary>
        /// Check NyukinKbn = 0
        /// </summary>
        [Test]
        public void GetAccountingInteractorTest_003_Check_NyukinKbnEqual0()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998;
            long ptId = 12345;
            int sinDate = 20180807;
            long raiinNo = 1234321;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyus[0].NyukinKbn = 0;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            raiinInfs.RemoveAt(1);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.SaveChanges();
            }
        }
        #endregion [test handle]

        [Test]
        public void GetAccountingInteractorTest_004_GetVisibilityPtKohiModelList_Check_Kohi1Id()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998;
            long ptId = 12345;
            int sinDate = 20180807;
            long raiinNo = 1234321;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            raiinInfs.RemoveAt(1);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAccountingInteractorTest_005_GetVisibilityPtKohiModelList_AddKohiIds()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998;
            long ptId = 12345;
            int sinDate = 20180807;
            long raiinNo = 1234321;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            raiinInfs.RemoveAt(1);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.Successed && !result.KohiInfModels.Any());
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAccountingInteractorTest_006_GetVisibilityPtKohiModelList_Check_ListKohi()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998;
            long ptId = 12345;
            int sinDate = 20180807;
            long raiinNo = 1234321;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var hokenMst2 = AccountingRepositoryData.ReadHokenMst();
            hokenMst2[0].StartDate = 20200101;
            var ptHokenChecks = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenChecks[0].HokenGrp = 2;
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            raiinInfs.RemoveAt(1);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(hpInfs);
            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            tenant.Add(hokenMst2[0]);
            tenant.AddRange(ptHokenChecks);
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.Successed && result.KohiInfModels.Any());
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.Remove(hokenMst2[0]);
                tenant.RemoveRange(ptHokenChecks);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAccountingInteractorTest_007_GetAccountingInf_CheckDebitBalance()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998;
            long ptId = 12345;
            int sinDate = 20180807;
            long raiinNo = 1234321;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyus[0].SeikyuGaku = 50;
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].SeikyuGaku = 100;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var hokenMst2 = AccountingRepositoryData.ReadHokenMst();
            hokenMst2[0].StartDate = 20200101;
            var ptHokenChecks = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenChecks[0].HokenGrp = 2;
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(hpInfs);
            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            tenant.Add(hokenMst2[0]);
            tenant.AddRange(ptHokenChecks);
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.Successed && result.KohiInfModels.Any() && result.SumAdjust == result.SumAdjustView);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.Remove(hokenMst2[0]);
                tenant.RemoveRange(ptHokenChecks);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAccountingInteractorTest_008_GetAccountingInf_CheckDebitBalance()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998;
            long ptId = 12345;
            int sinDate = 20180807;
            long raiinNo = 1234321;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyus[0].SeikyuGaku = 50;
            syunoSeikyus[0].NyukinKbn = 0;
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].SeikyuGaku = 100;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var hokenMst2 = AccountingRepositoryData.ReadHokenMst();
            hokenMst2[0].StartDate = 20200101;
            var ptHokenChecks = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenChecks[0].HokenGrp = 2;
            var keySystemConfig = TenantProvider.GetDomainName() + CacheKeyConstant.GetListSystemConf + "_" + hpId;
            if (cache.KeyExists(keySystemConfig))
            {
                cache.KeyDelete(keySystemConfig);
            }
            var systemConf = tenant.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == 3020 && p.GrpEdaNo == 0);
            var temp = systemConf?.Val ?? 0;
            if (systemConf != null) systemConf.Val = 1;
            else
            {
                systemConf = new SystemConf
                {
                    HpId = hpId,
                    GrpCd = 3020,
                    GrpEdaNo = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 1,
                    UpdateId = 1,
                    Val = 1
                };
                tenant.SystemConfs.Add(systemConf);
            }

            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(hpInfs);
            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            tenant.Add(hokenMst2[0]);
            tenant.AddRange(ptHokenChecks);
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.Successed && result.KohiInfModels.Any() && result.SumAdjustView == result.SumAdjust + result.DebitBalance);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.Remove(hokenMst2[0]);
                tenant.RemoveRange(ptHokenChecks);
                systemConf.Val = temp;
                tenant.SaveChanges();
            }
        }




        private void SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor, out StackExchange.Redis.IDatabase cache)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
            var systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            getAccountingInteractor = new GetAccountingInteractor(accountingRepository, systemConfRepository);
            cache = systemConfRepository.GetCache();
        }
    }
}
