using CloudUnitTest.SampleData;
using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.CheckedDisease;

public class CheckedDiseaseTest : BaseUT
{
    /// <summary>
    /// Check IgakuItem if it exist then return null
    /// </summary>
    [Test]
    public void CheckedDisease_001_Special()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111;
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
                "@BUNKATU",
                10
            ),
             new OrdInfDetailModel(
                "@REFILL",
                10
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),
        };
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository);
        // Act
        var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, byomeiModelList, ordInfs);
        // Assert
        Assert.True(iagkutokusitu.Count == 0);
    }

    /// <summary>
    /// Check IgakuItem if it exist then return null
    /// </summary>
    [Test]
    public void CheckedDisease_002_NoByomeis()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                1,
                10,
                1,
                1,
                1,
                1,
                "0020020"
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "0020020020",
                10
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),
        };
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository);
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CheckedDiseaseData.ReadTenMst("0020020020");
        //var byomeiMsts = CheckedDiseaseData.ReadByomeiMst("0020020");
        tenantTracking.TenMsts.AddRange(tenMsts);
        //tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        // Act
        var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, byomeiModelList, ordInfs);
        // Assert
        Assert.True(iagkutokusitu.Count == 0);
    }
}