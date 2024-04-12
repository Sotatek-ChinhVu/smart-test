using CloudUnitTest.SampleData.AccountingRepository;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Interactor.Accounting;
using Microsoft.Extensions.Configuration;
using Moq;
using PostgreDataContext;
using UseCase.Accounting.SaveAccounting;
using static Helper.Constants.UserConst;

namespace CloudUnitTest.Interactor.Accounting
{
    public class SaveAccountingInteractorTest : BaseUT
    {
        #region [test Validate]
        [Test]
        public void SaveAccountingInteractorTest_001_ValidateInputData_InvalidHpId()
        {
            // Arrange
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            int hpId = 0; long ptId = 12345; int userId = 1; int sinDate = 0; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
            try
            {
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.InvalidHpId);
            }
            finally
            {
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_002_ValidateInputData_InvalidUserId()
        {
            // Arrange
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 0; int sinDate = 0; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            tenant.AddRange(hpInfs);
            var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.InvalidUserId);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_003_ValidateInputData_InvalidPtId()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 0; int userId = 96789049; int sinDate = 0; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.InvalidPtId);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_004_ValidateInputData_InvalidPayType()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 0; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = -1; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.InvalidPayType);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_005_ValidateInputData_InvalidComment()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 0; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = new string('x', 101); ; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.InvalidComment);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_006_ValidateInputData_InvalidSindate()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 123456789; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.InvalidSindate);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_007_ValidateInputData_InvalidRaiinNo()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20240303; long raiinNo = 0;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.InvalidRaiinNo);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_008_ValidateInputData_NoPermission()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20240404; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.NoPermission);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_009_ValidateInputData_ValidateSuccess()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20240404; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.Unlimited;
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.ValidateInputData(inputData);
                //Assert
                Assert.True(result == SaveAccountingStatus.ValidateSuccess);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.SaveChanges();
            }
        }

        #endregion [test Validate]

        [Test]
        public void SaveAccountingInteractorTest_010_Handle_ValidateFaild()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20240404; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.ReadOnly;
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status != SaveAccountingStatus.ValidateSuccess);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_011_Handle_SyunoSeikyu_Null()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20240404; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.Unlimited;
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == SaveAccountingStatus.InputDataNull);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.SaveChanges();
            }
        }

        /// <summary>
        /// NyukinKbn != 0
        /// </summary>
        [Test]
        public void SaveAccountingInteractorTest_012_Handle_Check_NyukinKbn()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20180807; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.Unlimited;

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
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == SaveAccountingStatus.Success);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.SaveChanges();
            }
        }

        /// <summary>
        /// NyukinKbn == 0
        /// </summary>
        [Test]
        public void SaveAccountingInteractorTest_013_Handle_Check_NyukinKbn()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20180807; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.Unlimited;

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
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == SaveAccountingStatus.Success && result.RaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_014_Handle_Check_DebitBalance()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20180807; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.Unlimited;

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
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var hokenMst2 = AccountingRepositoryData.ReadHokenMst();
            hokenMst2[0].StartDate = 20200101;
            var ptHokenChecks = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenChecks[0].HokenGrp = 2;

            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            tenant.Add(hokenMst2[0]);
            tenant.AddRange(ptHokenChecks);
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);


            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == SaveAccountingStatus.Success && result.RaiinNoPrint.Count == 2);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.Remove(hokenMst2[0]);
                tenant.RemoveRange(ptHokenChecks);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_015_Handle_Check_AccDue()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20180807; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.Unlimited;

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
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var hokenMst2 = AccountingRepositoryData.ReadHokenMst();
            hokenMst2[0].StartDate = 20200101;
            var ptHokenChecks = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenChecks[0].HokenGrp = 2;

            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            tenant.Add(hokenMst2[0]);
            tenant.AddRange(ptHokenChecks);
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                UpdateValueSystemConf(cache, hpId, tenant);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == SaveAccountingStatus.Success && result.RaiinNoPrint.Count == 2);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.Remove(hokenMst2[0]);
                tenant.RemoveRange(ptHokenChecks);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void SaveAccountingInteractorTest_016_AddAuditTrailLog_isDisCharged()
        {
            // Arrange           
            var keyUser = TenantProvider.GetDomainName() + CacheKeyConstant.UserInfoCacheService;
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 96789049; int sinDate = 20180807; long raiinNo = 1234321;
            int sumAdjust = 0; int thisWari = 0; int credit = 0; int payType = 0; string comment = ""; bool isDisCharged = true; string kaikeiTime = "";
            var hpInfs = AccountingRepositoryData.ReadHpInf();
            var useMsts = AccountingRepositoryData.ReadUserMst();
            var ptInfs = AccountingRepositoryData.ReadPtInf();
            var userPermission = AccountingRepositoryData.ReadUserPermission();
            userPermission[0].Permission = (int)PermissionType.Unlimited;

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
            var ptKohis = AccountingRepositoryData.ReadPtKohi();
            var hokenMsts = AccountingRepositoryData.ReadHokenMst();
            var hokenMst2 = AccountingRepositoryData.ReadHokenMst();
            hokenMst2[0].StartDate = 20200101;
            var ptHokenChecks = AccountingRepositoryData.ReadPtHokenCheck();
            ptHokenChecks[0].HokenGrp = 2;

            tenant.AddRange(ptKohis);
            tenant.AddRange(hokenMsts);
            tenant.Add(hokenMst2[0]);
            tenant.AddRange(ptHokenChecks);
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            tenant.AddRange(hpInfs);
            tenant.AddRange(useMsts);
            tenant.AddRange(ptInfs);
            tenant.AddRange(userPermission);

            try
            {
                tenant.SaveChanges();
                SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache);
                UpdateValueSystemConf(cache, hpId, tenant);
                if (cache.KeyExists(keyUser))
                {
                    cache.KeyDelete(keyUser);
                }
                var inputData = new SaveAccountingInputData(hpId, ptId, userId, sinDate, raiinNo, sumAdjust, thisWari, credit, payType, comment, isDisCharged, kaikeiTime);
                //Act
                var result = saveAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == SaveAccountingStatus.Success && result.RaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(hpInfs);
                tenant.RemoveRange(useMsts);
                tenant.RemoveRange(ptInfs);
                tenant.RemoveRange(userPermission);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(ptKohis);
                tenant.RemoveRange(hokenMsts);
                tenant.Remove(hokenMst2[0]);
                tenant.RemoveRange(ptHokenChecks);
                tenant.SaveChanges();
            }
        }

        private void SetupTestEnvironment(out SaveAccountingInteractor saveAccountingInteractor, out StackExchange.Redis.IDatabase cache)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var userInfService = new UserInfoService(TenantProvider, mockConfiguration.Object);
            var accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
            var systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            var userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, userInfService);
            var hpInfConfRepository = new HpInfRepository(TenantProvider);
            var receptionRepository = new ReceptionRepository(TenantProvider);
            var patientInforRepository = new PatientInforRepository(TenantProvider, receptionRepository);
            var auditLogRepository = new AuditLogRepository(TenantProvider);
            saveAccountingInteractor = new SaveAccountingInteractor(TenantProvider, accountingRepository, systemConfRepository, userRepository, hpInfConfRepository, patientInforRepository, receptionRepository, auditLogRepository);
            cache = systemConfRepository.GetCache();
        }

        private void UpdateValueSystemConf(StackExchange.Redis.IDatabase cache, int hpId, TenantDataContext tenant)
        {
            var keySystemConfig = TenantProvider.GetDomainName() + CacheKeyConstant.GetListSystemConf + "_" + hpId;
            if (cache.KeyExists(keySystemConfig))
            {
                cache.KeyDelete(keySystemConfig);
            }
            var systemConf = tenant.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == 3020 && p.GrpEdaNo == 0);
            var temp = systemConf?.Val ?? 0;
            if (systemConf != null) systemConf.Val = 0;
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
                    Val = 0
                };
                tenant.SystemConfs.Add(systemConf);
            }
        }
    }
}
