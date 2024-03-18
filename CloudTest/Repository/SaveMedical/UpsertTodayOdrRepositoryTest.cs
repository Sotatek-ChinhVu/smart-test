using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models.ChartApproval;
using Domain.Models.KarteInfs;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Constants;
using Helper.Enum;
using Infrastructure.Repositories;
using Moq;
using System.Text;

namespace CloudUnitTest.Repository.SaveMedical;

public class UpsertTodayOdrRepositoryTest : BaseUT
{
    #region UpsertOdrInfs
    [Test]
    public void TC_001_UpsertTodayOdr_TestCreateOdrInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_002_UpsertTodayOdr_TestUpdateOdrInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_003_UpsertTodayOdr_TestDeleteOdrInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_004_UpsertTodayOdr_TestDeleteOdrInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_005_UpsertTodayOdr_TestRaiinStateReservationOdrInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }
    #endregion UpsertOdrInfs

    #region GetMaxRpNo
    [Test]
    public void TC_006_UpsertTodayOdr_TestGetMaxRpNoSuccess()
    {
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
            Assert.True(result == ordInfModel.RpNo);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_007_UpsertTodayOdr_TestGetMaxRpNoEqual0Success()
    {
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
            Assert.True(result == 0);
        }
        finally
        {
            todayOdrRepository.ReleaseResource();
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }
    #endregion GetMaxRpNo

    #region UpsertKarteInfs
    [Test]
    public void TC_008_UpsertTodayOdr_TestCreateKarteInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_009_UpsertTodayOdr_TestUpdateKarteInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_010_UpsertTodayOdr_TestUpdateKarteInfWithRaiinStateReservationSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_011_UpsertTodayOdr_TestDeletedKarteInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }

    [Test]
    public void TC_012_UpsertTodayOdr_TestUpdateKarteImgInfSuccess()
    {
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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }
    #endregion UpsertKarteInfs

    #region SaveRaiinInf
    [Test]
    public void TC_013_UpsertTodayOdr_TestSaveRaiinInfSuccess()
    {
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
               sinStartTime = "202202",
               sinEndTime = "202202";
        byte modeSaveData = DeleteTypes.None;
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
            bool result = todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, new() { }, new KarteInfModel(0, 0), userId, DeleteTypes.None);

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
            ReleaseSource(hpId, raiinNo, ptId);
        }
    }
    #endregion SaveRaiinInf

    #region GetModeSaveDate
    [Test]
    public void TC_014_UpsertTodayOdr_TestGetModeSaveDate_001()
    {
        byte modeSaveData = (byte)ModeSaveData.TempSave;
        int status = RaiinState.Reservation;
        string sinEndTime = "202202",
               sinStartTime = "202201",
               uketukeTime = "202203";


    }
    #endregion GetModeSaveDate



    private void ReleaseSource(int hpId, long raiinNo, long ptId)
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
        tenant.SaveChanges();
        #endregion
    }
}
