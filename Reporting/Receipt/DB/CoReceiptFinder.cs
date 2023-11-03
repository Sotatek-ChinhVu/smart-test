using Domain.Constant;
using Domain.Models.Accounting;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Reporting.Calculate.Constants;
using Reporting.Calculate.Ika.Models;
using Reporting.Calculate.Interface;
using Reporting.Calculate.Receipt.Models;
using Reporting.Receipt.Constants;
using Reporting.Receipt.Models;

namespace Reporting.Receipt.DB
{
    public class CoReceiptFinder : RepositoryBase, ICoReceiptFinder
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IEmrLogger _emrLogger;

        public CoReceiptFinder(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository, IEmrLogger emrLogger) : base(tenantProvider)
        {
            _systemConfRepository = systemConfRepository;
            _emrLogger = emrLogger;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public HpInfModel FindHpInf(int hpId, int sinDate)
        {
            return new HpInfModel(
                NoTrackingDataContext.HpInfs.Where(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate)
                    .OrderByDescending(p => p.StartDate)
                    .FirstOrDefault() ?? new());
        }

        public List<ReceInfModel> FindReceInf(
            int hpId, int mode, int target, int seikyuYm, List<long> ptId, int sinYm, int hokenId,
            string receSbt, bool includeTester, bool paperOnly, List<int> seikyuKbns, int tantoId, int kaId, int grpId)
        {
            try
            {
                List<List<int>> toHokenKbn =
                new List<List<int>>
                {
                    new List<int>{ 1 },     // 社保
                    new List<int>{ 2 },     // 国保
                    new List<int>{ 11 },    // 労災短期
                    new List<int>{ 12 },    // 労災傷病年金
                    new List<int>{ 13 },    // アフターケア
                    new List<int>{ 14 },    // 自賠
                    new List<int>{ 0 },     // 自費レセ
                    new List<int>{ 1, 2 },  // 健保
                };

                List<int> hokenKbn = null;

                if (mode >= 0)
                {
                    hokenKbn = toHokenKbn[mode];
                }
                else
                {
                    hokenKbn = new List<int> { 1, 2 };
                }

                List<int> isTester = null;
                if (includeTester)
                {
                    isTester = new List<int> { 0, 1 };
                }
                else
                {
                    isTester = new List<int> { 0 };
                }

                int tantoIdRaiin = 0;
                if ((int)_systemConfRepository.GetSettingValue(6002, 1, hpId) == 1) //ReceiptTantoIdTarget
                {
                    tantoIdRaiin = tantoId;
                    tantoId = 0;
                }

                int kaIdRaiin = 0;
                if ((int)_systemConfRepository.GetSettingValue(6002, 0, hpId) == 1) //ReceiptKaIdTarget
                {
                    kaIdRaiin = kaId;
                    kaId = 0;
                }

                var receInfs =
                    NoTrackingDataContext.ReceInfs.Where(p =>
                        p.HpId == hpId &&
                        p.SeikyuYm == seikyuYm &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                        p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                        p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                        (receSbt != "" ? p.ReceSbt.StartsWith(receSbt) : true) &&
                        hokenKbn.Contains(p.HokenKbn) &&
                        isTester.Contains(p.IsTester) &&
                        (tantoId > 0 ? p.TantoId == tantoId : true) &&
                        (kaId > 0 ? p.KaId == kaId : true)
                );

                var kaikeiInfs =
                    NoTrackingDataContext.KaikeiInfs.Where(p =>
                        p.HpId == hpId &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                        p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                        p.SinDate >= (sinYm > 0 ? sinYm * 100 + 1 : p.SinDate) &&
                        p.SinDate <= (sinYm > 0 ? sinYm * 100 + 31 : p.SinDate)
                );

                var raiinInfs =
                    NoTrackingDataContext.RaiinInfs.Where(p =>
                        p.HpId == hpId &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                        p.SinDate >= (sinYm > 0 ? sinYm * 100 + 1 : p.SinDate) &&
                        p.SinDate <= (sinYm > 0 ? sinYm * 100 + 31 : p.SinDate) &&
                        (tantoIdRaiin > 0 ? p.TantoId == tantoIdRaiin : true) &&
                        (kaIdRaiin > 0 ? p.KaId == kaIdRaiin : true) &&
                        p.Status >= 5 &&
                        p.IsDeleted == DeleteStatus.None
                    );

                var kaikei_raiins = (
                        from kaikeiInf in kaikeiInfs
                        join raiinInf in raiinInfs on
                            new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                            new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                        group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                        select new
                        {
                            A.Key.HpId,
                            A.Key.PtId,
                            A.Key.SinYm,
                            A.Key.HokenId
                        }
                    );

                if (target == TargetConst.OsakaSyouni)
                {
                    receInfs = receInfs.Where(p => p.Kohi1Houbetu == "98");
                }
                else if (target == TargetConst.NaganoRece2)
                {
                    receInfs = receInfs.Where(
                        p =>
                            (p.Kohi1Houbetu == "99" && p.Kohi1IchibuSotogaku + p.Kohi1Futan >= 1) ||
                            (p.Kohi2Houbetu == "99" && p.Kohi2IchibuSotogaku + p.Kohi2Futan >= 1) ||
                            (p.Kohi3Houbetu == "99" && p.Kohi3IchibuSotogaku + p.Kohi3Futan >= 1) ||
                            (p.Kohi4Houbetu == "99" && p.Kohi4IchibuSotogaku + p.Kohi4Futan >= 1));
                }
                var receStatusies =
                    NoTrackingDataContext.ReceStatuses.Where(p =>
                        p.HpId == hpId &&
                        p.SeikyuYm == seikyuYm &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                        p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                        p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                        //isPaper.Contains(p.IsPaperRece) &&
                        p.IsDeleted == DeleteStatus.None);
                var ptInfs =
                    NoTrackingDataContext.PtInfs.Where(p =>
                        p.HpId == hpId &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.IsDelete == DeleteStatus.None);
                var ptHokens =
                    NoTrackingDataContext.PtHokenInfs.Where(p =>
                        p.HpId == hpId &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                        p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                        p.IsDeleted == DeleteStatus.None
                );
                var receSeikyus =
                    NoTrackingDataContext.ReceSeikyus.Where(p =>
                        p.HpId == hpId &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                        p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                        p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                        p.IsDeleted == DeleteStatus.None
                );
                var ptGrpInfs =
                    NoTrackingDataContext.PtGrpInfs.Where(p =>
                        p.HpId == hpId &&
                        p.GroupId == (grpId > 0 ? grpId : -1) &&
                        (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                        p.IsDeleted == DeleteStatus.None
                    );

                var entities = (
                            from receInf in receInfs
                            join kaikei_raiin in kaikei_raiins on
                                new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                                new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                            join receStatus in receStatusies on
                                new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                                new { receStatus.HpId, receStatus.PtId, receStatus.SeikyuYm, receStatus.HokenId, receStatus.SinYm } into receStatusJoins
                            from receStatusJoin in receStatusJoins.DefaultIfEmpty()
                            join ptInf in ptInfs on
                                new { receInf.HpId, receInf.PtId } equals
                                new { ptInf.HpId, ptInf.PtId } into ptInfJoins
                            from ptInfJoin in ptInfJoins.DefaultIfEmpty()
                            join ptHoken in ptHokens on
                                new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                                new { ptHoken.HpId, ptHoken.PtId, ptHoken.HokenId } into ptHokenJoins
                            from ptHokenJoin in ptHokenJoins.DefaultIfEmpty()
                            join receSeikyu in receSeikyus on
                                new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm } equals
                                new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuJoins
                            from receSeikyuJoin in receSeikyuJoins.DefaultIfEmpty()
                            join ptGrpInf in ptGrpInfs on
                                new { receInf.HpId, receInf.PtId } equals
                                new { ptGrpInf.HpId, ptGrpInf.PtId } into ptGrpInfJoins
                            from ptGrpInfJoin in ptGrpInfJoins.DefaultIfEmpty()
                            select new
                            {
                                receInf,
                                ptInf = ptInfJoin,
                                ptHokenInf = ptHokenJoin,
                                receSeikyu = receSeikyuJoin,
                                receStatus = receStatusJoin,
                                ptGrpInf = ptGrpInfJoin
                            }
                        ).ToList();

                // 紙レセ対象しかない保険区分（紙レセ対象の設定によらず、出力する）
                List<int> hokenKbns = new List<int> { 0, 14 };

                if ((int)_systemConfRepository.GetSettingValue(100003, 0, hpId) != 1 ||
                    CIUtil.StrToIntDef(_systemConfRepository.GetSettingParams(100003, 0, hpId), 999999) > seikyuYm)
                {
                    // 労災レセプト電算を使用しない期間
                    hokenKbns.AddRange(new List<int> { 11, 12 });
                }

                if ((int)_systemConfRepository.GetSettingValue(100003, 1, hpId) != 1 ||
                    CIUtil.StrToIntDef(_systemConfRepository.GetSettingParams(100003, 1, hpId), 999999) > seikyuYm)
                {
                    // アフターケアレセプト電算を使用しない期間
                    hokenKbns.AddRange(new List<int> { 13 });
                }

                if (paperOnly == false)
                {
                    // 紙レセプトを除く場合
                    // 紙レセ対象しかない保険区分のもの、または
                    // 紙出力フラグが1以外のレセプトのみ出力
                    entities = entities.FindAll(p =>
                        ((seikyuKbns.Contains(p.receInf.SeikyuKbn)) &&
                         (p.receStatus == null || p.receStatus.IsPaperRece == 0)) ||
                        (hokenKbns.Contains(p.receInf.HokenKbn))
                    );
                }
                else
                {
                    // 紙レセプトを出力する場合
                    // 紙レセ対象しかない保険区分のもの、または
                    // 紙出力フラグが1のレセプトのみ出力
                    entities = entities.FindAll(p =>
                        (seikyuKbns.Contains(p.receInf.SeikyuKbn)) ||
                        (p.receStatus != null && p.receStatus.IsPaperRece == 1) ||
                        (hokenKbns.Contains(p.receInf.HokenKbn))
                    );
                }

                List<ReceInfModel> results = new List<ReceInfModel>();

                entities?.ForEach(entity =>
                {

                    var hokenMst = new HokenMst();

                    int rousaiCount = 0;
                    if (entity.ptHokenInf != null && entity.receInf != null)
                    {
                        hokenMst =
                        NoTrackingDataContext.HokenMsts.FirstOrDefault(p =>
                            p.HpId == hpId &&
                            p.PrefNo == 0 &&
                            p.HokenNo == entity.ptHokenInf.HokenNo &&
                            p.HokenEdaNo == entity.ptHokenInf.HokenEdaNo &&
                            p.StartDate <= entity.receInf.SinYm * 100 + 1 &&
                            p.EndDate >= entity.receInf.SinYm * 100 + 1
                            );

                        if (new int[] { 2, 3 }.Contains(mode))
                        {
                            // 労災回数
                            rousaiCount = entity.ptHokenInf.RousaiReceCount;

                            var rousaiReceInfs =
                                NoTrackingDataContext.ReceInfs.Where(p =>
                                    p.HpId == entity.receInf.HpId &&
                                    p.PtId == entity.receInf.PtId &&
                                    p.HokenId == entity.receInf.HokenId &&
                                    p.SinYm >= entity.ptHokenInf.RyoyoStartDate / 100 &&
                                    p.SinYm <= entity.receInf.SinYm &&
                                    p.SeikyuYm <= seikyuYm &&
                                    p.Tensu > 0 &&
                                    //seikyuKbns.Contains(p.SeikyuKbn) &&
                                    hokenKbn.Contains(p.HokenKbn))
                                .GroupBy(p => new { HpId = p.HpId, PtId = p.PtId, SinYm = p.SinYm, HokenId = p.HokenId })
                                .Select(p => new { HpId = p.Key.HpId, PtId = p.Key.PtId, p.Key.SinYm, HokenId = p.Key.HokenId });
                            if (rousaiReceInfs != null && rousaiReceInfs.Any())
                            {
                                rousaiCount += rousaiReceInfs.Count();
                            }
                        }
                    }
                    results.Add(
                        new ReceInfModel(entity.receInf ?? new(), entity.ptInf ?? new(), entity.ptHokenInf ?? new(), hokenMst ?? new(), entity.receSeikyu ?? new(), entity.receStatus ?? new(), entity.ptGrpInf ?? new(), rousaiCount));
                });

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReceInfModel> FindReceInfFukuoka(
           int hpId, int mode, int target, int seikyuYm, List<long> ptId, int sinYm, int hokenId,
            string receSbt, bool includeTester, bool paperOnly, List<int> seikyuKbns, int tantoId, int kaId, int grpId)
        {
            List<List<int>> toHokenKbn =
                new List<List<int>>
                {
                    new List<int>{ 1 },     // 社保
                    new List<int>{ 2 },     // 国保
                    new List<int>{ 11 },    // 労災短期
                    new List<int>{ 12 },    // 労災傷病年金
                    new List<int>{ 13 },    // アフターケア
                    new List<int>{ 14 },    // 自賠
                    new List<int>{ 0 },     // 自費レセ
                    new List<int>{ 1, 2 },  // 健保
                };

            List<int> hokenKbn = null;

            if (mode >= 0)
            {
                hokenKbn = toHokenKbn[mode];
            }
            else
            {
                hokenKbn = new List<int> { 1, 2 };
            }


            List<int> isTester = null;
            if (includeTester)
            {
                isTester = new List<int> { 0, 1 };
            }
            else
            {
                isTester = new List<int> { 0 };
            }

            List<string> tgtKohiHoubetu =
                new List<string>
                {
                    "80", "81", "90"
                };
            var receInfs =
                NoTrackingDataContext.ReceInfs.Where(p =>
                    p.HpId == hpId &&
                    p.SeikyuYm == seikyuYm &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                    (receSbt != "" ? p.ReceSbt.StartsWith(receSbt) : true) &&
                    //seikyuKbns.Contains(p.SeikyuKbn) &&
                    //hokenKbn.Contains(p.HokenKbn) &&
                    p.HokenKbn == 1 &&
                    isTester.Contains(p.IsTester) &&
                    (tantoId > 0 ? p.TantoId == tantoId : true) &&
                    (kaId > 0 ? p.KaId == kaId : true) &&
                    (
                        (tgtKohiHoubetu.Contains(p.Kohi1Houbetu) && p.Kohi1ReceKisai == 0 && p.Kohi1Futan10en > 0) ||
                        (tgtKohiHoubetu.Contains(p.Kohi2Houbetu) && p.Kohi2ReceKisai == 0 && p.Kohi2Futan10en > 0) ||
                        (tgtKohiHoubetu.Contains(p.Kohi3Houbetu) && p.Kohi3ReceKisai == 0 && p.Kohi3Futan10en > 0) ||
                        (tgtKohiHoubetu.Contains(p.Kohi4Houbetu) && p.Kohi4ReceKisai == 0 && p.Kohi4Futan10en > 0)
                    )
                );

            //if (target == TargetConst.OsakaSyouni)
            //{
            //    receInfs = receInfs.Where(p => p.Kohi1Houbetu == "98");
            //}

            var receStatusies =
                NoTrackingDataContext.ReceStatuses.Where(p =>
                    p.HpId == hpId &&
                    p.SeikyuYm == seikyuYm &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                    //isPaper.Contains(p.IsPaperRece) &&
                    p.IsDeleted == DeleteStatus.None);
            var ptInfs =
                NoTrackingDataContext.PtInfs.Where(p =>
                    p.HpId == hpId &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.IsDelete == DeleteStatus.None);
            var ptHokens =
                NoTrackingDataContext.PtHokenInfs.Where(p =>
                    p.HpId == hpId &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                    p.IsDeleted == DeleteStatus.None
                );
            var receSeikyus =
                NoTrackingDataContext.ReceSeikyus.Where(p =>
                    p.HpId == hpId &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                    p.IsDeleted == DeleteStatus.None
                );
            var ptGrpInfs =
                NoTrackingDataContext.PtGrpInfs.Where(p =>
                    p.HpId == hpId &&
                    p.GroupId == (grpId > 0 ? grpId : -1) &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.IsDeleted == DeleteStatus.None
                );

            var entities = (
                        from receInf in receInfs
                        join receStatus in receStatusies on
                            new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                            new { receStatus.HpId, receStatus.PtId, receStatus.SeikyuYm, receStatus.HokenId, receStatus.SinYm } into receStatusJoins
                        from receStatusJoin in receStatusJoins.DefaultIfEmpty()
                        join ptInf in ptInfs on
                            new { receInf.HpId, receInf.PtId } equals
                            new { ptInf.HpId, ptInf.PtId } into ptInfJoins
                        from ptInfJoin in ptInfJoins.DefaultIfEmpty()
                        join ptHoken in ptHokens on
                            new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                            new { ptHoken.HpId, ptHoken.PtId, ptHoken.HokenId } into ptHokenJoins
                        from ptHokenJoin in ptHokenJoins.DefaultIfEmpty()
                        join receSeikyu in receSeikyus on
                            new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm } equals
                            new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuJoins
                        from receSeikyuJoin in receSeikyuJoins.DefaultIfEmpty()
                        join ptGrpInf in ptGrpInfs on
                            new { receInf.HpId, receInf.PtId } equals
                            new { ptGrpInf.HpId, ptGrpInf.PtId } into ptGrpInfJoins
                        from ptGrpInfJoin in ptGrpInfJoins.DefaultIfEmpty()
                        select new
                        {
                            receInf,
                            ptInf = ptInfJoin,
                            ptHokenInf = ptHokenJoin,
                            receSeikyu = receSeikyuJoin,
                            receStatus = receStatusJoin,
                            ptGrpInf = ptGrpInfJoin
                        }
                    ).ToList();

            // 紙レセ対象しかない保険区分（紙レセ対象の設定によらず、出力する）
            var hokenKbns = new List<int> { 0, 13, 14 };

            if ((int)_systemConfRepository.GetSettingValue(100003, 0, hpId) != 1 ||
                CIUtil.StrToIntDef(_systemConfRepository.GetSettingParams(100003, 0, hpId), 999999) > seikyuYm)
            {
                // 労災レセプト電算を使用しない期間
                hokenKbns.AddRange(new List<int> { 11, 12 });
            }

            if (paperOnly == false)
            {
                // 紙レセプトを除く場合
                // 紙レセ対象しかない保険区分のもの、または
                // 紙出力フラグが1以外のレセプトのみ出力
                entities = entities.FindAll(p =>
                    ((seikyuKbns.Contains(p.receInf.SeikyuKbn)) &&
                     (p.receStatus == null || p.receStatus.IsPaperRece == 0)) ||
                    (hokenKbns.Contains(p.receInf.HokenKbn))
                );
            }
            else
            {
                // 紙レセプトを出力する場合
                // 紙レセ対象しかない保険区分のもの、または
                // 紙出力フラグが1のレセプトのみ出力
                entities = entities.FindAll(p =>
                    (seikyuKbns.Contains(p.receInf.SeikyuKbn)) ||
                    (p.receStatus != null && p.receStatus.IsPaperRece == 1) ||
                    (hokenKbns.Contains(p.receInf.HokenKbn))
                );
            }

            List<ReceInfModel> results = new List<ReceInfModel>();

            entities?.ForEach(entity =>
            {
                var hokenMsts =
                    NoTrackingDataContext.HokenMsts.Where(p =>
                        p.HpId == hpId &&
                        p.PrefNo == 0 &&
                        p.HokenNo == entity.ptHokenInf.HokenNo &&
                        p.HokenEdaNo == entity.ptHokenInf.HokenEdaNo &&
                        p.StartDate <= entity.receInf.SinYm * 100 + 1 &&
                        p.EndDate >= entity.receInf.SinYm * 100 + 1
                        );
                HokenMst hokenMst = null;
                if (hokenMsts != null && hokenMsts.Any())
                {
                    hokenMst = hokenMsts.First();
                }

                int rousaiCount = 0;

                if (new int[] { 2, 3 }.Contains(mode))
                {
                    // 労災回数
                    rousaiCount = entity.ptHokenInf.RousaiReceCount;

                    var rousaiReceInfs =
                        NoTrackingDataContext.ReceInfs.Where(p =>
                            p.HpId == entity.receInf.HpId &&
                            p.PtId == entity.receInf.PtId &&
                            p.HokenId == entity.receInf.HokenId &&
                            p.SinYm >= entity.ptHokenInf.RyoyoStartDate / 100 &&
                            p.SinYm <= entity.receInf.SinYm &&
                            p.SeikyuYm <= seikyuYm &&
                            p.Tensu > 0 &&
                            //seikyuKbns.Contains(p.SeikyuKbn) &&
                            hokenKbn.Contains(p.HokenKbn))
                        .GroupBy(p => new { HpId = p.HpId, PtId = p.PtId, SinYm = p.SinYm, HokenId = p.HokenId })
                        .Select(p => new { HpId = p.Key.HpId, PtId = p.Key.PtId, p.Key.SinYm, HokenId = p.Key.HokenId });
                    if (rousaiReceInfs != null && rousaiReceInfs.Any())
                    {
                        rousaiCount += rousaiReceInfs.Count();
                    }
                }

                results.Add(
                    new ReceInfModel(entity.receInf, entity.ptInf, entity.ptHokenInf, hokenMst, entity.receSeikyu, entity.receStatus, entity.ptGrpInf, rousaiCount));
            });

            return results;
        }

        public List<ReceInfModel> FindReceInf(int hpId,
            Reporting.Calculate.ReceFutan.Models.ReceInfModel receInf)
        {
            var ptInfs =
                NoTrackingDataContext.PtInfs.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == receInf.PtId &&
                    p.IsDelete == DeleteStatus.None);
            var ptHokens =
                NoTrackingDataContext.PtHokenInfs.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == receInf.PtId &&
                    p.HokenId == receInf.HokenId &&
                    p.IsDeleted == DeleteStatus.None
                );

            var entities = (
                        from ptInf in ptInfs
                        join ptHoken in ptHokens on
                            new { ptInf.HpId, ptInf.PtId } equals
                            new { ptHoken.HpId, ptHoken.PtId } into ptHokenJoins
                        from ptHokenJoin in ptHokenJoins.DefaultIfEmpty()
                        select new
                        {
                            ptInf,
                            ptHokenInf = ptHokenJoin

                        }
                    ).ToList();

            List<ReceInfModel> results = new List<ReceInfModel>();

            entities?.ForEach(entity =>
            {
                var hokenMsts =
                    NoTrackingDataContext.HokenMsts.Where(p =>
                        p.HpId == hpId &&
                        p.PrefNo == 0 &&
                        p.HokenNo == entity.ptHokenInf.HokenNo &&
                        p.HokenEdaNo == entity.ptHokenInf.HokenEdaNo &&
                        p.StartDate <= receInf.SinYm * 100 + 1 &&
                        p.EndDate >= receInf.SinYm * 100 + 1
                        );
                HokenMst hokenMst = null;
                if (hokenMsts != null && hokenMsts.Any())
                {
                    hokenMst = hokenMsts.First();
                }

                int rousaiCount = 0;

                if (new int[] { 11, 12 }.Contains(receInf.HokenKbn))
                {
                    // 労災回数
                    rousaiCount = entity.ptHokenInf.RousaiReceCount;

                    var rousaiReceInfs =
                        NoTrackingDataContext.ReceInfs.Where(p =>
                            p.HpId == receInf.HpId &&
                            p.PtId == receInf.PtId &&
                            p.HokenId == receInf.HokenId &&
                            p.SinYm >= entity.ptHokenInf.RyoyoStartDate / 100 &&
                            p.SinYm <= receInf.SinYm &&
                            p.SeikyuYm <= receInf.SinYm &&
                            p.Tensu > 0 &&
                            //seikyuKbns.Contains(p.SeikyuKbn) &&
                            p.HokenKbn == receInf.HokenKbn)
                        .GroupBy(p => new { HpId = p.HpId, PtId = p.PtId, SinYm = p.SinYm, HokenId = p.HokenId })
                        .Select(p => new { HpId = p.Key.HpId, PtId = p.Key.PtId, p.Key.SinYm, HokenId = p.Key.HokenId });
                    if (rousaiReceInfs != null && rousaiReceInfs.Any())
                    {
                        rousaiCount += rousaiReceInfs.Count();
                    }
                }

                ReceInfModel result = new ReceInfModel(receInf.ReceInf, entity.ptInf, entity.ptHokenInf, hokenMst, null, null, null, rousaiCount);
                //result.ReceStatusAddNew = false;
                results.Add(result);
            });

            return results;
        }

        public List<SinRpInfModel> FindSinRpInfDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId, int mode, bool includeTester, List<int> seikyuKbns, int tantoId, int kaId)
        {

            var sinRps = NoTrackingDataContext.SinRpInfs.Where(s =>
                    s.HpId == hpId &&
                    s.IsDeleted == DeleteStatus.None);
            //var receInfs = NoTrackingDataContext.ReceInfRepository.Where(r =>
            //    r.HpId == hpId &&
            //    r.SeikyuYm == seikyuYm);
            int tantoIdRaiin = 0;
            if ((int)_systemConfRepository.GetSettingValue(6002, 1, hpId) == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if ((int)_systemConfRepository.GetSettingValue(6002, 0, hpId) == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }


            var receInfs = GetReceInfVar(hpId, seikyuYm, ptId, sinYm, hokenId, mode, includeTester, seikyuKbns, tantoId, kaId);
            var kaikeiInfs = GetKaikeiInfVar(hpId, ptId, sinYm, hokenId);
            var raiinInfs = GetRaiinInfVar(hpId, ptId, sinYm, tantoIdRaiin, kaIdRaiin);
            var kaikei_raiins = (
                    from kaikeiInf in kaikeiInfs
                    join raiinInf in raiinInfs on
                        new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                    select new
                    {
                        A.Key.HpId,
                        A.Key.PtId,
                        A.Key.SinYm,
                        A.Key.HokenId
                    }
                );

            var joinReceInfs = (
                from receInf in receInfs
                join kaikei_raiin in kaikei_raiins on
                    new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                    new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                select new
                {
                    receInf
                });

            var joinQuery = (
                from sinRp in sinRps
                where (
                    (
                        from receInf in joinReceInfs
                        where
                            receInf.receInf.HpId == hpId &&
                            receInf.receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.receInf.HpId == sinRp.HpId &&
                            r.receInf.PtId == sinRp.PtId &&
                            r.receInf.SinYm == sinRp.SinYm
                    )
                )
                select new
                {
                    sinRp
                }

            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new SinRpInfModel(
                        data.sinRp
                )
            )
            .ToList();

            return result;
        }

        private IQueryable<ReceInf> GetReceInfVar(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId, int mode, bool includeTester, List<int> seikyuKbns, int tantoId, int kaId)
        {
            List<List<int>> toHokenKbn =
                new List<List<int>>
                {
                                new List<int>{ 1 },     // 社保
                                new List<int>{ 2 },     // 国保
                                new List<int>{ 11 },    // 労災短期
                                new List<int>{ 12 },    // 労災傷病年金
                                new List<int>{ 13 },    // アフターケア
                                new List<int>{ 14 },    // 自賠
                                new List<int>{ 0 },      // 自費レセ
                                new List<int>{ 1, 2}    // 健保（社保＋国保）
                };
            List<int> hokenKbn = null;

            if (mode >= 0)
            {
                hokenKbn = toHokenKbn[mode];
            }
            else
            {
                hokenKbn = new List<int> { 1, 2 };
            }

            List<int> isTester = null;

            if (includeTester)
            {
                isTester = new List<int> { 0, 1 };
            }
            else
            {
                isTester = new List<int> { 0 };
            }

            return
                NoTrackingDataContext.ReceInfs.Where(p =>
                    p.HpId == hpId &&
                    p.SeikyuYm == seikyuYm &&
                    //seikyuKbns.Contains(p.SeikyuKbn) &&
                    hokenKbn.Contains(p.HokenKbn) &&
                    isTester.Contains(p.IsTester) &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                    (tantoId > 0 ? p.TantoId == tantoId : true) &&
                    (kaId > 0 ? p.KaId == kaId : true)
                    );
        }

        private IQueryable<KaikeiInf> GetKaikeiInfVar(int hpId, List<long> ptId, int sinYm, int hokenId)
        {
            return
                            NoTrackingDataContext.KaikeiInfs.Where(p =>
                                p.HpId == hpId &&
                                (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                                p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                                p.SinDate >= (sinYm > 0 ? sinYm * 100 + 1 : p.SinDate) &&
                                p.SinDate <= (sinYm > 0 ? sinYm * 100 + 31 : p.SinDate)
                                );


        }

        private IQueryable<RaiinInf> GetRaiinInfVar(int hpId, List<long> ptId, int sinYm, int tantoId, int kaId)
        {
            return
                NoTrackingDataContext.RaiinInfs.Where(p =>
                    p.HpId == hpId &&
                    (ptId.Any() ? ptId.Contains(p.PtId) : true) &&
                    p.SinDate >= (sinYm > 0 ? sinYm * 100 + 1 : p.SinDate) &&
                    p.SinDate <= (sinYm > 0 ? sinYm * 100 + 31 : p.SinDate) &&
                    (tantoId > 0 ? p.TantoId == tantoId : true) &&
                    (kaId > 0 ? p.KaId == kaId : true) &&
                    p.Status >= 5 &&
                    p.IsDeleted == DeleteStatus.None
                );


        }

        public List<SinKouiModel> FindSinKouiDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId, int mode, bool includeTester, List<int> seikyuKbns, int tantoId, int kaId)
        {

            var sinKouis = NoTrackingDataContext.SinKouis.Where(s =>
                    s.HpId == hpId &&
                    s.IsDeleted == DeleteStatus.None);
            int tantoIdRaiin = 0;
            if ((int)_systemConfRepository.GetSettingValue(6002, 1, hpId) == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if ((int)_systemConfRepository.GetSettingValue(6002, 0, hpId) == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }

            var receInfs = GetReceInfVar(hpId, seikyuYm, ptId, sinYm, hokenId, mode, includeTester, seikyuKbns, tantoId, kaId);
            var kaikeiInfs = GetKaikeiInfVar(hpId, ptId, sinYm, hokenId);
            var raiinInfs = GetRaiinInfVar(hpId, ptId, sinYm, tantoIdRaiin, kaIdRaiin);
            var kaikei_raiins = (
                    from kaikeiInf in kaikeiInfs
                    join raiinInf in raiinInfs on
                        new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                    select new
                    {
                        A.Key.HpId,
                        A.Key.PtId,
                        A.Key.SinYm,
                        A.Key.HokenId
                    }
                );

            var joinReceInfs = (
                from receInf in receInfs
                join kaikei_raiin in kaikei_raiins on
                    new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                    new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                select new
                {
                    receInf
                });

            var joinQuery = (
                from sinKoui in sinKouis
                where (
                    (
                        from receInf in joinReceInfs
                        where
                            receInf.receInf.HpId == hpId &&
                            receInf.receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.receInf.HpId == sinKoui.HpId &&
                            r.receInf.PtId == sinKoui.PtId &&
                            r.receInf.SinYm == sinKoui.SinYm &&
                            (r.receInf.HokenId == sinKoui.HokenId || r.receInf.HokenId2 == sinKoui.HokenId)
                    )
                )
                select new
                {
                    sinKoui
                }

            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new SinKouiModel(
                        data.sinKoui
                )
            )
            .ToList();

            return result;
        }

        public List<SinKouiDetailModel> FindSinKouiDetailDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId, int mode, bool includeTester, List<int> seikyuKbns, int tantoId, int kaId)
        {
            var sinDtls = NoTrackingDataContext.SinKouiDetails.Where(s =>
                    s.HpId == hpId &&
                    (ptId.Any() ? ptId.Contains(s.PtId) : true) &&
                    s.SinYm == (sinYm > 0 ? sinYm : s.SinYm) &&
                    s.IsDeleted == DeleteStatus.None);
            var sinCounts = NoTrackingDataContext.SinKouiCounts.Where(s =>
                s.HpId == hpId &&
                (ptId.Any() ? ptId.Contains(s.PtId) : true) &&
                s.SinYm == (sinYm > 0 ? sinYm : s.SinYm));

            var sinCountMaxs = (
                    from sinCount in sinCounts
                    group new { sinCount } by new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } into A
                    select new
                    {
                        HpId = A.Key.HpId,
                        PtId = A.Key.PtId,
                        SinYm = A.Key.SinYm,
                        RpNo = A.Key.RpNo,
                        SeqNo = A.Key.SeqNo,
                        LastDate = A.Max(a => a.sinCount.SinDate)
                    }
                );
            //var receInfs = NoTrackingDataContext.ReceInfRepository.FindListQueryableNoTrack(r =>
            //    r.HpId == hpId &&
            //    r.SeikyuYm == seikyuYm);
            int tantoIdRaiin = 0;
            if (_systemConfRepository.GetSettingValue(6002, 1, hpId) == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if (_systemConfRepository.GetSettingValue(6002, 0, hpId) == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }

            var receInfs = GetReceInfVar(hpId,seikyuYm, ptId, sinYm, hokenId, mode, includeTester, seikyuKbns, tantoId, kaId);
            var kaikeiInfs = GetKaikeiInfVar(hpId, ptId, sinYm, hokenId);
            var raiinInfs = GetRaiinInfVar(hpId, ptId, sinYm, tantoIdRaiin, kaIdRaiin);
            var kaikei_raiins = (
                    from kaikeiInf in kaikeiInfs
                    join raiinInf in raiinInfs on
                        new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                    select new
                    {
                        A.Key.HpId,
                        A.Key.PtId,
                        A.Key.SinYm,
                        A.Key.HokenId
                    }
                );

            var joinReceInfs = (
                from receInf in receInfs
                join kaikei_raiin in kaikei_raiins on
                    new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                    new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                select new
                {
                    receInf
                });
            //var tenMsts = NoTrackingDataContext.TenMstRepository.FindListQueryableNoTrack(t =>
            //    t.HpId == HpId);

            // 診療月の最終日を取得
            int lastDateOfMonth = 0;
            if (sinYm > 0)
            {
                lastDateOfMonth = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
            }
            else if (receInfs != null && receInfs.Any())
            {
                lastDateOfMonth = CIUtil.GetLastDateOfMonth((receInfs.Min(p => p.SinYm)) * 100 + 1);
            }

            // 一旦、診療月の最終日基準でマスタ取得
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
                t.HpId == hpId &&
                t.StartDate <= lastDateOfMonth &&
                t.EndDate >= lastDateOfMonth);

            var joinQuery = (
                from sinDtl in sinDtls
                join sinCount in sinCountMaxs on
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo } equals
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } into sc
                from b in sc.DefaultIfEmpty()
                join tenMst in tenMsts on
                    new { sinDtl.HpId, sinDtl.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd } into tm
                from a in tm.DefaultIfEmpty()
                where (
                    //a.StartDate <= sinDtl.SinYm * 100 + 1 &&
                    //(a.EndDate >= sinDtl.SinYm * 100 + 1 || a.EndDate == 12341234)
                    //&&
                    (
                        from receInf in joinReceInfs
                        where
                            receInf.receInf.HpId ==hpId &&
                            receInf.receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.receInf.HpId == sinDtl.HpId &&
                            r.receInf.PtId == sinDtl.PtId &&
                            r.receInf.SinYm == sinDtl.SinYm
                    )
                )
                select new
                {
                    SinKouiDetail = sinDtl,
                    SinKouiCount = b,
                    TenMst = a
                }

            ).ToList();

            List<SinKouiDetailModel> results = new List<SinKouiDetailModel>();

            joinQuery.ForEach(entity =>
            {
                //if (entity.TenMst == null && entity.SinKouiDetail.OdrItemCd.StartsWith("Z"))
                if (entity.TenMst == null ||
                    entity.TenMst.StartDate > (entity.SinKouiCount == null ? entity.SinKouiDetail.SinYm * 100 + 1 : entity.SinKouiCount.LastDate) ||
                    entity.TenMst.EndDate < (entity.SinKouiCount == null ? entity.SinKouiDetail.SinYm * 100 + 1 : entity.SinKouiCount.LastDate) ||
                    (entity.SinKouiDetail.OdrItemCd != null && entity.SinKouiDetail.OdrItemCd.StartsWith("Z")))
                {
                    //var odrTenMst = NoTrackingDataContext.TenMstRepository.FindListQueryableNoTrack(t =>
                    //    t.HpId == HpId &&
                    //    t.StartDate <= entity.SinKouiDetail.SinYm * 100 + 1 &&
                    //    (t.EndDate >= entity.SinKouiDetail.SinYm * 100 + 1 || t.EndDate == 12341234) &&
                    //    t.ItemCd == entity.SinKouiDetail.OdrItemCd)
                    //.ToList();

                    // 点数マスタが取得できなかった or 診療日が有効期間外のマスタしか取得できなかった or 特材の場合
                    // 診療日にもっとも近い点数マスタを取得する
                    int stdDate = (entity.SinKouiCount == null ? entity.SinKouiDetail.SinYm * 100 + 1 : entity.SinKouiCount.LastDate);
                    string targetItemCd = entity.SinKouiDetail.ItemCd;
                    if (entity.SinKouiDetail.OdrItemCd != null && entity.SinKouiDetail.OdrItemCd.StartsWith("Z"))
                    {
                        // 特材の場合は、オーダー診療行為コード基準で取得
                        targetItemCd = entity.SinKouiDetail.OdrItemCd;
                    }

                    var tenMstsBase2 = NoTrackingDataContext.TenMsts.Where(t =>
                        t.HpId == hpId &&
                        t.StartDate <= stdDate &&
                        t.ItemCd == targetItemCd);

                    var tenMstsMax2 = (
                            from tenMst in tenMstsBase2
                            where tenMst.StartDate < stdDate
                            group tenMst by new { tenMst.HpId, tenMst.ItemCd } into A
                            select new { HpId = A.Key.HpId, ItemCd = A.Key.ItemCd, StartDate = A.Max(a => a.StartDate) }
                        );
                    var odrTenMst = (
                            from tenMst in tenMstsBase2
                            join tenMstMax in tenMstsMax2 on
                                new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                                new { tenMstMax.HpId, tenMstMax.ItemCd, tenMstMax.StartDate }
                            select new
                            {
                                tenMst
                            }
                        ).OrderByDescending(p => p.tenMst.StartDate).ToList();

                    if (odrTenMst != null && odrTenMst.Any())
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, odrTenMst.First().tenMst));
                    }
                    else
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst ?? null));
                    }
                }
                else
                {
                    results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst ?? null));
                }
            });

            return results;
        }

        public List<SinKouiCountModel> FindSinKouiCountDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId, int mode, bool includeTester, List<int> seikyuKbns, int tantoId, int kaId)
        {

            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(s =>
                    s.HpId == hpId);
            int tantoIdRaiin = 0;
            if ((int)_systemConfRepository.GetSettingValue(6002, 1, hpId) == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if ((int)_systemConfRepository.GetSettingValue(6002, 0, hpId) == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }

            var receInfs = GetReceInfVar(hpId, seikyuYm, ptId, sinYm, hokenId, mode, includeTester, seikyuKbns, tantoId, kaId);
            var kaikeiInfs = GetKaikeiInfVar(hpId, ptId, sinYm, hokenId);
            var raiinInfs = GetRaiinInfVar(hpId, ptId, sinYm, tantoIdRaiin, kaIdRaiin);
            var kaikei_raiins = (
                    from kaikeiInf in kaikeiInfs
                    join raiinInf in raiinInfs on
                        new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                    select new
                    {
                        A.Key.HpId,
                        A.Key.PtId,
                        A.Key.SinYm,
                        A.Key.HokenId
                    }
                );

            var joinReceInfs = (
                from receInf in receInfs
                join kaikei_raiin in kaikei_raiins on
                    new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                    new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                select new
                {
                    receInf
                });

            var joinQuery = (
                from sinKouiCount in sinKouiCounts
                where (
                    (
                        from receInf in joinReceInfs
                        where
                            receInf.receInf.HpId == hpId &&
                            receInf.receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.receInf.HpId == sinKouiCount.HpId &&
                            r.receInf.PtId == sinKouiCount.PtId &&
                            r.receInf.SinYm == sinKouiCount.SinYm
                    )
                )
                select new
                {
                    sinKouiCount
                }

            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new SinKouiCountModel(
                        data.sinKouiCount
                )
            )
            .ToList();

            return result;
        }

        public List<SinRpInfModel> FindSinRpInfDataForPreview(int hpId, List<long> ptId, int sinYm)
        {

            var sinRps = NoTrackingDataContext.SinRpInfs.Where(s =>
                    s.HpId == hpId &&
                    ptId.Contains(s.PtId) &&
                    s.SinYm == sinYm &&
                    s.IsDeleted == DeleteStatus.None)
                .ToList();

            List<SinRpInfModel> results = new List<SinRpInfModel>();
            sinRps?.ForEach(entity =>
            {
                results.Add(new SinRpInfModel(entity));
            }
            );

            return results;
        }

        public List<SinKouiModel> FindSinKouiDataForPreview(int hpId, List<long> ptId, int sinYm, int hokenId, int hokenId2)
        {

            var sinKouis = NoTrackingDataContext.SinKouis.Where(s =>
                    s.HpId == hpId &&
                    ptId.Contains(s.PtId) &&
                    s.SinYm == sinYm &&
                    (s.HokenId == hokenId || s.HokenId == hokenId2) &&
                    s.IsDeleted == DeleteStatus.None
                    )
                .ToList();

            List<SinKouiModel> results = new List<SinKouiModel>();

            sinKouis?.ForEach(entity =>
            {
                results.Add(new SinKouiModel(entity));
            }
            );

            return results;
        }

        public List<SinKouiDetailModel> FindSinKouiDetailDataForPreview(int hpId, List<long> ptId, int sinYm)
        {

            var sinDtls = NoTrackingDataContext.SinKouiDetails.Where(s =>
                    s.HpId == hpId &&
                    ptId.Contains(s.PtId) &&
                    s.SinYm == sinYm &&
                    s.IsDeleted == DeleteStatus.None);
            var sinCounts = NoTrackingDataContext.SinKouiCounts.Where(s =>
                    s.HpId == hpId &&
                    ptId.Contains(s.PtId) &&
                    s.SinYm == sinYm);

            var sinCountMaxs = (
                    from sinCount in sinCounts
                    group new { sinCount } by new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } into A
                    select new
                    {
                        HpId = A.Key.HpId,
                        PtId = A.Key.PtId,
                        SinYm = A.Key.SinYm,
                        RpNo = A.Key.RpNo,
                        SeqNo = A.Key.SeqNo,
                        LastDate = A.Max(a => a.sinCount.SinDate)
                    }
                );

            // 診療月の最終日を取得
            int lastDateOfMonth = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);

            // 一旦、診療月の最終日基準でマスタ取得
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
                t.HpId == hpId &&
                t.StartDate <= lastDateOfMonth &&
                t.EndDate >= lastDateOfMonth);

            var joinQuery = (
                from sinDtl in sinDtls
                join sinCount in sinCountMaxs on
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo } equals
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } into sc
                from b in sc.DefaultIfEmpty()
                join tenMst in tenMsts on
                    new { sinDtl.HpId, sinDtl.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd } into tm
                from a in tm.DefaultIfEmpty()
                select new
                {
                    SinKouiDetail = sinDtl,
                    SinKouiCount = b,
                    TenMst = a
                }

            ).ToList();

            List<SinKouiDetailModel> results = new List<SinKouiDetailModel>();

            joinQuery.ForEach(entity =>
            {
                if (entity.TenMst == null ||
                    entity.TenMst.StartDate > entity.SinKouiCount.LastDate ||
                    entity.TenMst.EndDate < entity.SinKouiCount.LastDate ||
                    entity.SinKouiDetail.OdrItemCd.StartsWith("Z"))
                {
                    // 診療日にもっとも近い点数マスタを取得する
                    int stdDate = (entity.SinKouiCount == null ? entity.SinKouiDetail.SinYm * 100 + 1 : entity.SinKouiCount.LastDate);
                    string targetItemCd = entity.SinKouiDetail.ItemCd;
                    if (entity.SinKouiDetail.OdrItemCd.StartsWith("Z"))
                    {
                        // 特材の場合は、オーダー診療行為コード基準で取得
                        targetItemCd = entity.SinKouiDetail.OdrItemCd;
                    }
                    var tenMstsBase2 = NoTrackingDataContext.TenMsts.Where(t =>
                        t.HpId == hpId &&
                        t.StartDate <= stdDate &&
                        t.ItemCd == targetItemCd);

                    var tenMstsMax2 = (
                            from tenMst in tenMstsBase2
                            where tenMst.StartDate < stdDate
                            group tenMst by new { tenMst.HpId, tenMst.ItemCd } into A
                            select new { HpId = A.Key.HpId, ItemCd = A.Key.ItemCd, StartDate = A.Max(a => a.StartDate) }
                        );
                    var odrTenMst = (
                            from tenMst in tenMstsBase2
                            join tenMstMax in tenMstsMax2 on
                                new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                                new { tenMstMax.HpId, tenMstMax.ItemCd, tenMstMax.StartDate }
                            select new
                            {
                                tenMst
                            }
                        ).OrderByDescending(p => p.tenMst.StartDate).ToList();
                    if (odrTenMst != null && odrTenMst.Any())
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, odrTenMst.First().tenMst));
                    }
                    else
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst ?? null));
                    }
                }
                else
                {
                    results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst ?? null));
                }
            });

            return results;
        }

        public List<SinKouiCountModel> FindSinKouiCountDataForPreview(int hpId, List<long> ptId, int sinYm)
        {

            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(s =>
                    s.HpId == hpId &&
                    ptId.Contains(s.PtId) &&
                    s.SinYm == sinYm
                    )
                .ToList();

            List<SinKouiCountModel> results = new List<SinKouiCountModel>();

            sinKouiCounts?.ForEach(entity =>
            {
                results.Add(new SinKouiCountModel(entity));
            }
            );

            return results;
        }

        public PtInfModel FindPtInf(int hpId, long ptId, int sinDate)
        {
            PtInf ptInf = NoTrackingDataContext.PtInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.IsDelete == DeleteStatus.None
            ).FirstOrDefault();

            if (ptInf == null)
            {
                return null;
            }
            return new PtInfModel(ptInf, sinDate);
        }

        public HokenDataModel FindHokenData(int hpId, long ptId, int hokenId)
        {
            if (hokenId == 0)
            {
                return null;
            }

            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs;

            var joinQuery = (
                from ptHokenInf in ptHokenInfs
                where
                    ptHokenInf.HpId == hpId &&
                    ptHokenInf.PtId == ptId &&
                    ptHokenInf.HokenId == hokenId &&
                    ptHokenInf.IsDeleted == DeleteStatus.None
                select new
                {
                    ptHokenInf,
                }
            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new HokenDataModel(
                        data.ptHokenInf
                    )
                )
                .FirstOrDefault();

            return result;
        }

        public List<KohiDataModel> FindKohiData(int hpId, long ptId, int sinDate)
        {
            var ptKohis = NoTrackingDataContext.PtKohis;
            var hokenMsts =
                NoTrackingDataContext.HokenMsts.Where(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate
                    );

            var joinQuery = (
                from ptKohi in ptKohis
                join hokenMst in hokenMsts on
                    new { ptKohi.HpId, ptKohi.PrefNo, ptKohi.HokenNo, ptKohi.HokenEdaNo } equals
                    new { hokenMst.HpId, hokenMst.PrefNo, hokenMst.HokenNo, hokenMst.HokenEdaNo } into hokenMstJoins
                from hokenMstJoin in hokenMstJoins.DefaultIfEmpty()
                where
                    ptKohi.HpId == hpId &&
                    ptKohi.PtId == ptId &&
                    ptKohi.IsDeleted == DeleteStatus.None
                select new
                {
                    ptKohi,
                    hokenMstJoin
                }
            );

            return joinQuery.AsEnumerable().Select(
                data =>
                    new KohiDataModel(
                        data.ptKohi, data.hokenMstJoin
                    )
                )
                .ToList();
        }

        public List<SyobyoDataModel> FindSyobyoData(int hpId, long ptId, int sinYm, int hokenId, int outputYm)
        {
            var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.StartDate <= sinYm * 100 + 31 &&
                ((p.TenkiKbn == TenkiKbnConst.Continued && p.TenkiDate <= 0) || p.TenkiDate >= sinYm * 100 + 1) &&
                (p.HokenPid == 0 || p.HokenPid == hokenId) &&
                p.IsNodspRece == 0 &&
                p.IsDeleted == DeleteStatus.None)
                .OrderBy(p => p.StartDate)
                .ThenBy(p => p.SortNo)
                .ToList();

            var results = new List<SyobyoDataModel>();

            ptByomeis?.ForEach(ptByomei =>
            {
                results.Add(new SyobyoDataModel(ptByomei, sinYm, outputYm, _emrLogger));
            }
            );

            return results;
        }

        public List<CoKaikeiDetailModel> FindKaikeiDetail(int hpId, long ptId, int sinYm, int hokenId)
        {
            var kaikeiDtl = NoTrackingDataContext.KaikeiDetails.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate >= sinYm * 100 + 1 &&
                p.SinDate <= sinYm * 100 + 31 &&
                p.HokenId == hokenId &&
                p.Tensu > 0)
                .OrderBy(p => p.SinDate)
                .ThenBy(p => p.OyaRaiinNo)
                .ThenBy(p => p.RaiinNo)
                .ToList();

            var results = new List<CoKaikeiDetailModel>();

            kaikeiDtl.ForEach(p => results.Add(new CoKaikeiDetailModel(p)));

            return results;
        }

        public PtKyuseiModel FindPtKyusei(int hpId, long ptId, int lastDate)
        {
            var entities =
                NoTrackingDataContext.PtKyuseis.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.EndDate >= lastDate &&
                    p.IsDeleted == DeleteStatus.None)
                    .OrderByDescending(p => p.EndDate)
                    .ToList();

            PtKyuseiModel result = null;

            if (entities != null && entities.Any())
            {
                result = new PtKyuseiModel(entities.First());
            }

            return result;
        }

        public PtRousaiTenki FindPtRousaiTenki(int hpId, long ptId, int sinYm, int hokenId)
        {
            var entities =
                NoTrackingDataContext.PtRousaiTenkis.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.HokenId == hokenId &&
                    p.EndDate >= sinYm &&
                    p.IsDeleted == DeleteStatus.None)
                    .OrderBy(p => p.EndDate)
                    .ToList();

            PtRousaiTenki result = null;

            if (entities != null && entities.Any())
            {
                result = entities.First();
            }

            return result;
        }

        public SyobyoKeikaModel FindSyobyoKeika(int hpId, long ptId, int sinYm, int hokenId)
        {
            var entities =
                NoTrackingDataContext.SyobyoKeikas.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.HokenId == hokenId &&
                    p.IsDeleted == DeleteStatus.None)
                .OrderBy(p => p.SinYm)
                .ThenBy(p => p.SinDay)
                .ThenBy(p => p.SeqNo)
                .ToList();

            SyobyoKeikaModel result = null;

            if (entities != null && entities.Any())
            {
                result = new SyobyoKeikaModel(entities.First());
            }

            return result;
        }

        public List<int> FindTuuinDays(int hpId, long ptId, int sinYm, int hokenId)
        {
            var kaikeiDtls = NoTrackingDataContext.KaikeiDetails.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate >= sinYm * 100 + 1 &&
                p.SinDate <= sinYm * 100 + 31 &&
                p.HokenId == hokenId &&
                p.Jitunisu > 0);

            var joinQuery = (
                from kaikeiDtl in kaikeiDtls
                where
                    kaikeiDtl.HpId == hpId &&
                    kaikeiDtl.PtId == ptId
                group kaikeiDtl by kaikeiDtl.SinDate into A
                orderby
                    A.Key descending
                select new
                {
                    SinDate = A.Key
                }
                );
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    data.SinDate
                )
                .ToList();

            List<int> results = new List<int>();

            entities?.ForEach(entity =>
            {
                results.Add(entity);
            });

            return results;
        }

        public SyobyoKeikaModel FindSyobyoKeikaForAfter(int hpId, long ptId, int sinDate, int hokenId)
        {
            var entities =
                NoTrackingDataContext.SyobyoKeikas.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinDate / 100 &&
                    p.SinDay == sinDate % 100 &&
                    p.HokenId == hokenId &&
                    p.IsDeleted == DeleteStatus.None)
                .OrderBy(p => p.SinYm)
                .ThenBy(p => p.SinDay)
                .ThenBy(p => p.SeqNo)
                .ToList();

            SyobyoKeikaModel result = null;

            if (entities != null && entities.Any())
            {
                result = new SyobyoKeikaModel(entities.First());
            }

            return result;
        }

        public int ZenkaiKensaDate(int hpId, long ptId, int sinDate, int hokenId)
        {
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate <= sinDate &&
                o.OdrKouiKbn >= OdrKouiKbnConst.KensaMin &&
                o.OdrKouiKbn <= OdrKouiKbnConst.GazoMax &&
                o.IsDeleted == DeleteStatus.None);
            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(o =>
                o.HpId == hpId &&
                o.HokenId == hokenId);
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.SinDate <= sinDate &&
                r.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from odrInf in odrInfs
                join PtHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
                join RaiinInf in raiinInfs on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo } equals
                    new { RaiinInf.HpId, RaiinInf.PtId, RaiinInf.RaiinNo }
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.IsDeleted == DeleteStatus.None
                group odrInf by odrInf.SinDate into A
                orderby
                    A.Key descending
                select new
                {
                    SinDate = A.Key
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    data.SinDate
                )
                .ToList();

            List<int> results = new List<int>();

            entities?.ForEach(entity =>
            {
                results.Add(entity);
            });

            int ret = 0;
            if (results.Any())
            {
                ret = results.First();
            }

            return ret;
        }

        public ReceInf? GetReceInf(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
        {
            return NoTrackingDataContext.ReceInfs.FirstOrDefault(
                x => x.HpId == hpId && x.PtId == ptId && x.SeikyuYm == seikyuYm && x.SinYm == sinYm && x.HokenId == hokenId);
        }

        public ReceSeikyu? GetReceSeikyu(int hpId, long ptId, int hokenId, int sinYm)
        {
            return NoTrackingDataContext.ReceSeikyus
                                        .FirstOrDefault(item => item.HpId == hpId &&
                                        item.PtId == ptId &&
                                        item.HokenId == hokenId &&
                                        item.SinYm == sinYm &&
                                        item.IsDeleted == 0);
        }

        public List<CoHokenMstModel> FindHokenMst(int hpId, int sinDate, int hokenNo, int hokenEdaNo, int prefNo)
        {
            var hokenMsts = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId);
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
                h => h.HpId == hpId && h.StartDate <= sinDate && h.HokenNo == hokenNo && h.HokenEdaNo == hokenEdaNo && h.PrefNo == prefNo

            ).GroupBy(
                x => new { x.HpId, x.PrefNo, x.HokenNo, x.HokenEdaNo }
            ).Select(
                x => new
                {
                    x.Key.HpId,
                    x.Key.PrefNo,
                    x.Key.HokenNo,
                    x.Key.HokenEdaNo,
                    StartDate = x.Max(d => d.StartDate)
                }
            );

            var ptKohis = NoTrackingDataContext.PtKohis.Where(x => x.HpId == hpId);
            //保険番号マスタの取得
            var retHokenMsts = (
                from hokenMst in hokenMsts
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                select new
                {
                    hokenMst
                }
            );

            var result = retHokenMsts.AsEnumerable().Select(
                data =>
                    new CoHokenMstModel(
                        data.hokenMst
                )
            )
            .ToList();

            return result;
        }

        public List<RecePreviewModel> GetReceInf(int hpId, long ptId)
        {
            try
            {
                return NoTrackingDataContext.ReceInfs
                    .Where(item =>item.HpId == hpId && item.PtId == ptId && item.SeikyuYm != 999999)
                    .AsEnumerable()
                    .Select(item => new RecePreviewModel(item))
                    .OrderByDescending(item => item.SeikyuYmDisplay)
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
