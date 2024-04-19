using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Entity.Tenant;
using Infrastructure.Repositories;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt;
using UseCase.Receipt.SaveReceiptEdit;

namespace CloudUnitTest.Interactor.Receipt
{
    public class SaveReceiptEditInteractorTest : BaseUT
    {
        [Test]
        public void TC_001_SaveReceiptEditInteractorTest_Handle_ValidateInput_Successed()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 0;
            long ptId = 20010328;
            bool isDeleted = true;
            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);
        
            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.Successed);
        }

        [Test]
        public void TC_002_SaveReceiptEditInteractorTest_Handle_ValidateInput_InvalidSinYm()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 20240328, hokenId = 1, seqNo = 1;
            long ptId = 20010328;
            bool isDeleted = false;
            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.InvalidSinYm);
        }

        [Test]
        public void TC_003_SaveReceiptEditInteractorTest_Handle_ValidateInput_InvalidSeikyuYm()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 20010419, sinYm = 202403, hokenId = 1, seqNo = 1;
            long ptId = 20010328;
            bool isDeleted = false;
            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.InvalidSeikyuYm);
        }

        [Test]
        public void TC_004_SaveReceiptEditInteractorTest_Handle_ValidateInput_InvalidPtId()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 1;
            long ptId = 20010328;
            bool isDeleted = false;
            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int hpid, List<long> ptIds) => false);

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.InvalidPtId);
        }

        [Test]
        public void TC_005_SaveReceiptEditInteractorTest_Handle_ValidateInput_InvalidHokenId()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 1;
            long ptId = 20010328;
            bool isDeleted = false;
            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>())).Returns((int hpid, List<long> ptIds) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>())).Returns((int hpid, int hokenId) => false);

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.InvalidHokenId);
        }

        [Test]
        public void TC_006_SaveReceiptEditInteractorTest_Handle_ValidateInput_InvalidSeqNo()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 1;
            long ptId = 20010328;
            bool isDeleted = false;
            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>()))
            .Returns((int hpid, List<long> ptIds) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpid, int hokenId) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistReceiptEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId, int seqNo) => false);

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.InvalidSeqNo);
        }

        [Test]
        public void TC_007_SaveReceiptEditInteractorTest_Handle_ValidateInput_InvalidTokkiItem()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 1;
            long ptId = 20010328;
            bool isDeleted = false;
            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "Kaito", "", "", "", "");
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>()))
            .Returns((int hpid, List<long> ptIds) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpid, int hokenId) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistReceiptEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId, int seqNo) => true);

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.InvalidTokkiItem);
        }
        
        [Test]
        public void TC_008_SaveReceiptEditInteractorTest_Handle_ValidateInput_InvalidNissuItem()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 1, hokenNissu = 100;
            long ptId = 20010328;
            bool isDeleted = false;

            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, hokenNissu, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>()))
            .Returns((int hpid, List<long> ptIds) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpid, int hokenId) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistReceiptEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId, int seqNo) => true);

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.InvalidNissuItem);
        }

        [Test]
        public void TC_009_SaveReceiptEditInteractorTest_Handle_Return_Successed()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 1, hokenNissu = 99;
            long ptId = 20010328;
            bool isDeleted = false;

            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, hokenNissu, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "", "", "", "", "");
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>()))
            .Returns((int hpid, List<long> ptIds) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpid, int hokenId) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistReceiptEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId, int seqNo) => true);
            mockIReceiptRepository.Setup(x => x.SaveReceiptEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ReceiptEditModel>()))
            .Returns((int hpId, int userId, int seikyuYm, long ptId, int sinYm, int hokenId, ReceiptEditModel model) => true);

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.Successed);
        }

        [Test]
        public void TC_010_SaveReceiptEditInteractorTest_Handle_Return_Failed()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var saveReceiptEditInteractor = new SaveReceiptEditInteractor(TenantProvider, mockIReceiptRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, userId = 999, seikyuYm = 200104, sinYm = 202403, hokenId = 1, seqNo = 1, hokenNissu = 99;
            long ptId = 20010328;
            bool isDeleted = false;
            Dictionary<string, string> tokkiMstDictionary = new Dictionary<string, string>();
            tokkiMstDictionary.Add("KAI", "kaito");

            ReceiptEditItem receiptEdit = new ReceiptEditItem(seqNo, hokenNissu, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isDeleted, "KAI", "", "", "", "");
            mockIPatientInforRepository.Setup(x => x.CheckExistIdList(It.IsAny<int>(), It.IsAny<List<long>>()))
            .Returns((int hpid, List<long> ptIds) => true);
            mockIInsuranceRepository.Setup(x => x.CheckExistHokenId(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpid, int hokenId) => true);
            mockIReceiptRepository.Setup(x => x.CheckExistReceiptEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, int seikyuYm, long ptId, int sinYm, int hokenId, int seqNo) => true);
            mockIReceiptRepository.Setup(x => x.SaveReceiptEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ReceiptEditModel>()))
            .Returns((int hpId, int userId, int seikyuYm, long ptId, int sinYm, int hokenId, ReceiptEditModel model) => false);
            mockIReceiptRepository.Setup(x => x.GetTokkiMstDictionary(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, int sinDate) => tokkiMstDictionary);

            SaveReceiptEditInputData inputData = new SaveReceiptEditInputData(hpId, userId, seikyuYm, ptId, sinYm, hokenId, receiptEdit);

            var result = saveReceiptEditInteractor.Handle(inputData);

            Assert.That(result.Status == SaveReceiptEditStatus.Failed);
        }
    }
}
