using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.TodayOdr;
using Moq;
using Domain.Models.AuditLog;
using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Interactor.CalculateService;
using Interactor.Family.ValidateFamilyList;
using Interactor.MedicalExamination.KensaIraiCommon;
using Infrastructure.Options;
using Interactor.MedicalExamination;
using Microsoft.Extensions.Options;
using UseCase.MedicalExamination.SaveMedical;

namespace CloudUnitTest.Interactor.SaveMedical;

public class AddAuditKaikeiSaveDataTest : BaseUT
{
    [Test]
    public void TC_001_AddAuditKaikeiSaveData_TestSuccess_01()
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

        int hpId = It.IsAny<int>();
        int userId = It.IsAny<int>();
        long ptId = It.IsAny<int>();
        int sinDate = It.IsAny<int>();
        long raiinNo = It.IsAny<int>();
        MedicalStateChanged stateChanged = new MedicalStateChanged(
                                               false, // fromRece
                                               false, // odrDrugInChanged
                                               false, // odrOrSyosaisinChanged
                                               false, // todayKarteChanged
                                               false, // nextOdrChanged
                                               false // periodicOdrChanged
                                           );

        // Act
        saveMedicalInteractor.AddAuditKaikeiSaveData(hpId, userId, ptId, sinDate, raiinNo, stateChanged);

        // Assert
        Assert.That(true);
    }

    [Test]
    public void TC_002_AddAuditKaikeiSaveData_TestOdrOrSyosaisinChangedSuccess()
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

        int hpId = It.IsAny<int>();
        int userId = It.IsAny<int>();
        long ptId = It.IsAny<int>();
        int sinDate = It.IsAny<int>();
        long raiinNo = It.IsAny<int>();
        MedicalStateChanged stateChanged = new MedicalStateChanged(
                                               false, // fromRece
                                               false, // odrDrugInChanged
                                               true, // odrOrSyosaisinChanged
                                               false, // todayKarteChanged
                                               false, // nextOdrChanged
                                               false // periodicOdrChanged
                                           );

        // Act
        saveMedicalInteractor.AddAuditKaikeiSaveData(hpId, userId, ptId, sinDate, raiinNo, stateChanged);

        // Assert
        Assert.That(true);
    }

    [Test]
    public void TC_003_AddAuditKaikeiSaveData_TestTodayKarteChangedSuccess()
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

        int hpId = It.IsAny<int>();
        int userId = It.IsAny<int>();
        long ptId = It.IsAny<int>();
        int sinDate = It.IsAny<int>();
        long raiinNo = It.IsAny<int>();
        MedicalStateChanged stateChanged = new MedicalStateChanged(
                                               false, // fromRece
                                               false, // odrDrugInChanged
                                               false, // odrOrSyosaisinChanged
                                               true, // todayKarteChanged
                                               false, // nextOdrChanged
                                               false // periodicOdrChanged
                                           );

        // Act
        saveMedicalInteractor.AddAuditKaikeiSaveData(hpId, userId, ptId, sinDate, raiinNo, stateChanged);

        // Assert
        Assert.That(true);
    }

    [Test]
    public void TC_004_AddAuditKaikeiSaveData_TestNextOdrChangedSuccess()
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

        int hpId = It.IsAny<int>();
        int userId = It.IsAny<int>();
        long ptId = It.IsAny<int>();
        int sinDate = It.IsAny<int>();
        long raiinNo = It.IsAny<int>();
        MedicalStateChanged stateChanged = new MedicalStateChanged(
                                               false, // fromRece
                                               false, // odrDrugInChanged
                                               false, // odrOrSyosaisinChanged
                                               false, // todayKarteChanged
                                               true, // nextOdrChanged
                                               false // periodicOdrChanged
                                           );

        // Act
        saveMedicalInteractor.AddAuditKaikeiSaveData(hpId, userId, ptId, sinDate, raiinNo, stateChanged);

        // Assert
        Assert.That(true);
    }

    [Test]
    public void TC_005_AddAuditKaikeiSaveData_TestPeriodicOdrChangedSuccess()
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

        int hpId = It.IsAny<int>();
        int userId = It.IsAny<int>();
        long ptId = It.IsAny<int>();
        int sinDate = It.IsAny<int>();
        long raiinNo = It.IsAny<int>();
        MedicalStateChanged stateChanged = new MedicalStateChanged(
                                               false, // fromRece
                                               false, // odrDrugInChanged
                                               false, // odrOrSyosaisinChanged
                                               false, // todayKarteChanged
                                               false, // nextOdrChanged
                                               true // periodicOdrChanged
                                           );

        // Act
        saveMedicalInteractor.AddAuditKaikeiSaveData(hpId, userId, ptId, sinDate, raiinNo, stateChanged);

        // Assert
        Assert.That(true);
    }
}
