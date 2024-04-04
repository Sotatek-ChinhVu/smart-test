using Domain.Models.ChartApproval;
using Domain.Models.KarteInfs;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemConf;
using Domain.Models.User;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Redis;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using StackExchange.Redis;
using System.Text;
using static Helper.Constants.UserConst;

namespace CloudUnitTest.Repository.SaveMedical;

public class UpsertTodayOdrRepositoryTest : BaseUT
{
    private readonly IDatabase _cache;

    public UpsertTodayOdrRepositoryTest()
    {
        string connection = string.Concat("10.2.15.78", ":", "6379");
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    #region UpsertOdrInfs
    [Test]
    public void TC_001_UpsertTodayOdr_TestCreateOdrInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              0, // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 random.Next(999, 99999),
                 "ItemCdUT",
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);

        OdrInfDetail? odrInfDetail = null;
        OdrInf? odrInf = null;

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            // Assert
            odrInf = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                           && item.PtId == ordInfModel.PtId
                                                           && item.SinDate == ordInfModel.SinDate
                                                           && item.RaiinNo == ordInfModel.RaiinNo
                                                           && item.RpEdaNo == 1
                                                           && item.HokenPid == ordInfModel.HokenPid
                                                           && item.OdrKouiKbn == ordInfModel.OdrKouiKbn
                                                           && item.RpName == ordInfModel.RpName
                                                           && item.InoutKbn == ordInfModel.InoutKbn
                                                           && item.SikyuKbn == ordInfModel.SikyuKbn
                                                           && item.SyohoSbt == ordInfModel.SyohoSbt
                                                           && item.SanteiKbn == ordInfModel.SanteiKbn
                                                           && item.TosekiKbn == ordInfModel.TosekiKbn
                                                           && item.DaysCnt == ordInfModel.DaysCnt
                                                           && item.SortNo == ordInfModel.SortNo
                                                           && item.IsDeleted == 0);

            odrInfDetail = tenant.OdrInfDetails.FirstOrDefault(item =>
                                                item.HpId == ordInfDetailModel.HpId
                                                && item.PtId == ordInfDetailModel.PtId
                                                && item.SinDate == ordInfDetailModel.SinDate
                                                && item.RaiinNo == ordInfDetailModel.RaiinNo
                                                && item.RpNo == (odrInf != null ? odrInf.RpNo : 0)
                                                && item.RpEdaNo == 1
                                                && item.RowNo == ordInfDetailModel.RowNo
                                                && item.SinKouiKbn == ordInfDetailModel.SinKouiKbn
                                                && item.ItemCd == ordInfDetailModel.ItemCd
                                                && item.ItemName == ordInfDetailModel.ItemName
                                                && item.Suryo == ordInfDetailModel.Suryo
                                                && item.UnitName == ordInfDetailModel.UnitName
                                                && item.UnitSBT == ordInfDetailModel.UnitSbt
                                                && item.TermVal == ordInfDetailModel.TermVal
                                                && item.KohatuKbn == ordInfDetailModel.KohatuKbn
                                                && item.SyohoKbn == ordInfDetailModel.SyohoKbn
                                                && item.SyohoLimitKbn == ordInfDetailModel.SyohoLimitKbn
                                                && item.DrugKbn == ordInfDetailModel.DrugKbn
                                                && item.YohoKbn == ordInfDetailModel.YohoKbn
                                                && item.Kokuji1 == ordInfDetailModel.Kokuji1
                                                && item.Kokiji2 == ordInfDetailModel.Kokuji2
                                                && item.IsNodspRece == ordInfDetailModel.IsNodspRece
                                                && item.IpnCd == ordInfDetailModel.IpnCd
                                                && item.IpnName == ordInfDetailModel.IpnName
                                                && item.JissiKbn == ordInfDetailModel.JissiKbn
                                                && item.JissiDate == (ordInfDetailModel.JissiDate == DateTime.MinValue ? null : DateTime.SpecifyKind(ordInfDetailModel.JissiDate, DateTimeKind.Utc))
                                                && item.JissiId == ordInfDetailModel.JissiId
                                                && item.JissiMachine == ordInfDetailModel.JissiMachine
                                                && item.ReqCd == ordInfDetailModel.ReqCd
                                                && item.Bunkatu == ordInfDetailModel.Bunkatu
                                                && item.CmtName == ordInfDetailModel.CmtName
                                                && item.CmtOpt == ordInfDetailModel.CmtOpt
                                                && item.FontColor == ordInfDetailModel.FontColor
                                                && item.CommentNewline == ordInfDetailModel.CommentNewline);

            result = result && odrInf != null && odrInfDetail != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_002_UpsertTodayOdr_TestUpdateOdrInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 random.Next(999, 99999),
                 "ItemCdUT",
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);

        OdrInfDetail? odrInfDetail = null;

        OdrInf? odrInf = new()
        {
            HpId = ordInfModel.HpId,
            PtId = ordInfModel.PtId,
            RaiinNo = ordInfModel.RaiinNo,
            RpNo = ordInfModel.RpNo,
            RpEdaNo = ordInfModel.RpEdaNo,
            Id = ordInfModel.Id,
        };

        tenant.OdrInfs.Add(odrInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            // Assert
            var odrInfAfter = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                            && item.PtId == ordInfModel.PtId
                                                            && item.SinDate == ordInfModel.SinDate
                                                            && item.RaiinNo == ordInfModel.RaiinNo
                                                            && item.RpNo == ordInfModel.RpNo
                                                            && item.RpEdaNo == ordInfModel.RpEdaNo + 1
                                                            && item.HokenPid == ordInfModel.HokenPid
                                                            && item.OdrKouiKbn == ordInfModel.OdrKouiKbn
                                                            && item.RpName == ordInfModel.RpName
                                                            && item.InoutKbn == ordInfModel.InoutKbn
                                                            && item.SikyuKbn == ordInfModel.SikyuKbn
                                                            && item.SyohoSbt == ordInfModel.SyohoSbt
                                                            && item.SanteiKbn == ordInfModel.SanteiKbn
                                                            && item.TosekiKbn == ordInfModel.TosekiKbn
                                                            && item.DaysCnt == ordInfModel.DaysCnt
                                                            && item.SortNo == ordInfModel.SortNo
                                                            && item.Id == ordInfModel.Id
                                                            && item.IsDeleted == 0);

            odrInfDetail = tenant.OdrInfDetails.FirstOrDefault(item =>
                                                item.HpId == ordInfDetailModel.HpId
                                                && item.PtId == ordInfDetailModel.PtId
                                                && item.SinDate == ordInfDetailModel.SinDate
                                                && item.RaiinNo == ordInfDetailModel.RaiinNo
                                                && item.RpNo == (odrInfAfter != null ? odrInfAfter.RpNo : 0)
                                                && item.RpEdaNo == (odrInfAfter != null ? odrInfAfter.RpEdaNo : 0)
                                                && item.RowNo == ordInfDetailModel.RowNo
                                                && item.SinKouiKbn == ordInfDetailModel.SinKouiKbn
                                                && item.ItemCd == ordInfDetailModel.ItemCd
                                                && item.ItemName == ordInfDetailModel.ItemName
                                                && item.Suryo == ordInfDetailModel.Suryo
                                                && item.UnitName == ordInfDetailModel.UnitName
                                                && item.UnitSBT == ordInfDetailModel.UnitSbt
                                                && item.TermVal == ordInfDetailModel.TermVal
                                                && item.KohatuKbn == ordInfDetailModel.KohatuKbn
                                                && item.SyohoKbn == ordInfDetailModel.SyohoKbn
                                                && item.SyohoLimitKbn == ordInfDetailModel.SyohoLimitKbn
                                                && item.DrugKbn == ordInfDetailModel.DrugKbn
                                                && item.YohoKbn == ordInfDetailModel.YohoKbn
                                                && item.Kokuji1 == ordInfDetailModel.Kokuji1
                                                && item.Kokiji2 == ordInfDetailModel.Kokuji2
                                                && item.IsNodspRece == ordInfDetailModel.IsNodspRece
                                                && item.IpnCd == ordInfDetailModel.IpnCd
                                                && item.IpnName == ordInfDetailModel.IpnName
                                                && item.JissiKbn == ordInfDetailModel.JissiKbn
                                                && item.JissiDate == (ordInfDetailModel.JissiDate == DateTime.MinValue ? null : DateTime.SpecifyKind(ordInfDetailModel.JissiDate, DateTimeKind.Utc))
                                                && item.JissiId == ordInfDetailModel.JissiId
                                                && item.JissiMachine == ordInfDetailModel.JissiMachine
                                                && item.ReqCd == ordInfDetailModel.ReqCd
                                                && item.Bunkatu == ordInfDetailModel.Bunkatu
                                                && item.CmtName == ordInfDetailModel.CmtName
                                                && item.CmtOpt == ordInfDetailModel.CmtOpt
                                                && item.FontColor == ordInfDetailModel.FontColor
                                                && item.CommentNewline == ordInfDetailModel.CommentNewline);

            result = result && odrInfAfter != null && odrInfDetail != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_003_UpsertTodayOdr_TestDeleteOdrInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.Deleted, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        OdrInf? odrInf = new()
        {
            HpId = ordInfModel.HpId,
            PtId = ordInfModel.PtId,
            RaiinNo = ordInfModel.RaiinNo,
            RpNo = ordInfModel.RpNo,
            RpEdaNo = ordInfModel.RpEdaNo,
            Id = ordInfModel.Id,
        };

        tenant.OdrInfs.Add(odrInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.Deleted);

            // Assert
            var odrInfAfter = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                            && item.PtId == ordInfModel.PtId
                                                            && item.RaiinNo == ordInfModel.RaiinNo
                                                            && item.RpNo == ordInfModel.RpNo
                                                            && item.RpEdaNo == ordInfModel.RpEdaNo
                                                            && item.Id == ordInfModel.Id
                                                            && item.IsDeleted == DeleteTypes.Deleted);

            result = result && odrInfAfter != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_004_UpsertTodayOdr_TestDeleteOdrInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.Confirm, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        OdrInf? odrInf = new()
        {
            HpId = ordInfModel.HpId,
            PtId = ordInfModel.PtId,
            RaiinNo = ordInfModel.RaiinNo,
            RpNo = ordInfModel.RpNo,
            RpEdaNo = ordInfModel.RpEdaNo,
            Id = ordInfModel.Id,
        };

        tenant.OdrInfs.Add(odrInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.Confirm);

            // Assert
            var odrInfAfter = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                            && item.PtId == ordInfModel.PtId
                                                            && item.RaiinNo == ordInfModel.RaiinNo
                                                            && item.RpNo == ordInfModel.RpNo
                                                            && item.RpEdaNo == ordInfModel.RpEdaNo
                                                            && item.Id == ordInfModel.Id
                                                            && item.IsDeleted == DeleteTypes.Confirm);

            result = result && odrInfAfter != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_005_UpsertTodayOdr_TestRaiinStateReservationOdrInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.Deleted, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        OdrInf? odrInf = new()
        {
            HpId = ordInfModel.HpId,
            PtId = ordInfModel.PtId,
            RaiinNo = ordInfModel.RaiinNo,
            RpNo = ordInfModel.RpNo,
            RpEdaNo = ordInfModel.RpEdaNo,
            Id = ordInfModel.Id,
        };

        tenant.OdrInfs.Add(odrInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, RaiinState.Reservation);

            // Assert
            var odrInfAfter = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                            && item.PtId == ordInfModel.PtId
                                                            && item.RaiinNo == ordInfModel.RaiinNo
                                                            && item.RpNo == ordInfModel.RpNo
                                                            && item.RpEdaNo == ordInfModel.RpEdaNo
                                                            && item.Id == ordInfModel.Id
                                                            && item.IsDeleted == DeleteTypes.Confirm);

            result = result && odrInfAfter != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }
    #endregion UpsertOdrInfs

    #region GetMaxRpNo
    [Test]
    public void TC_006_UpsertTodayOdr_TestGetMaxRpNoSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        OdrInf? odrInf = new()
        {
            HpId = ordInfModel.HpId,
            PtId = ordInfModel.PtId,
            RaiinNo = ordInfModel.RaiinNo,
            RpNo = ordInfModel.RpNo,
            RpEdaNo = ordInfModel.RpEdaNo,
            SinDate = ordInfModel.SinDate,
            Id = ordInfModel.Id,
        };

        tenant.OdrInfs.Add(odrInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = todayOdrRepository.GetMaxRpNo(odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.SinDate);

            // Assert
            Assert.True(result == ordInfModel.RpNo);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, 0);
        }
    }

    [Test]
    public void TC_007_UpsertTodayOdr_TestGetMaxRpNoEqual0Success()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        OdrInf? odrInf = new()
        {
            HpId = ordInfModel.HpId,
            PtId = ordInfModel.PtId,
            RaiinNo = ordInfModel.RaiinNo,
            RpNo = ordInfModel.RpNo,
            RpEdaNo = ordInfModel.RpEdaNo,
            SinDate = ordInfModel.SinDate,
            Id = ordInfModel.Id,
        };

        tenant.OdrInfs.Add(odrInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = todayOdrRepository.GetMaxRpNo(odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, random.Next(999, 99999));

            // Assert
            Assert.True(result == 0);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, 0);
        }
    }
    #endregion GetMaxRpNo

    #region UpsertKarteInfs
    [Test]
    public void TC_008_UpsertTodayOdr_TestCreateKarteInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        KarteInfModel karteModel = new KarteInfModel(
                                       hpId,
                                       raiinNo,
                                       1,
                                       0, // seqNo
                                       ptId,
                                       sinDate,
                                       "TextKarteInf",
                                       0,// isDeleted
                                       "RichTextKarteInf",
                                       DateTime.MinValue,
                                       DateTime.MinValue,
                                       "");

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, karteModel, userId, DeleteTypes.None);

            var karteInfAfter = tenant.KarteInfs.FirstOrDefault(item => item.HpId == karteModel.HpId
                                                                        && item.PtId == karteModel.PtId
                                                                        && item.SinDate == karteModel.SinDate
                                                                        && item.RaiinNo == karteModel.RaiinNo
                                                                        && item.KarteKbn == 1
                                                                        && item.SeqNo == 1
                                                                        && item.Text == karteModel.Text
                                                                        && item.RichText == Encoding.UTF8.GetBytes(karteModel.RichText)
                                                                        && item.IsDeleted == 0);
            result = result && karteInfAfter != null;

            // Assert
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_009_UpsertTodayOdr_TestUpdateKarteInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        KarteInfModel karteModel = new KarteInfModel(
                                       hpId,
                                       raiinNo,
                                       1,
                                       random.Next(999, 99999), // seqNo
                                       ptId,
                                       sinDate,
                                       "TextKarteInf",
                                       0,// isDeleted
                                       "RichTextKarteInf",
                                       DateTime.MinValue,
                                       DateTime.MinValue,
                                       "");

        KarteInf karteInf = new KarteInf()
        {
            HpId = karteModel.HpId,
            RaiinNo = karteModel.RaiinNo,
            PtId = karteModel.PtId,
            KarteKbn = 1,
            SeqNo = karteModel.SeqNo,
        };

        tenant.Add(karteInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, karteModel, userId, DeleteTypes.None);

            var karteInfAfter = tenant.KarteInfs.FirstOrDefault(item => item.HpId == karteModel.HpId
                                                                        && item.PtId == karteModel.PtId
                                                                        && item.SinDate == karteModel.SinDate
                                                                        && item.RaiinNo == karteModel.RaiinNo
                                                                        && item.KarteKbn == 1
                                                                        && item.SeqNo == karteModel.SeqNo + 1
                                                                        && item.Text == karteModel.Text
                                                                        && item.RichText == Encoding.UTF8.GetBytes(karteModel.RichText)
                                                                        && item.IsDeleted == 0);
            result = result && karteInfAfter != null;

            // Assert
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_010_UpsertTodayOdr_TestUpdateKarteInfWithRaiinStateReservationSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        KarteInfModel karteModel = new KarteInfModel(
                                       hpId,
                                       raiinNo,
                                       1,
                                       random.Next(999, 99999), // seqNo
                                       ptId,
                                       sinDate,
                                       "TextKarteInf",
                                       0,// isDeleted
                                       "RichTextKarteInf",
                                       DateTime.MinValue,
                                       DateTime.MinValue,
                                       "");

        KarteInf karteInf = new KarteInf()
        {
            HpId = karteModel.HpId,
            RaiinNo = karteModel.RaiinNo,
            PtId = karteModel.PtId,
            KarteKbn = 1,
            SeqNo = karteModel.SeqNo,
        };

        tenant.Add(karteInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, karteModel, userId, RaiinState.Reservation);

            var karteInfAfter = tenant.KarteInfs.FirstOrDefault(item => item.HpId == karteModel.HpId
                                                                        && item.PtId == karteModel.PtId
                                                                        && item.SinDate == karteModel.SinDate
                                                                        && item.RaiinNo == karteModel.RaiinNo
                                                                        && item.KarteKbn == 1
                                                                        && item.SeqNo == karteModel.SeqNo + 1
                                                                        && item.Text == karteModel.Text
                                                                        && item.RichText == Encoding.UTF8.GetBytes(karteModel.RichText)
                                                                        && item.IsDeleted == 0);

            var karteInfOldVersion = tenant.KarteInfs.FirstOrDefault(item => item.HpId == karteModel.HpId
                                                                        && item.PtId == karteModel.PtId
                                                                        && item.RaiinNo == karteModel.RaiinNo
                                                                        && item.KarteKbn == 1
                                                                        && item.SeqNo == karteModel.SeqNo
                                                                        && item.IsDeleted == DeleteTypes.Confirm);
            result = result && karteInfAfter != null && karteInfOldVersion != null;

            // Assert
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_011_UpsertTodayOdr_TestDeletedKarteInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        KarteInfModel karteModel = new KarteInfModel(
                                       hpId,
                                       raiinNo,
                                       1,
                                       random.Next(999, 99999), // seqNo
                                       ptId,
                                       sinDate,
                                       "TextKarteInf",
                                       1,// isDeleted
                                       "RichTextKarteInf",
                                       DateTime.MinValue,
                                       DateTime.MinValue,
                                       "");

        KarteInf karteInf = new KarteInf()
        {
            HpId = karteModel.HpId,
            RaiinNo = karteModel.RaiinNo,
            PtId = karteModel.PtId,
            KarteKbn = 1,
            SeqNo = karteModel.SeqNo,
        };
        tenant.Add(karteInf);

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, karteModel, userId, DeleteTypes.None);

            var karteInfAfter = tenant.KarteInfs.FirstOrDefault(item => item.HpId == karteModel.HpId
                                                                        && item.PtId == karteModel.PtId
                                                                        && item.RaiinNo == karteModel.RaiinNo
                                                                        && item.KarteKbn == 1
                                                                        && item.SeqNo == karteModel.SeqNo
                                                                        && item.IsDeleted == 1);
            result = result && karteInfAfter != null;

            // Assert
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_012_UpsertTodayOdr_TestUpdateKarteImgInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        KarteInfModel karteModel = new KarteInfModel(
                                       hpId,
                                       raiinNo,
                                       1,
                                       random.Next(999, 99999), // seqNo
                                       ptId,
                                       sinDate,
                                       "TextKarteInf",
                                       0,// isDeleted
                                       "RichTextKarteInfImgLinkUT",
                                       DateTime.MinValue,
                                       DateTime.MinValue,
                                       "");

        KarteInf karteInf = new KarteInf()
        {
            HpId = karteModel.HpId,
            RaiinNo = karteModel.RaiinNo,
            PtId = karteModel.PtId,
            KarteKbn = 1,
            SeqNo = karteModel.SeqNo,
        };
        KarteImgInf karteImgInf = new KarteImgInf()
        {
            HpId = karteModel.HpId,
            PtId = karteModel.PtId,
            FileName = "ImgLinkUT",
        };

        tenant.Add(karteInf);
        tenant.Add(karteImgInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, karteModel, userId, DeleteTypes.None);

            var karteInfAfter = tenant.KarteInfs.FirstOrDefault(item => item.HpId == karteModel.HpId
                                                                        && item.PtId == karteModel.PtId
                                                                        && item.SinDate == karteModel.SinDate
                                                                        && item.RaiinNo == karteModel.RaiinNo
                                                                        && item.KarteKbn == 1
                                                                        && item.SeqNo == karteModel.SeqNo + 1
                                                                        && item.Text == karteModel.Text
                                                                        && item.RichText == Encoding.UTF8.GetBytes(karteModel.RichText)
                                                                        && item.IsDeleted == 0);

            var karteImgInfAfter = tenant.KarteImgInfs.FirstOrDefault(item => item.HpId == karteModel.HpId
                                                                              && item.PtId == karteModel.PtId
                                                                              && item.FileName == karteImgInf.FileName
                                                                              && item.RaiinNo == karteModel.RaiinNo);

            result = result && karteInfAfter != null;

            // Assert
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }
    #endregion UpsertKarteInfs

    #region SaveRaiinInf
    [Test]
    public void TC_013_UpsertTodayOdr_TestSaveRaiinInfSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "202202",
               sinStartTime = "202201",
               sinEndTime = "202203";
        byte modeSaveData = (byte)ModeSaveData.KeisanSave;
        RaiinInf raiinInf = new RaiinInf()
        {
            HpId = hpId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            PtId = ptId
        };

        tenant.RaiinInfs.Add(raiinInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, new KarteInfModel(0, 0), userId, modeSaveData);

            var preProcess = todayOdrRepository.GetModeSaveDate(modeSaveData, raiinInf.Status, sinEndTime, sinStartTime, uketukeTime);
            var preProcessStatus = preProcess.status != 0 ? preProcess.status : raiinInf.Status;
            var raiinInfAfter = tenant.RaiinInfs.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.Status == ((raiinInf.Status <= RaiinState.TempSave && modeSaveData == 0) ? RaiinState.TempSave : preProcessStatus)
                                                                        && item.SyosaisinKbn == syosaiKbn
                                                                        && item.JikanKbn == jikanKbn
                                                                        && item.HokenPid == hokenPid
                                                                        && item.SanteiKbn == santeiKbn
                                                                        && item.TantoId == tantoId
                                                                        && item.KaId == kaId
                                                                        && item.SinEndTime == sinEndTime
                                                                        && item.SinStartTime == sinStartTime
                                                                        && item.UketukeTime == (string.IsNullOrEmpty(preProcess.uketukeTime) ? raiinInf.UketukeTime : preProcess.uketukeTime)
                                                                        && item.IsDeleted == 0);

            if (raiinInfAfter == null)
            {
                Assert.True(false);
            }
            if (string.IsNullOrEmpty(raiinInfAfter!.SinEndTime) && modeSaveData != 0)
            {
                result = raiinInfAfter.SinEndTime == sinEndTime;
            }
            if (string.IsNullOrEmpty(raiinInfAfter.SinStartTime))
            {
                result = raiinInfAfter.SinStartTime == sinStartTime;
            }
            result = result && raiinInfAfter != null;

            // Assert
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }
    #endregion SaveRaiinInf

    #region GetModeSaveDate
    [Test]
    public void TC_014_UpsertTodayOdr_TestGetModeSaveDate_001()
    {
        // Arrange
        byte modeSaveData = (byte)ModeSaveData.KeisanSave;
        int status = RaiinState.TempSave;
        string sinEndTime = "",
               sinStartTime = "202202",
               uketukeTime = "202203";

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        var result = todayOdrRepository.GetModeSaveDate(modeSaveData, status, sinEndTime, sinStartTime, uketukeTime);

        // Act
        bool success = result.status == RaiinState.Calculate
            && result.sinStartTime == ""
            && int.Parse(result.sinEndTime) <= int.Parse(CIUtil.DateTimeToTime(CIUtil.GetJapanDateTimeNow()))
            && result.uketukeTime == "";

        // Assert
        Assert.True(success);
    }

    [Test]
    public void TC_015_UpsertTodayOdr_TestGetModeSaveDate_002()
    {
        // Arrange
        byte modeSaveData = (byte)ModeSaveData.KeisanSave;
        int status = RaiinState.Reservation;
        string sinEndTime = "",
               sinStartTime = "202202",
               uketukeTime = "";

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        var result = todayOdrRepository.GetModeSaveDate(modeSaveData, status, sinEndTime, sinStartTime, uketukeTime);

        // Act
        bool success = result.status == RaiinState.Calculate
            && result.sinStartTime == sinStartTime
            && int.Parse(result.sinEndTime) <= int.Parse(CIUtil.DateTimeToTime(CIUtil.GetJapanDateTimeNow()))
            && result.uketukeTime == sinStartTime;

        // Assert
        Assert.True(success);
    }

    [Test]
    public void TC_016_UpsertTodayOdr_TestGetModeSaveDate_003()
    {
        // Arrange
        byte modeSaveData = (byte)ModeSaveData.KaikeiSave;
        int status = RaiinState.Calculate;
        string sinEndTime = "",
               sinStartTime = "202202",
               uketukeTime = "";

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        var result = todayOdrRepository.GetModeSaveDate(modeSaveData, status, sinEndTime, sinStartTime, uketukeTime);

        // Act
        bool success = result.status == RaiinState.Waiting
            && result.sinStartTime == ""
            && int.Parse(result.sinEndTime) <= int.Parse(CIUtil.DateTimeToTime(CIUtil.GetJapanDateTimeNow()))
            && result.uketukeTime == "";

        // Assert
        Assert.True(success);
    }

    [Test]
    public void TC_017_UpsertTodayOdr_TestGetModeSaveDate_004()
    {
        // Arrange
        byte modeSaveData = (byte)ModeSaveData.KaikeiSave;
        int status = RaiinState.Settled;
        string sinEndTime = "",
               sinStartTime = "202202",
               uketukeTime = "";

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

        // Act
        var result = todayOdrRepository.GetModeSaveDate(modeSaveData, status, sinEndTime, sinStartTime, uketukeTime);

        bool success = result.status == 0
            && result.sinStartTime == ""
            && result.sinEndTime == ""
            && result.uketukeTime == "";

        // Assert
        Assert.True(success);
    }

    [Test]
    public void TC_018_UpsertTodayOdr_TestGetModeSaveDate_005()
    {
        // Arrange
        byte modeSaveData = (byte)ModeSaveData.KaikeiSave;
        int status = RaiinState.Reservation;
        string sinEndTime = "",
               sinStartTime = "202202",
               uketukeTime = "";

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

        // Act
        var result = todayOdrRepository.GetModeSaveDate(modeSaveData, status, sinEndTime, sinStartTime, uketukeTime);

        bool success = result.status == RaiinState.Waiting
            && result.sinStartTime == sinStartTime
            && int.Parse(result.sinEndTime) <= int.Parse(CIUtil.DateTimeToTime(CIUtil.GetJapanDateTimeNow()))
            && result.uketukeTime == sinStartTime;

        // Assert
        Assert.True(success);
    }
    #endregion GetModeSaveDate

    #region SaveHeaderInf
    [Test]
    public void TC_019_UpsertTodayOdr_TestSaveHeaderInfSuccess_01()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220201",
               sinStartTime = "20220202",
               sinEndTime = "20220203";
        int jikanRow = 2;
        int shinRow = 1;
        string jikanItemCd = "@JIKAN";
        string shinItemCd = "@SHIN";

        var rpNo = random.NextInt64(99999, 999999999);
        var rpEdaNo = random.NextInt64(99999, 999999999);

        OdrInf? odrInf = new OdrInf()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            OdrKouiKbn = 10,
            HokenPid = hokenPid,
            SanteiKbn = santeiKbn,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            IsDeleted = 1
        };

        OdrInfDetail? oldSyosaiKihon = new OdrInfDetail()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            ItemCd = ItemCdConst.SyosaiKihon,
            RpNo = rpNo,
            Suryo = syosaiKbn,
            RpEdaNo = rpEdaNo,
            RowNo = shinRow
        };

        OdrInfDetail? oldJikanKihon = new OdrInfDetail()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            ItemCd = ItemCdConst.JikanKihon,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            Suryo = jikanKbn,
            RowNo = jikanRow
        };

        tenant.OdrInfs.Add(odrInf);
        tenant.OdrInfDetails.Add(oldSyosaiKihon);
        tenant.OdrInfDetails.Add(oldJikanKihon);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new(), new KarteInfModel(0, 0), userId, DeleteTypes.None);

            // Assert
            var odrInfAfter = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                                 && item.PtId == ptId
                                                                 && item.RaiinNo == raiinNo
                                                                 && item.SinDate == sinDate
                                                                 && item.OdrKouiKbn == 10
                                                                 && item.HokenPid == hokenPid
                                                                 && item.SanteiKbn == santeiKbn
                                                                 && item.RpNo == rpNo
                                                                 && item.RpEdaNo == rpEdaNo
                                                                 && item.IsDeleted == 0);

            var oldSyosaiKihonAfter = tenant.OdrInfDetails.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.RaiinNo == raiinNo
                                                                                  && item.RpNo == rpNo
                                                                                  && item.RpEdaNo == rpEdaNo
                                                                                  && item.RowNo == shinRow
                                                                                  && item.PtId == ptId
                                                                                  && item.SinDate == sinDate
                                                                                  && item.ItemCd == shinItemCd
                                                                                  && item.Suryo == syosaiKbn);

            var oldJikanKihonAfter = tenant.OdrInfDetails.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.RaiinNo == raiinNo
                                                                                  && item.RpNo == rpNo
                                                                                  && item.RpEdaNo == rpEdaNo
                                                                                  && item.RowNo == jikanRow
                                                                                  && item.PtId == ptId
                                                                                  && item.SinDate == sinDate
                                                                                  && item.ItemCd == jikanItemCd
                                                                                  && item.Suryo == jikanKbn);

            result = result && odrInfAfter != null && oldSyosaiKihonAfter != null && oldJikanKihonAfter != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_020_UpsertTodayOdr_TestSaveHeaderInfSuccess_02()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220201",
               sinStartTime = "20220202",
               sinEndTime = "20220203";
        int jikanRow = 2;
        int shinRow = 1;
        int daysCntDefalt = 1;
        int headerOdrKouiKbn = 10;
        string jikanItemCd = "@JIKAN";
        string shinItemCd = "@SHIN";
        string shinItemName = "診察料基本点数算定用";
        string jikanItemName = "時間外算定用";

        var rpNo = random.NextInt64(99999, 999999999);
        var rpEdaNo = random.NextInt64(99999, 999999999);

        OdrInf? odrInf = new OdrInf()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            OdrKouiKbn = 10,
            HokenPid = hokenPid,
            SanteiKbn = santeiKbn,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            IsDeleted = 1
        };

        tenant.OdrInfs.Add(odrInf);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new(), new KarteInfModel(0, 0), userId, DeleteTypes.None);

            // Assert
            var odrInfAfter = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                                 && item.PtId == ptId
                                                                 && item.RaiinNo == raiinNo
                                                                 && item.SinDate == sinDate
                                                                 && item.OdrKouiKbn == headerOdrKouiKbn
                                                                 && item.HokenPid == hokenPid
                                                                 && item.RpNo == rpNo
                                                                 && item.RpEdaNo == rpEdaNo + 1
                                                                 && item.DaysCnt == daysCntDefalt
                                                                 && item.IsDeleted == 0);

            if (odrInfAfter == null)
            {
                Assert.True(false);
            }

            var oldSyosaiKihonAfter = tenant.OdrInfDetails.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.RaiinNo == raiinNo
                                                                                  && item.RpNo == odrInfAfter!.RpNo
                                                                                  && item.RpEdaNo == odrInfAfter.RpEdaNo
                                                                                  && item.RowNo == shinRow
                                                                                  && item.PtId == ptId
                                                                                  && item.SinDate == sinDate
                                                                                  && item.SinKouiKbn == headerOdrKouiKbn
                                                                                  && item.ItemCd == shinItemCd
                                                                                  && item.ItemName == shinItemName
                                                                                  && item.Suryo == syosaiKbn);

            var oldJikanKihonAfter = tenant.OdrInfDetails.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.RaiinNo == raiinNo
                                                                                  && item.RpNo == odrInfAfter!.RpNo
                                                                                  && item.RpEdaNo == odrInfAfter.RpEdaNo
                                                                                  && item.RowNo == jikanRow
                                                                                  && item.PtId == ptId
                                                                                  && item.SinDate == sinDate
                                                                                  && item.SinKouiKbn == headerOdrKouiKbn
                                                                                  && item.ItemCd == jikanItemCd
                                                                                  && item.ItemName == jikanItemName
                                                                                  && item.Suryo == jikanKbn);

            result = result && odrInfAfter != null && oldSyosaiKihonAfter != null && oldJikanKihonAfter != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_021_UpsertTodayOdr_TestSaveHeaderInfSuccess_03()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220201",
               sinStartTime = "20220202",
               sinEndTime = "20220203";
        int jikanRow = 2;
        int shinRow = 1;
        int daysCntDefalt = 1;
        int headerOdrKouiKbn = 10;
        string jikanItemCd = "@JIKAN";
        string shinItemCd = "@SHIN";
        string shinItemName = "診察料基本点数算定用";
        string jikanItemName = "時間外算定用";
        int rpEdaNoDefault = 1;
        int rpNoDefault = 1;

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new(), new KarteInfModel(0, 0), userId, DeleteTypes.None);

            // Assert
            var odrInfAfter = tenant.OdrInfs.FirstOrDefault(item => item.HpId == hpId
                                                                 && item.PtId == ptId
                                                                 && item.RaiinNo == raiinNo
                                                                 && item.SinDate == sinDate
                                                                 && item.OdrKouiKbn == headerOdrKouiKbn
                                                                 && item.HokenPid == hokenPid
                                                                 && item.RpNo == rpNoDefault
                                                                 && item.RpEdaNo == rpEdaNoDefault
                                                                 && item.DaysCnt == daysCntDefalt
                                                                 && item.IsDeleted == 0);

            if (odrInfAfter == null)
            {
                Assert.True(false);
            }

            var oldSyosaiKihonAfter = tenant.OdrInfDetails.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.RaiinNo == raiinNo
                                                                                  && item.RpNo == odrInfAfter!.RpNo
                                                                                  && item.RpEdaNo == odrInfAfter.RpEdaNo
                                                                                  && item.RowNo == shinRow
                                                                                  && item.PtId == ptId
                                                                                  && item.SinDate == sinDate
                                                                                  && item.SinKouiKbn == headerOdrKouiKbn
                                                                                  && item.ItemCd == shinItemCd
                                                                                  && item.ItemName == shinItemName
                                                                                  && item.Suryo == syosaiKbn);

            var oldJikanKihonAfter = tenant.OdrInfDetails.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.RaiinNo == raiinNo
                                                                                  && item.RpNo == odrInfAfter!.RpNo
                                                                                  && item.RpEdaNo == odrInfAfter.RpEdaNo
                                                                                  && item.RowNo == jikanRow
                                                                                  && item.PtId == ptId
                                                                                  && item.SinDate == sinDate
                                                                                  && item.SinKouiKbn == headerOdrKouiKbn
                                                                                  && item.ItemCd == jikanItemCd
                                                                                  && item.ItemName == jikanItemName
                                                                                  && item.Suryo == jikanKbn);

            result = result && odrInfAfter != null && oldSyosaiKihonAfter != null && oldJikanKihonAfter != null;
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    #endregion SaveHeaderInf

    #region Test User jobCd = 1
    [Test]
    public void TC_022_UpsertTodayOdr_TestUserJobCdIs1()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220201",
               sinStartTime = "20220202",
               sinEndTime = "20220203";

        UserMst userMst = new UserMst()
        {
            HpId = hpId,
            UserId = userId,
            StartDate = 0,
            EndDate = 99999999,
            JobCd = 1
        };

        tenant.UserMsts.Add(userMst);
        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new(), new KarteInfModel(0, 0), userId, DeleteTypes.None);

            // Assert
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }
    #endregion

    #region UpdateApproveInf
    [Test]
    public void TC_023_UpsertTodayOdr_TestUpdateApproveInf_01()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);

        var mockUserRepository = new Mock<IUserRepository>();
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, mockUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            approvalinfRepository.UpdateApproveInf(hpId, ptId, sinDate, raiinNo, userId);

            var approveInf = tenant.ApprovalInfs.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.SinDate == sinDate
                                                                        && item.RaiinNo == raiinNo
                                                                        && item.IsDeleted == 0
                                                                        && item.SeqNo == 1);
            // Assert
            Assert.True(approveInf != null);
        }
        finally
        {
            approvalinfRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_024_UpsertTodayOdr_TestUpdateApproveInf_02()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);

        ApprovalInf approvalInf = new ApprovalInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            SeqNo = seqNo
        };

        tenant.ApprovalInfs.Add(approvalInf);
        var mockUserRepository = new Mock<IUserRepository>();
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, mockUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            approvalinfRepository.UpdateApproveInf(hpId, ptId, sinDate, raiinNo, userId);

            var approveInf = tenant.ApprovalInfs.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.SinDate == sinDate
                                                                        && item.RaiinNo == raiinNo
                                                                        && item.IsDeleted == 0
                                                                        && item.SeqNo == seqNo
                                                                        && item.UpdateId == userId);
            // Assert
            Assert.True(approveInf != null);
        }
        finally
        {
            approvalinfRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_025_UpsertTodayOdr_TestUpdateApproveInf_03()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);

        ApprovalInf approvalInf = new ApprovalInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            SeqNo = seqNo,
            IsDeleted = 1
        };

        tenant.ApprovalInfs.Add(approvalInf);
        var mockUserRepository = new Mock<IUserRepository>();
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, mockUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            approvalinfRepository.UpdateApproveInf(hpId, ptId, sinDate, raiinNo, userId);

            var approveInf = tenant.ApprovalInfs.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.SinDate == sinDate
                                                                        && item.RaiinNo == raiinNo
                                                                        && item.IsDeleted == 0
                                                                        && item.SeqNo == seqNo + 1
                                                                        && item.UpdateId == userId);
            // Assert
            Assert.True(approveInf != null);
        }
        finally
        {
            approvalinfRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_026_UpsertTodayOdr_TestUpdateApproveInf_04()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);

        ApprovalInf approvalInf = new ApprovalInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            SeqNo = seqNo,
            IsDeleted = 0
        };

        tenant.ApprovalInfs.Add(approvalInf);
        var mockUserRepository = new Mock<IUserRepository>();
        ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, mockUserRepository.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            mockUserRepository.Setup(finder => finder.GetPermissionByScreenCode(hpId, userId, FunctionCode.ApprovalInfo))
                              .Returns((int hpId, int userId, string permisionCode) => PermissionType.ReadOnly);
            approvalinfRepository.UpdateApproveInf(hpId, ptId, sinDate, raiinNo, userId);

            var approveInf = tenant.ApprovalInfs.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.SinDate == sinDate
                                                                        && item.RaiinNo == raiinNo
                                                                        && item.IsDeleted == 1
                                                                        && item.SeqNo == seqNo
                                                                        && item.UpdateId == userId);
            // Assert
            Assert.True(approveInf != null);
        }
        finally
        {
            approvalinfRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }
    #endregion UpdateApproveInf

    #region GetPermissionByScreenCode
    [Test]
    public void TC_027_UpsertTodayOdr_TestGetPermissionByScreenCode_01()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        long raiinNo = random.NextInt64(99999, 999999999);
        var permissionCode = "";

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var mockUserService = new Mock<IUserInfoService>();
        string finalKey = "" + CacheKeyConstant.UserInfoCacheService;
        _cache.KeyDelete(finalKey);
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object); try
        {
            tenant.SaveChanges();

            // Act
            var result = userRepository.GetPermissionByScreenCode(hpId, userId, permissionCode);

            // Assert
            Assert.True(result == PermissionType.NotAvailable);
        }
        finally
        {
            userRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_028_UpsertTodayOdr_TestGetPermissionByScreenCode_02()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        long raiinNo = random.NextInt64(99999, 999999999);
        var permissionCode = FunctionCode.ApprovalInfo;

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        string finalKey = "" + CacheKeyConstant.UserInfoCacheService;
        _cache.KeyDelete(finalKey);
        var mockUserService = new Mock<IUserInfoService>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        UserPermission userPermission = new UserPermission()
        {
            HpId = hpId,
            UserId = userId,
            FunctionCd = permissionCode,
            CreateId = userId
        };
        tenant.UserPermissions.Add(userPermission);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = userRepository.GetPermissionByScreenCode(hpId, userId, permissionCode);

            // Assert
            Assert.True(result == PermissionType.Unlimited);
        }
        finally
        {
            userRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_029_UpsertTodayOdr_TestGetPermissionByScreenCode_03()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        long raiinNo = random.NextInt64(99999, 999999999);
        var permissionCode = FunctionCode.ApprovalInfo;

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        string finalKey = "" + CacheKeyConstant.UserInfoCacheService;
        _cache.KeyDelete(finalKey);
        var mockUserService = new Mock<IUserInfoService>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        UserPermission userPermission = new UserPermission()
        {
            HpId = hpId,
            UserId = 0,
            FunctionCd = permissionCode,
            CreateId = userId
        };
        tenant.UserPermissions.Add(userPermission);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = userRepository.GetPermissionByScreenCode(hpId, userId, permissionCode);

            // Assert
            Assert.True(result == PermissionType.Unlimited);
        }
        finally
        {
            userRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_030_UpsertTodayOdr_TestGetPermissionByScreenCode_04()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        long raiinNo = random.NextInt64(99999, 999999999);
        var permissionCode = FunctionCode.ApprovalInfo;

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        string finalKey = "" + CacheKeyConstant.UserInfoCacheService;
        _cache.KeyDelete(finalKey);
        var mockUserService = new Mock<IUserInfoService>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = userRepository.GetPermissionByScreenCode(hpId, userId, permissionCode);

            // Assert
            Assert.True(result == PermissionType.NotAvailable);
        }
        finally
        {
            userRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_031_UpsertTodayOdr_TestGetPermissionByScreenCode_05()
    {
        // Arrange
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        long raiinNo = random.NextInt64(99999, 999999999);
        var permissionCode = FunctionCode.ApprovalInfo;

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        string finalKey = "" + CacheKeyConstant.UserInfoCacheService;
        _cache.KeyDelete(finalKey);
        var mockUserService = new Mock<IUserInfoService>();
        SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);

        UserMst userMst = new UserMst()
        {
            HpId = hpId,
            UserId = userId,
            JobCd = 1,
            CreateId = userId,
        };
        tenant.UserMsts.Add(userMst);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = userRepository.GetPermissionByScreenCode(hpId, userId, permissionCode);

            // Assert
            Assert.True(result == PermissionType.Unlimited);
        }
        finally
        {
            userRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }
    #endregion GetPermissionByScreenCode

    #region SaveRaiinListInf
    [Test]
    public void TC_032_UpsertTodayOdr_TestSaveRaiinListInfSuccess_01()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";

        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            // Assert
            var raiinListInfAfter = tenant.RaiinListInfs.Where(item => item.HpId == hpId
                                                                       && item.PtId == ptId
                                                                       && item.RaiinNo == raiinNo
                                                                       && item.SinDate == sinDate
                                                                       && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN)
                                                        .ToList();

            result = result && !raiinListInfAfter.Any();
            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId);
        }
    }

    [Test]
    public void TC_033_UpsertTodayOdr_TestSaveRaiinListInfSuccess_02()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";
        int grpId = random.Next(999, 99999);
        int kbnCd = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        int sinKouiKbn = random.Next(999, 99999);
        int kouiKbnId = random.Next(999, 99999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);

        RaiinListInf raiinListInfKouiKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.KOUI_KBN
        };
        RaiinListInf raiinListInfItemKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.ITEM_KBN
        };
        tenant.RaiinListInfs.Add(raiinListInfKouiKbn);
        tenant.RaiinListInfs.Add(raiinListInfItemKbn);

        KouiKbnMst kouiKbnMst = new KouiKbnMst()
        {
            KouiKbnId = kouiKbnId,
            KouiKbn1 = sinKouiKbn,
        };
        tenant.KouiKbnMsts.Add(kouiKbnMst);

        RaiinListItem raiinListItem = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd,
            KbnCd = kbnCd,
            IsExclude = 1
        };
        tenant.RaiinListItems.Add(raiinListItem);

        RaiinListKoui raiinListKoui = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd
        };
        tenant.RaiinListKouis.Add(raiinListKoui);

        RaiinListMst raiinListMst = new RaiinListMst()
        {
            HpId = hpId,
            GrpId = grpId
        };
        tenant.RaiinListMsts.Add(raiinListMst);

        RaiinListDetail raiinListDetail = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd
        };
        tenant.RaiinListDetails.Add(raiinListDetail);


        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            var raiinListInfAfter = tenant.RaiinListInfs.Where(item => item.HpId == hpId
                                                                       && item.PtId == ptId
                                                                       && item.SinDate == sinDate
                                                                       && item.RaiinNo == raiinNo
                                                                       && item.GrpId == grpId
                                                                       && item.KbnCd == kbnCd
                                                                       && (item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN
                                                                           || item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN))
                                                        .ToList();
            // Assert
            result = result && !raiinListInfAfter.Any();

            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId, kouiKbnId, sinKouiKbn, grpId);
        }
    }

    [Test]
    public void TC_034_UpsertTodayOdr_TestSaveRaiinListInfSuccess_03()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";
        int grpId = random.Next(999, 99999);
        int kbnCd = random.Next(999, 99999);
        int kbnCd2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string itemCd2 = "ItemCdUT2";
        int sinKouiKbn = random.Next(999, 99999);
        int kouiKbnId = random.Next(999, 99999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        var ordInfDetailModel2 = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 ordInfDetailModel.RowNo + 1,
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd2,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);
        ordInfModel.OrdInfDetails.Add(ordInfDetailModel2);

        RaiinListInf raiinListInfKouiKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.KOUI_KBN
        };
        RaiinListInf raiinListInfItemKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.ITEM_KBN
        };
        tenant.RaiinListInfs.Add(raiinListInfKouiKbn);
        tenant.RaiinListInfs.Add(raiinListInfItemKbn);

        KouiKbnMst kouiKbnMst = new KouiKbnMst()
        {
            KouiKbnId = kouiKbnId,
            KouiKbn1 = sinKouiKbn,
        };
        tenant.KouiKbnMsts.Add(kouiKbnMst);

        RaiinListItem raiinListItem = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd,
            KbnCd = kbnCd,
        };
        RaiinListItem raiinListItem2 = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd2,
            KbnCd = kbnCd2,
        };
        tenant.RaiinListItems.Add(raiinListItem);
        tenant.RaiinListItems.Add(raiinListItem2);

        RaiinListKoui raiinListKoui = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd,
        };
        RaiinListKoui raiinListKoui2 = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd2
        };
        tenant.RaiinListKouis.Add(raiinListKoui);
        tenant.RaiinListKouis.Add(raiinListKoui2);

        RaiinListMst raiinListMst = new RaiinListMst()
        {
            HpId = hpId,
            GrpId = grpId
        };
        tenant.RaiinListMsts.Add(raiinListMst);

        RaiinListDetail raiinListDetail = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd,
            SortNo = 2
        };
        RaiinListDetail raiinListDetail2 = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd2,
            SortNo = 1
        };
        tenant.RaiinListDetails.Add(raiinListDetail);
        tenant.RaiinListDetails.Add(raiinListDetail2);


        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            var raiinListInfKouiKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd2
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN);

            var raiinListInfItemKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd2
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN);

            // Assert
            result = result && raiinListInfKouiKbnAfter != null && raiinListInfItemKbnAfter != null;

            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId, kouiKbnId, sinKouiKbn, grpId);
        }
    }

    [Test]
    public void TC_035_UpsertTodayOdr_TestSaveRaiinListInfSuccess_04()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";
        int grpId = random.Next(999, 99999);
        int kbnCd = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string itemCd2 = "ItemCdUT2";
        int sinKouiKbn = random.Next(999, 99999);
        int kouiKbnId = random.Next(999, 99999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        var ordInfDetailModel2 = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 ordInfDetailModel.RowNo + 1,
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd2,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);
        ordInfModel.OrdInfDetails.Add(ordInfDetailModel2);

        KouiKbnMst kouiKbnMst = new KouiKbnMst()
        {
            KouiKbnId = kouiKbnId,
            KouiKbn1 = sinKouiKbn,
        };
        tenant.KouiKbnMsts.Add(kouiKbnMst);

        RaiinListItem raiinListItem = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd,
            KbnCd = kbnCd,
        };
        tenant.RaiinListItems.Add(raiinListItem);

        RaiinListKoui raiinListKoui = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd,
        };
        tenant.RaiinListKouis.Add(raiinListKoui);

        RaiinListMst raiinListMst = new RaiinListMst()
        {
            HpId = hpId,
            GrpId = grpId
        };
        tenant.RaiinListMsts.Add(raiinListMst);

        RaiinListDetail raiinListDetail = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd,
        };
        tenant.RaiinListDetails.Add(raiinListDetail);


        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            var raiinListInfKouiKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN);

            var raiinListInfItemKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN);

            // Assert
            result = result && raiinListInfKouiKbnAfter != null && raiinListInfItemKbnAfter != null;

            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId, kouiKbnId, sinKouiKbn, grpId);
        }
    }

    [Test]
    public void TC_036_UpsertTodayOdr_TestSaveRaiinListInfSuccess_05()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";
        int grpId = random.Next(999, 99999);
        int kbnCd = random.Next(999, 9999);
        int kbnCd2 = random.Next(9999, 99999);
        string itemCd = "ItemCdUT";
        string itemCd2 = "ItemCdUT2";
        int sinKouiKbn = random.Next(999, 99999);
        int kouiKbnId = random.Next(999, 99999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        var ordInfDetailModel2 = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 ordInfDetailModel.RowNo + 1,
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd2,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);
        ordInfModel.OrdInfDetails.Add(ordInfDetailModel2);

        KouiKbnMst kouiKbnMst = new KouiKbnMst()
        {
            KouiKbnId = kouiKbnId,
            KouiKbn1 = sinKouiKbn,
        };
        tenant.KouiKbnMsts.Add(kouiKbnMst);

        RaiinListItem raiinListItem = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd,
            KbnCd = kbnCd,
        };
        RaiinListItem raiinListItem2 = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd2,
            KbnCd = kbnCd2,
        };
        tenant.RaiinListItems.Add(raiinListItem);
        tenant.RaiinListItems.Add(raiinListItem2);

        RaiinListKoui raiinListKoui = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd,
        };
        tenant.RaiinListKouis.Add(raiinListKoui);

        RaiinListMst raiinListMst = new RaiinListMst()
        {
            HpId = hpId,
            GrpId = grpId
        };
        tenant.RaiinListMsts.Add(raiinListMst);

        RaiinListDetail raiinListDetail = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd,
            SortNo = 2
        };
        RaiinListDetail raiinListDetail2 = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd2,
            SortNo = 1
        };
        tenant.RaiinListDetails.Add(raiinListDetail);
        tenant.RaiinListDetails.Add(raiinListDetail2);


        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            var raiinListInfKouiKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN);

            var raiinListInfItemKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN);

            // Assert
            result = result && raiinListInfKouiKbnAfter != null && raiinListInfItemKbnAfter != null;

            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId, kouiKbnId, sinKouiKbn, grpId);
        }
    }

    [Test]
    public void TC_037_UpsertTodayOdr_TestSaveRaiinListInfSuccess_06()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";
        int grpId = random.Next(999, 99999);
        int kbnCd = random.Next(999, 99999);
        int kbnCd2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string itemCd2 = "ItemCdUT2";
        int sinKouiKbn = random.Next(999, 99999);
        int kouiKbnId = random.Next(999, 99999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        var ordInfDetailModel2 = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 ordInfDetailModel.RowNo + 1,
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd2,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        var ordInfDetailModel3 = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 ordInfDetailModel.RowNo + 2,
                 ptId,
                 sinDate,
                 0,
                 itemCd2,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);
        ordInfModel.OrdInfDetails.Add(ordInfDetailModel2);
        ordInfModel.OrdInfDetails.Add(ordInfDetailModel3);

        RaiinListInf raiinListInfKouiKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.KOUI_KBN
        };
        RaiinListInf raiinListInfItemKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.ITEM_KBN
        };
        tenant.RaiinListInfs.Add(raiinListInfKouiKbn);
        tenant.RaiinListInfs.Add(raiinListInfItemKbn);

        RaiinListItem raiinListItem = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd,
            KbnCd = kbnCd,
            IsExclude = 1
        };
        RaiinListItem raiinListItem2 = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd2,
            KbnCd = kbnCd2,
            IsExclude = 1
        };
        RaiinListItem raiinListItem3 = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd2,
            KbnCd = kbnCd2,
        };
        tenant.RaiinListItems.Add(raiinListItem);
        tenant.RaiinListItems.Add(raiinListItem2);
        tenant.RaiinListItems.Add(raiinListItem3);

        RaiinListKoui raiinListKoui = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd,
        };
        RaiinListKoui raiinListKoui2 = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd2
        };
        tenant.RaiinListKouis.Add(raiinListKoui);
        tenant.RaiinListKouis.Add(raiinListKoui2);

        RaiinListMst raiinListMst = new RaiinListMst()
        {
            HpId = hpId,
            GrpId = grpId
        };
        tenant.RaiinListMsts.Add(raiinListMst);

        RaiinListDetail raiinListDetail = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd,
            SortNo = 2
        };
        RaiinListDetail raiinListDetail2 = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd2,
            SortNo = 1
        };
        tenant.RaiinListDetails.Add(raiinListDetail);
        tenant.RaiinListDetails.Add(raiinListDetail2);


        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            var raiinListInfKouiKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN);

            var raiinListInfItemKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN);

            // Assert
            result = result && raiinListInfKouiKbnAfter != null && raiinListInfItemKbnAfter == null;

            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId, kouiKbnId, sinKouiKbn, grpId);
        }
    }

    [Test]
    public void TC_038_UpsertTodayOdr_TestSaveRaiinListInfSuccess_07()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;
        long raiinNo = random.NextInt64(99999, 999999999);
        int syosaiKbn = random.Next(999, 99999),
            jikanKbn = random.Next(999, 99999),
            hokenPid = random.Next(999, 99999),
            santeiKbn = random.Next(999, 99999),
            tantoId = random.Next(999, 99999),
            kaId = random.Next(999, 99999);
        string uketukeTime = "20220202",
               sinStartTime = "20220202",
               sinEndTime = "20220202";
        int grpId = random.Next(999, 99999);
        int kbnCd = random.Next(999, 99999);
        int kbnCd2 = random.Next(999, 99999);
        string itemCd = "ItemCdUT";
        string itemCd2 = "ItemCdUT2";
        int sinKouiKbn = random.Next(999, 99999);
        int kouiKbnId = random.Next(999, 99999);

        var ordInfModel = new OrdInfModel(
                              hpId,
                              raiinNo,
                              random.NextInt64(999, 999999999),
                              random.NextInt64(999, 999999999),
                              ptId,
                              sinDate,
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              "RpNameOrdInf",
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              random.Next(999, 99999),
                              DeleteTypes.None, // isDeleted
                              random.Next(999, 99999), // id
                              new(),
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              DateTime.MinValue,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty
                          );

        var ordInfDetailModel = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 random.Next(999, 99999),
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        var ordInfDetailModel2 = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 ordInfDetailModel.RowNo + 1,
                 ptId,
                 sinDate,
                 sinKouiKbn,
                 itemCd2,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        var ordInfDetailModel3 = new OrdInfDetailModel(
                 hpId,
                 raiinNo,
                 ordInfModel.RpNo,
                 ordInfModel.RpEdaNo,
                 ordInfDetailModel.RowNo + 2,
                 ptId,
                 sinDate,
                 0,
                 itemCd2,
                 "ItemNameOrdInfDetail",
                 random.Next(999, 99999),
                 "UnitNameUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 "Kokuji1UT",
                 "Kokuji2UT",
                 0,
                 "IpnCdUT",
                 "IpnNameUT",
                 random.Next(999, 99999),
                 DateTime.MinValue,
                 random.Next(999, 99999),
                 string.Empty,
                 "ReqCdUT",
                 "BunkatuUT",
                 "CmtNameUT",
                 "CmtOptUT",
                 "ColorUT",
                 random.Next(999, 99999),
                 "MasterSbtUT",
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 false,
                 random.Next(999, 99999),
                 random.Next(999, 99999),
                 random.Next(999, 99999),
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
                 ""
             );

        ordInfModel.OrdInfDetails.Add(ordInfDetailModel);
        ordInfModel.OrdInfDetails.Add(ordInfDetailModel2);
        ordInfModel.OrdInfDetails.Add(ordInfDetailModel3);

        RaiinListInf raiinListInfKouiKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.KOUI_KBN
        };
        RaiinListInf raiinListInfItemKbn = new RaiinListInf()
        {
            HpId = hpId,
            PtId = ptId,
            SinDate = sinDate,
            RaiinNo = raiinNo,
            GrpId = grpId,
            KbnCd = kbnCd,
            RaiinListKbn = RaiinListKbnConstants.ITEM_KBN
        };
        tenant.RaiinListInfs.Add(raiinListInfKouiKbn);
        tenant.RaiinListInfs.Add(raiinListInfItemKbn);

        RaiinListItem raiinListItem = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd,
            KbnCd = kbnCd,
            IsExclude = 1
        };
        RaiinListItem raiinListItem2 = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd2,
            KbnCd = kbnCd,
            IsExclude = 1
        };
        RaiinListItem raiinListItem3 = new RaiinListItem()
        {
            HpId = hpId,
            GrpId = grpId,
            ItemCd = itemCd2,
            KbnCd = kbnCd,
            IsExclude = 0
        };
        tenant.RaiinListItems.Add(raiinListItem);
        tenant.RaiinListItems.Add(raiinListItem2);
        tenant.RaiinListItems.Add(raiinListItem3);

        RaiinListKoui raiinListKoui = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd,
        };
        RaiinListKoui raiinListKoui2 = new RaiinListKoui()
        {
            HpId = hpId,
            GrpId = grpId,
            KouiKbnId = kouiKbnId,
            KbnCd = kbnCd2
        };
        tenant.RaiinListKouis.Add(raiinListKoui);
        tenant.RaiinListKouis.Add(raiinListKoui2);

        RaiinListMst raiinListMst = new RaiinListMst()
        {
            HpId = hpId,
            GrpId = grpId
        };
        tenant.RaiinListMsts.Add(raiinListMst);

        RaiinListDetail raiinListDetail = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd,
            SortNo = 2
        };
        RaiinListDetail raiinListDetail2 = new RaiinListDetail()
        {
            HpId = hpId,
            GrpId = grpId,
            KbnCd = kbnCd2,
            SortNo = 1
        };
        tenant.RaiinListDetails.Add(raiinListDetail);
        tenant.RaiinListDetails.Add(raiinListDetail2);


        var mockSystemConf = new Mock<ISystemConfRepository>();
        var mockapprovalInf = new Mock<IApprovalInfRepository>();
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { ordInfModel }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

            var raiinListInfKouiKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN);

            var raiinListInfItemKbnAfter = tenant.RaiinListInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                       && item.PtId == ptId
                                                                                       && item.SinDate == sinDate
                                                                                       && item.RaiinNo == raiinNo
                                                                                       && item.GrpId == grpId
                                                                                       && item.KbnCd == kbnCd
                                                                                       && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN);

            // Assert
            result = result && raiinListInfKouiKbnAfter != null && raiinListInfItemKbnAfter == null;

            Assert.True(result);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId, userId, kouiKbnId, sinKouiKbn, grpId);
        }
    }
    #endregion


    private void ReleaseSource(int hpId, long raiinNo, long ptId, int userId, int kouiKbnId, int sinKouiKbn, int grpId)
    {
        var tenantRelease = TenantProvider.GetTrackingTenantDataContext();
        var kouiKbnMsts = tenantRelease.KouiKbnMsts.Where(item => item.KouiKbnId == kouiKbnId && (item.KouiKbn1 == sinKouiKbn || item.KouiKbn2 == sinKouiKbn)).ToList();
        foreach (var item in kouiKbnMsts)
        {
            tenantRelease.KouiKbnMsts.Remove(item);
        }
        var raiinListItems = tenantRelease.RaiinListItems.Where(item => item.HpId == hpId && item.GrpId == grpId).ToList();
        foreach (var item in raiinListItems)
        {
            tenantRelease.RaiinListItems.Remove(item);
        }
        var raiinListKouis = tenantRelease.RaiinListKouis.Where(item => item.HpId == hpId && item.GrpId == grpId).ToList();
        foreach (var item in raiinListKouis)
        {
            tenantRelease.RaiinListKouis.Remove(item);
        }
        var raiinListMsts = tenantRelease.RaiinListMsts.Where(item => item.HpId == hpId && item.GrpId == grpId).ToList();
        foreach (var item in raiinListMsts)
        {
            tenantRelease.RaiinListMsts.Remove(item);
        }
        var raiinListDetails = tenantRelease.RaiinListDetails.Where(item => item.HpId == hpId && item.GrpId == grpId).ToList();
        foreach (var item in raiinListDetails)
        {
            tenantRelease.RaiinListDetails.Remove(item);
        }
        tenantRelease.SaveChanges();
        ReleaseSource(hpId, raiinNo, ptId, userId);
    }

    private void ReleaseSource(int hpId, long raiinNo, long ptId, int userId)
    {
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        #region Remove Data Fetch
        var raiinInfList = tenant.RaiinInfs.Where(item => item.HpId == hpId && item.RaiinNo == raiinNo).ToList();
        foreach (var item in raiinInfList)
        {
            tenant.RaiinInfs.Remove(item);
        }
        var orderInfList = tenant.OdrInfs.Where(item => item.HpId == hpId && item.RaiinNo == raiinNo).ToList();
        foreach (var item in orderInfList)
        {
            tenant.OdrInfs.Remove(item);
        }
        var orderInfDetailList = tenant.OdrInfDetails.Where(item => item.HpId == hpId && item.RaiinNo == raiinNo).ToList();
        foreach (var item in orderInfDetailList)
        {
            tenant.OdrInfDetails.Remove(item);
        }
        var raiinListInflList = tenant.RaiinListInfs.Where(item => item.HpId == hpId && item.RaiinNo == raiinNo).ToList();
        foreach (var item in raiinListInflList)
        {
            tenant.RaiinListInfs.Remove(item);
        }
        var karteInfList = tenant.KarteInfs.Where(item => item.HpId == hpId && item.RaiinNo == raiinNo).ToList();
        foreach (var item in karteInfList)
        {
            tenant.KarteInfs.Remove(item);
        }
        var karteImgInfList = tenant.KarteImgInfs.Where(item => item.HpId == hpId && item.PtId == ptId).ToList();
        foreach (var item in karteImgInfList)
        {
            tenant.KarteImgInfs.Remove(item);
        }
        var approvalInfList = tenant.ApprovalInfs.Where(item => item.HpId == hpId && item.PtId == ptId).ToList();
        foreach (var item in approvalInfList)
        {
            tenant.ApprovalInfs.Remove(item);
        }
        if (userId > 0)
        {
            var userMstList = tenant.UserMsts.Where(item => item.HpId == hpId && item.UserId == userId).ToList();
            foreach (var item in userMstList)
            {
                tenant.UserMsts.Remove(item);
            }
            var userPermissionList = tenant.UserPermissions.Where(item => item.HpId == hpId && (item.CreateId == userId || item.CreateId == 0)).ToList();
            foreach (var item in userPermissionList)
            {
                tenant.UserPermissions.Remove(item);
            }
        }
        tenant.SaveChanges();
        #endregion
    }
}
