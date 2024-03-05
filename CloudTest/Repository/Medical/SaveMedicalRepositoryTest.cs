using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.FlowSheet;
using Domain.Models.KarteInfs;
using Domain.Models.MonshinInf;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.SpecialNote;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.Medical;

public class SaveMedicalRepositoryTest : BaseUT
{
    [Test]
    public void SaveMedicalRepository_001_UpsertOdrInfs_Insert()
    {
        // Arrange
        int hpId = 1, userId = 2, sinDate = 20221111, syosaiKbn = 1, jikanKbn = 1, hokenPid = 1, santeiKbn = 1, tantoId = 1, kaId = 1;
        string uketukeTime = "0030030030", sinStartTime = "0030030030", sinEndTime = "0030030030";
        byte status = 1;
        long ptId = 1, raiinNo = 300011533;

        var ordInfModelList = new List<OrdInfModel>()
        {
            new OrdInfModel(
                hpId,
                raiinNo,
                1,
                1,
                1,
                20232303,
                1,
                1,
                "123",
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                0,
                127124,
                new List<OrdInfDetailModel>()
                {
                    new OrdInfDetailModel(
                        hpId,
                        sinDate,
                        raiinNo,
                        1,
                        1,
                        1,
                        "itemcd",
                        1,
                        "itemname"
                        )
                },
                DateTime.MinValue,
                1,
                "123",
                DateTime.MinValue,
                1,
                "123",
                "",
                ""
                )
        };

        var karteInfModel = new KarteInfModel(
            1,
            901960104
            );

        var familyModel = new List<FamilyModel>()
        {
            new FamilyModel(
                1,
                883,
                "0030030030",
                99999002
                )
        };

        var NextOrderModelList = new List<NextOrderModel>()
        {
            new NextOrderModel(
                1,
                883,
                1,
                1,
                1,
                "0030030030",
                0,
                1,
                new(),
                new(),
                new(),
                new()
                )
        };

        var summaryInfModel = new SummaryInfModel(
            );

        var importantNoteModel = new ImportantNoteModel();

        var patientInfoModel = new PatientInfoModel();

        var ptDiseaseModel = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel()
        };

        var flowSheetModel = new List<FlowSheetModel>()
        {
            new FlowSheetModel(
                20221111,
                883,
                901960104,
                "0030030030",
                1,
                1,
                true
                )
        };

        var monshinInforModel = new MonshinInforModel();

        var mockConfiguration = new Mock<IConfiguration>();
        var userInfoService = new Mock<IUserInfoService>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);

        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, userInfoService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);
        NextOrderRepository nextOrderRepository = new NextOrderRepository(TenantProvider);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        FlowSheetRepository flowSheetRepository = new FlowSheetRepository(TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, mockConfiguration.Object);
        MonshinInforRepository monshinInforRepository = new MonshinInforRepository(TenantProvider);
        SaveMedicalRepository saveMedicalRepository = new SaveMedicalRepository(TenantProvider, familyRepository, todayOdrRepository, nextOrderRepository, specialNoteRepository, diseaseRepository, flowSheetRepository, monshinInforRepository);
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        todayOdrRepository.UpsertOdrInfs(hpId, ptId, raiinNo, sinDate, ordInfModelList, userId, status);
        var checkUpsertOdrInfs = tenantNoTracking.OdrInfs.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo && x.RpEdaNo == ordInfModelList.First().RpEdaNo && x.PtId == ptId).ToList();
        var checkUpsertOdrInfDetails = tenantNoTracking.OdrInfDetails.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo && x.RpEdaNo == ordInfModelList.First().OrdInfDetails.First().RpEdaNo && x.PtId == ptId).ToList();

        try
        {
            //Act
            Assert.True(checkUpsertOdrInfs.Count != 0);
            Assert.True(checkUpsertOdrInfDetails.Count != 0);
        }
        finally
        {
            tenantTracking.OdrInfs.RemoveRange(checkUpsertOdrInfs);
            tenantTracking.OdrInfDetails.RemoveRange(checkUpsertOdrInfDetails);
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void SaveMedicalRepository_001_UpsertOdrInfs_Delete()
    {
        int hpId = 1, userId = 2, sinDate = 20221111;
        byte status = 1;
        long ptId = 1, raiinNo = 300011533;
        var ordInfModelInsertList = new List<OrdInfModel>()
        {
            new OrdInfModel(
                hpId,
                raiinNo,
                1,
                1,
                1,
                20232303,
                1,
                1,
                "123",
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                0,
                127124,
                new(),
                DateTime.MinValue,
                1,
                "123",
                DateTime.MinValue,
                1,
                "123",
                "",
                ""
                )
        };

        var karteInfModel = new KarteInfModel(
            1,
            901960104
            );

        var familyModel = new List<FamilyModel>()
        {
            new FamilyModel(
                1,
                883,
                "0030030030",
                99999002
                )
        };

        var NextOrderModelList = new List<NextOrderModel>()
        {
            new NextOrderModel(
                1,
                883,
                1,
                1,
                1,
                "0030030030",
                0,
                1,
                new(),
                new(),
                new(),
                new()
                )
        };

        var summaryInfModel = new SummaryInfModel(
            );

        var importantNoteModel = new ImportantNoteModel();

        var patientInfoModel = new PatientInfoModel();

        var ptDiseaseModel = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel()
        };

        var flowSheetModel = new List<FlowSheetModel>()
        {
            new FlowSheetModel(
                20221111,
                883,
                901960104,
                "0030030030",
                1,
                1,
                true
                )
        };

        var monshinInforModel = new MonshinInforModel();

        var mockConfiguration = new Mock<IConfiguration>();
        var userInfoService = new Mock<IUserInfoService>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);

        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, userInfoService.Object);
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);
        NextOrderRepository nextOrderRepository = new NextOrderRepository(TenantProvider);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        FlowSheetRepository flowSheetRepository = new FlowSheetRepository(TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, mockConfiguration.Object);
        MonshinInforRepository monshinInforRepository = new MonshinInforRepository(TenantProvider);
        SaveMedicalRepository saveMedicalRepository = new SaveMedicalRepository(TenantProvider, familyRepository, todayOdrRepository, nextOrderRepository, specialNoteRepository, diseaseRepository, flowSheetRepository, monshinInforRepository);
        var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        todayOdrRepository.UpsertOdrInfs(hpId, ptId, raiinNo, sinDate, ordInfModelInsertList, userId, status);
        var checkUpsertOdrInfs = tenantNoTracking.OdrInfs.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo && x.RpEdaNo == ordInfModelInsertList.First().RpEdaNo && x.PtId == ptId).ToList();
        List<OrdInfModel> ordInfModelDeleteList = new();
        foreach (var item in checkUpsertOdrInfs)
        {
            var ordInfModelList = new List<OrdInfModel>()
                {
                    new OrdInfModel(
                        hpId,
                        raiinNo,
                        item.RpNo,
                        1,
                        1,
                        20232303,
                        1,
                        1,
                        "123",
                        1,
                        1,
                        1,
                        1,
                        1,
                        1,
                        1,
                        1,
                        item.Id,
                        new(),
                        DateTime.MinValue,
                        1,
                        "123",
                        DateTime.MinValue,
                        1,
                        "123",
                        "",
                        ""
                        )
                };
            ordInfModelDeleteList.AddRange(ordInfModelList);
        }
        todayOdrRepository.UpsertOdrInfs(hpId, ptId, raiinNo, sinDate, ordInfModelDeleteList, userId, status);
        var checkDeleteOdrInfs = tenantNoTracking.OdrInfs.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo && x.RpEdaNo == ordInfModelDeleteList.First().RpEdaNo && x.RpNo == ordInfModelDeleteList.First().RpNo && x.PtId == ptId && x.IsDeleted == 1).ToList();

        try
        {
            //Act
            Assert.True(checkDeleteOdrInfs.Count != 0);
        }
        finally
        {
            tenantTracking.OdrInfs.RemoveRange(checkDeleteOdrInfs);
            tenantTracking.SaveChanges();
        }
    }
}
