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
    /// 投薬
    /// </summary>
    class IkaCalculateOdrToWrkTouyakuViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 処方料のリスト
        /// </summary>
        List<string> Syohoryols =
            new List<string>
            {
                ItemCdConst.TouyakuSyohoSonota,
                ItemCdConst.TouyakuSyohoKousei,
                ItemCdConst.TouyakuSyohoNaifuku,
                ItemCdConst.TouyakuSyohoryoNaifuku,
                ItemCdConst.TouyakuSyohoryoKouseiChoki,
                ItemCdConst.TouyakuSyohoKouseiChoki
            };

        /// <summary>
        /// 調剤料のリスト
        /// </summary>
        List<string> Chozairyols =
            new List<string>
            {
                ItemCdConst.TouyakuChozaiNaiTon,
                ItemCdConst.TouyakuChozaiGai
            };

        /// <summary>
        /// 調剤基本料のリスト
        /// </summary>
        List<string> ChoKiryols =
            new List<string>
            {
                ItemCdConst.TouyakuChoKi
            };

        /// <summary>
        /// 処方箋料のリスト
        /// </summary>
        List<string> Syohosenryols =
            new List<string>
            {
                ItemCdConst.TouyakuSyohosenSonota,
                ItemCdConst.TouyakuSyohosenKouSei,
                ItemCdConst.TouyakuSyohosenNaifukuKousei,
                ItemCdConst.TouyakuSyohosenKouseiChoki,
                ItemCdConst.TouyakuSyohosenSonotaRefill,
                ItemCdConst.TouyakuSyohosenKouSeiRefill,
                ItemCdConst.TouyakuSyohosenNaifukuKouseiRefill,
                ItemCdConst.TouyakuSyohosenKouseiChokiRefill
            };

        // 向精神薬多剤投与項目
        List<string> Kouseils =
            new List<string>
            {
                ItemCdConst.TouyakuSyohoKousei,
                ItemCdConst.TouyakuSyohosenKouSei,
                ItemCdConst.TouyakuSyohosenKouSeiRefill,
            };
        
        // 多剤投与
        List<string> Tazails =
            new List<string>
            {
                ItemCdConst.TouyakuSyohoNaifuku,
                ItemCdConst.TouyakuSyohosenNaifukuKousei,
                ItemCdConst.TouyakuSyohoryoNaifuku,
                ItemCdConst.TouyakuSyohosenNaifukuKouseiRefill,
            };

        /// <summary>
        /// 一般名処方加算 
        /// </summary>
        List<string> IpnKasanls =
            new List<string>
            {
                ItemCdConst.TouyakuIpnName1,
                ItemCdConst.TouyakuIpnName2
            };

        /// <summary>
        /// 特定疾患処方管理加算
        /// </summary>
        List<string> Tokusyo1ls =
            new List<string>
            {
                ItemCdConst.TouyakuTokuSyo1Syoho,
                ItemCdConst.TouyakuTokuSyo1Syohosen
            };
        List<string> Tokusyo2ls =
            new List<string>
            {
                ItemCdConst.TouyakuTokuSyo2Syoho,
                ItemCdConst.TouyakuTokuSyo2Syohosen
            };
        List<string> TokusyoSyohols =
            new List<string>
            {
                ItemCdConst.TouyakuTokuSyo1Syoho,
                ItemCdConst.TouyakuTokuSyo2Syoho
            };
        List<string> TokusyoSyohosenls =
            new List<string>
            {
                ItemCdConst.TouyakuTokuSyo1Syohosen,
                ItemCdConst.TouyakuTokuSyo2Syohosen
            };

        /// <summary>
        /// 処方箋料、処方料のオーダーがあるかどうか
        /// </summary>
        bool IsOrderSyohoryo = false;

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkTouyakuViewModel(IkaCalculateCommonDataViewModel common,
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

            IsOrderSyohoryo = false;

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.TouyakuMin, OdrKouiKbnConst.TouyakuMax))
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
            IEnumerable<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            bool commentSkipFlg = false;
            //check
            List<string> targetItemCds = new List<string>();
            targetItemCds.AddRange(Syohoryols);
            targetItemCds.AddRange(Syohosenryols);

            IsOrderSyohoryo = _common.Odr.ExistOdrDetailByItemCd(targetItemCds);

            // 向精神薬チェック
            bool kouseiTazai = CheckKouseisin();

            // 内服薬多剤投与チェック
            bool naifukuTazai = CheckNaifukuTazai(kouseiTazai);

            // 処方料
            Syohoryo(kouseiTazai, naifukuTazai);

            // 調剤料
            Chozairyo();

            // 調剤基本料
            ChoKiryo();

            // 処方箋料
            Syohosenryo(kouseiTazai, naifukuTazai);

            // 特定疾患処方管理加算
            Tokusyo();

            // 薬材料
            // 内服薬
            Yakuzai(OdrKouiKbnConst.Naifuku, kouseiTazai, naifukuTazai);
            // 頓服薬
            Yakuzai(OdrKouiKbnConst.Tonpuku, kouseiTazai, naifukuTazai);
            // 外用薬
            Yakuzai(OdrKouiKbnConst.Gaiyo, kouseiTazai, naifukuTazai);
            // 自己注
            Yakuzai(OdrKouiKbnConst.JikoCyu, kouseiTazai, naifukuTazai);

            // 処方のRpを取得(処方箋コメント29は除く）
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.TouyakuMin, OdrKouiKbnConst.JikoCyu)
                                .OrderBy(p=>p.SortNo)
                                .ThenBy(p=>p.RpNo)
                                .ThenBy(p=>p.RpEdaNo);
          
            if (filteredOdrInf.Any())
            {
                bool firstSinryoKoui;

                int sinKouiKbn = ReceKouiKbn.Touyaku;
                int sinId = ReceSinId.SyohoSonota;
                string syukeisaki = ReceSyukeisaki.TouyakuNaiYakuzai;

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any())
                    {
                        firstSinryoKoui = true;

                        // 薬剤の集計先
                        switch (odrInf.OdrKouiKbn)
                        {
                            case OdrKouiKbnConst.Naifuku:
                                sinKouiKbn = ReceKouiKbn.Naifuku;
                                sinId = ReceSinId.Naifuku;
                                syukeisaki = ReceSyukeisaki.TouyakuNaiYakuzai;
                                break;
                            case OdrKouiKbnConst.Tonpuku:
                                sinKouiKbn = ReceKouiKbn.Tonpuku;
                                sinId = ReceSinId.Tonpuku;
                                syukeisaki = ReceSyukeisaki.TouyakuTon;
                                break;
                            case OdrKouiKbnConst.Gaiyo:
                                sinKouiKbn = ReceKouiKbn.Gaiyo;
                                sinId = ReceSinId.Gaiyo;
                                syukeisaki = ReceSyukeisaki.TouyakuGaiYakuzai;
                                break;
                            case OdrKouiKbnConst.JikoCyu:
                                sinKouiKbn = ReceKouiKbn.Zaitaku;
                                sinId = ReceSinId.Zaitaku;
                                syukeisaki = ReceSyukeisaki.ZaiYakuzai;
                                break;
                            default:
                                sinKouiKbn = ReceKouiKbn.Touyaku;
                                sinId = ReceSinId.SyohoSonota;
                                syukeisaki = ReceSyukeisaki.TouyakuNaiYakuzai;
                                break;
                        }

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(sinKouiKbn, sinId, odrInf.SanteiKbn);

                        // 集計先は、後で内容により変更する                    
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "F"));

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            //if(odrInf.OdrKouiKbn == OdrKouiKbnConst.JikoCyu)
                            //{
                            //    // 自己注射、そのまま算定
                            //    //if (firstSinryoKoui == false)
                            //    //{
                            //    //    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "F"));
                            //    // }
                            //    //else
                            //    //{
                            //    //    firstSinryoKoui = false;
                            //    //}

                            //    //_common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            //}
                            //else 
                            if (odrDtl.IsYItem)
                            {
                                // 薬剤
                                //commentSkipFlg = true;

                                if (odrInf.OdrKouiKbn == OdrKouiKbnConst.JikoCyu)
                                {
                                    if (_systemConfigProvider.GetOutDrugYohoDsp() == 1)
                                    {
                                        // 院外の場合、コメントとして記録
                                        string comment = odrDtl.ItemName;
                                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, comment);
                                    }
                                }
                                else if (odrInf.OdrKouiKbn == OdrKouiKbnConst.Touyaku)
                                {
                                    // 薬剤が20投薬に入ってしまうことがあるらしい
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    _common.Wrk.wrkSinKouis.Last().Count = odrInf.DaysCnt;
                                }

                            }
                            else if (!(odrDtl.IsComment && commentSkipFlg) && odrInf.OdrKouiKbn != OdrKouiKbnConst.JikoCyu)
                            {
                                // 算定回数チェック
                                if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                                {
                                    // 算定回数マスタのチェックにより算定不可
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                }
                                else if(_common.CheckAge(odrDtl) == 2)
                                {
                                    // 年齢チェック
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                }
                                else
                                {
                                    if (firstSinryoKoui == false)
                                    {
                                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "F"));
                                    }
                                    else
                                    {
                                        firstSinryoKoui = false;
                                    }

                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                }

                                commentSkipFlg = false;
                            }
                        }
                    }
                }

                _common.Wrk.CommitWrkSinRpInf();
            }

            #region Local Method


            #endregion
        }

        /// <summary>
        /// 向精神薬の多剤投与にあたるオーダーがあるかチェックする
        /// </summary>
        /// <returns>true: 向精神薬多剤投与対象</returns>
        private bool CheckKouseisin()
        {
            const string conFncName = nameof(CheckKouseisin);

            bool ret = false;

            // 手オーダー確認
            if (_common.Odr.FilterOdrDetailByItemCd(Kouseils).Any())
            {
                ret = true;
            }
            //else if(CheckTiikiHokatuNoSantei())
            else
            {               

                string[] conName = { "抗不安薬", "睡眠薬", "抗うつ薬", "抗精神病薬", "抗不安薬または睡眠薬" };
                                
                List<OdrDtlTenModel> filteredOdrDtl = _common.Odr.FilterOdrKouiKouseisin();
                // 0 - 抗不安薬(TEN_MST.KOUSEISIN_KBN = 1)
                // 1 - 睡眠薬(TEN_MST.KOUSEISIN_KBN = 2)
                // 2 - 抗うつ薬(TEN_MST.KOUSEISIN_KBN = 3)
                // 3 - 抗精神病薬(TEN_MST.KOUSEISIN_KBN = 4)
                // 4 - 抗不安薬または睡眠薬(TEN_MST.KOUSEISIN_KBN = 1 or 2)
                List<string>[] kouSeisin = new List<string>[5];
                List<string>[] kouSeisinRinji = new List<string>[kouSeisin.Length];
                int[] kouSeisinCount = new int[kouSeisin.Length];

                for (int i = 0; i < kouSeisin.Length; i++)
                {
                    kouSeisin[i] = new List<string>();
                    kouSeisinRinji[i] = new List<string>();
                    kouSeisinCount[i] = 0;
                }

                kouSeisinCount[0] = 3;
                kouSeisinCount[1] = 3;
                kouSeisinCount[2] = 3;
                kouSeisinCount[3] = 3;
                kouSeisinCount[4] = 4;

                // オーダー内容から向精神薬を種類別にリスト化（重複あり）
                int rinji = 0;
                foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                {
                    rinji = 0;

                    if (odrDtl.SyohoSbt == 1 || (odrDtl.SyohoSbt == 0 && new int[] { 21, 22 }.Contains(odrDtl.OdrKouiKbn) && odrDtl.DaysCnt <= _systemConfigProvider.GetSyohoRinjiDays()))
                    {
                        // 臨時
                        rinji = 1;
                    }

                    if (rinji == 0 || (rinji == 1 && (odrDtl.KouseisinKbn == 1 || odrDtl.KouseisinKbn == 2)))
                    {
                        kouSeisin[odrDtl.KouseisinKbn - 1].Add((CIUtil.Copy(odrDtl.IpnNameCd, 1, 7)));

                        if (odrDtl.KouseisinKbn == 1 || odrDtl.KouseisinKbn == 2)
                        {
                            kouSeisin[4].Add(CIUtil.Copy(odrDtl.IpnNameCd, 1, 7));
                        }

                        _emrLogger.WriteLogMsg( this, conFncName, conName[odrDtl.KouseisinKbn - 1] + ":" + odrDtl.Name + "/" + odrDtl.IpnNameCd);
                    }
                    else
                    {
                        kouSeisinRinji[odrDtl.KouseisinKbn - 1].Add((CIUtil.Copy(odrDtl.IpnNameCd, 1, 7)));
                        if (odrDtl.KouseisinKbn == 1 || odrDtl.KouseisinKbn == 2)
                        {
                            kouSeisinRinji[4].Add(CIUtil.Copy(odrDtl.IpnNameCd, 1, 7));
                        }

                        _emrLogger.WriteLogMsg( this, conFncName, conName[odrDtl.KouseisinKbn - 1] + "(臨):" + odrDtl.Name + "/" + odrDtl.IpnNameCd);
                    }
                }

                // 同じ種類の向精神薬で臨時と臨時でないものがある場合、臨時ではないとして扱う
                for (int i = 0; i < kouSeisin.Length; i++)
                {
                    int j = 0;
                    while (j < kouSeisinRinji[i].Count)
                    {
                        int k = 0;
                        while (k < kouSeisin[i].Count)
                        {
                            if (kouSeisinRinji[i][j] == kouSeisin[i][k])
                            {
                                kouSeisinRinji[i].RemoveAt(j);
                                if(j >= kouSeisinRinji[i].Count)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                k++;
                            }
                        }
                        j++;
                    }
                }

                // 抗精神薬の上限チェック
                string kousei = "";
                string kouseiTazai = "";

                bool kouseiTaisyoGai = _common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KouseiTaisyoGai);

                for (int i = 0; i < kouSeisin.Length; i++)
                {
                    // 重複カット
                    IEnumerable<string> kouSeisinList = kouSeisin[i].Distinct();
                    IEnumerable<string> kouSeisinListRinji = kouSeisinRinji[i].Distinct();

                    // ログ出力用文字列生成
                    if (kouSeisinList.Any() || kouSeisinListRinji.Any())
                    {
                        if (kouSeisinList.Count() < kouSeisinCount[i] || kouseiTaisyoGai)
                        {
                            // 対象外の場合はこちら
                            kousei += conName[i] + "(" + kouSeisinList.Count().ToString() + ")";
                            if (kouSeisinListRinji.Any())
                            {
                                kousei += "(臨" + kouSeisinListRinji.Count().ToString() + ")";
                            }
                        }
                        else
                        {
                            kouseiTazai += conName[i] + "(" + kouSeisinList.Count().ToString() + ")";
                            if (kouSeisinListRinji.Any())
                            {
                                kouseiTazai += "(臨" + kouSeisinListRinji.Count().ToString() + ")";
                            }
                        }
                    }
                }

                if (kouseiTazai != "")
                {
                    // 多剤投与の場合、戻り値true
                    _common.AppendCalcLog(2, "【向精神薬多剤投与】" + kouseiTazai);
                    ret = true;
                }
                else if (kousei != "")
                {
                    _common.AppendCalcLog(0, "【向精神薬投与】" + kousei);
                }
            }

            return ret;
        }

        /// <summary>
        /// 内服薬の多剤投与にあたるオーダーがあるかチェックする
        /// </summary>
        /// <param name="excludeKouSeisin">true - 向精神薬を除いてカウント</param>
        /// <returns>true-内服薬の多剤投与に当たるオーダーがある</returns>
        private bool CheckNaifukuTazai(bool excludeKouSeisin)
        {
            bool ret = false;

            // 手オーダーチェック
            if (_common.Odr.FilterOdrDetailByItemCd(Tazails).Any())
            {
                ret = true;
            }
            else　if(CheckTiikiHokatuNoSantei())
            {
                // 当来院で地域包括診療加算を算定している or 当月内に地域包括診療料を算定している場合はチェックしない
                IEnumerable<OdrInfModel> filteredOdrInf = _common.Odr.FilterOdrKouiNaifuku().OrderBy(p => p.RpNo).ThenBy(p => p.RpEdaNo);
                HashSet<string> totalItemCds = new HashSet<string>();
                int rinji = 0;
                int yakuzaiCount = 0;
                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    rinji = 0;

                    if (odrInf.SyohoSbt == 1 || (odrInf.SyohoSbt == 0 && odrInf.DaysCnt <= _systemConfigProvider.GetSyohoRinjiDays()))
                    {
                        // 臨時
                        rinji = 1;
                    }

                    if (rinji == 0)
                    {
                        List<OdrDtlTenModel> odrDtls = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                        bool kongo = false;
                        double totalTen = 0;

                        // カウント対象薬剤を含むRpかどうか
                        // カウント対象薬剤とは、
                        // 　・向精神薬を除かない場合はすべての薬剤、
                        // 　・除く場合は向精神薬以外の薬剤
                        bool countRp = false;

                        HashSet<string> rpItemCds = new HashSet<string>();
                        HashSet<string> sanZaiItemCds = new HashSet<string>();
                        HashSet<string> ekiZaiItemCds = new HashSet<string>();
                        HashSet<string> karyuZaiItemCds = new HashSet<string>();

                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            if (odrDtl.IsDrug)
                            {
                                if (excludeKouSeisin == false || odrDtl.KouseisinKbn == 0)
                                {
                                    // 向精神薬を除かない、または向精神薬ではない                                   
                                    countRp = true;

                                    switch (odrDtl.ZaiKbn)
                                    {
                                        case 1:
                                            // 散剤
                                            sanZaiItemCds.Add(odrDtl.ItemCd);
                                            break;
                                        case 2:
                                            // 顆粒剤
                                            karyuZaiItemCds.Add(odrDtl.ItemCd);
                                            break;
                                        case 3:
                                            // 液剤
                                            ekiZaiItemCds.Add(odrDtl.ItemCd);
                                            break;
                                        default:
                                            rpItemCds.Add(odrDtl.ItemCd);
                                            break;
                                    }
                                }

                                // Rpの合計点数を取得する
                                if (odrInf.InoutKbn == 1 && odrDtl.SyohoKbn == 3 && _common.Mst.IsIpnKasanExclude(odrDtl.IpnCd, odrDtl.ItemCd) == false && odrDtl.MinYakka > 0)
                                {
                                    // 一般名処方の場合、最低薬価で計算
                                    totalTen += odrDtl.MinYakka * odrDtl.CalcedSuryo;
                                }
                                else
                                {
                                    totalTen += odrDtl.Ten * odrDtl.CalcedSuryo;
                                }
                                //}
                            }
                        }

                        if (countRp)
                        {
                            // 向精神薬を除かない、または向精神薬ではない薬剤を含む
                            if (totalTen > 0 && totalTen <= 205)
                            {
                                // 205円未満
                                yakuzaiCount++;
                            }
                            else
                            {
                                if (sanZaiItemCds.Any())
                                {
                                    // 散剤が存在する場合
                                    if (sanZaiItemCds.Count() == 1)
                                    {
                                        // 1種類の場合、重複を避けるため、銘柄チェック
                                        rpItemCds.Add(sanZaiItemCds.First());
                                    }
                                    else if (sanZaiItemCds.Count() >= 2)
                                    {
                                        // 2種類以上の場合、1種としてカウント
                                        yakuzaiCount++;
                                    }
                                }

                                if (karyuZaiItemCds.Any())
                                {
                                    // 顆粒剤が存在する場合
                                    if (karyuZaiItemCds.Count() == 1)
                                    {
                                        // 1種類の場合、重複を避けるため、銘柄チェック
                                        rpItemCds.Add(karyuZaiItemCds.First());
                                    }
                                    else if (karyuZaiItemCds.Count() >= 2)
                                    {
                                        // 2種類以上の場合、1種としてカウント
                                        yakuzaiCount++;
                                    }
                                }

                                if (ekiZaiItemCds.Any())
                                {
                                    // 液剤が存在する場合
                                    if (ekiZaiItemCds.Count() == 1)
                                    {
                                        // 1種類の場合、重複を避けるため、銘柄チェック
                                        rpItemCds.Add(ekiZaiItemCds.First());
                                    }
                                    else if (ekiZaiItemCds.Count() >= 2)
                                    {
                                        // 2種類以上の場合、1種としてカウント
                                        yakuzaiCount++;
                                    }
                                }

                                // 薬剤の種類をプラスする
                                //yakuzaiCount += rpItemCds.Count;
                                foreach (string itemCd in rpItemCds)
                                {
                                    totalItemCds.Add(itemCd);
                                }
                            }
                        }
                    }
                }

                yakuzaiCount += totalItemCds.Count;

                if (yakuzaiCount >= 7)
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 当来院で地域包括診療加算を算定している or 当月内に地域包括診療料を算定しているかチェック
        /// 算定している場合、多剤投与対象外になる(trueを返す)
        /// </summary>
        /// <returns>false-当来院で地域包括診療加算を算定している or 当月内に地域包括診療料を算定している</returns>
        private bool CheckTiikiHokatuNoSantei()
        {
            // 地域包括診療料
            List<string> TiikiHoukatuls =
                new List<string>
                {
                ItemCdConst.IgakuTiikiHoukatu1,
                ItemCdConst.IgakuTiikiHoukatu2,
                };
            // 地域包括診療加算
            List<string> TiikiHoukatuKasanls =
                new List<string>
                {
                ItemCdConst.SaisinTiikiHoukatu1,
                ItemCdConst.SaisinTiikiHoukatu2,
                };

            return (_common.Wrk.FindWrkSinKouiDetailByItemCd(FindWrkDtlMode.RaiinOnly, TiikiHoukatuKasanls).Any() == false &&
                    _common.Wrk.FindWrkSinKouiDetailByItemCd(FindWrkDtlMode.RaiinOnly, TiikiHoukatuls).Any() == false &&
                    _common.Sin.GetSanteiDaysSinYm(TiikiHoukatuls).Any() == false);
        }

        /// <summary>
        /// 処方箋料を算定する
        /// kouseiとtazaiNaifuku両方trueの場合は、kouiseを優先する
        /// いずれもfalseの場合は、処方箋料（その他）を算定する
        /// </summary>
        /// <param name="kousei">true-向精神薬多剤投与に該当、処方箋料（向精神薬多剤投与）を算定</param>
        /// <param name="tazaiNaifuku">true-多剤投与に該当、処方箋料（７種類以上内服薬又は向精神薬長期処方）を算定</param>
        private void Syohosenryo(bool kousei, bool tazaiNaifuku)
        {
            bool santei = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;
            int pid = _common.syosaiPid;
            int hid = _common.syosaiHokenId;
            int santeiKbn = _common.syosaiSanteiKbn;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Syohosenryols);

            if (minIndex >= 0)
            {
                // 手オーダーあり
                pid = odrDtls.First().HokenPid;
                hid = odrDtls.First().HokenId;
                santeiKbn = odrDtls.First().SanteiKbn;

                // Rp、行為を追加
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, santeiKbn);
                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.SonotaSyohoSen, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    if (_common.sinDate >= 20200401 &&
                        new string[] { ItemCdConst.TouyakuSyohosenKouseiChoki, ItemCdConst.TouyakuSyohosenKouseiChokiRefill }.Contains(odrDtl.ItemCd) &&
                        (kousei == true || tazaiNaifuku == true))
                    {
                        // 2020/04/01以降で、オーダーされた処方箋料が向精神薬長期投与だった場合で、向精神薬多剤投与か内服薬多剤投与に当たる場合、
                        // 向精神薬多剤投与か内服薬多剤投与に置き換える
                        string itemCd = "";

                        if (kousei)
                        {
                            //if (_common.sinDate >= 20220401 && _common.Odr.IsRefill)
                            if(_common.sinDate >= 20220401 && odrDtl.ItemCd == ItemCdConst.TouyakuSyohosenKouseiChokiRefill)
                            {
                                // 2022/04/01以降、オーダーされたのがリフィルの場合
                                // (IsRefillと迷ったが、オーダー優先の原則から考えると、
                                // この方がいいのではないかと思った
                                itemCd = ItemCdConst.TouyakuSyohosenKouSeiRefill;
                            }
                            else
                            {
                                itemCd = ItemCdConst.TouyakuSyohosenKouSei;
                            }
                        }
                        else if (tazaiNaifuku)
                        {
                            //if (_common.sinDate >= 20220401 && _common.Odr.IsRefill)
                            if (_common.sinDate >= 20220401 && odrDtl.ItemCd == ItemCdConst.TouyakuSyohosenKouseiChokiRefill)
                            {
                                // 2022/04/01以降、オーダーされたのがリフィルの場合
                                // (IsRefillと迷ったが、オーダー優先の原則から考えると、
                                // この方がいいのではないかと思った
                                itemCd = ItemCdConst.TouyakuSyohosenNaifukuKouseiRefill;
                            }
                            else
                            {
                                itemCd = ItemCdConst.TouyakuSyohosenNaifukuKousei;
                            }
                        }

                        if (itemCd != "")
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                            // 年齢加算
                            _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(itemCd);

                            santei = true;
                        }
                    }
                    else
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                        // 年齢加算
                        _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);

                        santei = true;
                    }
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Syohosenryols);
                }

            }
            else if (_common.Odr.ExistIngaiSyoho)
            {
                // 自動算定

                if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.TouyakuSyohosenCancel) == false)
                {
                    // pidを特定する
                    //(pid, hid, santeiKbn) = GetPid(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo, 1, false, true);
                    //if(pid == 0)
                    //{
                    //    // 特定できなかった場合、特処をチェック
                    //    (pid, hid, santeiKbn) = GetPid(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo, 1, false, false);
                    //}
                    (pid, hid, santeiKbn) = GetPid(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo, 1, false, false);

                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, santeiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.SonotaSyohoSen, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                    string itemCd = "";

                    if (kousei)
                    {
                        if (_common.sinDate >= 20220401 && _common.Odr.IsRefill)
                        {
                            // 2022/04/01以降、リフィル処方あり
                            itemCd = ItemCdConst.TouyakuSyohosenKouSeiRefill;
                        }
                        else
                        {
                            itemCd = ItemCdConst.TouyakuSyohosenKouSei;
                        }

                    }
                    else if (tazaiNaifuku)
                    {
                        if (_common.sinDate >= 20220401 && _common.Odr.IsRefill)
                        {
                            // 2022/04/01以降、リフィル処方あり
                            itemCd = ItemCdConst.TouyakuSyohosenNaifukuKouseiRefill;
                        }
                        else
                        {
                            itemCd = ItemCdConst.TouyakuSyohosenNaifukuKousei;
                        }
                    }
                    else
                    {
                        if (_common.sinDate >= 20220401 && _common.Odr.IsRefill)
                        {
                            // 2022/04/01以降、リフィル処方あり
                            itemCd = ItemCdConst.TouyakuSyohosenSonotaRefill;
                        }
                        else
                        {
                            itemCd = ItemCdConst.TouyakuSyohosenSonota;
                        }
                    }

                    if (itemCd != "")
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                        // 年齢加算
                        _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(itemCd);

                        santei = true;
                    }
                }
            }

            if (santei)
            {
                // 加算の算定処理

                // 一般名処方加算
                IpnNameKasan(pid, hid, santeiKbn);
            }

            if (_common.sinDate >= 20201001)
            {
                #region 湿布薬のコメント
                odrDtls =
                    _common.Odr.FilterOdrDetailComment(OdrKouiKbnConst.TouyakuMin, OdrKouiKbnConst.TouyakuMax)
                        .OrderBy(p => p.RpNo)
                        .ThenBy(p => p.RpEdaNo)
                        .ThenBy(p => p.RowNo)
                        .ToList();

                if (odrDtls != null && odrDtls.Any())
                {
                    if(santei == false)
                    {
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, santeiKbn);
                    }

                    HashSet<(long, long)> rpnos =
                        new HashSet<(long, long)>();

                    foreach (OdrDtlTenModel odrDtl in odrDtls.FindAll(p => p.SanteiKbn == 0))
                    {
                        rpnos.Add((odrDtl.RpNo, odrDtl.RpEdaNo));
                    }

                    foreach ((long rpno, long rpedano) in rpnos)
                    {
                        // 院外処方
                        List<OdrDtlTenModel> tgtOdrDtls = _common.Odr.FilterOdrDetailByRpNo(rpno, rpedano);
                        if (tgtOdrDtls != null && tgtOdrDtls.Any(p => p.DrugKbn > 0) && tgtOdrDtls.First().InoutKbn == 1)
                        {
                            // Rp、行為を追加
                            //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, santeiKbn);
                            //_common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.SonotaSyohoSen, cdKbn: _common.GetCdKbn(santeiKbn, "F"));
                            _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.SonotaSyohoSenComment, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                            if (tgtOdrDtls.Any(p => p.ItemCd == ItemCdConst.CommentSippuYoryo))
                            //if (tgtOdrDtls.Any(p => p.IsSelectComment))
                            {
                                foreach (OdrDtlTenModel tgtOdrDtl in tgtOdrDtls.FindAll(p => p.DrugKbn > 0))
                                {
                                    string yakuzaiName = tgtOdrDtl.ItemName;
                                    if (tgtOdrDtl.SyohoKbn == 3)
                                    {
                                        // 一般名処方の場合は一般名をセット
                                        yakuzaiName = tgtOdrDtl.IpnName;
                                    }

                                    yakuzaiName += $"　{CIUtil.ToWide(tgtOdrDtl.Suryo.ToString())}{tgtOdrDtl.UnitName}";

                                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, yakuzaiName, autoAdd: 1, isnodspRyosyu: 1);
                                    _common.Wrk.wrkSinKouiDetails.Last().CdKbn = "F";
                                }

                                tgtOdrDtls = _common.Odr.FilterOdrDetailByRpNo(rpno, rpedano).FindAll(p => p.ItemCd == ItemCdConst.CommentSippuYoryo).OrderBy(p => p.RowNo).ToList();
                                foreach (OdrDtlTenModel tgtOdrDtl in tgtOdrDtls.FindAll(p => p.ItemCd.StartsWith("8") && p.ItemCd.Length == 9 && p.MasterSbt == "C"))
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(tgtOdrDtl, _common.Odr.GetOdrCmt(tgtOdrDtl), autoAdd: 1, isNodspRece: 0, isNodspPaperRece: 0, isNodspRyosyu: 1);
                                    _common.Wrk.wrkSinKouiDetails.Last().CdKbn = "F";
                                }
                            }

                            tgtOdrDtls = _common.Odr.FilterOdrDetailByRpNo(rpno, rpedano).FindAll(p => p.ItemCd != ItemCdConst.CommentSippuYoryo).OrderBy(p => p.RowNo).ToList();

                            foreach (OdrDtlTenModel tgtOdrDtl in tgtOdrDtls.FindAll(p => p.ItemCd.StartsWith("8") && p.ItemCd.Length == 9 && p.MasterSbt == "C"))
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(tgtOdrDtl, _common.Odr.GetOdrCmt(tgtOdrDtl), autoAdd: 1, isNodspRece: 0, isNodspPaperRece: 0, isNodspRyosyu: 1);
                                _common.Wrk.wrkSinKouiDetails.Last().CdKbn = "F";
                            }
                        }
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 一般名処方加算
        /// </summary>
        /// <param name="pid">保険パターンID</param>
        /// <param name="hid">保険ID</param>
        /// <param name="santeiKbn">算定区分</param>
        private void IpnNameKasan(int pid, int hid, int santeiKbn)
        {
            // 手オーダー確認
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;
            //int pid = _common.syosaiPid;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(IpnKasanls);

            if (minIndex >= 0)
            {
                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.SonotaSyohoSen, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                }

                //オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(IpnKasanls);
                }
            }
            // 処方箋キャンセル、一般名処方キャンセルがオーダーされている場合は自動算定なし
            else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.TouyakuSyohosenCancel) == false &&
                     _common.Odr.ExistOdrDetailByItemCd(ItemCdConst.TouyakuIpnNameCancel) == false)
            {
                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.SonotaSyohoSen, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                odrDtls = _common.Odr.FilterOdrDtlIngaiSyoho();

                var odrDtlIpnKasan1 =
                    odrDtls.FindAll(p =>
                        p.Kasan1 == 1 &&
                        p.SyohoKbn != 0 &&
                        _common.Mst.IsIpnKasanExclude(p.IpnNameCd, p.ItemCd) == false)
                        .GroupBy(p =>
                            new { ipnCd = CIUtil.Copy(p.IpnNameCd, 1, 7), syohoKbn = p.SyohoKbn });
                        //.GroupBy(p =>
                        //    new { ipnCd = p.IpnNameCd, syohoKbn = p.SyohoKbn });
                var odrDtlIpnKasan1Count =
                    odrDtlIpnKasan1.GroupBy(p => p.Key.ipnCd);

                if (odrDtlIpnKasan1 != null &&
                    odrDtlIpnKasan1Count.Count() > 1 &&
                    odrDtlIpnKasan1.Any(p => p.Key.syohoKbn != 3) == false)
                {
                    // 対象薬剤が2種類以上で、一般名処方していない薬剤がない場合、
                    // 一般名処方加算１を算定する
                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.TouyakuIpnName1, autoAdd: 1);
                }
                else if (odrDtls.Any(p => p.SyohoKbn == 3 && p.Kasan2 == 1 && _common.Mst.IsIpnKasanExclude(p.IpnNameCd, p.ItemCd) == false) == true)
                {
                    // 一般名処方している薬剤が1つでも存在する場合、
                    // 一般名処方加算２を算定する
                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.TouyakuIpnName2, autoAdd: 1);
                }
            }
        }

        /// <summary>
        /// 特定疾患処方管理加算
        /// </summary>
        private void Tokusyo()

        {
            bool santei = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;
            int kouiKbn = ReceKouiKbn.Touyaku;
            int sinId = ReceSinId.Syoho;
            string syukeiSaki = ReceSyukeisaki.TouyakuSyoho;

            // 特処２からチェック
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Tokusyo2ls);

            //if (minIndex >= 0)
            while(minIndex >= 0)
            {
                _common.Wrk.AppendNewWrkSinRpInf(kouiKbn, sinId, odrDtls.First().SanteiKbn);
                _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "F"));

                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                    {
                        // 算定回数エラー
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                    }
                    else if (Tokusyo2ls.Contains(odrDtl.ItemCd) && CheckTokusyoSanteiKaisu(odrDtl, Tokusyo2ls) == false)
                    {
                        // 算定回数エラー
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                    }
                    //else if(_common.Odr.FilterOdrInfByKouiKbnRange(
                    //    OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo).Any(p=>p.SanteiKbn == SanteiKbnConst.Santei) == false)
                    // 算定外も含めるらしい
                    else if (IsOrderSyohoryo == false && Tokusyo2ls.Contains(odrDtl.ItemCd) && 
                        _common.Odr.FilterOdrInfByKouiKbnRange(
                        OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo).Any() == false)
                    {
                        // 処方オーダーなし
                        _common.AppendCalcLog(2,
                            string.Format(FormatConst.NotSyohoOdr, odrDtl.ReceName));
                    }
                    else
                    {
                        if (odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo2Syoho)
                        {
                            if (_common.Odr.ExistInnaiSyoho == false && _common.Odr.ExistIngaiSyoho)
                            {
                                // 院内なし、院外あり→処方箋料
                                _common.Odr.UpdateOdrDtlItemCd(odrDtl, ItemCdConst.TouyakuTokuSyo2Syohosen);
                            }
                        }
                        else if (odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo2Syohosen)
                        {
                            if (_common.Odr.ExistIngaiSyoho == false && _common.Odr.ExistInnaiSyoho)
                            {
                                // 院外なし、院内あり→処方料
                                _common.Odr.UpdateOdrDtlItemCd(odrDtl, ItemCdConst.TouyakuTokuSyo2Syoho);
                            }
                        }

                        if(odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo2Syoho)
                        {
                            kouiKbn = ReceKouiKbn.Touyaku;
                            sinId = ReceSinId.Syoho;
                            syukeiSaki = ReceSyukeisaki.TouyakuSyoho;
                        }
                        else if (odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo2Syohosen)
                        {
                            kouiKbn = ReceKouiKbn.Sonota;
                            sinId = ReceSinId.Sonota;
                            syukeiSaki = ReceSyukeisaki.SonotaSyohoSen;
                        }

                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        // 年齢加算
                        _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);
                        // コメント自動発生
                        _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, _common.Odr.odrDtlls);

                        _common.Wrk.wrkSinRpInfs.Last().SinKouiKbn = kouiKbn;
                        _common.Wrk.wrkSinRpInfs.Last().SinId = sinId;
                        _common.Wrk.wrkSinKouis.Last().SyukeiSaki = syukeiSaki;
                        santei = true;
                    }
                }

                //オーダーから削除
                _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Tokusyo2ls);
            }

            ////オーダーから削除
            //while (minIndex >= 0)
            //{
            //    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
            //    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Tokusyo2ls);
            //}

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Tokusyo1ls);

            if (santei == false)
            {
                //(odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Tokusyo1ls);

                //if (minIndex >= 0)
                while (minIndex >= 0)
                {
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, odrDtls.First().SanteiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.SonotaSyohoSen, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "F"));

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                        {
                            // 算定回数エラー
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                        }
                        else if (Tokusyo1ls.Contains(odrDtl.ItemCd) && CheckTokusyoSanteiKaisu(odrDtl, Tokusyo1ls) == false)
                        {
                            // 算定回数エラー
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                        }
                        //else if (_common.Odr.FilterOdrInfByKouiKbnRange(
                        //    OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo).Any(p => p.SanteiKbn == SanteiKbnConst.Santei) == false)
                        // 算定外も含めるらしい・・・。
                        else if (IsOrderSyohoryo == false && Tokusyo1ls.Contains(odrDtl.ItemCd) && 
                            _common.Odr.FilterOdrInfByKouiKbnRange(
                            OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo).Any() == false)
                        {
                            // 処方オーダーなし
                            _common.AppendCalcLog(2,
                                string.Format(FormatConst.NotSyohoOdr, odrDtl.ReceName));
                        }
                        else
                        {
                            if (odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo1Syoho)
                            {
                                if (_common.Odr.ExistInnaiSyoho == false && _common.Odr.ExistIngaiSyoho)
                                {
                                    // 院内処方なし、院外処方あり → 処方箋料
                                    _common.Odr.UpdateOdrDtlItemCd(odrDtl, ItemCdConst.TouyakuTokuSyo1Syohosen);
                                }
                            }
                            else if (odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo1Syohosen)
                            {
                                if (_common.Odr.ExistIngaiSyoho == false && _common.Odr.ExistInnaiSyoho)
                                {
                                    // 院外処方なし、院内処方あり → 処方料
                                    _common.Odr.UpdateOdrDtlItemCd(odrDtl, ItemCdConst.TouyakuTokuSyo1Syoho);
                                }
                            }

                            if (odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo1Syoho)
                            {
                                kouiKbn = ReceKouiKbn.Touyaku;
                                sinId = ReceSinId.Syoho;
                                syukeiSaki = ReceSyukeisaki.TouyakuSyoho;
                            }
                            else if(odrDtl.ItemCd == ItemCdConst.TouyakuTokuSyo1Syohosen)
                            {
                                kouiKbn = ReceKouiKbn.Sonota;
                                sinId = ReceSinId.Sonota;
                                syukeiSaki = ReceSyukeisaki.SonotaSyohoSen;
                            }

                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            // 年齢加算
                            _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);
                            // コメント自動発生
                            _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, _common.Odr.odrDtlls);

                            _common.Wrk.wrkSinRpInfs.Last().SinKouiKbn = kouiKbn;
                            _common.Wrk.wrkSinRpInfs.Last().SinId = sinId;
                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = syukeiSaki;

                            santei = true;
                        }
                    }

                    //オーダーから削除
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);

                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Tokusyo1ls);
                }

            }

            //オーダーから削除
            while (minIndex >= 0)
            {
                _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Tokusyo1ls);
            }
        }

        /// <summary>
        /// 特定疾患処方管理加算の算定回数チェック
        /// </summary>
        /// <param name="odrDtl">オーダーされた特処の項目情報（エラーログに名称を載せるのに使用）</param>
        /// <param name="checkItemCds">チェックする特処の項目コードリスト</param>
        /// <returns></returns>
        private bool CheckTokusyoSanteiKaisu(OdrDtlTenModel odrDtl, List<string> checkItemCds)
        {
            bool ret = true;

            double count = _common.GetSanteiCountSinYm(checkItemCds, _common.sinDate);
            count += _common.Wrk.WrkCountSindayIncThisRaiin(checkItemCds);

            List<TenMstModel> tenMsts = new List<TenMstModel>();
            int maxCount = 0;
            string itemNames = "";
            foreach (string getItemCd in checkItemCds)
            {
                tenMsts = _common.Mst.GetTenMst(getItemCd);
                if (maxCount == 0)
                {
                    maxCount = tenMsts.First().MaxCount;
                }

                if (itemNames != "")
                {
                    itemNames += " または、";
                }
                itemNames += tenMsts.First().Name;
            }

            if (tenMsts.Any())
            {
                if (count >= maxCount)
                {
                    ret = false;

                    _common.AppendCalcLog(2,
                        string.Format(FormatConst.SanteiJyogenMulti + FormatConst.NotSantei,
                        odrDtl.ReceName, itemNames, maxCount, "1月"));
                }
            }

            return ret;
        }

        /// <summary>
        /// 処方料を算定する
        /// kouseiとtazaiNaifuku両方trueの場合は、kouiseを優先する
        /// いずれもfalseの場合は、処方箋料（その他）を算定する
        /// </summary>
        /// <param name="kousei">true-向精神薬多剤投与に該当、処方料（向精神薬多剤投与）</param>
        /// <param name="tazaiNaifuku">true-多剤投与に該当、処方料（７種類以上内服薬又は向精神薬長期処方）</param>
        private void Syohoryo(bool kousei, bool tazaiNaifuku)
        {
            bool santei = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;
            int pid = _common.syosaiPid;
            int hid = _common.syosaiHokenId;
            int santeiKbn = _common.syosaiSanteiKbn;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Syohoryols);

            if (minIndex >= 0)
            {
                while (minIndex >= 0)
                {
                    if (_common.Odr.ExistInnaiSyoho && _common.Odr.ExistIngaiSyoho == false)
                    {
                        // 手オーダーあり
                        pid = odrDtls.First().HokenPid;
                        hid = odrDtls.First().HokenId;
                        santeiKbn = odrDtls.First().SanteiKbn;

                        // Rp、行為を追加
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, ReceSinId.Syoho, santeiKbn);
                        _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.TouyakuSyoho, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            if (_common.sinDate >= 20200401 &&
                                new string[] { ItemCdConst.TouyakuSyohoKouseiChoki, ItemCdConst.TouyakuSyohoryoKouseiChoki }.Contains(odrDtl.ItemCd) &&
                                (kousei == true || tazaiNaifuku == true))
                            {
                                // 2020/04/01以降で、オーダーされた処方料が向精神薬長期投与だった場合で、向精神薬多剤投与か内服薬多剤投与に当たる場合、
                                // 向精神薬多剤投与か内服薬多剤投与に置き換える
                                string itemCd = "";

                                if (kousei)
                                {
                                    itemCd = ItemCdConst.TouyakuSyohoKousei;
                                }
                                else if (tazaiNaifuku)
                                {
                                    itemCd = ItemCdConst.TouyakuSyohoNaifuku;
                                }

                                if (itemCd != "")
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                                    // 年齢加算
                                    _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(itemCd);

                                    santei = true;
                                }
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                // 年齢加算
                                _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);
                            }

                            santei = true;
                        }
                    }
                    else
                    {
                        _common.AppendCalcLog(2, "'処方料' は、院内処方がないか、院外処方があるため、算定できません。");
                    }

                    //オーダーから削除
                    //while (minIndex >= 0)
                    //{
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Syohoryols);
                    //}
                }

            }
            else if(_common.Odr.ExistInnaiSyoho && _common.Odr.ExistIngaiSyoho == false)
            {
                // 自動算定

                if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.TouyakuSyohoCancel) == false)
                {
                    // pidを特定する
                    (pid, hid, santeiKbn) = GetPid(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo, 0);

                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, ReceSinId.Syoho, santeiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.TouyakuSyoho, isNodspPaperRece: 1, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                    string itemCd = "";

                    if (kousei)
                    {
                        itemCd = ItemCdConst.TouyakuSyohoKousei;
                    }
                    else if (tazaiNaifuku)
                    {
                        itemCd = ItemCdConst.TouyakuSyohoNaifuku;
                    }
                    else
                    {
                        itemCd = ItemCdConst.TouyakuSyohoSonota;
                    }

                    if (itemCd != "")
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                        // 年齢加算
                        _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(itemCd);

                        santei = true;
                    }
                }
            }

            if(santei)
            {
                // 加算の算定処理

                // 外来後発医薬品使用体制加算
                GairaiKohatu(pid, hid, santeiKbn);

                // 手オーダーの処方料の加算項目をまとめる（向精神薬調整連携加算（処方料）等）
                SyohoKasan();

                // 麻薬加算　※別Ｒｐになるので注意
                MayakuKasan(ItemCdConst.TouyakuMayakuSyoho, pid, hid, santeiKbn);
            }
        }

        /// <summary>
        /// 指定の行為で使用している保険組み合わせの中から適当な保険組み合わせＩＤを返す
        /// </summary>
        /// <param name="kouiMin">検索する行為コードの下限</param>
        /// <param name="kouiMax">検索する行為コードの上限</param>
        /// <param name="inOut">院内院外区分</param>
        /// <param name="mayaku">true-麻薬のみ検索</param>
        /// <param name="tokusyo">true-特処除く</param>
        /// <returns></returns>
        private (int pid, int hid, int santeiKbn) GetPid(int kouiMin, int kouiMax, int inOut, bool mayaku = false, bool tokusyo = false)
        {
            int retPid = _common.syosaiPid;
            int retHid = _common.syosaiHokenId;
            int retSanteiKbn = _common.syosaiSanteiKbn;

            List<int> madokuKbn = new List<int> { 1, 2, 3, 5 };

            List<string> Tokusyols = new List<string>();
            if(inOut == 0)
            {
                Tokusyols.AddRange(TokusyoSyohols);
            }
            else
            {
                Tokusyols.AddRange(TokusyoSyohosenls);
            }

            if(mayaku == false)
            {
                madokuKbn.Add(0);
            }

            if (_systemConfigProvider.GetDrugPid() == 0)
            {
                // 公費優先
                List<int> hokenPids = new List<int>();
                List<OdrDtlTenModel> odrDtls = new List<OdrDtlTenModel>();

                if(tokusyo)
                {
                    odrDtls = _common.Odr.FilterOdrInfDtlByKouiKbnRange(kouiMin, kouiMax, inOut);
                }
                else
                {
                    odrDtls = _common.Odr.FilterOdrInfDtlByKouiKbnRangeItemCd(kouiMin, kouiMax, inOut, Tokusyols);
                }

                if (tokusyo == false)
                {
                    odrDtls =
                        odrDtls.FindAll(p =>
                            (p.DrugKbn > 0 && madokuKbn.Contains(p.MadokuKbn)) ||
                            (string.IsNullOrEmpty(p.ItemCd) == false && p.ItemCd.StartsWith("Z")) ||
                             Tokusyols.Contains(p.ItemCd))
                        .ToList();
                }

                hokenPids = odrDtls
                .Select(p => p.HokenPid)
                .Distinct()
                .OrderByDescending(p => p)
                .ToList();

                (retPid, retHid) = _common.GetPriorityPid(hokenPids);

                // 算定区分の確認
                // 保険→自費の順に並べ替え
                // 自費算定の薬剤しかない場合、自費算定にする
                odrDtls = odrDtls.OrderBy(p => p.SanteiKbn).ToList();

                //if(odrDtls.Any(p=>p.SanteiKbn == SanteiKbnConst.Santei))
                if (odrDtls.Any())
                {
                    retSanteiKbn = odrDtls.First().SanteiKbn;
                }
            }
            else
            {
                // オーダー優先
                //IEnumerable<OdrDtlTenModel> odrDtls =
                //_common.Odr.FilterOdrInfDtlByKouiKbnRange(kouiMin, kouiMax, inOut)
                //.Where(p=>p.DrugKbn > 0 && madokuKbn.Contains(p.MadokuKbn))
                //.OrderBy(p => p.OdrKouiKbn)
                //.ThenBy(p => p.SortNo)
                //.ThenBy(p => p.RpNo)
                //.ThenBy(p => p.RpEdaNo);
                List<OdrDtlTenModel> odrDtls = new List<OdrDtlTenModel>();
                if (tokusyo == false)
                {
                    odrDtls = _common.Odr.FilterOdrInfDtlByKouiKbnRange(kouiMin, kouiMax, inOut);
                }
                else
                {
                    odrDtls = _common.Odr.FilterOdrInfDtlByKouiKbnRangeItemCd(kouiMin, kouiMax, inOut, Tokusyols);
                }

                odrDtls =
                    odrDtls
                    .OrderBy(p => p.OdrKouiKbn)
                    .ThenBy(p => p.SortNo)
                    .ThenBy(p => p.RpNo)
                    .ThenBy(p => p.RpEdaNo)
                    .ToList();

                if (odrDtls.Any())
                {
                    retPid = 0;
                    retHid = 0;
                    retSanteiKbn = 0;

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if(_common.IsBuntenKohi(odrDtl.HokenPid))
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
                    odrDtls = odrDtls.OrderBy(p => p.SanteiKbn).ToList();

                    //if (odrDtls.Any(p => p.SanteiKbn == SanteiKbnConst.Santei))
                    if (odrDtls.Any())
                    {
                        retSanteiKbn = odrDtls.First().SanteiKbn;
                    }
                }
            }

            if (retSanteiKbn == SanteiKbnConst.SanteiGai) { retSanteiKbn = SanteiKbnConst.Santei; }

            return (retPid, retHid, retSanteiKbn);
        }

        /// <summary>
        /// 麻薬等加算（処方料）
        /// </summary>
        /// <param name="itemCd">算定する麻毒加算項目の診療行為コード</param>
        /// <param name="pid">保険パターンID</param>
        /// <param name="hid">保険ID</param>
        /// <param name="santeiKbn">算定区分</param>
        private void MayakuKasan(string itemCd, int pid, int hid, int santeiKbn)
        {
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCd);

            if (minIndex >= 0)
            {
                while (minIndex >= 0)
                {
                    // 手オーダーあり
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, ReceSinId.Madoku, odrDtls.First().SanteiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.TouyakuMadoku, isNodspPaperRece: 1, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "F"));

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                        // 年齢加算
                        _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);
                    }

                    //オーダーから削除
                    //while (minIndex >= 0)
                    //{
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCd);
                    //}
                }
            }
            else if (_common.Odr.ExistMadokuSyoho(0))
            {
                // 自動算定
                (pid, hid, santeiKbn) = GetPid(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo, 0, true);

                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, ReceSinId.Madoku, santeiKbn);
                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.TouyakuMadoku, isNodspPaperRece: 1, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                // 年齢加算
                _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(itemCd);
            }
        }

        /// <summary>
        /// 外来後発医薬品使用体制加算
        /// </summary>
        private void GairaiKohatu(int pid, int hid, int santeiKbn)
        {
            //まず、手オーダーがあるか検索する
            string itemCd = "";
            List<string> gairaiKohatuls =
                new List<string>
                {
                    ItemCdConst.TouyakuGairaiKohatu1,
                    ItemCdConst.TouyakuGairaiKohatu2,
                    ItemCdConst.TouyakuGairaiKohatu3
                };

            for (int i = 0; i < gairaiKohatuls.Count; i++)
            {
                if (_common.Odr.ExistOdrDetailByItemCd(gairaiKohatuls[i]))
                {
                    itemCd = gairaiKohatuls[i];
                    break;
                }
            }

            if (itemCd != "")
            {
                //見つかったら、算定する

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCd);

                //if (minIndex >= 0)
                while (minIndex >= 0)
                {
                    //_common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.TouyakuSyoho, isNodspPaperRece: 1, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "F"));
                    if (odrDtls.First().IsKihonKoumoku)
                    {
                        _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.TouyakuSyoho, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "F"));
                    }

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                        }
                        else
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            // 年齢加算
                            _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);
                        }
                    }
                //}

                ////オーダーから削除
                //while (minIndex >= 0)
                //{
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCd);
                }
            }
            else
            {
                // 自動算定チェック
                if(_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.TouyakuGairaiKohatuCancel) == false)
                {
                    for (int i = 0; i < gairaiKohatuls.Count; i++)
                    {
                        if (_common.Mst.ExistAutoSantei(gairaiKohatuls[i]))
                        {
                            if (_common.CheckSanteiKaisu(gairaiKohatuls[i], santeiKbn, 1) == 2)
                            {
                                // 算定回数マスタのチェックにより算定不可
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.TouyakuSyoho, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                                _common.Wrk.AppendNewWrkSinKouiDetail(gairaiKohatuls[i], autoAdd: 1);
                                // 年齢加算
                                _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(gairaiKohatuls[i]);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 処方料の加算項目
        /// 手オーダーを処方料と同一Rpにまとめるための処理
        /// </summary>
        private void SyohoKasan()
        {
            List<string> kasanItemCdls =
                new List<string>
                {
                    // 向精神薬調整連携加算（処方料）
                    ItemCdConst.TouyakuKouseiRenkeiSyoho
                };

            foreach (string itemCd in kasanItemCdls)
            {
                //手オーダーがあるか検索する
                if (_common.Odr.ExistOdrDetailByItemCd(itemCd))
                {
                    //見つかったら、算定する                    

                    List<OdrDtlTenModel> odrDtls;
                    int minIndex = 0;
                    int itemCnt = 0;

                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCd);

                    //if (minIndex >= 0)
                    while(minIndex >= 0)
                    {
                        if(odrDtls.First().IsKihonKoumoku)
                        {
                            _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.TouyakuSyoho, cdKbn: _common.GetCdKbn(odrDtls.First().SanteiKbn, "F"));
                        }

                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                            {
                                // 算定回数マスタのチェックにより算定不可
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                // 年齢加算
                                _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);
                            }
                        }
                    //}

                    ////オーダーから削除
                    //while (minIndex >= 0)
                    //{
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCd);
                    }
                }
            }
        }

        /// <summary>
        /// 調剤料を算定する
        /// </summary>
        private void Chozairyo()
        {
            bool santei = false;
            int pid = _common.syosaiPid;
            int hid = _common.syosaiHokenId;
            int santeiKbn = _common.syosaiSanteiKbn;

            for (int i = 0; i < Chozairyols.Count; i++)
            {
                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Chozairyols[i]);

                if (minIndex >= 0)
                {
                    while (minIndex >= 0)
                    {
                        // 手オーダーあり
                        pid = odrDtls.First().HokenPid;
                        hid = odrDtls.First().HokenId;
                        santeiKbn = odrDtls.First().SanteiKbn;

                        int sinId = 0;
                        string syukeisaki = "";

                        if (i == 0)
                        {
                            sinId = ReceSinId.Naifuku;
                            syukeisaki = ReceSyukeisaki.TouyakuNaiCyozai;
                        }
                        else
                        {
                            sinId = ReceSinId.Gaiyo;
                            syukeisaki = ReceSyukeisaki.TouyakuGaiCyozai;
                        }

                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, sinId, santeiKbn);
                        _common.Wrk.AppendNewWrkSinKoui(pid, hid, syukeisaki, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                            {
                                // 算定回数マスタのチェックにより算定不可
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                // 年齢加算
                                _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);

                                santei = true;
                            }
                        }

                        //オーダーから削除
                        //while (minIndex >= 0)
                        //{
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Chozairyols[i]);
                        //}
                    }
                }
                else if (_common.Odr.ExistInnaiSyoho && _common.Odr.ExistIngaiSyoho == false)
                {
                    // 自動算定
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.TouyakuChozaiCancel) == false)
                    {

                        int kouiMin;
                        int kouiMax;
                        int sinId;
                        string syukeiSaki;
                        string itemCd = "";

                        if (i == 0)
                        {
                            kouiMin = OdrKouiKbnConst.Naifuku;
                            kouiMax = OdrKouiKbnConst.Tonpuku;
                            sinId = ReceSinId.Naifuku;
                            syukeiSaki = ReceSyukeisaki.TouyakuNaiCyozai;
                            itemCd = ItemCdConst.TouyakuChozaiNaiTon;
                        }
                        else
                        {
                            kouiMin = OdrKouiKbnConst.Gaiyo;
                            kouiMax = OdrKouiKbnConst.Gaiyo;
                            sinId = ReceSinId.Gaiyo;
                            syukeiSaki = ReceSyukeisaki.TouyakuGaiCyozai;
                            itemCd = ItemCdConst.TouyakuChozaiGai;
                        }

                        if (_common.Odr.FilterOdrInfByKouiKbnRange(kouiMin, kouiMax).Any())
                        {
                            // 自動算定

                            // pidを特定する
                            (pid, hid, santeiKbn) = GetPid(kouiMin, kouiMax, 0, false, true);

                            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, sinId, santeiKbn);
                            _common.Wrk.AppendNewWrkSinKoui(pid, hid, syukeiSaki, isNodspPaperRece: 1, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                            if (itemCd != "")
                            {
                                if (_common.CheckSanteiKaisu(itemCd, santeiKbn, 1) == 2)
                                {
                                    // 算定回数マスタのチェックにより算定不可
                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                                    // 年齢加算
                                    _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(itemCd);

                                    santei = true;
                                }
                            }
                        }
                    }
                }
            }

            if (santei)
            {
                // 麻薬加算
                MayakuKasan(ItemCdConst.TouyakuMayakuChozai, pid, hid, santeiKbn);
            }
        }

        /// <summary>
        /// 調剤基本料を算定する
        /// </summary>
        private void ChoKiryo()
        {
            bool santei = false;
            int pid = _common.syosaiPid;
            int hid = _common.syosaiHokenId;
            int santeiKbn = _common.syosaiSanteiKbn;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.TouyakuChoKi);

            if (minIndex >= 0)
            {
                while(minIndex >= 0)
                { 
                    // 手オーダーあり
                    if (_systemConfigProvider.GetDrugPid() == 0)
                    {
                        // 公費優先
                    }
                    else
                    {
                        // オーダー優先
                        pid = odrDtls.First().HokenPid;
                        hid = odrDtls.First().HokenId;
                        santeiKbn = odrDtls.First().SanteiKbn;
                    }
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, ReceSinId.Cyoki, santeiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.TouyakuSyoho, cdKbn: _common.GetCdKbn(santeiKbn, "F"));

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                        }
                        else
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            // 年齢加算
                            _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, _common.Odr.odrDtlls);

                            santei = true;
                        }
                    }

                    ////オーダーから削除
                    //while (minIndex >= 0)
                    //{
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.TouyakuChoKi);
                }


            }
            else if (_common.Odr.ExistInnaiSyoho && _common.Odr.ExistIngaiSyoho == false && _common.Odr.ExistIngaiSyohoInMonth == false)
            {
                // 自動算定
                if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.TouyakuChoKiCancel) == false)
                {
                    // 取り消し項目なし

                    if (_common.Mst.ExistAutoSantei(ItemCdConst.TouyakuChoKi))
                    {
                        // 自動算定設定あり

                        // pidを特定する
                        (pid, hid, santeiKbn) = GetPid(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo, 0);

                        if (_common.CheckSanteiKaisu(ItemCdConst.TouyakuChoKi, santeiKbn, 1) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
                        }
                        else
                        {
                            IEnumerable<OdrInfModel> odrInf =
                                _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo)
                                .OrderBy(p => p.OdrKouiKbn)
                                .ThenBy(p => p.SortNo)
                                .ThenBy(p => p.RpNo)
                                .ThenBy(p => p.RpEdaNo);

                            if (odrInf != null && odrInf.Any())
                            {
                                // pidを特定する
                                //(pid, hid, santeiKbn) = GetPid(OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Gaiyo, 0);

                                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Touyaku, ReceSinId.Cyoki, santeiKbn);
                                _common.Wrk.AppendNewWrkSinKoui(pid, hid, ReceSyukeisaki.TouyakuChoKi, isNodspPaperRece: 1, cdKbn: _common.GetCdKbn(santeiKbn, "F"));
                                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.TouyakuChoKi, autoAdd: 1);

                                // 年齢加算
                                _common.Wrk.AppendNewWrkSinKouiDetailAgeKasan(ItemCdConst.TouyakuChoKi);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 薬剤料を算定する
        /// </summary>
        /// <param name="kouiKbn">行為区分（内服 or 頓服 or 外用 or 自己注）</param>
        /// <param name="kouseiTazai">true-向精神薬多剤投与に該当</param>
        /// <param name="naifukuTazai">true-多剤投与に該当</param>
        private void Yakuzai(int kouiKbn, bool kouseiTazai, bool naifukuTazai)
        {
            IEnumerable<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            bool commentSkipFlg = false;
            bool commentDelFlg = false;

            List<(int hokenPid, int hokenId, int santeiKbn)> pidList = _common.Odr.GetPidList(kouiKbn);

            for (int i = 0; i < pidList.Count; i++)
            {
                // 処方のRpを取得
                filteredOdrInf = null;

                filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(kouiKbn, kouiKbn)
                                    .Where(p => p.HokenPid == pidList[i].hokenPid && p.SanteiKbn == pidList[i].santeiKbn)
                                    .OrderBy(p => p.SortNo)
                                    .ThenBy(p => p.RpNo)
                                    .ThenBy(p => p.RpEdaNo);

                if (filteredOdrInf != null && filteredOdrInf.Any())
                {
                    bool firstSinryoKoui;

                    bool kouseiExist = false;
                    bool tazaiExist = false;
                    int kouseiPid = 0;
                    int naifukuPid = 0;

                    int kouseiCmt = 0;
                    int tazaiCmt = 0;

                    int receKouiKbn = 0;
                    int receSinId = 0;

                    string syukeiSaki = "";
                    if (kouiKbn == OdrKouiKbnConst.Naifuku)
                    {
                        // 内服薬
                        receKouiKbn = ReceKouiKbn.Naifuku;
                        receSinId = ReceSinId.Naifuku;
                        syukeiSaki = ReceSyukeisaki.TouyakuNaiYakuzai;
                    }
                    else if (kouiKbn == OdrKouiKbnConst.Tonpuku)
                    {
                        // 頓服薬
                        receKouiKbn = ReceKouiKbn.Tonpuku;
                        receSinId = ReceSinId.Tonpuku;
                        syukeiSaki = ReceSyukeisaki.TouyakuTon;
                    }
                    else if (kouiKbn == OdrKouiKbnConst.Gaiyo)
                    {
                        // 外用薬
                        receKouiKbn = ReceKouiKbn.Gaiyo;
                        receSinId = ReceSinId.Gaiyo;
                        syukeiSaki = ReceSyukeisaki.TouyakuGaiYakuzai;
                    }
                    else if (kouiKbn == OdrKouiKbnConst.JikoCyu)
                    {
                        // 自己注射
                        receKouiKbn = ReceKouiKbn.Zaitaku;
                        receSinId = ReceSinId.Zaitaku;
                        syukeiSaki = ReceSyukeisaki.ZaiYakuzai;
                    }

                    foreach (OdrInfModel odrInf in filteredOdrInf)
                    {
                        string cdKbn = "F";

                        if (kouiKbn == OdrKouiKbnConst.JikoCyu)
                        {
                            // 自己注射
                            if (odrInf.InoutKbn == 0)
                            {
                                receKouiKbn = ReceKouiKbn.Zaitaku;
                                receSinId = ReceSinId.Zaitaku;
                                syukeiSaki = ReceSyukeisaki.ZaiYakuzai;
                                cdKbn = "C";
                            }
                            else
                            {
                                receKouiKbn = ReceKouiKbn.JikoCyu;
                                receSinId = ReceSinId.JikoCyu;
                                syukeiSaki = ReceSyukeisaki.TouyakuJikoCyu;
                            }
                        }

                        commentSkipFlg = false;
                        commentDelFlg = false;

                        // 行為に紐づく詳細を取得
                        firstSinryoKoui = true;

                        filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(receKouiKbn, receSinId, odrInf.SanteiKbn);

                        // 集計先は、後で内容により変更する                    
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn));

                        bool kouseiExistRp = false;
                        bool tazaiExistRp = false;

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (odrDtl.IsYoho)
                            {
                                // 用法 
                                //commentSkipFlg = false;

                                if (odrInf.InoutKbn == 1)
                                {
                                    // 院外の場合、用法のコメントは出力する
                                    commentSkipFlg = false;

                                    if (_systemConfigProvider.GetOutDrugYohoDsp() == 1)
                                    {
                                        // 院外の場合、コメントとして記録
                                        //string comment = odrDtl.ItemName;
                                        string comment = odrDtl.ReceName;
                                        if (kouiKbn == OdrKouiKbnConst.Naifuku || kouiKbn == OdrKouiKbnConst.Tonpuku)
                                        {
                                            comment += "（" + odrDtl.Suryo.ToString() + odrDtl.UnitName + "）";
                                        }

                                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, comment);
                                        _common.Wrk.wrkSinKouiDetails.Last().IsNodspRece = NoDspConst.NoDspReceden;
                                    }
                                }
                                else
                                {
                                    //// 院内の場合、用法コメントは出力しない
                                    //commentSkipFlg = true;
                                    //commentDelFlg = true;

                                    // やっぱり出力するみたい
                                    //commentSkipFlg = false;
                                    //commentDelFlg = false;

                                    //場合によるらしい。内部データの項目の種別の入り具合で出力するかしないかが変わっている
                                    //しかし、それでは分かりにくいので、HAYABUSAは一律出力しないことにする
                                    //後々、問題になるようなら、院内処方用法コメントを出力するかどうかのオプションを作る(2020/08/14)
                                    // オプション追加(2022/03/02)

                                    if (_systemConfigProvider.GetInDrugYohoComment() == 1)
                                    {
                                        commentSkipFlg = false;
                                        commentDelFlg = false;
                                    }
                                    else
                                    {
                                        commentSkipFlg = true;
                                        commentDelFlg = true;
                                    }
                                }

                                // オーダーから削除
                                _common.Odr.odrDtlls.RemoveAll(p => p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && p.RowNo == odrDtl.RowNo);
                            }
                            else if (commentDelFlg && _common.IsCommentItemCd(odrDtl.ItemCd) && !odrDtl.IsSelectComment)
                            {
                                // オーダーから削除
                                _common.Odr.odrDtlls.RemoveAll(p => p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && p.RowNo == odrDtl.RowNo);
                            }
                            else if (odrDtl.IsYorCommentItem(commentSkipFlg) || odrDtl.IsSelectComment)
                            {
                                commentSkipFlg = false;
                                commentDelFlg = false;

                                if(odrDtl.IsComment)
                                {
                                    if (odrInf.InoutKbn == 1 && ((_systemConfigProvider.GetOutDrugYohoDsp() == 0) || (odrDtl.ItemCd.StartsWith("8") && odrDtl.ItemCd.Length == 9)))
                                    {
                                        // コメントで院外処方で院外処方のコメントを出さない設定の時、または、コメント項目の時は無視
                                    }
                                    else
                                    {
                                        if (odrInf.InoutKbn == 1)
                                        {
                                            // 院外は強制表示
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isNodspRece: 0);
                                        }
                                        else
                                        {
                                            // 院内はオーダー時の設定に従う
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isNodspRece: odrDtl.IsNodspRece);
                                        }
                                    }
                                }
                                else
                                {
                                    // 薬剤

                                    if (firstSinryoKoui == false)
                                    {
                                        if (odrDtl.OdrKouiKbn == OdrKouiKbnConst.Naifuku)
                                        {
                                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "F"));
                                        }
                                        else
                                        {
                                            firstSinryoKoui = false;
                                        }
                                    }
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    if (odrDtl.OdrKouiKbn == OdrKouiKbnConst.Gaiyo)
                                    {
                                        // 外用薬の場合、調整
                                        _common.Wrk.wrkSinKouiDetails.Last().Suryo =
                                            _common.Wrk.wrkSinKouiDetails.Last().Suryo * odrInf.DaysCnt;
                                        _common.Wrk.wrkSinKouis.Last().Count = 1;
                                    }

                                    // 逓減項目有無判定
                                    if (odrInf.InoutKbn == 0)
                                    {
                                        // 院内処方の場合
                                        if (kouseiTazai && odrDtl.KouseisinKbn > 0)
                                        {
                                            kouseiExist = true;
                                            kouseiExistRp = true;
                                            if (kouseiPid == 0)
                                            {
                                                kouseiPid = odrInf.HokenPid;
                                            }
                                        }
                                        else if (naifukuTazai && odrDtl.OdrKouiKbn == OdrKouiKbnConst.Naifuku)
                                        {
                                            tazaiExist = true;
                                            tazaiExistRp = true;
                                            if (naifukuPid == 0)
                                            {
                                                naifukuPid = odrInf.HokenPid;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _common.Wrk.wrkSinKouiDetails.Last().TenZero = true;
                                    }
                                }

                                // オーダーから削除
                                _common.Odr.odrDtlls.RemoveAll(p =>p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && p.RowNo == odrDtl.RowNo);
                            }
                            else if (odrDtl.IsTorCommentItem(commentSkipFlg))
                            {
                                // 特材が薬剤の代わりにオーダーされることもある

                                if (kouiKbn == OdrKouiKbnConst.JikoCyu && odrInf.InoutKbn == 0)
                                {
                                    // 自己注射で院内の場合、算定しない
                                    commentDelFlg = true;
                                }
                                else
                                {
                                    commentSkipFlg = false;
                                    commentDelFlg = false;

                                    if (firstSinryoKoui == false)
                                    {
                                        if (odrDtl.OdrKouiKbn == OdrKouiKbnConst.Naifuku)
                                        {
                                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "F"));
                                        }
                                    }
                                    else
                                    {
                                        firstSinryoKoui = false;
                                    }
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    if (odrInf.InoutKbn == 1)
                                    {
                                        // 院外処方の場合は0点にする
                                        _common.Wrk.wrkSinKouiDetails.Last().TenZero = true;
                                    }
                                }
                                
                                // オーダーから削除
                                _common.Odr.odrDtlls.RemoveAll(p => p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && p.RowNo == odrDtl.RowNo);
                            }
                            else if (odrDtl.IsSorCommentItem(commentSkipFlg))
                            {
                                if (kouiKbn == OdrKouiKbnConst.JikoCyu && odrInf.InoutKbn == 0 && odrDtl.ItemCd == ItemCdConst.ChusyaJikocyu)
                                {
                                    // 自己注射で院内の場合、算定しない
                                }
                                else
                                {
                                    // 自己注くらいしかない
                                    commentSkipFlg = false;
                                    commentDelFlg = false;

                                    if (firstSinryoKoui == false)
                                    {
                                        if (odrDtl.OdrKouiKbn == OdrKouiKbnConst.Naifuku)
                                        {
                                            _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeiSaki, count: odrInf.DaysCnt, inoutKbn: odrInf.InoutKbn, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "F"));
                                        }
                                    }
                                    else
                                    {
                                        firstSinryoKoui = false;
                                    }
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                }
                                // オーダーから削除
                                _common.Odr.odrDtlls.RemoveAll(p => p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && p.RowNo == odrDtl.RowNo);
                            }
                            else
                            {
                                if(commentDelFlg && odrDtl.IsComment)
                                {
                                    // コメント削除指示がある場合、コメントはオーダーから削除
                                    _common.Odr.odrDtlls.RemoveAll(p => p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && p.RowNo == odrDtl.RowNo);
                                }

                                commentSkipFlg = true;
                                commentDelFlg = false;
                            }

                            // 逓減項目有無判定
                            if (odrInf.InoutKbn == 0 &&
                                kouiKbn != OdrKouiKbnConst.JikoCyu &&
                                new int[] { OdrKouiKbnConst.Naifuku, OdrKouiKbnConst.Tonpuku, OdrKouiKbnConst.Gaiyo }.Contains(odrInf.OdrKouiKbn))
                            {
                                // 院内処方の場合
                                if (kouseiTazai && odrDtl.KouseisinKbn > 0)
                                {
                                    kouseiExist = true;
                                    kouseiExistRp = true;
                                }
                                else if (naifukuTazai && kouiKbn == OdrKouiKbnConst.Naifuku)
                                {
                                    // 内服薬のみ逓減する
                                    tazaiExist = true;
                                    tazaiExistRp = true;
                                }
                            }
                        }

                        if (kouseiExistRp)
                        {
                            // （精減）                        
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.TouyakuYakuGenKousei, autoAdd: 1, isNodspRece: kouseiCmt);
                            //kouseiCmt = 2;　レセ出力時に対処する
                        }

                        if (tazaiExistRp)
                        {
                            // （減）
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.TouyakuYakuGenNaifuku, autoAdd: 1, isNodspRece: tazaiCmt);
                            //tazaiCmt = 2;　レセ出力時に対処する
                        }

                    }

                    //if (kouseiExist || tazaiExist)
                    //{
                    //    // Rp
                    //    _common.Wrk.AppendNewWrkSinRpInf(receKouiKbn, receSinId, pidList[i].santeiKbn);

                    //    // 行為
                    //    _common.Wrk.AppendNewWrkSinKoui(pidList[i].hokenPid, pidList[i].hokenId, syukeiSaki, count: 1, inoutKbn: 0, cdKbn: _common.GetCdKbn(pidList[i].santeiKbn, "F"));
                    //}

                    if (kouseiExist)
                    {
                        // Rp
                        _common.Wrk.AppendNewWrkSinRpInf(receKouiKbn, receSinId, pidList[i].santeiKbn);

                        // 行為
                        _common.Wrk.AppendNewWrkSinKoui(pidList[i].hokenPid, pidList[i].hokenId, syukeiSaki, count: 1, inoutKbn: 0, cdKbn: _common.GetCdKbn(pidList[i].santeiKbn, "F"));
                        
                        // 薬剤料逓減（８０／１００）（向精神薬多剤投与）                        
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.TouyakuTeigenKousei, autoAdd: 1);
                    }

                    if (tazaiExist)
                    {
                        // Rp
                        _common.Wrk.AppendNewWrkSinRpInf(receKouiKbn, receSinId, pidList[i].santeiKbn);

                        // 行為
                        _common.Wrk.AppendNewWrkSinKoui(pidList[i].hokenPid, pidList[i].hokenId, syukeiSaki, count: 1, inoutKbn: 0, cdKbn: _common.GetCdKbn(pidList[i].santeiKbn, "F"));

                        // 薬剤料逓減（９０／１００）（内服薬）
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.TouyakuTeigenNaifuku, autoAdd: 1);
                    }

                    _common.Wrk.CommitWrkSinRpInf();
                }
            }
        }

        /// <summary>
        /// 自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {            
            _common.CalculateJihi(
                OdrKouiKbnConst.TouyakuMin,
                OdrKouiKbnConst.TouyakuMax,
                ReceKouiKbn.Touyaku,
                ReceSinId.SyohoSonota,
                ReceSyukeisaki.TouyakuSyoho,
                "JS");
        }
    }
}
