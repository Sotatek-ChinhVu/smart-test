using CloudUnitTest.SampleData;
using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.CheckedSpecialItem;

public class CheckedOrderTest : BaseUT
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

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);
        // Assert
        Assert.True(iagkutokusitu.Count == 0);
    }

    /// <summary>
    /// Check IsJouHou if it is true then check sinDate and tenMst, if it is false then only check tenMst
    /// </summary>
    [Test]
    public void IgakuTokusitu_IsJouhou()
    {
        // Arrange
        int hpId = 1, sinDate1 = 20221111, sinDate2 = 20210101, hokenId = 10, syosaisinKbn = 1;
        bool isJouhou1 = true, isJouhou2 = false;
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
            ), new OrdInfDetailModel(
                "113001810",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate1, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou2);
        // Assert
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0);
    }

    /// <summary>
    /// Check Syosai
    /// </summary>
    [Test]
    public void IgakuTokusitu_Syosai()
    {
        // Arrange
        int hpId = 1, sinDate1 = 20221111, sinDate2 = 20210101, hokenId = 10, syosaisinKbn1 = 1, syosaisinKbn2 = 6, syosaisinKbn3 = 2, syosaisinKbn4 = 4, syosaisinKbn5 = 8;
        bool isJouhou1 = true, isJouhou2 = false;
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
                "11303401012",
                10
            ), new OrdInfDetailModel(
                "11300181013",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate1, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn2, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn3, byomeiModelList, ordInfDetailModels, isJouhou2);
        var iagkutokusitu4 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn4, byomeiModelList, ordInfDetailModels, isJouhou2);
        var iagkutokusitu5 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn5, byomeiModelList, ordInfDetailModels, isJouhou2);
        // Assert
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0 && iagkutokusitu4.Count == 0 && iagkutokusitu5.Count == 0);
    }

    /// <summary>
    /// Check Special
    /// </summary>
    [Test]
    public void IgakuTokusitu_Special()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 15;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                5,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113001810131",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);
        Assert.True(iagkutokusitu1.Count > 0);
    }

    /// <summary>
    /// Check Orther
    /// </summary>
    [Test]
    public void IgakuTokusitu_Other()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 15;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                8,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113001810131",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);
        Assert.True(iagkutokusitu1.Count > 0);
    }

    /// <summary>
    /// Check Item Sihifu of Order Detail
    /// </summary>
    [Test]
    public void SihifuToku1_ItemSihifu()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, ptId = 1, hokenId = 10, syosaisinKbn = 15, raiinNo = 1, oyaRaiinNo = 1;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                8,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113000910",
                10
            ),
             new OrdInfDetailModel(
                "113000910",
                10
            ),
            new OrdInfDetailModel(
                "113034510",
                10
            ),
            new OrdInfDetailModel(
                "113002310",
                10
            ),
            new OrdInfDetailModel(
                "113034610",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        Assert.True(iagkutokusitu1.Count == 0);
    }

    /// <summary>
    /// Check TenMst follow IsJouHou
    /// </summary>
    [Test]
    public void SihifuToku1_TenMst()
    {
        // Arrange
        int hpId = 1, sinDate1 = 20220331, ptId = 1, sinDate2 = 20220430, hokenId = 10, syosaisinKbn = 15, raiinNo = 1, oyaRaiinNo = 1;
        bool isJouhou1 = true, isJouhou2 = false;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                10,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                10,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034510",
                10
            ),
             new OrdInfDetailModel(
                "113000910",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate1, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate2, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate2, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou2);
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0);
    }

    /// <summary>
    /// Check MeiSkin
    /// </summary>
    [Test]
    public void SihifuToku1_MeiSkin()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf();
        var odrInfs = CheckedOrderData.ReadOdrInf();
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail();
        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 8001
            && p.GrpEdaNo == 1
            && p.StartDate <= sinDate
            && p.EndDate >= sinDate);
        var temp = systemGenerationConf?.Val ?? 0;
        if (systemGenerationConf != null) systemGenerationConf.Val = 1;
        else
        {
            systemGenerationConf = new SystemGenerationConf
            {
                HpId = 1,
                GrpCd = 8001,
                GrpEdaNo = 1,
                StartDate = 0,
                EndDate = 99999999,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
        }
        tenant.RaiinInfs.AddRange(raiinInfs);
        tenant.OdrInfs.AddRange(odrInfs);
        tenant.OdrInfDetails.AddRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                15,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                15,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034510121",
                10
            ),
             new OrdInfDetailModel(
                "113000910122",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn1, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        Assert.True(iagkutokusitu1.Count == 0);
        if (systemGenerationConf != null) systemGenerationConf.Val = temp;
        tenant.RaiinInfs.RemoveRange(raiinInfs);
        tenant.OdrInfs.RemoveRange(odrInfs);
        tenant.OdrInfDetails.RemoveRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
    }

    /// <summary>
    /// Check Syosai
    /// </summary>
    [Test]
    public void SihifuToku1_Syosai()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, syosaisinKbn2 = 6, syosaisinKbn3 = 2, syosaisinKbn4 = 4, syosaisinKbn5 = 8;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf();
        var odrInfs = CheckedOrderData.ReadOdrInf();
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail();
        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 8001
            && p.GrpEdaNo == 1
            && p.StartDate <= sinDate
            && p.EndDate >= sinDate);
        var temp = systemGenerationConf?.Val ?? 0;
        if (systemGenerationConf != null) systemGenerationConf.Val = 1;
        else
        {
            systemGenerationConf = new SystemGenerationConf
            {
                HpId = 1,
                GrpCd = 8001,
                GrpEdaNo = 1,
                StartDate = 0,
                EndDate = 99999999,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
        }
        tenant.RaiinInfs.AddRange(raiinInfs);
        tenant.OdrInfs.AddRange(odrInfs);
        tenant.OdrInfDetails.AddRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                3,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                3,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034510121",
                10
            ),
             new OrdInfDetailModel(
                "113000910122",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn1, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        var iagkutokusitu2 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn2, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        var iagkutokusitu3 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn3, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        var iagkutokusitu4 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn4, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        var iagkutokusitu5 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn5, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0 && iagkutokusitu4.Count == 0 && iagkutokusitu5.Count == 0);
        if (systemGenerationConf != null) systemGenerationConf.Val = temp;
        tenant.RaiinInfs.RemoveRange(raiinInfs);
        tenant.OdrInfs.RemoveRange(odrInfs);
        tenant.OdrInfDetails.RemoveRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
    }

    /// <summary>
    /// True
    /// </summary>
    [Test]
    public void SihifuToku1_True()
    {
        // Arrange
        int hpId = 1, sinDate = 20220822, hokenId = 10, syosaisinKbn1 = 20;
        long ptId = 7318199999, raiinNo = 70096280111231, oyaRaiinNo = 1957703;
        bool isJouhou = true;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf();
        var odrInfs = CheckedOrderData.ReadOdrInf();
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail();
        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 8001
            && p.GrpEdaNo == 1
            && p.StartDate <= sinDate
            && p.EndDate >= sinDate);
        var temp = systemGenerationConf?.Val ?? 0;
        if (systemGenerationConf != null) systemGenerationConf.Val = 1;
        else
        {
            systemGenerationConf = new SystemGenerationConf
            {
                HpId = 1,
                GrpCd = 8001,
                GrpEdaNo = 1,
                StartDate = 0,
                EndDate = 99999999,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
        }
        tenant.RaiinInfs.AddRange(raiinInfs);
        tenant.OdrInfs.AddRange(odrInfs);
        tenant.OdrInfDetails.AddRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                3,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                3,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034510121",
                10
            ),
             new OrdInfDetailModel(
                "113000910122",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn1, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
        Assert.True(iagkutokusitu1.Count > 0);
        if (systemGenerationConf != null) systemGenerationConf.Val = temp;
        tenant.RaiinInfs.RemoveRange(raiinInfs);
        tenant.OdrInfs.RemoveRange(odrInfs);
        tenant.OdrInfDetails.RemoveRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
    }

    /// <summary>
    /// Check Item Sihifu of Order Detail
    /// </summary>
    [Test]
    public void SihifuToku2_Sihifu()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, ptId = 1, hokenId = 10, syosaisinKbn = 15, raiinNo = 1, oyaRaiinNo = 1, iBirthDay = 30;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                8,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113000910",
                10
            ),
             new OrdInfDetailModel(
                "113034510",
                10
            ),
            new OrdInfDetailModel(
                "113002310",
                10
            ),
            new OrdInfDetailModel(
                "113034610",
                10
            )
        };
        var odrInfs = new List<int> { 1, 2, 3 };
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var sihifu = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou);
        Assert.True(sihifu.Count == 0);
    }

    /// <summary>
    /// Check TenMst follow IsJouHou
    /// </summary>
    [Test]
    public void SihifuToku2_TenMst()
    {
        // Arrange
        int hpId = 1, sinDate1 = 20220331, ptId = 1, sinDate2 = 20220430, hokenId = 10, syosaisinKbn = 15, raiinNo = 1, oyaRaiinNo = 1, iBirthDay = 30;
        bool isJouhou1 = true, isJouhou2 = false;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                10,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                10,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "11303451011",
                10
            ),
             new OrdInfDetailModel(
                "11300091011",
                10
            )
        };
        var odrInfs = new List<int> { 1, 2, 3 };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate1, hokenId, iBirthDay, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate2, hokenId, iBirthDay, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate2, hokenId, iBirthDay, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou2);
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0);
    }

    /// <summary>
    /// Check MeiSkin
    /// </summary>
    [Test]
    public void SihifuToku1_Hifuka()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, iBirthDay = 30;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf();
        var odrInfs = CheckedOrderData.ReadOdrInf();
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail();
        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 8001
            && p.GrpEdaNo == 1
            && p.StartDate <= sinDate
            && p.EndDate >= sinDate);
        var temp = systemGenerationConf?.Val ?? 0;
        if (systemGenerationConf != null) systemGenerationConf.Val = 1;
        else
        {
            systemGenerationConf = new SystemGenerationConf
            {
                HpId = 1,
                GrpCd = 8001,
                GrpEdaNo = 1,
                StartDate = 0,
                EndDate = 99999999,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
        }
        tenant.RaiinInfs.AddRange(raiinInfs);
        tenant.OdrInfs.AddRange(odrInfs);
        tenant.OdrInfDetails.AddRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                15,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                15,
                0,
                1,
                20221010,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034510121",
                10
            ),
             new OrdInfDetailModel(
                "113000910122",
                10
            )
        };
        var odrInfInputs = new List<int> { 1, 2, 3 };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInputs, isJouhou);
        Assert.True(iagkutokusitu1.Count == 0);
        if (systemGenerationConf != null) systemGenerationConf.Val = temp;
        tenant.RaiinInfs.RemoveRange(raiinInfs);
        tenant.OdrInfs.RemoveRange(odrInfs);
        tenant.OdrInfDetails.RemoveRange(odrInfDetails);
        tenant.SaveChanges();
        tenantTracking.SaveChanges();
    }
}