using Domain.Models.Receipt;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt.GetReceiCheckList;

namespace CloudUnitTest.Interactor.Receipt
{
    public class GetReceiCheckListInteractorTest : BaseUT
    {
        [Test]
        public void TC_038_GetReceiCheckListInteractor_Handle()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();

            var getReceiCheckListInteractor = new GetReceiCheckListInteractor(mockIReceiptRepository.Object);

            int hpId = 99999999;
            int sinYm = 200103;
            long ptId = 99999999;
            int hokenIid = 0;
            int seqNo = 99999999;
            int isPending = 0;
            string cmt = "";
            int isChecked = 0;
            int sortNo = 0;
            string errCd = "";
            int sinDate = 20010328;
            string aCd = "";
            string bCd = "";

            List<ReceCheckCmtModel> receCheckCmtModels = new List<ReceCheckCmtModel>()
            {
                new ReceCheckCmtModel(ptId, seqNo, sinYm, hokenIid, isPending, cmt, isChecked, sortNo)
            };

            List<ReceCheckErrModel> receCheckErrModels = new List<ReceCheckErrModel>()
            {
                new ReceCheckErrModel(errCd, sinDate, aCd, bCd, isChecked)
            };

            mockIReceiptRepository.Setup(x => x.GetReceCheckCmtList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).
            Returns((int input1, int input2, long input3, int input4) => receCheckCmtModels);
            mockIReceiptRepository.Setup(x => x.GetReceCheckErrList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).
            Returns((int input1, int input2, long input3, int input4) => receCheckErrModels);

            GetReceiCheckListInputData inputData = new GetReceiCheckListInputData(hpId, sinYm, ptId, hokenIid);

            //Act
            var result = getReceiCheckListInteractor.Handle(inputData);

            //Assert
            Assert.True(result.ReceiptCheckCmtErrList.Any() && result.Status == GetReceiCheckListStatus.Successed);
        }
    }
}
