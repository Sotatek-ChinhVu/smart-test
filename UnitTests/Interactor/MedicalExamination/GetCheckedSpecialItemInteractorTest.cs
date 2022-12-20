using Amazon.Runtime.Internal.Transform;
using Domain.Models.Insurance;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Helper.Constants;
using Interactor.MedicalExamination;
using Interactor.User;
using Moq;
using UseCase.User.GetByLoginId;

namespace UnitTests.Interactor.MedicalExamination;

public class GetCheckedSpecialItemInteractorTest
{
    #region AgeLimitCheck
    //MinAge && MaxAge != 0
    [Fact]
    public void AgeLimitCheck_MinAge()
    {
        // Arrange
        var testLoginId = "test login id";
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetByLoginId(testLoginId))
            .Returns(new UserMstModel(1, 1, 1, 1, 1, 1, string.Empty, string.Empty,
                string.Empty, string.Empty, testLoginId, string.Empty,
                string.Empty, 1, 1, 1, string.Empty, DeleteTypes.None));
        var interactor = new GetUserByLoginIdInteractor(mockUserRepo.Object);
        var input = new GetUserByLoginIdInputData(testLoginId);

        // Act
        var output = interactor.Handle(input);

        // Assert
        Assert.Equal(GetUserByLoginIdStatus.Success, output.Status);
        Assert.NotNull(output.User);
    }
    #endregion

    #region ExpiredCheck
    [Fact]
    public void ExpiredCheck_True()
    {
        // Arrange
        var sinDate = 20221101;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();
        var tenMstError = new TenItemModel(
            1,
            "140064650",
            0,
            "IMV",
            "ＩＭＶ（５時間超１５日目以降）",
            0,
            0,
            0,
            "",
            99999999,
            0,
            "S",
            0,
            0,
            815,
            3,
            "",
            "",
            0,
            "",
            40,
            "",
            "",
            20220401,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            string.Empty,
            "00",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140037030",
            "腎盂洗浄（両）",
            120,
            "",
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            "",
            0,
            "3122007F2",
            "",
            0,
            DateTime.MinValue,
            0,
            "",
            "",
            "",
            "",
            "",
            "",
            0,
            "",
            0,
            0,
            false,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            new(),
            0,
            0,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ExpiredCheck(sinDate, tenMstItems, odrDetails);

        // Assert
        Assert.True(!output.Any());
    }

    [Fact]
    public void ExpiredCheck_MinStartDate()
    {
        // Arrange
        var sinDate = 20220101;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();
        var tenMstError = new TenItemModel(
            1,
            "140064650",
            0,
            "IMV",
            "ＩＭＶ（５時間超１５日目以降）",
            0,
            0,
            0,
            "",
            99999999,
            0,
            "S",
            0,
            0,
            815,
            3,
            "",
            "",
            0,
            "",
            40,
            "",
            "",
            20220401,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            string.Empty,
            "00",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140037030",
            "腎盂洗浄（両）",
            120,
            "",
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            "",
            0,
            "3122007F2",
            "",
            0,
            DateTime.MinValue,
            0,
            "",
            "",
            "",
            "",
            "",
            "",
            0,
            "",
            0,
            0,
            false,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            new(),
            0,
            0,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ExpiredCheck(sinDate, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Any());
    }

    [Fact]
    public void ExpiredCheck_MaxEndDate()
    {
        // Arrange
        var sinDate = 20221101;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();
        var tenMstError = new TenItemModel(
            1,
            "140064650",
            0,
            "IMV",
            "ＩＭＶ（５時間超１５日目以降）",
            0,
            0,
            0,
            "",
            20221001,
            0,
            "S",
            0,
            0,
            815,
            3,
            "",
            "",
            0,
            "",
            40,
            "",
            "",
            20220401,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            string.Empty,
            "00",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140037030",
            "腎盂洗浄（両）",
            120,
            "",
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            "",
            0,
            "3122007F2",
            "",
            0,
            DateTime.MinValue,
            0,
            "",
            "",
            "",
            "",
            "",
            "",
            0,
            "",
            0,
            0,
            false,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            new(),
            0,
            0,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ExpiredCheck(sinDate, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Any());
    }
    #endregion

    #region DuplicateCheck
    [Fact]
    public void DuplicateCheck_True()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();
        var tenMstError = new TenItemModel(
            1,
            "140064650",
            0,
            "IMV",
            "ＩＭＶ（５時間超１５日目以降）",
            0,
            0,
            0,
            "",
            99999999,
            0,
            "S",
            0,
            0,
            815,
            3,
            "",
            "",
            0,
            "",
            40,
            "",
            "",
            20220401,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            string.Empty,
            "00",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();

        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140037030",
            "腎盂洗浄（両）",
            120,
            "",
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            "",
            0,
            "3122007F2",
            "",
            0,
            DateTime.MinValue,
            0,
            "",
            "",
            "",
            "",
            "",
            "",
            0,
            "",
            0,
            0,
            false,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            new(),
            0,
            0,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "140038410",
           "ストーマ処置２",
           120,
           "",
           0,
           0,
           0,
           0,
           0,
           0,
           0,
           "",
           "",
           0,
           "3122007F2",
           "",
           0,
           DateTime.MinValue,
           0,
           "",
           "",
           "",
           "",
           "",
           "",
           0,
           "",
           0,
           0,
           false,
           0,
           0,
           0,
           0,
           0,
           0,
           0,
           0,
           "",
           new(),
           0,
           0,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetail2);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.DuplicateCheck(tenMstItems, odrDetails);

        // Assert
        Assert.True(!output.Any());
    }

    [Fact]
    public void DuplicateCheck_Fail()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();
        var tenMstError = new TenItemModel(
            1,
            "140064650",
            0,
            "IMV",
            "ＩＭＶ（５時間超１５日目以降）",
            0,
            0,
            0,
            "",
            99999999,
            0,
            "S",
            0,
            0,
            815,
            3,
            "",
            "",
            0,
            "",
            40,
            "",
            "",
            20220401,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            string.Empty,
            "00",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140037030",
            "腎盂洗浄（両）",
            120,
            "",
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            "",
            0,
            "3122007F2",
            "",
            0,
            DateTime.MinValue,
            0,
            "",
            "",
            "",
            "",
            "",
            "",
            0,
            "",
            0,
            0,
            false,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            "",
            new(),
            0,
            0,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail);
        odrDetails.Add(odrDetail);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.DuplicateCheck(tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Any());
    }
    #endregion

    #region DuplicateCheck
    [Fact]
    public void ItemCommentCheck_True()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        var items = new Dictionary<string, string>()
        {
            new ("140038410", "ストーマ処置２"),
            new("140039650", "人工呼吸（鼻マスク式人工呼吸器）（５時間超）")
        };

        var allCmtCheckMst = new List<ItemCmtModel>()
        {
            new ItemCmtModel ("140038410", "comment abc", 0),
            new ItemCmtModel ("140039650", "comment bcd", 1)
        };

        var karteInf = new KarteInfModel(1, 901072057, 1, 1, 1, 20221111, "comment abc", 0, "abc", DateTime.MinValue, DateTime.MinValue, "abc");

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ItemCommentCheck(items, allCmtCheckMst, karteInf);

        // Assert
        Assert.True(!output.Any());
    }

    [Fact]
    public void ItemCommentCheck_Fail()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        var items = new Dictionary<string, string>()
        {
            new ("140038410", "ストーマ処置２"),
            new("140039650", "人工呼吸（鼻マスク式人工呼吸器）（５時間超）")
        };

        var allCmtCheckMst = new List<ItemCmtModel>()
        {
            new ItemCmtModel ("140038410", "comment abc", 0),
            new ItemCmtModel ("140039650", "comment bcd", 1)
        };

        var karteInf = new KarteInfModel(1, 901072057, 1, 1, 1, 20221111, "abc", 0, "abc", DateTime.MinValue, DateTime.MinValue, "abc");

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ItemCommentCheck(items, allCmtCheckMst, karteInf);

        // Assert
        Assert.True(!output.Any());
    }
    #endregion
}
