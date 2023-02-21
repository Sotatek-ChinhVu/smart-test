using Domain.Models.MstItem;
using Domain.Models.Santei;
using Interactor.Santei;
using Moq;
using UseCase.Santei.GetListSanteiInf;

namespace CloudUnitTest.Interactor.Santei;

public class GetListSanteiInfInteractorTest : BaseUT
{
    #region ConvertToResult
    [Test]
    public void ConvertToResult_TestSuccess()
    {
        // Arrange
        int sinDate = 20221111;
        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();

        List<SanteiInfModel> listSanteiInfs = new();
        List<string> listByomeis = new();

        #region Data Example
        // byomei
        listByomeis.Add("byomei1");
        listByomeis.Add("byomei2");

        // santeiInf
        var santeiInf = new SanteiInfModel(
                                            1,
                                            883,
                                            "itemCd",
                                            1,
                                            1,
                                            1,
                                            "itemName",
                                            20221221,
                                            1,
                                            1,
                                            1,
                                            1.5,
                                            new List<SanteiInfDetailModel>()
                                            {
                                                new SanteiInfDetailModel(
                                                        1,
                                                        883,
                                                        "itemCd",
                                                        20230101,
                                                        1,
                                                        20220101,
                                                        "byomei",
                                                        "hosokuComment",
                                                        "comment"
                                                    )
                                            });
        listSanteiInfs.Add(santeiInf);
        #endregion

        var interactor = new GetListSanteiInfInteractor(mockSanteiInfRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ConvertToResult(sinDate, listSanteiInfs, listByomeis);

        // Assert
        var listSanteiInfResult = output.Item1;
        var listByomeiResult = output.Item2;
        Assert.True(listSanteiInfResult.Count == listSanteiInfs.Count && listByomeiResult.Count >= listByomeis.Count);
    }

    [Test]
    public void ConvertToResult_TestInvalid()
    {
        // Arrange
        int sinDate = 20221;
        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();

        List<SanteiInfModel> listSanteiInfs = new();
        List<string> listByomeis = new();

        var interactor = new GetListSanteiInfInteractor(mockSanteiInfRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.ConvertToResult(sinDate, listSanteiInfs, listByomeis);

        // Assert
        var listSanteiInfResult = output.Item1;
        var listByomeiResult = output.Item2;
        Assert.True(listSanteiInfResult.Count == 0 && listByomeiResult.Count == 0);
    }
    #endregion

    #region Handle
    [Test]
    public void Handle_TestSuccess()
    {
        // Arrange
        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();

        int hpId = 1;
        long ptId = 883;
        int sinDate = 20221212;
        int hokenPid = 10;

        GetListSanteiInfInputData inputData = new GetListSanteiInfInputData(
                                                                                hpId,
                                                                                ptId,
                                                                                sinDate,
                                                                                hokenPid
                                                                            );
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInf(hpId, ptId, sinDate))
        .Returns(new List<SanteiInfModel>()
                                            {
                                                new SanteiInfModel(
                                                                    1,
                                                                    883,
                                                                    "itemCd",
                                                                    1,
                                                                    1,
                                                                    1,
                                                                    "itemName",
                                                                    20221221,
                                                                    1,
                                                                    1,
                                                                    1,
                                                                    1.5,
                                                                    new List<SanteiInfDetailModel>()
                                                                    {
                                                                        new SanteiInfDetailModel(
                                                                                1,
                                                                                883,
                                                                                "itemCd",
                                                                                20230101,
                                                                                1,
                                                                                20220101,
                                                                                "byomei",
                                                                                "hosokuComment",
                                                                                "comment"
                                                                            )
                                                                    })
                                            });

        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() {
                                        "byomei1",
                                        "byomei2"
                                    });

        var interactor = new GetListSanteiInfInteractor(mockSanteiInfRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.Handle(inputData);

        // Assert
        var listSanteiInfResult = output.ListSanteiInfs;
        var listByomeiResult = output.ListByomeis;
        var alertTermCombobox = output.AlertTermCombobox;
        var kisanKbnCombobox = output.KisanKbnCombobox;
        var status = output.Status;
        Assert.True(listSanteiInfResult.Count > 0
                    && listByomeiResult.Count > 0
                    && alertTermCombobox.Count == 5
                    && kisanKbnCombobox.Count == 7
                    && status == GetListSanteiInfStatus.Successed);
    }

    [Test]
    public void Handle_TestInvalid()
    {
        // Arrange
        var mockSanteiInfRepo = new Mock<ISanteiInfRepository>();
        var mockMstItemRepo = new Mock<IMstItemRepository>();

        int hpId = 1;
        long ptId = 883;
        int sinDate = 20221212;
        int hokenPid = 10;

        GetListSanteiInfInputData inputData = new GetListSanteiInfInputData(
                                                                                hpId,
                                                                                ptId,
                                                                                sinDate,
                                                                                hokenPid
                                                                            );
        mockSanteiInfRepo.Setup(repo => repo.GetListSanteiInf(hpId, ptId, sinDate))
        .Returns(new List<SanteiInfModel>() { });

        mockMstItemRepo.Setup(repo => repo.GetListSanteiByomeis(hpId, ptId, sinDate, hokenPid))
        .Returns(new List<string>() { });

        var interactor = new GetListSanteiInfInteractor(mockSanteiInfRepo.Object, mockMstItemRepo.Object);

        // Act
        var output = interactor.Handle(inputData);

        // Assert
        var listSanteiInfResult = output.ListSanteiInfs;
        var listByomeiResult = output.ListByomeis;
        var alertTermCombobox = output.AlertTermCombobox;
        var kisanKbnCombobox = output.KisanKbnCombobox;
        var status = output.Status;
        Assert.True(listSanteiInfResult.Count == 0
                    && listByomeiResult.Count == 0
                    && alertTermCombobox.Count == 5
                    && kisanKbnCombobox.Count == 7
                    && status == GetListSanteiInfStatus.Successed);
    }
    #endregion
}
