﻿using CloudUnitTest.SampleData;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.CheckedSpecialItem;

public class CheckedSpecialItemTest : BaseUT
{
    /// <summary>
    /// Check get TenMstItem list
    /// </summary>
    [Test]
    public void GetTenMstItem()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sampleData = ReadSampleData.ReadTenMst();
        tenant.TenMsts.AddRange(sampleData);
        tenant.SaveChanges();
        MstItemRepository mstItemRepository = new MstItemRepository(TenantProvider);
        // Act
        var tenMsts = mstItemRepository.FindTenMst(1, new List<string>{
            "6412100651",
            "6412100672",
            "6412100783"
            }, 20201212, 20221212);
        // Assert
        Assert.True(tenMsts.Count == 3);

        tenant.TenMsts.RemoveRange(sampleData);
        tenant.SaveChanges();
    }

    [Test]
    public void FindDensiSanteiKaisuList()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sampleData = ReadSampleData.ReadDensiSanteiKaisu();
        tenant.DensiSanteiKaisus.AddRange(sampleData);
        tenant.SaveChanges();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);
        // Act
        var densiSanteis = todayOdrRepository.FindDensiSanteiKaisuList(1, new List<string>{
            "W12334"
            }, 20220101, 20221212);
        // Assert
        Assert.True(densiSanteis.Count == 1);
        tenant.DensiSanteiKaisus.RemoveRange(sampleData);
        tenant.SaveChanges();
    }

    [Test]
    public void GetSettingValue()
    {
        // Arrange
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider);
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
    public void GetFirstVisitWithSyosin()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sampleData = ReadSampleData.ReadRainInf();
        tenant.RaiinInfs.AddRange(sampleData);
        tenant.SaveChanges();
        // Arrange
        ReceptionRepository receptionRepository = new ReceptionRepository(TenantProvider);
        // Act
        var value = receptionRepository.GetFirstVisitWithSyosin(1, 56025, 20140331);
        // Assert
        Assert.True(value > 0);
        tenant.RaiinInfs.RemoveRange(sampleData);
        tenant.SaveChanges();
    }

    [Test]
    public void GetPtHokenInf()
    {
        // Arrange
        InsuranceRepository insuranceRepository = new InsuranceRepository(TenantProvider);
        // Act
        var hokenInf = insuranceRepository.GetPtHokenInf(1, 10, 56025, 20140325);
        // Assert
        Assert.True(hokenInf.HpId != 0 && hokenInf.PtId != 0 && hokenInf.HokenPid != 0);
    }

    [Test]
    public void SanteiCount()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sinKouiCounts = ReadSampleData.ReadSinKouiCount();
        var sinRpInfs = ReadSampleData.ReadSinRpInf();
        var sinKouiDetails = ReadSampleData.ReadSinKouiDetail();
        tenant.SinRpInfs.AddRange(sinRpInfs);
        tenant.SinKouiCounts.AddRange(sinKouiCounts);
        tenant.SinKouiDetails.AddRange(sinKouiDetails);
        tenant.SaveChanges();
        TodayOdrRepository todayRepository = new TodayOdrRepository(TenantProvider);
        // Act
        var santeiCount = todayRepository.SanteiCount(1, 54522111111, 20220101, 20221212, 20220401, 500000004, new List<string>() { "112009210" }, new List<int> {1}, new List<int> {10});
        // Assert
        Assert.True(santeiCount == 1);

        tenant.SinRpInfs.RemoveRange(sinRpInfs);
        tenant.SinKouiCounts.RemoveRange(sinKouiCounts);
        tenant.SinKouiDetails.RemoveRange(sinKouiDetails);
        tenant.SaveChanges();
    }
}