using Amazon.Runtime.Internal.Transform;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Interactor.MedicalExamination;
using Moq;
using System.Globalization;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.CheckedSpecialItem;

namespace CloudUnitTest.Interactor.MedicalExamination;

public class GetCheckedSpecialItemInteractorTest : BaseUT
{
    #region AgeLimitCheck
    [Test]
    public void AgeLimitCheck_True()
    {
        // Arrange
        int sinDate = 20221111, iBirthDay = 20221201, checkAge = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();

        #region Data Example
        //MaxAge = "AA"
        var tenMstAA = new TenItemModel(
            1,
            "140064650",
            "0",
            "AA",
            "140064650",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstAA);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetailAA = new OrdInfDetailModel(
            1,
            "140064650",
            20221111
            );
        odrDetails.Add(odrDetailAA);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(!output.Any());
    }

    [Test]
    public void AgeLimitCheck_MaxAge_WithAgeDiffer0()
    {
        // Arrange
        int sinDate = 20221111, iBirthDay = 19930903, checkAge = 29;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();

        #region Data Example
        //MaxAge = "B3"
        var tenMstB3 = new TenItemModel(
            1,
            "629901101",
            "15",
            "B3",
            "629901101",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstB3);

        var odrDetails = new List<OrdInfDetailModel>();

        var odrDetailB3 = new OrdInfDetailModel(
            1,
            "629901101",
            20221111
            );
        odrDetails.Add(odrDetailB3);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Test]
    public void AgeLimitCheck_MinAge_WithAgeDiffer0()
    {
        // Arrange
        int sinDate = 20221111, iBirthDay = 20221221, checkAge = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();

        #region Data Example
        //MaxAge = "B6"
        var tenMstB6 = new TenItemModel(
            1,
            "629901201",
             "B6",
            "15",
            "629901201",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstB6);

        var odrDetails = new List<OrdInfDetailModel>();

        var odrDetailB6 = new OrdInfDetailModel(
           1,
           "629901201",
           20221111
           );
        odrDetails.Add(odrDetailB6);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Test]
    public void AgeLimitCheck_MaxAge_WithMinAgeEqual0()
    {
        // Arrange
        int sinDate = 20221111, iBirthDay = 19930903, checkAge = 29;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();

        #region Data Example
        //MaxAge = "BF"
        var tenMstBF = new TenItemModel(
            1,
            "629901301",
            "0",
            "BF",
            "140064650",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstBF);

        var odrDetails = new List<OrdInfDetailModel>();

        var odrDetailBF = new OrdInfDetailModel(
           1,
           "629901301",
           20221111
           );
        odrDetails.Add(odrDetailBF);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Test]
    public void AgeLimitCheck_MinAge_WithMaxAgeEqual0()
    {
        // Arrange
        int sinDate = 20221111, iBirthDay = 20221221, checkAge = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var tenMstItems = new List<TenItemModel>();

        #region Data Example
        //MaxAge = "BF"
        var tenMstBF = new TenItemModel(
            1,
            "629901301",
            "BF",
            "0",
            "140064650",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstBF);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetailBF = new OrdInfDetailModel(
           1,
           "629901301",
           20221111
           );
        odrDetails.Add(odrDetailBF);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }
    #endregion

    #region ExpiredCheck
    [Test]
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
            "00",
            "AA",
            "140064650",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            "140037030",
            20221111
            );
        odrDetails.Add(odrDetail);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ExpiredCheck(sinDate, tenMstItems, odrDetails);

        // Assert
        Assert.True(!output.Any());
    }

    [Test]
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
            "00",
            "AA",
            "140064650",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            "140037030",
            20221111
            );
        odrDetails.Add(odrDetail);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ExpiredCheck(sinDate, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Any());
    }

    [Test]
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
            "00",
            "AA",
            "140064650",
            20220401,
            20221001
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            "140037030",
            20221111
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
    [Test]
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
            "00",
            "AA",
            "140064650",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();

        var odrDetail1 = new OrdInfDetailModel(
            1,
            "140037030",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
           1,
           "140038410",
           20221111
           );
        odrDetails.Add(odrDetail2);

        var odrDetail3 = new OrdInfDetailModel(
           1,
           "Y0001",
           20221111
           );
        odrDetails.Add(odrDetail3);

        var odrDetail4 = new OrdInfDetailModel(
          1,
          "Z0001",
          20221111
          );
        odrDetails.Add(odrDetail4);

        var odrDetail5 = new OrdInfDetailModel(
          1,
          "@BUNKATU",
          20221111
          );
        odrDetails.Add(odrDetail5);

        var odrDetail6 = new OrdInfDetailModel(
          1,
          "@REFILL",
          20221111
          );
        odrDetails.Add(odrDetail6);

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.DuplicateCheck(tenMstItems, odrDetails);

        // Assert
        Assert.True(!output.Any());
    }

    [Test]
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
            "00",
            "AA",
            "140064650",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMstError);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail = new OrdInfDetailModel(
            1,
            "140064650",
            20221111
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

    #region ItemCommentCheck
    [Test]
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
            new ItemCmtModel ("140039650", "comment abc", 1)
        };

        var karteInf = new KarteInfModel(1, 901072057, 1, 1, 1, 20221111, "comment abc", 0, "abc", DateTime.MinValue, DateTime.MinValue, "abc");

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.ItemCommentCheck(items, allCmtCheckMst, karteInf);

        var output2 = interactor.ItemCommentCheck(items, new(), karteInf);

        // Assert
        Assert.True(!output1.Any() && !output2.Any());
    }

    [Test]
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

        var karteInf = new KarteInfModel(1, 901072057, 1, 1, 1, 20221111, "comment abc", 0, "abc", DateTime.MinValue, DateTime.MinValue, "abc");

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ItemCommentCheck(items, allCmtCheckMst, karteInf);

        // Assert
        Assert.True(output.Any());
    }
    #endregion

    #region CalculationCountCheck
    [Test]
    public void CalculationCountCheck_UnitCd_997_998()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110;
        long raiinNo = 400201159, ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var tenMstItems = new List<TenItemModel>();
        var tenMst1 = new TenItemModel(
            1,
            "111014210",
            "0",
            "AA",
            "111014210",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "113003510",
            "0",
            "B3",
            "113003510",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111014210",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "113003510",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "111014210",
                997,
                10,
                0,
                20220101,
                99999999,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "113003510",
               998,
               10,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel1);
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel2);

        var itemGrpMsts = new List<ItemGrpMstModel>();

        var itemGrpMst1 = new ItemGrpMstModel(
                1,
                1,
                1,
                20220101,
                99999999,
                "111014210",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "113003510",
                1
           );
        itemGrpMsts.Add(itemGrpMst1);
        itemGrpMsts.Add(itemGrpMst2);

        var hokenIds = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 20) };
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.CalculationCountCheck(hpId, sinDate, raiinNo, ptId, santeiTenMsts, densiSanteiKaisuModels, tenMstItems, odrDetails, itemGrpMsts, hokenIds);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Test]
    public void CalculationCountCheck_UnitCd_Other()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110;
        long raiinNo = 400201159, ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var tenMstItems = new List<TenItemModel>();
        var tenMst1 = new TenItemModel(
            1,
            "113003710",
            "0",
            "AA",
            "113003710",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "113019710",
            "0",
            "B3",
            "113019710",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "113003710",
            20221111,
            120
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "113019710",
            20221111,
            120
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "113003710",
                4,
                10,
                0,
                20220101,
                99999999,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "113019710",
               5,
               10,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel1);
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel2);

        var itemGrpMsts = new List<ItemGrpMstModel>();

        var itemGrpMst1 = new ItemGrpMstModel(
                1,
                1,
                1,
                20220101,
                99999999,
                "113003710",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "113019710",
                1
           );
        itemGrpMsts.Add(itemGrpMst1);
        itemGrpMsts.Add(itemGrpMst2);

        var hokenIds = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 20) };
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.CalculationCountCheck(hpId, sinDate, raiinNo, ptId, santeiTenMsts, densiSanteiKaisuModels, tenMstItems, odrDetails, itemGrpMsts, hokenIds);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Test]
    public void CalculationCountCheck_UnitCd_Other_StartDateMoreThan0()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110;
        long raiinNo = 400201159, ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var tenMstItems = new List<TenItemModel>();
        var tenMst1 = new TenItemModel(
            1,
            "113019910",
            "0",
            "AA",
            "113019910",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "113037210",
            "0",
            "B3",
            "113037210",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "113019910",
            20221111,
            120
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "113037210",
            20221111,
            120
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "113019910",
                53,
                10,
                0,
                20220101,
                99999999,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "113037210",
               53,
               10,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel1);
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel2);

        var itemGrpMsts = new List<ItemGrpMstModel>();

        var itemGrpMst1 = new ItemGrpMstModel(
                1,
                1,
                1,
                20220101,
                99999999,
                "113019910",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "113037210",
                1
           );
        itemGrpMsts.Add(itemGrpMst1);
        itemGrpMsts.Add(itemGrpMst2);

        var hokenIds = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 20) };
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.CalculationCountCheck(hpId, sinDate, raiinNo, ptId, santeiTenMsts, densiSanteiKaisuModels, tenMstItems, odrDetails, itemGrpMsts, hokenIds);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Test]
    public void CalculationCountCheck_UnitCd_997_998_True()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110;
        long raiinNo = 400201159, ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var tenMstItems = new List<TenItemModel>();
        var tenMst1 = new TenItemModel(
            1,
            "1110001112",
            "0",
            "AA",
            "1110001112",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "111013857",
            "0",
            "B3",
            "111013857",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "1110001112",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013857",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "1110001112",
                997,
                10,
                0,
                20220101,
                99999999,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013857",
               998,
               10,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel1);
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel2);

        var itemGrpMsts = new List<ItemGrpMstModel>();

        var itemGrpMst1 = new ItemGrpMstModel(
                1,
                1,
                1,
                20220101,
                99999999,
                "1110001112",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "111013857",
                1
           );
        itemGrpMsts.Add(itemGrpMst1);
        itemGrpMsts.Add(itemGrpMst2);

        var hokenIds = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 20) };
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.CalculationCountCheck(hpId, sinDate, raiinNo, ptId, santeiTenMsts, densiSanteiKaisuModels, tenMstItems, odrDetails, itemGrpMsts, hokenIds);

        // Assert
        Assert.True(!output.Any());
    }

    [Test]
    public void CalculationCountCheck_UnitCd_Other_True()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110;
        long raiinNo = 400201159, ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var tenMstItems = new List<TenItemModel>();
        var tenMst1 = new TenItemModel(
            1,
            "113037410",
            "0",
            "AA",
            "113037410",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "113037610",
            "0",
            "B3",
            "113037610",
            20220401,
            99999999
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "113037410",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "113037610",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "113037410",
                53,
                100,
                0,
                20220101,
                99999999,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "113037610",
               53,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel1);
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel2);

        var itemGrpMsts = new List<ItemGrpMstModel>();

        var itemGrpMst1 = new ItemGrpMstModel(
                1,
                1,
                1,
                20220101,
                99999999,
                "113037410",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "113037610",
                1
           );
        itemGrpMsts.Add(itemGrpMst1);
        itemGrpMsts.Add(itemGrpMst2);

        var hokenIds = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 20) };
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.CalculationCountCheck(hpId, sinDate, raiinNo, ptId, santeiTenMsts, densiSanteiKaisuModels, tenMstItems, odrDetails, itemGrpMsts, hokenIds);

        // Assert
        Assert.True(!output.Any());
    }
    #endregion

    #region Check Age
    [Test]
    public void CheckAge_AA()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck = "AA";
        int iDays1 = 28, iDays2 = 1, sinDate = 20221111, iBirthDay = 19930903, iYear = 29;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck, iDays1, sinDate, iBirthDay, iYear);
        var output2 = interactor.CheckAge(tenMstAgeCheck, iDays2, sinDate, iBirthDay, iYear);

        // Assert
        Assert.True(output1 && !output2);
    }

    [Test]
    public void CheckAge_B3()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck = "B3";
        var now = DateTime.Now;
        var year = now.Year - 3;
        int iDays = 28, sinDate = year * 10000 + 1110, iBirthDay1 = year * 10000 + 1009, iBirthDay2 = year * 10000 + 1211, iYear1 = 29, iYear2 = 3, iYear3 = 1;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear1);
        var output2 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear2);
        var output3 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear3);
        var output4 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay2, iYear2);

        // Assert
        Assert.True(output1 && output2 && !output3 && !output4);
    }

    [Test]
    public void CheckAge_B6()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck = "B6";
        var now = DateTime.Now;
        var year = now.Year - 6;
        int iDays = 28, sinDate = year * 10000 + 1110, iBirthDay1 = year * 10000 + 1009, iBirthDay2 = year * 10000 + 1211, iYear1 = 29, iYear2 = 6, iYear3 = 1;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear1);
        var output2 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear2);
        var output3 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear3);
        var output4 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay2, iYear2);

        // Assert
        Assert.True(output1 && output2 && !output3 && !output4);
    }

    [Test]
    public void CheckAge_BF()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck = "BF";
        var now = DateTime.Now;
        var year = now.Year - 15;
        int iDays = 28, sinDate = year * 10000 + 1110, iBirthDay1 = year * 10000 + 1009, iBirthDay2 = year * 10000 + 1211, iYear1 = 29, iYear2 = 15, iYear3 = 1;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear1);
        var output2 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear2);
        var output3 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear3);
        var output4 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay2, iYear2);

        // Assert
        Assert.True(output1 && output2 && !output3 && !output4);
    }

    [Test]
    public void CheckAge_BK()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck = "BK";
        var now = DateTime.Now;
        var year = now.Year - 20;
        int iDays = 28, sinDate = year * 10000 + 1110, iBirthDay1 = year * 10000 + 1009, iBirthDay2 = year * 10000 + 1211, iYear1 = 29, iYear2 = 20, iYear3 = 1;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear1);
        var output2 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear2);
        var output3 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear3);
        var output4 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay2, iYear2);

        // Assert
        Assert.True(output1 && output2 && !output3 && !output4);
    }

    [Test]
    public void CheckAge_AE()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck = "AE";
        int iDays1 = 90, iDays2 = 1, sinDate = 20221111, iBirthDay = 19930903, iYear = 29;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck, iDays1, sinDate, iBirthDay, iYear);
        var output2 = interactor.CheckAge(tenMstAgeCheck, iDays2, sinDate, iBirthDay, iYear);

        // Assert
        Assert.True(output1 && !output2);
    }

    [Test]
    public void CheckAge_MG()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck = "MG";
        var now = DateTime.Now;
        var year = now.Year - 20;
        int iDays = 28, sinDate = year * 10000 + 1110, iBirthDay1 = (year - 7) * 10000 + 1009, iBirthDay2 = (year + 7) * 10000 + 1211,
           iBirthDay3 = (year - 6) * 10000 + 0301, iBirthDay4 = (year + 6) * 10000 + 0301, iYear = 29;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay1, iYear);
        var output2 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay2, iYear);
        var output3 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay3, iYear);
        var output4 = interactor.CheckAge(tenMstAgeCheck, iDays, sinDate, iBirthDay4, iYear);

        // Assert
        Assert.True(output1 && !output2 && output3 && !output4);
    }

    [Test]
    public void CheckAge_Other()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        string tenMstAgeCheck1 = "30", tenMstAgeCheck2 = "28", tenMstAgeCheck3 = "B";
        int iDays1 = 90, iDays2 = 1, sinDate = 20221111, iBirthDay = 19930903, iYear1 = 29, iYear2 = 0;

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.CheckAge(tenMstAgeCheck1, iDays1, sinDate, iBirthDay, iYear1);
        var output2 = interactor.CheckAge(tenMstAgeCheck2, iDays2, sinDate, iBirthDay, iYear1);
        var output3 = interactor.CheckAge(tenMstAgeCheck3, iDays2, sinDate, iBirthDay, iYear1);
        var output4 = interactor.CheckAge(tenMstAgeCheck3, iDays2, sinDate, iBirthDay, iYear2);

        // Assert
        Assert.True(!output1 && output2 && output3 && output4);
    }

    #endregion

    #region Common DensiSantei
    [Test]
    public void CommonDensiSantei_53()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               53,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;
        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);

        // Assert
        Assert.True(sTerm == "患者あたり");
    }

    [Test]
    public void CommonDensiSantei_121()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               121,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;
        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);

        // Assert
        Assert.True(sTerm == "日" && startDate == sinDate);
    }

    [Test]
    public void CommonDensiSantei_131()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "113037810",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "113037810",
               131,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;
        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = sinDate / 100 * 100 + 1;
        // Assert
        Assert.True(sTerm == "月" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_138()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               138,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;
        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.WeeksBefore(sinDate, 1);
        // Assert
        Assert.True(sTerm == "週" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_141()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               141,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);

        // Assert
        Assert.True(sTerm == "一連" && startDate == -1);
    }

    [Test]
    public void CommonDensiSantei_142()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               142,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.WeeksBefore(sinDate, 2);

        // Assert
        Assert.True(sTerm == "2週" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_143()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               143,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.MonthsBefore(sinDate, 1);

        // Assert
        Assert.True(sTerm == "2月" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_144()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               144,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.MonthsBefore(sinDate, 2);

        // Assert
        Assert.True(sTerm == "3月" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_145()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               145,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.MonthsBefore(sinDate, 3);

        // Assert
        Assert.True(sTerm == "4月" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_146()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               146,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.MonthsBefore(sinDate, 5);

        // Assert
        Assert.True(sTerm == "6月" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_147()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               147,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.MonthsBefore(sinDate, 11);

        // Assert
        Assert.True(sTerm == "12月" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_148()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               148,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm, sinDate, sysyosinDate);
        var newSinDate = interactor.YearsBefore(sinDate, 5);

        // Assert
        Assert.True(sTerm == "5年" && startDate == newSinDate);
    }

    [Test]
    public void CommonDensiSantei_997()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        mockTodayRepo.Setup(repo => repo.MonthsAfterExcludeHoliday(1, sysyosinDate, 1))
        .Returns(20191212);
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "11101385131",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
               1,
               1,
               "111014210",
               997,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
             1,
             1,
             "1110142111",
             997,
             100,
             0,
             20220101,
             99999999,
             1,
             1,
             1,
             1,
             1,
             1,
             2
         );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate1 = 0, endDate2 = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel1, odrDetail2, odrDetails, ref startDate, ref endDate1, ref sTerm, sinDate, sysyosinDate);

        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate2, ref sTerm, sinDate, sysyosinDate);

        // Assert
        Assert.True(endDate1 == 99999999 && endDate2 == 20191212);
    }

    [Test]
    public void CommonDensiSantei_998()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "11101385131",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
               1,
               1,
               "111014210",
               998,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
           );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
             1,
             1,
             "1110142111",
             998,
             100,
             0,
             20220101,
             99999999,
             1,
             1,
             1,
             1,
             1,
             1,
             2
         );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate1 = 0, endDate2 = 0;
        string sTerm = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel1, odrDetail2, odrDetails, ref startDate, ref endDate1, ref sTerm, sinDate, sysyosinDate);

        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate, ref endDate2, ref sTerm, sinDate, sysyosinDate);
        var newSysyosinDate = interactor.MonthsAfter(sysyosinDate, 1);
        // Assert
        Assert.True(endDate1 == 99999999 && endDate2 == newSysyosinDate);
    }

    [Test]
    public void CommonDensiSantei_999_TermSbt2()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               999,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               2,
               1,
               2
           );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
         1,
         1,
         "111013850",
         999,
         100,
         0,
         20220101,
         99999999,
         1,
         1,
         1,
         2,
         2,
         1,
         2
     );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate1 = 0, startDate2 = 0, endDate = 0;
        string sTerm1 = string.Empty, sTerm2 = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel1, odrDetail1, odrDetails, ref startDate1, ref endDate, ref sTerm1, sinDate, sysyosinDate);
        var newSinDate1 = interactor.DaysBefore(sinDate, densiSanteiKaisuModel1.TermCount);

        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate2, ref endDate, ref sTerm2, sinDate, sysyosinDate);
        var newSinDate2 = interactor.DaysBefore(sinDate, densiSanteiKaisuModel2.TermCount);

        // Assert
        Assert.True(sTerm1 == "日" && startDate1 == newSinDate1 && sTerm2 == densiSanteiKaisuModel2.TermCount + "日" && startDate2 == newSinDate2);
    }

    [Test]
    public void CommonDensiSantei_999_TermSbt3()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               999,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               3,
               1,
               2
           );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
         1,
         1,
         "111013850",
         999,
         100,
         0,
         20220101,
         99999999,
         1,
         1,
         1,
         2,
         3,
         1,
         2
     );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate1 = 0, startDate2 = 0, endDate = 0;
        string sTerm1 = string.Empty, sTerm2 = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel1, odrDetail1, odrDetails, ref startDate1, ref endDate, ref sTerm1, sinDate, sysyosinDate);
        var newSinDate1 = interactor.WeeksBefore(sinDate, densiSanteiKaisuModel1.TermCount);

        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate2, ref endDate, ref sTerm2, sinDate, sysyosinDate);
        var newSinDate2 = interactor.WeeksBefore(sinDate, densiSanteiKaisuModel2.TermCount);

        // Assert
        Assert.True(sTerm1 == "週" && startDate1 == newSinDate1 && sTerm2 == densiSanteiKaisuModel2.TermCount + "週" && startDate2 == newSinDate2);
    }

    [Test]
    public void CommonDensiSantei_999_TermSbt4()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               999,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               4,
               1,
               2
           );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
         1,
         1,
         "111013850",
         999,
         100,
         0,
         20220101,
         99999999,
         1,
         1,
         1,
         2,
         4,
         1,
         2
     );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate1 = 0, startDate2 = 0, endDate = 0;
        string sTerm1 = string.Empty, sTerm2 = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel1, odrDetail1, odrDetails, ref startDate1, ref endDate, ref sTerm1, sinDate, sysyosinDate);
        var newSinDate1 = interactor.MonthsBefore(sinDate, densiSanteiKaisuModel1.TermCount);

        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate2, ref endDate, ref sTerm2, sinDate, sysyosinDate);
        var newSinDate2 = interactor.MonthsBefore(sinDate, densiSanteiKaisuModel2.TermCount);

        // Assert
        Assert.True(sTerm1 == "月" && startDate1 == newSinDate1 && sTerm2 == densiSanteiKaisuModel2.TermCount + "月" && startDate2 == newSinDate2);
    }

    [Test]
    public void CommonDensiSantei_999_TermSbt5()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               999,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               5,
               1,
               2
           );

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
         1,
         1,
         "111013850",
         999,
         100,
         0,
         20220101,
         99999999,
         1,
         1,
         1,
         2,
         5,
         1,
         2
     );
        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate1 = 0, startDate2 = 0, endDate = 0;
        string sTerm1 = string.Empty, sTerm2 = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel1, odrDetail1, odrDetails, ref startDate1, ref endDate, ref sTerm1, sinDate, sysyosinDate);
        var newSinDate1 = (sinDate / 10000 - (densiSanteiKaisuModel1.TermCount - 1)) * 10000 + 101;

        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel2, odrDetail1, odrDetails, ref startDate2, ref endDate, ref sTerm2, sinDate, sysyosinDate);
        var newSinDate2 = (sinDate / 10000 - (densiSanteiKaisuModel2.TermCount - 1)) * 10000 + 101;

        // Assert
        Assert.True(sTerm1 == "年間" && startDate1 == newSinDate1 && sTerm2 == densiSanteiKaisuModel2.TermCount + "年間" && startDate2 == newSinDate2);
    }

    [Test]
    public void CommonDensiSantei_Other()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            "111000110",
            20221111
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            "111013850",
            20221111
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
               1,
               100,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               5,
               1,
               2
           );

        #endregion

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        int startDate = 0, endDate = 0;
        string sTerm1 = string.Empty;

        // Act
        interactor.CommonDensiSantei(hpId, densiSanteiKaisuModel, odrDetail1, odrDetails, ref startDate, ref endDate, ref sTerm1, sinDate, sysyosinDate);

        // Assert
        Assert.True(startDate == -1);
    }
    #endregion

    #region util function
    [Test]
    public void WeekBefore()
    {
        // Arrange
        int baseDate = 20191111, term = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        string s = baseDate.ToString("D8");
        var dt1 = DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture);
        dt1 = dt1.AddDays((int)dt1.DayOfWeek * -1 + -7 * (term - 1));
        var retDate = int.Parse(dt1.ToString("yyyyMMdd"));

        var output = interactor.WeeksBefore(baseDate, term);

        // Assert
        Assert.True(output == retDate);
    }

    [Test]
    public void MonthsBefore()
    {
        // Arrange
        int baseDate = 20191111, term = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        string s = baseDate.ToString("D8");
        var dt1 = DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture);
        dt1 = dt1.AddMonths(term * -1);
        var retDate = int.Parse(dt1.ToString("yyyyMMdd"));
        retDate = retDate / 100 * 100 + 1;

        var output = interactor.MonthsBefore(baseDate, term);

        // Assert
        Assert.True(output == retDate);
    }

    [Test]
    public void YearsBefore()
    {
        // Arrange
        int baseDate = 20191111, term = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        string s = baseDate.ToString("D8");
        var dt1 = DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture);
        dt1 = dt1.AddYears(term * -1);
        var retDate = int.Parse(dt1.ToString("yyyyMMdd"));
        retDate = retDate / 100 * 100 + 1;

        var output = interactor.YearsBefore(baseDate, term);

        // Assert
        Assert.True(output == retDate);
    }

    [Test]
    public void DaysBefore()
    {
        // Arrange
        int baseDate = 20191111, term = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        string s = baseDate.ToString("D8");
        var dt1 = DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture);
        dt1 = dt1.AddDays((term - 1) * -1);
        var retDate = int.Parse(dt1.ToString("yyyyMMdd"));

        var output = interactor.DaysBefore(baseDate, term);

        // Assert
        Assert.True(output == retDate);
    }

    [Test]
    public void MonthsAfter()
    {
        // Arrange
        int baseDate = 20191111, term = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        string s = baseDate.ToString("D8");
        var dt1 = DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture);
        dt1 = dt1.AddMonths(term);
        var retDate = int.Parse(dt1.ToString("yyyyMMdd"));

        var output = interactor.MonthsAfter(baseDate, term);

        // Assert
        Assert.True(output == retDate);
    }

    [Test]
    public void GetHokenKbn()
    {
        // Arrange
        int odrHokenKbn1 = 0, odrHokenKbn2 = 1, odrHokenKbn3 = 2, odrHokenKbn4 = 11, odrHokenKbn5 = 12, odrHokenKbn6 = 13, odrHokenKbn7 = 14;
        var mockMstItemRepo = new Mock<IMstItemRepository>();

        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act

        var output1 = interactor.GetHokenKbn(odrHokenKbn1);
        var output2 = interactor.GetHokenKbn(odrHokenKbn2);
        var output3 = interactor.GetHokenKbn(odrHokenKbn3);
        var output4 = interactor.GetHokenKbn(odrHokenKbn4);
        var output5 = interactor.GetHokenKbn(odrHokenKbn5);
        var output6 = interactor.GetHokenKbn(odrHokenKbn6);
        var output7 = interactor.GetHokenKbn(odrHokenKbn7);

        // Assert
        Assert.True(output1 == 4 && output2 == 0 && output3 == 0 && output4 == 1 && output5 == 1 && output6 == 2 && output7 == 3);
    }

    [Test]
    public void GetCheckSanteiKbns()
    {
        // Arrange
        int odrHokenKbn = 0, hokensyuHandling = 1;
        var mockMstItemRepo = new Mock<IMstItemRepository>();

        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.GetCheckSanteiKbns(odrHokenKbn, hokensyuHandling);

        // Assert
        Assert.True(output.Count == 2 && output.Contains(2));
    }

    [Test]
    public void GetCheckHokenKbns()
    {
        // Arrange
        int hokensyuHandling1 = 0, hokensyuHandling2 = 1, hokensyuHandling3 = 2, odrHokenKbn1 = 0, odrHokenKbn2 = 1, odrHokenKbn3 = 2, odrHokenKbn4 = 11, odrHokenKbn5 = 12, odrHokenKbn6 = 13, odrHokenKbn7 = 14;

        var mockMstItemRepo = new Mock<IMstItemRepository>();

        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.GetCheckHokenKbns(odrHokenKbn2, hokensyuHandling1);
        var output2 = interactor.GetCheckHokenKbns(odrHokenKbn3, hokensyuHandling1);
        var output3 = interactor.GetCheckHokenKbns(odrHokenKbn4, hokensyuHandling1);
        var output4 = interactor.GetCheckHokenKbns(odrHokenKbn5, hokensyuHandling1);
        var output5 = interactor.GetCheckHokenKbns(odrHokenKbn6, hokensyuHandling1);
        var output6 = interactor.GetCheckHokenKbns(odrHokenKbn7, hokensyuHandling1);
        var output7 = interactor.GetCheckHokenKbns(odrHokenKbn1, hokensyuHandling1);
        var output8 = interactor.GetCheckHokenKbns(odrHokenKbn1, hokensyuHandling2);
        var output9 = interactor.GetCheckHokenKbns(odrHokenKbn1, hokensyuHandling3);

        // Assert
        Assert.That(new List<int> { 0, 1, 2, 3 }, Is.EqualTo(output1));
        Assert.That(new List<int> { 0, 1, 2, 3 }, Is.EqualTo(output2));
        Assert.That(new List<int> { 0, 1, 2, 3 }, Is.EqualTo(output3));
        Assert.That(new List<int> { 0, 1, 2, 3 }, Is.EqualTo(output4));
        Assert.That(new List<int> { 0, 1, 2, 3 }, Is.EqualTo(output5));
        Assert.That(new List<int> { 0, 1, 2, 3 }, Is.EqualTo(output6));
        Assert.Contains(4, output7);
        Assert.That(new List<int> { 0, 1, 2, 3, 4, 0 }, Is.EqualTo(output8));
        Assert.True(output9.Contains(4) && output9.Contains(0));
    }


    [Test]
    public void GetPtHokenKbn()
    {
        // Arrange
        int hpId = 1, ptId = 5, sinDate = 20221111, rpNo = 1, edano = 1;
        List<(long rpno, long edano, int hokenId)> hokenIds1 = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 11) };

        List<(long rpno, long edano, int hokenId)> hokenIds2 = new List<(long rpno, long edano, int hokenId)> { new(1, 2, 10), new(1, 3, 11) };

        var mockMstItemRepo = new Mock<IMstItemRepository>();

        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        mockInsuranceRepo.Setup(repo => repo.GetPtHokenInf(1, 10, 5, sinDate))
.Returns(new InsuranceModel(1, 5, 1, 1, 10, 1, 1, 1, 1, 1, 1, 1, 1, 0));
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output1 = interactor.GetPtHokenKbn(hpId, ptId, sinDate, rpNo, edano, hokenIds1);
        var output2 = interactor.GetPtHokenKbn(hpId, ptId, sinDate, rpNo, edano, hokenIds2);

        // Assert
        Assert.True(output1 == 1 && output2 == 0);
    }
    #endregion

    #region Main
    [Test]
    public void Handle_HpId()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(-1, 1, 1, 20221111, 1, 1, 1, new(), new(), new(), true, true);
        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.Status == CheckedSpecialItemStatus.InvalidHpId && output.CheckSpecialItemModels.Count() == 0);
    }

    [Test]
    public void Handle_PtId()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(1, 1, 0, 20221111, 1, 1, 1, new(), new(), new(), true, true);
        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.Status == CheckedSpecialItemStatus.InvalidPtId && output.CheckSpecialItemModels.Count() == 0);
    }

    [Test]
    public void Handle_SinDate()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(1, 1, 1, 0, 1, 1, 1, new(), new(), new(), true, true);
        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.Status == CheckedSpecialItemStatus.InvalidSinDate && output.CheckSpecialItemModels.Count() == 0);
    }

    [Test]
    public void Handle_IBirthDay()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(1, 1, 1, 20221111, 0, 1, 1, new(), new(), new(), true, true);
        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.Status == CheckedSpecialItemStatus.InvalidIBirthDay && output.CheckSpecialItemModels.Count() == 0);
    }

    [Test]
    public void Handle_CheckAge()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(1, 1, 1, 20221111, 1, -1, 1, new(), new(), new(), true, true);
        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.Status == CheckedSpecialItemStatus.InvalidCheckAge && output.CheckSpecialItemModels.Count() == 0);
    }

    [Test]
    public void Handle_RaiinNo()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(1, 1, 1, 20221111, 1, 1, -1, new(), new(), new(), true, true);
        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.Status == CheckedSpecialItemStatus.InvalidRaiinNo && output.CheckSpecialItemModels.Count() == 0);
    }

    [Test]
    public void Handle_InvalidOrderAndKarte()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(1, 1, 1, 20221111, 1, 1, 1, new(), new(), new(), true, true);
        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.Status == CheckedSpecialItemStatus.InvalidOdrInfDetail && output.CheckSpecialItemModels.Count() == 0);
    }

    [Test]
    public void Handle_EnabledInputCheck_True()
    {
        // Arrange
        var mockMstItemRepo1 = new Mock<IMstItemRepository>();
        var mockMstItemRepo2 = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        mockMstItemRepo1.Setup(repo => repo.FindTenMst(1, new List<string>() { "1110001101", "111000110", "111013850", "111014210", "113019710" }, 20220101, 20220101)).Returns(new List<TenItemModel>() {
            new TenItemModel(
            1,
            "1110001101",
            "15",
            "AA",
            "1110001101",
            20210101,
            99999999
            ),
            new TenItemModel(
            1,
            "111000110",
            "15",
            "AA",
            "111000110",
            20220101,
            99999999
            ),
            new TenItemModel(
            1,
           "111013850",

           "00",
           "AA",
           "111013850",
           20220401,
           99999999
           ),
           new TenItemModel(
             1,
            "111014210",

            "0",
            "AA",
            "111014210",
            20220401,
            99999999
           ),
           new TenItemModel(
            1,
           "113019710",
           "00",
           "AA",
           "113019710",
           20220401,
           99999999
           )
        });
        mockMstItemRepo2.Setup(repo => repo.FindTenMst(1, new List<string>() { "1110001101", "111000110", "111013850", "111014210", "113019710" }, 20220101, 20220101)).Returns(new List<TenItemModel>() {
            new TenItemModel(
            1,
            "1110001101",
            "15",
            "AA",
            "1110001101",
            20221001,
            99999999
            )
        });
        mockTodayRepo.Setup(repo => repo.FindDensiSanteiKaisuList(1, new List<string>() { "1110001101", "111000110", "111013850", "111014210", "113019710" }, 20220101, 20220101)).Returns(new List<DensiSanteiKaisuModel> { new DensiSanteiKaisuModel(
                1,
                1,
                "111014210",
                997,
                10,
                0,
                20220101,
                99999999,
                1,
                1,
                1,
                1,
                1,
                1,
                1
          ),
          new DensiSanteiKaisuModel(
               1,
               1,
               "113019710",
               998,
               10,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
          )
        });
        mockMstItemRepo1.Setup(repo => repo.FindItemGrpMst(1, 20220101, 1, new List<long>() { 1, 2 })).Returns(new List<ItemGrpMstModel>() { new ItemGrpMstModel(
                    1,
                    1,
                    1,
                    20220101,
                    99999999,
                    "111014210",
                    1
                ),
                new ItemGrpMstModel(
                    1,
                    1,
                    2,
                    20220101,
                    99999999,
                    "113019710",
                    1
                )
        });
        mockMstItemRepo2.Setup(repo => repo.FindItemGrpMst(1, 20220101, 1, new List<long>() { 1, 2 })).Returns(new List<ItemGrpMstModel>() { new ItemGrpMstModel(
                    1,
                    1,
                    1,
                    20220101,
                    99999999,
                    "111014210",
                    1
                ),
                new ItemGrpMstModel(
                    1,
                    1,
                    2,
                    20220101,
                    99999999,
                    "113019710",
                    1
                )
        });

        var interactor1 = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo1.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);
        var interactor2 = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo2.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        var odrDetails = new List<OdrInfDetailItemInputData>();
        var odrDetail = new OdrInfDetailItemInputData(
            1,
            "1110001101",
            20220101
            );
        odrDetails.Add(odrDetail);

        var odrDetailAgeLimitCheck = new OdrInfDetailItemInputData(
            1,
            "111000110",
            20220101
            );
        odrDetails.Add(odrDetailAgeLimitCheck);

        var odrDetailExpiredCheck = new OdrInfDetailItemInputData(
            1,
            "111013850",
            20220101
            );
        odrDetails.Add(odrDetailExpiredCheck);

        var odrDetailCalculationCheck = new OdrInfDetailItemInputData(
            1,
            "111014210",
            20220101
            );
        odrDetails.Add(odrDetailCalculationCheck);

        var odrDetailDuplicateCheck = new OdrInfDetailItemInputData(
            1,
            "113019710",
            20220101
            );
        odrDetails.Add(odrDetailDuplicateCheck);
        odrDetails.Add(odrDetailDuplicateCheck);

        var hokenIds = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 20) };

        var checkedSpecialItemInputData1 = new CheckedSpecialItemInputData(1, 1, 1, 20220101, 19930903, 1, 1, new List<OdrInfItemInputData>() { new OdrInfItemInputData(1, 11111000, 1, 1, 1, 20220101, 1, 1, "abc", 1, 1, 1, 1, 1, 1, 1, 1, odrDetails, 0) }, new(), new(), true, false);
        var checkedSpecialItemInputData2 = new CheckedSpecialItemInputData(1, 1, 1, 20220101, 19930903, 1, 1, new List<OdrInfItemInputData>() { new OdrInfItemInputData(1, 11111000, 1, 1, 1, 20220101, 1, 1, "abc", 1, 1, 1, 1, 1, 1, 1, 1, odrDetails, 0) }, new(), new(), true, false);
        // Act
        var output1 = interactor1.Handle(checkedSpecialItemInputData1);
        var output2 = interactor2.Handle(checkedSpecialItemInputData2);

        // Assert
        Assert.True(output1.CheckSpecialItemModels.Any(o => o.CheckingType == Helper.Enum.CheckSpecialType.AgeLimit));
        Assert.True(output1.CheckSpecialItemModels.Any(o => o.CheckingType == Helper.Enum.CheckSpecialType.Duplicate));
        Assert.True(output1.CheckSpecialItemModels.Any(o => o.CheckingType == Helper.Enum.CheckSpecialType.CalculationCount));
        Assert.True(output2.CheckSpecialItemModels.Any(o => o.CheckingType == Helper.Enum.CheckSpecialType.Expiration));
        Assert.True(output1.Status == CheckedSpecialItemStatus.Successed && output2.Status == CheckedSpecialItemStatus.Successed);
    }

    [Test]
    public void Handle_EnabledCommentCheck_True()
    {
        // Arrange
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();
        mockMstItemRepo.Setup(repo => repo.FindTenMst(1, new List<string>() { "1110001101", "111000110", "111013850", "111014210", "113019710" }, 20220101, 20220101)).Returns(new List<TenItemModel>() {
            new TenItemModel(
            1,
            "1110001101",
            "15",
            "AA",
            "1110001101",
            20210101,
            99999999
            ),
            new TenItemModel(
            1,
            "111000110",
            "15",
            "AA",
            "111000110",
            20220101,
            99999999
            ),
            new TenItemModel(
            1,
           "111013850",
           "00",
           "AA",
           "111013850",
           20220401,
           99999999
           ),
           new TenItemModel(
             1,
            "111014210",
            "0",
            "AA",
            "111014210",
            20220401,
            99999999
           ),
           new TenItemModel(
            1,
           "113019710",
           "00",
           "AA",
           "113019710",
           20220401,
           99999999
           )
        });
        mockTodayRepo.Setup(repo => repo.FindDensiSanteiKaisuList(1, new List<string>() { "1110001101", "111000110", "111013850", "111014210", "113019710" }, 20220101, 20220101)).Returns(new List<DensiSanteiKaisuModel> { new DensiSanteiKaisuModel(
                1,
                1,
                "111014210",
                997,
                10,
                0,
                20220101,
                99999999,
                1,
                1,
                1,
                1,
                1,
                1,
                1
          ),
          new DensiSanteiKaisuModel(
               1,
               1,
               "113019710",
               998,
               10,
               0,
               20220101,
               99999999,
               1,
               1,
               1,
               1,
               1,
               1,
               2
          )
        });
        mockMstItemRepo.Setup(repo => repo.FindItemGrpMst(1, 20220101, 1, new List<long>() { 1, 2 })).Returns(new List<ItemGrpMstModel>() { new ItemGrpMstModel(
                    1,
                    1,
                    1,
                    20220101,
                    99999999,
                    "111014210",
                    1
                ),
                new ItemGrpMstModel(
                    1,
                    1,
                    2,
                    20220101,
                    99999999,
                    "113019710",
                    1
                )
        });
        mockMstItemRepo.Setup(repo => repo.FindItemGrpMst(1, 20220101, 1, new List<long>() { 1, 2 })).Returns(new List<ItemGrpMstModel>() { new ItemGrpMstModel(
                    1,
                    1,
                    1,
                    20220101,
                    99999999,
                    "111014210",
                    1
                ),
                new ItemGrpMstModel(
                    1,
                    1,
                    2,
                    20220101,
                    99999999,
                    "113019710",
                    1
                )
        });
        mockMstItemRepo.Setup(repo => repo.GetCmtCheckMsts(1, 1, new List<string>() { "1110001101", "111000110", "111013850", "111014210", "113019710" })).Returns(new List<ItemCmtModel>() {
            new ItemCmtModel(
                   "111000110",
                   "comment abc",
                   0
                ),
            new ItemCmtModel(
                   "111013850",
                   "comment bcd",
                    1
                )
        });

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        var odrDetails = new List<OdrInfDetailItemInputData>();
        var odrDetail = new OdrInfDetailItemInputData(
            1,
            "1110001101",
            20220101
            );
        odrDetails.Add(odrDetail);

        var odrDetailAgeLimitCheck = new OdrInfDetailItemInputData(
            1,
            "111000110",
            20220101
            );
        odrDetails.Add(odrDetailAgeLimitCheck);

        var odrDetailExpiredCheck = new OdrInfDetailItemInputData(
            1,
            "111013850",
            20220101
            );
        odrDetails.Add(odrDetailExpiredCheck);

        var odrDetailCalculationCheck = new OdrInfDetailItemInputData(
            1,
            "111014210",
            20220101
            );
        odrDetails.Add(odrDetailCalculationCheck);

        var odrDetailDuplicateCheck = new OdrInfDetailItemInputData(
            1,
            "113019710",
            20220101
            );
        odrDetails.Add(odrDetailDuplicateCheck);
        odrDetails.Add(odrDetailDuplicateCheck);

        var hokenIds = new List<(long rpno, long edano, int hokenId)> { new(1, 1, 10), new(2, 1, 20) };

        var checkedSpecialItemInputData = new CheckedSpecialItemInputData(1, 1, 1, 20220101, 19930903, 1, 1, new List<OdrInfItemInputData>() { new OdrInfItemInputData(1, 11111000, 1, 1, 1, 20220101, 1, 1, "abc", 1, 1, 1, 1, 1, 1, 1, 1, odrDetails, 0) }, new(), new KarteItemInputData(1, 901072057, 1, 20221111, "comment abc", 0, "abc"), false, true);

        // Act
        var output = interactor.Handle(checkedSpecialItemInputData);

        // Assert
        Assert.True(output.CheckSpecialItemModels.Any(o => o.CheckingType == Helper.Enum.CheckSpecialType.ItemComment));
        Assert.True(output.Status == CheckedSpecialItemStatus.Successed);
    }
    #endregion
}