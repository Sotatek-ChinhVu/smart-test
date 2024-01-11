using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CommonChecker.CommonMedicalCheck;
using Interactor.PatientInfor;
using Moq;
using System.Text;
using UseCase.PatientInfor.Save;

namespace CloudUnitTest.Interactor.PatientInfo
{
    public class SavePatientInfoInteractorTest : BaseUT
    {
        [Test]
        public void TC_001_CloneByomei_PtByomeis_Count_Greater_Than_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });


            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && !p.IsAddNew));
            Assert.That(result == true);
        }

        [Test]
        public void TC_002_CloneByomei_Count_Greater_Is_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });


            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel));
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && !p.IsAddNew) == false);
            Assert.That(result == false);
        }

        /// <summary>
        ///HokenInfs not any p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel
        /// </summary>
        [Test]
        public void TC_003_CloneByomei_HokenInfs_Not_Any_newHokenInfs()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });


            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && p.IsAddNew && !p.IsEmptyModel) == false);
            Assert.That(inputData.HokenInfs.Any(p => p.IsDeleted == DeleteTypes.None && !p.IsAddNew) == true);
            Assert.That(result == false);
        }

        [Test]
        public void TC_004_CloneByomei_ReactSave_ConfirmCloneByomei_Is_True()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var commonMedicalCheck = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };


            mockPtDisease.Setup(x => x.GetPtByomeisByHokenId(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>()))
            .Returns((int input1, long input2, int input3) => new List<PtDiseaseModel>() { new PtDiseaseModel() });

            var reactSave = new ReactSavePatientInfo()
            {

                ConfirmCloneByomei = true
            };

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), reactSave, new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            // Act

            var result = commonMedicalCheck.CloneByomei(inputData);

            // Assert
            Assert.That(inputData.ReactSave.ConfirmCloneByomei == true);
            Assert.That(result == false);
        }

        private static Stream GetSampleFileStream()
        {
            byte[] data = Encoding.UTF8.GetBytes("Sample file content.");
            return new MemoryStream(data);
        }

        /// <summary>
        /// model.ReactSave.ConfirmSamePatientInf = false
        /// samePatientInf count = 2
        /// </summary>
        [Test]
        public void TC_005_Validation_ConfirmSamePatientInf_SamePatientInf_GreaterThan_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            var resultFindSamePatientTest = new List<PatientInforModel>()
            {
               new PatientInforModel(
                        hpId: 123,
                        ptId: 987654,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
               new PatientInforModel(
                        hpId: 123,
                        ptId: 987655,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
            };
            mockPatientInfo.Setup(x => x.FindSamePatient(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, string input2, int input3, int input4) => resultFindSamePatientTest);
            //Act
            var resutl =  savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("同姓同名の患者が既に登録されています。\r\n登録しますか？\r\n患者番号：1001     \r\n患者番号：1001     "));
            Assert.That(resutl.First().Type, Is.EqualTo(3));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSamePatient));
        }

        /// <summary>
        /// model.ReactSave.ConfirmSamePatientInf = false
        /// samePatientInf count = 1
        /// </summary>
        [Test]
        public void TC_006_Validation_ConfirmSamePatientInf_SamePatientInf_GreaterThan_0_Msg_NotIN_NewLine()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            var resultFindSamePatientTest = new List<PatientInforModel>()
            {
               new PatientInforModel(
                        hpId: 123,
                        ptId: 987654,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
            };
            mockPatientInfo.Setup(x => x.FindSamePatient(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, string input2, int input3, int input4) => resultFindSamePatientTest);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("同姓同名の患者が既に登録されています。\r\n登録しますか？\r\n患者番号：1001     "));
            Assert.That(resutl.First().Type, Is.EqualTo(3));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSamePatient));
        }

        /// <summary>
        /// model.ReactSave.ConfirmSamePatientInf = false
        /// FindSamePatient item.PtId != model.Patient.PtId count is 0
        /// samePatientInf count = 0
        /// </summary>
        [Test]
        public void TC_007_Validation_ConfirmSamePatientInf_SamePatientInf_Is_0()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            var resultFindSamePatientTest = new List<PatientInforModel>()
            {
               new PatientInforModel(
                        hpId: 123,
                        ptId: 123456,
                        referenceNo: 56789,
                        seqNo: 1,
                        ptNum: 1001,
                        kanaName: "あなたのテスト名前",
                        name: "Your Test Name",
                        sex: 1,
                        birthday: 19900101,
                        limitConsFlg: 0,
                        isDead: 0,
                        deathDate: 0,
                        homePost: "123-4567",
                        homeAddress1: "123 Test Street",
                        homeAddress2: "Apt. 45",
                        tel1: "123-555-7890",
                        tel2: "987-654-3210",
                        mail: "test@example.com",
                        setanusi: "Test Setanusi",
                        zokugara: "Test Zokugara",
                        job: "Software Engineer",
                        renrakuName: "Emergency Contact",
                        renrakuPost: "456 Emergency Street",
                        renrakuAddress1: "Emergency City",
                        renrakuAddress2: "Emergency State",
                        renrakuTel: "555-123-7890",
                        renrakuMemo: "Emergency Contact Memo",
                        officeName: "Test Hospital",
                        officePost: "789 Hospital Street",
                        officeAddress1: "Hospital City",
                        officeAddress2: "Hospital State",
                        officeTel: "789-456-1230",
                        officeMemo: "Hospital Memo",
                        isRyosyoDetail: 1,
                        primaryDoctor: 123,
                        isTester: 0,
                        mainHokenPid: 456,
                        memo: "Test Memo",
                        lastVisitDate: 20220101,
                        firstVisitDate: 20190101,
                        rainCount: 3,
                        comment: "Test Comment",
                        sinDate: 20220101,
                        isShowKyuSeiName: false
                    ),
            };
            mockPatientInfo.Setup(x => x.FindSamePatient(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, string input2, int input3, int input4) => resultFindSamePatientTest);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// model.Patient.PtId == 0 && 
        /// model.Patient.PtNum != 0 && 
        /// _systemConfRepository.GetSettingValue(1001, 0, model.Patient.HpId) == 1 && 
        /// !CIUtil.PtNumCheckDigits(model.Patient.PtNum
        /// </summary>
        [Test]
        public void TC_008_Validation_InvalidPtNumCheckDigits()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 0,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 1);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("患者番号が正しくありません。"));
            Assert.That(resutl.First().Type, Is.EqualTo(2));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidPtNumCheckDigits));
        }

        [Test]
        public void TC_009_Validation_InvalidHpId()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 0,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: -1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HpId` property is invalid"));
            Assert.That(resutl1.First().Type, Is.EqualTo(2));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHpId));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("`Patient.HpId` property is invalid"));
            Assert.That(resutl2.First().Type, Is.EqualTo(2));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHpId));
        }

        [Test]
        public void TC_010_Validation_InvalidKanaName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Nam",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(2));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.KanaName` property is invalid"));
            Assert.That(resutl1.Last().Message, Is.EqualTo("患者名（カナ）は２０文字以下を入力してください。"));
            Assert.That(resutl1.First().Type, Is.EqualTo(2));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidKanaName));
            Assert.That(resutl1.Last().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanaLength));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
        }

        [Test]
        public void TC_011_Validation_InvalidName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Name Sample Kana Nam",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(2));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Name` property is invalid"));
            Assert.That(resutl1.Last().Message, Is.EqualTo("患者名は３０文字以下を入力してください。"));
            Assert.That(resutl1.First().Type, Is.EqualTo(2));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidName));
            Assert.That(resutl1.Last().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanjiLength));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("患者名は３０文字以下を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanjiLength));
        }

        [Test]
        public void TC_012_Validation_InvalidBirthday()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 0,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 0,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 0,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(0));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("生年月日を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidBirthday));
        }

        [Test]
        public void TC_013_Validation_InvalidSex()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 0,
                                            birthday: 20001211,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 3,
                                            birthday: 20011111,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("性別を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSex));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("性別を入力してください。"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSex));
        }

        [Test]
        public void TC_014_Validation_InvalidIsDead()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: -1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 2,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-45",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.IsDead` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidIsDead));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(1));
            Assert.That(resutl2.First().Message, Is.EqualTo("`Patient.IsDead` property is invalid"));
            Assert.That(resutl2.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidIsDead));
        }

        [Test]
        public void TC_015_Validation_InvalidHomePost()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HomePost` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHomePost));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_016_Validation_InvalidHomeAddress1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home11",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HomeAddress1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHomeAddress1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_017_Validation_InvalidHomeAddress2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home11",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home Address 1 Sample Home1",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.HomeAddress2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidHomeAddress2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_018_Validation_InvalidTel1()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890-789",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890-78",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Tel1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidTel1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_019_Validation_InvalidTel2()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-789",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Tel2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidTel2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_020_Validation_InvalidMail()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Mail` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidMail));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_021_Validation_InvalidSetanusi()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            //Setup Data
            #region Data Test
            var patientInfoSaveModel1 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 3344,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 20001211,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample@gmail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890",
                                            setanusi: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @gmail.com",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var patientInfoSaveModel2 = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 1231,
                                            ptNum: 789012,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 2,
                                            birthday: 20011111,
                                            isDead: 1,
                                            deathDate: 0,
                                            mail: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Sample Memo"
            );

            var insuranceScanModel = new InsuranceScanModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            seqNo: 789012,
                                            hokenGrp: 1,
                                            hokenId: 2,
                                            fileName: "SampleFile.pdf",
                                            file: GetSampleFileStream(),
                                            isDeleted: 0,
                                            updateTime: "2024-01-10T12:34:56"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC123",
                                  kigo: "K123",
                                  bango: "B567",
                                  edaNo: "E789",
                                  honkeKbn: 4,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 1,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7890",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>(),
                                  listRousaiTenki: new List<RousaiTenkiModel>(),
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: false,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);
            var inputData2 = new SavePatientInfoInputData(patientInfoSaveModel2, new(), new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;
            inputData2.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);
            var resutl2 = savePatientInfo.Validation(inputData2);

            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(1));
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Setanusi` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidSetanusi));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }
    }
}
