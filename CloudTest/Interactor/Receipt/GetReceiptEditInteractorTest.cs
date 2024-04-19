using Domain.Models.Receipt;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt.ReceiptEdit;

namespace CloudUnitTest.Interactor.Receipt
{
    public class GetReceiptEditInteractorTest : BaseUT
    {
        // SeqNo != 0
        [Test]
        public void TC_001_GetReceiptEditInteractorTest_Handle_SeqNo_1()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();

            var getReceiptEditInteractor = new GetReceiptEditInteractor(mockIReceiptRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 202404, hokenId = 0, seqNo = 1;
            long ptId = 20010328;
            
            Dictionary<string, string> tokkiMstDictionary = new Dictionary<string, string>();
            ReceiptEditModel receInfEdit = new ReceiptEditModel(seqNo, "", "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            ReceInfModel receInf = new ReceInfModel(hpId, seikyuYm, ptId, 0, sinYm, hokenId, 0, 0, 0, "", 0, 0, "", 0, 0, 0, 0, "", "",
                                                    "", "", 0, "", "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            GetReceiptEditInputData inputData = new GetReceiptEditInputData(hpId, seikyuYm, ptId, sinYm, hokenId);

            mockIReceiptRepository.Setup(x => x.GetTokkiMstDictionary(It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int sinDate) => tokkiMstDictionary);
            mockIReceiptRepository.Setup(x => x.GetReceInfEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receInfEdit);
            mockIReceiptRepository.Setup(x => x.GetReceInfPreEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receInfEdit);
            mockIReceiptRepository.Setup(x => x.GetReceInf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receInf);

            // Act
            var result = getReceiptEditInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetReceiptEditStatus.Successed && result.SeqNo == seqNo);
        }

        // SeqNo == 0
        [Test]
        public void TC_002_GetReceiptEditInteractorTest_Handle_SeqNo_0()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();

            var getReceiptEditInteractor = new GetReceiptEditInteractor(mockIReceiptRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 202404, hokenId = 0, seqNo = 0;
            long ptId = 20010328;
            Dictionary<string, string> tokkiMstDictionary = new Dictionary<string, string>();
            ReceiptEditModel receInfEdit = new ReceiptEditModel(seqNo, "", "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            ReceInfModel receInf = new ReceInfModel(hpId, seikyuYm, ptId, 0, sinYm, hokenId, 0, 0, 0, "", 0, 0, "", 0, 0, 0, 0, "", "",
                                                    "", "", 0, "", "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            GetReceiptEditInputData inputData = new GetReceiptEditInputData(hpId, seikyuYm, ptId, sinYm, hokenId);

            mockIReceiptRepository.Setup(x => x.GetTokkiMstDictionary(It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int sinDate) => tokkiMstDictionary);
            mockIReceiptRepository.Setup(x => x.GetReceInfEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receInfEdit);
            mockIReceiptRepository.Setup(x => x.GetReceInfPreEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receInfEdit);
            mockIReceiptRepository.Setup(x => x.GetReceInf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId) => receInf);

            // Act
            var result = getReceiptEditInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetReceiptEditStatus.Successed && result.SeqNo == seqNo);
        }
    }
}
