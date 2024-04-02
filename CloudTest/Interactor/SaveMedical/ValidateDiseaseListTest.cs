using Domain.Models.AuditLog;
using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInfs;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Interactor.CalculateService;
using Interactor.Family.ValidateFamilyList;
using Interactor.MedicalExamination.KensaIraiCommon;
using Interactor.MedicalExamination;
using Microsoft.Extensions.Options;
using Moq;
using Domain.Models.Diseases;
using UseCase.Diseases.Upsert;
using System.Drawing;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class ValidateDiseaseListTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_ValidateDiseaseList_InValid()
        {
            //Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIKaRepository = new Mock<IKaRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();
            var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
            var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
            var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
            var mockICalculateService = new Mock<ICalculateService>();
            var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
            var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
            var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
            var mockISystemConfRepository = new Mock<ISystemConfRepository>();
            var mockIAuditLogRepository = new Mock<IAuditLogRepository>();

            var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, TenantProvider, mockIOrdInfRepository.Object,
                                                                  mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object,
                                                                  mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                  mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object,
                                                                  mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object,
                                                                  mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

            int hpId = 1;
            string byomeiCd = "";
            string byomei = "";
            int sikkanKbn = 0;
            string icd10 = "";
            string icd102013 = "";
            string icd1012013 = "";
            string icd1022013 = "";

            List<PtDiseaseModel> ptDiseases = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(byomeiCd, byomei, sikkanKbn, icd10, icd102013, icd1012013,icd1022013)
            };

            // Act
            var result = saveMedicalInteractor.ValidateDiseaseList(hpId, ptDiseases);

            // Assert
            Assert.That(result != UpsertPtDiseaseListStatus.Valid && result != UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist);
        }

        [Test]
        public void TC_002_SaveMedicalInteractor_ValidateDiseaseList_PtDiseaseListPtIdNoExist()
        {
            //Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIKaRepository = new Mock<IKaRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();
            var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
            var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
            var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
            var mockICalculateService = new Mock<ICalculateService>();
            var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
            var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
            var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
            var mockISystemConfRepository = new Mock<ISystemConfRepository>();
            var mockIAuditLogRepository = new Mock<IAuditLogRepository>();

            var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, TenantProvider, mockIOrdInfRepository.Object,
                                                                  mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object,
                                                                  mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                  mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object,
                                                                  mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object,
                                                                  mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

            int hpId = 1;
            long ptId = 9999999999;
            long seqNo = 999;
            string byomeiCd = "kaito";
            int sortNo = 0;
            List<PrefixSuffixModel> prefixSuffixList = new();
            string byomei = "kaito";
            int startDate = 20240328;
            int tenkiKbn = 1;
            int tenkiDate = 20240328;
            int syubyoKbn = 0;
            int sikkanKbn = 0;
            int nanbyoCd = 9;
            int isNodspRece = 0;
            int isNodspKarte = 1;
            int isDeleted = 0;
            long id = 1;
            int isImportant = 0;
            int sinDate = 20240329;
            string icd10 = "";
            string icd102013 = "";
            string icd1012013 = "";
            string icd1022013 = "";
            int hokenPid = 0;
            string hosokuCmt = "kaito";
            int togetuByomei = 0;
            int delDate = 0;

            List<PtDiseaseModel> ptDiseases = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId, ptId, seqNo, byomeiCd, sortNo, prefixSuffixList, byomei, startDate, tenkiKbn, tenkiDate, syubyoKbn,
                                   sikkanKbn, nanbyoCd, isNodspRece, isNodspKarte, isDeleted, id, isImportant, sinDate, icd10, icd102013, 
                                   icd1012013, icd1022013, hokenPid, hosokuCmt, togetuByomei, delDate)
            };

            // Act
            var result = saveMedicalInteractor.ValidateDiseaseList(hpId, ptDiseases);

            //Assert
            Assert.That(result == UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist);
        }

        [Test]
        public void TC_003_SaveMedicalInteractor_ValidateDiseaseList_Valid()
        {
            //Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIKaRepository = new Mock<IKaRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();
            var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
            var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
            var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
            var mockICalculateService = new Mock<ICalculateService>();
            var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
            var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
            var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
            var mockISystemConfRepository = new Mock<ISystemConfRepository>();
            var mockIAuditLogRepository = new Mock<IAuditLogRepository>();

            var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, TenantProvider, mockIOrdInfRepository.Object,
                                                                  mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object,
                                                                  mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                  mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object,
                                                                  mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object,
                                                                  mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            List<long> ptIds = new List<long>();
            var ptIdTest = tenantTracking.PtInfs.FirstOrDefault().PtId;
            ptIds.Add(ptIdTest);

            int hpId = 1;
            long ptId = ptIdTest;
            long seqNo = 999;
            string byomeiCd = "kaito";
            int sortNo = 0;
            List<PrefixSuffixModel> prefixSuffixList = new();
            string byomei = "kaito";
            int startDate = 20240328;
            int tenkiKbn = 1;
            int tenkiDate = 20240328;
            int syubyoKbn = 0;
            int sikkanKbn = 0;
            int nanbyoCd = 9;
            int isNodspRece = 0;
            int isNodspKarte = 1;
            int isDeleted = 0;
            long id = 1;
            int isImportant = 0;
            int sinDate = 20240329;
            string icd10 = "";
            string icd102013 = "";
            string icd1012013 = "";
            string icd1022013 = "";
            int hokenPid = 0;
            string hosokuCmt = "kaito";
            int togetuByomei = 0;
            int delDate = 0;

            List<PtDiseaseModel> ptDiseases = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(hpId, ptId, seqNo, byomeiCd, sortNo, prefixSuffixList, byomei, startDate, tenkiKbn, tenkiDate, syubyoKbn,
                                   sikkanKbn, nanbyoCd, isNodspRece, isNodspKarte, isDeleted, id, isImportant, sinDate, icd10, icd102013,
                                   icd1012013, icd1022013, hokenPid, hosokuCmt, togetuByomei, delDate)
            };

            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, ptIds)).Returns((int hpId, List<long> ptIds) => true);

            // Act
            var result = saveMedicalInteractor.ValidateDiseaseList(hpId, ptDiseases);

            //Assert
            Assert.That(result == UpsertPtDiseaseListStatus.Valid);
        }
    }
}