using Domain.Models.Diseases;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt.GetReceByomeiChecking;

namespace CloudUnitTest.Interactor.Receipt
{
    public class GetReceByomeiCheckingInteractorTest : BaseUT
    {
        [Test]
        public void TC_001_GetReceByomeiCheckingInteractorTest_Handle()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var getReceByomeiCheckingInteractor = new GetReceByomeiCheckingInteractor(mockIReceiptRepository.Object, mockIOrdInfRepository.Object, mockIPtDiseaseRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, sinDate = -1, hokenId = 1, rowNo = 0, sinKouiKbn = 0, unitSbt = 0, kohatuKbn = 0, syohoKbn = 0, syohoLimitKbn = 0, drugKbn = 0, yohoKbn = 0, isNodspRece = 0, jissiKbn = 0, jissiId = 0, commentNewline = 0, sikkanKbn = 0;
            long ptId = 20010328, raiinNo = 0, rpNo = 0, rpEdaNo = 0;
            string itemCd = "Kaito", itemName = "", unitNam = "", kokuji1 = "", kokuji2 = "", ipnCd = "", ipnName = "", jissiMachine = "", reqCd = "", bunkatu = "", cmtName = "", cmtOpt = "", fontColor = "";
            double suryo = 0, termVal = 0;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", minAge = "", maxAge = "", santeiItemCd = "";
            int startDate = 0, endDate = 0;

            List<OrdInfDetailModel> todayOrderList = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(hpId, raiinNo, rpNo, rpEdaNo, rowNo, ptId, sinDate, sinKouiKbn, itemCd, itemName, suryo, unitNam, unitSbt, termVal, kohatuKbn, syohoKbn, syohoLimitKbn, drugKbn, yohoKbn,
                                      kokuji1, kokuji2, isNodspRece, ipnCd, ipnName, jissiKbn, new(), jissiId, jissiMachine, reqCd, bunkatu, cmtName, cmtOpt, fontColor, commentNewline)
            };
            List<PtDiseaseModel> todayByomeiList = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(byomeiCd, byomei, sikkanKbn, icd10, icd102013, icd1012013, icd1022013)
            };
            List<TenItemModel> tenMstItemList = new List<TenItemModel>()
            {
                new TenItemModel(hpId, itemCd, minAge, maxAge, santeiItemCd, startDate, endDate)
            };
            List<PtDiseaseModel> allByomeisByOdr = new List<PtDiseaseModel>();

            GetReceByomeiCheckingInputData inputData = new GetReceByomeiCheckingInputData(hpId, ptId, sinDate, hokenId);
            mockIOrdInfRepository.Setup(x => x.GetOdrInfsBySinDate(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, long ptId, int sinDate, int hokenPId) => todayOrderList);
            mockIPtDiseaseRepository.Setup(x => x.GetByomeiInThisMonth(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int hpId, int sinYm, long ptId, int hokenId) => todayByomeiList);
            mockIMstItemRepository.Setup(x => x.FindTenMst(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> itemCds, int minSinDate, int maxSinDate) => tenMstItemList);
            mockIPtDiseaseRepository.Setup(x => x.GetTekiouByomeiByOrder(It.IsAny<int>(), It.IsAny<List<string>>()))
            .Returns((int hpId, List<string> itemCds) => todayByomeiList);

            // Act
            var result = getReceByomeiCheckingInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetReceByomeiCheckingStatus.Successed);
        }

        // ItemCd = "@BUNKATU"
        [Test]
        public void TC_002_GetReceByomeiCheckingInteractorTest_Handle_ItemCd()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var getReceByomeiCheckingInteractor = new GetReceByomeiCheckingInteractor(mockIReceiptRepository.Object, mockIOrdInfRepository.Object, mockIPtDiseaseRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, sinDate = -1, hokenId = 1, rowNo = 0, sinKouiKbn = 0, unitSbt = 0, kohatuKbn = 0, syohoKbn = 0, syohoLimitKbn = 0, drugKbn = 0, yohoKbn = 0, isNodspRece = 0, jissiKbn = 0, jissiId = 0, commentNewline = 0, sikkanKbn = 0;
            long ptId = 20010328, raiinNo = 0, rpNo = 0, rpEdaNo = 0;
            string itemCd = "@BUNKATU", itemName = "", unitNam = "", kokuji1 = "", kokuji2 = "", ipnCd = "", ipnName = "", jissiMachine = "", reqCd = "", bunkatu = "", cmtName = "", cmtOpt = "", fontColor = "";
            double suryo = 0, termVal = 0;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", minAge = "", maxAge = "", santeiItemCd = "";
            int startDate = 0, endDate = 0;

            List<OrdInfDetailModel> todayOrderList = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(hpId, raiinNo, rpNo, rpEdaNo, rowNo, ptId, sinDate, sinKouiKbn, itemCd, itemName, suryo, unitNam, unitSbt, termVal, kohatuKbn, syohoKbn, syohoLimitKbn, drugKbn, yohoKbn,
                                      kokuji1, kokuji2, isNodspRece, ipnCd, ipnName, jissiKbn, new(), jissiId, jissiMachine, reqCd, bunkatu, cmtName, cmtOpt, fontColor, commentNewline)
            };
            List<PtDiseaseModel> todayByomeiList = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(byomeiCd, byomei, sikkanKbn, icd10, icd102013, icd1012013, icd1022013)
            };
            List<TenItemModel> tenMstItemList = new List<TenItemModel>()
            {
                new TenItemModel(hpId, itemCd, minAge, maxAge, santeiItemCd, startDate, endDate)
            };
            List<PtDiseaseModel> allByomeisByOdr = new List<PtDiseaseModel>();

            GetReceByomeiCheckingInputData inputData = new GetReceByomeiCheckingInputData(hpId, ptId, sinDate, hokenId);
            mockIOrdInfRepository.Setup(x => x.GetOdrInfsBySinDate(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, long ptId, int sinDate, int hokenPId) => todayOrderList);
            mockIPtDiseaseRepository.Setup(x => x.GetByomeiInThisMonth(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int hpId, int sinYm, long ptId, int hokenId) => todayByomeiList);
            mockIMstItemRepository.Setup(x => x.FindTenMst(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> itemCds, int minSinDate, int maxSinDate) => tenMstItemList);
            mockIPtDiseaseRepository.Setup(x => x.GetTekiouByomeiByOrder(It.IsAny<int>(), It.IsAny<List<string>>()))
            .Returns((int hpId, List<string> itemCds) => todayByomeiList);

            // Act
            var result = getReceByomeiCheckingInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetReceByomeiCheckingStatus.Successed);
        }

        // todayOrderList.ItemCd != tenMstItemList.ItemCd
        [Test]
        public void TC_003_GetReceByomeiCheckingInteractorTest_Handle_TodayOrderList_TenMstItemList()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var getReceByomeiCheckingInteractor = new GetReceByomeiCheckingInteractor(mockIReceiptRepository.Object, mockIOrdInfRepository.Object, mockIPtDiseaseRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, sinDate = -1, hokenId = 1, rowNo = 0, sinKouiKbn = 0, unitSbt = 0, kohatuKbn = 0, syohoKbn = 0, syohoLimitKbn = 0, drugKbn = 0, yohoKbn = 0, isNodspRece = 0, jissiKbn = 0, jissiId = 0, commentNewline = 0, sikkanKbn = 0;
            long ptId = 20010328, raiinNo = 0, rpNo = 0, rpEdaNo = 0;
            string itemCd = "Kaito", itemName = "", unitNam = "", kokuji1 = "", kokuji2 = "", ipnCd = "", ipnName = "", jissiMachine = "", reqCd = "", bunkatu = "", cmtName = "", cmtOpt = "", fontColor = "";
            double suryo = 0, termVal = 0;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", minAge = "", maxAge = "", santeiItemCd = "";
            int startDate = 0, endDate = 0;

            List<OrdInfDetailModel> todayOrderList = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(hpId, raiinNo, rpNo, rpEdaNo, rowNo, ptId, sinDate, sinKouiKbn, itemCd, itemName, suryo, unitNam, unitSbt, termVal, kohatuKbn, syohoKbn, syohoLimitKbn, drugKbn, yohoKbn,
                                      kokuji1, kokuji2, isNodspRece, ipnCd, ipnName, jissiKbn, new(), jissiId, jissiMachine, reqCd, bunkatu, cmtName, cmtOpt, fontColor, commentNewline)
            };
            List<PtDiseaseModel> todayByomeiList = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(byomeiCd, byomei, sikkanKbn, icd10, icd102013, icd1012013, icd1022013)
            };
            List<TenItemModel> tenMstItemList = new List<TenItemModel>()
            {
                new TenItemModel(hpId, itemCd + "1", minAge, maxAge, santeiItemCd, startDate, endDate)
            };
            List<PtDiseaseModel> allByomeisByOdr = new List<PtDiseaseModel>();

            GetReceByomeiCheckingInputData inputData = new GetReceByomeiCheckingInputData(hpId, ptId, sinDate, hokenId);
            mockIOrdInfRepository.Setup(x => x.GetOdrInfsBySinDate(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, long ptId, int sinDate, int hokenPId) => todayOrderList);
            mockIPtDiseaseRepository.Setup(x => x.GetByomeiInThisMonth(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int hpId, int sinYm, long ptId, int hokenId) => todayByomeiList);
            mockIMstItemRepository.Setup(x => x.FindTenMst(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> itemCds, int minSinDate, int maxSinDate) => tenMstItemList);
            mockIPtDiseaseRepository.Setup(x => x.GetTekiouByomeiByOrder(It.IsAny<int>(), It.IsAny<List<string>>()))
            .Returns((int hpId, List<string> itemCds) => todayByomeiList);

            // Act
            var result = getReceByomeiCheckingInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetReceByomeiCheckingStatus.Successed);
        }

        // allByomeisByOdr.Count() == 0
        [Test]
        public void TC_004_GetReceByomeiCheckingInteractorTest_Handle_AllByomeisByOdr()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();

            var getReceByomeiCheckingInteractor = new GetReceByomeiCheckingInteractor(mockIReceiptRepository.Object, mockIOrdInfRepository.Object, mockIPtDiseaseRepository.Object, mockIMstItemRepository.Object);

            int hpId = 99999999, sinDate = -1, hokenId = 1, rowNo = 0, sinKouiKbn = 0, unitSbt = 0, kohatuKbn = 0, syohoKbn = 0, syohoLimitKbn = 0, drugKbn = 0, yohoKbn = 0, isNodspRece = 0, jissiKbn = 0, jissiId = 0, commentNewline = 0, sikkanKbn = 0;
            long ptId = 20010328, raiinNo = 0, rpNo = 0, rpEdaNo = 0;
            string itemCd = "Kaito", itemName = "", unitNam = "", kokuji1 = "", kokuji2 = "", ipnCd = "", ipnName = "", jissiMachine = "", reqCd = "", bunkatu = "", cmtName = "", cmtOpt = "", fontColor = "";
            double suryo = 0, termVal = 0;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", minAge = "", maxAge = "", santeiItemCd = "";
            int startDate = 0, endDate = 0;

            List<OrdInfDetailModel> todayOrderList = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(hpId, raiinNo, rpNo, rpEdaNo, rowNo, ptId, sinDate, sinKouiKbn, itemCd, itemName, suryo, unitNam, unitSbt, termVal, kohatuKbn, syohoKbn, syohoLimitKbn, drugKbn, yohoKbn,
                                      kokuji1, kokuji2, isNodspRece, ipnCd, ipnName, jissiKbn, new(), jissiId, jissiMachine, reqCd, bunkatu, cmtName, cmtOpt, fontColor, commentNewline)
            };
            List<PtDiseaseModel> todayByomeiList = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(byomeiCd, byomei, sikkanKbn, icd10, icd102013, icd1012013, icd1022013)
            };
            List<TenItemModel> tenMstItemList = new List<TenItemModel>()
            {
                new TenItemModel(hpId, itemCd, minAge, maxAge, santeiItemCd, startDate, endDate)
            };
            List<PtDiseaseModel> allByomeisByOdr = new List<PtDiseaseModel>();

            GetReceByomeiCheckingInputData inputData = new GetReceByomeiCheckingInputData(hpId, ptId, sinDate, hokenId);
            mockIOrdInfRepository.Setup(x => x.GetOdrInfsBySinDate(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, long ptId, int sinDate, int hokenPId) => todayOrderList);
            mockIPtDiseaseRepository.Setup(x => x.GetByomeiInThisMonth(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int hpId, int sinYm, long ptId, int hokenId) => todayByomeiList);
            mockIMstItemRepository.Setup(x => x.FindTenMst(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> itemCds, int minSinDate, int maxSinDate) => tenMstItemList);
            mockIPtDiseaseRepository.Setup(x => x.GetTekiouByomeiByOrder(It.IsAny<int>(), It.IsAny<List<string>>()))
            .Returns((int hpId, List<string> itemCds) => allByomeisByOdr);

            // Act
            var result = getReceByomeiCheckingInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetReceByomeiCheckingStatus.Successed);
        }
    }
}
