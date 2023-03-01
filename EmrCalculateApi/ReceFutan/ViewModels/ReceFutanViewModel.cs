using EmrCalculateApi.Constants;
using EmrCalculateApi.Interface;
using EmrCalculateApi.ReceFutan.DB.CommandHandler;
using EmrCalculateApi.ReceFutan.DB.Finder;
using EmrCalculateApi.ReceFutan.Models;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace EmrCalculateApi.ReceFutan.ViewModels
{
    public class ReceFutanViewModel : IReceFutanViewModel
    {
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625
#pragma warning disable CS8603
#pragma warning disable CS8618
        private readonly SaveFutancalCommandHandler _saveFutancalCommandHandler;
        private readonly ClearCommandHandler _clearCommandHandler;
        private readonly ReceFutanFinder _receFutanFinder;
        private readonly KaikeiFinder _kaikeiFinder;
        private readonly SinKouiFinder _sinKouiFinder;
        private bool kaikeiCalculate;

        public List<KaikeiDetailModel> KaikeiDetails { get; private set; } = new List<KaikeiDetailModel>();
        public List<SumKaikeiPidModel> SumKaikeiPids { get; private set; } = new List<SumKaikeiPidModel>();
        public List<ReceInfModel> ReceInfs { get; private set; } = new List<ReceInfModel>();
        public List<ReceInfEditModel> ReceInfEdits { get; private set; } = new List<ReceInfEditModel>();
        public List<ReceInfPreEditModel> ReceInfPreEdits { get; private set; } = new List<ReceInfPreEditModel>();
        public List<ReceFutanKbnModel> ReceFutanKbns { get; private set; } = new List<ReceFutanKbnModel>();
        public List<ReceInfJdModel> ReceInfJds { get; private set; } = new List<ReceInfJdModel>();
        public PtHokenInfModel PtHoken { get; private set; }
        public PtInfModel PtInf { get; private set; }
        public List<PtKohiModel> PtKohis { get; private set; }
        public int HpPrefCd;

        public struct SystemConfs
        {
            public int ChokiTokki { get; private set; }
            public int ReceKyufuKisai { get; private set; }
            public int ReceKyufuKisai2 { get; private set; }
            public double JibaiRousaiRate { get; private set; }

            public SystemConfs(int chokiTokki, int receKyufuKisai, int receKyufuKisai2, double jibaiRousaiRate)
            {
                ChokiTokki = chokiTokki;
                ReceKyufuKisai = receKyufuKisai;
                ReceKyufuKisai2 = receKyufuKisai2;
                JibaiRousaiRate = jibaiRousaiRate;
            }
        };
        public readonly SystemConfs SystemConf;

        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public ReceFutanViewModel(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _systemConfigProvider = systemConfigProvider;
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
            _emrLogger = emrLogger;

            _receFutanFinder = new ReceFutanFinder(_tenantDataContext);
            _kaikeiFinder = new KaikeiFinder(_tenantDataContext);
            _sinKouiFinder = new SinKouiFinder(_tenantDataContext);
            _saveFutancalCommandHandler = new SaveFutancalCommandHandler(_tenantDataContext, emrLogger);
            _clearCommandHandler = new ClearCommandHandler(_tenantDataContext, emrLogger);

            SystemConf = new SystemConfs(
                chokiTokki: _systemConfigProvider.GetChokiTokki(),
                receKyufuKisai: _systemConfigProvider.GetReceKyufuKisai(),
                receKyufuKisai2: _systemConfigProvider.GetReceKyufuKisai2(),
                jibaiRousaiRate: _systemConfigProvider.GetJibaiRousaiRate()
            );
        }

        /// <summary>
        /// レセプト集計処理
        /// </summary>
        /// <param name="ptIds">患者ID(null:未指定)</param>
        /// <param name="seikyuYm">請求年月</param>
        public void ReceFutanCalculateMain(
            List<long> ptIds, int seikyuYm
        )
        {
            const string conFncName = nameof(ReceFutanCalculateMain);
            try
            {
                _emrLogger.WriteLogStart(this, conFncName, $"ptIds.Count:{ptIds?.Count ?? 0} seikyuYm:{seikyuYm}");

                kaikeiCalculate = false;

                //都道府県番号
                HpPrefCd = _receFutanFinder.FindHpPrefCd(Hardcode.HospitalID, seikyuYm);

                //レセデータ初期化
                _clearCommandHandler.ClearCalculate(Hardcode.HospitalID, ptIds, seikyuYm);

                //レセ対象の会計データ取得
                KaikeiDetails = _receFutanFinder.FindKaikeiDetail(Hardcode.HospitalID, ptIds, seikyuYm);

                //レセ編集情報の取得
                ReceInfEdits = _receFutanFinder.FindReceInfEdit(Hardcode.HospitalID, ptIds, seikyuYm);

                //レセデータ集計
                ReceCalculate(seikyuYm);

                if (IsStopCalc || CancellationToken.IsCancellationRequested)
                {
                    _emrLogger.WriteLogMsg(this, conFncName, "IsCancellationRequested");
                }
                else
                {
                    //DB保存
                    _saveFutancalCommandHandler.AddReceInf(ReceInfs);
                    _saveFutancalCommandHandler.UpdReceInfEdit(ReceInfEdits, ReceInfPreEdits);
                    _saveFutancalCommandHandler.AddReceInfPreEdit(ReceInfPreEdits);
                    _saveFutancalCommandHandler.AddReceFutanKbn(ReceFutanKbns);
                    _saveFutancalCommandHandler.AddReceInfJd(ReceInfJds);

                    //DB保存確定
                    _tenantDataContext.SaveChanges();
                }

                _emrLogger.WriteLogEnd(this, conFncName, $"ptIds.Count:{ptIds?.Count ?? 0} seikyuYm:{seikyuYm}");
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
                throw;
            }
        }

        /// <summary>
        /// 会計情報の月集計
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinYm">診療年月</param>
        public List<ReceInfModel> KaikeiTotalCalculate(long ptId, int sinYm)
        {
            const string conFncName = nameof(KaikeiTotalCalculate);
            try
            {
                _emrLogger.WriteLogStart(this, conFncName, $"ptId:{ptId} sinYm:{sinYm}");

                kaikeiCalculate = true;

                //都道府県番号
                HpPrefCd = _receFutanFinder.FindHpPrefCd(Hardcode.HospitalID, sinYm);

                //レセ対象の会計データ取得
                KaikeiDetails = _kaikeiFinder.FindKaikeiDetail(Hardcode.HospitalID, ptId, sinYm);

                //レセデータ集計
                ReceCalculate(sinYm);

                _emrLogger.WriteLogEnd(this, conFncName, $"ptId:{ptId} sinYm:{sinYm}");

                return ReceInfs;
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
                return null;
            }
        }

        public int AllCalcCount { get; set; }

        private int _calculatedCount;
        public int CalculatedCount
        {
            get => _calculatedCount;
            set
            {
                _calculatedCount = value;
                AfterCalcItemEvent?.Invoke();
            }
        }

        public bool IsStopCalc { get; set; }

        public AfterCalcItem AfterCalcItemEvent { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public delegate void AfterCalcItem();

        public void ReceCalculate(int seikyuYm)
        {
            const string conFncName = nameof(ReceCalculate);

            _emrLogger.WriteLogStart(this, conFncName, string.Format("seikyuYm:{0}", seikyuYm));

            //実日数の補正（保険分や公費分のいずれかにしかフラグが立っていない場合）
            var wrkNissu0s = KaikeiDetails.Where(k => k.Jitunisu == 0 && k.Tensu > 0).ToList();
            var wrkNissu1s = KaikeiDetails.Where(k => k.Jitunisu == 1).ToList();

            var wrkNissus = (
                from wrkNissu0 in wrkNissu0s
                join wrkNissu1 in wrkNissu1s on
                    new { wrkNissu0.PtId, wrkNissu0.HokenId, wrkNissu0.SinDate } equals
                    new { wrkNissu1.PtId, wrkNissu1.HokenId, wrkNissu1.SinDate }
                select new
                {
                    wrkNissu0
                }
            ).ToList();

            wrkNissus.ForEach(k => k.wrkNissu0.Jitunisu = 1);

            //保険ID毎にまとめる
            SumSameHoken(seikyuYm);
            //負担区分の初期設定
            InitFutanKbn();
            //請求がなければ（自費だけ）請求しない
            if (!kaikeiCalculate)
            {
                var receDels = ReceInfs.Where(r => r.TotalIryohi == 0).ToList();
                foreach (var d in receDels)
                {
                    ReceFutanKbns.RemoveAll(r =>
                        r.PtId == d.PtId &&
                        r.SinYm == d.SinYm &&
                        r.HokenId == d.HokenId
                    );
                }
                ReceInfs.RemoveAll(r => r.TotalIryohi == 0);
            }

            AllCalcCount = ReceInfs.Count;
            CalculatedCount = 0;

            for (int rCnt = ReceInfs.Count - 1; rCnt >= 0 && !IsStopCalc; rCnt--)
            {
                if (CancellationToken.IsCancellationRequested) return;
                ReceInfModel receInf = ReceInfs[rCnt];

                _emrLogger.WriteLogMsg(
                    this, conFncName,
                    string.Format("ptId:{0} sinYm{1} hokenId:{2}", receInf.PtId, receInf.SinYm, receInf.HokenId)
                );

                try
                {
                    //患者情報の取得
                    if (PtInf?.PtId != receInf.PtId)
                    {
                        PtInf = _receFutanFinder.FindPtInf(receInf.HpId, receInf.PtId);
                    }
                    receInf.IsTester = PtInf.IsTester;
                    //保険情報の取得
                    if (PtHoken == null)
                    {
                        PtHoken = _receFutanFinder.FindHokenInf(receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm);
                    }
                    if (PtHoken != null)
                    {
                        if (PtHoken.ReceKisai == ReceKisai.None && !kaikeiCalculate)
                        {
                            //レセプト未記載
                            ReceFutanKbns.RemoveAll(r =>
                                r.PtId == receInf.PtId &&
                                r.SinYm == receInf.SinYm &&
                                r.HokenId == receInf.HokenId
                            );
                            ReceInfs.RemoveAt(rCnt);
                            continue;
                        }
                        receInf.HokensyaNo = PtHoken.HokensyaNo;
                        receInf.Kigo = PtHoken.Kigo;
                        receInf.Bango = PtHoken.Bango;
                        receInf.EdaNo = PtHoken.EdaNo;
                        receInf.Sinkei = PtHoken.Sinkei;
                        receInf.Tenki = PtHoken.Tenki;
                    }

                    //公費情報の取得
                    if (receInf.Kohi1Id == 0 && receInf.Kohi2Id == 0 && receInf.Kohi3Id == 0 && receInf.Kohi4Id == 0)
                    {
                        PtKohis = new List<PtKohiModel>();
                    }
                    else if (PtKohis == null)
                    {
                        PtKohis = _receFutanFinder.FindKohiInf(receInf.HpId, receInf.PtId,
                            receInf.Kohi0Id, receInf.Kohi1Id, receInf.Kohi2Id, receInf.Kohi3Id, receInf.Kohi4Id,
                            receInf.SinYm, seikyuYm, receInf.HokensyaNo, receInf.HokenKbn);
                    }
                    //各公費の保険種別区分を設定
                    for (int i = 1; i <= 4; i++)
                    {
                        int kohiId = receInf.GetKohiId(i);

                        if (kohiId == 0) break;

                        receInf.SetKohiHokenSbtKbn(i, PtKohis.Find(p => p.HokenId == kohiId)?.HokenSbtKbn ?? 0);
                        receInf.SetKohiFutansyaNo(i, PtKohis.Find(p => p.HokenId == kohiId)?.FutansyaNo);
                        receInf.SetKohiJyukyusyaNo(i, PtKohis.Find(p => p.HokenId == kohiId)?.JyukyusyaNo);
                        receInf.SetKohiTokusyuNo(i, PtKohis.Find(p => p.HokenId == kohiId)?.TokusyuNo);
                        receInf.SetKohiNameCd(i, PtKohis.Find(p => p.HokenId == kohiId)?.HokenNameCd);
                    }

                    //実日数集計
                    SetJitunissu(receInf);

                    //点数
                    SetTensu(receInf);

                    //高額療養費の一部負担金額（仮設定）
                    SetKogakuIchibuFutan(receInf);

                    //一部負担金額
                    SetIchibuFutan(receInf);

                    //特記事項
                    SetTokki(receInf);

                    //単独併用判断
                    SetKohiKisai(receInf);

                    //高額療養費の発生有無により一部負担金の記載を調整
                    SetKogakuOverIchibuFutan(receInf);

                    //特殊処理
                    SetKisaiSp(receInf);

                    //患者の状態
                    receInf.PtStatus = receInf.IsNinpu == 0 ? string.Empty : String.Format("{0:D3}", receInf.IsNinpu);

                    //自賠負担金額
                    SetJibaiFutan(receInf);

                    //レセ編集情報の設定
                    SetReceInfEdit(receInf);

                    //受診日等レコード
                    SetReceInfJd(receInf);

                    CalculatedCount++;
                }
                catch (Exception E)
                {
                    _emrLogger.WriteLogError(this, conFncName, E);
                    throw;
                }
                finally
                {
                    PtHoken = null;
                    PtKohis = null;
                }
            }

            //負担区分の設定
            SetFutanKbn();

            _emrLogger.WriteLogEnd(this, conFncName, string.Format("seikyuYm:{0}", seikyuYm));
        }


        /// <summary>
        /// 保険ID毎にまとめる
        /// </summary>
        /// <param name="seikyuYm"></param>
        private void SumSameHoken(int seikyuYm)
        {
            //保険PID毎にまとめる
            SumKaikeiPids = SumKaikeiPid();

            //初期化
            ReceInfs.Clear();
            ReceInfPreEdits.Clear();
            ReceFutanKbns.Clear();
            ReceInfJds.Clear();

            //保険ID毎にまとめる
            foreach (var k in SumKaikeiPids)
            {
                ReceInfModel receInf = ReceInfs.Find(
                    r => r.PtId == k.PtId && r.SinYm == k.SinYm && r.HokenId == k.HokenId
                );  // ?? new ReceInfModel(new ReceInf());

                if (receInf == null)
                {
                    //社保生保+生保単独等の異なる保険IDのレセを1つにまとめる
                    if (k.SeihoId > 0)
                    {
                        receInf = ReceInfs.Find(
                            r =>
                                r.PtId == k.PtId && r.SinYm == k.SinYm &&
                                (r.Kohi1Id == k.SeihoId || r.Kohi2Id == k.SeihoId || r.Kohi3Id == k.SeihoId || r.Kohi4Id == k.SeihoId) &&
                                r.HokenId2 == 0
                        );
                    }
                    if (receInf == null)
                    {
                        receInf = new ReceInfModel(new ReceInf());
                    }
                }

                bool addRece = receInf.HokenId == 0;

                if (addRece)
                {
                    receInf.HpId = k.HpId;
                    receInf.SeikyuYm = seikyuYm;
                    receInf.PtId = k.PtId;
                    receInf.SinYm = k.SinYm;
                    receInf.HokenId = k.HokenId;
                    receInf.Kohi1Id = k.Kohi1Id;
                    receInf.Kohi2Id = k.Kohi2Id;
                    receInf.Kohi3Id = k.Kohi3Id;
                    receInf.Kohi4Id = k.Kohi4Id;
                    receInf.HokenKbn = k.HokenKbn;
                    receInf.HokenSbtCd = k.HokenSbtCd;
                    receInf.ReceSbt = k.ReceSbt;
                    receInf.Houbetu = k.Houbetu;
                    receInf.Kohi1Houbetu = k.Kohi1Houbetu;
                    receInf.Kohi2Houbetu = k.Kohi2Houbetu;
                    receInf.Kohi3Houbetu = k.Kohi3Houbetu;
                    receInf.Kohi4Houbetu = k.Kohi4Houbetu;
                    receInf.Kohi1Priority = k.Kohi1Priority;
                    receInf.Kohi2Priority = k.Kohi2Priority;
                    receInf.Kohi3Priority = k.Kohi3Priority;
                    receInf.Kohi4Priority = k.Kohi4Priority;
                    receInf.HonkeKbn = k.HonkeKbn;
                    receInf.KogakuKbn = k.KogakuKbn;
                    receInf.KogakuTekiyoKbn = k.KogakuTekiyoKbn;
                    receInf.IsTokurei = k.IsTokurei;
                    receInf.IsTasukai = k.IsTasukai;
                    //receInf.IsNotKogakuTotal = k.IsNotKogakuTotal;
                    receInf.IsChoki = k.IsChoki;
                    receInf.KogakuKohi1Limit = k.Kohi1Id == 0 ? 0 : k.KogakuLimit;
                    receInf.KogakuKohi2Limit = k.Kohi2Id == 0 ? 0 : k.KogakuLimit;
                    receInf.KogakuKohi3Limit = k.Kohi3Id == 0 ? 0 : k.KogakuLimit;
                    receInf.KogakuKohi4Limit = k.Kohi4Id == 0 ? 0 : k.KogakuLimit;
                    receInf.TotalKogakuLimit = k.TotalKogakuLimit;
                    if (receInf.TotalKogakuLimit == 0)
                    {
                        receInf.TotalKogakuLimit = k.KogakuLimit;
                    }
                    receInf.GenmenKbn = k.GenmenKbn;
                    receInf.HokenRate = k.HokenRate;
                    receInf.PtRate = k.PtRate;
                    receInf.EnTen = k.EnTen;
                    receInf.Kohi1Limit = k.Kohi1Limit;
                    receInf.Kohi1OtherFutan = k.Kohi1OtherFutan;
                    receInf.Kohi2Limit = k.Kohi2Limit;
                    receInf.Kohi2OtherFutan = k.Kohi2OtherFutan;
                    receInf.Kohi3Limit = k.Kohi3Limit;
                    receInf.Kohi3OtherFutan = k.Kohi3OtherFutan;
                    receInf.Kohi4Limit = k.Kohi4Limit;
                    receInf.Kohi4OtherFutan = k.Kohi4OtherFutan;
                    receInf.IsNinpu = k.IsNinpu;
                    receInf.IsZaiiso = k.IsZaiiso;
                    receInf.SeikyuKbn = k.SeikyuKbn;

                    for (int i = 1; i <= 4; i++)
                    {
                        receInf.SetKohiFutan(i, k.GetKohiFutan(i));
                        receInf.SetKohiFutan10en(i, k.GetKohiFutan10en(i));
                    }
                    receInf.ReceSbt = k.ReceSbt;
                }
                else
                {
                    if (receInf.HokenId != k.HokenId) receInf.HokenId2 = k.HokenId;

                    for (int kCnt = 1; kCnt <= 4; kCnt++)
                    {
                        if (k.GetKohiId(kCnt) == 0) break;

                        bool isSetKohi = false;

                        for (int rCnt = 1; rCnt <= 4; rCnt++)
                        {
                            if (k.GetKohiId(kCnt) == receInf.GetKohiId(rCnt))
                            {
                                receInf.SetKohiFutan(rCnt, receInf.GetKohiFutan(rCnt) + k.GetKohiFutan(kCnt));
                                receInf.SetKohiFutan10en(rCnt, receInf.GetKohiFutan10en(rCnt) + k.GetKohiFutan10en(kCnt));
                                //公１の限度額を登録する
                                if (kCnt == 1 && k.KogakuLimit > 0)
                                {
                                    receInf.SetKogakuLimit(rCnt, k.KogakuLimit);
                                }
                                isSetKohi = true;
                                break;
                            }
                        }

                        if (!isSetKohi)
                        {
                            if (receInf.Kohi4Id != 0 &&
                                receInf.Kohi1Houbetu == KohiHoubetu.Choki)
                            {
                                receInf.Kohi0Id = receInf.Kohi1Id;
                                receInf.Kohi0Limit = receInf.Kohi1Limit;
                                receInf.KogakuFutan += receInf.Kohi1Futan;
                                receInf.KogakuFutan10en += receInf.Kohi1Futan10en;
                                //既に４種で公１がマル長の場合は、マル長を削除してスライド
                                deleteKohi1();
                            }

                            for (int rCnt = 1; rCnt <= 4; rCnt++)
                            {
                                if (k.GetKohiPriority(kCnt).CompareTo(receInf.GetKohiPriority(rCnt)) == -1)
                                {
                                    //公費情報を格納する
                                    setKohiInf(rCnt, kCnt);
                                    //左から2桁目と3桁目を+1する
                                    receInf.HokenSbtCd += 11;
                                    break;
                                }
                            }
                        }
                    }

                    //公１を削除してスライド
                    void deleteKohi1()
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            receInf.SetKohiId(i, receInf.GetKohiId(i + 1));
                            receInf.SetKohiHoubetu(i, receInf.GetKohiHoubetu(i + 1));
                            receInf.SetKohiPriority(i, receInf.GetKohiPriority(i + 1));
                            receInf.SetKohiLimit(i, receInf.GetKohiLimit(i + 1));
                            receInf.SetOtherFutan(i, receInf.GetOtherFutan(i + 1));
                            receInf.SetKohiFutan(i, receInf.GetKohiFutan(i + 1));
                            receInf.SetKohiKyufu(i, receInf.GetKohiKyufu(i + 1));
                            receInf.SetKohiFutan10en(i, receInf.GetKohiFutan10en(i + 1));
                            receInf.SetKogakuLimit(i, receInf.GetKogakuLimit(i + 1));
                            receInf.SetKohiTensu(i, receInf.GetKohiTensu(i + 1));
                            receInf.SetKohiIchibuSotogaku(i, receInf.GetKohiIchibuSotogaku(i + 1));
                            receInf.SetKohiIchibuSotogaku10en(i, receInf.GetKohiIchibuSotogaku10en(i + 1));
                            receInf.SetKohiIchibuFutan(i, receInf.GetKohiIchibuFutan(i + 1));
                        }
                    }

                    //公費をスライドして格納する
                    void setKohiInf(int insNo, int kohiNo)
                    {
                        for (int i = 4; i >= insNo + 1; i--)
                        {
                            receInf.SetKohiId(i, receInf.GetKohiId(i - 1));
                            receInf.SetKohiHoubetu(i, receInf.GetKohiHoubetu(i - 1));
                            receInf.SetKohiPriority(i, receInf.GetKohiPriority(i - 1));
                            receInf.SetKohiLimit(i, receInf.GetKohiLimit(i - 1));
                            receInf.SetOtherFutan(i, receInf.GetOtherFutan(i - 1));
                            receInf.SetKohiFutan(i, receInf.GetKohiFutan(i - 1));
                            receInf.SetKohiKyufu(i, receInf.GetKohiKyufu(i - 1));
                            receInf.SetKohiFutan10en(i, receInf.GetKohiFutan10en(i - 1));
                            receInf.SetKogakuLimit(i, receInf.GetKogakuLimit(i - 1));
                            receInf.SetKohiTensu(i, receInf.GetKohiTensu(i - 1));
                            receInf.SetKohiIchibuSotogaku(i, receInf.GetKohiIchibuSotogaku(i - 1));
                            receInf.SetKohiIchibuSotogaku10en(i, receInf.GetKohiIchibuSotogaku10en(i - 1));
                            receInf.SetKohiIchibuFutan(i, receInf.GetKohiIchibuFutan(i - 1));
                        }

                        receInf.SetKohiId(insNo, k.GetKohiId(kohiNo));
                        receInf.SetKohiHoubetu(insNo, k.GetHoubetu(kohiNo));
                        receInf.SetKohiPriority(insNo, k.GetKohiPriority(kohiNo));
                        receInf.SetKohiLimit(insNo, k.GetKohiLimit(kohiNo));
                        receInf.SetOtherFutan(insNo, k.GetOtherFutan(kohiNo));
                        receInf.SetKohiFutan(insNo, k.GetKohiFutan(kohiNo));
                        receInf.SetKohiFutan10en(insNo, k.GetKohiFutan10en(kohiNo));
                        receInf.SetKogakuLimit(insNo, k.KogakuLimit);
                        receInf.SetKohiTensu(insNo, 0);
                        receInf.SetKohiIchibuSotogaku(insNo, 0);
                        receInf.SetKohiIchibuSotogaku10en(insNo, 0);
                        receInf.SetKohiIchibuFutan(insNo, 0);
                    }
                }

                receInf.PtRate = Math.Min(receInf.PtRate, k.PtRate);
                receInf.Tensu += k.Tensu;
                receInf.TotalIryohi += k.TotalIryohi;
                receInf.HokenFutan += k.HokenFutan;
                receInf.KogakuFutan += k.KogakuFutan;
                receInf.IchibuFutan += k.IchibuFutan;
                receInf.GenmenGaku += k.GenmenGaku;
                receInf.HokenFutan10en += k.HokenFutan10en;
                receInf.KogakuFutan10en += k.KogakuFutan10en;
                receInf.IchibuFutan10en += k.IchibuFutan10en;
                receInf.GenmenGaku10en += k.GenmenGaku10en;
                receInf.PtFutan += k.PtFutan;
                receInf.KogakuOverKbn = Math.Max(receInf.KogakuOverKbn, k.KogakuOverKbn);
                receInf.KogakuIchibuFutan += k.KogakuIchibuFutan;

                const int maxVal = 999999999;
                receInf.TotalKogakuLimit =
                    Math.Min(
                        receInf.TotalKogakuLimit == 0 ? maxVal : receInf.TotalKogakuLimit,
                        k.TotalKogakuLimit == 0 ? maxVal : k.TotalKogakuLimit
                    );
                receInf.TotalKogakuLimit = receInf.TotalKogakuLimit == maxVal ? 0 : receInf.TotalKogakuLimit;

                receInf.RousaiIFutan += k.RousaiIFutan;
                receInf.RousaiRoFutan += k.RousaiRoFutan;
                receInf.JibaiITensu += k.JibaiITensu;
                receInf.JibaiRoTensu += k.JibaiRoTensu;
                receInf.JibaiHaFutan += k.JibaiHaFutan;
                receInf.JibaiNiFutan += k.JibaiNiFutan;
                receInf.JibaiHoSindan += k.JibaiHoSindan;
                receInf.JibaiHoSindanCount += k.JibaiHoSindanCount;
                receInf.JibaiHeMeisai += k.JibaiHeMeisai;
                receInf.JibaiHeMeisaiCount += k.JibaiHeMeisaiCount;
                receInf.JibaiAFutan += k.JibaiAFutan;
                receInf.JibaiBFutan += k.JibaiBFutan;
                receInf.JibaiCFutan += k.JibaiCFutan;
                receInf.JibaiDFutan += k.JibaiDFutan;
                receInf.JibaiKenpoTensu += k.JibaiKenpoTensu;
                receInf.JibaiKenpoFutan += k.JibaiKenpoFutan;
                receInf.IsNinpu = Math.Max(receInf.IsNinpu, k.IsNinpu);
                receInf.IsZaiiso = Math.Max(receInf.IsZaiiso, k.IsZaiiso);
                //receInf.isNotKogakuTotal = Math.Max(receInf.IsNotKogakuTotal, k.IsNotKogakuTotal);
                receInf.IsChoki = Math.Max(receInf.IsChoki, k.IsChoki);

                if (!k.IsNoHoken)
                {
                    receInf.HokenTensu += k.Tensu;
                    receInf.HokenIchibuFutan += k.IchibuFutan + k.Kohi1Futan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan;
                    receInf.HokenIchibuFutan10en += k.IchibuFutan10en + k.Kohi1Futan10en + k.Kohi2Futan10en + k.Kohi3Futan10en + k.Kohi4Futan10en;
                }

                receInf.AddKohiTensu(k.Kohi1Id, k.Tensu);
                receInf.AddKohiIchibuSotogaku(k.Kohi1Id, k.IchibuFutan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan);
                receInf.AddKohiIchibuSotogaku10en(k.Kohi1Id, k.IchibuFutan10en + k.Kohi2Futan10en + k.Kohi3Futan10en + k.Kohi4Futan10en);
                receInf.AddKohiIchibuFutan(k.Kohi1Id, k.IchibuFutan);

                receInf.AddKohiTensu(k.Kohi2Id, k.Tensu);
                receInf.AddKohiIchibuSotogaku(
                    k.Kohi2Futan == 0 && k.GenmenGaku > 0 && k.IchibuFutan - k.GenmenGaku == 0 ? 0 :
                        k.Kohi2Id, k.IchibuFutan + k.Kohi3Futan + k.Kohi4Futan
                );
                receInf.AddKohiIchibuSotogaku10en(
                    k.Kohi2Futan10en == 0 && k.GenmenGaku10en > 0 && k.IchibuFutan10en - k.GenmenGaku10en == 0 ? 0 :
                        k.Kohi2Id, k.IchibuFutan10en + k.Kohi3Futan10en + k.Kohi4Futan10en
                );
                receInf.AddKohiIchibuFutan(
                    k.Kohi2Futan == 0 && k.GenmenGaku > 0 && k.IchibuFutan - k.GenmenGaku == 0 ? 0 : k.Kohi2Id, k.IchibuFutan
                );

                receInf.AddKohiTensu(k.Kohi3Id, k.Tensu);
                receInf.AddKohiIchibuSotogaku(
                    k.Kohi3Futan == 0 && k.GenmenGaku > 0 && k.IchibuFutan - k.GenmenGaku == 0 ? 0 :
                        k.Kohi3Id, k.IchibuFutan + k.Kohi4Futan
                );
                receInf.AddKohiIchibuSotogaku10en(
                    k.Kohi3Futan10en == 0 && k.GenmenGaku10en > 0 && k.IchibuFutan10en - k.GenmenGaku10en == 0 ? 0 :
                        k.Kohi3Id, k.IchibuFutan10en + k.Kohi4Futan10en
                );
                receInf.AddKohiIchibuFutan(
                    k.Kohi3Futan == 0 && k.GenmenGaku > 0 && k.IchibuFutan - k.GenmenGaku == 0 ? 0 : k.Kohi3Id, k.IchibuFutan
                );

                receInf.AddKohiTensu(k.Kohi4Id, k.Tensu);
                receInf.AddKohiIchibuSotogaku(
                    k.Kohi4Futan == 0 && k.GenmenGaku > 0 && k.IchibuFutan - k.GenmenGaku == 0 ? 0 :
                        k.Kohi4Id, k.IchibuFutan
                );
                receInf.AddKohiIchibuSotogaku10en(
                    k.Kohi4Futan10en == 0 && k.GenmenGaku10en > 0 && k.IchibuFutan10en - k.GenmenGaku10en == 0 ? 0 :
                        k.Kohi4Id, k.IchibuFutan10en
                );
                receInf.AddKohiIchibuFutan(
                    k.Kohi4Futan == 0 && k.GenmenGaku > 0 && k.IchibuFutan - k.GenmenGaku == 0 ? 0 : k.Kohi4Id, k.IchibuFutan
                );

                //都道府県別
                switch (HpPrefCd)
                {
                    //長崎県
                    case PrefCode.Nagasaki:
                        //86被爆+12生保の場合、86被爆に12生保は掛からないようにする
                        if (k.GetKohiNo("86") >= 1)
                        {
                            int seihoNo = k.GetKohiNo("12");
                            if (seihoNo >= 1 && k.GetKohiFutan(seihoNo) == 0)
                            {
                                receInf.AddKohiTensu(k.GetKohiId(seihoNo), -k.Tensu);
                            }
                        }
                        break;
                }

                receInf.TotalIchibuFutan +=
                    k.AdjustKid == 0 ? k.IchibuFutan + k.Kohi1Futan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan :
                    k.Kohi1Id == k.AdjustKid ? k.IchibuFutan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan :
                    k.Kohi2Id == k.AdjustKid ? k.IchibuFutan + k.Kohi3Futan + k.Kohi4Futan :
                    k.Kohi3Id == k.AdjustKid ? k.IchibuFutan + k.Kohi4Futan :
                    k.IchibuFutan;
                receInf.TotalIchibuFutan10en +=
                    k.AdjustKid == 0 ? k.IchibuFutan10en + k.Kohi1Futan10en + k.Kohi2Futan10en + k.Kohi3Futan10en + k.Kohi4Futan10en :
                    k.Kohi1Id == k.AdjustKid ? k.IchibuFutan10en + k.Kohi2Futan10en + k.Kohi3Futan10en + k.Kohi4Futan10en :
                    k.Kohi2Id == k.AdjustKid ? k.IchibuFutan10en + k.Kohi3Futan10en + k.Kohi4Futan10en :
                    k.Kohi3Id == k.AdjustKid ? k.IchibuFutan10en + k.Kohi4Futan10en :
                    k.IchibuFutan;

                if (addRece)
                {
                    ReceInfs.Add(receInf);
                }
            }

            foreach (var receInf in ReceInfs)
            {
                //診療科IDの設定
                receInf.KaId = KaikeiDetails.Where(k =>
                    k.HpId == receInf.HpId &&
                    k.PtId == receInf.PtId &&
                    (k.HokenId == receInf.HokenId || k.HokenId == receInf.HokenId2)
                ).GroupBy(
                    k => k.KaId
                ).Select(
                    k => new { KaId = k.Key, Tensu = k.Sum(x => x.Tensu) }
                ).OrderByDescending(
                    k => k.Tensu
                ).FirstOrDefault().KaId;

                //担当医IDの設定
                receInf.TantoId = KaikeiDetails.Where(k =>
                    k.HpId == receInf.HpId &&
                    k.PtId == receInf.PtId &&
                    (k.HokenId == receInf.HokenId || k.HokenId == receInf.HokenId2)
                ).GroupBy(
                    k => k.TantoId
                ).Select(
                    k => new { TantoId = k.Key, Tensu = k.Sum(x => x.Tensu) }
                ).OrderByDescending(
                    k => k.Tensu
                ).FirstOrDefault().TantoId;

                if (receInf.KogakuOverKbn != KogakuOverStatus.None &&
                    receInf.KogakuFutan == 0 &&
                    receInf.KogakuFutan10en >= 0)
                {
                    //1円単位で上限に達していない場合は超えていない扱い
                    receInf.KogakuOverKbn = KogakuOverStatus.None;
                }
            }
        }

        /// <summary>
        /// 負担区分の初期設定
        /// </summary>
        private void InitFutanKbn()
        {
            foreach (var k in SumKaikeiPids)
            {
                ReceInfModel receInf = ReceInfs.Find(
                    r => r.PtId == k.PtId &&
                    r.SinYm == k.SinYm &&
                    (r.HokenId == k.HokenId || r.HokenId2 == k.HokenId)
                );

                if (receInf == null) continue;

                ReceFutanKbnModel receFutan = new ReceFutanKbnModel(
                    new ReceFutanKbn()
                    {
                        HpId = receInf.HpId,
                        SeikyuYm = receInf.SeikyuYm,
                        PtId = receInf.PtId,
                        SinYm = receInf.SinYm,
                        HokenId = receInf.HokenId,
                        HokenPid = k.HokenPid

                    }
                );

                receFutan.IsHoken = !k.IsNoHoken;

                if (k.Kohi1Id > 0)
                {
                    if (k.Kohi1Id == receInf.Kohi1Id) receFutan.IsKohi1 = true;
                    if (k.Kohi1Id == receInf.Kohi2Id) receFutan.IsKohi2 = true;
                    if (k.Kohi1Id == receInf.Kohi3Id) receFutan.IsKohi3 = true;
                    if (k.Kohi1Id == receInf.Kohi4Id) receFutan.IsKohi4 = true;
                }
                if (k.Kohi2Id > 0)
                {
                    if (k.Kohi2Id == receInf.Kohi1Id) receFutan.IsKohi1 = true;
                    if (k.Kohi2Id == receInf.Kohi2Id) receFutan.IsKohi2 = true;
                    if (k.Kohi2Id == receInf.Kohi3Id) receFutan.IsKohi3 = true;
                    if (k.Kohi2Id == receInf.Kohi4Id) receFutan.IsKohi4 = true;
                }
                if (k.Kohi3Id > 0)
                {
                    if (k.Kohi3Id == receInf.Kohi1Id) receFutan.IsKohi1 = true;
                    if (k.Kohi3Id == receInf.Kohi2Id) receFutan.IsKohi2 = true;
                    if (k.Kohi3Id == receInf.Kohi3Id) receFutan.IsKohi3 = true;
                    if (k.Kohi3Id == receInf.Kohi4Id) receFutan.IsKohi4 = true;
                }
                if (k.Kohi4Id > 0)
                {
                    if (k.Kohi4Id == receInf.Kohi1Id) receFutan.IsKohi1 = true;
                    if (k.Kohi4Id == receInf.Kohi2Id) receFutan.IsKohi2 = true;
                    if (k.Kohi4Id == receInf.Kohi3Id) receFutan.IsKohi3 = true;
                    if (k.Kohi4Id == receInf.Kohi4Id) receFutan.IsKohi4 = true;
                }

                //都道府県別
                switch (HpPrefCd)
                {
                    //長崎県
                    case PrefCode.Nagasaki:
                        //86被爆+12生保の場合、86被爆に12生保は掛からないようにする
                        if (k.GetKohiNo("86") >= 1)
                        {
                            int seihoNo = k.GetKohiNo("12");
                            if (seihoNo >= 1 && k.GetKohiFutan(seihoNo) == 0)
                            {
                                int seihoId = k.GetKohiId(seihoNo);

                                if (seihoId == receInf.Kohi1Id) receFutan.IsKohi1 = false;
                                if (seihoId == receInf.Kohi2Id) receFutan.IsKohi2 = false;
                                if (seihoId == receInf.Kohi3Id) receFutan.IsKohi3 = false;
                                if (seihoId == receInf.Kohi4Id) receFutan.IsKohi4 = false;
                            }
                        }
                        break;
                }

                ReceFutanKbns.Add(receFutan);
            }
        }

        /// <summary>
        /// 保険PID毎にまとめる
        /// </summary>
        /// <returns></returns>
        private List<SumKaikeiPidModel> SumKaikeiPid()
        {
            return KaikeiDetails.GroupBy(
                k => new { k.HpId, k.PtId, SinYm = k.SinDate / 100, k.HokenPid }
            ).Select(
                k => new SumKaikeiPidModel(
                    hpId: k.Key.HpId,
                    ptId: k.Key.PtId,
                    sinYm: k.Key.SinYm,
                    hokenPid: k.Key.HokenPid,
                    adjustKid: k.Max(x => x.AdjustKid),
                    hokenSbtCd: k.Max(x => x.HokenSbtCd),
                    hokenKbn: k.Max(x => x.HokenKbn),
                    hokenId: k.Max(x => x.HokenId),
                    kohi1Id: k.Max(x => x.Kohi1Id),
                    kohi2Id: k.Max(x => x.Kohi2Id),
                    kohi3Id: k.Max(x => x.Kohi3Id),
                    kohi4Id: k.Max(x => x.Kohi4Id),
                    houbetu: k.Max(x => x.Houbetu),
                    kohi1Houbetu: k.Max(x => x.Kohi1Houbetu),
                    kohi2Houbetu: k.Max(x => x.Kohi2Houbetu),
                    kohi3Houbetu: k.Max(x => x.Kohi3Houbetu),
                    kohi4Houbetu: k.Max(x => x.Kohi4Houbetu),
                    kohi1Priority: k.Max(x => x.Kohi1Priority),
                    kohi2Priority: k.Max(x => x.Kohi2Priority),
                    kohi3Priority: k.Max(x => x.Kohi3Priority),
                    kohi4Priority: k.Max(x => x.Kohi4Priority),
                    honkeKbn: k.Max(x => x.HonkeKbn),
                    kogakuKbn: k.Max(x => x.KogakuKbn),
                    kogakuTekiyoKbn: k.Max(x => x.KogakuTekiyoKbn),
                    isTokurei: k.Max(x => x.IsTokurei),
                    isTasukai: k.Max(x => x.IsTasukai),
                    //isNotKogakuTotal: k.Max(x => x.IsNotKogakuTotal),
                    isChoki: k.Max(x => x.IsChoki),
                    kogakuLimit: k.Max(x => x.KogakuLimit),
                    totalKogakuLimit: k.Max(x => x.TotalKogakuLimit),
                    genmenKbn: k.Max(x => x.GenmenKbn),
                    hokenRate: k.Max(x => x.HokenRate),
                    ptRate: k.Max(x => x.PtRate),
                    enTen: k.Max(x => x.EnTen),
                    kohi1Limit: k.Max(x => x.Kohi1Limit),
                    kohi1OtherFutan: k.Max(x => x.Kohi1OtherFutan),
                    kohi2Limit: k.Max(x => x.Kohi2Limit),
                    kohi2OtherFutan: k.Max(x => x.Kohi2OtherFutan),
                    kohi3Limit: k.Max(x => x.Kohi3Limit),
                    kohi3OtherFutan: k.Max(x => x.Kohi3OtherFutan),
                    kohi4Limit: k.Max(x => x.Kohi4Limit),
                    kohi4OtherFutan: k.Max(x => x.Kohi4OtherFutan),
                    tensu: k.Sum(x => x.Tensu),
                    totalIryohi: k.Sum(x => x.TotalIryohi),
                    hokenFutan: k.Sum(x => x.HokenFutan),
                    kogakuFutan: k.Sum(x => x.KogakuFutan),
                    kohi1Futan: k.Sum(x => x.Kohi1Futan),
                    kohi2Futan: k.Sum(x => x.Kohi2Futan),
                    kohi3Futan: k.Sum(x => x.Kohi3Futan),
                    kohi4Futan: k.Sum(x => x.Kohi4Futan),
                    ichibuFutan: k.Sum(x => x.IchibuFutan),
                    genmenGaku: k.Sum(x => x.GenmenGaku),
                    hokenFutan10en: k.Sum(x => x.HokenFutan10en),
                    kogakuFutan10en: k.Sum(x => x.KogakuFutan10en),
                    kohi1Futan10en: k.Sum(x => x.Kohi1Futan10en),
                    kohi2Futan10en: k.Sum(x => x.Kohi2Futan10en),
                    kohi3Futan10en: k.Sum(x => x.Kohi3Futan10en),
                    kohi4Futan10en: k.Sum(x => x.Kohi4Futan10en),
                    ichibuFutan10en: k.Sum(x => x.IchibuFutan10en),
                    genmenGaku10en: k.Sum(x => x.GenmenGaku10en),
                    ptFutan: k.Sum(x => x.PtFutan),
                    kogakuOverKbn: k.Max(x => x.KogakuOverKbn),
                    receSbt: k.Max(x => x.ReceSbt),
                    rousaiIFutan: k.Sum(x => x.RousaiIFutan),
                    rousaiRoFutan: k.Sum(x => x.RousaiRoFutan),
                    jibaiITensu: k.Sum(x => x.JibaiITensu),
                    jibaiRoTensu: k.Sum(x => x.JibaiRoTensu),
                    jibaiHaFutan: k.Sum(x => x.JibaiHaFutan),
                    jibaiNiFutan: k.Sum(x => x.JibaiNiFutan),
                    jibaiHoSindan: k.Sum(x => x.JibaiHoSindan),
                    jibaiHoSindanCount: k.Sum(x => x.JibaiHoSindanCount),
                    jibaiHeMeisai: k.Sum(x => x.JibaiHeMeisai),
                    jibaiHeMeisaiCount: k.Sum(x => x.JibaiHeMeisaiCount),
                    jibaiAFutan: k.Sum(x => x.JibaiAFutan),
                    jibaiBFutan: k.Sum(x => x.JibaiBFutan),
                    jibaiCFutan: k.Sum(x => x.JibaiCFutan),
                    jibaiDFutan: k.Sum(x => x.JibaiDFutan),
                    jibaiKenpoTensu: k.Sum(x => x.JibaiKenpoTensu),
                    jibaiKenpoFutan: k.Sum(x => x.JibaiKenpoFutan),
                    isNinpu: k.Max(x => x.IsNinpu),
                    isZaiiso: k.Max(x => x.IsZaiiso),
                    seikyuKbn: k.Max(x => x.SeikyuKbn)
                )
            ).OrderBy(x => x.HpId)
            .ThenBy(x => x.PtId)
            .ThenBy(x => x.SinYm)
            .ThenBy(x => x.IsNoHoken)               //社保生保に生保単独を結合するために公費単独は後ろに回す
            .ThenByDescending(x => x.HokenSbtCd)
            .ThenBy(x => x.HokenPid)
            .ToList();
        }


        /// <summary>
        /// 実日数設定
        /// </summary>
        private void SetJitunissu(ReceInfModel receInf)
        {
            //総実日数
            int wrkNissu = KaikeiDetails.Where(k =>
                k.PtId == receInf.PtId &&
                k.SinDate / 100 == receInf.SinYm &&
                k.HokenId == receInf.HokenId
            ).GroupBy(
                k => k.SinDate
            ).Select(
                k => new { Jitunissu = k.Max(x => x.Jitunisu) }
            ).Sum(
                k => k.Jitunissu
            );
            receInf.Nissu = wrkNissu;

            //保険分実日数
            wrkNissu = KaikeiDetails.Where(k =>
                k.PtId == receInf.PtId &&
                k.SinDate / 100 == receInf.SinYm &&
                k.HokenId == receInf.HokenId &&
                k.IsNoHoken == false
            ).GroupBy(
                k => k.SinDate
            ).Select(
                k => new { Jitunissu = k.Max(x => x.Jitunisu) }
            ).Sum(
                k => k.Jitunissu
            );

            if (receInf.IsNoHoken == false)
            {
                receInf.HokenNissu = wrkNissu;
            }

            //公１分実日数
            if (receInf.Kohi1Id == 0) return;
            wrkNissu = KaikeiDetails.Where(k =>
                k.PtId == receInf.PtId &&
                k.SinDate / 100 == receInf.SinYm &&
                (k.HokenId == receInf.HokenId || k.HokenId == receInf.HokenId2) &&
                (k.Kohi1Id == receInf.Kohi1Id || k.Kohi2Id == receInf.Kohi1Id ||
                 k.Kohi3Id == receInf.Kohi1Id || k.Kohi4Id == receInf.Kohi1Id)
            ).GroupBy(
                k => k.SinDate
            ).Select(
                k => new { Jitunissu = k.Max(x => x.Jitunisu) }
            ).Sum(
                k => k.Jitunissu
            );
            receInf.Kohi1Nissu = wrkNissu;

            //公２分実日数
            if (receInf.Kohi2Id == 0) return;
            wrkNissu = KaikeiDetails.Where(k =>
                k.PtId == receInf.PtId &&
                k.SinDate / 100 == receInf.SinYm &&
                (k.HokenId == receInf.HokenId || k.HokenId == receInf.HokenId2) &&
                (k.Kohi1Id == receInf.Kohi2Id || k.Kohi2Id == receInf.Kohi2Id ||
                 k.Kohi3Id == receInf.Kohi2Id || k.Kohi4Id == receInf.Kohi2Id)
            ).GroupBy(
                k => k.SinDate
            ).Select(
                k => new { Jitunissu = k.Max(x => x.Jitunisu) }
            ).Sum(
                k => k.Jitunissu
            );
            receInf.Kohi2Nissu = wrkNissu;

            //公３分実日数
            if (receInf.Kohi3Id == 0) return;
            wrkNissu = KaikeiDetails.Where(k =>
                k.PtId == receInf.PtId &&
                k.SinDate / 100 == receInf.SinYm &&
                (k.HokenId == receInf.HokenId || k.HokenId == receInf.HokenId2) &&
                (k.Kohi1Id == receInf.Kohi3Id || k.Kohi2Id == receInf.Kohi3Id ||
                 k.Kohi3Id == receInf.Kohi3Id || k.Kohi4Id == receInf.Kohi3Id)
            ).GroupBy(
                k => k.SinDate
            ).Select(
                k => new { Jitunissu = k.Max(x => x.Jitunisu) }
            ).Sum(
                k => k.Jitunissu
            );
            receInf.Kohi3Nissu = wrkNissu;

            //公４分実日数
            if (receInf.Kohi4Id == 0) return;
            wrkNissu = KaikeiDetails.Where(k =>
                k.PtId == receInf.PtId &&
                k.SinDate / 100 == receInf.SinYm &&
                (k.HokenId == receInf.HokenId || k.HokenId == receInf.HokenId2) &&
                (k.Kohi1Id == receInf.Kohi4Id || k.Kohi2Id == receInf.Kohi4Id ||
                 k.Kohi3Id == receInf.Kohi4Id || k.Kohi4Id == receInf.Kohi4Id)
            ).GroupBy(
                k => k.SinDate
            ).Select(
                k => new { Jitunissu = k.Max(x => x.Jitunisu) }
            ).Sum(
                k => k.Jitunissu
            );
            receInf.Kohi4Nissu = wrkNissu;
        }

        /// <summary>
        /// 特記事項
        /// </summary>
        /// <param name="receInf"></param>
        private void SetTokki(ReceInfModel receInf)
        {
            string[] tokkiCds = new string[0];

            PtKohiModel ptChoki = PtKohis.Find(p => p.HokenSbtKbn == HokenSbtKbn.Choki);

            if (ptChoki != null)
            {
                //int chokiFutan =
                //    receInf.Kohi1Id == ptChoki.HokenId ? receInf.Kohi1Futan :
                //    receInf.Kohi2Id == ptChoki.HokenId ? receInf.Kohi2Futan :
                //    receInf.Kohi3Id == ptChoki.HokenId ? receInf.Kohi3Futan :
                //    receInf.Kohi4Futan;

                int chokiLimit =
                    receInf.Kohi0Id == ptChoki.HokenId ? receInf.Kohi0Limit :
                    receInf.Kohi1Id == ptChoki.HokenId ? receInf.Kohi1Limit :
                    receInf.Kohi2Id == ptChoki.HokenId ? receInf.Kohi2Limit :
                    receInf.Kohi3Id == ptChoki.HokenId ? receInf.Kohi3Limit :
                    receInf.Kohi4Limit;

                //int chokiIchibu =
                //    receInf.Kohi1Id == ptChoki.HokenId ? receInf.Kohi1IchibuFutan :
                //    receInf.Kohi2Id == ptChoki.HokenId ? receInf.Kohi2IchibuFutan :
                //    receInf.Kohi3Id == ptChoki.HokenId ? receInf.Kohi3IchibuFutan :
                //    receInf.Kohi4IchibuFutan;

                bool isTokki = receInf.IsChoki == 1;
                //指定公費の負担額を加算して限度額を超えているかチェックする
                if (!isTokki && receInf.IsSiteiKohi)
                {
                    //公費に係る高額療養費上限と公費上限の低い方
                    //int kogakuKohiLimit =
                    //    receInf.Kohi1Id == ptChoki.HokenId ? receInf.KogakuKohi1Limit :
                    //    receInf.Kohi2Id == ptChoki.HokenId ? receInf.KogakuKohi2Limit :
                    //    receInf.Kohi3Id == ptChoki.HokenId ? receInf.KogakuKohi3Limit :
                    //    receInf.KogakuKohi4Limit;

                    //if (chokiLimit <= kogakuKohiLimit || chokiLimit <= receInf.TotalKogakuLimit)
                    //{
                    //    isTokki = chokiLimit < chokiIchibu * 2;
                    //}


                    isTokki = chokiLimit <= receInf.TotalKogakuLimit ? receInf.SiteiKohiIchibuFutan > chokiLimit : false;
                    if (!isTokki && receInf.Kohi1Id != ptChoki.HokenId)
                    {
                        isTokki = chokiLimit <= receInf.KogakuKohi1Limit ? receInf.Kohi1ReceKyufu * 2 > chokiLimit : false;
                    }
                    if (!isTokki && receInf.Kohi2Id != ptChoki.HokenId)
                    {
                        isTokki = chokiLimit <= receInf.KogakuKohi2Limit ? receInf.Kohi2ReceKyufu * 2 > chokiLimit : false;
                    }
                    if (!isTokki && receInf.Kohi3Id != ptChoki.HokenId)
                    {
                        isTokki = chokiLimit <= receInf.KogakuKohi3Limit ? receInf.Kohi3ReceKyufu * 2 > chokiLimit : false;
                    }
                    if (!isTokki && receInf.Kohi4Id != ptChoki.HokenId)
                    {
                        isTokki = chokiLimit <= receInf.KogakuKohi4Limit ? receInf.Kohi4ReceKyufu * 2 > chokiLimit : false;
                    }

                    if (isTokki)
                    {
                        receInf.IsChoki = 1;
                    }
                }

                if (!isTokki && receInf.KogakuKbn == 41)
                {
                    //後期２割（上限未満でも配慮措置適用外であることを明示するため印字する）
                    isTokki = true;
                }

                if (isTokki || SystemConf.ChokiTokki == 1)
                {
                    switch (ptChoki.HokenEdaNo)
                    {
                        case 0:
                            addCode("02長");
                            break;
                        case 1:
                            addCode("16長２");
                            break;
                    }
                }

                receInf.ChokiKbn = (receInf.IsChoki == 1 ? 2 : 1);
            }

            switch (receInf.KogakuKbn)
            {
                case 7: addCode("17上位"); break;
                case 8: addCode("18一般"); break;
                case 9: addCode("19低所"); break;
            }

            if (receInf.IsElder)
            {
                //70歳以上高齢者
                if (receInf.SinYm >= KaiseiDate.m202210 &&
                    receInf.IsKouki &&
                    new int[] { 0, 41 }.Contains(receInf.KogakuKbn))
                {
                    switch (receInf.KogakuKbn)
                    {
                        case 0: addCode("42区キ"); break;
                        case 41: addCode("41区カ"); break;
                    }
                }
                else if (receInf.SinYm >= KaiseiDate.m201808)
                {
                    switch (receInf.KogakuKbn)
                    {
                        case 0: addCode("29区エ"); break;
                        case 4:
                        case 5: addCode("30区オ"); break;
                        case 26: addCode("26区ア"); break;
                        case 27: addCode("27区イ"); break;
                        case 28: addCode("28区ウ"); break;
                    }
                }
                else if (receInf.SinYm >= KaiseiDate.m201804)
                {
                    if (PtHoken?.KogakuType != KogakuType.TekiyoIppan)
                    {
                        PtKohiModel ptKohi = PtKohis.Find(p =>
                            CIUtil.Copy(p.FutansyaNo, 1, 2) == "51" &&
                            (CIUtil.Copy(p.FutansyaNo, 5, 3) == "601" || CIUtil.Copy(p.FutansyaNo, 5, 3) == "602")
                        );
                        int kohiNo = receInf.GetKohiNo(ptKohi?.HokenId ?? 0);

                        bool isTokki = receInf.GetKohiTensu(kohiNo) > 0;

                        if (!isTokki)
                        {
                            ptKohi = PtKohis.Find(p =>
                                CIUtil.Copy(p.FutansyaNo, 1, 2) == "52" &&
                                (CIUtil.Copy(p.FutansyaNo, 5, 1) == "7" || CIUtil.Copy(p.FutansyaNo, 5, 1) == "8")
                            );
                            kohiNo = receInf.GetKohiNo(ptKohi?.HokenId ?? 0);

                            isTokki = receInf.GetKohiTensu(kohiNo) > 0;
                        }

                        if (!isTokki)
                        {
                            ptKohi = PtKohis.Find(p =>
                                CIUtil.Copy(p.FutansyaNo, 1, 2) == "54" &&
                                (CIUtil.Copy(p.FutansyaNo, 5, 3) == "501" ||
                                 CIUtil.Copy(p.FutansyaNo, 5, 3) == "601" || CIUtil.Copy(p.FutansyaNo, 5, 3) == "602" ||
                                 CIUtil.Copy(p.FutansyaNo, 5, 1) == "7" || CIUtil.Copy(p.FutansyaNo, 5, 1) == "8")
                            );
                            kohiNo = receInf.GetKohiNo(ptKohi?.HokenId ?? 0);

                            isTokki = receInf.GetKohiTensu(kohiNo) > 0;
                        }

                        if (isTokki)
                        {
                            switch (receInf.KogakuKbn)
                            {
                                case 0: addCode("18一般"); break;
                                case 3: addCode("17上位"); break;
                                case 4:
                                case 5: addCode("19低所"); break;
                            }
                        }
                    }
                }
            }
            else
            {
                //70歳未満
                switch (receInf.KogakuKbn)
                {
                    case 26: addCode("26区ア"); break;
                    case 27: addCode("27区イ"); break;
                    case 28: addCode("28区ウ"); break;
                    case 29: addCode("29区エ"); break;
                    case 30: addCode("30区オ"); break;
                }
            }

            //診療年月の月末日
            int sinDate = CIUtil.GetLastDateOfMonth(receInf.SinYm * 100 + 1);
            //診療年月の月末日時点の年齢
            int ptAge = CIUtil.SDateToAge(PtInf.Birthday, sinDate);

            if (receInf.IsTokurei == 1 && receInf.HonkeKbn == HonkeKbn.Family && ptAge < 75)
            {
                if (!(HpPrefCd == PrefCode.Hiroshima &&
                        receInf.HokenKbn == HokenKbn.Syaho &&
                        PtKohis.Find(p => new string[] { "90", "91", "92" }.Contains(p.Houbetu)) != null))
                {
                    addCode("21高半");
                }
            }

            //後期高齢者医療の対象者において、公費負担医療のみの場合
            if (receInf.IsNoHoken &&
                //PtKohis.Find(p => new string[] { "12", "25" }.Contains(p.Houbetu)) != null &&
                ptAge >= 75)
            {
                addCode("04後保");
            }

            //都道府県別
            switch (HpPrefCd)
            {
                case PrefCode.Kanagawa:
                    if (receInf.HokenKbn == HokenKbn.Kokho && !receInf.IsKouki &&
                        PtKohis.Find(p => p.FutansyaNo == "80000000") != null)
                    {
                        addCode("80障");
                    }
                    break;
                case PrefCode.Kyoto:
                    if (receInf.HokenKbn == HokenKbn.Syaho &&
                        receInf.SinYm >= 201808 &&
                        PtKohis.Any(p => new string[] { "43", "44", "45" }.Contains(p.Houbetu)))
                    {
                        int totalTensu = receInf.Tensu;
                        for (int kohiNo = 1; kohiNo <= 4; kohiNo++)
                        {
                            if (receInf.GetKohiHokenSbtKbn(kohiNo) == HokenSbtKbn.Bunten &&
                                receInf.GetKohiFutan(kohiNo) + receInf.GetKohiIchibuSotogaku(kohiNo) < KogakuIchibu.BorderVal)
                            {
                                totalTensu -= receInf.GetKohiTensu(kohiNo);
                            }
                        }
                        var kogakuLimit = _receFutanFinder.FindKyotoKogakuLimit(receInf.HpId, receInf.SinYm, receInf.AgeKbn, receInf.KogakuKbn, receInf.IsTasukai);
                        int limitFutan = kogakuLimit.Limit;
                        int adjustFutan = kogakuLimit.Adjust;

                        if (limitFutan == 0) break;

                        if (adjustFutan > 0)
                        {
                            //総医療費合計
                            int totalIryohi = totalTensu * PtHoken.EnTen;

                            if (receInf.IsTokurei == 1)
                            {
                                //限度額特例
                                if (totalIryohi < adjustFutan / 2) break;

                                limitFutan =
                                    CIUtil.RoundInt(
                                        (limitFutan / 2) + (totalIryohi - (adjustFutan / 2)) * 0.01, 0
                                    );
                            }
                            else
                            {
                                if (totalIryohi < adjustFutan) break;

                                limitFutan =
                                    CIUtil.RoundInt(
                                        limitFutan + (totalIryohi - adjustFutan) * 0.01, 0
                                    );
                            }
                        }
                        else
                        {
                            if (receInf.IsTokurei == 1)
                            {
                                //限度額特例
                                limitFutan = limitFutan / 2;
                            }
                        }

                        for (int kohiNo = 1; kohiNo <= 4; kohiNo++)
                        {
                            if (new string[] { "43", "44", "45" }.Contains(receInf.GetKohiHoubetu(kohiNo)))
                            {
                                if (receInf.GetKohiFutan(kohiNo) + receInf.GetKohiIchibuSotogaku(kohiNo) >= limitFutan)
                                {
                                    addCode("01公");
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }

            //手入力
            for (int i = 1; i <= 5; i++)
            {
                string wrkCd = PtHoken?.GetTokki(i);
                if (wrkCd != null)
                {
                    string wrkName = _receFutanFinder.FindTokkiName(receInf.HpId, receInf.SinYm, wrkCd);
                    if (wrkName != null) addCode(wrkCd + wrkName);
                }
            }

            //各カラムへ格納
            for (int i = 0; i < tokkiCds.Length; i++)
            {
                receInf.SetTokki(i + 1, tokkiCds[i]);
            }


            void addCode(string value)
            {
                if (tokkiCds.Length >= 5) return;

                if (!tokkiCds.Contains(value))
                {
                    Array.Resize(ref tokkiCds, tokkiCds.Length + 1);
                    tokkiCds[tokkiCds.Length - 1] = value;
                }
            }
        }


        /// <summary>
        /// 点数の設定
        /// </summary>
        /// <param name="receInf"></param>
        private void SetTensu(ReceInfModel receInf)
        {
            receInf.HokenReceTensu = receInf.IsNoHoken ? null : (int?)receInf.HokenTensu;

            PtKohiModel ptKohi1 = PtKohis.Find(p => p.HokenId == receInf.Kohi1Id);

            receInf.Kohi1ReceTensu = getTensu(1, receInf.Kohi1Id);
            receInf.Kohi2ReceTensu = getTensu(2, receInf.Kohi2Id);
            receInf.Kohi3ReceTensu = getTensu(3, receInf.Kohi3Id);
            receInf.Kohi4ReceTensu = getTensu(4, receInf.Kohi4Id);

            int? getTensu(int kohiNo, int kohiId)
            {
                if (kohiId == 0) return null;

                int retTensu = receInf.GetKohiTensu(kohiNo);

                PtKohiModel ptKohi = PtKohis.Find(p => p.HokenId == kohiId);

                if (kohiNo == 1 || (kohiNo == 2 && ptKohi1.HokenSbtKbn == HokenSbtKbn.Choki)) return retTensu;
                //マル長が登録されている場合は公２が公１になる
                int kohi1Tensu = (ptKohi1.HokenSbtKbn == HokenSbtKbn.Choki ? receInf.Kohi2ReceTensu : receInf.Kohi1ReceTensu) ?? 0;
                var kohi1Houbetu = ptKohi1.HokenSbtKbn == HokenSbtKbn.Choki ? receInf.Kohi2Houbetu : receInf.Kohi1Houbetu;
                //16育成+12生保の場合、16育成分を除く
                if (kohi1Houbetu == "16" && ptKohi.HokenSbtKbn == HokenSbtKbn.Seiho &&
                    retTensu >= kohi1Tensu)
                {
                    retTensu -= kohi1Tensu;
                }

                //公２以降は異点数の場合に公１分を除くオプション設定
                int difTensu = (receInf.HokenReceTensu ?? 0) - kohi1Tensu;
                if (receInf.HokenReceTensu == retTensu && difTensu > 0)
                {
                    if ((ptKohi.ReceTenKisai == 1 && receInf.HokenKbn == HokenKbn.Syaho) ||
                        (ptKohi.ReceTenKisai == 2 && receInf.HokenKbn == HokenKbn.Kokho) ||
                        (ptKohi.ReceTenKisai == 3))
                    {
                        retTensu = difTensu;

                        //負担区分に反映
                        List<ReceFutanKbnModel> receFutanKbns = ReceFutanKbns.Where(
                            r => r.PtId == receInf.PtId &&
                            r.SinYm == receInf.SinYm &&
                            r.HokenId == receInf.HokenId &&
                            (r.IsKohi1 || (ptKohi1.HokenSbtKbn == HokenSbtKbn.Choki && r.IsKohi2))
                        ).ToList();

                        foreach (var r in receFutanKbns)
                        {
                            if (kohiNo == 2) r.IsKohi2 = false;
                            if (kohiNo == 3) r.IsKohi3 = false;
                            if (kohiNo == 4) r.IsKohi4 = false;
                        }
                    }

                }

                return retTensu;
            }
        }

        /// <summary>
        /// 一部負担金額の設定
        /// </summary>
        /// <param name="receInf"></param>
        private void SetIchibuFutan(ReceInfModel receInf)
        {
            receInf.Kohi1ReceFutan = getReceFutan(1, receInf.Kohi1Id);
            receInf.Kohi2ReceFutan = getReceFutan(2, receInf.Kohi2Id);
            receInf.Kohi3ReceFutan = getReceFutan(3, receInf.Kohi3Id);
            receInf.Kohi4ReceFutan = getReceFutan(4, receInf.Kohi4Id);

            int? getReceFutan(int kohiNo, int kohiId)
            {
                if (kohiId == 0) return null;

                int retFutan;

                PtKohiModel ptKohi = PtKohis.Find(p => p.HokenId == kohiId);

                //未記載
                if (ptKohi.ReceFutanHide) return null;

                //まるめ設定
                if ((ptKohi.ReceFutanRound == 1) ||
                    (new int[] { 5, 6 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Syaho) ||
                    (new int[] { 3, 8 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Kokho))
                {
                    retFutan =
                        kohiNo == 1 ? receInf.Kohi1IchibuSotogaku10en :
                        kohiNo == 2 ? receInf.Kohi2IchibuSotogaku10en :
                        kohiNo == 3 ? receInf.Kohi3IchibuSotogaku10en :
                        receInf.Kohi4IchibuSotogaku10en;
                }
                else if ((ptKohi.ReceFutanRound == 2) ||
                    (new int[] { 7, 8 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Syaho) ||
                    (new int[] { 4, 6 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Kokho))
                {
                    retFutan =
                        kohiNo == 1 ? CIUtil.RoundInt(receInf.Kohi1IchibuSotogaku, 1) :
                        kohiNo == 2 ? CIUtil.RoundInt(receInf.Kohi2IchibuSotogaku, 1) :
                        kohiNo == 3 ? CIUtil.RoundInt(receInf.Kohi3IchibuSotogaku, 1) :
                        CIUtil.RoundInt(receInf.Kohi4IchibuSotogaku, 1);
                }
                else
                {
                    retFutan =
                        kohiNo == 1 ? receInf.Kohi1IchibuSotogaku :
                        kohiNo == 2 ? receInf.Kohi2IchibuSotogaku :
                        kohiNo == 3 ? receInf.Kohi3IchibuSotogaku :
                        receInf.Kohi4IchibuSotogaku;
                }

                //指定公費の負担額を加算
                if (receInf.IsSiteiKohi && retFutan > 0)
                {
                    string kohiHoubetu =
                        kohiNo == 1 ? receInf.Kohi1Houbetu :
                        kohiNo == 2 ? receInf.Kohi2Houbetu :
                        kohiNo == 3 ? receInf.Kohi3Houbetu :
                        receInf.Kohi4Houbetu;

                    if (new string[] { "38", "51", "54" }.Contains(kohiHoubetu))
                    {
                        //公費上限
                        int kohiLimit =
                            kohiNo == 1 ? receInf.Kohi1Limit :
                            kohiNo == 2 ? receInf.Kohi2Limit :
                            kohiNo == 3 ? receInf.Kohi3Limit :
                            receInf.Kohi4Limit;
                        //公費上限から他院負担分を引く
                        kohiLimit -=
                            kohiNo == 1 ? receInf.Kohi1OtherFutan :
                            kohiNo == 2 ? receInf.Kohi2OtherFutan :
                            kohiNo == 3 ? receInf.Kohi3OtherFutan :
                            receInf.Kohi4OtherFutan;
                        //公費に係る高額療養費上限と公費上限の低い方
                        int kogakuKohiLimit =
                            kohiNo == 1 ? receInf.KogakuKohi1Limit :
                            kohiNo == 2 ? receInf.KogakuKohi2Limit :
                            kohiNo == 3 ? receInf.KogakuKohi3Limit :
                            receInf.KogakuKohi4Limit;

                        kohiLimit = Math.Min(kohiLimit, kogakuKohiLimit == 0 ? 999999999 : kogakuKohiLimit);
                        //総医療費に係る高額療養費上限と公費上限の低い方
                        kohiLimit = Math.Min(kohiLimit, receInf.TotalKogakuLimit == 0 ? 999999999 : receInf.TotalKogakuLimit);

                        //2割換算の負担額と上限の低い方を記載する
                        retFutan = Math.Min(retFutan * 2, kohiLimit);
                    }
                }

                if (ptKohi.ReceZeroKisai == 0 && retFutan == 0)
                {
                    //0円記載なし
                    return null;
                }
                return retFutan;
            }
        }

        /// <summary>
        /// 高額療養費の一部負担記載（レセ給付対象額）
        /// </summary>
        /// <param name="receInf"></param>
        private void SetKogakuIchibuFutan(ReceInfModel receInf)
        {
            PtKohiModel ptChoki = PtKohis.Find(p => p.HokenSbtKbn == HokenSbtKbn.Choki);

            receInf.HokenReceFutan = receInf.HokenIchibuFutan;

            if (receInf.HokenIchibuFutan > 0)
            {
                if (ptChoki != null)
                {
                    receInf.HokenReceFutan -=
                        receInf.Kohi1Id == ptChoki.HokenId ? receInf.Kohi1Futan :
                        receInf.Kohi2Id == ptChoki.HokenId ? receInf.Kohi2Futan :
                        receInf.Kohi3Id == ptChoki.HokenId ? receInf.Kohi3Futan :
                        receInf.Kohi4Futan;
                }
            }

            receInf.Kohi1ReceKyufu = getReceKyufu(1, receInf.Kohi1Id);
            receInf.Kohi2ReceKyufu = getReceKyufu(2, receInf.Kohi2Id);
            receInf.Kohi3ReceKyufu = getReceKyufu(3, receInf.Kohi3Id);
            receInf.Kohi4ReceKyufu = getReceKyufu(4, receInf.Kohi4Id);

            int? getReceKyufu(int kohiNo, int kohiId)
            {
                if (kohiId == 0) return null;
                if (kohiId == (ptChoki?.HokenId ?? 0)) return null;
                //if (receInf.GetKohiHokenSbtKbn(kohiNo) == HokenSbtKbn.Choki) return null;

                int? retFutan =
                    kohiNo == 1 ? receInf.GetKohiIchibuSotogaku(1) + receInf.GetKohiFutan(1) :
                    kohiNo == 2 ? receInf.GetKohiIchibuSotogaku(2) + receInf.GetKohiFutan(2) :
                    kohiNo == 3 ? receInf.GetKohiIchibuSotogaku(3) + receInf.GetKohiFutan(3) :
                        receInf.GetKohiIchibuSotogaku(4) + receInf.GetKohiFutan(4);

                //合算対象外の公費を所持する場合は分点分を含めない
                if (retFutan != null)
                {
                    int kogakuTotalKbn = receInf.IsElder ? 0 : (PtKohis.Find(p => p.HokenId == kohiId)?.KogakuTotalKbn ?? 0);
                    if ((kogakuTotalKbn == 1 && receInf.HokenKbn == HokenKbn.Kokho) ||
                        (kogakuTotalKbn == 2 && receInf.HokenKbn == HokenKbn.Syaho) ||
                        (kogakuTotalKbn == 3))
                    {
                        for (int i = 1; i < kohiNo; i++)
                        {
                            int wrkKohiId = receInf.GetKohiId(i);
                            int wrkHokenSbtKbn = PtKohis.Find(p => p.HokenId == wrkKohiId)?.HokenSbtKbn ?? 0;

                            if (wrkHokenSbtKbn == HokenSbtKbn.Bunten)
                            {
                                retFutan = retFutan - receInf.GetKohiIchibuSotogaku(i);
                            }
                        }

                        if (retFutan == 0) retFutan = null;
                    }
                }

                //if (receInf.KogakuOverKbn == KogakuOverStatus.Over)
                //{
                //    retFutan = CIUtil.RoundInt(retFutan, 1);
                //}

                return retFutan;
            }
        }

        //高額療養費の発生有無により一部負担金の記載を調整
        private void SetKogakuOverIchibuFutan(ReceInfModel receInf)
        {
            //全額減免の時は高額療養費が発生しないため一部負担金の記載を省略する
            if ((receInf.HokenReceFutan ?? 0) - receInf.GenmenGaku == 0)
            {
                receInf.HokenReceFutan = null;
                receInf.Kohi1ReceKyufu = null;
                receInf.Kohi2ReceKyufu = null;
                receInf.Kohi3ReceKyufu = null;
                receInf.Kohi4ReceKyufu = null;
                return;
            }

            //高額療養費の限度額を超えているか
            bool isKogaku = receInf.KogakuOverKbn != KogakuOverStatus.None;

            if (!isKogaku)
            {
                //指定公費の場合は2割換算で上限を超えるかチェックする
                if (receInf.IsSiteiKohi)
                {
                    isKogaku = (receInf.TotalKogakuLimit > 0 && receInf.SiteiKohiIchibuFutan > receInf.TotalKogakuLimit) ||
                        (receInf.KogakuKohi1Limit > 0 && receInf.Kohi1ReceKyufu * 2 > receInf.KogakuKohi1Limit) ||
                        (receInf.KogakuKohi2Limit > 0 && receInf.Kohi2ReceKyufu * 2 > receInf.KogakuKohi2Limit) ||
                        (receInf.KogakuKohi3Limit > 0 && receInf.Kohi3ReceKyufu * 2 > receInf.KogakuKohi3Limit) ||
                        (receInf.KogakuKohi4Limit > 0 && receInf.Kohi4ReceKyufu * 2 > receInf.KogakuKohi4Limit);

                    if (isKogaku)
                    {
                        receInf.KogakuOverKbn = KogakuOverStatus.Over;
                        if (receInf.TotalKogakuLimit != CIUtil.RoundInt(receInf.TotalKogakuLimit, 1))
                        {
                            receInf.KogakuOverKbn = KogakuOverStatus.OverOneYen;
                        }
                    }
                }
            }

            //マル長の限度額を超えているか
            if (!isKogaku)
            {
                //70歳未満マル長で限度額認定証の提示がない場合は記載しない
                if (receInf.IsElder ||
                    receInf.TokkiContains("17") || receInf.TokkiContains("18") || receInf.TokkiContains("19") ||
                    receInf.TokkiContains("26") || receInf.TokkiContains("27") || receInf.TokkiContains("28") ||
                    receInf.TokkiContains("29") || receInf.TokkiContains("30"))
                {
                    isKogaku = receInf.IsChoki == 1;  //receInf.TokkiContains("02") || receInf.TokkiContains("16");
                    if (isKogaku)
                    {
                        receInf.KogakuOverKbn = KogakuOverStatus.Over;
                    }
                }
            }

            //結核特殊処理（患者負担の総額を記載）
            if (isKogaku)
            {
                if (receInf.Kohi1Houbetu == "10")
                {
                    receInf.Kohi1ReceFutan = receInf.IchibuFutan + receInf.Kohi2Futan + receInf.Kohi3Futan + receInf.Kohi4Futan;
                }
                if (receInf.Kohi2Houbetu == "10")
                {
                    receInf.Kohi2ReceFutan = receInf.IchibuFutan + receInf.Kohi3Futan + receInf.Kohi4Futan;
                }
                if (receInf.Kohi3Houbetu == "10")
                {
                    receInf.Kohi3ReceFutan = receInf.IchibuFutan + receInf.Kohi4Futan;
                }
                if (receInf.Kohi4Houbetu == "10")
                {
                    receInf.Kohi4ReceFutan = receInf.IchibuFutan;
                }
            }

            //限度額を超過していない場合は一部負担金を記載しない            
            if (!isKogaku)
            {
                receInf.HokenReceFutan = null;
                receInf.Kohi1ReceKyufu = null;
                receInf.Kohi2ReceKyufu = null;
                receInf.Kohi3ReceKyufu = null;
                receInf.Kohi4ReceKyufu = null;
                return;
            }

            //公費レセ給付対象額の記載オプション
            int? kohiTensu =
                receInf.Kohi1ReceKisai ? receInf.Kohi1ReceTensu :
                receInf.Kohi2ReceKisai ? receInf.Kohi2ReceTensu :
                receInf.Kohi3ReceKisai ? receInf.Kohi3ReceTensu :
                receInf.Kohi4ReceKisai ? receInf.Kohi4ReceTensu :
                0;

            bool buntenKohi =
                receInf.Kohi1ReceKisai ? receInf.Kohi1HokenSbtKbn == HokenSbtKbn.Bunten :
                receInf.Kohi2ReceKisai ? receInf.Kohi2HokenSbtKbn == HokenSbtKbn.Bunten :
                receInf.Kohi3ReceKisai ? receInf.Kohi3HokenSbtKbn == HokenSbtKbn.Bunten :
                receInf.Kohi4ReceKisai ? receInf.Kohi4HokenSbtKbn == HokenSbtKbn.Bunten :
                false;

            bool kohiKyufu = false;

            //記載オプション
            switch (SystemConf.ReceKyufuKisai)
            {
                case 1:
                    kohiKyufu = (receInf.HokenReceTensu != kohiTensu && receInf.HokenKbn == HokenKbn.Syaho) ||
                        (buntenKohi && receInf.HokenKbn == HokenKbn.Kokho);
                    break;
                case 2:
                    kohiKyufu = (receInf.HokenReceTensu != kohiTensu && receInf.HokenKbn == HokenKbn.Kokho) ||
                        (buntenKohi && receInf.HokenKbn == HokenKbn.Syaho);
                    break;
                case 3:
                    kohiKyufu = buntenKohi;
                    break;
                default:
                    kohiKyufu = receInf.HokenReceTensu != kohiTensu;
                    break;
            }

            if (!kohiKyufu)
            {
                receInf.Kohi1ReceKyufu = null;
                receInf.Kohi2ReceKyufu = null;
                receInf.Kohi3ReceKyufu = null;
                receInf.Kohi4ReceKyufu = null;
            }

            //公費給付対象額（公２以降）マル長の場合に記載しないオプション
            if (receInf.IsChoki == 1 && kohiKyufu)
            {
                bool notKyufu = false;

                switch (SystemConf.ReceKyufuKisai2)
                {
                    case 1:
                        if (receInf.HokenKbn == 1) notKyufu = true;
                        break;
                    case 2:
                        if (receInf.HokenKbn == 2) notKyufu = true;
                        break;
                    case 3:
                        notKyufu = true;
                        break;
                }

                int kohiNo = 0;
                for (int i = 1; i <= 4; i++)
                {
                    if (receInf.GetKohiReceKisai(i))
                    {
                        if (receInf.GetKohiHokenSbtKbn(i) == HokenSbtKbn.Bunten)
                        {
                            kohiNo = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (notKyufu && kohiNo >= 1)
                {
                    for (int i = kohiNo + 1; i <= 4; i++)
                    {
                        switch (i)
                        {
                            case 2:
                                receInf.Kohi2ReceKyufu = null;
                                break;
                            case 3:
                                receInf.Kohi3ReceKyufu = null;
                                break;
                            case 4:
                                receInf.Kohi4ReceKyufu = null;
                                break;
                        }
                    }
                }
            }

            if (receInf.KogakuOverKbn == KogakuOverStatus.Over)
            {
                //int adjustFutan(int kohiNo)
                //{
                //    if (receInf.GenmenGaku > 0) return 0;

                //    int hokenSbtKbn = PtKohis.Find(p => p.HokenId == receInf.GetKohiId(kohiNo))?.HokenSbtKbn ?? 0;
                //    int kogakuTotalKbn = PtKohis.Find(p => p.HokenId == receInf.GetKohiId(kohiNo))?.KogakuTotalKbn ?? 0;

                //    int retFutan = 0;

                //    if ((kogakuTotalKbn == 0) ||
                //        (kogakuTotalKbn == 1 && receInf.HokenKbn == HokenKbn.Syaho) ||
                //        (kogakuTotalKbn == 2 && receInf.HokenKbn == HokenKbn.Kokho))
                //    {
                //        for (int i = 1; i < kohiNo; i++)
                //        {
                //            int wrkKohiId = receInf.GetKohiId(i);
                //            int wrkHokenSbtKbn = PtKohis.Find(p => p.HokenId == wrkKohiId)?.HokenSbtKbn ?? 0;

                //            if (wrkHokenSbtKbn == HokenSbtKbn.Bunten)
                //            {
                //                retFutan += CIUtil.RoundInt((int)receInf.GetKohiReceFutan(i), 1) - receInf.GetKohiReceFutan(i);
                //            }
                //        }
                //        return retFutan;
                //    }
                //    return 0;
                //}

                //まるめ誤差
                int adjustHokenReceFutan = 0;
                if (ReceFutanKbns.Any(
                    r => r.PtId == receInf.PtId &&
                    r.SinYm == receInf.SinYm &&
                    r.HokenId == receInf.HokenId &&
                    r.IsKohi1 &&
                    r.IsKohi2))
                {
                    if (receInf.Kohi1HokenSbtKbn == HokenSbtKbn.Choki)
                    {
                        int kyufu1 = receInf.GetKohiKyufu(2) - receInf.GetKohiReceFutan(2) + receInf.GetKohiKyufu(3);
                        int kyufu2 = CIUtil.RoundInt(receInf.GetKohiKyufu(2), 1) - CIUtil.RoundInt(receInf.GetKohiReceFutan(2), 1) + CIUtil.RoundInt(receInf.GetKohiKyufu(3), 1);

                        adjustHokenReceFutan = kyufu2 - kyufu1;
                    }
                    else
                    {
                        int kyufu1 = receInf.GetKohiKyufu(1) - receInf.GetKohiReceFutan(1) + receInf.GetKohiKyufu(2);
                        int kyufu2 = CIUtil.RoundInt(receInf.GetKohiKyufu(1), 1) - CIUtil.RoundInt(receInf.GetKohiReceFutan(1), 1) + CIUtil.RoundInt(receInf.GetKohiKyufu(2), 1);

                        adjustHokenReceFutan = kyufu2 - kyufu1;
                    }
                }
                else if (ReceFutanKbns.Any(
                    r => r.PtId == receInf.PtId &&
                    r.SinYm == receInf.SinYm &&
                    r.HokenId == receInf.HokenId &&
                    r.IsKohi1 &&
                    !r.IsKohi2))
                {
                    if (receInf.Kohi1HokenSbtKbn == HokenSbtKbn.Choki)
                    {
                        int kyufu1 = receInf.GetKohiKyufu(2) - receInf.GetKohiReceFutan(2) + receInf.GetKohiKyufu(3);
                        int kyufu2 = CIUtil.RoundInt(receInf.GetKohiKyufu(2), 1) - CIUtil.RoundInt(receInf.GetKohiReceFutan(2), 1) + CIUtil.RoundInt(receInf.GetKohiKyufu(3), 1);

                        adjustHokenReceFutan = kyufu2 - kyufu1;
                    }
                    else
                    {
                        int kyufu1 = receInf.GetKohiKyufu(1) - receInf.GetKohiReceFutan(1);
                        int kyufu2 = CIUtil.RoundInt(receInf.GetKohiKyufu(1), 1) - CIUtil.RoundInt(receInf.GetKohiReceFutan(1), 1);

                        adjustHokenReceFutan = kyufu2 - kyufu1;
                    }
                }

                //高額療養費の限度額が10円単位で、限度額を超えた場合はまるめ
                if (receInf.Kohi1ReceKyufu != null) receInf.Kohi1ReceKyufu = CIUtil.RoundInt((int)receInf.Kohi1ReceKyufu, 1);
                if (receInf.Kohi2ReceKyufu != null) receInf.Kohi2ReceKyufu = CIUtil.RoundInt((int)receInf.Kohi2ReceKyufu /*+ adjustFutan(2)*/, 1);
                if (receInf.Kohi3ReceKyufu != null) receInf.Kohi3ReceKyufu = CIUtil.RoundInt((int)receInf.Kohi3ReceKyufu /*+ adjustFutan(3)*/, 1);
                if (receInf.Kohi4ReceKyufu != null) receInf.Kohi4ReceKyufu = CIUtil.RoundInt((int)receInf.Kohi4ReceKyufu /*+ adjustFutan(4)*/, 1);

                if (receInf.HokenReceFutan != null) receInf.HokenReceFutan = CIUtil.RoundInt((int)receInf.HokenReceFutan + adjustHokenReceFutan, 1);
                if (receInf.Kohi1ReceFutan != null) receInf.Kohi1ReceFutan = CIUtil.RoundInt((int)receInf.Kohi1ReceFutan, 1);
                if (receInf.Kohi2ReceFutan != null) receInf.Kohi2ReceFutan = CIUtil.RoundInt((int)receInf.Kohi2ReceFutan, 1);
                if (receInf.Kohi3ReceFutan != null) receInf.Kohi3ReceFutan = CIUtil.RoundInt((int)receInf.Kohi3ReceFutan, 1);
                if (receInf.Kohi4ReceFutan != null) receInf.Kohi4ReceFutan = CIUtil.RoundInt((int)receInf.Kohi4ReceFutan, 1);
            }
            else if (receInf.KogakuOverKbn == KogakuOverStatus.OverOneYen)
            {
                if (receInf.Kohi1ReceKyufu != null && receInf.Kohi1ReceKyufu < receInf.KogakuKohi1Limit &&
                    receInf.Kohi1HokenSbtKbn == HokenSbtKbn.Bunten)
                {
                    int wrkKyufu = CIUtil.RoundInt((int)receInf.Kohi1ReceKyufu, 1);
                    receInf.HokenReceFutan -= receInf.Kohi1ReceKyufu - wrkKyufu;
                    receInf.Kohi1ReceKyufu = wrkKyufu;
                }
                if (receInf.Kohi2ReceKyufu != null && receInf.Kohi2ReceKyufu < receInf.KogakuKohi2Limit &&
                    receInf.Kohi2HokenSbtKbn == HokenSbtKbn.Bunten)
                {
                    int wrkKyufu = CIUtil.RoundInt((int)receInf.Kohi2ReceKyufu, 1);
                    receInf.HokenReceFutan -= receInf.Kohi2ReceKyufu - wrkKyufu;
                    receInf.Kohi2ReceKyufu = wrkKyufu;
                }
                if (receInf.Kohi3ReceKyufu != null && receInf.Kohi3ReceKyufu < receInf.KogakuKohi3Limit &&
                    receInf.Kohi3HokenSbtKbn == HokenSbtKbn.Bunten)
                {
                    int wrkKyufu = CIUtil.RoundInt((int)receInf.Kohi3ReceKyufu, 1);
                    receInf.HokenReceFutan -= receInf.Kohi3ReceKyufu - wrkKyufu;
                    receInf.Kohi3ReceKyufu = wrkKyufu;
                }
                if (receInf.Kohi4ReceKyufu != null && receInf.Kohi4ReceKyufu < receInf.KogakuKohi4Limit &&
                    receInf.Kohi4HokenSbtKbn == HokenSbtKbn.Bunten)
                {
                    int wrkKyufu = CIUtil.RoundInt((int)receInf.Kohi4ReceKyufu, 1);
                    receInf.HokenReceFutan -= receInf.Kohi4ReceKyufu - wrkKyufu;
                    receInf.Kohi4ReceKyufu = wrkKyufu;
                }
            }
        }

        //単独併用判断
        private void SetKohiKisai(ReceInfModel receInf)
        {
            if (receInf.ReceSbt.Length < 4) return;

            receInf.Kohi1ReceKisai = getReceKisai(1, receInf.Kohi1Id);
            receInf.Kohi2ReceKisai = getReceKisai(2, receInf.Kohi2Id);
            receInf.Kohi3ReceKisai = getReceKisai(3, receInf.Kohi3Id);
            receInf.Kohi4ReceKisai = getReceKisai(4, receInf.Kohi4Id);

            int wrkSbt = Convert.ToInt32(receInf.Kohi1ReceKisai) + Convert.ToInt32(receInf.Kohi2ReceKisai) +
                Convert.ToInt32(receInf.Kohi3ReceKisai) + Convert.ToInt32(receInf.Kohi4ReceKisai);
            //主保険を追加
            wrkSbt = !receInf.IsNoHoken ? wrkSbt + 1 : wrkSbt;

            receInf.ReceSbt = receInf.ReceSbt.Substring(0, 2) + wrkSbt.ToString() + receInf.ReceSbt.Substring(3, 1);


            //負担区分に反映
            List<ReceFutanKbnModel> receFutanKbns = ReceFutanKbns.Where(
                r => r.PtId == receInf.PtId &&
                r.SinYm == receInf.SinYm &&
                r.HokenId == receInf.HokenId
            ).ToList();

            foreach (var r in receFutanKbns)
            {
                if (!receInf.Kohi4ReceKisai)
                {
                    r.IsKohi4 = false;
                }
                if (!receInf.Kohi3ReceKisai)
                {
                    r.IsKohi3 = r.IsKohi4;
                    r.IsKohi4 = false;
                }
                if (!receInf.Kohi2ReceKisai)
                {
                    r.IsKohi2 = r.IsKohi3;
                    r.IsKohi3 = r.IsKohi4;
                    r.IsKohi4 = false;
                }
                if (!receInf.Kohi1ReceKisai)
                {
                    r.IsKohi1 = r.IsKohi2;
                    r.IsKohi2 = r.IsKohi3;
                    r.IsKohi3 = r.IsKohi4;
                    r.IsKohi4 = false;
                }
            }


            bool getReceKisai(int kohiNo, int kohiId)
            {
                if (kohiId == 0) return false;

                if (receInf.GetKohiTensu(kohiNo) == 0)
                {
                    switch (HpPrefCd)
                    {
                        //42.長崎県
                        case PrefCode.Nagasaki:
                            //86被爆+12生保の場合、生保は0点でも記載する
                            if (receInf.GetKohiHoubetu(kohiNo) == "12")
                            {
                                for (int kCnt = 1; kCnt < kohiNo; kCnt++)
                                {
                                    if (receInf.GetKohiHoubetu(kCnt) == "86")
                                    {
                                        return true;
                                    }
                                }
                            }
                            break;
                    }

                    if (!_sinKouiFinder.IsSinKouiReceKisai(Hardcode.HospitalID, receInf.PtId, receInf.SinYm, receInf.HokenId, receInf.HokenId2, kohiId))
                    {
                        //当該公費を使用していて、レセに記録する診療行為がない場合は未記載
                        return false;
                    }
                }

                PtKohiModel ptKohi = PtKohis.Find(p => p.HokenId == kohiId);

                if (ptKohi == null) return false;

                bool retKisai = true;

                //レセプト請求区分
                switch (ptKohi.ReceSeikyuKbn)
                {
                    case 1:
                        retKisai = receInf.HokenKbn == HokenKbn.Syaho;
                        break;
                    case 2:
                        retKisai = receInf.HokenKbn == HokenKbn.Kokho;
                        break;
                    case 3:
                        retKisai = false;
                        break;
                    case 4:
                        retKisai = receInf.HokenKbn == HokenKbn.Syaho ||
                            receInf.IsKokuhoKumiai && !ptKohi.ExceptHokensya;
                        break;
                    case 5:
                        retKisai = receInf.HokenKbn == HokenKbn.Kokho ||
                            receInf.IsKokuhoKumiai && ptKohi.ExceptHokensya;
                        break;
                    default:
                        retKisai = true;
                        break;
                }

                //レセプト記載
                switch (ptKohi.ReceKisai)
                {
                    case 1:   //上限未満記載なし
                        if (receInf.GetKohiFutan(kohiNo) == 0 && receInf.GetKohiReceFutan(kohiNo) < receInf.GetKohiLimit(kohiNo)) retKisai = false;
                        break;
                    case 2:   //上限以下記載なし
                        int kohiFutan = receInf.GetKohiFutan(kohiNo);
                        if ((ptKohi.ReceFutanRound == 1) ||
                            (new int[] { 5, 6 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Syaho) ||
                            (new int[] { 3, 8 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Kokho))
                        {
                            kohiFutan = receInf.GetKohiFutan(kohiNo, 1);
                        }
                        else if ((ptKohi.ReceFutanRound == 2) ||
                            (new int[] { 7, 8 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Syaho) ||
                            (new int[] { 4, 6 }.Contains(ptKohi.ReceFutanRound) && receInf.HokenKbn == HokenKbn.Kokho))
                        {
                            kohiFutan = CIUtil.RoundInt(kohiFutan, 1);
                        }

                        if (kohiFutan == 0) retKisai = false;
                        break;
                    case 3:
                        retKisai = false;
                        break;
                    default:  //記載あり
                        break;
                }

                //レセプト記載２
                switch (ptKohi.ReceKisai2)
                {
                    case 1:    //一部負担相当額なし記載あり
                        break;
                    default:   //一部負担相当額なし記載なし
                        if (receInf.GetKohiFutan(kohiNo) == 0)
                        {
                            if (receInf.GetKohiReceFutan(kohiNo) == 0)
                            {
                                retKisai = false;
                            }
                            else if (kohiNo >= 2)
                            {
                                //第２公費（以降）の場合は、全分点で公費負担がない場合も記載なしとする
                                for (int wrkKohiNo = kohiNo - 1; wrkKohiNo >= 1; wrkKohiNo--)
                                {
                                    if (receInf.GetKohiReceKisai(wrkKohiNo))
                                    {
                                        if (receInf.Tensu == receInf.GetKohiTensu(kohiNo) &&
                                            receInf.Tensu == receInf.GetKohiTensu(wrkKohiNo) &&
                                            receInf.GetKohiIchibuSotogaku(wrkKohiNo) == receInf.GetKohiIchibuSotogaku(kohiNo))
                                        {
                                            retKisai = false;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                }

                //都道府県
                switch (ptKohi.PrefNo)
                {
                    //11.埼玉県
                    case PrefCode.Saitama:
                        switch (ptKohi.ReceSpKbn)
                        {
                            //高額療養費が発生する場合は単独扱い（専用紙で請求するため）
                            case 1:
                                if (receInf.KogakuOverKbn != KogakuOverStatus.None) retKisai = false;
                                break;
                        }
                        break;

                    //13.東京都
                    case PrefCode.Tokyo:
                        switch (ptKohi.ReceSpKbn)
                        {
                            //都外は単独
                            case 1:
                                if (receInf.HokenKbn == HokenKbn.Kokho && !PrefCode.isTokyo(receInf.HokensyaNo))
                                {
                                    retKisai = false;
                                }
                                break;
                        }
                        //80136...は未使用でも記載する
                        if (receInf.HokenKbn == HokenKbn.Kokho && CIUtil.Copy(ptKohi.FutansyaNo, 1, 5) == "80136")
                        {
                            retKisai = true;
                        }
                        break;

                    //14.神奈川県
                    case PrefCode.Kanagawa:
                        if (receInf.HokenKbn == HokenKbn.Syaho &&
                            ptKohi.FutansyaNo == "41140120" && ptKohi.FutansyaNo == "41140153" && ptKohi.FutansyaNo == "41140187")
                        {
                            retKisai = false;
                        }
                        break;

                    //28.兵庫県
                    case PrefCode.Hyogo:
                        switch (ptKohi.ReceSpKbn)
                        {
                            case 1:
                                if (receInf.HokenKbn == HokenKbn.Syaho &&
                                    receInf.SinYm >= KaiseiDate.m201704 && receInf.SinYm <= KaiseiDate.m201902 &&
                                    receInf.SinYm != receInf.SeikyuYm)
                                {
                                    //返戻は2020年2月請求分から、月遅れは2019年4月請求分から併用扱い
                                    if ((receInf.SeikyuYm >= KaiseiDate.m202001 && new int[] { 2, 3 }.Contains(receInf.SeikyuKbn)) ||
                                         (receInf.SeikyuYm >= KaiseiDate.m201903 && new int[] { 1 }.Contains(receInf.SeikyuKbn)))
                                    {
                                        retKisai = true;
                                    }
                                }
                                break;
                        }
                        break;

                    //29.奈良県
                    case PrefCode.Nara:
                        switch (ptKohi.ReceSpKbn)
                        {
                            case 1:
                                if (receInf.HokenKbn == HokenKbn.Kokho &&
                                    receInf.SeikyuYm >= KaiseiDate.m201104 &&
                                    PrefCode.isNara(PtHoken?.HokensyaNo) &&
                                    PrefCode.isNara(ptKohi.FutansyaNo))
                                {
                                    retKisai = true;
                                }
                                break;
                        }
                        break;

                    //30.和歌山県
                    case PrefCode.Wakayama:
                        switch (ptKohi.ReceSpKbn)
                        {
                            case 1:
                                //公費負担なし記載になし（全角償還になったケース）
                                if (receInf.GetKohiFutan(kohiNo) == 0)
                                {
                                    retKisai = false;
                                }
                                break;
                        }
                        break;

                    //40.福岡県
                    case PrefCode.Fukuoka:
                        switch (ptKohi.ReceSpKbn)
                        {
                            case 1:
                                if (receInf.HokenKbn == HokenKbn.Syaho && receInf.HonkeKbn == HonkeKbn.Mine)
                                {
                                    retKisai = false;
                                }
                                break;
                        }
                        break;
                }

                return retKisai;
            }
        }

        //特殊処理
        private void SetKisaiSp(ReceInfModel receInf)
        {
            //自立支援+生保 の場合は生保は0円記載ありにする
            if (new string[] { "21", "15", "52", "54" }.Contains(receInf.Kohi1Houbetu) && receInf.Kohi1ReceKisai)
            {
                //主保険なしの場合は0印字あり
                if (receInf.IsNoHoken && receInf.Kohi1ReceFutan == null) receInf.Kohi1ReceFutan = 0;
                //生保は0印字あり
                if (receInf.Kohi2HokenSbtKbn == HokenSbtKbn.Seiho && receInf.Kohi2ReceFutan == null) receInf.Kohi2ReceFutan = 0;
                if (receInf.Kohi3HokenSbtKbn == HokenSbtKbn.Seiho && receInf.Kohi3ReceFutan == null) receInf.Kohi3ReceFutan = 0;
                if (receInf.Kohi4HokenSbtKbn == HokenSbtKbn.Seiho && receInf.Kohi4ReceFutan == null) receInf.Kohi4ReceFutan = 0;
            }
        }

        private void SetJibaiFutan(ReceInfModel receInf)
        {
            if (receInf.HokenKbn == HokenKbn.Jibai)
            {
                //※加算率は月合算に対して適用しないと誤差が発生する
                //Ａ（イ×単価×加算率）
                if (PtHoken != null)
                {
                    int wrkAFutan = CIUtil.RoundInt(receInf.JibaiITensu * PtHoken.EnTen * SystemConf.JibaiRousaiRate, 0);
                    receInf.TotalIryohi += wrkAFutan - receInf.JibaiAFutan;
                    receInf.JibaiAFutan = wrkAFutan;
                }
                //Ｃ（ハ×加算率）
                int wrkCFutan = CIUtil.RoundInt(receInf.JibaiHaFutan * SystemConf.JibaiRousaiRate, 0);
                receInf.TotalIryohi += wrkCFutan - receInf.JibaiCFutan;
                receInf.JibaiCFutan = wrkCFutan;
            }
        }

        //レセ編集情報の設定
        private void SetReceInfEdit(ReceInfModel receInf)
        {
            ReceInfEditModel receInfEdit = ReceInfEdits.Find(
                r =>
                    r.PtId == receInf.PtId &&
                    r.SinYm == receInf.SinYm &&
                    r.HokenId == receInf.HokenId &&
                    r.ReceSbt == receInf.ReceSbt &&
                    r.Houbetu == receInf.Houbetu &&
                    r.Kohi1Houbetu == receInf.Kohi1Houbetu &&
                    r.Kohi2Houbetu == receInf.Kohi2Houbetu &&
                    r.Kohi3Houbetu == receInf.Kohi3Houbetu &&
                    r.Kohi4Houbetu == receInf.Kohi4Houbetu
            );

            if (receInfEdit == null) return;

            //編集前情報を退避
            ReceInfPreEditModel receInfPreEdit = new ReceInfPreEditModel(
                new ReceInfPreEdit()
                {
                    HpId = receInf.HpId,
                    PtId = receInf.PtId,
                    SeikyuYm = receInf.SeikyuYm,
                    SinYm = receInf.SinYm,
                    HokenId = receInf.HokenId,
                    ReceSbt = receInf.ReceSbt,
                    Houbetu = receInf.Houbetu,
                    Kohi1Houbetu = receInf.Kohi1Houbetu,
                    Kohi2Houbetu = receInf.Kohi2Houbetu,
                    Kohi3Houbetu = receInf.Kohi3Houbetu,
                    Kohi4Houbetu = receInf.Kohi4Houbetu,
                    HokenReceTensu = receInf.HokenReceTensu,
                    HokenReceFutan = receInf.HokenReceFutan,
                    Kohi1ReceTensu = receInf.Kohi1ReceTensu,
                    Kohi1ReceFutan = receInf.Kohi1ReceFutan,
                    Kohi1ReceKyufu = receInf.Kohi1ReceKyufu,
                    Kohi2ReceTensu = receInf.Kohi2ReceTensu,
                    Kohi2ReceFutan = receInf.Kohi2ReceFutan,
                    Kohi2ReceKyufu = receInf.Kohi2ReceKyufu,
                    Kohi3ReceTensu = receInf.Kohi3ReceTensu,
                    Kohi3ReceFutan = receInf.Kohi3ReceFutan,
                    Kohi3ReceKyufu = receInf.Kohi3ReceKyufu,
                    Kohi4ReceTensu = receInf.Kohi4ReceTensu,
                    Kohi4ReceFutan = receInf.Kohi4ReceFutan,
                    Kohi4ReceKyufu = receInf.Kohi4ReceKyufu,
                    HokenNissu = receInf.HokenNissu,
                    Kohi1Nissu = receInf.Kohi1Nissu,
                    Kohi2Nissu = receInf.Kohi2Nissu,
                    Kohi3Nissu = receInf.Kohi3Nissu,
                    Kohi4Nissu = receInf.Kohi4Nissu,
                    Tokki = receInf.Tokki,
                    Tokki1 = receInf.Tokki1,
                    Tokki2 = receInf.Tokki2,
                    Tokki3 = receInf.Tokki3,
                    Tokki4 = receInf.Tokki4,
                    Tokki5 = receInf.Tokki5
                }
            );
            ReceInfPreEdits.Add(receInfPreEdit);

            //ユーザー編集を適用
            receInf.HokenReceTensu = receInfEdit.HokenReceTensu;
            receInf.HokenReceFutan = receInfEdit.HokenReceFutan;
            receInf.Kohi1ReceTensu = receInfEdit.Kohi1ReceTensu;
            receInf.Kohi1ReceFutan = receInfEdit.Kohi1ReceFutan;
            receInf.Kohi1ReceKyufu = receInfEdit.Kohi1ReceKyufu;
            receInf.Kohi2ReceTensu = receInfEdit.Kohi2ReceTensu;
            receInf.Kohi2ReceFutan = receInfEdit.Kohi2ReceFutan;
            receInf.Kohi2ReceKyufu = receInfEdit.Kohi2ReceKyufu;
            receInf.Kohi3ReceTensu = receInfEdit.Kohi3ReceTensu;
            receInf.Kohi3ReceFutan = receInfEdit.Kohi3ReceFutan;
            receInf.Kohi3ReceKyufu = receInfEdit.Kohi3ReceKyufu;
            receInf.Kohi4ReceTensu = receInfEdit.Kohi4ReceTensu;
            receInf.Kohi4ReceFutan = receInfEdit.Kohi4ReceFutan;
            receInf.Kohi4ReceKyufu = receInfEdit.Kohi4ReceKyufu;
            receInf.HokenNissu = receInfEdit.HokenNissu;
            receInf.Kohi1Nissu = receInfEdit.Kohi1Nissu;
            receInf.Kohi2Nissu = receInfEdit.Kohi2Nissu;
            receInf.Kohi3Nissu = receInfEdit.Kohi3Nissu;
            receInf.Kohi4Nissu = receInfEdit.Kohi4Nissu;
            receInf.Tokki = receInfEdit.Tokki;
            receInf.Tokki1 = receInfEdit.Tokki1;
            receInf.Tokki2 = receInfEdit.Tokki2;
            receInf.Tokki3 = receInfEdit.Tokki3;
            receInf.Tokki4 = receInfEdit.Tokki4;
            receInf.Tokki = receInfEdit.Tokki;
        }

        /// <summary>
        /// 負担区分の設定
        /// </summary>
        private void SetFutanKbn()
        {
            foreach (var r in ReceFutanKbns)
            {
                r.FutanKbnCd =
                    //１者
                    r.IsHoken && !r.IsKohi1 && !r.IsKohi2 && !r.IsKohi3 && !r.IsKohi4 ? "1" :
                    !r.IsHoken && r.IsKohi1 && !r.IsKohi2 && !r.IsKohi3 && !r.IsKohi4 ? "5" :
                    !r.IsHoken && !r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && !r.IsKohi4 ? "6" :
                    !r.IsHoken && !r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "B" :
                    !r.IsHoken && !r.IsKohi1 && !r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "C" :
                    //２者
                    r.IsHoken && r.IsKohi1 && !r.IsKohi2 && !r.IsKohi3 && !r.IsKohi4 ? "2" :
                    r.IsHoken && !r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && !r.IsKohi4 ? "3" :
                    r.IsHoken && !r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "E" :
                    r.IsHoken && !r.IsKohi1 && !r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "G" :
                    !r.IsHoken && r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && !r.IsKohi4 ? "7" :
                    !r.IsHoken && r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "H" :
                    !r.IsHoken && r.IsKohi1 && !r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "I" :
                    !r.IsHoken && !r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "J" :
                    !r.IsHoken && !r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "K" :
                    !r.IsHoken && !r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "L" :
                    //３者
                    r.IsHoken && r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && !r.IsKohi4 ? "4" :
                    r.IsHoken && r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "M" :
                    r.IsHoken && r.IsKohi1 && !r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "N" :
                    r.IsHoken && !r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "O" :
                    r.IsHoken && !r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "P" :
                    r.IsHoken && !r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "Q" :
                    !r.IsHoken && r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "R" :
                    !r.IsHoken && r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "S" :
                    !r.IsHoken && r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "T" :
                    !r.IsHoken && !r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "U" :
                    //４者
                    r.IsHoken && r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && !r.IsKohi4 ? "V" :
                    r.IsHoken && r.IsKohi1 && r.IsKohi2 && !r.IsKohi3 && r.IsKohi4 ? "W" :
                    r.IsHoken && r.IsKohi1 && !r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "X" :
                    r.IsHoken && !r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "Y" :
                    !r.IsHoken && r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "Z" :
                    //５者
                    r.IsHoken && r.IsKohi1 && r.IsKohi2 && r.IsKohi3 && r.IsKohi4 ? "9" :
                    //不明
                    "";
            }

            //不要なレコードを削除
            ReceFutanKbns.RemoveAll(r => r.FutanKbnCd == "");
        }

        /// <summary>
        /// 受診日等レコード
        /// </summary>
        /// <param name="receInf"></param>
        private void SetReceInfJd(ReceInfModel receInf)
        {
            if (receInf.SinYm < 202109) return;

            ReceInfJdModel HokenJd = new ReceInfJdModel(new ReceInfJd());

            //保険分実日数
            if (!receInf.IsNoHoken)
            {
                var wrkNissus = KaikeiDetails.Where(k =>
                    k.PtId == receInf.PtId &&
                    k.SinDate / 100 == receInf.SinYm &&
                    k.HokenId == receInf.HokenId &&
                    k.IsNoHoken == false
                ).GroupBy(
                    k => k.SinDate
                ).Select(
                    k => new { SinDate = k.Key, Jitunissu = k.Max(x => x.Jitunisu), Tensu = k.Sum(x => x.Tensu) }
                ).ToList();

                var receInfJd = new ReceInfJdModel
                    (
                        new ReceInfJd()
                        {
                            HpId = receInf.HpId,
                            SeikyuYm = receInf.SeikyuYm,
                            PtId = receInf.PtId,
                            SinYm = receInf.SinYm,
                            HokenId = receInf.HokenId,
                            KohiId = 0,
                            FutanSbtCd = 1
                        }
                    );

                for (int i = 1; i <= 31; i++)
                {
                    var wrkNissu = wrkNissus.Find(k => k.SinDate == receInf.SinYm * 100 + i);

                    if (wrkNissu == null)
                    {
                        //来院がない日
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 0);
                    }
                    else if (wrkNissu.Jitunissu >= 1)
                    {
                        //実日数に計上する受診
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 1);
                    }
                    else if (wrkNissu.Tensu >= 1)
                    {
                        //実日数に計上しない受診
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 2);
                    }
                    else
                    {
                        //診療費なし
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 0);
                    }
                }

                ReceInfJds.Add(receInfJd);
                HokenJd = receInfJd;
            }

            //公費実日数
            int futanSbtCd = 1;

            for (int kohiNo = 1; kohiNo <= 4; kohiNo++)
            {
                if (receInf.GetKohiId(kohiNo) == 0) break;
                if (!receInf.GetKohiReceKisai(kohiNo)) continue;

                futanSbtCd++;

                var wrkNissus = KaikeiDetails.Where(k =>
                    k.PtId == receInf.PtId &&
                    k.SinDate / 100 == receInf.SinYm &&
                    (k.HokenId == receInf.HokenId || k.HokenId == receInf.HokenId2) &&
                    (k.Kohi1Id == receInf.GetKohiId(kohiNo) || k.Kohi2Id == receInf.GetKohiId(kohiNo) ||
                     k.Kohi3Id == receInf.GetKohiId(kohiNo) || k.Kohi4Id == receInf.GetKohiId(kohiNo))
                ).GroupBy(
                    k => k.SinDate
                ).Select(
                    k => new { SinDate = k.Key, Jitunissu = k.Max(x => x.Jitunisu), Tensu = k.Sum(x => x.Tensu) }
                ).ToList();

                var receInfJd = new ReceInfJdModel
                    (
                        new ReceInfJd()
                        {
                            HpId = receInf.HpId,
                            SeikyuYm = receInf.SeikyuYm,
                            PtId = receInf.PtId,
                            SinYm = receInf.SinYm,
                            HokenId = receInf.HokenId,
                            KohiId = receInf.GetKohiId(kohiNo),
                            FutanSbtCd = futanSbtCd
                        }
                    );

                for (int i = 1; i <= 31; i++)
                {
                    var wrkNissu = wrkNissus.Find(k => k.SinDate == receInf.SinYm * 100 + i);

                    if (wrkNissu == null)
                    {
                        //来院がない日
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 0);
                    }
                    else if (wrkNissu.Jitunissu >= 1)
                    {
                        //実日数に計上する受診
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 1);
                    }
                    else if (wrkNissu.Tensu >= 1)
                    {
                        //実日数に計上しない受診
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 2);
                    }
                    else
                    {
                        //診療費なし
                        typeof(ReceInfJdModel).GetProperty($"Nissu{i}").SetValue(receInfJd, 0);
                    }
                }

                ReceInfJds.Add(receInfJd);
            }
        }

        /// <summary>
        /// 高額療養費の現物給付有無
        /// </summary>
        //private bool IsKogaku(ReceInfModel receInf)
        //{
        //    //高額療養費の限度額を超過
        //    if (receInf.KogakuOverKbn != KogakuOverStatus.None) return true;

        //    //指定公費の場合は2割換算で上限を超えるかチェックする
        //    if (receInf.IsSiteiKohi)
        //    {

        //    }

        //    //マル長の限度額を超過
        //    if (receInf.TokkiContains("02") || receInf.TokkiContains("16")) return true;

        //    return false;
        //}
    }
}
