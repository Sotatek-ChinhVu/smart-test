using EmrCalculateApi.Interface;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using EmrCalculateApi.Futan.ViewModels;
using EmrCalculateApi.Receipt.Models;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using EmrCalculateApi.Receipt.ViewModels;
using EmrCalculateApi.Constants;
using PostgreDataContext;
using Domain.Constant;

namespace EmrCalculateApi.Ika.ViewModels
{
    public class IkaCalculateViewModel : IIkaCalculateViewModel
    {
        private readonly IFutancalcViewModel _iFutancalcViewModel;

        private const string ModuleName = ModuleNameConst.EmrCalculateIka;
        private IkaCalculateFinder _ikaCalculateFinder;
        private OdrInfFinder _odrInfFinder;
        private RaiinInfFinder _raiinInfFinder;
        private MasterFinder _masterFinder;
        //private CommonBase.CommonMasters.DbAccess.MasterFinder _commonMstFinder;
        private SanteiFinder _santeiFinder;

        private ClearIkaCalculateCommandHandler _clearIkaCalculateCommandHandler;
        private SaveIkaCalculateCommandHandler _saveIkaCalculateCommandHandler;

        private List<TenMstModel> _cacheTenMst;
        private List<DensiSanteiKaisuModel> _cacheDensiSanteiKaisu;
        private List<ItemGrpMstModel> _cacheItemGrpMst;

        /// <summary>
        /// 来院情報
        /// </summary>
        private List<RaiinInfModel> _raiinInfModels;

        /// <summary>
        /// 共通データ
        /// </summary>
        IkaCalculateCommonDataViewModel _common;

        public List<CalcLogModel> ListCalcLog
        {
            get
            {
                if (_common?.calcLogs?.Count > 0)
                {
                    return _common.calcLogs;
                }
                return new List<CalcLogModel>();
            }
        }

        IkaCalculateArgumentViewModel ikaCalculateArgumentViewModel;

        /// <summary>
        /// 現在計算中の医療機関識別ID
        /// </summary>
        private int _hpId;
        /// <summary>
        /// 現在計算中の患者ID        
        /// </summary>
        private long _ptId;
        /// <summary>
        /// 現在計算中の診療日
        /// </summary>
        private int _sinDate;

        private readonly ITenantProvider _tenantProvider;
        private TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public IkaCalculateViewModel(IFutancalcViewModel iFutancalcViewModel, ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _iFutancalcViewModel = iFutancalcViewModel;
            // 変数初期化
            _ptId = 0;
            _sinDate = 0;

            _tenantProvider = tenantProvider;
            _systemConfigProvider = systemConfigProvider;
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
            _emrLogger = emrLogger;

            // オブジェクトの生成
            // 来院情報
            _raiinInfModels = new List<RaiinInfModel>();

            _ikaCalculateFinder = new IkaCalculateFinder(_tenantDataContext);
            _odrInfFinder = new OdrInfFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
            _raiinInfFinder = new RaiinInfFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
            _masterFinder = new MasterFinder(_tenantDataContext);

            // 点数マスタのキャッシュ
            //_cacheTenMst = new List<TenMstModel>();
            _cacheTenMst = GetDefaultTenMst();
            // 電子算定回数マスタのキャッシュ
            //_cacheDensiSanteiKaisu = _masterFinder.FindAllDensiSanteiKaisu();
            //FutanCalcVM = new FutancalcViewModel();
            _saveIkaCalculateCommandHandler = new SaveIkaCalculateCommandHandler(_tenantProvider, _tenantDataContext, _emrLogger);
            _clearIkaCalculateCommandHandler = new ClearIkaCalculateCommandHandler(_tenantDataContext, _emrLogger);
        }
        /// <summary>
        /// 計算準備ロジック
        /// 必要なデータを取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        public void StartCalculate(int hpId, long ptId, int sinDate, int seikyuUp, int calcMode, string preFix, int clearReceChk = 1)
        {
            const string conFncName = nameof(StartCalculate);

            _emrLogger.WriteLogStart( this, conFncName,
                String.Format("hpId={0} ptId={1} sinDate={2} seikyuUp={3}", hpId, ptId, sinDate, seikyuUp));

            //指定の診療日の来院情報を取得
            _raiinInfModels = _raiinInfFinder.FindRaiinInfData(hpId, ptId, sinDate);

            //メンバー変数設定
            _hpId = hpId;
            _ptId = ptId;
            _sinDate = sinDate;

            //引数クラス生成

            ikaCalculateArgumentViewModel.hpId = _hpId;
            ikaCalculateArgumentViewModel.ptId = _ptId;
            ikaCalculateArgumentViewModel.sinDate = _sinDate;
            ikaCalculateArgumentViewModel.seikyuUp = seikyuUp;
            ikaCalculateArgumentViewModel.calcMode = calcMode;
            ikaCalculateArgumentViewModel.clearReceChk = clearReceChk;
            ikaCalculateArgumentViewModel.raiinInfs = _raiinInfModels;
            ikaCalculateArgumentViewModel.masterFinder = _masterFinder;
            //ikaCalculateArgumentViewModel.commonMstFinder = _commonMstFinder;
            ikaCalculateArgumentViewModel.santeiFinder = _santeiFinder;
            ikaCalculateArgumentViewModel.odrInfFinder = _odrInfFinder;
            ikaCalculateArgumentViewModel.ikaCalculateFinder = _ikaCalculateFinder;
            ikaCalculateArgumentViewModel.saveHandler = _saveIkaCalculateCommandHandler;
            ikaCalculateArgumentViewModel.cacheTenMst = _cacheTenMst;
            ikaCalculateArgumentViewModel.cacheDensiSanteiKaisu = _cacheDensiSanteiKaisu;
            ikaCalculateArgumentViewModel.cacheItemGrpMst = _cacheItemGrpMst;
            ikaCalculateArgumentViewModel.preFix = preFix;

            _common = new IkaCalculateCommonDataViewModel(ikaCalculateArgumentViewModel, _systemConfigProvider, _emrLogger);

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        /// <summary>
        /// 計算結果をデータベースに反映する
        /// </summary>
        public void UpdateDatabase()
        {
            const string conFncName = nameof(UpdateDatabase);
            _emrLogger.WriteLogStart(this, conFncName, "");

            _saveIkaCalculateCommandHandler.UpdateData(_common);

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 算定ログをデータベースに反映する
        /// 例外発生時、問題が発生したことを算定ログに残すための専用メソッド
        /// </summary>
        public void UpdateCalcLog()
        {
            const string conFncName = nameof(UpdateCalcLog);
            _emrLogger.WriteLogStart(this, conFncName, "");

            try
            {
                _saveIkaCalculateCommandHandler.AddErrorCalcLog(_common.calcLogs);
                //_tenantDataContext.SaveChanged();
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        public int AllCalcCount { get; set; }

        private int _calculatedCount;
        public int CalculatedCount
        {
            get => _calculatedCount;
            set
            {
                _calculatedCount = value;
                AfterCalcItemEvent?.Invoke();
            }
        }

        public bool IsStopCalc { get; set; }

        public AfterCalcItem AfterCalcItemEvent { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public delegate void AfterCalcItem();

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="seikyuUp">請求UP情報</param>
        public void RunCalculate(int hpId, long ptId, int sinDate, int seikyuUp, string preFix)
        {
            CalculatedCount = 0;
            const string conFncName = nameof(RunCalculate);

            // 電子算定回数マスタのキャッシュ
            _cacheDensiSanteiKaisu = _masterFinder.FindAllDensiSanteiKaisu();
            // 項目グループマスタのキャッシュ
            _cacheItemGrpMst = _masterFinder.FindAllItemGrpMst();

            CalcStatusModel calcStatus = new CalcStatusModel(new CalcStatus());

            ikaCalculateArgumentViewModel = new IkaCalculateArgumentViewModel();

            // 要求登録           
            AddCalcStatus(hpId, ptId, sinDate, seikyuUp, preFix);

            // 要求がある限りループ
            while (!IsStopCalc && GetCalcStatus(hpId, ptId, sinDate, ref calcStatus, preFix))
            {
                if (CancellationToken.IsCancellationRequested) return;
                _emrLogger.WriteLogMsg( this, conFncName, "req start");

                if (calcStatus.SinDate <= 20180331)
                {
                    _emrLogger.WriteLogError( this, conFncName, new Exception($"2018/03以前 {calcStatus.SinDate}"));

                    List<CalcStatusModel> calcStatusies = new List<CalcStatusModel>();

                    if (calcStatus.PtId == ptId && calcStatus.SinDate == sinDate)
                    {
                        calcStatusies.AddRange(_ikaCalculateFinder.GetSameCalcStatus(calcStatus.CalcId, preFix));
                    }
                    else
                    {
                        calcStatusies.AddRange(_ikaCalculateFinder.GetSameCalcStatus(calcStatus, preFix));
                    }
                    foreach (CalcStatusModel updCalcStatus in calcStatusies)
                    {
                        updCalcStatus.Status = 8;
                    }

                    if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(calcStatusies) == false)
                    {
                        // falseのまま、放置するわけにいかないのでリトライする
                        List<long> calcIds = calcStatusies.Select(p => p.CalcId).ToList();

                        List<CalcStatusModel> updCalcStatusies = _ikaCalculateFinder.GetCalcStatusies(calcIds, preFix);

                        foreach (CalcStatusModel updCalcStatus in updCalcStatusies)
                        {
                            updCalcStatus.Status = 8;
                        }

                        if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(updCalcStatusies) == false)
                        {
                            _emrLogger.WriteLogError( this, conFncName, new Exception("update calcstatus error (8)"));
                        }
                    }
                }
                else
                {
                    int i = 0;

                    if (CheckCalcStatus(calcStatus))
                    {
                        // チェック
                        // 他端末で当該患者の当該診療日が属する月の計算中の場合、待機する
                        while (CheckCalcStatusOther(calcStatus) && i < 300)
                        {
                            i++;
                            _emrLogger.WriteLogMsg(this, conFncName,
                                string.Format("Calculating Other ptId={0}, sinDate={1}, retry={2}", calcStatus.PtId, calcStatus.SinDate, i));
                            System.Threading.Thread.Sleep(1000);

                            if (GetCalcStatus(calcStatus.CalcId) != 0)
                            {
                                // 待機中にステータスが変更された場合、この要求は他のプロセスが処理しているので処理しない
                                break;
                            }
                        }

                        // 自端末で当該患者の当該診療日が属する月の計算中の場合、待機する
                        i = 0;
                        while (CheckCalcStatusSelf(calcStatus) && i < 30)
                        {
                            i++;
                            _emrLogger.WriteLogMsg( this, conFncName,
                                string.Format("Calculating Self ptId={0}, sinDate={1}, retry={2}", calcStatus.PtId, calcStatus.SinDate, i));

                            System.Threading.Thread.Sleep(1000);

                            if (GetCalcStatus(calcStatus.CalcId) != 0)
                            {
                                // 待機中にステータスが変更された場合、この要求は他のプロセスが処理しているので処理しない
                                break;
                            }

                        }

                        if (GetCalcStatus(calcStatus.CalcId) != 0)
                        {
                            // 待機中にステータスが変更された場合、この要求は他のプロセスが処理しているので処理しない
                            _emrLogger.WriteLogMsg(this, conFncName, $"Calculating by Other Process CalcId={calcStatus.CalcId}");
                            continue;
                        }
                    }

                    _tenantDataContext?.Dispose();
                    _tenantDataContext = _tenantProvider.GetTrackingTenantDataContext();

                    // 要求ロック
                    List<CalcStatusModel> calcStatusies = new List<CalcStatusModel>();

                    if (calcStatus.PtId == ptId && calcStatus.SinDate == sinDate)
                    {
                        calcStatusies.AddRange(_ikaCalculateFinder.GetSameCalcStatus(calcStatus.CalcId, preFix));
                    }
                    else
                    {
                        calcStatusies.AddRange(_ikaCalculateFinder.GetSameCalcStatus(calcStatus, preFix));
                    }
                    foreach (CalcStatusModel updCalcStatus in calcStatusies)
                    {
                        updCalcStatus.Status = 1;
                    }

                    if (GetCalcStatus(calcStatus.CalcId) != 0)
                    {
                        // 直前に最終チェック
                        _emrLogger.WriteLogMsg( this, conFncName, $"Calculating by Other Process CalcId={calcStatus.CalcId}");
                        continue;
                    }

                    if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(calcStatusies) == false)
                    {
                        // falseのまま、放置するわけにいかないので、0に戻すようリトライする
                        List<long> calcIds = calcStatusies.Select(p => p.CalcId).ToList();

                        List<CalcStatusModel> updCalcStatusies = _ikaCalculateFinder.GetCalcStatusies(calcIds, preFix);

                        foreach (CalcStatusModel updCalcStatus in updCalcStatusies)
                        {
                            updCalcStatus.Status = 0;
                        }

                        if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(updCalcStatusies) == false)
                        {
                            _emrLogger.WriteLogError( this, conFncName, new Exception("update calcstatus error (0)"));
                        }
                    }
                    else
                    {
                        //計算準備
                        StartCalculate(calcStatus.HpId, calcStatus.PtId, calcStatus.SinDate, calcStatus.SeikyuUp, calcStatus.CalcMode, preFix);

                        //計算処理
                        bool ret = MainCalculate();

                        // 点数マスタのキャッシュを受け取り（次回の計算に引き継ぐため）
                        _cacheTenMst = _common.CacheTenMst;

                        if (ret == true)
                        {
                            // 負担金計算用引数データ取得
                            (List<Futan.Models.SinKouiCountModel> argSinKouiCountModels,
                                List<Futan.Models.SinKouiModel> argSinKouiModels,
                                List<Futan.Models.SinKouiDetailModel> argSinKouiDetailModels,
                                List<Futan.Models.SinRpInfModel> argSinRpInfModels) =
                                    GetArgSinData();

                            FutancalcViewModel FutanCalcVM = new FutancalcViewModel(_tenantProvider, _systemConfigProvider, _emrLogger);
                            try
                            {
                                FutanCalcVM.FutanCalculation(_common.ptId, _common.sinDate, argSinKouiCountModels, argSinKouiModels, argSinKouiDetailModels, argSinRpInfModels, calcStatus.SeikyuUp);
                            }
                            catch (Exception e)
                            {
                                _emrLogger.WriteLogError(this, conFncName, new Exception(string.Format("PtId={0} SinDate={1}", _common.ptId, _common.sinDate) + ":" + e.Message));
                                ret = false;

                                _common.AppendCalcLog(9, "負担金計算で問題が発生したため、計算できません。");
                                UpdateCalcLog();
                            }
                            finally
                            {
                                //FutanCalcVM.Dispose();
                            }
                        }

                        // 要求更新
                        if (ret == true)
                        {
                            // 正常終了
                            //calcStatus.Status = 9;
                            foreach (CalcStatusModel updCalcStatus in calcStatusies)
                            {
                                updCalcStatus.Status = 9;
                            }
                        }
                        else
                        {
                            // エラー
                            //calcStatus.Status = 8;
                            foreach (CalcStatusModel updCalcStatus in calcStatusies)
                            {
                                updCalcStatus.Status = 8;
                            }
                        }
                        //_saveIkaCalculateCommandHandler.UpdateCalcStatus(calcStatus);
                        if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(calcStatusies) == false)
                        {
                            // falseのまま、放置するわけにいかないのでリトライする
                            List<long> calcIds = calcStatusies.Select(p => p.CalcId).ToList();

                            List<CalcStatusModel> updCalcStatusies = _ikaCalculateFinder.GetCalcStatusies(calcIds, preFix);

                            foreach (CalcStatusModel updCalcStatus in updCalcStatusies)
                            {
                                updCalcStatus.Status = 8;
                            }

                            if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(updCalcStatusies) == false)
                            {
                                _emrLogger.WriteLogError(this, conFncName, new Exception("update calcstatus error (8)"));
                            }
                        }

                        // レセプト状態情報のSTATUS_KBN=8を0に戻す
                        if (ikaCalculateArgumentViewModel.clearReceChk == 1)
                        {
                            List<int> hokenIds = _common.Sin.SinKouis.FindAll(p => p.IsDeleted == DeleteStatus.None).GroupBy(p => p.HokenId).Select(p => p.Key).ToList();
                            List<EmrCalculateApi.Ika.Models.ReceStatusModel> receStatusies = _ikaCalculateFinder.FindReceStatus(hpId, ptId, sinDate / 100, hokenIds);
                            _saveIkaCalculateCommandHandler.UpdateReceStatus(receStatusies);
                        }

                        // ★Test用
                        //if (ICDebugConf.expSinMei >= 0)
                        //{
                        //    List<long> raiinNos = new List<long> { _common.raiinNo };
                        //    SinMeiViewModel sinMeiVM = new SinMeiViewModel(ICDebugConf.expSinMei, false, _common.hpId, _common.ptId, _common.sinDate, raiinNos,
                        //        _common.Sin.SinRpInfs.FindAll(p => p.IsDeleted == DeleteStatus.None),
                        //        _common.Sin.SinKouis.FindAll(p => p.IsDeleted == DeleteStatus.None),
                        //        _common.Sin.SinKouiCounts.FindAll(p => p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add),
                        //        _common.Sin.SinKouiDetails.FindAll(p => p.IsDeleted == DeleteStatus.None));

                        //    sinMeiVM.SaveToFile(System.AppDomain.CurrentDomain.BaseDirectory + @"e3_sinmei_" + _common.ptId.ToString() + "_" + _common.sinDate.ToString().Substring(0, 6) + ".txt");

                        //}
                    }
                }

                CalculatedCount++;
            }
        }
        /// <summary>
        /// 再計算処理
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="seikyuYm">請求年月</param>
        /// <param name="ptIds">患者ID(null:未指定)</param>
        public void RunCalculateMonth(int hpId, int seikyuYm, List<long> ptIds, string preFix)
        {
            const string conFncName = nameof(RunCalculateMonth);
            _emrLogger.WriteLogStart( this, conFncName, "");

            preFix = preFix + "MON_";

            //要求登録
            AddCalcStatusMonth(hpId, seikyuYm, ptIds, preFix);

            AllCalcCount = _ikaCalculateFinder.GetCountCalcInMonth(preFix);

            //計算処理
            RunCalculate(hpId, 0, 0, 0, preFix);

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }
        /// <summary>
        /// 計算処理（指定の診療日のみ）
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        /// <param name="seikyuUp"></param>
        public void RunCalculateOne(int hpId, long ptId, int sinDate, int seikyuUp, string preFix)
        {
            preFix = preFix + "ONE_";

            // 要求登録
            AddCalcStatusOne(hpId, ptId, sinDate, seikyuUp, preFix);
            RunCalculate(0, 0, 0, seikyuUp, preFix);
        }
        /// <summary>
        /// 負担金計算の引数用診療データを取得する
        /// </summary>
        /// <returns></returns>
        private (List<Futan.Models.SinKouiCountModel>, List<Futan.Models.SinKouiModel>,
        List<Futan.Models.SinKouiDetailModel>, List<Futan.Models.SinRpInfModel>)
        GetArgSinData()
        {
            List<Futan.Models.SinKouiCountModel> argSinKouiCountModels = new List<Futan.Models.SinKouiCountModel>();
            List<Futan.Models.SinKouiModel> argSinKouiModels = new List<Futan.Models.SinKouiModel>();
            List<Futan.Models.SinKouiDetailModel> argSinKouiDetailModels = new List<Futan.Models.SinKouiDetailModel>();
            List<Futan.Models.SinRpInfModel> argSinRpInfModels = new List<Futan.Models.SinRpInfModel>();

            if (_common.Sin.SinKouiCounts != null)
            {
                argSinKouiCountModels.AddRange(
                    _common.Sin.SinKouiCounts.Where(p =>
                        p.UpdateState == UpdateStateConst.None)
                        .Select(x => new Futan.Models.SinKouiCountModel(x.SinKouiCount)).ToList());
            }

            if (_common.Sin.SinKouis != null)
            {
                argSinKouiModels.AddRange(
                    _common.Sin.SinKouis.Where(p =>
                        p.IsDeleted == DeleteStatus.None &&
                        p.UpdateState == UpdateStateConst.None)
                        .Select(x => new Futan.Models.SinKouiModel(x.SinKoui)).ToList());
            }

            if (_common.Sin.SinKouiDetails != null)
            {
                argSinKouiDetailModels.AddRange(
                    _common.Sin.SinKouiDetails.Where(p =>
                        p.IsDeleted == DeleteStatus.None &&
                        p.UpdateState == UpdateStateConst.None)
                        .Select(x => new Futan.Models.SinKouiDetailModel(x.SinKouiDetail, x.TenMst?.TenMst ?? null)).ToList());
            }

            if (_common.Sin.SinRpInfs != null)
            {
                argSinRpInfModels.AddRange(
                    _common.Sin.SinRpInfs.Where(p =>
                        p.IsDeleted == DeleteStatus.None &&
                        p.UpdateState == UpdateStateConst.None)
                        .Select(x => new Futan.Models.SinRpInfModel(x.SinRpInf)).ToList());
            }

            return (argSinKouiCountModels, argSinKouiModels, argSinKouiDetailModels, argSinRpInfModels);
        }

        /// <summary>
        /// 試算用負担金計算の引数を取得する
        /// </summary>
        /// <returns></returns>
        private (List<Futan.Models.SinKouiCountModel>, List<Futan.Models.SinKouiModel>,
            List<Futan.Models.SinKouiDetailModel>, List<Futan.Models.SinRpInfModel>)
            GetArgTrialSinData()
        {
            List<Futan.Models.SinKouiCountModel> argSinKouiCountModels = new List<Futan.Models.SinKouiCountModel>();
            List<Futan.Models.SinKouiModel> argSinKouiModels = new List<Futan.Models.SinKouiModel>();
            List<Futan.Models.SinKouiDetailModel> argSinKouiDetailModels = new List<Futan.Models.SinKouiDetailModel>();
            List<Futan.Models.SinRpInfModel> argSinRpInfModels = new List<Futan.Models.SinRpInfModel>();

            if (_common.Sin.SinKouiCounts != null)
            {
                argSinKouiCountModels.AddRange(
                    _common.Sin.SinKouiCounts.Where(p =>
                        p.UpdateState == UpdateStateConst.Add)
                        .Select(x => new Futan.Models.SinKouiCountModel(x.SinKouiCount)).ToList());
            }

            if (_common.Sin.SinKouis != null)
            {
                argSinKouiModels.AddRange(
                    _common.Sin.SinKouis.Where(p =>
                        p.IsDeleted == DeleteStatus.None &&
                        p.UpdateState == UpdateStateConst.Add)
                        .Select(x => new Futan.Models.SinKouiModel(x.SinKoui)).ToList());
            }

            if (_common.Sin.SinKouiDetails != null)
            {
                argSinKouiDetailModels.AddRange(
                    _common.Sin.SinKouiDetails.Where(p =>
                        p.IsDeleted == DeleteStatus.None &&
                        p.UpdateState == UpdateStateConst.Add)
                        .Select(x => new Futan.Models.SinKouiDetailModel(x.SinKouiDetail, x.TenMst?.TenMst ?? null)).ToList());
            }

            if (_common.Sin.SinRpInfs != null)
            {
                argSinRpInfModels.AddRange(
                    _common.Sin.SinRpInfs.Where(p =>
                        p.IsDeleted == DeleteStatus.None &&
                        p.UpdateState == UpdateStateConst.Add)
                        .Select(x => new Futan.Models.SinRpInfModel(x.SinRpInf)).ToList());
            }

            return (argSinKouiCountModels, argSinKouiModels, argSinKouiDetailModels, argSinRpInfModels);
        }

        /// <summary>
        /// 計算要求登録処理
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="seikyuYm">請求年月</param>
        /// <param name="ptIds">患者ID</param>
        public void AddCalcStatusMonth(int hpId, int seikyuYm, List<long> ptIds, string preFix = "")
        {
            List<RaiinDaysModel> raiinDays = _raiinInfFinder.FindRaiinInfDaysInMonth(hpId, seikyuYm, ptIds);

            List<CalcStatusModel> calcStatusies = raiinDays.Select(
                r =>
                    new CalcStatusModel(
                        new CalcStatus()
                        {
                            HpId = r.HpId,
                            PtId = r.PtId,
                            SinDate = r.SinDate,
                            CalcMode = CalcModeConst.Continuity
                        }
                    )
            ).ToList();

            _saveIkaCalculateCommandHandler.AddCalcStatus(calcStatusies, preFix);
        }

        /// <summary>
        /// 計算要求登録処理
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="seikyuUp">請求情報更新</param>
        public void AddCalcStatus(int hpId, long ptId, int sinDate, int seikyuUp, string preFix)
        {
            List<CalcStatusModel> calcStatusies = new List<CalcStatusModel>();

            CalcStatusModel calcStatus;

            // 患者ID、診療日の指定がある場合は当月分追加
            if (hpId > 0 && ptId > 0 && sinDate > 0)
            {
                calcStatus = new CalcStatusModel(new CalcStatus());
                calcStatus.HpId = hpId;
                calcStatus.PtId = ptId;
                calcStatus.SinDate = sinDate;
                calcStatus.SeikyuUp = seikyuUp;
                calcStatus.CalcMode = CalcModeConst.Normal;
                calcStatusies.Add(calcStatus);

                List<RaiinDaysModel> raiinDays = _raiinInfFinder.FindRaiinInfDays(hpId, ptId, sinDate);
                foreach (RaiinDaysModel raiinDay in raiinDays)
                {
                    calcStatus = new CalcStatusModel(new CalcStatus());
                    calcStatus.HpId = raiinDay.HpId;
                    calcStatus.PtId = raiinDay.PtId;
                    calcStatus.SinDate = raiinDay.SinDate;
                    calcStatus.CalcMode = CalcModeConst.Continuity;
                    calcStatusies.Add(calcStatus);
                }
            }
            _saveIkaCalculateCommandHandler.AddCalcStatus(calcStatusies, preFix);

        }
        /// <summary>
        /// 計算要求登録処理（指定の診療日のみ）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="seikyuUp">請求情報更新</param>
        public void AddCalcStatusOne(int hpId, long ptId, int sinDate, int seikyuUp, string preFix)
        {
            List<CalcStatusModel> calcStatusies = new List<CalcStatusModel>();

            CalcStatusModel calcStatus;

            // 患者ID、診療日の指定がある場合は追加
            if (hpId > 0 && ptId > 0 && sinDate > 0)
            {
                calcStatus = new CalcStatusModel(new CalcStatus());
                calcStatus.HpId = hpId;
                calcStatus.PtId = ptId;
                calcStatus.SinDate = sinDate;
                calcStatus.SeikyuUp = seikyuUp;
                calcStatus.CalcMode = CalcModeConst.Normal;
                calcStatusies.Add(calcStatus);
            }
            _saveIkaCalculateCommandHandler.AddCalcStatus(calcStatusies, preFix);

        }
        /// <summary>
        /// 計算要求取得
        /// </summary>
        /// <param name="calcStatus"></param>
        /// <returns></returns>
        public bool GetCalcStatus(int hpId, long ptId, int sinDate, ref CalcStatusModel calcStatus, string preFix)
        {
            bool ret = false;

            calcStatus = _ikaCalculateFinder.GetCalcStatus(hpId, ptId, sinDate, preFix);

            if (calcStatus != null)
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 指定の計算要求が別端末で処理中ではないかチェック
        /// </summary>
        /// <param name="calcStatus">計算要求情報</param>
        /// <returns>true: 他端末で処理中</returns>
        public bool CheckCalcStatus(CalcStatusModel calcStatus)
        {
            return _ikaCalculateFinder.CheckCalcStatus(calcStatus);
        }
        public bool CheckCalcStatusOther(CalcStatusModel calcStatus)
        {
            return _ikaCalculateFinder.CheckCalcStatusOther(calcStatus);
        }
        /// <summary>
        /// 指定の計算要求が自端末で処理中ではないかチェック
        /// </summary>
        /// <param name="calcStatus">計算要求情報</param>
        /// <returns>true: 他端末で処理中</returns>
        public bool CheckCalcStatusSelf(CalcStatusModel calcStatus)
        {
            return _ikaCalculateFinder.CheckCalcStatusSelf(calcStatus);
        }
        public int GetCalcStatus(long calcId)
        {
            return _ikaCalculateFinder.GetCalcStatus(calcId);
        }
        /// <summary>
        /// メインの計算ロジック
        /// </summary>
        public bool MainCalculate()
        {
            const string conFncName = nameof(MainCalculate);
            _emrLogger.WriteLogStart( this, conFncName, "");
            bool ret = true;

            int hokenKbn = 0;
            try
            {
                //データを削除する
                ClearData();

                foreach (RaiinInfModel raiinInfModel in _raiinInfModels)
                {
                    //_common.Sin.GetSinInf();
                    bool checkOdr = false;

                    // 引数設定
                    _common.raiinInf = raiinInfModel;

                    // 検査重複オーダー削除
                    _common.DelKensa();

                    // 並び順を調整
                    _common.SortOdrDtlRaiin();

                    //if (_common.IsRosai)
                    //{
                    // 労災合成コード
                    //_common.Odr.MakeRosaiGosei(_common.IsRosai);                    
                    //}
                    // 労災合成コード
                    _common.Odr.MakeRosaiGosei();

                    // 保険の種類分ループを回す
                    hokenKbn = 0;
                    while (hokenKbn <= 4)
                    {
                        // 引数設定
                        _common.hokenKbn = hokenKbn;

                        //オーダー存在確認
                        //if (_common.Odr.ExistOdrHokenSyu())
                        if (_common.Odr.ExistOdrDtlHokenSyu())
                        {
                            if (checkOdr == false)
                            {
                                // マスタと紐づけのない項目をログ出力する
                                _common.CheckOdr();
                                checkOdr = true;
                            }

                            //オーダー情報からワーク情報へ変換
                            OdrToWrk();

                            ////背反等調整処理
                            if ((_common.syosai != SyosaiConst.Jihi) && _raiinInfModels.Count() > 1)
                            {
                                // 同日複数来院がある場合は2回チェックする
                                AdjustWrk(true);
                            }

                            ////ワーク情報から算定情報へ変換
                            //WrkToSin();
                        }

                        hokenKbn++;
                    }

                    // 来院ごとに計算結果を確定する
                    //UpdateDatabase();

                    //_common.ClearKeisanData();
                }

                foreach (RaiinInfModel raiinInfModel in _raiinInfModels)
                {
                    hokenKbn = 0;
                    while (hokenKbn <= 4)
                    {
                        _common.raiinInf = raiinInfModel;
                        _common.hokenKbn = hokenKbn;

                        if ((_common.syosai != SyosaiConst.Jihi) &&
                            _common.Wrk.wrkSinKouiDetails.Any(p => p.RaiinNo == _common.raiinNo && p.HokenKbn == hokenKbn))
                        {
                            //背反等調整処理
                            AdjustWrk(false);
                        }

                        hokenKbn++;
                    }
                }

                foreach (RaiinInfModel raiinInfModel in _raiinInfModels)
                {
                    hokenKbn = 0;
                    while (hokenKbn <= 4)
                    {
                        _common.raiinInf = raiinInfModel;
                        _common.hokenKbn = hokenKbn;

                        //if (_common.Wrk.wrkSinKouiDetails.Any(p=>p.RaiinNo == _common.raiinNo && p.HokenKbn == hokenKbn))
                        //{
                        if (_common.Odr.ExistOdrDtlHokenSyu())
                        {
                            //ワーク情報から算定情報へ変換
                            WrkToSin();
                        }

                        hokenKbn++;
                    }
                }

                // データベースに反映
                UpdateDatabase();
                _common.ClearKeisanData();

                _clearIkaCalculateCommandHandler.ClearSinData(_hpId, _ptId, _sinDate);
                _tenantDataContext.SaveChanges();

            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);

                // 変更取り消し
                _common.Sin.Reload();
                _common.Wrk.Reload();

                _common.ClearCalcLog();
                _common.AppendCalcLog(9, "医科計算で問題が発生したため、計算できません。");
                UpdateCalcLog();

                ret = false;

                //throw;
            }

            _emrLogger.WriteLogEnd( this, conFncName, "");

            return ret;
        }

        /// <summary>
        /// 頻用自動発生項目の点数マスタを取得
        /// </summary>
        /// <returns></returns>
        private List<TenMstModel> GetDefaultTenMst()
        {
            List<string> itemCds = new List<string>()
            {
                ItemCdConst.Syosin,
                ItemCdConst.Saisin,

                // 特定疾患療養管理料（診療所）
                ItemCdConst.IgakuTokusitu,
                // 皮膚科特定疾患療養指導料（Ⅰ）
                ItemCdConst.SiHifuToku1,
                // 皮膚科特定疾患療養指導料（Ⅱ）
                ItemCdConst.SiHifuToku2,
                // 特定疾患処方管理加算１（処方料）
                ItemCdConst.TouyakuTokuSyo1Syoho,
                // 特定疾患処方管理加算１（処方箋料）
                ItemCdConst.TouyakuTokuSyo1Syohosen,
                // 特定疾患処方管理加算２（処方料）
                ItemCdConst.TouyakuTokuSyo2Syoho,
                // 特定疾患処方管理加算２（処方箋料）
                ItemCdConst.TouyakuTokuSyo2Syohosen,
                // てんかん指導料
                ItemCdConst.IgakuTenkan,
                // 難病外来指導管理料
                ItemCdConst.IgakuNanbyo,
                // 薬剤情報提供料
                ItemCdConst.YakuzaiJoho,
                // 薬剤情報提供料
                ItemCdConst.YakuzaiJohoTeiyo,
                // 乳幼児育児栄養指導料
                ItemCdConst.SiIkuji,
                // 認知症地域包括診療加算１
                ItemCdConst.SaisinNintiTiikiHoukatu1,
                // 認知症地域包括診療加算２
                ItemCdConst.SaisinNintiTiikiHoukatu2,
                // 外来感染対策向上加算（医学管理等）
                ItemCdConst.IgakuKansenKojo,
                // 連携強化加算（医学管理等）
                ItemCdConst.IgakuRenkeiKyoka,
                // サーベイランス強化加算（医学管理等）
                ItemCdConst.IgakuSurveillance,

                // 処方料（向精神薬多剤投与）
                ItemCdConst.TouyakuSyohoKousei,
                // 処方料（７種類以上内服薬又は向精神薬長期処方）
                ItemCdConst.TouyakuSyohoNaifuku,
                // 処方料（その他）
                ItemCdConst.TouyakuSyohoSonota,
                // 処方箋料（向精神薬多剤投与）
                ItemCdConst.TouyakuSyohosenKouSei,
                // 処方箋料（７種類以上内服薬又は向精神薬長期処方）
                ItemCdConst.TouyakuSyohosenNaifukuKousei,
                // 処方箋料（その他）
                ItemCdConst.TouyakuSyohosenSonota,
                // 一般名処方加算１（処方箋料）
                ItemCdConst.TouyakuIpnName1,
                // 一般名処方加算２（処方箋料）
                ItemCdConst.TouyakuIpnName2,

                // Ｂ－Ｖ
                ItemCdConst.KensaBV,
                // Ｂ－Ｃ
                ItemCdConst.KensaBC,
                // 尿・糞便等検査判断料
                ItemCdConst.KensaHandanNyou, 
                // 血液学的検査判断料
                ItemCdConst.KensaHandanKetueki, 
                // 生化学的検査（１）判断料
                ItemCdConst.KensaHandanSeika1, 
                // 生化学的検査（２）判断料
                ItemCdConst.KensaHandanSeika2, 
                // 免疫学的検査判断料
                ItemCdConst.KensaHandanMeneki, 
                // 微生物学的検査判断料
                ItemCdConst.KensaHandanBiseibutu, 
                // 呼吸機能検査等判断料
                ItemCdConst.KensaHandanKokyu, 
                // 脳波検査判断料２
                ItemCdConst.KensaHandanNoha2, 
                // 神経・筋検査判断料
                ItemCdConst.KensaHandanSinkei, 
                // ラジオアイソトープ検査判断料
                ItemCdConst.KensaHandanRadio, 
                // 病理判断料
                ItemCdConst.KensaHandanByori,
                // フリーコメント
                ItemCdConst.CommentFree
            };

            return _masterFinder.FindTenMstByItemCd(Hardcode.HospitalID, itemCds);
        }

        /// <summary>
        /// 試算処理
        /// </summary>
        /// <param name="todayOdrInfs">本日のオーダー内容</param>
        /// <param name="reception">来院情報</param>
        /// <param name="calcFutan">true-負担金計算を行う</param>
        /// <returns>
        /// 計算結果を返す
        ///     診療明細情報
        ///     会計情報
        /// </returns>
        //public (List<SinMeiDataModel>, List<Futan.Models.KaikeiInfModel>) RunTraialCalculate(List<TodayOdrInfModel> todayOdrInfs, ReceptionModel reception, bool calcFutan = true)
        //{
        //    const string conFncName = nameof(RunTraialCalculate);

        //    List<SinMeiDataModel> retSinMeis = new List<SinMeiDataModel>();
        //    List<Futan.Models.KaikeiInfModel> retKaikeiInfModels = new List<Futan.Models.KaikeiInfModel>();

        //    List<SinRpInfModel> retSinRpInfModels = new List<SinRpInfModel>();
        //    List<SinKouiModel> retSinKouiModels = new List<SinKouiModel>();
        //    List<SinKouiDetailModel> retSinKouiDetailModels = new List<SinKouiDetailModel>();
        //    List<SinKouiCountModel> retSinKouiCountModels = new List<SinKouiCountModel>();
        //    List<CalcLogModel> retCalcLogModels = new List<CalcLogModel>();

        //    if (todayOdrInfs != null && todayOdrInfs.Any())
        //    {
        //        if (todayOdrInfs.First().SinDate <= 20180331)
        //        {
        //            CalcLogModel addCalcLog = new CalcLogModel(new CalcLog());
        //            addCalcLog.HpId = Hardcode.HospitalID;
        //            addCalcLog.PtId = todayOdrInfs.First().PtId;
        //            addCalcLog.SinDate = todayOdrInfs.First().SinDate;
        //            addCalcLog.RaiinNo = todayOdrInfs.First().RaiinNo;
        //            addCalcLog.LogSbt = 2;
        //            addCalcLog.Text = "2018年3月以前の計算には対応していません。";
        //            addCalcLog.CreateDate = DateTime.Now;
        //            addCalcLog.CreateId = 0;
        //            retCalcLogModels.Add(addCalcLog);

        //            ikaCalculateArgumentViewModel = new IkaCalculateArgumentViewModel();

        //            //計算準備
        //            StartTrialCalculate(todayOdrInfs.First().HpId, todayOdrInfs.First().PtId, todayOdrInfs.First().SinDate, reception);

        //            _common.calcLogs.Add(addCalcLog);
        //        }
        //        else
        //        {
        //            // 電子算定回数マスタのキャッシュ
        //            _cacheDensiSanteiKaisu = _masterFinder.FindAllDensiSanteiKaisu();
        //            // 項目グループマスタのキャッシュ
        //            _cacheItemGrpMst = _masterFinder.FindAllItemGrpMst();

        //            ikaCalculateArgumentViewModel = new IkaCalculateArgumentViewModel();

        //            //計算準備
        //            StartTrialCalculate(todayOdrInfs.First().HpId, todayOdrInfs.First().PtId, todayOdrInfs.First().SinDate, reception);

        //            //計算処理
        //            if (MainTrialCalculate(todayOdrInfs))
        //            {
        //                // 負担金計算用引数データ取得
        //                if (calcFutan)
        //                {
        //                    (List<Futan.Models.SinKouiCountModel> argSinKouiCountModels,
        //                    List<Futan.Models.SinKouiModel> argSinKouiModels,
        //                    List<Futan.Models.SinKouiDetailModel> argSinKouiDetailModels,
        //                    List<Futan.Models.SinRpInfModel> argSinRpInfModels) =
        //                        GetArgTrialSinData();

        //                    // 負担金計算実行
        //                    FutancalcViewModel FtanCalcVM = new FutancalcViewModel(_tenantProvider, _systemConfigProvider, _emrLogger);
        //                    List<Futan.Models.RaiinInfModel> raiinInfs = new List<Futan.Models.RaiinInfModel>();
        //                    raiinInfs.Add(new Futan.Models.RaiinInfModel(reception.RaiinInf));
        //                    retKaikeiInfModels = FtanCalcVM.TrialFutanCalculation(_common.ptId,
        //                                                                          _common.sinDate,
        //                                                                          _common.raiinNo,
        //                                                                          argSinKouiCountModels,
        //                                                                          argSinKouiModels,
        //                                                                          argSinKouiDetailModels,
        //                                                                          argSinRpInfModels,
        //                                                                          raiinInfs);
        //                    //FtanCalcVM.Dispose();
        //                }

        //                retSinRpInfModels = _common.Sin.SinRpInfs.FindAll(p => p.IsDeleted == DeleteStatus.None && (p.UpdateState == UpdateStateConst.Add || p.UpdateState == UpdateStateConst.None));
        //                retSinKouiModels = _common.Sin.SinKouis.FindAll(p => p.IsDeleted == DeleteStatus.None && (p.UpdateState == UpdateStateConst.Add || p.UpdateState == UpdateStateConst.None));
        //                retSinKouiDetailModels = _common.Sin.SinKouiDetails.FindAll(p => p.IsDeleted == DeleteStatus.None && (p.UpdateState == UpdateStateConst.Add || p.UpdateState == UpdateStateConst.None));
        //                retSinKouiCountModels = _common.Sin.SinKouiCounts.FindAll(p => p.UpdateState == UpdateStateConst.Add || p.UpdateState == UpdateStateConst.None);
        //                retCalcLogModels = _common.calcLogs;

        //                List<long> raiinNos = new List<long> { _common.raiinNo };
        //                SinMeiViewModel sinMeiVM =
        //                    new SinMeiViewModel(
        //                        3, false, _common.hpId, _common.ptId, _common.sinDate, raiinNos,
        //                        retSinRpInfModels, retSinKouiModels, retSinKouiCountModels, retSinKouiDetailModels, retKaikeiInfModels);

        //                retSinMeis = sinMeiVM.SinMei;
        //                //sinMeiVM.DbService?.Dispose();
        //            }

        //            // 点数マスタのキャッシュを受け取り（次回の計算に引き継ぐため）
        //            _cacheTenMst = _common.CacheTenMst;
        //        }
        //    }

        //    return (retSinMeis, retKaikeiInfModels);
        //}

        /// <summary>
        /// 試算準備処理
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        //public void StartTrialCalculate(int hpId, long ptId, int sinDate, ReceptionModel reception)
        //{
        //    const string conFncName = nameof(StartTrialCalculate);

        //    _emrLogger.WriteLogStart( this, conFncName,
        //        String.Format("hpId={0} ptId={1} sinDate={2} ", hpId, ptId, sinDate));

        //    //指定の診療日の来院情報を取得
        //    _raiinInfModels = _raiinInfFinder.FindRaiinInfData(hpId, ptId, sinDate);
        //    _raiinInfModels.RemoveAll(p => p.RaiinNo == reception.RaiinNo);
        //    _raiinInfModels.Add(new RaiinInfModel(reception.RaiinInf, _masterFinder.FindKaMst(hpId, reception.KaId)));

        //    //メンバー変数設定
        //    _hpId = hpId;
        //    _ptId = ptId;
        //    _sinDate = sinDate;

        //    //引数クラス生成

        //    ikaCalculateArgumentViewModel.hpId = _hpId;
        //    ikaCalculateArgumentViewModel.ptId = _ptId;
        //    ikaCalculateArgumentViewModel.sinDate = _sinDate;
        //    ikaCalculateArgumentViewModel.seikyuUp = 0;
        //    ikaCalculateArgumentViewModel.calcMode = CalcModeConst.Trial;
        //    ikaCalculateArgumentViewModel.raiinInfs = _raiinInfModels;
        //    ikaCalculateArgumentViewModel.masterFinder = _masterFinder;
        //    //ikaCalculateArgumentViewModel.commonMstFinder = _commonMstFinder;
        //    ikaCalculateArgumentViewModel.santeiFinder = _santeiFinder;
        //    ikaCalculateArgumentViewModel.odrInfFinder = _odrInfFinder;
        //    ikaCalculateArgumentViewModel.ikaCalculateFinder = _ikaCalculateFinder;
        //    ikaCalculateArgumentViewModel.saveHandler = _saveIkaCalculateCommandHandler;
        //    ikaCalculateArgumentViewModel.raiinInf = _raiinInfModels.FirstOrDefault(p => p.RaiinNo == reception.RaiinNo);
        //    ikaCalculateArgumentViewModel.cacheTenMst = _cacheTenMst;
        //    ikaCalculateArgumentViewModel.cacheDensiSanteiKaisu = _cacheDensiSanteiKaisu;
        //    ikaCalculateArgumentViewModel.cacheItemGrpMst = _cacheItemGrpMst;

        //    _common = new IkaCalculateCommonDataViewModel(ikaCalculateArgumentViewModel, _systemConfigProvider, _emrLogger);

        //    List<long> delSinRaiinNo = _raiinInfModels.FindAll(p => string.Compare(p.SinStartTime, reception.RaiinInf.SinStartTime) > 0).Select(p => p.RaiinNo).ToList();
        //    _common.Sin.DelSinKouiCountByRaiinNo(delSinRaiinNo);

        //    _emrLogger.WriteLogEnd( this, conFncName, "");
        //}

        /// <summary>
        /// 試算のメインの計算ロジック
        /// </summary>
        //public bool MainTrialCalculate(List<TodayOdrInfModel> todayOdrInfs)
        //{
        //    const string conFncName = nameof(MainTrialCalculate);
        //    _emrLogger.WriteLogStart( this, conFncName, "");

        //    bool ret = true;

        //    if (todayOdrInfs != null && todayOdrInfs.Any())
        //    {

        //        // 前準備
        //        int hpId = todayOdrInfs.First().HpId;
        //        long ptId = todayOdrInfs.First().PtId;
        //        int sinDate = todayOdrInfs.First().SinDate;
        //        long raiinNo = todayOdrInfs.First().RaiinNo;

        //        int hokenKbn = 0;
        //        try
        //        {
        //            bool checkOdr = false;

        //            //RaiinInfModel raiinInfModel = _raiinInfFinder.FindRaiinInfData(hpId, ptId, sinDate).Where(p => p.RaiinNo == raiinNo).FirstOrDefault();
        //            RaiinInfModel raiinInfModel = _common.raiinInfs.Find(p => p.RaiinNo == raiinNo);

        //            if (raiinInfModel != null)
        //            {
        //                _common.raiinInf = raiinInfModel;

        //                // オーダーを計算用に変換
        //                _common.CreatOdrCommon(todayOdrInfs);

        //                //if (_common.IsRosai)
        //                //{
        //                //// 労災合成コード
        //                //_common.Odr.MakeRosaiGosei(_common.IsRosai);
        //                //}
        //                // 労災合成コード
        //                _common.Odr.MakeRosaiGosei();

        //                // 保険の種類分ループを回す
        //                hokenKbn = 0;
        //                while (hokenKbn <= 4)
        //                {
        //                    // 引数設定

        //                    _common.hokenKbn = hokenKbn;

        //                    //オーダー存在確認
        //                    if (_common.Odr.ExistOdrDtlHokenSyu())
        //                    {
        //                        if (checkOdr == false)
        //                        {
        //                            // マスタと紐づけのない項目をログ出力する
        //                            _common.CheckOdr();
        //                            checkOdr = true;
        //                        }

        //                        //オーダー情報からワーク情報へ変換
        //                        OdrToWrk();

        //                    }

        //                    hokenKbn++;
        //                }

        //                hokenKbn = 0;
        //                while (hokenKbn <= 4)
        //                {
        //                    _common.hokenKbn = hokenKbn;

        //                    if ((_common.syosai != SyosaiConst.Jihi) &&
        //                        _common.Wrk.wrkSinKouiDetails.Any(p => p.RaiinNo == _common.raiinNo && p.HokenKbn == hokenKbn))
        //                    {
        //                        //背反等調整処理
        //                        AdjustWrk(false);
        //                    }

        //                    hokenKbn++;
        //                }

        //                hokenKbn = 0;
        //                while (hokenKbn <= 4)
        //                {
        //                    _common.hokenKbn = hokenKbn;

        //                    //if (_common.Wrk.wrkSinKouiDetails.Any(p => p.RaiinNo == _common.raiinNo && p.HokenKbn == hokenKbn))
        //                    if (_common.Odr.ExistOdrDtlHokenSyu())
        //                    {
        //                        //ワーク情報から算定情報へ変換
        //                        WrkToSin();
        //                    }

        //                    hokenKbn++;
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            _emrLogger.WriteLogError(this, conFncName, new Exception(string.Format("raiinNo={0} hokenKbn={1}", _common.raiinNo, hokenKbn) + ":" + e.Message));

        //            _common.ClearCalcLog();
        //            _common.AppendCalcLog(9, "医科計算で問題が発生したため、試算できません。");

        //            ret = false;

        //        }

        //    }

        //    _emrLogger.WriteLogEnd( this, conFncName, "");

        //    return ret;

        //}
        /// <summary>
        /// 本日の算定にかかわるデータを削除する
        /// </summary>
        private void ClearData()
        {
            const string conFncName = nameof(ClearData);

            _emrLogger.WriteLogStart( this, conFncName, "");

            // ワーク情報クリア
            _clearIkaCalculateCommandHandler.ClearWrkData(_common.hpId, _common.ptId, _common.sinDate);
            // 算定ログクリア
            _clearIkaCalculateCommandHandler.ClearCalcLog(_common.hpId, _common.ptId, _common.sinDate);
            // 診療情報クリア
            _clearIkaCalculateCommandHandler.ClearSanteiInf
                (
                    _common.hpId, _common.ptId, _common.sinDate
                );

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// オーダーからワークへ変換
        /// </summary>
        private void OdrToWrk()
        {
            const string conFncName = nameof(OdrToWrk);

            _emrLogger.WriteLogStart( this, conFncName, string.Format("RaiinNo:{0} HokenKbn:{1}", _common.raiinInf.RaiinNo, _common.hokenKbn));

            try
            {
                // 初再診
                new IkaCalculateOdrToWrkSyosaiViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                if (_common.syosai != SyosaiConst.Jihi)
                {
                    // 医学管理
                    new IkaCalculateOdrToWrkIgakuViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 在宅
                    new IkaCalculateOdrToWrkZaitakuViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 検査・病理
                    new IkaCalculateOdrToWrkKensaViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 画像
                    new IkaCalculateOdrToWrkGazoViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 投薬
                    new IkaCalculateOdrToWrkTouyakuViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 注射
                    new IkaCalculateOdrToWrkChusyaViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // その他
                    new IkaCalculateOdrToWrkSonotaViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 処置
                    new IkaCalculateOdrToWrkSyotiViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 手術・麻酔
                    new IkaCalculateOdrToWrkSyujyutuViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();

                    // 自費
                    new IkaCalculateOdrToWrkJihiViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();
                }
                else
                {
                    // 医科計算なし
                    new IkaCalculateOdrToWrkNoIkaViewModel(_common, _systemConfigProvider, _emrLogger).Calculate();
                }
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError( this, conFncName, e);
                throw;
            }

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 調整処理
        /// </summary>
        private void AdjustWrk(bool first)
        {
            new IkaCalculateAdjustViewModel(_common, _systemConfigProvider, _emrLogger).Adjust(first);
        }

        /// <summary>
        /// ワーク情報から算定情報へ変換
        /// </summary>
        private void WrkToSin()
        {
            new IkaCalculateWrkToSinViewModel(_common).Calculate();
        }

        public void Dispose()
        {
            _tenantDataContext?.Dispose();
        }
    }
}
