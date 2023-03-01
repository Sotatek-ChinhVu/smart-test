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
    class IkaCalculateOdrToWrkSyujyutuViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkSyujyutuViewModel(IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider,IEmrLogger emrLogger)
        {
            _common = common;

            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 計算ロジック
        /// </summary>
        public void Calculate()
        {
            const string conFncName = nameof(Calculate);
            _emrLogger.WriteLogStart( this, conFncName, "");

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.SyujyutuMin, OdrKouiKbnConst.SyujyutuMax))
            {
                // 保険
                CalculateHoken();

                // 自費
                CalculateJihi();
            }

            _common.Wrk.CommitWrkSinRpInf();

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 保険分を処理する
        /// </summary>
        private void CalculateHoken()
        {
            const string conFncName = nameof(CalculateHoken);

            // 通常算定処理
            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    // 手術・輸血のRpを取得
                    filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.SyujyutuMin, OdrKouiKbnConst.Yuketu);
                }
                else
                {
                    // 麻酔
                    filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.Masui, OdrKouiKbnConst.Masui);
                }

                if (filteredOdrInf.Any())
                {
                    foreach (OdrInfModel odrInf in filteredOdrInf)
                    {

                        // 行為に紐づく詳細を取得
                        filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                        if (filteredOdrDtl.Any())
                        {
                            string cdKbn = "K";

                            // 初回、必ずRpと行為のレコードを用意
                            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syujyutu, ReceSinId.Syujyutu, odrInf.SanteiKbn);

                            // 集計先は、後で内容により変更する
                            cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.OpeMasui, cdKbn: cdKbn);

                            bool checkAgeKasan = false;
                            bool checkJikanKasan = false;

                            int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                            // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                            //bool commentSkipFlg = (firstItem != 0);
                            // 手術のコメントは手技につける
                            bool commentSkipFlg = false;
                            // 最初のコメント以外の項目であることを示すフラグ
                            bool firstSinryoKoui = true;

                            // 労災四肢加算が手オーダーされているRpかどうかチェック
                            bool existsRosaiSisiKasan =
                                ( _common.IsRosai && 
                                  filteredOdrDtl.Any(p => 
                                    p.ItemCd == ItemCdConst.SyujyutuRosaiSisiKasan || 
                                    p.ItemCd == ItemCdConst.SyujyutuRosaiSisiKasan2)
                                );

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || _common.IsSelectComment(odrDtl.ItemCd)))
                                {
                                    // 診療行為・コメント

                                    commentSkipFlg = false;

                                    if (odrDtl.IsKihonKoumoku && existsRosaiSisiKasan == false)
                                    {
                                        // 基本項目

                                        if (firstSinryoKoui == true)
                                        {
                                            // もう一カ所使用するので、falseにするのは最後にする
                                            //firstSinryoKoui = false;
                                        }
                                        else
                                        {
                                            //最初以外の基本項目が来たらRpを分ける　
                                            //※最初にコメントが入っていると困るのでこういう処理にする

                                            //※手術は同一Rpにオーダーされていれば、一連の行為とみなすようにしておく
                                            //if (odrDtl.Kokuji2 != "7")
                                            //{
                                            //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syujyutu, ReceSinId.Syujyutu, odrInf.SanteiKbn);
                                            //}
                                            //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.OpeMasui);

                                            if (checkJikanKasan)
                                            {
                                                JikanKasan(i);
                                            }

                                            cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.OpeMasui, cdKbn: cdKbn);
                                        }
                                    }
                                    //firstSinryoKoui = false;

                                    if (!(odrDtl.IsComment))
                                    {
                                        // コメント項目以外

                                        // 算定回数チェック
                                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
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

                                            // 労災加算チェック
                                            if (_common.IsRosai)
                                            {
                                                if ((filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.SyujyutuRosaiSisiKasan || p.ItemCd == ItemCdConst.SyujyutuRosaiSisiKasan2) == false) &&
                                                        (_common.Wrk.wrkSinKouiDetails.Any(p => 
                                                        p.RaiinNo == _common.Wrk.RaiinNo && 
                                                        p.HokenKbn == _common.Wrk.HokenKbn && 
                                                        p.RpNo == _common.Wrk.RpNo &&
                                                        p.SeqNo == _common.Wrk.SeqNo &&
                                                        (p.ItemCd == ItemCdConst.SyujyutuRosaiSisiKasan || p.ItemCd == ItemCdConst.SyujyutuRosaiSisiKasan2) &&
                                                        p.IsDeleted == DeleteStatus.None)
                                                        == false))
                                                {
                                                    // 四肢加算項目のオーダーはない
                                                    if (filteredOdrDtl.Any(p => p.BuiKbn == 10))
                                                    {
                                                        if (odrDtl.SisiKbn == 1 || odrDtl.SisiKbn == 3)
                                                        {
                                                            // 四肢が存在する場合、２倍を自動算定
                                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyujyutuRosaiSisiKasan2, autoAdd: 1);
                                                        }
                                                        else if (odrDtl.SisiKbn == 2)
                                                        {
                                                            // 四肢が存在する場合、１．５倍を自動算定
                                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyujyutuRosaiSisiKasan, autoAdd: 1);
                                                        }
                                                    }
                                                    else if (filteredOdrDtl.Any(p => p.BuiKbn == 3))
                                                    {
                                                        if (odrDtl.SisiKbn == 1 || odrDtl.SisiKbn == 2)
                                                        {
                                                            // 四肢が存在する場合、１．５倍を自動算定
                                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyujyutuRosaiSisiKasan, autoAdd: 1);
                                                        }
                                                    }
                                                }
                                            }

                                            // 年齢加算自動算定
                                            if (_common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl) == false)
                                            {
                                                if (odrDtl.IsKihonKoumoku && odrDtl.SanteiItemCd != ItemCdConst.NoSantei)
                                                {
                                                    //AgeKasan(i, filteredOdrDtl);
                                                    checkAgeKasan = true;
                                                }
                                            }

                                            // 時間加算算定
                                            if (odrDtl.IsKihonKoumoku &&
                                                !(odrDtl.CdKbn == "K" && odrDtl.CdKbnno == 914) &&
                                                !(odrDtl.CdKbn == "K" && odrDtl.CdKbnno == 915) &&
                                                odrDtl.TimeKasanKbn > 0 &&
                                                new string[] { "1", "3", "5" }.Contains(odrDtl.Kokuji2))
                                            {
                                                //JikanKasan(i);
                                                checkJikanKasan = true;
                                            }

                                            // コメント自動追加
                                            _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);

                                            if (odrDtl.CdKbn != "")
                                            {
                                                if (odrDtl.SanteiKbn == SanteiKbnConst.Jihi)
                                                {
                                                    cdKbn = "JS";
                                                }
                                                else
                                                {
                                                    cdKbn = odrDtl.CdKbn;
                                                    if (cdKbn == "-")
                                                    {
                                                        if (odrDtl.SinKouiKbn == 50)
                                                        {
                                                            cdKbn = "K";
                                                        }
                                                        else
                                                        {
                                                            cdKbn = "L";
                                                        }
                                                    }
                                                }

                                                _common.Wrk.wrkSinKouis.Last().CdKbn = cdKbn;

                                            }

                                            //// 診療区分の設定
                                            //if(odrDtl.TenMst != null && odrDtl.TenMst.SinKouiKbn == OdrKouiKbnConst.Masui)
                                            //{
                                            //    // 初期値はOdrKouiKbnConst.Syujyutu(50)なので、麻酔の場合だけ、麻酔(OdrKouiKbnConst.Masui(54))に設定する
                                            //    _common.Wrk.wrkSinRpInfs.Last().SinId = ReceSinId.Masui;
                                            //}
                                        }

                                        // 診療区分の設定（オーダーが算定回数上限等で算定できなかったとしても、後の薬剤・特材の行為をこの項目の診区に変える必要があるため）
                                        if (firstSinryoKoui && odrDtl.TenMst != null && odrDtl.TenMst.SinKouiKbn == OdrKouiKbnConst.Masui)
                                        {
                                            // 初期値はOdrKouiKbnConst.Syujyutu(50)なので、麻酔の場合だけ、麻酔(OdrKouiKbnConst.Masui(54))に設定する
                                            _common.Wrk.wrkSinRpInfs.Last().SinId = ReceSinId.Masui;
                                        }
                                    }
                                    else
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    }

                                    if (odrDtl.IsKihonKoumoku && existsRosaiSisiKasan == false)
                                    {
                                        // 基本項目

                                        if (firstSinryoKoui == true)
                                        {
                                            firstSinryoKoui = false;
                                        }
                                    }
                                }
                                else
                                {
                                    // 手術のコメントは手技につける
                                    //commentSkipFlg = true;
                                }

                            }

                            if (checkAgeKasan)
                            {
                                if (i == 0)
                                {
                                    SyujyutuAgeKasan(filteredOdrDtl);
                                }
                                else
                                {
                                    MasuiAgeKasan(filteredOdrDtl);
                                }
                            }

                            if (checkJikanKasan)
                            {
                                JikanKasan(i);
                            }

                            // 薬剤・コメント算定

                            commentSkipFlg = false;

                            _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.OpeYakuzai, cdKbn, ref firstSinryoKoui);

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                //if (odrDtl.IsYorCommentItem(commentSkipFlg))
                                if (odrDtl.IsYItem)
                                {
                                    // 薬剤・コメント
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    //commentSkipFlg = false;
                                }
                                //else if (_common.IsSelectComment(odrDtl.ItemCd))
                                //{
                                //    // 選択式コメントは手技で対応しているので読み飛ばす
                                //}
                                //else
                                //{
                                //    commentSkipFlg = true;
                                //}
                            }

                            // 特材・コメント算定

                            commentSkipFlg = false;

                            _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.OpeYakuzai, cdKbn, ref firstSinryoKoui);

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                //if (odrDtl.IsTorCommentItem(commentSkipFlg))
                                if (odrDtl.IsTItem)
                                {
                                    // 特材・コメント
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    if (odrDtl.IsSanso)
                                    {
                                        // 酸素補正率
                                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SansoHoseiRitu, autoAdd: 1);
                                    }
                                    //commentSkipFlg = false;
                                }
                                //else if (_common.IsSelectComment(odrDtl.ItemCd))
                                //{
                                //    // 選択式コメントは手技で対応しているので読み飛ばす
                                //}
                                //else
                                //{
                                //    commentSkipFlg = true;
                                //}
                            }
                        }
                    }

                    _common.Wrk.CommitWrkSinRpInf();
                }
            }
        }

        /// <summary>
        /// 時間外加算
        /// </summary>
        /// <param name="mode">
        ///     0-手術・輸血
        ///     1-麻酔
        /// </param>
        private void JikanKasan(int mode)
        {
            string[] jikangails = 
                new string[] { ItemCdConst.SyujyutuJikangai, ItemCdConst.MasuiJikangai };
            string[] kyujituls =
                new string[] { ItemCdConst.SyujyutuKyujitu, ItemCdConst.MasuiKyujitu };
            string[] sinyals =
                new string[] { ItemCdConst.SyujyutuSinya, ItemCdConst.MasuiSinya };


            if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.SyujyutuTimeKasanCancel) == false)
            {
                if (_common.jikan == JikanConst.JikanGai)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(jikangails[mode], autoAdd: 1);
                }
                else if (_common.jikan == JikanConst.Kyujitu)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(kyujituls[mode], autoAdd: 1);
                }
                else if (_common.jikan == JikanConst.Sinya)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(sinyals[mode], autoAdd: 1);
                }
            }
        }

        /// <summary>
        /// 年齢加算（手術・輸血）
        /// </summary>
        /// <param name="odrDtls"></param>
        private void SyujyutuAgeKasan(List<OdrDtlTenModel> odrDtls)
        {
            string itemCd = "";

            if (odrDtls.Any(p => p.ItemCd == ItemCdConst.SyujyutuMijyuku))
            {
                // 未熟児加算がある場合、年齢加算自動算定しない
            }
            else
            {
                if (_common.IsSinseiJi && odrDtls.Any(p => p.LowWeightKbn == 1))
                {
                    itemCd = ItemCdConst.SyujyutuSinseiji;
                }
                else if (_common.IsNyuyoJi)
                {
                    itemCd = ItemCdConst.SyujyutuNyuyoji;
                }
                else if (_common.IsYoJi)
                {
                    itemCd = ItemCdConst.SyujyutuYoji;
                }

                if (itemCd != "")
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
                }
            }
        }

        /// <summary>
        /// 年齢加算（麻酔）
        /// </summary>
        /// <param name="odrDtls"></param>
        private void MasuiAgeKasan(List<OdrDtlTenModel> odrDtls)
        {

            string itemCd = "";

            if (odrDtls.Any(p => p.ItemCd == ItemCdConst.MasuiMijyuku))
            {
                // 未熟児加算がある場合、年齢加算自動算定しない
            }
            else
            {
                if (_common.IsSinseiJi)
                {
                    itemCd = ItemCdConst.MasuiSinseiji;
                }
                else if (_common.IsNyuJi)
                {
                    itemCd = ItemCdConst.MasuiNyuji;
                }
                else if (_common.IsNyuyoJi)
                {
                    itemCd = ItemCdConst.MasuiYoji;
                }

                if (itemCd != "")
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
                }
            }
        }

        /// <summary>
        /// 自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {
            const string conFncName = nameof(CalculateJihi);

            _common.CalculateJihi(
                OdrKouiKbnConst.SyujyutuMin,
                OdrKouiKbnConst.SyujyutuMax,
                ReceKouiKbn.Syujyutu,
                ReceSinId.Syujyutu,
                ReceSyukeisaki.OpeMasui,
                "JS");
        }
    }
}
