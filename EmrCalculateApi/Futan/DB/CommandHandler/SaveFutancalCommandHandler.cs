using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using Helper.Common;
using PostgreDataContext;

namespace EmrCalculateApi.Futan.DB.CommandHandler
{
    public class SaveFutancalCommandHandler
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly IEmrLogger _emrLogger;
        public SaveFutancalCommandHandler(TenantDataContext tenantDataContext, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _emrLogger = emrLogger;
        }

        public void AddKaikeiInf(List<KaikeiInfModel> kaikeiInfModels)
        {
            const string conFncName = nameof(AddKaikeiInf);
            try
            {
                List<KaikeiInf> kaikeiInfs = kaikeiInfModels.Select(x => x.KaikeiInf).ToList();

                kaikeiInfs.ForEach(k =>
                    {
                        k.CreateDate = CIUtil.GetJapanDateTimeNow();
                        k.CreateId = Hardcode.UserID;
                        k.CreateMachine = Hardcode.ComputerName;
                    }
                );

                _tenantDataContext.KaikeiInfs.AddRange(kaikeiInfs);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void AddKaikeiDetail(KaikeiDetailModel kaikeiDetailModel)
        {
            const string conFncName = nameof(AddKaikeiDetail);
            try
            {
                kaikeiDetailModel.CreateDate = CIUtil.GetJapanDateTimeNow();
                kaikeiDetailModel.CreateId = Hardcode.UserID;
                kaikeiDetailModel.CreateMachine = Hardcode.ComputerName;


                _tenantDataContext.KaikeiDetails.Add(kaikeiDetailModel.KaikeiDetail);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void AddKaikeiDetails(List<KaikeiDetailModel> kaikeiDetailModels)
        {
            const string conFncName = nameof(AddKaikeiDetails);
            try
            {
                List<KaikeiDetail> kaikeiDetails = kaikeiDetailModels.Select(x => x.KaikeiDetail).ToList();

                kaikeiDetails.ForEach(x =>
                    {
                        x.CreateDate = CIUtil.GetJapanDateTimeNow();
                        x.CreateId = Hardcode.UserID;
                        x.CreateMachine = Hardcode.ComputerName;
                    }
                );

                _tenantDataContext.KaikeiDetails.AddRange(kaikeiDetails);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void AddLimitListInfs(List<LimitListInfModel> limitListInfModels)
        {
            const string conFncName = nameof(AddLimitListInfs);
            try
            {
                List<LimitListInf> limitListInfs = limitListInfModels.Select(x => x.LimitListInf).ToList();

                limitListInfs.ForEach(x =>
                    {
                        x.SeqNo = 0;
                        x.CreateDate = CIUtil.GetJapanDateTimeNow();
                        x.CreateId = Hardcode.UserID;
                        x.CreateMachine = Hardcode.ComputerName;
                    }
                );

                _tenantDataContext.LimitListInfs.AddRange(limitListInfs);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void AddLimitCntListInfs(List<LimitCntListInfModel> limitCntListInfModels)
        {
            const string conFncName = nameof(AddLimitCntListInfs);
            try
            {
                List<LimitCntListInf> limitCntListInfs = limitCntListInfModels.Select(x => x.LimitCntListInf).ToList();

                limitCntListInfs.ForEach(x =>
                {
                    x.SeqNo = 0;
                    x.CreateDate = CIUtil.GetJapanDateTimeNow();
                    x.CreateId = Hardcode.UserID;
                    x.CreateMachine = Hardcode.ComputerName;
                }
                );

                _tenantDataContext.LimitCntListInfs.AddRange(limitCntListInfs);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        /// <summary>
        /// 収納請求情報の更新
        /// </summary>
        /// <param name="syunoSeikyu">収納請求情報</param>
        /// <param name="nyukinGaku">収納入金情報</param>
        /// <param name="kaikeiInf">会計情報</param>
        /// <param name="sinKouiCounts">診療行為回数情報</param>
        /// <param name="sinKouis">診療行為情報</param>
        /// <param name="seikyuUp">請求情報更新(0:反映しない 1:反映する)</param>
        public void UpdateSyunoSeikyu(
            SyunoSeikyuModel syunoSeikyu, int nyukinGaku, List<KaikeiInfModel> kaikeiInfs,
            List<SinKouiCountModel> sinKouiCounts, List<SinKouiModel> sinKouis,
            long raiinNo, int seikyuUp
        )
        {
            const string conFncName = nameof(UpdateSyunoSeikyu);
            try
            {

                var kaikeiInf = kaikeiInfs
                    .FindAll(k => k.RaiinNo == raiinNo)
                    .GroupBy(k => new { k.HpId, k.PtId, k.SinDate, k.RaiinNo })
                    .Select(k =>
                        new
                        {
                            k.Key.HpId,
                            k.Key.PtId,
                            k.Key.SinDate,
                            k.Key.RaiinNo,
                            Tensu = k.Sum(x => x.Tensu),
                            TotalPtFutan = k.Sum(x => x.TotalPtFutan),
                            AdjustFutan = -k.Sum(x => x.AdjustFutan)
                        })
                    .First();

                //診療情報の詳細情報を取得
                var sinKouiJoin = (
                    from sinKouiCount in sinKouiCounts
                    join sinKoui in sinKouis on
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                    where
                        sinKouiCount.HpId == kaikeiInf.HpId &&
                        sinKouiCount.PtId == kaikeiInf.PtId &&
                        sinKouiCount.RaiinNo == kaikeiInf.RaiinNo
                    orderby
                        sinKoui.SyukeiSaki, sinKoui.RpNo, sinKoui.SeqNo
                    select new
                    {
                        sinKoui.DetailData
                    }
                );
                //複数行を改行コードで連結
                string detailData = string.Join("\r\n", sinKouiJoin.Select(s => s.DetailData));

                if (syunoSeikyu == null)
                {
                    syunoSeikyu = new SyunoSeikyuModel(new SyunoSeikyu());

                    syunoSeikyu.HpId = kaikeiInf.HpId;
                    syunoSeikyu.PtId = kaikeiInf.PtId;
                    syunoSeikyu.SinDate = kaikeiInf.SinDate;
                    syunoSeikyu.RaiinNo = kaikeiInf.RaiinNo;
                    syunoSeikyu.SeikyuTensu = kaikeiInf.Tensu;
                    syunoSeikyu.AdjustFutan = kaikeiInf.AdjustFutan;
                    syunoSeikyu.SeikyuGaku = kaikeiInf.TotalPtFutan;
                    syunoSeikyu.SeikyuDetail = detailData;
                    syunoSeikyu.NewSeikyuTensu = kaikeiInf.Tensu;
                    syunoSeikyu.NewAdjustFutan = kaikeiInf.AdjustFutan;
                    syunoSeikyu.NewSeikyuGaku = kaikeiInf.TotalPtFutan;
                    syunoSeikyu.NewSeikyuDetail = detailData;

                    syunoSeikyu.CreateDate = CIUtil.GetJapanDateTimeNow();
                    syunoSeikyu.CreateId = Hardcode.UserID;
                    syunoSeikyu.CreateMachine = Hardcode.ComputerName;

                    _tenantDataContext.SyunoSeikyus.Add(syunoSeikyu.SyunoSeikyu);
                }
                else if (new int[] { 0, 2 }.Contains(syunoSeikyu.NyukinKbn) ||
                    syunoSeikyu.NewSeikyuTensu != kaikeiInf.Tensu || syunoSeikyu.NewAdjustFutan != kaikeiInf.AdjustFutan ||
                    syunoSeikyu.NewSeikyuGaku != kaikeiInf.TotalPtFutan || syunoSeikyu.NewSeikyuDetail != detailData ||
                    (seikyuUp == 1 && new int[] { 1, 3 }.Contains(syunoSeikyu.NyukinKbn) &&
                        (syunoSeikyu.SeikyuTensu != kaikeiInf.Tensu ||
                         syunoSeikyu.SeikyuGaku != kaikeiInf.TotalPtFutan ||
                         syunoSeikyu.AdjustFutan != kaikeiInf.AdjustFutan ||
                         syunoSeikyu.SeikyuGaku != kaikeiInf.TotalPtFutan ||
                         syunoSeikyu.SeikyuDetail != detailData)))
                {
                    switch (syunoSeikyu.NyukinKbn)
                    {
                        case 1:  //一部精算
                        case 3:  //精算済
                            if (seikyuUp == 1)
                            {
                                syunoSeikyu.SeikyuTensu = kaikeiInf.Tensu;
                                syunoSeikyu.AdjustFutan = kaikeiInf.AdjustFutan;
                                syunoSeikyu.SeikyuGaku = kaikeiInf.TotalPtFutan;
                                syunoSeikyu.SeikyuDetail = detailData;

                                syunoSeikyu.NyukinKbn = syunoSeikyu.SeikyuGaku == nyukinGaku ? 3 : 1;
                            }
                            else if (syunoSeikyu.SeikyuTensu != kaikeiInf.Tensu)
                            {
                                syunoSeikyu.SeikyuTensu = kaikeiInf.Tensu;
                                syunoSeikyu.SeikyuDetail = detailData;
                            }
                            break;
                        default:
                            syunoSeikyu.SeikyuTensu = kaikeiInf.Tensu;
                            syunoSeikyu.AdjustFutan = kaikeiInf.AdjustFutan;
                            syunoSeikyu.SeikyuGaku = kaikeiInf.TotalPtFutan;
                            syunoSeikyu.SeikyuDetail = detailData;
                            break;
                    }

                    syunoSeikyu.NewSeikyuTensu = kaikeiInf.Tensu;
                    syunoSeikyu.NewAdjustFutan = kaikeiInf.AdjustFutan;
                    syunoSeikyu.NewSeikyuGaku = kaikeiInf.TotalPtFutan;
                    syunoSeikyu.NewSeikyuDetail = detailData;
                    syunoSeikyu.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    syunoSeikyu.UpdateId = Hardcode.UserID;
                    syunoSeikyu.UpdateMachine = Hardcode.ComputerName;
                }
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void AddCalcLogs(List<CalcLogModel> calcLogModels)
        {
            const string conFncName = nameof(AddCalcLogs);
            try
            {
                if (calcLogModels.Count == 0) return;
                List<CalcLog> calcLogs = calcLogModels.Select(c => c.CalcLog).ToList();

                var hpId = calcLogs[0].HpId;
                var ptId = calcLogs[0].PtId;
                var raiinNo = calcLogs[0].RaiinNo;

                //点数計算で登録されたCalcLogのSeqNo最大値を取得
                var seqNos = _tenantDataContext.CalcLogs.FindListNoTrack(x =>
                    x.HpId == hpId &&
                    x.PtId == ptId &&
                    x.RaiinNo == raiinNo
                ).Select(x => x.SeqNo).ToList();

                var seqNo = seqNos?.Count >= 1 ? seqNos.Max() : 0;

                calcLogs.ForEach(c =>
                {
                    c.SeqNo += seqNo;
                    c.CreateDate = CIUtil.GetJapanDateTimeNow();
                    c.CreateId = Hardcode.UserID;
                    c.CreateMachine = Hardcode.ComputerName;
                }
                );

                _tenantDataContext.CalcLogs.AddRange(calcLogs);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }
    }
}
