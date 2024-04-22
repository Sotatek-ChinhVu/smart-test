using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt;
using UseCase.Receipt.SaveReceCheckCmtList;

namespace CloudUnitTest.Interactor.Receipt
{
    public class SaveReceCheckCmtListInteractorTest : BaseUT
    {
        #region
        [Test]
        public void TC_001_SaveReceCheckCmtListInteractor_Handle_ValidateInput_InvalidPtId()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = -20010328;
            int sinYm = 200103;
            int hokenId = 0;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>();
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>();
            List<ReceCheckErrModel> receCheckErrModels = new List<ReceCheckErrModel>();
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            
            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.InvalidPtId);
        }

        [Test]
        public void TC_002_SaveReceCheckCmtListInteractor_Handle_ValidateInput_InvalidSinYm()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 20010328;
            int hokenId = 0;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>();
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>();
            List<ReceCheckErrModel> receCheckErrModels = new List<ReceCheckErrModel>();
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.InvalidSinYm);
        }

        [Test]
        public void TC_003_SaveReceCheckCmtListInteractor_Handle_ValidateInput_InvalidHokenId()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = -1;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>();
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>();
            List<ReceCheckErrModel> receCheckErrModels = new List<ReceCheckErrModel>();
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.InvalidHokenId);
        }

        [Test]
        public void TC_004_SaveReceCheckCmtListInteractor_Handle_ValidateInput_Failed()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = 1;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>();
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>();
            List<ReceCheckErrModel> receCheckErrModels = new List<ReceCheckErrModel>();
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int input1, int input2) => true);

            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.Failed);
        }

        [Test]
        public void TC_005_SaveReceCheckCmtListInteractor_Handle_ValidateInput_InvalidStatusColor()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = 1;
            string errCd = "";
            int sinDate = 28032001;
            string aCd = "";
            string bCd = "";
            string message1 = "";
            string message2 = "";
            int isChecked = 0;
            int seqNo = 0;
            int statusColor = -1;
            string cmt = "";
            int sortNo = 0;
            bool isDeleted = true;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>()
            {
                new ReceCheckCmtItem(seqNo, statusColor, cmt, isChecked, sortNo, isDeleted)
            };
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>()
            {
                new ReceCheckErrorItem(errCd, sinDate, aCd, bCd, message1, message2, isChecked)
            };
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int input1, int input2) => true);

            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.InvalidStatusColor);
        }

        [Test]
        public void TC_006_SaveReceCheckCmtListInteractor_Handle_ValidateInput_InvalidCmt()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = 1;
            string errCd = "";
            int sinDate = 28032001;
            string aCd = "";
            string bCd = "";
            string message1 = "";
            string message2 = "";
            int isChecked = 0;
            int seqNo = 0;
            int statusColor = 2;
            string cmt = "";
            int sortNo = 0;
            bool isDeleted = true;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>()
            {
                new ReceCheckCmtItem(seqNo, statusColor, cmt, isChecked, sortNo, isDeleted)
            };
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>()
            {
                new ReceCheckErrorItem(errCd, sinDate, aCd, bCd, message1, message2, isChecked)
            };
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int input1, int input2) => true);

            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.InvalidCmt);
        }

        [Test]
        public void TC_007_SaveReceCheckCmtListInteractor_Handle_ValidateInput_InvalidSeqNo()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = 1;
            string errCd = "";
            int sinDate = 28032001;
            string aCd = "";
            string bCd = "";
            string message1 = "";
            string message2 = "";
            int isChecked = 0;
            int seqNo = 99999999;
            int statusColor = 2;
            string cmt = "Kaito";
            int sortNo = 0;
            bool isDeleted = true;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>()
            {
                new ReceCheckCmtItem(seqNo, statusColor, cmt, isChecked, sortNo, isDeleted)
            };
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>()
            {
                new ReceCheckErrorItem(errCd, sinDate, aCd, bCd, message1, message2, isChecked)
            };
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int input1, int input2) => true);

            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.InvalidSeqNo);
        }

        [Test]
        public void TC_008_SaveReceCheckCmtListInteractor_Handle_ValidateInput_InvalidReceCheckErrorItem()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = 1;
            string errCd = "";
            int sinDate = 28032001;
            string aCd = "";
            string bCd = "";
            string message1 = "";
            string message2 = "";
            int isChecked = 0;
            int seqNo = 99999999;
            int statusColor = 2;
            string cmt = "Kaito";
            int sortNo = 0;
            bool isDeleted = true;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>()
            {
                new ReceCheckCmtItem(seqNo, statusColor, cmt, isChecked, sortNo, isDeleted)
            };
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>()
            {
                new ReceCheckErrorItem(errCd, sinDate, aCd, bCd, message1, message2, isChecked)
            };
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int input1, int input2) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistSeqNoReceCheckCmtList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<int>>())).Returns((int input1, int input2, int input3, long input4, List<int> input5) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistSeqNoReceCheckErrorList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<ReceCheckErrModel>>())).Returns((int input1, int input2, int input3, long input4, List<ReceCheckErrModel> input5) => false);

            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.InvalidReceCheckErrorItem);
        }
        #endregion

        [Test]
        public void TC_009_SaveReceCheckCmtListInteractor_Handle_Successed()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = 1;
            string errCd = "";
            int sinDate = 28032001;
            string aCd = "";
            string bCd = "";
            string message1 = "";
            string message2 = "";
            int isChecked = 0;
            int seqNo = 99999999;
            int statusColor = 2;
            string cmt = "Kaito";
            int sortNo = 0;
            bool isDeleted = true;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>()
            {
                new ReceCheckCmtItem(seqNo, statusColor, cmt, isChecked, sortNo, isDeleted)
            };
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>()
            {
                new ReceCheckErrorItem(errCd, sinDate, aCd, bCd, message1, message2, isChecked)
            };
            List<ReceCheckCmtModel> receCheckErrModels = new List<ReceCheckCmtModel>()
            {
                new ReceCheckCmtModel(seqNo, 0, cmt, isChecked, sortNo, isDeleted)
            };
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int input1, int input2) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistSeqNoReceCheckCmtList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<int>>())).Returns((int input1, int input2, int input3, long input4, List<int> input5) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistSeqNoReceCheckErrorList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<ReceCheckErrModel>>())).Returns((int input1, int input2, int input3, long input4, List<ReceCheckErrModel> input5) => true);
            mockIReceiptRepository.Setup(x => x.SaveReceCheckErrList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<ReceCheckErrModel>>())).Returns((int input1, int input2, int input3, int input4, long input5, List<ReceCheckErrModel> input6) => true);
            mockIReceiptRepository.Setup(x => x.SaveReceCheckCmtList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<ReceCheckCmtModel>>())).Returns((int input1, int input2, int input3, int input4, long input5, List<ReceCheckCmtModel> input6) => receCheckErrModels);

            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.Successed);
        }

        [Test]
        public void TC_010_SaveReceCheckCmtListInteractor_Handle_Failed()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceCheckCmtListInteractor = new SaveReceCheckCmtListInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999;
            int userId = 999;
            long ptId = 20010328;
            int sinYm = 200103;
            int hokenId = 1;
            string errCd = "";
            int sinDate = 28032001;
            string aCd = "";
            string bCd = "";
            string message1 = "";
            string message2 = "";
            int isChecked = 0;
            int seqNo = 99999999;
            int statusColor = 2;
            string cmt = "Kaito";
            int sortNo = 0;
            bool isDeleted = true;

            List<ReceCheckCmtItem> receCheckCmtItems = new List<ReceCheckCmtItem>()
            {
                new ReceCheckCmtItem(seqNo, statusColor, cmt, isChecked, sortNo, isDeleted)
            };
            List<ReceCheckErrorItem> receCheckErrorItems = new List<ReceCheckErrorItem>()
            {
                new ReceCheckErrorItem(errCd, sinDate, aCd, bCd, message1, message2, isChecked)
            };
            List<ReceCheckCmtModel> receCheckErrModels = new List<ReceCheckCmtModel>();
            SaveReceCheckCmtListInputData inputData = new SaveReceCheckCmtListInputData(hpId, userId, ptId, sinYm, hokenId, receCheckCmtItems, receCheckErrorItems);
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int input1, List<long> input2) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int input1, int input2) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistSeqNoReceCheckCmtList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<int>>())).Returns((int input1, int input2, int input3, long input4, List<int> input5) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistSeqNoReceCheckErrorList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<ReceCheckErrModel>>())).Returns((int input1, int input2, int input3, long input4, List<ReceCheckErrModel> input5) => true);
            mockIReceiptRepository.Setup(x => x.SaveReceCheckErrList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<ReceCheckErrModel>>())).Returns((int input1, int input2, int input3, int input4, long input5, List<ReceCheckErrModel> input6) => false);
            mockIReceiptRepository.Setup(x => x.SaveReceCheckCmtList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<List<ReceCheckCmtModel>>())).Returns((int input1, int input2, int input3, int input4, long input5, List<ReceCheckCmtModel> input6) => receCheckErrModels);

            // Act
            var result = saveReceCheckCmtListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == SaveReceCheckCmtListStatus.Failed);
        }
    }
}
