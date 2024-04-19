using Domain.Enum;
using Domain.Models.Diseases;
using Domain.Models.UserConf;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt.GetDiseaseReceList;

namespace CloudUnitTest.Interactor.Receipt
{
    public class GetDiseaseReceListInteractorTest : BaseUT
    {
        // sikkanKbn = 3
        [Test]
        public void TC_001_GetDiseaseReceListInteractorTest_Handle_sikkanKbn_3()
        {
            //Arrange
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIUserConfRepository = new Mock<IUserConfRepository>();

            var getDiseaseReceListInteractor = new GetDiseaseReceListInteractor(mockIPtDiseaseRepository.Object, mockIUserConfRepository.Object);

            int hpId = 99999999, userId = 999, hokenId = 0, sinYm = 200103, isImportant = 0, sinDate = 20010328, hokenPid = 0, togetuByomei = 0, delDate = 0;
            long seqNo = 999999, id = 999999, ptId = 28032001;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", hosokuCmt = "kaito";
            string code = "8064", name = "前頭部";
            int sortNo = 0, startDate = 0, tenkiKbn = 0, tenkiDate = 0, syubyoKbn = 1, sikkanKbn = 3, nanbyoCd = 9, isNodspRece = 0, isNodspKarte = 0, isDeleted = 0;
            List<PrefixSuffixModel> prefixSuffixList = new List<PrefixSuffixModel>()
            {
                new PrefixSuffixModel(code, name)
            };

            List<PtDiseaseModel> ptDiseaseModels = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId, ptId, seqNo, byomeiCd, sortNo, prefixSuffixList, byomei, startDate, tenkiKbn, tenkiDate, syubyoKbn,
                                   sikkanKbn, nanbyoCd, isNodspRece, isNodspKarte, isDeleted, id, isImportant, sinDate, icd10, icd102013,
                                   icd1012013, icd1022013, hokenPid, hosokuCmt, togetuByomei, delDate)
            };

            mockIPtDiseaseRepository.Setup(x => x.GetPatientDiseaseList(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DiseaseViewType>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered) => ptDiseaseModels);
            mockIUserConfRepository.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int userId, int groupCd, int grpItemCd, int grpItemEdaNo) => 0);

            GetDiseaseReceListInputData inputData = new GetDiseaseReceListInputData(hpId, userId, ptId, hokenId, sinYm);

            // Act
            var result = getDiseaseReceListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.DiseaseReceList.Any(x => x.Byomei == "(主)(皮1)(難)前頭部(" + hosokuCmt + ")") && result.Status == GetDiseaseReceListStatus.Successed);
        }

        // sikkanKbn = 4
        [Test]
        public void TC_002_GetDiseaseReceListInteractorTest_Handle_sikkanKbn_4()
        {
            //Arrange
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIUserConfRepository = new Mock<IUserConfRepository>();

            var getDiseaseReceListInteractor = new GetDiseaseReceListInteractor(mockIPtDiseaseRepository.Object, mockIUserConfRepository.Object);

            int hpId = 99999999, userId = 999, hokenId = 0, sinYm = 200103, isImportant = 0, sinDate = 20010328, hokenPid = 0, togetuByomei = 0, delDate = 0;
            long seqNo = 999999, id = 999999, ptId = 28032001;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", hosokuCmt = "kaito";
            string code = "1064", name = "前頭部";
            int sortNo = 0, startDate = 0, tenkiKbn = 0, tenkiDate = 0, syubyoKbn = 1, sikkanKbn = 4, nanbyoCd = 9, isNodspRece = 0, isNodspKarte = 0, isDeleted = 0;
            List<PrefixSuffixModel> prefixSuffixList = new List<PrefixSuffixModel>()
            {
                new PrefixSuffixModel(code, name)
            };

            List<PtDiseaseModel> ptDiseaseModels = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId, ptId, seqNo, byomeiCd, sortNo, prefixSuffixList, byomei, startDate, tenkiKbn, tenkiDate, syubyoKbn,
                                   sikkanKbn, nanbyoCd, isNodspRece, isNodspKarte, isDeleted, id, isImportant, sinDate, icd10, icd102013,
                                   icd1012013, icd1022013, hokenPid, hosokuCmt, togetuByomei, delDate)
            };

            mockIPtDiseaseRepository.Setup(x => x.GetPatientDiseaseList(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DiseaseViewType>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered) => ptDiseaseModels);
            mockIUserConfRepository.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int userId, int groupCd, int grpItemCd, int grpItemEdaNo) => 0);

            GetDiseaseReceListInputData inputData = new GetDiseaseReceListInputData(hpId, userId, ptId, hokenId, sinYm);

            // Act
            var result = getDiseaseReceListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.DiseaseReceList.Any(x => x.Byomei == "(主)(皮2)(難)前頭部(" + hosokuCmt + ")") && result.Status == GetDiseaseReceListStatus.Successed);
        }

        // sikkanKbn = 5
        [Test]
        public void TC_003_GetDiseaseReceListInteractorTest_Handle_sikkanKbn_5()
        {
            //Arrange
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIUserConfRepository = new Mock<IUserConfRepository>();

            var getDiseaseReceListInteractor = new GetDiseaseReceListInteractor(mockIPtDiseaseRepository.Object, mockIUserConfRepository.Object);

            int hpId = 99999999, userId = 999, hokenId = 0, sinYm = 200103, isImportant = 0, sinDate = 20010328, hokenPid = 0, togetuByomei = 0, delDate = 0;
            long seqNo = 999999, id = 999999, ptId = 28032001;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", hosokuCmt = "kaito";
            string code = "1064", name = "前頭部";
            int sortNo = 0, startDate = 0, tenkiKbn = 0, tenkiDate = 0, syubyoKbn = 1, sikkanKbn = 5, nanbyoCd = 9, isNodspRece = 0, isNodspKarte = 0, isDeleted = 0;
            List<PrefixSuffixModel> prefixSuffixList = new List<PrefixSuffixModel>()
            {
                new PrefixSuffixModel(code, name)
            };

            List<PtDiseaseModel> ptDiseaseModels = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId, ptId, seqNo, byomeiCd, sortNo, prefixSuffixList, byomei, startDate, tenkiKbn, tenkiDate, syubyoKbn,
                                   sikkanKbn, nanbyoCd, isNodspRece, isNodspKarte, isDeleted, id, isImportant, sinDate, icd10, icd102013,
                                   icd1012013, icd1022013, hokenPid, hosokuCmt, togetuByomei, delDate)
            };

            mockIPtDiseaseRepository.Setup(x => x.GetPatientDiseaseList(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DiseaseViewType>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered) => ptDiseaseModels);
            mockIUserConfRepository.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int userId, int groupCd, int grpItemCd, int grpItemEdaNo) => 0);

            GetDiseaseReceListInputData inputData = new GetDiseaseReceListInputData(hpId, userId, ptId, hokenId, sinYm);

            // Act
            var result = getDiseaseReceListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.DiseaseReceList.Any(x => x.Byomei == "(主)(特)(難)前頭部(" + hosokuCmt + ")") && result.Status == GetDiseaseReceListStatus.Successed);
        }

        // sikkanKbn = 7
        [Test]
        public void TC_004_GetDiseaseReceListInteractorTest_Handle_sikkanKbn_7()
        {
            //Arrange
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIUserConfRepository = new Mock<IUserConfRepository>();

            var getDiseaseReceListInteractor = new GetDiseaseReceListInteractor(mockIPtDiseaseRepository.Object, mockIUserConfRepository.Object);

            int hpId = 99999999, userId = 999, hokenId = 0, sinYm = 200103, isImportant = 0, sinDate = 20010328, hokenPid = 0, togetuByomei = 0, delDate = 0;
            long seqNo = 999999, id = 999999, ptId = 28032001;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", hosokuCmt = "kaito";
            string code = "1064", name = "前頭部";
            int sortNo = 0, startDate = 0, tenkiKbn = 0, tenkiDate = 0, syubyoKbn = 1, sikkanKbn = 7, nanbyoCd = 9, isNodspRece = 0, isNodspKarte = 0, isDeleted = 0;
            List<PrefixSuffixModel> prefixSuffixList = new List<PrefixSuffixModel>()
            {
                new PrefixSuffixModel(code, name)
            };

            List<PtDiseaseModel> ptDiseaseModels = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId, ptId, seqNo, byomeiCd, sortNo, prefixSuffixList, byomei, startDate, tenkiKbn, tenkiDate, syubyoKbn,
                                   sikkanKbn, nanbyoCd, isNodspRece, isNodspKarte, isDeleted, id, isImportant, sinDate, icd10, icd102013,
                                   icd1012013, icd1022013, hokenPid, hosokuCmt, togetuByomei, delDate)
            };

            mockIPtDiseaseRepository.Setup(x => x.GetPatientDiseaseList(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DiseaseViewType>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered) => ptDiseaseModels);
            mockIUserConfRepository.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int userId, int groupCd, int grpItemCd, int grpItemEdaNo) => 0);

            GetDiseaseReceListInputData inputData = new GetDiseaseReceListInputData(hpId, userId, ptId, hokenId, sinYm);

            // Act
            var result = getDiseaseReceListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.DiseaseReceList.Any(x => x.Byomei == "(主)(て)(難)前頭部(" + hosokuCmt + ")") && result.Status == GetDiseaseReceListStatus.Successed);
        }

        // sikkanKbn = 8
        [Test]
        public void TC_005_GetDiseaseReceListInteractorTest_Handle_sikkanKbn_8()
        {
            //Arrange
            var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
            var mockIUserConfRepository = new Mock<IUserConfRepository>();

            var getDiseaseReceListInteractor = new GetDiseaseReceListInteractor(mockIPtDiseaseRepository.Object, mockIUserConfRepository.Object);

            int hpId = 99999999, userId = 999, hokenId = 0, sinYm = 200103, isImportant = 0, sinDate = 20010328, hokenPid = 0, togetuByomei = 0, delDate = 0;
            long seqNo = 999999, id = 999999, ptId = 28032001;
            string byomeiCd = "", byomei = "", icd10 = "", icd102013 = "", icd1012013 = "", icd1022013 = "", hosokuCmt = "kaito";
            string code = "1064", name = "前頭部";
            int sortNo = 0, startDate = 0, tenkiKbn = 0, tenkiDate = 0, syubyoKbn = 1, sikkanKbn = 8, nanbyoCd = 9, isNodspRece = 0, isNodspKarte = 0, isDeleted = 0;
            List<PrefixSuffixModel> prefixSuffixList = new List<PrefixSuffixModel>()
            {
                new PrefixSuffixModel(code, name)
            };

            List<PtDiseaseModel> ptDiseaseModels = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId, ptId, seqNo, byomeiCd, sortNo, prefixSuffixList, byomei, startDate, tenkiKbn, tenkiDate, syubyoKbn,
                                   sikkanKbn, nanbyoCd, isNodspRece, isNodspKarte, isDeleted, id, isImportant, sinDate, icd10, icd102013,
                                   icd1012013, icd1022013, hokenPid, hosokuCmt, togetuByomei, delDate)
            };

            mockIPtDiseaseRepository.Setup(x => x.GetPatientDiseaseList(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DiseaseViewType>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered) => ptDiseaseModels);
            mockIUserConfRepository.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((int hpId, int userId, int groupCd, int grpItemCd, int grpItemEdaNo) => 1);

            GetDiseaseReceListInputData inputData = new GetDiseaseReceListInputData(hpId, userId, ptId, hokenId, sinYm);

            // Act
            var result = getDiseaseReceListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.DiseaseReceList.Any(x => x.Byomei == "(主)(特て)(難)前頭部(" + hosokuCmt + ")") && result.Status == GetDiseaseReceListStatus.Successed);
        }
    }
}
