using CloudUnitTest.SampleData.AccountingRepository;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Interactor.Accounting;
using Microsoft.Extensions.Configuration;
using Moq;
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
    }
}
