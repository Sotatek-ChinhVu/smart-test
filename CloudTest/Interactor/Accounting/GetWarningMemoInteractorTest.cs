using CloudUnitTest.SampleData.AccountingRepository;
using Infrastructure.Repositories;
using Interactor.Accounting;
using Microsoft.Extensions.Configuration;
using Moq;
using UseCase.Accounting.WarningMemo;

namespace CloudUnitTest.Interactor.Accounting
{
    public class GetWarningMemoInteractorTest : BaseUT
    {
        [Test]
        public void GetWarningMemoInteractorTest_001_Handle_RaiinInf_NotAny()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            int hpId = 998; long ptId = 12345; int sinDate = 0; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(!result.WarningMemoModels.Any() && result.Status == GetWarningMemoStatus.NoData);
            }
            finally
            {

            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_002_Handle_Successed()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(!result.WarningMemoModels.Any() && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_003_GetWarningMemo_HokenPatternModel_Kohi1()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 2 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_004_GetWarningMemo_CheckHokenIsExpirated_IsHoken()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 2;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 4 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_005_GetWarningMemo_CheckHokenIsExpirated_IsRousai()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 12;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 3 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_006_GetWarningMemo_CheckHokenIsExpirated_IsJibai()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 14;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 3 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_007_GetWarningMemo_HokenPatternModel_Kohi2()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi2Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 2 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_008_GetWarningMemo_HokenPatternModel_Kohi3()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi3Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 2 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_009_GetWarningMemo_HokenPatternModel_Kohi4()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi4Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 2 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_010_GetWarningMemo_CheckKohiIsExpirated_IsExpirated_Faild()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            ptKohis[0].EndDate = 99999999;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 1 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_011_GetWarningMemo_CheckKohiHasDateConfirmed_HasDateConfirmed_Faild()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            ptKohis[0].EndDate = 99999999;
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenCheck[0].HokenGrp = 2;
            DateTime.TryParse("2018-08-17", out DateTime date);
            ptHokenCheck[0].CheckDate = date.ToUniversalTime();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 0 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_012_GetWarningMemo_CheckFutansyaNo()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi4Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            ptKohis[0].FutansyaNo = "test";
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            hokenMsts[0].HokenNo = 0;
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 2 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_013_GetWarningMemo_CheckListCalcLog()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi4Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            ptKohis[0].FutansyaNo = "test";
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var readCalcLog = AccountingRepositoryData.ReadCalcLog();
            hokenMsts[0].HokenNo = 0;
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            tenant.AddRange(readCalcLog);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 3 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.RemoveRange(readCalcLog);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetWarningMemoInteractorTest_014_GetWarningMemo_CheckListCalcLog_TextNull()
        {
            // Arrange 
            SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321;
            var inputData = new GetWarningMemoInputData(hpId, ptId, sinDate, raiinNo);
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[0].HokenPid = 10;
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var hokenInfs = AccountingRepositoryData.ReadPtHokenInf();
            hokenInfs[0].HokenKbn = 0;
            var hokenPattern = AccountingRepositoryData.ReadPtHokenPattern();
            hokenPattern[0].Kohi1Id = 0;
            hokenPattern[0].Kohi4Id = 10;
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            ptKohis[0].HokenNo = 0;
            ptKohis[0].FutansyaNo = "test";
            var ptHokenCheck = AccountingRepositoryData.ReadPtHokenCheck();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            hokenMsts[0].HokenNo = 0;
            var calcLog = AccountingRepositoryData.ReadCalcLog();
            calcLog[0].Text = string.Empty;
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(ptInfs);
            tenant.AddRange(hokenInfs);
            tenant.AddRange(hokenPattern);
            tenant.AddRange(ptKohis);
            tenant.AddRange(ptHokenCheck);
            tenant.AddRange(hokenMsts);
            tenant.AddRange(calcLog);

            try
            {
                tenant.SaveChanges();
                //Act
                var result = getWarningMemoInteractor.Handle(inputData);
                //Assert
                Assert.True(result.WarningMemoModels.Count == 2 && result.Status == GetWarningMemoStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(hokenInfs);
                tenant.RemoveRange(hokenPattern);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(ptHokenCheck);
                tenant.RemoveRange(hokenMsts);
                tenant.RemoveRange(calcLog);
                tenant.SaveChanges();
            }
        }
        private void SetupTestEnvironment(out GetWarningMemoInteractor getWarningMemoInteractor)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
            getWarningMemoInteractor = new GetWarningMemoInteractor(accountingRepository);
        }
    }
}
