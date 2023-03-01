using Domain.Models.Santei;
using Moq;

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
                        && santeiInfModel.ListSanteiInfDetails == listSanteiInfDetails
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
                        && !santeiInfModel.ListSanteiInfDetails.Any()
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
                        && santeiInfModel.ListSanteiInfDetails == listSanteiInfDetails
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
                        && !santeiInfModel.ListSanteiInfDetails.Any()
                        && !santeiInfModel.IsDeleted
                    );
    }
}
