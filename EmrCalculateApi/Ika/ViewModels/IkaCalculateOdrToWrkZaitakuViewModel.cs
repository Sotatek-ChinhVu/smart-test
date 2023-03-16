using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using Domain.Constant;
using Helper.Common;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    /// <summary>
    /// オーダー情報からワーク情報へ変換
    /// 在宅
    /// </summary>
    class IkaCalculateOdrToWrkZaitakuViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 数量振替が必要な項目リスト
        /// 2つ目が振り返る項目
        /// ただし、往診は、114002970：往診往復時間加算（２号地域）がオーダーされている場合は、
        /// それを優先する
        /// </summary>
        (string itemCd, string kasanCd, int min)[] ZaiSuryoFurikaels =
        {
            ( ItemCdConst.ZaiOusin, ItemCdConst.ZaiOusinKanka, 60 ),
            ( ItemCdConst.ZaiOusinTokubetu, ItemCdConst.ZaiOusinKankaTokubetu, 60 ),
            ( ItemCdConst.ZaiHoumon1_1Dou, ItemCdConst.ZaiHoumonKanka, 60 ),
            ( ItemCdConst.ZaiHoumon1_1DouIgai, ItemCdConst.ZaiHoumonKanka, 60 ),
            ( ItemCdConst.ZaiHoumon1_2Dou, ItemCdConst.ZaiHoumonKanka, 60 ),
            ( ItemCdConst.ZaiHoumon1_2DouIgai, ItemCdConst.ZaiHoumonKanka, 60 ),
            ( ItemCdConst.ZaiHoumon2i, ItemCdConst.ZaiHoumonKanka, 60 ),
            ( ItemCdConst.ZaiHoumon2ro, ItemCdConst.ZaiHoumonKanka, 60 )
        };

        // 往診のRpにくっつける項目
        private string[] OusinKasanls =
        {
            // 滞在時間加算（１号地域）
            ItemCdConst.ZaiOusinTaizai1Go,
            // 往診往復時間加算（２号地域）
            ItemCdConst.ZaiOusinOufuku2Go
        };

        /// <summary>
        /// 往診の項目リスト
        /// </summary>
        private string[] Oushinls =
        {
            ItemCdConst.ZaiOusin,
            ItemCdConst.ZaiOusinTokubetu
        };

        /// <summary>
        /// 訪問看護の項目リスト
        /// </summary>
        private string[] HoumonKango =
        {
            ItemCdConst.ZaiHoumon1_1Dou,
            ItemCdConst.ZaiHoumon1_1DouIgai,
            ItemCdConst.ZaiHoumon1_2Dou,
            ItemCdConst.ZaiHoumon1_2DouIgai,
            ItemCdConst.ZaiHoumon2i,
            ItemCdConst.ZaiHoumon2ro
        };

        /// <summary>
        /// 在がん医総の項目リスト
        /// </summary>
        private List<string> ZaiganIsols =
            new List<string>
            {
                ItemCdConst.ZaiZaiganSyohoAri,
                ItemCdConst.ZaiZaiganSyohoNasi,
                ItemCdConst.ZaiZaiganByoAriSyohoAri,
                ItemCdConst.ZaiZaiganByoAriSyohoNasi,
                ItemCdConst.ZaiZaiganByoNasiSyohoAri,
                ItemCdConst.ZaiZaiganByoNasiSyohoNasi
            };

        // 今週の初日
        int _weekFirstDate = 0;
        /// <summary>
        /// 今週の初日
        /// </summary>
        private int WeekFirstDate
        {
            get
            {
                if (_weekFirstDate == 0)
                {
                    _weekFirstDate = _common.SinFirstDateOfWeek;
                }
                return _weekFirstDate;
            }
        }

        // 今週末
        int _weekLastDate = 0;
        /// <summary>
        /// 今週末
        /// </summary>
        private int WeekLastDate
        {
            get
            {
                if (_weekLastDate == 0)
                {
                    _weekLastDate = _common.SinLastDateOfWeek;
                }
                return _weekLastDate;
            }
        }

        // 当月初日
        int _monthFirstDate = 0;
        /// <summary>
        /// 当月初日
        /// </summary>
        private int MonthFirstDate
        {
            get
            {
                if (_monthFirstDate == 0)
                {
                    _monthFirstDate = _common.SinFirstDateOfMonth;
                }
                return _monthFirstDate;
            }
        }

        // 当月末日
        int _monthLastDate = 0;
        /// <summary>
        /// 当月末日
        /// </summary>
        private int MonthLastDate
        {
            get
            {
                if (_monthLastDate == 0)
                {
                    _monthLastDate = _common.SinLastDateOfMonth;
                }
                return _monthLastDate;
            }
        }

        // 当月初日の属する週の日曜日
        int _weekFirstDateOfmonthFirstDate = 0;
        /// <summary>
        /// 当月初日の属する週の日曜日
        /// </summary>
        private int WeekFirstDateOfMonthFirstDate
        {
            get
            {
                if (_weekFirstDateOfmonthFirstDate == 0)
                {
                    _weekFirstDateOfmonthFirstDate = _common.GetFirstDateOfWeek(MonthFirstDate);
                }
                return _weekFirstDateOfmonthFirstDate;
            }
        }

        //// 当月初日の属する週の土曜日
        int _weekLastDateOfmonthFirstDate = 0;
        /// <summary>
        /// 当月初日の属する週の土曜日
        /// </summary>
        private int WeekLastDateOfMonthFirstDate
        {
            get
            {
                if (_weekLastDateOfmonthFirstDate == 0)
                {
                    _weekLastDateOfmonthFirstDate = _common.GetLastDateOfWeek(MonthFirstDate);
                }
                return _weekLastDateOfmonthFirstDate;
            }
        }
        // 当月末日の属する週の日曜日
        int _weekFirstDateOfmonthLastDate = 0;
        /// <summary>
        /// 当月末日の属する週の日曜日
        /// </summary>
        private int WeekFirstDateOfMonthLastDate
        {
            get
            {
                if (_weekFirstDateOfmonthLastDate == 0)
                {
                    _weekFirstDateOfmonthLastDate = _common.GetFirstDateOfWeek(MonthLastDate);
                }
                return _weekFirstDateOfmonthLastDate;
            }
        }
        //// 当月末日の属する週の土曜日
        int _weekLastDateOfmonthLastDate = 0;
        /// <summary>
        /// 当月末日の属する週の土曜日
        /// </summary>
        int WeekLastDateOfmonthLastDate
        {
            get
            {
                if (_weekLastDateOfmonthLastDate == 0)
                {
                    _weekLastDateOfmonthLastDate = _common.GetLastDateOfWeek(MonthLastDate);
                }
                return _weekLastDateOfmonthLastDate;
            }
        }

        //// 直近の算定開始日を求める
        int _zaiganSanteiStartDate = 0;
        /// <summary>
        /// 直近の算定開始日を求める
        /// </summary>
        int ZaiganSanteiStartDate
        {
            get
            {
                if (_zaiganSanteiStartDate == 0)
                {
                    _zaiganSanteiStartDate = _common.GetZenkaiOdrDate(_common.sinDate, ItemCdConst.ZaiZaiganStart);
                }
                return _zaiganSanteiStartDate;
            }
        }

        //// 死亡日
        int _deathDate = 0;
        /// <summary>
        /// 死亡日
        /// </summary>
        private int DeathDate
        {
            get
            {
                if (_deathDate == 0)
                {
                    _deathDate = _common.DeathDate;
                    if (_deathDate == 0)
                    {
                        _deathDate = 99999999;
                    }
                }
                return _deathDate;
            }
        }

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkZaitakuViewModel(IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _common = common;

            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 在宅の計算ロジック
        /// </summary>
        public void Calculate()
        {
            const string conFncName = nameof(Calculate);
            _emrLogger.WriteLogStart( this, conFncName, "");

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.Zaitaku, OdrKouiKbnConst.Zaitaku))
            {
                // 保険
                Console.WriteLine("Start CalculateHoken IkaCalculateOdrToWrkZaitakuViewModel");
                CalculateHoken();
                Console.WriteLine("End CalculateHoken IkaCalculateOdrToWrkZaitakuViewModel");

                // 自費
                Console.WriteLine("Start CalculateJihi IkaCalculateOdrToWrkZaitakuViewModel");
                CalculateJihi();
                Console.WriteLine("End CalculateJihi IkaCalculateOdrToWrkZaitakuViewModel");
            }

            Console.WriteLine("Start CommitWrkSinRpInf IkaCalculateOdrToWrkZaitakuViewModel");
            _common.Wrk.CommitWrkSinRpInf();
            Console.WriteLine("End CommitWrkSinRpInf IkaCalculateOdrToWrkZaitakuViewModel");

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 在宅の計算
        /// </summary>
        private void CalculateHoken()
        {
            const string conFncName = nameof(CalculateHoken);

            // 通常算定処理
            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            // 在宅のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbn(OdrKouiKbnConst.Zaitaku);

            if (filteredOdrInf.Any())
            {
                // 前月・翌月の週単位計算調整処理を実行したかどうか（1回でOK）
                bool weekCalc = false;

                // 在医総・施医総不適合減算チェック
                bool zaiisoFutekiSantei = CheckZaiisoFuteki();
                // 在医総・施医総無交付加算チェック
                bool zaiisoMukofu = CheckZaiisoMukofu();

                // 数量振替が必要な項目があるかチェック
                List<(string, double, int)> suryoFurikaels = new List<(string, double, int)>();

                // 往診加算対象項目有無（往診料・在宅患者訪問診療料）
                bool existOushinKasanTarget = ExistsOushinKasanTarget();
                // 往診加算有無（滞在時間加算（１号地域）・往診往復時間加算（２号地域））
                bool existOushinKasan = _common.Odr.ExistOdrDetailByItemCd(OusinKasanls.ToList());

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    // 在医総・施医総無交付加算を追加するRpかどうかの判定
                    bool addMukofu = false;
                    (int HokenPid, int HokenId, int SanteiKbn) mukofuHokenStatus = (0, 0, 0);

                    if (filteredOdrDtl.Any())
                    {
                        // 医科外来等感染症対策実施加算（在宅医療）チェック
                        bool kansenTaisaku = CheckKansenTaisaku(filteredOdrDtl);
                        bool existKansenTaisakuTarget = false;
                        // 外来感染対策向上加算（医学管理等）チェック
                        bool kansenKojo = CheckKansenKojo(filteredOdrDtl);
                        bool existKansenKojoTarget = false;
                        // 連携強化加算（医学管理等）チェック
                        bool renkeiKyoka = CheckRenkeiKyoka(filteredOdrDtl, kansenKojo);
                        // サーベランス強化加算（医学管理等）チェック
                        bool surveillance = CheckSurveillance(filteredOdrDtl, kansenKojo);

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Zaitaku, ReceSinId.Zaitaku, odrInf.SanteiKbn);

                        // 集計先は、後で内容により変更する
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.ZaiSonota, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "C"));

                        suryoFurikaels.Clear();

                        int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        bool commentSkipFlg = (firstItem != 0);
                        // 最初のコメント以外の項目であることを示すフラグ
                        bool firstSinryoKoui = true;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || _common.IsSelectComment(odrDtl.ItemCd)))
                            {
                                // 診療行為・コメント

                                if (existOushinKasanTarget && OusinKasanls.Contains(odrDtl.ItemCd))
                                {
                                    // 往診の加算
                                    // 往診加算対象項目が存在する場合は、ここでは算定しない
                                    commentSkipFlg = true;
                                }
                                else
                                {
                                    if (odrDtl.IsKihonKoumoku)
                                    {
                                        // 基本項目

                                        if (firstSinryoKoui == true)
                                        {
                                            firstSinryoKoui = false;
                                        }
                                        else
                                        {
                                            // 医科外来等感染症対策実施加算対応
                                            if (kansenTaisaku && existKansenTaisakuTarget)
                                            {
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaiKansenTaisaku, autoAdd: 1);
                                                existKansenTaisakuTarget = false;
                                            }

                                            // 外来感染対策向上加算等算定
                                            if (kansenKojo && existKansenKojoTarget)
                                            {
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaitakuKansenKojo, autoAdd: 1);
                                                if (renkeiKyoka)
                                                {
                                                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaitakuRenkeiKyoka, autoAdd: 1);
                                                }
                                                if (surveillance)
                                                {
                                                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaitakuSurveillance, autoAdd: 1);
                                                }
                                                existKansenKojoTarget = false;
                                            }

                                            //最初以外の基本項目が来たらRpを分ける　
                                            //※最初にコメントが入っていると困るのでこういう処理にする
                                            if (odrDtl.Kokuji2 != "7" || IsWeekCalc(odrDtl.ItemCd))
                                            //if (odrDtl.Kokuji1 != "7" && odrDtl.Kokuji1 != "9")
                                            {
                                                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Zaitaku, ReceSinId.Zaitaku, odrInf.SanteiKbn);
                                            }
                                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.ZaiSonota, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "C"));
                                        }
                                    }
                                    else if (
                                        new string[] {
                                            ItemCdConst.CommentOusinJissi,
                                            ItemCdConst.CommentZaiganHoumon,
                                            ItemCdConst.CommentZaiganKango}.Contains(odrDtl.ItemCd))
                                    {
                                        // 往診実施日、在がん訪問日のコメントの場合は別Rpに分ける
                                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Zaitaku, ReceSinId.Zaitaku, odrInf.SanteiKbn);
                                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.ZaiSonota, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "C"));
                                    }

                                    if (!(odrDtl.IsComment))
                                    {
                                        // コメント項目以外
                                        commentSkipFlg = false;

                                        // 集計先の調整
                                        _common.Wrk.wrkSinKouis.Last().SyukeiSaki =
                                            GetSyukeiSaki(odrDtl, _common.Wrk.wrkSinKouis.Last().SyukeiSaki);

                                        // 算定回数チェック
                                        double suryo = odrDtl.Suryo;
                                        if (ZaiSuryoFurikaels.Any(p => p.itemCd == odrDtl.ItemCd))
                                        {
                                            // 数量振替項目は、1でカウントする
                                            suryo = 1;
                                        }

                                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, suryo) == 2)
                                        {
                                            // 算定回数マスタのチェックにより算定不可
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                        }
                                        else if (_common.CheckAge(odrDtl) == 2)
                                        {
                                            // 年齢チェックにより算定不可
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                        }
                                        else
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                            int retKizamiMin = 0;

                                            if (odrDtl.IsZaiisoFutakiTarget || odrDtl.IsZaiiso)
                                            {
                                                // 在医総・施医総施設基準不適合減算
                                                if (odrDtl.IsZaiisoFutakiTarget && zaiisoFutekiSantei)
                                                {
                                                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaiZaiisoFuteki, autoAdd: 1);
                                                }

                                                // 在医総・施医総無交付加算
                                                if (zaiisoMukofu)
                                                {
                                                    //    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaiZaiisoMukofu, autoAdd: 1);
                                                    addMukofu = true;
                                                    mukofuHokenStatus = (odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn);
                                                }
                                            }
                                            else if (IsSuryoFurikae(odrDtl.ItemCd, odrDtl.Suryo, ref retKizamiMin))
                                            {
                                                if (IsOhsinKasanTarget(odrDtl) && existOushinKasan)
                                                {
                                                    // 往診加算対象項目で、往診加算項目が存在する場合、きざみ最小値を30に設定
                                                    retKizamiMin = 30;
                                                }

                                                // 数量の振り替え
                                                if (odrDtl.Suryo > retKizamiMin)
                                                {
                                                    // きざみ下限を超える場合
                                                    _common.Wrk.wrkSinKouiDetails.Last().FmtKbn = FmtKbnConst.Minute;
                                                    _common.Wrk.wrkSinKouiDetails.Last().Suryo2 = _common.Wrk.wrkSinKouiDetails.Last().Suryo;
                                                }

                                                _common.Wrk.wrkSinKouiDetails.Last().Suryo = 1;

                                                //if (odrDtl.ItemCd == ItemCdConst.ZaiOusin && Ousin2Go(odrDtl.Suryo))
                                                //{
                                                //    // 往診で、往診往復加算２号地域がオーダーされている場合
                                                //}
                                                //else
                                                //{
                                                // 後で数量振替処理が必要なものは、ここでリストに追加しておく
                                                suryoFurikaels.Add((odrDtl.ItemCd, odrDtl.Suryo, retKizamiMin));
                                                //}
                                                if (IsOhsinKasanTarget(odrDtl) && existOushinKasan)
                                                {
                                                    // 往診加算対象項目の場合、手オーダーされている往診加算をくっつけて算定する
                                                    OusinKasan(odrDtl.Suryo);
                                                }
                                            }

                                            // 年齢加算自動算定
                                            _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl);

                                            // 医科外来等感染症対策実施加算対応
                                            if (odrDtl.IsKansenTaisakuTargetZaitaku)
                                            {
                                                if (kansenTaisaku)
                                                {
                                                    existKansenTaisakuTarget = true;
                                                    //_common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaiKansenTaisaku, autoAdd: 1);
                                                }
                                            }

                                            // 外来感染対策向上加算の加算対象フラグ
                                            if (odrDtl.IsKansenKojoTargetZaitaku)
                                            {
                                                if (kansenKojo)
                                                {
                                                    existKansenKojoTarget = true;
                                                }
                                            }

                                            // コメント自動追加
                                            _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);

                                            if (IsWeekCalc(odrDtl.ItemCd))
                                            {
                                                bool firstWeek = false;
                                                bool lastWeek = false;
                                                int zaiRpNo = _common.Wrk.RpNo;
                                                int zaiSeqNo = _common.Wrk.SeqNo;

                                                // 週単位計算処理
                                                if (weekCalc == false)
                                                {
                                                    // 月初月末週の特別調整処理（1回だけ通す）
                                                    firstWeek = ZaiWeekCalcFirst(odrDtl.HokenPid, odrDtl.HokenId, odrDtl.SanteiKbn);
                                                    lastWeek = ZaiWeekCalcLast(odrDtl.HokenPid, odrDtl.HokenId, odrDtl.SanteiKbn);
                                                    weekCalc = true;
                                                }

                                                ZaiWeekCalc(odrDtl.ItemCd, odrDtl.HokenPid, odrDtl.HokenId, odrDtl.SanteiKbn);

                                                // 実施日列挙ダミー項目の数量を置き換える
                                                // ExchangeJissiRekkyoSuryo(firstWeek, lastWeek, zaiRpNo, zaiSeqNo);
                                            }
                                        }
                                    }
                                    else if (commentSkipFlg == false || _common.IsSelectComment(odrDtl.ItemCd))
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    }
                                }
                            }
                            else
                            {
                                commentSkipFlg = true;
                            }

                        }

                        if (suryoFurikaels.Any())
                        {
                            // 当該Rpに数量振替が必要な項目が存在した場合、加算用項目を算定する
                            foreach ((string itemCd, double suryo, int kizamiMin) in suryoFurikaels)
                            {
                                SuryoFurikae(itemCd, suryo, kizamiMin, odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn, filteredOdrDtl);
                            }
                        }

                        // 医科外来等感染症対策実施加算対応
                        if (kansenTaisaku && existKansenTaisakuTarget)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaiKansenTaisaku, autoAdd: 1);
                        }

                        // 外来感染対策向上加算等算定
                        if (kansenKojo && existKansenKojoTarget)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaitakuKansenKojo, autoAdd: 1);
                            if (renkeiKyoka)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaitakuRenkeiKyoka, autoAdd: 1);
                            }
                            if (surveillance)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaitakuSurveillance, autoAdd: 1);
                            }
                        }

                        // 在医総・施医総無交付加算
                        if (addMukofu)
                        {
                            // Rp分ける
                            _common.Wrk.AppendNewWrkSinKoui(mukofuHokenStatus.HokenPid, mukofuHokenStatus.HokenId, ReceSyukeisaki.ZaiSonota, cdKbn: _common.GetCdKbn(mukofuHokenStatus.SanteiKbn, "C"));
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ZaiZaiisoMukofu, autoAdd: 1);
                        }

                        // 薬剤・コメント算定
                        commentSkipFlg = (firstItem != 1);
                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.ZaiYakuzai, _common.GetCdKbn(odrInf.SanteiKbn, "C"), ref firstSinryoKoui);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if (odrDtl.IsYorCommentItem(commentSkipFlg))
                            {
                                // 薬剤・コメント
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                commentSkipFlg = false;
                            }
                            else
                            {
                                // 薬剤・コメント以外の項目が来たら、以降のコメントはこの項目に付随するものなのでスキップ
                                commentSkipFlg = true;
                            }
                        }

                        // 特材・コメント算定

                        commentSkipFlg = (firstItem != 2);
                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.ZaiYakuzai, _common.GetCdKbn(odrInf.SanteiKbn, "C"), ref firstSinryoKoui);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if (odrDtl.IsTorCommentItem(commentSkipFlg))
                            {
                                // 特材・コメント
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                commentSkipFlg = false;
                            }
                            else
                            {
                                // 特材・コメント以外の項目が来たら、以降のコメントはこの項目に付随するものなのでスキップ
                                commentSkipFlg = true;
                            }
                        }
                    }
                }

                _common.Wrk.CommitWrkSinRpInf();
            }


            #region Local Method

            // 集計先を返す
            string GetSyukeiSaki(OdrDtlTenModel odrDtl, string defaultSyukeiSaki)
            {
                string retSyukeiSaki = defaultSyukeiSaki;

                if (IsOhsin(odrDtl))
                {
                    // 往診
                    retSyukeiSaki = ReceSyukeisaki.ZaiOusin;
                }
                else if (IsHoumonSinryo(odrDtl))
                {
                    // 訪問診療料
                    retSyukeiSaki = ReceSyukeisaki.ZaiHoumon;
                }
                else if (IsYakanOhsin(odrDtl))
                {
                    // 夜間
                    retSyukeiSaki = ReceSyukeisaki.ZaiYakan;
                }
                else if (IsSinyaOhsin(odrDtl))
                {
                    // 深夜・緊急
                    retSyukeiSaki = ReceSyukeisaki.ZaiSinya;
                }

                return retSyukeiSaki;
            }
            #endregion
        }

        /// <summary>
        /// 往診項目かどうか
        /// </summary>
        /// <param name="odrDtl"></param>
        /// <returns></returns>
        private bool IsOhsin(OdrDtlTenModel odrDtl)
        {
            bool ret = false;

            if (Oushinls.Contains(odrDtl.ItemCd))
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 数量振替項目かどうか
        /// </summary>
        /// <param name="itemCd">チェックする項目の診療行為コード</param>
        /// <returns>true: 数量振替項目</returns>
        private bool IsSuryoFurikae(string itemCd, double suryo, ref int kizamiMin)
        {
            bool ret = false;

            int i = 0;
            while (i < ZaiSuryoFurikaels.GetLength(0))
            {
                if (ZaiSuryoFurikaels[i].itemCd == itemCd)
                {
                    kizamiMin = ZaiSuryoFurikaels[i].min;

                    // きざみ点数を超える場合

                    ret = true;

                    break;
                }

                i++;
            }

            return ret;
        }

        /// <summary>
        /// 数量振替加算項目かどうか
        /// </summary>
        /// <param name="itemCd"></param>
        /// <returns></returns>
        private bool IsSuryoFurikaeKasan(string itemCd)
        {
            bool ret = false;

            int i = 0;
            while (i < ZaiSuryoFurikaels.GetLength(1))
            {
                if (ZaiSuryoFurikaels[i].kasanCd == itemCd)
                {
                    ret = true;
                    break;
                }

                i++;
            }

            return ret;
        }

        /// <summary>
        /// 訪問診療料かどうか
        /// </summary>
        /// <param name="odrDtl"></param>
        /// <returns></returns>
        private bool IsHoumonSinryo(OdrDtlTenModel odrDtl)
        {
            bool ret = false;

            //if(HoumonKango.Contains(itemCd))
            //{
            //    ret = true;
            //}

            if (odrDtl.CdKbn == "C")
            {
                if ((odrDtl.CdKbnno == 1 && odrDtl.CdEdano == 0 && odrDtl.CdKouno == 1) ||
                    (odrDtl.CdKbnno == 1 && odrDtl.CdEdano == 0 && odrDtl.CdKouno == 2) ||
                    (odrDtl.CdKbnno == 1 && odrDtl.CdEdano == 2 && odrDtl.CdKouno == 0))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 夜間往診かどうか
        /// </summary>
        /// <param name="odrDtl"></param>
        /// <returns></returns>
        private bool IsYakanOhsin(OdrDtlTenModel odrDtl)
        {
            bool ret = false;

            if (odrDtl.CdKbn == "C")
            {
                if (odrDtl.ItemName.Contains("夜間往診"))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 深夜・緊急往診かどうか
        /// </summary>
        /// <param name="odrDtl"></param>
        /// <returns></returns>
        private bool IsSinyaOhsin(OdrDtlTenModel odrDtl)
        {
            bool ret = false;

            if (odrDtl.CdKbn == "C")
            {
                if (odrDtl.ItemName.Contains("深夜往診"))
                {
                    ret = true;
                }
                else if (odrDtl.ItemName.Contains("緊急往診"))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 在医総・施医総不適合減算を算定可能かどうかチェック
        /// 手オーダーあり、または、自動算定設定があって取消項目のオーダーがない場合、算定可能
        /// </summary>
        /// <returns></returns>
        private bool CheckZaiisoFuteki()
        {
            bool ret = false;

            // 手オーダー確認
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiZaiisoFuteki);

            if (minIndex >= 0)
            {
                ret = true;

                //オーダーから削除
                _common.Odr.RemoveOdrDtlByItemCd(ItemCdConst.ZaiZaiisoFuteki);
                //while (minIndex >= 0)
                //{
                //    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                //    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiZaiisoFuteki);
                //}
            }
            else
            {
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiZaiisoFutekiCancel);
                if (minIndex >= 0)
                {
                    // 取消項目あり
                    ret = false;

                    //オーダーから削除
                    _common.Odr.RemoveOdrDtlByItemCd(ItemCdConst.ZaiZaiisoFutekiCancel);
                    //while (minIndex >= 0)
                    //{
                    //    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    //    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiZaiisoFuteki);
                    //}
                }
                else if (_common.Mst.ExistAutoSantei(ItemCdConst.ZaiZaiisoFuteki))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 在医総・施医総無交付加算を自動算定可能かどうかチェック
        /// 当月院内処方あり院外処方なしで、取消項目のオーダーがなく、手オーダーもない場合、自動算定可能
        /// </summary>
        /// <returns></returns>
        private bool CheckZaiisoMukofu()
        {
            bool ret = false;

            ret = (_common.Odr.ExistIngaiSyohoInMonth == false && _common.Odr.ExistInnaiSyohoInMonth);

            // 手オーダー確認
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiZaiisoMukofuCancel);

            if (minIndex >= 0)
            {
                // 取消項目があるときは算定しない

                ret = false;

                //オーダーから削除
                _common.Odr.RemoveOdrDtlByItemCd(ItemCdConst.ZaiZaiisoMukofuCancel);
                //while (minIndex >= 0)
                //{
                //    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                //    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiZaiisoMukofuCancel);
                //}
            }
            else
            {
                if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.ZaiZaiisoMukofu))
                {
                    ret = false;
                }
            }

            return ret;
        }
        /// <summary>
        /// 医科外来等感染症対策実施加算（在宅医療）を算定可能かチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckKansenTaisaku(List<OdrDtlTenModel> filteredOdrDtl)
        {
            bool ret = false;
            string itemName = "";

            if (_common.sinDate >= 20210401 && _common.sinDate <= 20210930)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.ZaiKansenTaisaku)
                    {
                        filteredOdrDtl.RemoveAt(i);
                        i = 0;
                        ret = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (ret == true)
                {
                    if (filteredOdrDtl.Any(p =>
                            p.IsKansenTaisakuTargetZaitaku) == false)
                    {
                        ret = false;
                        if (string.IsNullOrEmpty(itemName))
                        {
                            itemName = "医科外来等感染症対策実施加算（在宅医療）";
                        }
                        _common.AppendCalcLog(2, string.Format(FormatConst.NotSanteiReason, itemName, "同一Rpに算定対象となる手技がない"));
                    }
                }
                if (ret == false)
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenTaisakuCancel))
                    {
                        ret = false;
                    }
                    else if (new List<int> { SyosaiConst.SaisinDenwa, SyosaiConst.SaisinDenwa2, SyosaiConst.SyosinCorona,
                    SyosaiConst.SyosinJouhou, SyosaiConst.Syosin2Jouhou, SyosaiConst.SaisinJouhou, SyosaiConst.Saisin2Jouhou}.Contains((int)_common.syosai))
                    {
                        // 電話、通信機器等を用いている場合、自動算定しない
                        ret = false;
                    }
                    else if (_common.Mst.ExistAutoSantei(ItemCdConst.KansenTaisaku))
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// 週単位計算が必要な項目かどうかを判定する
        /// </summary>
        /// <param name="itemCd"></param>
        /// <returns>true: 週単位計算する項目</returns>
        private bool IsWeekCalc(string itemCd)
        {
            bool ret = false;

            if (_common.Mst.ZaiWeekCalcList.Contains(itemCd))
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 週単位計算処理（当月初週（前月末週）分の調整処理）
        /// </summary>
        /// <param name="hokenPid">調整項目算定時の保険組み合わせID</param>
        /// <param name="hokenId">保険ID</param>
        /// <param name="santeiKbn">調整項目算定時の算定区分</param>
        private bool ZaiWeekCalcFirst(int hokenPid, int hokenId, int santeiKbn)
        {
            const string conFncName = nameof(ZaiWeekCalcFirst);

            bool ret = false;

            // 当月初日を求める
            //int monthFirstDate = _common.SinFirstDateOfMonth;

            // 当月初日の属する週の土曜日
            //int weekLastDateOfmonthFirstDate = _common.GetLastDateOfWeek(monthFirstDate);

            // 診療日が属する週の土曜日と当月初日の属する週の土曜日が異なる場合（診療日は当月初週ではない）
            // かつ、当月初日の属する週の土曜日が3日以下（前月末に4日以上ある）の場合
            if (_common.GetLastDateOfWeek(_common.sinDate) != WeekLastDateOfMonthFirstDate)
            {
                // 当月初週以外（日曜始まりの月の場合は調整不要）

                // 当月初日の属する週の日曜日
                //int weekFirstDateOfmonthFirstDate = _common.GetFirstDateOfWeek(monthFirstDate);

                // 直近の算定開始日を求める
                //int santeiStart = _common.GetZenkaiOdrDate(_common.sinDate, ItemCdConst.ZaiZaiganStart);

                // 死亡日を求める
                //int deathDate = _common.DeathDate;
                //if(deathDate == 0)
                //{
                //    deathDate = 99999999;
                //}

                // 算定開始日を決める
                int santeiFirstDate =
                    new int[]
                    {
                        MonthFirstDate,
                        ZaiganSanteiStartDate
                    }.Max();

                // 算定終了日を決める
                int santeiLastDate =
                    new int[]
                    {
                        WeekLastDateOfMonthFirstDate,
                        DeathDate
                    }.Min();

                // 当月初週（前月末週）が月跨ぎである場合
                if (WeekFirstDateOfMonthFirstDate / 100 != WeekLastDateOfMonthFirstDate / 100)
                {
                    // 当月初日が属する週の日曜日～診療日までを取得する
                    List<SanteiDaysModel> santeiDays =
                        _common.GetSanteiDays(WeekFirstDateOfMonthFirstDate, _common.sinDate, _common.Mst.ZaiWeekCalcList, _common.hokenKbn, false, true);

                    if (santeiDays.Count(p => p.SinDate >= MonthFirstDate && p.SinDate <= _common.sinDate) <= 0)
                    {
                        // 当月初回の算定である場合

                        // 算定取消がないかチェック
                        List<int> cancelOdrDays =
                            _common.GetOdrDays(WeekFirstDateOfMonthFirstDate, WeekLastDateOfMonthFirstDate, new List<string> { ItemCdConst.ZaiZaiganCancel }, _common.hokenKbn, true);
                        foreach (SanteiDaysModel santeiDay in santeiDays)
                        {
                            cancelOdrDays.RemoveAll(p => p == santeiDay.SinDate);
                        }

                        foreach (string itemCd in _common.Mst.ZaiWeekCalcList)
                        {
                            if (santeiDays.Count
                                (p => p.SinDate >= WeekFirstDateOfMonthFirstDate && p.SinDate <= WeekLastDateOfMonthFirstDate && p.ItemCd == itemCd) >= 4)
                            {
                                // 前月末週の算定回数が4回以上の場合　※ここは4回なので注意！

                                // 調整項目追加
                                int count = santeiLastDate % 100 - cancelOdrDays.Count(p => p >= santeiFirstDate && p <= santeiLastDate);

                                if (count > 0)
                                {
                                    // 当月初週の日数
                                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Zaitaku, ReceSinId.Zaitaku, santeiKbn);
                                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.ZaiCyosei, cdKbn: _common.GetCdKbn(santeiKbn, "C"), isNodspRece: 1);

                                    _common.Wrk.wrkSinKouis.Last().Count = count;

                                    for (int checkDate = MonthFirstDate; checkDate <= santeiLastDate; checkDate++)
                                    {
                                        if (checkDate != _common.sinDate && santeiDays.Any(p => p.SinDate == checkDate) == false)
                                        {
                                            _common.Wrk.wrkSinKouis.Last().WeekCalcAppendDays.Add(checkDate % 100);
                                        }
                                    }

                                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1, isNodspRece: 1, fmtKbn: FmtKbnConst.ZaiCyosei);
                                    _common.Wrk.wrkSinKouiDetails.Last().ItemName = _common.Wrk.wrkSinKouiDetails.Last().ItemName + "(週４日以上 第１週分)";
                                }

                                ret = true;
                            }
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 週単位計算処理（当月末週（翌月初週）分の調整処理）
        /// </summary>
        /// <param name="hokenPid">調整項目算定時の保険組み合わせID</param>
        /// <param name="hokenId">保険ID</param>
        /// <param name="santeiKbn">調整項目算定時の算定区分</param>
        private bool ZaiWeekCalcLast(int hokenPid, int hokenId, int santeiKbn)
        {
            const string conFncName = nameof(ZaiWeekCalcLast);

            bool ret = false;

            // 当月末日を求める
            //int monthLastDate = _common.SinLastDateOfMonth;

            // 当月末日の属する週の土曜日
            //int weekLastDateOfmonthLastDate = _common.GetLastDateOfWeek(monthLastDate);

            // 
            if (_common.GetLastDateOfWeek(_common.sinDate) != WeekLastDateOfmonthLastDate)
            {
                // 当月末週以外（土曜終わりの月の場合は調整不要）

                // 当月末日の属する週の日曜日
                //int weekFirstDateOfmonthLastDate = _common.GetFirstDateOfWeek(monthLastDate);

                // 直近の算定開始日を求める
                //int santeiStart = _common.GetZenkaiOdrDate(_common.sinDate, ItemCdConst.ZaiZaiganStart);

                // 死亡日を求める
                //int deathDate = _common.DeathDate;
                //if(deathDate == 0)
                //{
                //    deathDate = 99999999;
                //}

                // 算定開始日を決める
                int santeiFirstDate =
                    new int[]
                    {
                        WeekFirstDateOfMonthLastDate,
                        ZaiganSanteiStartDate
                    }.Max();

                // 算定終了日を決める
                int santeiLastDate =
                    new int[]
                    {
                        MonthLastDate,
                        DeathDate
                    }.Min();

                // 当月末週（翌月初週）が月跨ぎである場合
                if (WeekFirstDateOfMonthLastDate / 100 != WeekLastDateOfmonthLastDate / 100)
                {
                    // 当月最終日が属する週の日曜～当月最終日が属する週の土曜日までの算定日を取得する
                    //List<SanteiDaysModel> santeiDays = _common.GetSanteiDays(weekFirstDateOfmonthLastDate, weekLastDateOfmonthLastDate, _common.Mst.ZaiWeekCalcList);
                    List<SanteiDaysModel> santeiDays =
                        _common.GetSanteiDays(_common.sinDate, WeekLastDateOfmonthLastDate, _common.Mst.ZaiWeekCalcList, _common.hokenKbn, false, true);

                    if (santeiDays.Count(p => p.SinDate >= _common.sinDate && p.SinDate <= MonthLastDate) <= 0)
                    {
                        // 当月最後の算定である場合

                        // 算定取消がないかチェック
                        List<int> cancelOdrDays =
                            _common.GetOdrDays(WeekFirstDateOfMonthLastDate, WeekLastDateOfmonthLastDate, new List<string> { ItemCdConst.ZaiZaiganCancel }, _common.hokenKbn, true);

                        foreach (SanteiDaysModel santeiDay in santeiDays)
                        {
                            cancelOdrDays.RemoveAll(p => p == santeiDay.SinDate);
                        }

                        foreach (string itemCd in _common.Mst.ZaiWeekCalcList)
                        {
                            if (santeiDays.Count(p =>
                                    p.SinDate >= WeekFirstDateOfMonthLastDate &&
                                    p.SinDate <= WeekLastDateOfmonthLastDate &&
                                    p.ItemCd == itemCd) >= 4)
                            {
                                // 当月末週の算定回数が4回以上の場合　※ここは4回なので注意！

                                // 調整項目追加
                                int count = santeiLastDate - santeiFirstDate + 1 -
                                    cancelOdrDays.Count(p => p >= santeiFirstDate && p <= santeiLastDate);

                                if (count > 0)
                                {
                                    // 当月末週の日数
                                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Zaitaku, ReceSinId.Zaitaku, santeiKbn);
                                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.ZaiCyosei, cdKbn: _common.GetCdKbn(santeiKbn, "C"), isNodspRece: 1);

                                    _common.Wrk.wrkSinKouis.Last().Count = count;// santeiLastDate - santeiFirstDate + 1;
                                    for (int checkDate = santeiFirstDate; checkDate <= MonthLastDate; checkDate++)
                                    {
                                        if (checkDate != _common.sinDate && santeiDays.Any(p => p.SinDate == checkDate) == false)
                                        {
                                            _common.Wrk.wrkSinKouis.Last().WeekCalcAppendDays.Add(checkDate % 100);
                                        }
                                    }

                                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1, isNodspRece: 1, fmtKbn: FmtKbnConst.ZaiCyosei);
                                    _common.Wrk.wrkSinKouiDetails.Last().ItemName = _common.Wrk.wrkSinKouiDetails.Last().ItemName + "(週４日以上 最終週分)";
                                }

                                ret = true;
                            }
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 週単位計算処理（在がん医総等）
        /// </summary>
        private void ZaiWeekCalc(string itemCd, int hokenPid, int hokenId, int santeiKbn)
        {
            const string conFncName = nameof(ZaiWeekCalc);

            //// 今週の初日を求める
            //int weekFirstDate = _common.SinFirstDateOfWeek;

            //// 今週末を求める
            //int weekLastDate = _common.SinLastDateOfWeek;

            //// 当月初日を求める
            //int monthFirstDate = _common.SinFirstDateOfMonth;

            //// 当月末日を求める
            //int monthLastDate = _common.SinLastDateOfMonth;

            //// 当月初日の属する週の日曜日
            //int weekFirstDateOfmonthFirstDate = 
            //    _common.GetFirstDateOfWeek(monthFirstDate);

            //// 当月初日の属する週の土曜日
            int weekLastDateOfmonthFirstDate =
                _common.GetLastDateOfWeek(MonthFirstDate);

            ////当月末週の属する週の日曜日
            int weekFirstDateOfmonthLastDate =
                _common.GetFirstDateOfWeek(MonthLastDate);

            //// 当月末日の属する週の土曜日
            //int weekLastDateOfmonthLastDate = 
            //    _common.GetLastDateOfWeek(monthLastDate);

            //// 直近の算定開始日を求める
            //int santeiStart = 
            //    _common.GetZenkaiOdrDate(_common.sinDate, ItemCdConst.ZaiZaiganStart);

            //// 死亡日を求める
            //int deathDate = _common.DeathDate;
            //if(deathDate == 0)
            //{
            //    deathDate = 99999999;
            //}

            // 算定開始日を決める
            int santeiFirstDate =
                new int[]
                {
                    WeekFirstDate,
                    MonthFirstDate,
                    ZaiganSanteiStartDate
                }.Max();

            // 算定終了日を決める
            int santeiLastDate =
                new int[]
                {
                    WeekLastDate,
                    MonthLastDate,
                    DeathDate
                }.Min();

            // 算定処理

            // 当月初日が属する週の日曜日から当月末日が属する週の土曜日までの算定日を取得する
            //List<SanteiDaysModel> santeiDays = 
            //    _common.GetSanteiDays(weekFirstDateOfmonthFirstDate, weekLastDateOfmonthLastDate, ZaiganIsols, _common.hokenKbn, false, true);
            List<SanteiDaysModel> santeiDays =
                _common.GetSanteiDays(WeekFirstDateOfMonthFirstDate, WeekLastDateOfmonthLastDate, _common.Mst.ZaiWeekCalcList, _common.hokenKbn, false, true);
            // 算定取消がないかチェック
            List<int> cancelOdrDays =
                _common.GetOdrDays(WeekFirstDateOfMonthFirstDate, WeekLastDateOfmonthLastDate, new List<string> { ItemCdConst.ZaiZaiganCancel }, _common.hokenKbn, true);

            foreach (SanteiDaysModel santeiDay in santeiDays)
            {
                cancelOdrDays.RemoveAll(p => p == santeiDay.SinDate);
            }

            // 当週の算定状況を確認する
            List<SanteiDaysModel> santeiDaysOfSinWeek =
                santeiDays.FindAll(p =>
                    p.SinDate >= WeekFirstDate &&
                    p.SinDate >= ZaiganSanteiStartDate &&
                    p.SinDate <= WeekLastDate &&
                    p.SinDate <= DeathDate &&
                    p.ItemCd == itemCd);
            //List<SanteiDaysModel> santeiDaysOfSinWeek = santeiDays.FindAll(p => p.SinDate >= weekFirstDate && p.SinDate <= _common.sinDate);

            // 診療日が当週の最後の算定日
            var sinDays = santeiDaysOfSinWeek.Where(p => p.SinDate <= MonthLastDate && p.SinDate >= MonthFirstDate).Select(p => p.SinDate);

            // 最後の算定日 or 月末
            //int maxDate = monthLastDate;
            int maxDate = _common.sinDate;
            int minDate = _common.sinDate;

            if (sinDays.Any())
            {
                maxDate = sinDays.Max(p => p);
                minDate = sinDays.Min(p => p);
            }

            // 再計算する診療日のリスト（重複登録しないよう、HashSetにしておく）
            HashSet<int> addCalcStatusDays = new HashSet<int>();

            //if (santeiDays.Any() == false || santeiDaysOfSinWeek.Any() == false || _common.sinDate >= maxDate)
            if (santeiDays.Any() == false || santeiDaysOfSinWeek.Any() == false || santeiDaysOfSinWeek.Count() >= 3)
            {
                // 当週既に3回以上の算定がある場合　※今回算定分があるので3回なので注意！

                int santeiDayCount = santeiDaysOfSinWeek.Count();

                //if (santeiDayCount >= 3)
                //if (santeiDaysOfSinWeek.Count(p => p.SinDate <= _common.sinDate) == 3 ||
                //    (santeiDaysOfSinWeek.Count(p => p.SinDate >= MonthFirstDate && p.SinDate <= MonthLastDate) < 3 && 
                //     ((weekLastDateOfmonthFirstDate >= _common.sinDate && _common.sinDate <= minDate) || 
                //      (weekFirstDateOfmonthLastDate <= _common.sinDate && _common.sinDate >= maxDate)))
                //   )
                //if (santeiDaysOfSinWeek.Count(p => p.SinDate <= _common.sinDate) == 3)
                if (santeiDaysOfSinWeek.Count(p => p.SinDate <= _common.sinDate) == 3 ||
                    (santeiDaysOfSinWeek.Count(p => p.SinDate < MonthFirstDate) > 3 && weekLastDateOfmonthFirstDate >= _common.sinDate && _common.sinDate <= minDate) ||
                    (santeiDaysOfSinWeek.Count() >= 3 && santeiDaysOfSinWeek.Count(p => p.SinDate <= MonthLastDate) < 3 && weekFirstDateOfmonthLastDate <= _common.sinDate && _common.sinDate >= maxDate)
                    )
                {
                    // 当週3回目の算定日 or
                    // 前月末週の算定回数 > 3 and 診療日が当月初週（当月初週の末尾 >= 診療日）and 診療日が当週初日 or
                    // 当週の算定回数 > 3 and 診療日が当月末週（当月末週の初日 <= 診療日）and 診療日が当週末日
                    // 当週の算定回数 >= 3 and (当週月末までの算定回数 < 3) and (診療日が当月初週で最小算定日以前 or 診療日が当月末週で最大算定日以降)
                    // 調整項目追加
                    AppendItem();
                }

                // 過去分再計算
                if (_common.calcMode == CalcModeConst.Normal && santeiDayCount >= 4)
                {
                    //List<int> addCalcStatusDays = new List<int>();

                    for (int i = santeiDayCount - 1; i >= 3; i--)
                    {
                        addCalcStatusDays.Add(santeiDaysOfSinWeek[i].SinDate);
                    }
                }
            }

            // 当月初回で、当月初週が前月から続く場合、前月の最終来院日を再計算する
            if (_common.calcMode == CalcModeConst.Normal)
            {
                if (minDate == _common.sinDate && WeekFirstDateOfMonthFirstDate < MonthFirstDate)
                {
                    List<int> prevMonthSanteiDays = santeiDays.FindAll(p => p.SinDate < MonthFirstDate).Select(p => p.SinDate).ToList();
                    if (prevMonthSanteiDays.Any())
                    {
                        addCalcStatusDays.Add(prevMonthSanteiDays.Max(p => p));
                    }
                }
            }

            // 再計算要求があれば、計算要求を追加する
            if (addCalcStatusDays.Any())
            {
                _common.AppendCalcStatusDays(addCalcStatusDays, 0, 1);
            }

            #region Local Method
            void AppendItem()
            {
                // 調整項目追加
                //int count = (santeiLastDate - santeiFirstDate + 1) - santeiDays.Count(p => p.SinDate >= weekFirstDate && p.SinDate <= _common.sinDate) - 1;
                int count = (santeiLastDate - santeiFirstDate + 1) -
                    santeiDays.Count(p => p.SinDate >= santeiFirstDate && p.SinDate <= santeiLastDate && p.ItemCd == itemCd) - 1 -
                    cancelOdrDays.Count(p => p >= santeiFirstDate && p <= santeiLastDate);

                if (count > 0)
                {
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Zaitaku, ReceSinId.Zaitaku, santeiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.ZaiCyosei, cdKbn: _common.GetCdKbn(santeiKbn, "C"), isNodspRece: 1);

                    // 算定終了日 - 算定開始日 + 1 -当週の算定回数 - 1(今回の分)

                    _common.Wrk.wrkSinKouis.Last().Count = count;

                    for (int checkDate = santeiFirstDate; checkDate <= santeiLastDate; checkDate++)
                    {
                        if (checkDate != _common.sinDate && santeiDays.Any(p => p.SinDate == checkDate) == false)
                        {
                            _common.Wrk.wrkSinKouis.Last().WeekCalcAppendDays.Add(checkDate % 100);
                        }
                    }

                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1, isNodspRece: 1, fmtKbn: FmtKbnConst.ZaiCyosei);

                    _common.Wrk.wrkSinKouiDetails.Last().ItemName = _common.Wrk.wrkSinKouiDetails.Last().ItemName + "(週４日以上)";
                }
            }
            #endregion
        }

        /// <summary>
        /// 往診往復加算２号地域の算定処理
        /// 手オーダーがあれば、往診の後に算定する
        /// </summary>
        /// <param name="suryo"></param>
        /// <returns></returns>
        private bool Ousin2Go(double suryo)
        {
            const string conFncName = nameof(Ousin2Go);

            try
            {
                bool ret = false;

                // 往診の場合、往診往復時間加算（２号地域）をオーダーしている場合は、
                // 指定の数量で算定する
                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiOusinOufuku2Go);

                if (minIndex >= 0)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        if (odrDtl.ItemCd == ItemCdConst.ZaiOusinOufuku2Go)
                        {
                            _common.Wrk.wrkSinKouiDetails.Last().Suryo = suryo;
                        }
                    }

                    ret = true;

                    //オーダーから削除
                    while (minIndex >= 0)
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.ZaiOusinOufuku2Go);
                    }
                }

                return ret;

            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                throw;
            }
        }

        /// <summary>
        /// 往診加算の算定処理
        /// 手オーダーがあれば、往診の後に算定する
        /// </summary>
        /// <param name="suryo"></param>
        /// <returns></returns>
        private bool OusinKasan(double ousinSuryo)
        {
            const string conFncName = nameof(OusinKasan);

            try
            {
                bool ret = false;

                foreach (string tgtItemCd in OusinKasanls)
                {
                    // 往診の場合、手オーダー加算項目をオーダーしている場合は、
                    // 往診と同一Rpにまとめる
                    List<OdrDtlTenModel> odrDtls;
                    int minIndex = 0;
                    int itemCnt = 0;
                    double suryo = 0;

                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(tgtItemCd);

                    if (minIndex >= 0)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            if (odrDtl.ItemCd == tgtItemCd)
                            {
                                _common.Wrk.wrkSinKouiDetails.Last().Suryo = ousinSuryo;
                                suryo = odrDtl.Suryo;
                            }
                        }

                        RecedenCmtSelectModel recedenCmtSelect = _common.Mst.FindRecedenCmtSelect(tgtItemCd, CmtSbtConst.SinryoJikan);

                        if (recedenCmtSelect != null)
                        {
                            if (_common.Odr.odrDtlls.Any(p =>
                                p.ItemCd == recedenCmtSelect.CommentCd &&
                                p.RpNo == odrDtls.First().RpNo &&
                                p.RpEdaNo == odrDtls.First().RpEdaNo) == false)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(recedenCmtSelect.CommentCd, cmtOpt: CIUtil.ToWide(suryo.ToString()), autoAdd: 1);
                            }
                        }

                        ret = true;

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(tgtItemCd);
                        }
                    }
                }

                return ret;

            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                throw;
            }
        }

        /// <summary>
        /// 数量を数量２に振り替え、数量がきざみ下限を超える場合、加算項目を追加する
        /// </summary>
        /// <param name="itemCd"></param>
        /// <param name="suryo"></param>
        /// <param name="kizamiMin"></param>
        private void SuryoFurikae(string itemCd, double suryo, int kizamiMin, int hokenPid, int hokenId, int santeiKbn, List<OdrDtlTenModel> filteredOdrDtls)
        {
            const string conFncName = nameof(SuryoFurikae);

            int i = 0;

            while (i < ZaiSuryoFurikaels.GetLength(0))
            {
                if (itemCd == ZaiSuryoFurikaels[i].itemCd)
                {
                    if (suryo > kizamiMin)
                    {
                        // 新たなRpに追加する
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Zaitaku, ReceSinId.Zaitaku, santeiKbn);
                        _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.ZaiSonota, cdKbn: _common.GetCdKbn(santeiKbn, "C"));

                        List<OdrDtlTenModel> odrDtls;
                        int minIndex = 0;
                        int maxIndex = 0;

                        (odrDtls, minIndex, maxIndex) = _common.Odr.FilterOdrDetailRangeByItemCd(ZaiSuryoFurikaels[i].kasanCd);

                        if (minIndex < 0)
                        {
                            // 手オーダーなしの場合、加算項目を自動算定する
                            // 手オーダーありの場合は、別で算定される                            
                            _common.Wrk.AppendNewWrkSinKouiDetail(ZaiSuryoFurikaels[i].kasanCd, autoAdd: 1);
                            _common.Wrk.wrkSinKouiDetails.Last().Suryo = suryo;

                            RecedenCmtSelectModel recedenCmtSelect = _common.Mst.FindRecedenCmtSelect(ZaiSuryoFurikaels[i].kasanCd, CmtSbtConst.SinryoJikan);

                            if (recedenCmtSelect != null)
                            {
                                if (filteredOdrDtls.Any(p => p.ItemCd == recedenCmtSelect.CommentCd) == false)
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(recedenCmtSelect.CommentCd, cmtOpt: CIUtil.ToWide(suryo.ToString()), autoAdd: 1);
                                }
                            }
                        }

                        break;
                    }
                }

                i++;
            }

        }

        /// <summary>
        /// 在宅の自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {
            const string conFncName = nameof(CalculateJihi);

            _common.CalculateJihi(
                OdrKouiKbnConst.Zaitaku,
                OdrKouiKbnConst.Zaitaku,
                ReceKouiKbn.Zaitaku,
                ReceSinId.Zaitaku,
                ReceSyukeisaki.ZaiSonota,
                "JS");
        }

        private bool ExistsOushinKasanTarget()
        {
            List<OdrDtlTenModel> checkOdrDtls = _common.Odr.FilterOdrDetail();

            if (checkOdrDtls == null || checkOdrDtls.Any() == false)
            {
                return false;
            }

            return checkOdrDtls.Any(p => IsOhsinKasanTarget(p));
        }

        private bool IsOhsinKasanTarget(OdrDtlTenModel odrDtl)
        {
            return IsOhsin(odrDtl) || IsHoumonSinryo(odrDtl);
        }

        /// <summary>
        /// 外来感染対策向上加算（在宅）を算定可能かチェック
        /// </summary>
        /// <returns>true: 外来感染対策向上加算（在宅）を算定可能</returns>
        private bool CheckKansenKojo(List<OdrDtlTenModel> filteredOdrDtl)
        {
            bool santei = false;
            bool santeiJoken = true;
            string itemName = "";

            if (_common.sinDate >= 20220401)
            {
                // 手オーダー確認
                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.ZaitakuKansenKojo)
                    {
                        itemName = filteredOdrDtl[i].ItemName;

                        filteredOdrDtl.RemoveAt(i);
                        i = 0;
                        santei = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (filteredOdrDtl.Any(p => p.IsKansenKojoTargetZaitaku == true) == false)
                {

                    if (string.IsNullOrEmpty(itemName))
                    {
                        itemName = "外来感染対策向上加算（在宅）";
                    }
                    if (santei)
                    {
                        //手オーダーがあるときだけメッセージを出す
                        _common.AppendCalcLog(2, string.Format(FormatConst.NotSanteiReason, itemName, "同一Rpに算定対象となる手技がない"));
                    }
                    santeiJoken = false;
                    santei = false;
                }

                if (santeiJoken && santei == false)
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenKojoCancel))
                    {
                        //取消し
                    }
                    else if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinKansenKojo))
                    {
                        //自動算定
                        santei = true;
                    }
                }

                if (santei)
                {
                    //メッセージがたくさん出てしまうので、算定できるときだけチェックする
                    if (_common.CheckSanteiKaisu(ItemCdConst.ZaitakuKansenKojo, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;

        }

        /// <summary>
        /// 連携強化加算（在宅）を算定可能かチェック
        /// </summary>
        /// <param name="filteredOdrDtl">チェック対象RPのオーダー情報</param>
        /// <param name="kansenKojo">true: 外来感染対策向上加算（在宅）を算定可能</param>
        /// <returns>true: 連携強化加算（在宅）を算定可能</returns>
        private bool CheckRenkeiKyoka(List<OdrDtlTenModel> filteredOdrDtl, bool kansenKojo)
        {
            bool santei = false;

            if (_common.sinDate >= 20220401 && kansenKojo)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.ZaitakuRenkeiKyoka)
                    {
                        filteredOdrDtl.RemoveAt(i);
                        i = 0;
                        santei = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (santei == false)
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.RenkeiKyokaCancel))
                    {
                        //取消し
                    }
                    else if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinRenkeiKyoka))
                    {
                        //自動算定
                        santei = true;
                    }
                }

                if (santei)
                {
                    //メッセージがたくさん出てしまうので、算定できるときだけチェック
                    if (_common.CheckSanteiKaisu(ItemCdConst.ZaitakuRenkeiKyoka, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;
        }

        /// <summary>
        /// サーベランス強化加算（在宅）を算定可能かチェック
        /// </summary>
        /// <param name="filteredOdrDtl">チェック対象RPのオーダー情報</param>
        /// <param name="kansenKojo">true: 外来感染対策向上加算（在宅）を算定可能</param>
        /// <returns>true: サーベランス強化加算（在宅）を算定可能</returns>
        private bool CheckSurveillance(List<OdrDtlTenModel> filteredOdrDtl, bool kansenKojo)
        {
            bool santei = false;

            if (_common.sinDate >= 20220401 && kansenKojo)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.ZaitakuSurveillance)
                    {
                        filteredOdrDtl.RemoveAt(i);
                        i = 0;
                        santei = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (santei == false)
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.SurveillanceCancel))
                    {
                        //取消し
                    }
                    else if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinSurveillance))
                    {
                        //自動算定
                        santei = true;
                    }
                }

                if (santei)
                {
                    //メッセージがたくさん出てしまうので、算定できるときだけチェック
                    if (_common.CheckSanteiKaisu(ItemCdConst.ZaitakuSurveillance, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;
        }

    }
}
