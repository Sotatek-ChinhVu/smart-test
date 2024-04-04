﻿using CloudUnitTest.SampleData;
using CommonChecker.DB;
using Domain.Models.Diseases;
using Domain.Models.MedicalExamination;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using static Helper.Constants.OrderInfConst;

namespace CloudUnitTest.Repository.CheckedSpecialItem;

public class CheckedOrderTest : BaseUT
{
    /// <summary>
    /// Check IgakuItem if it exist then return null
    /// </summary>
    [Test]
    public void IgakuTokusitu_001_Exist_IgakuItem()
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        // Act
        var iagkutokusitu = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);

        // Assert
        Assert.True(iagkutokusitu.Count == 0);
    }

    /// <summary>
    /// Check IsJouHou if it is true then check sinDate and tenMst, if it is false then only check tenMst
    /// </summary>
    [Test]
    public void IgakuTokusitu_002_IsJouhou()
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

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
    public void IgakuTokusitu_003_Syosai()
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

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
    public void IgakuTokusitu_004_Disease_Special()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 15;
        bool isJouhou1 = true, isJouhou2 = false;
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou2);

        // Assert
        Assert.True(iagkutokusitu1.Count == 1 && iagkutokusitu1.Any(i => i.CheckingContent == "\"特定疾患療養管理料（診療所・情報通信機器）\"を算定できる可能性があります。"));
        Assert.True(iagkutokusitu2.Count == 1 && iagkutokusitu2.Any(i => i.CheckingContent == "\"特定疾患療養管理料（診療所）\"を算定できる可能性があります。"));
    }

    /// <summary>
    /// Check Orther
    /// </summary>
    [Test]
    public void IgakuTokusitu_005_Disease_Other()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 15;
        bool isJouhou1 = true, isJouhou2 = false;
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou2);

        // Assert
        Assert.True(iagkutokusitu1.Count == 1 && iagkutokusitu1.Any(i => i.CheckingContent == "\"特定疾患療養管理料（診療所・情報通信機器）\"を算定できる可能性があります。"));
        Assert.True(iagkutokusitu2.Count == 1 && iagkutokusitu2.Any(i => i.CheckingContent == "\"特定疾患療養管理料（診療所）\"を算定できる可能性があります。"));
    }

    /// <summary>
    /// Check Not Special and Other
    /// </summary>
    [Test]
    public void IgakuTokusitu_006_Disease_NoMainDisease()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 15;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                9,
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
                "113001810131",
                10
            )
        };
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 4);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 4,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Not Special and Other
    /// </summary>
    [Test]
    public void IgakuTokusitu_007_Disease_NoDisease()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 15;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>();
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113001810131",
                10
            )
        };
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 4);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 4,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Item Sihifu of Order Detail
    /// </summary>
    [Test]
    public void SihifuToku1_008_ItemSihifu()
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);

        // Assert
        Assert.True(iagkutokusitu1.Count == 0);
    }

    /// <summary>
    /// Check TenMst follow IsJouHou
    /// </summary>
    [Test]
    public void SihifuToku1_009_TenMst()
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate1, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate2, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate2, hokenId, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou2);

        // Assert
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0);
    }

    /// <summary>
    /// Check MeiSkin
    /// </summary>
    [Test]
    public void SihifuToku1_010_MeiSkin()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        int randomKey = 10;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn1, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
            Assert.True(iagkutokusitu1.Count == 0);
            systemGenerationConf.Val = temp;

        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Syosai
    /// </summary>
    [Test]
    public void SihifuToku1_011_Syosai()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, syosaisinKbn2 = 6, syosaisinKbn3 = 2, syosaisinKbn4 = 4, syosaisinKbn5 = 8;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        int randomKey = 11;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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
        try
        {
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn1, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu2 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn2, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu3 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn3, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu4 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn4, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu5 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn5, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0 && iagkutokusitu4.Count == 0 && iagkutokusitu5.Count == 0);
            systemGenerationConf.Val = temp;

        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// True
    /// </summary>
    [Test]
    public void SihifuToku1_012_True()
    {
        // Arrange
        int hpId = 1, sinDate = 20220822, hokenId = 10, syosaisinKbn1 = 20;
        long ptId = 7318199999, raiinNo = 70096280111231, oyaRaiinNo = 1957703;
        bool isJouhou1 = true, isJouhou2 = false;
        int randomKey = 12;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn1, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou1);
            var iagkutokusitu2 = medicalExaminationRepository.SihifuToku1(hpId, ptId, sinDate, hokenId, syosaisinKbn1, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, isJouhou2);

            // Assert
            Assert.True(iagkutokusitu1.Count > 0);
            Assert.True(iagkutokusitu1.Count == 1 && iagkutokusitu1.Any(i => i.CheckingContent == "\"皮膚科特定疾患指導管理料（１）（情報通信機器）\"を算定できる可能性があります。"));
            Assert.True(iagkutokusitu2.Count == 1 && iagkutokusitu2.Any(i => i.CheckingContent == "\"皮膚科特定疾患指導管理料（１）\"を算定できる可能性があります。"));
            systemGenerationConf.Val = temp;

        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Item Sihifu of Order Detail
    /// </summary>
    [Test]
    public void SihifuToku2_013_Sihifu()
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
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        // Act
        var sihifu = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou);

        // Assert
        Assert.True(sihifu.Count == 0);
    }

    /// <summary>
    /// Check TenMst follow IsJouHou
    /// </summary>
    [Test]
    public void SihifuToku2_014_TenMst()
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

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate1, hokenId, iBirthDay, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate2, hokenId, iBirthDay, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate2, hokenId, iBirthDay, syosaisinKbn, raiinNo, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfs, isJouhou2);

        // Assert
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0);
    }

    /// <summary>
    /// Check MeiSkin
    /// </summary>
    [Test]
    public void SihifuToku2_015_Hifuka()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, iBirthDay = 30;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        int randomKey = 15;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 8001
            && p.GrpEdaNo == 1
            && p.StartDate <= sinDate
            && p.EndDate >= sinDate);
        var temp = systemGenerationConf?.Val ?? 0;
        if (systemGenerationConf != null) systemGenerationConf.Val = 0;
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
                Val = 0
            };
            tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
        }
        tenant.RaiinInfs.AddRange(raiinInfs);
        tenant.OdrInfs.AddRange(odrInfs);
        tenant.OdrInfDetails.AddRange(odrInfDetails);

        try
        {
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInputs, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check SihifuToku1 Skin2 contain L20
    /// </summary>
    [Test]
    public void SihifuToku2_016_Skin2_L20()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, iBirthDay1 = 30, iBirthDay2 = 15;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        int randomKey = 16;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
            {
                new PtDiseaseModel(
                    4,
                    10,
                    0,
                    20221212,
                    1,
                    "L2010"
                ),
                 new PtDiseaseModel(
                    4,
                    0,
                    1,
                    20221010,
                    1,
                    "L2010"
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
            var odrInfInput1s = new List<int> { 1, 2, 3 };
            var odrInfInput2s = new List<int> { 1, 23, 3 };

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay1, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);
            var iagkutokusitu2 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay2, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput2s, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Skin2 doesn't no L20
    /// </summary>
    [Test]
    public void SihifuToku2_017_Skin2_No_L20()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, iBirthDay1 = 30, iBirthDay2 = 15;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        int randomKey = 17;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                202210504,
                1,
                "M23"
            ),
             new PtDiseaseModel(
                5,
                0,
                1,
                20220504,
                1,
                "M23"
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
            var odrInfInput1s = new List<int> { 1, 2, 3 };
            var odrInfInput2s = new List<int> { 1, 23, 3 };

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay1, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);
            var iagkutokusitu2 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay2, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput2s, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Syosai
    /// </summary>
    [Test]
    public void SihifuToku2_018_Syosai()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, syosaisinKbn2 = 6, syosaisinKbn3 = 2, syosaisinKbn4 = 4, syosaisinKbn5 = 8, iBirthDay = 30;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        int randomKey = 18;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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
        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                4,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                4,
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
            var odrInfInput1s = new List<int> { 1, 2, 3 };

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);
            var iagkutokusitu2 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn2, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);
            var iagkutokusitu3 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn3, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);
            var iagkutokusitu4 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn4, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);
            var iagkutokusitu5 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn5, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0 && iagkutokusitu4.Count == 0 && iagkutokusitu5.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check True
    /// </summary>
    [Test]
    public void SihifuToku2_019_True()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 20, iBirthDay = 30;
        long ptId = 7318199999, raiinNo = 70096280111231300, oyaRaiinNo = 1;
        bool isJouhou = true;
        int randomKey = 19;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                4,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                4,
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
            var odrInfInput1s = new List<int> { 1, 2, 3 };

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.SihifuToku2(hpId, ptId, sinDate, hokenId, iBirthDay, raiinNo, syosaisinKbn1, oyaRaiinNo, byomeiModelList, ordInfDetailModels, odrInfInput1s, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count > 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void IgakuTenkan_020_Igaku()
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
                "113002850",
                10
            ),
             new OrdInfDetailModel(
                "113029610",
                10
            )
        };
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        // Act
        var igakuTenka = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);

        // Assert
        Assert.True(igakuTenka.Count == 0);
    }

    /// <summary>
    /// Check Syosai
    /// </summary>
    [Test]
    public void IgakuTenkan_021_Syosai()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, syosaisinKbn2 = 6, syosaisinKbn3 = 2, syosaisinKbn4 = 4, syosaisinKbn5 = 8;
        bool isJouhou = true;
        int randomKey = 21;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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
        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                4,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                4,
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu2 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn2, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu3 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn3, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu4 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn4, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu5 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn5, byomeiModelList, ordInfDetailModels, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0 && iagkutokusitu4.Count == 0 && iagkutokusitu5.Count == 0);
            systemGenerationConf.Val = temp;
        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check meiEpi
    /// </summary>
    [Test]
    public void IgakuTenkan_022_MeiEpi()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 20;
        bool isJouhou1 = true, isJouhou2 = false;
        int randomKey = 22;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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
        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                7,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                7,
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou1);
            var iagkutokusitu2 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou2);

            // Assert
            Assert.True(iagkutokusitu1.Count == 1 && iagkutokusitu1.Any(i => i.CheckingContent == "\"てんかん指導料（情報通信機器）\"を算定できる可能性があります。"));
            Assert.True(iagkutokusitu2.Count == 1 && iagkutokusitu2.Any(i => i.CheckingContent == "\"てんかん指導料\"を算定できる可能性があります。"));
            systemGenerationConf.Val = temp;
        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check other
    /// </summary>
    [Test]
    public void IgakuTenkan_023_Orther()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 20;
        bool isJouhou1 = true, isJouhou2 = false;
        int randomKey = 23;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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
        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
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
                "113034510121",
                10
            ),
             new OrdInfDetailModel(
                "113000910122",
                10
            )
        };

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou1);
            var iagkutokusitu2 = medicalExaminationRepository.IgakuTenkan(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou2);

            // Assert
            Assert.True(iagkutokusitu1.Count == 1 && iagkutokusitu1.Any(i => i.CheckingContent == "\"てんかん指導料（情報通信機器）\"を算定できる可能性があります。"));
            Assert.True(iagkutokusitu2.Count == 1 && iagkutokusitu2.Any(i => i.CheckingContent == "\"てんかん指導料\"を算定できる可能性があります。"));
            systemGenerationConf.Val = temp;

        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check some itemCd is Nanbyo
    /// </summary>
    [Test]
    public void IgakuNanbyo_024_IgakuNanByo()
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
                "113002910",
                10
            ),
             new OrdInfDetailModel(
                "113029710",
                10
            )
        };
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        // Act
        var igakuTenka = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);

        // Assert
        Assert.True(igakuTenka.Count == 0);
    }

    /// <summary>
    /// Check Syosai
    /// </summary>
    [Test]
    public void IgakuNanbyo_025_Syosai()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 1, syosaisinKbn2 = 6, syosaisinKbn3 = 2, syosaisinKbn4 = 4, syosaisinKbn5 = 8;
        bool isJouhou = true;
        int randomKey = 25;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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
        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                4,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                4,
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            //  Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu2 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn2, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu3 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn3, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu4 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn4, byomeiModelList, ordInfDetailModels, isJouhou);
            var iagkutokusitu5 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn5, byomeiModelList, ordInfDetailModels, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0 && iagkutokusitu4.Count == 0 && iagkutokusitu5.Count == 0);
            systemGenerationConf.Val = temp;
        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check IsJouhou
    /// </summary>
    [Test]
    public void IgakuNanbyo_026_IsJouhou()
    {
        // Arrange
        int hpId = 1, sinDate = 20220301, hokenId = 10, syosaisinKbn1 = 1;
        bool isJouhou = true;
        int randomKey = 26;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                4,
                10,
                0,
                20220302,
                1
            ),
             new PtDiseaseModel(
                4,
                0,
                1,
                20220302,
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0);
            systemGenerationConf.Val = temp;
        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check santeigai
    /// </summary>
    [Test]
    public void IgakuNanbyo_027_SanteiGai()
    {
        // Arrange
        int hpId = 1, sinDate = 20221212, hokenId = 10, syosaisinKbn1 = 20;
        bool isJouhou1 = true, isJouhou2 = false;
        int randomKey = 27;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                20,
                10,
                0,
                20221212,
                1,
                "abc",
                9
            ),
             new PtDiseaseModel(
                20,
                0,
                1,
                20221010,
                1,
                "abc",
                9
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou1);
            var iagkutokusitu2 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou2);

            // Assert
            Assert.True(iagkutokusitu1.Count == 1 && iagkutokusitu1.Any(i => i.CheckingContent == "\"難病外来指導管理料（情報通信機器）\"を算定できる可能性があります。"));
            Assert.True(iagkutokusitu2.Count == 1 && iagkutokusitu2.Any(i => i.CheckingContent == "\"難病外来指導管理料\"を算定できる可能性があります。"));
            systemGenerationConf.Val = temp;

        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check true
    /// </summary>
    [Test]
    public void IgakuNanbyo_028_True()
    {
        // Arrange
        int hpId = 1, sinDate = 20220501, hokenId = 10, syosaisinKbn1 = 20;
        bool isJouhou = true;
        int randomKey = 28;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var raiinInfs = CheckedOrderData.ReadRainInf(randomKey);
        var odrInfs = CheckedOrderData.ReadOdrInf(randomKey);
        var odrInfDetails = CheckedOrderData.ReadOdrInfDetail(randomKey);
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

        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                20,
                10,
                0,
                20221212,
                1
            ),
             new PtDiseaseModel(
                20,
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

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            // Act
            var iagkutokusitu1 = medicalExaminationRepository.IgakuNanbyo(hpId, sinDate, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou);

            // Assert
            Assert.True(iagkutokusitu1.Count == 0);
            systemGenerationConf.Val = temp;
        }
        finally
        {
            tenant.RaiinInfs.RemoveRange(raiinInfs);
            tenant.OdrInfs.RemoveRange(odrInfs);
            tenant.OdrInfDetails.RemoveRange(odrInfDetails);
            tenant.SaveChanges();
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check some ItemCd are IgakuNanbyo
    /// </summary>
    [Test]
    public void InitPriorityCheckDetail_029_IgakuNanbyo()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var checkOrders = new List<CheckedOrderModel>() {
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113002910",
                    1,
                    "Item 1",
                    1
                ),
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113001810",
                    1,
                    "Item 2",
                    1
                ),
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113002310",
                    1,
                    "Item 3",
                    1
                ),
              new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113002850",
                    1,
                    "Item 4",
                    1
                ),
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113000910",
                    1,
                    "Item 5",
                    1
                )
        };

        // Act
        checkOrders = medicalExaminationRepository.InitPriorityCheckDetail(checkOrders);

        //Assert
        Assert.False(checkOrders.Any(c => c.Santei && c.ItemCd != "113002910"));
    }

    /// <summary>
    /// Check ItemCd are IakuTenkan
    /// </summary>
    [Test]
    public void InitPriorityCheckDetail_030_IgakuTenkan()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var checkOrders = new List<CheckedOrderModel>() {
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113002850",
                    1,
                    "Item 1",
                    1
                ),    new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113001810",
                    1,
                    "Item 2",
                    1
                ),
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113002310",
                    1,
                    "Item 3",
                    1
                ),
              new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113000910",
                    1,
                    "Item 4",
                    1
                )
        };

        // Act
        checkOrders = medicalExaminationRepository.InitPriorityCheckDetail(checkOrders);

        //Assert
        Assert.False(checkOrders.Any(c => c.Santei && c.ItemCd != "113002850"));
    }

    /// <summary>
    /// Check some ItemCd are SihifuToku
    /// </summary>
    [Test]
    public void InitPriorityCheckDetail_031_SihifuToku1()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var checkOrders = new List<CheckedOrderModel>() {
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113000910",
                    1,
                    "Item 1",
                    1
                ),    new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113001810",
                    1,
                    "Item 2",
                    1
                ),
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113002310",
                    1,
                    "Item 3",
                    1
                )
        };

        // Act
        checkOrders = medicalExaminationRepository.InitPriorityCheckDetail(checkOrders);

        //Assert
        Assert.False(checkOrders.Any(c => c.Santei && c.ItemCd != "113000910"));
    }

    /// <summary>
    /// Check some ItemCd are IgakuTokusitu
    /// </summary>
    [Test]
    public void InitPriorityCheckDetail_032_IgakuTokusitu()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var checkOrders = new List<CheckedOrderModel>() {
            new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113001810",
                    1,
                    "Item 1",
                    1
                ),    new CheckedOrderModel(
                    CheckingType.Order,
                    true,
                    "Checked Order Model",
                    "113002310",
                    1,
                    "Item 2",
                    1
                )
        };

        // Act
        checkOrders = medicalExaminationRepository.InitPriorityCheckDetail(checkOrders);

        //Assert
        Assert.False(checkOrders.Any(c => c.Santei && c.ItemCd != "113001810"));
    }

    /// <summary>
    /// Check tikiHokatu
    /// </summary>
    [Test]
    public void ChikiHokatu_033_TikiHokatu()
    {
        //Arrange
        int hpId = 1, userId = 1, sinDate = 20220101, primaryDoctor = 1, tantoId = 1, syosaisinKbn = 1;
        long ptId = 1;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "112021770",
                10
            ),
             new OrdInfDetailModel(
                "112017270",
                10
            ),
            new OrdInfDetailModel(
                "112021870",
                10
            ),
            new OrdInfDetailModel(
                "112017570",
                10
            )
        };

        // Act
        var checkModels = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

        //Assert
        Assert.True(checkModels.Count == 0);
    }

    /// <summary>
    /// Check SyosaisinKbn
    /// </summary>
    [Test]
    public void ChikiHokatu_034_SyosaisinKbn()
    {
        //Arrange
        int hpId = 1, userId = 1, sinDate = 20220101, primaryDoctor = 1, tantoId = 1, syosaisinKbn = 1;
        long ptId = 1;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1120217701231",
                10
            ),
             new OrdInfDetailModel(
                "11201727011",
                10
            ),
            new OrdInfDetailModel(
                "112021870131",
                10
            ),
            new OrdInfDetailModel(
                "112017570131",
                10
            )
        };

        // Act
        var checkModels = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

        //Assert
        Assert.True(checkModels.Count == 0);
    }

    [Test]
    public void ChikiHokatu_035_TiikiSantei()
    {
        //Arrange
        int randomKey = 35;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var ptSanteiConfs = CheckedOrderData.ReadPtSanteiConf(randomKey);
        tenant.PtSanteiConfs.AddRange(ptSanteiConfs);
        try
        {
            tenant.SaveChanges();
            int hpId = 1, userId = 1, sinDate = 20220101, primaryDoctor = 1, tantoId = 1, syosaisinKbn = 3;
            long ptId = long.MaxValue;
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1120217701231",
                10
            ),
             new OrdInfDetailModel(
                "11201727011",
                10
            ),
            new OrdInfDetailModel(
                "112021870131",
                10
            ),
            new OrdInfDetailModel(
                "112017570131",
                10
            )
        };

            // Act
            var checkModels = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            tenant.RemoveRange(ptSanteiConfs);
            tenant.SaveChanges();
        }
    }

    /// <summary>
    /// Check Primary doctor
    /// </summary>
    [Test]
    public void ChikiHokatu_036_PrimaryDoctor()
    {
        //Arrange
        int randomKey = 36;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var ptSanteiConfs = CheckedOrderData.ReadPtSanteiConfToNoCheckSantei(randomKey);
        tenant.PtSanteiConfs.AddRange(ptSanteiConfs);
        try
        {
            tenant.SaveChanges();
            int hpId = 1, userId = 1, sinDate = 20220101, primaryDoctor = 0, tantoId = 1, syosaisinKbn = 3;
            long ptId = long.MaxValue;
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1120217701231",
                10
            ),
             new OrdInfDetailModel(
                "11201727011",
                10
            ),
            new OrdInfDetailModel(
                "112021870131",
                10
            ),
            new OrdInfDetailModel(
                "112017570131",
                10
            )
        };

            // Act
            var checkModels = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            tenant.RemoveRange(ptSanteiConfs);
            tenant.SaveChanges();
        }
    }

    /// <summary>
    /// Check TantoId
    /// </summary>
    [Test]
    public void ChikiHokatu_037_TantoId()
    {
        //Arrange
        int randomKey = 37;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var ptSanteiConfs = CheckedOrderData.ReadPtSanteiConfToNoCheckSantei(randomKey);
        var users = CheckedOrderData.ReadUserMst(randomKey);
        tenant.PtSanteiConfs.AddRange(ptSanteiConfs);
        tenant.UserMsts.AddRange(users);

        try
        {
            tenant.SaveChanges();
            int hpId = 1, userId = 99999, sinDate = 20110101, primaryDoctor = 2, tantoId = 1, syosaisinKbn = 3;
            long ptId = long.MaxValue;
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "114001610",
                14
            )
        };

            // Act
            var checkModels = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            tenant.RemoveRange(ptSanteiConfs);
            tenant.RemoveRange(users);
            tenant.SaveChanges();
        }
    }

    /// <summary>
    /// Check Oshin
    /// </summary>
    [Test]
    public void ChikiHokatu_038_Oshin()
    {
        //Arrange
        int randomKey = 38;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var ptSanteiConfs = CheckedOrderData.ReadPtSanteiConfToNoCheckSantei(randomKey);
        var users = CheckedOrderData.ReadUserMst(randomKey);
        tenant.PtSanteiConfs.AddRange(ptSanteiConfs);
        tenant.UserMsts.AddRange(users);
        try
        {
            tenant.SaveChanges();
            int hpId = 1, userId = 99999, sinDate1 = 20110101, sinDate2 = 20180402, primaryDoctor = 1, tantoId = 1, syosaisinKbn = 3;
            long ptId = long.MaxValue;
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "114001610",
                14
            ),
             new OrdInfDetailModel(
                "114042810",
                14
            )
        };

            // Act
            var checkModel1s = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate1, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

            var checkModel2s = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate2, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

            //Assert
            Assert.True(checkModel1s.Count == 0 && checkModel2s.Count == 0);
        }
        finally
        {
            tenant.RemoveRange(ptSanteiConfs);
            tenant.RemoveRange(users);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void ChikiHokatu_039_JidoSantei()
    {
        //Arrange
        int randomKey = 39;
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var ptSanteiConfs = CheckedOrderData.ReadPtSanteiConfToNoCheckSantei(randomKey);
        var users = CheckedOrderData.ReadUserMst(randomKey);
        tenant.PtSanteiConfs.AddRange(ptSanteiConfs);
        tenant.UserMsts.AddRange(users);
        try
        {
            tenant.SaveChanges();
            int hpId = 1, userId = 99999, sinDate = 20220402, primaryDoctor = 1, tantoId = 1, syosaisinKbn = 3;
            long ptId = long.MaxValue - randomKey;
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "Y90298",
                1
            )
        };

            // Act
            var checkModels = medicalExaminationRepository.ChikiHokatu(hpId, ptId, userId, sinDate, primaryDoctor, tantoId, ordInfDetailModels, syosaisinKbn);

            //Assert
            Assert.True(checkModels.Count > 0);
        }
        finally
        {
            tenant.RemoveRange(ptSanteiConfs);
            tenant.RemoveRange(users);
            tenant.SaveChanges();
        }
    }

    /// <summary>
    /// Check some ItemCd are Yakkuzai
    /// </summary>
    [Test]
    public void YakkuZai_040_Item()
    {
        //Arrange
        int hpId = 1, birthDay = 20, sinDate = 20220402;
        long ptId = long.MaxValue;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "120002370",
                1
            )
        };

        // Act
        var checkModels = medicalExaminationRepository.YakkuZai(hpId, ptId, sinDate, birthDay, ordInfDetailModels, new());

        //Assert
        Assert.True(checkModels.Count == 0);
    }

    [Test]
    public void YakkuZai_041_IsDrug()
    {
        //Arrange
        int hpId = 1, birthDay = 20, sinDate = 20220402;
        long ptId = long.MaxValue;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "12000237012",
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                25,
                new()
                )
        };
        // Act
        var checkModels = medicalExaminationRepository.YakkuZai(hpId, ptId, sinDate, birthDay, ordInfDetailModels, ordInfs);

        //Assert
        Assert.True(checkModels.Count == 0);
    }

    /// <summary>
    /// Check Age
    /// </summary>
    [Test]
    public void YakkuZai_042_Age()
    {
        //Arrange
        int hpId = 1, birthDay = CIUtil.DateTimeToInt(DateTime.UtcNow.AddYears(-1)), sinDate = CIUtil.DateTimeToInt(DateTime.UtcNow);
        long ptId = long.MaxValue;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

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

        var autoSanteiCheck = tenantTracking.AutoSanteiMsts.Any(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate);
        var autoSanteiMst = new AutoSanteiMst
        {
            HpId = 1,
            ItemCd = "113003510",
            SeqNo = 1,
            StartDate = 0,
            EndDate = 99999999,
            CreateId = 1,
            CreateDate = DateTime.UtcNow,
            UpdateId = 1
        };
        if (!autoSanteiCheck)
        {
            tenantTracking.Add(
                autoSanteiMst
           );

        }

        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "12000237012",
                1
            )
        };
            var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                new()
                )
        };

            // Act
            var checkModels = medicalExaminationRepository.YakkuZai(hpId, ptId, sinDate, birthDay, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            if (autoSanteiMst.Id > 0) tenantTracking.RemoveRange(autoSanteiMst);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check YakuzaiJoho
    /// </summary>
    [Test]
    public void YakkuZai_043_YakuzaiJoho()
    {
        //Arrange
        int hpId = 1, birthDay = 19900101, sinDate = 999999999;
        long ptId = long.MaxValue;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

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

        var autoSanteiCheck = tenantTracking.AutoSanteiMsts.Any(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate);
        var autoSanteiMst = new AutoSanteiMst
        {
            HpId = 1,
            ItemCd = "113003510",
            SeqNo = 1,
            StartDate = 0,
            EndDate = 99999999,
            CreateId = 1,
            CreateDate = DateTime.UtcNow,
            UpdateId = 1
        };
        if (!autoSanteiCheck)
        {
            tenantTracking.Add(
                autoSanteiMst
           );

        }

        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "12000237012",
                1
            )
        };
            var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                new()
                )
        };
            // Act
            var checkModels = medicalExaminationRepository.YakkuZai(hpId, ptId, sinDate, birthDay, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenantTracking.RemoveRange(autoSanteiMst);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check YakuzaiJohoTeiyo
    /// </summary>
    [Test]
    public void YakkuZai_044_YakuzaiJohoTeiyo()
    {
        //Arrange
        int hpId = 1, birthDay = 19900101, sinDate = 999999999;
        long ptId = long.MaxValue;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

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

        var autoSanteiCheck = tenantTracking.AutoSanteiMsts.Any(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate);
        var autoSanteiMst = new AutoSanteiMst
        {
            HpId = 1,
            ItemCd = "113003510",
            SeqNo = 1,
            StartDate = 0,
            EndDate = 99999999,
            CreateId = 1,
            CreateDate = DateTime.UtcNow,
            UpdateId = 1
        };
        if (!autoSanteiCheck)
        {
            tenantTracking.Add(
                autoSanteiMst
           );

        }

        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113701310",
                1
            )
        };
            var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                new()
                )
        };
            // Act
            var checkModels = medicalExaminationRepository.YakkuZai(hpId, ptId, sinDate, birthDay, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenantTracking.RemoveRange(autoSanteiMst);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Exist message which are returned
    /// </summary>
    [Test]
    public void YakkuZai_045_ExistMessage()
    {
        //Arrange
        int hpId = 1, birthDay = 19900101, sinDate = 21000101;
        long ptId = long.MaxValue;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
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

        var autoSanteiCheck = tenantTracking.AutoSanteiMsts.Any(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate);
        var autoSanteiMst = new AutoSanteiMst
        {
            HpId = 1,
            ItemCd = "113003510",
            SeqNo = 1,
            StartDate = 0,
            EndDate = 99999999,
            CreateId = 1,
            CreateDate = DateTime.UtcNow,
            UpdateId = 1
        };
        if (!autoSanteiCheck)
        {
            tenantTracking.Add(
                autoSanteiMst
           );

        }
        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113701310",
                1
            )
        };
            var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                new()
                )
        };

            // Act
            var checkModels = medicalExaminationRepository.YakkuZai(hpId, ptId, sinDate, birthDay, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 2);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            if (autoSanteiMst.Id > 0) tenantTracking.RemoveRange(autoSanteiMst);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Some ItemCd are SiIkuji
    /// </summary>
    [Test]
    public void SiIkuji_046_Item()
    {
        //Arrange
        int hpId = 1, birthDay = 19900101, sinDate = 21000101;

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "111000470",
                1
            ),
            new OrdInfDetailModel(
                "113037110",
                1
            )
        };

        // Act
        var checkModels = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, true, 1);

        //Assert
        Assert.True(checkModels.Count == 0);
    }

    /// <summary>
    /// Check isjouhou
    /// </summary>
    [Test]
    public void SiIkuji_047_IsJouhou()
    {
        //Arrange
        int hpId = 1, birthDay = 19900101, sinDate1 = 20220330, sinDate2 = 999999999;

        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1110004701",
                1
            ),
            new OrdInfDetailModel(
                "1130371101",
                1
            )
        };

        // Act
        var checkModel1s = medicalExaminationRepository.SiIkuji(hpId, sinDate1, birthDay, ordInfDetailModels, true, 1);
        var checkModel2s = medicalExaminationRepository.SiIkuji(hpId, sinDate2, birthDay, ordInfDetailModels, true, 1);
        var checkModel3s = medicalExaminationRepository.SiIkuji(hpId, sinDate2, birthDay, ordInfDetailModels, false, 1);

        //Assert
        Assert.True(checkModel1s.Count == 0 && checkModel2s.Count == 0 && checkModel3s.Count == 0);
    }

    /// <summary>
    /// Check shonika
    /// </summary>
    [Test]
    public void SiIkuji_048_Shonika()
    {
        //Arrange
        int hpId = 1, birthDay = 19900101, sinDate = 21000101;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 8001
          && p.GrpEdaNo == 0
          && p.StartDate <= sinDate
          && p.EndDate >= sinDate);
        var temp = systemGenerationConf?.Val ?? 0;
        if (systemGenerationConf != null) systemGenerationConf.Val = 0;
        else
        {
            systemGenerationConf = new SystemGenerationConf
            {
                HpId = 1,
                GrpCd = 8001,
                GrpEdaNo = 0,
                StartDate = 0,
                EndDate = 99999999,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemGenerationConfs.Add(systemGenerationConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113701310",
                1
            )
        };

            // Act
            var checkModels = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, true, 1);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check autosantei
    /// </summary>
    [Test]
    public void SiIkuji_049_AutoSantei()
    {
        //Arrange
        int hpId = 1, birthDay = 19900101, sinDate = 21000101;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 8001
          && p.GrpEdaNo == 0
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
                GrpEdaNo = 0,
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

        var autoSanteiCheck = tenantTracking.AutoSanteiMsts.Any(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate);
        var autoSanteiMst = new AutoSanteiMst
        {
            HpId = 1,
            ItemCd = "113003510",
            SeqNo = 1,
            StartDate = 0,
            EndDate = 99999999,
            CreateId = 1,
            CreateDate = DateTime.UtcNow,
            UpdateId = 1
        };
        if (!autoSanteiCheck)
        {
            tenantTracking.Add(
                autoSanteiMst
           );

        }
        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "12000237012",
                1
            )
        };
            // Act
            var checkModels = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, true, 1);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            if (autoSanteiMst.Id > 0) tenantTracking.RemoveRange(autoSanteiMst);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Age
    /// </summary>
    [Test]
    public void SiIkuji_050_Age()
    {
        //Arrange
        int hpId = 1, birthDay = 21000101, sinDate = 21100101;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 8001
          && p.GrpEdaNo == 0
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
                GrpEdaNo = 0,
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

        var autoSanteis = tenantTracking.AutoSanteiMsts.Where(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate).ToList();

        if (autoSanteis.Count > 0)
        {
            tenantTracking.RemoveRange(autoSanteis);
        }

        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "12000237012",
                1
            )
        };
            // Act
            var checkModels = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, true, 1);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            if (autoSanteis.Count > 0) tenantTracking.AddRange(autoSanteis);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check Syosin
    /// </summary>
    [Test]
    public void SiIkuji_051_Syosin()
    {
        //Arrange
        int hpId = 1, birthDay = 21000101, sinDate = 21000101;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 8001
          && p.GrpEdaNo == 0
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
                GrpEdaNo = 0,
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

        var autoSanteis = tenantTracking.AutoSanteiMsts.Where(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate).ToList();

        if (autoSanteis.Count > 0)
        {
            tenantTracking.RemoveRange(autoSanteis);
        }

        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "12000237012",
                1
            )
        };
            // Act
            var checkModels = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, true, 5);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemGenerationConf.Val = temp;
            if (autoSanteis.Count > 0) tenantTracking.AddRange(autoSanteis);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check when messages are returned
    /// </summary>
    [Test]
    public void SiIkuji_052_ExistMessage()
    {
        //Arrange
        int hpId = 1, birthDay = 21000101, sinDate = 21000101;
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

        var systemGenerationConf = tenantTracking.SystemGenerationConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 8001
          && p.GrpEdaNo == 0
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
                GrpEdaNo = 0,
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

        var autoSanteis = tenantTracking.AutoSanteiMsts.Where(e =>
                 e.HpId == hpId &&
                 e.ItemCd == "113003510" &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate).ToList();

        if (autoSanteis.Count > 0)
        {
            tenantTracking.RemoveRange(autoSanteis);
        }

        try
        {
            tenantTracking.SaveChanges();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
            var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "12000237012",
                1
            )
        };

            // Act
            var checkModel1s = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, true, 1);
            var checkModel2s = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, true, 6);
            var checkModel3s = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, false, 1);
            var checkModel4s = medicalExaminationRepository.SiIkuji(hpId, sinDate, birthDay, ordInfDetailModels, false, 6);

            //Assert
            Assert.True(checkModel1s.Count == 1 && checkModel1s.Any(i => i.CheckingContent == "\"乳幼児育児栄養指導料（情報通信機器）\"を算定できる可能性があります。"));
            Assert.True(checkModel2s.Count == 1 && checkModel2s.Any(i => i.CheckingContent == "\"乳幼児育児栄養指導料（情報通信機器）\"を算定できる可能性があります。"));
            Assert.True(checkModel3s.Count == 1 && checkModel3s.Any(i => i.CheckingContent == "\"乳幼児育児栄養指導料\"を算定できる可能性があります。"));
            Assert.True(checkModel4s.Count == 1 && checkModel4s.Any(i => i.CheckingContent == "\"乳幼児育児栄養指導料\"を算定できる可能性があります。"));
        }
        finally
        {
            systemGenerationConf.Val = temp;
            if (autoSanteis.Count > 0) tenantTracking.AddRange(autoSanteis);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check some ItemCd are zanyaku
    /// </summary>
    [Test]
    public void Zanyaku_053_Item()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "@ZANGIGI",
                1
            ),
            new OrdInfDetailModel(
                "@ZANTEIKYO",
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                new()
                )
        };

        // Act
        var checkModel1s = medicalExaminationRepository.Zanyaku(hpId, sinDate, ordInfDetailModels, ordInfs);

        //Assert
        Assert.True(checkModel1s.Count == 0);
    }

    /// <summary>
    /// Check Zanyaku is drug
    /// </summary>
    [Test]
    public void Zanyaku_054_Drug()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        var mockSystemConf = new Mock<ISystemConfRepository>();
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, mockSystemConf.Object, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "123421341",
                1
            ),
            new OrdInfDetailModel(
                "123421342",
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                new()
                )
        };

        var tenItem = new TenItemModel();

        mockMstItem.Setup(finder => finder.GetTenMstInfo(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns((int hpId, string itemCd, int sinDate) => tenItem);

        // Act
        var checkModel1s = medicalExaminationRepository.Zanyaku(hpId, sinDate, ordInfDetailModels, ordInfs);

        //Assert
        Assert.True(checkModel1s.Count > 0);
    }

    /// <summary>
    /// Check some ItemCd are TouyakutokusyoSyoho
    /// </summary>
    [Test]
    public void TouyakuTokusyoSyoho_055_Item()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "120002270",
                1
            ),
            new OrdInfDetailModel(
                "120003170",
                1
            ),
            new OrdInfDetailModel(
                "120002570",
                1
            ),
            new OrdInfDetailModel(
                "120003270",
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                new()
                )
        };

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

        // Act
        var checkModel1s = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

        //Assert
        Assert.True(checkModel1s.Count == 0);
    }

    /// <summary>
    /// Check TouyakuTokusyoSyoho is drug
    /// </summary>
    [Test]
    public void TouyakuTokusyoSyoho_056_Drug()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1200022701",
                1
            ),
            new OrdInfDetailModel(
                "1200031702",
                1
            ),
            new OrdInfDetailModel(
                "1200025703",
                1
            ),
            new OrdInfDetailModel(
                "1200032704",
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                25,
                new()
                )
        };

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

        // Act
        var checkModel1s = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

        //Assert
        Assert.True(checkModel1s.Count == 0);
    }


    /// <summary>
    /// Check TouyakuTokusyoSyoho is true
    /// </summary>
    [Test]
    public void TouyakuTokusyoSyoho_057_True()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        var ordInfDetailModel1s = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1200022701",
                20,
                1
            ),
            new OrdInfDetailModel(
                "Z101",
                20
            ),
            new OrdInfDetailModel(
                "120000710",
                1
            ),
            new OrdInfDetailModel(
                "120001010",
                1
            ),
            new OrdInfDetailModel(
                21,
                28
            )
        };

        var ordInfDetailModel2s = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1200022701",
                20,
                1
            ),
            new OrdInfDetailModel(
                "Z101",
                20
            ),
            new OrdInfDetailModel(
                "120000710",
                1
            ),
            new OrdInfDetailModel(
                "120001010",
                1
            ),
            new OrdInfDetailModel(
                21,
                21
            )
        };
        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModel1s
                ),

        };

        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModel2s
                ),
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModel1s = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModel1s, ordInf1s);
            var checkModel2s = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModel1s, ordInf2s);

            //Assert
            Assert.True(checkModel1s.Count > 0 && checkModel2s.Count > 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check special
    /// </summary>
    [Test]
    public void CheckByoMei_058_Special()
    {
        //Arrange
        int hpId = 1, sinDate = 21100101, hokenId = 10, inoutKbn = 0;
        bool isCheckShuByomeiOnly1 = true, isCheckTeikyoByomei1 = true, isCheckTeikyoByomei2 = false, isCheckShuByomeiOnly2 = false;
        string itemTokusyoCd = "", itemCd = "88888888";
        var tenant = TenantProvider.GetNoTrackingDataContext();

        var byomeiMsts = new List<ByomeiMst>()
        {
            new ByomeiMst
            {
                HpId = 1,
                ByomeiCd = "10000",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1
            },
            new ByomeiMst
            {
                HpId = 1,
                ByomeiCd = "11111",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1
            }
        };
        tenant.ByomeiMsts.AddRange(byomeiMsts);

        var tekiouByomeiMst = new List<TekiouByomeiMst>() { new TekiouByomeiMst {
            HpId = 1,
            ItemCd = itemCd,
            ByomeiCd = "10000",
            SystemData = 1,
            IsInvalidTokusyo = 2
        } };
        tenant.TekiouByomeiMsts.AddRange(tekiouByomeiMst);
        try
        {
            tenant.SaveChanges();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                21000101,
                1,
                21010101,
                1,
                "10000"
            ),
            new PtDiseaseModel(
                5,
                0,
                21000101,
                0,
                1,
                1,
                "10000"
            )
        };

            // Act
            var checkModel1 = medicalExaminationRepository.CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly1, isCheckTeikyoByomei1, itemTokusyoCd, itemCd, inoutKbn, byomeiModelList);
            var checkModel2 = medicalExaminationRepository.CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly2, isCheckTeikyoByomei2, itemTokusyoCd, itemCd, inoutKbn, byomeiModelList);

            //Assert
            Assert.True(checkModel1.CheckingType > 0 && checkModel2.CheckingType > 0);
        }
        finally
        {
            tenant.ByomeiMsts.RemoveRange(byomeiMsts);
            tenant.TekiouByomeiMsts.RemoveRange(tekiouByomeiMst);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void CheckByoMei_059_Other()
    {
        //Arrange
        int hpId = 1, sinDate = 21100101, hokenId = 10, inoutKbn = 0;
        bool isCheckShuByomeiOnly1 = true, isCheckTeikyoByomei1 = true, isCheckTeikyoByomei2 = false, isCheckShuByomeiOnly2 = false;
        string itemTokusyoCd = "", itemCd = "88888888";
        var tenant = TenantProvider.GetNoTrackingDataContext();

        var byomeiMsts = new List<ByomeiMst>()
        {
            new ByomeiMst
            {
                HpId = 1,
                ByomeiCd = "10000",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1
            },
            new ByomeiMst
            {
                HpId = 1,
                ByomeiCd = "11111",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1
            }
        };
        tenant.ByomeiMsts.AddRange(byomeiMsts);

        var tekiouByomeiMst = new List<TekiouByomeiMst>() { new TekiouByomeiMst {
            HpId = 1,
            ItemCd = itemCd,
            ByomeiCd = "10000",
            SystemData = 1,
            IsInvalidTokusyo = 2
        } };
        tenant.TekiouByomeiMsts.AddRange(tekiouByomeiMst);
        try
        {
            tenant.SaveChanges();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                21000101,
                1,
                21010101,
                1,
                "10000"
            ),
            new PtDiseaseModel(
                8,
                0,
                21000101,
                0,
                1,
                1,
                "10000"
            )
        };

            // Act
            var checkModel1 = medicalExaminationRepository.CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly1, isCheckTeikyoByomei1, itemTokusyoCd, itemCd, inoutKbn, byomeiModelList);
            var checkModel2 = medicalExaminationRepository.CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly2, isCheckTeikyoByomei2, itemTokusyoCd, itemCd, inoutKbn, byomeiModelList);

            //Assert
            Assert.True(checkModel1.CheckingType > 0 && checkModel2.CheckingType > 0 && !checkModel1.Santei && !checkModel2.Santei);

        }
        finally
        {
            tenant.ByomeiMsts.RemoveRange(byomeiMsts);
            tenant.TekiouByomeiMsts.RemoveRange(tekiouByomeiMst);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void CheckByoMei_060_True()
    {
        //Arrange
        int hpId = 1, sinDate = 21100101, hokenId = 10, inoutKbn = 0;
        bool isCheckShuByomeiOnly1 = true, isCheckTeikyoByomei1 = true, isCheckTeikyoByomei2 = false, isCheckShuByomeiOnly2 = false;
        string itemTokusyoCd = "", itemCd = "88888888";
        var tenant = TenantProvider.GetNoTrackingDataContext();

        var byomeiMsts = new List<ByomeiMst>()
        {
            new ByomeiMst
            {
                HpId = 1,
                ByomeiCd = "10000",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1
            },
            new ByomeiMst
            {
                HpId = 1,
                ByomeiCd = "11111",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1
            }
        };
        tenant.ByomeiMsts.AddRange(byomeiMsts);

        var tekiouByomeiMst = new List<TekiouByomeiMst>() { new TekiouByomeiMst {
            HpId = 1,
            ItemCd = itemCd,
            ByomeiCd = "10000",
            SystemData = 1,
            IsInvalidTokusyo = 2
        } };
        tenant.TekiouByomeiMsts.AddRange(tekiouByomeiMst);
        try
        {
            tenant.SaveChanges();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMstItem = new Mock<IMstItemRepository>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

            var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                6,
                10,
                21000101,
                1,
                21010101,
                1,
                "10000"
            ),
            new PtDiseaseModel(
                6,
                0,
                21000101,
                0,
                1,
                1,
                "10000"
            )
        };

            // Act
            var checkModel1 = medicalExaminationRepository.CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly1, isCheckTeikyoByomei1, itemTokusyoCd, itemCd, inoutKbn, byomeiModelList);
            var checkModel2 = medicalExaminationRepository.CheckByoMei(hpId, sinDate, hokenId, isCheckShuByomeiOnly2, isCheckTeikyoByomei2, itemTokusyoCd, itemCd, inoutKbn, byomeiModelList);

            //Assert
            Assert.True(checkModel1.CheckingType == 0 && checkModel2.CheckingType == 0);
        }
        finally
        {
            tenant.ByomeiMsts.RemoveRange(byomeiMsts);
            tenant.TekiouByomeiMsts.RemoveRange(tekiouByomeiMst);
            tenant.SaveChanges();
        }
    }

    #region TouyakuTokusyoSyoho Special
    /// <summary>
    /// Check  day < 28
    /// </summary>
    [Test]
    public void TouyakuTokusyoSyoho_061_SystemSetting1_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_062_SystemSetting1_InOutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
               "1221",
                20,
                1
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };
        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                )
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s.Union(ordInf2s).ToList());

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_063_SystemSetting1_InHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                ),

        };
        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                )
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s.Union(ordInf2s).ToList());

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 0 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_064_SystemSetting1_NoMainDisease_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                0
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_065_SystemSetting1_MainDisease_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_066_SystemSetting1_MainDisease_NoMapDrug_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 20000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                21,
                21
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                1,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_067SystemSetting1_MainDisease_MapDrug_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        string byomeiCd = "0670670", itemCd = "0670670670";
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                itemCd,
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                20220101,
                0,
                1,
                1,
                byomeiCd
            )
        };

        var tenant = TenantProvider.GetNoTrackingDataContext();
        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var byomeiMsts = CheckedOrderData.ReadByomeiMst(byomeiCd);
        var tekiouByomeiMsts = CheckedOrderData.ReadTekiouByomeiMst(byomeiCd, itemCd);

        tenant.ByomeiMsts.AddRange(byomeiMsts);
        tenant.TekiouByomeiMsts.AddRange(tekiouByomeiMsts);

        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenant.SaveChanges();
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.TekiouByomeiMsts.RemoveRange(tekiouByomeiMsts);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check day > 28
    /// </summary>
    [Test]
    public void TouyakuTokusyoSyoho_068_SystemSetting2_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 2,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 3,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_069_SystemSetting2_InOutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };
        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                )
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                1,
                22000101,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s.Union(ordInf2s).ToList());

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_070_SystemSetting2_InHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                ),

        };
        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                )
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s.Union(ordInf2s).ToList());

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 0 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_071_SystemSetting2_NoMainDisease_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
           new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                0
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_072_SystemSetting2_MainDisease_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
           new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）"); systemConf.Val = temp;
        }
        finally
        {
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_073_SystemSetting2_MainDisease_NoMapDrug_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 20000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
             new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_074_SystemSetting2_MainDisease_MapDrug_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        string byomeiCd = "0740740", itemCd = "0740740740";

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                itemCd,
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                5,
                10,
                1,
                1,
                22000101,
                1,
                byomeiCd
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var byomeiMsts = CheckedOrderData.ReadByomeiMst(byomeiCd);
        var tekiouByomeiMsts = CheckedOrderData.ReadTekiouByomeiMst(byomeiCd, itemCd);
        tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        tenantTracking.TekiouByomeiMsts.AddRange(tekiouByomeiMsts);
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.TekiouByomeiMsts.RemoveRange(tekiouByomeiMsts);
            tenantTracking.SaveChanges();
        }
    }

    #endregion

    #region TouyakuTokusyoSyoho Other
    /// <summary>
    /// Check  day < 28
    /// </summary>
    [Test]
    public void TouyakuTokusyoSyoho_075_SystemSetting1_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "123",
                20,
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_076_SystemSetting1_InOutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "123",
                20,
                1
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };
        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                )
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s.Union(ordInf2s).ToList());

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_077_SystemSetting1_InHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "123",
                20,
                1
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 0 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_078_SystemSetting1_NoMainDisease_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "123",
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                0
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_079_SystemSetting1_MainDisease_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "123",
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_080_SystemSetting1_MainDisease_NoMapDrug_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 20000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
               "123",
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_081_SystemSetting1_MainDisease_MapDrug_OutHospistal_LessThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        string byomeiCd = "0810810", itemCd = "0810810810";

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                itemCd,
                20,
                1
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                1,
                0,
                22000101,
                1,
                byomeiCd
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var byomeiMsts = CheckedOrderData.ReadByomeiMst(byomeiCd);
        var tekiouByomeiMsts = CheckedOrderData.ReadTekiouByomeiMst(byomeiCd, itemCd);
        tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        tenantTracking.TekiouByomeiMsts.AddRange(tekiouByomeiMsts);
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 0);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 1);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算１（処方箋料）");
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.TekiouByomeiMsts.RemoveRange(tekiouByomeiMsts);
            tenantTracking.SaveChanges();
        }
    }

    /// <summary>
    /// Check day > 28
    /// </summary>
    [Test]
    public void TouyakuTokusyoSyoho_082_SystemSetting2_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                1,
                0,
                1,
                1,
                "8846347"
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 2,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 3,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_083_SystemSetting2_InOutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
             new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };
        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                )
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s.Union(ordInf2s).ToList());

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_084_SystemSetting2_InHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInf1s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                ),

        };
        var ordInf2s = new List<OrdInfModel>() {
            new OrdInfModel(
                0,
                21,
                ordInfDetailModels
                )
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 0;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInf1s.Union(ordInf2s).ToList());

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 0 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_085_SystemSetting2_NoMainDisease_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                0
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_086_SystemSetting2_MainDisease_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),

        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                1,
                1,
                22000101,
                1,
                "8846347"
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 0;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 0
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_087_SystemSetting2_MainDisease_NoMapDrug_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 20000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "1221",
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                0,
                1,
                1
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }

        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 0);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.SaveChanges();
        }
    }

    [Test]
    public void TouyakuTokusyoSyoho_088_SystemSetting2_MainDisease_MapDrug_OutHospistal_MoreThan28()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101, hokenId = 10;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, systemConfRepository, mockMstItem.Object);
        string byomeiCd = "0880880", itemCd = "0880880880";

        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                itemCd,
                20,
                1
            ),
            new OrdInfDetailModel(
                21,
                30
            )
        };

        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                ordInfDetailModels
                ),
        };

        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                8,
                10,
                1,
                0,
                22000101,
                1,
                byomeiCd
            )
        };

        var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
        var byomeiMsts = CheckedOrderData.ReadByomeiMst(byomeiCd);
        var tekiouByomeiMsts = CheckedOrderData.ReadTekiouByomeiMst(byomeiCd, itemCd);
        tenantTracking.ByomeiMsts.AddRange(byomeiMsts);
        tenantTracking.TekiouByomeiMsts.AddRange(tekiouByomeiMsts);
        var systemConf = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
            && p.GrpCd == 2002
            && p.GrpEdaNo == 2);
        var systemConf2 = tenantTracking.SystemConfs.FirstOrDefault(p => p.HpId == 1
          && p.GrpCd == 2002
          && p.GrpEdaNo == 3);
        var temp = systemConf?.Val ?? 0;
        if (systemConf != null) systemConf.Val = 1;
        else
        {
            systemConf = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 0,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        if (systemConf2 != null) systemConf2.Val = 1;
        else
        {
            systemConf2 = new SystemConf
            {
                HpId = 1,
                GrpCd = 2002,
                GrpEdaNo = 1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateId = 1,
                UpdateId = 1,
                Val = 1
            };
            tenantTracking.SystemConfs.Add(systemConf);
        }
        try
        {
            tenantTracking.SaveChanges();

            // Act
            var checkModels = medicalExaminationRepository.TouyakuTokusyoSyoho(hpId, sinDate, hokenId, byomeiModelList, ordInfDetailModels, ordInfs);

            //Assert
            Assert.True(checkModels.Count == 1 && checkModels.First().InOutKbn == 1 && checkModels.First().CheckingType == CheckingType.MissingCalculate && checkModels.First().ItemName == "特定疾患処方管理加算２（処方箋料）" && !checkModels.First().Santei);
        }
        finally
        {
            systemConf.Val = temp;
            systemConf2.Val = temp;
            tenantTracking.ByomeiMsts.RemoveRange(byomeiMsts);
            tenantTracking.TekiouByomeiMsts.RemoveRange(tekiouByomeiMsts);
            tenantTracking.SaveChanges();
        }
    }

    #endregion

    /// <summary>
    /// Check Zanyaku is drug
    /// </summary>
    [Test]
    public void Zanyaku_076_Drug()
    {
        //Arrange
        int hpId = 1, sinDate = 21000101;
        var mockConfiguration = new Mock<IConfiguration>();
        var mockMstItem = new Mock<IMstItemRepository>();
        var mockSystemConf = new Mock<ISystemConfRepository>();
        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider, mockSystemConf.Object, mockMstItem.Object);
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "123421341",
                1
            ),
            new OrdInfDetailModel(
                "123421342",
                1
            )
        };
        var ordInfs = new List<OrdInfModel>() {
            new OrdInfModel(
                1,
                21,
                new()
                )
        };

        var tenItem = new TenItemModel();

        var mock = new Mock<IMstItemRepository>();
        mock.Setup(finder => finder.GetTenMstInfo(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns((int hpId, string itemCd, int sinDate) => null);

        // Act
        var checkModel1s = medicalExaminationRepository.Zanyaku(hpId, sinDate, ordInfDetailModels, ordInfs);

        //Assert
        Assert.True(checkModel1s.Count == 0);
    }
}