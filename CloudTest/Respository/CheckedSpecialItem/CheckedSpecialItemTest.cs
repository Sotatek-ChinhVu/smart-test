using CloudUnitTest.SampleData;
using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.Reception;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Infrastructure.CommonDB;
using Infrastructure.Repositories;
using Interactor.MedicalExamination;
using Moq;

namespace CloudUnitTest.Interactor.MedicalExamination;

public class CheckedSpecialItemTest : BaseUT
{

    /// <summary>
    /// Check get TenMstItem list
    /// </summary>
    [Test]
    public void GetTenMstItem()
    {
        // Arrange
        MstItemRepository mstItemRepository = new MstItemRepository(TenantProvider);
        // Act
        var tenMsts = mstItemRepository.FindTenMst(1, new List<string>{
            "641210065",
            "641210067",
            "641210078"
            }, 20000101, 20001212);
        // Assert
        Assert.True(tenMsts.Count == 3);
    }
    
    [Test]
    public void FindDensiSanteiKaisuList()
    {
        // Arrange
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);
        // Act
        var tenMsts = todayOdrRepository.FindDensiSanteiKaisuList(1, new List<string>{
            "160233850",
            "113000670",
            "113000770"
            }, 20220101, 20221212);
        // Assert
        Assert.True(tenMsts.Count == 3);
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
}