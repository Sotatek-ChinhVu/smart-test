using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using Domain.Constant;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    /// <summary>
    /// オーダー情報からワーク情報へ変換
    /// 医学管理
    /// </summary>
    class IkaCalculateOdrToWrkIgakuViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 小児かかりつけ診療料のリスト
        /// </summary>
        private List<string> SyouniKakaritukels =
            new List<string>
            {
                ItemCdConst.IgakuSyouniKakarituke,
                ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakaritukeSaisinKofuAri,
                ItemCdConst.IgakuSyouniKakaritukeSaisinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke1,
                ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke1SaisinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke1SaisinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke2,
                ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke2SaisinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke2SaisinKofuNasi
            };

        /// <summary>
        /// 小児かかりつけ診療料の時間外加算リスト
        /// </summary>
        private List<string> SyouniKakaritukeJikanls =
            new List<string>
            {
                ItemCdConst.IgakuSyouniKakaritukeSyosinJikangai,
                ItemCdConst.IgakuSyouniKakaritukeSyosinKyujitu,
                ItemCdConst.IgakuSyouniKakaritukeSyosinSinya,
                ItemCdConst.IgakuSyouniKakaritukeSyosinNyuJikangai,
                ItemCdConst.IgakuSyouniKakaritukeSyosinNyuKyujitu,
                ItemCdConst.IgakuSyouniKakaritukeSyosinNyuSinya,
                ItemCdConst.IgakuSyouniKakaritukeSyosinJikangaiToku,
                ItemCdConst.IgakuSyouniKakaritukeSyosinNyuYakan,
                ItemCdConst.IgakuSyouniKakaritukeSaisinJikangai,
                ItemCdConst.IgakuSyouniKakaritukeSaisinKyujitu,
                ItemCdConst.IgakuSyouniKakaritukeSaisinSinya,
                ItemCdConst.IgakuSyouniKakaritukeSaisinNyuJikangai,
                ItemCdConst.IgakuSyouniKakaritukeSaisinNyuKyujitu,
                ItemCdConst.IgakuSyouniKakaritukeSaisinNyuSinya,
                ItemCdConst.IgakuSyouniKakaritukeSaisinJikangaiToku,
                ItemCdConst.IgakuSyouniKakaritukeSaisinNyuYakan
            };

        /// <summary>
        /// 小児科外来診療料のリスト
        /// </summary>
        private List<string> SyouniGairails =
            new List<string>
            {
                ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                ItemCdConst.IgakuSyouniGairaiSaisinKofuAri,
                ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                ItemCdConst.IgakuSyouniGairaiSaisinKofuNasi
            };

        /// <summary>
        /// 小児科外来診療料の時間加算項目のリスト
        /// </summary>
        private List<string> SyouniGairaiJikanls =
            new List<string>
            {
                ItemCdConst.IgakuSyouniGairaiSyosinJikangai,
                ItemCdConst.IgakuSyouniGairaiSyosinJikangaiToku,
                ItemCdConst.IgakuSyouniGairaiSyosinSyouniYakan,
                ItemCdConst.IgakuSyouniGairaiSyosinSyouniKyujitu,
                ItemCdConst.IgakuSyouniGairaiSyosinSyouniSinya,
                ItemCdConst.IgakuSyouniGairaiSaisinJikangai,
                ItemCdConst.IgakuSyouniGairaiSaisinJikangaiToku,
                ItemCdConst.IgakuSyouniGairaiSaisinSyouniYakan,
                ItemCdConst.IgakuSyouniGairaiSaisinSyouniKyujitu,
                ItemCdConst.IgakuSyouniGairaiSaisinSyouniSinya
            };

        /// <summary>
        /// 認知症地域包括診療料のリスト
        /// </summary>
        private List<string> NintiTiikiHoukatuls =
            new List<string>
            {
                ItemCdConst.IgakuNintiTiikiHoukatu1,
                ItemCdConst.IgakuNintiTiikiHoukatu2
            };

        /// <summary>
        /// 認知症地域包括診療料の時間加算リスト
        /// </summary>
        private List<string> NintiTiikiHoukatuJikanls =
            new List<string>
            {
                ItemCdConst.IgakuNintiTiikiHoukatuJikangai,
                ItemCdConst.IgakuNintiTiikiHoukatuKyujitu,
                ItemCdConst.IgakuNintiTiikiHoukatuSinya,
                ItemCdConst.IgakuNintiTiikiHoukatuJikangaiToku,
                ItemCdConst.IgakuNintiTiikiHoukatuNyuJikangai,
                ItemCdConst.IgakuNintiTiikiHoukatuNyuKyujitu,
                ItemCdConst.IgakuNintiTiikiHoukatuNyuSinya,
                ItemCdConst.IgakuNintiTiikiHoukatuNyuJikangaiToku,
                ItemCdConst.IgakuNintiTiikiHoukatuYasou,
                ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniYakan,
                ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniKyujitu,
                ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniSinya,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuJikangai,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuKyujitu,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSinya,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuJikangaiToku,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaYakan,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaKyujitu,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSinya
            };

        /// <summary>
        /// 地域包括診療料のリスト
        /// </summary>
        private List<string> TiikiHoukatuls =
            new List<string>
            {
                ItemCdConst.IgakuTiikiHoukatu1,
                ItemCdConst.IgakuTiikiHoukatu2
            };

        /// <summary>
        /// 地域包括診療料の時間加算リスト
        /// </summary>
        private List<string> TiikiHoukatuJikanls =
            new List<string>
            {
                ItemCdConst.IgakuTiikiHoukatuJikangai,
                ItemCdConst.IgakuTiikiHoukatuKyujitu,
                ItemCdConst.IgakuTiikiHoukatuSinya,
                ItemCdConst.IgakuTiikiHoukatuJikangaiToku,
                ItemCdConst.IgakuTiikiHoukatuNyuJikangai,
                ItemCdConst.IgakuTiikiHoukatuNyuKyujitu,
                ItemCdConst.IgakuTiikiHoukatuNyuSinya,
                ItemCdConst.IgakuTiikiHoukatuNyuJikangaiToku,
                ItemCdConst.IgakuTiikiHoukatuYasou,
                ItemCdConst.IgakuTiikiHoukatuNyuSyouniYakan,
                ItemCdConst.IgakuTiikiHoukatuNyuSyouniKyujitu,
                ItemCdConst.IgakuTiikiHoukatuNyuSyouniSinya,
                ItemCdConst.IgakuTiikiHoukatuNinpuJikangai,
                ItemCdConst.IgakuTiikiHoukatuNinpuKyujitu,
                ItemCdConst.IgakuTiikiHoukatuNinpuSinya,
                ItemCdConst.IgakuTiikiHoukatuNinpuJikangaiToku,
                ItemCdConst.IgakuTiikiHoukatuNinpuSankaYakan,
                ItemCdConst.IgakuTiikiHoukatuNinpuSankaKyujitu,
                ItemCdConst.IgakuTiikiHoukatuNinpuSinya
            };

        /// <summary>
        /// ニコチン依存症のリスト
        /// </summary>
        private List<string> Nicols =
            new List<string>
            {
                ItemCdConst.IgakuNico1,
                ItemCdConst.IgakuNico2_4,
                ItemCdConst.IgakuNico5,
                ItemCdConst.IgakuNico2,
                ItemCdConst.IgakuNanbyoJyohoTusin,
                ItemCdConst.IgakuNico1Rinsyojyo,
                ItemCdConst.IgakuNico5Rinsyojyo
            };

        /// <summary>
        /// オンライン診療料対象医学管理料リスト
        /// </summary>
        private List<string> OnlineTaisyoIgakuKanrils =
            new List<string>
            {
                ItemCdConst.IgakuTokusitu,
                ItemCdConst.IgakuSyouniRyoyo,
                ItemCdConst.IgakuTenkan,
                ItemCdConst.IgakuNanbyo,
                ItemCdConst.IgakuTounyou,
                ItemCdConst.IgakuTounyouToku,
                ItemCdConst.IgakuSeikatuKofuAriSisitu,
                ItemCdConst.IgakuSeikatuKofuAriTounyou,
                ItemCdConst.IgakuSeikatuKofuAriKouketuatu,
                ItemCdConst.IgakuSeikatuKofuNasiSisitu,
                ItemCdConst.IgakuSeikatuKofuNasiTounyou,
                ItemCdConst.IgakuSeikatuKofuNasiKouketuatu,
                ItemCdConst.IgakuTiikiHoukatu1,
                ItemCdConst.IgakuTiikiHoukatu2,
                ItemCdConst.IgakuNintiTiikiHoukatu1,
                ItemCdConst.IgakuNintiTiikiHoukatu2
            };

        /// <summary>
        /// 外来腫瘍化学療法診療料のリスト
        /// </summary>
        private List<string> GairaiSyuyols =
            new List<string>
            {
                ItemCdConst.IgakuGairaiSyuyo1,
                ItemCdConst.IgakuGairaiSyuyo1Sonota,
                ItemCdConst.IgakuGairaiSyuyo2,
                ItemCdConst.IgakuGairaiSyuyo2Sonota
            };

        /// <summary>
        /// 外来腫瘍化学療法診療料の加算項目のリスト
        /// 告示等識別区分１=7
        /// </summary>
        private List<string> GairaiSyuyoKasanls =
            new List<string>
            {
                ItemCdConst.IgakuGairaiSyuyoSyoni,
                ItemCdConst.IgakuGairaiSyuyoRenkeiJujitu,
                ItemCdConst.IgakuGairaiSyuyoBio
            };

        /// <summary>
        /// 外来腫瘍化学療法診療料の時間加算リスト
        /// </summary>
        private List<string> GairaiSyuyoJikanOrNyuls =
            new List<string>
            {
                ItemCdConst.IgakuGairaiSyuyoJikangaiSyosin,
                ItemCdConst.IgakuGairaiSyuyoKyujituSyosin,
                ItemCdConst.IgakuGairaiSyuyoSinyaSyosin,
                ItemCdConst.IgakuGairaiSyuyoJikangaiTokuSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuJikangaiSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuKyujituSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuSinyaSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuJikangaiTokuSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuSyouniYakanSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuSyouniKyujituSyosin,
                ItemCdConst.IgakuGairaiSyuyoNyuSyouniSinyaSyosin,
                ItemCdConst.IgakuGairaiSyuyoJikangaiSaisin,
                ItemCdConst.IgakuGairaiSyuyoKyujituSaisin,
                ItemCdConst.IgakuGairaiSyuyoSinyaSaisin,
                ItemCdConst.IgakuGairaiSyuyoJikangaiTokuSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuJikangaiSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuKyujituSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuSinyaSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuJikangaiTokuSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuSyouniYakanSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuSyouniKyujituSaisin,
                ItemCdConst.IgakuGairaiSyuyoNyuSyouniSinyaSaisin
            };

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkIgakuViewModel(IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _common = common;

            _systemConfigProvider= systemConfigProvider;
            _emrLogger= emrLogger;
        }

        /// <summary>
        /// 医学管理の計算ロジック
        /// </summary>
        public void Calculate()
        {
            const string conFncName = nameof(Calculate);

            _emrLogger.WriteLogStart( this, conFncName, "");

            // 自動算定分を先に処理

            // 認知症地域包括診療料
            Console.WriteLine("Start NintiTiikiHoukatu Caculate 1");
            NintiTiikiHoukatu();
            Console.WriteLine("End NintiTiikiHoukatu Caculate 1");

            // 地域包括診療料
            Console.WriteLine("Start TiikiHoukatu Caculate 2");
            TiikiHoukatu();
            Console.WriteLine("End TiikiHoukatu Caculate 2");

            // 小児かかりつけ診療料
            if (SyouniKakarituke() == false)
            {
                // 小児科外来診療料
                Console.WriteLine("Start SyouniGairai Caculate 3");
                SyouniGairai();
                Console.WriteLine("End SyouniGairai Caculate 3");
            }

            //// 小児かかりつけ診療料
            //if (SyouniKakarituke() == false)
            //{
            //    // 小児科外来診療料
            //    // 小児かかりつけ診療料を算定した場合は算定しない。
            //    SyouniGairai();
            //}

            //if (!(new List<double> { SyosaiConst.Syosin, SyosaiConst.Syosin2, SyosaiConst.SyosinCorona }.Contains(_common.syosai)))
            //{
            //// 認知症地域包括診療料
            //NintiTiikiHoukatu();

            //// 地域包括診療料
            //TiikiHoukatu();
            ////}

            //外来腫瘍化学療法診療料
            Console.WriteLine("Start GairaiSyuyo Caculate 4");
            GairaiSyuyo();
            Console.WriteLine("End GairaiSyuyo Caculate 4");

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.Igaku, OdrKouiKbnConst.Igaku))
            {
                // 保険
                Console.WriteLine("Start CalculateHoken Caculate 5");
                CalculateHoken();
                Console.WriteLine("End CalculateHoken Caculate 5");

                // 自費
                CalculateJihi();
            }

            if (_common.IsRosai && _common.hokenKbn != HokenSyu.After &&
                    new double[] { SyosaiConst.Saisin }.Contains(_common.syosai))
            {
                // 再診時療養指導料
                Console.WriteLine("Start SaisinRyoyo Caculate 6");
                SaisinRyoyo();
                Console.WriteLine("End SaisinRyoyo Caculate 6");
            }

            Console.WriteLine("Start SaisinRyoyo Caculate 7");
            _common.Wrk.CommitWrkSinRpInf();
            Console.WriteLine("End SaisinRyoyo Caculate 7");

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 医学管理の計算
        /// </summary>
        private void CalculateHoken()
        {
            const string conFncName = nameof(CalculateHoken);

            // オンライン診療料の対象項目がオーダーされているかチェックする
            // 自動算定処理でオーダーが削除されるため、事前にチェックしておく
            bool onlineTaisyoIgakuKanri = _common.Odr.FilterOdrDetailByItemCd(OnlineTaisyoIgakuKanrils).Any();

            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            // 医学管理のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbn(OdrKouiKbnConst.Igaku);

            foreach (OdrInfModel odrInf in filteredOdrInf)
            {
                // 行為に紐づく詳細を取得
                filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                if (filteredOdrDtl.Any())
                {
                    // // 医科外来等感染症対策実施加算（医学管理等）チェック
                    bool kansenTaisaku = CheckKansenTaisaku(ref filteredOdrDtl);
                    bool existKansenTaisakuTarget = false;
                    // 外来感染対策向上加算（医学管理等）チェック
                    bool kansenKojo = CheckKansenKojo(filteredOdrDtl);
                    bool existKansenKojoTarget = false;
                    // 連携強化加算（医学管理等）チェック
                    bool renkeiKyoka = CheckRenkeiKyoka(filteredOdrDtl, kansenKojo);
                    // サーベランス強化加算（医学管理等）チェック
                    bool surveillance = CheckSurveillance(filteredOdrDtl, kansenKojo);

                    // 初回、必ずRpと行為のレコードを用意
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, odrInf.SanteiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "B"));

                    int firstItem = _common.CheckFirstItemSbtSyoti(filteredOdrDtl);
                    // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                    bool commentSkipFlg = false;
                    // 最初のコメント以外の項目であることを示すフラグ
                    bool firstSinryoKoui = true;

                    foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                    {
                        if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || _common.IsSelectComment(odrDtl.ItemCd)))
                        {
                            if (odrDtl.IsKihonKoumoku)
                            {
                                // 基本項目
                                if (odrDtl.SyukeiSaki == "A13")
                                {
                                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSido;
                                }

                                if (firstSinryoKoui == true)
                                {
                                    firstSinryoKoui = false;
                                }
                                else
                                {
                                    // 医科外来等感染症対策実施加算
                                    if (kansenTaisaku && existKansenTaisakuTarget)
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuKansenTaisaku, autoAdd: 1);
                                        existKansenTaisakuTarget = false;
                                    }

                                    // 外来感染対策向上加算等算定
                                    if (kansenKojo && existKansenKojoTarget)
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuKansenKojo, autoAdd: 1);
                                        if (renkeiKyoka)
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuRenkeiKyoka, autoAdd: 1);
                                        }
                                        if (surveillance)
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSurveillance, autoAdd: 1);
                                        }
                                        existKansenKojoTarget = false;
                                    }

                                    //最初以外の基本項目が来たらRpを分ける　
                                    //※最初にコメントが入っていると困るのでこういう処理にする
                                    if (odrDtl.Kokuji2 != "7")
                                    {
                                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, odrInf.SanteiKbn);
                                    }
                                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "B"));
                                }
                            }

                            commentSkipFlg = false;

                            if (!(_common.IsCommentItemCd(odrDtl.ItemCd)))
                            {
                                // コメント項目以外

                                // 算定回数チェック
                                if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                                {
                                    // 算定回数マスタのチェックにより算定不可
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                }
                                // 年齢チェック
                                else if (_common.CheckAge(odrDtl) == 2)
                                {
                                    // 年齢チェックにより算定不可
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                }
                                else if (odrDtl.ItemCd == ItemCdConst.IgakuOnlineIgakuKanri &&
                                    (CheckOnlineIgakuKanri() == false))
                                {
                                    // オンライン医学管理料の対象項目がない
                                    // または、直近2カ月にオンライン診療料の算定がない
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                }
                                else if ((odrDtl.ItemCd == ItemCdConst.IgakuSyouniCounseling1 || odrDtl.ItemCd == ItemCdConst.IgakuSyouniCounseling2) &&
                                    (CheckSyouniCounseling(odrDtl) == false))
                                {
                                    // 小児特定疾患カウンセリング料のチェック
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    if (_common.IsRosai && _common.hokenKbn != HokenSyu.After && odrDtl.IsEnKoumoku)
                                    {
                                        // 円項目を含む場合、集計先を変える
                                        _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSido;
                                    }

                                    // 年齢加算自動算定
                                    _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl);

                                    // 医科外来等感染症対策実施加算
                                    if (odrDtl.IsKansenTaisakuTargetIgaku)
                                    {
                                        if (kansenTaisaku)
                                        {
                                            existKansenTaisakuTarget = true;
                                            //_common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuKansenTaisaku, autoAdd: 1);
                                        }
                                    }

                                    // 外来感染対策向上加算の加算対象フラグ
                                    if (odrDtl.IsKansenKojoTargetIgaku)
                                    {
                                        if (kansenKojo)
                                        {
                                            existKansenKojoTarget = true;
                                        }
                                    }

                                    // コメント自動追加
                                    _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);

                                    if (odrDtl.ItemCd == ItemCdConst.IgakuManseiIji)
                                    {
                                        // 慢性維持透析患者外来医学管理料の腎代替療法実績加算
                                        if (filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.IgakuManseiIjiJinDaitai) == false)
                                        {
                                            if (_common.Mst.ExistAutoSantei(ItemCdConst.IgakuManseiIjiJinDaitai))
                                            {
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuManseiIjiJinDaitai, autoAdd: 1);
                                            }
                                        }
                                    }
                                    else if (Nicols.Contains(odrDtl.ItemCd))
                                    {
                                        // ニコチン依存症管理料の施設基準不適合減算
                                        if (filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.IgakuSisetuKijyun) == false)
                                        {
                                            if (_common.Mst.ExistAutoSantei(ItemCdConst.IgakuSisetuKijyun))
                                            {
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSisetuKijyun, autoAdd: 1);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }
                        else
                        {
                            commentSkipFlg = true;
                        }

                    }

                    // 医科外来等感染症対策実施加算
                    if (kansenTaisaku && existKansenTaisakuTarget)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuKansenTaisaku, autoAdd: 1);
                    }

                    // 外来感染対策向上加算等算定
                    if (kansenKojo && existKansenKojoTarget)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuKansenKojo, autoAdd: 1);
                        if (renkeiKyoka)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuRenkeiKyoka, autoAdd: 1);
                        }
                        if (surveillance)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSurveillance, autoAdd: 1);
                        }
                    }

                    // 特材・コメント算定

                    // 最初の項目が特材じゃなければ、最初のコメントは特材用じゃないのでスキップ
                    commentSkipFlg = (firstItem != 2);

                    //改正TODO 特材の集計先が変われば修正する
                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "B"));

                    foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                    {
                        if (_common.IsSelectComment(odrDtl.ItemCd))
                        {
                            // 選択式コメントは手技で対応しているので読み飛ばす
                        }
                        else if (!odrDtl.IsJihi && odrDtl.IsTorCommentItem(commentSkipFlg))
                        {
                            // 特材・コメント
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            commentSkipFlg = false;
                        }
                        else
                        {
                            commentSkipFlg = true;
                        }
                    }
                }
            }

            _common.Wrk.CommitWrkSinRpInf();

            #region Local Method

            /// <summary>
            /// オンライン医学管理料算定可否チェック
            /// 算定不可の場合、ログに記録する
            /// </summary>
            /// <parameter "onlineTaisyoIgakuKanri">オンライン医学管理料の算定対象医学管理料がオーダーされているかどうか</parameter>
            /// <return>false: オンライン医学管理料算定不可</return>
            bool CheckOnlineIgakuKanri()
            {
                //const string calcLogFormat = "オンライン医学管理料は、{0}、算定できません。";

                bool ret = true;

                if (onlineTaisyoIgakuKanri == false)
                {
                    _common.AppendCalcLog(2, string.Format(FormatConst.MaybeNotSanteiOnline, "対象の医学管理料がないため"));
                    //ret = false;
                }
                else if (_common.SanteiCount(_common.MonthsBefore(_common.sinDate, 2), _common.sinDate, ItemCdConst.OnlineSinryo) <= 0)
                {
                    _common.AppendCalcLog(2, string.Format(FormatConst.MaybeNotSanteiOnline, "過去2カ月にオンライン診療を行っていないため"));
                    //ret = false;
                }

                return ret;
            }

            // 小児特定疾患カウンセリングのチェック
            bool CheckSyouniCounseling(OdrDtlTenModel odrDtl)
            {
                bool ret = true;
                if (odrDtl.ItemCd == ItemCdConst.IgakuSyouniCounseling2)
                {
                    // 当月に１回目を算定しているか？
                    if (_common.CheckSanteiTerm(ItemCdConst.IgakuSyouniCounseling1, _common.SinFirstDateOfMonth, _common.sinDate) == false)
                    {
                        // 初回を算定していないので算定不可
                        _common.AppendCalcLog(2, string.Format(FormatConst.NotSanteiReason, odrDtl.ItemName, "'小児特定疾患カウンセリング料（１回目）' を算定していない"));
                        ret = false;
                    }
                }

                if (ret)
                {
                    if (_systemConfigProvider.GetSyouniCounselingCheck() == 0)
                    {
                        int syokaiDate = _common.GetSyokaiDate(_common.sinDate, ItemCdConst.IgakuSyouniCounseling1);

                        if (syokaiDate > 0)
                        {
                            // 2年以上経過
                            int sinYm = _common.sinDate / 100;
                            int syokaiYm = syokaiDate / 100;
                            int sabun = (sinYm / 100 * 12 + sinYm % 100) - (syokaiYm / 100 * 12 + syokaiYm % 100);

                            if (sabun > 24 || (sabun == 24 && (syokaiDate % 100 < _common.sinDate % 100)))
                            {
                                _common.AppendCalcLog(2, string.Format(FormatConst.NotSanteiReason, odrDtl.ItemName, "初回算定日から2年以上経過している"));
                                ret = false;
                            }
                        }
                    }
                }

                return ret;
            }
            #endregion
        }

        /// <summary>
        /// 小児かかりつけ診療慮、小児科外来診療料を自動算定するとき、処方箋を交付するで算定するかどうか
        /// </summary>
        /// <returns>
        /// ture - 処方箋を交付する、で算定
        /// </returns>
        bool IsKofuAriSantei()
        {
            bool ret = false;

            if (_common.sinDate >= 20200401)
            {
                ret = true;

                if (_common.Odr.ExistInnaiSyohoInDay)
                {
                    // 当日に院内処方がある場合、"交付しない"で算定
                    ret = false;
                }
            }
            else
            {
                if (_common.Odr.ExistIngaiSyohoInMonthHokenSyu && !(_common.Odr.ExistInnaiSyohoInDay && _common.jikan > 0))
                {
                    // 当月に院外処方があって、当日院内処方あり＋時間外 ではない場合、"交付する"で算定
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 小児かかりつけ診療料算定処理
        /// 当該項目はオーダーから削除する
        /// </summary>
        private bool SyouniKakarituke()
        {
            // 手技料算定条件が整っているか？
            bool santeiJyoken = true;
            // 手技料を算定したか？
            bool santeiFlg = false;

            int hokenPId = -1;
            int hokenId = -1;
            int santeiKbn = SanteiKbnConst.Santei;

            string itemCd = "";

            // 算定条件チェック
            //if (new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin }.Contains(_common.syosai) == false)
            //if(_common.Odr.FilterOdrDetailByItemCdToday(ItemCdConst.SyosaiKihon)
            //    .Any(p=> new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin }.Contains(p.Suryo)) == false)
            //if(new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin, SyosaiConst.SyosinCorona }.Contains(_common.syosai) == false)
            if (new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin}.Contains(_common.syosai) == false)
            {
                // 当日、初診・再診のオーダーがない場合は算定不可
                santeiJyoken = false;
            }
            //else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.IgakuSyouniGairaiCancel))
            //{
            //    // 小児科外来診療料取消がある場合、は算定不可
            //    santeiJyoken = false;
            //}                
            else if (_common.Mst.ExistAutoSantei(ItemCdConst.IgakuSyouniGairaiSyosinKofuAri) == false)
            {
                //小児科外来診療料自動算定期間外は算定不可
                santeiJyoken = false;
            }
            else if (_common.IsStudent)
            {
                //就学以降は算定不可
                santeiJyoken = false;
            }
            //else if (_common.Wrk.ExistWrkSinKouiDetailByItemCd(SyouniKakaritukels, false))
            else if (_common.ExistWrkOrSinKouiDetailByItemCd(SyouniKakaritukels, false))
            {
                // 既に小児かかりつけ診療料を算定済みの場合
                _common.AppendCalcLog(2, "'小児かかりつけ診療料' は、上限（1回／1日）に達するため、算定できません。");
                santeiJyoken = false;
            }

            // Rpと行為を準備
            string cdKbn = "B";
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, 0);
            _common.Wrk.AppendNewWrkSinKoui(-1, -1, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

            //まず、代表項目があるか検索する
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(new List<string> { ItemCdConst.IgakuSyouniKakarituke, ItemCdConst.IgakuSyouniKakarituke1, ItemCdConst.IgakuSyouniKakarituke2 });

            //見つかったら、適切な項目を算定する
            if (minIndex >= 0)
            {
                if (santeiJyoken)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if ((_common.sinDate < 20220401) && (odrDtl.ItemCd == ItemCdConst.IgakuSyouniKakarituke))
                        {
                            // 代表項目の場合は、初再診から自動算定
                            //if (_common.syosai == SyosaiConst.Syosin)
                            if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou, SyosaiConst.SyosinCorona }.Contains(_common.syosai))
                            {
                                // 初診
                                //if (_common.Odr.ExistIngaiSyohoInMonth)
                                if (IsKofuAriSantei())
                                {
                                    //院外あり
                                    itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri;
                                }
                                else
                                {
                                    //院外なし
                                    itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi;
                                }

                            }
                            else if (new double[] { SyosaiConst.Saisin, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                            {
                                // 再診
                                //if (_common.Odr.ExistIngaiSyohoInMonth)
                                if (IsKofuAriSantei())
                                {
                                    //院外あり
                                    itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinKofuAri;
                                }
                                else
                                {
                                    //院外なし
                                    itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinKofuNasi;
                                }

                            }
                            //odrDtl.ItemCd = itemCd;
                            _common.Odr.UpdateOdrDtlItemCd(odrDtl, itemCd);
                        }
                        else if (odrDtl.ItemCd == ItemCdConst.IgakuSyouniKakarituke1)
                        {
                            //小児かかりつけ診療料１
                            // 代表項目の場合は、初再診から自動算定
                            if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou, SyosaiConst.SyosinCorona }.Contains(_common.syosai))
                            {
                                // 初診
                                if (IsKofuAriSantei())
                                {
                                    //院外あり
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri;
                                }
                                else
                                {
                                    //院外なし
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi;
                                }

                            }
                            else if (new double[] { SyosaiConst.Saisin, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                            {
                                // 再診
                                if (IsKofuAriSantei())
                                {
                                    //院外あり
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke1SaisinKofuAri;
                                }
                                else
                                {
                                    //院外なし
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke1SaisinKofuNasi;
                                }

                            }
                            _common.Odr.UpdateOdrDtlItemCd(odrDtl, itemCd);
                        }
                        else if (odrDtl.ItemCd == ItemCdConst.IgakuSyouniKakarituke2)
                        {
                            //小児かかりつけ診療料２
                            // 代表項目の場合は、初再診から自動算定
                            if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou, SyosaiConst.SyosinCorona }.Contains(_common.syosai))
                            {
                                // 初診
                                if (IsKofuAriSantei())
                                {
                                    //院外あり
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri;
                                }
                                else
                                {
                                    //院外なし
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi;
                                }

                            }
                            else if (new double[] { SyosaiConst.Saisin, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                            {
                                // 再診
                                if (IsKofuAriSantei())
                                {
                                    //院外あり
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke2SaisinKofuAri;
                                }
                                else
                                {
                                    //院外なし
                                    itemCd = ItemCdConst.IgakuSyouniKakarituke2SaisinKofuNasi;
                                }

                            }
                            _common.Odr.UpdateOdrDtlItemCd(odrDtl, itemCd);
                        }

                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        santeiFlg = true;

                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                        santeiKbn = odrDtls[0].SanteiKbn;
                        _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = odrDtls[0].SanteiKbn;
                        _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                        _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;

                        cdKbn = _common.GetCdKbn(odrDtls[0].SanteiKbn, "B");
                        _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;
                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(new List<string> { ItemCdConst.IgakuSyouniKakarituke, ItemCdConst.IgakuSyouniKakarituke1, ItemCdConst.IgakuSyouniKakarituke2 });
                }

            }
            else
            {
                //見つからなかったら、小児かかりつけ診療料の項目がオーダーされているか検索する
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniKakaritukels);

                if (minIndex >= 0)
                {
                    if (santeiJyoken)
                    {
                        //見つかったら、そのまま算定する
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            if (SyouniKakaritukels.Contains(odrDtl.ItemCd))
                            {
                                // 機能強化加算の算定可否判定に使用するため、記憶しておく
                                itemCd = odrDtl.ItemCd;
                            }
                        }

                        santeiFlg = true;

                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                        santeiKbn = odrDtls[0].SanteiKbn;
                        _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = odrDtls[0].SanteiKbn;
                        _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                        _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;
                        cdKbn = _common.GetCdKbn(odrDtls[0].SanteiKbn, "B");
                        _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;
                    }

                    //オーダーから削除
                    while (minIndex >= 0)
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniKakaritukels);
                    }
                }
            }

            // 小児抗菌薬適正使用支援加算 初診時のみ算定可
            SyouniKakaritukeKoukin(santeiFlg && (new[] {ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi}.Contains(itemCd)));

            // 乳幼児感染予防策加算
            NyuyojiKansen(santeiFlg, hokenPId, hokenId, santeiKbn, false);

            // 医科外来等感染症対策実施加算対応
            KansenTaisaku(santeiFlg, hokenPId, hokenId, santeiKbn, false);

            //外来感染対策向上加算
            bool kojo = KansenKojo(santeiFlg, hokenPId, hokenId, santeiKbn, false);
            //連携強化加算
            RenkeiKyoka(kojo, hokenPId, hokenId, santeiKbn, false);
            //サーベランス強化加算
            Surveillance(kojo, hokenPId, hokenId, santeiKbn, false);

            // 時間加算（当日算定があれば、算定可）
            //_common.Wrk.AppendNewWrkSinKoui(hokenPId, hokenId, ReceSyukeisaki.Igaku, cdKbn: cdKbn);
            //SyouniKakaritukeJikanKasan(santeiFlg || _common.Wrk.ExistWrkSinKouiDetailByItemCd(SyouniKakaritukels,false), hokenPId, hokenId, santeiKbn);
            SyouniKakaritukeJikanKasan(santeiFlg || _common.ExistTodayKouiDetailByItemCd(itemCds: SyouniKakaritukels, onlyThisRaiin: false, includeDelete: true), hokenPId, hokenId, santeiKbn);

            // 機能強化加算 初診時のみ算定可
            //_common.Wrk.AppendNewWrkSinKoui(hokenPId, hokenId, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

            SyouniKakaritukeKinokyoka(santeiFlg && (new[] {ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi}.Contains(itemCd)), hokenPId, hokenId, santeiKbn);

            //if (hokenPId >= 0)
            //{
            //    // 手オーダーがあった場合は、その保険IDで更新しておく
            //    _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
            //}


            return santeiFlg;
        }

        /// <summary>
        /// 小児かかりつけ診療料の時間外加算算定処理
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void SyouniKakaritukeJikanKasan(bool santei, int pid, int hid, int santeiKbn)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniKakaritukeJikanls);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santei)
                {
                    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "B"));

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniKakaritukeJikanls);
                }
            }
            else if (santei)
            {
                string itemCd = "";

                // 自動算定
                if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou }.Contains(_common.syosai))
                {
                    // 初診
                    if (_common.jikan == JikanConst.JikanGai)
                    {
                        // 時間外
                        itemCd = ChoiceItemCdNyu(ItemCdConst.IgakuSyouniKakaritukeSyosinJikangai, ItemCdConst.IgakuSyouniKakaritukeSyosinNyuJikangai);
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        // 休日
                        itemCd = ChoiceItemCdNyu(ItemCdConst.IgakuSyouniKakaritukeSyosinKyujitu, ItemCdConst.IgakuSyouniKakaritukeSyosinNyuKyujitu);
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        // 深夜
                        itemCd = ChoiceItemCdNyu(ItemCdConst.IgakuSyouniKakaritukeSyosinSinya, ItemCdConst.IgakuSyouniKakaritukeSyosinNyuSinya);
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        // 夜間早朝

                        if (_common.IsYoJi)
                        {
                            if (_common.IsHolidaySinDate)
                            {
                                // 休日
                                itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinNyuKyujitu;
                            }
                            else if (_common.IsSinyaTime)
                            {
                                // 深夜
                                itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinNyuSinya;
                            }
                            else
                            {
                                // 夜間
                                itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinNyuYakan;
                            }
                        }
                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        //特例夜間
                        itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinNyuYakan;
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        //特例休日
                        itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinNyuKyujitu;
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        //特例深夜
                        itemCd = ItemCdConst.IgakuSyouniKakaritukeSyosinNyuSinya;
                    }
                }
                else if (new double[] { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                {
                    // 再診
                    if (_common.jikan == JikanConst.JikanGai)
                    {
                        // 時間外
                        itemCd = ChoiceItemCdNyu(ItemCdConst.IgakuSyouniKakaritukeSaisinJikangai, ItemCdConst.IgakuSyouniKakaritukeSaisinNyuJikangai);
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        // 休日
                        itemCd = ChoiceItemCdNyu(ItemCdConst.IgakuSyouniKakaritukeSaisinKyujitu, ItemCdConst.IgakuSyouniKakaritukeSaisinNyuKyujitu);
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        // 深夜
                        itemCd = ChoiceItemCdNyu(ItemCdConst.IgakuSyouniKakaritukeSaisinSinya, ItemCdConst.IgakuSyouniKakaritukeSaisinNyuSinya);
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        // 夜間早朝
                        if (_common.IsYoJi)
                        {
                            if (_common.IsHolidaySinDate)
                            {
                                // 休日
                                itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinNyuKyujitu;
                            }
                            else if (_common.IsSinyaTime)
                            {
                                // 深夜
                                itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinNyuSinya;
                            }
                            else
                            {
                                // 夜間
                                itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinNyuYakan;
                            }
                        }
                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        //特例夜間
                        itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinNyuYakan;
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        //特例休日
                        itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinNyuKyujitu;
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        //特例深夜
                        itemCd = ItemCdConst.IgakuSyouniKakaritukeSaisinNyuSinya;
                    }

                }

                if (itemCd != "")
                {
                    if (pid <= 0)
                    {
                        pid = _common.syosaiPid;
                        hid = _common.syosaiHokenId;
                        santeiKbn = _common.syosaiSanteiKbn;
                    }
                    _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));
                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                }
            }
        }

        /// <summary>
        /// 小児かかりつけ診療料の小児抗菌薬適正使用支援加算算定処理
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// ※小児かかりつけ診療料と同じRpに算定する必要がある
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void SyouniKakaritukeKoukin(bool santei)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniKakaritukeKokin);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santei)
                {
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuSyouniKakaritukeKokin, odrDtls.First().SanteiKbn, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSyouniKakaritukeKokin, isDeleted: DeleteStatus.DeleteFlag);
                    }
                    else
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniKakaritukeKokin);
                }
            }
        }

        /// <summary>
        /// 小児かかりつけ診療料の機能強化加算算定処理
        /// 手入力されていない場合は、AUTO_SANTEI_MSTを参照し、登録があれば算定する
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void SyouniKakaritukeKinokyoka(bool santei, int pid, int hid, int santeiKbn)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            // 機能強化加算をオーダーしているかチェック
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniKakaritukeKinoKyoka);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santei)
                {
                    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "B"));

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniKakaritukeKinoKyoka);
                }
            }
            else if (santei)
            {
                if (_common.syosai == SyosaiConst.SyosinCorona)
                {
                    // コロナ電話初診の場合は算定不可
                }
                else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KinoKyokaCancel) == false)
                {
                    // 見つからない場合、自動算定設定があるかチェックする

                    if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinKinoKyoka))
                    {
                        // 自動算定設定あり
                        if (pid <= 0)
                        {
                            pid = _common.syosaiPid;
                            hid = _common.syosaiHokenId;
                            santeiKbn = _common.syosaiSanteiKbn;
                        }

                        _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSyouniKakaritukeKinoKyoka, autoAdd: 1);
                    }
                }
            }
        }

        /// <summary>
        /// 乳幼児感染予防策加算算定処理
        /// 手入力されていない場合は、AUTO_SANTEI_MSTを参照し、登録があれば算定する
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void NyuyojiKansen(bool santei, int pid, int hid, int santeiKbn, bool odrdel = true)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            if (_common.Age < 6 && _common.sinDate >= 20201215 && _common.sinDate <= 20220331)
            {
                // 乳幼児感染予防策加算をオーダーしているかチェック
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuNyuyojiKansen);

                if (minIndex >= 0)
                {
                    //見つかったら、そのまま算定する
                    if (santei)
                    {
                        //if (pid != odrDtls.First().HokenPid)
                        //{
                        //    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "B"));
                        //}

                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                    }

                    //オーダーから削除
                    if (santei || odrdel)
                    {
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuNyuyojiKansen);
                        }
                    }
                }
                else if (santei)
                {
                    if (_common.syosai == SyosaiConst.SyosinCorona)
                    {
                        // コロナ電話初診の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.NyuyojiKansenCancel) == false)
                    {
                        // 見つからない場合、自動算定設定があるかチェックする

                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinNyuyojiKansen))
                        {
                            // 自動算定設定あり
                            if (pid <= 0)
                            {
                                pid = _common.syosaiPid;
                                hid = _common.syosaiHokenId;
                                santeiKbn = _common.syosaiSanteiKbn;

                                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));
                            }
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuNyuyojiKansen, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 医科外来等感染症対策実施加算算定処理
        /// 手入力されていない場合は、AUTO_SANTEI_MSTを参照し、登録があれば算定する
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void KansenTaisaku(bool santei, int pid, int hid, int santeiKbn, bool odrdel = true)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            if (_common.sinDate >= 20210401 && _common.sinDate <= 20210930)
            {
                // 医科外来等感染症対策実施加算をオーダーしているかチェック
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuKansenTaisaku);

                if (minIndex >= 0)
                {
                    //見つかったら、そのまま算定する
                    if (santei)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                    }

                    //オーダーから削除
                    if (santei || odrdel)
                    {
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuKansenTaisaku);
                        }
                    }
                }
                else if (santei)
                {
                    if (_common.syosai == SyosaiConst.SyosinCorona)
                    {
                        // コロナ電話初診の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenTaisakuCancel) == false)
                    {
                        // 見つからない場合、自動算定設定があるかチェックする

                        if (_common.Mst.ExistAutoSantei(ItemCdConst.KansenTaisaku))
                        {
                            // 自動算定設定あり
                            if (pid <= 0)
                            {
                                pid = _common.syosaiPid;
                                hid = _common.syosaiHokenId;
                                santeiKbn = _common.syosaiSanteiKbn;

                                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));
                            }
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuKansenTaisaku, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 外来等感染対策向上加算算定処理
        /// 手入力されていない場合は、AUTO_SANTEI_MSTを参照し、登録があれば算定する
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private bool KansenKojo(bool santei, int pid, int hid, int santeiKbn, bool odrdel = true)
        {
            bool ret = false;
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            if (_common.sinDate >= 20220401)
            {
                // 外来等感染対策向上加算をオーダーしているかチェック
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuKansenKojo);

                if (santei)
                {
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuKansenKojo, 0, 0, 0, minIndex < 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }

                if (minIndex >= 0)
                {
                    //見つかったら、そのまま算定する
                    if (santei)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            ret = true;
                        }
                    }

                    //オーダーから削除
                    if (santei || odrdel)
                    {
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuKansenKojo);
                        }
                    }
                }
                else if (santei)
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenKojoCancel) == false)
                    {
                        // 見つからない場合、自動算定設定があるかチェックする

                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinKansenKojo))
                        {
                            // 自動算定設定あり
                            if (pid <= 0)
                            {
                                pid = _common.syosaiPid;
                                hid = _common.syosaiHokenId;
                                santeiKbn = _common.syosaiSanteiKbn;

                                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));
                            }
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuKansenKojo, autoAdd: 1);
                            ret = true;
                        }
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// 連携強化加算算定処理
        /// 手入力されていない場合は、AUTO_SANTEI_MSTを参照し、登録があれば算定する
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void RenkeiKyoka(bool santei, int pid, int hid, int santeiKbn, bool odrdel = true)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            if (_common.sinDate >= 20220401)
            {
                if (santei)
                {
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuRenkeiKyoka, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }

                // 外来等感染対策向上加算をオーダーしているかチェック
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuRenkeiKyoka);

                if (minIndex >= 0)
                {
                    //見つかったら、そのまま算定する
                    if (santei)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                    }

                    //オーダーから削除
                    if (santei || odrdel)
                    {
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuRenkeiKyoka);
                        }
                    }
                }
                else if (santei)
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.RenkeiKyokaCancel) == false)
                    {
                        // 見つからない場合、自動算定設定があるかチェックする

                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinRenkeiKyoka))
                        {
                            // 自動算定設定あり
                            if (pid <= 0)
                            {
                                pid = _common.syosaiPid;
                                hid = _common.syosaiHokenId;
                                santeiKbn = _common.syosaiSanteiKbn;

                                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));
                            }
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuRenkeiKyoka, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// サーベイラインス強化加算算定処理
        /// 手入力されていない場合は、AUTO_SANTEI_MSTを参照し、登録があれば算定する
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void Surveillance(bool santei, int pid, int hid, int santeiKbn, bool odrdel = true)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            if (_common.sinDate >= 20220401)
            {
                if (santei)
                {
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuSurveillance, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }

                // 外来等感染対策向上加算をオーダーしているかチェック
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSurveillance);

                if (minIndex >= 0)
                {
                    //見つかったら、そのまま算定する
                    if (santei)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                    }

                    //オーダーから削除
                    if (santei || odrdel)
                    {
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSurveillance);
                        }
                    }
                }
                else if (santei)
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.SurveillanceCancel) == false)
                    {
                        // 見つからない場合、自動算定設定があるかチェックする

                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinSurveillance))
                        {
                            // 自動算定設定あり
                            if (pid <= 0)
                            {
                                pid = _common.syosaiPid;
                                hid = _common.syosaiHokenId;
                                santeiKbn = _common.syosaiSanteiKbn;

                                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));
                            }
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSurveillance, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 小児外来診療料算定処理
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        private void SyouniGairai()
        {
            bool _IsSanteiAge()
            {
                bool ret = false;

                if (_common.sinDate >= 20200401)
                {
                    // 6歳未満または、6歳誕生月で当月、診療日以前に小児科外来診療料を算定している人
                    ret = (_common.IsYoJi == true ||
                           (_common.Age == 6 && _common.AgeMonth == 0 &&
                            _common.CheckSanteiTerm(SyouniGairails, _common.sinDate / 100 * 100 + 1, _common.sinDate)));
                }
                else
                {
                    // 3歳未満または、3歳誕生月で当月、診療日以前に小児科外来診療料を算定している人
                    ret = (_common.IsNyuyoJi == true ||
                           (_common.Age == 3 && _common.AgeMonth == 0 &&
                            _common.CheckSanteiTerm(SyouniGairails, _common.sinDate / 100 * 100 + 1, _common.sinDate)));
                }

                return ret;
            }

            string itemCd = "";
            // 手技料を算定する条件が整っているか？
            bool santeiJyoken = true;
            // 手技料を算定したか？
            bool santeiFlg = false;

            int hokenPId = -1;
            int hokenId = -1;
            int santeiKbn = SanteiKbnConst.Santei;

            // 算定条件チェック
            //if (new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin }.Contains(_common.syosai) == false)
            //if (_common.Odr.FilterOdrDetailByItemCdToday(ItemCdConst.SyosaiKihon)
            //    .Any(p => new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin }.Contains(p.Suryo)) == false)
            //if(new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin, SyosaiConst.SyosinCorona }.Contains(_common.syosai) == false)
            if (new double[] { SyosaiConst.Syosin, SyosaiConst.Saisin }.Contains(_common.syosai) == false)
            {
                // 当日、初診・再診のオーダーがない場合は算定不可
                santeiJyoken = false;
            }
            else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.IgakuSyouniGairaiCancel))
            {
                // 小児科外来診療料取消がある場合、は算定不可
                santeiJyoken = false;
            }
            else if (_common.Mst.ExistAutoSantei(ItemCdConst.IgakuSyouniGairaiSyosinKofuAri) == false)
            {
                //小児科外来診療料自動算定期間外は算定不可
                santeiJyoken = false;
            }
            else if (!_IsSanteiAge())
            {
                //算定可能年齢ではない場合、算定不可
                santeiJyoken = false;
            }
            //else if(_common.Wrk.ExistWrkSinKouiDetailByItemCd(SyouniGairails, false))
            else if (_common.ExistWrkOrSinKouiDetailByItemCd(SyouniGairails, false))
            {
                // 既に小児科外来診療料を算定済みの場合
                _common.AppendCalcLog(2, "'小児科外来診療料' は、上限（1回／1日）に達するため、算定できません。");
                santeiJyoken = false;
            }
            //else if(_common.Odr.FilterOdrDetailByItemCdToday(SyouniKakaritukels).Any() || _common.Wrk.ExistWrkSinKouiDetailByItemCd(SyouniKakaritukels, false))
            else if (_common.Odr.FilterOdrDetailByItemCdToday(SyouniKakaritukels).Any() || _common.ExistWrkOrSinKouiDetailByItemCd(SyouniKakaritukels, false))
            {
                // 小児かかりつけ診療料が当日にオーダーまたは算定されている場合は算定しない
                santeiJyoken = false;
            }

            // Rpと行為を準備
            string cdKbn = "B";
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, 0);
            _common.Wrk.AppendNewWrkSinKoui(-1, -1, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

            // 手オーダー確認
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniGairails);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santeiJyoken)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                        if (SyouniGairails.Contains(odrDtl.ItemCd))
                        {
                            // 機能強化加算の算定可否判定に使用するため、記憶しておく
                            itemCd = odrDtl.ItemCd;
                        }
                    }
                    santeiFlg = true;
                    hokenPId = odrDtls[0].HokenPid;
                    hokenId = odrDtls[0].HokenId;
                    santeiKbn = odrDtls[0].SanteiKbn;
                    _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = santeiKbn;
                    _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                    _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;
                    cdKbn = _common.GetCdKbn(santeiKbn, "B");
                    _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniGairails);
                }
            }
            else if (santeiJyoken)
            {
                if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou, SyosaiConst.SyosinCorona }.Contains(_common.syosai))
                {
                    // 初診
                    //if (_common.Odr.ExistIngaiSyohoInMonth)
                    if (IsKofuAriSantei())
                    {
                        //院外あり
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinKofuAri;
                    }
                    else
                    {
                        //院外なし
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi;
                    }

                }
                else if (new double[] { SyosaiConst.Saisin, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                {
                    // 再診
                    //if (_common.Odr.ExistIngaiSyohoInMonth)
                    if (IsKofuAriSantei())
                    {
                        //院外あり
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinKofuAri;
                    }
                    else
                    {
                        //院外なし
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinKofuNasi;
                    }

                }

                if (itemCd != "")
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
                    santeiFlg = true;

                    // 保険ID・算定区分チェック
                    // 加算項目があれば、そのフラグを使用する
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniGairaiKokin);

                    if (minIndex < 0)
                    {
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuNyuyojiKansen);
                    }

                    if (minIndex < 0)
                    {
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniGairaiJikanls);
                    }

                    if (minIndex < 0)
                    {
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniGairaiKinoKyoka);
                    }

                    if (minIndex >= 0)
                    {
                        //見つかったら、そのまま算定する
                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                        santeiKbn = odrDtls[0].SanteiKbn;
                    }
                    else
                    {
                        hokenPId = _common.syosaiPid;
                        hokenId = _common.syosaiHokenId;
                        santeiKbn = _common.syosaiSanteiKbn;
                    }

                    _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = santeiKbn;
                    _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                    _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;
                    cdKbn = _common.GetCdKbn(santeiKbn, "B");
                    _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;

                    if(_common.sinDate < 20200401 && 
                        new string[] { ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi, ItemCdConst.IgakuSyouniGairaiSaisinKofuNasi }.Contains(itemCd) &&
                        (_common.Odr.ExistIngaiSyohoInMonthHokenSyu && (_common.Odr.ExistInnaiSyohoInDay && _common.jikan > 0)))
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSyouniGairaiJikangaiComment, autoAdd: 1);
                    }
                }
            }

            // 小児抗菌薬適正使用支援加算 初診時のみ算定可
            // 小児科外来診療料と同一Rpに算定する
            SyouniGairaiKoukin(santeiFlg && (new[] {ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi}.Contains(itemCd)));

            // 乳幼児感染予防策加算
            NyuyojiKansen(santeiFlg, hokenPId, hokenId, santeiKbn);

            // 医科外来等感染症対策実施加算対応
            KansenTaisaku(santeiFlg, hokenPId, hokenId, santeiKbn, false);

            //外来感染対策向上加算
            bool kojo = KansenKojo(santeiFlg, hokenPId, hokenId, santeiKbn, false);
            //連携強化加算
            RenkeiKyoka(kojo, hokenPId, hokenId, santeiKbn, false);
            //サーベランス強化加算
            Surveillance(kojo, hokenPId, hokenId, santeiKbn, false);

            // ここからはRpを分ける

            // 時間加算（同日に算定があれば算定可能）
            // -> 2021/11/30 同日に算定があっても、ここでは算定せず、初再診の時間加算を削除しないように変更
            // 　 包括マスタと調整処理で対処する
            _common.Wrk.AppendNewWrkSinKoui(hokenPId, hokenId, ReceSyukeisaki.Igaku, cdKbn: "B");
            //SyouniGairaiJikanKasan(santeiFlg || _common.Wrk.ExistWrkSinKouiDetailByItemCd(SyouniGairails, false), hokenPId, hokenId, santeiKbn);
            SyouniGairaiJikanKasan(santeiFlg || _common.ExistTodayKouiDetailByItemCd(itemCds: SyouniGairails, onlyThisRaiin: false, includeDelete: true), hokenPId, hokenId, santeiKbn);

            // 機能強化加算 初診時のみ算定可
            _common.Wrk.AppendNewWrkSinKoui(hokenPId, hokenId, ReceSyukeisaki.Igaku, cdKbn: "B");
            SyouniGairaiKinokyoka(santeiFlg && (new[] {ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi}.Contains(itemCd)), hokenPId, hokenId, santeiKbn);

            //if (hokenPId >= 0)
            //{
            //    // 手オーダーがあった場合は、その保険IDで更新しておく
            //    _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
            //}
        }

        /// <summary>
        /// 小児科外来診療料の小児抗菌薬適正使用支援加算算定処理
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// ※小児科外来診療料と同じRpに算定する必要がある
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void SyouniGairaiKoukin(bool santei)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            // 小児抗菌薬適正使用支援加算がオーダーされているかチェックする
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniGairaiKokin);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santei)
                {
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuSyouniGairaiKokin, odrDtls.First().SanteiKbn, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSyouniGairaiKokin, isDeleted: DeleteStatus.DeleteFlag);
                    }
                    else
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniGairaiKokin);
                }
            }
        }

        /// <summary>
        /// 小児科外来診療料の時間外加算算定処理
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void SyouniGairaiJikanKasan(bool santei, int pid, int hid, int santeiKbn)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniGairaiJikanls);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santei)
                {
                    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrDtls[0].SanteiKbn, "B"));

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyouniGairaiJikanls);
                }
            }
            else if (santei)
            {
                string itemCd = "";

                // 自動算定
                if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou }.Contains(_common.syosai))
                {
                    // 初診
                    if (_common.jikan == JikanConst.JikanGai)
                    {
                        // 時間外
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinJikangai;
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        // 休日
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniKyujitu;
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        // 深夜
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniSinya;
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        // 夜間早朝
                        if (_common.IsHolidaySinDate)
                        {
                            // 休日
                            itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniKyujitu;
                        }
                        else if (_common.IsSinyaTime)
                        {
                            // 深夜
                            itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniSinya;
                        }
                        else
                        {
                            // 夜間
                            itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniYakan;
                        }
                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        //特例夜間
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniYakan;
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        //特例休日
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniKyujitu;
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        //特例深夜
                        itemCd = ItemCdConst.IgakuSyouniGairaiSyosinSyouniSinya;
                    }
                }
                //else if (new double[] { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa}.Contains(_common.syosai))
                else if (new double[] { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                {
                    // 再診
                    if (_common.jikan == JikanConst.JikanGai)
                    {
                        // 時間外
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinJikangai;
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        // 休日
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniKyujitu;
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        // 深夜
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniSinya;
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        // 夜間早朝
                        if (_common.IsHolidaySinDate)
                        {
                            // 休日
                            itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniKyujitu;
                        }
                        else if (_common.IsSinyaTime)
                        {
                            // 深夜
                            itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniSinya;
                        }
                        else
                        {
                            // 夜間
                            itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniYakan;
                        }
                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        //特例夜間
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniYakan;
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        //特例休日
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniKyujitu;
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        //特例深夜
                        itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniSinya;
                    }
                }

                if (itemCd != "")
                {
                    if (pid <= 0)
                    {
                        pid = _common.syosaiPid;
                        hid = _common.syosaiHokenId;
                        santeiKbn = _common.syosaiSanteiKbn;
                    }
                    _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));

                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
                }
            }
        }

        /// <summary>
        /// 小児科外来診療料の機能強化加算算定処理
        /// 手入力されていない場合は、AUTO_SANTEI_MSTを参照し、登録があれば算定する
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void SyouniGairaiKinokyoka(bool santei, int pid, int hid, int santeiKbn)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniGairaiKinoKyoka);

            //見つかったら、適切な項目を算定する
            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santei)
                {
                    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(odrDtls[0].SanteiKbn, "B"));

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniGairaiKinoKyoka);
                }
            }
            else if (santei)
            {
                if (_common.syosai == SyosaiConst.SyosinCorona)
                {
                    // コロナ電話初診の場合は算定不可
                }
                else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KinoKyokaCancel) == false)
                {
                    if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinKinoKyoka))
                    {
                        if (pid <= 0)
                        {
                            pid = _common.syosaiPid;
                            hid = _common.syosaiHokenId;
                            santeiKbn = _common.syosaiSanteiKbn;
                        }
                        _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.Igaku, cdKbn: _common.GetCdKbn(santeiKbn, "B"));

                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSyouniGairaiKinoKyoka, autoAdd: 1);
                    }
                }
            }
        }

        /// <summary>
        /// 認知症地域包括診療料
        /// 当該項目はオーダーから削除する
        /// </summary>
        private void NintiTiikiHoukatu()
        {
            // 算定条件
            bool santeiJyoken = true;
            bool santeiJikanJyoken = false;
            int hokenPId = _common.syosaiPid;
            int hokenId = _common.syosaiHokenId;
            int santeiKbn = -1;
            string santeiJyokenMessage = "";

            //算定条件チェック
            if (new List<double> { SyosaiConst.Syosin, SyosaiConst.Syosin2, SyosaiConst.SyosinCorona, SyosaiConst.SyosinJouhou, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
            {
                // 初診時不可
                santeiJyoken = false;
                santeiJikanJyoken = false;

                santeiJyokenMessage = "初診時の";
            }
            else if (_common.ExistWrkOrSinKouiDetailByItemCd(
                new List<string> { ItemCdConst.Syosin, ItemCdConst.Syosin2, ItemCdConst.Syosin2Rousai, ItemCdConst.SyosinCorona,
                    ItemCdConst.SyosinJouhou, ItemCdConst.Syosin2Jouhou }, false))
            {
                // 初診時不可
                santeiJyoken = false;
                santeiJikanJyoken = false;

                santeiJyokenMessage = "初診算定日の";
            }
            else if (_common.Odr.ExistOdrDetailHoumonSinryo())
            {
                // 訪問診療時不可
                santeiJyoken = false;
                santeiJikanJyoken = false;

                santeiJyokenMessage = "訪問診療時の";
            }
            else if (_common.CheckSanteiTerm(NintiTiikiHoukatuls, _common.SinFirstDateOfMonth, _common.sinDate))
            {
                // 当月内に算定があるときは算定不可
                santeiJyoken = false;
                santeiJyokenMessage = "上限（1回／1月）に達している";

                // ただし、時間加算は算定可
                santeiJikanJyoken = true;
            }

            // Rpと行為を準備
            string cdKbn = "B";
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, 0);
            _common.Wrk.AppendNewWrkSinKoui(-1, -1, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            //if (santeiJyoken)
            //{
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuNintiTiikiHoukatu1);

            //見つかったら、適切な項目を算定する
            if (minIndex >= 0)
            {
                if (santeiJyoken == false)
                {
                    _common.AppendCalcLog(2, $"'認知症地域包括診療料' は、{santeiJyokenMessage}ため、算定できません。");
                }
                else
                {
                    if (_common.CheckSanteiNintiTiiki(ItemCdConst.IgakuNintiTiikiHoukatu1, "認知症地域包括診療料", odrDtls.First().SanteiKbn, 0))
                    {
                        if (_common.CheckSanteiKaisu(ItemCdConst.IgakuNintiTiikiHoukatu1, odrDtls.First().SanteiKbn, 0) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuNintiTiikiHoukatu1, isDeleted: DeleteStatus.DeleteFlag);
                        }
                        else
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                if (odrDtl.ItemCd == ItemCdConst.IgakuNintiTiikiHoukatu1)
                                {
                                    hokenPId = odrDtl.HokenPid;
                                    hokenId = odrDtl.HokenId;
                                    santeiKbn = odrDtls[0].SanteigaiKbn;

                                    // 医科外来等感染症対策実施加算
                                    KansenTaisaku(true, hokenPId, hokenId, santeiKbn);

                                    //外来感染対策向上加算
                                    bool kojo = KansenKojo(true, hokenPId, hokenId, santeiKbn);
                                    //連携強化加算
                                    RenkeiKyoka(kojo, hokenPId, hokenId, santeiKbn);
                                    //サーベランス強化加算
                                    Surveillance(kojo, hokenPId, hokenId, santeiKbn);
                                }

                            }
                        }

                        santeiJikanJyoken = true;
                    }
                }
            }
            else
            {
                //見つからなかったら
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuNintiTiikiHoukatu2);

                //見つかったら、適切な項目を算定する
                if (minIndex >= 0)
                {
                    if (santeiJyoken == false)
                    {
                        _common.AppendCalcLog(2, $"'認知症地域包括診療料' は、{santeiJyokenMessage}ため、算定できません。");
                    }
                    else
                    {
                        if (_common.CheckSanteiNintiTiiki(ItemCdConst.IgakuNintiTiikiHoukatu2, "認知症地域包括診療料", odrDtls.First().SanteiKbn, 0))
                        {
                            if (_common.CheckSanteiKaisu(ItemCdConst.IgakuNintiTiikiHoukatu2, odrDtls.First().SanteiKbn, 0) == 2)
                            {
                                // 算定回数マスタのチェックにより算定不可
                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuNintiTiikiHoukatu2, isDeleted: DeleteStatus.DeleteFlag);
                            }
                            else
                            {
                                foreach (OdrDtlTenModel odrDtl in odrDtls)
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    if (odrDtl.ItemCd == ItemCdConst.IgakuNintiTiikiHoukatu2)
                                    {
                                        // 医科外来等感染症対策実施加算
                                        KansenTaisaku(true, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteigaiKbn);

                                        //外来感染対策向上加算
                                        bool kojo = KansenKojo(true, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteigaiKbn);
                                        //連携強化加算
                                        RenkeiKyoka(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteigaiKbn);
                                        //サーベランス強化加算
                                        Surveillance(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteigaiKbn);
                                    }
                                }
                                hokenPId = odrDtls[0].HokenPid;
                                hokenId = odrDtls[0].HokenId;
                                santeiKbn = odrDtls[0].SanteigaiKbn;
                            }

                            santeiJikanJyoken = true;
                        }
                    }
                }
            }
            //}

            //オーダーから削除
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuls);
            while (minIndex >= 0)
            {
                _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuls);
            }

            if (santeiJyoken)
            {
                // 算定区分を割り当てる
                if (santeiKbn < 0)
                {
                    // 見つからない場合、加算がないかチェック
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuJikanls);

                    if (minIndex >= 0)
                    {
                        // あればその算定区分で算定
                        santeiKbn = odrDtls[0].SanteiKbn;
                    }
                    else
                    {
                        // 加算も見つからない場合
                        santeiKbn = _common.syosaiSanteiKbn;
                    }
                }

                _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = santeiKbn;
                cdKbn = _common.GetCdKbn(santeiKbn, "B");
                _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;

                // 保険組み合わせ番号を割り当てる
                if (hokenPId < 0)
                {
                    // 見つからない場合、加算がないかチェック
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuJikanls);

                    if (minIndex >= 0)
                    {
                        // あればその算定区分で算定
                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                        santeiKbn = odrDtls[0].SanteigaiKbn;
                    }
                    else
                    {
                        // 加算も見つからない場合
                        hokenPId = _common.syosaiPid;
                        hokenId = _common.syosaiHokenId;
                        santeiKbn = _common.syosaiSanteiKbn;
                    }
                }
                _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;

            }

            if (santeiJikanJyoken)
            {
                // 時間加算（基本項目と別に算定）

                if (santeiJyoken == false)
                {
                    // 算定区分を割り当てる
                    if (santeiKbn < 0)
                    {
                        // 見つからない場合、加算がないかチェック
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuJikanls);

                        if (minIndex >= 0)
                        {
                            // あればその算定区分で算定
                            santeiKbn = odrDtls[0].SanteiKbn;
                            hokenPId = odrDtls[0].HokenPid;
                            hokenId = odrDtls[0].HokenId;
                        }
                        else
                        {
                            // 加算も見つからない場合
                            santeiKbn = _common.syosaiSanteiKbn;
                            hokenPId = _common.syosaiPid;
                            hokenId = _common.syosaiHokenId;
                        }
                    }
                    cdKbn = _common.GetCdKbn(santeiKbn, "B");
                }

                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, santeiKbn);
                _common.Wrk.AppendNewWrkSinKoui(hokenPId, hokenId, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

                NintiTiikiHoukatuJikan();
            }

            //#region Local Method
            //// 算定可否チェック
            //bool SanteiCheck(string checkItemCd)
            //{
            //    bool result = true;

            //    if (_common.CheckSanteiKaisu(checkItemCd) == 2)
            //    {
            //        //算定上限を超える為、算定不可
            //        result = false;
            //    }
            //    else if (_common.CheckNaifuku5Syu())
            //    {
            //        //同月に1処方につき5種類を超える内服薬の投薬がある場合は算定しない
            //        _common.AppendCalcLog(2, "'認知症地域包括診療料' は、同月に1処方につき5種類を超える内服薬の投薬があるため、算定できません。");
            //        result = false;
            //    }
            //    else if (_common.CheckKouseisin())
            //    {
            //        //同月に1処方につき抗うつ薬、抗精神病薬、抗不安薬又は睡眠薬を合わせて3種類を超えて投薬を行った場合は算定しない
            //        //ただし、抗うつ薬及び、抗精神病薬については、臨時に投薬した場合は種類数に含めない
            //        _common.AppendCalcLog(2, "'認知症地域包括診療料' は、同月に1処方につき抗うつ薬、抗精神病薬、抗不安薬又は睡眠薬を合わせて3種類を超えて投薬があるため、算定できません。");
            //        result = false;
            //    }

            //    return result;
            //}
            //#endregion
        }

        /// <summary>
        /// 認知症地域包括診療料の時間加算
        /// 手技料とは別に算定できる
        /// 当該項目はオーダーから削除する
        /// </summary>
        /// <param name="santei"></param>
        private void NintiTiikiHoukatuJikan()
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;
            int hokenPId = -1;
            int santeiKbn = -1;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuJikanls);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                }

                // 保険組み合わせIDを記憶
                hokenPId = odrDtls[0].HokenPid;
                santeiKbn = odrDtls[0].SanteiKbn;

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuJikanls);
                }
            }
            else
            {
                string itemCd = "";

                // 自動算定
                if (_common.jikan == JikanConst.JikanGai)
                {
                    // 時間外
                    itemCd = ChoiceItemCdNyuNinpu(
                                ItemCdConst.IgakuNintiTiikiHoukatuJikangai,
                                ItemCdConst.IgakuNintiTiikiHoukatuNyuJikangai,
                                ItemCdConst.IgakuNintiTiikiHoukatuNinpuJikangai);
                }
                else if (_common.jikan == JikanConst.Kyujitu)
                {
                    // 休日
                    itemCd = ChoiceItemCdNyuNinpu(
                                ItemCdConst.IgakuNintiTiikiHoukatuKyujitu,
                                ItemCdConst.IgakuNintiTiikiHoukatuNyuKyujitu,
                                ItemCdConst.IgakuNintiTiikiHoukatuNinpuKyujitu);
                }
                else if (_common.jikan == JikanConst.Sinya)
                {
                    // 深夜
                    itemCd = ChoiceItemCdNyuNinpu(
                                ItemCdConst.IgakuNintiTiikiHoukatuSinya,
                                ItemCdConst.IgakuNintiTiikiHoukatuNyuSinya,
                                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSinya);
                }
                else if (_common.jikan == JikanConst.Yasou)
                {
                    // 夜間早朝

                    if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                    {
                        // 6歳未満、小児科標榜あり
                        itemCd = ChoiceItemCdTime(
                                    ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniYakan,
                                    ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniKyujitu,
                                    ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniSinya
                                    );
                    }
                    else if (_common.IsNinpu && (_common.Mst.GetHyoboSanka() == 1))
                    {
                        // 妊婦、産科標榜あり
                        itemCd = ChoiceItemCdTime(
                                    ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaYakan,
                                    ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaKyujitu,
                                    ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaSinya
                                    );
                    }
                    else
                    {
                        // その他
                        itemCd = ItemCdConst.IgakuNintiTiikiHoukatuYasou;
                    }
                }
                else if (_common.jikan == JikanConst.YakanKotoku)
                {
                    //特例夜間
                    if (_common.IsYoJi)
                    {
                        itemCd = ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniYakan;
                    }
                    else
                    {
                        itemCd = ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaYakan;
                    }
                }
                else if (_common.jikan == JikanConst.KyujituKotoku)
                {
                    //特例休日
                    if (_common.IsYoJi)
                    {
                        itemCd = ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniKyujitu;
                    }
                    else
                    {
                        itemCd = ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaKyujitu;
                    }
                }
                else if (_common.jikan == JikanConst.SinyaKotoku)
                {
                    //特例深夜
                    if (_common.IsYoJi)
                    {
                        itemCd = ItemCdConst.IgakuNintiTiikiHoukatuNyuSyouniSinya;
                    }
                    else
                    {
                        itemCd = ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaSinya;
                    }
                    itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniSinya;
                }

                if (itemCd != "")
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                }
            }

            // 算定区分を割り当てる
            if (santeiKbn >= 0)
            {
                _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = santeiKbn;
                _common.Wrk.wrkSinKouis.Last().CdKbn = _common.GetCdKbn(santeiKbn, "B");
            }

            // 保険組み合わせ番号を割り当てる
            if (hokenPId >= 0)
            {
                _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
            }
        }

        /// <summary>
        /// 地域包括診療料
        /// 当該項目はオーダーから削除する
        /// </summary>
        private void TiikiHoukatu()
        {
            // 算定条件
            bool santeiJyoken = true;
            bool santeiJikanJyoken = false;

            int hokenPId = -1;
            int hokenId = -1;
            int santeiKbn = -1;

            string santeiJyokenMessage = "";

            //算定条件チェック
            if (new List<double> { SyosaiConst.Syosin, SyosaiConst.Syosin2, SyosaiConst.SyosinCorona, SyosaiConst.SyosinJouhou, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
            {
                // 初診時不可
                santeiJyoken = false;
                santeiJikanJyoken = false;

                santeiJyokenMessage = "初診時の";
            }
            else if (_common.ExistWrkOrSinKouiDetailByItemCd(
                new List<string> { ItemCdConst.Syosin, ItemCdConst.Syosin2, ItemCdConst.Syosin2Rousai, ItemCdConst.SyosinCorona,
                    ItemCdConst.SyosinJouhou, ItemCdConst.Syosin2Jouhou }, false))
            {
                // 初診時不可
                santeiJyoken = false;
                santeiJikanJyoken = false;

                santeiJyokenMessage = "初診算定日の";
            }
            else if (_common.Odr.ExistOdrDetailHoumonSinryo())
            {
                // 訪問診療時不可
                santeiJyoken = false;
                santeiJikanJyoken = false;

                santeiJyokenMessage = "訪問診療時の";
            }
            else if (_common.CheckSanteiTerm(TiikiHoukatuls, _common.SinFirstDateOfMonth, _common.sinDate))
            {
                // 算定回数
                santeiJyoken = false;
                santeiJyokenMessage = "上限（1回／1月）に達している";

                santeiJikanJyoken = true;
            }

            // Rpと行為を準備
            string cdKbn = "B";
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, 0);
            _common.Wrk.AppendNewWrkSinKoui(-1, -1, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            //if (santeiJyoken)
            //{
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuTiikiHoukatu1);

            //見つかったら、適切な項目を算定する
            if (minIndex >= 0)
            {
                if (santeiJyoken == false)
                {
                    _common.AppendCalcLog(2, $"'地域包括診療料' は、{santeiJyokenMessage}ため、算定できません。");
                }
                else
                {

                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuTiikiHoukatu1, odrDtls.First().SanteiKbn, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuTiikiHoukatu1, isDeleted: DeleteStatus.DeleteFlag);
                    }
                    else
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            if (odrDtl.ItemCd == ItemCdConst.IgakuTiikiHoukatu1)
                            {
                                // 医科外来等感染症対策実施加算
                                KansenTaisaku(true, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);

                                //外来感染対策向上加算
                                bool kojo = KansenKojo(true, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                //連携強化加算
                                RenkeiKyoka(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                //サーベランス強化加算
                                Surveillance(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                            }
                        }
                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                        santeiKbn = odrDtls[0].SanteiKbn;
                    }
                    santeiJikanJyoken = true;
                }
            }
            else
            {
                //見つからなかったら
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuTiikiHoukatu2);

                //見つかったら、適切な項目を算定する
                if (minIndex >= 0)
                {
                    if (santeiJyoken == false)
                    {
                        _common.AppendCalcLog(2, $"'地域包括診療料' は、{santeiJyokenMessage}ため、算定できません。");
                    }
                    else
                    {
                        if (_common.CheckSanteiKaisu(ItemCdConst.IgakuTiikiHoukatu2, odrDtls.First().SanteiKbn, 0) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuTiikiHoukatu2, isDeleted: DeleteStatus.DeleteFlag);
                        }
                        else
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                if (odrDtl.ItemCd == ItemCdConst.IgakuTiikiHoukatu2)
                                {
                                    // 医科外来等感染症対策実施加算
                                    KansenTaisaku(true, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);

                                    //外来感染対策向上加算
                                    bool kojo = KansenKojo(true, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                    //連携強化加算
                                    RenkeiKyoka(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                    //サーベランス強化加算
                                    Surveillance(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                }
                            }
                            hokenPId = odrDtls[0].HokenPid;
                            hokenId = odrDtls[0].HokenId;
                            santeiKbn = odrDtls[0].SanteiKbn;
                        }
                        santeiJikanJyoken = true;
                    }
                }
            }
            //}

            //オーダーから削除
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuls);
            while (minIndex >= 0)
            {
                _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuls);
            }

            if (santeiJyoken)
            {
                // 算定区分を割り当てる
                if (santeiKbn < 0)
                {
                    // 見つからない場合、加算がないかチェック
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuJikanls);

                    if (minIndex >= 0)
                    {
                        // あればその算定区分で算定
                        santeiKbn = odrDtls[0].SanteiKbn;
                    }
                    else
                    {
                        // 加算も見つからない場合
                        santeiKbn = _common.syosaiSanteiKbn;
                    }
                }
                _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = santeiKbn;
                cdKbn = _common.GetCdKbn(santeiKbn, "B");
                _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;

                // 保険組み合わせIDを割り当て
                if (hokenPId < 0)
                {
                    // 見つからない場合、加算がないかチェック
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuJikanls);

                    if (minIndex >= 0)
                    {
                        // あればその算定区分で算定
                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                        santeiKbn = odrDtls[0].SanteiKbn;
                    }
                    else
                    {
                        // 加算も見つからない場合
                        hokenPId = _common.syosaiPid;
                        hokenId = _common.syosaiHokenId;
                        santeiKbn = _common.syosaiSanteiKbn;
                    }
                }
                _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;
            }

            if (santeiJikanJyoken)
            {
                // 時間加算

                if (santeiJyoken == false)
                {
                    // 算定区分を割り当てる
                    if (santeiKbn < 0)
                    {
                        // 見つからない場合、加算がないかチェック
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuJikanls);

                        if (minIndex >= 0)
                        {
                            // あればその算定区分で算定
                            santeiKbn = odrDtls[0].SanteiKbn;
                            hokenPId = odrDtls[0].HokenPid;
                            hokenId = odrDtls[0].HokenId;
                        }
                        else
                        {
                            // 加算も見つからない場合
                            santeiKbn = _common.syosaiSanteiKbn;
                            hokenPId = _common.syosaiPid;
                            hokenId = _common.syosaiHokenId;
                        }
                    }
                    cdKbn = _common.GetCdKbn(santeiKbn, "B");
                }
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, santeiKbn);
                _common.Wrk.AppendNewWrkSinKoui(hokenPId, hokenId, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

                TiikiHoukatuJikan();
            }

        }

        /// <summary>
        /// 地域包括診療料の時間加算
        /// 手技料とは別に算定できる
        /// 当該項目はオーダーから削除する
        /// </summary>
        private void TiikiHoukatuJikan()
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            int hokenPId = -1;
            int hokenId = -1;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuJikanls);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                }

                hokenPId = odrDtls[0].HokenPid;
                hokenId = odrDtls[0].HokenId;

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuJikanls);
                }
            }
            else
            {
                string itemCd = "";

                // 自動算定
                if (_common.jikan == JikanConst.JikanGai)
                {
                    // 時間外
                    itemCd = ChoiceItemCdNyuNinpu(
                                ItemCdConst.IgakuTiikiHoukatuJikangai,
                                ItemCdConst.IgakuTiikiHoukatuNyuJikangai,
                                ItemCdConst.IgakuTiikiHoukatuNinpuJikangai);
                }
                else if (_common.jikan == JikanConst.Kyujitu)
                {
                    // 休日
                    itemCd = ChoiceItemCdNyuNinpu(
                                ItemCdConst.IgakuTiikiHoukatuKyujitu,
                                ItemCdConst.IgakuTiikiHoukatuNyuKyujitu,
                                ItemCdConst.IgakuTiikiHoukatuNinpuKyujitu);
                }
                else if (_common.jikan == JikanConst.Sinya)
                {
                    // 深夜
                    itemCd = ChoiceItemCdNyuNinpu(
                                ItemCdConst.IgakuTiikiHoukatuSinya,
                                ItemCdConst.IgakuTiikiHoukatuNyuSinya,
                                ItemCdConst.IgakuTiikiHoukatuNinpuSinya);
                }
                else if (_common.jikan == JikanConst.Yasou)
                {
                    // 夜間早朝

                    if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                    {
                        itemCd = ChoiceItemCdTime(
                                    ItemCdConst.IgakuTiikiHoukatuNyuSyouniYakan,
                                    ItemCdConst.IgakuTiikiHoukatuNyuSyouniKyujitu,
                                    ItemCdConst.IgakuTiikiHoukatuNyuSyouniSinya
                                    );
                    }
                    else if (_common.IsNinpu && (_common.Mst.GetHyoboSanka() == 1))
                    {
                        itemCd = ChoiceItemCdTime(
                                    ItemCdConst.IgakuTiikiHoukatuNinpuSankaYakan,
                                    ItemCdConst.IgakuTiikiHoukatuNinpuSankaKyujitu,
                                    ItemCdConst.IgakuTiikiHoukatuNinpuSankaSinya
                                    );
                    }
                    else
                    {
                        itemCd = ItemCdConst.IgakuTiikiHoukatuYasou;
                    }
                }
                else if (_common.jikan == JikanConst.YakanKotoku)
                {
                    //特例夜間
                    if (_common.IsYoJi)
                    {
                        itemCd = ItemCdConst.IgakuTiikiHoukatuNyuSyouniYakan;
                    }
                    else
                    {
                        itemCd = ItemCdConst.IgakuTiikiHoukatuNinpuSankaYakan;
                    }
                }
                else if (_common.jikan == JikanConst.KyujituKotoku)
                {
                    //特例休日
                    if (_common.IsYoJi)
                    {
                        itemCd = ItemCdConst.IgakuTiikiHoukatuNyuSyouniKyujitu;
                    }
                    else
                    {
                        itemCd = ItemCdConst.IgakuTiikiHoukatuNinpuSankaKyujitu;
                    }
                }
                else if (_common.jikan == JikanConst.SinyaKotoku)
                {
                    //特例深夜
                    if (_common.IsYoJi)
                    {
                        itemCd = ItemCdConst.IgakuTiikiHoukatuNyuSyouniSinya;
                    }
                    else
                    {
                        itemCd = ItemCdConst.IgakuTiikiHoukatuNinpuSankaSinya;
                    }
                    itemCd = ItemCdConst.IgakuSyouniGairaiSaisinSyouniSinya;
                }

                if (itemCd != "")
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                }
            }

            // 保険組み合わせIDを割り当てる_common.Wrk.CommitWrkSinRpInf
            if (hokenPId >= 0)
            {
                _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;
            }
        }

        /// <summary>
        /// 再診時療養指導料（労災）
        /// </summary>
        private void SaisinRyoyo()
        {
            List<string> saisinRyoyo =
                new List<string>
                {
                    ItemCdConst.IgakuSaisinRyoyo,
                    ItemCdConst.IgakuSaisinRyoyoCancel
                };

            // 手オーダー or キャンセル項目が存在するかチェックする
            List<OdrDtlTenModel> odrDtls = _common.Odr.FilterOdrDetailByItemCd(saisinRyoyo);

            //見つからなかった場合、自動発生
            if (odrDtls == null || odrDtls.Count <= 0)
            {
                if (_common.CheckSanteiKaisu(ItemCdConst.IgakuSaisinRyoyo, 0, 1) == 2)
                {
                    // 算定回数マスタのチェックにより算定不可
                }
                else
                {
                    // Rpと行為を準備
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, _common.syosaiSanteiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.EnSido, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "B"));

                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.IgakuSaisinRyoyo);
                }
            }
        }

        /// <summary>
        /// 医学管理の自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {
            const string conFncName = nameof(CalculateJihi);

            _common.CalculateJihi(
                OdrKouiKbnConst.Igaku,
                OdrKouiKbnConst.Igaku,
                ReceKouiKbn.Igaku,
                ReceSinId.Igaku,
                ReceSyukeisaki.Igaku,
                "JS");
        }

        /// <summary>
        /// 乳幼児かどうかで適切な項目のITEM_CDを返す
        /// </summary>
        /// <param name="defaultItemCd"></param>
        /// <param name="NyuItemCd"></param>
        /// <returns></returns>
        private string ChoiceItemCdNyu(string defaultItemCd, string NyuItemCd)
        {
            string itemCd = "";

            if (_common.IsYoJi)
            {
                //幼児
                itemCd = NyuItemCd;
            }
            else
            {
                itemCd = defaultItemCd;
            }

            return itemCd;
        }
        /// <summary>
        /// 乳幼児と妊婦の場合で適切な項目のITEM_CDを返す
        /// </summary>
        /// <param name="defaultItemCd">乳幼児でも妊婦でもない場合のITEM_CD</param>
        /// <param name="NyuItemCd">乳幼児の場合のITEM_CD</param>
        /// <param name="NinpuItemCd">妊婦の場合のITEM_CD</param>
        /// <returns>条件に合うITEM_CD</returns>
        private string ChoiceItemCdNyuNinpu(string defaultItemCd, string NyuItemCd, string NinpuItemCd)
        {
            string itemCd = "";

            if (_common.IsYoJi)
            {
                //幼児
                itemCd = NyuItemCd;
            }
            else if (_common.IsNinpu)
            {
                //妊婦
                itemCd = NinpuItemCd;
            }
            else
            {
                itemCd = defaultItemCd;
            }

            return itemCd;
        }

        /// <summary>
        /// 時間帯で適切な項目のITEM_CDを返す
        /// </summary>
        /// <param name="DefaultItemCd">初期値</param>
        /// <param name="KyujituItemCd">休日の場合のITEM_CD</param>
        /// <param name="SinyaItemCd">受付時間が深夜時間帯の場合のITEM_CD</param>
        /// <returns>条件に合うITEM_CD</returns>
        private string ChoiceItemCdTime(string DefaultItemCd, string KyujituItemCd, string SinyaItemCd)
        {
            string itemCd = "";

            if (_common.IsHolidaySinDate)
            {
                //休日
                itemCd = KyujituItemCd;
            }
            else if (_common.IsSinyaTime)
            {
                //深夜時間帯　22:00～6:00
                itemCd = SinyaItemCd;
            }
            else
            {
                //その他
                itemCd = DefaultItemCd;
            }

            return itemCd;
        }
        /// <summary>
        /// 医科外来等感染症対策実施加算（医学管理等）を算定可能かチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckKansenTaisaku(ref List<OdrDtlTenModel> filteredOdrDtl)
        {
            bool ret = false;
            string itemName = "";

            if (_common.sinDate >= 20210401 && _common.sinDate <= 20210930)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.IgakuKansenTaisaku)
                    {
                        itemName = filteredOdrDtl[i].ItemName;

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
                    if (filteredOdrDtl.Any(p => p.IsKansenTaisakuTargetIgaku) == false)
                    {
                        ret = false;
                        if (string.IsNullOrEmpty(itemName))
                        {
                            itemName = "医科外来等感染症対策実施加算（医学管理等）";
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
                    SyosaiConst.SyosinJouhou, SyosaiConst.Syosin2Jouhou, SyosaiConst.SaisinJouhou, SyosaiConst.Saisin2Jouhou }.Contains((int)_common.syosai))
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
        /// 外来感染対策向上加算（医学管理）を算定可能かチェック
        /// </summary>
        /// <returns>true: 外来感染対策向上加算（医学管理）を算定可能</returns>
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
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.IgakuKansenKojo)
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

                if (filteredOdrDtl.Any(p => p.IsKansenKojoTargetIgaku == true) == false)
                {

                    if (string.IsNullOrEmpty(itemName))
                    {
                        itemName = "外来感染対策向上加算（医学管理）";
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
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuKansenKojo, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }

            }

            return santei;

        }

        /// <summary>
        /// 連携強化加算（医学管理）を算定可能かチェック
        /// </summary>
        /// <param name="filteredOdrDtl">チェック対象RPのオーダー情報</param>
        /// <param name="kansenKojo">true: 外来感染対策向上加算（医学管理）を算定可能</param>
        /// <returns>true: 連携強化加算（医学管理）を算定可能</returns>
        private bool CheckRenkeiKyoka(List<OdrDtlTenModel> filteredOdrDtl, bool kansenKojo)
        {
            bool santei = false;

            if (_common.sinDate >= 20220401 && kansenKojo)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.IgakuRenkeiKyoka)
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
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuRenkeiKyoka, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;
        }

        /// <summary>
        /// サーベランス強化加算（医学管理）を算定可能かチェック
        /// </summary>
        /// <param name="filteredOdrDtl">チェック対象RPのオーダー情報</param>
        /// <param name="kansenKojo">true: 外来感染対策向上加算（医学管理）を算定可能</param>
        /// <returns>true: サーベランス強化加算（医学管理）を算定可能</returns>
        private bool CheckSurveillance(List<OdrDtlTenModel> filteredOdrDtl, bool kansenKojo)
        {
            bool santei = false;

            if (_common.sinDate >= 20220401 && kansenKojo)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.IgakuSurveillance)
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
                    if (_common.CheckSanteiKaisu(ItemCdConst.IgakuSurveillance, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;
        }

        /// <summary>
        /// 外来腫瘍化学療法診療料算定処理
        /// 当該項目はオーダーから削除する
        /// </summary>
        private bool GairaiSyuyo()
        {
            // 手技料を算定したか？
            bool santeiFlg = false;

            if (_common.sinDate >= 20220401)
            {
                // 算定条件
                bool santeiJyoken = true;
                bool santeiKasanJyoken = false;

                int hokenPId = -1;
                int hokenId = -1;
                int santeiKbn = -1;

                //算定条件チェック 
                if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou, SyosaiConst.Saisin, SyosaiConst.SaisinJouhou }.Contains(_common.syosai) == false)
                {
                    // 当日、初診・再診のオーダーがない場合は算定不可 ※明記されていないが、小児かかりつけ診療料と合わせる
                    santeiJyoken = false;
                }

                // Rpと行為を準備
                string cdKbn = "B";
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, 0);
                _common.Wrk.AppendNewWrkSinKoui(-1, -1, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiSyuyols);

                //見つかったら、適切な項目を算定する
                if (minIndex >= 0)
                {
                    if (santeiJyoken)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtls.First().SanteiKbn, 0) == 2)
                            {
                                // 算定回数マスタのチェックにより算定不可
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl.ItemCd, isDeleted: DeleteStatus.DeleteFlag);
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                if (GairaiSyuyols.Contains(odrDtl.ItemCd))
                                {
                                    //外来感染対策向上加算
                                    bool kojo = KansenKojo(true, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                    //連携強化加算
                                    RenkeiKyoka(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                    //サーベランス強化加算
                                    Surveillance(kojo, odrDtls[0].HokenPid, odrDtls[0].HokenId, odrDtls[0].SanteiKbn);
                                }

                                // 年齢加算自動算定
                                _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, odrDtls);

                                // コメント自動追加
                                _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, odrDtls);
                            }
                        }
                        santeiFlg = true;
                        santeiKasanJyoken = true;

                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                        santeiKbn = odrDtls[0].SanteiKbn;

                        _common.Wrk.wrkSinRpInfs.Last().SanteiKbn = santeiKbn;
                        _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                        _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;

                        cdKbn = _common.GetCdKbn(odrDtls[0].SanteiKbn, "B");
                        _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;
                    }
                }

                //オーダーから削除
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiSyuyols);
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiSyuyols);
                }

                //まずは加算項目（告示等識別区分１=7）を同じRpに算定
                GairaiSyuyoKasans(santeiKasanJyoken);

                // 次に基本項目をRpを分けて算定

                // 時間加算又は乳幼児加算
                // 算定区分を割り当てる
                if (santeiKbn < 0)
                {
                    // 見つからない場合、加算がないかチェック
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiSyuyoJikanOrNyuls);

                    if (minIndex >= 0)
                    {
                        // あればその算定区分で算定
                        santeiKbn = odrDtls[0].SanteiKbn;
                        hokenPId = odrDtls[0].HokenPid;
                        hokenId = odrDtls[0].HokenId;
                    }
                    else
                    {
                        // 加算も見つからない場合
                        santeiKbn = _common.syosaiSanteiKbn;
                        hokenPId = _common.syosaiPid;
                        hokenId = _common.syosaiHokenId;
                    }
                }
                cdKbn = _common.GetCdKbn(santeiKbn, "B");
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Igaku, ReceSinId.Igaku, santeiKbn);
                _common.Wrk.AppendNewWrkSinKoui(hokenPId, hokenId, ReceSyukeisaki.Igaku, cdKbn: cdKbn);

                GairaiSyuyoJikanOrNyu(santeiKasanJyoken);

            }
            return santeiFlg;
        }

        /// <summary>
        /// 外来腫瘍化学療法診療料の加算項目（告示等識別区分１=7）算定処理
        /// 当該項目はオーダーから削除する
        /// 手技料の算定がない場合、オーダーからの削除のみ行う
        /// ※外来腫瘍化学療法診療料と同じRpに算定する必要がある
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void GairaiSyuyoKasans(bool santei)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            foreach (string item in GairaiSyuyoKasanls)
            {
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(item);

                if (minIndex >= 0)
                {
                    //見つかった
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if (santei)
                        {
                            // 算定回数チェック
                            if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                            {
                                // 算定回数マスタのチェックにより算定不可
                            }
                            // 年齢チェック
                            else if (_common.CheckAge(odrDtl) == 2)
                            {
                                // 年齢チェックにより算定不可
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.IgakuSyouniKakaritukeKokin);
                        }

                    }

                }
            }

        }

        /// <summary>
        /// 外来腫瘍化学療法診療料の時間加算及び乳幼児加算
        /// 手技料とは別に算定できる
        /// 当該項目はオーダーから削除する
        /// </summary>
        /// <param name="santei">
        /// 手技料算定有無
        ///     falseの場合、オーダー削除のみ
        /// </param>
        private void GairaiSyuyoJikanOrNyu(bool santei)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            int hokenPId = -1;
            int hokenId = -1;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiSyuyoJikanOrNyuls);

            if (minIndex >= 0)
            {
                //見つかったら、そのまま算定する
                if (santei)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }

                    hokenPId = odrDtls[0].HokenPid;
                    hokenId = odrDtls[0].HokenId;
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiSyuyoJikanOrNyuls);
                }
            }
            else if (santei)
            {
                string itemCd = "";
                // 自動算定

                if (new double[] { SyosaiConst.Syosin, SyosaiConst.SyosinJouhou, SyosaiConst.SyosinCorona }.Contains(_common.syosai))
                {
                    //初診
                    if (_common.IsYoJi)
                    {
                        //乳幼児加算
                        itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSyosin;
                    }

                    if (_common.jikan == JikanConst.JikanGai)
                    {
                        // 時間外
                        itemCd = ChoiceItemCdNyu(
                                    ItemCdConst.IgakuGairaiSyuyoJikangaiSyosin,
                                    ItemCdConst.IgakuGairaiSyuyoNyuJikangaiSyosin);
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        // 休日
                        itemCd = ChoiceItemCdNyu(
                                    ItemCdConst.IgakuGairaiSyuyoKyujituSyosin,
                                    ItemCdConst.IgakuGairaiSyuyoNyuKyujituSyosin);
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        // 深夜
                        itemCd = ChoiceItemCdNyu(
                                    ItemCdConst.IgakuGairaiSyuyoSinyaSyosin,
                                    ItemCdConst.IgakuGairaiSyuyoNyuSinyaSyosin);
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        // 夜間早朝
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ChoiceItemCdTime(
                                        ItemCdConst.IgakuGairaiSyuyoNyuSyouniYakanSyosin,
                                        ItemCdConst.IgakuGairaiSyuyoNyuSyouniKyujituSyosin,
                                        ItemCdConst.IgakuGairaiSyuyoNyuSyouniSinyaSyosin
                                        );
                        }
                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        //特例夜間
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSyouniYakanSyosin;
                        }
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        //特例休日
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSyouniKyujituSyosin;
                        }
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        //特例深夜
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSyouniSinyaSyosin;
                        }
                    }

                }
                else
                {
                    //再診
                    if (_common.IsYoJi)
                    {
                        //乳幼児加算
                        itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSaisin;
                    }

                    if (_common.jikan == JikanConst.JikanGai)
                    {
                        // 時間外
                        itemCd = ChoiceItemCdNyu(
                                    ItemCdConst.IgakuGairaiSyuyoJikangaiSaisin,
                                    ItemCdConst.IgakuGairaiSyuyoNyuJikangaiSaisin);
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        // 休日
                        itemCd = ChoiceItemCdNyu(
                                    ItemCdConst.IgakuGairaiSyuyoKyujituSaisin,
                                    ItemCdConst.IgakuGairaiSyuyoNyuKyujituSaisin);
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        // 深夜
                        itemCd = ChoiceItemCdNyu(
                                    ItemCdConst.IgakuGairaiSyuyoSinyaSaisin,
                                    ItemCdConst.IgakuGairaiSyuyoNyuSinyaSaisin);
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        // 夜間早朝
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ChoiceItemCdTime(
                                        ItemCdConst.IgakuGairaiSyuyoNyuSyouniYakanSaisin,
                                        ItemCdConst.IgakuGairaiSyuyoNyuSyouniKyujituSaisin,
                                        ItemCdConst.IgakuGairaiSyuyoNyuSyouniSinyaSaisin
                                        );
                        }
                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        //特例夜間
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSyouniYakanSaisin;
                        }
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        //特例休日
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSyouniKyujituSaisin;
                        }
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        //特例深夜
                        if (_common.IsYoJi && (_common.Mst.GetHyoboSyounika() == 1))
                        {
                            itemCd = ItemCdConst.IgakuGairaiSyuyoNyuSyouniSinyaSaisin;
                        }
                    }
                    
                }

                if (itemCd != "")
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                }
            }

            // 保険組み合わせIDを割り当てる_common.Wrk.CommitWrkSinRpInf
            if (hokenPId >= 0)
            {
                _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPId;
                _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;
            }
        }
    }
}
