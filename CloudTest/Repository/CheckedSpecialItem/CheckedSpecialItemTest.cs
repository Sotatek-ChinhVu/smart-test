using CloudUnitTest.SampleData;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace CloudUnitTest.Repository.CheckedSpecialItem;

public class CheckedSpecialItemTest : BaseUT
{
    /// <summary>
    /// Check get TenMstItem list
    /// </summary>
    [Test]
    public void TC_001_GetTenMstItem()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sampleData = CheckedSpecialItemData.ReadTenMst();
        tenant.TenMsts.AddRange(sampleData);
        tenant.SaveChanges();
        var mockOptions = new Mock<IOptions<AmazonS3Options>>();
        MstItemRepository mstItemRepository = new MstItemRepository(TenantProvider, mockOptions.Object);
        try
        {
            // Act
            var tenMsts = mstItemRepository.FindTenMst(1, new List<string>{
            "6412100651",
            "6412100672",
            "6412100783"
            }, 20201212, 20221212);

            // Assert
            Assert.True(tenMsts.Count == 3);
        }
        finally
        {
            tenant.TenMsts.RemoveRange(sampleData);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_002_FindDensiSanteiKaisuList()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sampleData = CheckedSpecialItemData.ReadDensiSanteiKaisu();
        tenant.DensiSanteiKaisus.AddRange(sampleData);
        tenant.SaveChanges();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var mockUserService = new Mock<IUserInfoService>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);

        try
        {
            // Act
            var densiSanteis = todayOdrRepository.FindDensiSanteiKaisuList(1, new List<string>{
            "W12334"
            }, 20220101, 20221212);
            // Assert
            Assert.True(densiSanteis.Count == 1);
        }
        finally
        {
            tenant.DensiSanteiKaisus.RemoveRange(sampleData);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_003_GetSettingValue()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        var tenanant = TenantProvider.GetTrackingTenantDataContext();
        var system = tenanant.SystemConfs.FirstOrDefault(s => s.HpId == 1 && s.GrpCd == 3013 && s.GrpEdaNo == 0);
        if (system != null)
        {
            var tempVal = system.Val;
            system.Val = 1;
            tenanant.SaveChanges();
            // Act
            var systemVal = systemConfRepository.GetSettingValue(3013, 0, 1);
            // Assert
            Assert.True(systemVal != 0);
            system.Val = tempVal;
            tenanant.SaveChanges();
        }
    }

    [Test]
    public void TC_004_GetFirstVisitWithSyosin()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sampleData = CheckedSpecialItemData.ReadRainInf();
        tenant.RaiinInfs.AddRange(sampleData);
        tenant.SaveChanges();

        // Arrange
        ReceptionRepository receptionRepository = new ReceptionRepository(TenantProvider);

        try
        {
            // Act
            var value = receptionRepository.GetFirstVisitWithSyosin(1, 602, 20220915);
            // Assert
            Assert.True(value > 0);
        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(sampleData);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_005_GetPtHokenInf()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var hokenMsts = CheckedSpecialItemData.ReadHokenMst();
        var ptHokenInfs = CheckedSpecialItemData.ReadPtHokenInf();
        var ptKohis = CheckedSpecialItemData.ReadPtKoHi();
        var ptHokenChecks = CheckedSpecialItemData.ReadPtHokenCheck();
        var ptInfs = CheckedSpecialItemData.ReadPtInf();
        var userMsts = CheckedSpecialItemData.ReadUserMst();
        var hokenSyaMsts = CheckedSpecialItemData.ReadHokenSyaMst();
        var roudous = CheckedSpecialItemData.ReadRoudouMst();
        var ptRouSaiTenkis = CheckedSpecialItemData.ReadPtRouSaiTenKi();
        var ptHokenPatterns = CheckedSpecialItemData.ReadPtHokenPattern();
        tenant.HokenMsts.AddRange(hokenMsts);
        tenant.PtHokenInfs.AddRange(ptHokenInfs);
        tenant.PtKohis.AddRange(ptKohis);
        tenant.PtHokenChecks.AddRange(ptHokenChecks);
        tenant.PtInfs.AddRange(ptInfs);
        tenant.UserMsts.AddRange(userMsts);
        tenant.HokensyaMsts.AddRange(hokenSyaMsts);
        tenant.RoudouMsts.AddRange(roudous);
        tenant.PtRousaiTenkis.AddRange(ptRouSaiTenkis);
        tenant.PtHokenPatterns.AddRange(ptHokenPatterns);
        try
        {
            tenant.SaveChanges();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            InsuranceRepository insuranceRepository = new InsuranceRepository(TenantProvider, mockConfiguration.Object);

            // Act
            var hokenInf = insuranceRepository.GetPtHokenInf(1, 99999, 999999, 20220325);
            // Assert
            Assert.True(hokenInf.HpId != 0 && hokenInf.PtId != 0 && hokenInf.HokenPid != 0 && hokenInf.Kohi1.HokenId != 0 && hokenInf.Kohi3.HokenId != 0 && hokenInf.Kohi2.HokenId != 0 && hokenInf.Kohi4.HokenId != 0);
        }
        finally
        {
            tenant.HokenMsts.RemoveRange(hokenMsts);
            tenant.PtHokenInfs.RemoveRange(ptHokenInfs);
            tenant.PtKohis.RemoveRange(ptKohis);
            tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
            tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
            tenant.PtInfs.RemoveRange(ptInfs);
            tenant.UserMsts.RemoveRange(userMsts);
            tenant.HokensyaMsts.RemoveRange(hokenSyaMsts);
            tenant.RoudouMsts.RemoveRange(roudous);
            tenant.PtRousaiTenkis.RemoveRange(ptRouSaiTenkis);
            tenant.PtHokenPatterns.RemoveRange(ptHokenPatterns);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_006_SanteiCount()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sinKouiCounts = CheckedSpecialItemData.ReadSinKouiCount();
        var sinRpInfs = CheckedSpecialItemData.ReadSinRpInf();
        var sinKouiDetails = CheckedSpecialItemData.ReadSinKouiDetail();
        tenant.SinRpInfs.AddRange(sinRpInfs);
        tenant.SinKouiCounts.AddRange(sinKouiCounts);
        tenant.SinKouiDetails.AddRange(sinKouiDetails);
        tenant.SaveChanges();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConf = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        var mockUserService = new Mock<IUserInfoService>();
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository); TodayOdrRepository todayRepository = new TodayOdrRepository(TenantProvider, systemConf, approvalinfRepository);
        try
        {
            // Act
            var santeiCount = todayRepository.SanteiCount(1, 54522111111, 20220101, 20221212, 20220401, 500000004, new List<string>() { "112009210" }, new List<int> { 1 }, new List<int> { 10 });
            var santeiCount2 = todayRepository.SanteiCount(1, 54522111111, 20220101, 20221212, 20220401, 0, new List<string>() { "112009210" }, new List<int> { 1 }, new List<int> { 10 });
            // Assert
            Assert.True(santeiCount == 1 && santeiCount2 == 0);
        }
        finally
        {
            tenant.SinRpInfs.RemoveRange(sinRpInfs);
            tenant.SinKouiCounts.RemoveRange(sinKouiCounts);
            tenant.SinKouiDetails.RemoveRange(sinKouiDetails);
            tenant.SaveChanges();
        }
    }
}