using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using Helper.Common;
using EmrCalculateApi.Interface;

namespace EmrCalculateApi.Ika.ViewModels
{
    public class IkaCalculateCommonMasterViewModel
    {
        /// <summary>
        /// マスタファインダー
        /// </summary>
        MasterFinder _masterFinder;
        //Emr.CommonBase.CommonMasters.DbAccess.MasterFinder _commonMstFinder;

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        int _hpId;
        /// <summary>
        /// 診療日
        /// </summary>
        int _sinDate;

        /// <summary>
        /// 点数マスタ
        /// </summary>
        private List<TenMstModel> _tenMst;
        /// <summary>
        /// 自動算定マスタ
        /// </summary>
        private List<AutoSanteiMstModel> _autoSanteiMsts;
        /// <summary>
        /// コメントマスタ
        /// </summary>
        private List<TenMstModel> _commentMsts;
        /// <summary>
        /// コメント関連テーブル
        /// </summary>
        private List<RecedenCmtSelectModel> _recedenCmtSelects;
        /// <summary>
        /// 在宅週単位計算項目の診療行為コードのリスト
        /// </summary>
        private List<string> _zaiWeekCalcList;
        /// <summary>
        /// 在医総項目の診療行為コードのリスト
        /// </summary>
        private List<string> _zaiisoList;
        /// <summary>
        /// システム世代管理設定
        /// </summary>
        private List<SystemGenerationConfModel> _systemGenerationConfs;
        /// <summary>
        /// 電子点数表背反マスタ
        /// </summary>
        private List<DensiHaihanMstModel> _densiHaihanMsts;
        /// <summary>
        /// 行為包括マスタ
        /// </summary>
        private List<KouiHoukatuMstModel> _kouiHoukatuMsts;
        /// <summary>
        /// 一般名処方加算除外一般名コード
        /// </summary>
        private List<IpnKasanExcludeModel> _ipnKasanExcludes;
        /// <summary>
        /// 一般名処方加算除外項目コード
        /// </summary>
        private List<IpnKasanExcludeItemModel> _ipnKasanExcludeItems;
        /// <summary>
        /// 電子算定回数マスタ
        /// </summary>
        private List<DensiSanteiKaisuModel> _densiSanteiKaisus;
        /// <summary>
        /// 項目グループマスタ
        /// </summary>
        private List<ItemGrpMstModel> _itemGrpMsts;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        /// <summary>
        /// マスタ情報管理クラス
        /// </summary>
        /// <param name="masterFinder">マスタファインダー</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        public IkaCalculateCommonMasterViewModel(
            MasterFinder masterFinder, //CommonBase.CommonMasters.DbAccess.MasterFinder commonMstFinder,
            int hpId, int sinDate,
            List<TenMstModel> cacheTenMst, List<DensiSanteiKaisuModel> cacheDensiSanteiKaisu, List<ItemGrpMstModel> cacheItemGrpMst, List<KouiHoukatuMstModel> cacheKouiHoukatuMst,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            //マスタファインダー
            _masterFinder = masterFinder;
            //_commonMstFinder = commonMstFinder;

            //点数マスタ
            _tenMst = cacheTenMst;// new List<TenMstModel>();
            //電子点数表背反マスタ
            _densiHaihanMsts = new List<DensiHaihanMstModel>();
            //行為包括マスタ
            _kouiHoukatuMsts = new List<KouiHoukatuMstModel>();

            //医療機関識別ID
            _hpId = hpId;
            //診療日
            _sinDate = sinDate;

            _densiSanteiKaisus = cacheDensiSanteiKaisu;
            _itemGrpMsts = cacheItemGrpMst;

            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;

            _kouiHoukatuMsts = cacheKouiHoukatuMst;
        }

        public List<TenMstModel> CacheTenMst
        {
            get { return _tenMst; }
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
        }

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
            set
            {
                if (_sinDate != value)
                {
                    _sinDate = value;
                    _autoSanteiMsts = null;
                    _commentMsts = null;
                    _zaiisoList = null;
                    _zaiWeekCalcList = null;
                    _systemGenerationConfs = null;
                    _ipnKasanExcludes = null;
                    _ipnKasanExcludeItems = null;
                }
            }
        }

        /// <summary>
        /// 自動算定マスタ
        /// </summary>
        private List<AutoSanteiMstModel> AutoSanteiMsts
        {
            get
            {
                if (_autoSanteiMsts == null)
                {
                    _autoSanteiMsts = _masterFinder.FindAutoSanteiMst(_hpId, _sinDate);
                }
                return _autoSanteiMsts;
            }
        }

        /// <summary>
        /// コメントマスタ
        /// </summary>
        private List<TenMstModel> CmtMsts
        {
            get
            {
                if (_commentMsts == null)
                {
                    _commentMsts = _masterFinder.FindCommentMst(_hpId, _sinDate);
                }
                return _commentMsts;
            }
        }
        /// <summary>
        /// レセ電コメント関連テーブル
        /// </summary>
        private List<RecedenCmtSelectModel> RecedenCmtSelects
        {
            get
            {
                if (_recedenCmtSelects == null)
                {
                    _recedenCmtSelects = _masterFinder.FindRecedenCmtSelect(_hpId, _sinDate);
                }
                return _recedenCmtSelects;
            }
        }
        /// <summary>
        /// 在医総・施医総項目のリスト（ITEM_CD(string)のList）
        /// </summary>
        public List<string> ZaiisoList
        {
            get
            {
                if (_zaiisoList == null)
                {
                    //_zaiisoList = new List<string>();
                    _zaiisoList = _masterFinder.GetZaiisoList(_hpId, _sinDate);
                    //foreach (TenMstModel tenMst in Zaiisos)
                    //{
                    //    _zaiisoList.Add(tenMst.ItemCd);
                    //}
                }
                return _zaiisoList;
            }
        }

        /// <summary>
        /// 在宅週単位計算項目のリスト（ITEM_CD(string)のList）
        /// </summary>
        public List<string> ZaiWeekCalcList
        {
            get
            {
                if (_zaiWeekCalcList == null)
                {
                    //_zaiWeekCalcList = new List<string>();
                    _zaiWeekCalcList = _masterFinder.GetZaiWeekCalc(_hpId, _sinDate);
                    //foreach (TenMstModel tenMst in ZaiWeekCalcs)
                    //{
                    //    _zaiWeekCalcList.Add(tenMst.ItemCd);
                    //}
                }
                return _zaiWeekCalcList;
            }
        }

        private List<string> _zaitakuRyoyo;
        /// <summary>
        /// 在宅療養指導管理料項目のリスト
        /// </summary>
        public List<string> ZaitakuRyoyoList
        {
            get
            {
                if (_zaitakuRyoyo == null)
                {
                    _zaitakuRyoyo = _masterFinder.GetZaitakuryoyo(_hpId, _sinDate);
                }
                return _zaiisoList;
            }
        }

        private List<string> _igakuGairaiKansenTgt;
        private List<string> _zaitakuGairaiKansenTgt;
        /// <summary>
        /// 外来感染症対策向上加算等の加算対象項目リスト
        /// </summary>
        /// <param name="koui">1:医学管理 2:在宅</param>
        /// <returns>加算対象項目リスト</returns>
        public List<string> GairaiKansenTgtList(int koui)
        {
            List<string> ls = new List<string> { };
            switch (koui)
            {
                case 1:
                    if (_igakuGairaiKansenTgt == null)
                    {
                        _igakuGairaiKansenTgt = _masterFinder.GetGairaiKansenTgt(_hpId, _sinDate, koui);
                    }
                    ls = _igakuGairaiKansenTgt;
                    break;
                case 2:
                    if (_zaitakuGairaiKansenTgt == null)
                    {
                        _zaitakuGairaiKansenTgt = _masterFinder.GetGairaiKansenTgt(_hpId, _sinDate, koui);
                    }
                    ls = _zaitakuGairaiKansenTgt;
                    break;
                default:
                    break;
            }
            return ls;
        }

        /// <summary>
        /// システム世代管理マスタ
        /// </summary>
        private List<SystemGenerationConfModel> SystemGenerationConfs
        {
            get
            {
                if (_systemGenerationConfs == null)
                {
                    _systemGenerationConfs = _masterFinder.FindSystemGenerationConf(_hpId, _sinDate);
                }
                return _systemGenerationConfs;
            }
        }

        private List<IpnKasanExcludeModel> IpnKasanExcludes
        {
            get
            {
                if (_ipnKasanExcludes == null)
                {
                    _ipnKasanExcludes = _masterFinder.FindIpnKasanExclude(_hpId, _sinDate);
                }
                return _ipnKasanExcludes;
            }
        }

        private List<IpnKasanExcludeItemModel> IpnKasanExcludeItems
        {
            get
            {
                if (_ipnKasanExcludeItems == null)
                {
                    _ipnKasanExcludeItems = _masterFinder.FindIpnKasanExcludeItem(_hpId, _sinDate);
                }
                return _ipnKasanExcludeItems;
            }
        }

        /// <summary>
        /// 指定の診療行為コードの点数マスタを取得する
        /// </summary>
        /// <param name="itemCd"></param>
        /// <returns></returns>
        public List<TenMstModel> GetTenMst(string itemCd)
        {
            List<TenMstModel> ret;
            ret = _tenMst.FindAll(p =>
                    p.ItemCd == itemCd &&
                    (p.StartDate <= _sinDate &&
                    (p.EndDate >= _sinDate || p.EndDate == 12341234)))
                .OrderByDescending(p => p.StartDate).ToList();

            if (ret.Any() == false)
            {
                ret = _masterFinder.FindTenMstByItemCd(_hpId, _sinDate, itemCd);
                _tenMst.AddRange(ret);
            }
            return ret;
        }

        /// <summary>
        /// 指定の包括区分の点数マスタを取得する
        /// </summary>
        /// <param name="hokatuKbn"></param>
        /// <returns></returns>
        public List<TenMstModel> GetTenMstByHokatuKbn(int hokatuKbn)
        {
            //var tenMsts = _tenMst.FindAll(p =>
            //    p.HpId == _hpId &&
            //    p.StartDate <= _sinDate &&
            //    p.EndDate >= _sinDate &&
            //    p.HokatuKbn == hokatuKbn
            //    );
            var tenMsts = _masterFinder.FindTenMstByHokatuKbn(_hpId, _sinDate, hokatuKbn);
            return tenMsts;
        }

        public List<TenMstModel> GetTenMstByHokatuKbn(int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno)
        {
            //var tenMsts = _tenMst.FindAll(p =>
            //    p.HpId == _hpId &&
            //    p.StartDate <= _sinDate &&
            //    p.EndDate >= _sinDate &&
            //    p.HokatuKbn == hokatuKbn
            //    );
            var tenMsts = _masterFinder.FindTenMstByHokatuKbn(_hpId, _sinDate, hokatuKbn, cdKbn, cdKbnno, cdEdano, cdKouno);
            return tenMsts;
        }
        public List<string> GetZaitakuRyoyoList()
        {
            return _masterFinder.GetZaitakuryoyo(_hpId, _sinDate);
        }
        /// <summary>
        /// 点数マスタの内容を元に表示用コメントを取得する
        /// </summary>
        /// <param name="itemCd">コメントのITEM_CD</param>
        /// <param name="cmtOpt">コメント文</param>
        /// <param name="maskEdit">true: 不足桁をマスク文字で埋める</param>
        /// <returns></returns>
        public string GetCommentStr(string itemCd, ref string cmtOpt, bool maskEdit = false)
        {
            string ret = "";

            TenMstModel tenMst = GetTenMst(itemCd).FirstOrDefault();
            if (tenMst != null)
            {
                List<int> cmtCol = new List<int>();
                List<int> cmtLen = new List<int>();

                //if (tenMst.ItemCd.Substring(0, 3) == "840")
                if (CIUtil.Copy(tenMst.ItemCd, 1, 3) == "840")
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        cmtCol.Add(tenMst.CmtCol(i));
                        cmtLen.Add(tenMst.CmtColKeta(i));
                    }
                }

                return _masterFinder.GetCommentStr(SinDate, tenMst.ItemCd, cmtCol, cmtLen, tenMst.Name, tenMst.Name, ref cmtOpt, maskEdit);
            }

            return ret;
        }

        /// <summary>
        /// 自動算定マスタに登録があるかチェック
        /// </summary>
        /// <param name="itemCd">チェックしたいITEM_CD</param>
        /// <returns>true: 登録あり</returns>
        public bool ExistAutoSantei(string itemCd)
        {
            if (AutoSanteiMsts == null)
            {
                return false;
            }
            return AutoSanteiMsts.Any(p => p.ItemCd == itemCd);
        }

        #region SYSTEM_GENERATION_CONF
        /// <summary>
        /// システム世代管理の設定値を返す
        /// </summary>
        /// <param name="grpCd">分類コード</param>
        /// <param name="grpEdaNo">分類枝番</param>
        /// <param name="defaultVal">該当するレコードがない場合に返す値</param>
        /// <returns>取得した設定値（該当するレコードがない場合、defaultVal）</returns>
        public int GetSystemGenerationConfVal(int grpCd, int grpEdaNo = 0, int defaultVal = 0)
        {
            int ret = 0;

            List<SystemGenerationConfModel> systemGenerationConfs =
                SystemGenerationConfs.FindAll(p =>
                    p.GrpCd == grpCd &&
                    p.GrpEdaNo == grpEdaNo &&
                    p.StartDate <= _sinDate &&
                    p.EndDate >= _sinDate);

            if (systemGenerationConfs.Any())
            {
                ret = systemGenerationConfs.First().Val;
            }

            return ret;
        }

        /// <summary>
        /// システム世代管理のパラメータを取得する
        /// </summary>
        /// <param name="grpCd">分類コード</param>
        /// <param name="grpEdaNo">分類枝番</param>
        /// <param name="defaultParam">該当するレコードがない場合に返す値</param>
        /// <returns>取得したパラメータ（該当するレコードがない場合、defaultParam）</returns>
        public string GetSystemGenerationConfParam(int grpCd, int grpEdaNo = 0, string defaultParam = "")
        {
            string ret = defaultParam;

            List<SystemGenerationConfModel> systemGenerationConfs =
                SystemGenerationConfs.FindAll(p =>
                    p.GrpCd == grpCd &&
                    p.GrpEdaNo == grpEdaNo &&
                    p.StartDate <= _sinDate &&
                    p.EndDate >= _sinDate);

            if (systemGenerationConfs.Any())
            {
                ret = systemGenerationConfs.First().Param;
            }

            return ret;
        }

        /// <summary>
        /// 診療日時点の消費税率を取得する
        /// </summary>
        /// <returns>診療日時点の消費税率</returns>
        public int GetZei()
        {
            return GetSystemGenerationConfVal(3001);
        }

        /// <summary>
        /// 診療日時点の軽減税率を取得する
        /// </summary>
        /// <returns>診療日時点の軽減税率</returns>
        public int GetKeigenZei()
        {
            return GetSystemGenerationConfVal(3001, 1);
        }

        /// <summary>
        /// 消費税の端数処理
        /// </summary>
        /// <returns>
        ///     0:四捨五入
        ///     1:切り捨て
        ///     2:切り上げ
        /// </returns>
        public int GetZeiHasu()
        {
            return GetSystemGenerationConfVal(3002);
        }

        /// <summary>
        /// 自費算定分に課税するかどうか
        /// </summary>
        /// <returns></returns>
        public int GetJihisanteiKazei()
        {
            return GetSystemGenerationConfVal(3003);
        }

        /// <summary>
        /// 小児科標榜
        /// </summary>
        /// <returns></returns>
        public int GetHyoboSyounika()
        {
            return GetSystemGenerationConfVal(8001, 0);
        }

        /// <summary>
        /// 皮膚科標榜
        /// </summary>
        /// <returns></returns>
        public int GetHyoboHifuka()
        {
            return GetSystemGenerationConfVal(8001, 1);
        }

        /// <summary>
        /// 産科または産婦人科標榜
        /// </summary>
        /// <returns></returns>
        public int GetHyoboSanka()
        {
            return GetSystemGenerationConfVal(8001, 2);
        }

        #endregion

        #region 電子点数表関連

        public List<DensiHoukatuMstModel> GetDensiHoukatu(long ptId, List<string> itemCds, bool isRosai, bool excludeMaybe)
        {
            return _masterFinder.GetDensiHoukatu(_hpId, ptId, _sinDate, isRosai, itemCds, excludeMaybe);
        }

        public List<DensiHoukatuMstModel> GetDensiHiHoukatu(long ptId, List<string> itemCds, bool isRosai)
        {
            return _masterFinder.GetDensiHiHoukatu(_hpId, ptId, _sinDate, isRosai, itemCds);
        }

        /// <summary>
        /// 背反マスタ取得
        /// index-0:日 1:月 2:週 3:来院 4:カスタム
        /// </summary>
        /// <param name="index">
        /// 取得する背反マスタの種類
        ///     0:日
        ///     1:月
        ///     2:週
        ///     3:来院
        ///     4:カスタム
        /// </param>
        /// <param name="itemCd"></param>
        /// <returns></returns>
        public List<DensiHaihanMstModel> GetDensiHaihanMst(int index, string itemCd, bool isKenpo)
        {
            List<DensiHaihanMstModel> ret = new List<DensiHaihanMstModel>();

            switch (index)
            {
                case 0:// 日
                    ret = GetDensiHaihanDay(itemCd, isKenpo);
                    break;
                case 1: // 月
                    ret = GetDensiHaihanMonth(itemCd, isKenpo);
                    break;
                case 2: // 週
                    ret = GetDensiHaihanWeek(itemCd, isKenpo);
                    break;
                case 3: // 来院
                    ret = GetDensiHaihanKarte(itemCd, isKenpo);
                    break;
                case 4: // カスタム
                    ret = GetDensiHaihanCustom(itemCd, isKenpo);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 背反マスタ取得
        /// </summary>
        /// <param name="itemCds">取得する項目のリスト</param>
        /// <param name="isRosai">true: 労災の場合</param>
        /// <param name="excludeMaybe">ture-可能性なし</param>
        /// <returns>指定の条件に合う背反マスタのリストを返す</returns>
        public List<DensiHaihanMstModel> GetDensiHaihanAll(List<string> itemCds, bool isRosai, bool excludeMaybe)
        {
            return _masterFinder.GetDensiHaihanAll(_hpId, _sinDate, isRosai, itemCds, excludeMaybe);
        }

        public List<PriorityHaihanMstModel> GetPriorityHaihanAll(List<string> itemCds, bool isRosai, bool excludeMaybe)
        {
            return _masterFinder.GetPriorityHaihanAll(_hpId, _sinDate, isRosai, itemCds, excludeMaybe);
        }

        /// <summary>
        /// 背反マスタ取得（日）
        /// ITEM_CD1をキーに検索、ITEM_CD2が自身を背反する項目である
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<DensiHaihanMstModel> GetDensiHaihanDay(string itemCd, bool isKenpo)
        {
            List<DensiHaihanMstModel> haihanMsts = new List<DensiHaihanMstModel>();

            var datas = _masterFinder.GetDensiHaihanDay(_hpId, _sinDate, isKenpo, itemCd);
            datas?.ForEach(data =>
            {
                haihanMsts.Add(new DensiHaihanMstModel());
                haihanMsts.Last().HpId = data.HpId;
                haihanMsts.Last().StartDate = data.StartDate;
                haihanMsts.Last().EndDate = data.EndDate;
                haihanMsts.Last().ItemCd1 = data.ItemCd1;
                haihanMsts.Last().ItemCd2 = data.ItemCd2;
                haihanMsts.Last().HaihanKbn = data.HaihanKbn;
                haihanMsts.Last().SpJyoken = data.SpJyoken;
                haihanMsts.Last().TermCnt = 1;
                haihanMsts.Last().termSbt = 2;
                haihanMsts.Last().TargetKbn = data.TargetKbn;
                haihanMsts.Last().mstSbt = 0;
            });

            return haihanMsts;
        }

        /// <summary>
        /// 背反マスタ取得（月）
        /// ITEM_CD1をキーに検索、ITEM_CD2が自身を背反する項目である
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<DensiHaihanMstModel> GetDensiHaihanMonth(string itemCd, bool isKenpo)
        {
            List<DensiHaihanMstModel> haihanMsts = new List<DensiHaihanMstModel>();

            var datas = _masterFinder.GetDensiHaihanMonth(_hpId, _sinDate, isKenpo, itemCd);
            datas?.ForEach(data =>
            {
                haihanMsts.Add(new DensiHaihanMstModel());
                haihanMsts.Last().HpId = data.HpId;
                haihanMsts.Last().StartDate = data.StartDate;
                haihanMsts.Last().EndDate = data.EndDate;
                haihanMsts.Last().ItemCd1 = data.ItemCd1;
                haihanMsts.Last().ItemCd2 = data.ItemCd2;
                haihanMsts.Last().HaihanKbn = data.HaihanKbn;
                haihanMsts.Last().SpJyoken = data.SpJyoken;
                haihanMsts.Last().TermCnt = 1;
                haihanMsts.Last().termSbt = 6;
                haihanMsts.Last().TargetKbn = data.TargetKbn;
                haihanMsts.Last().mstSbt = 1;
            });

            return haihanMsts;
        }

        /// <summary>
        /// 背反マスタ取得（週）
        /// ITEM_CD1をキーに検索、ITEM_CD2が自身を背反する項目である
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<DensiHaihanMstModel> GetDensiHaihanWeek(string itemCd, bool isKenpo)
        {
            List<DensiHaihanMstModel> haihanMsts = new List<DensiHaihanMstModel>();

            var datas = _masterFinder.GetDensiHaihanWeek(_hpId, _sinDate, isKenpo, itemCd);
            datas?.ForEach(data =>
            {
                haihanMsts.Add(new DensiHaihanMstModel());
                haihanMsts.Last().HpId = data.HpId;
                haihanMsts.Last().StartDate = data.StartDate;
                haihanMsts.Last().EndDate = data.EndDate;
                haihanMsts.Last().ItemCd1 = data.ItemCd1;
                haihanMsts.Last().ItemCd2 = data.ItemCd2;
                haihanMsts.Last().HaihanKbn = data.HaihanKbn;
                haihanMsts.Last().SpJyoken = data.SpJyoken;
                haihanMsts.Last().TermCnt = 1;
                haihanMsts.Last().termSbt = 3;
                haihanMsts.Last().TargetKbn = data.TargetKbn;
                haihanMsts.Last().mstSbt = 2;
            });

            return haihanMsts;
        }

        /// <summary>
        /// 背反マスタ取得（来院）
        /// ITEM_CD1をキーに検索、ITEM_CD2が自身を背反する項目である
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<DensiHaihanMstModel> GetDensiHaihanKarte(string itemCd, bool isKenpo)
        {
            List<DensiHaihanMstModel> haihanMsts = new List<DensiHaihanMstModel>();

            var datas = _masterFinder.GetDensiHaihanKarte(_hpId, _sinDate, isKenpo, itemCd);
            datas?.ForEach(data =>
            {
                haihanMsts.Add(new DensiHaihanMstModel());
                haihanMsts.Last().HpId = data.HpId;
                haihanMsts.Last().StartDate = data.StartDate;
                haihanMsts.Last().EndDate = data.EndDate;
                haihanMsts.Last().ItemCd1 = data.ItemCd1;
                haihanMsts.Last().ItemCd2 = data.ItemCd2;
                haihanMsts.Last().HaihanKbn = data.HaihanKbn;
                haihanMsts.Last().SpJyoken = data.SpJyoken;
                haihanMsts.Last().TermCnt = 1;
                haihanMsts.Last().termSbt = 1;
                haihanMsts.Last().TargetKbn = data.TargetKbn;
                haihanMsts.Last().mstSbt = 3;
            });

            return haihanMsts;
        }

        /// <summary>
        /// 背反マスタ取得（カスタム）
        /// ITEM_CD1をキーに検索、ITEM_CD2が自身を背反する項目である
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<DensiHaihanMstModel> GetDensiHaihanCustom(string itemCd, bool isKenpo)
        {
            List<DensiHaihanMstModel> haihanMsts = new List<DensiHaihanMstModel>();

            var datas = _masterFinder.GetDensiHaihanCustom(_hpId, _sinDate, isKenpo, itemCd);
            datas?.ForEach(data =>
            {
                haihanMsts.Add(new DensiHaihanMstModel());
                haihanMsts.Last().HpId = data.HpId;
                haihanMsts.Last().StartDate = data.StartDate;
                haihanMsts.Last().EndDate = data.EndDate;
                haihanMsts.Last().ItemCd1 = data.ItemCd1;
                haihanMsts.Last().ItemCd2 = data.ItemCd2;
                haihanMsts.Last().HaihanKbn = data.HaihanKbn;
                haihanMsts.Last().SpJyoken = data.SpJyoken;
                haihanMsts.Last().TermCnt = data.TermCnt;
                haihanMsts.Last().termSbt = data.TermSbt;
                haihanMsts.Last().TargetKbn = data.TargetKbn;
                haihanMsts.Last().mstSbt = 4;
            });

            return haihanMsts;
        }

        #endregion

        /// <summary>
        /// 一般名処方加算の対象外かどうかチェック
        /// </summary>
        /// <param name="ipnNameCd"></param>
        /// <param name="itemCd"></param>
        /// <returns></returns>
        public bool IsIpnKasanExclude(string ipnNameCd, string itemCd)
        {
            bool ret = false;
            if (IpnKasanExcludes.Any(p => p.IpnNameCd == ipnNameCd))
            {
                ret = true;
            }
            else if (IpnKasanExcludeItems.Any(p => p.ItemCd == itemCd))
            {
                ret = true;
            }

            return ret;
        }

        private List<int> unitCdls = new List<int>
            {
                    53,121,131,138,141,142,143,144,145,146,147,148,159,997,998,999
            };

        public List<DensiSanteiKaisuModel> FindDensiSanteiKaisu(int sinDate, bool isRosai, string itemCd)
        {
            List<DensiSanteiKaisuModel> masters =
                _densiSanteiKaisus.FindAll(p =>
                    p.HpId == _hpId &&
                    p.ItemCd == itemCd &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isRosai ? 2 : 1)) &&
                    p.IsInvalid == 0 &&
                    unitCdls.Contains(p.UnitCd))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenBy(p => p.UnitCd)
                //.ThenBy(p => p.StartDate)
                .ThenByDescending(p => p.UserSetting)
                .ThenBy(p => p.MaxCount)
                .ThenBy(p => p.SpJyoken)
                .ToList();

            int hpId = 0;
            string ItemCd = "";
            int unitCd = 0;
            List<DensiSanteiKaisuModel> results = new List<DensiSanteiKaisuModel>();

            // 同一unitCd内では、UserSettingが大きい方を優先する
            bool sp0 = false;
            bool sp1 = false;
            foreach (DensiSanteiKaisuModel master in masters)
            {
                if (hpId != master.HpId || ItemCd != master.ItemCd || unitCd != master.UnitCd)
                {
                    sp0 = false;
                    sp1 = false;
                }

                if ((master.SpJyoken == 0 && sp0 == false) ||
                    (master.SpJyoken == 1 && sp0 == false && sp1 == false))
                {
                    // SP_JYOKEN=0(無条件削除) で、 SP_JYOKEN=0(無条件削除)のものをまだ追加していない or
                    // SP_JYOKEN=1(条件あり) で、 SP_JYOKEN=0(無条件削除), 1(条件あり)のものをまだ追加していない
                    results.Add(master);
                }

                hpId = master.HpId;
                ItemCd = master.ItemCd;
                unitCd = master.UnitCd;

                if (master.SpJyoken == 0)
                {
                    sp0 = true;
                }
                else if (master.SpJyoken == 1)
                {
                    sp1 = true;
                }
            }
            //if (results.Any() == false)
            //{
            //    results = _masterFinder.FindDensiSanteiKaisu(_hpId, sinDate, isRosai, itemCd);
            //    _densiSanteiKaisus.AddRange(results);
            //}

            return results;
        }

        public List<DensiSanteiKaisuModel> FindDensiSanteiKaisuSyosin(int sinDate, bool isRosai, string itemCd)
        {
            List<DensiSanteiKaisuModel> masters =
                _densiSanteiKaisus.FindAll(p =>
                    p.HpId == _hpId &&
                    p.ItemCd == itemCd &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isRosai ? 2 : 1)) &&
                    p.IsInvalid == 0 &&
                    new int[] { 997, 998 }.Contains(p.UnitCd))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenBy(p => p.UnitCd)
                //.ThenBy(p => p.StartDate)
                .ThenByDescending(p => p.UserSetting)
                .ThenBy(p => p.MaxCount)
                .ThenBy(p => p.SpJyoken)
                .ToList();

            int hpId = 0;
            string ItemCd = "";
            int unitCd = 0;
            List<DensiSanteiKaisuModel> results = new List<DensiSanteiKaisuModel>();

            // 同一unitCd内では、UserSettingが大きい方を優先する
            bool sp0 = false;
            bool sp1 = false;
            foreach (DensiSanteiKaisuModel master in masters)
            {
                if (hpId != master.HpId || ItemCd != master.ItemCd || unitCd != master.UnitCd)
                {
                    sp0 = false;
                    sp1 = false;
                }

                if ((master.SpJyoken == 0 && sp0 == false) ||
                    (master.SpJyoken == 1 && sp0 == false && sp1 == false))
                {
                    // SP_JYOKEN=0(無条件削除) で、 SP_JYOKEN=0(無条件削除)のものをまだ追加していない or
                    // SP_JYOKEN=1(条件あり) で、 SP_JYOKEN=0(無条件削除), 1(条件あり)のものをまだ追加していない
                    results.Add(master);
                }

                hpId = master.HpId;
                ItemCd = master.ItemCd;
                unitCd = master.UnitCd;

                if (master.SpJyoken == 0)
                {
                    sp0 = true;
                }
                else if (master.SpJyoken == 1)
                {
                    sp1 = true;
                }
            }
            //if (results.Any() == false)
            //{
            //    results = _masterFinder.FindDensiSanteiKaisu(_hpId, sinDate, isRosai, itemCd);
            //    _densiSanteiKaisus.AddRange(results);
            //}

            return results;
        }

        /// <summary>
        /// 項目グループマスタ取得
        /// </summary>
        /// <param name="sinDate">診療日</param>
        /// <param name="grpSbt">項目グループ種別 1:算定回数マスタ</param>
        /// <param name="itemGrpCd">項目グループコード</param>
        /// <returns></returns>
        public List<ItemGrpMstModel> FindItemGrpMst(int sinDate, int grpSbt, int itemGrpCd)
        {
            List<ItemGrpMstModel> masters =
                _itemGrpMsts.FindAll(p =>
                    p.HpId == _hpId &&
                    p.GrpSbt == grpSbt &&
                    p.ItemGrpCd == itemGrpCd &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenBy(p => p.SeqNo)
                .ToList();

            return masters;
        }
        /// <summary>
        /// 指定の項目コードの指定のコメント種別に関連するコメントコードを取得
        /// </summary>
        /// <param name="ItemCd"></param>
        /// <param name="cmtSbt"></param>
        /// <returns></returns>
        public RecedenCmtSelectModel FindRecedenCmtSelect(string ItemCd, int cmtSbt)
        {
            RecedenCmtSelectModel result = null;

            if (RecedenCmtSelects != null && RecedenCmtSelects.Any(p => p.ItemCd == ItemCd && p.CmtSbt == cmtSbt))
            {
                result = RecedenCmtSelects.Find(p => p.ItemCd == ItemCd && p.CmtSbt == cmtSbt);
            }

            return result;
        }

        public List<RecedenCmtSelectModel> FindRecedenCmtSelectsByItemCd(string ItemCd)
        {
            List<RecedenCmtSelectModel> result = null;

            if (RecedenCmtSelects != null && RecedenCmtSelects.Any(p => p.ItemCd == ItemCd))
            {
                result = RecedenCmtSelects.FindAll(p => p.ItemCd == ItemCd);
            }

            return result;
        }
        public bool RecedenCmtSelectExistByItemCd(string ItemCd)
        {
            return (RecedenCmtSelects != null && RecedenCmtSelects.Any(p => p.ItemCd == ItemCd));
        }
        public List<RecedenCmtSelectModel> FindRecedenCmtSelectsByCommentCd(string CommentCd)
        {
            List<RecedenCmtSelectModel> result = null;

            if (RecedenCmtSelects != null && RecedenCmtSelects.Any(p => p.CommentCd == CommentCd))
            {
                result = RecedenCmtSelects.FindAll(p => p.CommentCd == CommentCd);
            }

            return result;
        }
        public bool RecedenCmtSelectExistByCommentCd(string CommentCd)
        {
            return (RecedenCmtSelects != null && RecedenCmtSelects.Any(p => p.CommentCd == CommentCd));
        }
        /// <summary>
        /// 行為包括マスタを取得する
        /// </summary>
        /// <param name="sinDate">診療日</param>
        /// <param name="isRosai">労災かどうか</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<KouiHoukatuMstModel> FindKouiHoukatuMst(int sinDate, bool isRosai, string itemCd = "")
        {
            List<KouiHoukatuMstModel> masters =
                _kouiHoukatuMsts.FindAll(p =>
                    p.HpId == _hpId &&
                    (string.IsNullOrEmpty(itemCd) ? true : p.ItemCd == itemCd) &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isRosai ? 2 : 1)) &&
                    p.IsInvalid == 0)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.UserSetting)
                .ToList();

            List<KouiHoukatuMstModel> results = new List<KouiHoukatuMstModel>();

            foreach (KouiHoukatuMstModel master in masters)
            {
                results.Add(master);
            }
            return results;
        }
    }

}
