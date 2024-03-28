using Domain.Models.Santei;
using Helper.Constants;

namespace CloudUnitTest.Model.Santei;

public class SanteiInfDetailModelTest
{
    [Test]
    public void SanteiInfDetailModel_TestContructor1Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int endDate = 20221212;
        int kisanSbt = 2;
        int kisanDate = 20121212;
        string byomei = "byomei";
        string hosokuComment = "hosokuComment";
        string comment = "comment";

        #endregion
        var santeiInfDetailModel = new SanteiInfDetailModel(
                                                        id,
                                                        ptId,
                                                        itemCd,
                                                        endDate,
                                                        kisanSbt,
                                                        kisanDate,
                                                        byomei,
                                                        hosokuComment,
                                                        comment
                                                );
        Assert.True(
                        santeiInfDetailModel.Id == id
                        && santeiInfDetailModel.PtId == ptId
                        && santeiInfDetailModel.ItemCd == itemCd
                        && santeiInfDetailModel.EndDate == endDate
                        && santeiInfDetailModel.KisanSbt == kisanSbt
                        && santeiInfDetailModel.KisanDate == kisanDate
                        && santeiInfDetailModel.Byomei == byomei
                        && santeiInfDetailModel.HosokuComment == hosokuComment
                        && santeiInfDetailModel.Comment == comment
                        && !santeiInfDetailModel.IsDeleted
                    );
    }

    [Test]
    public void SanteiInfDetailModel_TestContructor2Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int endDate = 20221212;
        int kisanSbt = 2;
        int kisanDate = 20121212;
        string byomei = "byomei";
        string hosokuComment = "hosokuComment";
        string comment = "comment";
        bool isDeleted = true;

        #endregion
        var santeiInfDetailModel = new SanteiInfDetailModel(
                                                        id,
                                                        ptId,
                                                        itemCd,
                                                        endDate,
                                                        kisanSbt,
                                                        kisanDate,
                                                        byomei,
                                                        hosokuComment,
                                                        comment,
                                                        isDeleted
                                                );
        Assert.True(
                        santeiInfDetailModel.Id == id
                        && santeiInfDetailModel.PtId == ptId
                        && santeiInfDetailModel.ItemCd == itemCd
                        && santeiInfDetailModel.EndDate == endDate
                        && santeiInfDetailModel.KisanSbt == kisanSbt
                        && santeiInfDetailModel.KisanDate == kisanDate
                        && santeiInfDetailModel.Byomei == byomei
                        && santeiInfDetailModel.HosokuComment == hosokuComment
                        && santeiInfDetailModel.Comment == comment
                        && santeiInfDetailModel.IsDeleted == isDeleted
                    );
    }

    [Test]
    public void SanteiInfDetailModel_TestContructor3Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        string itemCd = "itemCd";
        int startDate = 20221211;
        int endDate = 20221212;

        #endregion
        var santeiInfDetailModel = new SanteiInfDetailModel(
                                       id,
                                       itemCd,
                                       startDate,
                                       endDate);
        Assert.True(
                        santeiInfDetailModel.Id == id
                        && santeiInfDetailModel.ItemCd == itemCd
                        && santeiInfDetailModel.EndDate == endDate
                        && santeiInfDetailModel.StartDate == startDate
                    );
    }

    [Test]
    public void SanteiInfDetailModel_TestContructor4Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        string itemCd = "itemCd";
        int endDate = 20221212;
        int kisanSbt = 2;
        int kisanDate = 20121212;

        #endregion
        var santeiInfDetailModel = new SanteiInfDetailModel(id, itemCd, kisanSbt, kisanDate, endDate);
        Assert.True(
                        santeiInfDetailModel.Id == id
                        && santeiInfDetailModel.ItemCd == itemCd
                        && santeiInfDetailModel.EndDate == endDate
                        && santeiInfDetailModel.KisanSbt == kisanSbt
                        && santeiInfDetailModel.KisanDate == kisanDate
                    );
    }

    [Test]
    public void SanteiInfDetailModel_TestContructor5Success()
    {
        // Arrange
        #region Data Example
        int id = 1;
        long ptId = 1;
        string itemCd = "itemCd";
        int endDate = 20221212;
        int startDate = 20221210;
        int kisanSbt = 2;
        int kisanDate = 20121212;
        string byomei = "byomei";
        string hosokuComment = "hosokuComment";
        string comment = "comment";
        bool isDeleted = true;
        ModelStatus autoSanteiMstModelStatus = ModelStatus.Deleted;


        #endregion
        var santeiInfDetailModel = new SanteiInfDetailModel(id, ptId, itemCd, startDate, endDate, kisanSbt, kisanDate, byomei, hosokuComment, comment, isDeleted, autoSanteiMstModelStatus);
        Assert.True(
                        santeiInfDetailModel.Id == id
                        && santeiInfDetailModel.PtId == ptId
                        && santeiInfDetailModel.ItemCd == itemCd
                        && santeiInfDetailModel.EndDate == endDate
                        && santeiInfDetailModel.StartDate == startDate
                        && santeiInfDetailModel.KisanSbt == kisanSbt
                        && santeiInfDetailModel.KisanDate == kisanDate
                        && santeiInfDetailModel.Byomei == byomei
                        && santeiInfDetailModel.HosokuComment == hosokuComment
                        && santeiInfDetailModel.Comment == comment
                        && santeiInfDetailModel.IsDeleted == isDeleted
                        && santeiInfDetailModel.AutoSanteiMstModelStatus == autoSanteiMstModelStatus
                    );
    }

    [Test]
    public void SanteiInfDetailModel_TestSetKisanDate()
    {
        // Arrange
        #region Data Example
        int id = 1;
        string itemCd = "itemCd";
        int kisanSbt = 2;
        int kisanDate = 20121212;

        #endregion
        var santeiInfDetailModel = new SanteiInfDetailModel();
        santeiInfDetailModel = santeiInfDetailModel.SetKisanDate(id, itemCd, kisanSbt, kisanDate);
        Assert.True(
                        santeiInfDetailModel.Id == id
                        && santeiInfDetailModel.ItemCd == itemCd
                        && santeiInfDetailModel.KisanSbt == kisanSbt
                        && santeiInfDetailModel.KisanDate == kisanDate
                    );
    }

}
