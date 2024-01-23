using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.CommonMasters.Config;
using Reporting.ReceiptList.Model;
using Helper.Extension;

namespace Reporting.ReceiptList.DB;

public class CoReceiptListFinder : RepositoryBase, ICoReceiptListFinder
{
    private readonly ISystemConfig _systemConfig;

    public CoReceiptListFinder(ITenantProvider tenantProvider, ISystemConfig systemConfig) : base(tenantProvider)
    {
        _systemConfig = systemConfig;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
        _systemConfig.ReleaseResource();
    }

    public List<ReceiptListModel> AdvancedSearchReceList(int hpId, int sinym)
    {
        int fromDay = sinym * 100 + 1;
        int toDay = sinym * 100 + 31;

        List<ReceiptListModel> result;
        // Rece
        var receInfs = NoTrackingDataContext.ReceInfs.Where(item => item.SeikyuYm == sinym
                                                                    && item.HpId == hpId)
                                                     .ToList();
        var receInfEdits = NoTrackingDataContext.ReceInfEdits.Where(item => item.SeikyuYm == sinym
                                                                            && item.IsDeleted == 0
                                                                            && item.HpId == hpId)
                                                             .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm, item.SeikyuYm })
                                                             .Select(grp => grp.FirstOrDefault());
        var receStatuses = NoTrackingDataContext.ReceStatuses.Where(item => item.SeikyuYm == sinym
                                                                            && item.IsDeleted == 0
                                                                            && item.HpId == hpId);
        var receCheckCmts = NoTrackingDataContext.ReceCheckCmts.Where(item => item.IsDeleted == 0
                                                                              && item.HpId == hpId)
                                                               .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                               .Select(grp => grp.FirstOrDefault());
        var receCmts = NoTrackingDataContext.ReceCmts.Where(item => item.IsDeleted == 0
                                                                    && item.HpId == hpId)
                                                     .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                     .Select(grp => grp.FirstOrDefault());
        var receSeikyus = NoTrackingDataContext.ReceSeikyus.Where(item => item.SeikyuYm == sinym
                                                                          && item.IsDeleted == 0
                                                                          && item.HpId == hpId);
        var syoukiInfs = NoTrackingDataContext.SyoukiInfs.Where(item => item.IsDeleted == 0
                                                                        && item.HpId == hpId)
                                                         .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                         .Select(grp => grp.FirstOrDefault());
        var syobyokeikas = NoTrackingDataContext.SyobyoKeikas.Where(item => item.SinYm == sinym
                                                                            && item.IsDeleted == 0
                                                                            && item.HpId == hpId)
                                                             .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                             .Select(grp => grp.FirstOrDefault());
        // Patient
        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId);
        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId
                                                                          && item.IsDeleted == 0);
        var ptLastVisitDates = NoTrackingDataContext.PtLastVisitDates.Where(item => item.HpId == hpId);
        var ptKyuseis = NoTrackingDataContext.PtKyuseis.Where(item => item.HpId == hpId
                                                                      && item.IsDeleted == 0
                                                                      && item.EndDate / 100 >= sinym)
                                                       .GroupBy(item => new { item.HpId, item.PtId })
                                                       .Select(grp => grp.FirstOrDefault()); ;
        var ptKohis = NoTrackingDataContext.PtKohis.Where(item => item.HpId == hpId
                                                                  && item.IsDeleted == 0);

        // Master
        var kaMsts = NoTrackingDataContext.KaMsts.Where(u => u.IsDeleted == 0
                                                             && u.HpId == hpId);
        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.StartDate <= fromDay
                                                                 && toDay <= u.EndDate
                                                                 && u.JobCd == 1
                                                                 && u.IsDeleted == 0
                                                                 && u.HpId == hpId);

        var query = from receInf in receInfs
                    join receInfEdit in receInfEdits on new { receInf.HpId, receInf.SeikyuYm, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receInfEdit.HpId, receInfEdit.SeikyuYm, receInfEdit.PtId, receInfEdit.HokenId, receInfEdit.SinYm } into receInfEditLeft
                    from receInfEdit in receInfEditLeft.DefaultIfEmpty()

                    join receStatus in receStatuses on new { receInf.HpId, receInf.SeikyuYm, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receStatus.HpId, receStatus.SeikyuYm, receStatus.PtId, receStatus.HokenId, receStatus.SinYm } into receStatusLeft
                    from receStatus in receStatusLeft.DefaultIfEmpty()

                    join receCheckCmt in receCheckCmts on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receCheckCmt.HpId, receCheckCmt.PtId, receCheckCmt.HokenId, receCheckCmt.SinYm } into receCheckCmtLeft
                    from receCheckCmt in receCheckCmtLeft.DefaultIfEmpty()

                    join receCmt in receCmts on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receCmt.HpId, receCmt.PtId, receCmt.HokenId, receCmt.SinYm } into receCmtLeft
                    from receCmt in receCmtLeft.DefaultIfEmpty()

                    join receSeikyu in receSeikyus on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuLeft
                    from receSeikyu in receSeikyuLeft.DefaultIfEmpty()

                    join syoukiInf in syoukiInfs on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { syoukiInf.HpId, syoukiInf.PtId, syoukiInf.HokenId, syoukiInf.SinYm } into syoukiInfLeft
                    from syoukiInf in syoukiInfLeft.DefaultIfEmpty()

                    join syobyokeika in syobyokeikas on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { syobyokeika.HpId, syobyokeika.PtId, syobyokeika.HokenId, syobyokeika.SinYm } into syobyokeikaLeft
                    from syobyokeika in syobyokeikaLeft.DefaultIfEmpty()

                    join ptInf in ptInfs on new { receInf.HpId, receInf.PtId }
                                                equals new { ptInf.HpId, ptInf.PtId } into ptInfLeft
                    from ptInf in ptInfLeft.DefaultIfEmpty()

                    join ptHokenInf in ptHokenInfs on new { receInf.HpId, receInf.PtId, receInf.HokenId }
                                                equals new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfLeft
                    from ptHokenInf in ptHokenInfLeft.DefaultIfEmpty()

                    join ptLastVisitDate in ptLastVisitDates on new { receInf.HpId, receInf.PtId }
                                                equals new { ptLastVisitDate.HpId, ptLastVisitDate.PtId } into ptLastVisitDateInf
                    from ptLastVisitDate in ptLastVisitDateInf.DefaultIfEmpty()

                    join ptKyusei in ptKyuseis on new { receInf.HpId, receInf.PtId }
                                                equals new { ptKyusei.HpId, ptKyusei.PtId } into ptKyuseiLeft
                    from ptKyusei in ptKyuseiLeft.DefaultIfEmpty()

                    join kaMst in kaMsts on new { receInf.HpId, receInf.KaId }
                                                equals new { kaMst.HpId, kaMst.KaId } into kaMstLeft
                    from kaMst in kaMstLeft.DefaultIfEmpty()

                    join userMst in userMsts on new { receInf.HpId, receInf.TantoId }
                                                equals new { userMst.HpId, TantoId = userMst.UserId } into userMstLeft
                    from userMst in userMstLeft.DefaultIfEmpty()

                    join ptKohi1 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi1Id }
                                                equals new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1Left
                    from ptKohi1 in ptKohi1Left.DefaultIfEmpty()

                    join ptKohi2 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi2Id }
                                                equals new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2Left
                    from ptKohi2 in ptKohi2Left.DefaultIfEmpty()

                    join ptKohi3 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi3Id }
                                                equals new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3Left
                    from ptKohi3 in ptKohi3Left.DefaultIfEmpty()

                    join ptKohi4 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi4Id }
                                                equals new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4Left
                    from ptKohi4 in ptKohi4Left.DefaultIfEmpty()

                    select new
                    {
                        ReceInf = receInf,
                        IsReceInfDetailExist = receInfEdit != null ? 1 : 0,
                        IsPaperRece = receStatus != null ? receStatus.IsPaperRece : 0,
                        Output = receStatus != null ? receStatus.Output : 0,
                        FusenKbn = receStatus != null ? receStatus.FusenKbn : 0,
                        StatusKbn = receStatus != null ? receStatus.StatusKbn : 0,
                        ReceStatusCreateId = receStatus != null ? receStatus.CreateId : 0,
                        ReceCheckCmt = receCheckCmt != null ? receCheckCmt.Cmt : string.Empty,
                        ptInf,
                        ptInf.PtNum,
                        ptInf.KanaName,
                        ptInf.Name,
                        ptInf.Sex,
                        ptInf.Birthday,
                        ptHokenInf.HokensyaNo,
                        IsSyoukiInfExist = syoukiInf != null ? 1 : 0,
                        IsReceCmtExist = receCmt != null ? 1 : 0,
                        IsSyobyoKeikaExist = syobyokeika != null ? 1 : 0,
                        SeikyuCmt = receSeikyu != null ? receSeikyu.Cmt : string.Empty,
                        LastVisitDate = ptLastVisitDate != null ? ptLastVisitDate.LastVisitDate : 0,
                        kaMst.KaName,
                        userMst.Sname,
                        IsPtKyuseiExist = ptKyusei != null ? 1 : 0,
                        FutansyaNoKohi1 = ptKohi1 != null ? ptKohi1.FutansyaNo : string.Empty,
                        FutansyaNoKohi2 = ptKohi2 != null ? ptKohi2.FutansyaNo : string.Empty,
                        FutansyaNoKohi3 = ptKohi3 != null ? ptKohi3.FutansyaNo : string.Empty,
                        FutansyaNoKohi4 = ptKohi4 != null ? ptKohi4.FutansyaNo : string.Empty,
                        IsReceStatusExists = receStatus != null,
                        IsPtInfExists = ptInf != null,
                        IsPtHokenInfExists = ptHokenInf != null,
                        IsKohi1Exists = ptKohi1 != null,
                        IsKohi2Exists = ptKohi2 != null,
                        IsKohi3Exists = ptKohi3 != null,
                        IsKohi4Exists = ptKohi4 != null,
                        IsPtLastVisitDateExist = ptLastVisitDate != null,
                        IsKaMstExists = kaMst != null,
                        IsUserMstExist = userMst != null,
                        ptHokenInf.RousaiKofuNo,
                        ptHokenInf.RousaiJigyosyoName,
                        ptHokenInf.RousaiPrefName,
                        ptHokenInf.RousaiCityName,
                        ptHokenInf.JibaiHokenName,
                        ptHokenInf.JibaiHokenTanto,
                        ptHokenInf.JibaiHokenTel
                    };

        var rosaiReceden = _systemConfig.RosaiReceden();
        string rosaiRecedenTerm = _systemConfig.RosaiRecedenTerm();
        result = query.AsEnumerable().Select(
                data => new ReceiptListModel(data.ReceInf, rosaiReceden, rosaiRecedenTerm)
                {
                    IsReceInfDetailExist = data.IsReceInfDetailExist,
                    IsPaperRece = data.IsPaperRece,
                    Output = data.Output,
                    FusenKbn = data.FusenKbn,
                    StatusKbn = data.StatusKbn,
                    ReceStatusCreateId = data.ReceStatusCreateId,
                    ReceCheckCmt = data.ReceCheckCmt,
                    PtNum = data.PtNum,
                    KanaName = data.KanaName,
                    Name = data.Name,
                    Sex = data.Sex,
                    BirthDay = data.Birthday,
                    HokensyaNo = data.HokensyaNo,
                    IsSyoukiInfExist = data.IsSyoukiInfExist,
                    IsReceCmtExist = data.IsReceCmtExist,
                    IsSyobyoKeikaExist = data.IsSyobyoKeikaExist,
                    ReceSeikyuCmt = data.SeikyuCmt,
                    LastVisitDate = data.LastVisitDate,
                    KaName = data.KaName,
                    SName = data.Sname,
                    IsPtKyuseiExist = data.IsPtKyuseiExist,
                    FutansyaNoKohi1 = data.FutansyaNoKohi1,
                    FutansyaNoKohi2 = data.FutansyaNoKohi2,
                    FutansyaNoKohi3 = data.FutansyaNoKohi3,
                    FutansyaNoKohi4 = data.FutansyaNoKohi4,
                    IsPtTest = (data.ptInf != null ? data.ptInf.IsTester == 1 : false),
                    RousaiKofuNo = data.RousaiKofuNo,
                    RousaiJigyosyoName = data.RousaiJigyosyoName,
                    RousaiPrefName = data.RousaiPrefName,
                    RousaiCityName = data.RousaiCityName,
                    JibaiHokenName = data.JibaiHokenName,
                    JibaiHokenTanto = data.JibaiHokenTanto,
                    JibaiHokenTel = data.JibaiHokenTel
                })
            .ToList();
        return result;

    }

    public List<ReceiptListModel> GetDataReceReport(int hpId, int seikyuYm, List<ReceiptInputModel> receiptInputList)
    {
        if (seikyuYm == 0)
        {
            return new();
        }
        int fromDay = seikyuYm * 100 + 1;
        int toDay = seikyuYm * 100 + DateTime.DaysInMonth(seikyuYm / 100, seikyuYm % 100);

        List<ReceiptListModel> result = new();

        var sinYmList = receiptInputList.Select(item => item.SinYm).Distinct().ToList();
        var hokenIdList = receiptInputList.Select(item => item.HokenId).Distinct().ToList();
        var ptIdList = receiptInputList.Select(item => item.PtId).Distinct().ToList();

        #region Simple query 
        // Rece
        var receInfs = NoTrackingDataContext.ReceInfs.Where(item => item.SeikyuYm == seikyuYm
                                                                    && item.HpId == hpId
                                                                    && sinYmList.Contains(item.SinYm)
                                                                    && hokenIdList.Contains(item.HokenId));

        var minSinYM = receInfs.Select(item => item.SinYm).DefaultIfEmpty().Min();
        var minDay = minSinYM * 100 + 1;

        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(item => item.HpId == hpId
                                                                        && item.SinDate >= minDay
                                                                        && item.SinDate <= toDay
                                                                        && ptIdList.Contains(item.PtId))
                                                         .Select(item => new { item.PtId, item.HokenId, item.SinDate });

        var receInfEdits = NoTrackingDataContext.ReceInfEdits.Where(item => item.SeikyuYm == seikyuYm
                                                                            && item.IsDeleted == 0
                                                                            && item.HpId == hpId
                                                                            && ptIdList.Contains(item.PtId))
                                                             .AsEnumerable()
                                                             .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm, item.SeikyuYm })
                                                             .Select(grp => grp.FirstOrDefault() ?? new ReceInfEdit())
                                                             .Select(item => new { item.SinYm, item.SeikyuYm, item.HpId, item.HokenId, item.PtId });

        var receStatuses = NoTrackingDataContext.ReceStatuses.Where(item => item.SeikyuYm == seikyuYm
                                                                            && item.IsDeleted == 0
                                                                            && item.HpId == hpId
                                                                            && ptIdList.Contains(item.PtId))
                                                            .Select(item => new { item.SinYm, item.SeikyuYm, item.HpId, item.HokenId, item.PtId, item.StatusKbn, item.FusenKbn, item.IsPaperRece, item.Output });

        var receCheckCmts = NoTrackingDataContext.ReceCheckCmts.Where(item => item.IsDeleted == 0
                                                                              && item.HpId == hpId
                                                                              && item.IsChecked == 0
                                                                              && item.SinYm >= minSinYM
                                                                              && item.SinYm <= seikyuYm
                                                                              && ptIdList.Contains(item.PtId))
                                                                .AsEnumerable()
                                                                .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                                .Select(grp => grp.OrderByDescending(item => item.SortNo).FirstOrDefault() ?? new ReceCheckCmt());

        var receCheckErrors = NoTrackingDataContext.ReceCheckErrs.Where(item => item.HpId == hpId
                                                                                && item.IsChecked == 0
                                                                                && item.SinYm >= minSinYM
                                                                                && item.SinYm <= seikyuYm
                                                                                && ptIdList.Contains(item.PtId))
                                                                  .AsEnumerable()
                                                                  .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                                  .Select(grp => grp.OrderBy(item => item.ErrCd).FirstOrDefault() ?? new ReceCheckErr());

        var receCmts = NoTrackingDataContext.ReceCmts.Where(item => item.IsDeleted == 0
                                                                    && item.HpId == hpId
                                                                    && item.SinYm >= minSinYM
                                                                    && item.SinYm <= seikyuYm
                                                                    && ptIdList.Contains(item.PtId))
                                                     .AsEnumerable()
                                                     .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                     .Select(grp => grp.FirstOrDefault() ?? new ReceCmt())
                                                     .Select(item => new { item.SinYm, item.HpId, item.HokenId, item.PtId });

        var receSeikyus = NoTrackingDataContext.ReceSeikyus.Where(item => item.SeikyuYm == seikyuYm
                                                                          && item.IsDeleted == 0
                                                                          && item.HpId == hpId
                                                                          && ptIdList.Contains(item.PtId));

        var syoukiInfs = NoTrackingDataContext.SyoukiInfs.Where(item => item.IsDeleted == 0
                                                                        && item.HpId == hpId
                                                                        && item.SinYm >= minSinYM
                                                                        && item.SinYm <= seikyuYm
                                                                        && ptIdList.Contains(item.PtId))
                                                         .AsEnumerable()
                                                         .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                         .Select(grp => grp.FirstOrDefault() ?? new SyoukiInf())
                                                         .Select(item => new { item.SinYm, item.HpId, item.HokenId, item.PtId });

        var syobyokeikas = NoTrackingDataContext.SyobyoKeikas.Where(item => item.IsDeleted == 0
                                                                            && item.HpId == hpId
                                                                            && !string.IsNullOrEmpty(item.Keika)
                                                                            && item.SinYm >= minSinYM
                                                                            && item.SinYm <= seikyuYm
                                                                            && ptIdList.Contains(item.PtId)).AsEnumerable()
                                                             .GroupBy(item => new { item.HpId, item.PtId, item.HokenId, item.SinYm })
                                                             .Select(item => new { item.Key.SinYm, item.Key.HpId, item.Key.HokenId, item.Key.PtId });

        // Patient
        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && ptIdList.Contains(item.PtId));

        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId
                                                                          && item.IsDeleted == 0
                                                                          && ptIdList.Contains(item.PtId));

        var ptLastVisitDates = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == 0
                                                                             && ptIdList.Contains(item.PtId)
                                                                             && item.Status > RaiinState.TempSave)
                                                              .Select(item => new { item.PtId, item.HpId, item.SinDate, item.RaiinNo });

        var ptKyuseis = NoTrackingDataContext.PtKyuseis.Where(item => item.HpId == hpId
                                                                      && item.IsDeleted == 0
                                                                      && ptIdList.Contains(item.PtId))
                                                       .OrderBy(item => item.EndDate);

        var ptKohis = NoTrackingDataContext.PtKohis.Where(item => item.HpId == hpId
                                                                  && item.IsDeleted == 0
                                                                  && ptIdList.Contains(item.PtId));

        // Master
        var kaMsts = NoTrackingDataContext.KaMsts.Where(u => u.IsDeleted == 0
                                                             && u.HpId == hpId);

        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.StartDate <= fromDay
                                                                 && toDay <= u.EndDate
                                                                 && u.JobCd == 1
                                                                 && u.IsDeleted == 0
                                                                 && u.HpId == hpId);
        #endregion

        #region main query
        var listReceInfs = receInfs.ToList();
        var query = from receInf in listReceInfs
                    join receInfEdit in receInfEdits on new { receInf.HpId, receInf.SeikyuYm, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receInfEdit.HpId, receInfEdit.SeikyuYm, receInfEdit.PtId, receInfEdit.HokenId, receInfEdit.SinYm } into receInfEditLeft
                    from receInfEdit in receInfEditLeft.DefaultIfEmpty()

                    join receStatus in receStatuses on new { receInf.HpId, receInf.SeikyuYm, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receStatus.HpId, receStatus.SeikyuYm, receStatus.PtId, receStatus.HokenId, receStatus.SinYm } into receStatusLeft
                    from receStatus in receStatusLeft.DefaultIfEmpty()

                    join receCheckCmt in receCheckCmts on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receCheckCmt.HpId, receCheckCmt.PtId, receCheckCmt.HokenId, receCheckCmt.SinYm } into receCheckCmtLeft
                    from receCheckCmt in receCheckCmtLeft.DefaultIfEmpty()

                    join receCheckErr in receCheckErrors on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                               equals new { receCheckErr.HpId, receCheckErr.PtId, receCheckErr.HokenId, receCheckErr.SinYm } into receCheckErrLeft
                    from receCheckErr in receCheckErrLeft.DefaultIfEmpty()

                    join receCmt in receCmts on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receCmt.HpId, receCmt.PtId, receCmt.HokenId, receCmt.SinYm } into receCmtLeft
                    from receCmt in receCmtLeft.DefaultIfEmpty()

                    join receSeikyu in receSeikyus on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuLeft
                    from receSeikyu in receSeikyuLeft.DefaultIfEmpty()

                    join syoukiInf in syoukiInfs on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { syoukiInf.HpId, syoukiInf.PtId, syoukiInf.HokenId, syoukiInf.SinYm } into syoukiInfLeft
                    from syoukiInf in syoukiInfLeft.DefaultIfEmpty()

                    join syobyokeika in syobyokeikas on new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm }
                                                equals new { syobyokeika.HpId, syobyokeika.PtId, syobyokeika.HokenId, syobyokeika.SinYm } into syobyokeikaLeft
                    from syobyokeika in syobyokeikaLeft.Take(1).DefaultIfEmpty()

                    join ptInf in ptInfs on new { receInf.HpId, receInf.PtId }
                                                equals new { ptInf.HpId, ptInf.PtId }

                    join ptHokenInf in ptHokenInfs on new { receInf.HpId, receInf.PtId, receInf.HokenId }
                                                equals new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfLeft
                    from ptHokenInf in ptHokenInfLeft.DefaultIfEmpty()

                    join ptLastVisitDate in ptLastVisitDates on new { receInf.HpId, receInf.PtId }
                                               equals new { ptLastVisitDate.HpId, ptLastVisitDate.PtId } into ptLastVisitDateInf
                    from ptLastVisitDate in ptLastVisitDateInf.OrderByDescending(p => p.SinDate).ThenByDescending(p => p.RaiinNo).Take(1).DefaultIfEmpty()

                    join kaikeiInf in kaikeiInfs on new { receInf.PtId, receInf.HokenId }
                                               equals new { kaikeiInf.PtId, kaikeiInf.HokenId } into kaikeiInfLeft
                    from kaikeiInf in kaikeiInfLeft.OrderByDescending(item => item.SinDate).Take(1).DefaultIfEmpty()

                    join ptKyusei in ptKyuseis on new { receInf.HpId, receInf.PtId }
                                              equals new { ptKyusei.HpId, ptKyusei.PtId } into ptKyuseiLeft
                    from ptKyusei in ptKyuseiLeft.Where(item => item.EndDate >= kaikeiInf.SinDate && item.PtId == kaikeiInf.PtId).Take(1).DefaultIfEmpty()

                    join kaMst in kaMsts on new { receInf.HpId, receInf.KaId }
                                               equals new { kaMst.HpId, kaMst.KaId } into kaMstLeft
                    from kaMst in kaMstLeft.DefaultIfEmpty()

                    join userMst in userMsts on new { receInf.HpId, receInf.TantoId }
                                               equals new { userMst.HpId, TantoId = userMst.UserId } into userMstLeft
                    from userMst in userMstLeft.DefaultIfEmpty()

                    join ptKohi1 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi1Id }
                                              equals new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1Left
                    from ptKohi1 in ptKohi1Left.DefaultIfEmpty()

                    join ptKohi2 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi2Id }
                                              equals new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2Left
                    from ptKohi2 in ptKohi2Left.DefaultIfEmpty()

                    join ptKohi3 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi3Id }
                                              equals new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3Left
                    from ptKohi3 in ptKohi3Left.DefaultIfEmpty()

                    join ptKohi4 in ptKohis on new { receInf.HpId, receInf.PtId, receInf.Kohi4Id }
                                              equals new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4Left
                    from ptKohi4 in ptKohi4Left.DefaultIfEmpty()
                    select new
                    {
                        IsReceInfDetailExist = receInfEdit != null ? 1 : 0,
                        IsPaperRece = receStatus != null ? receStatus.IsPaperRece : 0,
                        Output = receStatus != null ? receStatus.Output : 0,
                        FusenKbn = receStatus != null ? receStatus.FusenKbn : 0,
                        StatusKbn = receStatus != null ? receStatus.StatusKbn : 0,
                        ReceCheckCmt = receCheckCmt != null ? receCheckCmt.Cmt : (receCheckErr != null ? receCheckErr.Message1 + receCheckErr.Message2 : string.Empty),
                        IsPending = receCheckCmt != null ? receCheckCmt.IsPending : -1,
                        ptInf.PtNum,
                        Name = ptKyusei != null ? ptKyusei.Name : ptInf.Name,
                        KanaName = ptKyusei != null ? ptKyusei.KanaName : ptInf.KanaName,
                        ptInf.Sex,
                        ptInf.Birthday,
                        receInf.IsTester,
                        HokensyaNo = ptHokenInf?.HokensyaNo ?? string.Empty,
                        IsSyoukiInfExist = syoukiInf != null ? 1 : 0,
                        IsReceCmtExist = receCmt != null ? 1 : 0,
                        IsSyobyoKeikaExist = syobyokeika != null ? 1 : 0,
                        SeikyuCmt = receSeikyu != null ? receSeikyu.Cmt : string.Empty,
                        LastVisitDate = ptLastVisitDate != null ? ptLastVisitDate.SinDate : 0,
                        KaName = kaMst?.KaName ?? string.Empty, // Check null KaMst.KaName
                        UserName = userMst?.Name ?? string.Empty,
                        IsPtKyuseiExist = ptKyusei != null ? 1 : 0,
                        FutansyaNoKohi1 = ptKohi1 != null ? ptKohi1.FutansyaNo : string.Empty,
                        FutansyaNoKohi2 = ptKohi2 != null ? ptKohi2.FutansyaNo : string.Empty,
                        FutansyaNoKohi3 = ptKohi3 != null ? ptKohi3.FutansyaNo : string.Empty,
                        FutansyaNoKohi4 = ptKohi4 != null ? ptKohi4.FutansyaNo : string.Empty,
                        IsReceStatusExists = receStatus != null,
                        IsPtInfExists = ptInf != null,
                        IsPtHokenInfExists = ptHokenInf != null,
                        IsKohi1Exists = ptKohi1 != null,
                        IsKohi2Exists = ptKohi2 != null,
                        IsKohi3Exists = ptKohi3 != null,
                        IsKohi4Exists = ptKohi4 != null,
                        IsPtLastVisitDateExist = ptLastVisitDate != null,
                        IsKaMstExists = kaMst != null,
                        IsUserMstExist = userMst != null,
                        receInf.SeikyuYm,
                        receInf.PtId,
                        receInf.SeikyuKbn,
                        receInf.SinYm,
                        receInf.ReceSbt,
                        receInf.HokenSbtCd,
                        receInf.HokenKbn,
                        receInf.HokenId,
                        receInf.Tensu,
                        receInf.HokenNissu,
                        receInf.Kohi1Nissu,
                        receInf.HpId,
                        receInf.Kohi1ReceKisai,
                        receInf.Kohi2ReceKisai,
                        receInf.Kohi3ReceKisai,
                        receInf.Kohi4ReceKisai,
                        receInf.Tokki,
                        LastSinDateByHokenId = kaikeiInf?.SinDate ?? 0,
                        ptHokenInf.RousaiKofuNo,
                        ptHokenInf.RousaiJigyosyoName,
                        ptHokenInf.RousaiPrefName,
                        ptHokenInf.RousaiCityName,
                        ptHokenInf.JibaiHokenName,
                        ptHokenInf.JibaiHokenTanto,
                        ptHokenInf.JibaiHokenTel
                    };
        #endregion

        #region Convert to list model
        var rosaiReceden = _systemConfig.RosaiReceden();
        string rosaiRecedenTerm = _systemConfig.RosaiRecedenTerm();

        var receInfAll = query.Select(
        data => new ReceiptListModel(
                    data.PtId,
                    seikyuYm,
                    data.SeikyuKbn,
                    data.SinYm,
                    data.IsReceInfDetailExist,
                    data.IsPaperRece,
                    data.HokenId,
                    data.HokenKbn,
                    data.Output,
                    data.FusenKbn,
                    data.StatusKbn,
                    data.PtNum,
                    data.KanaName,
                    data.Name,
                    data.Sex,
                    data.LastSinDateByHokenId != 0 ? data.LastSinDateByHokenId : data.SinYm * 100 + 1,
                    data.Birthday,
                    data.ReceSbt,
                    data.HokensyaNo,
                    data.Tensu,
                    data.IsSyoukiInfExist,
                    data.IsReceCmtExist,
                    data.IsSyobyoKeikaExist,
                    data.SeikyuCmt,
                    data.LastVisitDate,
                    data.KaName,
                    data.UserName,
                    data.IsPtKyuseiExist,
                    data.FutansyaNoKohi1,
                    data.FutansyaNoKohi2,
                    data.FutansyaNoKohi3,
                    data.FutansyaNoKohi4,
                    data.IsTester == 1,
                    data.HokenNissu ?? 0,
                    data.ReceCheckCmt, 
                    data.RousaiKofuNo,
                    data.RousaiJigyosyoName,
                    data.RousaiPrefName,
                    data.RousaiCityName,
                    data.JibaiHokenName,
                    data.JibaiHokenTanto,
                    data.JibaiHokenTel,
                    rosaiReceden,
                    rosaiRecedenTerm
        ))
        .OrderBy(item => item.SinYm)
        .ThenBy(item => item.PtNum)
        .ToList();
        #endregion

        foreach (var input in receiptInputList)
        {
            var receiptInf = receInfAll.FirstOrDefault(item => item.PtId == input.PtId
                                                               && item.SinYm == input.SinYm
                                                               && item.HokenId == input.HokenId);

            var receipt = listReceInfs.FirstOrDefault(item => item.PtId == input.PtId
                                                              && item.SinYm == input.SinYm
                                                              && item.HokenId == input.HokenId);
            if (receiptInf == null || receipt == null)
            {
                continue;
            }
            var receiptReport = new ReceiptListModel(receipt, rosaiReceden, rosaiRecedenTerm);
            receiptReport.IsReceInfDetailExist = receiptInf.IsReceInfDetailExist;
            receiptReport.IsPaperRece = receiptInf.IsPaperRece;
            receiptReport.Output = receiptInf.Output;
            receiptReport.FusenKbn = receiptInf.FusenKbn;
            receiptReport.StatusKbn = receiptInf.StatusKbn;
            receiptReport.ReceCheckCmt = receiptInf.ReceCheckCmt;
            receiptReport.PtNum = receiptInf.PtNum;
            receiptReport.KanaName = receiptInf.KanaName;
            receiptReport.Name = receiptInf.Name;
            receiptReport.Sex = receiptInf.Sex;
            receiptReport.BirthDay = receiptInf.BirthDay;
            receiptReport.ReceSbt = receiptInf.ReceSbt;
            receiptReport.HokensyaNo = receiptInf.HokensyaNo;
            receiptReport.Tensu = receiptInf.Tensu;
            receiptReport.HokenNissu = receiptInf.HokenNissu.AsInteger();
            receiptReport.IsSyoukiInfExist = receiptInf.IsSyoukiInfExist;
            receiptReport.IsReceCmtExist = receiptInf.IsReceCmtExist;
            receiptReport.IsSyobyoKeikaExist = receiptInf.IsSyobyoKeikaExist;
            receiptReport.ReceSeikyuCmt = receiptInf.ReceSeikyuCmt;
            receiptReport.LastVisitDate = receiptInf.LastVisitDate;
            receiptReport.KaName = receiptInf.KaName;
            receiptReport.SName = receiptInf.SName;
            receiptReport.IsPtKyuseiExist = receiptInf.IsPtKyuseiExist;
            receiptReport.FutansyaNoKohi1 = receiptInf.FutansyaNoKohi1;
            receiptReport.FutansyaNoKohi2 = receiptInf.FutansyaNoKohi2;
            receiptReport.FutansyaNoKohi3 = receiptInf.FutansyaNoKohi3;
            receiptReport.FutansyaNoKohi4 = receiptInf.FutansyaNoKohi4;
            receiptReport.IsPtTest = receiptInf.IsPtTest;
            receiptReport.Age = receiptInf.Age;
            receiptReport.RousaiKofuNo = receiptInf.RousaiKofuNo;
            receiptReport.RousaiJigyosyoName = receiptInf.RousaiJigyosyoName;
            receiptReport.RousaiPrefName = receiptInf.RousaiPrefName;
            receiptReport.RousaiCityName = receiptInf.RousaiCityName;
            receiptReport.JibaiHokenName = receiptInf.JibaiHokenName;
            receiptReport.JibaiHokenTanto = receiptInf.JibaiHokenTanto;
            receiptReport.JibaiHokenTel = receiptInf.JibaiHokenTel;
            receiptReport.RousaiKofuNo = receiptInf.RousaiKofuNo;
            receiptReport.RousaiJigyosyoName = receiptInf.RousaiJigyosyoName;
            receiptReport.RousaiPrefName = receiptInf.RousaiPrefName;
            receiptReport.RousaiCityName = receiptInf.RousaiCityName;
            receiptReport.JibaiHokenName = receiptInf.JibaiHokenName;
            receiptReport.JibaiHokenTanto = receiptInf.JibaiHokenTanto;
            receiptReport.JibaiHokenTel = receiptInf.JibaiHokenTel;
            result.Add(receiptReport);
        }
        return result;
    }
}
