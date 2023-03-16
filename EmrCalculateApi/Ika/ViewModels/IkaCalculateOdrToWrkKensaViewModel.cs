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
    /// 検査
    /// </summary>
    class IkaCalculateOdrToWrkKensaViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 採血料の項目リスト
        /// </summary>
        List<string> Saiketuls =
            new List<string>
            {
                ItemCdConst.KensaBV,
                ItemCdConst.KensaBC
            };

        /// <summary>
        /// 内視鏡検査時間加算の項目リスト
        /// </summary>
        List<string> NaisiJikanls =
            new List<string>
            {
                ItemCdConst.KensaNaisiJikangai,
                ItemCdConst.KensaNaisiKyujitu,
                ItemCdConst.KensaNaisiSinya
            };

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkKensaViewModel(IkaCalculateCommonDataViewModel common,
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

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.KensaMin, OdrKouiKbnConst.KensaMax))
            {
                // 保険
                Console.WriteLine("Start CalculateHoken IkaCalculateOdrToWrkKensaViewModel");
                CalculateHoken();
                Console.WriteLine("End CalculateHoken IkaCalculateOdrToWrkKensaViewModel");

                // 自費
                Console.WriteLine("Start CalculateJihi IkaCalculateOdrToWrkKensaViewModel");
                CalculateJihi();
                Console.WriteLine("End CalculateJihi IkaCalculateOdrToWrkKensaViewModel");
            }

            Console.WriteLine("Start CommitWrkSinRpInf IkaCalculateOdrToWrkKensaViewModel");
            _common.Wrk.CommitWrkSinRpInf();
            Console.WriteLine("End CommitWrkSinRpInf IkaCalculateOdrToWrkKensaViewModel");

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 保険分を処理する
        /// </summary>
        private void CalculateHoken()
        {
            const string conFncName = nameof(CalculateHoken);

            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            //List<string> manseiIjils =
            //    new List<string>
            //    {
            //        ItemCdConst.IgakuManseiIji
            //    };

            //// 慢性維持透析と検査項目の背反チェック
            //if (_common.Wrk.wrkSinKouiDetails.Any(p => manseiIjils.Contains(p.ItemCd)) ||
            //    _common.Sin.GetSanteiDaysSinYm(manseiIjils).Any())
            //{
            //    List<DensiHoukatuMstModel> houkatuMsts = _common.Mst.GetDensiHiHoukatu(_common.ptId, manseiIjils, _common.IsRosai);

            //    filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.KensaMin, OdrKouiKbnConst.KensaMax);

            //    if (filteredOdrInf.Any())
            //    {
            //        List<(long rpNo, long rpEdaNo, int rowNo)> delDtlKeys = new List<(long rpNo, long seqNo, int rowNo)>();

            //        foreach (OdrInfModel odrInf in filteredOdrInf)
            //        {
            //            // 行為に紐づく詳細を取得
            //            filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);
            //            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
            //            {
            //                DensiHoukatuMstModel houkatuMst = houkatuMsts.Find(p => p.ItemCd == odrDtl.ItemCd && p.SpJyoken == 0);

            //                if(houkatuMst != null)
            //                {
            //                    delDtlKeys.Add((odrDtl.RpNo, odrDtl.RpEdaNo, odrDtl.RowNo));
            //                    _common.Wrk.AppendNewWrkSinKouiDetailDel
            //                        (hokenKbn: odrDtl.HokenKbn,
            //                            rpNo: 0,
            //                            seqNo: 0,
            //                            rowNo: 0,
            //                            itemCd: odrDtl.ItemCd,
            //                            delItemCd: houkatuMst.DelItemCd,
            //                            santeiDate: 0,
            //                            delSbt: 0,
            //                            isWarning: 1,
            //                            termCnt: 1,
            //                            termSbt: 1);
            //                }
            //            }
            //        }

            //        // オーダーから削除
            //        foreach((long rpNo, long rpEdaNo, int rowNo) delDtlKey in delDtlKeys)
            //        {
            //            _common.Odr.odrDtlls.RemoveAll(p => p.RpNo == delDtlKey.rpNo && p.RpEdaNo == delDtlKey.rpEdaNo && p.RowNo == delDtlKey.rowNo);
            //        }
            //    }
            //}

            // 採血料
            Saiketu();

            // 判断料
            Handan();

            // まるめ計算
            Marume();

            // 通常算定処理

            // 検査のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.KensaMin, OdrKouiKbnConst.KensaMax);

            if (filteredOdrInf.Any())
            {
                List<(int hokatuKbn, string cdKbn, int cdKbnNo, int cdEdaNo, int cdKouNo)> santeiTeigenKbn =
                    new List<(int hokatuKbn, string cdKbn, int cdKbnNo, int cdEdaNo, int cdKouNo)>();

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any())
                    {
                        string cdKbn = "D";
                        /*
                        if (filteredOdrDtl.Any(p => p.TyuCd == "0" && p.IsKihonKoumoku) == false &&
                            filteredOdrDtl.Any(p => p.TyuCd != "0" && !string.IsNullOrEmpty(p.TyuCd)))
                        {
                            string tyuCd = filteredOdrDtl.First(p => p.TyuCd != "0" && !string.IsNullOrEmpty(p.TyuCd)).TyuCd;

                            for (int i = 0; i < filteredOdrDtl.Count; i++)
                            {
                                if (filteredOdrDtl[i].TyuCd == "0" || string.IsNullOrEmpty(filteredOdrDtl[i].TyuCd))
                                {
                                    //if (filteredOdrDtl[i].IsKihonKoumoku)
                                    //{
                                    filteredOdrDtl[i].TyuCd = tyuCd + "D";
                                    //}
                                    //else
                                    //{
                                    //   filteredOdrDtl[i].TyuCd = tyuCd;
                                    //}
                                }
                                else
                                {
                                    tyuCd = filteredOdrDtl[i].TyuCd;
                                }
                            }
                        }
                        */
                        string tyuCD = "0";
                        bool firstSItem = true;

                        // TYU_CD=0の加算項目を上書きするかどうか
                        // TYU_CD=0の基本項目がなく、TYU_CD!=0の基本項目が存在する場合、true
                        bool zeroOverWrite =
                            (filteredOdrDtl.Any(p => p.TyuCd == "0" && p.IsKihonKoumoku) == false &&
                            filteredOdrDtl.Any(p => p.TyuCd != "0" && !string.IsNullOrEmpty(p.TyuCd)));

                        for (int i = 0; i < filteredOdrDtl.Count; i++)
                        {
                            if (filteredOdrDtl[i].IsSItem)
                            {

                                if ((string.IsNullOrEmpty(filteredOdrDtl[i].TyuCd) == false && filteredOdrDtl[i].TyuCd != "0") ||
                                    zeroOverWrite == false)
                                {
                                    // TYU_CD=0の上書きをしない場合
                                    // または、TYU_CD != 0の場合
                                    // これ以降の項目に反映するため、TYU_CDを記憶する
                                    // ※先頭S項目の場合は、この前の項目にも反映
                                    tyuCD = filteredOdrDtl[i].TyuCd;
                                }

                                if (firstSItem)
                                {

                                    if ((string.IsNullOrEmpty(filteredOdrDtl[i].TyuCd) == false && filteredOdrDtl[i].TyuCd != "0") ||
                                        zeroOverWrite == false)
                                    {
                                        // TYU_CD != 0 の場合
                                        // または、TYU_CD=0の上書きをしない場合
                                        // この項目より上にある項目のTYU_CDを、この項目のTYU_CDで上書きする
                                        firstSItem = false;
                                        for (int j = i - 1; j >= 0; j--)
                                        {
                                            if (filteredOdrDtl[j].IsSItem == false ||
                                                (zeroOverWrite &&
                                                    filteredOdrDtl[j].IsKihonKoumoku == false &&
                                                    (filteredOdrDtl[j].TyuCd == "0" || string.IsNullOrEmpty(filteredOdrDtl[j].TyuCd))))
                                            {
                                                // S項目以外、またはTYU_CD=0を上書き可能な場合
                                                filteredOdrDtl[j].TyuCd = tyuCD + "D";
                                            }
                                        }
                                    }
                                }
                                else if (zeroOverWrite &&
                                    filteredOdrDtl[i].IsKihonKoumoku == false &&
                                    (string.IsNullOrEmpty(filteredOdrDtl[i].TyuCd) || filteredOdrDtl[i].TyuCd == "0"))
                                {
                                    // TYU_CD=0を上書きする場合で、
                                    // 基本項目ではない、TYU_CD=0の項目の場合
                                    // 直前のS項目のTYU_CDで上書き
                                    filteredOdrDtl[i].TyuCd = tyuCD + "D";
                                }
                            }
                            else
                            {
                                if (firstSItem == false && (tyuCD != "0" || zeroOverWrite == false))
                                {
                                    // 最初のS項目を処理済みで、
                                    // 　TYU_CDが0ではない
                                    // 　または、TYU_CD=0を上書きしない場合
                                    filteredOdrDtl[i].TyuCd = tyuCD + "D";
                                }
                                else
                                {
                                    filteredOdrDtl[i].TyuCd = "0";
                                }
                            }
                        }

                        //List<string> tyuCds = filteredOdrDtl.Where(p => string.IsNullOrEmpty(p.TyuCd) == false).GroupBy(p => p.TyuCd).Select(p => p.Key).ToList();
                        //List<string> tyuCds = filteredOdrDtl.GroupBy(p => p.TyuCd).Select(p => p.Key).ToList();
                        List<string> tyuCds =
                            filteredOdrDtl.Where(p => string.IsNullOrEmpty(p.TyuCd) == false && p.TyuCd.EndsWith("D") == false).GroupBy(p => p.TyuCd).Select(p => p.Key).ToList();
                        if (tyuCds.Any() == false)
                        {
                            tyuCds.Add("");
                        }

                        //// 注加算コードの設定がある加算項目を含む場合、分割しない
                        //bool split = 
                        //    filteredOdrDtl.Any(p => 
                        //        string.IsNullOrEmpty(p.TyuCd) == false && 
                        //        p.TyuCd != "0" && 
                        //        (new string[] { "7", "9" }.Contains(p.Kokuji2))) == false;

                        //// 初回、必ずRpと行為のレコードを用意
                        //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, odrInf.SanteiKbn);

                        //// 集計先は、後で内容により変更する
                        //cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                        //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Kensa, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn), odrRpNo: odrInf.RpNo);

                        int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        bool commentSkipFlg = (firstItem != 0);
                        bool selectCommentSkipFlg = commentSkipFlg;

                        // 最初のコメント以外の項目であることを示すフラグ
                        bool firstSinryoKoui = true;

                        //// 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, odrInf.SanteiKbn);

                        //// 集計先は、後で内容により変更する
                        cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Kensa, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn), odrRpNo: odrInf.RpNo);

                        foreach (string tyuCd in tyuCds)
                        {
                            // 初回、必ずRpと行為のレコードを用意
                            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, odrInf.SanteiKbn);

                            // 集計先は、後で内容により変更する
                            cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Kensa, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn), odrRpNo: odrInf.RpNo);

                            // 最初のコメント以外の項目であることを示すフラグ
                            firstSinryoKoui = true;

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || (selectCommentSkipFlg == false && _common.IsSelectComment(odrDtl.ItemCd))))
                                {
                                    if (odrDtl.IsComment == false && !odrDtl.TyuCd.StartsWith(tyuCd))
                                    {
                                        commentSkipFlg = true;
                                        selectCommentSkipFlg = true;
                                    }
                                    else
                                    {
                                        if (odrDtl.TyuCd.StartsWith(tyuCd) && odrDtl.TyuCd.EndsWith("D"))
                                        {
                                            odrDtl.TyuCd = "9999";
                                        }

                                        // 診療行為・コメント
                                        // 以降のコメントはこの項目に付随するコメントとみなす
                                        commentSkipFlg = false;
                                        selectCommentSkipFlg = false;

                                        if (odrDtl.IsKihonKoumoku && odrDtl.KensaCmt == 0)
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

                                                // ※検査は基本項目は別Rpに分けたほうがいいのかもしれない。例えば外来迅速とか。
                                                // →分けてしまうとコメントの扱いで混乱するので、付随に再修正
                                                // →外来迅速検体検査加算は別Rpにしたいので分ける
                                                if (odrDtl.Kokuji2 != "7" || odrDtl.ItemCd == ItemCdConst.KensaGairaiJinsoku)
                                                {
                                                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, odrInf.SanteiKbn);
                                                }
                                                cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, cdKbn);
                                                _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Kensa, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn), odrRpNo: odrInf.RpNo);
                                            }

                                            if (_common.Wrk.wrkSinKouis.Last().CdKbn != "JS")
                                            {
                                                if (odrDtl.CdKbn != "")
                                                {
                                                    cdKbn = odrDtl.CdKbn;
                                                    if (cdKbn == "-")
                                                    {
                                                        cdKbn = "D";
                                                    }
                                                    _common.Wrk.wrkSinKouis.Last().CdKbn = odrDtl.CdKbn;

                                                }
                                            }
                                        }

                                        if (!(_common.IsCommentItemCd(odrDtl.ItemCd)))
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

                                                // 年齢加算自動算定
                                                _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl);

                                                if (odrDtl.TeigenKbn == 1)
                                                {
                                                    // 逓減項目
                                                    Teigen(odrDtl, santeiTeigenKbn);
                                                    santeiTeigenKbn.Add((odrDtl.HokatuKbn, odrDtl.CdKbn, odrDtl.CdKbnno, odrDtl.CdEdano, odrDtl.CdKouno));
                                                }
                                                //else if (odrDtl.SinKouiKbn == 62 && odrDtl.TimeKasanKbn == 1)
                                                if (odrDtl.IsKihonKoumoku && odrDtl.TenMst != null && odrDtl.TenMst.CdKbn == "D" && (odrDtl.TenMst.CdKbnno >= 295 && odrDtl.TenMst.CdKbnno <= 325) && odrDtl.TimeKasanKbn == 1)
                                                {
                                                    // 内視鏡時間外加算
                                                    NaisikyoJikan();
                                                }

                                                // コメント自動追加
                                                _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);

                                            }
                                        }
                                        else
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                        }
                                    }
                                }
                                else
                                {
                                    commentSkipFlg = true;

                                    if (odrDtl.IsSItem)
                                    {
                                        selectCommentSkipFlg = true;
                                    }
                                }

                            }
                        //}

                        // 薬剤・コメント算定

                        //commentSkipFlg = false;
                        commentSkipFlg = (firstItem != 1);

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Kensayakuzai, cdKbn, ref firstSinryoKoui, odrInf.RpNo);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl.FindAll(p => p.TyuCd.StartsWith(tyuCd)))
                        {
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                                commentSkipFlg = true;
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

                        //commentSkipFlg = false;
                        commentSkipFlg = (firstItem != 2);

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Kensayakuzai, cdKbn, ref firstSinryoKoui, odrInf.RpNo);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl.FindAll(p => p.TyuCd.StartsWith(tyuCd)))
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
                                commentSkipFlg = true;
                            }
                        }
                        }
                    }
                }

                _common.Wrk.CommitWrkSinRpInf();
            }

        }

        /// <summary>
        /// 採血料算定処理
        /// </summary>
        private void Saiketu()
        {
            if (_common.Odr.ExistOdrDetailByItemCd(Saiketuls) == false)
            {
                if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KensaSaiketuCancel) == false)
                {
                    // 採取料を算定可能な項目があるかチェック（存在する場合、最初に該当する項目の保険組み合わせIDが返ってくる）
                    int hokenPid = -1;
                    int hokenId = -1;
                    int santeiKbn = -1;
                    //(hokenPid, hokenId, santeiKbn) = _common.Odr.ExistSaiketuKensa();
                    (hokenPid, hokenId, santeiKbn) = GetPid(_common.Odr.FindSaiketuKensa());

                    if (hokenPid >= 0)
                    {
                        // 算定回数チェック
                        if (_common.CheckSanteiKaisu(ItemCdConst.KensaBV, santeiKbn, 1) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
                        }
                        else
                        {
                            // 採血料を算定するオーダーが存在するとき
                            // Rpと行為を追加
                            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, santeiKbn);
                            _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.Kensa, cdKbn: _common.GetCdKbn(santeiKbn, "D"));

                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaBV, autoAdd: 1);

                            // 年齢加算
                            _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(ItemCdConst.KensaBV);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指定のオーダー詳細内から適した保険番号、算定区分を取得する
        /// </summary>
        /// <param name="filteredOdrDtls"></param>
        /// <returns></returns>
        private (int pid, int hid, int santeiKbn) GetPid(List<OdrDtlTenModel> filteredOdrDtls)
        {
            int retPid = -1;
            int retHid = -1;
            int retSanteiKbn = -1;

            if (filteredOdrDtls.Any())
            {
                retPid = _common.syosaiPid;
                retHid = _common.syosaiHokenId;
                retSanteiKbn = _common.syosaiSanteiKbn;

                if (_systemConfigProvider.GetDrugPid() == 0)
                {
                    // 公費優先
                    List<int> hokenPids =
                        filteredOdrDtls
                        .Select(p => p.HokenPid)
                        .Distinct()
                        .OrderByDescending(p => p)
                        .ToList();

                    (retPid, retHid) = _common.GetPriorityPid(hokenPids);

                    // 算定区分の確認
                    // 保険→自費の順に並べ替え
                    // 自費算定の薬剤しかない場合、自費算定にする
                    IEnumerable<OdrDtlTenModel> odrDtls =
                        filteredOdrDtls
                        .OrderBy(p => p.SanteiKbn);

                    if (odrDtls.Any())
                    {
                        retSanteiKbn = odrDtls.First().SanteiKbn;
                    }
                }
                else
                {
                    // オーダー優先
                    IEnumerable<OdrDtlTenModel> odrDtls =
                        filteredOdrDtls
                        .OrderBy(p => p.OdrKouiKbn)
                        .ThenBy(p => p.SortNo)
                        .ThenBy(p => p.RpNo)
                        .ThenBy(p => p.RpEdaNo);

                    if (odrDtls.Any())
                    {
                        retPid = 0;
                        retHid = 0;
                        retSanteiKbn = 0;

                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            if (_common.IsBuntenKohi(odrDtl.HokenPid))
                            {
                                retPid = odrDtl.HokenPid;
                                retHid = odrDtl.HokenId;
                                retSanteiKbn = odrDtl.SanteiKbn;
                                break;
                            }
                        }

                        if (retPid <= 0)
                        {
                            retPid = odrDtls.First().HokenPid;
                            retHid = odrDtls.First().HokenId;
                        }

                        // 算定区分の確認
                        // 保険→自費の順に並べ替え
                        // 自費算定の薬剤しかない場合、自費算定にする
                        odrDtls = odrDtls.OrderBy(p => p.SanteiKbn);

                        //if (odrDtls.Any(p => p.SanteiKbn == SanteiKbnConst.Santei))
                        if (odrDtls.Any())
                        {
                            retSanteiKbn = odrDtls.First().SanteiKbn;
                        }
                    }
                }
            }

            if (retSanteiKbn == SanteiKbnConst.SanteiGai) { retSanteiKbn = SanteiKbnConst.Santei; }

            return (retPid, retHid, retSanteiKbn);
        }

        /// <summary>
        /// 判断料算定処理
        /// </summary>
        private void Handan()
        {
            List<string> NohaHandanryo = new List<string>
            {
                ItemCdConst.KensaHandanNoha1,
                ItemCdConst.KensaHandanNoha2
            };

            //List<(int[] kbn, string santeiItem, string cancelItem)> handanls =
            //    new List<(int[], string, string)>
            //    {
            //        // 尿・糞便等検査判断料
            //        (new int[]{1}, ItemCdConst.KensaHandanNyou, ItemCdConst.KensaHandanNyouCancel),
            //        // 血液学的検査判断料
            //        (new int[]{2}, ItemCdConst.KensaHandanKetueki, ItemCdConst.KensaHandanKetuekiCancel),
            //        // 生化学的検査（１）判断料
            //        (new int[]{3}, ItemCdConst.KensaHandanSeika1, ItemCdConst.KensaHandanSeika1Cancel),
            //        // 生化学的検査（２）判断料
            //        (new int[]{4}, ItemCdConst.KensaHandanSeika2, ItemCdConst.KensaHandanSeika2Cancel),
            //        // 免疫学的検査判断料
            //        (new int[]{5}, ItemCdConst.KensaHandanMeneki, ItemCdConst.KensaHandanMenekiCancel),
            //        // 微生物学的検査判断料
            //        (new int[]{6}, ItemCdConst.KensaHandanBiseibutu, ItemCdConst.KensaHandanBiseibutuCancel),
            //        // 呼吸機能検査等判断料
            //        (new int[]{11}, ItemCdConst.KensaHandanKokyu, ItemCdConst.KensaHandanKokyuCancel),
            //        // 脳波検査判断料２
            //        (new int[]{13}, ItemCdConst.KensaHandanNoha2, ItemCdConst.KensaHandanNohaCancel),
            //        // 神経・筋検査判断料
            //        (new int[]{14}, ItemCdConst.KensaHandanSinkei, ItemCdConst.KensaHandanSinkeiCancel),
            //        // ラジオアイソトープ検査判断料
            //        (new int[]{15}, ItemCdConst.KensaHandanRadio, ItemCdConst.KensaHandanRadioCancel),
            //        // 遺伝子関連・染色体検査判断料
            //        (new int[]{17}, ItemCdConst.KensaHandanIdensi, ItemCdConst.KensaHandanIdensiCancel),
            //        // 病理判断料
            //        (new int[]{40,41,42}, ItemCdConst.KensaHandanByori, ItemCdConst.KensaHandanByoriCancel)

            //    };

            for (int i = 0; i < KensaHandanConst.KensaHandanList.Count; i++)
            {
                if (_common.Odr.ExistKensaHandanGrpKbn(KensaHandanConst.KensaHandanList[i].kbn))
                {
                    List<OdrDtlTenModel> filteredOdrDtls = _common.Odr.FilterKensaHandanGrpKbn(KensaHandanConst.KensaHandanList[i].kbn);
                    if (filteredOdrDtls.Any())
                    {
                        if (_common.Odr.ExistOdrDetailByItemCd(KensaHandanConst.KensaHandanList[i].santeiItem) == false)
                        {
                            if (_common.Odr.ExistOdrDetailByItemCd(KensaHandanConst.KensaHandanList[i].cancelItem) == false)
                            {
                                int hokenPid = -1;
                                int hokenId = -1;
                                int santeiKbn = -1;

                                (hokenPid, hokenId, santeiKbn) = GetPid(filteredOdrDtls);

                                if (KensaHandanConst.KensaHandanList[i].kbn.Contains(KensaHandanConst.Noha))
                                {
                                    // 脳波検査判断料
                                    if (_common.CheckSanteiTerm(NohaHandanryo, _common.sinDate / 100 * 100 + 1, _common.sinDate))
                                    {
                                        // 脳波検査判断料算定済み
                                        _common.AppendCalcLog(2, string.Format(FormatConst.SanteiJyogen + FormatConst.DontSantei, "脳波検査判断料", 1, "1月"));
                                    }
                                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KensaHandanNoha1))
                                    {
                                        // 脳波１オーダーあり
                                    }
                                    else
                                    {
                                        //int hokenPid = -1;
                                        //int hokenId = -1;
                                        //int santeiKbn = -1;

                                        //(hokenPid, hokenId, santeiKbn) = GetPid(filteredOdrDtls);

                                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, santeiKbn);
                                        _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.Kensa, cdKbn: _common.GetCdKbn(santeiKbn, "D"));

                                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaHandanNoha2, autoAdd: 1);
                                    }

                                }
                                // 算定回数チェック
                                else if (_common.CheckSanteiKaisu(KensaHandanConst.KensaHandanList[i].santeiItem, santeiKbn, 1) == 2)
                                {
                                    // 算定回数マスタのチェックにより算定不可
                                }
                                else
                                {
                                    // 判断料を算定するオーダーが存在するとき
                                    // Rpと行為を追加

                                    //int hokenPid = -1;
                                    //int hokenId = -1;
                                    //int santeiKbn = -1;

                                    // コード区分
                                    string cdKbn = "D";
                                    if (KensaHandanConst.KensaHandanList[i].santeiItem == ItemCdConst.KensaHandanByori)
                                    {
                                        // 病理の場合は、N
                                        cdKbn = "N";
                                    }

                                    //(hokenPid, hokenId, santeiKbn) = GetPid(filteredOdrDtls);

                                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, santeiKbn);
                                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.Kensa, cdKbn: _common.GetCdKbn(santeiKbn, cdKbn));

                                    _common.Wrk.AppendNewWrkSinKouiDetail(KensaHandanConst.KensaHandanList[i].santeiItem, autoAdd: 1);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// まるめ検査項目の処理
        /// </summary>
        private void Marume()
        {
            // 項目数によるまるめ
            // HOKATU_KENSA が 1,2,3,5,6,7,9,10,12 のもの
            MarumeByCount();

            // 内分泌負荷試験
            // HOKATU_KENSA が 8 のもの
            MarumeNaibunFuka();

            // IGE・HRT
            // HOKATU_KENSA が 11 のもの
            MarumeIge();

        }

        private void MarumeByCount()
        {
            if (_common.hokenKbn == HokenSyu.Kenpo && _common.syosaiHokenKbn == HokenKbn.Kokho)
            {
                // 国保
                if (_systemConfigProvider.GetKensaMarumeBuntenKokuho() == 1)
                {
                    MarumeByCountBuntenDevide();
                }
                else
                {
                    MarumeByCountBuntenBundle();
                }
            }
            else
            {
                // 国保以外
                if (_systemConfigProvider.GetKensaMarumeBuntenSyaho() == 1)
                {
                    MarumeByCountBuntenDevide();
                }
                else
                {
                    MarumeByCountBuntenBundle();
                }
            }
        }
        /// <summary>
        /// 項目数による検査まるめ処理（分点していてもまとめる）
        /// HOKATU_KENSA が 1,2,3,5,6,7,9,10,12 のもの
        /// </summary>
        private void MarumeByCountBuntenBundle()
        {
            // まるめ検査チェック用データ
            // 包括検査区分, List(項目数範囲（最小,最大）,算定項目)
            //List<(int kbn, List<(int min, int max, string item)> ranges)> conMarume =
            //    new List<(int, List<(int, int, string)>)>
            //    {
            //        // 生化学検査（Ｉ）
            //        (HokatuKensaConst.Seika1, new List<(int, int, string)>
            //            {
            //                (5, 7, ItemCdConst.KensaMarumeSeika5_7),
            //                (8, 9, ItemCdConst.KensaMarumeSeika8_9),
            //                (10, 999, ItemCdConst.KensaMarumeSeika10)
            //            }),
            //        // 内分泌学検査
            //        (HokatuKensaConst.Naibunpitu, new List<(int, int, string)>
            //            {
            //                (3, 5, ItemCdConst.KensaMarumeNaibunpitu3_5),
            //                (6, 7, ItemCdConst.KensaMarumeNaibunpitu6_7),
            //                (8, 999, ItemCdConst.KensaMarumeNaibunpitu8)
            //            }),
            //        // 肝炎ウイルス関連検査
            //        (HokatuKensaConst.Kanen, new List<(int, int, string)>
            //            {
            //                (3, 3, ItemCdConst.KensaMarumeKanen3),
            //                (4, 4, ItemCdConst.KensaMarumeKanen4),
            //                (5, 999, ItemCdConst.KensaMarumeKanen5)
            //            }),
            //        // 腫瘍マーカー精密検査
            //        (HokatuKensaConst.Syuyo, new List<(int, int, string)>
            //            {
            //                (2, 2, ItemCdConst.KensaMarumeSyuyou2),
            //                (3, 3, ItemCdConst.KensaMarumeSyuyou3),
            //                (4, 999, ItemCdConst.KensaMarumeSyuyou4)
            //            }),
            //        // 出血・凝固検査
            //        (HokatuKensaConst.Syukketu, new List<(int, int, string)>
            //            {
            //                (3, 4, ItemCdConst.KensaMarumeSyukketu3_4),
            //                (5, 999, ItemCdConst.KensaMarumeSyukketu5)
            //            }),
            //        // 自己抗体検査
            //        (HokatuKensaConst.JikoKoutai, new List<(int, int, string)>
            //            {
            //                (2, 2, ItemCdConst.KensaMarumeJikoKoutai2),
            //                (3, 999, ItemCdConst.KensaMarumeJikoKoutai3)
            //            }),
            //        // ウイルス抗体価（定性・半定量・定量） 
            //        (HokatuKensaConst.Virus, new List<(int, int, string)>
            //            {
            //                (8, 999, ItemCdConst.KensaMarumeVirus8)
            //            }),
            //        // グロブリンクラス別ウイルス抗体価 
            //        (HokatuKensaConst.Globulin, new List<(int, int, string)>
            //            {
            //                (2, 999, ItemCdConst.KensaMarumeGlobulin2)
            //            }),
            //        // 悪性腫瘍遺伝子検査
            //        (HokatuKensaConst.Akusei, new List<(int, int, string)>
            //            {
            //                (2, 2, ItemCdConst.KensaMarumeAkusei2),
            //                (3, 999, ItemCdConst.KensaMarumeAkusei3)
            //            }),
            //        // 悪性腫瘍遺伝子検査（容易なもの）2020/04～
            //        (HokatuKensaConst.AkuseiYoui, new List<(int, int, string)>
            //            {
            //                (2, 2, ItemCdConst.KensaMarumeAkusei2),
            //                (3, 3, ItemCdConst.KensaMarumeAkusei3),
            //                (4, 999, ItemCdConst.KensaMarumeAkusei4)
            //            }),
            //        // 悪性腫瘍遺伝子検査（複雑なもの）2020/04～
            //        (HokatuKensaConst.AkuseiFukuzatu, new List<(int, int, string)>
            //            {
            //                (2, 2, ItemCdConst.KensaMarumeAkuseiFukuzatu2),
            //                (3, 999, ItemCdConst.KensaMarumeAkuseiFukuzatu3)
            //            })
            //    };

            for (int santeiKbnIndex = 0; santeiKbnIndex <= 1; santeiKbnIndex++)
            {
                // まるめ検査項目取得
                List<(List<OdrDtlTenModel> odrDtls, int minIndex, int itemCnt)> marumels = new List<(List<OdrDtlTenModel>, int, int)>();
                List<OdrDtlTenModel> retOdrDtl;
                int retMin;
                int retCount;

                //int findCount;
                string itemCd;

                // 包括逓減区分
                List<(int hokatuKbn, string cdKbn, int cdKbnNo, int cdEdaNo, int cdKouNo)> santeiTeigenKbn =
                        new List<(int hokatuKbn, string cdKbn, int cdKbnNo, int cdEdaNo, int cdKouNo)>();

                int santeiKbn = 0;
                if (santeiKbnIndex == 1)
                {
                    santeiKbn = 2;
                }
                //List<int> houkatuKensas = _common.Odr.FindHoukatuKensa(hokenPidList[i].hokenPid, hokenPidList[i].santeiKbn);
                //foreach((int kbn, List<(int min, int max, string item)> ranges) in HokatuKensaConst.HoukatuKensaList.FindAll(p=>houkatuKensas.Contains(p.kbn)))
                foreach ((int kbn, List<(int min, int max, string item)> ranges) in HokatuKensaConst.HoukatuKensaList)
                {
                    int itemCount = _common.Odr.CountHokatuKensa(kbn, santeiKbn);
                    bool isMarume = false;

                    for (int k = 0; k < ranges.Count; k++)
                    {
                        if (itemCount >= ranges[k].min && itemCount <= ranges[k].max)
                        {
                            // まるめ条件に合う場合
                            isMarume = true;
                            break;
                        }
                    }

                    List<(int hokenPid, int hokenId, int santeiKbn, string sortKey)> hokenPidList = new List<(int, int, int, string)>();
                    foreach ((int hokenPid, int hokenId, int santeiKbn) hokenPid in _common.Odr.GetKensaHokenPidList(kbn, santeiKbn))
                    {
                        hokenPidList.Add((hokenPid.hokenPid, hokenPid.hokenId, hokenPid.santeiKbn, _common.GetSortKey(hokenPid.hokenPid)));
                    }
                    hokenPidList = hokenPidList.OrderBy(p => p.sortKey).ThenBy(p => p.santeiKbn).ToList();

                    if (hokenPidList.Any())
                    {
                        bool first = true;

                        for (int i = 0; i < hokenPidList.Count; i++)
                        {
                            if (isMarume == false)
                            {
                                first = true;
                            }

                            //findCount = 0;
                            marumels.Clear();

                            // まるめ対象の検査を検索
                            (retOdrDtl, retMin, retCount) =
                                _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                                    kbn, hokenPidList[i].hokenPid, hokenPidList[i].santeiKbn);

                            if (retMin < 0)
                            {
                                continue;
                            }
                            else
                            {
                                // -------------- ↓ もしかすると、背反後にカウントした方がいいかもしれない、その場合ここはコメントアウト ----------------------------//

                                // 対象項目の数をカウント
                                while (retMin >= 0)
                                {
                                    //findCount++;
                                    marumels.Add((retOdrDtl, retMin, retCount));

                                    // オーダーから削除
                                    _common.Odr.odrDtlls.RemoveRange(retMin, retCount);

                                    // 他にないかチェック
                                    (retOdrDtl, retMin, retCount) =
                                        _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                                            kbn, hokenPidList[i].hokenPid, hokenPidList[i].santeiKbn);

                                }

                                //// まるめになる数かどうかチェック
                                //itemCd = "";
                                //for (int k = 0; k < conMarume[j].ranges.Count; k++)
                                //{
                                //    if (findCount >= conMarume[j].ranges[k].min && findCount <= conMarume[j].ranges[k].max)
                                //    {
                                //        // まるめ条件に合う場合、ITEM_CDを記憶する
                                //        itemCd = conMarume[j].ranges[k].item;
                                //        break;
                                //    }
                                //}

                                // -------------- ↑ もしかすると、背反後にカウントした方がいいかもしれない、その場合ここはコメントアウト ----------------------------//

                                // 算定処理
                                // Rpと行為を追加
                                if (first)
                                {
                                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, hokenPidList[i].santeiKbn);
                                    first = false;
                                }
                                _common.Wrk.AppendNewWrkSinKoui(hokenPidList[i].hokenPid, hokenPidList[i].hokenId, ReceSyukeisaki.Kensa, hokatuKensa: kbn, cdKbn: _common.GetCdKbn(hokenPidList[i].santeiKbn, "D"));

                                for (int k = 0; k < marumels.Count; k++)
                                {
                                    foreach (OdrDtlTenModel odrDtl in marumels[k].odrDtls)
                                    {
                                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0) != 2 &&
                                            _common.CheckAge(odrDtl) != 2)
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                            if (odrDtl.TeigenKbn == 1)
                                            {
                                                // 逓減項目
                                                Teigen(odrDtl, santeiTeigenKbn, isMarume);
                                                santeiTeigenKbn.Add((odrDtl.HokatuKbn, odrDtl.CdKbn, odrDtl.CdKbnno, odrDtl.CdEdano, odrDtl.CdKouno));
                                            }

                                        }
                                        else
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                        }
                                        //if (itemCd != "" && odrDtl.MasterSbt == "S")
                                        //{
                                        //    // まるめの場合、書式区分を1に設定（紙レセ列挙）
                                        //    _common.Wrk.wrkSinKouiDetails.Last().FmtKbn = 1;
                                        //}
                                    }

                                    // オーダーから削除
                                    // _common.Odr.odrDtlls.RemoveRange(marumels[j].minIndex, marumels[j].itemCnt);
                                }

                                // -------------- ↓ もしかすると、背反後にカウントした方がいいかもしれない、その場合ここはコメントアウト ----------------------------//

                                //// まるめ項目を追加する
                                //if (itemCd != "")
                                //{
                                //    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1, isNodspRece: 1);
                                //    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, "（" + CIUtil.ToWide(findCount.ToString()) + "項目）", fmtKbn: 1);
                                //}

                                // -------------- ↑ もしかすると、背反後にカウントした方がいいかもしれない、その場合ここはコメントアウト ----------------------------//

                                // 自動発生コメントは下部に持ってくる
                                for (int k = 0; k < marumels.Count; k++)
                                {
                                    foreach (OdrDtlTenModel odrDtl in marumels[k].odrDtls)
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, marumels[k].odrDtls);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 項目数による検査まるめ処理（分点ごとに分ける）
        /// HOKATU_KENSA が 1,2,3,5,6,7,9,10,12 のもの
        /// </summary>
        private void MarumeByCountBuntenDevide()
        {
            // まるめ検査項目取得
            List<(List<OdrDtlTenModel> odrDtls, int minIndex, int itemCnt)> marumels = new List<(List<OdrDtlTenModel>, int, int)>();
            List<OdrDtlTenModel> retOdrDtl;
            int retMin;
            int retCount;

            //int findCount;
            string itemCd;

            // 包括逓減区分
            List<(int hokatuKbn, string cdKbn, int cdKbnNo, int cdEdaNo, int cdKouNo)> santeiTeigenKbn =
                    new List<(int hokatuKbn, string cdKbn, int cdKbnNo, int cdEdaNo, int cdKouNo)>();

            for (int i = 0; i < _common.Odr.HokenPidList.Count; i++)
            {
                List<int> houkatuKensas = _common.Odr.FindHoukatuKensa(_common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].santeiKbn);
                foreach ((int kbn, List<(int min, int max, string item)> ranges) in HokatuKensaConst.HoukatuKensaList.FindAll(p => houkatuKensas.Contains(p.kbn)))
                {
                    int itemCount = _common.Odr.CountHokatuKensa(kbn, _common.Odr.HokenPidList[i].santeiKbn);
                    bool isMarume = false;

                    for (int k = 0; k < ranges.Count; k++)
                    {
                        if (itemCount >= ranges[k].min && itemCount <= ranges[k].max)
                        {
                            // まるめ条件に合う場合
                            isMarume = true;
                            break;
                        }
                    }

                    //findCount = 0;
                    marumels.Clear();

                    // まるめ対象の検査を検索
                    (retOdrDtl, retMin, retCount) =
                        _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                            kbn, _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].santeiKbn);

                    if (retMin < 0)
                    {
                        continue;
                    }
                    else
                    {
                        // -------------- ↓ もしかすると、背反後にカウントした方がいいかもしれない、その場合ここはコメントアウト ----------------------------//

                        // 対象項目の数をカウント
                        while (retMin >= 0)
                        {
                            //findCount++;
                            marumels.Add((retOdrDtl, retMin, retCount));

                            // オーダーから削除
                            _common.Odr.odrDtlls.RemoveRange(retMin, retCount);

                            // 他にないかチェック
                            (retOdrDtl, retMin, retCount) =
                                _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                                    kbn, _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].santeiKbn);

                        }

                        // -------------- ↑ もしかすると、背反後にカウントした方がいいかもしれない、その場合ここはコメントアウト ----------------------------//

                        // 算定処理
                        // Rpと行為を追加
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, _common.Odr.HokenPidList[i].santeiKbn);
                        _common.Wrk.AppendNewWrkSinKoui(_common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].hokenId, ReceSyukeisaki.Kensa, hokatuKensa: kbn, cdKbn: _common.GetCdKbn(_common.Odr.HokenPidList[i].santeiKbn, "D"));

                        for (int k = 0; k < marumels.Count; k++)
                        {
                            foreach (OdrDtlTenModel odrDtl in marumels[k].odrDtls)
                            {
                                if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0) != 2 &&
                                    _common.CheckAge(odrDtl) != 2)
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    if (odrDtl.TeigenKbn == 1)
                                    {                                               
                                        // 逓減項目
                                        Teigen(odrDtl, santeiTeigenKbn, isMarume);
                                        santeiTeigenKbn.Add((odrDtl.HokatuKbn, odrDtl.CdKbn, odrDtl.CdKbnno, odrDtl.CdEdano, odrDtl.CdKouno));
                                    }

                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                }
                                //if (itemCd != "" && odrDtl.MasterSbt == "S")
                                //{
                                //    // まるめの場合、書式区分を1に設定（紙レセ列挙）
                                //    _common.Wrk.wrkSinKouiDetails.Last().FmtKbn = 1;
                                //}
                            }

                            // オーダーから削除
                            // _common.Odr.odrDtlls.RemoveRange(marumels[j].minIndex, marumels[j].itemCnt);
                        }

                        // 自動発生コメントは下部に持ってくる
                        for (int k = 0; k < marumels.Count; k++)
                        {
                            foreach (OdrDtlTenModel odrDtl in marumels[k].odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, marumels[k].odrDtls);
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 内分泌負荷試験
        /// HOKATU_KENSA が 8 のもの
        /// 月3600点の調整が必要だが、OdrToWrkでは調整せず、項目をまとめるだけに留める
        /// </summary>
        private void MarumeNaibunFuka()
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex;
            int itemCnt;

            for (int i = 0; i < _common.Odr.HokenPidList.Count; i++)
            {
                //// Rpと行為を追加
                //_common.Wrk.AppendNewWrkSinRpInf( ReceKouiKbn.Kensa, ReceSinId.Kensa, _common.Odr.HokenPidList[i].santeiKbn);
                //_common.Wrk.AppendNewWrkSinKoui(
                //    _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].hokenId, ReceSyukeisaki.Kensa, hokatuKensa: HokatuKensaConst.NaibunpituFuka, 
                //    cdKbn: _common.GetCdKbn(_common.Odr.HokenPidList[i].santeiKbn, "D"));

                // まるめ対象の検査を検索
                (odrDtls, minIndex, itemCnt) =
                    _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                        HokatuKensaConst.NaibunpituFuka, _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].santeiKbn);

                if (minIndex >= 0)
                {
                    // Rpと行為を追加
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, _common.Odr.HokenPidList[i].santeiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(
                        _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].hokenId, ReceSyukeisaki.Kensa, hokatuKensa: HokatuKensaConst.NaibunpituFuka,
                        cdKbn: _common.GetCdKbn(_common.Odr.HokenPidList[i].santeiKbn, "D"));

                    // 対象項目を算定
                    double totalTen = _common.Sin.GetNaibunpituTotalCost();

                    while (minIndex >= 0)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0) == 2 ||
                                _common.CheckAge(odrDtl) == 2)
                            {
                                // 算定回数または年齢上限を超える
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                            }
                            else if (totalTen >= 3600)
                            {
                                // すでに当月3600点を算定している場合、算定不可
                                _common.AppendCalcLog(2, $"'{odrDtl.ItemName}' は、月3,600点を超えるため、算定できません。");
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                // 年齢加算自動算定
                                _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.FilterOdrDetailByRpNo(odrDtl.RpNo, odrDtl.RpEdaNo));
                            }

                        }

                        // オーダーから削除
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);

                        (odrDtls, minIndex, itemCnt) =
                            _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                                HokatuKensaConst.NaibunpituFuka, _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].santeiKbn);
                    }
                }
            }
        }

        /// <summary>
        /// IGE・HRT
        /// HOKATU_KENSA が 11 のもの
        /// </summary>
        private void MarumeIge()
        {
            // まるめ検査項目取得
            List<(List<OdrDtlTenModel> odrDtls, int minIndex, int itemCnt)> marumels = new List<(List<OdrDtlTenModel>, int, int)>();
            List<OdrDtlTenModel> retOdrDtl;
            int retMin;
            int retCount;

            int igeCount;
            int hrtCount;

            for (int i = 0; i < _common.Odr.HokenPidList.Count; i++)
            {

                OdrDtlTenModel igeOdrDtl = null;
                OdrDtlTenModel hrtOdrDtl = null;

                igeCount = 0;
                hrtCount = 0;

                marumels.Clear();

                // まるめ対象の検査を検索
                (retOdrDtl, retMin, retCount) =
                    _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                        HokatuKensaConst.IgeHrt, _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].santeiKbn);

                // 対象項目の数をカウント
                while (retMin >= 0)
                {
                    marumels.Add((retOdrDtl, retMin, retCount));

                    foreach (OdrDtlTenModel odrDtl in retOdrDtl)
                    {
                        if (odrDtl.HokatuKensa == HokatuKensaConst.IgeHrt)
                        {
                            if (odrDtl.SanteiItemCd == ItemCdConst.KensaIge)
                            {
                                igeCount += odrDtl.Suryo == 0 ? 1 : (int)odrDtl.Suryo;

                                if (odrDtl.OdrItemCd == ItemCdConst.KensaIge)
                                {
                                    igeOdrDtl = odrDtl;
                                }
                            }
                            else if (odrDtl.SanteiItemCd == ItemCdConst.KensaHrt)
                            {
                                hrtCount += odrDtl.Suryo == 0 ? 1 : (int)odrDtl.Suryo;

                                if (odrDtl.OdrItemCd == ItemCdConst.KensaHrt)
                                {
                                    hrtOdrDtl = odrDtl;
                                }
                            }
                        }
                    }

                    // オーダーから削除
                    _common.Odr.odrDtlls.RemoveRange(retMin, retCount);

                    (retOdrDtl, retMin, retCount) =
                        _common.Odr.FilterOdrDetailRangeByHokatuKensa(
                            HokatuKensaConst.IgeHrt, _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].santeiKbn);
                }

                // Ige算定処理
                if (igeCount > 0)
                {
                    if (igeCount > 13) { igeCount = 13; }

                    Santei(
                        igeOdrDtl, ItemCdConst.KensaIge, ItemCdConst.KensaIge, igeCount,
                        _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].hokenId, _common.Odr.HokenPidList[i].santeiKbn);
                }

                // Hrt算定処理
                if (hrtCount > 0)
                {
                    string hrtSanteiItemCd = ItemCdConst.KensaHrt;

                    if (hrtCount >= 9)
                    {
                        hrtSanteiItemCd = ItemCdConst.KensaHrt9;
                        hrtCount = 1;
                    }

                    Santei(
                        igeOdrDtl, ItemCdConst.KensaHrt, hrtSanteiItemCd, hrtCount,
                        _common.Odr.HokenPidList[i].hokenPid, _common.Odr.HokenPidList[i].hokenId, _common.Odr.HokenPidList[i].santeiKbn);
                }
            }

            #region Local Method

            // IGE/HRT項目を算定する
            void Santei(OdrDtlTenModel AodrDtl, string AstdItemCd, string AsanteiItemCd, int AitemCnt, int AhokenPid, int AhokenId, int AsanteiKbn)
            {
                // Rpと行為を追加
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Kensa, ReceSinId.Kensa, AsanteiKbn);
                _common.Wrk.AppendNewWrkSinKoui(AhokenPid, AhokenId, ReceSyukeisaki.Kensa, hokatuKensa: HokatuKensaConst.IgeHrt, cdKbn: _common.GetCdKbn(AsanteiKbn, "D"));

                if (AodrDtl != null)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(AodrDtl, _common.Odr.GetOdrCmt(AodrDtl));
                }
                else
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(AsanteiItemCd, autoAdd: 1);
                }

                // 数量に項目数をセット
                _common.Wrk.wrkSinKouiDetails.Last().Suryo = AitemCnt;

                for (int j = 0; j < marumels.Count; j++)
                {
                    if (marumels[j].odrDtls.Any(p => p.SanteiItemCd == AstdItemCd))
                    {
                        foreach (OdrDtlTenModel odrDtl in marumels[j].odrDtls)
                        {
                            if (odrDtl.OdrItemCd != AstdItemCd && (odrDtl.OdrItemCd.StartsWith("IGE") || odrDtl.IsComment))
                            {
                                // IGE項目またはコメント項目のみ。KNはコメント付加しない。
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                if (odrDtl.MasterSbt == "S")
                                {
                                    // まるめの場合、書式区分を1に設定（紙レセ列挙）
                                    // フリーコメント扱いにする
                                    _common.Wrk.wrkSinKouiDetails.Last().RecId = "CO";
                                    _common.Wrk.wrkSinKouiDetails.Last().ItemSbt = 1;
                                    _common.Wrk.wrkSinKouiDetails.Last().Suryo = 1;
                                    _common.Wrk.wrkSinKouiDetails.Last().FmtKbn = 1;
                                    _common.Wrk.wrkSinKouiDetails.Last().ItemCd = ItemCdConst.CommentFree;
                                    _common.Wrk.wrkSinKouiDetails.Last().ItemName = odrDtl.ReceName;
                                    _common.Wrk.wrkSinKouiDetails.Last().UnitCd = 0;
                                    _common.Wrk.wrkSinKouiDetails.Last().UnitName = "";
                                    _common.Wrk.wrkSinKouiDetails.Last().CmtOpt = _common.Wrk.wrkSinKouiDetails.Last().ItemName;
                                }
                            }
                        }
                    }

                }
            }
            #endregion
        }

        /// <summary>
        /// 逓減検査処理
        /// </summary>
        /// <param name="rpNo"></param>
        /// <param name="rpEdaNo"></param>
        /// <param name="hokatuKbn"></param>
        private void Teigen(OdrDtlTenModel odrDtl, List<(int hokatuKbn, string cdKbn, int cdKbnNo, int cdEdaNo, int cdKouNo)> santeiTeigenKbn, bool isMarume = false)
        {
            List<string> teigennls =
                new List<string>
                {
                    // 検査逓減
                    ItemCdConst.KensaTeigen,
                    // 検査逓減取り消し
                    ItemCdConst.KensaTeigenCancel
                };

            bool santei = false;

            if (_common.Odr.FilterOdrDetailRpByItemCd(odrDtl.RpNo, odrDtl.RpEdaNo, teigennls).Any() == false)
            {
                //if (santeiTeigenKbn.Any(p=>p.hokatuKbn == odrDtl.HokatuKbn) || 
                //    _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, odrDtl.HokatuKbn) > 0)
                //{
                if (odrDtl.HokatuKbn == 40)
                {

                    //if (_common.SanteiCountByHokatuKbn(
                    //        _common.SinFirstDateOfMonth, _common.sinDate,
                    //        odrDtl.HokatuKbn, odrDtl.CdKbn, odrDtl.CdKbnno, odrDtl.CdEdano, odrDtl.CdKouno) > 0)
                    //{
                    //    // すでに同一区分の検査を算定している場合は逓減
                    //    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaTeigen, autoAdd: 1);
                    //}
                    if (_common.SanteiCountByHokatuKbnTerm(
                            _common.SinFirstDateOfMonth, _common.sinDate,
                            odrDtl.HokatuKbn, odrDtl.CdKbn, odrDtl.CdKbnno, odrDtl.CdEdano, odrDtl.CdKouno) > 0)
                    {
                        // すでに同一区分の検査を算定している場合は逓減
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaTeigen, autoAdd: 1);
                        santei = true;
                    }
                    else if (_common.SanteiCountByHokatuKbnToday(
                        _common.sinDate,
                        odrDtl.HokatuKbn, odrDtl.CdKbn, odrDtl.CdKbnno, odrDtl.CdEdano, odrDtl.CdKouno) > 0)
                    {
                        // 当日、すでに同一区分の検査している場合は逓減
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaTeigen, autoAdd: 1);
                        //_common.Wrk.wrkSinKouiDetails.Last().TeigenTargetInRaiin = true;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdKbn = odrDtl.CdKbn;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdKbnno = odrDtl.CdKbnno;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdEdano = odrDtl.CdEdano;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdKouno = odrDtl.CdKouno;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenHokatuKbn = odrDtl.HokatuKbn;
                        santei = true;
                    }
                    else if (santeiTeigenKbn.Any(p =>
                            p.hokatuKbn == odrDtl.HokatuKbn &&
                            p.cdKbn == odrDtl.CdKbn &&
                            p.cdKbnNo == odrDtl.CdKbnno &&
                            p.cdEdaNo == odrDtl.CdEdano &&
                            p.cdKouNo == odrDtl.CdKouno))
                    {
                        // すでに同一区分の検査を算定している場合は逓減
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaTeigen, autoAdd: 1);
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenTargetInRaiin = true;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdKbn = odrDtl.CdKbn;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdKbnno = odrDtl.CdKbnno;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdEdano = odrDtl.CdEdano;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenCdKouno = odrDtl.CdKouno;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenHokatuKbn = odrDtl.HokatuKbn;
                        santei = true;
                    }
                    else if (santeiTeigenKbn.Any(p => p.hokatuKbn == odrDtl.HokatuKbn) ||
                        _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, odrDtl.HokatuKbn) > 0)
                    {
                        // 警告
                        _common.AppendCalcLog(1, "'超音波検査' が、月２回以上算定されているため、逓減になる可能性があります。");
                    }
                }
                else
                {
                    if (_common.SanteiCountByHokatuKbnTerm(
                        _common.SinFirstDateOfMonth, _common.sinDate, odrDtl.HokatuKbn) > 0)
                    {
                        // すでに同一区分の検査を算定している場合は逓減
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaTeigen, autoAdd: 1);
                        santei = true;
                    }
                    else if (_common.SanteiCountByHokatuKbnToday(odrDtl.HokatuKbn) > 0)
                    {
                        // 当日、すでに同一区分の検査している場合は逓減
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaTeigen, autoAdd: 1);
                        //_common.Wrk.wrkSinKouiDetails.Last().TeigenTargetInRaiin = true;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenHokatuKbn = odrDtl.HokatuKbn;
                        santei = true;
                    }
                    else if (santeiTeigenKbn.Any(p =>
                        p.hokatuKbn == odrDtl.HokatuKbn))
                    {
                        // すでに同一区分の検査を算定している場合は逓減
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaTeigen, autoAdd: 1);
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenTargetInRaiin = true;
                        _common.Wrk.wrkSinKouiDetails.Last().TeigenHokatuKbn = odrDtl.HokatuKbn;
                        santei = true;

                    }
                }

                if (santei && isMarume)
                {
                    // ＥＧＦＲ対応
                    // まるめの場合同じRPに別項目もあるので、逓減項目の直後に逓減が記載されるように調整が必要
                    _common.Wrk.wrkSinKouiDetails.Last().Kokuji1 = odrDtl.Kokuji1;
                    _common.Wrk.wrkSinKouiDetails.Last().Kokuji2 = odrDtl.Kokuji2;
                    _common.Wrk.wrkSinKouiDetails.Last().TusokuAge = odrDtl.TusokuAge;
                    if (odrDtl.TyuCd != "0") _common.Wrk.wrkSinKouiDetails.Last().TyuCd = odrDtl.TyuCd;
                    _common.Wrk.wrkSinKouiDetails.Last().TyuSeq = odrDtl.TyuSeq;
                }
                //}
            }
        }

        /// <summary>
        /// 内視鏡時間外
        /// </summary>
        private void NaisikyoJikan()
        {
            if (_common.Odr.FilterOdrDetailByItemCd(NaisiJikanls).Any() == false)
            {
                if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KensaNaisiJikanCancel) == false)
                {
                    if (_common.jikan == JikanConst.JikanGai)
                    {
                        // 時間外
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaNaisiJikangai, autoAdd: 1);
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        // 休日
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaNaisiKyujitu, autoAdd: 1);
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        // 深夜
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.KensaNaisiSinya, autoAdd: 1);
                    }
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
                OdrKouiKbnConst.KensaMin,
                OdrKouiKbnConst.KensaMax,
                ReceKouiKbn.Kensa,
                ReceSinId.Kensa,
                ReceSyukeisaki.Kensa,
                "JS");
        }
    }
}
