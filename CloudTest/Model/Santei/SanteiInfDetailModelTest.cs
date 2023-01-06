using Domain.Models.Santei;

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
}
