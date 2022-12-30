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
    /// 初再診
    /// </summary>
    public class IkaCalculateOdrToWrkSyosaiViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 初診の時間等加算項目リスト
        /// </summary>
        private List<string> SyosinKasanls =
            new List<string>
            {
                ItemCdConst.SyosinNinpu,
                ItemCdConst.SyosinNinpuJikangai,
                ItemCdConst.SyosinNinpuKyujitu,
                ItemCdConst.SyosinNinpuSinya,
                ItemCdConst.SyosinNinpuJikangaiToku,
                ItemCdConst.SyosinNinpuYakanToku,
                ItemCdConst.SyosinNinpuKyujituToku,
                ItemCdConst.SyosinNinpuSinyaToku,
                ItemCdConst.SyosinNyuJikangai,
                ItemCdConst.SyosinNyuKyujitu,
                ItemCdConst.SyosinNyuSinya,
                ItemCdConst.SyosinNyuJikangaiToku,

                ItemCdConst.SyosinJikangai,
                ItemCdConst.SyosinKyujitu,
                ItemCdConst.SyosinSinya,
                ItemCdConst.SyosinYasou,

                ItemCdConst.SyosinSyouniNyuYakan,
                ItemCdConst.SaisinSyouniNyuYakan,
                ItemCdConst.SyosinSyouniGairaiYakan,
                ItemCdConst.SaisinSyouniGairaiYakan,
                ItemCdConst.SyosinSyouniNyuKyujitu,
                ItemCdConst.SaisinSyouniNyuKyujitu,
                ItemCdConst.SyosinSyouniGairaiKyujitu,
                ItemCdConst.SaisinSyouniGairaiKyujitu,
                ItemCdConst.SyosinSyouniNyuSinya,
                ItemCdConst.SaisinSyouniNyuSinya,
                ItemCdConst.SyosinSyouniGairaiSinya,
                ItemCdConst.SaisinSyouniGairaiSinya
            };

        /// <summary>
        /// 初診時間外項目のリスト
        /// </summary>
        private List<string> SyosinJikangails =
            new List<string>
            {
                ItemCdConst.SyosinJikangai,
                ItemCdConst.SyosinNinpuJikangai,
                ItemCdConst.SyosinNinpuJikangaiToku,
                ItemCdConst.SyosinNinpuYakanToku,
                ItemCdConst.SyosinNyuJikangai,
                ItemCdConst.SyosinNyuJikangaiToku,
                ItemCdConst.SyosinSyouniNyuYakan,
            };

        /// <summary>
        /// 初診休日加算項目のリスト
        /// </summary>
        private List<string> SyosinKyujituls =
            new List<string>
            {
                ItemCdConst.SyosinKyujitu,
                ItemCdConst.SyosinNinpuKyujitu,
                ItemCdConst.SyosinNinpuKyujituToku,
                ItemCdConst.SyosinNyuKyujitu,
                ItemCdConst.SyosinSyouniNyuKyujitu,
            };

        /// <summary>
        /// 初診深夜加算項目のリスト
        /// </summary>
        private List<string> SyosinSinyals =
            new List<string>
            {
                ItemCdConst.SyosinSinya,
                ItemCdConst.SyosinNinpuSinya,
                ItemCdConst.SyosinNinpuSinyaToku,
                ItemCdConst.SyosinNyuSinya,
                ItemCdConst.SyosinSyouniNyuSinya,
            };

        /// <summary>
        /// 初診夜間早朝加算のリスト
        /// </summary>
        private List<string> SyosinYasouls =
            new List<string>
            {
                ItemCdConst.SyosinYasou
            };
        /// <summary>
        /// 再診の時間等加算項目リスト
        /// </summary>
        //private List<string> SaisinKasanls =
        //    new List<string>
        //    {
        //        ItemCdConst.SaisinNyuJikangai,
        //        ItemCdConst.SaisinNyuKyujitu,
        //        ItemCdConst.SaisinNyuSinya,
        //        ItemCdConst.SaisinNyuJikangaiToku
        //    };
        private List<string> SaisinKasanls =
            new List<string>
            {
                ItemCdConst.SaisinJikangai,
                ItemCdConst.SaisinKyujitu,
                ItemCdConst.SaisinSinya,

                ItemCdConst.SaisinNyuJikangai,
                ItemCdConst.SaisinNyuKyujitu,
                ItemCdConst.SaisinNyuSinya,
                ItemCdConst.SaisinNyuJikangaiToku,

                ItemCdConst.SyosinSyouniNyuYakan,
                ItemCdConst.SaisinSyouniNyuYakan,
                ItemCdConst.SyosinSyouniGairaiYakan,
                ItemCdConst.SaisinSyouniGairaiYakan,
                ItemCdConst.SyosinSyouniNyuKyujitu,
                ItemCdConst.SaisinSyouniNyuKyujitu,
                ItemCdConst.SyosinSyouniGairaiKyujitu,
                ItemCdConst.SaisinSyouniGairaiKyujitu,
                ItemCdConst.SyosinSyouniNyuSinya,
                ItemCdConst.SaisinSyouniNyuSinya,
                ItemCdConst.SyosinSyouniGairaiSinya,
                ItemCdConst.SaisinSyouniGairaiSinya
            };
        /// <summary>
        /// 再診の時間外加算項目リスト
        /// </summary>
        private List<string> SaisinJikangails =
            new List<string>
            {
                ItemCdConst.SaisinJikangai,
                ItemCdConst.SaisinNyuJikangai,
                ItemCdConst.SaisinNyuJikangaiToku,
                ItemCdConst.SaisinSyouniNyuYakan
            };
        /// <summary>
        /// 再診の休日加算項目リスト
        /// </summary>
        private List<string> SaisinKyujituls =
            new List<string>
            {
                ItemCdConst.SaisinKyujitu,
                ItemCdConst.SaisinNyuKyujitu,
                ItemCdConst.SaisinSyouniNyuKyujitu
            };
        /// <summary>
        /// 再診の深夜加算項目リスト
        /// </summary>
        private List<string> SaisinSinyals =
            new List<string>
            {
                ItemCdConst.SaisinSinya,
                ItemCdConst.SaisinNyuSinya,
                ItemCdConst.SaisinSyouniNyuSinya
            };
        /// <summary>
        /// 再診の摘要欄記載加算項目リスト
        /// </summary>
        private List<string> SaisinTekiyols =
            new List<string>
            {
                ItemCdConst.SaisinNyuJikangaiToku,
                ItemCdConst.SaisinSyouniNyuYakan,
                ItemCdConst.SaisinSyouniNyuKyujitu,
                ItemCdConst.SaisinSyouniNyuSinya,
                ItemCdConst.SaisinYasou
            };
        private List<string> SaisinNinpuls =
            new List<string>
            {
                ItemCdConst.SaisinNinpu,
                ItemCdConst.SaisinNinpuJikangai,
                ItemCdConst.SaisinNinpuJikangaiToku,
                ItemCdConst.SaisinNinpuKyujitu,
                ItemCdConst.SaisinNinpuKyujituToku,
                ItemCdConst.SaisinNinpuSinya,
                ItemCdConst.SaisinNinpuSinyaToku,
                ItemCdConst.SaisinNinpuYakanToku
            };
        /// <summary>
        /// 小児特例夜間加算項目リスト
        /// </summary>
        private List<string> SyouniYakanKasanls =
            new List<string>
            {
                ItemCdConst.SyosinSyouniNyuYakan,
                ItemCdConst.SaisinSyouniNyuYakan,
                ItemCdConst.SyosinSyouniGairaiYakan,
                ItemCdConst.SaisinSyouniGairaiYakan
            };
        /// <summary>
        /// 小児特例休日加算項目リスト
        /// </summary>
        private List<string> SyouniKyujituKasanls =
            new List<string>
            {
                ItemCdConst.SyosinSyouniNyuKyujitu,
                ItemCdConst.SaisinSyouniNyuKyujitu,
                ItemCdConst.SyosinSyouniGairaiKyujitu,
                ItemCdConst.SaisinSyouniGairaiKyujitu
            };
        /// <summary>
        /// 小児特例深夜加算項目リスト
        /// </summary>
        private List<string> SyouniSinyaKasanls =
            new List<string>
            {
                ItemCdConst.SyosinSyouniNyuSinya,
                ItemCdConst.SaisinSyouniNyuSinya,
                ItemCdConst.SyosinSyouniGairaiSinya,
                ItemCdConst.SaisinSyouniGairaiSinya
            };
        /// <summary>
        /// 外来管理加算項目リスト
        /// </summary>
        private List<string> GairaiKanrils =
            new List<string>
            {
                ItemCdConst.GairaiKanriKasan,
                ItemCdConst.GairaiKanriKasanRousai
            };
        /// <summary>
        /// 認知症地域包括診療加算の項目のリスト
        /// </summary>
        List<string> NintiTiikiHoukatuls =
            new List<string>
            {
                    ItemCdConst.SaisinNintiTiikiHoukatu1,
                    ItemCdConst.SaisinNintiTiikiHoukatu2
            };

        /// <summary>
        /// 地域包括診療加算の項目のリスト
        /// </summary>
        List<string> TiikiHoukatuls =
            new List<string>
            {
                    ItemCdConst.SaisinTiikiHoukatu1,
                    ItemCdConst.SaisinTiikiHoukatu2
            };

        /// <summary>
        /// 時間外対応加算リスト
        /// </summary>
        private List<string> JikangaiTaiouls =
            new List<string>
            {
                ItemCdConst.JikangaiTaiou1,
                ItemCdConst.JikangaiTaiou2,
                ItemCdConst.JikangaiTaiou3
            };

        private List<string> SyouniTokureiKasanls;

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkSyosaiViewModel
            (IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _common = common;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 初再診の計算ロジック
        /// </summary>
        public void Calculate()
        {
            const string conFncName = nameof(Calculate);

            SyouniTokureiKasanls = new List<string>();
            SyouniTokureiKasanls.AddRange(SyouniYakanKasanls);
            SyouniTokureiKasanls.AddRange(SyouniKyujituKasanls);
            SyouniTokureiKasanls.AddRange(SyouniSinyaKasanls);

            SyosinKasanls.AddRange(SyouniTokureiKasanls);
            SaisinKasanls.AddRange(SyouniTokureiKasanls);

            _common.syosai = -1;
            _common.jikan = -1;

            // 初再診を確認
            List<OdrDtlTenModel> odrDtlTenModel;

            odrDtlTenModel = _common.Odr.FilterOdrDetailByItemCd(ItemCdConst.SyosaiKihon);
            if (odrDtlTenModel.Any())
            {
                // 1つしかないはず
                _common.syosai = odrDtlTenModel[0].Suryo;

                int coronaPid = 0;
                int coronaHokenKbn = 0;
                int coronaHokenId = 0;
                int coronaSanteiKbn = 0;

                if (_common.syosai != SyosaiConst.Jihi && 
                    CheckSyosaiCorona(ref coronaPid, ref coronaHokenKbn, ref coronaHokenId, ref coronaSanteiKbn))
                {
                    _common.syosai = SyosaiConst.SyosinCorona;
                }

                if (_common.syosai != SyosaiConst.Jihi && !_common.IsRosai && _common.Odr.ExistOdrDetailByItemCd(ItemCdConst.Con_Jouhou))
                {
                    //情報通信機器のダミーがあれば情報通信機器項目に置き換える
                    _common.syosai = JouhouSyosaiKbn;
                }

                if (_common.syosai == SyosaiConst.SyosinCorona)
                {
                    // コロナ初診の場合、オーダーされた項目の設定を採用
                    _common.syosaiPid = coronaPid;
                    _common.syosaiHokenKbn = coronaHokenKbn;
                    _common.syosaiHokenId = coronaHokenId;
                    _common.syosaiSanteiKbn = coronaSanteiKbn;
                }
                else
                {
                    // それ以外は、@SHINの設定を採用
                    _common.syosaiPid = odrDtlTenModel[0].HokenPid;
                    _common.syosaiHokenKbn = odrDtlTenModel[0].HokenKbn;
                    _common.syosaiHokenId = odrDtlTenModel[0].HokenId;
                    _common.syosaiSanteiKbn = odrDtlTenModel[0].SanteiKbn;
                }
            }

            //if (_common.syosai != SyosaiConst.None && _common.syosai != SyosaiConst.Jihi)
            //{
            odrDtlTenModel = _common.Odr.FilterOdrDetailByItemCd(ItemCdConst.JikanKihon);
            if (odrDtlTenModel.Any())
            {
                // 1つしかないはず
                _common.jikan = odrDtlTenModel[0].Suryo;
            }

            if (new List<double> { SyosaiConst.Syosin, SyosaiConst.Syosin2, SyosaiConst.SyosinCorona, SyosaiConst.SyosinJouhou, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
            {
                // 初診計算ロジック
                CalculateSyosin();

                ////初診その他の項目
                //SyosaiSonota(OdrKouiKbnConst.Syosin, ReceKouiKbn.Syosin, ReceSinId.Syosin, ReceSyukeisaki.Syosin, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

                ////初診自費項目
                //SyosinSonotaJihi();

            }
            //else if (new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.Saisin2, SyosaiConst.SaisinDenwa2 }.Contains(_common.syosai))
            else if (new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.Saisin2, SyosaiConst.SaisinDenwa2, SyosaiConst.SaisinJouhou, SyosaiConst.Saisin2Jouhou }.Contains(_common.syosai))
            {
                // 再診計算ロジック
                CalculateSaisin();

                ////再診のその他の手オーダー項目
                //SyosaiSonota(OdrKouiKbnConst.Saisin, ReceKouiKbn.Saisin, ReceSinId.Saisin, ReceSyukeisaki.Saisin, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

                ////再診の自費算定分
                //SaisinSonotaJihi();
            }
            //else
            //{
            //// 初再診に紐づく加算を削除
            //List<string> DelOrderls = new List<string>();
            //DelOrderls.AddRange(SyosinKasanls);
            //DelOrder(DelOrderls, "初診料");

            //DelOrderls.Clear();
            //DelOrderls.AddRange(GairaiKanrils);
            //DelOrderls.AddRange(NintiTiikiHoukatuls);
            //DelOrderls.AddRange(TiikiHoukatuls);
            //DelOrderls.AddRange(JikangaiTaiouls);
            //DelOrderls.Add(ItemCdConst.MeisaiHakko);
            //DelOrderls.AddRange(SaisinKasanls);

            //DelOrder(DelOrderls, "再診料");

            ////初診その他の項目
            //SyosaiSonota(OdrKouiKbnConst.Syosin, ReceKouiKbn.Syosin, ReceSinId.Syosin, ReceSyukeisaki.Syosin, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
            ////再診のその他の手オーダー項目
            //SyosaiSonota(OdrKouiKbnConst.Saisin, ReceKouiKbn.Saisin, ReceSinId.Saisin, ReceSyukeisaki.Saisin, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            //}

            //初診その他の項目
            SyosaiSonota(OdrKouiKbnConst.Syosin, ReceKouiKbn.Syosin, ReceSinId.Syosin, ReceSyukeisaki.Syosin, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            //初診自費項目
            SyosinSonotaJihi();

            //再診のその他の手オーダー項目
            SyosaiSonota(OdrKouiKbnConst.Saisin, ReceKouiKbn.Saisin, ReceSinId.Saisin, ReceSyukeisaki.Saisin, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            //再診の自費算定分
            SaisinSonotaJihi();

            //初再診その他
            SyosaiSonota(OdrKouiKbnConst.SyosaiSonota, ReceKouiKbn.SyosaiSonota, ReceSinId.SyosaiSonota, ReceSyukeisaki.SyosaiSonota, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
            //}

            //同日再診コメント
            SaisinDojituComment();

            _common.Wrk.CommitWrkSinRpInf();
        }

        /// <summary>
        /// 初診料（新型コロナウイルス感染症・診療報酬上臨時的取扱）がオーダーされているかどうかチェックする
        /// オーダーされている場合、ここで削除する
        /// </summary>
        /// <returns></returns>
        private bool CheckSyosaiCorona(ref int pid, ref int hokenKbn, ref int hokenId, ref int santeiKbn)
        {
            bool ret = false;

            if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.SyosinCorona))
            {
                if (_common.IsRosai == false)
                {
                    ret = true;
                }

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.SyosinCorona);

                // オーダーから削除
                while (minIndex >= 0)
                {
                    foreach(OdrDtlTenModel odrDtl in odrDtls)
                    {
                        if(odrDtl.ItemCd == ItemCdConst.SyosinCorona)
                        {
                            pid = odrDtl.HokenPid;
                            hokenKbn = odrDtl.HokenKbn;
                            hokenId = odrDtl.HokenId;
                            santeiKbn = odrDtl.SanteiKbn;

                            break;
                        }
                    }
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.SyosinCorona);
                }
            }

            return ret;
        }

        /// <summary>
        /// 情報通信機器に置き換えた初再診区分
        /// </summary>
        private double JouhouSyosaiKbn
        {
            get
            {
                double ret = _common.syosai;

                switch (_common.syosai)
                {
                    case SyosaiConst.Syosin:     //初診
                        ret = SyosaiConst.SyosinJouhou; break;
                    case SyosaiConst.Syosin2:    //初診２科目
                        ret = SyosaiConst.Syosin2Jouhou; break;
                    case SyosaiConst.Saisin:     //再診
                        ret = SyosaiConst.SaisinJouhou; break;
                    case SyosaiConst.Saisin2:    //再診２科目
                        ret = SyosaiConst.Saisin2Jouhou; break;
                }
                return ret;
            }
        }

        private void DelOrder(List<string> DelOrderls, string syosaiName)
        {
            //加算項目を探す
            List<OdrDtlTenModel> filteredOdrDtls = _common.Odr.FilterOdrDetailByItemCd(DelOrderls);
            bool bSanteiKasan = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(DelOrderls);

            foreach (OdrDtlTenModel odrDtl in odrDtls.FindAll(p => DelOrderls.Contains(p.ItemCd)))
            {
                _common.AppendCalcLog(2, $"'{odrDtl.Name}' は、{syosaiName}を算定していないため、算定できません。");
            }

            // オーダーから削除
            while (minIndex >= 0)
            {
                _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiKanrils);
            }
        }

        /// <summary>
        /// 初診の計算
        /// </summary>
        private void CalculateSyosin()
        {
            const string conFncName = nameof(CalculateSyosin);
            _emrLogger.WriteLogStart( this, conFncName, "");

            //Rp
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syosin, ReceSinId.Syosin, _common.syosaiSanteiKbn);
            //行為
            string syukeiSaki = ReceSyukeisaki.Syosin;
            if (_common.IsRosai)
            {
                syukeiSaki = ReceSyukeisaki.EnSyosin;
            }
            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, syukeiSaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            //初診の基本項目を算定
            SyosinKihon();

            //if (_common.IsRosai)
            //{
            // 行為を分ける
            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.Syosin, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
            //}

            //初診の時間外、乳幼児、妊婦加算算定
            SyosinJikanKasan();

            // 行為を分ける
            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.SyosinSonota, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            //初診のその他加算項目算定
            SyosinSonotaKasan();

            //初診のコメント
            //改正TODO　情報通信機器の場合もコメント必要？
            if (_common.syosai == SyosaiConst.Syosin)
            {
                // 初診の場合、コメントを付けるか判断する
                SyosinJikanNaiComment();
            }

            _common.Wrk.CommitWrkSinRpInf();

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 初診項目を算定する
        /// </summary>
        private void SyosinKihon()
        {
            const string conFncName = nameof(SyosinKihon);

            string itemCd = "";

            switch (_common.syosai)
            {
                case SyosaiConst.Syosin:     //初診
                    itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.Syosin, ItemCdConst.SyosinRousai);
                    break;
                case SyosaiConst.Syosin2:     //初診２科目
                    itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.Syosin2, ItemCdConst.Syosin2Rousai);
                    break;
                case SyosaiConst.SyosinCorona:  // 初診料（新型コロナウイルス感染症・診療報酬上臨時的取扱）
                    itemCd = ItemCdConst.SyosinCorona;
                    break;
                case SyosaiConst.SyosinJouhou:  // 初診料（情報通信機器を用いた場合）
                    itemCd = ItemCdConst.SyosinJouhou;
                    break;
                case SyosaiConst.Syosin2Jouhou:  // 初診料（同一日複数科受診時の２科目）（情報通信機器を用いた場合）
                    itemCd = ItemCdConst.Syosin2Jouhou;
                    break;
            }

            if (itemCd != "")
            {
                _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                if (_common.IsRosai && _common.Wrk.wrkSinKouiDetails.Last().IsEnKoumoku)
                {
                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSyosin;
                }

                if (_common.syosai == SyosaiConst.Syosin2 || _common.syosai == SyosaiConst.Syosin2Jouhou)
                {
                    //2科目の場合は診療科名を追加する
                    if (_common.sinDate >= 20200401)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSyosin2Kame, _common.kaName, autoAdd: 1);
                    }
                    else
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, "（" + _common.kaName + "）", autoAdd: 1);
                    }
                }

                if (new List<double> { SyosaiConst.SyosinJouhou, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                {
                    //情報通信機器の場合はコメントを追加する
                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentOnlineSinryoSyosin, "", autoAdd: 1);
                    if (_common.Odr.ExistIngaiSyoho || _common.Odr.ExistInnaiSyoho)
                    {
                        // 同じ来院に処方がある場合は処方のコメントも追加する
                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentOnlineSyohoSyosin, "", autoAdd: 1);
                    }
                }
            }

        }

        /// <summary>
        /// 初診の時間枠/乳幼児/妊婦加算算定処理
        /// </summary>        
        private void SyosinJikanKasan()
        {
            const string conFncName = nameof(SyosinJikanKasan);

            string itemCd = "";
            string itemCd2 = "";
            string syukeiSaki = ReceSyukeisaki.SyosinJikanNai;

            List<OdrDtlTenModel> filteredOdrDtls = _common.Odr.FilterOdrDetailByItemCd(SyosinKasanls);
            bool bSanteiKasan = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyosinKasanls);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                while (minIndex >= 0)
                {
                    if (_common.IsYoJi == false && odrDtls.Any(p => CIUtil.StrToIntDef(p.MaxAge, 0) == 6))
                    {
                        // 6歳以上算定不可
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.AppendCalcLog(2, odrDtl.ItemName + "は、6歳以上のため、算定できません。");
                        }
                    }
                    else if (!(new List<double> { SyosaiConst.Syosin, SyosaiConst.SyosinCorona, SyosaiConst.SyosinJouhou }.Contains(_common.syosai)))
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.AppendCalcLog(2, odrDtl.ItemName + "は、初診ではないため、算定できません。");
                        }
                    }
                    else
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            // 乳幼児特例加算の置き換え
                            OdrDtlTenModel appendDlt = _common.Odr.CopyOdrDtl(odrDtl);

                            if (SyouniYakanKasanls.Contains(appendDlt.ItemCd))
                            {
                                _common.Odr.UpdateOdrDtlItemCd(appendDlt, ItemCdConst.SyosinSyouniNyuYakan);
                            }
                            else if (SyouniKyujituKasanls.Contains(appendDlt.ItemCd))
                            {
                                _common.Odr.UpdateOdrDtlItemCd(appendDlt, ItemCdConst.SyosinSyouniNyuKyujitu);
                            }
                            else if (SyouniSinyaKasanls.Contains(appendDlt.ItemCd))
                            {
                                _common.Odr.UpdateOdrDtlItemCd(appendDlt, ItemCdConst.SyosinSyouniNyuSinya);
                            }

                            _common.Wrk.AppendNewWrkSinKouiDetail(appendDlt, _common.Odr.GetOdrCmt(appendDlt));

                            if (SyosinJikangails.Contains(appendDlt.ItemCd))
                            {
                                //時間外
                                syukeiSaki = ReceSyukeisaki.SyosinJikanGai;
                            }
                            else if (SyosinKyujituls.Contains(appendDlt.ItemCd))
                            {
                                // 休日
                                syukeiSaki = ReceSyukeisaki.SyosinKyujitu;
                            }
                            else if (SyosinSinyals.Contains(appendDlt.ItemCd))
                            {
                                //深夜
                                syukeiSaki = ReceSyukeisaki.SyosinSinya;
                            }
                            else if (SyosinYasouls.Contains(appendDlt.ItemCd))
                            {
                                //夜間早朝
                                syukeiSaki = ReceSyukeisaki.SyosinYasou;
                            }

                            _common.Wrk.wrkSinKouis.Last().SyukeiSaki = syukeiSaki;
                            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.Syosin, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                        }
                        bSanteiKasan = true;
                    }

                    //オーダーから削除

                    if (odrDtls.Any(p => p.OdrKouiKbn <= 12))
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyosinKasanls);
                    }
                    else
                    {
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SyosinKasanls, minIndex + itemCnt);
                    }
                }
            }

            if (new List<double> { SyosaiConst.Syosin, SyosaiConst.SyosinCorona, SyosaiConst.SyosinJouhou }.Contains(_common.syosai))
            {
                if (!bSanteiKasan)
                {
                    if (_common.IsYoJi && _common.Mst.GetHyoboSyounika() == 1)
                    {
                        // 幼児で小児科標榜あり

                        // 小児特例夜間加算がオーダーされているかチェック
                        bSanteiKasan = JikanKasanOdr(odrDtls, SyouniYakanKasanls, ItemCdConst.SyosinSyouniNyuYakan);
                        if (!bSanteiKasan)
                        {
                            // 休日加算
                            bSanteiKasan = JikanKasanOdr(odrDtls, SyouniKyujituKasanls, ItemCdConst.SyosinSyouniNyuKyujitu);
                            if (bSanteiKasan)
                            {
                                syukeiSaki = ReceSyukeisaki.SyosinKyujitu;
                            }
                        }

                        if (!bSanteiKasan)
                        {
                            // 深夜加算
                            bSanteiKasan = JikanKasanOdr(odrDtls, SyouniSinyaKasanls, ItemCdConst.SyosinSyouniNyuSinya);
                            if (bSanteiKasan)
                            {
                                syukeiSaki = ReceSyukeisaki.SyosinSinya;
                            }
                        }
                    }
                }

                if (!bSanteiKasan)
                {
                    itemCd = "";
                    itemCd2 = "";
                    if (_common.jikan == JikanConst.JikanNai)
                    {
                        // 時間内                        
                        itemCd = ChoiceItemCdNyuNinpu("", ItemCdConst.SyosinNyu, ItemCdConst.SyosinNinpu);
                    }
                    else if (_common.jikan == JikanConst.JikanGai)
                    {
                        //時間外
                        itemCd = ChoiceItemCdNyuNinpu(ItemCdConst.SyosinJikangai, ItemCdConst.SyosinNyuJikangai, ItemCdConst.SyosinNinpuJikangai);
                        syukeiSaki = ReceSyukeisaki.SyosinJikanGai;
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        //休日
                        itemCd = ChoiceItemCdNyuNinpu(ItemCdConst.SyosinKyujitu, ItemCdConst.SyosinNyuKyujitu, ItemCdConst.SyosinNinpuKyujitu);
                        syukeiSaki = ReceSyukeisaki.SyosinKyujitu;
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        //深夜
                        itemCd = ChoiceItemCdNyuNinpu(ItemCdConst.SyosinSinya, ItemCdConst.SyosinNyuSinya, ItemCdConst.SyosinNinpuSinya);
                        syukeiSaki = ReceSyukeisaki.SyosinSinya;
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        //夜間早朝
                        if (_common.Mst.GetHyoboSyounika() == 1)
                        {
                            // 小児科標榜あり

                            if (_common.IsYoJi)
                            {
                                //幼児（6歳未満）
                                itemCd = ChoiceItemCdTime(ItemCdConst.SyosinSyouniNyuYakan, ItemCdConst.SyosinSyouniNyuKyujitu, ItemCdConst.SyosinSyouniNyuSinya);

                                if (itemCd == ItemCdConst.SyosinSyouniNyuYakan)
                                {
                                    // 夜間（=夜間早朝）
                                    syukeiSaki = ReceSyukeisaki.SyosinYasou;
                                }
                                else if (itemCd == ItemCdConst.SyosinSyouniNyuKyujitu)
                                {
                                    // 休日
                                    syukeiSaki = ReceSyukeisaki.SyosinKyujitu;
                                }
                                else if (itemCd == ItemCdConst.SyosinSyouniNyuSinya)
                                {
                                    // 深夜
                                    syukeiSaki = ReceSyukeisaki.SyosinSinya;
                                }
                            }
                        }
                        else if (_common.Mst.GetHyoboSanka() == 1)
                        {
                            // 産科標榜あり

                            if (_common.IsNinpu)
                            {
                                //妊婦
                                itemCd = ChoiceItemCdTime(ItemCdConst.SyosinNinpuYakanToku, ItemCdConst.SyosinNinpuSinyaToku, ItemCdConst.SyosinNinpuKyujituToku);

                                if (itemCd == ItemCdConst.SyosinNinpuYakanToku)
                                {
                                    // 夜間（=夜間早朝）
                                    syukeiSaki = ReceSyukeisaki.SyosinYasou;
                                }
                                else if (itemCd == ItemCdConst.SyosinNinpuSinyaToku)
                                {
                                    // 休日
                                    syukeiSaki = ReceSyukeisaki.SyosinKyujitu;
                                }
                                else if (itemCd == ItemCdConst.SyosinNinpuKyujituToku)
                                {
                                    // 深夜
                                    syukeiSaki = ReceSyukeisaki.SyosinSinya;
                                }
                            }
                        }

                        if (itemCd == "")
                        {
                            //夜間・早朝等加算
                            itemCd = ItemCdConst.SyosinYasou;
                            itemCd2 = ChoiceItemCdNyuNinpu("", ItemCdConst.SyosinNyu, ItemCdConst.SyosinNinpu);
                            syukeiSaki = ReceSyukeisaki.SyosinYasou;
                        }
                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        // 特例夜間
                        if (_common.IsYoJi)
                        {
                            itemCd = ItemCdConst.SyosinSyouniNyuYakan;
                        }
                        else
                        {
                            itemCd = ItemCdConst.SyosinYasou;
                        }
                        syukeiSaki = ReceSyukeisaki.SyosinYasou;
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        // 特例休日
                        if (_common.IsYoJi)
                        {
                            itemCd = ItemCdConst.SyosinSyouniNyuKyujitu;
                        }
                        else
                        {
                            itemCd = ItemCdConst.SyosinKyujitu;
                        }

                        syukeiSaki = ReceSyukeisaki.SyosinKyujitu;
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        // 特例深夜
                        if (_common.IsYoJi)
                        {
                            itemCd = ItemCdConst.SyosinSyouniNyuSinya;
                        }
                        else
                        {
                            itemCd = ItemCdConst.SyosinSinya;
                        }

                        syukeiSaki = ReceSyukeisaki.SyosinSinya;
                    }

                    if (itemCd != "")
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
                        bSanteiKasan = true;
                    }

                    if (itemCd2 != "")
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(itemCd2, autoAdd: 1);
                    }
                }

                _common.Wrk.wrkSinKouis.Last().SyukeiSaki = syukeiSaki;
            }
        }

        /// <summary>
        /// 初診その他の加算
        /// </summary>
        private void SyosinSonotaKasan()
        {
            const string conFncName = nameof(SyosinSonotaKasan);

            // 機能強化加算
            SyosinKinoukyoka();

            // 乳幼児感染加算
            SyosinNyuyojiKansen();

            // 医科外来等感染症対策実施加算（初診料）
            SyosinKansenTaisaku();

            //外来感染対策向上加算
            SyosinKansenKojo();

            //連携強化加算
            SyosinRenkeiKyoka();

            //サーベイランス強化加算
            SyosinSurveillance();

            // 初診料と同一Rpにまとめる加算（手オーダー専用）
            SyosinManuralKansen();

            // 初診料とと異なる保険組み合わせの手オーダーを算定する
            SyosinManuralKansenManual();
        }
        /// <summary>
        /// 機能強化加算
        /// </summary>
        private void SyosinKinoukyoka()
        {
            const string conFncName = nameof(SyosinKinoukyoka);

            // 機能強化加算

            //加算項目を探す
            List<string> KinouKyokals =
                new List<string>
                {
                    ItemCdConst.SyosinKinoKyoka
                };

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;
            bool bSanteiKasan = false;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KinouKyokals);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                if (odrDtls.First().HokenPid == _common.syosaiPid)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                    //オーダーから削除
                    while (minIndex >= 0)
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KinouKyokals);
                    }
                }
                bSanteiKasan = true;
            }

            if (!bSanteiKasan)
            {
                if (new double[] { SyosaiConst.SyosinCorona, SyosaiConst.Syosin2, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                {
                    // コロナ電話初診、初診２科目の場合は算定不可
                }
                else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KinoKyokaCancel) == false)
                {
                    if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinKinoKyoka))
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyosinKinoKyoka, autoAdd: 1);
                    }
                }
            }
        }
        /// <summary>
        /// 乳幼児感染加算
        /// </summary>
        private void SyosinNyuyojiKansen()
        {
            const string conFncName = nameof(SyosinNyuyojiKansen);

            // 機能強化加算

            if (_common.Age < 6 && _common.sinDate >= 20201215 && _common.sinDate <= 20220331)
            {
                //加算項目を探す
                List<string> NyuyojiKansenls =
                    new List<string>
                    {
                        ItemCdConst.SyosinNyuyojiKansen,
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NyuyojiKansenls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NyuyojiKansenls);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (!bSanteiKasan)
                {
                    if (_common.syosai == SyosaiConst.SyosinCorona)
                    {
                        // コロナ電話初診の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.NyuyojiKansenCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinNyuyojiKansen))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyosinNyuyojiKansen, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 医科外来等感染症対策実施加算（初診料）
        /// </summary>
        private void SyosinKansenTaisaku()
        {
            const string conFncName = nameof(SyosinKansenTaisaku);

            // 機能強化加算

            if (_common.sinDate >= 20210401 && _common.sinDate <= 20210930)
            {
                //加算項目を探す
                List<string> KansenTaisakuKasanls =
                    new List<string>
                    {
                        ItemCdConst.SyosinKansenTaisaku,
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenTaisakuKasanls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenTaisakuKasanls);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (!bSanteiKasan)
                {
                    if (_common.syosai == SyosaiConst.SyosinCorona)
                    {
                        // コロナ電話初診の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenTaisakuCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.KansenTaisaku))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyosinKansenTaisaku, autoAdd: 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 外来感染対策向上加算（初診）
        /// </summary>
        private void SyosinKansenKojo()
        {
            const string conFncName = nameof(SyosinKansenKojo);
            if (_common.sinDate >= 20220401)
            {
                bool santeiJoken = true;

                if (_common.CheckSanteiKaisu(ItemCdConst.SyosinKansenKojo, 0, 0) == 2)
                {
                    // 算定回数マスタのチェックにより算定不可
                    santeiJoken = false;
                }

                // 外来感染対策向上加算（初診）

                //加算項目を探す
                List<string> KansenKojols =
                    new List<string>
                    {
                    ItemCdConst.SyosinKansenKojo
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenKojols);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        if (santeiJoken)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenKojols);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (santeiJoken && !bSanteiKasan)
                {
                    //if (new double[] { SyosaiConst.SyosinCorona, SyosaiConst.Syosin2, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                    if (new double[] { SyosaiConst.Syosin2, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                    {
                        // コロナ電話初診、初診２科目の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenKojoCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinKansenKojo))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyosinKansenKojo, autoAdd: 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 連携強化加算（初診）
        /// </summary>
        private void SyosinRenkeiKyoka()
        {
            const string conFncName = nameof(SyosinRenkeiKyoka);
            if (_common.sinDate >= 20220401)
            {
                bool santeiJoken = true;

                if (_common.CheckSanteiKaisu(ItemCdConst.SyosinRenkeiKyoka, 0, 0) == 2)
                {
                    // 算定回数マスタのチェックにより算定不可
                    santeiJoken = false;
                }

                // 連携強化加算（初診）

                //加算項目を探す
                List<string> RenkeiKyokals =
                    new List<string>
                    {
                    ItemCdConst.SyosinRenkeiKyoka
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(RenkeiKyokals);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        if (santeiJoken)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(RenkeiKyokals);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (santeiJoken && !bSanteiKasan)
                {
                    //if (new double[] { SyosaiConst.SyosinCorona, SyosaiConst.Syosin2, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                    if (new double[] { SyosaiConst.Syosin2, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                    {
                        // コロナ電話初診、初診２科目の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.RenkeiKyokaCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinRenkeiKyoka))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyosinRenkeiKyoka, autoAdd: 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// サーベイランス強化加算（初診）
        /// </summary>
        private void SyosinSurveillance()
        {
            const string conFncName = nameof(SyosinSurveillance);
            if (_common.sinDate >= 20220401)
            {
                bool santeiJoken = true;

                if (_common.CheckSanteiKaisu(ItemCdConst.SyosinSurveillance, 0, 0) == 2)
                {
                    // 算定回数マスタのチェックにより算定不可
                    santeiJoken = false;
                }

                //サーベイランス強化加算（初診）

                //加算項目を探す
                List<string> Surveillancels =
                    new List<string>
                    {
                    ItemCdConst.SyosinSurveillance
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Surveillancels);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        if (santeiJoken)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Surveillancels);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (santeiJoken && !bSanteiKasan)
                {
                    //if (new double[] { SyosaiConst.SyosinCorona, SyosaiConst.Syosin2, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                    if (new double[] { SyosaiConst.Syosin2, SyosaiConst.Syosin2Jouhou }.Contains(_common.syosai))
                    {
                        // コロナ電話初診、初診２科目の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.SurveillanceCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinSurveillance))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SyosinSurveillance, autoAdd: 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初診料と同一Rpにまとめる加算（手オーダー専用）
        /// 自動算定のある項目は別
        /// </summary>
        private void SyosinManuralKansen()
        {
            const string conFncName = nameof(SyosinManuralKansen);

            //加算項目を探す
            List<string> KasanItemls =
                new List<string>
                {
                    // 二類感染症患者入院診療加算（電話等初診料・診療報酬上臨時的取扱）
                    ItemCdConst.Syosin2RuiKansen,
                    // 電子的保健医療情報活用加算（初診）
                    ItemCdConst.SyosinDensiHoken,
                    // 電子的保健医療情報活用加算（初診）（診療情報等の取得が困難等）
                    ItemCdConst.SyosinDensiHokenKonnan,
                    // 医療情報・システム基盤整備体制充実加算１（初診）
                    ItemCdConst.SyosinIryoJyohoKiban1,
                    // 医療情報・システム基盤整備体制充実加算２（初診）
                    ItemCdConst.SyosinIryoJyohoKiban2,

                };

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KasanItemls);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                if (odrDtls.First().HokenPid == _common.syosaiPid)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        // 算定回数チェック
                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
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
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KasanItemls);
                    }
                }
            }
        }
        private void SyosinManuralKansenManual()
        {
            const string conFncName = nameof(SyosinManuralKansenManual);

            //加算項目を探す
            List<List<string>> KasanlsList =
                new List<List<string>>
                {
                    // 機能強化加算
                    new List<string>
                    {
                        ItemCdConst.SyosinKinoKyoka
                    },
                    // 乳幼児感染予防策加算（初診料・診療報酬上臨時的取扱）
                    new List<string>
                    {
                        ItemCdConst.SyosinNyuyojiKansen,
                    },
                    // 医科外来等感染症対策実施加算（初診料）
                    new List<string>
                    {
                        ItemCdConst.SyosinKansenTaisaku,
                    },
                    // 二類感染症患者入院診療加算（電話等初診料・診療報酬上臨時的取扱）
                    new List<string>
                    {
                        ItemCdConst.Syosin2RuiKansen,
                    },
                    // 外来感染対策向上加算（初診）
                    new List<string>
                    {
                        ItemCdConst.SyosinKansenKojo,
                    },
                    // 連携強化加算（初診）
                    new List<string>
                    {
                        ItemCdConst.SyosinRenkeiKyoka,
                    },
                    // サーベイランス強化加算（初診）
                    new List<string>
                    {
                        ItemCdConst.SyosinSurveillance,
                    },
                    // 電子的保健医療情報活用加算（初診）
                    new List<string>
                    {
                        ItemCdConst.SyosinDensiHoken,
                    },
                    // 電子的保健医療情報活用加算（初診）（診療情報等の取得が困難等）
                    new List<string>
                    {
                        ItemCdConst.SyosinDensiHokenKonnan,
                    },
                    // 医療情報・システム基盤整備体制充実加算１（初診）
                    new List<string>
                    {
                        ItemCdConst.SyosinIryoJyohoKiban1,
                    },
                    // 医療情報・システム基盤整備体制充実加算２（初診）
                    new List<string>
                    {
                        ItemCdConst.SyosinIryoJyohoKiban2,
                    }
                };
            foreach (List<string> Kasanls in KasanlsList)
            {
                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Kasanls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    if (odrDtls.First().HokenPid != _common.syosaiPid)
                    {
                        //Rp
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syosin, ReceSinId.Syosin, _common.syosaiSanteiKbn);
                        //行為
                        string syukeiSaki = ReceSyukeisaki.Syosin;
                        if (_common.IsRosai)
                        {
                            syukeiSaki = ReceSyukeisaki.EnSyosin;
                        }
                        _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, syukeiSaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                    }

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        // 算定回数チェック
                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
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
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Kasanls);
                    }
                }
            }
        }
        /// <summary>
        /// 初再診の残り項目を処理する
        /// </summary>
        /// <param name="odrKouiKbn">オーダー行為区分</param>
        /// <param name="receKouiKbn">レセ行為区分</param>
        /// <param name="sinId">診療識別</param>
        /// <param name="cdKbn">コード区分</param>
        private void SyosaiSonota(int odrKouiKbn, int receKouiKbn, int sinId, string syukeisaki, string cdKbn)
        {
            const string conFncName = nameof(SyosaiSonota);

            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbn(odrKouiKbn);
            foreach (OdrInfModel odrInf in filteredOdrInf)
            {
                filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                if (filteredOdrDtl.Any())
                {
                    int santeiKbn = odrInf.SanteiKbn;

                    if (_common.syosai == SyosaiConst.Jihi)
                    {
                        santeiKbn = 2;
                    }

                    _common.Wrk.AppendNewWrkSinRpInf(receKouiKbn, sinId, santeiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn));

                    int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                    // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                    bool commentSkipFlg = (firstItem == 3);
                    // 最初のコメント以外の項目であることを示すフラグ
                    bool firstSinryoKoui = true;

                    foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                    {
                        if (odrDtl.IsJihi)
                        {
                            commentSkipFlg = true;
                        }
                        else if (odrDtl.IsComment && commentSkipFlg == true)
                        {
                            // コメントで読み飛ばしの場合
                        }
                        else if (odrDtl.TyuCd == "1101" && odrDtl.TyuSeq != "0" &&
                            !(new List<double> { SyosaiConst.Syosin, SyosaiConst.SyosinCorona, SyosaiConst.SyosinJouhou }.Contains(_common.syosai)) &&
                            _common.Odr.ExistOdrDetailByTyuCd("1101", "0") == false)
                        {
                            // 初診がないのに初診に紐づく加算をオーダーしている場合、算定不可
                            _common.AppendCalcLog(2, $"'{odrDtl.Name}' は、初診料を算定していないため、算定できません。");
                        }
                        else if (((odrDtl.TyuCd == "1201" && odrDtl.TyuSeq != "0") || odrDtl.GairaiKanriKbn == 2) &&
                            //_common.syosai != SyosaiConst.Saisin && _common.syosai != SyosaiConst.SaisinDenwa &
                            _common.syosai != SyosaiConst.Saisin && _common.syosai != SyosaiConst.SaisinJouhou && _common.syosai != SyosaiConst.SaisinDenwa &
                            _common.Odr.ExistOdrDetailByTyuCd("1201", "0") == false)
                        {
                            // 再診 or 電話再診がないのに再診に紐づく加算をオーダーしている場合、算定不可
                            _common.AppendCalcLog(2, $"'{odrDtl.Name}' は、再診料を算定していないため、算定できません。");
                        }
                        // 算定回数チェック
                        //else if ((odrDtl.TenMst == null || odrDtl.TenMst.SinKouiKbn > 12 || (odrDtl.TenMst.CdKbn == "A" && odrDtl.TenMst.CdKbnno >= 3)) &&
                        else if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                        {
                            // 算定回数マスタのチェックにより算定不可
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: 1);
                        }
                        else if (odrDtl.ItemCd == ItemCdConst.OnlineSinryo &&
                            _common.SanteiCount(_common.MonthsBefore(_common.sinDate, 2), CIUtil.GetLastDateOfMonth(_common.MonthsBefore(_common.sinDate, 2)), ItemCdConst.OnlineSinryo) >= 1 &&
                            _common.SanteiCount(_common.MonthsBefore(_common.sinDate, 1), CIUtil.GetLastDateOfMonth(_common.MonthsBefore(_common.sinDate, 1)), ItemCdConst.OnlineSinryo) >= 1)
                        {
                            // オンライン診療料 過去2ヶ月連続で算定されている場合は算定不可
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: 1);
                            _common.AppendCalcLog(2, string.Format(FormatConst.NotSanteiReason, odrDtl.ItemName, "2カ月連続で算定している"));
                        }
                        else if (_common.CheckAge(odrDtl) == 2)
                        {
                            // 年齢チェックにより算定不可
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                        }
                        else
                        {
                            if (odrDtl.IsKihonKoumoku)
                            {
                                if (firstSinryoKoui)
                                {
                                    firstSinryoKoui = false;
                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, syukeisaki, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, cdKbn));
                                }

                                #region '時間外加算が手オーダーされている場合の集計先を変更'
                                if (SaisinJikangails.Contains(odrDtl.ItemCd))
                                {
                                    // 時間外関連の項目が存在する場合
                                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SaisinJikangai;
                                }
                                else if (SaisinKyujituls.Contains(odrDtl.ItemCd))
                                {
                                    // 休日関連の項目が存在する場合
                                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SaisinKyujitu;
                                }
                                else if (SaisinSinyals.Contains(odrDtl.ItemCd))
                                {
                                    // 深夜関連の項目が存在する場合
                                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SaisinSinya;
                                }
                                else if (odrDtl.ItemCd == ItemCdConst.SaisinYasou)
                                {
                                    // 夜間・早朝等加算
                                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SaisinJikangai;
                                }
                                #endregion
                            }

                            if (_common.IsRosai)
                            {
                                // 労災の初再診項目の場合、集計先を変えておく
                                if (new string[]
                                    {
                                        ItemCdConst.SyosinRousai,
                                        ItemCdConst.Syosin2Rousai
                                    }.Contains(odrDtl.ItemCd))
                                {
                                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSyosin;
                                }
                                else if (
                                    new string[]
                                    {
                                        ItemCdConst.SaisinRousai,
                                        ItemCdConst.Saisin2Rousai,
                                        ItemCdConst.SaisinDenwaRousai,
                                        ItemCdConst.SaisinDenwa2Rousai,
                                        ItemCdConst.SaisinDojituRousai
                                    }.Contains(odrDtl.ItemCd))
                                {
                                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSyosin;
                                }
                            }

                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            //if (odrDtl.ItemCd == ItemCdConst.Syosin2 || odrDtl.ItemCd == ItemCdConst.Saisin2 || odrDtl.ItemCd == ItemCdConst.SaisinDenwa2)
                            if (
                                    new string[]
                                    {
                                        ItemCdConst.Syosin2,
                                        ItemCdConst.Syosin2Jouhou,
                                        ItemCdConst.Saisin2,
                                        ItemCdConst.SaisinDenwa2,
                                        ItemCdConst.Saisin2Jouhou
                                    }.Contains(odrDtl.ItemCd))
                            {
                                //2科目の場合は診療科名を追加する
                                if (_common.sinDate >= 20200401)
                                {
                                    string itemcd = ItemCdConst.CommentSyosin2Kame;
                                    //if (odrDtl.ItemCd != ItemCdConst.Syosin2)
                                    if (!new string[]
                                    {
                                        ItemCdConst.Syosin2,
                                        ItemCdConst.Syosin2Jouhou,
                                    }.Contains(odrDtl.ItemCd))
                                    {
                                        itemcd = ItemCdConst.CommentSaisin2Kame;
                                    }
                                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(itemcd, _common.kaName, autoAdd: 1);
                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, "（" + _common.kaName + "）", autoAdd: 1);
                                }
                            }

                            commentSkipFlg = false;
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 初診の自費算定分を処理する
        /// </summary>
        private void SyosinSonotaJihi()
        {
            const string conFncName = nameof(SyosinSonotaJihi);
            _common.CalculateJihi(
                OdrKouiKbnConst.Syosin,
                OdrKouiKbnConst.Syosin,
                ReceKouiKbn.Syosin,
                ReceSinId.Syosin,
                ReceSyukeisaki.Syosin,
                "JS");
        }

        /// <summary>
        /// 再診の計算
        /// </summary>
        private void CalculateSaisin()
        {
            const string conFncName = nameof(CalculateSaisin);

            //Rp
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);

            string syukeiSaki = ReceSyukeisaki.Saisin;
            //行為
            if (_common.IsRosai)
            {
                syukeiSaki = ReceSyukeisaki.EnSaisin;
            }
            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, syukeiSaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            //再診の基本項目を算定
            SaisinKihon();

            //if(_common.IsRosai)
            //{
            //    // 労災の場合、行為を分ける
            //    _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.Saisin, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
            //}
            string syukeisaki = ReceSyukeisaki.Saisin;
            if (_common.hokenKbn == HokenSyu.Jibai)
            {
                syukeisaki = ReceSyukeisaki.SyosaiSonota;
            }
            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, syukeisaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            //再診の加算を算定
            SaisinKasan(syukeisaki);

            // 認知症地域包括診療加算
            if (SaisinKasanNintuTiikiHoukatu() == false)
            {
                // 地域包括診療加算
                SaisinKasanTiikiHoukatu();
            }

            //乳幼児感染予防策加算
            SaisinNyuyojiKansen(syukeisaki);

            //医科外来等感染症対策実施加算
            SaisinKansenTaisaku(syukeisaki);

            //外来感染対策向上加算
            SaisinKansenKojo(syukeisaki);

            //連携強化加算
            SaisinRenkeiKyoka(syukeisaki);

            //サーベイランス強化加算
            SaisinSurveillance(syukeisaki);

            //再診料と同一Rpにまとめる加算（手オーダー専用）
            SaisinManualKansen(syukeisaki);

            // 再診料と異なる保険組み合わせの手オーダーを算定する
            SaisinManualKansenManual(syukeisaki);

            //Rp
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
            //行為（集計先は仮）
            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.SaisinJikangai, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"), isNodspPaperRece: 0);

            //再診の時間外、乳幼児、妊婦加算算定
            SaisinJikanKasan();

            ////Rp
            //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
            ////行為
            //_common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.SaisinGairai, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"), isNodspPaperRece: 1);

            //外来管理加算
            SaisinGairaiKanriKasan();

            ////再診のその他の手オーダー項目
            //SyosaiSonota(OdrKouiKbnConst.Saisin, ReceKouiKbn.Saisin, ReceSinId.Saisin, ReceSyukeisaki.Saisin, _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            ////再診の自費算定分
            //SaisinSonotaJihi();

            //同日再診
            //SaisinDojituComment();

            _common.Wrk.CommitWrkSinRpInf();

        }
        /// <summary>
        /// 乳幼児感染予防策加算（再診）
        /// </summary>
        /// <param name="syukeisaki"></param>
        private void SaisinNyuyojiKansen(string syukeisaki)
        {
            // 乳幼児感染予防策加算
            if (_common.Age < 6 && _common.sinDate >= 20201215 && _common.sinDate <= 20220331)
            {
                // 6歳未満
                const string conFncName = nameof(SaisinNyuyojiKansen);

                // 機能強化加算

                //加算項目を探す
                List<string> NyuyojiKansenls =
                    new List<string>
                    {
                        ItemCdConst.SaisinNyuyojiKansen,
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NyuyojiKansenls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    // ここでは、再診と同じ保険組合せのものだけ処理する
                    //if (odrDtls.First().HokenPid != _common.syosaiPid)
                    //{
                    //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                    //    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, syukeisaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                    //}

                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NyuyojiKansenls);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (!bSanteiKasan)
                {
                    if (new List<double>{
                        SyosaiConst.SaisinDenwa,
                        SyosaiConst.SaisinDenwa2,
                        SyosaiConst.SaisinJouhou,
                        SyosaiConst.Saisin2Jouhou}.Contains(_common.syosai))
                    {
                        // 電話再診又は情報通信機器を用いた場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.NyuyojiKansenCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinNyuyojiKansen))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SaisinNyuyojiKansen, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 医科外来等感染症対策実施加算（再診料・外来診療料）
        /// </summary>
        /// <param name="syukeisaki"></param>
        private void SaisinKansenTaisaku(string syukeisaki)
        {
            // 乳幼児感染予防策加算
            if (_common.sinDate >= 20210401 && _common.sinDate <= 20210930)
            {
                const string conFncName = nameof(SaisinKansenTaisaku);

                //加算項目を探す
                List<string> KansenTaisakuKasanls =
                    new List<string>
                    {
                        ItemCdConst.SaisinKansenTaisaku,
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenTaisakuKasanls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    // ここでは、再診と同じ保険組合せのものだけ処理する
                    //if (odrDtls.First().HokenPid != _common.syosaiPid)
                    //{
                    //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                    //    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, syukeisaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                    //}

                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenTaisakuKasanls);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (!bSanteiKasan)
                {
                    if (new List<double>{
                        SyosaiConst.SaisinDenwa,
                        SyosaiConst.SaisinDenwa2,
                        SyosaiConst.SaisinJouhou,
                        SyosaiConst.Saisin2Jouhou }.Contains(_common.syosai))
                    {
                        // 電話再診又は情報通信機器を用いた場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenTaisakuCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.KansenTaisaku))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SaisinKansenTaisaku, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 外来感染対策向上加算（再診）
        /// </summary>
        /// <param name="syukeisaki"></param>
        private void SaisinKansenKojo(string syukeisaki)
        {
            // 外来感染対策向上加算（再診）
            if (_common.sinDate >= 20220401)
            {
                const string conFncName = nameof(SaisinKansenKojo);

                bool santeiJoken = true;

                if (_common.CheckSanteiKaisu(ItemCdConst.SaisinKansenKojo, 0, 0) == 2)
                {
                    // 算定回数マスタのチェックにより算定不可
                    santeiJoken = false;
                }

                //加算項目を探す
                List<string> KansenKojoKasanls =
                    new List<string>
                    {
                        ItemCdConst.SaisinKansenKojo,
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenKojoKasanls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {

                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        if (santeiJoken)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(KansenKojoKasanls);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (santeiJoken && !bSanteiKasan)
                {
                    if (new List<double>{
                        SyosaiConst.SaisinDenwa,
                        SyosaiConst.SaisinDenwa2,
                        SyosaiConst.Saisin2,
                        SyosaiConst.Saisin2Jouhou}.Contains(_common.syosai))
                    {
                        // 電話再診又は２科目の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.KansenKojoCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinKansenKojo))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SaisinKansenKojo, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 連携強化加算（再診）
        /// </summary>
        /// <param name="syukeisaki"></param>
        private void SaisinRenkeiKyoka(string syukeisaki)
        {
            // 連携強化加算（再診）
            if (_common.sinDate >= 20220401)
            {
                const string conFncName = nameof(SaisinRenkeiKyoka);

                bool santeiJoken = true;

                if (_common.CheckSanteiKaisu(ItemCdConst.SaisinRenkeiKyoka, 0, 0) == 2)
                {
                    // 算定回数マスタのチェックにより算定不可
                    santeiJoken = false;
                }

                //加算項目を探す
                List<string> RenkeiKyokaKasanls =
                    new List<string>
                    {
                        ItemCdConst.SaisinRenkeiKyoka,
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(RenkeiKyokaKasanls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {

                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        if (santeiJoken)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(RenkeiKyokaKasanls);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (santeiJoken && !bSanteiKasan)
                {
                    if (new List<double>{
                        SyosaiConst.SaisinDenwa,
                        SyosaiConst.SaisinDenwa2,
                        SyosaiConst.Saisin2,
                        SyosaiConst.Saisin2Jouhou}.Contains(_common.syosai))
                    {
                        // 電話再診又は２科目の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.RenkeiKyokaCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinRenkeiKyoka))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SaisinRenkeiKyoka, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// サーベイランス強化加算（再診）
        /// </summary>
        /// <param name="syukeisaki"></param>
        private void SaisinSurveillance(string syukeisaki)
        {
            // サーベイランス加算（再診）
            if (_common.sinDate >= 20220401)
            {
                const string conFncName = nameof(SaisinSurveillance);

                bool santeiJoken = true;

                if (_common.CheckSanteiKaisu(ItemCdConst.SaisinSurveillance, 0, 0) == 2)
                {
                    // 算定回数マスタのチェックにより算定不可
                    santeiJoken = false;
                }

                //加算項目を探す
                List<string> SurveillanceKasanls =
                    new List<string>
                    {
                        ItemCdConst.SaisinSurveillance,
                    };

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;
                bool bSanteiKasan = false;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SurveillanceKasanls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {

                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        if (santeiJoken)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        //オーダーから削除
                        while (minIndex >= 0)
                        {
                            _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SurveillanceKasanls);
                        }
                    }
                    bSanteiKasan = true;
                }

                if (santeiJoken && !bSanteiKasan)
                {
                    if (new List<double>{
                        SyosaiConst.SaisinDenwa,
                        SyosaiConst.SaisinDenwa2,
                        SyosaiConst.Saisin2,
                        SyosaiConst.Saisin2Jouhou}.Contains(_common.syosai))
                    {
                        // 電話再診又は２科目の場合は算定不可
                    }
                    else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.SurveillanceCancel) == false)
                    {
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.SyosinSurveillance))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.SaisinSurveillance, autoAdd: 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 再診料と同一Rpにまとめる加算（手オーダー専用）
        /// 自動算定のある項目は別
        /// </summary>
        /// <param name="syukeisaki"></param>
        private void SaisinManualKansen(string syukeisaki)
        {
            const string conFncName = nameof(SaisinManualKansen);

            //加算項目を探す
            List<string> Kasanls =
                new List<string>
                {
                    // 二類感染症患者入院診療加算（電話等再診料・診療報酬上臨時的取扱）
                    ItemCdConst.Saisin2RuiKansen,
                };

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Kasanls);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                //if (odrDtls.First().HokenPid != _common.syosaiPid)
                //{
                //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                //    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, syukeisaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                //}

                if (odrDtls.First().HokenPid == _common.syosaiPid)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                    //オーダーから削除
                    while (minIndex >= 0)
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Kasanls);
                    }
                }
            }
        }
        private void SaisinManualKansenManual(string syukeisaki)
        {
            const string conFncName = nameof(SaisinManualKansenManual);

            //加算項目を探す
            List<List<string>> KasanlsList =
                new List<List<string>>
                {
                    new List<string>
                    {
                        ItemCdConst.SaisinNintiTiikiHoukatu1
                    },
                    new List<string>
                    {
                        ItemCdConst.SaisinNintiTiikiHoukatu2
                    },
                    new List<string>
                    {
                        ItemCdConst.SaisinTiikiHoukatu2
                    },
                    new List<string>
                    {
                        ItemCdConst.SaisinNintiTiikiHoukatu2
                    },
                    new List<string>
                    {
                        ItemCdConst.SaisinNyuyojiKansen,
                    },
                    new List<string>
                    {
                        ItemCdConst.SaisinKansenTaisaku,
                    },
                    new List<string>
                    {
                        ItemCdConst.Saisin2RuiKansen,
                    }
                };

            foreach (List<string> Kasanls in KasanlsList)
            {
                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Kasanls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    if (odrDtls.First().HokenPid != _common.syosaiPid)
                    {
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                        _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, syukeisaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                    }

                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                    //オーダーから削除
                    while (minIndex >= 0)
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Kasanls);
                    }
                }
            }
        }
        /// <summary>
        /// 再診基本項目を算定する
        /// </summary>
        private void SaisinKihon()
        {
            const string conFncName = nameof(SaisinKihon);

            //同日再診チェック用項目のリスト
            List<string> doujituSaisinCheckitemCds = new List<string>
                    {
                        ItemCdConst.Syosin,
                        ItemCdConst.SyosinCorona,
                        ItemCdConst.SyosinJouhou,
                        ItemCdConst.Saisin,
                        ItemCdConst.SaisinDenwa,
                        ItemCdConst.SaisinDojitu,
                        ItemCdConst.SaisinDenwaDojitu,
                        ItemCdConst.SaisinDenwaKeizoku,
                        ItemCdConst.SaisinJouhou,
                        ItemCdConst.SaisinJouhouDojitu,
                        ItemCdConst.ZaiHoumon1_1Dou,
                        ItemCdConst.ZaiHoumon1_1DouIgai,
                        ItemCdConst.ZaiHoumon1_2Dou,
                        ItemCdConst.ZaiHoumon1_2DouIgai,
                        ItemCdConst.ZaiHoumon2i,
                        ItemCdConst.ZaiHoumon2ro,
                        ItemCdConst.ZaiKaihouSido1,
                        ItemCdConst.SyosinRousai,
                        ItemCdConst.SaisinRousai,
                        ItemCdConst.SaisinDenwaRousai,
                        ItemCdConst.SaisinDojituRousai,
                        ItemCdConst.SaisinDenwaDojituRousai
                    };

            string itemCd = "";

            switch (_common.syosai)
            {
                case SyosaiConst.Saisin:     //再診
                    //if (_common.santeiFinder.CheckSanteiDay(_common.hpId, _common.ptId, _common.sinDate, _common.raiinNo, doujituSaisinCheckitemCds) == false)
                    if (_common.ExistWrkOrSinKouiDetailByItemCd(doujituSaisinCheckitemCds, false, true, true) == false)
                    {
                        itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.Saisin, ItemCdConst.SaisinRousai);
                    }
                    else
                    {
                        itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.SaisinDojitu, ItemCdConst.SaisinDojituRousai);
                    }

                    break;
                case SyosaiConst.SaisinDenwa:     //電話再診
                    //if (_common.santeiFinder.CheckSanteiDay(_common.hpId, _common.ptId, _common.sinDate, _common.raiinNo, doujituSaisinCheckitemCds) == false)
                    //if(_common.Sin.ExistSinKouiDetailByItemCd(doujituSaisinCheckitemCds) == false)
                    //if (_common.Wrk.ExistWrkSinKouiDetailByItemCd(doujituSaisinCheckitemCds, false) == false)
                    if (_common.ExistWrkOrSinKouiDetailByItemCd(doujituSaisinCheckitemCds, false, true, true) == false)
                    {
                        itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.SaisinDenwa, ItemCdConst.SaisinDenwaRousai);
                    }
                    else
                    {
                        itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.SaisinDenwaDojitu, ItemCdConst.SaisinDenwaDojituRousai);
                    }
                    break;
                case SyosaiConst.Saisin2:     //再診2科目
                    itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.Saisin2, ItemCdConst.Saisin2Rousai);
                    break;
                case SyosaiConst.SaisinDenwa2:     //電話再診2科目
                    itemCd = ChoiceItemCdKenpoRosai(ItemCdConst.SaisinDenwa2, ItemCdConst.SaisinDenwa2Rousai);
                    break;
                case SyosaiConst.SaisinJouhou:     //再診料（情報通信機器を用いた場合）
                    if (_common.ExistWrkOrSinKouiDetailByItemCd(doujituSaisinCheckitemCds, false, true, true) == false)
                    {
                        itemCd = ItemCdConst.SaisinJouhou;
                    }
                    else
                    {
                        itemCd = ItemCdConst.SaisinJouhouDojitu;
                    }
                    break;
                case SyosaiConst.Saisin2Jouhou:     //再診料（同一日複数科受診時の２科目）（情報通信機器）
                    itemCd = ItemCdConst.Saisin2Jouhou;
                    break;

            }

            if (itemCd != "")
            {
                _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);

                if (_common.IsRosai && _common.Wrk.wrkSinKouiDetails.Last().IsEnKoumoku)
                {
                    // 労災で円項目の場合、集計先変更
                    _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.EnSaisin;
                }

                //if (_common.syosai == SyosaiConst.Saisin2 || _common.syosai == SyosaiConst.SaisinDenwa2)
                if (new List<double> { SyosaiConst.Saisin2, SyosaiConst.SaisinDenwa2, SyosaiConst.Saisin2Jouhou }.Contains(_common.syosai))
                {
                    //2科目の場合は診療科名を追加する
                    if (_common.sinDate >= 20200401)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentSaisin2Kame, _common.kaName, autoAdd: 1);
                    }
                    else
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentFree, "（" + _common.kaName + "）", autoAdd: 1);
                    }
                }

                if (new List<double> { SyosaiConst.SaisinJouhou, SyosaiConst.Saisin2Jouhou }.Contains(_common.syosai))
                {
                    //情報通信機器の場合はコメントを追加する
                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentOnlineSinryoSaisin, "", autoAdd: 1);
                    if (_common.Odr.ExistIngaiSyoho || _common.Odr.ExistInnaiSyoho)
                    {
                        // 同じ来院に処方がある場合は処方のコメントも追加する
                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(ItemCdConst.CommentOnlineSyohoSaisin, "", autoAdd: 1);
                    }                    
                }

            }

        }

        /// <summary>
        /// 再診の加算項目算定処理
        /// </summary>
        private void SaisinKasan(string syukeisaki)
        {
            const string conFncName = nameof(SaisinKasan);

            // 乳幼児加算
            SaisinKasanNyuNinpu();

            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, syukeisaki, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));

            // 時間外対応加算
            SaisinKasanJikangaiTaiou();

            // 明細書発行等体制加算
            SaisinKasanMeisaiHakko();

            //// 認知症地域包括診療加算
            //if (SaisinKasanNintuTiikiHoukatu() == false)
            //{
            //    // 地域包括診療加算
            //    SaisinKasanTiikiHoukatu();
            //}

        }

        /// <summary>
        /// 乳幼児加算と妊婦加算の算定処理
        /// この後の時間加算の処理で削除される可能性あり
        /// </summary>
        private void SaisinKasanNyuNinpu()
        {
            List<string> Ninpuls =
                new List<string>
                {
                        ItemCdConst.SaisinNinpu
                };

            if (_common.jikan == JikanConst.JikanNai || _common.jikan == JikanConst.Yasou || _common.Odr.odrDtlls.Any(p => Ninpuls.Contains(p.ItemCd)))
            {
                // 時間内と夜間早朝、または妊婦加算が手オーダーされている場合のみここで処理、再診とＲｐをまとめる
                bool bSanteiKasan = false;

                List<OdrDtlTenModel> odrDtls;
                int minIndex = 0;
                int itemCnt = 0;

                //手オーダーされている加算項目を探す
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Ninpuls);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    //if (new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa }.Contains(_common.syosai))
                    if (new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            bSanteiKasan = true;
                        }
                    }
                    else
                    {
                        //bSanteiKasan = true;
                    }

                    // オーダーから削除
                    while (minIndex >= 0)
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(Ninpuls);
                    }
                }

                if (!bSanteiKasan &&
                    //(new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa }.Contains(_common.syosai)))
                    new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                {
                    string itemCd = ChoiceItemCdNyuNinpu("", ItemCdConst.SaisinNyu, ItemCdConst.SaisinNinpu);

                    if (itemCd != "")
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
                        // オーダーから削除
                        _common.Odr.RemoveOdrDtlByItemCd(itemCd);
                    }
                }
            }
        }

        /// <summary>
        /// 時間外対応加算自動算定処理
        /// </summary>
        private void SaisinKasanJikangaiTaiou()
        {
            const string conFncName = nameof(SaisinKasanJikangaiTaiou);

            //再診２科目の場合は算定不可
            //if (_common.syosai == SyosaiConst.Saisin2 || _common.syosai == SyosaiConst.SaisinDenwa2)
            if (new List<double> { SyosaiConst.Saisin2, SyosaiConst.SaisinDenwa2, SyosaiConst.Saisin2Jouhou }.Contains(_common.syosai))
            {
                return;
            }

            //手オーダーされている加算項目を探す
            bool bSanteiKasan = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            //手オーダーされている加算項目を探す
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(JikangaiTaiouls);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                }

                // オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(JikangaiTaiouls);
                }
                bSanteiKasan = true;
            }

            if (!bSanteiKasan)
            {
                //if (_common.IsCoronaDenwaSaisin())
                //{
                //    // コロナ電話再診
                //}
                //else
                if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.JikangaitaiouCancel) == false)
                {
                    // キャンセル項目がなければ自動算定
                    if (_common.Mst.ExistAutoSantei(ItemCdConst.JikangaiTaiou1))
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.JikangaiTaiou1, autoAdd: 1);
                    }
                    else if (_common.Mst.ExistAutoSantei(ItemCdConst.JikangaiTaiou2))
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.JikangaiTaiou2, autoAdd: 1);
                    }
                    else if (_common.Mst.ExistAutoSantei(ItemCdConst.JikangaiTaiou3))
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.JikangaiTaiou3, autoAdd: 1);
                    }
                }
            }

        }

        /// <summary>
        /// 明細書発行体制等加算自動算定処理
        /// </summary>
        private void SaisinKasanMeisaiHakko()
        {
            const string conFncName = nameof(SaisinKasanMeisaiHakko);

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            //再診２科目の場合は算定不可
            //if (!(_common.syosai == SyosaiConst.Saisin2 || _common.syosai == SyosaiConst.SaisinDenwa2))
            if (!new List<double> { SyosaiConst.Saisin2, SyosaiConst.SaisinDenwa2, SyosaiConst.Saisin2Jouhou }.Contains(_common.syosai))
            {
                //手オーダーされている加算項目を探す
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.MeisaiHakko);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                }
                else
                {
                    if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.MeisaiHakkoCancel) == false)
                    {
                        // キャンセル項目がなければ自動算定
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.MeisaiHakko))
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.MeisaiHakko, autoAdd: 1);
                        }
                    }
                }
            }

            // オーダーから削除
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.MeisaiHakko);
            while (minIndex >= 0)
            {
                _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.MeisaiHakko);
            }
        }


        /// <summary>
        /// 認知症地域包括診療加算
        /// </summary>
        /// <returns>true: 算定した</returns>
        private bool SaisinKasanNintuTiikiHoukatu()
        {
            const string conFncName = nameof(SaisinKasanNintuTiikiHoukatu);

            bool ret = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            //if (_common.syosai != SyosaiConst.Saisin) //再診以外は算定不可
            if (!new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
            {
                // 再診２科目又は電話再診は算定不可
                ret = false;
            }
            else
            {
                //手オーダーされている加算項目を探す
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.SaisinNintiTiikiHoukatu1);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    // 算定回数上限チェック
                    if (_common.CheckSanteiNintiTiiki(ItemCdConst.SaisinNintiTiikiHoukatu1, "認知症地域包括診療加算", odrDtls.First().SanteiKbn, 0))
                    {
                        // ここでは、初再診と同じ保険組合せのものだけ処理する
                        //if (odrDtls.First().HokenPid != _common.syosaiPid)
                        //{
                        //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                        //    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Saisin, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                        //}
                        if (odrDtls.First().HokenPid == _common.syosaiPid)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }

                        ret = true;
                    }
                }
                else
                {
                    //手オーダーされている加算項目を探す
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.SaisinNintiTiikiHoukatu2);
                    //見つかったら、項目を算定する
                    if (minIndex >= 0)
                    {
                        // 算定回数上限チェック
                        if (_common.CheckSanteiNintiTiiki(ItemCdConst.SaisinNintiTiikiHoukatu2, "認知症地域包括診療加算", odrDtls.First().SanteiKbn, 0))
                        {
                            // ここでは、初再診と同じ保険組合せのものだけ処理する
                            //if (odrDtls.First().HokenPid != _common.syosaiPid)
                            //{
                            //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                            //    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Saisin, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                            //}
                            if (odrDtls.First().HokenPid == _common.syosaiPid)
                            {
                                foreach (OdrDtlTenModel odrDtl in odrDtls)
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                }
                            }

                            ret = true;
                        }
                    }
                }
            }

            //オーダーから削除
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuls);
            while (minIndex >= 0)
            {
                if (odrDtls.First().HokenPid == _common.syosaiPid)
                {
                    // ここでは、初再診と同じ保険組合せのものだけ処理する
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuls);
                }
                else
                {
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(NintiTiikiHoukatuls, minIndex + itemCnt);
                }
            }

            return ret;
        }

        /// <summary>
        /// 地域包括診療加算
        /// </summary>
        private void SaisinKasanTiikiHoukatu()
        {
            const string conFncName = nameof(SaisinKasanTiikiHoukatu);

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            //if (_common.syosai != SyosaiConst.Saisin)// 再診以外は算定不可
            if (!new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
            {
                // 再診２科目又は電話再診は算定不可
            }
            else
            {
                //手オーダーされている加算項目を探す
                (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.SaisinTiikiHoukatu1);
                //見つかったら、項目を算定する
                if (minIndex >= 0)
                {
                    // 異なる場合は、後で別Rpに算定する
                    //if (odrDtls.First().HokenPid != _common.syosaiPid)
                    //{
                    //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                    //    _common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Saisin, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                    //}
                    if (odrDtls.First().HokenPid == _common.syosaiPid)
                    {
                        foreach (OdrDtlTenModel odrDtl in odrDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                        }
                    }
                }
                else
                {
                    //手オーダーされている加算項目を探す
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(ItemCdConst.SaisinTiikiHoukatu2);
                    //見つかったら、項目を算定する
                    if (minIndex >= 0)
                    {
                        // 異なる場合は、後で別Rpに算定する
                        //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                        //_common.Wrk.AppendNewWrkSinKoui(odrDtls.First().HokenPid, odrDtls.First().HokenId, ReceSyukeisaki.Saisin, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                        if (odrDtls.First().HokenPid == _common.syosaiPid)
                        {
                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                            }
                        }
                    }
                }
            }

            //オーダーから削除
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuls);
            while (minIndex >= 0)
            {
                if (odrDtls.First().HokenPid == _common.syosaiPid)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuls);
                }
                else
                {
                    // 異なる場合は、後で別Rpに算定するのでここでは削除しない
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(TiikiHoukatuls, minIndex + itemCnt);
                }
            }

        }

        /// <summary>
        /// 再診の時間枠/乳幼児/妊婦加算算定処理
        /// </summary>
        /// <returns></returns>
        private void SaisinJikanKasan()
        {
            const string conFncName = nameof(SaisinJikanKasan);
            List<string> Ninpuls =
                new List<string>
                {
                    ItemCdConst.SaisinNinpu
                };
            List<string> NyuNinCdls = new List<string> { ItemCdConst.SaisinNyu, ItemCdConst.SaisinNinpu };

            string itemCd = "";
            string itemCd2 = "";
            string syukeiSaki = ReceSyukeisaki.Saisin;

            //手オーダーされている加算項目を探す
            bool bSanteiKasan = false;
            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            if (_common.Wrk.wrkSinKouiDetails.Any(p => Ninpuls.Contains(p.ItemCd) && p.IsAutoAdd == 0))
            {
                // 既に手動による加算項目が算定されている場合は処理しない
                return;
            }

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SaisinKasanls);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                if (_common.IsYoJi == false && odrDtls.Any(p => CIUtil.StrToIntDef(p.MaxAge, 0) == 6))
                {
                    // 6歳以上算定不可
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.AppendCalcLog(2, odrDtl.ItemName + "は、6歳以上のため、算定できません。");
                    }
                }
                //else if (!(new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa }.Contains(_common.syosai)))
                else if (!new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {
                        _common.AppendCalcLog(2, odrDtl.ItemName + "は、再診・電話再診・再診料（情報通信機器を用いた場合）ではないため、算定できません。");
                    }
                }
                else
                {
                    foreach (OdrDtlTenModel odrDtl in odrDtls)
                    {

                        // 乳幼児特例加算の置き換え
                        OdrDtlTenModel appendDlt = _common.Odr.CopyOdrDtl(odrDtl);

                        if (SyouniYakanKasanls.Contains(appendDlt.ItemCd))
                        {
                            _common.Odr.UpdateOdrDtlItemCd(appendDlt, ItemCdConst.SaisinSyouniNyuYakan);
                        }
                        else if (SyouniKyujituKasanls.Contains(appendDlt.ItemCd))
                        {
                            _common.Odr.UpdateOdrDtlItemCd(appendDlt, ItemCdConst.SaisinSyouniNyuKyujitu);
                        }
                        else if (SyouniSinyaKasanls.Contains(appendDlt.ItemCd))
                        {
                            _common.Odr.UpdateOdrDtlItemCd(appendDlt, ItemCdConst.SaisinSyouniNyuSinya);
                        }

                        _common.Wrk.AppendNewWrkSinKouiDetail(appendDlt, _common.Odr.GetOdrCmt(appendDlt));

                        itemCd = appendDlt.ItemCd;

                        if (SaisinJikangails.Contains(appendDlt.ItemCd))
                        {
                            // 時間外関連の項目が存在する場合
                            syukeiSaki = ReceSyukeisaki.SaisinJikangai;
                        }
                        else if (SaisinKyujituls.Contains(appendDlt.ItemCd))
                        {
                            // 休日関連の項目が存在する場合
                            syukeiSaki = ReceSyukeisaki.SaisinKyujitu;
                        }
                        else if (SaisinSinyals.Contains(appendDlt.ItemCd))
                        {
                            // 深夜関連の項目が存在する場合
                            syukeiSaki = ReceSyukeisaki.SaisinSinya;
                        }
                    }
                    bSanteiKasan = true;
                }

                // オーダーから削除
                while (minIndex >= 0)
                {
                    if (odrDtls.Any(p => p.OdrKouiKbn <= 12))
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SaisinKasanls);
                    }
                    else
                    {
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(SaisinKasanls, minIndex + itemCnt);
                    }
                }
            }

            //if ((new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa }.Contains(_common.syosai)))
            if (new List<double> { SyosaiConst.Saisin, SyosaiConst.SaisinDenwa, SyosaiConst.SaisinJouhou }.Contains(_common.syosai))
            {
                if (!bSanteiKasan)
                {
                    if (_common.IsYoJi && _common.Mst.GetHyoboSyounika() == 1)
                    {
                        // 幼児で小児科標榜ありの場合

                        // 夜間加算項目をチェック
                        bSanteiKasan = JikanKasanOdr(odrDtls, SyouniYakanKasanls, ItemCdConst.SaisinSyouniNyuYakan);
                        if (!bSanteiKasan)
                        {
                            //休日加算項目をチェック
                            bSanteiKasan = JikanKasanOdr(odrDtls, SyouniKyujituKasanls, ItemCdConst.SaisinSyouniNyuKyujitu);
                        }
                        if (!bSanteiKasan)
                        {
                            //深夜加算項目をチェック
                            bSanteiKasan = JikanKasanOdr(odrDtls, SyouniSinyaKasanls, ItemCdConst.SaisinSyouniNyuSinya);
                        }
                    }
                }

                if (!bSanteiKasan)
                {
                    //自動算定処理

                    itemCd = "";
                    itemCd2 = "";
                    if (_common.jikan == JikanConst.JikanNai)
                    {
                        //時間内
                        itemCd = ChoiceItemCdNyuNinpu("", ItemCdConst.SaisinNyu, ItemCdConst.SaisinNinpu);
                    }
                    else if (_common.jikan == JikanConst.JikanGai)
                    {
                        //時間外
                        itemCd = ChoiceItemCdNyuNinpu(ItemCdConst.SaisinJikangai, ItemCdConst.SaisinNyuJikangai, ItemCdConst.SaisinNinpuJikangai);
                        syukeiSaki = ReceSyukeisaki.SaisinJikangai;
                    }
                    else if (_common.jikan == JikanConst.Kyujitu)
                    {
                        //休日
                        itemCd = ChoiceItemCdNyuNinpu(ItemCdConst.SaisinKyujitu, ItemCdConst.SaisinNyuKyujitu, ItemCdConst.SaisinNinpuKyujitu);
                        syukeiSaki = ReceSyukeisaki.SaisinKyujitu;
                    }
                    else if (_common.jikan == JikanConst.Sinya)
                    {
                        //深夜
                        itemCd = ChoiceItemCdNyuNinpu(ItemCdConst.SaisinSinya, ItemCdConst.SaisinNyuSinya, ItemCdConst.SaisinNinpuSinya);
                        syukeiSaki = ReceSyukeisaki.SaisinSinya;
                    }
                    else if (_common.jikan == JikanConst.Yasou)
                    {
                        //夜間早朝
                        if (_common.Mst.GetHyoboSyounika() == 1)
                        {
                            // 小児科標榜あり

                            if (_common.IsYoJi)
                            {
                                //幼児（6歳未満）
                                itemCd = ChoiceItemCdTime(ItemCdConst.SaisinSyouniNyuYakan, ItemCdConst.SaisinSyouniNyuKyujitu, ItemCdConst.SaisinSyouniNyuSinya);
                            }
                        }
                        else if (_common.Mst.GetHyoboSanka() == 1)
                        {
                            // 産科標榜あり

                            if (_common.IsNinpu)
                            {
                                //妊婦
                                itemCd = ChoiceItemCdTime(ItemCdConst.SaisinNinpuYakanToku, ItemCdConst.SaisinNinpuKyujituToku, ItemCdConst.SaisinNinpuSinyaToku);
                            }
                        }

                        if (itemCd == "")
                        {
                            //夜間・早朝等加算
                            itemCd = ItemCdConst.SaisinYasou;
                            syukeiSaki = ReceSyukeisaki.SaisinJikangai;
                        }

                    }
                    else if (_common.jikan == JikanConst.YakanKotoku)
                    {
                        // 特例夜間
                        if (_common.IsYoJi)
                        {
                            itemCd = ItemCdConst.SaisinSyouniNyuYakan;
                        }
                        else
                        {
                            itemCd = ItemCdConst.SaisinYasou;
                        }
                        syukeiSaki = ReceSyukeisaki.Saisin;
                    }
                    else if (_common.jikan == JikanConst.KyujituKotoku)
                    {
                        // 特例休日
                        if (_common.IsYoJi)
                        {
                            itemCd = ItemCdConst.SaisinSyouniNyuKyujitu;
                        }
                        else
                        {
                            itemCd = ItemCdConst.SaisinKyujitu;
                        }
                        syukeiSaki = ReceSyukeisaki.SaisinKyujitu;
                    }
                    else if (_common.jikan == JikanConst.SinyaKotoku)
                    {
                        // 特例深夜
                        if (_common.IsYoJi)
                        {
                            itemCd = ItemCdConst.SaisinSyouniNyuSinya;
                        }
                        else
                        {
                            itemCd = ItemCdConst.SaisinSinya;
                        }
                        syukeiSaki = ReceSyukeisaki.SaisinSinya;
                    }

                    if (itemCd != "" && NyuNinCdls.Contains(itemCd) == false)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1);
                        bSanteiKasan = true;
                    }

                    if (itemCd2 != "" && NyuNinCdls.Contains(itemCd2) == false)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(itemCd2, autoAdd: 1);
                    }
                }

                if (bSanteiKasan && SaisinTekiyols.Contains(itemCd) == false)
                {
                    // この関数内で加算項目を算定した場合、合成コードであるため、事前に算定していた乳幼児加算と妊婦加算を削除する
                    // 時間外特例・小児科特例・夜間早朝加算の場合は合成コードではないので削除しない
                    //List<string> ItemCds = new List<string> { ItemCdConst.SaisinNyu, ItemCdConst.SaisinNinpu };
                    _common.Wrk.RemoveWrkSinKouiDetailByItemCd(NyuNinCdls);
                    _common.Wrk.wrkSinKouis[_common.Wrk.wrkSinKouis.Count - 1].IsNodspPaperRece = 1;
                }

                // 集計先を調整
                if (_common.hokenKbn == HokenSyu.After)
                {
                    // アフターケアの場合は、再診欄に集計
                    syukeiSaki = ReceSyukeisaki.Saisin;
                }

                _common.Wrk.wrkSinKouis[_common.Wrk.wrkSinKouis.Count - 1].SyukeiSaki = syukeiSaki;
            }
        }

        /// <summary>
        /// 特定時間加算項目が算定されているとき、その項目をリプレイスして算定する
        /// </summary>
        /// <param name="odrDtls">オーダー詳細</param>
        /// <param name="itemCds">チェックする時間加算項目のリスト</param>
        /// <param name="replaceItemCd">置き換える項目の診療行為コード</param>
        /// <returns>true: 算定した</returns>
        private bool JikanKasanOdr(List<OdrDtlTenModel> odrDtls, List<string> itemCds, string replaceItemCd)
        {
            bool ret = false;

            int minIndex;
            int itemCnt;

            // 夜間加算項目をチェック
            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCds);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    OdrDtlTenModel appendDlt = _common.Odr.CopyOdrDtl(odrDtl);

                    if (itemCds.Contains(appendDlt.ItemCd))
                    {
                        // 項目コード差し替え
                        _common.Odr.UpdateOdrDtlItemCd(appendDlt, replaceItemCd);
                    }

                    _common.Wrk.AppendNewWrkSinKouiDetail(appendDlt, _common.Odr.GetOdrCmt(odrDtl));
                }

                // オーダーから削除
                while (minIndex >= 0)
                {
                    if (odrDtls.Any(p => p.OdrKouiKbn <= 12))
                    {
                        _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCds);
                    }
                    else
                    {
                        (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(itemCds, minIndex + itemCnt);
                    }
                }
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 外来管理加算算定処理
        /// </summary>
        /// <returns></returns>
        private void SaisinGairaiKanriKasan()
        {
            const string conFncName = nameof(SaisinGairaiKanriKasan);

            //Rp
            _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
            //行為
            _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.SaisinGairai, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"), isNodspPaperRece: 1);

            //加算項目を探す
            List<OdrDtlTenModel> filteredOdrDtls = _common.Odr.FilterOdrDetailByItemCd(GairaiKanrils);
            bool bSanteiKasan = false;

            List<OdrDtlTenModel> odrDtls;
            int minIndex = 0;
            int itemCnt = 0;

            (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiKanrils);
            //見つかったら、項目を算定する
            if (minIndex >= 0)
            {
                foreach (OdrDtlTenModel odrDtl in odrDtls)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                    if (odrDtl.ItemCd == ItemCdConst.GairaiKanriKasanRousai)
                    {
                        // 労災外来管理加算の場合、集計先を変更する
                        _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.SaisinGairaiRousai;
                        _common.Wrk.wrkSinKouis.Last().IsNodspPaperRece = 0;
                    }
                }

                // オーダーから削除
                while (minIndex >= 0)
                {
                    _common.Odr.odrDtlls.RemoveRange(minIndex, itemCnt);
                    (odrDtls, minIndex, itemCnt) = _common.Odr.FilterOdrDetailRangeByItemCd(GairaiKanrils);
                }
                bSanteiKasan = true;
            }

            if (!bSanteiKasan)
            {
                bool bSantei = true;

                // 労災アフターケアのときは算定不可
                if (_common.hokenKbn == HokenSyu.After)
                {
                    bSantei = false;
                }
                else if (_common.IsCoronaDenwaSaisin())
                {
                    // コロナ電話再診
                    bSantei = false;
                }
                else if (new double[]{ SyosaiConst.SaisinJouhou, SyosaiConst.Saisin2Jouhou }.Contains(_common.syosai))
                {
                    // 情報通信機器
                    bSantei = false;
                }
                else
                {
                    // 同一来院に設定している来院で既に算定済みの場合は算定不可
                    List<RaiinInfModel> raiinInfs = _common.GetDouituRaiin(_common.raiinNo, _common.oyaRaiinNo);
                    if (raiinInfs.Any())
                    {
                        foreach (RaiinInfModel raiinInf in raiinInfs)
                        {
                            if (_common.Sin.ExistSinKouiDetailByItemCdRaiinNo(GairaiKanrils, raiinInf.RaiinNo))
                            {
                                bSantei = false;
                                _common.AppendCalcLog(2, "'外来管理加算' は、同一来院で算定済みのため、算定できません。");

                                break;
                            }
                        }
                    }
                }

                if (bSantei == true && _common.Odr.ExistOdrDetailByItemCd(ItemCdConst.GairaiKanriKasanCancel) == false)
                {
                    // キャンセル項目がなければ自動算定
                    if (_common.Mst.ExistAutoSantei(ItemCdConst.GairaiKanriKasan))
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GairaiKanriKasan, autoAdd: 1);
                    }
                }
            }

        }

        /// <summary>
        /// 再診の残り項目を処理する
        /// </summary>
        private void SaisinSonota()
        {
            const string conFncName = nameof(SaisinSonota);

            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbn(OdrKouiKbnConst.Saisin);
            foreach (OdrInfModel odrInf in filteredOdrInf)
            {
                filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                if (filteredOdrDtl.Any())
                {
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, odrInf.SanteiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Saisin, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "A"));

                    foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    }
                }
            }

        }

        /// <summary>
        /// 再診の自費算定分を処理する
        /// </summary>
        private void SaisinSonotaJihi()
        {
            const string conFncName = nameof(SaisinSonotaJihi);
            _common.CalculateJihi(
                OdrKouiKbnConst.Saisin,
                OdrKouiKbnConst.Saisin,
                ReceKouiKbn.Saisin,
                ReceSinId.Saisin,
                ReceSyukeisaki.Saisin,
                "JS");
        }

        private void SyosinJikanNaiComment()
        {
            List<string> JikanGailSyukeiSakis =
                new List<string>
                {
                    ReceSyukeisaki.SyosinJikanGai,
                    ReceSyukeisaki.SyosinKyujitu,
                    ReceSyukeisaki.SyosinSinya,
                    ReceSyukeisaki.SyosinYasou
                };

            List<string> JikanGaiItemCds =
                new List<string>
                {
                    ItemCdConst.SyosinSyouniNyuYakan,
                    ItemCdConst.SyosinSyouniNyuKyujitu,
                    ItemCdConst.SyosinSyouniNyuYakan
                };

            if (_common.Wrk.ExistWrkSinKouiDetailBySyukeiSaki(JikanGailSyukeiSakis) == false)
            {
                // 今回が時間内である

                if (_common.Wrk.wrkSinKouis.Any(p =>
                        p.IsDeleted == DeleteStatus.None &&
                        JikanGailSyukeiSakis.Contains(p.SyukeiSaki)) ||
                    _common.Wrk.ExistWrkSinKouiDetailByItemCdExcludeThisRaiin(JikanGaiItemCds) ||
                    _common.Sin.CheckSyukeisakiTerm(JikanGailSyukeiSakis, _common.SinFirstDateOfMonth, _common.SinLastDateOfMonth) ||
                    _common.Sin.CheckSanteiTerm(JikanGaiItemCds, _common.SinFirstDateOfMonth, _common.SinLastDateOfMonth))
                {
                    // 当月に時間外の算定がある
                    AddComment(ItemCdConst.CommentSyosinJikanNai);
                }

            }

            #region Local Method
            // 時間内の回数用コメントを追加する
            void AddComment(string itemCd)
            {
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Syosin, ReceSinId.Syosin, _common.syosaiSanteiKbn);
                _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.SyosinComment, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1, isNodspRyosyu: 1);
            }
            #endregion
        }

        /// <summary>
        /// 同日再診等の回数用コメント
        /// </summary>
        private void SaisinDojituComment()
        {
            //改正TODO 情報通信機器を用いた場合もコメント発生させるか？
            //if(_common.Wrk.ExistWrkSinKouiDetailByItemCd(ItemCdConst.SaisinDojitu))
            if (_common.ExistWrkOrSinKouiDetailByItemCd(new List<string> { ItemCdConst.SaisinDojitu, ItemCdConst.SaisinDojituRousai }))
            {
                // 同日再診あり
                //if(_common.Sin.SinKouiDetails.Any(p=> p.ItemCd == ItemCdConst.CommentSaisinDojitu && p.UpdateState == UpdateStateConst.None) == false)
                //if(_common.Sin.ExistSinKouiDetailByItemCd(ItemCdConst.CommentSaisinDojitu))
                //{
                AddComment(ItemCdConst.CommentSaisinDojitu);
                //}
            }
            //else if (_common.Wrk.ExistWrkSinKouiDetailByItemCd(new List<string> { ItemCdConst.SaisinDenwa, ItemCdConst.SaisinDenwa2 }))
            else if (_common.ExistWrkOrSinKouiDetailByItemCd(new List<string> { ItemCdConst.SaisinDenwa, ItemCdConst.SaisinDenwa2, ItemCdConst.SaisinDenwaRousai, ItemCdConst.SaisinDenwa2Rousai }))
            {
                // 電話再診あり
                //if (_common.Sin.SinKouiDetails.Any(p => p.ItemCd == ItemCdConst.CommentDenwaSaisin && p.UpdateState == UpdateStateConst.None) == false)
                //if (_common.Sin.ExistSinKouiDetailByItemCd(ItemCdConst.CommentDenwaSaisin))
                //{
                AddComment(ItemCdConst.CommentDenwaSaisin);
                //}
            }
            //else if (_common.Wrk.ExistWrkSinKouiDetailByItemCd(ItemCdConst.SaisinDenwaDojitu))
            else if (_common.ExistWrkOrSinKouiDetailByItemCd(new List<string> { ItemCdConst.SaisinDenwaDojitu, ItemCdConst.SaisinDenwaDojituRousai }))
            {
                // 同日電話再診あり
                //if (_common.Sin.SinKouiDetails.Any(p => p.ItemCd == ItemCdConst.CommentDenwaSaisinDojitu && p.UpdateState == UpdateStateConst.None) == false)
                //if (_common.Sin.ExistSinKouiDetailByItemCd(ItemCdConst.CommentDenwaSaisinDojitu))
                //{
                AddComment(ItemCdConst.CommentDenwaSaisinDojitu);
                //}
            }

            #region Local Method
            // 同日再診等の回数用コメントを追加する
            void AddComment(string itemCd)
            {
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.SaisinComment, cdKbn: _common.GetCdKbn(_common.syosaiSanteiKbn, "A"));
                //_common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1, isNodspRece: 2, isNodspRyosyu: 1);  // レセ電には出すらしい？
                _common.Wrk.AppendNewWrkSinKouiDetail(itemCd, autoAdd: 1, isNodspRyosyu: 1);
            }
            #endregion
        }

        /// <summary>
        /// 健保の場合と労災の場合で適切な項目のITEM_CDを返す
        /// </summary>
        /// <param name="KenpoItemCd">健保の場合のITEM_CD</param>
        /// <param name="RousaiItemCd">労災の場合のITEM_CD</param>
        /// <returns>条件に合うITEM_CD</returns>
        string ChoiceItemCdKenpoRosai(string KenpoItemCd, string RousaiItemCd)
        {
            if (_common.IsRosai)
            {
                return RousaiItemCd;
            }
            else
            {
                return KenpoItemCd;
            }
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

    }
}
