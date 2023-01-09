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
using Helper.Common;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    class IkaCalculateOdrToWrkChusyaViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 点滴項目のリスト
        /// </summary>
        List<string> Tentekils =
            new List<string>
            {
                ItemCdConst.ChusyaTenteki,
                ItemCdConst.ChusyaTentekiNyu100,
                ItemCdConst.ChusyaTenteki500,
                //ItemCdConst.ChusyaHoumonTenteki
            };

        /// <summary>
        /// 手技なし項目のリスト
        /// </summary>
        List<string> SyugiNasils =
            new List<string>
            {
                ItemCdConst.ChusyaSyuginasi31,
                ItemCdConst.ChusyaSyuginasi32,
                ItemCdConst.ChusyaSyuginasi33
            };

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkChusyaViewModel(IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _common = common;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger= emrLogger;
        }

        /// <summary>
        /// 計算ロジック
        /// </summary>
        public void Calculate()
        {
            const string conFncName = nameof(Calculate);
            _emrLogger.WriteLogStart( this, conFncName, "");

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.ChusyaMin, OdrKouiKbnConst.ChusyaMax))
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

            // 注射のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.ChusyaMin, OdrKouiKbnConst.ChusyaMax);

            if (filteredOdrInf.Any())
            {
                // 点滴注射
                if (_common.Odr.ExistOdrDetailByItemCd(Tentekils))
                {
                    // 点滴手技のオーダーがある場合、専用計算処理へ
                    Tenteki();
                }

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    //if (filteredOdrDtl.Any(p => Tentekils.Contains(p.ItemCd) == true || p.ItemCd == ItemCdConst.ChusyaSyuginasi33) == false)
                    if(_common.Odr.ExistOdrDetailByItemCd(Tentekils) == false ||
                       (filteredOdrDtl.Any(p => Tentekils.Contains(p.ItemCd) == true || p.ItemCd == ItemCdConst.ChusyaSyuginasi33) == false))
                    {
                        // 点滴手技が当来院のオーダーに存在しない or
                        // (点滴手技 or 手技料なし33が存在しないRpの場合)
                        // ※点滴は、Tenteki()で算定済みのため、読み飛ばす
                        // ※手技料なし33は、点滴手技があればTenteki()で算定済みなので読み飛ばす
                        //   点滴手技がなければこちらで処理する（点滴以外の注射に続く場合があるため、オーダー順に処理したい）

                        if (odrInf.OdrKouiKbn == OdrKouiKbnConst.Jyoumyaku || odrInf.OdrKouiKbn == OdrKouiKbnConst.Hikakin)
                        {
                            if(filteredOdrDtl.Any(p=>p.DrugKbn == 4) == false)
                            {
                                string itemname = "注射手技";
                                
                                if (filteredOdrDtl.Any(p => p.Kokuji1 == "1" && (p.SinKouiKbn == OdrKouiKbnConst.Jyoumyaku || p.SinKouiKbn == OdrKouiKbnConst.Hikakin)))
                                {
                                    itemname =
                                        filteredOdrDtl.First(p => p.Kokuji1 == "1" && (p.SinKouiKbn == OdrKouiKbnConst.Jyoumyaku || p.SinKouiKbn == OdrKouiKbnConst.Hikakin)).ItemName;

                                }
                                else if(odrInf.OdrKouiKbn == OdrKouiKbnConst.Jyoumyaku)
                                {
                                    itemname = "静脈内注射";
                                }
                                else if (odrInf.OdrKouiKbn == OdrKouiKbnConst.Hikakin)
                                {
                                    itemname = "皮内、皮下及び筋肉内注射";
                                }

                                _common.AppendCalcLog(2,
                                    $"'{itemname}' は、薬剤がないため、算定できません。");

                                continue;
                            }
                        }

                        int kouiKbn = ReceKouiKbn.ChusyaSonota;
                        int sinId = ReceSinId.ChusyaSonota;
                        string syukeiSaki = ReceSyukeisaki.ChusyaSonota;

                        if (odrInf.OdrKouiKbn == OdrKouiKbnConst.Jyoumyaku)
                        {
                            kouiKbn = ReceKouiKbn.Jyomyaku;
                            sinId = ReceSinId.Jyomyaku;
                            syukeiSaki = ReceSyukeisaki.ChusyaJyoumyaku;
                        }
                        else if (odrInf.OdrKouiKbn == OdrKouiKbnConst.Hikakin)
                        {
                            kouiKbn = ReceKouiKbn.Hikakin;
                            sinId = ReceSinId.Hikakin;
                            syukeiSaki = ReceSyukeisaki.ChusyaHikakin;
                        }

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(kouiKbn, sinId, odrInf.SanteiKbn);

                        // 集計先は、後で内容により変更する
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "G"));

                        int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        //bool commentSkipFlg = (firstItem != 0);
                        // 注射はすべてのコメントを手技につける
                        bool commentSkipFlg = false;
                        // 最初のコメント以外の項目であることを示すフラグ
                        bool firstSinryoKoui = true;

                        //bool syugi = false;
                        bool jyomyakuOdr = false;
                        bool hikakinOdr = false;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {                            
                            if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || _common.IsSelectComment(odrDtl.ItemCd)))
                            {
                                // 診療行為・コメント

                                //commentSkipFlg = false;

                                if(odrDtl.ItemCd == ItemCdConst.ChusyaSyuginasi31)
                                {
                                    hikakinOdr = true;
                                }
                                else if(odrDtl.ItemCd == ItemCdConst.ChusyaSyuginasi32)
                                {                                    
                                    jyomyakuOdr = true;
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
                                        //最初以外の基本項目が来たらRpを分ける　
                                        //※最初にコメントが入っていると困るのでこういう処理にする
                                        //if (odrDtl.Kokuji2 != "7")
                                        //{
                                            _common.Wrk.AppendNewWrkSinRpInf(kouiKbn, sinId, odrInf.SanteiKbn);
                                        //}
                                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "G"));
                                    }

                                    if (!SyugiNasils.Contains(odrDtl.ItemCd))
                                    {
                                        //syugi = true;
                                    }

                                }

                                if (!odrDtl.IsComment)
                                {
                                    // コメント項目以外

                                    // 算定回数チェック
                                    if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                                    {
                                        // 算定回数マスタのチェックにより算定不可
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                    }
                                    else if(_common.CheckAge(odrDtl) == 2)
                                    {
                                        // 年齢チェックにより算定不可
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                    }
                                    else
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                        if (odrDtl.IsKihonKoumoku && odrDtl.ItemCd != ItemCdConst.ChusyaHoumonTenteki && SyugiNasils.Contains(odrDtl.ItemCd) == false)
                                        {
                                            // 訪問点滴、手技なしダミーの場合、加算はつけない
                                            if (filteredOdrDtl.Any(p => p.SeibutuKbn == 1) && filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.ChusyaSeibutuKasan) == false)
                                            {
                                                // 生物学的製剤加算
                                                //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "G"));
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaSeibutuKasan, autoAdd: 1);
                                            }

                                            if (filteredOdrDtl.Any(p => p.MadokuKbn == 1) && filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.ChusyaMayakuKasan) == false)
                                            {
                                                // 麻薬加算
                                                //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "G"));
                                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaMayakuKasan, autoAdd: 1);
                                            }
                                        }

                                        // 年齢加算自動算定
                                        _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl);

                                        // コメント自動追加
                                        _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);

                                    }
                                }
                                else
                                {
                                    // コメント
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                }
                            }
                            else
                            {
                                //commentSkipFlg = true;
                            }

                        }

                        if(jyomyakuOdr)
                        {
                            // 静脈注射手技なしがオーダーされている場合
                            _common.Wrk.wrkSinRpInfs.Last().SinKouiKbn = ReceKouiKbn.Jyomyaku;
                            _common.Wrk.wrkSinRpInfs.Last().SinId = ReceSinId.Jyomyaku;
                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.ChusyaJyoumyaku;
                            syukeiSaki = ReceSyukeisaki.ChusyaJyoumyaku;
                        }
                        else if(hikakinOdr)
                        {
                            // 皮下筋肉注射手技なしがオーダーされている場合
                            _common.Wrk.wrkSinRpInfs.Last().SinKouiKbn = ReceKouiKbn.Hikakin;
                            _common.Wrk.wrkSinRpInfs.Last().SinId = ReceSinId.Hikakin;
                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.ChusyaHikakin;
                            syukeiSaki = ReceSyukeisaki.ChusyaHikakin;
                        }

                        if(_common.IsJibaiRosai)
                        {
                            // 自賠労災準拠の場合、薬剤は別集計
                            syukeiSaki = ReceSyukeisaki.ChusyaYakuzai;
                        }

                        // 薬剤・コメント算定
                        //commentSkipFlg = (firstItem != 1);
                        commentSkipFlg = true;

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, _common.GetCdKbn(odrInf.SanteiKbn, "G"), ref firstSinryoKoui);
                        
                        //bool SeibutuKasan = false;
                        //bool MayakuKasan = false;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            //if (odrDtl.IsYorCommentItem(commentSkipFlg))
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if (odrDtl.IsYItem)
                            {
                                // 薬剤・コメント
                                if (odrDtl.DrugKbn != 4)
                                {
                                    // 注射薬以外は算定不可
                                    //_common.AppendCalcLog(2, $"{odrDtl.ItemName}は、注射薬ではないため、算定できません。");
                                    _common.AppendCalcLog(1, $"{odrDtl.ItemName}は、注射薬ではないため、算定できない可能性があります。");
                                }
                                //else
                                //{
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    //commentSkipFlg = false;

                                    //// 生物学的製剤加算チェック
                                    //if (odrDtl.SeibutuKbn == 1)
                                    //{
                                    //    SeibutuKasan = true;
                                    //}
                                    //// 麻薬チェック
                                    //if (odrDtl.MadokuKbn == 1)
                                    //{
                                    //    MayakuKasan = true;
                                    //}
                                //}
                            }
                            //else
                            //{
                            //    commentSkipFlg = true;
                            //}
                        }

                        //if (syugi)
                        //{
                        //    // 手技料の算定がある場合は加算チェック
                        //    if (SeibutuKasan)
                        //    {
                        //        // 生物学的製剤加算
                        //        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "G"));
                        //        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaSeibutuKasan, autoAdd: 1);
                        //    }

                        //    if (MayakuKasan)
                        //    {
                        //        // 麻薬加算
                        //        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "G"));
                        //        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaMayakuKasan, autoAdd: 1);
                        //    }

                        //    // 1回だけなので、ここでfalse
                        //    syugi = false;
                        //}

                        // 特材・コメント算定

                        //commentSkipFlg = (firstItem != 2);
                        commentSkipFlg = true;
                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, _common.GetCdKbn(odrInf.SanteiKbn, "G"), ref firstSinryoKoui);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            //if (odrDtl.IsTorCommentItem(commentSkipFlg))
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if(odrDtl.IsTItem)
                            {
                                // 特材・コメント
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                //commentSkipFlg = false;
                            }
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

        /// <summary>
        /// 点滴の計算
        /// </summary>
        private void Tenteki()
        {
            for (int i = 0; i <= 1; i++)
            {
                List<OdrInfModel> tentekiOdrInf;
                List<OdrInfModel> filteredOdrInf;
                List<OdrDtlTenModel> filteredOdrDtls;
                bool ret = false;
                int isNodspRece = 0;
                int addDummy = 0;
                bool excludeComment = false;
                double totalSuryo = 0;

                //tentekiOdrInf = _common.Odr.FilterOdrInfByKouiKbnRangeToday(OdrKouiKbnConst.Chusya, OdrKouiKbnConst.Tenteki);
                //tentekiOdrInf = _common.Odr.FilterOdrInfByKouiKbnRangeToday(OdrKouiKbnConst.Tenteki, OdrKouiKbnConst.Tenteki);

                int santeiKbn = 0;
                if(i == 1)
                {
                    santeiKbn = 2;
                }

                tentekiOdrInf = _common.Odr.FilterOdrInfTentekiToday(santeiKbn);

                //// 注射薬の総量を求める
                //foreach (OdrInfModel odrInf in tentekiOdrInf)
                //{
                //    filteredOdrDtls = _common.Odr.FilterOdrDetailTentekiByRpNo(odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo);

                //    if (filteredOdrDtls.Any(p => Tentekils.Contains(p.ItemCd) || p.ItemCd == ItemCdConst.ChusyaSyuginasi33))
                //    {
                //        // 点滴 or 手技なし33
                //        foreach (OdrDtlTenModel odrDtl in filteredOdrDtls.FindAll(p => p.DrugKbn == 4))
                //        {
                //            totalSuryo += odrDtl.CalcedSuryo * odrDtl.Capacity;
                //        }
                //    }
                //}

                var raiinOdrInfs = tentekiOdrInf.FindAll(p => p.RaiinNo == _common.raiinNo);
                bool existTentekiItem = false;
                foreach (OdrInfModel raiinOdrInf in raiinOdrInfs)
                {
                    if (_common.Odr.FilterOdrDetailByRpNo(raiinOdrInf.RpNo, raiinOdrInf.RpEdaNo).Any(p => Tentekils.Contains(p.ItemCd)))
                    {
                        existTentekiItem = true;
                        break;
                    }
                }

                if (existTentekiItem)
                {

                    // 前回分
                    filteredOdrInf =
                        tentekiOdrInf.FindAll(p =>
                            (
                                CIUtil.StrToIntDef(p.SinStartTime, 0) < CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) ||
                                (CIUtil.StrToIntDef(p.SinStartTime, 0) == CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) && p.RaiinNo < _common.raiinNo)
                            ) &&
                            p.RaiinNo != _common.raiinNo);

                    if (filteredOdrInf.Any())
                    {
                        // 注射薬の総量を求める
                        totalSuryo = 0;
                        foreach (OdrInfModel odrInf in filteredOdrInf)
                        {
                            filteredOdrDtls = _common.Odr.FilterOdrDetailTentekiByRpNo(odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo);

                            if (filteredOdrDtls.Any(p => Tentekils.Contains(p.ItemCd) || p.ItemCd == ItemCdConst.ChusyaSyuginasi33))
                            {
                                // 点滴 or 手技なし33
                                foreach (OdrDtlTenModel odrDtl in filteredOdrDtls.FindAll(p => p.DrugKbn == 4))
                                {
                                    totalSuryo += odrDtl.CalcedSuryo * odrDtl.Capacity;
                                }
                            }
                        }

                        isNodspRece = 1;
                        addDummy = 2;
                        excludeComment = true;

                        ret = TentekiCalc(filteredOdrInf, totalSuryo, isNodspRece, addDummy, excludeComment);
                    }

                    // 今回含め
                    //filteredOdrInf = tentekiOdrInf.FindAll(p => CIUtil.StrToIntDef(p.SinStartTime, 0) <= CIUtil.StrToIntDef(_common.raiinInf.SinStartTime));
                    filteredOdrInf =
                        tentekiOdrInf.FindAll(p =>
                            (
                                CIUtil.StrToIntDef(p.SinStartTime, 0) < CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) ||
                                (CIUtil.StrToIntDef(p.SinStartTime, 0) == CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) && p.RaiinNo < _common.raiinNo)
                            ) ||
                            p.RaiinNo == _common.raiinNo);

                    if (filteredOdrInf.Any())
                    {
                        isNodspRece = 0;
                        addDummy = 0;
                        excludeComment = false;
                        totalSuryo = 0;

                        // 注射薬の総量を求める
                        foreach (OdrInfModel odrInf in filteredOdrInf)
                        {
                            filteredOdrDtls = _common.Odr.FilterOdrDetailTentekiByRpNo(odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo);

                            if (filteredOdrDtls.Any(p => Tentekils.Contains(p.ItemCd) || p.ItemCd == ItemCdConst.ChusyaSyuginasi33))
                            {
                                // 点滴 or 手技なし33
                                foreach (OdrDtlTenModel odrDtl in filteredOdrDtls.FindAll(p => p.DrugKbn == 4))
                                {
                                    totalSuryo += odrDtl.CalcedSuryo * odrDtl.Capacity;
                                }
                            }
                        }

                        if (_common.Odr.odrDtlls.Any(p =>
                                p.HpId == _common.hpId &&
                                p.PtId == _common.ptId &&
                                p.SinDate == _common.sinDate &&
                                (
                                    CIUtil.StrToIntDef(p.SinStartTime, 0) > CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) ||
                                    (CIUtil.StrToIntDef(p.SinStartTime, 0) == CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) && p.RaiinNo > _common.raiinNo)
                                )
                                &&
                                p.RaiinNo != _common.raiinNo &&
                                p.HokenSyu == _common.Odr.HokenKbn &&
                                //(Tentekils.Contains(p.ItemCd) || p.ItemCd == ItemCdConst.ChusyaSyuginasi33)))
                                (Tentekils.Contains(p.ItemCd))))
                        {
                            // 以降に点滴のある来院が存在する場合、今回分はレセに表示しない
                            isNodspRece = 1;
                            addDummy = 1;
                            //excludeComment = false;
                            excludeComment = true;
                        }
                        else
                        {
                            // 以降に点滴のある来院がない場合で、
                            // 以降に手技なしダミー33のみの点滴行為のオーダーがある来院がある場合、
                            // その来院の手技なしダミー33の薬剤も点滴の薬剤とみなし、総量に加える
                            if (_common.Odr.odrDtlls.Any(p =>
                                    p.HpId == _common.hpId &&
                                    p.PtId == _common.ptId &&
                                    p.SinDate == _common.sinDate &&
                                    (
                                        CIUtil.StrToIntDef(p.SinStartTime, 0) > CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) ||
                                        (CIUtil.StrToIntDef(p.SinStartTime, 0) == CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) && p.RaiinNo > _common.raiinNo)
                                    ) &&
                                    p.RaiinNo != _common.raiinNo &&
                                    p.HokenSyu == _common.Odr.HokenKbn &&
                                    p.ItemCd == ItemCdConst.ChusyaSyuginasi33))
                            {
                                // 手技なしダミーのある来院がある場合
                                foreach (var odrkey in
                                    _common.Odr.odrDtlls.Where(p =>
                                        p.HpId == _common.hpId &&
                                        p.PtId == _common.ptId &&
                                        p.SinDate == _common.sinDate &&
                                        CIUtil.StrToIntDef(p.SinStartTime, 0) >= CIUtil.StrToIntDef(_common.raiinInf.SinStartTime, 0) &&
                                        p.RaiinNo != _common.raiinNo &&
                                        p.HokenSyu == _common.Odr.HokenKbn &&
                                        p.ItemCd == ItemCdConst.ChusyaSyuginasi33)
                                    .GroupBy(p => new { p.RaiinNo, p.RpNo, p.RpEdaNo }).ToList())
                                {
                                    // 手技なしダミーを含むRpのキー情報を取得してループ

                                    if (_common.Odr.odrDtlls.Any(p =>
                                        p.RaiinNo == odrkey.Key.RaiinNo &&
                                        p.IsKihonKoumoku &&
                                        p.HokenSyu == _common.Odr.HokenKbn &&
                                        p.ItemCd != ItemCdConst.ChusyaSyuginasi33 &&
                                        p.OdrKouiKbn == OdrKouiKbnConst.Tenteki) == false)
                                    {
                                        // 手技なしダミー以外に点滴行為のオーダーがない来院が対象
                                        foreach (OdrInfModel odrInf in tentekiOdrInf.FindAll(p =>
                                            p.RaiinNo == odrkey.Key.RaiinNo && p.RpNo == odrkey.Key.RpNo && p.RpEdaNo == odrkey.Key.RpEdaNo))
                                        {
                                            filteredOdrDtls =
                                                _common.Odr.FilterOdrDetailTentekiByRpNo(odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo);

                                            if (filteredOdrDtls.Any(p => Tentekils.Contains(p.ItemCd) || p.ItemCd == ItemCdConst.ChusyaSyuginasi33))
                                            {
                                                // 点滴 or 手技なし33
                                                foreach (OdrDtlTenModel odrDtl in filteredOdrDtls.FindAll(p => p.DrugKbn == 4))
                                                {
                                                    totalSuryo += odrDtl.CalcedSuryo * odrDtl.Capacity;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        TentekiCalc(filteredOdrInf, totalSuryo, isNodspRece, addDummy, excludeComment);
                    }
                }
                else
                {
                    isNodspRece = 0;
                    addDummy = 0;
                    excludeComment = false;

                    filteredOdrInf = tentekiOdrInf.FindAll(p => p.RaiinNo == _common.raiinNo);
                    TentekiCalc(filteredOdrInf, totalSuryo, isNodspRece, addDummy, excludeComment);
                }
            }

        }

        /// <summary>
        /// 点滴の計算本体
        /// </summary>
        /// <param name="filteredOdrInf"></param>
        /// <param name="totalSuryo">薬剤総量</param>
        /// <param name="isNodspRece">
        /// レセ非表示設定
        ///     0-表示する
        ///     1-表示しない
        /// </param>
        /// <param name="addDummy">
        /// 相殺ダミー項目追加モード
        ///     0-追加しない
        ///     1-点数調整
        ///     2-レセ非表示
        /// </param>
        /// <param name="excludeComment">
        ///     true-コメント読み飛ばし
        /// </param>
        private bool TentekiCalc(List<OdrInfModel> filteredOdrInf, double totalSuryo, int isNodspRece, int addDummy, bool excludeComment)
        {
            bool ret = false;

            if (filteredOdrInf.Any())
            {
                
                List<OdrDtlTenModel> filteredOdrDtl;
                string itemCd = "";

                // 数量による手技料の差し替え
                itemCd = ItemCdConst.ChusyaTenteki;
                if (_common.IsYoJi && totalSuryo >= 100)
                {
                    // 6歳未満 100ml以上
                    itemCd = ItemCdConst.ChusyaTentekiNyu100;
                }
                else if (totalSuryo >= 500)
                {
                    // 6歳以上 500ml以上
                    itemCd = ItemCdConst.ChusyaTenteki500;
                }

                // 点滴手技算定
                bool firstSinryoKoui = true;
                bool santei = false;
                bool syugiSantei = false;
                //bool seibutuKasan = false;
                //bool mayakuKasan = false;

                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Tenteki, ReceSinId.ChusyaSonota, filteredOdrInf.First().SanteiKbn);
                _common.Wrk.AppendNewWrkSinKoui(filteredOdrInf.First().HokenPid, filteredOdrInf.First().HokenId, ReceSyukeisaki.ChusyaSonota, isNodspRece: isNodspRece, cdKbn: _common.GetCdKbn(filteredOdrInf.First().SanteiKbn, "G"));

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    filteredOdrDtl = _common.Odr.FilterOdrDetailTentekiByRpNo(odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any(p => Tentekils.Contains(p.ItemCd)))
                    {
                        // 点滴手技料を含むRpの場合
                        if (TentekiSyugiDtlCalc(filteredOdrDtl, odrInf.HokenPid, odrInf.HokenId, itemCd, isNodspRece, excludeComment, ref syugiSantei, ref firstSinryoKoui))
                        {
                            santei = true;
                            ret = true;

                            firstSinryoKoui = false;
                        }
                    }
                }

                _AddSousatuDummy(santei, addDummy);

                // 薬剤・コメント算定
                //_AddOrReplaceKoui(firstSinryoKoui, filteredOdrInf.First().HokenPid, filteredOdrInf.First().HokenId, filteredOdrInf.First().SanteiKbn, isNodspRece);
                
                santei = false;
                bool firstYakuzai = true;

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    filteredOdrDtl = _common.Odr.FilterOdrDetailTentekiByRpNo(odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any(p => Tentekils.Contains(p.ItemCd)))
                    {
                        if (firstYakuzai)
                        {
                            _AddOrReplaceKoui(firstSinryoKoui, odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn, isNodspRece);
                            firstYakuzai = false;
                        }

                        if (TentekiYakuzaiDtlCalc(filteredOdrDtl, true))
                        {
                            santei = true;
                            ret = true;

                            firstSinryoKoui = false;
                        }
                    }
                }

                //if (santei)
                //{
                //    // 手技料の算定がある場合は加算チェック
                //    if (seibutuKasan)
                //    {
                //        // 生物学的製剤加算
                //        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaSeibutuKasan, autoAdd: 1);
                //    }

                //    if (mayakuKasan)
                //    {
                //        // 麻薬加算
                //        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaMayakuKasan, autoAdd: 1);
                //    }
                //}

                _AddSousatuDummy(santei, addDummy);

                // 特材・コメント算定
                //_AddOrReplaceKoui(firstSinryoKoui, filteredOdrInf.First().HokenPid, filteredOdrInf.First().HokenId, filteredOdrInf.First().SanteiKbn, isNodspRece);
                
                santei = false;
                foreach (OdrInfModel odrInf in filteredOdrInf)
                {

                    filteredOdrDtl = _common.Odr.FilterOdrDetailTentekiByRpNo(odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any(p => Tentekils.Contains(p.ItemCd)))
                    {
                        _AddOrReplaceKoui(firstSinryoKoui, odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn, isNodspRece);

                        if (TentekiTokuzaiDtlCalc(filteredOdrDtl, odrInf.HokenPid, odrInf.HokenId, isNodspRece, true, ref firstSinryoKoui))
                        {
                            santei = true;
                            ret = true;

                            firstSinryoKoui = false;
                        }
                    }
                }

                _AddSousatuDummy(santei, addDummy);

                // 点滴手技なし算定
                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    firstSinryoKoui = true;

                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.ChusyaSyuginasi33))
                    {
                        // 手技料算定（コメント出力なし、薬剤につける）
                        syugiSantei = false;
                        //TentekiSyugiDtlCalc(filteredOdrDtl, odrInf.HokenPid, odrInf.HokenId, "", 0, false, ref syugiSantei, ref firstSinryoKoui);
                        TentekiSyugiDtlCalc(filteredOdrDtl, odrInf.HokenPid, odrInf.HokenId, "", 0, true, ref syugiSantei, ref firstSinryoKoui);

                        // 薬剤・コメント算定
                        _AddOrReplaceKoui(firstSinryoKoui, odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn, 0);
                        firstSinryoKoui = false;

                        TentekiYakuzaiDtlCalc(filteredOdrDtl, false, false);

                        // 特材・コメント算定
                        _AddOrReplaceKoui(firstSinryoKoui, odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn, 0);
                        firstSinryoKoui = false;

                        TentekiTokuzaiDtlCalc(filteredOdrDtl, odrInf.HokenPid, odrInf.HokenId, 0, false, ref firstSinryoKoui);

                        // オーダーから削除
                        //_common.Odr.odrDtlls.RemoveAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo);
                        //_common.odrInfls.RemoveAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo);
                    }
                }
            }

            return ret;

            #region Local Method

            void _AddSousatuDummy(bool Asantei, int AaddDummy)
            {
                if (Asantei)
                {
                    if (AaddDummy == 1)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaTentekiSosatuNoDsp, autoAdd: 1);
                    }
                    else if (AaddDummy == 2)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaTentekiSosatuTenAdj, autoAdd: 1);
                    }
                }
            }

            void _AddOrReplaceKoui(bool AfirstSinryoKoui, int hokenPid, int hokenId, int santeiKbn, int AisNodspRece)
            {
                string syukeiSaki = ReceSyukeisaki.ChusyaSonota;

                if(_common.hokenKbn == HokenSyu.Jibai)
                {
                    // 自賠の場合、注射薬剤に集計
                    syukeiSaki = ReceSyukeisaki.ChusyaYakuzai;
                }

                if (AfirstSinryoKoui == true)
                {
                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = syukeiSaki;
                }
                else
                {
                    // 行為を追加する
                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, syukeiSaki, isNodspRece: AisNodspRece, cdKbn: _common.GetCdKbn(santeiKbn, "G"));
                }
            }
            #endregion
        }

        /// <summary>
        /// 点滴手技算定
        /// </summary>
        /// <param name="filteredOdrDtl"></param>
        /// <param name="hokenPid"></param>
        /// <param name="itemCd"></param>
        /// <param name="isNodspRece"></param>
        /// <param name="excludeComment"></param>
        /// <param name="firstSinryoKoui"></param>
        /// <returns></returns>
        private bool TentekiSyugiDtlCalc(List<OdrDtlTenModel> filteredOdrDtl, int hokenPid, int hokenId, string itemCd, int isNodspRece, bool excludeComment, ref bool syugiSantei, ref bool firstSinryoKoui)
        {
            bool ret = false;
            bool commentSkipFlg = false;

            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
            {
                if (excludeComment && _common.IsCommentItemCd(odrDtl.ItemCd))
                {
                    // コメント読み飛ばし
                }
                else
                {
                    if (odrDtl.IsSorCommentItem(commentSkipFlg))
                    {
                        // 診療行為・コメント

                        commentSkipFlg = false;

                        if (odrDtl.IsKihonKoumoku)
                        {
                            // 基本項目

                            if (firstSinryoKoui == true)
                            {
                                firstSinryoKoui = false;
                                _common.Wrk.wrkSinKouis.Last().HokenPid = hokenPid;
                                _common.Wrk.wrkSinKouis.Last().HokenId = hokenId;
                            }
                            else
                            {
                                //_common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.ChusyaSonota, isNodspRece: isNodspRece, cdKbn: _common.GetCdKbn(odrDtl.SanteiKbn, "G"));
                            }
                        }

                        if (!(_common.IsCommentItemCd(odrDtl.ItemCd)))
                        {
                            // コメント項目以外

                            //// 算定回数チェック
                            //if (_common.CheckSanteiKaisu(odrDtl.ItemCd) == 2)
                            //{
                            //    // 算定回数マスタのチェックにより算定不可
                            //}
                            //else
                            {
                                if (syugiSantei == false || Tentekils.Contains(odrDtl.ItemCd) == false)
                                {
                                    if (itemCd != "" && Tentekils.Contains(odrDtl.ItemCd))
                                    {
                                        // 手技料の項目コード差し替え
                                        _common.Odr.UpdateOdrDtlItemCd(odrDtl, itemCd);

                                        syugiSantei = true;
                                    }
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    if (itemCd != "" && Tentekils.Contains(odrDtl.ItemCd))
                                    {
                                        if (filteredOdrDtl.Any(p => p.SeibutuKbn == 1) && filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.ChusyaSeibutuKasan) == false)
                                        {
                                            // 生物学的製剤注射加算
                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaSeibutuKasan, autoAdd: 1);
                                        }

                                        if (filteredOdrDtl.Any(p => p.MadokuKbn == 1) && filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.ChusyaMayakuKasan) == false)
                                        {
                                            // 麻薬加算
                                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.ChusyaMayakuKasan, autoAdd: 1);
                                        }
                                    }

                                    // 年齢加算自動算定
                                    _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl);

                                    // コメント自動追加
                                    _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);
                                }
                            }
                        }
                        else
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }

                        ret = true;
                    }
                    else
                    {
                        //commentSkipFlg = true;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 点滴薬剤算定
        /// </summary>
        /// <param name="filteredOdrDtl"></param>
        /// <param name="excludeComment"></param>
        /// <returns></returns>
        //private bool TentekiYakuzaiDtlCalc(List<OdrDtlTenModel> filteredOdrDtl, bool excludeComment, ref bool SeibutuKasan, ref bool MayakuKasan, bool commentSkipFlg = true)
        private bool TentekiYakuzaiDtlCalc(List<OdrDtlTenModel> filteredOdrDtl, bool excludeComment, bool commentSkipFlg = true)
        {
            bool ret = false;
            //bool commentSkipFlg = false;
            //bool commentSkipFlg = true;

            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
            {
                if (excludeComment && _common.IsCommentItemCd(odrDtl.ItemCd))
                {
                    // コメント読み飛ばし
                }
                else
                {
                    if (odrDtl.IsYorCommentItem(commentSkipFlg))
                    {
                        // 薬剤・コメント
                        if (odrDtl.IsYItem && odrDtl.DrugKbn != 4)
                        {
                            // 注射薬以外は算定不可
                            //_common.AppendCalcLog(2, $"{odrDtl.ItemName}は、注射薬ではないため、算定できません。");
                            _common.AppendCalcLog(1, $"{odrDtl.ItemName}は、注射薬ではないため、算定できない可能性があります。");
                        }
                        //else
                        //{

                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            commentSkipFlg = false;
                        //}
                        ret = true;
                    }
                    else if(!odrDtl.IsSItem)
                    {
                        commentSkipFlg = true;
                    }
                }

                //// 生物学的製剤加算チェック
                //if (odrDtl.SeibutuKbn == 1)
                //{
                //    SeibutuKasan = true;
                //}
                //// 麻毒チェック
                //if (odrDtl.MadokuKbn == 1)
                //{
                //    MayakuKasan = true;
                //}
            }

            return ret;

        }

        /// <summary>
        /// 点滴特材算定
        /// </summary>
        /// <param name="filteredOdrDtl"></param>
        /// <param name="hokenId"></param>
        /// <param name="isNodspRece"></param>
        /// <param name="firstSinryoKoui"></param>
        /// <returns></returns>
        private bool TentekiTokuzaiDtlCalc(List<OdrDtlTenModel> filteredOdrDtl, int hokenPid, int hokenId, int isNodspRece, bool excludeComment, ref bool firstSinryoKoui)
        {
            bool ret = false;
            //bool commentSkipFlg = false;
            bool commentSkipFlg = true;

            if (firstSinryoKoui == true)
            {
                firstSinryoKoui = false;
                _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.ChusyaSonota;
            }
            else
            {
                // 行為を追加する
                _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.ChusyaSonota, isNodspRece: isNodspRece, cdKbn: _common.GetCdKbn(filteredOdrDtl.First().SanteiKbn, "G"));
            }

            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
            {
                if (excludeComment && _common.IsCommentItemCd(odrDtl.ItemCd))
                {
                    // コメント読み飛ばし
                }
                else
                {
                    if (odrDtl.IsTorCommentItem(commentSkipFlg))
                    {
                        // 特材・コメント
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                        commentSkipFlg = false;

                        ret = true;
                    }
                    else
                    {
                        commentSkipFlg = true;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {
            const string conFncName = nameof(CalculateJihi);

            _common.CalculateJihi(
                OdrKouiKbnConst.ChusyaMin,
                OdrKouiKbnConst.ChusyaMax,
                ReceKouiKbn.ChusyaSonota,
                ReceSinId.ChusyaSonota,
                ReceSyukeisaki.ChusyaSonota,
                "JS");
        }
    }
}
