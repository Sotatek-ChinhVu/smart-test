using CloudUnitTest.SampleData.AccountingRepository;
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
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor);
            int hpId = 0;
            long ptId = 0;
            int sinDate = 0;
            long raiinNo = 0;
            var inputData = new GetAccountingInputData(hpId, ptId, sinDate, raiinNo);
            try
            {
                //Act
                var result = getAccountingInteractor.Handle(inputData);
                //Assert
                Assert.True(result.GetAccountingStatus == GetAccountingStatus.NoData);
            }
            finally
            {

            }
        }


        /// <summary>
        /// Check NyukinKbn =1
        /// </summary>
        [Test]
        public void GetAccountingInteractorTest_002_Check_NyukinKbn()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor);
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
        /// Check NyukinKbn =0
        /// </summary>
        [Test]
        public void GetAccountingInteractorTest_003_Check_NyukinKbn()
        {
            // Arrange
            SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor);
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

        private void SetupTestEnvironment(out GetAccountingInteractor getAccountingInteractor)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
            var systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            getAccountingInteractor = new GetAccountingInteractor(accountingRepository, systemConfRepository);
        }
    }
}
