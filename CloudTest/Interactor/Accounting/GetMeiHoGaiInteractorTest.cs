using Domain.Models.CalculateModel;
using Helper.Constants;
using Infrastructure.Repositories;
using Interactor.Accounting;
using Interactor.CalculateService;
using Microsoft.Extensions.Configuration;
using Moq;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.GetSinMei;

namespace CloudUnitTest.Interactor.Accounting
{
    public class GetMeiHoGaiInteractorTest : BaseUT
    {
        [Test]
        public void GetMeiHoGaiInteractorTest_001_Handle_RaiinNo_NotAny()
        {
            // Arrange 
            SetupTestEnvironment(out GetMeiHoGaiInteractor getMeiHoGaiInteractor);
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; List<long> raiinNos = new List<long>() { };
            var inputData = new GetMeiHoGaiInputData(hpId, ptId, sinDate, raiinNos);
            try
            {
                //Act
                var result = getMeiHoGaiInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == GetMeiHoGaiStatus.NoData);
            }
            finally
            {

            }

        }

        [Test]
        public void GetMeiHoGaiInteractorTest_002_Handle_SinMeiNoNotAny()
        {
            // Arrange 
            SetupTestEnvironment(out GetMeiHoGaiInteractor getMeiHoGaiInteractor);
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; List<long> raiinNos = new List<long>() { 1234321 };
            var inputData = new GetMeiHoGaiInputData(hpId, ptId, sinDate, raiinNos);
            try
            {
                //Act
                var result = getMeiHoGaiInteractor.Handle(inputData);
                //Assert
                Assert.True(result.Status == GetMeiHoGaiStatus.NoData);
            }
            finally
            {

            }

        }


        private void SetupTestEnvironment(out GetMeiHoGaiInteractor getMeiHoGaiInteractor)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; List<long> raiinNos = new List<long>() { 1234321 };
            var inputData = new GetMeiHoGaiInputData(hpId, ptId, sinDate, raiinNos);
            var calculateService = new Mock<ICalculateService>();
            var mockSinMeiData = new List<SinMeiDataModel>
            {
                new SinMeiDataModel
                {
                    PtId = 12345,
                    RecId = "REC001",
                    SinId = 1,
                    // Các trường khác...
                }
            };
            var sinMeiInputData = new GetSinMeiDtoInputData(inputData.RaiinNos, inputData.PtId, inputData.SinDate, inputData.HpId, SinMeiModeConst.Kaikei);
            calculateService.Setup(x => x.GetSinMeiList(sinMeiInputData)).Returns(new SinMeiDataModelDto { sinMeiList = mockSinMeiData });

            var result = calculateService.Object.GetSinMeiList(sinMeiInputData);
            getMeiHoGaiInteractor = new GetMeiHoGaiInteractor(accountingRepository, calculateService.Object, TenantProvider);
        }
    }
}
