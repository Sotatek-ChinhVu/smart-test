using Domain.Constant;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Ika.Constants;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Requests;
using EmrCalculateApi.Utils;
using Helper.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    class RousaiGoseiItemInfModel
    {
        public long _rpNo;
        public long _edaNo;
        public string _itemCd;
        public int _sisiKbn;
        public int _sisiKasan;

        public RousaiGoseiItemInfModel(long rpNo, long edaNo, string itemCd, int sisiKbn, int sisiKasan)
        {
            _rpNo = rpNo;
            _edaNo = edaNo;
            _itemCd = itemCd;
            _sisiKbn = sisiKbn;
            _sisiKasan = sisiKasan;
        }

        public long rpNo
        {
            get { return _rpNo; }
            set { _rpNo = value; }
        }

        public long edaNo
        {
            get { return _edaNo; }
            set { _edaNo = value; }
        }

        public string itemCd
        {
            get { return _itemCd; }
            set { _itemCd = value; }
        }

        public int sisiKbn
        {
            get { return _sisiKbn; }
            set { _sisiKbn = value; }
        }

        public int sisiKasan
        {
            get { return _sisiKasan; }
            set { _sisiKasan = value; }
        }
    }

    public class IkaCalculateCommonOdrDataViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        OdrInfFinder _odrInfFinder;
        MasterFinder _masterFinder;

        #region Order Data
        private List<OdrInfModel> _odrInfModels;
        private List<OdrDtlTenModel> _odrDtlTenModels;
        private List<OdrInfCmtModel> _odrInfCmtModels;
        #endregion

        private List<RousaiGoseiMstModel> _rousaiGoseiMsts;

        int _hpId;
        long _ptId;
        int _sinDate;
        int _hokenKbn;
        long _raiinNo;

        int _existIngaiSyoho;
        int _existsIngaiSyohoInMonth;
        int _existsInnaiSyoho;
        int _existsInnaiSyohoInMonth;


        int _existsIngaiSyohoInMonthHokenSyu;

        int _existsInnaiSyohoInMonthHokenSyu;

        int _isRefill;

        private List<int> checkHokenKbn;
        private List<int> checkSanteiKbn;

        List<(long raiinNo, int hokenSyu)> _hokenSyuList;

        IkaCalculateCommonMasterViewModel _tenMstCommon;

        private List<(int hokenPid, int hokenId, int santeiKbn)> _hokenPids;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public IkaCalculateCommonOdrDataViewModel(OdrInfFinder odrInfFinder, MasterFinder masterFinder, IkaCalculateCommonMasterViewModel tenMstCommon, int hpId, long ptId, int sinDate, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(IkaCalculateCommonOdrDataViewModel);

            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;

            _emrLogger.WriteLogStart(this, conFncName, "");

            SetArgData(odrInfFinder, masterFinder, tenMstCommon, hpId, ptId, sinDate);

            //指定の診療日のオーダー情報を取得する
            _emrLogger.WriteLogMsg(this, conFncName, "get odrinf start");
            //_odrInfModels = odrInfFinder.FindOdrInfData(hpId, ptId, sinDate);
            //_emrLogger.WriteLogMsg( this, conFncName, "get odrdtl start", ICDebugConf.logLevel);
            //_odrDtlTenModels = odrInfFinder.FindOdrInfDetailData(hpId, ptId, sinDate);

            (_odrInfModels, _odrDtlTenModels) = odrInfFinder.FindOdrInfDetailDatas(hpId, ptId, sinDate);

            if (_systemConfigProvider.GetReceNoDspComment() == 0)
            {
                if (_systemConfigProvider.GetOutDrugYohoDsp() == 0)
                {
                    _odrDtlTenModels =
                        _odrDtlTenModels.FindAll(p =>
                            p.IsComment == false ||
                            p.IsNodspRece == 0
                            );
                }
                else
                {
                    _odrDtlTenModels =
                        _odrDtlTenModels.FindAll(p =>
                            p.IsComment == false ||
                            p.IsNodspRece == 0 ||
                            (p.OdrKouiKbn >= OdrKouiKbnConst.TouyakuMin && p.OdrKouiKbn <= OdrKouiKbnConst.TouyakuMax && p.InoutKbn == 1)
                            );
                }
            }
            _emrLogger.WriteLogMsg(this, conFncName, "get odrdtlcmt start");
            _odrInfCmtModels = odrInfFinder.FindOdrInfCmtData(hpId, ptId, sinDate);

            // 検査重複オーダー削除
            //DelSameKensa();

            //注射アンプルの調整
            CheckChusyaAmpoule();

            // オーダーで使用している保険種のリスト
            _hokenSyuList = GetHokenSyuList();

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        public IkaCalculateCommonOdrDataViewModel(List<OrderInfo> todayOdrInfs, RaiinInfModel raiinInfModel, List<PtHokenPatternModel> ptHokenPatternModels, OdrInfFinder odrInfFinder, MasterFinder masterFinder, IkaCalculateFinder ikaCalculateFinder, IkaCalculateCommonMasterViewModel tenMstCommon, int hpId, long ptId, int sinDate, IEmrLogger emrLogger, ISystemConfigProvider systemConfigProvider)
        {
            _emrLogger = emrLogger;
            _systemConfigProvider = systemConfigProvider;

            const string conFncName = nameof(IkaCalculateCommonOdrDataViewModel);

            _emrLogger.WriteLogStart(this, conFncName, "");

            SetArgData(odrInfFinder, masterFinder, tenMstCommon, hpId, ptId, sinDate);
            _raiinNo = raiinInfModel.RaiinNo;

            //オーダー情報をコンバートする

            //まず、指定の診療日のオーダー情報を取得する
            _emrLogger.WriteLogMsg(this, conFncName, "get odrinf start");
            //_odrInfModels = odrInfFinder.FindOdrInfData(hpId, ptId, sinDate);
            //_emrLogger.WriteLogMsg( this, conFncName, "get odrdtl start", ICDebugConf.logLevel);
            //_odrDtlTenModels = odrInfFinder.FindOdrInfDetailData(hpId, ptId, sinDate);

            (_odrInfModels, _odrDtlTenModels) = odrInfFinder.FindOdrInfDetailDatas(hpId, ptId, sinDate, _raiinNo);
            if (_systemConfigProvider.GetReceNoDspComment() == 0)
            {
                if (_systemConfigProvider.GetOutDrugYohoDsp() == 0)
                {
                    _odrDtlTenModels =
                        _odrDtlTenModels.FindAll(p =>
                            p.IsComment == false ||
                            p.IsNodspRece == 0
                            );
                }
                else
                {
                    _odrDtlTenModels =
                        _odrDtlTenModels.FindAll(p =>
                            p.IsComment == false ||
                            p.IsNodspRece == 0 ||
                            (p.OdrKouiKbn >= OdrKouiKbnConst.TouyakuMin && p.OdrKouiKbn <= OdrKouiKbnConst.TouyakuMax && p.InoutKbn == 1)
                            );
                }
            }
            _emrLogger.WriteLogMsg(this, conFncName, "get odrdtlcmt start");
            _odrInfCmtModels = odrInfFinder.FindOdrInfCmtData(hpId, ptId, sinDate, _raiinNo);

            //// 今から計算する来院のオーダー情報を削除する
            //_odrInfModels.RemoveAll(p => p.RaiinNo == _raiinNo);
            //_odrDtlTenModels.RemoveAll(p => p.RaiinNo == _raiinNo);
            //_odrInfCmtModels.RemoveAll(p => p.RaiinNo == _raiinNo);

            List<string> itemCds = new List<string>();
            List<string> ipnCds = new List<string>();

            foreach (OrderInfo todayOdrInf in todayOdrInfs.FindAll(p => p.SanteiKbn == SanteiKbnConst.Santei || p.SanteiKbn == SanteiKbnConst.Jihi))
            {
                itemCds.AddRange(todayOdrInf.DetailInfoList.Select(p => p.ItemCd).Distinct().ToList());
            }

            _emrLogger.WriteLogMsg(this, conFncName, "get tenmst start");
            List<TenMstModel> tenMsts = new List<TenMstModel>();
            List<CmtKbnMstModel> cmtKbnMsts = new List<CmtKbnMstModel>();

            if (itemCds.Any())
            {
                tenMsts = _masterFinder.FindTenMst(hpId, sinDate, itemCds);

                // 算定用の項目コードも
                itemCds = tenMsts.Where(p => p.ItemCd != p.SanteiItemCd && p.SanteiItemCd != ItemCdConst.NoSantei).Select(p => p.SanteiItemCd).ToList();
                if (itemCds.Any())
                {
                    tenMsts.AddRange(_masterFinder.FindTenMst(hpId, sinDate, itemCds));
                    cmtKbnMsts.AddRange(_masterFinder.FindCmtKbnMst(hpId, sinDate, itemCds));
                }

                ipnCds = tenMsts.Where(p => string.IsNullOrEmpty(p.IpnNameCd) == false).Select(p => p.IpnNameCd).Distinct().ToList();
            }

            List<IpnKasanMstModel> ipnKasans = new List<IpnKasanMstModel>();
            List<IpnMinYakkaMstModel> ipnMinYakkas = new List<IpnMinYakkaMstModel>();

            if (ipnCds.Any())
            {
                ipnKasans = _masterFinder.FindIpnKasanMst(hpId, sinDate, ipnCds);
                ipnMinYakkas = _masterFinder.FindIpnMinYakkaMst(hpId, sinDate, ipnCds);
            }

            _emrLogger.WriteLogMsg(this, conFncName, "get convert start");
            // コンバート
            foreach (OrderInfo todayOdrInf in todayOdrInfs.FindAll(p => p.SanteiKbn == SanteiKbnConst.Santei || p.SanteiKbn == SanteiKbnConst.Jihi))
            {
                PtHokenPatternModel ptHokenPatternModel =
                    ptHokenPatternModels.FirstOrDefault(p => p.HokenPid == todayOdrInf.HokenPid);

                if (ptHokenPatternModel == null)
                {
                    // 保険組み合わせ情報が取得できなかった場合
                    _emrLogger.WriteLogMsg(this, conFncName, $"ptHokenPattern not found HokenPid:{todayOdrInf.HokenPid}");

                    // エラーを発生させ、計算を中断する
                    throw new Exception($"マスタに登録のない保険組み合わせIDがあります。[OdrKouiKbn:{todayOdrInf.OdrKouiKbn}, Pid:{todayOdrInf.HokenPid}]");
                }
                else
                {
                    OdrInfDataModel odrInfData =
                        new OdrInfDataModel(
                            hpId: todayOdrInf.HpId,
                            ptId: todayOdrInf.PtId,
                            sinDate: todayOdrInf.SinDate,
                            raiinNo: todayOdrInf.RaiinNo,
                            rpNo: todayOdrInf.RpNo,
                            rpEdaNo: todayOdrInf.RpEdaNo,
                            hokenId: todayOdrInf.HokenPid,
                            odrKouiKbn: todayOdrInf.OdrKouiKbn,
                            inoutKbn: todayOdrInf.InoutKbn,
                            sikyuKbn: todayOdrInf.SikyuKbn,
                            syohoSbt: todayOdrInf.SyohoSbt,
                            santeiKbn: todayOdrInf.SanteiKbn,
                            daysCnt: todayOdrInf.DaysCnt,
                            sortNo: todayOdrInf.SortNo,
                            isDeleted: todayOdrInf.IsDeleted
                            );

                    OdrInfModel odrInf = new OdrInfModel(odrInfData, ptHokenPatternModel, raiinInfModel);
                    _odrInfModels.Add(odrInf);

                    foreach (OrderDetailInfo todayOdrInfDtl in todayOdrInf.DetailInfoList)
                    {
                        OdrInfDetailDataModel odrInfDtlData =
                            new OdrInfDetailDataModel(
                                hpId: todayOdrInfDtl.HpId,
                                ptId: todayOdrInfDtl.PtId,
                                sinDate: todayOdrInfDtl.SinDate,
                                raiinNo: todayOdrInfDtl.RaiinNo,
                                rpNo: todayOdrInfDtl.RpNo,
                                rpEdaNo: todayOdrInfDtl.RpEdaNo,
                                rowNo: todayOdrInfDtl.RowNo,
                                itemCd: todayOdrInfDtl.ItemCd,
                                itemName: todayOdrInfDtl.ItemName,
                                suryo: todayOdrInfDtl.Suryo,
                                unitName: todayOdrInfDtl.UnitName,
                                unitSBT: 0, //todayOdrInfDtl.UnitSBT,
                                termVal: todayOdrInfDtl.TermVal,
                                kohatuKbn: 0,   //todayOdrInfDtl.KohatuKbn,
                                syohoKbn: todayOdrInfDtl.SyohoKbn,
                                syohoLimitKbn: 0,   //todayOdrInfDtl.SyohoLimitKbn,
                                isNodspRece: todayOdrInfDtl.IsNodspRece,
                                yohoKbn: todayOdrInfDtl.YohoKbn,
                                ipnCd: todayOdrInfDtl.IpnCd,
                                ipnName: todayOdrInfDtl.IpnName,
                                bunkatu: "",    //todayOdrInfDtl.Bunkatu,
                                cmtName: "",    //todayOdrInfDtl.CmtName,
                                cmtOpt: todayOdrInfDtl.CmtOpt,
                                isdummy: todayOdrInfDtl.IsDummy
                                );


                        TenMstModel tenMst = _FindTenMst(todayOdrInfDtl.ItemCd);

                        string receName = "";
                        if (tenMst != null)
                        {
                            if (!string.IsNullOrEmpty(todayOdrInfDtl.ItemCd) && todayOdrInfDtl.ItemCd.StartsWith("IGE") && tenMst != null)
                            {
                                receName = tenMst.ReceName;
                            }

                            if (tenMst.SanteiItemCd != tenMst.ItemCd && tenMst.ItemCd != null && tenMst.ItemCd != "" && tenMst.ItemCd.StartsWith("Z") == false && tenMst.SanteiItemCd != ItemCdConst.NoSantei)
                            {
                                tenMst = _FindTenMst(tenMst.SanteiItemCd);
                            }
                        }

                        CmtKbnMstModel cmtKbn = _FindCmtMst(todayOdrInfDtl.ItemCd, tenMst?.SanteiItemCd ?? "");

                        IpnKasanMstModel ipnKasanMst = null;
                        IpnMinYakkaMstModel ipnMinYakkaMst = null;

                        if (todayOdrInf.OdrKouiKbn >= 20 && todayOdrInf.OdrKouiKbn <= 29 && tenMst != null)
                        {
                            ipnKasanMst = _FindIpnKasanMst(tenMst.IpnNameCd);
                            ipnMinYakkaMst = _FindIpnMinYakkaMst(tenMst.IpnNameCd);
                        }

                        OdrDtlTenModel odrDtl =
                            new OdrDtlTenModel(
                                odrInfDtlData, tenMst, (cmtKbn?.CmtKbnMst ?? null),
                                receName: receName,
                                hokenKbn: ptHokenPatternModel.HokenKbn, hokenPid: ptHokenPatternModel.HokenPid, hokenId: ptHokenPatternModel.HokenId, hokenSbt: ptHokenPatternModel.HokenSbtCd,
                                odrKouiKbn: todayOdrInf.OdrKouiKbn, santeiKbn: todayOdrInf.SanteiKbn, inoutKbn: todayOdrInf.InoutKbn,
                                syohoSbt: todayOdrInf.SyohoSbt, daysCnt: todayOdrInf.DaysCnt, sortNo: todayOdrInf.SortNo,
                                kasan1: ipnKasanMst?.Kasan1 ?? 0, kasan2: ipnKasanMst?.Kasan2 ?? 0,
                                sinStartTime: raiinInfModel.SinStartTime,
                                minYakka: ipnMinYakkaMst?.Yakka ?? 0);
                        _odrDtlTenModels.Add(odrDtl);

                        if (tenMst != null && tenMst.ItemCd != null && tenMst.ItemCd.StartsWith("Z"))
                        {
                            var tenEntities = masterFinder.FindTenMstByItemCd(HpId, todayOdrInfDtl.SinDate, tenMst.SanteiItemCd);
                            if (tenEntities.FirstOrDefault() != null)
                            {
                                _odrDtlTenModels.Last().Z_MasterSbt = tenEntities.First().MasterSbt;
                                _odrDtlTenModels.Last().Z_TenId = tenEntities.First().TenId;
                            }
                        }

                    }
                }

                #region local method
                // 点数マスタ取得、キャッシュ優先
                TenMstModel _FindTenMst(string itemCd)
                {
                    TenMstModel result =
                        tenMsts.FirstOrDefault(p => p.ItemCd == itemCd && p.StartDate <= sinDate && p.EndDate >= sinDate);
                    if (result == null)
                    {
                        result = _masterFinder.FindTenMstByItemCd(hpId, sinDate, itemCd).FirstOrDefault();
                    }

                    return result;
                }

                CmtKbnMstModel _FindCmtMst(string itemCd, string santeiItemCd)
                {
                    CmtKbnMstModel result =
                        cmtKbnMsts.FirstOrDefault(p => p.ItemCd == itemCd && p.StartDate <= sinDate && p.EndDate >= sinDate);

                    if (result == null)
                    {
                        if (itemCd != santeiItemCd && string.IsNullOrEmpty(santeiItemCd) == false)
                        {
                            result =
                                cmtKbnMsts.FirstOrDefault(p => p.ItemCd == santeiItemCd && p.StartDate <= sinDate && p.EndDate >= sinDate);
                        }
                    }

                    if (result == null)
                    {
                        result = _masterFinder.FindCmtKbnMstByItemCd(hpId, sinDate, itemCd).FirstOrDefault();
                    }

                    if (result == null)
                    {
                        if (itemCd != santeiItemCd && string.IsNullOrEmpty(santeiItemCd) == false)
                        {
                            result =
                                _masterFinder.FindCmtKbnMstByItemCd(hpId, sinDate, santeiItemCd).FirstOrDefault();
                        }
                    }

                    return result;
                }

                // 一般名処方加算マスタ取得、キャッシュ優先
                IpnKasanMstModel _FindIpnKasanMst(string ipnCd)
                {
                    IpnKasanMstModel result =
                        ipnKasans.FirstOrDefault(p => p.IpnNameCd == ipnCd);

                    //if (result == null)
                    //{
                    //    result = _masterFinder.FindIpnKasanMst(hpId, sinDate, ipnCd).FirstOrDefault();
                    //}

                    return result;
                }

                // 最低薬価マスタ取得、キャッシュ優先
                IpnMinYakkaMstModel _FindIpnMinYakkaMst(string ipnCd)
                {
                    IpnMinYakkaMstModel result =
                        ipnMinYakkas.FirstOrDefault(p => p.IpnNameCd == ipnCd);

                    //if (result == null)
                    //{
                    //    _masterFinder.FindIpnMinYakkaMst(hpId, sinDate, ipnCd).FirstOrDefault();
                    //}

                    return result;

                }
                #endregion
            }

            // 検査重複オーダー削除
            //DelSameKensa();

            // 注射アンプルの調整
            CheckChusyaAmpoule();

            // オーダーで使用している保険種のリスト
            _hokenSyuList = GetHokenSyuList();

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        private void SetArgData(OdrInfFinder odrInfFinder, MasterFinder masterFinder, IkaCalculateCommonMasterViewModel tenMstCommon, int hpId, long ptId, int sinDate)
        {
            const string conFncName = nameof(SetArgData);

            // ローカルデータ初期化
            InitializeLocalData();

            _emrLogger.WriteLogMsg(this, conFncName, "init end");

            _odrInfFinder = odrInfFinder;

            _emrLogger.WriteLogMsg(this, conFncName, "get odrfinder end");
            _masterFinder = masterFinder;
            _emrLogger.WriteLogMsg(this, conFncName, "get mstfinder end");

            _tenMstCommon = tenMstCommon;

            _emrLogger.WriteLogMsg(this, conFncName, "mstCommon end");

            _hpId = hpId;
            _ptId = ptId;
            _sinDate = sinDate;
        }

        private void InitializeLocalData()
        {
            _existIngaiSyoho = -1;
            _existsIngaiSyohoInMonth = -1;
            _existsInnaiSyoho = -1;
            _existsInnaiSyohoInMonth = -1;

            _existsIngaiSyohoInMonthHokenSyu = -1;
            _existsInnaiSyohoInMonthHokenSyu = -1;
            _hokenPids = null;

            _isRefill = -1;
        }

        /// <summary>
        /// オーダー情報
        /// </summary>
        public List<OdrInfModel> OdrInfls
        {
            get { return _odrInfModels; }
        }

        /// <summary>
        /// オーダー情報詳細
        /// </summary>
        public List<OdrDtlTenModel> odrDtlls
        {
            get { return _odrDtlTenModels; }
            set { _odrDtlTenModels = value; }
        }

        /// <summary>
        /// オーダーコメント情報
        /// </summary>
        public List<OdrInfCmtModel> odrCmtls
        {
            get { return _odrInfCmtModels; }
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
            set
            {
                _hpId = value;
                InitializeLocalData();
            }
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get { return _ptId; }
            set
            {
                _ptId = value;
                InitializeLocalData();
            }
        }

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
            set
            {
                _sinDate = value;
                InitializeLocalData();
            }
        }

        /// <summary>
        /// 保険種
        /// </summary>
        public int HokenKbn
        {
            get { return _hokenKbn; }
            set
            {
                _hokenKbn = value;
                InitializeLocalData();

                checkHokenKbn = CalcUtils.GetCheckHokenKbns(_hokenKbn, _systemConfigProvider.GetHokensyuHandling());
                checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(_hokenKbn, _systemConfigProvider.GetHokensyuHandling());
            }
        }

        /// <summary>
        /// 予約番号
        /// </summary>
        public long RaiinNo
        {
            get { return _raiinNo; }
            set
            {
                _raiinNo = value;
                InitializeLocalData();
            }
        }

        /// <summary>
        /// 労災合成コードマスタ
        /// </summary>
        private List<RousaiGoseiMstModel> RousaiGoseiMsts
        {
            get
            {
                if (_rousaiGoseiMsts == null)
                {
                    _rousaiGoseiMsts = _masterFinder.FindRousaiGoseiMst(_hpId, _sinDate);
                }
                return _rousaiGoseiMsts;
            }
        }

        /// <summary>
        /// 検査で使用しているHOKEN_PIDのリスト
        /// </summary>
        public List<(int hokenPid, int hokenId, int santeiKbn)> HokenPidList
        {
            get
            {
                if (_hokenPids == null)
                {
                    _hokenPids = new List<(int, int, int)>();

                    // 検査で使用しているHOKEN_PID
                    var ret = _odrDtlTenModels.FindAll(p =>
                                    p.HpId == _hpId &&
                                    p.PtId == _ptId &&
                                    p.RaiinNo == _raiinNo &&
                                    p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                                    p.SinKouiKbn >= OdrKouiKbnConst.KensaMin &&
                                    p.SinKouiKbn <= OdrKouiKbnConst.KensaMax)
                                .OrderBy(p => p.HokenPid)
                                .GroupBy(p => new { hokenPid = p.HokenPid, hokenId = p.HokenId, santeiKbn = p.SanteiKbn });

                    foreach (var odrDtl in ret)
                    {
                        _hokenPids.Add((odrDtl.Key.hokenPid, odrDtl.Key.hokenId, odrDtl.Key.santeiKbn));
                    }
                }
                return _hokenPids;
            }
        }
        /// <summary>
        /// 検査で使用しているHOKEN_PIDのリスト
        /// </summary>
        public List<(int hokenPid, int hokenId, int santeiKbn)> GetKensaHokenPidList(int hokatuKensa, int santeiKbn)
        {
            List<(int hokenPid, int hokenId, int santeiKbn)> results = new List<(int hokenPid, int hokenId, int santeiKbn)>();

            // 検査で使用しているHOKEN_PID
            var ret = _odrDtlTenModels.FindAll(p =>
                            p.HpId == _hpId &&
                            p.PtId == _ptId &&
                            p.RaiinNo == _raiinNo &&
                            p.HokatuKensa == hokatuKensa &&
                            //p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                            p.SanteiKbn == santeiKbn &&
                            p.SinKouiKbn >= OdrKouiKbnConst.KensaMin &&
                            p.SinKouiKbn <= OdrKouiKbnConst.KensaMax)
                        .OrderBy(p => p.HokenPid)
                        .GroupBy(p => new { hokenPid = p.HokenPid, hokenId = p.HokenId, santeiKbn = p.SanteiKbn });

            foreach (var odrDtl in ret)
            {
                results.Add((odrDtl.Key.hokenPid, odrDtl.Key.hokenId, odrDtl.Key.santeiKbn));
            }

            return results;
        }
        /// <summary>
        /// 検査重複オーダーの削除
        /// </summary>
        public HashSet<(int, long, string, string)> DelSameKensa()
        {
            HashSet<(int, long, string, string)> delItemNames = new HashSet<(int, long, string, string)>();

            // 検査重複オーダー削除
            IOrderedEnumerable<OdrDtlTenModel> sortOdrDtls =
                _odrDtlTenModels.FindAll
                    (p => p.RaiinNo == RaiinNo &&
                          p.SinKouiKbn >= OdrKouiKbnConst.KensaMin &&
                          p.SinKouiKbn <= OdrKouiKbnConst.KensaMax &&
                          //p.UnitName == "" && 
                          p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                          (p.TenMst == null || new string[] { "1", "3", "5" }.Contains(p.Kokuji2)) &&
                          (
                            //(p.KensaFukusuSantei == 0 && (p.OdrItemCd.StartsWith("KN") || p.KensaItemCd != "")) ||
                            (p.KensaFukusuSantei == 0 && (p.OdrItemCd.StartsWith("KN") || p.OdrItemCd.StartsWith("IGE"))) ||
                            (p.KensaFukusuSantei == 2)
                          ) //&& p.HokatuKensa != HokatuKensaConst.IgeHrt
                    )
                    .OrderBy(p => p.RaiinNo)
                    .ThenBy(p => p.SanteiItemCd)
                    .ThenBy(p => p.OdrItemCd);

            string preItemCd = "";
            string preItemName = "";
            string preOdrItemCd = "";

            foreach (OdrDtlTenModel odrDtl in sortOdrDtls)
            {
                if (
                    (!(odrDtl.OdrItemCd.StartsWith("IGE")) && (!preOdrItemCd.StartsWith("IGE")) && (odrDtl.SanteiItemCd == preItemCd)) ||
                    ((odrDtl.OdrItemCd.StartsWith("IGE")) && (odrDtl.SanteiItemCd == preItemCd && odrDtl.OdrItemCd == preOdrItemCd))
                    )
                {
                    _odrDtlTenModels.RemoveAll
                        (p => p.RaiinNo == odrDtl.RaiinNo && p.RpNo == odrDtl.RpNo && p.RpEdaNo == odrDtl.RpEdaNo && p.RowNo == odrDtl.RowNo);

                    if (odrDtl.ItemName.Trim() == preItemName)
                    {
                        delItemNames.Add((odrDtl.SinDate, odrDtl.RaiinNo, odrDtl.ItemName.Trim(), ""));
                    }
                    else
                    {
                        delItemNames.Add((odrDtl.SinDate, odrDtl.RaiinNo, odrDtl.ItemName.Trim(), preItemName));
                    }
                }
                else
                {
                    preItemCd = odrDtl.SanteiItemCd;
                    preItemName = odrDtl.ItemName.Trim();
                    preOdrItemCd = odrDtl.OdrItemCd;
                }
            }

            return delItemNames;
        }

        private void CheckChusyaAmpoule()
        {
            // 注射薬で残量破棄のものを取得
            List<OdrDtlTenModel> chusyaAmpoules =
                _odrDtlTenModels.FindAll(p =>
                    p.OdrKouiKbn >= OdrKouiKbnConst.ChusyaMin &&
                    p.OdrKouiKbn <= OdrKouiKbnConst.ChusyaMax &&
                    p.TenMst != null &&
                    new int[] { 1, 2 }.Contains(p.TenMst.SuryoRoundupKbn) &&
                    p.TenMst.DrugKbn == 4
                    );

            HashSet<(string, long)> upItemCds = new HashSet<(string, long)>();

            foreach (OdrDtlTenModel chusyaAmpoule in chusyaAmpoules)
            {
                if (_odrDtlTenModels.Count(p =>
                     //p.OdrKouiKbn >= OdrKouiKbnConst.ChusyaMin &&
                     //p.OdrKouiKbn <= OdrKouiKbnConst.ChusyaMax &&
                     p.ItemCd == chusyaAmpoule.ItemCd &&
                     p.RaiinNo == chusyaAmpoule.RaiinNo) > 1)
                {
                    // 同じ来院・オーダー行為区分に同じ薬が複数の場合
                    upItemCds.Add((chusyaAmpoule.ItemCd, chusyaAmpoule.RaiinNo));
                }
            }

            foreach ((string upItemCd, long upRaiinNo) in upItemCds)
            {
                foreach (OdrDtlTenModel upOdrDtl in chusyaAmpoules.FindAll(p =>
                    p.ItemCd == upItemCd && p.RaiinNo == upRaiinNo))
                {
                    if (upOdrDtl.OdrKouiKbn == OdrKouiKbnConst.Tenteki &&
                       _odrDtlTenModels.Any(p =>
                         p.RaiinNo == upOdrDtl.RaiinNo &&
                         p.RpNo == upOdrDtl.RpNo &&
                         p.RpEdaNo == upOdrDtl.RpEdaNo &&
                         new string[] { ItemCdConst.ChusyaSyuginasi33, ItemCdConst.ChusyaHoumonTenteki }.Contains(p.ItemCd)) == false)
                    {
                        // 点滴で、手技なしダミーも訪問点滴もないRpに属している場合は、調整しない
                    }
                    else if (upOdrDtl.TenMst.SuryoRoundupKbn == 1)
                    {
                        // 注射のみ切り上げ → 切り上げない
                        upOdrDtl.TenMst.SuryoRoundupKbn = 0;
                    }
                    else if (upOdrDtl.TenMst.SuryoRoundupKbn == 2)
                    {
                        // 注射以外も切り上げ → 切り上げない
                        upOdrDtl.TenMst.SuryoRoundupKbn = 3;
                    }
                }
            }

            // 残量破棄ダミーチェック
            List<string> zanryoHakils =
                new List<string>()
                {
                    ItemCdConst.ChusyaZanryoHaki,
                    ItemCdConst.ZaitakuZanryoHaki,
                    ItemCdConst.SyotiZanryoHaki,
                    ItemCdConst.SyujyutuZanryoHaki
                };

            // 残量破棄しない薬剤をチェック
            foreach (OdrDtlTenModel zanryoHakiDrug in
                        _odrDtlTenModels.FindAll(p =>
                            p.TenMst != null &&
                            new int[] { 0, 1 }.Contains(p.TenMst.SuryoRoundupKbn) &&
                            p.DrugKbn > 0
                            ))
            {
                if (zanryoHakiDrug.OdrKouiKbn == OdrKouiKbnConst.Tenteki)
                {
                    // 点滴の場合、注射行為のオーダー内に残量破棄があるかチェック
                    if (_odrDtlTenModels.Any(p =>
                            p.OdrKouiKbn >= OdrKouiKbnConst.ChusyaMin &&
                            p.OdrKouiKbn <= OdrKouiKbnConst.ChusyaMax &&
                            p.RaiinNo == zanryoHakiDrug.RaiinNo &&
                            zanryoHakils.Contains(p.ItemCd)))
                    {
                        zanryoHakiDrug.TenMst.SuryoRoundupKbn = 2;

                        //break;
                    }

                }
                else
                {
                    // 同一Rp内に残量破棄があるか？
                    if (_odrDtlTenModels.Any(p =>
                            p.RaiinNo == zanryoHakiDrug.RaiinNo &&
                            p.RpNo == zanryoHakiDrug.RpNo &&
                            p.RpEdaNo == zanryoHakiDrug.RpEdaNo &&
                            zanryoHakils.Contains(p.ItemCd)))
                    {
                        zanryoHakiDrug.TenMst.SuryoRoundupKbn = 2;

                        //break;
                    }
                }
            }
        }

        /// <summary>
        /// 労災合成項目の生成
        /// </summary>
        //public void MakeRosaiGosei(bool IsRosai)
        public void MakeRosaiGosei()
        {
            //if (IsRosai)
            //{
            // 労災の場合

            //労災四肢加算１．５倍
            List<string> rousaiSisi1_5 =
                new List<string>
                {
                                    ItemCdConst.SonotaRosaiSisiKasan,
                                    ItemCdConst.SyotiRosaiSisiKasan,
                                    ItemCdConst.SyujyutuRosaiSisiKasan
                };

            //労災四肢加算２倍
            List<string> rousaiSisi2 =
                new List<string>
                {
                                    ItemCdConst.SyotiRosaiSisiKasan2,
                                    ItemCdConst.SyujyutuRosaiSisiKasan2
                };

            // 保険種
            List<int> hokensyus =
                new List<int>
                {
                        HokenSyu.Rosai,
                        HokenSyu.After
                };
            if (_systemConfigProvider.GetJibaiJunkyo() == 1)
            {
                hokensyus.Add(HokenSyu.Jibai);
            }

            // 合成になるかもしれない項目のリスト
            List<RousaiGoseiItemInfModel> roOdrls =
                new List<RousaiGoseiItemInfModel>();

            // 新しいRpNo
            int newRpNo = 0;

            if (RousaiGoseiMsts == null || RousaiGoseiMsts.Any() == false)
            {
                // マスタがない場合は処理しない
                return;
            }

            // 最大の合成グループコードを取得
            var maxGrp = RousaiGoseiMsts.Max(p => p.GoseiGrp);

            if (maxGrp != null)
            {
                for (int i = 1; i <= maxGrp; i++)
                {
                    // 合成グループコードの回数回す

                    // このグループに含まれるITEM_CDを取得（合成化する可能性がある診療行為コード）
                    var grp = RousaiGoseiMsts.Where(p =>
                            p.GoseiGrp == i)
                            .GroupBy(p => new { itemCd = p.ItemCd })
                            .ToList();

                    if (grp != null)
                    {
                        List<string> itemCds = new List<string>();

                        foreach (var rousaiGoseiMst in grp)
                        {
                            itemCds.Add(rousaiGoseiMst.Key.itemCd);
                        }

                        // 対象となりうる項目を含むオーダー詳細を取得
                        List<OdrDtlTenModel> filteredOdrDtls =
                            _odrDtlTenModels.FindAll(p =>
                                p.RaiinNo == _raiinNo &&
                                itemCds.Contains(p.ItemCd) &&
                                hokensyus.Contains(p.HokenSyu) &&
                                p.SanteiKbn != SanteiKbnConst.SanteiGai);

                        if (filteredOdrDtls.Any())
                        {
                            // 対象項目がオーダーされている場合

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtls)
                            {
                                // RpNo, RpEdaNoを記憶する
                                roOdrls.Add(new RousaiGoseiItemInfModel(odrDtl.RpNo, odrDtl.RpEdaNo, odrDtl.ItemCd, odrDtl.SisiKbn, 0));
                            }

                            // roOdrlsの重複カット
                            List<RousaiGoseiItemInfModel> uniqueRoOdrls =
                                roOdrls.Distinct()
                                .OrderBy(p => p.rpNo)
                                .ThenBy(p => p.edaNo)
                                .ToList();

                            // Rp内をチェックし、加算対象かどうかチェック
                            for (int j = 0; j < uniqueRoOdrls.Count; j++)
                            {
                                filteredOdrDtls =
                                    _odrDtlTenModels.FindAll(p =>
                                        p.RpNo == uniqueRoOdrls[j].rpNo &&
                                        p.RpEdaNo == uniqueRoOdrls[j].edaNo &&
                                        p.SanteiKbn != SanteiKbnConst.SanteiGai);

                                foreach (OdrDtlTenModel odrDtl in filteredOdrDtls)
                                {
                                    int kasanKbn = 0;

                                    if (rousaiSisi2.Contains(odrDtl.ItemCd) ||
                                       (odrDtl.BuiKbn == 10 &&
                                            (uniqueRoOdrls[j].sisiKbn == 1 || uniqueRoOdrls[j].sisiKbn == 3)))
                                    {
                                        // 労災四肢加算2.0倍の項目を含んでいる or 
                                        // 指の部位(BUI_KBN=10)を含んでおり、当該項目が1.5倍又は2.0倍の対象(SISI_KBN=1)か、2.0倍のみ対象(SISI_KBN=3)
                                        // の場合
                                        kasanKbn = 2;
                                    }
                                    else if (rousaiSisi1_5.Contains(odrDtl.ItemCd) ||
                                            (odrDtl.BuiKbn == 3 &&
                                                (uniqueRoOdrls[j].sisiKbn == 1 || uniqueRoOdrls[j].sisiKbn == 2)) ||
                                            (odrDtl.BuiKbn == 10 &&
                                                (uniqueRoOdrls[j].sisiKbn == 1 || uniqueRoOdrls[j].sisiKbn == 3)))
                                    {
                                        // 労災四肢加算1.5倍の項目を含んでいる or 
                                        // 四肢の部位(BUI_KBN=3)を含んでおり、当該項目が1.5倍又は2.0倍の対象(SISI_KBN=1)か、1.5倍のみ対象(SISI_KBN=2) or
                                        // 指の部位(BUI_KBN=10)を含んでおり、当該項目が1.5倍又は2.0倍の対象(SISI_KBN=1)か、2.0倍のみ対象(SISI_KBN=3)
                                        // の場合
                                        kasanKbn = 1;
                                    }

                                    if (kasanKbn > 0)
                                    {
                                        // 加算区分を更新
                                        uniqueRoOdrls[j] =
                                            (new RousaiGoseiItemInfModel(uniqueRoOdrls[j].rpNo, uniqueRoOdrls[j].edaNo, uniqueRoOdrls[j].itemCd, uniqueRoOdrls[j].sisiKbn, kasanKbn));
                                        break;
                                    }

                                }
                            }

                            // 取得したオーダー情報と一致するマスタは存在するかチェック

                            // 労災合成コードマスタの、合成診療行為コード, カウントのリスト生成
                            var rousaiGoseiMstCounts =
                                RousaiGoseiMsts
                                    .Where(p =>
                                        p.GoseiGrp == i)
                                    .GroupBy(p =>
                                        new { gouseiItemCd = p.GoseiItemCd })
                                    .Select(p => new { p.Key, count = p.Count() });

                            List<(string gouseiItemCd, int count)> rousaiGoseiMstCountls =
                                new List<(string, int)>();

                            // 件数が一致するものに絞り込む
                            // この合成診療行為コードのリストが候補となる
                            for (int uq = uniqueRoOdrls.Count; uq > 1; uq--)
                            {
                                foreach (var rousaiGoseiMstCount in rousaiGoseiMstCounts)
                                {
                                    rousaiGoseiMstCountls.Add((rousaiGoseiMstCount.Key.gouseiItemCd, rousaiGoseiMstCount.count));
                                }

                                rousaiGoseiMstCountls =
                                    rousaiGoseiMstCountls.FindAll(p => p.count == uq);


                                // 診療行為コードの組み合わせをチェック
                                // 診療行為コードと四肢加算区分が一致するマスタのレコードを取得
                                List<string> gouseiItemCds = GetGouseiItemCds(uniqueRoOdrls, uq);

                                int k = 0;
                                while (k < rousaiGoseiMstCountls.Count)
                                {
                                    // 取得したレコードの中にないものは候補から外す
                                    if (gouseiItemCds.Any(p =>
                                            p == rousaiGoseiMstCountls[k].gouseiItemCd) == false)
                                    {
                                        rousaiGoseiMstCountls.RemoveAt(k);
                                    }
                                    else
                                    {
                                        k++;
                                    }
                                }


                                if (rousaiGoseiMstCountls.Count > 0)
                                {
                                    break;
                                }
                            }

                            int newRpEdaNo = 1;
                            int newRowNo = 1;

                            if (rousaiGoseiMstCountls.Any())
                            {
                                // マスタが存在する場合、該当するRpを１つのRpにまとめる
                                List<RousaiGoseiMstModel> tgtRousaiGoseiMsts =
                                    RousaiGoseiMsts.FindAll(p => p.GoseiItemCd == rousaiGoseiMstCountls.First().gouseiItemCd).ToList();

                                int uidx = 0;
                                while (uidx < uniqueRoOdrls.Count)
                                {
                                    if (tgtRousaiGoseiMsts.Any(p => p.ItemCd == uniqueRoOdrls[uidx].itemCd) == false)
                                    {
                                        uniqueRoOdrls.RemoveAt(uidx);
                                    }
                                    else
                                    {
                                        uidx++;
                                    }
                                }

                                List<RousaiGoseiItemInfModel> convertRoOdrls = new List<RousaiGoseiItemInfModel>();

                                foreach (RousaiGoseiMstModel tgtRousaiGoseiMst in tgtRousaiGoseiMsts)
                                {
                                    foreach (RousaiGoseiItemInfModel uniqueRoOdr in uniqueRoOdrls)
                                    {
                                        if (tgtRousaiGoseiMst.ItemCd == uniqueRoOdr.itemCd &&
                                           tgtRousaiGoseiMst.SisiKbn == uniqueRoOdr.sisiKasan)
                                        {
                                            convertRoOdrls.Add(uniqueRoOdr);
                                            break;
                                        }
                                    }
                                }

                                // ODR_INF

                                // 一時変数にフィルタの結果を受ける
                                OdrInfModel tmpOdrInf =
                                    _odrInfModels.FindAll(p =>
                                        p.RpNo == convertRoOdrls[0].rpNo &&
                                        p.RpEdaNo == convertRoOdrls[0].edaNo)
                                    .ToList()
                                    .First();

                                // ODR_INF追加用（新たに生成、こうしないと元データを変更してしまう為）
                                OdrInfModel addOdrInf =
                                    new OdrInfModel(tmpOdrInf.OdrInf, tmpOdrInf.PtHokenPattern, tmpOdrInf.RaiinInf);

                                // 新しく追加するオーダー情報のRpNoはマイナス値
                                newRpNo--;

                                // RpNo, RpEdaNoをセット
                                addOdrInf.RpNo = newRpNo;
                                addOdrInf.RpEdaNo = newRpEdaNo;

                                // ODR_INF追加
                                _odrInfModels.Add(addOdrInf);

                                // ODR_INF_DETAIL
                                long preRpNo = 0;
                                long preRpEdaNo = 0;
                                bool replaceItemCd = false;

                                // 該当項目を含むRpの数、ループ
                                for (int j = 0; j < convertRoOdrls.Count; j++)
                                {
                                    // ODR_INF_DETAIL 追加用
                                    List<OdrDtlTenModel> addOdrDtls = new List<OdrDtlTenModel>();

                                    // まとめる対象のRpの詳細を取得する
                                    // 一時変数にフィルタの結果を受ける
                                    var tmpOdrDtls =
                                        _odrDtlTenModels.FindAll(p => p.RpNo == convertRoOdrls[j].rpNo && p.RpEdaNo == convertRoOdrls[j].edaNo && p.SanteiKbn != SanteiKbnConst.SanteiGai);

                                    // 追加用変数にAddする（新たに生成、こうしないと元データを変更してしまう為）
                                    tmpOdrDtls?.ForEach(entity =>
                                    {
                                        //addOdrDtls.Add(new OdrDtlTenModel(entity.OdrInfDetail, entity.TenMst, entity.PtHokenPattern, entity.OdrInf, entity.IpnKasanMst, entity.RaiinInf));
                                        addOdrDtls.Add(
                                            new OdrDtlTenModel(
                                                entity.OdrInfDetail,
                                                entity.TenMst,
                                                entity.CmtKbnMst,
                                                entity.ReceName,
                                                //entity.PtHokenPattern, 
                                                entity.HokenSyu,
                                                entity.HokenPid,
                                                entity.HokenId,
                                                entity.HokenSbt,
                                                //entity.OdrInf, 
                                                entity.OdrKouiKbn,
                                                entity.SanteiKbn,
                                                entity.InoutKbn,
                                                entity.SyohoSbt,
                                                entity.DaysCnt,
                                                entity.SortNo,
                                                //entity.IpnKasanMst, 
                                                entity.Kasan1,
                                                entity.Kasan2,
                                                //entity.RaiinInf
                                                entity.SinStartTime,
                                                entity.MinYakka
                                                //entity.Kbn,
                                                //entity.JunSenpatu
                                                ));
                                    });


                                    int k = 0;
                                    bool del = false;

                                    // まとめる対象のRpを加工する
                                    while (k < addOdrDtls.Count)
                                    {
                                        del = false;

                                        addOdrDtls[k].RpNo = newRpNo;
                                        addOdrDtls[k].RpEdaNo = newRpEdaNo;
                                        addOdrDtls[k].RowNo = newRowNo;
                                        newRowNo++;

                                        // 合成する診療行為であるかどうかをチェックする
                                        bool hit = false;

                                        for (int l = 0; l < convertRoOdrls.Count; l++)
                                        {
                                            if (addOdrDtls[k].ItemCd == convertRoOdrls[l].itemCd)
                                            {
                                                hit = true;
                                                break;
                                            }
                                        }

                                        if (hit)
                                        {
                                            // 合成する診療行為の場合

                                            if (replaceItemCd == false)
                                            {
                                                // まだ置き換えしてない場合
                                                // 最初に該当した項目は合成診療行為コードに置き換える

                                                replaceItemCd = true;

                                                UpdateOdrDtlItemCd(addOdrDtls[k], rousaiGoseiMstCountls.First().gouseiItemCd);

                                            }
                                            else
                                            {
                                                // すでに1つでも置き換えている場合、削除する
                                                del = true;
                                            }
                                        }
                                        else if (rousaiSisi1_5.Contains(addOdrDtls[k].ItemCd) ||
                                                rousaiSisi2.Contains(addOdrDtls[k].ItemCd))
                                        {
                                            // 労災四肢加算は削除
                                            del = true;
                                        }

                                        if (del)
                                        {
                                            addOdrDtls.RemoveAt(k);
                                        }
                                        else
                                        {
                                            k++;
                                        }
                                    }

                                    // 前に処理したRpと違うRpの場合、追加する
                                    if (convertRoOdrls[j].rpNo != preRpNo || convertRoOdrls[j].edaNo != preRpEdaNo)
                                    {
                                        _odrDtlTenModels.AddRange(addOdrDtls);

                                        preRpNo = convertRoOdrls[j].rpNo;
                                        preRpEdaNo = convertRoOdrls[j].edaNo;
                                    }

                                    // オーダーから削除
                                    _odrInfModels.RemoveAll(p => p.RaiinNo == _raiinNo && p.RpNo == convertRoOdrls[j].rpNo && p.RpEdaNo == convertRoOdrls[j].edaNo);
                                    _odrDtlTenModels.RemoveAll(p => p.RaiinNo == _raiinNo && p.RpNo == convertRoOdrls[j].rpNo && p.RpEdaNo == convertRoOdrls[j].edaNo);

                                }

                            }
                        }
                    }
                }
            }
            //}
        }

        private List<string> GetGouseiItemCds(List<RousaiGoseiItemInfModel> uniqueRoOdrls, int uq)
        {
            List<string> ret = new List<string>();

            if (uniqueRoOdrls.Count == 3)
            {
                var gouseiItemCds =
                    RousaiGoseiMsts
                        .Where(p =>
                            (p.ItemCd == uniqueRoOdrls[0].itemCd && p.SisiKbn == uniqueRoOdrls[0].sisiKasan) ||
                            (p.ItemCd == uniqueRoOdrls[1].itemCd && p.SisiKbn == uniqueRoOdrls[1].sisiKasan) ||
                            (p.ItemCd == uniqueRoOdrls[2].itemCd && p.SisiKbn == uniqueRoOdrls[2].sisiKasan)
                            )
                        .GroupBy(p =>
                            new { gouseiItemCd = p.GoseiItemCd })
                        .Select(p => new { gouseiItemCd = p.Key.gouseiItemCd, count = p.Count() });

                gouseiItemCds = gouseiItemCds.Where(p => p.count == uq);

                if (gouseiItemCds != null)
                {
                    foreach (var gouseiItemCd in gouseiItemCds)
                    {
                        ret.Add(gouseiItemCd.gouseiItemCd);
                    }
                }

            }
            else
            {
                var gouseiItemCds =
                    RousaiGoseiMsts
                        .Where(p =>
                            (p.ItemCd == uniqueRoOdrls[0].itemCd && p.SisiKbn == uniqueRoOdrls[0].sisiKasan) ||
                            (p.ItemCd == uniqueRoOdrls[1].itemCd && p.SisiKbn == uniqueRoOdrls[1].sisiKasan)
                            )
                        .GroupBy(p =>
                            new { gouseiItemCd = p.GoseiItemCd })
                        .Select(p => new { gouseiItemCd = p.Key.gouseiItemCd, count = p.Count() });

                gouseiItemCds = gouseiItemCds.Where(p => p.count == uq);

                if (gouseiItemCds != null)
                {
                    foreach (var gouseiItemCd in gouseiItemCds)
                    {
                        ret.Add(gouseiItemCd.gouseiItemCd);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// オーダーで使用している保険種のリストを返す
        /// 後で使用するが、計算処理中にOdrDtlは削除されるので、先に取得しておく
        /// </summary>
        /// <returns></returns>
        private List<(long, int)> GetHokenSyuList()
        {
            List<(long, int)> ret = new List<(long, int)>();

            var hokenSyuList = _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.SanteiKbn != SanteiKbnConst.SanteiGai)
            .GroupBy(p => new { raiinNo = p.RaiinNo, hokenSyu = p.HokenSyu });

            foreach (var hokenSyu in hokenSyuList)
            {
                ret.Add((hokenSyu.Key.raiinNo, hokenSyu.Key.hokenSyu));
            }

            return ret;
        }

        /// <summary>
        /// この来院で院外処方オーダーがあるかどうか
        ///     true:ある
        /// </summary>
        public bool ExistIngaiSyoho
        {
            get
            {
                if (_existIngaiSyoho < 0)
                {
                    _existIngaiSyoho = ExistSyoho(1, true, 0);
                }

                return _existIngaiSyoho == 1;
            }
        }

        /// <summary>
        /// この診療日で院外処方オーダーがあるかどうか
        ///     true:ある
        /// </summary>
        public bool ExistIngaiSyohoInDay
        {
            get
            {
                if (_existIngaiSyoho < 0)
                {
                    _existIngaiSyoho = ExistSyoho(1, true, 1);
                }

                return _existIngaiSyoho == 1;
            }
        }

        /// <summary>
        /// 当月に院外処方オーダーがあるかどうか
        ///     true:ある
        /// </summary>
        public bool ExistIngaiSyohoInMonth
        {
            get
            {
                if (_existsIngaiSyohoInMonth < 0)
                {
                    _existsIngaiSyohoInMonth = ExistSyoho(1, true, 2);
                }
                return _existsIngaiSyohoInMonth == 1;
            }
        }
        public bool ExistIngaiSyohoInMonthHokenSyu
        {
            get
            {
                if (_existsIngaiSyohoInMonthHokenSyu < 0)
                {
                    _existsIngaiSyohoInMonthHokenSyu = ExistSyohoHokenSyu(1, true, 2);
                }
                return _existsIngaiSyohoInMonthHokenSyu == 1;
            }
        }
        /// <summary>
        /// この来院で院内処方オーダーがあるかどうか
        ///     true:ある
        /// </summary>
        public bool ExistInnaiSyoho
        {
            get
            {
                if (_existsInnaiSyoho < 0)
                {
                    _existsInnaiSyoho = ExistSyoho(0, true, 0);
                }
                return _existsInnaiSyoho == 1;
            }
        }

        /// <summary>
        /// この診療日で院内処方オーダーがあるかどうか
        ///     true:ある
        /// </summary>
        public bool ExistInnaiSyohoInDay
        {
            get
            {
                if (_existsInnaiSyoho < 0)
                {
                    _existsInnaiSyoho = ExistSyoho(0, true, 1);
                }
                return _existsInnaiSyoho == 1;
            }
        }

        /// <summary>
        /// 当月に院内処方オーダーがあるかどうか
        ///     true:ある
        /// </summary>
        public bool ExistInnaiSyohoInMonth
        {
            get
            {
                if (_existsInnaiSyohoInMonth < 0)
                {
                    _existsInnaiSyohoInMonth = ExistSyoho(0, true, 2);
                }
                return _existsInnaiSyohoInMonth == 1;
            }
        }

        /// <summary>
        /// 指定の種類の処方オーダーがあるかチェック
        /// </summary>
        /// <param name="inout">
        ///     0:院内
        ///     1:院外
        /// </param>
        /// <param name="incTokusyo">
        ///     true:院外のチェック時、特処を含める
        /// </param>
        /// <param name="term">
        ///     0: 来院のみ
        ///     1: 診療日のみ
        ///     2: 同月内オーダーもチェックする
        /// </param>
        /// <returns></returns>
        public int ExistSyoho(int inout, bool incTokusyo, int term)
        {
            int ret = 0;

            List<string> TokusyoSyohosenls =
                new List<string>
                {
                            ItemCdConst.TouyakuTokuSyo1Syohosen,
                            ItemCdConst.TouyakuTokuSyo2Syohosen
                };

            if (_odrInfModels.Any(p =>
                    p.HokenSyu == _hokenKbn &&
                    (term == 0 ? p.RaiinNo == _raiinNo : true) &&
                    //p.SanteiKbn != 1 &&
                    p.OdrKouiKbn >= OdrKouiKbnConst.Naifuku &&
                    p.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo &&
                    (inout >= 0 ? p.InoutKbn == inout : true)))
            {
                // 当来院に存在する
                ret = 1;
            }
            else if (inout == 1 &&
                    (
                        incTokusyo &&
                        _odrDtlTenModels.Any(p =>
                        p.HokenSyu == _hokenKbn &&
                        (term == 0 ? p.RaiinNo == _raiinNo : true) &&
                        (inout >= 0 ? p.InoutKbn == inout : true) &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        TokusyoSyohosenls.Contains(p.ItemCd))))
            {
                // 院外処方で特処が当来院に存在する
                ret = 1;
            }
            else if (term == 2 && _odrInfFinder.InOutSyohoExists(_hpId, _ptId, _sinDate, inout, _hokenKbn))
            {
                // 当月内に存在する
                ret = 1;
            }
            else
            {
                ret = 0;
            }

            return ret;
        }
        public int ExistSyohoHokenSyu(int inout, bool incTokusyo, int term)
        {
            int ret = 0;

            List<string> TokusyoSyohosenls =
                new List<string>
                {
                            ItemCdConst.TouyakuTokuSyo1Syohosen,
                            ItemCdConst.TouyakuTokuSyo2Syohosen
                };

            if (_odrInfModels.Any(p =>
                    p.HokenSyu == _hokenKbn &&
                    (term == 0 ? p.RaiinNo == _raiinNo : true) &&
                    //p.SanteiKbn != 1 &&
                    p.OdrKouiKbn >= OdrKouiKbnConst.Naifuku &&
                    p.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo &&
                    (inout >= 0 ? p.InoutKbn == inout : true)))
            {
                // 当来院に存在する
                ret = 1;
            }
            else if (inout == 1 &&
                    (
                        incTokusyo &&
                        _odrDtlTenModels.Any(p =>
                        checkHokenKbn.Contains(p.HokenSyu) &&
                        (term == 0 ? p.RaiinNo == _raiinNo : true) &&
                        (inout >= 0 ? p.InoutKbn == inout : true) &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        TokusyoSyohosenls.Contains(p.ItemCd))))
            {
                // 院外処方で特処が当来院に存在する
                ret = 1;
            }
            else if (term == 2 && _odrInfFinder.InOutSyohoExistsHokenSyu(_hpId, _ptId, _sinDate, inout, checkHokenKbn))
            {
                // 当月内に存在する
                ret = 1;
            }
            else
            {
                ret = 0;
            }

            return ret;
        }
        /// <summary>
        /// この来院で採血料を算定する検査オーダーがあるかどうか
        /// </summary>
        /// <returns>最初に該当する検査項目の保険組み合わせIDと算定区分</returns>
        public (int, int, int) ExistSaiketuKensa()
        {
            int hokenPid = -1;
            int hokenId = -1;
            int santeiKbn = -1;
            int index = _odrDtlTenModels.FindIndex(p =>
                            p.HokenSyu == _hokenKbn &&
                            p.RaiinNo == _raiinNo &&
                            p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                            p.SaiketuKbn > 0);
            if (index > 0)
            {
                hokenPid = _odrDtlTenModels[index].HokenPid;
                hokenId = _odrDtlTenModels[index].HokenId;
                santeiKbn = _odrDtlTenModels[index].SanteiKbn;
            }
            return (hokenPid, hokenId, santeiKbn);
        }

        /// <summary>
        /// この来院で採血料を算定する検査項目を返す
        /// </summary>
        /// <returns></returns>
        public List<OdrDtlTenModel> FindSaiketuKensa()
        {
            return _odrDtlTenModels.FindAll(p =>
                            p.HokenSyu == _hokenKbn &&
                            p.RaiinNo == _raiinNo &&
                            p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                            p.SaiketuKbn > 0);
        }

        /// <summary>
        /// この来院で、指定の判断グループの検査オーダーがあるかどうか
        /// </summary>
        /// <param name="handanGrpKbn"></param>
        /// <returns>true: ある</returns>
        public bool ExistKensaHandanGrpKbn(int handanGrpKbn)
        {
            return (_odrDtlTenModels.Any(p =>
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.HandanGrpKbn == handanGrpKbn));
        }

        public bool ExistKensaHandanGrpKbn(int[] handanGrpKbn)
        {
            return (_odrDtlTenModels.Any(p =>
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        handanGrpKbn.Contains(p.HandanGrpKbn)));
        }

        /// <summary>
        /// 指定の行為の範囲で自費項目が存在するかチェック
        /// </summary>
        /// <param name="kouiKbnMin"></param>
        /// <param name="kouiKbnMax"></param>
        /// <returns></returns>
        public bool ExistJihiByKouiCd(int kouiKbnMin, int kouiKbnMax)
        {
            var odrInfs = _odrInfModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.OdrKouiKbn >= kouiKbnMin &&
                        p.OdrKouiKbn <= kouiKbnMax);
            var odrDtls = _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.TenMst != null &&
                        (p.TenMst.JihiSbt > 0 &&
                            (string.IsNullOrEmpty(p.ItemCd) == true ||
                                (p.ItemCd.StartsWith("FCR") == false && p.ItemCd.StartsWith("KONI") == false))));

            var _join = (
                from odrInf in odrInfs
                join odrDtl in odrDtls on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RpNo } equals
                    new { odrDtl.HpId, odrDtl.PtId, odrDtl.RpNo }
                select new
                {
                    odrInf
                }
               );

            return _join.Any();
        }

        /// <summary>
        /// 指定のRpに自費項目が存在するかチェック
        /// </summary>
        /// <param name="RpNo"></param>
        /// <param name="RpEdaNo"></param>
        /// <returns></returns>
        public bool ExistJihiByRp(long RpNo, long RpEdaNo)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.RpNo == RpNo &&
                        p.RpEdaNo == RpEdaNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.TenMst != null &&
                        p.TenMst.JihiSbt > 0);
        }

        /// <summary>
        /// 指定の検査判断グループのオーダーを取得する
        /// </summary>
        /// <param name="handanGrpKbn"></param>
        /// <returns>指定の検査判断グループのオーダー</returns>
        public List<OdrDtlTenModel> FilterKensaHandanGrpKbn(int[] handanGrpKbn)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        handanGrpKbn.Contains(p.HandanGrpKbn));
        }

        /// <summary>
        /// この来院で麻薬処方オーダーがあるかどうか
        /// </summary>
        /// <param name="inOut">
        ///     0:院内
        ///     1:院外
        /// </param>
        /// <returns>true:ある</returns>
        public bool ExistMadokuSyoho(int inOut)
        {
            bool ret = false;

            if (_odrDtlTenModels.Any(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.HokenSyu == _hokenKbn &&
                    p.RaiinNo == _raiinNo &&
                    p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                    p.OdrKouiKbn >= OdrKouiKbnConst.Naifuku &&
                    p.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo &&
                    p.InoutKbn == inOut &&
                    new int[] { 1, 2, 3, 5 }.Contains(p.MadokuKbn)))
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 指定のRpに向精神薬のオーダーがあるかチェック
        /// </summary>
        /// <param name="rpNo">Rp番号</param>
        /// <param name="rpEdaNo">Rp枝番</param>
        /// <returns>true:ある</returns>
        public bool ExistKouseiDrug(long rpNo, long rpEdaNo)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.RpNo == rpNo &&
                        p.RpEdaNo == rpEdaNo &&
                        p.KouseisinKbn > 0);
        }

        ///// <summary>
        ///// 指定の保険種のオーダーが存在しているかどうかチェック
        ///// </summary>
        ///// <returns>true: 存在している</returns>
        //public bool ExistOdrHokenSyu()
        //{
        //    return _odrInfModels.Any(p =>
        //                p.HpId == _hpId &&
        //                p.PtId == _ptId &&
        //                p.HokenSyu == _hokenKbn && 
        //                p.RaiinNo == _raiinNo);
        //}

        /// <summary>
        /// 指定の保険種のオーダーが存在しているかどうか
        /// </summary>
        /// <returns></returns>
        public bool ExistOdrDtlHokenSyu()
        {
            return _hokenSyuList.Any(p =>
                        p.raiinNo == _raiinNo &&
                        p.hokenSyu == _hokenKbn);

            //return _odrDtlTenModels.Any(p =>
            //            p.HpId == _hpId &&
            //            p.PtId == _ptId &&
            //            p.HokenSyu == _hokenKbn &&
            //            p.RaiinNo == _raiinNo &&
            //            p.SanteiKbn != SanteiKbnConst.SanteiGai);
        }

        /// <summary>
        /// 指定の行為のオーダーが存在しているかどうかチェック
        /// </summary>
        /// <returns>true: 存在している</returns>
        public bool ExistOdrKoui(int kouiMin, int kouiMax)
        {
            return _odrInfModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.OdrKouiKbn >= kouiMin &&
                        p.OdrKouiKbn <= kouiMax);
        }

        /// <summary>
        /// オーダーを指定の行為区分でフィルタ
        /// </summary>
        /// <param name="kouiKbn">フィルタ条件に指定するODR_KOUI_KBN</param>
        /// </param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrInfModel> FilterOdrInfByKouiKbn(int kouiKbn)
        {
            return _odrInfModels.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.HokenSyu == _hokenKbn &&
                    p.RaiinNo == _raiinNo &&
                    //p.SanteiKbn != 1 &&
                    p.OdrKouiKbn == kouiKbn);
        }

        /// <summary>
        /// 当来院のオーダーを指定の行為区分範囲でフィルタ
        /// </summary>
        /// <param name="kouiKbnMin">フィルタ条件に指定するODR_KOUI_KBNの最小値</param>
        /// <param name="kouiKbnMax">フィルタ条件に指定するODR_KOUI_KBNの最大値</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrInfModel> FilterOdrInfByKouiKbnRange(int kouiKbnMin, int kouiKbnMax)
        {
            return _odrInfModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        //p.SanteiKbn != 1 && 
                        p.OdrKouiKbn >= kouiKbnMin &&
                        p.OdrKouiKbn <= kouiKbnMax);
        }

        /// <summary>
        /// 当日オーダーを指定の行為区分範囲でフィルタ
        /// </summary>
        /// <param name="kouiKbnMin">フィルタ条件に指定するODR_KOUI_KBNの最小値</param>
        /// <param name="kouiKbnMax">フィルタ条件に指定するODR_KOUI_KBNの最大値</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrInfModel> FilterOdrInfByKouiKbnRangeToday(int kouiKbnMin, int kouiKbnMax)
        {
            return _odrInfModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        //p.SanteiKbn != 1 && 
                        p.OdrKouiKbn >= kouiKbnMin &&
                        p.OdrKouiKbn <= kouiKbnMax);
        }

        //public List<OdrInfModel> FilterOdrInfTentekiToday(int kouiKbnMin, int kouiKbnMax)
        //{
        //    return _odrInfModels.FindAll(p =>
        //                p.HpId == _hpId &&
        //                p.PtId == _ptId &&
        //                p.HokenSyu == _hokenKbn &&
        //                //p.SanteiKbn != 1 && 
        //                p.OdrKouiKbn >= kouiKbnMin &&
        //                p.OdrKouiKbn <= kouiKbnMax);
        //}

        public List<OdrInfModel> FilterOdrInfTentekiToday(int santeiKbn)
        {
            List<(long rpNo, long rpEdaNo)> rpList = new List<(long, long)>();

            var entities = _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        //p.SanteiKbn != 1 && 
                        p.SanteiKbn == santeiKbn &&
                        (p.OdrKouiKbn == OdrKouiKbnConst.Tenteki ||
                        p.ItemCd == ItemCdConst.ChusyaSyuginasi33))
                .GroupBy(p => new { rpNo = p.RpNo, rpEdaNo = p.RpEdaNo })
                .ToList();

            List<OdrInfModel> results = new List<OdrInfModel>();

            entities?.ForEach(data =>
                    { results.AddRange(_odrInfModels.FindAll(p => p.RpNo == data.Key.rpNo && p.RpEdaNo == data.Key.rpEdaNo)); }
                );

            return results;
        }

        /// <summary>
        /// オーダー詳細を指定のITEM_CDでフィルタ（診療日単位）
        /// </summary>
        /// <param name="itemCds"></param>
        /// <returns></returns>
        public List<OdrDtlTenModel> FilterOdrDetailByItemCdToday(List<string> itemCds)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        //p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.SinDate == _sinDate &&
                        itemCds.Contains(p.ItemCd));
        }
        public List<OdrDtlTenModel> FilterOdrDetailByItemCdToday(string itemCd)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        //p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.SinDate == _sinDate &&
                        p.ItemCd == itemCd);
        }

        /// <summary>
        /// 指定Rpのオーダー詳細を指定のITEM_CDでフィルタ
        /// </summary>
        /// <param name="itemCd">フィルタ条件に指定するITEM_CD</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailRpByItemCd(long rpNo, long rpEdaNo, string itemCd)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        //p.SanteiKbn != 1 &&
                        p.RpNo == rpNo &&
                        p.RpEdaNo == rpEdaNo &&
                        p.ItemCd == itemCd);
        }

        /// <summary>
        /// 指定Rpのオーダー詳細を指定のITEM_CDでフィルタ
        /// </summary>
        /// <param name="rpNo">Rp番号</param>
        /// <param name="rpEdaNo">Rp枝番号</param>
        /// <param name="itemCds">フィルタ条件に指定するITEM_CDのリスト</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailRpByItemCd(long rpNo, long rpEdaNo, List<string> itemCds)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.RpNo == rpNo &&
                        p.RpEdaNo == rpEdaNo &&
                        itemCds.Contains(p.ItemCd));
        }

        /// <summary>
        /// オーダー詳細を指定のITEM_CDでフィルタ
        /// </summary>
        /// <param name="itemCds">フィルタ条件に指定するITEM_CDのリスト</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailByItemCd(List<string> itemCds)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        itemCds.Contains(p.ItemCd));
        }

        /// <summary>
        /// オーダー詳細を指定のITEM_CDでフィルタ
        /// </summary>
        /// <param name="itemCd">フィルタ条件に指定するITEM_CD</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailByItemCd(string itemCd)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.ItemCd == itemCd);
        }

        /// <summary>
        /// オーダー詳細を指定のITEM_CDでフィルタ
        /// </summary>
        /// <param name="itemCd">フィルタ条件に指定するITEM_CD</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailComment(int minKoui, int maxKoui)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.OdrKouiKbn >= minKoui &&
                        p.OdrKouiKbn <= maxKoui &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.ItemCd.StartsWith("8") &&
                        p.MasterSbt == "C" &&
                        p.ItemCd.Length == 9);
        }

        /// <summary>
        /// オーダー詳細に指定の項目が存在するかチェック
        /// </summary>
        /// <param name="itemCds">フィルタ条件に指定するITEM_CD</param>
        /// <returns>true-存在する</returns>
        public bool ExistOdrDetailByItemCd(List<string> itemCds)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        itemCds.Contains(p.ItemCd));
        }
        /// <summary>
        /// オーダー詳細に指定の項目が存在するかチェック
        /// </summary>
        /// <param name="itemCd">フィルタ条件に指定するITEM_CD</param>
        /// <returns>true-存在する</returns>
        public bool ExistOdrDetailByItemCd(string itemCd)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.ItemCd == itemCd);
        }
        public bool ExistOdrDetailByItemCdRp(long rpNo, long rpEdaNo, string itemCd)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.RpNo == rpNo &&
                        p.RpEdaNo == rpEdaNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.ItemCd == itemCd);
        }
        public bool ExistOdrDetailByHokatuKbn(long rpNo, long rpEdaNo, int hokatuKbn)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.RpNo == rpNo &&
                        p.RpEdaNo == rpEdaNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.HokatuKbn == hokatuKbn);
        }
        /// <summary>
        /// オーダー詳細に指定の項目が存在するかチェック
        /// </summary>
        /// <param name="itemCds">フィルタ条件に指定するITEM_CD</param>
        /// <returns>true-存在する</returns>
        public bool ExistOdrDetailByItemCdToday(List<string> itemCds)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.SinDate == _sinDate &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        itemCds.Contains(p.ItemCd));
        }
        /// <summary>
        /// オーダー詳細に指定の注コード、注コード枝番の項目が存在するかチェック
        /// </summary>
        /// <returns></returns>
        public bool ExistOdrDetailByTyuCd(string tyucd, string tyuseq)
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.TyuCd == tyucd &&
                        p.TyuSeq == tyuseq);
        }
        /// <summary>
        /// オーダー詳細に訪問診療の項目が存在するかチェック
        /// </summary>
        /// <param name="itemCds">フィルタ条件に指定するITEM_CD</param>
        /// <returns>true-存在する</returns>
        public bool ExistOdrDetailHoumonSinryo()
        {
            return _odrDtlTenModels.Any(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.CdKbn == "C" &&
                        p.CdKbnno == 1 &&
                        p.Kokuji1 == "1" &&
                        p.Kokuji2 == "1");
        }
        /// <summary>
        /// 指定の項目コードを持つオーダー詳細を取得する
        /// 前後の関連するコメントレコードも含めて返す
        /// </summary>
        /// <param name="itemCd">フィルタ条件に指定するITEM_CD</param>
        /// <returns>
        ///     オーダー詳細
        ///     最小Index
        ///     個数
        /// </returns>
        public (List<OdrDtlTenModel>, int, int) FilterOdrDetailRangeByItemCd(string itemCd)
        {
            List<string> itemCds = new List<string>();

            itemCds.Add(itemCd);

            return FilterOdrDetailRangeByItemCd(itemCds);
        }

        /// <summary>
        /// 指定の項目コードを持つオーダー詳細を取得する
        /// 前後の関連するコメントレコードも含めて返す
        /// </summary>
        /// <param name="itemCd">フィルタ条件に指定するITEM_CD</param>
        /// <returns>
        ///     オーダー詳細
        ///     最小Index
        ///     個数
        /// </returns>
        public (List<OdrDtlTenModel>, int, int) FilterOdrDetailRangeByItemCd(List<string> itemCd, int startIndex = 0)
        {
            List<OdrDtlTenModel> result = new List<OdrDtlTenModel>();
            int minIndex = -1;
            int maxIndex = -1;
            long itemRaiinNo, itemRpNo, itemRpEdaNo;

            int itemIndex =
                _odrDtlTenModels.FindIndex(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.HokenSyu == _hokenKbn &&
                    p.RaiinNo == _raiinNo &&
                    p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                    itemCd.Contains(p.ItemCd));

            if (startIndex > 0)
            {
                if (itemIndex < startIndex)
                {
                    itemIndex = -1;
                    for (int i = startIndex; i < _odrDtlTenModels.Count; i++)
                    {
                        if (itemCd.Contains(_odrDtlTenModels[i].ItemCd))
                        {
                            itemIndex = i;
                            break;
                        }
                    }
                }
            }

            if (itemIndex >= 0)
            {
                itemRaiinNo = _odrDtlTenModels[itemIndex].RaiinNo;
                itemRpNo = _odrDtlTenModels[itemIndex].RpNo;
                itemRpEdaNo = _odrDtlTenModels[itemIndex].RpEdaNo;

                // 上方向に検索
                bool findNotComment = false;
                minIndex = itemIndex;

                while (minIndex > 0)
                {
                    //if (itemRaiinNo != _odrDtlTenModels[minIndex - 1].RaiinNo ||
                    //    itemRpNo != _odrDtlTenModels[minIndex - 1].RpNo ||
                    //    itemRpEdaNo != _odrDtlTenModels[minIndex - 1].RpEdaNo)
                    if (itemRpEdaNo != _odrDtlTenModels[minIndex - 1].RpEdaNo ||
                        itemRpNo != _odrDtlTenModels[minIndex - 1].RpNo ||
                        itemRaiinNo != _odrDtlTenModels[minIndex - 1].RaiinNo
                        )
                    {
                        break;
                    }
                    else if (IsCommentItemCd(_odrDtlTenModels[minIndex - 1].ItemCd) == false)
                    {
                        findNotComment = true;
                        break;
                    }
                    else
                    {
                        minIndex--;
                    }
                }

                //if (minIndex != 0)
                if (findNotComment)
                {
                    // 上方向に別のコメント以外の項目がある場合は、
                    // 下に続くコメントのみ付随すると考える
                    minIndex = itemIndex;
                }

                // 下方向に検索
                maxIndex = itemIndex;

                while (maxIndex < _odrDtlTenModels.Count - 1)
                {
                    if (IsCommentItemCd(_odrDtlTenModels[maxIndex + 1].ItemCd) &&
                        itemRpEdaNo == _odrDtlTenModels[maxIndex + 1].RpEdaNo &&
                        itemRpNo == _odrDtlTenModels[maxIndex + 1].RpNo &&
                        itemRaiinNo == _odrDtlTenModels[maxIndex + 1].RaiinNo
                        )
                    {
                        maxIndex++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = minIndex; i <= maxIndex; i++)
                {
                    result.Add(_odrDtlTenModels[i]);
                }
            }

            return (result, minIndex, maxIndex - minIndex + 1);
        }
        /// <summary>
        /// 当来院のオーダー詳細を取得する
        /// </summary>
        /// <returns></returns>
        public List<OdrDtlTenModel> FilterOdrDetail()
        {
            return _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.HokenSyu == _hokenKbn &&
                p.RaiinNo == _raiinNo &&
                p.SanteiKbn != SanteiKbnConst.SanteiGai);
        }
        /// <summary>
        /// 指定の包括検査に該当するオーダー詳細を取得する
        /// </summary>
        /// <param name="hokatuKensa">包括グループコード</param>
        /// <returns>フィルター後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailByHokatuKensa(int hokatuKensa)
        {
            return _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.HokenSyu == _hokenKbn &&
                p.RaiinNo == _raiinNo &&
                p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                p.HokatuKensa == hokatuKensa);
        }

        /// <summary>
        /// 指定の項目コードを持つオーダー詳細を取得する
        /// 前後の関連するコメントレコードも含めて返す
        /// </summary>
        /// <param name="hokatuKensa">包括グループコード</param>
        /// <param name="pid">保険組み合わせID</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>
        ///     フィルタ後のリスト
        ///     最小Index
        ///     個数
        /// </returns>
        public (List<OdrDtlTenModel>, int, int) FilterOdrDetailRangeByHokatuKensa(int hokatuKensa, int pid, int santeiKbn)
        {
            List<OdrDtlTenModel> result = new List<OdrDtlTenModel>();
            int minIndex = -1;
            int maxIndex = -1;
            long itemRaiinNo, itemRpNo, itemRpEdaNo;

            //int itemIndex = _odrDtlTenModels.FindIndex(p =>
            //    p.HpId == _hpId &&
            //    p.PtId == _ptId &&
            //    p.HokenSyu == _hokenKbn && 
            //    p.RaiinNo == _raiinNo && 
            //    p.HokenPid == pid && 
            //    p.SanteiKbn == santeiKbn && 
            //    p.HokatuKensa == hokatuKensa);
            int itemIndex = _odrDtlTenModels.FindIndex(p =>
                p.HokatuKensa == hokatuKensa &&
                p.RaiinNo == _raiinNo &&
                p.HokenSyu == _hokenKbn &&
                p.HokenPid == pid &&
                p.SanteiKbn == santeiKbn &&
                p.PtId == _ptId &&
                p.HpId == _hpId
                );

            if (itemIndex >= 0)
            {
                itemRaiinNo = _odrDtlTenModels[itemIndex].RaiinNo;
                itemRpNo = _odrDtlTenModels[itemIndex].RpNo;
                itemRpEdaNo = _odrDtlTenModels[itemIndex].RpEdaNo;

                // 上方向に検索
                minIndex = itemIndex;
                bool findNotComment = false;

                while (minIndex > 0)
                {
                    if (itemRpEdaNo != _odrDtlTenModels[minIndex - 1].RpEdaNo ||
                        itemRpNo != _odrDtlTenModels[minIndex - 1].RpNo ||
                        itemRaiinNo != _odrDtlTenModels[minIndex - 1].RaiinNo
                        )
                    {
                        break;
                    }
                    else if (IsCommentItemCd(_odrDtlTenModels[minIndex - 1].ItemCd) == false)
                    {
                        findNotComment = true;
                        break;
                    }
                    else
                    {
                        minIndex--;
                    }
                }

                //if (minIndex != 0)
                if (findNotComment)
                {
                    // 上方向に別のコメント以外の項目がある場合は、
                    // 下に続くコメントのみ付随すると考える
                    minIndex = itemIndex;
                }

                // 下方向に検索
                maxIndex = itemIndex;

                while (maxIndex < _odrDtlTenModels.Count - 1)
                {
                    if (IsCommentItemCd(_odrDtlTenModels[maxIndex + 1].ItemCd) &&
                        itemRpEdaNo == _odrDtlTenModels[maxIndex + 1].RpEdaNo &&
                        itemRpNo == _odrDtlTenModels[maxIndex + 1].RpNo &&
                        itemRaiinNo == _odrDtlTenModels[maxIndex + 1].RaiinNo
                        )
                    {
                        maxIndex++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = minIndex; i <= maxIndex; i++)
                {
                    result.Add(_odrDtlTenModels[i]);
                }
            }

            return (result, minIndex, maxIndex - minIndex + 1);
        }

        /// <summary>
        /// 包括検査項目の数を数える
        /// </summary>
        /// <param name="hokatuKensa"></param>
        /// <param name="santeiKbn"></param>
        /// <returns></returns>
        public int CountHokatuKensa(int hokatuKensa, int santeiKbn)
        {
            if (_odrDtlTenModels == null || _odrDtlTenModels.Any() == false)
            {
                return 0;
            }

            return _odrDtlTenModels.Count(p =>
                p.HokatuKensa == hokatuKensa &&
                p.RaiinNo == _raiinNo &&
                p.HokenSyu == _hokenKbn &&
                p.SanteiKbn == santeiKbn &&
                p.PtId == _ptId &&
                p.HpId == _hpId
                );
        }
        public List<int> FindHoukatuKensa(int pid, int santeiKbn)
        {
            return _odrDtlTenModels.Where(p =>
                p.HokatuKensa > 0 &&
                p.RaiinNo == _raiinNo &&
                p.HokenSyu == _hokenKbn &&
                p.HokenPid == pid &&
                p.SanteiKbn == santeiKbn &&
                p.PtId == _ptId &&
                p.HpId == _hpId
                ).Select(p => p.HokatuKensa).Distinct().ToList();
        }

        public (List<OdrDtlTenModel>, int, int) FilterOdrDetailRangeByHoukatuKbn(int houkatuKbn)
        {
            List<int> houkatuKbns = new List<int>();
            houkatuKbns.Add(houkatuKbn);

            return FilterOdrDetailRangeByHoukatuKbn(houkatuKbns);
        }

        /// <summary>
        /// 指定の項目コードを持つオーダー詳細を取得する
        /// 前後の関連するコメントレコードも含めて返す
        /// </summary>
        /// <param name="itemCd">フィルタ条件に指定するITEM_CD</param>
        /// <returns>
        ///     オーダー詳細
        ///     最小Index
        ///     個数
        /// </returns>
        public (List<OdrDtlTenModel>, int, int) FilterOdrDetailRangeByHoukatuKbn(List<int> houkatuKbn)
        {
            List<OdrDtlTenModel> result = new List<OdrDtlTenModel>();
            int minIndex = -1;
            int maxIndex = -1;
            long itemRaiinNo, itemRpNo, itemRpEdaNo;

            int itemIndex =
                _odrDtlTenModels.FindIndex(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.HokenSyu == _hokenKbn &&
                    p.RaiinNo == _raiinNo &&
                    p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                    houkatuKbn.Contains(p.HokatuKbn));

            if (itemIndex >= 0)
            {
                itemRaiinNo = _odrDtlTenModels[itemIndex].RaiinNo;
                itemRpNo = _odrDtlTenModels[itemIndex].RpNo;
                itemRpEdaNo = _odrDtlTenModels[itemIndex].RpEdaNo;

                // 上方向に検索
                bool findNotComment = false;
                minIndex = itemIndex;

                while (minIndex > 0)
                {
                    //if (itemRaiinNo != _odrDtlTenModels[minIndex - 1].RaiinNo ||
                    //    itemRpNo != _odrDtlTenModels[minIndex - 1].RpNo ||
                    //    itemRpEdaNo != _odrDtlTenModels[minIndex - 1].RpEdaNo)
                    if (itemRpEdaNo != _odrDtlTenModels[minIndex - 1].RpEdaNo ||
                        itemRpNo != _odrDtlTenModels[minIndex - 1].RpNo ||
                        itemRaiinNo != _odrDtlTenModels[minIndex - 1].RaiinNo
                        )
                    {
                        break;
                    }
                    else if (IsCommentItemCd(_odrDtlTenModels[minIndex - 1].ItemCd) == false)
                    {
                        findNotComment = true;
                        break;
                    }
                    else
                    {
                        minIndex--;
                    }
                }

                //if (minIndex != 0)
                if (findNotComment)
                {
                    // 上方向に別のコメント以外の項目がある場合は、
                    // 下に続くコメントのみ付随すると考える
                    minIndex = itemIndex;
                }

                // 下方向に検索
                maxIndex = itemIndex;

                while (maxIndex < _odrDtlTenModels.Count - 1)
                {
                    if (IsCommentItemCd(_odrDtlTenModels[maxIndex + 1].ItemCd) &&
                        itemRpEdaNo == _odrDtlTenModels[maxIndex + 1].RpEdaNo &&
                        itemRpNo == _odrDtlTenModels[maxIndex + 1].RpNo &&
                        itemRaiinNo == _odrDtlTenModels[maxIndex + 1].RaiinNo
                        )
                    {
                        maxIndex++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = minIndex; i <= maxIndex; i++)
                {
                    result.Add(_odrDtlTenModels[i]);
                }
            }

            return (result, minIndex, maxIndex - minIndex + 1);
        }
        /// <summary>
        /// オーダーされている向精神薬を取得する
        /// </summary>
        /// <returns>フィルター後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrKouiKouseisin()
        {
            return _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.HokenSyu == _hokenKbn &&
                p.RaiinNo == _raiinNo &&
                p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                p.KouseisinKbn > 0 &&
                p.OdrKouiKbn >= OdrKouiKbnConst.Naifuku &&
                p.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo);

        }

        /// <summary>
        /// オーダーされている内服薬を取得する
        /// </summary>
        /// <param name="santeiKbn"></param>
        /// <returns>フィルター後のリスト</returns>
        public List<OdrInfModel> FilterOdrKouiNaifuku()
        {
            return _odrInfModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.HokenSyu == _hokenKbn &&
                p.RaiinNo == _raiinNo &&
                //p.SanteiKbn != 1 && 
                p.OdrKouiKbn == OdrKouiKbnConst.Naifuku);

        }

        /// <summary>
        /// オーダーされている院外処方薬を取得する
        /// </summary>
        /// <param name="santeiKbn"></param>
        /// <returns>フィルター後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDtlIngaiSyoho()
        {
            return _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.HokenSyu == _hokenKbn &&
                p.RaiinNo == _raiinNo &&
                p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                p.OdrKouiKbn >= OdrKouiKbnConst.Touyaku &&
                p.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo &&
                p.InoutKbn == 1);
        }

        /// <summary>
        /// 指定の行為コードのオーダー詳細を取得する
        /// </summary>
        /// <param name="kouiKbnMin"></param>
        /// <param name="kouiKbnMax"></param>
        /// <param name="inOut"></param>
        /// <returns></returns>
        public List<OdrDtlTenModel> FilterOdrInfDtlByKouiKbnRange(int kouiKbnMin, int kouiKbnMax, int inOut)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.InoutKbn == inOut &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.OdrKouiKbn >= kouiKbnMin &&
                        p.OdrKouiKbn <= kouiKbnMax);
        }

        public List<OdrDtlTenModel> FilterOdrInfDtlByKouiKbnRangeItemCd(int kouiKbnMin, int kouiKbnMax, int inOut, List<string> itemCds)
        {
            return _odrDtlTenModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        (
                            (p.InoutKbn == inOut &&
                            p.OdrKouiKbn >= kouiKbnMin &&
                            p.OdrKouiKbn <= kouiKbnMax) ||
                        itemCds.Contains(p.ItemCd)) &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai
                        );
        }

        /// <summary>
        /// 指定のITEM_CDがコメント用のITEM_CDかどうかチェック
        /// (空文字　または　"8"で始まる9桁の値の場合、true
        /// </summary>
        /// <param name="itemCd"></param>
        /// <returns>true: コメント用項目コード</returns>
        public bool IsCommentItemCd(string itemCd)
        {
            return itemCd == "" ||
                        (itemCd.StartsWith("8") && itemCd.Length == 9);
        }


        /// <summary>
        /// 現在処理中の来院のオーダー詳細を指定のRP_NO, RP_EDA_NOでフィルタ
        /// odrInfとの連動に使用
        /// </summary>
        /// <param name="rpNo">フィルタ条件に指定するRP_NO</param>
        /// <param name="rpEdaNo">フィルタ条件に指定するRP_EDA_NO</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailByRpNo(long rpNo, long rpEdaNo)
        {
            return _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                //p.HokenSyu == _hokenKbn && 
                p.RaiinNo == _raiinNo &&
                //p.SanteiKbn != 1 && 
                p.RpNo == rpNo &&
                p.RpEdaNo == rpEdaNo);
        }

        /// <summary>
        /// 指定のRpの自費項目を取得する
        /// </summary>
        /// <param name="rpNo"></param>
        /// <param name="rpEdaNo"></param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailJihiByRpNo(long rpNo, long rpEdaNo)
        {
            return _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.HokenSyu == _hokenKbn &&
                p.RaiinNo == _raiinNo &&
                //p.SanteiKbn != 1 &&
                p.RpNo == rpNo &&
                p.RpEdaNo == rpEdaNo &&
                p.TenMst != null &&
                p.TenMst.JihiSbt > 0);
        }

        /// <summary>
        /// 現在処理中の診療日のオーダー詳細を指定のRP_NO, RP_EDA_NOでフィルタ
        /// 点滴計算用
        /// </summary>
        /// <param name="rpNo">フィルタ条件に指定するRP_NO</param>
        /// <param name="rpEdaNo">フィルタ条件に指定するRP_EDA_NO</param>
        /// <returns>フィルタ後のリスト</returns>
        public List<OdrDtlTenModel> FilterOdrDetailTentekiByRpNo(long raiinNo, long rpNo, long rpEdaNo)
        {
            return _odrDtlTenModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.RaiinNo == raiinNo &&
                p.HokenSyu == _hokenKbn &&
                //p.SanteiKbn != 1 && 
                p.RpNo == rpNo &&
                p.RpEdaNo == rpEdaNo);
        }

        /// <summary>
        /// 現在処理中の来院の指定のITEM_CDに該当するODR_DETTAILのレコードを削除する
        /// </summary>
        /// <param name="itemCds">削除したい項目のITEM_CD</param>
        public void RemoveOdrDtlByItemCd(List<string> itemCds)
        {
            _odrDtlTenModels.RemoveAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.HokenSyu == _hokenKbn &&
                p.RaiinNo == _raiinNo &&
                p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                itemCds.Contains(p.ItemCd));
        }

        public void RemoveOdrDtlByItemCd(string itemCd)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            RemoveOdrDtlByItemCd(itemCds);
        }

        /// <summary>
        /// 指定のオーダー詳細レコードに紐づくオーダーコメント情報を取得
        /// </summary>
        /// <param name="rpNo">取得対象オーダー詳細レコードのRP_NO</param>
        /// <param name="rpEdaNo">取得対象オーダー詳細レコードのRP_EDA_NO</param>
        /// <param name="rowNo">取得対象オーダー詳細レコードのROW_NO</param>
        /// <returns>指定のオーダー詳細レコードに紐づくオーダーコメント情報</returns>
        public List<OdrInfCmtModel> FilterOdrInfComment(long rpNo, long rpEdaNo, int rowNo)
        {
            return _odrInfCmtModels.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.RaiinNo == _raiinNo &&
                p.RpNo == rpNo &&
                p.RpEdaNo == rpEdaNo &&
                p.RowNo == rowNo);
        }

        public List<OdrInfCmtModel> GetOdrCmt(OdrDtlTenModel odrDtl)
        {
            return FilterOdrInfComment(odrDtl.RpNo, odrDtl.RpEdaNo, odrDtl.RowNo);
        }

        /// <summary>
        /// 指定の行為番号で使用しているHOKEN_PIDとSANTEI_KBNのリストを返す
        /// </summary>
        /// <returns>指定の行為番号で使用しているHOKEN_PIDとSANTEI_KBNのリスト</returns>
        //public List<(int hokenPid, int hokenId, int santeiKbn)> GetPidList(int kouiKbn)
        //{
        //    List<(int, int)> ret = new List<(int, int)>();

        //    // 薬剤で使用しているHOKEN_PID
        //    var odrInfs = _odrInfModels.FindAll(p =>
        //                p.HpId == _hpId &&
        //                p.PtId == _ptId &&
        //                p.HokenSyu == _hokenKbn &&
        //                p.RaiinNo == _raiinNo &&
        //                p.OdrKouiKbn == kouiKbn &&
        //                p.SanteiKbn != SanteiKbnConst.SanteiGai &&
        //                p.IsDeleted == DeleteStatus.None);
        //    var odrDtls = _odrDtlTenModels.FindAll(p =>
        //                p.HpId == _hpId &&
        //                p.PtId == _ptId &&
        //                p.HokenSyu == _hokenKbn &&
        //                p.RaiinNo == _raiinNo);

        //    var _join = (
        //        from odrInf in odrInfs
        //        join odrDtl in odrDtls on
        //            new { odrInf.HpId, odrInf.PtId, odrInf.RpNo, odrInf.RpEdaNo } equals
        //            new { odrDtl.HpId, odrDtl.PtId, odrDtl.RpNo, odrDtl.RpEdaNo }
        //        group odrInf by new { odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn } into A
        //        select new
        //        {
        //            A.Key.HokenPid,
        //            A.Key.HokenId,
        //            A.Key.SanteiKbn
        //        }
        //       );
        //    var entities = _join.ToList();

        //    List<(int, int , int)> results = new List<(int, int, int)>();

        //    entities?.ForEach(entity => {
        //        results.Add((entity.HokenPid, entity.HokenId, entity.SanteiKbn));
        //    });

        //    return results;
        //}

        public List<(int hokenPid, int hokenId, int santeiKbn)> GetPidList(int kouiKbn)
        {
            List<(int, int)> ret = new List<(int, int)>();

            // 薬剤で使用しているHOKEN_PID
            var odrInfs = _odrInfModels.FindAll(p =>
                        p.HpId == _hpId &&
                        p.PtId == _ptId &&
                        p.HokenSyu == _hokenKbn &&
                        p.RaiinNo == _raiinNo &&
                        p.OdrKouiKbn == kouiKbn &&
                        p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                        p.IsDeleted == DeleteStatus.None)
                .Select(p => new { p.HokenPid, p.HokenId, p.SanteiKbn })
                .Distinct()
                .ToList();

            List<(int, int, int)> results = new List<(int, int, int)>();

            odrInfs?.ForEach(entity =>
            {
                results.Add((entity.HokenPid, entity.HokenId, entity.SanteiKbn));
            });

            return results;
        }

        /// <summary>
        /// オーダー詳細の項目コードを差し替える
        /// 点数マスタからレセ名称と算定診療行為コードを取得し更新する
        /// </summary>
        /// <param name="odrDtl"></param>
        /// <param name="itemCd"></param>
        public void UpdateOdrDtlItemCd(OdrDtlTenModel odrDtl, string itemCd)
        {
            odrDtl.ItemCd = itemCd;
            odrDtl.OdrItemCd = itemCd;

            List<TenMstModel> tenMst = _tenMstCommon.GetTenMst(itemCd);
            if (tenMst.Any())
            {
                odrDtl.TenMst = tenMst.First();
                odrDtl.ReceName = odrDtl.TenMst.ReceName;
                odrDtl.SanteiItemCd = odrDtl.TenMst.SanteiItemCd;
                odrDtl.TyuCd = odrDtl.TenMst.TyuCd;
            }
        }

        public bool CheckNaifuku5Syu()
        {
            const string conFncName = nameof(CheckNaifuku5Syu);

            var odrInfDetails = _odrDtlTenModels.FindAll(o =>
                o.DrugKbn > 0);
            var odrInfs = _odrInfModels.FindAll(o =>
                o.OdrKouiKbn == 21 &&
                o.SanteiKbn == SanteiKbnConst.Santei &&
                (o.SyohoSbt == 2 || (o.SyohoSbt == 0 && o.DaysCnt > _systemConfigProvider.GetSyohoRinjiDays())) &&
                o.IsDeleted == DeleteStatus.None);

            var joinTemp = (
                from odrInfDetail in odrInfDetails
                join odrInf in odrInfs on
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo } equals
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                where
                    odrInf.OdrKouiKbn == 21 &&
                    odrInf.SanteiKbn == SanteiKbnConst.Santei &&
                    (odrInf.SyohoSbt == 2 || (odrInf.SyohoSbt == 0 && odrInf.DaysCnt > _systemConfigProvider.GetSyohoRinjiDays())) &&
                    odrInf.IsDeleted == DeleteStatus.None
                group odrInfDetail by new { odrInfDetail.RaiinNo, odrInfDetail.ItemCd } into A
                select new
                {
                    raiinNo = A.Key.RaiinNo,
                    count = A.Count()
                }
            );

            var joinQuery = (
                from j in joinTemp
                group j by new { j.raiinNo } into A
                select new
                {
                    raiinNo = A.Key.raiinNo,
                    count = A.Count()
                }
                )
            .ToList();

            bool ret = false;
            if (joinQuery.Any(p => p.count > 5))
            {
                ret = true;
            }
            return ret;
        }

        public bool CheckKouseisin()
        {
            const string conFncName = nameof(CheckKouseisin);

            var odrInfDetails = _odrDtlTenModels.FindAll(o =>
                o.DrugKbn > 0);
            var odrInfs = _odrInfModels.FindAll(o =>
                (o.OdrKouiKbn == 21 || o.OdrKouiKbn == 22 || o.OdrKouiKbn == 23) &&
                o.SanteiKbn == SanteiKbnConst.Santei &&
                o.IsDeleted == DeleteStatus.None);

            var tenMsts = _masterFinder.FindTenMstByKouseisinKbn(_hpId, _sinDate, new List<int> { 1, 2, 3, 4 });

            var joinQuery = (
                from odrInfDetail in odrInfDetails
                join odrInf in odrInfs on
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo } equals
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                join tenMst in tenMsts on
                    new { odrInfDetail.HpId, odrInfDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd }
                where
                    //odrInf.OdrKouiKbn == 21 &&
                    new int[] { 21, 22, 23 }.Contains(odrInf.OdrKouiKbn) &&
                    odrInf.SanteiKbn == SanteiKbnConst.Santei &&
                    !(odrInf.OdrKouiKbn == 21 &&
                        new int[] { 3, 4 }.Contains(tenMst.KouseisinKbn) &&
                        (odrInf.SyohoSbt == 1 || (odrInf.SyohoSbt == 0 && odrInf.DaysCnt <= _systemConfigProvider.GetSyohoRinjiDays()))) &&
                    odrInf.IsDeleted == DeleteStatus.None
                group new { odrInfDetail, tenMst } by new { odrInfDetail.RaiinNo, ipnCd7 = tenMst.IpnNameCd.Substring(0, 7) } into A
                select new
                {
                    raiinNo = A.Key.RaiinNo
                }
                )
                .GroupBy(p => p.raiinNo).Select(p => new { p.Key, count = p.Count() })
                .ToList();

            bool ret = false;

            if (joinQuery.Any(p => p.count > 3))
            {
                ret = true;
            }

            return ret;
        }

        public OdrDtlTenModel CopyOdrDtl(OdrDtlTenModel odrDtl)
        {
            return new OdrDtlTenModel(
                odrDtl.OdrInfDetail,
                odrDtl.TenMst,
                odrDtl.CmtKbnMst,
                odrDtl.ReceName,
                odrDtl.HokenKbn,
                odrDtl.HokenPid,
                odrDtl.HokenId,
                odrDtl.HokenSyu,
                odrDtl.OdrKouiKbn,
                odrDtl.SanteiKbn,
                odrDtl.InoutKbn,
                odrDtl.SyohoSbt,
                odrDtl.DaysCnt,
                odrDtl.SortNo,
                odrDtl.Kasan1,
                odrDtl.Kasan2,
                odrDtl.SinStartTime,
                odrDtl.MinYakka
                );
        }

        public bool IsRefill
        {
            get
            {
                if (_isRefill < 0)
                {
                    _isRefill = 0;
                    if (_odrDtlTenModels.Any(p =>
                                 p.HpId == _hpId &&
                                 p.PtId == _ptId &&
                                 p.HokenSyu == _hokenKbn &&
                                 p.RaiinNo == _raiinNo &&
                                 p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                                 p.ItemCd == ItemCdConst.Con_Refill))
                    {
                        _isRefill = 1;
                    }
                }

                return _isRefill == 1;
            }
        }
    }
}
