using EmrCalculateApi.Futan.DB.CommandHandler;
using EmrCalculateApi.Futan.DB.Finder;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace EmrCalculateApi.Futan.ViewModels
{
#pragma warning disable S3358 // Ternary operators should not be nested
#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable CS8618
#pragma warning disable S1155
#pragma warning disable S2971
#pragma warning disable S1121
#pragma warning disable S1481
#pragma warning disable CS8602
#pragma warning disable CS8625
#pragma warning disable S2184
#pragma warning disable S3267
#pragma warning disable S1125
#pragma warning disable S1066
#pragma warning disable S1871
    public class FutancalcViewModel : IFutancalcViewModel
    {
        private readonly FutancalcFinder _futancalcFinder;
        private readonly OdrInfFinder _odrInfFinder;
        private readonly RaiinInfFinder _raiinInfFinder;
        private readonly SaveFutancalCommandHandler _saveFutancalCommandHandler;
        private readonly ClearCommandHandler _clearCommandHandler;
        private FutancalcListModel _futancalcAggregate;

        public RaiinTensuModel RaiinTensu { get; set; } = new RaiinTensuModel();
        public PtInfModel PtInf { get; set; }
        public List<KaikeiInfModel> KaikeiInfs { get; private set; } = new List<KaikeiInfModel>();
        public KaikeiDetailModel KaikeiDetail { get; set; } = new KaikeiDetailModel(new KaikeiDetail());
        public List<KaikeiDetailModel> AdjustDetails { get; private set; } = new List<KaikeiDetailModel>();
        public List<LimitListInfModel> LimitListInfs { get; private set; } = new List<LimitListInfModel>();
        public List<LimitCntListInfModel> LimitCntListInfs { get; private set; } = new List<LimitCntListInfModel>();
        public List<KaikeiDetailModel> KaikeiDetails { get; set; } = new List<KaikeiDetailModel>();
        public List<KaikeiDetailModel> KaikeiAdjustDetails { get; set; } = new List<KaikeiDetailModel>();
        public List<KaikeiDetailModel> KaikeiTotalDetails { get; private set; } = new List<KaikeiDetailModel>();
        public List<LimitListInfModel> LimitListOthers { get; set; } = new List<LimitListInfModel>();
        public List<LimitCntListInfModel> LimitCntListOthers { get; set; } = new List<LimitCntListInfModel>();
        public List<PtSanteiConfModel> PtSanteiConfs { get; private set; } = new List<PtSanteiConfModel>();
        public List<OdrInfModel> OdrInfs { get; private set; } = new List<OdrInfModel>();
        public PtHokenPatternModel HokenPattern { get; set; }
        public PtHokenInfModel PtHoken { get; set; }
        public List<PtKohiModel> PtKohis { get; private set; } = new List<PtKohiModel>();
        public List<CalcLogModel> CalcLogs { get; private set; } = new List<CalcLogModel>();

        public struct SystemConfs
        {
            /// <summary>
            /// 15更生があり異点数の場合のマル長の負担額
            ///     0:公費負担額を含む
            ///     1:社保/公費負担額を含む　　　国保/公費負担額を含まない
            ///     2:社保/公費負担額を含まない　国保/公費負担額を含む
            ///     3:公費負担額を含まない
            /// </summary>
            public int ChokiFutan { get; set; }

            /// <summary>
            /// マル長計算オプション
            ///     0:月単位
            ///     1:日単位（公費負担額を含むのみ）
            ///     2:社保/日単位　国保/月単位
            ///     3:社保/月単位　国保/日単位     
            /// </summary>
            /// <remarks>
            ///     公１が5000円上限、マル長10000円で、1日目にマル長上限に達して公1上限未満だった場合に、
            ///     2日目以降に公1上限まで患者負担させるかどうか（月単位の場合は公1上限まで患者負担させる）
            /// </remarks>
            public int ChokiDateRange { get; set; }

            /// <summary>
            /// 高額療養費の窓口負担まるめ設定
            ///     0:1円単位
            ///     1:10円単位(四捨五入)
            ///     2:10円単位(切り捨て)
            /// </summary>
            public int RoundKogakuPtFutan { get; set; }

            public SystemConfs(int chokiFutan, int chokiDateRange, int roundKogakuPtFutan)
            {
                ChokiFutan = chokiFutan;
                ChokiDateRange = chokiDateRange;
                RoundKogakuPtFutan = roundKogakuPtFutan;
            }
        }
        public SystemConfs SystemConf;

        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public FutancalcViewModel(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _systemConfigProvider = systemConfigProvider;
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
            _emrLogger = emrLogger;

            _futancalcFinder = new FutancalcFinder(_tenantDataContext);
            _odrInfFinder = new OdrInfFinder(_tenantDataContext);
            _raiinInfFinder = new RaiinInfFinder(_tenantDataContext, _systemConfigProvider);
            _saveFutancalCommandHandler = new SaveFutancalCommandHandler(_tenantDataContext, emrLogger);
            _clearCommandHandler = new ClearCommandHandler(_tenantDataContext, emrLogger);

            SystemConf = new SystemConfs(
                chokiFutan: _systemConfigProvider.GetChokiFutan(),
                chokiDateRange: _systemConfigProvider.GetChokiDateRange(),
                roundKogakuPtFutan: _systemConfigProvider.GetRoundKogakuPtFutan()
            );
        }

        /// <summary>
        /// 負担金計算
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="sinKouiCounts">診療行為回数(点数計算の結果/当月分)</param>
        /// <param name="sinKouis">診療行為(点数計算の結果/当月分)</param>
        /// <param name="sinKouiDetails">診療行為詳細(点数計算の結果/当月分)</param>
        /// <param name="sinRpInfs">診療Rp情報(点数計算の結果/当月分)</param>
        /// <param name="seikyuUp">請求情報更新(0:反映しない 1:反映する)</param>
        public void FutanCalculation(
            long ptId, int sinDate,
            List<SinKouiCountModel> sinKouiCounts, List<SinKouiModel> sinKouis,
            List<SinKouiDetailModel> sinKouiDetails, List<SinRpInfModel> sinRpInfs,
            int seikyuUp
        )
        {
            FutanCalculateMain(
                ptId, sinDate, default, sinKouiCounts, sinKouis, sinKouiDetails, sinRpInfs, null, seikyuUp
            );
        }

        /// <summary>
        /// 試算（データベースに登録しない）
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="sinKouiCounts">診療行為回数(点数計算の結果/当月分)</param>
        /// <param name="sinKouis">診療行為(点数計算の結果/当月分)</param>
        /// <param name="sinKouiDetails">診療行為詳細(点数計算の結果/当月分)</param>
        /// <param name="sinRpInfs">診療Rp情報(点数計算の結果/当月分)</param>
        public List<KaikeiInfModel> TrialFutanCalculation(
            long ptId, int sinDate, long raiinNo,
            List<SinKouiCountModel> sinKouiCounts, List<SinKouiModel> sinKouis,
            List<SinKouiDetailModel> sinKouiDetails, List<SinRpInfModel> sinRpInfs,
            List<RaiinInfModel> raiinInfs
        )
        {
            FutanCalculateMain(
                ptId, sinDate, raiinNo, sinKouiCounts, sinKouis, sinKouiDetails, sinRpInfs, raiinInfs, default
            );

            return KaikeiInfs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="sinKouiCounts">診療行為回数(点数計算の結果/当月分)</param>
        /// <param name="sinKouis">診療行為(点数計算の結果/当月分)</param>
        /// <param name="sinKouiDetails">診療行為詳細(点数計算の結果/当月分)</param>
        /// <param name="sinRpInfs">診療Rp情報(点数計算の結果/当月分)</param>
        /// <param name="seikyuUp">請求情報更新(0:反映しない 1:反映する)</param>
        private void FutanCalculateMain(
            long ptId, int sinDate, long raiinNo,
            List<SinKouiCountModel> sinKouiCounts, List<SinKouiModel> sinKouis,
            List<SinKouiDetailModel> sinKouiDetails, List<SinRpInfModel> sinRpInfs,
            List<RaiinInfModel> raiinInfs, int seikyuUp
        )
        {
            const string conFncName = nameof(FutanCalculateMain);

            _emrLogger.WriteLogStart(this, conFncName, string.Format("ptId:{0} sinDate:{1} raiinNo:{2} seikyuUp:{3}", ptId, sinDate, raiinNo, seikyuUp)
            );

            //来院番号の指定がある場合は試算モード
            bool trialCalc = raiinNo > 0;

            //来院情報の取得
            List<RaiinTensuModel> raiinTensus = _raiinInfFinder.FindRaiinInf(
                Hardcode.HospitalID, ptId, sinDate, raiinNo,
                //sinKouiCountModels, sinKouiModels, sinKouiDetailModels, sinRpInfModels
                ref sinKouiCounts, ref sinKouis, ref sinKouiDetails, ref sinRpInfs, ref raiinInfs
            );


            //if (raiinTensus == null || raiinTensus.Count == 0) return;

            //会計詳細情報取得（月単位）
            //  ※削除前に当日分も含めて取得する
            KaikeiTotalDetails = _futancalcFinder.FindTotalKaikeiDetail(
                Hardcode.HospitalID, ptId, sinDate
            );

            if (!trialCalc)
            {
                List<long> raiinNos = raiinTensus == null || raiinTensus.Count == 0 ? new List<long>() : raiinTensus.Select(s => s.RaiinNo).ToList();

                //計算結果初期化
                _clearCommandHandler.ClearCalculate(
                    Hardcode.HospitalID,
                    ptId,
                    sinDate,
                    raiinNos
                );
            }

            if (raiinTensus == null || raiinTensus.Count == 0)
            {
                if (!trialCalc)
                {
                    _tenantDataContext.SaveChanges();
                }

                return;
            }

            //患者情報取得
            PtInf = _futancalcFinder.FindPtInf(raiinTensus[0].HpId, raiinTensus[0].PtId, raiinTensus[0].SinDate);
            if (PtInf == null) return;

            //初期化
            KaikeiInfs = new List<KaikeiInfModel>();

            for (int iCnt = 0; iCnt < raiinTensus.Count; iCnt++)
            {
                RaiinTensu = raiinTensus[iCnt];

                //初期化
                if (iCnt >= 1)
                {
                    KaikeiDetail = new KaikeiDetailModel(new KaikeiDetail());
                    AdjustDetails = new List<KaikeiDetailModel>();
                    LimitListInfs = new List<LimitListInfModel>();
                    LimitCntListInfs = new List<LimitCntListInfModel>();
                    CalcLogs = new List<CalcLogModel>();
                }

                //保険パターン情報取得
                HokenPattern = _futancalcFinder.FindHokenPattern(
                    RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.HokenPid
                );
                if (HokenPattern == null) continue;

                //保険情報取得
                PtHoken = _futancalcFinder.FindHokenInf(
                    RaiinTensu.HpId, RaiinTensu.PtId,
                    HokenPattern.HokenId, RaiinTensu.SinDate
                );
                if (PtHoken == null) continue;

                //公費情報取得
                PtKohis.Clear();
                PtKohiModel kohiInf;
                for (int i = 1; i <= 4; i++)
                {
                    kohiInf = _futancalcFinder.FindKohiInf(
                        RaiinTensu.HpId, RaiinTensu.PtId, HokenPattern.KohiNoToId(i), RaiinTensu.SinDate, PtHoken.HokensyaNo
                    );
                    if (kohiInf == null)
                    {
                        break;
                    }
                    PtKohis.Add(kohiInf);
                }

                if (iCnt == 0)
                {
                    //会計詳細情報取得
                    KaikeiDetails = _futancalcFinder.FindKaikeiDetail(
                        RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.SinDate, RaiinTensu.RaiinNo,
                        RaiinTensu.HokenPid, RaiinTensu.SortKey, false
                    );
                    //会計詳細情報取得（調整レコード）
                    KaikeiAdjustDetails = _futancalcFinder.FindKaikeiDetail(
                        RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.SinDate, RaiinTensu.RaiinNo,
                        RaiinTensu.HokenPid, RaiinTensu.SortKey, true
                    );
                }
                //上限管理情報取得
                LimitListOthers = _futancalcFinder.FindLimitListInf(
                    RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.SinDate, RaiinTensu.RaiinNo,
                    RaiinTensu.HokenPid, RaiinTensu.SortKey
                );

                //上限回数管理情報取得
                LimitCntListOthers = _futancalcFinder.FindLimitCntListInf(
                    RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.SinDate, RaiinTensu.SortKey
                );

                //調整額情報取得
                PtSanteiConfs = _futancalcFinder.FindPtSanteiConf(RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.SinDate);

                //オーダー情報取得
                OdrInfs = _odrInfFinder.FindOdrInf(RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.SinDate);

                //計算処理
                DetailCalculate(raiinTensus.Find(r => r.OyaRaiinNo == RaiinTensu.OyaRaiinNo && r.HokenId == RaiinTensu.HokenId && r.SortKey.CompareTo(RaiinTensu.SortKey) == 1) == null);

                if (!trialCalc)
                {
                    //DB保存
                    _saveFutancalCommandHandler.AddKaikeiDetail(KaikeiDetail);
                    _saveFutancalCommandHandler.AddKaikeiDetails(AdjustDetails);
                    _saveFutancalCommandHandler.AddLimitListInfs(LimitListInfs);
                    _saveFutancalCommandHandler.AddLimitCntListInfs(LimitCntListInfs);
                    _saveFutancalCommandHandler.AddCalcLogs(CalcLogs);
                    if (iCnt == raiinTensus.Count - 1 || RaiinTensu.RaiinNo != raiinTensus[iCnt + 1].RaiinNo)
                    {
                        //収納情報取得
                        SyunoSeikyuModel syunoSeikyu = _futancalcFinder.FindSyunoSeikyu(RaiinTensu.HpId, RaiinTensu.PtId, RaiinTensu.SinDate, RaiinTensu.RaiinNo);
                        int nyukinGaku = _futancalcFinder.FindSyunoNyukin(RaiinTensu.HpId, RaiinTensu.RaiinNo);

                        _saveFutancalCommandHandler.AddKaikeiInf(KaikeiInfs);
                        _saveFutancalCommandHandler.UpdateSyunoSeikyu(
                            syunoSeikyu, nyukinGaku, KaikeiInfs, sinKouiCounts, sinKouis, RaiinTensu.RaiinNo, seikyuUp);

                        //初期化
                        KaikeiInfs = new List<KaikeiInfModel>();
                    }
                    //DB保存確定
                    _tenantDataContext.SaveChanges();
                }

                KaikeiDetails.Add(KaikeiDetail);
                KaikeiAdjustDetails.AddRange(AdjustDetails);
            }

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            if (!trialCalc)
            {
                //DB更新
                //DbService.SaveChanged();
            }
            //stopwatch.Stop();
            //Console.WriteLine("DB savechanges: " + stopwatch.ElapsedMilliseconds);

            _emrLogger.WriteLogEnd(this, conFncName, string.Format("ptId:{0} sinDate:{1} raiinNo:{2} seikyuUp:{3}", ptId, sinDate, raiinNo, seikyuUp)
            );
        }

        public void DetailCalculate(bool raiinAdjust)
        {
            //KaikeiDetail = new KaikeiDetailModel(new KaikeiDetail());
            KaikeiDetail.HpId = RaiinTensu.HpId;
            KaikeiDetail.PtId = RaiinTensu.PtId;
            KaikeiDetail.SinDate = RaiinTensu.SinDate;
            KaikeiDetail.RaiinNo = RaiinTensu.RaiinNo;
            KaikeiDetail.OyaRaiinNo = RaiinTensu.OyaRaiinNo;
            KaikeiDetail.HokenPid = RaiinTensu.HokenPid;
            KaikeiDetail.Tensu = RaiinTensu.Tensu;
            if (new int[] { 0, 4 }.Contains(RaiinTensu.HokenKbn))
            {
                KaikeiDetail.Tensu += RaiinTensu.RousaiEnTensu;
            }
            KaikeiDetail.HokenKbn = HokenPattern.HokenKbn;
            KaikeiDetail.HokenSbtCd = HokenPattern.HokenSbtCd;
            KaikeiDetail.HokenId = HokenPattern.HokenId;
            KaikeiDetail.Kohi1Id = HokenPattern.Kohi1Id;
            KaikeiDetail.Kohi2Id = HokenPattern.Kohi2Id;
            KaikeiDetail.Kohi3Id = HokenPattern.Kohi3Id;
            KaikeiDetail.Kohi4Id = HokenPattern.Kohi4Id;
            KaikeiDetail.Jitunisu = Convert.ToInt32(RaiinTensu.JituNisu);
            KaikeiDetail.SortKey = RaiinTensu.SortKey;
            KaikeiDetail.JihiFutan = RaiinTensu.JihiFutan;
            KaikeiDetail.JihiOuttax = RaiinTensu.OutTax;
            KaikeiDetail.JihiTax = RaiinTensu.InclTax;
            KaikeiDetail.JihiFutanTaxfree = RaiinTensu.JihiTaxFree;
            KaikeiDetail.JihiFutanOuttaxNr = RaiinTensu.JihiOutTaxNr;
            KaikeiDetail.JihiFutanOuttaxGen = RaiinTensu.JihiOutTaxGen;
            KaikeiDetail.JihiFutanTaxNr = RaiinTensu.JihiTaxNr;
            KaikeiDetail.JihiFutanTaxGen = RaiinTensu.JihiTaxGen;
            KaikeiDetail.JihiOuttaxNr = RaiinTensu.OutTaxNr;
            KaikeiDetail.JihiOuttaxGen = RaiinTensu.OutTaxGen;
            KaikeiDetail.JihiTaxNr = RaiinTensu.InclTaxNr;
            KaikeiDetail.JihiTaxGen = RaiinTensu.InclTaxGen;
            KaikeiDetail.IsNinpu = RaiinTensu.IsNinpu;
            KaikeiDetail.IsZaiiso = RaiinTensu.IsZaiiso;

            _futancalcAggregate = new FutancalcListModel(KaikeiDetail, KaikeiDetails, OdrInfs);
            AdjustDetails.Clear();
            LimitListInfs.Clear();
            LimitCntListInfs.Clear();

            KaikeiDetails.ForEach(k => k.RoundKogakuPtFutan = SystemConf.RoundKogakuPtFutan);
            KaikeiDetail.RoundKogakuPtFutan = SystemConf.RoundKogakuPtFutan;

            for (int i = 0; i <= 1; i++)
            {
                KaikeiDetails.ForEach(k => k.RoundTo10en = i == 0);
                KaikeiDetail.RoundTo10en = i == 0;

                //主保険計算
                CalculateHoken();

                //マル長限度額
                int chokiLimit = 0;

                //公費計算
                for (int iKohiNo = 1; iKohiNo <= 4; iKohiNo++)
                {
                    CalculateKohi(iKohiNo, ref chokiLimit);
                    //特殊処理
                    CalculateKohiSp(iKohiNo);
                }

                //高額療養費計算
                CalculateKogaku(chokiLimit);

                //高額療養費（公費併用と保険単独の合算）
                CalculateKogakuTotal(chokiLimit);

                //国保減免（高額療養費及び、公費を適用した後の最終的な一部負担金相当額を減免する）                
                CalculateGenmen();
            }

            //上限額管理
            for (int iKohiNo = 1; iKohiNo <= 4; iKohiNo++)
            {
                SetLimitList(iKohiNo);
            }

            //患者負担
            KaikeiDetail.PtFutan = KaikeiDetail.IchibuFutan10en - KaikeiDetail.GenmenGaku10en;
            if ((KaikeiDetail.KogakuOverKbn == KogakuOverStatus.OverOneYen || KaikeiDetail.IchibuFutan10en % 10 != 0) &&
                !KaikeiDetail.IsKohiLimitOver)
            {
                if (KaikeiDetail.KogakuOverKbn == KogakuOverStatus.OverOneYen ||
                    (KaikeiDetail.KogakuKbn == 41 && KaikeiDetail.IchibuFutan10en % 10 != 0)) //配慮措置適用の日がある場合、高額上限に達した日が1円単位になるため丸める
                {
                    switch (SystemConf.RoundKogakuPtFutan)
                    {
                        case 1:  //10円単位(四捨五入)
                            int ptFutan = CIUtil.RoundInt(KaikeiDetail.IchibuFutan10en - KaikeiDetail.GenmenGaku10en, 1);
                            KaikeiDetail.IchibuFutan10en += ptFutan - KaikeiDetail.PtFutan;
                            KaikeiDetail.PtFutan = ptFutan;
                            break;
                        case 2:  //10円単位(切り捨て)
                            ptFutan = (int)Math.Truncate((double)(KaikeiDetail.IchibuFutan10en - KaikeiDetail.GenmenGaku10en) / 10) * 10;
                            KaikeiDetail.IchibuFutan10en += ptFutan - KaikeiDetail.PtFutan;
                            KaikeiDetail.PtFutan = ptFutan;
                            break;
                        default: //1円単位
                            KaikeiDetail.PtFutan = KaikeiDetail.IchibuFutan10en - KaikeiDetail.GenmenGaku10en;
                            break;
                    }
                }
                else
                {
                    KaikeiDetail.PtFutan = KaikeiDetail.IchibuFutan - KaikeiDetail.GenmenGaku;
                }
            }
            //患者負担(合算調整)
            AdjustDetails.ForEach(x =>
            {
                x.PtFutan = x.IchibuFutan10en;
                if (KaikeiDetail.KogakuOverKbn == KogakuOverStatus.OverOneYen && !KaikeiDetail.IsKohiLimitOver)
                {
                    if (KaikeiDetail.KogakuOverKbn == KogakuOverStatus.OverOneYen)
                    {
                        switch (SystemConf.RoundKogakuPtFutan)
                        {
                            case 1:  //10円単位(四捨五入)
                                int ptFutan = CIUtil.RoundInt(x.IchibuFutan10en - x.GenmenGaku10en, 1);
                                x.IchibuFutan10en += ptFutan - x.PtFutan;
                                x.PtFutan = ptFutan;
                                break;
                            case 2:  //10円単位(切り捨て)
                                ptFutan = (int)Math.Truncate((double)(x.IchibuFutan10en - x.GenmenGaku10en) / 10) * 10;
                                x.IchibuFutan10en += ptFutan - x.PtFutan;
                                x.PtFutan = ptFutan;
                                break;
                            default: //1円単位
                                x.PtFutan = x.IchibuFutan10en - x.GenmenGaku10en;
                                break;
                        }
                    }
                    else
                    {
                        x.PtFutan = x.IchibuFutan - x.GenmenGaku;
                    }
                }
            }
            );

            if (KaikeiDetail.KogakuOverKbn == KogakuOverStatus.Over)
            {
                int wrkPtFutan = _futancalcAggregate.GetTotalPtFutan(KaikeiDetail.HokenId) +
                    KaikeiDetail.PtFutan + AdjustDetails.Sum(x => x.PtFutan) +
                    KaikeiDetail.Kohi1OtherFutan + KaikeiDetail.Kohi2OtherFutan +
                    KaikeiDetail.Kohi3OtherFutan + KaikeiDetail.Kohi4OtherFutan;

                //窓口負担が上限に達していない場合は、まるめ誤差を考慮しない（＝1円単位の四捨五入）
                if (wrkPtFutan < KaikeiDetail.KogakuLimit && wrkPtFutan < KaikeiDetail.TotalKogakuLimit &&
                    (KaikeiDetail.Kohi1Limit == 0 || wrkPtFutan < KaikeiDetail.Kohi1Limit) &&
                    (KaikeiDetail.Kohi2Limit == 0 || wrkPtFutan < KaikeiDetail.Kohi2Limit) &&
                    (KaikeiDetail.Kohi3Limit == 0 || wrkPtFutan < KaikeiDetail.Kohi3Limit) &&
                    (KaikeiDetail.Kohi4Limit == 0 || wrkPtFutan < KaikeiDetail.Kohi4Limit))
                {
                    KaikeiDetail.PtFutan = CIUtil.RoundInt(KaikeiDetail.IchibuFutan - KaikeiDetail.GenmenGaku, 1);

                    if (AdjustDetails.Count >= 1)
                    {
                        AdjustDetails.ForEach(x => x.PtFutan = 0);
                        AdjustDetails[0].PtFutan = CIUtil.RoundInt(KaikeiDetail.IchibuFutan + AdjustDetails.Sum(x => x.IchibuFutan) - KaikeiDetail.GenmenGaku, 1) - KaikeiDetail.PtFutan;
                    }
                }
            }
            else if (KaikeiDetail.IsChoki && KaikeiDetail.ChokiKohiNo > 0 && !PtKohis.Any(p => p.PrefNo == 13 && p.CalcSpKbn == 1))
            {
                int wrkPtFutan = _futancalcAggregate.GetKohiFutan(KaikeiDetail.ChokiKohiNo, CountType.Month, PtKohis[KaikeiDetail.ChokiKohiNo - 1].LimitKbn) +
                    KaikeiDetail.PtFutan + AdjustDetails.Sum(x => x.PtFutan) +
                    KaikeiDetail.Kohi1OtherFutan + KaikeiDetail.Kohi2OtherFutan +
                    KaikeiDetail.Kohi3OtherFutan + KaikeiDetail.Kohi4OtherFutan;

                //窓口負担が上限に達していない場合は、まるめ誤差を考慮しない（＝1円単位の四捨五入）
                if (wrkPtFutan < KaikeiDetail.ChokiLimit &&
                    (KaikeiDetail.Kohi2Limit == 0 || KaikeiDetail.ChokiKohiNo >= 2 || wrkPtFutan < KaikeiDetail.Kohi2Limit) &&
                    (KaikeiDetail.Kohi3Limit == 0 || KaikeiDetail.ChokiKohiNo >= 3 || wrkPtFutan < KaikeiDetail.Kohi3Limit) &&
                    (KaikeiDetail.Kohi4Limit == 0 || KaikeiDetail.ChokiKohiNo >= 4 || wrkPtFutan < KaikeiDetail.Kohi4Limit))
                {
                    KaikeiDetail.PtFutan = CIUtil.RoundInt(KaikeiDetail.IchibuFutan - KaikeiDetail.GenmenGaku, 1);

                    if (AdjustDetails.Count >= 1)
                    {
                        AdjustDetails.ForEach(x => x.PtFutan = 0);
                        AdjustDetails[0].PtFutan = CIUtil.RoundInt(KaikeiDetail.IchibuFutan + AdjustDetails.Sum(x => x.IchibuFutan) - KaikeiDetail.GenmenGaku, 1) - KaikeiDetail.PtFutan;
                    }
                }
            }

            //労災・自賠
            int wrkFutan = CalculateRosaiJibai();
            if (wrkFutan > 0)
            {
                KaikeiDetail.Tensu += RaiinTensu.RousaiEnTensu;
                KaikeiDetail.TotalIryohi = wrkFutan;
                KaikeiDetail.PtFutan = wrkFutan * KaikeiDetail.PtRate / 100;
            }

            //会計情報の設定
            SetKaikeiInf(raiinAdjust, KaikeiDetail);
            foreach (KaikeiDetailModel kaikeiDetail in AdjustDetails)
            {
                SetKaikeiInf(raiinAdjust, kaikeiDetail);
            }

            //まるめ調整額をDetailに反映する
            KaikeiDetail.AdjustRound = raiinAdjust ? KaikeiInfs.Where(k => k.RaiinNo == KaikeiDetail.RaiinNo && k.HokenId == KaikeiDetail.HokenId).Sum(k => k.AdjustRound) : 0;
            if (KaikeiDetail.AdjustRound != 0)
            {
                AddCalcLog(1, string.Format("【負担金額】同一来院の患者負担調整を行いました。 調整額:{0}円", KaikeiDetail.AdjustRound));
            }
        }

        /// <summary>
        /// 主保険計算
        /// </summary>
        private void CalculateHoken()
        {
            //主保険がない場合
            if (KaikeiDetail.HokenId == 0) return;

            if (PtHoken == null) return;

            //法別番号
            KaikeiDetail.Houbetu = PtHoken.Houbetu;
            //本人家族区分
            KaikeiDetail.HonkeKbn = PtHoken.HonkeKbn;
            //高額療養費区分
            KaikeiDetail.KogakuKbn = PtHoken.KogakuKbn;
            //限度額特例フラグ
            KaikeiDetail.IsTokurei =
                PtHoken.TokureiYm1 == KaikeiDetail.SinDate / 100 || PtHoken.TokureiYm2 == KaikeiDetail.SinDate / 100;
            //多数回該当フラグ
            KaikeiDetail.IsTasukai =
                PtHoken.TasukaiYm > 0 && PtHoken.TasukaiYm <= KaikeiDetail.SinDate / 100;
            //国保減免区分
            KaikeiDetail.GenmenKbn = PtHoken.GenmenKbn;

            //主保険負担率
            KaikeiDetail.HokenRate = GetHokenRate(out string wrkReceSbt);
            //患者負担率
            KaikeiDetail.PtRate = KaikeiDetail.HokenRate;
            //点数単価
            KaikeiDetail.EnTen = PtHoken.EnTen;
            //レセプト種別
            KaikeiDetail.ReceSbt = wrkReceSbt;

            //総医療費
            KaikeiDetail.TotalIryohi = KaikeiDetail.Tensu * PtHoken.EnTen;

            if (PtHoken.FutanKbn == 0)
            {
                //負担なし
                KaikeiDetail.HokenFutan = KaikeiDetail.TotalIryohi;
                KaikeiDetail.IchibuFutan = 0;
            }
            else
            {
                //負担率
                KaikeiDetail.IchibuFutan = CIUtil.RoundInt((double)KaikeiDetail.TotalIryohi * KaikeiDetail.HokenRate / 100, 0);
                if (KaikeiDetail.RoundTo10en)
                {
                    KaikeiDetail.IchibuFutan = CIUtil.RoundInt(KaikeiDetail.IchibuFutan, 1);
                }

                //保険負担
                KaikeiDetail.HokenFutan = KaikeiDetail.TotalIryohi - KaikeiDetail.IchibuFutan;

                //船員保険特殊処理
                switch (PtHoken.SyokumuKbn)
                {
                    case 1:  //職務上（2010年1月以降は労災保険）
                    case 2:  //下船後3ヶ月以内
                        //負担なし
                        KaikeiDetail.HokenFutan += KaikeiDetail.IchibuFutan;
                        KaikeiDetail.IchibuFutan = 0;
                        break;
                    case 3:  //通勤災害（2010年1月以降は労災保険）
                        //初診時200円
                        break;
                }
            }
        }

        /// <summary>
        /// 主保険負担率計算
        /// </summary>
        /// <param name="receSbt">レセプト種別</param>
        /// <returns></returns>
        private int GetHokenRate(out string receSbt)
        {
            int wrkRate = PtHoken.FutanRate;
            char[] wrkReceSbt = new char[] { '1', '1', 'x', '2' };

            switch (PtHoken.HokenSbtKbn)
            {
                case 0:
                    //主保険なし
                    wrkReceSbt[1] = '2';
                    break;
                case 1:
                    //主保険
                    if (PtHoken.HonkeKbn == 2)
                    {
                        //家族
                        wrkReceSbt[3] = '6';
                    }

                    if (PtInf.IsPreSchool())
                    {
                        //６歳未満未就学児
                        wrkRate = 20;
                        wrkReceSbt[3] = '4';
                    }
                    else if (PtInf.IsElder() && PtHoken.Houbetu != "39")
                    {
                        wrkRate =
                            PtInf.IsElder20per() ? wrkRate = 20 :  //前期高齢
                            PtInf.IsElderExpat() ? wrkRate = 20 :  //75歳以上海外居住者
                            wrkRate = 10;

                        wrkReceSbt[3] = '8';
                    }
                    else if (PtHoken.Houbetu == "39")
                    {
                        //後期
                        wrkReceSbt[1] = '3';
                        wrkReceSbt[3] = '8';
                    }

                    if (PtHoken.Houbetu == "67")
                    {
                        //退職
                        wrkReceSbt[1] = '4';
                    }

                    int kogakuKbn = PtHoken.KogakuKbn;
                    if (PtInf.IsElder() || PtHoken.Houbetu == "39")
                    {
                        //過去分の計算に対する予防処置
                        if (new int[] { 26, 27, 28 }.Contains(kogakuKbn) && KaikeiDetail.SinDate < KaiseiDate.d20180801)
                        {
                            KaikeiDetail.KogakuKbn = 3;
                            kogakuKbn = 3;
                        }

                        if ((kogakuKbn == 3) ||  //(kogakuKbn == 3 && KaikeiDetail.SinDate < KaiseiDate.d20180801) ||
                            (kogakuKbn == 6) ||  //特定収入(～2008/12)
                            (new int[] { 26, 27, 28 }.Contains(kogakuKbn) && KaikeiDetail.SinDate >= KaiseiDate.d20180801))
                        {
                            //後期７割 or 高齢７割
                            wrkRate = 30;
                            wrkReceSbt[3] = '0';
                        }
                        else if (PtHoken.Houbetu == "39" && kogakuKbn == 41 && KaikeiDetail.SinDate >= KaiseiDate.d20221001)
                        {
                            //後期８割
                            wrkRate = 20;
                        }
                        else if (kogakuKbn == 41)
                        {
                            KaikeiDetail.KogakuKbn = 0;
                        }
                    }
                    break;
                case 8:
                    //自費
                    wrkReceSbt[0] = '9';
                    wrkReceSbt[1] = '0';
                    wrkReceSbt[3] = 'x';
                    if (PtHoken.ReceKisai == ReceKisai.None)
                    {
                        wrkReceSbt[0] = '8';
                    }
                    if (PtHoken.HokenNo == 68)
                    {
                        //特別療養費
                        if (PtHoken.HokensyaNo.Length == 6)
                        {
                            wrkReceSbt[1] = '1';  //一般
                        }
                        else if (PtHoken.HokensyaNo.Length == 8 && PtHoken.HokensyaNo.Substring(0, 2) == "67")
                        {
                            wrkReceSbt[1] = '4';  //退職
                        }
                        else if (PtHoken.HokensyaNo.Length == 8 && PtHoken.HokensyaNo.Substring(0, 2) == "39")
                        {
                            wrkReceSbt[1] = '3';  //後期
                        }
                    }
                    break;
                default:
                    wrkReceSbt[0] = 'x';
                    wrkReceSbt[1] = 'x';
                    wrkReceSbt[3] = 'x';
                    break;
            }

            receSbt = new string(wrkReceSbt);

            return wrkRate;
        }

        /// <summary>
        /// 公費計算
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        private void CalculateKohi(int kohiNo, ref int chokiLimit)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);

            //公費がない場合
            if (kohiId == 0) return;
            if (kohiNo > PtKohis.Count)
            {
                //公費IDを初期化
                KaikeiDetail.ClearKohiId(kohiNo);

                //throw new Exception(
                //    String.Format("KohiId:{0} 保険番号マスタが存在しない可能性があります。", kohiId)
                //);
                var logText = string.Format("【公費ID{0}】保険番号マスタが未設定か存在しない可能性があります。保険登録を確認してください。", kohiId);
                if (CalcLogs.Find(c => c.Text == logText) == null)
                {
                    AddCalcLog(2, logText);
                }
                return;
            }

            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            if (kohiInf == null) return;

            //自立支援減免の場合は全額助成されているため以降の公費は出番なし
            if (KaikeiDetail.GenmenGaku > 0 && KaikeiDetail.IchibuFutan == KaikeiDetail.GenmenGaku) return;

            //法別番号
            KaikeiDetail.SetKohiHoubetu(kohiNo, kohiInf.Houbetu);
            KaikeiDetail.SetKohiPriority(kohiNo, kohiInf.Priority);

            //高額療養費合算対象外
            if (KaikeiDetail.KogakuTotalKbn == KogakuTotalKbn.None &&
                KaikeiDetail.AgeKbn != AgeKbn.Elder)
            {
                if (kohiInf.KogakuTotalKbn == 1 && KaikeiDetail.HokenKbn == HokenKbn.Kokho ||
                    kohiInf.KogakuTotalKbn == 2 && KaikeiDetail.HokenKbn == HokenKbn.Syaho ||
                    kohiInf.KogakuTotalKbn == 3)
                {
                    KaikeiDetail.KogakuTotalKbn = KogakuTotalKbn.ExcludeKogakuTotal;
                }
                else if (kohiInf.KogakuTotalAll == 1 && KaikeiDetail.HokenKbn == HokenKbn.Syaho ||
                    kohiInf.KogakuTotalAll == 2 && KaikeiDetail.HokenKbn == HokenKbn.Kokho ||
                    kohiInf.KogakuTotalAll == 3)
                {
                    KaikeiDetail.KogakuTotalKbn = KogakuTotalKbn.IncludeKogakuTotal;
                }
            }

            int ichibuFutan = KaikeiDetail.IchibuFutan;
            int kohiFutan = 0;
            int otherFutan = 0;
            int kohiLimit = 0;
            //計算処理
            subCalculateKohi(ref ichibuFutan, ref kohiFutan, ref otherFutan, ref kohiLimit);

            //計算結果格納
            KaikeiDetail.SetKohiLimit(kohiNo, kohiLimit);

            //マル長が月単位計算の場合は高額療養費計算で処理する
            if (kohiInf.HokenSbtKbn == HokenSbtKbn.Choki)
            {
                if (SystemConf.ChokiDateRange == 1 ||
                    SystemConf.ChokiDateRange == 2 && KaikeiDetail.HokenKbn == HokenKbn.Syaho ||
                    SystemConf.ChokiDateRange == 3 && KaikeiDetail.HokenKbn == HokenKbn.Kokho ||
                    (
                        //東京マル都
                        kohiNo + 1 <= PtKohis.Count &&
                        PtKohis[kohiNo]?.PrefNo == PrefCode.Tokyo &&
                        PtKohis[kohiNo]?.CalcSpKbn == 1
                    )
                )
                {
                    //公費計算
                    KaikeiDetail.IsChoki = kohiFutan > 0;
                    KaikeiDetail.ChokiLimit = kohiLimit;
                    KaikeiDetail.ChokiKohiNo = kohiNo;
                }
                else
                {
                    //高額療養費計算
                    chokiLimit = kohiLimit;
                    return;
                }
            }

            //計算結果格納
            KaikeiDetail.IchibuFutan = ichibuFutan;
            KaikeiDetail.SetKohiFutan(kohiNo, kohiFutan);
            KaikeiDetail.SetOtherFutan(kohiNo, otherFutan);

            //既に上位の公費で上限に達している場合でも窓口負担を発生させる
            if (kohiInf.FutanYusen == 1 &&
                (kohiNo == 1 || kohiNo >= 2 && PtKohis[kohiNo - 2].HokenSbtKbn == HokenSbtKbn.Choki))
            {
                KaikeiDetail.YusenFutanKohiNo = kohiNo;

                if (kohiNo == 1)
                {
                    //今までの負担額を取得
                    int wrkFutan = _futancalcAggregate.GetKohiFutan(
                        kohiId, CountType.Month, kohiInf.LimitKbn
                    ) + KaikeiDetail.GetKohiFutan(kohiNo);

                    if (wrkFutan < ichibuFutan)
                    {
                        ichibuFutan = wrkFutan;
                    }
                    //発生させる負担額
                    KaikeiDetail.YusenFutan = ichibuFutan;
                }
                else
                {
                    ichibuFutan = KaikeiDetail.TotalIryohi * KaikeiDetail.PtRate / 100;
                    if (KaikeiDetail.RoundTo10en)
                    {
                        ichibuFutan = CIUtil.RoundInt(ichibuFutan, 1);
                    }
                    kohiFutan = 0;
                    //計算処理
                    subCalculateKohi(ref ichibuFutan, ref kohiFutan, ref otherFutan, ref kohiLimit);

                    //今までの負担額を取得
                    int wrkFutan = _futancalcAggregate.GetKohiFutan(
                        kohiId, CountType.Month, kohiInf.LimitKbn
                    ) + KaikeiDetail.GetKohiFutan(kohiNo);

                    if (wrkFutan < ichibuFutan)
                    {
                        ichibuFutan = wrkFutan;
                    }
                    //発生させる負担額
                    KaikeiDetail.YusenFutan = ichibuFutan;

                    if (KaikeiDetail.IchibuFutan < ichibuFutan)
                    {
                        int diffFutan = ichibuFutan - KaikeiDetail.IchibuFutan;

                        KaikeiDetail.AddKohiFutan(kohiNo, -diffFutan);
                        KaikeiDetail.IchibuFutan = ichibuFutan;
                    }
                }
            }

            //自立支援減免
            if (KaikeiDetail.GenmenKbn == GenmenKbn.Jiritusien &&
                KaikeiDetail.HokenKbn == HokenKbn.Kokho &&
                new string[] { "15", "16", "21" }.Contains(kohiInf.Houbetu) &&
                KaikeiDetail.IchibuFutan > 0)
            {
                KaikeiDetail.GenmenGaku = KaikeiDetail.IchibuFutan;
            }

            //計算処理
            void subCalculateKohi(ref int retIchibuFutan, ref int retKohiFutan, ref int retOtherFutan, ref int retLimitFutan)
            {
                if (kohiInf.FutanKbn == 0)
                {
                    //負担なし
                    retKohiFutan = retIchibuFutan;
                    retIchibuFutan = 0;

                    KaikeiDetail.IsKohiLimitOver = true;
                }
                else
                {
                    int wrkKohiFutan = 0;

                    //負担割合
                    if (kohiInf.FutanRate > 0)
                    {
                        int wrkRate = kohiInf.FutanRate;
                        if (wrkRate != kohiInf.Rate && kohiInf.Rate > 0)
                        {
                            wrkRate = kohiInf.Rate;
                        }
                        if (KaikeiDetail.PtRate > wrkRate)
                        {
                            KaikeiDetail.PtRate = wrkRate;

                            int wrkIchibu = KaikeiDetail.TotalIryohi * wrkRate / 100;
                            if (KaikeiDetail.RoundTo10en)
                            {
                                wrkIchibu = CIUtil.RoundInt(wrkIchibu, 1);
                            }

                            if (retIchibuFutan > wrkIchibu)
                            {
                                wrkKohiFutan = retIchibuFutan - wrkIchibu;
                                retIchibuFutan = wrkIchibu;
                            }
                            else if (kohiInf.HokenSbtKbn == HokenSbtKbn.Ippan &&
                                kohiInf.DayLimitCount == 0 && kohiInf.MonthLimitCount == 0 && kohiInf.KaiLimitFutan == 0 && kohiInf.DayLimitFutan == 0 &&
                                kohiInf.MonthLimitFutan >= 0)
                            {
                                //上限 = 今までの公費負担 + 今回負担
                                int wrkFutan = _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn, KaikeiDetail.HokenPid) + retIchibuFutan;
                                //負担率の公費の場合、月上限までは負担あり
                                if (wrkIchibu < wrkFutan)
                                {
                                    wrkKohiFutan = retIchibuFutan - wrkIchibu;
                                    retIchibuFutan = wrkIchibu;
                                }
                                else if (wrkFutan > 0)
                                {
                                    wrkKohiFutan = retIchibuFutan - wrkFutan;
                                    retIchibuFutan = wrkFutan;
                                }
                            }
                        }
                    }

                    //負担割合適用後の一部負担
                    int rateIchibuFutan = retIchibuFutan;

                    //上限回数
                    foreach (int countType in Enum.GetValues(typeof(CountType)))
                    {
                        int maxCount = 0;

                        switch (countType)
                        {
                            case (int)CountType.Day:
                                maxCount = kohiInf.DayLimitCount;
                                break;
                            case (int)CountType.Month:
                                maxCount = kohiInf.MonthLimitCount;
                                break;
                        }

                        if (maxCount <= 0) continue;

                        //今までの負担回数を取得
                        int wrkCount = _futancalcAggregate.GetKohiUsageCount(
                            kohiId, (CountType)Enum.ToObject(typeof(CountType), countType),
                            kohiInf.CountKbn, kohiInf.LimitKbn, countType == (int)CountType.Month && kohiInf.DayLimitFutan > 0
                        );

                        if (wrkCount >= maxCount)
                        {
                            wrkKohiFutan += retIchibuFutan;
                            retIchibuFutan = 0;
                        }
                    }

                    //上限額
                    foreach (int intCountType in Enum.GetValues(typeof(CountType)))
                    {
                        int limitFutan = 0;
                        CountType countType = (CountType)Enum.ToObject(typeof(CountType), intCountType);

                        switch (intCountType)
                        {
                            case (int)CountType.Times:
                                limitFutan = kohiInf.KaiLimitFutan;
                                break;
                            case (int)CountType.Day:
                                limitFutan = kohiInf.DayLimitFutan;
                                break;
                            case (int)CountType.Month:
                                limitFutan = kohiInf.MonthLimitFutan;
                                //マル長 限度額特例
                                if (kohiInf.HokenSbtKbn == HokenSbtKbn.Choki && KaikeiDetail.IsTokurei)
                                {
                                    limitFutan = limitFutan / 2;
                                }
                                break;
                        }

                        if (limitFutan <= 0) continue;

                        retLimitFutan = limitFutan;

                        //今までの負担額を取得
                        int wrkFutan = _futancalcAggregate.GetKohiIchibuFutan(kohiId, countType, kohiInf.LimitKbn);

                        //int wrkIchibuFutan = KaikeiDetail.IchibuFutan;

                        //一部負担の調整
                        if (wrkFutan + retIchibuFutan >= limitFutan)
                        {
                            wrkKohiFutan += retIchibuFutan - (limitFutan - wrkFutan);
                            retIchibuFutan = limitFutan - wrkFutan;
                        }
                    }

                    //上限額管理
                    if (kohiInf.IsLimitList == IsLimitList.Yes && kohiInf.MonthLimitFutan > 0)
                    {
                        int limitFutan = kohiInf.MonthLimitFutan;
                        //今までの他院負担額を取得
                        retOtherFutan = LimitListOthers.Where(x => x.KohiId == kohiId && x.RaiinNo == 0).Sum(x => x.FutanGaku);
                        //今までの負担額を取得
                        int wrkMyFutan = _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                        int wrkFutan = wrkMyFutan + retOtherFutan;

                        //一部負担の調整
                        if (retOtherFutan > limitFutan)
                        {
                            //他院負担額だけで上限を超える（本来あり得ないケース）場合は、自院分を返金する..を凍結
                            //wrkKohiFutan += retIchibuFutan + wrkMyFutan;
                            //retIchibuFutan = - wrkMyFutan;

                            //他院負担額だけで上限を超える（本来あり得ないケース）場合は、負担なしにする
                            wrkKohiFutan += retIchibuFutan;
                            retIchibuFutan = 0;
                        }
                        else if (wrkFutan >= limitFutan)
                        {
                            //他院分で上限を超過する金額を登録した後に自院に来院した場合は返金を発生させる..を凍結
                            //wrkKohiFutan += retIchibuFutan - (limitFutan - wrkFutan);
                            //retIchibuFutan = limitFutan - wrkFutan;
                            wrkKohiFutan += retIchibuFutan;
                            retIchibuFutan = 0;
                        }
                        else if (wrkFutan + retIchibuFutan >= limitFutan)
                        {
                            wrkKohiFutan += retIchibuFutan - (limitFutan - wrkFutan);
                            retIchibuFutan = limitFutan - wrkFutan;
                            if (retIchibuFutan < 0)
                            {
                                wrkKohiFutan -= retIchibuFutan;
                                retIchibuFutan = 0;
                            }
                        }
                    }

                    retKohiFutan = wrkKohiFutan;

                    //公費上限に達したか？
                    if (rateIchibuFutan > retIchibuFutan)
                    {
                        KaikeiDetail.IsKohiLimitOver = true;
                    }
                }
            }

        }

        /// <summary>
        /// 公費計算特殊処理
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        private void CalculateKohiSp(int kohiNo)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);

            //公費がない場合
            if (kohiId == 0) return;
            if (kohiNo > PtKohis.Count) return;

            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            if (kohiInf == null) return;
            //特殊処理対象の公費
            if (kohiInf.CalcSpKbn == 0) return;

            int ichibuFutan = KaikeiDetail.IchibuFutan;
            int genmenGaku = KaikeiDetail.GenmenGaku;
            int kohiFutan = 0;

            //計算処理
            switch (kohiInf.PrefNo)
            {
                //04.宮城県
                case PrefCode.Miyagi:
                    CalculateMiyagi(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //05.秋田県
                case PrefCode.Akita:
                    CalculateAkita(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //08.茨城県
                case PrefCode.Ibaraki:
                    CalculateIbaraki(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //11.埼玉県
                case PrefCode.Saitama:
                    CalculateSaitama(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //12.千葉県
                case PrefCode.Chiba:
                    CalculateChiba(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //13.東京都
                case PrefCode.Tokyo:
                    CalculateTokyo(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //22.静岡県
                case PrefCode.Shizuoka:
                    CalculateShizuoka(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //27.大阪府
                case PrefCode.Osaka:
                    CalculateOsaka(kohiNo, ref ichibuFutan, ref genmenGaku, ref kohiFutan);
                    break;

                //30.和歌山県
                case PrefCode.Wakayama:
                    CalculateWakayama(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //34.広島県
                case PrefCode.Hiroshima:
                    CalculateHiroshima(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //40.福岡県
                case PrefCode.Fukuoka:
                    CalculateFukuoka(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;

                //43.熊本県
                case PrefCode.Kumamoto:
                    CalculateKumamoto(kohiNo, ref ichibuFutan, ref kohiFutan);
                    break;
            }

            ////計算結果格納
            KaikeiDetail.IchibuFutan = ichibuFutan;
            KaikeiDetail.GenmenGaku = genmenGaku;
            KaikeiDetail.AddKohiFutan(kohiNo, kohiFutan);
        }

        /// <summary>
        /// 04.宮城県（特殊処理）
        ///  1: 一部負担金相当額 - {80,100円 ＋ （総医療費 - 267,000円）×1％｝（一般の上限額）を患者が窓口で支払い
        ///    ※社保または国保組合(市町村国保又は４国保組合以外)で、認定証が提示されなかった場合のみ
        ///  2: 1の処理 + 初診時500円再診時無料       
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateMiyagi(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                // 2: 初診時のみ負担あり、再診時無料
                case 2:
                    if (!_futancalcAggregate.IsOdrSyoshin(kohiId, kohiInf.LimitKbn, false))
                    {
                        //負担なし（初診２科目も負担なし）
                        kohiFutan = ichibuFutan;
                        ichibuFutan = 0;
                    }
                    break;
            }

            switch (kohiInf.CalcSpKbn)
            {
                case 1:
                case 2:
                    //保険者番号下6桁の3桁目が"3" は組合国保（４国保組合以外）
                    bool kokhoUnion = PtHoken.IsKokKumiai && !kohiInf.ExceptHokensya;

                    if (
                            KaikeiDetail.AgeKbn == AgeKbn.Ippan &&                 //こども医療費助成なので70歳未満しかいない
                            KaikeiDetail.KogakuKbn == 0 &&                           //限度額認定証の提示がない
                            (KaikeiDetail.HokenKbn == HokenKbn.Syaho || kokhoUnion)  //社保または国保組合(市町村国保又は４国保組合以外)
                        )
                    {
                        //一律「区分ウ」と見なして額を算出
                        var kogakuLimit = KogakuLimit(28);
                        int limitFutan = kogakuLimit.Limit;

                        //今回の自己負担相当額
                        int wrkIchibu = ichibuFutan + kohiFutan + KaikeiDetail.GetKohiFutan(kohiNo);
                        //今までの負担額を取得
                        int wrkFutan =
                            _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn) +
                            _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                        //今までの負担額が高額療養費限度額を超えている場合
                        int preKogakuLimit = _futancalcAggregate.GetPreKogakuLimit();
                        if (wrkFutan > preKogakuLimit)
                        {
                            wrkFutan = preKogakuLimit;
                        }

                        if (wrkFutan + wrkIchibu > limitFutan)
                        {
                            //上限を超えた分を患者に請求する
                            int limitOver = wrkFutan + wrkIchibu - limitFutan;
                            //- _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn) - ichibuFutan;
                            if (KaikeiDetail.RoundTo10en)
                            {
                                limitOver = CIUtil.RoundInt(limitOver, 1);
                            }

                            kohiFutan -= limitOver;
                            ichibuFutan += limitOver;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 05.秋田県（特殊処理）
        ///  1: 半額助成（全国公費適用前の患者負担の5割）
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateAkita(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                // 1: 半額助成（全国公費適用前の患者負担の5割）
                case 1:
                    if (ichibuFutan >= 0)
                    {
                        int wrkIchibu = CIUtil.RoundInt((double)KaikeiDetail.TotalIryohi * KaikeiDetail.HokenRate / 100 * 50 / 100, 0);
                        if (KaikeiDetail.RoundTo10en)
                        {
                            wrkIchibu = CIUtil.RoundInt(wrkIchibu, 1);
                        }

                        if (ichibuFutan > wrkIchibu)
                        {
                            kohiFutan = ichibuFutan - wrkIchibu;
                            ichibuFutan = wrkIchibu;
                        }
                        //int wrkIchibu = RoundOff((double)(ichibuFutan + GetKohiFutan(kohiNo)) * 50 / 100, 0);
                        //if (KaikeiDetail.RoundTo10En)
                        //{
                        //    wrkIchibu = RoundOff(wrkIchibu, -1);
                        //}

                        //if (ichibuFutan > wrkIchibu)
                        //{
                        //    kohiFutan = ichibuFutan - wrkIchibu;
                        //    ichibuFutan = wrkIchibu;
                        //}
                    }
                    break;
            }
        }

        /// <summary>
        /// 08.茨城県（特殊処理）
        ///  1: 実日数にカウントしない検査のみの来院は患者負担なし
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateIbaraki(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                //1: 実日数にカウントしない検査のみの来院は患者負担なし
                case 1:
                    if (!RaiinTensu.JituNisu)
                    {
                        kohiFutan = ichibuFutan;
                        ichibuFutan = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// 11.埼玉県（特殊処理）
        ///  1: 上限21,000円までは通常通りだが、上限を超えたら全額償還になる
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateSaitama(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                // 1: 上限21,000円までは通常通りだが、上限を超えたら全額償還になる
                case 1:
                    if (kohiInf.MonthSpLimit > 0)
                    {
                        //今回の自己負担相当額
                        int wrkIchibu = ichibuFutan + KaikeiDetail.GetKohiFutan(kohiNo);
                        //今までの負担額を取得
                        int wrkFutan =
                            _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn) +
                            _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);

                        if (wrkFutan > kohiInf.MonthSpLimit)
                        {
                            //前回までに自己負担相当額の合計が上限を超えている場合は全額負担
                            kohiFutan = -KaikeiDetail.GetKohiFutan(kohiNo);
                            ichibuFutan = wrkIchibu;
                        }
                        else if (wrkFutan + wrkIchibu > kohiInf.MonthSpLimit)
                        {
                            //今回で自己負担相当額の合計が上限を超えている場合は前回分と合わせて全額負担
                            int wrkKohiFutan = _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                            kohiFutan = -(KaikeiDetail.GetKohiFutan(kohiNo) + wrkKohiFutan);
                            ichibuFutan = wrkIchibu + wrkKohiFutan;

                            var logText = string.Format("【負担金額】今回分で当月{0:#,0}円を超えたため、償還払い切替処理を行いました。", kohiInf.MonthSpLimit);
                            if (CalcLogs.Find(c => c.Text == logText) == null)
                            {
                                AddCalcLog(1, logText);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 12.千葉県（特殊処理）
        ///  1: ？？
        ///  2: 一部負担金相当額 - {80,100円 ＋ （総医療費 - 267,000円）×1％｝（一般の上限額）を患者が窓口で支払い
        ///    ※県外国保組合で、認定証が提示されなかった場合のみ
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateChiba(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                case 2:
                    if (
                            KaikeiDetail.AgeKbn == AgeKbn.Ippan &&                          //70歳未満
                            KaikeiDetail.KogakuKbn == 0 &&                                  //限度額認定証の提示がない
                            PtHoken.IsKokKumiai && !PtHoken.IsKokPrefIn                     //県外国保組合
                        )
                    {
                        //一律「区分ウ」と見なして額を算出
                        var kogakuLimit = KogakuLimit(28);
                        int limitFutan = kogakuLimit.Limit;

                        //今回の自己負担相当額
                        int wrkIchibu = ichibuFutan + kohiFutan + KaikeiDetail.GetKohiFutan(kohiNo);
                        //今までの負担額を取得
                        int wrkFutan =
                            _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn) +
                            _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                        //今までの負担額が高額療養費限度額を超えている場合
                        int preKogakuLimit = _futancalcAggregate.GetPreKogakuLimit();
                        if (wrkFutan > preKogakuLimit)
                        {
                            wrkFutan = preKogakuLimit;
                        }

                        if (wrkFutan + wrkIchibu > limitFutan)
                        {
                            //上限を超えた分を患者に請求する
                            int limitOver = wrkFutan + wrkIchibu - limitFutan;
                            if (KaikeiDetail.RoundTo10en)
                            {
                                limitOver = CIUtil.RoundInt(limitOver, 1);
                            }

                            kohiFutan -= limitOver;
                            ichibuFutan += limitOver;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 13.東京都（特殊処理）
        ///  1: 上限額までは患者負担無、上限額を超えた分が患者負担
        ///  2: 実日数にカウントしない検査のみの来院は患者負担なし
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateTokyo(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                //1: 上限額までは患者負担無、上限額を超えた分が患者負担
                case 1:
                    if (kohiInf.MonthSpLimit > 0)
                    {
                        //今回の自己負担相当額
                        int wrkIchibu = ichibuFutan + KaikeiDetail.GetKohiFutan(kohiNo);
                        //今までの負担額を取得
                        int wrkFutan =
                            _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn) +
                            _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);

                        if (wrkIchibu < 0)
                        {
                            //マイナスの場合は調整なし
                        }
                        else if (wrkFutan >= kohiInf.MonthSpLimit)
                        {
                            //前回までに自己負担相当額の合計が上限に達している場合は全額負担
                            kohiFutan = -KaikeiDetail.GetKohiFutan(kohiNo);
                            ichibuFutan = wrkIchibu;
                        }
                        else if (wrkFutan + wrkIchibu > kohiInf.MonthSpLimit)
                        {
                            //自己負担相当額の合計が上限を超える場合は、超えた分を負担する
                            kohiFutan = ichibuFutan - (wrkFutan + wrkIchibu - kohiInf.MonthSpLimit);
                            ichibuFutan = wrkFutan + wrkIchibu - kohiInf.MonthSpLimit;
                        }
                    }
                    break;
                //2: 実日数にカウントしない検査のみの来院は患者負担なし
                case 2:
                    if (!RaiinTensu.JituNisu)
                    {
                        kohiFutan = ichibuFutan;
                        ichibuFutan = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// 22.静岡県（特殊処理）
        ///  1: 月の上限回数に他院分も含める
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateShizuoka(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                // 1: 月の上限回数に他院分も含める
                case 1:
                    if (kohiInf.MonthLimitCount == 0) break;

                    //今までの負担回数を取得
                    int wrkCount = _futancalcAggregate.GetKohiUsageCount(
                        kohiId, (CountType)Enum.ToObject(typeof(CountType), CountType.Month),
                        kohiInf.CountKbn, kohiInf.LimitKbn, false
                    );

                    //他院分の負担回数を合算
                    wrkCount = wrkCount +
                        LimitCntListOthers.Where(x => x.KohiId == kohiId && x.OyaRaiinNo == 0).ToList().Count;

                    if (wrkCount >= kohiInf.MonthLimitCount)
                    {
                        kohiFutan += ichibuFutan;
                        ichibuFutan = 0;
                    }

                    //上限回数管理データ作成
                    LimitCntListInfModel limitCntListInf = new LimitCntListInfModel(new LimitCntListInf());

                    int wrkPos = LimitCntListInfs.FindIndex(x => x.KohiId == kohiId);
                    if (wrkPos >= 0)
                    {
                        limitCntListInf = LimitCntListInfs[wrkPos];
                    }

                    limitCntListInf.HpId = KaikeiDetail.HpId;
                    limitCntListInf.PtId = KaikeiDetail.PtId;
                    limitCntListInf.KohiId = kohiId;
                    limitCntListInf.SinDate = KaikeiDetail.SinDate;
                    limitCntListInf.HokenPid = KaikeiDetail.HokenPid;
                    limitCntListInf.SortKey = KaikeiDetail.SortKey;
                    limitCntListInf.OyaRaiinNo = KaikeiDetail.OyaRaiinNo;

                    if (wrkPos >= 0)
                    {
                        LimitCntListInfs[wrkPos] = limitCntListInf;
                    }
                    else
                    {
                        LimitCntListInfs.Add(limitCntListInf);
                    }

                    break;
            }
        }

        /// <summary>
        /// 27.大阪府（特殊処理）
        ///  1: 一部負担金相当額 - {80,100円 ＋ （総医療費 - 267,000円）×1％｝（一般の上限額）を患者が窓口で支払い
        ///    ※70歳未満国保の府外保険者で、認定証が提示されなかった場合のみ
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="genmenGaku">減免額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateOsaka(int kohiNo, ref int ichibuFutan, ref int genmenGaku, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                case 1:
                    if (
                            KaikeiDetail.AgeKbn == AgeKbn.Ippan &&          //70歳未満
                            KaikeiDetail.KogakuKbn == 0 &&                  //限度額認定証の提示がない
                            PtHoken.IsKokPrefOut                            //府外国保
                        )
                    {
                        //一律「区分ウ」と見なして額を算出
                        var kogakuLimit = KogakuLimit(28);
                        int limitFutan = kogakuLimit.Limit;

                        //今回の自己負担相当額
                        int wrkIchibu = ichibuFutan + kohiFutan + KaikeiDetail.GetKohiFutan(kohiNo);
                        //今までの負担額を取得
                        int wrkFutan =
                            _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn) +
                            _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                        //今までの負担額が高額療養費限度額を超えている場合
                        int preKogakuLimit = _futancalcAggregate.GetPreKogakuLimit();
                        if (wrkFutan > preKogakuLimit)
                        {
                            wrkFutan = preKogakuLimit;
                        }

                        if (wrkFutan + wrkIchibu > limitFutan)
                        {
                            //上限を超えた分を患者に請求する
                            int limitOver = wrkFutan + wrkIchibu - limitFutan;
                            if (KaikeiDetail.RoundTo10en)
                            {
                                limitOver = CIUtil.RoundInt(limitOver, 1);
                            }

                            kohiFutan -= limitOver;
                            //レセプトの公費一部負担金記載に影響しないよう減免額扱い
                            //ichibuFutan += limitOver;
                            genmenGaku -= limitOver;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 30.和歌山県（特殊処理）
        ///  1: 上限26,700点までは通常通りだが、上限を超えたら全額償還になる
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateWakayama(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                // 1: 上限26,700点までは通常通りだが、上限を超えたら全額償還になる
                case 1:
                    if (kohiInf.MonthSpLimit > 0)
                    {
                        //今回の自己負担相当額
                        int wrkIchibu = ichibuFutan;
                        //今までの負担額を取得
                        int wrkFutan = _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                        //今までの点数を取得
                        int wrkTensu = _futancalcAggregate.GetKohiTensu(kohiId, CountType.Month, kohiInf.LimitKbn);

                        if (wrkTensu > kohiInf.MonthSpLimit)
                        {
                            //前回までに自己負担相当額の合計が上限を超えている場合は全額負担
                            kohiFutan = -KaikeiDetail.GetKohiFutan(kohiNo);
                            ichibuFutan = wrkIchibu;
                        }
                        else if (wrkTensu + KaikeiDetail.Tensu > kohiInf.MonthSpLimit)
                        {
                            //今回で自己負担相当額の合計が上限を超えている場合は前回分と合わせて全額負担
                            int wrkKohiFutan = _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                            kohiFutan = -(KaikeiDetail.GetKohiFutan(kohiNo) + wrkKohiFutan);
                            ichibuFutan = wrkIchibu + wrkKohiFutan;

                            var logText = string.Format("【負担金額】今回分で当月{0:#,0}点を超えたため、償還払い切替処理を行いました。", kohiInf.MonthSpLimit);
                            if (CalcLogs.Find(c => c.Text == logText) == null)
                            {
                                AddCalcLog(1, logText);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 34.広島県（特殊処理）
        ///  1: 初診料算定時のみ負担あり
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateHiroshima(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                // 1: 初診料算定時のみ負担あり
                case 1:
                    if (!_futancalcAggregate.IsOdrSyoshin(kohiId, kohiInf.LimitKbn, true))
                    {
                        //負担なし
                        kohiFutan = ichibuFutan;
                        ichibuFutan = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// 40.福岡県（特殊処理）
        ///  1: 初診料・往診料に係る費用のみ徴収
        ///  3: 3歳まで無料
        ///  4: 4歳まで無料
        ///  5: 5歳まで無料
        ///  6: 未就学児無料
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateFukuoka(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            switch (kohiInf.CalcSpKbn)
            {
                // 1: 初診料・往診料に係る費用のみ徴収
                case 1:
                    //※未実装
                    break;
                // 3..5: 3..5歳まで無料
                case 3:
                case 4:
                case 5:
                    if (!PtInf.IsAge(kohiInf.CalcSpKbn))
                    {
                        //負担なし
                        kohiFutan = ichibuFutan;
                        ichibuFutan = 0;
                    }
                    break;
                // 6: 未就学児無料
                case 6:
                    if (PtInf.IsPreSchool())
                    {
                        //負担なし
                        kohiFutan = ichibuFutan;
                        ichibuFutan = 0;
                    }
                    break;
            }
        }


        /// <summary>
        /// 43.熊本県（特殊処理）
        ///  1: 一部負担金の1/3を自己負担（小数点切り上げ、1円単位の窓口負担）
        ///  2: 一部負担金の1/3を自己負担（小数点切り上げ、1円単位の窓口負担）+ 21,000円超で全額償還
        ///  3: 21,000円超で全額償還
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <param name="ichibuFutan">一部負担額</param>
        /// <param name="kohiFutan">公費負担額</param>
        private void CalculateKumamoto(int kohiNo, ref int ichibuFutan, ref int kohiFutan)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);
            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            bool IsSyokan = false;

            //21,000円超で全額償還
            if (kohiInf.CalcSpKbn == 2 || kohiInf.CalcSpKbn == 3)
            {
                if (kohiInf.MonthSpLimit > 0)
                {
                    //今回の自己負担相当額
                    int wrkIchibu = ichibuFutan + KaikeiDetail.GetKohiFutan(kohiNo);
                    //今までの負担額を取得
                    int wrkFutan =
                        _futancalcAggregate.GetKohiIchibuFutan(kohiId, CountType.Month, kohiInf.LimitKbn) +
                        _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);

                    if (wrkFutan > kohiInf.MonthSpLimit)
                    {
                        //前回までに自己負担相当額の合計が上限を超えている場合は全額負担
                        kohiFutan = -KaikeiDetail.GetKohiFutan(kohiNo);
                        ichibuFutan = wrkIchibu;
                        IsSyokan = true;
                    }
                    else if (wrkFutan + wrkIchibu > kohiInf.MonthSpLimit)
                    {
                        //今回で自己負担相当額の合計が上限を超えている場合は前回分と合わせて全額負担
                        int wrkKohiFutan = _futancalcAggregate.GetKohiFutan(kohiId, CountType.Month, kohiInf.LimitKbn);
                        kohiFutan = -(KaikeiDetail.GetKohiFutan(kohiNo) + wrkKohiFutan);
                        ichibuFutan = wrkIchibu + wrkKohiFutan;
                        IsSyokan = true;
                    }
                }
            }

            //一部負担金の1/3を自己負担（小数点切り上げ、1円単位の窓口負担）
            if (kohiInf.CalcSpKbn == 1 || kohiInf.CalcSpKbn == 2)
            {
                if (ichibuFutan >= 0 && !IsSyokan)
                {
                    //窓口は1円単位なので丸めない
                    int wrkIchibu = (int)Math.Ceiling((double)(ichibuFutan + KaikeiDetail.GetKohiFutan(kohiNo)) / 3);

                    if (ichibuFutan > wrkIchibu)
                    {
                        kohiFutan = ichibuFutan - wrkIchibu;
                        ichibuFutan = wrkIchibu;
                    }
                }
            }
        }

        /// <summary>
        /// 上限額管理票に記載する金額の設定
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        private void SetLimitList(int kohiNo)
        {
            int kohiId = HokenPattern.KohiNoToId(kohiNo);

            //公費がない場合
            if (kohiId == 0) return;
            if (kohiNo > PtKohis.Count) return;

            PtKohiModel kohiInf = PtKohis[kohiNo - 1];

            if (kohiInf == null) return;

            //負担なし
            //if (kohiInf.FutanKbn == 0) return;

            //上限額管理
            if (kohiInf.IsLimitList != IsLimitList.Yes) return;

            LimitListInfModel limitListInf = new LimitListInfModel(new LimitListInf());

            int wrkPos = LimitListInfs.FindIndex(x => x.KohiId == kohiId);
            if (wrkPos >= 0)
            {
                limitListInf = LimitListInfs[wrkPos];
            }

            limitListInf.HpId = KaikeiDetail.HpId;
            limitListInf.PtId = KaikeiDetail.PtId;
            limitListInf.KohiId = kohiId;
            limitListInf.SinDate = KaikeiDetail.SinDate;
            limitListInf.HokenPid = KaikeiDetail.HokenPid;
            limitListInf.SortKey = KaikeiDetail.SortKey;
            limitListInf.RaiinNo = KaikeiDetail.RaiinNo;
            //患者負担額
            limitListInf.FutanGaku =
                KaikeiDetail.Kohi1Id == kohiId ? KaikeiDetail.IchibuFutan10en + KaikeiDetail.Kohi2Futan10en + KaikeiDetail.Kohi3Futan10en + KaikeiDetail.Kohi4Futan10en :
                KaikeiDetail.Kohi2Id == kohiId ? KaikeiDetail.IchibuFutan10en + KaikeiDetail.Kohi3Futan10en + KaikeiDetail.Kohi4Futan10en :
                KaikeiDetail.Kohi3Id == kohiId ? KaikeiDetail.IchibuFutan10en + KaikeiDetail.Kohi4Futan10en :
                KaikeiDetail.IchibuFutan10en;

            foreach (var adjustDetail in AdjustDetails)
            {
                limitListInf.FutanGaku +=
                    adjustDetail.Kohi1Id == kohiId ? adjustDetail.IchibuFutan10en + adjustDetail.Kohi2Futan10en + adjustDetail.Kohi3Futan10en + adjustDetail.Kohi4Futan10en :
                    adjustDetail.Kohi2Id == kohiId ? adjustDetail.IchibuFutan10en + adjustDetail.Kohi3Futan10en + adjustDetail.Kohi4Futan10en :
                    adjustDetail.Kohi3Id == kohiId ? adjustDetail.IchibuFutan10en + adjustDetail.Kohi4Futan10en :
                    adjustDetail.Kohi4Id == kohiId ? adjustDetail.IchibuFutan10en : 0;
            }

            //総額表示
            if (kohiInf.IsLimitListSum == IsLimitListSum.Yes)
            {
                limitListInf.TotalGaku = KaikeiDetail.TotalIryohi;
            }

            if (wrkPos >= 0)
            {
                LimitListInfs[wrkPos] = limitListInf;
            }
            else
            {
                LimitListInfs.Add(limitListInf);
            }

        }

        /// <summary>
        /// 高額療養費の限度額取得
        /// </summary>
        /// <param name="totalCheck">true: 総医療費(公費併用と保険単独の合算)に対する上限</param>
        /// <returns></returns>
        private dynamic KogakuLimit(int kogakuKbn, bool totalCheck = false)
        {
            if (totalCheck)
            {
                List<KaikeiDetailModel> totalDetails = new List<KaikeiDetailModel>(KaikeiDetails);
                totalDetails.Add(KaikeiDetail);

                var wrkDetails = totalDetails.Where(d =>
                    d.HokenId == KaikeiDetail.HokenId ||
                    //後期は月中で保険が変わっても通算で上限をかける
                    KaikeiDetail.IsKouki && d.IsKouki
                ).GroupBy(d => d.KogakuTekiyoKbn).ToList();

                if (wrkDetails.Count() == 1)
                {
                    kogakuKbn = KaikeiDetail.KogakuTekiyoKbn;
                }
            }
            else
            {
                int kogakuTekiyo = PtKohis.Find(k => k.HokenSbtKbn != HokenSbtKbn.Choki)?.KogakuTekiyo ?? 11;

                //高額療養費適用区分
                int tekiyoKbn =
                    KaikeiDetail.HokenKbn == HokenKbn.Syaho ? kogakuTekiyo / 10 :
                    kogakuTekiyo % 10;

                if (tekiyoKbn == 3)
                {
                    //一般(上位優先)
                    //  ※1日目一般、2日目所得区分等のケースに対応するためには、2日目の後に1日目を計算する必要がある（2回計算が必要）
                    var tekiyoKbns = KaikeiTotalDetails.Where(d =>
                        (
                            d.HokenId == KaikeiDetail.HokenId ||
                            //後期は月中で保険が変わっても通算で上限をかける
                            KaikeiDetail.IsKouki && d.IsKouki
                        ) &&
                        (
                            KaikeiDetail.AgeKbn == AgeKbn.Elder ? d.KogakuTekiyoKbn != 0 : !new int[] { 0, 18, 28 }.Contains(d.KogakuTekiyoKbn)
                        ) &&
                        (
                            !(d.RaiinNo == KaikeiDetail.RaiinNo && d.HokenPid == KaikeiDetail.HokenPid)
                        )
                    )
                    .GroupBy(d => d.KogakuTekiyoKbn)
                    .Select(d => d.Key)
                    .ToList();

                    tekiyoKbn = 0;
                    if (tekiyoKbns.Count() >= 1)
                    {
                        if (KaikeiDetail.AgeKbn == AgeKbn.Elder)
                        {
                            tekiyoKbn = tekiyoKbns.First() == 4 ? 2 : 1;
                        }
                        else
                        {
                            tekiyoKbn = new int[] { 19, 30 }.Contains(tekiyoKbns.First()) ? 2 : 1;
                        }
                    }
                }

                switch (tekiyoKbn)
                {
                    case 0:  //一般
                        kogakuKbn =
                            KaikeiDetail.AgeKbn == AgeKbn.Elder ? 0 :
                            kogakuKbn >= 17 && kogakuKbn <= 19 ? 18 :
                            kogakuKbn >= 26 && kogakuKbn <= 30 ? 28 :
                            kogakuKbn;
                        KaikeiDetail.IsKogakuTekiyoIppan = true;
                        break;
                    case 2:  //低所
                        kogakuKbn =
                            KaikeiDetail.AgeKbn == AgeKbn.Elder ? 4 :
                            kogakuKbn >= 17 && kogakuKbn <= 19 ? 19 :
                            kogakuKbn >= 26 && kogakuKbn <= 30 ? 30 :
                            kogakuKbn;
                        break;
                    default: //所得区分
                        break;
                }
            }
            //実際に適用された所得区分
            KaikeiDetail.KogakuTekiyoKbn = kogakuKbn;

            var kogakuLimit = _futancalcFinder.FindKogakuLimit(
                KaikeiDetail.HpId, KaikeiDetail.SinDate, KaikeiDetail.AgeKbn,
                kogakuKbn, KaikeiDetail.IsTasukai
            );

            int limitFutan = kogakuLimit.Limit;
            int adjustFutan = kogakuLimit.Adjust;

            if (limitFutan > 0)
            {
                //総医療費合計
                int totalIryohi =
                    _futancalcAggregate.GetTotalIryohi(
                        KaikeiDetail.HokenPid * Convert.ToInt32(!totalCheck), KaikeiDetail.HokenId
                    );

                if (adjustFutan > 0)
                {
                    if (KaikeiDetail.IsTokurei)
                    {
                        //限度額特例
                        if (totalIryohi > adjustFutan / 2)
                        {
                            limitFutan =
                                CIUtil.RoundInt(
                                    limitFutan / 2 + (totalIryohi - adjustFutan / 2) * 0.01, 0
                                );
                        }
                    }
                    else
                    {
                        if (totalIryohi > adjustFutan)
                        {
                            limitFutan =
                                CIUtil.RoundInt(
                                    limitFutan + (totalIryohi - adjustFutan) * 0.01, 0
                                );
                        }
                    }
                }
                else
                {
                    if (KaikeiDetail.IsTokurei)
                    {
                        //限度額特例
                        limitFutan = limitFutan / 2;
                    }
                }

                //後期高齢２割 配慮措置
                if (KaikeiDetail.KogakuKbn == 41 && !totalCheck && !PtKohis.Any(p => p.HokenSbtKbn == HokenSbtKbn.Choki))
                {
                    var kohiInf = PtKohis.FirstOrDefault();

                    if (kohiInf == null ||
                        (kohiInf.KogakuHairyoKbn == 1 && PtHoken.IsKokPrefOut) ||
                        (kohiInf.KogakuHairyoKbn == 2 && PtHoken.IsKokPrefIn) ||
                        (kohiInf.KogakuHairyoKbn == 3))
                    {
                        int wrkLimitFutan = 6000;
                        int wrkAdjustFutan = 30000;

                        if (totalIryohi > wrkAdjustFutan)
                        {
                            wrkLimitFutan =
                                CIUtil.RoundInt(
                                    wrkLimitFutan + (totalIryohi - wrkAdjustFutan) * 0.1, 0
                                );

                            if (limitFutan > wrkLimitFutan)
                            {
                                limitFutan = wrkLimitFutan;
                            }
                        }
                    }
                }

                //上限額
                if (totalCheck)
                {
                    KaikeiDetail.TotalKogakuLimit = limitFutan;
                }
                else
                {
                    KaikeiDetail.KogakuLimit = limitFutan;
                }
            }
            return new
            {
                Limit = limitFutan,
                Adjust = adjustFutan
            };
        }

        /// <summary>
        /// 高額療養費計算処理
        /// </summary>
        private void CalculateKogaku(int chokiLimit)
        {
            //限度額を取得
            var kogakuLimit = KogakuLimit(KaikeiDetail.KogakuKbn);
            if (KaikeiDetail.KogakuTotalKbn == KogakuTotalKbn.IncludeKogakuTotal)
            {
                kogakuLimit = KogakuLimit(KaikeiDetail.KogakuKbn, true);
            }
            int limitFutan = kogakuLimit.Limit;
            int adjustFutan = kogakuLimit.Adjust;

            bool bChoki = false;
            if (chokiLimit > 0 && (chokiLimit < limitFutan || limitFutan == 0))
            {
                limitFutan = chokiLimit;
                adjustFutan = 0;
                bChoki = true;
            }

            if (limitFutan <= 0) return;

            //限度額
            KaikeiDetail.KogakuLimit = limitFutan;

            int getFutan(int seqNo)
            {
                switch (seqNo)
                {
                    case 1:
                        return KaikeiDetail.IchibuFutan;
                    case 2:
                        return KaikeiDetail.Kohi4Futan;
                    case 3:
                        return KaikeiDetail.Kohi3Futan;
                    case 4:
                        return KaikeiDetail.Kohi2Futan;
                    case 5:
                        return KaikeiDetail.Kohi1Futan;
                    default:
                        return 0;
                }
            }

            void setFutan(int seqNo, int value)
            {
                switch (seqNo)
                {
                    case 1:
                        KaikeiDetail.IchibuFutan = value;
                        break;
                    case 2:
                        KaikeiDetail.Kohi4Futan = value;
                        break;
                    case 3:
                        KaikeiDetail.Kohi3Futan = value;
                        break;
                    case 4:
                        KaikeiDetail.Kohi2Futan = value;
                        break;
                    case 5:
                        KaikeiDetail.Kohi1Futan = value;
                        break;
                }
            }

            void decFutan(int kohiNo, int value)
            {
                switch (kohiNo)
                {
                    case 1:
                        KaikeiDetail.Kohi1Futan -= value;
                        break;
                    case 2:
                        KaikeiDetail.Kohi2Futan -= value;
                        break;
                    case 3:
                        KaikeiDetail.Kohi3Futan -= value;
                        break;
                    case 4:
                        KaikeiDetail.Kohi4Futan -= value;
                        break;
                }
            }

            int getSeqNo(int kohiNo)
            {
                switch (kohiNo)
                {
                    case 1: return 5;
                    case 2: return 4;
                    case 3: return 3;
                    case 4: return 2;
                    default:
                        return 0;
                }
            }

            //負担率の公費の場合は、月上限まで一部負担を発生させるために公費負担の調整が必要
            int adjustKohiNo = 0;
            int adjustKohiId = PtKohis.Find(k => k.HokenSbtKbn == HokenSbtKbn.Bunten)?.HokenId ?? 0;
            if (adjustKohiId > 0)
            {
                var ptKohi = PtKohis.Find(k => k.HokenId == adjustKohiId);
                if (ptKohi.FutanRate > 0 && ptKohi.MonthLimitFutan > 0 &&
                    ptKohi.KaiLimitFutan == 0 && ptKohi.DayLimitFutan == 0)
                {
                    adjustKohiNo = PtKohis.FindIndex(k => k.HokenId == adjustKohiId) + 1;
                }
            }

            //今までの負担額を取得
            int wrkFutan = _futancalcAggregate.GetKogakuFutan(KaikeiDetail.HokenPid);

            int wrkIchibu = 0;
            bool limitOver = false;
            int decKohiFutan = 0;

            for (int i = 0; i <= 5; i++)
            {
                wrkIchibu += getFutan(i);

                if (wrkFutan + wrkIchibu >= limitFutan)
                {
                    if (limitOver)
                    {
                        if (getSeqNo(adjustKohiNo) > i)
                        {
                            decKohiFutan += getFutan(i);
                            KaikeiDetail.KogakuFutan += getFutan(i);
                        }
                        else
                        {
                            KaikeiDetail.KogakuFutan += getFutan(i);
                            setFutan(i, 0);
                        }
                    }
                    else
                    {
                        limitOver = true;
                        if (!bChoki && KaikeiDetail.TotalIryohi > 0 && !(KaikeiDetail.IsChoki && KaikeiDetail.ChokiLimit < limitFutan))
                        {
                            KaikeiDetail.KogakuOverKbn =
                                Convert.ToInt32(limitFutan > 0) + Convert.ToInt32(adjustFutan > 0);

                            if (KaikeiDetail.KogakuKbn == 41 &&
                                limitFutan % 10 >= 1)
                            {
                                KaikeiDetail.KogakuOverKbn = KogakuOverStatus.OverOneYen;
                            }
                        }

                        int wrkBuf = limitFutan - wrkFutan;

                        if (getSeqNo(adjustKohiNo) > i)
                        {
                            decKohiFutan += wrkIchibu - wrkBuf;
                            KaikeiDetail.KogakuFutan += wrkIchibu - wrkBuf;
                        }
                        else
                        {
                            KaikeiDetail.KogakuFutan += wrkIchibu - wrkBuf;
                            setFutan(i, getFutan(i) - KaikeiDetail.KogakuFutan);
                        }
                    }
                }

                if (bChoki &&
                    KaikeiDetail.KogakuOverKbn == KogakuOverStatus.None &&
                    kogakuLimit.Limit > 0 &&
                    wrkFutan + wrkIchibu > kogakuLimit.Limit
                    )
                {
                    KaikeiDetail.KogakuOverKbn = KogakuOverStatus.Over;
                }
            }
            //公費負担のマイナス調整
            if (adjustKohiId > 0)
            {
                int kohiFutan = _futancalcAggregate.GetKohiFutan(
                    adjustKohiId, CountType.Month,
                    PtKohis.Find(k => k.HokenId == adjustKohiId).LimitKbn) + getFutan(getSeqNo(adjustKohiNo)
                );

                if (kohiFutan >= decKohiFutan)
                {
                    decFutan(adjustKohiNo, decKohiFutan);
                }
                else if (kohiFutan == 0)
                {
                    if (KaikeiDetail.IchibuFutan >= decKohiFutan)
                    {
                        KaikeiDetail.IchibuFutan -= decKohiFutan;
                    }
                    else
                    {
                        KaikeiDetail.IchibuFutan = 0;
                    }
                }
                else
                {
                    decFutan(adjustKohiNo, kohiFutan);
                    KaikeiDetail.KogakuFutan += decKohiFutan - kohiFutan;
                }
            }
            //マル長に公費給付額を含む場合の特殊処理
            if (SystemConf.ChokiFutan == 0 ||
                SystemConf.ChokiFutan == 1 && KaikeiDetail.HokenKbn == HokenKbn.Syaho ||
                SystemConf.ChokiFutan == 2 && KaikeiDetail.HokenKbn == HokenKbn.Kokho)
            {
                if (PtKohis.Any(k => k.HokenSbtKbn == HokenSbtKbn.Bunten))
                {
                    int wrkLimit = limitFutan - wrkFutan;

                    if (KaikeiDetail.IchibuFutan > wrkLimit)
                    {
                        for (int i = 4; i >= 1; i--)
                        {
                            int kohiId = KaikeiDetail.GetKohiId(i);
                            if (kohiId >= 1 &&
                                PtKohis.Any(k => k.HokenId == kohiId && k.HokenSbtKbn == HokenSbtKbn.Ippan))
                            {
                                //既に上限に達している場合は一部負担が発生しないように公費負担に振り替える
                                int kohiFutan = KaikeiDetail.IchibuFutan - wrkLimit;

                                KaikeiDetail.AddKohiFutan(i, kohiFutan);
                                KaikeiDetail.IchibuFutan -= kohiFutan;
                                break;
                            }
                        }
                    }
                }
            }

            //既に高額療養費で上限に達している場合でも窓口負担を発生させる
            if (KaikeiDetail.YusenFutan > 0 && KaikeiDetail.IchibuFutan < KaikeiDetail.YusenFutan)
            {
                int kohiNo = KaikeiDetail.YusenFutanKohiNo;
                //今までの負担額を取得
                //wrkFutan = _futancalcAggregate.GetKohiFutan(
                //    PtKohis[kohiNo - 1].HokenId, CountType.Month, PtKohis[kohiNo - 1].LimitKbn
                //) + KaikeiDetail.GetKohiFutan(kohiNo);

                //if (wrkFutan < KaikeiDetail.YusenFutan)
                //{
                //    KaikeiDetail.YusenFutan = wrkFutan;
                //}

                //一部負担上限の設定
                int limitIchibu = limitFutan - _futancalcAggregate.GetKogakuIchibuFutan(KaikeiDetail.HokenPid);
                if (limitIchibu < KaikeiDetail.YusenFutan)
                {
                    //一部負担が上限を超えないように調整
                    KaikeiDetail.YusenFutan = limitIchibu;
                }
                int diffFutan = KaikeiDetail.YusenFutan - KaikeiDetail.IchibuFutan;

                KaikeiDetail.AddKohiFutan(kohiNo, -diffFutan);
                KaikeiDetail.IchibuFutan = KaikeiDetail.YusenFutan;
            }

            //高額療養費が負担してマル長が未使用になった場合はフラグを戻す
            if (KaikeiDetail.IsChoki && KaikeiDetail.ChokiKohiNo > 0 && KaikeiDetail.GetKohiFutan(KaikeiDetail.ChokiKohiNo) == 0)
            {
                KaikeiDetail.IsChoki = false;
            }

            //マル長上限超
            if (bChoki && KaikeiDetail.KogakuFutan > 0)
            {
                KaikeiDetail.IsChoki = true;
            }
        }

        /// <summary>
        /// 高額療養費(公費併用と保険単独の合算)
        /// </summary>
        private void CalculateKogakuTotal(int chokiLimit)
        {
            KaikeiDetail.AdjustKid =
                PtKohis.Find(k => k.HokenSbtKbn == HokenSbtKbn.Bunten)?.HokenId ??
                PtKohis.Find(k => k.HokenSbtKbn == HokenSbtKbn.Seiho)?.HokenId ??
                PtKohis.Find(k => k.KogakuTotalExcFutan == 1)?.HokenId ?? 0;

            List<KaikeiDetailModel> totalDetails = new List<KaikeiDetailModel>(KaikeiDetails);
            totalDetails.Add(KaikeiDetail);
            totalDetails.AddRange(KaikeiAdjustDetails);
            totalDetails.ForEach(k => k.RoundTo10en = KaikeiDetail.RoundTo10en);

            List<KaikeiDetailModel> preDetails = new List<KaikeiDetailModel>(KaikeiDetails);
            preDetails.AddRange(KaikeiAdjustDetails);
            preDetails.ForEach(k => k.RoundTo10en = KaikeiDetail.RoundTo10en);

            //List<int> adjustKids = new List<int>();

            int wrkIchibu = 0;
            //限度額を取得
            var kogakuLimit = KogakuLimit(KaikeiDetail.KogakuKbn, true);
            int limitFutan = kogakuLimit.Limit;
            int adjustFutan = kogakuLimit.Adjust;

            //マル長に公費給付額を含むかどうか
            bool isChokiIncludeKohiFutan = false;
            if (SystemConf.ChokiFutan == 0 ||
                SystemConf.ChokiFutan == 1 && KaikeiDetail.HokenKbn == HokenKbn.Syaho ||
                SystemConf.ChokiFutan == 2 && KaikeiDetail.HokenKbn == HokenKbn.Kokho)
            {
                isChokiIncludeKohiFutan = true;

                if (chokiLimit > 0 &&
                    limitFutan > 0 &&
                    chokiLimit > limitFutan &&
                    !KaikeiDetail.IsKogakuTekiyoIppan)
                {
                    chokiLimit = limitFutan;
                }
            }

            //マル長を持つ保険パターンが複数ある場合は、マル長上限に対して合算調整が必要
            bool isChokiAdjust =
                chokiLimit > 0 ?
                    totalDetails
                    .Where
                        (d =>
                            (
                                d.HokenId == KaikeiDetail.HokenId ||
                                d.IsKouki && KaikeiDetail.IsKouki   //後期は月中で保険が変わっても通算で上限をかける
                            ) &&
                            d.Kohi1Id == KaikeiDetail.Kohi1Id     //マル長を持つ保険パターンのみ集計する
                        )
                    .GroupBy(d => d.HokenPid).ToList().Count() >= 2 : false;

            bool bChoki = false;
            if (chokiLimit > 0 && (chokiLimit < limitFutan || limitFutan == 0 || chokiLimit == limitFutan && isChokiIncludeKohiFutan) && isChokiAdjust)
            {
                limitFutan = chokiLimit;
                adjustFutan = 0;
                bChoki = true;
            }
            else
            {
                isChokiIncludeKohiFutan = false;
            }

            if (limitFutan == 0) return;

            //月単位で分点公費に優先して負担させるためにソートする
            var wrkDetails = totalDetails
                .Where
                    (d =>
                        (
                            d.HokenId == KaikeiDetail.HokenId ||
                            d.IsKouki && KaikeiDetail.IsKouki   //後期は月中で保険が変わっても通算で上限をかける
                        ) &&
                        (
                            !bChoki || bChoki && d.Kohi1Id == KaikeiDetail.Kohi1Id  //マル長の場合はマル長を持つ保険パターンのみ集計する
                        )
                    )
                .OrderByDescending(d => d.AdjustKid).ThenBy(d => d.HokenId)
                .GroupBy(d => new { d.HokenPid, d.AdjustKid, d.Kohi1Id, d.Kohi2Id, d.Kohi3Id, d.Kohi4Id }).ToList();

            if (wrkDetails.Count() < 2) return;

            //今までの負担額
            int preFutan = preDetails
                .Where
                    (d =>
                        (
                            d.HokenId == KaikeiDetail.HokenId ||
                            d.IsKouki && KaikeiDetail.IsKouki   //後期は月中で保険が変わっても通算で上限をかける
                        ) &&
                        (
                            !bChoki || bChoki && d.Kohi1Id == KaikeiDetail.Kohi1Id  //マル長の場合はマル長を持つ保険パターンのみ集計する
                        )
                    )
                .Sum
                    (d =>
                        d.AdjustKid == 0 ? d.IchibuFutan + d.Kohi1Futan + d.Kohi2Futan + d.Kohi3Futan + d.Kohi4Futan :
                        d.Kohi1Id == d.AdjustKid ? d.IchibuFutan + (isChokiIncludeKohiFutan ? d.Kohi1Futan : 0) + d.Kohi2Futan + d.Kohi3Futan + d.Kohi4Futan :
                        d.Kohi2Id == d.AdjustKid ? d.IchibuFutan + (isChokiIncludeKohiFutan ? d.Kohi2Futan : 0) + d.Kohi3Futan + d.Kohi4Futan :
                        d.Kohi3Id == d.AdjustKid ? d.IchibuFutan + (isChokiIncludeKohiFutan ? d.Kohi3Futan : 0) + d.Kohi4Futan :
                        d.IchibuFutan
                    );

            //今回の一部負担上限
            int limitIchibu = limitFutan - preFutan;
            int baselimitIchibu = limitIchibu;

            //既に高額療養費で上限に達している場合でも窓口負担を発生させる
            if (KaikeiDetail.YusenFutan > 0 && limitIchibu < KaikeiDetail.YusenFutan)
            {
                int kohiNo = KaikeiDetail.YusenFutanKohiNo;
                //今までの負担額を取得
                int wrkFutan = _futancalcAggregate.GetKohiFutan(
                    PtKohis[kohiNo - 1].HokenId, CountType.Month, PtKohis[kohiNo - 1].LimitKbn
                ) + KaikeiDetail.GetKohiFutan(kohiNo);

                if (wrkFutan < KaikeiDetail.YusenFutan)
                {
                    KaikeiDetail.YusenFutan = wrkFutan;
                }

                wrkFutan = limitFutan - preDetails
                    .Where
                        (d =>
                            (
                                d.HokenId == KaikeiDetail.HokenId ||
                                d.IsKouki && KaikeiDetail.IsKouki   //後期は月中で保険が変わっても通算で上限をかける
                            ) &&
                            (
                                !bChoki || bChoki && d.Kohi1Id == KaikeiDetail.Kohi1Id  //マル長の場合はマル長を持つ保険パターンのみ集計する
                            )
                        )
                    .Sum
                        (d =>
                            d.Kohi1Id == PtKohis[kohiNo - 1].HokenId ? d.IchibuFutan + d.Kohi2Futan + d.Kohi3Futan + d.Kohi4Futan :
                            d.Kohi2Id == PtKohis[kohiNo - 1].HokenId ? d.IchibuFutan + d.Kohi3Futan + d.Kohi4Futan :
                            d.Kohi3Id == PtKohis[kohiNo - 1].HokenId ? d.IchibuFutan + d.Kohi4Futan :
                            d.IchibuFutan
                        );
                if (wrkFutan < KaikeiDetail.YusenFutan)
                {
                    KaikeiDetail.YusenFutan = wrkFutan;
                }
            }

            foreach (var wrkGroup in wrkDetails)
            {
                KaikeiDetailModel adjustDetail;
                //int wrkPos = AdjustDetails.FindIndex(x => x.AdjustPid == wrkGroup.Key.HokenPid);
                int wrkPos = AdjustDetails.FindIndex(x => x.HokenPid == wrkGroup.Key.HokenPid && x.AdjustPid == KaikeiDetail.HokenPid);
                if (wrkPos >= 0)
                {
                    adjustDetail = AdjustDetails[wrkPos];
                }
                else
                {
                    adjustDetail = new KaikeiDetailModel(
                        new KaikeiDetail()
                        {
                            HpId = KaikeiDetail.HpId,
                            PtId = KaikeiDetail.PtId,
                            SinDate = KaikeiDetail.SinDate,
                            RaiinNo = KaikeiDetail.RaiinNo,
                            OyaRaiinNo = KaikeiDetail.OyaRaiinNo,
                            HokenPid = wrkGroup.Key.HokenPid,
                            AdjustPid = KaikeiDetail.HokenPid,  //wrkGroup.Key.HokenPid;
                            AdjustKid = wrkGroup.Key.AdjustKid,
                            HokenId = totalDetails.Find(x => x.HokenPid == wrkGroup.Key.HokenPid).HokenId,
                            Kohi1Id = totalDetails.Find(x => x.HokenPid == wrkGroup.Key.HokenPid).Kohi1Id,
                            Kohi2Id = totalDetails.Find(x => x.HokenPid == wrkGroup.Key.HokenPid).Kohi2Id,
                            Kohi3Id = totalDetails.Find(x => x.HokenPid == wrkGroup.Key.HokenPid).Kohi3Id,
                            Kohi4Id = totalDetails.Find(x => x.HokenPid == wrkGroup.Key.HokenPid).Kohi4Id,
                            ReceSbt = KaikeiDetail.ReceSbt,
                            SortKey = KaikeiDetail.SortKey
                        }
                    );
                }
                adjustDetail.RoundTo10en = KaikeiDetail.RoundTo10en;

                int getFutan(int seqNo)
                {
                    int kohiId =
                        seqNo == 1 ? wrkGroup.Key.Kohi1Id :
                        seqNo == 2 ? wrkGroup.Key.Kohi2Id :
                        seqNo == 3 ? wrkGroup.Key.Kohi3Id :
                        seqNo == 4 ? wrkGroup.Key.Kohi4Id :
                        0;

                    int maxFutan =
                        totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid && d.Kohi1Id == kohiId).Sum(d => d.Kohi1Futan) +
                        totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid && d.Kohi2Id == kohiId).Sum(d => d.Kohi2Futan) +
                        totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid && d.Kohi3Id == kohiId).Sum(d => d.Kohi3Futan) +
                        totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid && d.Kohi4Id == kohiId).Sum(d => d.Kohi4Futan);

                    if (seqNo == 5)
                    {
                        maxFutan = totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d => d.IchibuFutan);
                    }

                    switch (seqNo)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            return
                                KaikeiDetail.Kohi1Id == kohiId ? Math.Min(KaikeiDetail.Kohi1Futan, maxFutan) :
                                KaikeiDetail.Kohi2Id == kohiId ? Math.Min(KaikeiDetail.Kohi2Futan, maxFutan) :
                                KaikeiDetail.Kohi3Id == kohiId ? Math.Min(KaikeiDetail.Kohi3Futan, maxFutan) :
                                KaikeiDetail.Kohi4Id == kohiId ? Math.Min(KaikeiDetail.Kohi4Futan, maxFutan) :
                                0;
                        case 5:
                            return Math.Min(KaikeiDetail.IchibuFutan, maxFutan);
                        case 6:
                            return preDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d => d.Kohi1Futan) + (KaikeiDetail.Kohi1Id == kohiId ? Math.Max(KaikeiDetail.Kohi1Futan - maxFutan, 0) : 0);
                        case 7:
                            return preDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d => d.Kohi2Futan) + (KaikeiDetail.Kohi2Id == kohiId ? Math.Max(KaikeiDetail.Kohi2Futan - maxFutan, 0) : 0);
                        case 8:
                            return preDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d => d.Kohi3Futan) + (KaikeiDetail.Kohi3Id == kohiId ? Math.Max(KaikeiDetail.Kohi3Futan - maxFutan, 0) : 0);
                        case 9:
                            return preDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d => d.Kohi4Futan) + (KaikeiDetail.Kohi4Id == kohiId ? Math.Max(KaikeiDetail.Kohi4Futan - maxFutan, 0) : 0);
                        case 10:
                            return preDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d => d.IchibuFutan);
                        default:
                            return 0;
                    }
                }

                void setFutan(int seqNo, int value)
                {
                    int wrkSeqNo = seqNo >= 6 ? seqNo - 5 : seqNo;

                    switch (wrkSeqNo)
                    {
                        case 1:
                            adjustDetail.Kohi1Futan += value;
                            break;
                        case 2:
                            adjustDetail.Kohi2Futan += value;
                            break;
                        case 3:
                            adjustDetail.Kohi3Futan += value;
                            break;
                        case 4:
                            adjustDetail.Kohi4Futan += value;
                            break;
                        case 5:
                            adjustDetail.IchibuFutan += value;
                            break;
                    }
                }

                bool chkKohiId(int seqNo, int value)
                {
                    if (value == 0)
                    {
                        return false;
                    }

                    int wrkSeqNo = seqNo >= 6 ? seqNo - 5 : seqNo;

                    var wrkTotal = totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid);

                    int kohiNo =
                        wrkTotal.Where(d => d.Kohi1Id == value).Count() >= 1 ? 1 :
                        wrkTotal.Where(d => d.Kohi2Id == value).Count() >= 1 ? 2 :
                        wrkTotal.Where(d => d.Kohi3Id == value).Count() >= 1 ? 3 :
                        wrkTotal.Where(d => d.Kohi4Id == value).Count() >= 1 ? 4 : 0;

                    if (kohiNo >= 1 && wrkSeqNo <= kohiNo)
                    {
                        return true;
                    }

                    return false;
                }

                //同一保険パターンの月内の一部負担相当額を取得
                int wrkFutan = totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d =>
                    wrkGroup.Key.AdjustKid == 0 ? d.IchibuFutan + d.Kohi1Futan + d.Kohi2Futan + d.Kohi3Futan + d.Kohi4Futan :
                    d.Kohi1Id == wrkGroup.Key.AdjustKid ? d.IchibuFutan + (isChokiIncludeKohiFutan ? d.Kohi1Futan : 0) + d.Kohi2Futan + d.Kohi3Futan + d.Kohi4Futan :
                    d.Kohi2Id == wrkGroup.Key.AdjustKid ? d.IchibuFutan + (isChokiIncludeKohiFutan ? d.Kohi2Futan : 0) + d.Kohi3Futan + d.Kohi4Futan :
                    d.Kohi3Id == wrkGroup.Key.AdjustKid ? d.IchibuFutan + (isChokiIncludeKohiFutan ? d.Kohi3Futan : 0) + d.Kohi4Futan :
                    d.IchibuFutan
                );
                //同一保険パターンの月内の給付対象額を取得
                int wrkKyufu = totalDetails.Where(d => d.HokenPid == wrkGroup.Key.HokenPid).Sum(d =>
                    d.IchibuFutan + d.Kohi1Futan + d.Kohi2Futan + d.Kohi3Futan + d.Kohi4Futan + d.KogakuFutan
                );

                //公費併用と保険単独の療養が併せて行われている場合、
                //70歳未満では一部負担金等がそれぞれ21,000円以上で公費の費用徴収額があることが条件
                if (KaikeiDetail.AgeKbn == AgeKbn.Elder || bChoki ||
                    wrkKyufu >= KogakuIchibu.BorderVal && KaikeiDetail.KogakuTotalKbn != KogakuTotalKbn.ExcludeKogakuTotal ||
                    KaikeiDetail.KogakuTotalKbn == KogakuTotalKbn.IncludeKogakuTotal)
                {
                    if (wrkIchibu + wrkFutan >= limitFutan)
                    {
                        int kogakuFutan = wrkIchibu + wrkFutan - limitFutan;
                        if (wrkIchibu >= limitFutan)
                        {
                            kogakuFutan = wrkFutan;
                        }
                        adjustDetail.KogakuFutan = kogakuFutan;

                        #region 'マル長特殊処理　日単位計算'
                        if (SystemConf.ChokiDateRange == 1 ||
                            SystemConf.ChokiDateRange == 2 && KaikeiDetail.HokenKbn == HokenKbn.Syaho ||
                            SystemConf.ChokiDateRange == 3 && KaikeiDetail.HokenKbn == HokenKbn.Kokho)
                        {
                            var totalChoki = totalDetails
                                .Any
                                    (d =>
                                        (
                                            d.HokenId == KaikeiDetail.HokenId ||
                                            d.IsKouki && KaikeiDetail.IsKouki   //後期は月中で保険が変わっても通算で上限をかける
                                        ) &&
                                        d.Kohi1Id == KaikeiDetail.Kohi1Id &&    //マル長を持つ保険パターンのみ集計する                                         
                                        d.Kohi1Houbetu == "102"
                                    );

                            if (totalChoki)
                            {
                                for (int i = 1; i <= 5; i++)
                                {
                                    if (kogakuFutan == 0)
                                    {
                                        break;
                                    }
                                    if (chkKohiId(i, isChokiIncludeKohiFutan ? 0 : wrkGroup.Key.AdjustKid))
                                    {
                                        continue;
                                    }

                                    int wrkBuf =
                                        i == 1 ? KaikeiDetail.Kohi1Futan :
                                        i == 2 ? KaikeiDetail.Kohi2Futan :
                                        i == 3 ? KaikeiDetail.Kohi3Futan :
                                        i == 4 ? KaikeiDetail.Kohi4Futan :
                                        KaikeiDetail.IchibuFutan;

                                    if (wrkBuf > 0 &&
                                        KaikeiDetail.AdjustKid > 0)
                                    {
                                        int wrkKohiNo =
                                            KaikeiDetail.Kohi1Id == KaikeiDetail.AdjustKid ? 2 :
                                            KaikeiDetail.Kohi2Id == KaikeiDetail.AdjustKid ? 3 :
                                            KaikeiDetail.Kohi3Id == KaikeiDetail.AdjustKid ? 4 :
                                            5;

                                        if (i >= wrkKohiNo && wrkKohiNo < 5 && KaikeiDetail.GetKohiId(wrkKohiNo) > 0)
                                        {
                                            int wrkAdjust = wrkBuf <= kogakuFutan ? wrkBuf : kogakuFutan;

                                            if (i == 5)
                                            {
                                                KaikeiDetail.IchibuFutan -= wrkAdjust;
                                            }
                                            else
                                            {
                                                KaikeiDetail.AddKohiFutan(i, -wrkAdjust);
                                            }

                                            for (int j = 1; j <= 5; j++)
                                            {
                                                if (KaikeiDetail.GetKohiId(j) == KaikeiDetail.AdjustKid)
                                                {
                                                    KaikeiDetail.AddKohiFutan(j, wrkAdjust);
                                                    adjustDetail.KogakuFutan -= wrkAdjust;
                                                    kogakuFutan -= wrkAdjust;
                                                    break;
                                                }
                                                else if (j == 5)
                                                {
                                                    KaikeiDetail.IchibuFutan += wrkAdjust;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        //２併 or 分点公費との３併の場合、公費併用分の一部負担を優先する
                        for (int i = 1; i <= 10; i++)
                        {
                            if (kogakuFutan == 0)
                            {
                                break;
                            }
                            if (chkKohiId(i, isChokiIncludeKohiFutan ? 0 : wrkGroup.Key.AdjustKid))
                            {
                                continue;
                            }

                            int wrkBuf = getFutan(i);

                            if (wrkBuf < 0 && isChokiIncludeKohiFutan)
                            {
                                //念のためisChokiIncludeKohiFutanに限定
                            }
                            else if (kogakuFutan >= wrkBuf)
                            {
                                setFutan(i, -wrkBuf);
                                if (adjustDetail.AdjustKid > 0 &&
                                    adjustDetail.GetKohiNo(adjustDetail.AdjustKid) > 0 &&
                                    KaikeiDetail.AdjustKid == 0)
                                {
                                    setFutan(adjustDetail.GetKohiNo(adjustDetail.AdjustKid), wrkBuf);
                                }
                                kogakuFutan -= wrkBuf;
                                limitIchibu = Math.Max(limitIchibu - wrkBuf, 0);
                            }
                            else
                            {
                                setFutan(i, -kogakuFutan);
                                if (adjustDetail.AdjustKid > 0 &&
                                    adjustDetail.GetKohiNo(adjustDetail.AdjustKid) > 0 &&
                                    KaikeiDetail.AdjustKid == 0)
                                {
                                    setFutan(adjustDetail.GetKohiNo(adjustDetail.AdjustKid), kogakuFutan);
                                }
                                kogakuFutan = 0;
                                limitIchibu = 0;
                            }
                        }

                        for (int i = 4; i >= 1; i--)
                        {
                            int kohiId =
                                i == 1 ? wrkGroup.Key.Kohi1Id :
                                i == 2 ? wrkGroup.Key.Kohi2Id :
                                i == 3 ? wrkGroup.Key.Kohi3Id :
                                i == 4 ? wrkGroup.Key.Kohi4Id :
                                0;
                            if (kohiId >= 1 &&
                                kohiId != KaikeiDetail.AdjustKid &&
                                PtKohis.Any(k => k.HokenId == kohiId && k.HokenSbtKbn == HokenSbtKbn.Ippan))
                            {
                                var wrkKohiNo = adjustDetail.GetKohiNo(kohiId);
                                if (wrkKohiNo == 0) continue;

                                //返金を発生させない
                                if (adjustDetail.IchibuFutan < 0)
                                {
                                    //既に上限に達している場合は一部負担を発生させない
                                    if (KaikeiDetail.IchibuFutan > 0)
                                    {
                                        var futan = Math.Min(baselimitIchibu, KaikeiDetail.IchibuFutan);
                                        if (KaikeiDetail.IchibuFutan <= futan)
                                        {
                                            adjustDetail.AddKohiFutan(wrkKohiNo, adjustDetail.IchibuFutan);
                                            adjustDetail.IchibuFutan = 0;
                                        }
                                        else
                                        {
                                            var decFutan = KaikeiDetail.IchibuFutan - futan;
                                            decFutan = -(decFutan + adjustDetail.IchibuFutan);

                                            adjustDetail.AddKohiFutan(wrkKohiNo, -decFutan);
                                            adjustDetail.IchibuFutan += decFutan;
                                        }
                                    }
                                }
                                else if (KaikeiDetail.KogakuFutan > 0 && KaikeiDetail.IchibuFutan > 0)
                                {
                                    if (preFutan >= limitFutan)
                                    {
                                        //既に上限に達している場合は一部負担を発生させない
                                        var decFutan = KaikeiDetail.IchibuFutan;
                                        if (decFutan <= baselimitIchibu)
                                        {
                                            decFutan = 0;
                                        }
                                        else
                                        {
                                            decFutan -= baselimitIchibu;
                                        }

                                        if (adjustDetail.HokenPid == KaikeiDetail.HokenPid)
                                        {
                                            KaikeiDetail.AddKohiFutan(wrkKohiNo, decFutan);
                                            KaikeiDetail.IchibuFutan -= decFutan;
                                        }
                                        else
                                        {
                                            adjustDetail.AddKohiFutan(wrkKohiNo, decFutan);
                                            adjustDetail.IchibuFutan -= decFutan;
                                        }
                                    }
                                    else if (KaikeiDetail.IchibuFutan > limitFutan - preFutan)
                                    {
                                        var decFutan = KaikeiDetail.IchibuFutan - (limitFutan - preFutan);

                                        KaikeiDetail.AddKohiFutan(wrkKohiNo, decFutan);
                                        KaikeiDetail.IchibuFutan -= decFutan;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //21,000円を超えていない場合は合算対象外
                    break;
                }
                wrkIchibu += wrkFutan;

                if (adjustDetail.KogakuFutan > 0)
                {
                    //既に高額療養費で上限に達している場合でも窓口負担を発生させる
                    if (KaikeiDetail.YusenFutan > 0)
                    {
                        int adjustIchibu = KaikeiDetail.YusenFutan - KaikeiDetail.IchibuFutan;

                        adjustDetail.AddKohiFutan(KaikeiDetail.YusenFutanKohiNo,
                            adjustDetail.IchibuFutan - adjustIchibu);
                        adjustDetail.IchibuFutan = adjustIchibu;
                    }

                    //合算調整レコードの作成
                    if (wrkPos >= 0)
                    {
                        AdjustDetails[wrkPos] = adjustDetail;
                    }
                    else
                    {
                        AdjustDetails.Add(adjustDetail);
                    }
                    if (!bChoki)
                    {
                        KaikeiDetail.KogakuOverKbn =
                            Convert.ToInt32(limitFutan > 0) + Convert.ToInt32(adjustFutan > 0);
                    }
                    //マル長上限超
                    if (bChoki)
                    {
                        KaikeiDetail.IsChoki = true;
                    }
                }
            }

        }

        /// <summary>
        /// 国保減免
        /// </summary>
        private void CalculateGenmen()
        {
            if (KaikeiDetail.IchibuFutan <= 0)
            {
                return;
            }

            switch (KaikeiDetail.GenmenKbn)
            {
                //減額
                case GenmenKbn.Gengaku:
                    int wrkGenmen = PtHoken.GenmenGaku;

                    if (wrkGenmen == 0 && PtHoken.GenmenRate > 0)
                    {
                        wrkGenmen = KaikeiDetail.IchibuFutan +
                            KaikeiDetail.Kohi1Futan + KaikeiDetail.Kohi2Futan +
                            KaikeiDetail.Kohi3Futan + KaikeiDetail.Kohi4Futan;

                        wrkGenmen = wrkGenmen * PtHoken.GenmenRate / 100;
                        if (KaikeiDetail.RoundTo10en)
                        {
                            //切り捨て
                            wrkGenmen = (int)Math.Floor((double)wrkGenmen / 10) * 10;
                        }
                    }

                    if (wrkGenmen > 0)
                    {
                        if (KaikeiDetail.IchibuFutan <= wrkGenmen)
                        {
                            KaikeiDetail.GenmenGaku = KaikeiDetail.IchibuFutan;
                        }
                        else
                        {
                            KaikeiDetail.GenmenGaku = wrkGenmen;
                        }
                    }
                    break;
                //免除・支払猶予
                case GenmenKbn.Menjyo:
                case GenmenKbn.Yuyo:
                    KaikeiDetail.GenmenGaku = KaikeiDetail.IchibuFutan;
                    break;
            }
        }

        private int CalculateRosaiJibai()
        {
            switch (RaiinTensu.HokenKbn)
            {
                case 1: //労災
                case 2: //労災アフターケア
                    KaikeiDetail.RousaiIFutan = RaiinTensu.RousaiIFutan * PtHoken.EnTen;
                    KaikeiDetail.RousaiRoFutan = RaiinTensu.RousaiRoFutan;
                    return KaikeiDetail.RousaiIFutan + KaikeiDetail.RousaiRoFutan;
                case 3: //自賠
                    KaikeiDetail.JibaiNiFutan = RaiinTensu.JibaiNiFutan;
                    KaikeiDetail.JibaiHoSindan = RaiinTensu.JibaiHoSindan;
                    KaikeiDetail.JibaiHoSindanCount = RaiinTensu.JibaiHoSindanCount;
                    KaikeiDetail.JibaiHeMeisai = RaiinTensu.JibaiHeMeisai;
                    KaikeiDetail.JibaiHeMeisaiCount = RaiinTensu.JibaiHeMeisaiCount;
                    //Ｄ（ニ＋ホ＋ヘ）
                    KaikeiDetail.JibaiDFutan = KaikeiDetail.JibaiNiFutan + KaikeiDetail.JibaiHoSindan + KaikeiDetail.JibaiHeMeisai;

                    //健保準拠
                    if (_systemConfigProvider.GetJibaiJunkyo() == 0)
                    {
                        KaikeiDetail.JibaiKenpoTensu = RaiinTensu.JibaiKenpoTensu;
                        KaikeiDetail.JibaiKenpoFutan = RaiinTensu.JibaiKenpoTensu * PtHoken.EnTen;
                        return KaikeiDetail.JibaiKenpoFutan + KaikeiDetail.JibaiDFutan;
                    }
                    //労災準拠
                    else
                    {
                        KaikeiDetail.JibaiITensu = RaiinTensu.JibaiITensu;
                        KaikeiDetail.JibaiRoTensu = RaiinTensu.JibaiRoTensu;
                        KaikeiDetail.JibaiHaFutan = RaiinTensu.JibaiHaFutan;
                        //Ａ（イ×単価×加算率）
                        KaikeiDetail.JibaiAFutan = CIUtil.RoundInt(KaikeiDetail.JibaiITensu * PtHoken.EnTen * _systemConfigProvider.GetJibaiRousaiRate(), 0);
                        //Ｂ（ロ×単価）
                        KaikeiDetail.JibaiBFutan = KaikeiDetail.JibaiRoTensu * PtHoken.EnTen;
                        //Ｃ（ハ×加算率）
                        KaikeiDetail.JibaiCFutan = CIUtil.RoundInt(KaikeiDetail.JibaiHaFutan * _systemConfigProvider.GetJibaiRousaiRate(), 0);
                    }

                    return KaikeiDetail.JibaiAFutan + KaikeiDetail.JibaiBFutan + KaikeiDetail.JibaiCFutan + KaikeiDetail.JibaiDFutan;
                default:
                    return 0;
            }
        }


        /// <summary>
        /// 会計情報の設定
        /// </summary>
        private void SetKaikeiInf(bool raiinAdjust, KaikeiDetailModel kaikeiDetail)
        {
            KaikeiDetails.ForEach(k => k.RoundTo10en = false);

            KaikeiInfModel kaikeiInf = KaikeiInfs.Find(
                k => k.RaiinNo == kaikeiDetail.RaiinNo && k.HokenId == kaikeiDetail.HokenId
            ) ?? new KaikeiInfModel(new KaikeiInf());
            bool addRaiin = kaikeiInf.RaiinNo == 0;

            if (addRaiin)
            {
                kaikeiInf.HpId = kaikeiDetail.HpId;
                kaikeiInf.PtId = kaikeiDetail.PtId;
                kaikeiInf.SinDate = kaikeiDetail.SinDate;
                kaikeiInf.RaiinNo = kaikeiDetail.RaiinNo;

                kaikeiInf.HokenId = kaikeiDetail.HokenId;
                kaikeiInf.Kohi1Id = kaikeiDetail.Kohi1Id;
                kaikeiInf.Kohi2Id = kaikeiDetail.Kohi2Id;
                kaikeiInf.Kohi3Id = kaikeiDetail.Kohi3Id;
                kaikeiInf.Kohi4Id = kaikeiDetail.Kohi4Id;
                kaikeiInf.HokenKbn = kaikeiDetail.HokenKbn;
                kaikeiInf.HokenSbtCd = kaikeiDetail.HokenSbtCd;
                kaikeiInf.ReceSbt = kaikeiDetail.ReceSbt;
                kaikeiInf.Houbetu = kaikeiDetail.Houbetu;
                kaikeiInf.Kohi1Houbetu = kaikeiDetail.Kohi1Houbetu;
                kaikeiInf.Kohi2Houbetu = kaikeiDetail.Kohi2Houbetu;
                kaikeiInf.Kohi3Houbetu = kaikeiDetail.Kohi3Houbetu;
                kaikeiInf.Kohi4Houbetu = kaikeiDetail.Kohi4Houbetu;
                kaikeiInf.HonkeKbn = kaikeiDetail.HonkeKbn;
                kaikeiInf.HokenRate = kaikeiDetail.HokenRate;
                kaikeiInf.PtRate = kaikeiDetail.PtRate;
                kaikeiInf.DispRate = kaikeiDetail.PtRate;

                for (int i = 0; i < PtKohis.Count; i++)
                {
                    kaikeiInf.SetKohiPriority(i + 1, PtKohis[i].Priority);
                }
            }
            else
            {
                for (int pCnt = 0; pCnt < PtKohis.Count; pCnt++)
                {
                    if (PtKohis[pCnt].HokenId == 0) break;

                    for (int kCnt = 1; kCnt <= 4; kCnt++)
                    {
                        if (PtKohis[pCnt].HokenId == kaikeiInf.GetKohiId(kCnt))
                        {
                            break;
                        }
                        else if (PtKohis[pCnt].Priority.CompareTo(kaikeiInf.GetKohiPriority(kCnt)) == -1)
                        {
                            //公費情報を格納する
                            setKohiInf(kCnt, pCnt);
                            //左から2桁目と3桁目を+1する
                            kaikeiInf.HokenSbtCd += 11;
                            break;
                        }
                    }
                }

                //公費をスライドして格納する
                void setKohiInf(int insNo, int kohiNo)
                {
                    for (int i = 4; i >= insNo + 1; i--)
                    {
                        kaikeiInf.SetKohiId(i, kaikeiInf.GetKohiId(i - 1));
                        kaikeiInf.SetKohiHoubetu(i, kaikeiInf.GetKohiHoubetu(i - 1));
                        kaikeiInf.SetKohiPriority(i, kaikeiInf.GetKohiPriority(i - 1));
                    }

                    kaikeiInf.SetKohiId(insNo, PtKohis[kohiNo].HokenId);
                    kaikeiInf.SetKohiHoubetu(insNo, PtKohis[kohiNo].Houbetu);
                    kaikeiInf.SetKohiPriority(insNo, PtKohis[kohiNo].Priority);
                }
            }

            kaikeiInf.Tensu += kaikeiDetail.Tensu;
            kaikeiInf.TotalIryohi += kaikeiDetail.TotalIryohi;
            kaikeiInf.PtFutan += kaikeiDetail.PtFutan;
            kaikeiInf.JihiFutan += kaikeiDetail.JihiFutan;
            kaikeiInf.JihiTax += kaikeiDetail.JihiTax;
            kaikeiInf.JihiOuttax += kaikeiDetail.JihiOuttax;
            kaikeiInf.JihiFutanTaxfree += KaikeiDetail.JihiFutanTaxfree;
            kaikeiInf.JihiFutanOuttaxNr += KaikeiDetail.JihiFutanOuttaxNr;
            kaikeiInf.JihiFutanOuttaxGen += KaikeiDetail.JihiFutanOuttaxGen;
            kaikeiInf.JihiFutanTaxNr += KaikeiDetail.JihiFutanTaxNr;
            kaikeiInf.JihiFutanTaxGen += KaikeiDetail.JihiFutanTaxGen;
            kaikeiInf.JihiOuttaxNr += KaikeiDetail.JihiOuttaxNr;
            kaikeiInf.JihiOuttaxGen += KaikeiDetail.JihiOuttaxGen;
            kaikeiInf.JihiTaxNr += KaikeiDetail.JihiTaxNr;
            kaikeiInf.JihiTaxGen += KaikeiDetail.JihiTaxGen;
            kaikeiInf.HokenRate = Math.Min(kaikeiInf.HokenRate, kaikeiDetail.HokenRate);
            kaikeiInf.PtRate = Math.Min(kaikeiInf.PtRate, kaikeiDetail.PtRate);
            kaikeiInf.DispRate = Math.Min(kaikeiInf.DispRate, kaikeiDetail.PtRate);
            foreach (var ptKohi in PtKohis)
            {
                if (ptKohi == null) break;
                //負担なしの公費が全額負担する場合は、表示用負担率を０％にする
                if (ptKohi.FutanKbn == 0) kaikeiInf.DispRate = 0;
            }

            //同一来院のまるめ調整額
            raiinAdjust = raiinAdjust && KaikeiDetails.Where(k => k.OyaRaiinNo == kaikeiDetail.OyaRaiinNo && k.HokenId == kaikeiDetail.HokenId && k.RaiinNo != kaikeiDetail.RaiinNo).ToList().Count >= 1;

            if (raiinAdjust && CIUtil.RoundInt(kaikeiDetail.IchibuFutan - kaikeiDetail.GenmenGaku, 1) == kaikeiDetail.PtFutan)
            {
                //誤差がある場合は公費上限で調整が入っているので処理しない

                //int wrkIchibu = KaikeiDetails.Where(k =>
                //    k.OyaRaiinNo == kaikeiDetail.OyaRaiinNo
                //).Sum(k => k.IchibuFutan - k.GenmenGaku) + kaikeiDetail.IchibuFutan - kaikeiDetail.GenmenGaku;
                //wrkIchibu = CIUtil.RoundInt(wrkIchibu, 1);

                //Pid毎（公費併用分と保険単独分それぞれ）に四捨五入する
                var wrkDetails = KaikeiDetails.Where(k =>
                    k.OyaRaiinNo == kaikeiDetail.OyaRaiinNo &&
                    k.HokenId == kaikeiDetail.HokenId
                ).GroupBy(
                    k => k.HokenPid
                ).Select(
                    k => new
                    {
                        HokenPid = k.Key,
                        IchibuFutan = k.Sum(x => x.IchibuFutan - x.GenmenGaku)
                    }
                ).ToList();

                int wrkIchibu = 0;
                foreach (var k in wrkDetails)
                {
                    if (k.HokenPid == kaikeiDetail.HokenPid)
                    {
                        wrkIchibu += CIUtil.RoundInt(k.IchibuFutan + kaikeiDetail.IchibuFutan - kaikeiDetail.GenmenGaku, 1);
                    }
                    else
                    {
                        wrkIchibu += CIUtil.RoundInt(k.IchibuFutan, 1);
                    }
                }
                if (wrkDetails.Find(k => k.HokenPid == kaikeiDetail.HokenPid) == null)
                {
                    wrkIchibu += CIUtil.RoundInt(kaikeiDetail.IchibuFutan - kaikeiDetail.GenmenGaku, 1);
                }

                int wrkPtFutan = KaikeiDetails.Where(k =>
                    k.OyaRaiinNo == kaikeiDetail.OyaRaiinNo &&
                    k.HokenId == kaikeiDetail.HokenId
                ).Sum(k => k.PtFutan) + kaikeiDetail.PtFutan;

                //元々ある誤差を除く
                int difFutan =
                    KaikeiDetails.Where(k =>
                            k.OyaRaiinNo == kaikeiDetail.OyaRaiinNo &&
                            k.HokenId == kaikeiDetail.HokenId
                        ).Sum(k => CIUtil.RoundInt(k.IchibuFutan - k.GenmenGaku, 1)) +
                    CIUtil.RoundInt(kaikeiDetail.IchibuFutan - kaikeiDetail.GenmenGaku, 1) - wrkPtFutan;

                kaikeiInf.AdjustRound = wrkIchibu - wrkPtFutan - difFutan;
            }

            foreach (PtSanteiConfModel wrkConf in PtSanteiConfs)
            {
                int allFutan = kaikeiInf.PtFutan + kaikeiInf.JihiFutan + kaikeiInf.JihiOuttax;
                int wrkFutan = 0;
                switch (wrkConf.EdaNo)
                {
                    //すべて
                    case 0:
                        wrkFutan = allFutan;
                        break;
                    //自費除く
                    case 1:
                        wrkFutan = kaikeiInf.PtFutan;
                        break;
                    //自費のみ
                    case 2:
                        wrkFutan = kaikeiInf.JihiFutan + kaikeiInf.JihiOuttax;
                        break;
                }

                if (allFutan == wrkFutan)
                {
                    wrkFutan += kaikeiInf.AdjustRound;
                }

                int adjFutan = 0;
                switch (wrkConf.KbnNo)
                {
                    //調整額
                    case 1:
                        adjFutan = wrkConf.KbnVal;
                        kaikeiInf.AdjustFutanVal = wrkConf.KbnVal;
                        kaikeiInf.AdjustFutanRange = wrkConf.EdaNo;
                        break;
                    //調整率
                    case 2:
                        adjFutan = CIUtil.RoundInt(wrkFutan * wrkConf.KbnVal / 100, 1);
                        kaikeiInf.AdjustRateVal = wrkConf.KbnVal;
                        kaikeiInf.AdjustRateRange = wrkConf.EdaNo;
                        break;
                }

                if (wrkFutan <= adjFutan)
                {
                    kaikeiInf.AdjustFutan = -wrkFutan;
                }
                else if (kaikeiInf.AdjustFutan < adjFutan)
                {
                    kaikeiInf.AdjustFutan = -adjFutan;
                }
            }

            kaikeiInf.TotalPtFutan = kaikeiInf.PtFutan + kaikeiInf.JihiFutan + kaikeiInf.JihiOuttax +
                kaikeiInf.AdjustFutan + kaikeiInf.AdjustRound;

            if (addRaiin)
            {
                KaikeiInfs.Add(kaikeiInf);
            }
        }

        private void AddCalcLog(int logSbt, string logText)
        {
            int seqNo = CalcLogs.Count == 0 ? 0 : CalcLogs.Max(c => c.SeqNo);
            seqNo++;

            var calcLog = new CalcLogModel(
                new CalcLog()
                {
                    HpId = RaiinTensu.HpId,
                    PtId = RaiinTensu.PtId,
                    RaiinNo = RaiinTensu.RaiinNo,
                    SeqNo = seqNo,
                    SinDate = RaiinTensu.SinDate,
                    LogSbt = logSbt,
                    Text = logText
                }
            );

            CalcLogs.Add(calcLog);
        }
    }
}
