using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using EmrCalculateApi.Interface;
using Domain.Constant;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    class IkaCalculateOdrToWrkNoIkaViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkNoIkaViewModel(IkaCalculateCommonDataViewModel common,
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
            _emrLogger.WriteLogStart(this, conFncName, "");

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.Igaku, 999))
            {
                // 保険
                CalculateHoken();
            }

            _common.Wrk.CommitWrkSinRpInf();

            _emrLogger.WriteLogEnd(this, conFncName, "");
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

            //bool firstSinryoKoui = false;
            //bool commentSkipFlg = false;

            // Rpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.Igaku, 999);

            if (filteredOdrInf.Any())
            {
                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any())
                    {

                        // 行為に紐づく詳細を取得
                        //firstSinryoKoui = true;
                        //commentSkipFlg = false;

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(GetReceKouiKbn(odrInf), GetReceSinId(odrInf), 2);

                        // 集計先は、後で内容により変更する
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, GetReceSyukeisaki(odrInf), isNodspRece: 1, cdKbn: "JS", count: odrInf.DaysCnt);

                        int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        bool commentSkipFlg = (firstItem != 0);
                        // 最初のコメント以外の項目であることを示すフラグ
                        bool firstSinryoKoui = true;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (odrDtl.IsSorCommentItem(commentSkipFlg))
                            {
                                // 診療行為・コメント
                                if (!(_common.IsCommentItemCd(odrDtl.ItemCd)))
                                {
                                    if (odrDtl.IsKihonKoumoku || odrInf.OdrKouiKbn == OdrKouiKbnConst.Jihi)
                                    {
                                        if (firstSinryoKoui)
                                        {
                                            firstSinryoKoui = false;
                                        }
                                        else
                                        {
                                            _common.Wrk.AppendNewWrkSinRpInf(GetReceKouiKbn(odrInf), GetReceSinId(odrInf), 2);
                                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, GetReceSyukeisaki(odrInf), isNodspRece: 1, cdKbn: "JS", count: odrInf.DaysCnt);
                                        }
                                    }
                                }
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                if (odrDtl.JihiSbt > 0 && _common.Wrk.wrkSinKouis.Last().JihiSbt <= 0)
                                {
                                    _common.Wrk.wrkSinKouis.Last().JihiSbt = odrDtl.JihiSbt;
                                }
                            }
                        }

                        // 薬剤・コメント算定

                        //commentSkipFlg = false;
                        commentSkipFlg = (firstItem != 1);

                        if (firstSinryoKoui == true)
                        {
                            firstSinryoKoui = false;
                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = GetReceSyukeisakiYakuzai(odrInf);
                            if (odrInf.OdrKouiKbn >= OdrKouiKbnConst.TouyakuMin && odrInf.OdrKouiKbn <= OdrKouiKbnConst.TouyakuMax)
                            {
                                _common.Wrk.wrkSinKouis.Last().InoutKbn = odrInf.InoutKbn;
                            }
                        }
                        else
                        {
                            // 行為を追加する
                            if (odrInf.OdrKouiKbn >= OdrKouiKbnConst.TouyakuMin && odrInf.OdrKouiKbn <= OdrKouiKbnConst.TouyakuMax)
                            {
                                _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, GetReceSyukeisakiYakuzai(odrInf), cdKbn: "JS", count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn);
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, GetReceSyukeisakiYakuzai(odrInf), cdKbn: "JS");
                            }
                        }

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (odrDtl.IsYoho)
                            {
                                if (odrInf.InoutKbn == 1)
                                {
                                    if (_systemConfigProvider.GetOutDrugYohoDsp() == 1)
                                    {
                                        // 院外の場合、コメントとして記録
                                        //string comment = odrDtl.ItemName;
                                        string comment = odrDtl.ReceName;
                                        if (odrInf.OdrKouiKbn == OdrKouiKbnConst.Naifuku || odrInf.OdrKouiKbn == OdrKouiKbnConst.Tonpuku)
                                        {
                                            comment += "（" + odrDtl.Suryo.ToString() + odrDtl.UnitName + "）";
                                        }

                                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, comment);
                                    }
                                }
                                commentSkipFlg = true;
                            }
                            else if (odrDtl.IsYorCommentItem(commentSkipFlg))
                            {
                                // 薬剤・コメント
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                if (odrInf.OdrKouiKbn >= OdrKouiKbnConst.Naifuku && odrInf.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo)
                                {
                                    if (odrInf.InoutKbn == 1)
                                    {
                                        var tempTenZero = _common.Wrk.wrkSinKouiDetails.LastOrDefault();
                                        if (tempTenZero != null)
                                            tempTenZero.TenZero = true;

                                        if (odrDtl.SyohoKbn == 3 && _common.Mst.IsIpnKasanExclude(odrDtl.IpnCd, odrDtl.ItemCd) == false)
                                        {
                                            // 一般名処方の場合、最低薬価で計算
                                            var minYakkaObject = _common.Wrk.wrkSinKouiDetails.LastOrDefault();
                                            if (minYakkaObject != null)
                                                minYakkaObject.MinYakka = odrDtl.MinYakka;
                                        }
                                    }
                                }
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

                        if (firstSinryoKoui == true)
                        {
                            firstSinryoKoui = false;
                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SonotaYakuzai;
                        }
                        else
                        {
                            // 行為を追加する
                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.SonotaYakuzai, cdKbn: "JS", count: odrInf.DaysCnt);
                        }

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (odrDtl.IsTorCommentItem(commentSkipFlg))
                            {
                                // 特材・コメント
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                if (odrInf.OdrKouiKbn >= OdrKouiKbnConst.Naifuku && odrInf.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo)
                                {
                                    if (odrInf.InoutKbn == 1)
                                    {
                                        // 院外処方の場合は0点にする
                                        _common.Wrk.wrkSinKouiDetails.Last().TenZero = true;
                                    }
                                }

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

            #region Local Method

            int GetReceKouiKbn(OdrInfModel AodrInf)
            {
                int ret = 0;

                #region OdrKouiKbn -> ReceKouiKbn
                var dic = new Dictionary<int, int>()
                {
                    { OdrKouiKbnConst.None, 0 },
                    { OdrKouiKbnConst.Syosai, ReceKouiKbn.Syosin },
                    { OdrKouiKbnConst.Syosin, ReceKouiKbn.Syosin },
                    { OdrKouiKbnConst.Saisin, ReceKouiKbn.Saisin },
                    { OdrKouiKbnConst.Igaku, ReceKouiKbn.Igaku },
                    { OdrKouiKbnConst.Zaitaku, ReceKouiKbn.Zaitaku },
                    { OdrKouiKbnConst.Touyaku, ReceKouiKbn.Touyaku },
                    { OdrKouiKbnConst.Naifuku, ReceKouiKbn.Naifuku },
                    { OdrKouiKbnConst.Tonpuku, ReceKouiKbn.Tonpuku },
                    { OdrKouiKbnConst.Gaiyo, ReceKouiKbn.Gaiyo },
                    { OdrKouiKbnConst.Cyozai, ReceKouiKbn.Touyaku },
                    { OdrKouiKbnConst.Syoho, ReceKouiKbn.Touyaku },
                    { OdrKouiKbnConst.Madoku, ReceKouiKbn.Touyaku },
                    { OdrKouiKbnConst.Cyoki, ReceKouiKbn.Touyaku },
                    { OdrKouiKbnConst.JikoCyu, ReceKouiKbn.Touyaku },
                    { OdrKouiKbnConst.Chusya, ReceKouiKbn.Cyusya },
                    { OdrKouiKbnConst.Hikakin, ReceKouiKbn.Hikakin },
                    { OdrKouiKbnConst.Jyoumyaku, ReceKouiKbn.Jyomyaku },
                    { OdrKouiKbnConst.Tenteki, ReceKouiKbn.Tenteki },
                    { OdrKouiKbnConst.ChusyaSonota, ReceKouiKbn.ChusyaSonota },
                    { OdrKouiKbnConst.Syoti, ReceKouiKbn.Syoti },
                    { OdrKouiKbnConst.Syujyutu, ReceKouiKbn.Syujyutu },
                    { OdrKouiKbnConst.Yuketu, ReceKouiKbn.Syujyutu },
                    { OdrKouiKbnConst.Kensa, ReceKouiKbn.Kensa },
                    { OdrKouiKbnConst.Kentai, ReceKouiKbn.Kensa },
                    { OdrKouiKbnConst.Seitai, ReceKouiKbn.Kensa },
                    { OdrKouiKbnConst.Byouri, ReceKouiKbn.Kensa },
                    { OdrKouiKbnConst.Gazo, ReceKouiKbn.Gazo },
                    { OdrKouiKbnConst.Sonota, ReceKouiKbn.Sonota },
                    { OdrKouiKbnConst.Riha, ReceKouiKbn.Sonota },
                    { OdrKouiKbnConst.Seisin, ReceKouiKbn.Sonota },
                    { OdrKouiKbnConst.Syohosen, ReceKouiKbn.Sonota },
                    { OdrKouiKbnConst.Housya, ReceKouiKbn.Sonota },
                    { OdrKouiKbnConst.Jihi, ReceKouiKbn.Jihi },
                    { OdrKouiKbnConst.SyohoCmt, ReceKouiKbn.Sonota},
                    { OdrKouiKbnConst.SyohoBiko, ReceKouiKbn.Sonota}
                };
                #endregion

                int val = 0;
                if (dic.TryGetValue(AodrInf.OdrKouiKbn, out val))
                {
                    ret = val;
                }

                return ret;
            }

            int GetReceSinId(OdrInfModel AodrInf)
            {
                int ret = 0;

                #region OdrKouiKbn -> ReceSinId
                var dic = new Dictionary<int, int>()
                {
                    { OdrKouiKbnConst.None, 0 },
                    { OdrKouiKbnConst.Syosai, ReceSinId.Syosin },
                    { OdrKouiKbnConst.Syosin, ReceSinId.Syosin },
                    { OdrKouiKbnConst.Saisin, ReceSinId.Saisin },
                    { OdrKouiKbnConst.Igaku, ReceSinId.Igaku },
                    { OdrKouiKbnConst.Zaitaku, ReceSinId.Zaitaku },
                    { OdrKouiKbnConst.Touyaku, ReceSinId.Syoho },
                    { OdrKouiKbnConst.Naifuku, ReceSinId.Naifuku },
                    { OdrKouiKbnConst.Tonpuku, ReceSinId.Tonpuku },
                    { OdrKouiKbnConst.Gaiyo, ReceSinId.Gaiyo },
                    { OdrKouiKbnConst.Cyozai, ReceSinId.Cyozai },
                    { OdrKouiKbnConst.Syoho, ReceSinId.SyohoSonota },
                    { OdrKouiKbnConst.Madoku, ReceSinId.Madoku },
                    { OdrKouiKbnConst.Cyoki, ReceSinId.Cyoki },
                    { OdrKouiKbnConst.JikoCyu, ReceSinId.SyohoSonota },
                    { OdrKouiKbnConst.Chusya, ReceSinId.ChusyaSonota },
                    { OdrKouiKbnConst.Hikakin, ReceSinId.Hikakin },
                    { OdrKouiKbnConst.Jyoumyaku, ReceSinId.Jyomyaku },
                    { OdrKouiKbnConst.Tenteki, ReceSinId.ChusyaSonota },
                    { OdrKouiKbnConst.ChusyaSonota, ReceSinId.ChusyaSonota },
                    { OdrKouiKbnConst.Syoti, ReceSinId.Syoti },
                    { OdrKouiKbnConst.Syujyutu, ReceSinId.Syujyutu },
                    { OdrKouiKbnConst.Yuketu, ReceSinId.Syujyutu },
                    { OdrKouiKbnConst.Kensa, ReceSinId.Kensa },
                    { OdrKouiKbnConst.Kentai, ReceSinId.Kensa },
                    { OdrKouiKbnConst.Seitai, ReceSinId.Kensa },
                    { OdrKouiKbnConst.Byouri, ReceSinId.Kensa },
                    { OdrKouiKbnConst.Gazo, ReceSinId.Gazo },
                    { OdrKouiKbnConst.Sonota, ReceSinId.Sonota },
                    { OdrKouiKbnConst.Riha, ReceSinId.Sonota },
                    { OdrKouiKbnConst.Seisin, ReceSinId.Sonota },
                    { OdrKouiKbnConst.Syohosen, ReceSinId.Sonota },
                    { OdrKouiKbnConst.Housya, ReceSinId.Sonota },
                    { OdrKouiKbnConst.Jihi, ReceSinId.Jihi },
                    { OdrKouiKbnConst.SyohoCmt, ReceSinId.Sonota},
                    { OdrKouiKbnConst.SyohoBiko, ReceSinId.Sonota}
                };
                #endregion

                int val = 0;
                if (dic.TryGetValue(AodrInf.OdrKouiKbn, out val))
                {
                    ret = val;
                }

                return ret;
            }

            string GetReceSyukeisaki(OdrInfModel AodrInf)
            {
                string ret = "0";

                #region OdrKouiKbn -> ReceSyukeisaki
                var dic = new Dictionary<int, string>()
                {
                    { OdrKouiKbnConst.None, "0" },
                    { OdrKouiKbnConst.Syosai, ReceSyukeisaki.Syosin },
                    { OdrKouiKbnConst.Syosin, ReceSyukeisaki.Syosin },
                    { OdrKouiKbnConst.Saisin, ReceSyukeisaki.Saisin },
                    { OdrKouiKbnConst.Igaku, ReceSyukeisaki.Igaku },
                    { OdrKouiKbnConst.Zaitaku, ReceSyukeisaki.ZaiSonota },
                    { OdrKouiKbnConst.Touyaku, ReceSyukeisaki.TouyakuSyoho },
                    { OdrKouiKbnConst.Naifuku, ReceSyukeisaki.TouyakuNaiYakuzai },
                    { OdrKouiKbnConst.Tonpuku, ReceSyukeisaki.TouyakuTon },
                    { OdrKouiKbnConst.Gaiyo, ReceSyukeisaki.TouyakuGaiYakuzai },
                    { OdrKouiKbnConst.Cyozai, ReceSyukeisaki.TouyakuNaiCyozai },
                    { OdrKouiKbnConst.Syoho, ReceSyukeisaki.TouyakuSyoho },
                    { OdrKouiKbnConst.Madoku, ReceSyukeisaki.TouyakuMadoku },
                    { OdrKouiKbnConst.Cyoki, ReceSyukeisaki.TouyakuChoKi },
                    { OdrKouiKbnConst.JikoCyu, ReceSyukeisaki.ZaiYakuzai },
                    { OdrKouiKbnConst.Chusya, ReceSyukeisaki.ChusyaSonota },
                    { OdrKouiKbnConst.Hikakin, ReceSyukeisaki.ChusyaHikakin },
                    { OdrKouiKbnConst.Jyoumyaku, ReceSyukeisaki.ChusyaJyoumyaku },
                    { OdrKouiKbnConst.Tenteki, ReceSyukeisaki.ChusyaSonota },
                    { OdrKouiKbnConst.ChusyaSonota, ReceSyukeisaki.ChusyaSonota },
                    { OdrKouiKbnConst.Syoti, ReceSyukeisaki.Syoti },
                    { OdrKouiKbnConst.Syujyutu, ReceSyukeisaki.OpeMasui },
                    { OdrKouiKbnConst.Yuketu, ReceSyukeisaki.OpeMasui },
                    { OdrKouiKbnConst.Kensa, ReceSyukeisaki.Kensa },
                    { OdrKouiKbnConst.Kentai, ReceSyukeisaki.Kensa },
                    { OdrKouiKbnConst.Seitai, ReceSyukeisaki.Kensa },
                    { OdrKouiKbnConst.Byouri, ReceSyukeisaki.Kensa },
                    { OdrKouiKbnConst.Gazo, ReceSyukeisaki.Gazo },
                    { OdrKouiKbnConst.Sonota, ReceSyukeisaki.Sonota },
                    { OdrKouiKbnConst.Riha, ReceSyukeisaki.Sonota },
                    { OdrKouiKbnConst.Seisin, ReceSyukeisaki.Sonota },
                    { OdrKouiKbnConst.Syohosen, ReceSyukeisaki.Sonota },
                    { OdrKouiKbnConst.Housya, ReceSyukeisaki.Sonota },
                    { OdrKouiKbnConst.Jihi, ReceSyukeisaki.Jihi },
                    { OdrKouiKbnConst.SyohoCmt, ReceSyukeisaki.Sonota},
                    { OdrKouiKbnConst.SyohoBiko, ReceSyukeisaki.Sonota}
                };
                #endregion

                string val = "0";
                if (dic.TryGetValue(AodrInf.OdrKouiKbn, out val))
                {
                    ret = val;
                }

                return ret;
            }

            string GetReceSyukeisakiYakuzai(OdrInfModel AodrInf)
            {
                string ret = "0";

                #region OdrKouiKbn -> ReceSyukeisaki
                var dic = new Dictionary<int, string>()
                {
                    { OdrKouiKbnConst.None, "0" },
                    { OdrKouiKbnConst.Syosai, ReceSyukeisaki.Syosin },
                    { OdrKouiKbnConst.Syosin, ReceSyukeisaki.Syosin },
                    { OdrKouiKbnConst.Saisin, ReceSyukeisaki.Saisin },
                    { OdrKouiKbnConst.Igaku, ReceSyukeisaki.Igaku },
                    { OdrKouiKbnConst.Zaitaku, ReceSyukeisaki.ZaiYakuzai },
                    { OdrKouiKbnConst.Touyaku, ReceSyukeisaki.TouyakuSyoho },
                    { OdrKouiKbnConst.Naifuku, ReceSyukeisaki.TouyakuNaiYakuzai },
                    { OdrKouiKbnConst.Tonpuku, ReceSyukeisaki.TouyakuTon },
                    { OdrKouiKbnConst.Gaiyo, ReceSyukeisaki.TouyakuGaiYakuzai },
                    { OdrKouiKbnConst.Cyozai, ReceSyukeisaki.TouyakuNaiCyozai },
                    { OdrKouiKbnConst.Syoho, ReceSyukeisaki.TouyakuSyoho },
                    { OdrKouiKbnConst.Madoku, ReceSyukeisaki.TouyakuMadoku },
                    { OdrKouiKbnConst.Cyoki, ReceSyukeisaki.TouyakuChoKi },
                    { OdrKouiKbnConst.JikoCyu, ReceSyukeisaki.ZaiYakuzai },
                    { OdrKouiKbnConst.Chusya, ReceSyukeisaki.ChusyaYakuzai },
                    { OdrKouiKbnConst.Hikakin, ReceSyukeisaki.ChusyaHikakin },
                    { OdrKouiKbnConst.Jyoumyaku, ReceSyukeisaki.ChusyaJyoumyaku },
                    { OdrKouiKbnConst.Tenteki, ReceSyukeisaki.ChusyaYakuzai },
                    { OdrKouiKbnConst.ChusyaSonota, ReceSyukeisaki.ChusyaYakuzai },
                    { OdrKouiKbnConst.Syoti, ReceSyukeisaki.SyotiYakuzai },
                    { OdrKouiKbnConst.Syujyutu, ReceSyukeisaki.OpeYakuzai },
                    { OdrKouiKbnConst.Yuketu, ReceSyukeisaki.OpeYakuzai },
                    { OdrKouiKbnConst.Kensa, ReceSyukeisaki.Kensayakuzai },
                    { OdrKouiKbnConst.Kentai, ReceSyukeisaki.Kensayakuzai },
                    { OdrKouiKbnConst.Seitai, ReceSyukeisaki.Kensayakuzai },
                    { OdrKouiKbnConst.Byouri, ReceSyukeisaki.Kensayakuzai },
                    { OdrKouiKbnConst.Gazo, ReceSyukeisaki.GazoYakuzai },
                    { OdrKouiKbnConst.Sonota, ReceSyukeisaki.SonotaYakuzai },
                    { OdrKouiKbnConst.Riha, ReceSyukeisaki.SonotaYakuzai },
                    { OdrKouiKbnConst.Seisin, ReceSyukeisaki.SonotaYakuzai },
                    { OdrKouiKbnConst.Syohosen, ReceSyukeisaki.SonotaYakuzai },
                    { OdrKouiKbnConst.Housya, ReceSyukeisaki.SonotaYakuzai },
                    { OdrKouiKbnConst.Jihi, ReceSyukeisaki.Jihi },
                    { OdrKouiKbnConst.SyohoCmt, ReceSyukeisaki.SonotaYakuzai},
                    { OdrKouiKbnConst.SyohoBiko, ReceSyukeisaki.SonotaYakuzai}
                };
                #endregion

                string val = "0";
                if (dic.TryGetValue(AodrInf.OdrKouiKbn, out val))
                {
                    ret = val;
                }

                return ret;
            }
            #endregion

        }

    }
}
