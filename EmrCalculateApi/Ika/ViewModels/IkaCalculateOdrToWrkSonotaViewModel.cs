using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    /// <summary>
    /// オーダー情報からワーク情報へ変換
    /// その他
    /// </summary>
    class IkaCalculateOdrToWrkSonotaViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 目標設定等支援・管理料
        /// </summary>
        List<string> Mokuhyols =
            new List<string>
            {
                ItemCdConst.SonotaMokuhyo1,
                ItemCdConst.SonotaMokuhyo2
            };

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkSonotaViewModel(IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
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

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.SonotaMin, OdrKouiKbnConst.SonotaMax))
            {
                // 保険
                CalculateHoken();

                // 自費
                CalculateJihi();
            }

            // 労災電子化加算
            RosaiDensika();

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

            // その他のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.SonotaMin, OdrKouiKbnConst.SonotaMax);

            if (filteredOdrInf.Any())
            {
                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any())
                    {
                        // 医科外来等感染症対策実施加算（精神科訪問看護・指導料）チェック
                        bool kansenTaisaku = CheckKansenTaisaku(filteredOdrDtl);
                        bool existKansenTaisakuTarget = false;
                        // 外来感染対策向上加算（精神科訪問看護・指導料）チェック
                        bool kansenKojo = CheckKansenKojo(filteredOdrDtl);
                        bool existKansenKojoTarget = false;
                        // 連携強化加算（精神科訪問看護・指導料）チェック
                        bool renkeiKyoka = CheckRenkeiKyoka(filteredOdrDtl, kansenKojo);
                        // サーベランス強化加算（精神科訪問看護・指導料）チェック
                        bool surveillance = CheckSurveillance(filteredOdrDtl, kansenKojo);

                        string cdKbn = "H";
                        string AddJissiNissuItemCd = "";

                        string syukeisaki = ReceSyukeisaki.SonotaSonota;
                        if (new int[] { 1, 2 }.Contains(_common.hokenKbn))
                        {
                            syukeisaki = ReceSyukeisaki.Sonota;
                        }

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, odrInf.SanteiKbn);

                        // 集計先は、後で内容により変更する
                        cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn));

                        int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        bool commentSkipFlg = (firstItem != 0);
                        // 最初のコメント以外の項目であることを示すフラグ
                        bool firstSinryoKoui = true;
                        bool commentOnly = true;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || _common.IsSelectComment(odrDtl.ItemCd)))
                            {
                                // 診療行為・コメント

                                if (odrDtl.IsComment == false)
                                {
                                    commentSkipFlg = false;
                                }

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
                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaKansenTaisaku, autoAdd: 1);
                                            existKansenTaisakuTarget = false;
                                        }
                                        // 外来感染対策向上加算等算定
                                        if (kansenKojo && existKansenKojoTarget)
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaKansenKojo, autoAdd: 1);
                                            if(renkeiKyoka)
                                            {
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaRenkeiKyoka, autoAdd: 1);
                                            }
                                            if (surveillance)
                                            {
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaSurveillance, autoAdd: 1);
                                            }
                                            existKansenKojoTarget = false;
                                        }

                                        //最初以外の基本項目が来たらRpを分ける　
                                        //※最初にコメントが入っていると困るのでこういう処理にする
                                        if (odrDtl.Kokuji2 != "7")
                                        {
                                            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, 0);
                                        }
                                        //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Sonota);
                                        cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn));
                                    }
                                }

                                //firstSinryoKoui = false;

                                if (!(_common.IsCommentItemCd(odrDtl.ItemCd)))
                                {
                                    commentOnly = false;

                                    // コメント項目以外

                                    //if (odrDtl.TyuCd == "8043" && odrDtl.Kokuji1 == "1")
                                    //{
                                    //    AddJissiNissuItemCd = odrDtl.ItemCd;
                                    //}
                                    if (odrDtl.IsRihabiri)
                                    {
                                        // リハビリ項目の場合、実施日数
                                        AddJissiNissuItemCd = odrDtl.ItemCd;
                                    }

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
                                    //else if(Mokuhyols.Contains(odrDtl.ItemCd) && CheckMokuhyoSanteiKaisu())
                                    //{
                                    //    // 目標設定等支援・管理料の算定回数上限超
                                    //}
                                    else if (odrDtl.ItemCd == ItemCdConst.SonotaSeisinOnline && CheckSeisinOnlineSanteiKaisu())
                                    {
                                        // 精神科オンライン在宅管理料の算定回数上限超
                                    }
                                    else
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                        if (odrDtl.SyukeiSaki == "ZZ0")
                                        {
                                            // 診断書料
                                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SindanSyo;
                                        }
                                        else if (odrDtl.SyukeiSaki == "ZZ1")
                                        {
                                            // 明細書料
                                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.MeisaiSyo;
                                        }
                                        else if (odrDtl.SyukeiSaki == "A18")
                                        {
                                            // 円その他
                                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSonota;
                                        }
                                        else if (odrDtl.SyukeiSaki == "A13")
                                        {
                                            // 医学管理（労災特掲）
                                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSido;
                                        }
                                        else if (odrDtl.ItemCd == ItemCdConst.KyuKyuIryoKanriKasan)
                                        {
                                            // 救急医療管理加算
                                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnKyukyu;
                                        }
                                        else if (_common.IsRosai && _common.hokenKbn != HokenSyu.After &&
                                            (odrDtl.IsEnKoumoku || odrDtl.IsRosaiEnKoumoku || odrDtl.SyukeiSaki == "A18"))
                                        {
                                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSonota;
                                        }

                                        // 労災加算チェック
                                        if (_common.IsRosai)
                                        {
                                            if (filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.SonotaRosaiSisiKasan) == false)
                                            {
                                                // 四肢加算項目のオーダーはない

                                                if (filteredOdrDtl.Any(p => p.BuiKbn == 3))
                                                {
                                                    if (odrDtl.SisiKbn == 1 || odrDtl.SisiKbn == 2)
                                                    {
                                                        // 四肢が存在する場合、１．５倍を自動算定
                                                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaRosaiSisiKasan, autoAdd: 1);
                                                    }
                                                }
                                            }
                                        }

                                        // 年齢加算自動算定
                                        _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl);

                                        // 医科外来等感染症対策実施加算対応
                                        if (odrDtl.IsKansenTaisakuTargetSeisin)
                                        {
                                            if (kansenTaisaku)
                                            {
                                                existKansenTaisakuTarget = true;
                                                //_common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaKansenTaisaku, autoAdd: 1);
                                            }
                                        }

                                        // 外来感染対策向上加算の加算対象フラグ
                                        if (odrDtl.IsKansenKojoTargetSeisin)
                                        {
                                            if (kansenKojo)
                                            {
                                                existKansenKojoTarget = true;
                                            }
                                        }

                                        // コメント自動追加
                                        _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);

                                        if (odrDtl.ItemCd.StartsWith("S") &&
                                            (odrDtl.SyukeiSaki == "ZZ0" || odrDtl.SyukeiSaki == "ZZ1" || odrDtl.SyukeiSaki == "A18"))
                                        {
                                            // 自賠文書料項目の場合
                                            _common.Wrk.wrkSinKouis.Last().CdKbn = "JB";
                                        }
                                        else if (odrDtl.CdKbn != "")
                                        {
                                            cdKbn = odrDtl.CdKbn;
                                            if (cdKbn == "-")
                                            {
                                                if (odrDtl.MasterSbt == "R" && odrDtl.SinKouiKbn == 80)
                                                {
                                                    cdKbn = "R";
                                                }
                                                else
                                                {
                                                    cdKbn = "I";
                                                }
                                            }
                                            _common.Wrk.wrkSinKouis.Last().CdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
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

                        if (commentOnly)
                        {
                            _common.Wrk.wrkSinKouis.Last().CdKbn = _common.GetCdKbn(odrInf.SanteiKbn, "SO");
                        }

                        // 医科外来等感染症対策実施加算対応
                        if (kansenTaisaku && existKansenTaisakuTarget)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaKansenTaisaku, autoAdd: 1);
                        }

                        // 外来感染対策向上加算等算定
                        if (kansenKojo && existKansenKojoTarget)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaKansenKojo, autoAdd: 1);
                            if (renkeiKyoka)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaRenkeiKyoka, autoAdd: 1);
                            }
                            if (surveillance)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaSurveillance, autoAdd: 1);
                            }
                        }

                        // リハビリ項目の場合、実施日数
                        if (AddJissiNissuItemCd != "")
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.CommentJissiNissuDummy, autoAdd: 1, cmtOpt: AddJissiNissuItemCd);
                        }

                        // 薬剤・コメント算定
                        commentSkipFlg = (firstItem != 1);

                        syukeisaki = ReceSyukeisaki.SonotaYakuzai;
                        if (_common.hokenKbn == HokenSyu.After || _common.IsJibaiRosai)
                        {
                            // アフターケア、自賠労災準拠の場合はその他
                            syukeisaki = ReceSyukeisaki.Sonota;
                        }

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, cdKbn, ref firstSinryoKoui);

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
                                commentSkipFlg = true;
                            }
                        }

                        // 特材・コメント算定
                        commentSkipFlg = (firstItem != 2);

                        syukeisaki = ReceSyukeisaki.SonotaYakuzai;
                        if (_common.hokenKbn == HokenSyu.After || _common.IsJibaiRosai)
                        {
                            // アフターケア、自賠労災準拠の場合はその他
                            syukeisaki = ReceSyukeisaki.Sonota;
                        }

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, cdKbn, ref firstSinryoKoui);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if (odrDtl.IsTorCommentItem(commentSkipFlg))
                            {
                                // 特材・コメント
                                if (_common.IsRosai && _common.hokenKbn != HokenSyu.After && (odrDtl.IsEnKoumoku || odrDtl.IsRosaiEnKoumoku))
                                {
                                    _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.EnSonota, cdKbn, ref firstSinryoKoui);
                                }

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
            }
        }

        /// <summary>
        /// 労災電子化加算
        /// </summary>
        private void RosaiDensika()
        {
            List<string> rosaiDensikals =
                new List<string>
                {
                    ItemCdConst.SonotaRosaiDensika,
                    ItemCdConst.SonotaRosaiDensikaCancel
                };

            if ((_common.hokenKbn == HokenSyu.Rosai &&
                    _systemConfigProvider.GetRousaiRecedenLicense() == 1 &&
                    CIUtil.StrToIntDef(_systemConfigProvider.GetRousaiRecedenStartYm(), 999999) <= (_common.sinDate / 100)) ||
                (_common.hokenKbn == HokenSyu.After &&
                    _systemConfigProvider.GetAfterCareRecedenLicense() == 1 &&
                    CIUtil.StrToIntDef(_systemConfigProvider.GetAfterCareRecedenStartYm(), 999999) <= (_common.sinDate / 100)))
            {
                // 労災である or アフターケアで電算開始日以降
                //if(_common.Odr.FilterOdrDetailByItemCd(rosaiDensikals).Any() == false)
                if (_common.Odr.ExistOdrDetailByItemCdToday(rosaiDensikals) == false)
                {
                    // 手オーダーなし
                    if (_common.Wrk.wrkSinRpInfs.Any(p => p.SinId != 96 && p.IsDeleted == DeleteStatus.None))
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SonotaRosaiDensika))
                        {
                            // 自動算定設定あり
                            int checkFrom = _common.SinFirstDateOfMonth;
                            int checkTo = _common.SinLastDateOfMonth;

                            if (_common.hokenKbn == HokenSyu.After)
                            {
                                // アフターケアの場合、1日1回
                                checkFrom = _common.sinDate;
                                checkTo = _common.sinDate;
                            }

                            //if (_common.CheckSanteiTerm
                            //    (rosaiDensikals, checkFrom, checkTo) == false &&
                            //    _common.ExistWrkOrSinKouiDetailByItemCd(rosaiDensikals, false) == false)
                            if (_common.CheckSanteiTerm
                                (rosaiDensikals, checkFrom, checkTo) == false)
                            {
                                // 当月に、労災電子化加算もキャンセル項目も算定されていない場合
                                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, _common.syosaiSanteiKbn);
                                _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.Sonota, cdKbn: "R");

                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SonotaRosaiDensika, autoAdd: 1);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 目標設定等支援・管理料の算定回数をチェック
        /// </summary>
        /// <returns>
        ///     true - 算定負荷（上限を超える）
        ///     false - 算定可能（上限に達してない）
        /// </returns>
        private bool CheckMokuhyoSanteiKaisu()
        {
            const string conCalcLog = "目標設定等支援・管理料は、上限（1回／3月）に達しているため、算定できません。";

            bool ret = false;
            double santeiKaisu = _common.SanteiCount(_common.MonthsBefore(_common.sinDate, 2), _common.sinDate, Mokuhyols);

            if (santeiKaisu > 0)
            {
                _common.AppendCalcLog(2, conCalcLog);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 精神科オンライン在宅管理料の算定回数をチェック
        /// </summary>
        /// <returns>
        ///     true - 算定負荷（上限を超える）
        ///     false - 算定可能（上限に達してない）
        /// </returns>
        private bool CheckSeisinOnlineSanteiKaisu()
        {
            const string conCalcLog = "精神科オンライン在宅管理料は、連続する２月に算定できません。";

            bool ret = false;
            double santeiKaisu = _common.SanteiCount(_common.MonthsBefore(_common.sinDate, 1), _common.sinDate, ItemCdConst.SonotaSeisinOnline);

            if (santeiKaisu > 0)
            {
                _common.AppendCalcLog(2, conCalcLog);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 医科外来等感染症対策実施加算（精神科訪問看護・指導料）を算定可能かチェック
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
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.SonotaKansenTaisaku)
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
                    if (filteredOdrDtl.Any(p => p.IsKansenTaisakuTargetSeisin == true) == false)
                    {
                        ret = false;
                        if (string.IsNullOrEmpty(itemName))
                        {
                            itemName = "医科外来等感染症対策実施加算（精神科訪問看護・指導料）";
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
        /// 外来感染対策向上加算（精神科訪問看護・指導料）を算定可能かチェック
        /// </summary>
        /// <returns>true: 外来感染対策向上加算（精神科訪問看護・指導料）を算定可能</returns>
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
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.SonotaKansenKojo)
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

                if (filteredOdrDtl.Any(p => p.IsKansenKojoTargetSeisin == true) == false)
                {

                    if (string.IsNullOrEmpty(itemName))
                    {
                        itemName = "外来感染対策向上加算（精神科訪問看護・指導料）";
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
                    if (_common.CheckSanteiKaisu(ItemCdConst.SonotaKansenKojo, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;

        }

        /// <summary>
        /// 連携強化加算（精神科訪問看護・指導料）を算定可能かチェック
        /// </summary>
        /// <param name="filteredOdrDtl">チェック対象RPのオーダー情報</param>
        /// <param name="kansenKojo">true: 外来感染対策向上加算（精神科訪問看護・指導料）を算定可能</param>
        /// <returns>true: 連携強化加算（精神科訪問看護・指導料）を算定可能</returns>
        private bool CheckRenkeiKyoka(List<OdrDtlTenModel> filteredOdrDtl, bool kansenKojo)
        {
            bool santei = false;

            if (_common.sinDate >= 20220401 && kansenKojo)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.SonotaRenkeiKyoka)
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
                    //メッセージがたくさん出てしまうので、算定できるときだけチェックする
                    if (_common.CheckSanteiKaisu(ItemCdConst.SonotaRenkeiKyoka, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;
        }

        /// <summary>
        /// サーベランス強化加算（精神科訪問看護・指導料）を算定可能かチェック
        /// </summary>
        /// <param name="filteredOdrDtl">チェック対象RPのオーダー情報</param>
        /// <param name="kansenKojo">true: 外来感染対策向上加算（精神科訪問看護・指導料）を算定可能</param>
        /// <returns>true: サーベランス強化加算（精神科訪問看護・指導料）を算定可能</returns>
        private bool CheckSurveillance(List<OdrDtlTenModel> filteredOdrDtl, bool kansenKojo)
        {
            bool santei = false;

            if (_common.sinDate >= 20220401 && kansenKojo)
            {
                // 手オーダー確認

                int i = 0;
                while (i < filteredOdrDtl.Count)
                {
                    if (filteredOdrDtl[i].ItemCd == ItemCdConst.SonotaSurveillance)
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
                    if (_common.CheckSanteiKaisu(ItemCdConst.SonotaSurveillance, 0, 0) == 2)
                    {
                        // 算定回数マスタのチェックにより算定不可
                        santei = false;
                    }
                }
            }

            return santei;
        }

        /// <summary>
        /// 自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {
            const string conFncName = nameof(CalculateJihi);

            _common.CalculateJihi(
                OdrKouiKbnConst.SonotaMin,
                OdrKouiKbnConst.SonotaMax,
                ReceKouiKbn.Sonota,
                ReceSinId.Sonota,
                ReceSyukeisaki.Sonota,
                "JS");
        }
    }
}
