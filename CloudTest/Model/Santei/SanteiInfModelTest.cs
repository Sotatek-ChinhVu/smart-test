using Domain.Models.Santei;

namespace CloudUnitTest.Model.Santei;

public class SanteiInfModelTest
{
    [Test]
    public void SanteiInfModel_TestContructor1Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 3;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    2,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                                    id,
                                                    ptId,
                                                    itemCd,
                                                    seqNo,
                                                    alertDays,
                                                    alertTerm,
                                                    itemName,
                                                    lastOdrDate,
                                                    santeiItemCount,
                                                    santeiItemSum,
                                                    currentMonthSanteiItemCount,
                                                    currentMonthSanteiItemSum,
                                                    sinDate,
                                                    listSanteiInfDetails
                                                );
        Assert.True(
                        santeiInfModel.Id == id
                        && santeiInfModel.PtId == ptId
                        && santeiInfModel.ItemCd == itemCd
                        && santeiInfModel.SeqNo == seqNo
                        && santeiInfModel.AlertDays == alertDays
                        && santeiInfModel.AlertTerm == alertTerm
                        && santeiInfModel.ItemName == itemName
                        && santeiInfModel.LastOdrDate == lastOdrDate
                        && santeiInfModel.SanteiItemCount == santeiItemCount
                        && santeiInfModel.SanteiItemSum == santeiItemSum
                        && santeiInfModel.CurrentMonthSanteiItemCount == currentMonthSanteiItemCount
                        && santeiInfModel.CurrentMonthSanteiItemSum == currentMonthSanteiItemSum
                        && santeiInfModel.SanteiInfDetailList == listSanteiInfDetails
                        && !santeiInfModel.IsDeleted
                    );
    }

    [Test]
    public void SanteiInfModel_TestContructor2Success()
    {
        // Arrange
        #region Data Example
        string itemCd = "itemCd";
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;


        #endregion
        var santeiInfModel = new SanteiInfModel(
                                                    itemCd,
                                                    santeiItemCount,
                                                    santeiItemSum,
                                                    currentMonthSanteiItemCount,
                                                    currentMonthSanteiItemSum
                                                );
        Assert.True(
                        santeiInfModel.Id == 0
                        && santeiInfModel.PtId == 0
                        && santeiInfModel.ItemCd == itemCd
                        && santeiInfModel.SeqNo == 0
                        && santeiInfModel.AlertDays == 0
                        && santeiInfModel.AlertTerm == 0
                        && santeiInfModel.ItemName == string.Empty
                        && santeiInfModel.LastOdrDate == 0
                        && santeiInfModel.SanteiItemCount == santeiItemCount
                        && santeiInfModel.SanteiItemSum == santeiItemSum
                        && santeiInfModel.CurrentMonthSanteiItemCount == currentMonthSanteiItemCount
                        && santeiInfModel.CurrentMonthSanteiItemSum == currentMonthSanteiItemSum
                        && !santeiInfModel.SanteiInfDetailList.Any()
                        && !santeiInfModel.IsDeleted
                    );
    }

    [Test]
    public void SanteiInfModel_TestContructor3Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int alertDays = 1;
        int alertTerm = 3;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    2,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };
        bool isDelete = true;

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                                    id,
                                                    ptId,
                                                    itemCd,
                                                    alertDays,
                                                    alertTerm,
                                                    0,
                                                    listSanteiInfDetails,
                                                    isDelete
                                                );
        Assert.True(
                        santeiInfModel.Id == id
                        && santeiInfModel.PtId == ptId
                        && santeiInfModel.ItemCd == itemCd
                        && santeiInfModel.SeqNo == 0
                        && santeiInfModel.AlertDays == alertDays
                        && santeiInfModel.AlertTerm == alertTerm
                        && santeiInfModel.ItemName == string.Empty
                        && santeiInfModel.LastOdrDate == 0
                        && santeiInfModel.SanteiItemCount == 0
                        && santeiInfModel.SanteiItemSum == 0
                        && santeiInfModel.CurrentMonthSanteiItemCount == 0
                        && santeiInfModel.CurrentMonthSanteiItemSum == 0
                        && santeiInfModel.SanteiInfDetailList == listSanteiInfDetails
                        && santeiInfModel.IsDeleted == isDelete
                    );
    }

    [Test]
    public void SanteiInfModel_TestContructor4Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int alertDays = 1;
        int alertTerm = 3;


        #endregion
        var santeiInfModel = new SanteiInfModel(
                                                    id,
                                                    ptId,
                                                    itemCd,
                                                    alertDays,
                                                    alertTerm
                                                );
        Assert.True(
                        santeiInfModel.Id == id
                        && santeiInfModel.PtId == ptId
                        && santeiInfModel.ItemCd == itemCd
                        && santeiInfModel.SeqNo == 0
                        && santeiInfModel.AlertDays == alertDays
                        && santeiInfModel.AlertTerm == alertTerm
                        && santeiInfModel.ItemName == string.Empty
                        && santeiInfModel.LastOdrDate == 0
                        && santeiInfModel.SanteiItemCount == 0
                        && santeiInfModel.SanteiItemSum == 0
                        && santeiInfModel.CurrentMonthSanteiItemCount == 0
                        && santeiInfModel.CurrentMonthSanteiItemSum == 0
                        && !santeiInfModel.SanteiInfDetailList.Any()
                        && !santeiInfModel.IsDeleted
                    );
    }

    [Test]
    public void SanteiInfModel_TestDayCountDisplay_1()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 2;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 new());
        Assert.True(santeiInfModel.DayCountDisplay == $"{santeiInfModel.DayCount}日");
    }

    [Test]
    public void SanteiInfModel_TestDayCountDisplay_2()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 3;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 new());
        Assert.True(santeiInfModel.DayCountDisplay == $"{santeiInfModel.DayCount}週");
    }

    [Test]
    public void SanteiInfModel_TestDayCountDisplay_3()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 4;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 new());
        Assert.True(santeiInfModel.DayCountDisplay == $"{santeiInfModel.DayCount}ヶ月");
    }

    [Test]
    public void SanteiInfModel_TestDayCountDisplay_4()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 5;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 new());
        Assert.True(santeiInfModel.DayCountDisplay == $"{santeiInfModel.DayCount}週");
    }

    [Test]
    public void SanteiInfModel_TestDayCountDisplay_5()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 new());
        Assert.True(santeiInfModel.DayCountDisplay == $"{santeiInfModel.DayCount}ヶ月");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_1()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 new());
        Assert.True(santeiInfModel.KisanType == "前回日");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_2()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        int kisanSbt = 1;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    kisanSbt,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 listSanteiInfDetails);
        Assert.True(santeiInfModel.KisanType == "前回日");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_3()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        int kisanSbt = 2;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    kisanSbt,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 listSanteiInfDetails);
        Assert.True(santeiInfModel.KisanType == "発症日");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_4()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        int kisanSbt = 3;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    kisanSbt,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 listSanteiInfDetails);
        Assert.True(santeiInfModel.KisanType == "急性増悪");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_5()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        int kisanSbt = 4;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    kisanSbt,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 listSanteiInfDetails);
        Assert.True(santeiInfModel.KisanType == "治療開始");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_6()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        int kisanSbt = 5;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    kisanSbt,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 listSanteiInfDetails);
        Assert.True(santeiInfModel.KisanType == "手術日");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_7()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        int kisanSbt = 6;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    kisanSbt,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 listSanteiInfDetails);
        Assert.True(santeiInfModel.KisanType == "初回診断");
    }

    [Test]
    public void SanteiInfModel_TestKisanType_8()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int seqNo = 1;
        int alertDays = 1;
        int alertTerm = 6;
        string itemName = "itemName";
        int lastOdrDate = 20221212;
        int santeiItemCount = 1;
        double santeiItemSum = 1.1;
        int currentMonthSanteiItemCount = 1;
        double currentMonthSanteiItemSum = 1.1;
        int sinDate = 20220101;
        int kisanSbt = 7;
        var listSanteiInfDetails = new List<SanteiInfDetailModel>()
        {
            new SanteiInfDetailModel(
                    1,
                    ptId,
                    itemCd,
                    20221212,
                    kisanSbt,
                    20111212,
                    "byomei",
                    "hosokuComment",
                    "comment"
                )
        };

        #endregion
        var santeiInfModel = new SanteiInfModel(
                                 id,
                                 ptId,
                                 itemCd,
                                 seqNo,
                                 alertDays,
                                 alertTerm,
                                 itemName,
                                 lastOdrDate,
                                 santeiItemCount,
                                 santeiItemSum,
                                 currentMonthSanteiItemCount,
                                 currentMonthSanteiItemSum,
                                 sinDate,
                                 listSanteiInfDetails);
        Assert.True(santeiInfModel.KisanType == "前回日");
    }
}
