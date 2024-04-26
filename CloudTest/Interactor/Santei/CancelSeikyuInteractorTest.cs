using Domain.Models.Receipt;
using Domain.Models.ReceSeikyu;
using Interactor.CalculateService;
using Interactor.ReceSeikyu;
using Moq;
using UseCase.ReceSeikyu.CancelSeikyu;

namespace CloudUnitTest.Interactor.Santei
{
    public class CancelSeikyuInteractorTest : BaseUT
    {
        [Test]
        public void TC_001_CancelSeikyuInteractorTest_Handle_InvalidInputItem()
        {
            //Arrange
            var mockIReceSeikyuRepository = new Mock<IReceSeikyuRepository>();
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockICalculateService = new Mock<ICalculateService>();

            var cancelSeikyuInteractor = new CancelSeikyuInteractor(mockIReceSeikyuRepository.Object, mockICalculateService.Object, mockIReceiptRepository.Object);

            int hpId = 999999, seikyuYm = 200104, seikyuKbn = 0, sinYm = 202404, hokenId = 0, userId = 999;
            long ptId = 20010328;

            CancelSeikyuInputData inputData = new CancelSeikyuInputData(hpId, seikyuYm, seikyuKbn, ptId, sinYm, hokenId, userId);
            ReceInfModel receCheckItem = new ReceInfModel();

            mockIReceiptRepository.Setup(x => x.GetReceInf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receCheckItem);

            // Act
            var result = cancelSeikyuInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == CancelSeikyuStatus.InvalidInputItem);
        }

        [Test]
        public void TC_002_CancelSeikyuInteractorTest_Handle_Failed()
        {
            //Arrange
            var mockIReceSeikyuRepository = new Mock<IReceSeikyuRepository>();
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockICalculateService = new Mock<ICalculateService>();

            var cancelSeikyuInteractor = new CancelSeikyuInteractor(mockIReceSeikyuRepository.Object, mockICalculateService.Object, mockIReceiptRepository.Object);

            int hpId = 999999, seikyuYm = 200104, seikyuKbn = 0, sinYm = 202404, hokenId = 0, userId = 999, hokenId2 = 0, kaId = 0, tantoId = 0, hokenKbn = 0, hokenSbtCd = 0, kohi3ReceTensu = 0;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, honkeKbn = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKyufu = 0, kohi3ReceFutan = 0;
            int kohi2ReceKyufu = 0, kohi3ReceKyufu = 0, kohi4ReceKyufu = 0, hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi4ReceTensu = 0, kohi4ReceFutan = 0;
            long ptId = 20010328, ptNum = 0;
            string receSbt = "", houbetu = "", kohi1Houbetu = "", kohi2Houbetu = "", kohi3Houbetu = "", kohi4Houbetu = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "";
            string tokki5 = "";

            CancelSeikyuInputData inputData = new CancelSeikyuInputData(hpId, seikyuYm, seikyuKbn, ptId, sinYm, hokenId, userId);
            ReceInfModel receCheckItem = new ReceInfModel(hpId, seikyuYm, ptId, ptNum, sinYm, hokenId, hokenId2, kaId, tantoId, receSbt, hokenKbn, hokenSbtCd, houbetu, kohi1Id, kohi2Id,
                                                          kohi3Id, kohi4Id, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, honkeKbn, tokki1, tokki2, tokki3, tokki4, tokki5,
                                                          hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu, kohi4Nissu, kohi1ReceKyufu, kohi2ReceKyufu, kohi3ReceKyufu, kohi4ReceKyufu, 
                                                          hokenReceTensu, hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi2ReceTensu, kohi2ReceFutan, kohi3ReceTensu,
                                                          kohi3ReceFutan, kohi4ReceTensu, kohi4ReceFutan);

            mockIReceiptRepository.Setup(x => x.GetReceInf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receCheckItem);

            // Act
            var result = cancelSeikyuInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == CancelSeikyuStatus.Failed);
        }

        [Test]
        public void TC_003_CancelSeikyuInteractorTest_Handle_Successed()
        {
            //Arrange
            var mockIReceSeikyuRepository = new Mock<IReceSeikyuRepository>();
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockICalculateService = new Mock<ICalculateService>();

            var cancelSeikyuInteractor = new CancelSeikyuInteractor(mockIReceSeikyuRepository.Object, mockICalculateService.Object, mockIReceiptRepository.Object);

            int hpId = 999999, seikyuYm = 200104, seikyuKbn = 0, sinYm = 202404, hokenId = 0, userId = 999, hokenId2 = 0, kaId = 0, tantoId = 0, hokenKbn = 0, hokenSbtCd = 0, kohi3ReceTensu = 0, seqNo = 9;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, honkeKbn = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKyufu = 0, kohi3ReceFutan = 0;
            int kohi2ReceKyufu = 0, kohi3ReceKyufu = 0, kohi4ReceKyufu = 0, hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi4ReceTensu = 0, kohi4ReceFutan = 0;
            long ptId = 20010328, ptNum = 0;
            string receSbt = "", houbetu = "", kohi1Houbetu = "", kohi2Houbetu = "", kohi3Houbetu = "", kohi4Houbetu = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "";
            string tokki5 = "";

            CancelSeikyuInputData inputData = new CancelSeikyuInputData(hpId, seikyuYm, seikyuKbn, ptId, sinYm, hokenId, userId);
            
            ReceInfModel receCheckItem = new ReceInfModel(hpId, seikyuYm, ptId, ptNum, sinYm, hokenId, hokenId2, kaId, tantoId, receSbt, hokenKbn, hokenSbtCd, houbetu, kohi1Id, kohi2Id,
                                                          kohi3Id, kohi4Id, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, honkeKbn, tokki1, tokki2, tokki3, tokki4, tokki5,
                                                          hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu, kohi4Nissu, kohi1ReceKyufu, kohi2ReceKyufu, kohi3ReceKyufu, kohi4ReceKyufu,
                                                          hokenReceTensu, hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi2ReceTensu, kohi2ReceFutan, kohi3ReceTensu,
                                                          kohi3ReceFutan, kohi4ReceTensu, kohi4ReceFutan);
            ReceSeikyuModel receSeikyuDuplicate = new ReceSeikyuModel(ptId, sinYm, hokenId, ptNum, seikyuKbn);

            mockIReceiptRepository.Setup(x => x.GetReceInf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receCheckItem);
            mockIReceSeikyuRepository.Setup(x => x.GetReceSeikyuDuplicate(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, long ptId, int sinYm, int hokenId) => receSeikyuDuplicate);
            mockIReceSeikyuRepository.Setup(x => x.InsertNewReceSeikyu(It.IsAny<ReceSeikyuModel>(), It.IsAny<int>(), It.IsAny<int>())).Returns((ReceSeikyuModel model, int userId, int hpId) => seqNo);

            // Act
            var result = cancelSeikyuInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == CancelSeikyuStatus.Successed);
        }
    }
}
