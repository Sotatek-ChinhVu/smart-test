using CloudUnitTest.SampleData;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKubunMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;

namespace CloudUnitTest.Repository.InitKbnSetting;

public class InitKbnSettingTest : BaseUT
{
    [Test]
    public void TC_001_GetRaiinKbns_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnMst
        var raiinKbnMstList = ReadDataInitKbnSetting.ReadRaiinKbnMst();
        tenant.RaiinKbnMsts.AddRange(raiinKbnMstList);

        // RaiinKbnDetail
        var raiinKbnDetailList = ReadDataInitKbnSetting.ReadRaiinKbnDetail();
        tenant.RaiinKbnDetails.AddRange(raiinKbnDetailList);

        // RaiinKbnInf
        var raiinKbnInflList = ReadDataInitKbnSetting.ReadRaiinKbnInf();
        tenant.RaiinKbnInfs.AddRange(raiinKbnInflList);
        #endregion
        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);

        try
        {
            tenant.SaveChanges();

            // Act
            long ptId = 123456789;
            long raiinNo = 999999999;
            int sindate = 22221212;
            var resultQuery = raiinKubunMstRepository.GetRaiinKbns(1, ptId, raiinNo, sindate);


            Assert.True(CompareListRaiinKubunMst(ptId, raiinNo, sindate, resultQuery, raiinKbnMstList, raiinKbnDetailList, raiinKbnInflList));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbnMsts.RemoveRange(raiinKbnMstList);
            tenant.RaiinKbnDetails.RemoveRange(raiinKbnDetailList);
            tenant.RaiinKbnInfs.RemoveRange(raiinKbnInflList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_002_GetRaiinKouiKbns_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnKoui
        var raiinKbnKouiList = ReadDataInitKbnSetting.ReadRaiinKbnKoui();
        tenant.RaiinKbnKouis.AddRange(raiinKbnKouiList);

        // KouiKbnMst
        var kouiKbnMstlList = ReadDataInitKbnSetting.ReadKouiKbnMst();
        tenant.KouiKbnMsts.AddRange(kouiKbnMstlList);
        #endregion

        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var resultQuery = raiinKubunMstRepository.GetRaiinKouiKbns(1);

            // Assert

            Assert.True(CompareListRaiinKouiKbn(resultQuery, raiinKbnKouiList, kouiKbnMstlList));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbnKouis.RemoveRange(raiinKbnKouiList);
            tenant.KouiKbnMsts.RemoveRange(kouiKbnMstlList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_003_GetRaiinKbnItems_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbItem
        var raiinKbnItemList = ReadDataInitKbnSetting.ReadRaiinKbnItem();
        tenant.RaiinKbItems.AddRange(raiinKbnItemList);
        #endregion

        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var resultQuery = raiinKubunMstRepository.GetRaiinKbnItems(1);

            // Assert

            Assert.True(CompareListRaiinKbnItem(resultQuery, raiinKbnItemList));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbItems.RemoveRange(raiinKbnItemList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_004_GetNextOdrInfModels_TestSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        long rsvkrtNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int sinKouiKbn = random.Next(999, 99999);
        string itemCd = "ItemCdUT";

        RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            RsvDate = sinDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
        };
        tenant.RsvkrtOdrInfs.Add(rsvkrtOdrInf);

        RsvkrtOdrInfDetail rsvkrtOdrInfDetail = new RsvkrtOdrInfDetail()
        {
            HpId = hpId,
            PtId = ptId,
            RsvDate = sinDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            SinKouiKbn = sinKouiKbn,
            ItemCd = itemCd
        };
        tenant.RsvkrtOdrInfDetails.Add(rsvkrtOdrInfDetail);


        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            // Act

            var resultQuery = nextOrderRepository.GetNextOdrInfModels(hpId, ptId, sinDate);

            // Assert
            var success = resultQuery.Any(item => item.sinKouiKbn == sinKouiKbn
                                                  && item.rsvDate == sinDate
                                                  && item.itemCd == itemCd);
            Assert.True(success);
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
            tenant.RsvkrtOdrInfDetails.Remove(rsvkrtOdrInfDetail);
            tenant.SaveChanges();
            #endregion
        }
    }

    #region InitDefaultByNextOrder
    [Test]
    public void TC_005_InitDefaultByNextOrder_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnMst
        var raiinKbnMstList = ReadDataInitKbnSetting.ReadRaiinKbnMst();
        tenant.RaiinKbnMsts.AddRange(raiinKbnMstList);

        // RaiinKbnDetail
        var raiinKbnDetailList = ReadDataInitKbnSetting.ReadRaiinKbnDetail();
        tenant.RaiinKbnDetails.AddRange(raiinKbnDetailList);

        // RaiinKbnInf
        var raiinKbnInflList = ReadDataInitKbnSetting.ReadRaiinKbnInf();
        tenant.RaiinKbnInfs.AddRange(raiinKbnInflList);

        // RaiinKbnKoui
        var raiinKbnKouiList = ReadDataInitKbnSetting.ReadRaiinKbnKoui();
        tenant.RaiinKbnKouis.AddRange(raiinKbnKouiList);

        // KouiKbnMst
        var kouiKbnMstlList = ReadDataInitKbnSetting.ReadKouiKbnMst();
        tenant.KouiKbnMsts.AddRange(kouiKbnMstlList);

        // RaiinKbItem
        var raiinKbnItemList = ReadDataInitKbnSetting.ReadRaiinKbnItem();
        tenant.RaiinKbItems.AddRange(raiinKbnItemList);
        #endregion

        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            // Act
            int hpId = 1;
            long ptId = 123456789;
            long raiinNo = 999999999;
            int sinDate = 22221212;

            var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
            var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
            var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

            var resultQuery = nextOrderRepository.InitDefaultByNextOrder(hpId, ptId, sinDate, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds);

            // Assert
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbItems.RemoveRange(raiinKbnItemList);
            tenant.RaiinKbnKouis.RemoveRange(raiinKbnKouiList);
            tenant.KouiKbnMsts.RemoveRange(kouiKbnMstlList);
            tenant.RaiinKbnMsts.RemoveRange(raiinKbnMstList);
            tenant.RaiinKbnDetails.RemoveRange(raiinKbnDetailList);
            tenant.RaiinKbnInfs.RemoveRange(raiinKbnInflList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_006_InitDefaultByNextOrder_TestChangeKbnCd()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        long rsvkrtNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int sinKouiKbn = random.Next(999, 99999);
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int kbnCd = 0;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 2;
        int isAutoDelete = 1;
        int isExclude = 2;

        RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            RsvDate = sinDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
        };
        tenant.RsvkrtOdrInfs.Add(rsvkrtOdrInf);

        RsvkrtOdrInfDetail rsvkrtOdrInfDetail = new RsvkrtOdrInfDetail()
        {
            HpId = hpId,
            PtId = ptId,
            RsvDate = sinDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            SinKouiKbn = sinKouiKbn,
            ItemCd = itemCd
        };
        tenant.RsvkrtOdrInfDetails.Add(rsvkrtOdrInfDetail);
        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };

        var raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            var resultQuery = nextOrderRepository.InitDefaultByNextOrder(hpId, ptId, sinDate, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds);

            // Assert
            var success = resultQuery.Select(item => item.RaiinKbnInfModel).Any(item => item.KbnCd == kbnCd);
            Assert.True(success);
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
            tenant.RsvkrtOdrInfDetails.Remove(rsvkrtOdrInfDetail);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_007_InitDefaultByNextOrder_TestDefaultRsvDate()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        long rsvkrtNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int sinKouiKbn = random.Next(999, 99999);
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int kbnCd = 0;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd1 = "ItemCdUT1";
        string itemCd2 = "ItemCdUT2";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 2;
        int isAutoDelete = 1;
        int isExclude = 2;

        RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            RsvDate = NextOrderConst.DefaultRsvDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
        };
        tenant.RsvkrtOdrInfs.Add(rsvkrtOdrInf);

        RsvkrtOdrInfDetail rsvkrtOdrInfDetail = new RsvkrtOdrInfDetail()
        {
            HpId = hpId,
            PtId = ptId,
            RsvDate = NextOrderConst.DefaultRsvDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            SinKouiKbn = sinKouiKbn,
            ItemCd = itemCd1
        };
        tenant.RsvkrtOdrInfDetails.Add(rsvkrtOdrInfDetail);
        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd2, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };

        var raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            var resultQuery = nextOrderRepository.InitDefaultByNextOrder(hpId, ptId, sinDate, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds);

            // Assert
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
            tenant.RsvkrtOdrInfDetails.Remove(rsvkrtOdrInfDetail);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_008_InitDefaultByNextOrder_TestExcludeItemContinue()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        long rsvkrtNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int sinKouiKbn = random.Next(999, 99999);
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int kbnCd = 0;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 2;
        int isAutoDelete = 1;
        int isExclude = 1;

        RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            RsvDate = NextOrderConst.DefaultRsvDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
        };
        tenant.RsvkrtOdrInfs.Add(rsvkrtOdrInf);

        RsvkrtOdrInfDetail rsvkrtOdrInfDetail = new RsvkrtOdrInfDetail()
        {
            HpId = hpId,
            PtId = ptId,
            RsvDate = NextOrderConst.DefaultRsvDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            SinKouiKbn = sinKouiKbn,
            ItemCd = itemCd
        };
        tenant.RsvkrtOdrInfDetails.Add(rsvkrtOdrInfDetail);
        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };

        var raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            var resultQuery = nextOrderRepository.InitDefaultByNextOrder(hpId, ptId, sinDate, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds);

            // Assert
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
            tenant.RsvkrtOdrInfDetails.Remove(rsvkrtOdrInfDetail);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_009_InitDefaultByNextOrder_TestKbnCdLager0()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        long rsvkrtNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int sinKouiKbn = random.Next(999, 99999);
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int kbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 2;
        int isAutoDelete = 1;
        int isExclude = 1;

        RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            RsvDate = NextOrderConst.DefaultRsvDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
        };
        tenant.RsvkrtOdrInfs.Add(rsvkrtOdrInf);

        RsvkrtOdrInfDetail rsvkrtOdrInfDetail = new RsvkrtOdrInfDetail()
        {
            HpId = hpId,
            PtId = ptId,
            RsvDate = NextOrderConst.DefaultRsvDate,
            RsvkrtNo = rsvkrtNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            SinKouiKbn = sinKouiKbn,
            ItemCd = itemCd
        };
        tenant.RsvkrtOdrInfDetails.Add(rsvkrtOdrInfDetail);
        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };

        var raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            var resultQuery = nextOrderRepository.InitDefaultByNextOrder(hpId, ptId, sinDate, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds);

            // Assert
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
            tenant.RsvkrtOdrInfDetails.Remove(rsvkrtOdrInfDetail);
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion InitDefaultByNextOrder

    #region InitDefaultByTodayOrder
    [Test]
    public void TC_010_InitDefaultByTodayOrder_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnMst
        var raiinKbnMstList = ReadDataInitKbnSetting.ReadRaiinKbnMst();
        tenant.RaiinKbnMsts.AddRange(raiinKbnMstList);

        // RaiinKbnDetail
        var raiinKbnDetailList = ReadDataInitKbnSetting.ReadRaiinKbnDetail();
        tenant.RaiinKbnDetails.AddRange(raiinKbnDetailList);

        // RaiinKbnInf
        var raiinKbnInflList = ReadDataInitKbnSetting.ReadRaiinKbnInf();
        tenant.RaiinKbnInfs.AddRange(raiinKbnInflList);

        // RaiinKbnKoui
        var raiinKbnKouiList = ReadDataInitKbnSetting.ReadRaiinKbnKoui();
        tenant.RaiinKbnKouis.AddRange(raiinKbnKouiList);

        // KouiKbnMst
        var kouiKbnMstlList = ReadDataInitKbnSetting.ReadKouiKbnMst();
        tenant.KouiKbnMsts.AddRange(kouiKbnMstlList);

        // RaiinKbItem
        var raiinKbnItemList = ReadDataInitKbnSetting.ReadRaiinKbnItem();
        tenant.RaiinKbItems.AddRange(raiinKbnItemList);
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;

        var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
        var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
        var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

        List<OrdInfModel> orderInfItems = new() {
                    new OrdInfModel(
                        1,
                        raiinNo,
                        99,
                        99,
                        ptId,
                        sinDate,
                        1,
                        999,
                        "",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        999999999,
                        new(){
                            new OrdInfDetailModel(
                                1,
                                raiinNo,
                                99,
                                99,
                                99,
                                ptId,
                                sinDate,
                                999,
                                "613120001",
                                "",
                                0,
                                "",
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                "",
                                0,
                                "",
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                0,
                                string.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                "")
                        },
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )};

        var resultQuery = todayOdrRepository.InitDefaultByTodayOrder(raiinKbnModels, raiinKouiKbns, raiinKbnItemCds, orderInfItems);

        // Assert
        try
        {
            tenant.SaveChanges();
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbItems.RemoveRange(raiinKbnItemList);
            tenant.RaiinKbnKouis.RemoveRange(raiinKbnKouiList);
            tenant.KouiKbnMsts.RemoveRange(kouiKbnMstlList);
            tenant.RaiinKbnMsts.RemoveRange(raiinKbnMstList);
            tenant.RaiinKbnDetails.RemoveRange(raiinKbnDetailList);
            tenant.RaiinKbnInfs.RemoveRange(raiinKbnInflList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_011_InitDefaultByTodayOrder_TestFullProgress()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int rowNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int sinKouiKbn = random.Next(999, 99999);
        int kbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string itemCd2 = "ItemCdUT2";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;
        int isExclude = 1;

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd2, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };

        List<OrdInfModel> orderInfItems = new() {
                    new OrdInfModel(
                        hpId,
                        raiinNo,
                        rpNo,
                        rpEdaNo,
                        ptId,
                        sinDate,
                        1,
                        999,
                        "",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        2,// isDeleted
                        999999999,
                        new(){
                            new OrdInfDetailModel(
                                hpId,
                                raiinNo,
                                rpNo,
                                rpEdaNo,
                                rowNo,
                                ptId,
                                sinDate,
                                sinKouiKbn,
                                itemCd,
                                "",
                                0,
                                "",
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                "",
                                0,
                                "",
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                0,
                                string.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                "")
                        },
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )};

        var resultQuery = todayOdrRepository.InitDefaultByTodayOrder(raiinKbnModels, raiinKouiKbns, raiinKbnItemCds, orderInfItems);

        // Assert
        try
        {
            tenant.SaveChanges();
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_012_InitDefaultByTodayOrder_TestIncludeItemsExist()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int rowNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int sinKouiKbn = random.Next(999, 99999);
        int kbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;
        int isExclude = 2;

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };


        List<OrdInfModel> orderInfItems = new() {
                    new OrdInfModel(
                        hpId,
                        raiinNo,
                        rpNo,
                        rpEdaNo,
                        ptId,
                        sinDate,
                        1,
                        999,
                        "",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        2,// isDeleted
                        999999999,
                        new(){
                            new OrdInfDetailModel(
                                hpId,
                                raiinNo,
                                rpNo,
                                rpEdaNo,
                                rowNo,
                                ptId,
                                sinDate,
                                sinKouiKbn,
                                itemCd,
                                "",
                                0,
                                "",
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                "",
                                0,
                                "",
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                0,
                                string.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                "")
                        },
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )};

        var resultQuery = todayOdrRepository.InitDefaultByTodayOrder(raiinKbnModels, raiinKouiKbns, raiinKbnItemCds, orderInfItems);

        // Assert
        try
        {
            tenant.SaveChanges();
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_013_InitDefaultByTodayOrder_TestExcludeItemsExists()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int rowNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int sinKouiKbn = random.Next(999, 99999);
        int kbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;
        int isExclude = 1;

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };


        List<OrdInfModel> orderInfItems = new() {
                    new OrdInfModel(
                        hpId,
                        raiinNo,
                        rpNo,
                        rpEdaNo,
                        ptId,
                        sinDate,
                        1,
                        999,
                        "",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        2,// isDeleted
                        999999999,
                        new(){
                            new OrdInfDetailModel(
                                hpId,
                                raiinNo,
                                rpNo,
                                rpEdaNo,
                                rowNo,
                                ptId,
                                sinDate,
                                sinKouiKbn,
                                itemCd,
                                "",
                                0,
                                "",
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                "",
                                0,
                                "",
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                0,
                                string.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                "")
                        },
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )};

        var resultQuery = todayOdrRepository.InitDefaultByTodayOrder(raiinKbnModels, raiinKouiKbns, raiinKbnItemCds, orderInfItems);

        // Assert
        try
        {
            tenant.SaveChanges();
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_014_InitDefaultByTodayOrder_TestExcludeItemsContinue()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int rowNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int sinKouiKbn = random.Next(999, 99999);
        int kbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;
        int isExclude = 1;

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, kbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, kbnCd, seqNo, itemCd, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };


        List<OrdInfModel> orderInfItems = new() {
                    new OrdInfModel(
                        hpId,
                        raiinNo,
                        rpNo,
                        rpEdaNo,
                        ptId,
                        sinDate,
                        1,
                        999,
                        "",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,// isDeleted
                        999999999,
                        new(){
                            new OrdInfDetailModel(
                                hpId,
                                raiinNo,
                                rpNo,
                                rpEdaNo,
                                rowNo,
                                ptId,
                                sinDate,
                                sinKouiKbn,
                                itemCd,
                                "",
                                0,
                                "",
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                "",
                                0,
                                "",
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                0,
                                string.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                "")
                        },
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )};

        var resultQuery = todayOdrRepository.InitDefaultByTodayOrder(raiinKbnModels, raiinKouiKbns, raiinKbnItemCds, orderInfItems);

        // Assert
        try
        {
            tenant.SaveChanges();
            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_016_InitDefaultByTodayOrder_TestIsExistItem()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int rowNo = random.Next(999, 99999999);
        long rpNo = random.Next(999, 99999999);
        long rpEdaNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int sinKouiKbn = random.Next(999, 99999);
        int raiinKbnInfModelKbnCd = 0;
        int raiinKbnDetailKbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        int kouiKbn1 = random.Next(999, 99999);
        int kouiKbn2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;
        int isExclude = 1;

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

        RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, raiinKbnInfModelKbnCd, 0);
        RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, raiinKbnDetailKbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
        RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
        List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns = new()
        {
            new(grpId, raiinKbnDetailKbnCd, kouiKbn1, kouiKbn2)
        };
        RaiinKbnItemModel raiinKbnItemModel = new RaiinKbnItemModel(hpId, grpCd, raiinKbnInfModelKbnCd, seqNo, itemCd, isExclude, false, 0);
        var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };
        var raiinKbnItemCds = new List<RaiinKbnItemModel> { raiinKbnItemModel };

        List<OrdInfModel> orderInfItems = new() {
                    new OrdInfModel(
                        hpId,
                        raiinNo,
                        rpNo,
                        rpEdaNo,
                        ptId,
                        sinDate,
                        1,
                        kouiKbn2,
                        "",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,// isDeleted
                        999999999,
                        new(){
                            new OrdInfDetailModel(
                                hpId,
                                raiinNo,
                                rpNo,
                                rpEdaNo,
                                rowNo,
                                ptId,
                                sinDate,
                                sinKouiKbn,
                                itemCd,
                                "",
                                0,
                                "",
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                "",
                                0,
                                "",
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                0,
                                string.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                "")
                        },
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )};

        var resultQuery = todayOdrRepository.InitDefaultByTodayOrder(raiinKbnModels, raiinKouiKbns, raiinKbnItemCds, orderInfItems);

        // Assert
        try
        {
            tenant.SaveChanges();
            var success = resultQuery.Any(mst => mst.RaiinKbnDetailModels.Any(item => item.KbnCd == raiinKbnDetailKbnCd));
            Assert.True(success);
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion InitDefaultByTodayOrder

    #region InitDefaultByRsv
    [Test]
    public void TC_017_InitDefaultByRsv_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int raiinKbnInfModelKbnCd = 0;
        int raiinKbnDetailKbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;

        // RaiinKbnYayoku
        var raiinKbnYayokuList = ReadDataInitKbnSetting.ReadRaiinKbnYayoku(hpId);
        tenant.RaiinKbnYayokus.AddRange(raiinKbnYayokuList);
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();


            // Arrange
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

            RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, raiinKbnInfModelKbnCd, 0);
            RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, raiinKbnDetailKbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
            RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
            var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };

            int frameID = 12345;
            var resultQuery = raiinKubunMstRepository.InitDefaultByRsv(hpId, frameID, raiinKbnModels);

            // Assert

            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            tenant.RaiinKbnYayokus.RemoveRange(raiinKbnYayokuList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_018_InitDefaultByRsv_TestKbnCdNotEqual0()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int kbnCd = 1;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;

        // RaiinKbnYayoku
        var raiinKbnYayokuList = ReadDataInitKbnSetting.ReadRaiinKbnYayoku(hpId);
        tenant.RaiinKbnYayokus.AddRange(raiinKbnYayokuList);
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

            RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
            RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, kbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
            RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
            var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };

            int frameID = 12345;
            var resultQuery = raiinKubunMstRepository.InitDefaultByRsv(hpId, frameID, raiinKbnModels);

            // Assert

            Assert.True(CompareInitDefault(hpId, resultQuery, raiinKbnModels));
        }
        finally
        {
            #region Remove Data Fetch
            tenant.RaiinKbnYayokus.RemoveRange(raiinKbnYayokuList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_019_InitDefaultByRsv_TestKbnCdNotEqual0()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 99999999);
        int sinDate = 22221212;
        int grpId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int kbnCd = 0;
        int detailKbnCd = 2;
        int grpCd = grpId;
        int sortNo = random.Next(999, 99999);
        string kbnName = "KbnNameUT";
        string colorCd = "ColorCdUT";
        string grpName = "grpNameUT";
        int isConfirmed = 1;
        int isAuto = 1;
        int isAutoDelete = 1;

        // RaiinKbnYayoku
        var raiinKbnYayokuList = ReadDataInitKbnSetting.ReadRaiinKbnYayoku(hpId);
        var raiinKbnYayoku = raiinKbnYayokuList.FirstOrDefault();
        if (raiinKbnYayoku != null)
        {
            raiinKbnYayoku.GrpId = grpId;
            raiinKbnYayoku.KbnCd = detailKbnCd;
            tenant.RaiinKbnYayokus.Add(raiinKbnYayoku);
        }
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Arrange
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

            RaiinKbnInfModel raiinKbnInfModel = new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, grpId, seqNo, kbnCd, 0);
            RaiinKbnDetailModel raiinKbnDetailModel = new RaiinKbnDetailModel(hpId, grpCd, detailKbnCd, sortNo, kbnName, colorCd, isConfirmed, isAuto, isAutoDelete, 0);
            RaiinKbnModel raiinKbnModel = new RaiinKbnModel(hpId, grpCd, sortNo, grpName, 0, raiinKbnInfModel, new() { raiinKbnDetailModel });
            var raiinKbnModels = new List<RaiinKbnModel> { raiinKbnModel };

            int frameID = 12345;
            var resultQuery = raiinKubunMstRepository.InitDefaultByRsv(hpId, frameID, raiinKbnModels);
            var success = resultQuery.Any(mst => mst.RaiinKbnDetailModels.Any(item => item.KbnCd == detailKbnCd));
            // Assert

            Assert.True(success);
        }
        finally
        {
            #region Remove Data Fetch
            if (raiinKbnYayoku != null)
            {
                tenant.RaiinKbnYayokus.Remove(raiinKbnYayoku);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion InitDefaultByRsv

    #region private function
    private bool CompareInitDefault(int hpId, List<RaiinKbnModel> resultQuery, List<RaiinKbnModel> raiinKbnModels)
    {
        var grpCd = raiinKbnModels.FirstOrDefault()?.GrpCd ?? 0;
        var raiinKbnModel = raiinKbnModels.FirstOrDefault(item => item.GrpCd == grpCd);
        if (raiinKbnModel == null)
        {
            return false;
        }
        var resultTest = resultQuery.FirstOrDefault(item => item.GrpCd == grpCd);
        if (resultTest == null)
        {
            return false;
        }
        if (resultTest.HpId != hpId)
        {
            return false;
        }
        else if (resultTest.GrpCd != raiinKbnModel.GrpCd)
        {
            return false;
        }
        else if (resultTest.SortNo != raiinKbnModel.SortNo)
        {
            return false;
        }
        else if (resultTest.GrpName != raiinKbnModel.GrpName)
        {
            return false;
        }
        else if (resultTest.IsDeleted != raiinKbnModel.IsDeleted)
        {
            return false;
        }

        var raiinKbnInfModel = resultTest.RaiinKbnInfModel;
        if (raiinKbnInfModel == null)
        {
            return false;
        }
        else if (raiinKbnInfModel.HpId != raiinKbnModel.RaiinKbnInfModel.HpId)
        {
            return false;
        }
        else if (raiinKbnInfModel.PtId != raiinKbnModel.RaiinKbnInfModel.PtId)
        {
            return false;
        }
        else if (raiinKbnInfModel.SinDate != raiinKbnModel.RaiinKbnInfModel.SinDate)
        {
            return false;
        }
        else if (raiinKbnInfModel.RaiinNo != raiinKbnModel.RaiinKbnInfModel.RaiinNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.GrpId != raiinKbnModel.GrpCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.SeqNo != raiinKbnModel.RaiinKbnInfModel.SeqNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.KbnCd != raiinKbnModel.RaiinKbnInfModel.KbnCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.IsDelete != raiinKbnModel.RaiinKbnInfModel.IsDelete)
        {
            return false;
        }

        var raiinKbnDetailModel = resultTest.RaiinKbnDetailModels.FirstOrDefault();
        if (raiinKbnDetailModel == null)
        {
            return false;
        }
        var raiinKbnDetail = raiinKbnModel.RaiinKbnDetailModels.FirstOrDefault();
        if (raiinKbnDetail == null)
        {
            return false;
        }
        else if (raiinKbnDetailModel.HpId != raiinKbnDetail.HpId)
        {
            return false;
        }
        else if (raiinKbnDetailModel.GrpCd != raiinKbnDetail.GrpCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.KbnCd != raiinKbnDetail.KbnCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.SortNo != raiinKbnDetail.SortNo)
        {
            return false;
        }
        else if (raiinKbnDetailModel.KbnName != raiinKbnDetail.KbnName)
        {
            return false;
        }
        else if (raiinKbnDetailModel.ColorCd != raiinKbnDetail.ColorCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsConfirmed != raiinKbnDetail.IsConfirmed)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsAuto != raiinKbnDetail.IsAuto)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsAutoDelete != raiinKbnDetail.IsAutoDelete)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsDeleted != raiinKbnDetail.IsDeleted)
        {
            return false;
        }
        return true;
    }

    private bool CompareListRaiinKbnItem(List<RaiinKbnItemModel> resultQuery, List<RaiinKbItem> raiinKbnItemList)
    {
        int id = raiinKbnItemList.FirstOrDefault()?.GrpCd ?? 0;
        var result = resultQuery.FirstOrDefault(item => item.GrpCd == id);
        if (result == null)
        {
            return false;
        }
        var raiinKbnItem = raiinKbnItemList.FirstOrDefault();
        if (raiinKbnItem == null)
        {
            return false;
        }
        else if (result.HpId != raiinKbnItem.HpId)
        {
            return false;
        }
        else if (result.GrpCd != raiinKbnItem.GrpCd)
        {
            return false;
        }
        else if (result.KbnCd != raiinKbnItem.KbnCd)
        {
            return false;
        }
        else if (result.SeqNo != raiinKbnItem.SeqNo)
        {
            return false;
        }
        else if (result.ItemCd != raiinKbnItem.ItemCd)
        {
            return false;
        }
        else if (result.IsExclude != raiinKbnItem.IsExclude)
        {
            return false;
        }
        else if (result.SortNo != raiinKbnItem.SortNo)
        {
            return false;
        }
        return true;
    }

    private bool CompareListRaiinKubunMst(long ptId, long raiinNo, int sinDate, List<RaiinKbnModel> resultQuery, List<RaiinKbnMst> raiinKbnMstList, List<RaiinKbnDetail> raiinKbnDetailList, List<RaiinKbnInf> raiinKbnInflList)
    {
        int id = raiinKbnMstList.FirstOrDefault()?.GrpCd ?? 0;
        var result = resultQuery.FirstOrDefault(item => item.GrpCd == id);
        if (result == null)
        {
            return false;
        }
        var raiinKbnMst = raiinKbnMstList.FirstOrDefault();
        var raiinKbnDetail = raiinKbnDetailList.FirstOrDefault();
        var raiinKbnInf = raiinKbnInflList.FirstOrDefault();
        if (raiinKbnMst == null || raiinKbnDetail == null || raiinKbnInf == null)
        {
            return false;
        }
        if (result.HpId != 1)
        {
            return false;
        }
        else if (result.GrpCd != raiinKbnMst.GrpCd)
        {
            return false;
        }
        else if (result.SortNo != raiinKbnMst.SortNo)
        {
            return false;
        }
        else if (result.GrpName != raiinKbnMst.GrpName)
        {
            return false;
        }
        else if (result.IsDeleted != raiinKbnMst.IsDeleted)
        {
            return false;
        }

        var raiinKbnInfModel = result.RaiinKbnInfModel;
        if (raiinKbnInfModel == null)
        {
            return false;
        }
        else if (raiinKbnInfModel.HpId != raiinKbnInf.HpId)
        {
            return false;
        }
        else if (raiinKbnInfModel.PtId != ptId)
        {
            return false;
        }
        else if (raiinKbnInfModel.SinDate != sinDate)
        {
            return false;
        }
        else if (raiinKbnInfModel.RaiinNo != raiinNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.GrpId != raiinKbnMst.GrpCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.SeqNo != raiinKbnInf.SeqNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.KbnCd != raiinKbnInf.KbnCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.IsDelete != raiinKbnInf.IsDelete)
        {
            return false;
        }

        var raiinKbnDetailModel = result.RaiinKbnDetailModels.FirstOrDefault();
        if (raiinKbnDetailModel == null)
        {
            return false;
        }
        else if (raiinKbnDetailModel.HpId != raiinKbnDetail.HpId)
        {
            return false;
        }
        else if (raiinKbnDetailModel.GrpCd != raiinKbnDetail.GrpCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.KbnCd != raiinKbnDetail.KbnCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.SortNo != raiinKbnDetail.SortNo)
        {
            return false;
        }
        else if (raiinKbnDetailModel.KbnName != raiinKbnDetail.KbnName)
        {
            return false;
        }
        else if (raiinKbnDetailModel.ColorCd != raiinKbnDetail.ColorCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsConfirmed != raiinKbnDetail.IsConfirmed)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsAuto != raiinKbnDetail.IsAuto)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsAutoDelete != raiinKbnDetail.IsAutoDelete)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsDeleted != raiinKbnDetail.IsDeleted)
        {
            return false;
        }
        return true;
    }

    private bool CompareListRaiinKouiKbn(List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> resultQuery, List<RaiinKbnKoui> raiinKbnKouiList, List<KouiKbnMst> kouiKbnMstlList)
    {
        int grpId = raiinKbnKouiList.FirstOrDefault()?.GrpId ?? 0;
        int kbnCd = raiinKbnKouiList.FirstOrDefault()?.KbnCd ?? 0;
        int kouiKbn1 = kouiKbnMstlList.FirstOrDefault()?.KouiKbn1 ?? 0;
        int kouiKbn2 = kouiKbnMstlList.FirstOrDefault()?.KouiKbn2 ?? 0;
        var result = resultQuery.FirstOrDefault(item => item.grpId == grpId
                                                        && item.kbnCd == kbnCd
                                                        && item.kouiKbn1 == kouiKbn1
                                                        && item.kouiKbn2 == kouiKbn2);
        if (result.grpId != grpId)
        {
            return false;
        }

        return true;
    }
    #endregion
}
