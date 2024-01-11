using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
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

        [Test]
        public void TC_005_Validation()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();
            var mockLoggingHandle = new Mock<ILoggingHandler>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            //Act
           var resutl =  savePatientInfo.Validation();
        }
    }
}
