using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.PatientInfor;
using Moq;
using NUnit.Framework;
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
            var resutl = savePatientInfo.Validation(inputData);

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

        [Test]
        public void TC_022_Validation_InvalidZokugara()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @gmail.com",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "samplesample samplesample samplesample samplesample samplesample samplesample samplesample @mail.com",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Zokugara` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidZokugara));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_023_Validation_InvalidJob()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "Sample Job Sample Job Sample Job Sample41",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "Sample Job Sample Job Sample Job Sample2",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.Job` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidJob));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_024_Validation_InvalidRenrakuPost()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6567",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuPost` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuPost));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_025_Validation_InvalidRenrakuAddress1()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R1",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuAddress1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuAddress1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_026_Validation_InvalidRenrakuAddress2()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R1",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample Renraku Address Sample R",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuAddress2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuAddress2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_027_Validation_InvalidRenrakuTel()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-1234",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuTel` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuTel));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_028_Validation_InvalidRenrakuMemo()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo12",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo1",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.RenrakuMemo` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidRenrakuMemo));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_029_Validation_InvalidOfficeName()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo12",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo Sample Renraku Memo1",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeName` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeName));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_030_Validation_InvalidOfficePost()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-2121",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficePost` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficePost));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_031_Validation_InvalidOfficeAddress1()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sampl",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Samp",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeAddress1` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeAddress1));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_032_Validation_InvalidOfficeAddress2()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sampl",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Sample Office Address 1 Samp",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeAddress2` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeAddress2));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_033_Validation_InvalidOfficeTel()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-999",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeTel` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeTel));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_034_Validation_InvalidOfficeMemo()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sample",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sample Office Memo Sampl",
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
            Assert.That(resutl1.First().Message, Is.EqualTo("`Patient.OfficeMemo` property is invalid"));
            Assert.That(resutl1.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidOfficeMemo));
            Assert.IsNotNull(resutl2);
            Assert.That(resutl2.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_035_Validation_Hoken()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
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
                                  hokenId: 77,
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

            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel
                (
                    hpId : 1,
                    ptId: 1,
                    ptBirthday: 19901212,
                    seqNo: 999,
                    hokenSbtCd: 45677,
                    hokenPid: 30,
                    hokenKbn: 1,
                    hokenMemo: "",
                    sinDate: 20240101,
                    startDate: 2030101,
                    endDate: 20240101,
                    hokenId: 40,
                    kohi1Id: 30,
                    kohi2Id: 20,
                    kohi3Id: 10,
                    kohi4Id : 50,
                    isAddNew: false,
                    isDeleted: 0,
                    hokenPatternSelected: true
                ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData1 = new SavePatientInfoInputData(patientInfoSaveModel1, new(), new(), insurances, hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData1.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl1 = savePatientInfo.Validation(inputData1);

            var resutl = resutl1.ToList();
            Assert.IsNotNull(resutl1);
            Assert.That(resutl1.Count, Is.EqualTo(5));
            Assert.That(resutl[0].Message, Is.EqualTo("`Insurances[0].HokenId` property is invalid"));
            Assert.That(resutl[0].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidHokenId));
            Assert.That(resutl[1].Message, Is.EqualTo("`Insurances[0].Kohi1Id` property is invalid"));
            Assert.That(resutl[1].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi1Id));
            Assert.That(resutl[2].Message, Is.EqualTo("`Insurances[0].Kohi2Id` property is invalid"));
            Assert.That(resutl[2].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi2Id));
            Assert.That(resutl[3].Message, Is.EqualTo("`Insurances[0].Kohi3Id` property is invalid"));
            Assert.That(resutl[3].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi3Id));
            Assert.That(resutl[4].Message, Is.EqualTo("`Insurances[0].Kohi4Id` property is invalid"));
            Assert.That(resutl[4].Code, Is.EqualTo(SavePatientInforValidationCode.ÍnuranceInvalidKohi4Id));
        }

        [Test]
        public void TC_036_Validation_PtKyuseiInvalidSeqNo()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
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
                                  hokenId: 77,
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

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: -1,
                        kanaName: "Sample KanaName",
                        name: "Sample Name",
                        endDate: 20241212
                    ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel1, ptKyuseis, new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("`PtKyuseis[0].SeqNo` property is invalid"));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidSeqNo));
        }

        [Test]
        public void TC_037_Validation_PtKyuseiInvalidKanaName()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
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
                                  hokenId: 77,
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

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: 0,
                        kanaName: "",
                        name: "Sample Name",
                        endDate: 20241212
                    ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel1, ptKyuseis, new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(1));
            Assert.That(resutl.First().Message, Is.EqualTo("`PtKyuseis[0].KanaName` property is required"));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidKanaName));
        }

        [Test]
        public void TC_038_Validation_PtKyuseiInvalidName()
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
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Renraku",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999-888-99",
                                            officeMemo: "Sample Office",
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
                                            mail: "samplesample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "987-654-3210",
                                            tel2: "123-456-7890-78",
                                            setanusi: "sample setanusi",
                                            zokugara: "sample zokugara",
                                            job: "sample job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-656",
                                            renrakuAddress1: "Sample Renraku Address",
                                            renrakuAddress2: "Sample Renraku Address",
                                            renrakuTel: "555-1234555-123",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample officeName",
                                            officePost: "543-212",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 1",
                                            officeTel: "888-9999-888-99",
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
                                  hokenId: 77,
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

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: 0,
                        kanaName: "Sample KanaName",
                        name: "Sample KanaName Sample KanaName Sample KanaName Sample KanaName Sample KanaName Sample KanaName Sampl",
                        endDate: 20241212
                    ),

                new PtKyuseiModel
                    (
                        hpId:  1,
                        ptId: 999,
                        seqNo: 0,
                        kanaName: "Sample KanaName",
                        name: "",
                        endDate: 20241212
                    ),
            };
            #endregion

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            var inputData = new SavePatientInfoInputData(patientInfoSaveModel1, ptKyuseis, new(), new(), hokenInfs, new(), new(), new(), new(), insuranceScans, new List<int> { 1, 2 }, 9999, 9999);

            inputData.ReactSave.ConfirmSamePatientInf = true;

            mockSystemConf.Setup(x => x.GetSettingValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int input1, int input2, int input3) => 0);
            //Act
            var resutl = savePatientInfo.Validation(inputData);

            Assert.IsNotNull(resutl);
            Assert.That(resutl.Count, Is.EqualTo(2));
            Assert.That(resutl.First().Message, Is.EqualTo("`PtKyuseis[0].Name` property is invalid"));
            Assert.That(resutl.First().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidName));
            Assert.That(resutl.Last().Message, Is.EqualTo("`PtKyuseis[1].Name` property is required"));
            Assert.That(resutl.Last().Code, Is.EqualTo(SavePatientInforValidationCode.PtKyuseiInvalidName));
        }

        [Test]
        public void TC_039_SplitName_WithEmptyName_ShouldSetFirstAndLastNameToEmptyString()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            string name = "";
            string firstName, lastName;

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo(""));
            Assert.That(lastName, Is.EqualTo(""));
        }

        [Test]
        public void TC_040_SplitName_WithSingleName_ShouldSetFirstNameToName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            // Arrange
            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);
            string name = "John";
            string firstName, lastName;

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo("John"));
            Assert.That(lastName, Is.EqualTo(""));
        }

        [Test]
        public void TC_041_SplitName_WithEmptyName_ShouldSplitCorrectly()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            // Arrange
            string name = "John Doe";
            string firstName, lastName;

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo("Doe"));
            Assert.That(lastName, Is.EqualTo("John"));
        }

        [Test]
        public void TC_041_SplitName_WithJapaneseEmptyName_ShouldSplitCorrectly()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);
            // Arrange
            string name = "山田 太郎";
            string firstName, lastName;

            // Act
            savePatientInfo.SplitName(name, out firstName, out lastName);

            // Assert
            Assert.That(firstName, Is.EqualTo("太郎"));
            Assert.That(lastName, Is.EqualTo("山田"));
        }

        [Test]
        public void TC_042_IsValidAgeCheckConfirm_ValidAgeCheck()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            int ageCheck = 18;
            int confirmDate = 20240114;
            int birthDay = 20000102;
            int sinDay = 20240114;

            // Act
            var result = savePatientInfo.IsValidAgeCheckConfirm(ageCheck, confirmDate, birthDay, sinDay);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_043_IsValidAgeCheckConfirm_InvalidAgeCheck()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            int ageCheck = 21;
            int confirmDate = 0;
            int birthDay = 20000102;
            int sinDay = 20220114;

            // Act
            var result = savePatientInfo.IsValidAgeCheckConfirm(ageCheck, confirmDate, birthDay, sinDay);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void TC_044_IsValidAgeCheckConfirm_BirthdayAfterSecondDay()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            int ageCheck = 18;
            int confirmDate = 20240114; // YYYYMMDD format
            int birthDay = 20000202;    // YYYYMMDD format (2nd day of February)
            int sinDay = 20240114;      // YYYYMMDD format

            // Act
            var result = savePatientInfo.IsValidAgeCheckConfirm(ageCheck, confirmDate, birthDay, sinDay);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TC_045_IsValidKanjiName_InvalidFirstNameKana_InvalidFirstNameKanji()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = string.Empty; 
            var kanjiName = string.Empty;

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 1);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Message, Is.EqualTo("カナを入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKana));
            Assert.That(result.Last().Message, Is.EqualTo("氏名を入力してください。"));
            Assert.That(result.Last().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidFirstNameKanji));
        }

        [Test]
        public void TC_046_IsValidKanjiName_InvalidLastKanaName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "Ariga";
            var kanjiName = "救按 土";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("カナを入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanaName));
        }

        [Test]
        public void TC_047_IsValidKanjiName_InvalidLastKanjiName()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "Ariga To";
            var kanjiName = "救按";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("氏名を入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanjiName));
        }

        [Test]
        public void TC_048_IsValidKanjiName_InvalidLastKanjiNameLength()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "Ariga To";
            var kanjiName = "KanjiNameKanjiNameKanjiNameKanj Sample";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("患者姓は３０文字以下を入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanjiNameLength));
        }

        [Test]
        public void TC_049_IsValidKanjiName_InvalidLastKanaNameLength()
        {
            //Mock
            var mockPatientInfo = new Mock<IPatientInforRepository>();
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockPtDisease = new Mock<IPtDiseaseRepository>();
            var mockAmazonS3 = new Mock<IAmazonS3Service>();

            var savePatientInfo = new SavePatientInfoInteractor(TenantProvider, mockPatientInfo.Object, mockSystemConf.Object, mockAmazonS3.Object, mockPtDisease.Object);

            // Arrange
            var hpId = 1;
            var kanaName = "SampleKanaNameSampleS Sample";
            var kanjiName = "KanjiNameKanjiNameKanjiNameKan Sample";

            var react = new ReactSavePatientInfo();

            //Mock
            mockSystemConf.Setup(x => x.GetSettingValue(1017, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            mockSystemConf.Setup(x => x.GetSettingValue(1003, 0, hpId))
            .Returns((int input1, int input2, int input3) => 0);

            // Act
            var resultIEnum = savePatientInfo.IsValidKanjiName(kanaName, kanjiName, hpId, react);

            var result = resultIEnum.ToList();
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("患者姓（カナ）は２０文字以下を入力してください。"));
            Assert.That(result.First().Code, Is.EqualTo(SavePatientInforValidationCode.InvalidLastKanaNameLength));
        }
    }
}
