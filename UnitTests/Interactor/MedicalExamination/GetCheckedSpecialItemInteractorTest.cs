using Amazon.Runtime.Internal.Transform;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.Insurance;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Entity.Tenant;
using Helper.Constants;
using Interactor.MedicalExamination;
using Moq;
using System;
using System.Globalization;

namespace UnitTests.Interactor.MedicalExamination;

public class GetCheckedSpecialItemInteractorTest
{
    #region AgeLimitCheck
    [Fact]
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
            "0",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAA);

        //MaxAge = "B3"
        var tenMstB3 = new TenItemModel(
            1,
            "629901101",
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
            "0",
            "B3",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB3);

        //MaxAge = "B6"
        var tenMstB6 = new TenItemModel(
            1,
            "629901201",
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
            "0",
            "B6",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB6);

        //MaxAge = "BF"
        var tenMstBF = new TenItemModel(
            1,
            "629901301",
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
            "0",
            "BF",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBF);

        //MaxAge = "BK"
        var tenMstBK = new TenItemModel(
            1,
            "629901401",
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
            "0",
            "BK",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBK);

        //MaxAge = "AE"
        var tenMstAE = new TenItemModel(
            1,
            "629901501",
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
            "0",
            "AE",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAE);

        //MaxAge = "MG"
        var tenMstMG = new TenItemModel(
            1,
            "629901601",
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
            "0",
            "MG",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstMG);

        //MaxAge = "MG"
        var tenMstOther = new TenItemModel(
            1,
            "620507802",
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
            "0",
            "30",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstOther);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetailAA = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140064650",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailAA);

        var odrDetailB3 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "629901101",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailB3);

        var odrDetailB6 = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901201",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailB6);

        var odrDetailBF = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901301",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBF);

        var odrDetailBK = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901401",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBK);

        var odrDetailAE = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901501",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailAE);

        var odrDetailMG = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901601",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailMG);

        var odrDetailOrther = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "620507802",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailOrther);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(!output.Any());
    }

    [Fact]
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
        //MaxAge = "AA"
        var tenMstAA = new TenItemModel(
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
            "15",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAA);

        //MaxAge = "B3"
        var tenMstB3 = new TenItemModel(
            1,
            "629901101",
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
            "15",
            "B3",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB3);

        //MaxAge = "B6"
        var tenMstB6 = new TenItemModel(
            1,
            "629901201",
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
            "15",
            "B6",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB6);

        //MaxAge = "BF"
        var tenMstBF = new TenItemModel(
            1,
            "629901301",
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
            "15",
            "BF",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBF);

        //MaxAge = "BK"
        var tenMstBK = new TenItemModel(
            1,
            "629901401",
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
            "15",
            "BK",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBK);

        //MaxAge = "AE"
        var tenMstAE = new TenItemModel(
            1,
            "629901501",
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
            "15",
            "AE",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAE);

        //MaxAge = "MG"
        var tenMstMG = new TenItemModel(
            1,
            "629901601",
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
            "15",
            "MG",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstMG);

        //MaxAge = "MG"
        var tenMstOther = new TenItemModel(
            1,
            "620507802",
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
            "15",
            "29",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstOther);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetailAA = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140064650",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailAA);

        var odrDetailB3 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "629901101",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailB3);

        var odrDetailB6 = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901201",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailB6);

        var odrDetailBF = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901301",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBF);

        var odrDetailBK = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901401",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBK);

        var odrDetailAE = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901501",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailAE);

        var odrDetailMG = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901601",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailMG);

        var odrDetailOrther = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "620507802",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailOrther);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Fact]
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
        //MaxAge = "AA"
        var tenMstAA = new TenItemModel(
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
            "AA",
            "15",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAA);

        //MaxAge = "B3"
        var tenMstB3 = new TenItemModel(
            1,
            "629901101",
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
            "B3",
            "15",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB3);

        //MaxAge = "B6"
        var tenMstB6 = new TenItemModel(
            1,
            "629901201",
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
            "B6",
            "15",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB6);

        //MaxAge = "BF"
        var tenMstBF = new TenItemModel(
            1,
            "629901301",
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
            "BF",
            "15",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBF);

        //MaxAge = "BK"
        var tenMstBK = new TenItemModel(
            1,
            "629901401",
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
            "BK",
            "15",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBK);

        //MaxAge = "AE"
        var tenMstAE = new TenItemModel(
            1,
            "629901501",
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
            "AE",
            "15",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAE);

        //MaxAge = "MG"
        var tenMstMG = new TenItemModel(
            1,
            "629901601",
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
            "MG",
            "15",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstMG);

        //MaxAge = "MG"
        var tenMstOther = new TenItemModel(
            1,
            "620507802",
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
            "15",
            "30",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstOther);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetailAA = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140064650",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailAA);

        var odrDetailB3 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "629901101",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailB3);

        var odrDetailB6 = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901201",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailB6);

        var odrDetailBF = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901301",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBF);

        var odrDetailBK = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901401",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBK);

        var odrDetailAE = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901501",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailAE);

        var odrDetailMG = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901601",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailMG);

        var odrDetailOrther = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "620507802",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailOrther);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Fact]
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
        //MaxAge = "AA"
        var tenMstAA = new TenItemModel(
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
            "0",
            "AA",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAA);

        //MaxAge = "B3"
        var tenMstB3 = new TenItemModel(
            1,
            "629901101",
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
            "0",
            "B3",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB3);

        //MaxAge = "B6"
        var tenMstB6 = new TenItemModel(
            1,
            "629901201",
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
            "0",
            "B6",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB6);

        //MaxAge = "BF"
        var tenMstBF = new TenItemModel(
            1,
            "629901301",
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
            "0",
            "BF",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBF);

        //MaxAge = "BK"
        var tenMstBK = new TenItemModel(
            1,
            "629901401",
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
            "0",
            "BK",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBK);

        //MaxAge = "AE"
        var tenMstAE = new TenItemModel(
            1,
            "629901501",
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
            "0",
            "AE",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAE);

        //MaxAge = "MG"
        var tenMstMG = new TenItemModel(
            1,
            "629901601",
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
            "0",
            "MG",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstMG);

        //MaxAge = "MG"
        var tenMstOther = new TenItemModel(
            1,
            "620507802",
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
            "0",
            "29",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstOther);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetailAA = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140064650",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailAA);

        var odrDetailB3 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "629901101",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailB3);

        var odrDetailB6 = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901201",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailB6);

        var odrDetailBF = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901301",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBF);

        var odrDetailBK = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901401",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBK);

        var odrDetailAE = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901501",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailAE);

        var odrDetailMG = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901601",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailMG);

        var odrDetailOrther = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "620507802",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailOrther);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
    }

    [Fact]
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
        //MaxAge = "AA"
        var tenMstAA = new TenItemModel(
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
            "AA",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAA);

        //MaxAge = "B3"
        var tenMstB3 = new TenItemModel(
            1,
            "629901101",
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
            "B3",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB3);

        //MaxAge = "B6"
        var tenMstB6 = new TenItemModel(
            1,
            "629901201",
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
            "B6",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstB6);

        //MaxAge = "BF"
        var tenMstBF = new TenItemModel(
            1,
            "629901301",
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
            "BF",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBF);

        //MaxAge = "BK"
        var tenMstBK = new TenItemModel(
            1,
            "629901401",
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
            "BK",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstBK);

        //MaxAge = "AE"
        var tenMstAE = new TenItemModel(
            1,
            "629901501",
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
            "AE",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstAE);

        //MaxAge = "MG"
        var tenMstMG = new TenItemModel(
            1,
            "629901601",
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
            "MG",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstMG);

        //MaxAge = "MG"
        var tenMstOther = new TenItemModel(
            1,
            "620507802",
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
            "15",
            "0",
            "140064650",
            0,
            0
            );
        tenMstItems.Add(tenMstOther);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetailAA = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "140064650",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailAA);

        var odrDetailB3 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            1,
            20221111,
            20,
            "629901101",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetailB3);

        var odrDetailB6 = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901201",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailB6);

        var odrDetailBF = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901301",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBF);

        var odrDetailBK = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901401",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailBK);

        var odrDetailAE = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901501",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailAE);

        var odrDetailMG = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "629901601",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailMG);

        var odrDetailOrther = new OrdInfDetailModel(
           1,
           200219890,
           1,
           1,
           1,
           1,
           20221111,
           20,
           "620507802",
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
           string.Empty,
           string.Empty,
           string.Empty
           );
        odrDetails.Add(odrDetailOrther);
        #endregion  

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.AgeLimitCheck(sinDate, iBirthDay, checkAge, tenMstItems, odrDetails);

        // Assert
        Assert.True(output.Count == odrDetails.Count);
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
            string.Empty,
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
            string.Empty,
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
            string.Empty,
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
            string.Empty,
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
           string.Empty,
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
            "140064650",
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
            string.Empty,
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

    #region ItemCommentCheck
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

        var karteInf = new KarteInfModel(1, 901072057, 1, 1, 1, 20221111, "comment abc", 0, "abc", DateTime.MinValue, DateTime.MinValue, "abc");

        var interactor = new CheckedSpecialItemInteractor(mockTodayRepo.Object, mockMstItemRepo.Object, mockInsuranceRepo.Object, mockSystemConfigRepo.Object, mockReceptionRepo.Object);

        // Act
        var output = interactor.ItemCommentCheck(items, allCmtCheckMst, karteInf);

        // Assert
        Assert.True(output.Any());
    }
    #endregion

    #region CalculationCountCheck
    [Fact]
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
            "111000110",
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
            "0",
            "AA",
            "111000110",
            0,
            0
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "111013850",
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
            "0",
            "B3",
            "111013850",
            0,
            0
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "111000110",
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
               "111013850",
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
                "111000110",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "111013850",
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

    [Fact]
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
            "111000110",
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
            "0",
            "AA",
            "111000110",
            0,
            0
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "111013850",
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
            "0",
            "B3",
            "111013850",
            0,
            0
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "111000110",
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
               "111013850",
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
                "111000110",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "111013850",
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

    [Fact]
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
            "111000110",
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
            "0",
            "AA",
            "111000110",
            0,
            0
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "111013850",
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
            "0",
            "B3",
            "111013850",
            0,
            0
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "111000110",
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
               "111013850",
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
                "111000110",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "111013850",
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

    [Fact]
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
            "0",
            "AA",
            "1110001112",
            0,
            0
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "111013857",
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
            "0",
            "B3",
            "111013857",
            0,
            0
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "1110001112",
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013857",
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
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
            "111000110",
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
            "0",
            "AA",
            "111000110",
            0,
            0
            );
        tenMstItems.Add(tenMst1);

        var tenMst2 = new TenItemModel(
            1,
            "111013850",
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
            "0",
            "B3",
            "111013850",
            0,
            0
            );
        tenMstItems.Add(tenMst2);

        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail2);

        var santeiTenMsts = new List<TenItemModel>();
        santeiTenMsts.Add(tenMst1);
        santeiTenMsts.Add(tenMst2);

        var densiSanteiKaisuModels = new List<DensiSanteiKaisuModel>();

        var densiSanteiKaisuModel1 = new DensiSanteiKaisuModel(
                1,
                1,
                "111000110",
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
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel1);
        densiSanteiKaisuModels.Add(densiSanteiKaisuModel2);

        var itemGrpMsts = new List<ItemGrpMstModel>();

        var itemGrpMst1 = new ItemGrpMstModel(
                1,
                1,
                1,
                20220101,
                99999999,
                "111000110",
                1
            );

        var itemGrpMst2 = new ItemGrpMstModel(
                1,
                1,
                2,
                20220101,
                99999999,
                "111013850",
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
    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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
    [Fact]
    public void CommonDensiSantei_53()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_121()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_131()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail2);

        var densiSanteiKaisuModel2 = new DensiSanteiKaisuModel(
               1,
               1,
               "111013850",
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

    [Fact]
    public void CommonDensiSantei_138()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_141()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_142()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_143()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_144()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_145()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_146()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_147()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_148()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_997()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
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
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "11101385131",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_998()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "11101385131",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_999_TermSbt2()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_999_TermSbt3()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_999_TermSbt4()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_999_TermSbt5()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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

    [Fact]
    public void CommonDensiSantei_Other()
    {
        // Arrange
        int hpId = 1, sinDate = 20221110, sysyosinDate = 20191111;
        long ptId = 54109;
        var mockMstItemRepo = new Mock<IMstItemRepository>();
        var mockTodayRepo = new Mock<ITodayOdrRepository>();
        var mockInsuranceRepo = new Mock<IInsuranceRepository>();
        var mockSystemConfigRepo = new Mock<ISystemConfRepository>();
        var mockReceptionRepo = new Mock<IReceptionRepository>();

        #region Data Example
        var odrDetails = new List<OrdInfDetailModel>();
        var odrDetail1 = new OrdInfDetailModel(
            1,
            200219890,
            1,
            1,
            1,
            ptId,
            20221111,
            20,
            "111000110",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
            );
        odrDetails.Add(odrDetail1);

        var odrDetail2 = new OrdInfDetailModel(
            1,
            200219890,
            2,
            1,
            1,
            ptId,
            20221111,
            20,
            "111013850",
            "腎盂洗浄（両）",
            0,
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
            string.Empty,
            string.Empty,
            string.Empty
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
    [Fact]
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
        dt1 = dt1.AddDays((int)dt1.DayOfWeek * -1 + (-7 * (term - 1)));
        var retDate = int.Parse(dt1.ToString("yyyyMMdd"));

        var output = interactor.WeeksBefore(baseDate, term);

        // Assert
        Assert.True(output == retDate);
    }

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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
    #endregion
}
