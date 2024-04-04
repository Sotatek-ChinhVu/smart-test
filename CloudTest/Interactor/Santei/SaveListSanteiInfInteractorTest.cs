using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Santei;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Interactor.Santei;
using Moq;
using System.Text.Json;
using UseCase.Santei.SaveListSanteiInf;

namespace CloudUnitTest.Interactor.Santei;

public class SaveListSanteiInfInteractorTest : BaseUT
{
    #region ValidateInput
    [Test]
    public void TC_001_ConvertToResult_TestSuccess()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>()
            {
                new SanteiInfInputItem(
                        1,
                        1,
                        "itemCd",
                        1,
                        4,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        }),
                new SanteiInfInputItem(
                        0,
                        1,
                        "itemCdAddNew",
                        1,
                        2,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    0,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        })
            };
        var input = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCds = listSanteiInfs.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepo.Setup(repo => repo.CheckExistItemCd(hpId, listItemCds))
        .Returns(true);
        mockSanteiInfRepo.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    ),
                                                new SanteiInfModel(
                                                        2,
                                                        ptId,
                                                        "itemCd2",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        });

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { });

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateInput(input);

        // Assert
        Assert.True(output == SaveListSanteiInfStatus.ValidateSuccess);
    }

    [Test]
    public void TC_002_ConvertToResult_TestInvalidHpId()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>() { };
        var inputHpEqual0 = new SaveListSanteiInfInputData(0, userId, ptId, sinDate, hokenPid, listSanteiInfs);
        var inputDataInvalid = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(false);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var outputHpEqual0 = interactor.ValidateInput(inputHpEqual0);
        var outputInvalidHpId = interactor.ValidateInput(inputDataInvalid);

        // Assert
        Assert.True(outputHpEqual0 == SaveListSanteiInfStatus.InvalidHpId && outputInvalidHpId == SaveListSanteiInfStatus.InvalidHpId);
    }

    [Test]
    public void TC_003_ConvertToResult_TestInvalidPtId()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>() { };
        var inputDataPtIdEqual0 = new SaveListSanteiInfInputData(hpId, userId, 0, sinDate, hokenPid, listSanteiInfs);
        var inputDataInvalidPtId = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(false);

        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var outputPtIdEqual0 = interactor.ValidateInput(inputDataPtIdEqual0);
        var outputInvalidPtId = interactor.ValidateInput(inputDataInvalidPtId);

        // Assert
        Assert.True(outputPtIdEqual0 == SaveListSanteiInfStatus.InvalidPtId && outputInvalidPtId == SaveListSanteiInfStatus.InvalidPtId);
    }

    [Test]
    public void TC_004_ConvertToResult_TestInvalidUserId()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>() { };
        var inputDataUserIdEqual0 = new SaveListSanteiInfInputData(hpId, 0, ptId, sinDate, hokenPid, listSanteiInfs);
        var inputDataInvalidUserId = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(false);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var outputUserIdEqual0 = interactor.ValidateInput(inputDataUserIdEqual0);
        var outputInvalidUserId = interactor.ValidateInput(inputDataInvalidUserId);

        // Assert
        Assert.True(outputUserIdEqual0 == SaveListSanteiInfStatus.InvalidUserId && outputInvalidUserId == SaveListSanteiInfStatus.InvalidUserId);
    }

    [Test]
    public void TC_005_ConvertToResult_TestInvalidItemCd()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>()
        {
            new SanteiInfInputItem(
                    0,
                    ptId,
                    "itemCdAddNew",
                    1,
                    2,
                    false,
                    1,
                    new List<SanteiInfDetailInputItem>(){
                    })
        };

        var listSanteiCheckDuplicateIdEqual0 = new List<SanteiInfInputItem>()
        {
            new SanteiInfInputItem(
                    0,
                    ptId,
                    "itemCd",
                    1,
                    2,
                    false,
                    1,
                    new List<SanteiInfDetailInputItem>(){})
        };

        var listSanteiCheckDuplicateIdNotEqual0 = new List<SanteiInfInputItem>()
        {
            new SanteiInfInputItem(
                    1,
                    1,
                    "itemCdError",
                    1,
                    2,
                    false,
                    1,
                    new List<SanteiInfDetailInputItem>(){})
        };

        var inputDataSuccess = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var inputDataCheckDuplicateIdEqual0 = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiCheckDuplicateIdEqual0);
        var inputDataCheckDuplicateIdNotEqual0 = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiCheckDuplicateIdNotEqual0);

        var mockSanteiInfRepoCheckExistItemCd = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCdCheckExistItemCd = listSanteiInfs.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepoCheckExistItemCd.Setup(repo => repo.CheckExistItemCd(hpId, listItemCdCheckExistItemCd))
        .Returns(false);

        var mockSanteiInfRepoCheckDuplicateIdEqual0 = new Mock<ISanteiInfRepository>();
        var listItemCdCheckDuplicateIdEqual0 = listSanteiCheckDuplicateIdEqual0.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepoCheckDuplicateIdEqual0.Setup(repo => repo.CheckExistItemCd(hpId, listItemCdCheckDuplicateIdEqual0))
        .Returns(true);
        mockSanteiInfRepoCheckDuplicateIdEqual0.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
       .Returns(new List<SanteiInfModel>(){
                                            new SanteiInfModel(
                                                    1,
                                                    ptId,
                                                    "itemCd",
                                                    1,
                                                    3
                                                )
                                   });
        mockSanteiInfRepoCheckDuplicateIdEqual0.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockSanteiInfRepoCheckDuplicateIdNotEqual0 = new Mock<ISanteiInfRepository>();
        var listItemCdCheckDuplicateIdNotEqual0 = listSanteiCheckDuplicateIdNotEqual0.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepoCheckDuplicateIdNotEqual0.Setup(repo => repo.CheckExistItemCd(hpId, listItemCdCheckDuplicateIdNotEqual0))
        .Returns(true);
        mockSanteiInfRepoCheckDuplicateIdNotEqual0.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
       .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                   });
        mockSanteiInfRepoCheckDuplicateIdNotEqual0.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { });

        var interactorCheckExistItemCd = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepoCheckExistItemCd.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);
        var interactorCheckDuplicateIdEqual0 = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepoCheckDuplicateIdEqual0.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);
        var interactorCheckDuplicateIdNotEqual0 = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepoCheckDuplicateIdNotEqual0.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var outputDataCheckDuplicateIdEqual0 = interactorCheckDuplicateIdEqual0.ValidateInput(inputDataCheckDuplicateIdEqual0);
        var outputDataCheckDuplicateIdNotEqual0 = interactorCheckDuplicateIdNotEqual0.ValidateInput(inputDataCheckDuplicateIdNotEqual0);
        var outputDataCheckExistItemCd = interactorCheckExistItemCd.ValidateInput(inputDataSuccess);

        // Assert
        Assert.True(
            outputDataCheckDuplicateIdEqual0 == SaveListSanteiInfStatus.InvalidItemCd
            && outputDataCheckDuplicateIdNotEqual0 == SaveListSanteiInfStatus.InvalidItemCd
            && outputDataCheckExistItemCd == SaveListSanteiInfStatus.InvalidItemCd);
    }

    [Test]
    public void TC_006_ConvertToResult_TestInvalidAlertTerm()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfAlertTermGreaterThan6 = new List<SanteiInfInputItem>()
        {
            new SanteiInfInputItem(
                    1,
                    ptId,
                    "itemCd",
                    1,
                    7,
                    false,
                    1,
                    new List<SanteiInfDetailInputItem>(){})
        };

        var listSanteiInfAlertTermLessThan2 = new List<SanteiInfInputItem>()
        {
            new SanteiInfInputItem(
                    1,
                    ptId,
                    "itemCd",
                    1,
                    1,
                    false,
                    1,
                    new List<SanteiInfDetailInputItem>(){})
        };
        var inputAlertTermGreaterThan6 = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfAlertTermGreaterThan6);
        var inputAlertTermLessThan2 = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfAlertTermLessThan2);

        var mockSanteiInfRepoAlertTermGreaterThan6 = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCdAlertTermGreaterThan6 = listSanteiInfAlertTermGreaterThan6.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepoAlertTermGreaterThan6.Setup(repo => repo.CheckExistItemCd(hpId, listItemCdAlertTermGreaterThan6))
        .Returns(true);
        mockSanteiInfRepoAlertTermGreaterThan6.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepoAlertTermGreaterThan6.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockSanteiInfRepoAlertTermLessThan2 = new Mock<ISanteiInfRepository>();
        var listItemCdAlertTermLessThan2 = listSanteiInfAlertTermLessThan2.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepoAlertTermLessThan2.Setup(repo => repo.CheckExistItemCd(hpId, listItemCdAlertTermLessThan2))
        .Returns(true);
        mockSanteiInfRepoAlertTermLessThan2.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepoAlertTermLessThan2.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { });

        var interactorAlertTermGreaterThan6 = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepoAlertTermGreaterThan6.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);
        var interactorAlertTermLessThan2 = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepoAlertTermLessThan2.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var outputAlertTermGreaterThan6 = interactorAlertTermGreaterThan6.ValidateInput(inputAlertTermGreaterThan6);
        var outputAlertTermLessThan2 = interactorAlertTermLessThan2.ValidateInput(inputAlertTermLessThan2);

        // Assert
        Assert.True(outputAlertTermGreaterThan6 == SaveListSanteiInfStatus.InvalidAlertTerm && outputAlertTermLessThan2 == SaveListSanteiInfStatus.InvalidAlertTerm);
    }

    [Test]
    public void TC_007_ConvertToResult_TestInvalidAlertDays()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;

        var listSanteiInfAlertDaysLessThan0 = new List<SanteiInfInputItem>()
                                                            {
                                                                new SanteiInfInputItem(
                                                                        1,
                                                                        ptId,
                                                                        "itemCd",
                                                                        -1,
                                                                        4,
                                                                        false,
                                                                        1,
                                                                        new List<SanteiInfDetailInputItem>(){})
                                                            };
        var input = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfAlertDaysLessThan0);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCds = listSanteiInfAlertDaysLessThan0.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepo.Setup(repo => repo.CheckExistItemCd(hpId, listItemCds))
        .Returns(true);
        mockSanteiInfRepo.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { });

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateInput(input);

        // Assert
        Assert.True(output == SaveListSanteiInfStatus.InvalidAlertDays);
    }

    [Test]
    public void TC_008_ValidateInput_TestValidateSanteiInfDetail()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;

        var santeiInfDetail = new SanteiInfDetailInputItem(
                                  1,
                                  20221201,
                                  0,
                                  20221212,
                                  "byomei",
                                  "hosokuComment",
                                  "commnet",
                                  false);

        var listSanteiInf = new List<SanteiInfInputItem>()
                               {
                                   new SanteiInfInputItem(
                                           1,
                                           ptId,
                                           "itemCd",
                                           1,
                                           4,
                                           true,
                                           1,
                                           new List<SanteiInfDetailInputItem>(){santeiInfDetail})
                               };
        var input = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInf);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCds = listSanteiInf.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepo.Setup(repo => repo.CheckExistItemCd(hpId, listItemCds))
        .Returns(true);
        mockSanteiInfRepo.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { });

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateInput(input);

        // Assert
        Assert.True(output != SaveListSanteiInfStatus.ValidateSuccess);
    }

    [Test]
    public void TC_009_ConvertToResult_TestSanteiInfDoesNotAllowSanteiInfDetail()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfStartKN = new List<SanteiInfInputItem>()
            {
                new SanteiInfInputItem(
                        0,
                        ptId,
                        "KN_itemCd",
                        1,
                        4,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    0,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        })
            };

        var listSanteiInfStartIGE = new List<SanteiInfInputItem>()
            {
                new SanteiInfInputItem(
                        0,
                        ptId,
                        "IGE_itemCd",
                        1,
                        4,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    0,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        })
            };
        var inputStartKN = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfStartKN);
        var inputStartIGE = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfStartIGE);

        var mockSanteiInfRepoStartKN = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCdStartKN = listSanteiInfStartKN.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepoStartKN.Setup(repo => repo.CheckExistItemCd(hpId, listItemCdStartKN))
        .Returns(true);
        mockSanteiInfRepoStartKN.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepoStartKN.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockSanteiInfRepoStartIGE = new Mock<ISanteiInfRepository>();
        var listItemCdStartIGE = listSanteiInfStartIGE.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepoStartIGE.Setup(repo => repo.CheckExistItemCd(hpId, listItemCdStartIGE))
        .Returns(true);
        mockSanteiInfRepoStartIGE.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepoStartIGE.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>() { });

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { });

        var interactorStartKN = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepoStartKN.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);
        var interactorStartIGE = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepoStartIGE.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var outputStartKN = interactorStartKN.ValidateInput(inputStartKN);
        var outputStartIGE = interactorStartIGE.ValidateInput(inputStartIGE);

        // Assert
        Assert.True(outputStartIGE == SaveListSanteiInfStatus.ThisSanteiInfDoesNotAllowSanteiInfDetail && outputStartKN == SaveListSanteiInfStatus.ThisSanteiInfDoesNotAllowSanteiInfDetail);
    }
    #endregion

    #region ValidateSanteiInfDetail
    [Test]
    public void TC_010_ValidateSanteiInfDetail_TestSuccess()
    {
        // Arrange
        long ptId = 1;
        string itemCd = "itemCd";
        var santeiInfDetail = new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                );

        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        };

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockHpInfRepo = new Mock<IHpInfRepository>();
        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateSanteiInfDetail(itemCd, santeiInfDetail, listSanteiInfDetails);

        // Assert
        Assert.True(output == SaveListSanteiInfStatus.ValidateSuccess);
    }

    [Test]
    public void TC_011_ValidateSanteiInfDetail_TestInvalidKisanSbt()
    {
        // Arrange
        long ptId = 1;
        string itemCd = "itemCd";
        var santeiInfDetailKisanSbtGreaterThan6 = new SanteiInfDetailInputItem(
                                                                                    1,
                                                                                    20221201,
                                                                                    7,
                                                                                    20221212,
                                                                                    "byomei",
                                                                                    "hosokuComment",
                                                                                    "commnet",
                                                                                    false
                                                                                );

        var santeiInfDetailKisanSbtLessThan1 = new SanteiInfDetailInputItem(
                                                                                    1,
                                                                                    20221201,
                                                                                    0,
                                                                                    20221212,
                                                                                    "byomei",
                                                                                    "hosokuComment",
                                                                                    "commnet",
                                                                                    false
                                                                                );
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        };

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockHpInfRepo = new Mock<IHpInfRepository>();
        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var outputKisanSbtGreaterThan6 = interactor.ValidateSanteiInfDetail(itemCd, santeiInfDetailKisanSbtGreaterThan6, listSanteiInfDetails);
        var outputKisanSbtLessThan1 = interactor.ValidateSanteiInfDetail(itemCd, santeiInfDetailKisanSbtLessThan1, listSanteiInfDetails);

        // Assert
        Assert.True(outputKisanSbtGreaterThan6 == SaveListSanteiInfStatus.InvalidKisanSbt && outputKisanSbtLessThan1 == SaveListSanteiInfStatus.InvalidKisanSbt);
    }

    [Test]
    public void TC_012_ValidateSanteiInfDetail_TestInvalidSanteiInfDetail()
    {
        // Arrange
        long ptId = 1;
        string itemCd = "itemCdError";
        var santeiInfDetail = new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                );

        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        };

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockHpInfRepo = new Mock<IHpInfRepository>();
        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateSanteiInfDetail(itemCd, santeiInfDetail, listSanteiInfDetails);

        // Assert
        Assert.True(output == SaveListSanteiInfStatus.InvalidSanteiInfDetail);
    }

    [Test]
    public void TC_013_ValidateSanteiInfDetail_TestInvalidEndDate()
    {
        // Arrange
        long ptId = 1;
        string itemCd = "itemCd";
        var santeiInfDetail = new SanteiInfDetailInputItem(
                                    1,
                                    1,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                );

        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        };

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockHpInfRepo = new Mock<IHpInfRepository>();
        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateSanteiInfDetail(itemCd, santeiInfDetail, listSanteiInfDetails);

        // Assert
        Assert.True(output == SaveListSanteiInfStatus.InvalidEndDate);
    }

    [Test]
    public void TC_014_ValidateSanteiInfDetail_TestInvalidKisanDate()
    {
        // Arrange
        long ptId = 1;
        string itemCd = "itemCd";
        var santeiInfDetail = new SanteiInfDetailInputItem(
                                    1,
                                    20221212,
                                    1,
                                    1,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                );

        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        };

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockHpInfRepo = new Mock<IHpInfRepository>();
        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateSanteiInfDetail(itemCd, santeiInfDetail, listSanteiInfDetails);

        // Assert
        Assert.True(output == SaveListSanteiInfStatus.InvalidKisanDate);
    }

    [Test]
    public void TC_015_ValidateSanteiInfDetail_TestInvalidHosokuComment()
    {
        // Arrange
        long ptId = 1;
        string itemCd = "itemCd";
        var santeiInfDetail = new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789",
                                    "commnet",
                                    false
                                );

        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        };

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockHpInfRepo = new Mock<IHpInfRepository>();
        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ValidateSanteiInfDetail(itemCd, santeiInfDetail, listSanteiInfDetails);

        // Assert
        Assert.True(output == SaveListSanteiInfStatus.InvalidHosokuComment);
    }
    #endregion

    #region ConvertToSanteiInfModel
    [Test]
    public void TC_016_ConvertToSanteiInfModel_TestSuccess()
    {
        // Arrange
        long ptId = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>()
            {
                new SanteiInfInputItem(
                        1,
                        ptId,
                        "itemCd",
                        1,
                        4,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        })
            };

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockHpInfRepo = new Mock<IHpInfRepository>();
        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ConvertToSanteiInfModel(ptId, listSanteiInfs);
        // Assert
        var listChecks = new List<SanteiInfModel>()
            {
                new SanteiInfModel(
                            1,
                            ptId,
                            "itemCd",
                            1,
                            4,
                            1,
                            new List<SanteiInfDetailModel>()
                            {
                                new SanteiInfDetailModel(
                                        1,
                                        ptId,
                                        "itemCd",
                                        20221201,
                                        1,
                                        20221212,
                                        "byomei",
                                        "hosokuComment",
                                        "commnet",
                                        false
                                    )
                            },
                            false
                        )
            };
        var jsonStringOutput = JsonSerializer.Serialize(output).ToString();
        var jsonStringListComper = JsonSerializer.Serialize(listChecks).ToString();
        var resultString = jsonStringOutput.Equals(jsonStringListComper);
        Assert.True(resultString);
    }
    #endregion

    #region Handle
    [Test]
    public void TC_017_Handle_TestSuccess()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>()
            {
                new SanteiInfInputItem(
                        1,
                        ptId,
                        "itemCd",
                        1,
                        4,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        })
            };
        var input = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCds = listSanteiInfs.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepo.Setup(repo => repo.CheckExistItemCd(hpId, listItemCds))
        .Returns(true);
        mockSanteiInfRepo.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        });
        mockSanteiInfRepo.Setup(repo => repo.SaveSantei(hpId, userId, ptId, It.IsAny<List<SanteiInfModel>>()))
        .Returns(true);

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { "byomei", "byomei1", "byomei2" });

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.Handle(input);

        // Assert
        Assert.True(output.Status == SaveListSanteiInfStatus.Successed);
    }

    [Test]
    public void TC_018_Handle_TestFalse()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>()
            {
                new SanteiInfInputItem(
                        1,
                        ptId,
                        "itemCd",
                        1,
                        4,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        })
            };
        var input = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCds = listSanteiInfs.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepo.Setup(repo => repo.CheckExistItemCd(hpId, listItemCds))
        .Returns(true);
        mockSanteiInfRepo.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        });
        mockSanteiInfRepo.Setup(repo => repo.SaveSantei(hpId, userId, ptId, It.IsAny<List<SanteiInfModel>>()))
        .Returns(false);

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(true);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { "byomei", "byomei1", "byomei2" });

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.Handle(input);

        // Assert
        Assert.True(output.Status == SaveListSanteiInfStatus.Failed);
    }

    [Test]
    public void TC_019_Handle_TestInvalid()
    {
        // Arrange
        int hpId = 1;
        int userId = 1;
        long ptId = 1;
        int sinDate = 20221111;
        int hokenPid = 1;
        var listSanteiInfs = new List<SanteiInfInputItem>()
            {
                new SanteiInfInputItem(
                        1,
                        ptId,
                        "itemCd",
                        1,
                        4,
                        false,
                        1,
                        new List<SanteiInfDetailInputItem>()
                        {
                            new SanteiInfDetailInputItem(
                                    1,
                                    20221201,
                                    1,
                                    20221212,
                                    "byomei",
                                    "hosokuComment",
                                    "commnet",
                                    false
                                )
                        })
            };
        var input = new SaveListSanteiInfInputData(hpId, userId, ptId, sinDate, hokenPid, listSanteiInfs);

        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockTenantProvider = new Mock<ITenantProvider>();
        var listItemCds = listSanteiInfs.Select(item => item.ItemCd).ToList();
        mockSanteiInfRepo.Setup(repo => repo.CheckExistItemCd(hpId, listItemCds))
        .Returns(true);
        mockSanteiInfRepo.Setup(repo => repo.GetOnlyListSanteiInf(hpId, ptId))
        .Returns(new List<SanteiInfModel>(){
                                                new SanteiInfModel(
                                                        1,
                                                        ptId,
                                                        "itemCd",
                                                        1,
                                                        3
                                                    )
                                    });
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInfDetails(hpId, ptId))
        .Returns(new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    "itemCd",
                    20221201,
                    1,
                    20221212,
                    "byomei",
                    "hosokuComment",
                    "commnet"
                )
        });
        mockSanteiInfRepo.Setup(repo => repo.SaveSantei(hpId, userId, ptId, It.IsAny<List<SanteiInfModel>>()))
        .Returns(false);

        var mockHpInfRepo = new Mock<IHpInfRepository>();
        mockHpInfRepo.Setup(repo => repo.CheckHpId(hpId))
        .Returns(false);

        var mockPatientInforRepo = new Mock<IPatientInforRepository>();
        mockPatientInforRepo.Setup(repo => repo.CheckExistIdList(hpId, new List<long> { ptId }))
        .Returns(true);

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.CheckExistedUserId(hpId, userId))
        .Returns(true);

        var mockMstItemRepo = new Mock<IMstItemRepository>();
        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { "byomei", "byomei1", "byomei2" });

        var interactor = new SaveListSanteiInfInteractor(mockTenantProvider.Object, mockSanteiInfRepo.Object, mockHpInfRepo.Object, mockPatientInforRepo.Object, mockUserRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.Handle(input);

        // Assert
        Assert.True(output.Status != SaveListSanteiInfStatus.Successed);
    }
    #endregion
}
