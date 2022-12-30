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
    class IkaCalculateOdrToWrkSyotiViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkSyotiViewModel(IkaCalculateCommonDataViewModel common,
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

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.SyotiMin, OdrKouiKbnConst.SyotiMax))
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

            bool rosaiSippu150 = false;

            // 通常算定処理
            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            // 処置のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.SyotiMin, OdrKouiKbnConst.SyotiMax);

            if (filteredOdrInf.Any())
            {
                if (_common.IsRosai)
                {
                    // 労災の場合、湿布の合算をチェック
                    rosaiSippu150 = CheckRosaiSippu(filteredOdrInf);
                }

                // 人工腎臓の算定有無
                bool orderJinkoJinzo = false;

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);
                    if(filteredOdrDtl.Any(p=>p.IsJinkoJinzo))
                    {
                        orderJinkoJinzo = true;
                        break;
                    }
                }

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {                    

                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any())
                    {
                        // 初回、必ずRpと行為のレコードを用意
                        string cdKbn = "J";
                        string syukeiSaki = ReceSyukeisaki.Syoti;

                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syoti, ReceSinId.Syoti, odrInf.SanteiKbn);
                        cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, "J");
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki: syukeiSaki, cdKbn: cdKbn, odrRpNo: odrInf.RpNo);

                        // 時間加算を算定するかどうか
                        bool timeKasan = false;
                        // このRpの診療行為の合計点数
                        double rpTen = 0;

                        // 労災で消炎鎮痛（湿布）の合計が150点を超える場合で、
                        // このRpに消炎鎮痛（湿布）が存在する場合、trueとし、
                        // 時間外加算算定対象Rpとする
                        bool rosaiSippuTimeKasan = rosaiSippu150 && (filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.SyotiSyoenSippu));

                        int firstItem = _common.CheckFirstItemSbtSyoti(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        bool commentSkipFlg = (firstItem != 0);
                        // 最初のコメント以外の項目であることを示すフラグ
                        bool firstSinryoKoui = true;

                        // このrpに人工腎臓がオーダーされているかどうか
                        bool existJinkoJinzo = false;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            //if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || odrDtl.IsSanso))
                            if (!odrDtl.IsJihi && (odrDtl.IsSorCommentItem(commentSkipFlg) || _common.IsSelectComment(odrDtl.ItemCd)))
                            {
                                // 診療行為・コメント・選択式コメント //or 酸素ボンベ

                                // 以降のコメントはこの項目に付随するコメントとみなす
                                if (odrDtl.IsComment == false)
                                {
                                    commentSkipFlg = false;
                                }

                                if (odrDtl.CdKbn == "J" && odrDtl.CdKbnno < 200)
                                {
                                    // 時間外加算対象項目 J000-J200
                                    timeKasan = true;
                                }

                                if (odrDtl.IsKihonKoumoku)
                                {
                                    // 基本項目

                                    if (firstSinryoKoui == true)
                                    {
                                        firstSinryoKoui = false;

                                        if (_common.hokenKbn == HokenSyu.Jibai &&
                                            (odrDtl.ItemCd == ItemCdConst.SyotiYobuKoteitaiKasan ||
                                              odrDtl.ItemCd == ItemCdConst.SyotiKyobuKoteitaiKasan ||
                                              odrDtl.ItemCd == ItemCdConst.SyotiKeibuKoteitaiKasan))
                                        {
                                            // 腰部固定帯加算、胸部固定帯加算、頸部固定帯加算について、
                                            // 自賠のときは、薬剤に集計
                                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SyotiYakuzai;
                                        }
                                    }
                                    else
                                    {
                                        //最初以外の基本項目が来たらRpを分ける　
                                        //※最初にコメントが入っていると困るのでこういう処理にする

                                        //※処置は同一Rpにオーダーされていれば、一連の行為とみなすようにしておく
                                        //if (odrDtl.Kokuji2 != "7")
                                        //{
                                        //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syoti, ReceSinId.Syoti, odrInf.SanteiKbn);
                                        //}
                                        //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Syoti);
                                        cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, "J");
                                        syukeiSaki = ReceSyukeisaki.Syoti;

                                        if(_common.hokenKbn == HokenSyu.Jibai &&
                                            ( odrDtl.ItemCd == ItemCdConst.SyotiYobuKoteitaiKasan ||
                                              odrDtl.ItemCd == ItemCdConst.SyotiKyobuKoteitaiKasan ||
                                              odrDtl.ItemCd == ItemCdConst.SyotiKeibuKoteitaiKasan) )
                                        {
                                            // 腰部固定帯加算、胸部固定帯加算、頸部固定帯加算について、
                                            // 自賠のときは、薬剤に集計
                                            syukeiSaki = ReceSyukeisaki.SyotiYakuzai;
                                        }

                                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki: syukeiSaki, cdKbn: cdKbn, odrRpNo: odrInf.RpNo);
                                    }
                                }

                                //firstSinryoKoui = false;

                                //if (odrDtl.IsSanso)
                                //{
                                //    // 酸素ボンベの場合、補正率を自動算定する
                                //    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                //    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SansoHoseiRitu, autoAdd: 1);
                                //}
                                //else if (!odrDtl.IsComment)
                                if (!odrDtl.IsComment)
                                {
                                    // コメント項目以外
                                    if (odrDtl.IsJinkoJinzo || odrDtl.ItemCd == ItemCdConst.SyotiSyuginasi)
                                    {
                                        // 人工腎臓、または手技なしの場合
                                        // 算定回数上限等で算定不可になったとしても、オーダーしている時点で薬剤包括する必要がある
                                        existJinkoJinzo = true;
                                    }

                                    // 算定回数チェック
                                    if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                                    {
                                        // 算定回数マスタのチェックにより算定不可
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: 1);                                    
                                    }
                                    else if(_common.CheckAge(odrDtl) == 2)
                                    {
                                        // 年齢チェックにより算定不可
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: 1);
                                    }
                                    else
                                    {
                                        // 詳細追加
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                        if (odrDtl.TenMst != null && 
                                            (odrDtl.TenMst.IsSyotiJikangaiTarget))
                                        {
                                            // 
                                            if (!(new List<int> { 5, 6 }.Contains(odrDtl.TenId)))
                                            {
                                                rpTen += (odrDtl.Suryo == 0 ? 1 : odrDtl.Suryo) * odrDtl.Ten;
                                            }
                                            else
                                            {
                                                // 倍率加算の場合
                                                rpTen += _common.Wrk.GetSyoteiTen(_common.Wrk.RpNo, _common.Wrk.SeqNo, odrDtl.Kokuji1) * (odrDtl.Ten / 100);
                                            }
                                            
                                        }

                                        //if (odrDtl.IsJinkoJinzo)
                                        //{
                                        //    jinkoJinzo = true;
                                        //}

                                        // 労災加算チェック
                                        if (_common.IsRosai)
                                        {
                                            rpTen = RosaiSisiKasan(filteredOdrDtl, odrDtl, rpTen);
                                        }

                                        // 年齢加算自動算定
                                        double retTen = _common.AppendNewWrkSinKouiDetailAgeKasanSyoti(odrDtl);

                                        rpTen += retTen;

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
                                commentSkipFlg = true;
                            }
                        }

                        // 時間外加算
                        if (timeKasan)
                        {
                            JikanKasan(rpTen, rosaiSippuTimeKasan);
                        }

                        // 酸素・コメント算定

                        commentSkipFlg = (firstItem != 4);

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Syoti, cdKbn, ref firstSinryoKoui);
                        //cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, "J");
                        //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Syoti, cdKbn: cdKbn);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if (!odrDtl.IsJihi)
                            {
                                if (odrDtl.IsSanso)
                                {
                                    // 酸素
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    //補正率
                                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SansoHoseiRitu, autoAdd: 1);

                                    commentSkipFlg = false;
                                }
                                else if (odrDtl.IsComment && commentSkipFlg == false)
                                {
                                    // コメント
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                }
                                else
                                {
                                    commentSkipFlg = true;
                                }
                            }
                            else
                            {
                                commentSkipFlg = true;
                            }
                        }

                        // 薬剤・コメント算定

                        commentSkipFlg = (firstItem != 1);

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.SyotiYakuzai, cdKbn, ref firstSinryoKoui);
                        //処置は別Rpにするものなのかもしれない・・・ちがう・・・のか？
                        //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syoti, ReceSinId.Syoti, odrInf.SanteiKbn);

                        //cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, "J");
                        //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.SyotiYakuzai, cdKbn: cdKbn);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if (!odrDtl.IsJihi && (odrDtl.IsYorCommentItem(commentSkipFlg)))
                            {
                                // 薬剤・コメント
                                if (orderJinkoJinzo && existJinkoJinzo && new int[] { 1, 2, 3, 4, 5, 6 }.Contains(odrDtl.ChusyaDrugSbt))
                                {
                                    // 人工腎臓が算定される場合、1:透析液　2:血液凝固阻止剤　3:エリスロポエチン　4:ダルベポエチン　5:生理食塩水は包括　6:HIF阻害薬
                                    _common.AppendCalcLog(2, string.Format(FormatConst.HokatuNotSantei, odrDtl.ItemName, "人工腎臓", "同来院"));
                                    commentSkipFlg = true;
                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    commentSkipFlg = false;
                                }
                            }
                            else
                            {
                                commentSkipFlg = true;
                            }
                        }

                        // 特材・コメント算定

                        commentSkipFlg = (firstItem != 2);

                        _common.Wrk.AppendOrUpdateKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.SyotiYakuzai, cdKbn, ref firstSinryoKoui);
                        //処置は別Rpにするものなのかもしれない・・・ちがう・・・のか？
                        //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syoti, ReceSinId.Syoti, odrInf.SanteiKbn);
                        //cdKbn = _common.GetCdKbn(odrInf.SanteiKbn, "J");
                        //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.SyotiYakuzai, cdKbn: cdKbn);

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (_common.IsSelectComment(odrDtl.ItemCd))
                            {
                                // 選択式コメントは手技で対応しているので読み飛ばす
                            }
                            else if (!odrDtl.IsJihi && (odrDtl.IsTorCommentItem(commentSkipFlg) && (odrDtl.IsSanso == false)))
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
            }
        }

        /// <summary>
        /// 労災時、消炎鎮痛湿布の合算が150点以上かどうかチェック
        /// </summary>
        /// <param name="filteredOdrInf"></param>
        /// <returns>true: 150点以上</returns>
        private bool CheckRosaiSippu(List<OdrInfModel> filteredOdrInf)
        {
            bool ret = false;
            double totalSippuTen = 0;

            // 労災の場合、湿布の合算をチェック
            foreach (OdrInfModel odrInf in filteredOdrInf)
            {
                double bairitu = 1;
                int bui = 0;

                List<OdrDtlTenModel> odrDtls = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                if (odrDtls.Any(p => p.ItemCd == ItemCdConst.SyotiSyoenSippu))
                {
                    // このRpに倍率項目が存在するかチェック
                    if (odrDtls.Any(p => p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan))
                    {
                        bairitu = 1.5;
                    }
                    else if (odrDtls.Any(p => p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan2))
                    {
                        bairitu = 2;
                    }
                    else
                    {
                        // 四肢加算項目のオーダーはない
                        if (odrDtls.Any(p => p.BuiKbn == 10))
                        {
                            // 部位あり
                            bui = 10;
                        }
                        else if (odrDtls.Any(p => p.BuiKbn == 3))
                        {
                            // 部位あり
                            bui = 3;
                        }
                    }

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if (odrDtl.ItemCd == ItemCdConst.SyotiSyoenSippu)
                        {
                            if (bui == 10)
                            {
                                if (odrDtl.SisiKbn == 1 || odrDtl.SisiKbn == 3)
                                {
                                    // 四肢が存在する場合、２倍を自動算定
                                    bairitu = 2;
                                }
                                else if (odrDtl.SisiKbn == 2)
                                {
                                    // 四肢が存在する場合、１．５倍を自動算定
                                    bairitu = 1.5;
                                }
                            }
                            else if (bui == 3)
                            {
                                if (odrDtl.SisiKbn == 1 || odrDtl.SisiKbn == 2)
                                {
                                    // 四肢が存在する場合、１．５倍を自動算定
                                    bairitu = 1.5;
                                }
                            }

                            totalSippuTen += odrDtl.Ten * bairitu;
                        }
                    }
                }
            }

            if (totalSippuTen >= 150)
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 労災四肢加算の算定処理
        /// </summary>
        /// <param name="filteredOdrDtl"></param>
        /// <param name="odrDtl"></param>
        /// <param name="rpTen"></param>
        /// <return>Rp合計点数（四肢加算を含めた点数）</return>
        private double RosaiSisiKasan(List<OdrDtlTenModel> filteredOdrDtl, OdrDtlTenModel odrDtl, double rpTen)
        {
            double ret = rpTen;

            if (filteredOdrDtl.Any(p => p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan || p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan2) == false)
            {
                // 四肢加算項目のオーダーはない
                if (filteredOdrDtl.Any(p => p.BuiKbn == 10))
                {
                    if (odrDtl.SisiKbn == 1 || odrDtl.SisiKbn == 3)
                    {
                        // 四肢が存在する場合、２倍を自動算定
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyotiRosaiSisiKasan2, autoAdd: 1);
                        ret += (odrDtl.Suryo == 0 ? 1 : odrDtl.Suryo) * odrDtl.Ten * 1;
                    }
                    else if (odrDtl.SisiKbn == 2)
                    {
                        // 四肢が存在する場合、１．５倍を自動算定
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyotiRosaiSisiKasan, autoAdd: 1);
                        ret += (odrDtl.Suryo == 0 ? 1 : odrDtl.Suryo) * odrDtl.Ten * 0.5;
                    }
                }
                else if (filteredOdrDtl.Any(p => p.BuiKbn == 3))
                {
                    if (odrDtl.SisiKbn == 1 || odrDtl.SisiKbn == 2)
                    {
                        // 四肢が存在する場合、１．５倍を自動算定
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyotiRosaiSisiKasan, autoAdd: 1);
                        ret += (odrDtl.Suryo == 0 ? 1 : odrDtl.Suryo) * odrDtl.Ten * 0.5;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 時間外加算の算定
        /// </summary>
        /// <param name="rpTen">RPの合計点数</param>
        /// <param name="rosaiSippuTimeKasan">労災のとき、消炎鎮痛湿布合計が150点を超える場合、true</param>
        private void JikanKasan(double rpTen, bool rosaiSippuTimeKasan)
        {
            string itemCd = "";
            // 時間外加算

            // 四捨五入
            rpTen = Math.Round(rpTen, MidpointRounding.AwayFromZero);

            if ((rpTen >= 150 || rosaiSippuTimeKasan) && _common.Odr.ExistOdrDetailByItemCd(ItemCdConst.SyotiTimeKasanCancel) == false)
            {
                if (_common.jikan == JikanConst.JikanGai)
                {
                    itemCd = ItemCdConst.SyotiJikangai;
                }
                else if (_common.jikan == JikanConst.Kyujitu)
                {
                    itemCd = ItemCdConst.SyotiKyujitu;
                }
                else if (_common.jikan == JikanConst.Sinya)
                {
                    itemCd = ItemCdConst.SyotiSinya;
                }
            }

            if(itemCd != "")
            {
                _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
            }

        }

        /// <summary>
        /// 自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {
            const string conFncName = nameof(CalculateJihi);

            _common.CalculateJihi(
                OdrKouiKbnConst.SyotiMin,
                OdrKouiKbnConst.SyotiMax,
                ReceKouiKbn.Syoti,
                ReceSinId.Syoti,
                ReceSyukeisaki.Syoti,
                "JS");
         }
    }
}
