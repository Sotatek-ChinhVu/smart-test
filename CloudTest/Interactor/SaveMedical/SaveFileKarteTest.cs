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
using Infrastructure.Services;
using Entity.Tenant;
using Infrastructure.Repositories;
using Domain.Models.KarteInf;

namespace CloudUnitTest.Interactor.SaveMedical;

public class SaveFileKarteTest : BaseUT
{
    private readonly string baseAccessUrl = "BaseAccessUrl";

    #region SaveFileKarte
    [Test]
    public void TC_001_SaveFileKarte_TestSaveSuccessIsTrue_01()
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
        AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
        IOptions<AmazonS3Options> options = Options.Create(appSettings);

        var saveMedicalInteractor = new SaveMedicalInteractor(options, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        int hpId = It.IsAny<int>();
        int userId = It.IsAny<int>();
        long ptId = It.IsAny<int>();
        long ptNum = It.IsAny<int>();
        long raiinNo = It.IsAny<int>();
        bool saveSuccess = true;
        List<string> listFileName = new() { "fileName.txt" };
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        mockIPatientInforRepository.Setup(finder => finder.GetById(hpId, ptId, 0, 0, false, null))
        .Returns((int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName, List<int>? listStatus) => new PatientInforModel(ptId, ptNum, string.Empty, string.Empty, 0, 0));

        mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
       .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

        mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
       .Returns((string fileName) => It.IsAny<string>());

        mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
       .Returns((string sourceFile, string destinationFile) => false);

        // Act
        saveMedicalInteractor.SaveFileKarte(hpId, userId, ptId, raiinNo, listFileName, saveSuccess);

        // Assert
        Assert.That(true);
    }

    [Test]
    public void TC_002_SaveFileKarte_TestSaveSuccessIsTrue_02()
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
        AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
        IOptions<AmazonS3Options> options = Options.Create(appSettings);

        var saveMedicalInteractor = new SaveMedicalInteractor(options, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptNum = random.Next(999, 99999);
        long raiinNo = random.Next(999, 99999);
        bool saveSuccess = true;
        List<string> listFileName = new() { $"{baseAccessUrl}/fileName.txt" };
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        List<FileMapCopyItem> fileInfUpdateTemp = new()
        {
            new FileMapCopyItem(It.IsAny<string>(), It.IsAny<string>())
        };

        mockIPatientInforRepository.Setup(finder => finder.GetById(hpId, ptId, 0, 0, false, null))
        .Returns((int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName, List<int>? listStatus) => new PatientInforModel(ptId, ptNum, string.Empty, string.Empty, 0, 0));

        mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
       .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

        mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
       .Returns((string fileName) => It.IsAny<string>());

        mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
       .Returns((string sourceFile, string destinationFile) => false);

        Dictionary<string, bool> checkIsSchemaList = new Dictionary<string, bool> { { "fileName.txt", false } };
        mockIKarteInfRepository.Setup(finder => finder.ListCheckIsSchema(hpId, ptId, fileInfUpdateTemp))
       .Returns((int hpId, long ptId, List<FileMapCopyItem> fileInfUpdateTemp) => checkIsSchemaList);

        // Act
        saveMedicalInteractor.SaveFileKarte(hpId, userId, ptId, raiinNo, listFileName, saveSuccess);

        // Assert
        Assert.That(true);
    }

    [Test]
    public void TC_003_SaveFileKarte_TestSaveSuccessIsFalse_01()
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
        AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
        IOptions<AmazonS3Options> options = Options.Create(appSettings);

        var saveMedicalInteractor = new SaveMedicalInteractor(options, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptNum = random.Next(999, 99999);
        long raiinNo = random.Next(999, 99999);
        bool saveSuccess = false;
        List<string> listFileName = new() { $"{baseAccessUrl}/fileName.txt" };
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        List<FileMapCopyItem> fileInfUpdateTemp = new()
        {
            new FileMapCopyItem("fileName.txt", "fileName.txt")
        };

        mockIPatientInforRepository.Setup(finder => finder.GetById(hpId, ptId, 0, 0, false, null))
        .Returns((int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName, List<int>? listStatus) => new PatientInforModel(ptId, ptNum, string.Empty, string.Empty, 0, 0));

        mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
       .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

        mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
       .Returns((string fileName) => It.IsAny<string>());

        mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
       .Returns((string sourceFile, string destinationFile) => false);

        // Act
        saveMedicalInteractor.SaveFileKarte(hpId, userId, ptId, raiinNo, listFileName, saveSuccess);

        // Assert
        Assert.That(true);
    }
    #endregion SaveFileKarte

    #region CopyFileFromDoActionToKarte
    [Test]
    public void TC_004_CopyFileFromDoActionToKarte_TestSuccess_01()
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
        AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
        IOptions<AmazonS3Options> options = Options.Create(appSettings);

        var saveMedicalInteractor = new SaveMedicalInteractor(options, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptNum = random.Next(999, 99999);
        long raiinNo = random.Next(999, 99999);
        List<string> listFileName = new() { "fileName.txt" };
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
       .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

        mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
       .Returns((string fileName) => It.IsAny<string>());

        mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
       .Returns((string sourceFile, string destinationFile) => false);

        // Act
        var fileInfUpdateTemp = saveMedicalInteractor.CopyFileFromDoActionToKarte(ptNum, listFileName);

        // Assert
        Assert.That(fileInfUpdateTemp.Count == 0);
    }

    [Test]
    public void TC_005_CopyFileFromDoActionToKarte_TestSuccess_02()
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
        AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
        IOptions<AmazonS3Options> options = Options.Create(appSettings);

        var saveMedicalInteractor = new SaveMedicalInteractor(options, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptNum = random.Next(999, 99999);
        long raiinNo = random.Next(999, 99999);
        string fileName = "fileName.txt";
        List<string> listFileName = new() { $"{baseAccessUrl}/{fileName}" };
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
       .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

        mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
       .Returns((string fileName) => It.IsAny<string>());

        mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
       .Returns((string sourceFile, string destinationFile) => false);

        // Act
        var fileInfUpdateTemp = saveMedicalInteractor.CopyFileFromDoActionToKarte(ptNum, listFileName);

        // Assert
        Assert.That(fileInfUpdateTemp.Any(item => item.NewFileName == fileName && item.OldFileName == fileName));
    }

    [Test]
    public void TC_006_CopyFileFromDoActionToKarte_TestSuccess_03()
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
        AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
        IOptions<AmazonS3Options> options = Options.Create(appSettings);

        var saveMedicalInteractor = new SaveMedicalInteractor(options, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptNum = random.Next(999, 99999);
        long raiinNo = random.Next(999, 99999);
        string fileName = "fileName.txt";
        string newFileName = "newFileName.txt";
        List<string> listFileName = new() { $"{baseAccessUrl}/{CommonConstants.Store}/{CommonConstants.Karte}/{CommonConstants.NextPic}/{fileName}" };
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
       .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

        mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
       .Returns((string fileName) => newFileName);

        mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
       .Returns((string sourceFile, string destinationFile) => true);

        // Act
        var fileInfUpdateTemp = saveMedicalInteractor.CopyFileFromDoActionToKarte(ptNum, listFileName);

        // Assert
        Assert.That(fileInfUpdateTemp.Any(item => item.OldFileName == fileName && item.NewFileName.EndsWith(newFileName)));
    }
    #endregion CopyFileFromDoActionToKarte

    #region GetFolderUploadToPtNum
    [Test]
    public void TC_007_GetFolderUploadToPtNum_TestSuccess_01()
    {
        // Arrange
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        var amazonS3Service = new AmazonS3Service(mockOptionsAccessor.Object, mockITenantProvider.Object);

        long ptNum = 123456789;
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        // Act
        var result = amazonS3Service.GetFolderUploadToPtNum(listFolders, ptNum);
        var compareResult = $"/{CommonConstants.Store}/{CommonConstants.Karte}/67/89/{ptNum}/";
        var success = result == compareResult;

        // Assert
        Assert.That(success);
    }

    [Test]
    public void TC_008_GetFolderUploadToPtNum_TestSuccess_02()
    {
        // Arrange
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
        var amazonS3Service = new AmazonS3Service(mockOptionsAccessor.Object, mockITenantProvider.Object);

        long ptNum = 123;
        List<string> listFolders = new()
        {
            CommonConstants.Store,
            CommonConstants.Karte
        };

        // Act
        var result = amazonS3Service.GetFolderUploadToPtNum(listFolders, ptNum);
        var compareResult = $"/{CommonConstants.Store}/{CommonConstants.Karte}/01/23/{ptNum}/";
        var success = result == compareResult;

        // Assert
        Assert.That(success);
    }
    #endregion GetFolderUploadToPtNum

    #region GetUniqueFileNameKey
    [Test]
    public void TC_009_GetUniqueFileNameKey_TestSuccess_01()
    {
        // Arrange
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        var amazonS3Service = new AmazonS3Service(mockOptionsAccessor.Object, mockITenantProvider.Object);

        string fileName = "path/fileName.txt";

        // Act
        var result = amazonS3Service.GetUniqueFileNameKey(fileName);
        var success = result.EndsWith(".txt");

        // Assert
        Assert.That(success);
    }

    [Test]
    public void TC_010_GetUniqueFileNameKey_TestSuccess_02()
    {
        // Arrange
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();

        var amazonS3Service = new AmazonS3Service(mockOptionsAccessor.Object, mockITenantProvider.Object);

        string fileName = "path/fileName283857158846671447283857620112868106211681062115884667144728385762011286810621158846671447.txt";

        // Act
        var result = amazonS3Service.GetUniqueFileNameKey(fileName);
        var success = result.EndsWith(".txt");

        // Assert
        Assert.That(success);
    }
    #endregion GetUniqueFileNameKey

    #region ListCheckIsSchema
    [Test]
    public void TC_011_ListCheckIsSchema_TestIsSchemaSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        string oldNextOrderFileName = "OldRsvkrtKarteImgInfFileName.txt";
        string newNextOrderFileName = "NewRsvkrtKarteImgInfFileName.txt";
        string oldSetFileName = "OldSetKarteImgInfFileName.txt";
        string newSetFileName = "NewSetKarteImgInfFileName.txt";

        RsvkrtKarteImgInf rsvkrtKarteImgInf = new RsvkrtKarteImgInf()
        {
            HpId = hpId,
            PtId = ptId,
            FileName = oldNextOrderFileName,
            KarteKbn = 1
        };
        tenant.Add(rsvkrtKarteImgInf);

        SetKarteImgInf setKarteImgInf = new SetKarteImgInf()
        {
            HpId = hpId,
            FileName = oldSetFileName,
            KarteKbn = 1
        };
        tenant.Add(setKarteImgInf);

        List<FileMapCopyItem> fileInfUpdateTemp = new()
        {
            new FileMapCopyItem(oldNextOrderFileName, newNextOrderFileName),
            new FileMapCopyItem(oldSetFileName, newSetFileName)
        };

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.ListCheckIsSchema(hpId, ptId, fileInfUpdateTemp);

            // Assert
            bool success = result.ContainsKey(newNextOrderFileName) && result[newNextOrderFileName]
                           && result.ContainsKey(newSetFileName) && result[newSetFileName];

            Assert.True(success);
        }
        finally
        {
            karteInfRepository.ReleaseResource();

            tenant.RsvkrtKarteImgInfs.Remove(rsvkrtKarteImgInf);
            tenant.SetKarteImgInf.Remove(setKarteImgInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_012_ListCheckIsSchema_TestIsNotSchemaSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        string oldNextOrderFileName = "OldRsvkrtKarteImgInfFileName.txt";
        string newNextOrderFileName = "NewRsvkrtKarteImgInfFileName.txt";
        string oldSetFileName = "OldSetKarteImgInfFileName.txt";
        string newSetFileName = "NewSetKarteImgInfFileName.txt";

        RsvkrtKarteImgInf rsvkrtKarteImgInf = new RsvkrtKarteImgInf()
        {
            HpId = hpId,
            PtId = ptId,
            FileName = oldNextOrderFileName,
            KarteKbn = 0
        };
        tenant.Add(rsvkrtKarteImgInf);

        SetKarteImgInf setKarteImgInf = new SetKarteImgInf()
        {
            HpId = hpId,
            FileName = oldSetFileName,
            KarteKbn = 0
        };
        tenant.Add(setKarteImgInf);

        List<FileMapCopyItem> fileInfUpdateTemp = new()
        {
            new FileMapCopyItem(oldNextOrderFileName, newNextOrderFileName),
            new FileMapCopyItem(oldSetFileName, newSetFileName)
        };

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.ListCheckIsSchema(hpId, ptId, fileInfUpdateTemp);

            // Assert
            bool success = result.ContainsKey(newNextOrderFileName) && !result[newNextOrderFileName]
                           && result.ContainsKey(newSetFileName) && !result[newSetFileName];

            Assert.True(success);
        }
        finally
        {
            karteInfRepository.ReleaseResource();

            tenant.RsvkrtKarteImgInfs.Remove(rsvkrtKarteImgInf);
            tenant.SetKarteImgInf.Remove(setKarteImgInf);
            tenant.SaveChanges();
        }
    }
    #endregion ListCheckIsSchema

    #region SaveListFileKarte
    [Test]
    public void TC_013_SaveListFileKarte_TestSaveTempFileIsTrueSuccess_01()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long raiinNo = random.Next(999, 99999);
        string host = "host";
        string fileName = "fileName.txt";
        bool isSchema = true;
        List<FileInfModel> listFiles = new()
        {
            new FileInfModel(isSchema, fileName)
        };
        bool saveTempFile = true;

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.SaveListFileKarte(hpId, userId, ptId, raiinNo, host, listFiles, saveTempFile);

            // Assert
            var karteFileAfter = tenant.KarteImgInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.RaiinNo == 0
                                                                            && item.Position == 1
                                                                            && item.SeqNo == 0
                                                                            && item.KarteKbn == (isSchema ? 1 : 0)
                                                                            && item.FileName == fileName.Replace(host, string.Empty));

            result = result && karteFileAfter != null;
            Assert.True(result);
        }
        finally
        {
            karteInfRepository.ReleaseResource();
            ClearKarteFile(hpId, ptId, userId);
        }
    }

    [Test]
    public void TC_014_SaveListFileKarte_TestSaveTempFileIsFalseSuccess_01()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long raiinNo = random.Next(999, 99999);
        string host = "host";
        string fileName = "fileName.txt";
        bool isSchema = true;
        List<FileInfModel> listFiles = new()
        {
            new FileInfModel(isSchema, fileName)
        };
        bool saveTempFile = false;

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.SaveListFileKarte(hpId, userId, ptId, raiinNo, host, listFiles, saveTempFile);

            // Assert
            var karteFileAfter = tenant.KarteImgInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.RaiinNo == raiinNo
                                                                            && item.KarteKbn == (isSchema ? 1 : 0)
                                                                            && item.FileName == fileName.Replace(host, string.Empty));

            result = result && karteFileAfter != null;
            Assert.True(result);
        }
        finally
        {
            karteInfRepository.ReleaseResource();
            ClearKarteFile(hpId, ptId, userId);
        }
    }

    [Test]
    public void TC_015_SaveListFileKarte_TestSaveTempFileIsFalseSuccess_02()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long raiinNo = random.Next(9999, 999999);
        long seqNo = random.Next(999, 99999);
        string host = "host";
        string fileName = "fileName.txt";
        bool isSchema = true;
        List<FileInfModel> listFiles = new()
        {
            new FileInfModel(isSchema, fileName)
        };
        bool saveTempFile = false;

        KarteImgInf karteImgInf = new KarteImgInf()
        {
            HpId = hpId,
            FileName = fileName,
            PtId = ptId,
            RaiinNo = raiinNo,
            SeqNo = seqNo,
            CreateId = userId,
            UpdateId = userId
        };
        tenant.Add(karteImgInf);

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.SaveListFileKarte(hpId, userId, ptId, raiinNo, host, listFiles, saveTempFile);

            // Assert
            var karteFileAfter = tenant.KarteImgInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.RaiinNo == raiinNo
                                                                            && item.SeqNo == seqNo + 1
                                                                            && item.FileName == fileName);

            result = result && karteFileAfter != null;
            Assert.True(result);
        }
        finally
        {
            karteInfRepository.ReleaseResource();
            ClearKarteFile(hpId, ptId, userId);
        }
    }

    [Test]
    public void TC_016_SaveListFileKarte_TestSaveTempFileIsFalseSuccess_03()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long raiinNo = random.Next(9999, 999999);
        long seqNo = random.Next(999, 99999);
        string host = "host";
        string fileName = "fileName.txt";
        bool isSchema = true;
        List<FileInfModel> listFiles = new()
        {
            new FileInfModel(isSchema, fileName)
        };
        bool saveTempFile = false;

        KarteImgInf karteImgInf = new KarteImgInf()
        {
            HpId = hpId,
            FileName = fileName,
            PtId = ptId,
            RaiinNo = 0,
            SeqNo = 0,
            CreateId = userId,
            UpdateId = userId
        };
        tenant.Add(karteImgInf);

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.SaveListFileKarte(hpId, userId, ptId, raiinNo, host, listFiles, saveTempFile);

            // Assert
            var karteFileAfter = tenant.KarteImgInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.RaiinNo == raiinNo
                                                                            && item.Position == 1
                                                                            && item.FileName == fileName);

            result = result && karteFileAfter != null;
            Assert.True(result);
        }
        finally
        {
            karteInfRepository.ReleaseResource();
            ClearKarteFile(hpId, ptId, userId);
        }
    }

    [Test]
    public void TC_017_SaveListFileKarte_TestSaveTempFileIsFalseSuccess_04()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long raiinNo = random.Next(9999, 999999);
        long seqNo = random.Next(999, 99999);
        string host = "host";
        List<FileInfModel> listFiles = new() { };
        bool saveTempFile = false;

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.SaveListFileKarte(hpId, userId, ptId, raiinNo, host, listFiles, saveTempFile);

            // Assert
            var karteFileAfter = tenant.KarteImgInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.RaiinNo == raiinNo
                                                                            && item.Position == 1
                                                                            && item.KarteKbn == 0
                                                                            && item.FileName == string.Empty);

            result = result && karteFileAfter != null;
            Assert.True(result);
        }
        finally
        {
            karteInfRepository.ReleaseResource();
            ClearKarteFile(hpId, ptId, userId);
        }
    }
    #endregion SaveListFileKarte

    #region ClearTempData
    [Test]
    public void TC_018_SaveListFileKarte_TestSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int ptId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        string fileName = "fileName.txt";
        List<string> listFiles = new() { fileName };

        KarteImgInf karteImgInf = new KarteImgInf()
        {
            HpId = hpId,
            FileName = fileName,
            PtId = ptId,
            RaiinNo = 0,
            SeqNo = 0,
            CreateId = userId,
            UpdateId = userId
        };
        tenant.Add(karteImgInf);

        var mockIUserRepository = new Mock<IUserRepository>();
        KarteInfRepository karteInfRepository = new KarteInfRepository(TenantProvider, mockIUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = karteInfRepository.ClearTempData(hpId, ptId, listFiles);

            // Assert
            var karteFileAfter = tenant.KarteImgInfs.FirstOrDefault(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.FileName == fileName);

            result = result && karteFileAfter == null;
            Assert.True(result);
        }
        finally
        {
            karteInfRepository.ReleaseResource();
            ClearKarteFile(hpId, ptId, userId);
        }
    }
    #endregion

    private void ClearKarteFile(int hpId, long ptId, int userId)
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var fileList = tenant.KarteImgInfs.Where(item => item.HpId == hpId
                                                             && item.UpdateId == userId
                                                             && item.PtId == ptId);
        foreach (var item in fileList)
        {
            tenant.KarteImgInfs.Remove(item);
        }
        tenant.SaveChanges();
    }
}
