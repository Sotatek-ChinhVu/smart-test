using CloudUnitTest.SampleData;
using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Microsoft.Extensions.Configuration;
using Infrastructure.Repositories;
using Moq;
using Infrastructure.Interfaces;

namespace CloudUnitTest.Repository.CheckedDisease;

public class CheckedDiseaseTest : BaseUT
{
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
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);

        var mockUserService = new Mock<IUserInfoService>();
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);

        // Act
        var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, byomeiModelList, ordInfs);

        // Assert
        Assert.True(iagkutokusitu.Count == 0);
    }

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
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        var mockUserService = new Mock<IUserInfoService>();
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CheckedDiseaseData.ReadTenMst("0020020020");
        var byomeiMsts = CheckedDiseaseData.ReadByomeiMst("0020020");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, byomeiModelList, ordInfs);

            // Assert
            Assert.True(iagkutokusitu.Count == 0);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void CheckedDisease_003_True()
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
                "0030030"
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "0030030030",
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
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        var mockUserService = new Mock<IUserInfoService>();
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CheckedDiseaseData.ReadTenMst("0030030030");
        var byomeiMsts = CheckedDiseaseData.ReadByomeiMst("0030030");
        var tekiouByomeiMsts = CheckedDiseaseData.ReadTekiouByomeiMst("0030030030", "0030030");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        tenantTracking.TekiouByomeiMsts.AddRange(tekiouByomeiMsts);
        try
        {
            tenantTracking.SaveChanges();
            // Act
            var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, byomeiModelList, ordInfs);
            // Assert
            Assert.True(iagkutokusitu.Count == 1);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.TekiouByomeiMsts.RemoveRange(tekiouByomeiMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void CheckedDisease_004_ExistItemCd()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                1,
                10,
                1,
                0,
                1,
                1,
                "0030030",
                "ByomeiNameの疑い"
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "0030030030",
                10
            ),
            new OrdInfDetailModel(
                "0030030030",
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
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        var mockUserService = new Mock<IUserInfoService>();
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CheckedDiseaseData.ReadTenMst("0030030030");
        var byomeiMsts = CheckedDiseaseData.ReadByomeiMst("0030030");
        var tekiouByomeiMsts = CheckedDiseaseData.ReadTekiouByomeiMst("0030030030", "0030030");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        tenantTracking.TekiouByomeiMsts.AddRange(tekiouByomeiMsts);
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, byomeiModelList, ordInfs);
            // Assert
            Assert.True(iagkutokusitu.Count == 1);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.TekiouByomeiMsts.RemoveRange(tekiouByomeiMsts);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void CheckedDisease_005_ExistByomeiCd()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                1,
                10,
                1,
                0,
                1,
                1,
                "0030030",
                "ByomeiNameの疑い"
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "0030030030",
                10
            ),
            new OrdInfDetailModel(
                "0030030030",
                10
            ),
            new OrdInfDetailModel(
                "0030030030",
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                10,
                ordInfDetailModels
                ),
        };
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        var mockUserService = new Mock<IUserInfoService>();
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var tenMsts = CheckedDiseaseData.ReadTenMst("0030030030");
        var byomeiMsts = CheckedDiseaseData.ReadByomeiMst("0030030");
        var tekiouByomeiMsts = CheckedDiseaseData.ReadTekiouByomeiMst("0030030030", "0030030");
        tenantTracking.TenMsts.AddRange(tenMsts);
        tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        tenantTracking.TekiouByomeiMsts.AddRange(tekiouByomeiMsts);
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var iagkutokusitu = todayOdrRepository.GetCheckDiseases(hpId, sinDate, byomeiModelList, ordInfs);
            // Assert
            Assert.True(iagkutokusitu.Count == 1);
        }
        finally
        {
            tenantTracking.TenMsts.RemoveRange(tenMsts);
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.TekiouByomeiMsts.RemoveRange(tekiouByomeiMsts);
            tenantTracking.SaveChanges();
        }
    }
}