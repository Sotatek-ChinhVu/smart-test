using Domain.Models.Diseases;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using Moq;
using Domain.Models.DrugDetail;
using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.Receipt;
using Domain.Models.ReceSeikyu;
using Domain.Models.SystemConf;
using Helper.Messaging;
using Infrastructure.Interfaces;
using Interactor.CalculateService;
using Interactor.CommonChecker.CommonMedicalCheck;
using Interactor.Receipt;
using Interactor.ReceSeikyu;
using UseCase.ReceSeikyu.Save;

namespace CloudUnitTest.Interactor.SaveReceSeiKyu;

public class SaveReceSeiKyuTest : BaseUT
{
    #region SaveReceSeiKyuInteractor
    [Test]
    public void TC_001_SaveReceSeiKyuInteractor_TestInvalidHpId()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = 0;
        int userId = random.Next(999, 999999);
        int sinYm = It.IsAny<int>();

        var mockListReceSeikyuList = It.IsAny<List<ReceSeikyuModel>>();
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.InvalidHpId);
    }

    [Test]
    public void TC_002_SaveReceSeiKyuInteractor_TestInvalidUserId()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = 0;
        int sinYm = It.IsAny<int>();

        var mockListReceSeikyuList = It.IsAny<List<ReceSeikyuModel>>();
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.InvalidUserId);
    }

    [Test]
    public void TC_003_SaveReceSeiKyuInteractor_TestIsAddNewAndIsDeletedContinue()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202202;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = false;
        bool isAddNew = true;
        bool isChecked = false;
        int isDeleted = 1;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_004_SaveReceSeiKyuInteractor_TestIsAddNewSuccess()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202202;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = false;
        bool isAddNew = true;
        bool isChecked = false;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_005_SaveReceSeiKyuInteractor_TestIsAddNewAndIsCheckedSuccess()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202202;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = false;
        bool isAddNew = true;
        bool isChecked = true;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_006_SaveReceSeiKyuInteractor_TestIsDeletedSuccess()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202202;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = false;
        bool isAddNew = false;
        bool isChecked = false;
        int isDeleted = 1;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_007_SaveReceSeiKyuInteractor_TestUpdateSeikyuYmEqual999999Success()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202202;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = false;
        bool isAddNew = false;
        bool isChecked = false;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_008_SaveReceSeiKyuInteractor_TestUpdateSeikyuYmGreaterThan0Success()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202201;
        int originSeikyuYm = 999999;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = false;
        bool isAddNew = false;
        bool isChecked = true;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_009_SaveReceSeiKyuInteractor_TestUpdateOriginSeikyuYmDifferent999999Success()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, setupData.mockIReceSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202201;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = false;
        bool isAddNew = false;
        bool isChecked = true;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_010_SaveReceSeiKyuInteractor_TestNotCompleteSeikyu()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var receSeikyuRepository = setupData.mockIReceSeikyuRepository;
        receSeikyuRepository.Setup(finder => finder.SaveReceSeiKyu(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<ReceSeikyuModel>>()))
        .Returns((int hpId, int userId, List<ReceSeikyuModel> data) => true);

        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, receSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 202202;
        int receListSinYm = 202201;
        int originSeikyuYm = 999999;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = true;
        bool isAddNew = false;
        bool isChecked = true;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_011_SaveReceSeiKyuInteractor_TestNotCompleteSeikyuAndIsChecked()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var receSeikyuRepository = setupData.mockIReceSeikyuRepository;
        receSeikyuRepository.Setup(finder => finder.SaveReceSeiKyu(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<ReceSeikyuModel>>()))
        .Returns((int hpId, int userId, List<ReceSeikyuModel> data) => true);

        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, receSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 999999;
        int receListSinYm = 999999;
        int originSeikyuYm = 999999;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = true;
        bool isAddNew = false;
        bool isChecked = true;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_012_SaveReceSeiKyuInteractor_TestCompleteSeikyu()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var receSeikyuRepository = setupData.mockIReceSeikyuRepository;
        receSeikyuRepository.Setup(finder => finder.InsertNewReceSeikyu(It.IsAny<List<ReceSeikyuModel>>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns((List<ReceSeikyuModel> listInsert, int userId, int hpId) => true);

        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, receSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 999999;
        int receListSinYm = 999999;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = true;
        bool isAddNew = false;
        bool isChecked = false;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_013_SaveReceSeiKyuInteractor_TestListSourceSeikyuContinue()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var receSeikyuRepository = setupData.mockIReceSeikyuRepository;
        receSeikyuRepository.Setup(finder => finder.SaveReceSeiKyu(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<ReceSeikyuModel>>()))
        .Returns((int hpId, int userId, List<ReceSeikyuModel> data) => true);
        receSeikyuRepository.Setup(finder => finder.InsertNewReceSeikyu(It.IsAny<List<ReceSeikyuModel>>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns((List<ReceSeikyuModel> listInsert, int userId, int hpId) => true);

        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, receSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 999999;
        int receListSinYm = 999999;
        int originSeikyuYm = 202202;
        int originSeikyuYm2 = 999999;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = true;
        bool isAddNew = false;
        bool isChecked = false;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        ReceSeikyuModel receSeikyuModel2 = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm2, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel, receSeikyuModel2 };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_014_SaveReceSeiKyuInteractor_TestCompleteSeikyuAndInsertNewSinym()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var receSeikyuRepository = setupData.mockIReceSeikyuRepository;
        receSeikyuRepository.Setup(finder => finder.InsertNewReceSeikyu(It.IsAny<List<ReceSeikyuModel>>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns((List<ReceSeikyuModel> listInsert, int userId, int hpId) => true);

        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, receSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202203;
        int seikyuYm = 999999;
        int receListSinYm = 999999;
        int originSeikyuYm = 202202;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = true;
        bool isAddNew = false;
        bool isChecked = false;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Successful);
    }

    [Test]
    public void TC_015_SaveReceSeiKyuInteractor_TestStatusFailed()
    {
        // Arrange
        var setupData = SetupTestEnvironment();
        var receSeikyuRepository = setupData.mockIReceSeikyuRepository;
        receSeikyuRepository.Setup(finder => finder.InsertNewReceSeikyu(It.IsAny<List<ReceSeikyuModel>>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns((List<ReceSeikyuModel> listInsert, int userId, int hpId) => true);

        var saveReceSeiKyuInteractor = new SaveReceSeiKyuInteractor(setupData.tenantProvider, receSeikyuRepository.Object, setupData.calcultateCustomerService, setupData.receiptRepository, setupData.systemConfRepository, setupData.insuranceMstRepository, setupData.mstItemRepository, setupData.ptDiseaseRepository, setupData.ordInfRepository, setupData.commonMedicalCheck, setupData.todayOdrRepository, setupData.drugDetailRepository, setupData.commonReceRecalculation);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int sinDate = 20220202;
        int sinYm = 202202;
        int seikyuYm = 999999;
        int receListSinYm = 999999;
        int originSeikyuYm = 202202;
        int originSeikyuYm2 = 999999;
        int originSinYm = 202202;
        int hokenStartDate = 20220202;
        int hokenEndDate = 20220202;
        string ptName = "PtName";
        string hokensyaNo = "HokensyaNo";
        string cmt = "Cmt";
        string houbetu = "Houbetu";
        bool isModified = true;
        bool isAddNew = false;
        bool isChecked = false;
        int isDeleted = 0;

        ReceSeikyuModel receSeikyuModel = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, new());
        ReceSeikyuModel receSeikyuModel2 = new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn, houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm2, originSinYm, isAddNew, isDeleted, isChecked, new());
        List<ReceSeikyuModel> mockListReceSeikyuList = new() { receSeikyuModel, receSeikyuModel2 };
        SaveReceSeiKyuInputData inputData = new SaveReceSeiKyuInputData(mockListReceSeikyuList, sinYm, hpId, userId, setupData.messenger);

        // Act
        var result = saveReceSeiKyuInteractor.Handle(inputData);

        // Assert
        Assert.That(result.Status == SaveReceSeiKyuStatus.Failed);
    }
    #endregion SaveReceSeiKyuInteractor

    private (ITenantProvider tenantProvider, Mock<IReceSeikyuRepository> mockIReceSeikyuRepository, ICalcultateCustomerService calcultateCustomerService, IReceiptRepository receiptRepository, ISystemConfRepository systemConfRepository, IInsuranceMstRepository insuranceMstRepository, IMstItemRepository mstItemRepository, IPtDiseaseRepository ptDiseaseRepository, IOrdInfRepository ordInfRepository, ICommonMedicalCheck commonMedicalCheck, ITodayOdrRepository todayOdrRepository, IDrugDetailRepository drugDetailRepository, ICommonReceRecalculation commonReceRecalculation, IMessenger messenger) SetupTestEnvironment()
    {
        var mockIReceSeikyuRepository = new Mock<IReceSeikyuRepository>();
        var mockICalcultateCustomerService = new Mock<ICalcultateCustomerService>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIReceiptRepository = new Mock<IReceiptRepository>();
        var mockIInsuranceMstRepository = new Mock<IInsuranceMstRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockICommonMedicalCheck = new Mock<ICommonMedicalCheck>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIDrugDetailRepository = new Mock<IDrugDetailRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockICommonReceRecalculation = new Mock<ICommonReceRecalculation>();
        var mockIMessenger = new Mock<IMessenger>();

        var tenantProvider = mockITenantProvider.Object;
        var calcultateCustomerService = mockICalcultateCustomerService.Object;
        var receiptRepository = mockIReceiptRepository.Object;
        var systemConfRepository = mockISystemConfRepository.Object;
        var insuranceMstRepository = mockIInsuranceMstRepository.Object;
        var mstItemRepository = mockIMstItemRepository.Object;
        var ptDiseaseRepository = mockIPtDiseaseRepository.Object;
        var ordInfRepository = mockIOrdInfRepository.Object;
        var commonMedicalCheck = mockICommonMedicalCheck.Object;
        var todayOdrRepository = mockITodayOdrRepository.Object;
        var drugDetailRepository = mockIDrugDetailRepository.Object;
        var commonReceRecalculation = mockICommonReceRecalculation.Object;
        var messenger = mockIMessenger.Object;
        return (tenantProvider, mockIReceSeikyuRepository, calcultateCustomerService, receiptRepository, systemConfRepository, insuranceMstRepository, mstItemRepository, ptDiseaseRepository, ordInfRepository, commonMedicalCheck, todayOdrRepository, drugDetailRepository, commonReceRecalculation, messenger);
    }
}
