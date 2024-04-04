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
using Helper.Constants;

namespace CloudUnitTest.Interactor.SaveMedical;

public class CheckCommonTest : BaseUT
{
    #region CheckCommon
    [Test]
    public void TC_001_CheckCommon_TestInvalidHpIdWithHpIdIs0()
    {        
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { 0 }, new() { ptId }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHpId);
    }

    [Test]
    public void TC_002_CheckCommon_TestInvalidHpIdWithHpIdListHasMultipleParams()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId, random.Next(999, 99999) }, new() { ptId }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHpId);
    }

    [Test]
    public void TC_003_CheckCommon_TestInvalidPtIdWithPtIdIs0()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { 0 }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidPtId);
    }

    [Test]
    public void TC_004_CheckCommon_TestInvalidPtIdWithPtIdListHasMultipleParams()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId, random.Next(999, 99999) }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidPtId);
    }

    [Test]
    public void TC_005_CheckCommon_TestInvalidRaiinNoWithRaiinNoIs0()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { 0 }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidRaiinNo);
    }

    [Test]
    public void TC_006_CheckCommon_TestInvalidPtIdWithRaiinNoListHasMuiltipeParams()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { raiinNo, random.Next(999, 99999) }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidRaiinNo);
    }

    [Test]
    public void TC_007_CheckCommon_TestInvalidSinDateWithSinDateIs0()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { raiinNo }, new() { 0 }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinDate);
    }

    [Test]
    public void TC_008_CheckCommon_TestInvalidSinDateWithSinDateListHasMultipleParams()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { raiinNo }, new() { sinDate, random.Next(999, 99999) }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinDate);
    }

    [Test]
    public void TC_009_CheckCommon_TestHpIdNoExist()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId))
       .Returns((int hpId) => false);

        mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new() { ptId }))
       .Returns((int hpId, List<long> ptIds) => false);

        mockIReceptionRepository.Setup(finder => finder.CheckListNo(hpId, new() { raiinNo }))
       .Returns((int hpId, List<long> raininNos) => false);

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.HpIdNoExist);
    }

    [Test]
    public void TC_010_CheckCommon_TestPtIdNoExist()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId))
       .Returns((int hpId) => true);

        mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new() { ptId }))
       .Returns((int hpId, List<long> ptIds) => false);

        mockIReceptionRepository.Setup(finder => finder.CheckListNo(hpId, new() { raiinNo }))
       .Returns((int hpId, List<long> raininNos) => false);

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.PtIdNoExist);
    }

    [Test]
    public void TC_011_CheckCommon_TestRaiinIdNoExist()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId))
       .Returns((int hpId) => true);

        mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new() { ptId }))
       .Returns((int hpId, List<long> ptIds) => true);

        mockIReceptionRepository.Setup(finder => finder.CheckListNo(hpId, new() { raiinNo }))
       .Returns((int hpId, List<long> raininNos) => false);

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.RaiinIdNoExist);
    }

    [Test]
    public void TC_012_CheckCommon_TestSuccess()
    {
        // Arrange
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

        var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        bool isCheckOrder = true;

        mockIHpInfRepository.Setup(finder => finder.CheckHpId(hpId))
       .Returns((int hpId) => true);

        mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, new() { ptId }))
       .Returns((int hpId, List<long> ptIds) => true);

        mockIReceptionRepository.Setup(finder => finder.CheckListNo(hpId, new() { raiinNo }))
       .Returns((int hpId, List<long> raininNos) => true);

        // Act
        var result = saveMedicalInteractor.CheckCommon(isCheckOrder, new() { hpId }, new() { ptId }, new() { raiinNo }, new() { sinDate }, hpId, ptId, raiinNo);

        // Assert
        Assert.That(result == RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid);
    }
    #endregion
}
