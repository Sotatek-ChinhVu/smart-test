using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.CheckedDisease;

public class CheckedDiseaseTest : BaseUT
{
    /// <summary>
    /// Check IgakuItem if it exist then return null
    /// </summary>
    [Test]
    public void IgakuTokusitu_Exist_IgakuItem()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 1;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                1,
                10,
                1,
                1,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034010",
                10
            ),
             new OrdInfDetailModel(
                "113001810",
                10
            )
        };
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository);
        // Act
        var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);
        // Assert
        Assert.True(iagkutokusitu.Count == 0);
    }
}