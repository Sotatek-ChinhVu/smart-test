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
using UseCase.MedicalExamination.SaveMedical;
using UseCase.Diseases.Upsert;
using UseCase.Family;
using UseCase.FlowSheet.Upsert;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.NextOrder;
using Domain.Models.MonshinInf;
using Helper.Constants;

namespace CloudUnitTest.Interactor.SaveMedical;

public class CheckRaiinInfTest : BaseUT
{
    [Test]
    public void TC_001_CheckRaiinInf_TestInvalidSyosaiKbn()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = 9;
        int jikanKbn = random.Next(999, 99999);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(999, 99999);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "20220202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSyosaiKbn);
    }

    [Test]
    public void TC_002_CheckRaiinInf_TestInvalidJikanKbn()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = 9;
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(999, 99999);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "20220202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidJikanKbn);
    }

    [Test]
    public void TC_003_CheckRaiinInf_TestInvalidHokenPid()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = 0;
        int santeiKbn = random.Next(999, 99999);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "20220202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHokenPid);
    }

    [Test]
    public void TC_004_CheckRaiinInf_TestInvalidSanteiKbn()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = 3;
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "20220202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSanteiKbn);
    }

    [Test]
    public void TC_005_CheckRaiinInf_TestInvalidTantoId()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = -1;
        int kaId = random.Next(999, 99999);
        string uketukeTime = "20220202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidTantoId);
    }

    [Test]
    public void TC_006_CheckRaiinInf_TestInvalidKaId()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = -1;
        string uketukeTime = "20220202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidKaId);
    }

    [Test]
    public void TC_007_CheckRaiinInf_TestInvalidUKetukeTime()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "20220202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidUKetukeTime);
    }

    [Test]
    public void TC_008_CheckRaiinInf_TestInvalidSinStartTime()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "202202";
        string sinStartTime = "20220202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinStartTime);
    }

    [Test]
    public void TC_009_CheckRaiinInf_TestInvalidSinEndTime()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "202202";
        string sinStartTime = "202202";
        string sinEndTime = "20220202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();

        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinEndTime);
    }

    [Test]
    public void TC_010_CheckRaiinInf_TestHokenPidNoExist()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "202202";
        string sinStartTime = "202202";
        string sinEndTime = "202202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();
        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        mockIInsuranceRepository.Setup(finder => finder.CheckExistHokenPid(inputData.HpId, inputData.HokenPid))
        .Returns((int hpId, int hokenPid) => false);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.HokenPidNoExist);
    }

    [Test]
    public void TC_011_CheckRaiinInf_TestTatoIdNoExist()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "202202";
        string sinStartTime = "202202";
        string sinEndTime = "202202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();
        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        mockIInsuranceRepository.Setup(finder => finder.CheckExistHokenPid(inputData.HpId, inputData.HokenPid))
        .Returns((int hpId, int hokenPid) => true);

        mockIUserRepository.Setup(finder => finder.CheckExistedUserId(inputData.HpId, inputData.TantoId))
        .Returns((int hpId, int userId) => false);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.TatoIdNoExist);
    }

    [Test]
    public void TC_012_CheckRaiinInf_TestKaIdNoExist()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "202202";
        string sinStartTime = "202202";
        string sinEndTime = "202202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();
        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        mockIInsuranceRepository.Setup(finder => finder.CheckExistHokenPid(inputData.HpId, inputData.HokenPid))
        .Returns((int hpId, int hokenPid) => true);

        mockIUserRepository.Setup(finder => finder.CheckExistedUserId(inputData.HpId, inputData.TantoId))
        .Returns((int hpId, int userId) => true);

        mockIKaRepository.Setup(finder => finder.CheckKaId(inputData.HpId, inputData.KaId))
        .Returns((int hpId, int kaId) => false);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.KaIdNoExist);
    }

    [Test]
    public void TC_013_CheckRaiinInf_TestValid()
    {
        //Setup Data Test
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        // Arrange
        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(99999, 99999999);
        long raiinNo = random.Next(99999, 99999999);
        int sinDate = random.Next(999, 99999);
        int syosaiKbn = random.Next(0, 8);
        int jikanKbn = random.Next(0, 7);
        int hokenPid = random.Next(999, 99999);
        int santeiKbn = random.Next(0, 2);
        int tantoId = random.Next(999, 99999);
        int kaId = random.Next(999, 99999);
        string uketukeTime = "202202";
        string sinStartTime = "202202";
        string sinEndTime = "202202";
        byte status = 0;
        List<OdrInfItemInputData> odrItems = new();
        KarteItemInputData karteInf = new();
        int userId = random.Next(999, 99999);
        bool isSagaku = false;
        bool autoSaveKensaIrai = false;
        FileItemInputItem fileItem = new();
        List<FamilyItem> listFamily = new();
        List<NextOrderItem> nextOrderItems = new();
        SpecialNoteItem specialNoteItem = new();
        List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems = new();
        List<UpsertFlowSheetItemInputData> flowSheetItems = new();
        MonshinInforModel monshins = new();
        MedicalStateChanged stateChanged = new();
        SaveMedicalInputData inputData = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrItems, karteInf, userId, isSagaku, autoSaveKensaIrai, fileItem, listFamily, nextOrderItems, specialNoteItem, upsertPtDiseaseListInputItems, flowSheetItems, monshins, stateChanged);

        mockIInsuranceRepository.Setup(finder => finder.CheckExistHokenPid(inputData.HpId, inputData.HokenPid))
        .Returns((int hpId, int hokenPid) => true);

        mockIUserRepository.Setup(finder => finder.CheckExistedUserId(inputData.HpId, inputData.TantoId))
        .Returns((int hpId, int userId) => true);

        mockIKaRepository.Setup(finder => finder.CheckKaId(inputData.HpId, inputData.KaId))
        .Returns((int hpId, int kaId) => true);

        // Act
        var result = saveMedicalInteractor.CheckRaiinInf(inputData);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid);
    }

}
