using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Common;
using Helper.Constants;
using EmrCalculateApi.Receipt.DB.Finder;
using EmrCalculateApi.Receipt.Models;
using EmrCalculateApi.Receipt.Constants;
using Domain.Constant;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Interface;
using Infrastructure.Interfaces;
using PostgreDataContext;
using Infrastructure.CommonDB;

namespace EmrCalculateApi.Receipt.ViewModels
{
    class CmtRecordData
    {
        public CmtRecordData(string itemCd, string itemName, string cmtData)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            CmtData = cmtData;
        }

        public string ItemCd { get; set; } = "";
        public string ItemName { get; set; } = "";
        public string CmtData { get; set; } = "";
    }

    class SinMeiDataSet
    {

        public SinMeiDataSet
            (ReceSinRpInfModel sinRp, ReceSinKouiModel sinKoui, ReceSinKouiDetailModel sinDtl, int count,
             int day1, int day2, int day3, int day4, int day5, int day6, int day7, int day8, int day9, int day10, 
             int day11, int day12, int day13, int day14, int day15, int day16, int day17, int day18, int day19, int day20,
             int day21, int day22, int day23, int day24, int day25, int day26, int day27, int day28, int day29, int day30,
             int day31,
             int day1add, int day2add, int day3add, int day4add, int day5add, int day6add, int day7add, int day8add, int day9add, int day10add,
             int day11add, int day12add, int day13add, int day14add, int day15add, int day16add, int day17add, int day18add, int day19add, int day20add,
             int day21add, int day22add, int day23add, int day24add, int day25add, int day26add, int day27add, int day28add, int day29add, int day30add,
             int day31add
             )
        {
            SinRp = sinRp;
            SinKoui = sinKoui;
            SinDtl = sinDtl;
            Count = count;
            Day1 = day1;
            Day2 = day2;
            Day3 = day3;
            Day4 = day4;
            Day5 = day5;
            Day6 = day6;
            Day7 = day7;
            Day8 = day8;
            Day9 = day9;
            Day10 = day10;
            Day11 = day11;
            Day12 = day12;
            Day13 = day13;
            Day14 = day14;
            Day15 = day15;
            Day16 = day16;
            Day17 = day17;
            Day18 = day18;
            Day19 = day19;
            Day20 = day20;
            Day21 = day21;
            Day22 = day22;
            Day23 = day23;
            Day24 = day24;
            Day25 = day25;
            Day26 = day26;
            Day27 = day27;
            Day28 = day28;
            Day29 = day29;
            Day30 = day30;
            Day31 = day31;

            Day1Add = day1add;
            Day2Add = day2add;
            Day3Add = day3add;
            Day4Add = day4add;
            Day5Add = day5add;
            Day6Add = day6add;
            Day7Add = day7add;
            Day8Add = day8add;
            Day9Add = day9add;
            Day10Add = day10add;
            Day11Add = day11add;
            Day12Add = day12add;
            Day13Add = day13add;
            Day14Add = day14add;
            Day15Add = day15add;
            Day16Add = day16add;
            Day17Add = day17add;
            Day18Add = day18add;
            Day19Add = day19add;
            Day20Add = day20add;
            Day21Add = day21add;
            Day22Add = day22add;
            Day23Add = day23add;
            Day24Add = day24add;
            Day25Add = day25add;
            Day26Add = day26add;
            Day27Add = day27add;
            Day28Add = day28add;
            Day29Add = day29add;
            Day30Add = day30add;
            Day31Add = day31add;
        }
        public ReceSinRpInfModel SinRp { get; set; }
        public ReceSinKouiModel SinKoui { get; set; }
        public ReceSinKouiDetailModel SinDtl { get; set; }
        public int Count { get; set; }

        public int Day1 { get; set; }
        public int Day2 { get; set; }
        public int Day3 { get; set; }
        public int Day4 { get; set; }
        public int Day5 { get; set; }
        public int Day6 { get; set; }
        public int Day7 { get; set; }
        public int Day8 { get; set; }
        public int Day9 { get; set; }
        public int Day10 { get; set; }
        public int Day11 { get; set; }
        public int Day12 { get; set; }
        public int Day13 { get; set; }
        public int Day14 { get; set; }
        public int Day15 { get; set; }
        public int Day16 { get; set; }
        public int Day17 { get; set; }
        public int Day18 { get; set; }
        public int Day19 { get; set; }
        public int Day20 { get; set; }
        public int Day21 { get; set; }
        public int Day22 { get; set; }
        public int Day23 { get; set; }
        public int Day24 { get; set; }
        public int Day25 { get; set; }
        public int Day26 { get; set; }
        public int Day27 { get; set; }
        public int Day28 { get; set; }
        public int Day29 { get; set; }
        public int Day30 { get; set; }
        public int Day31 { get; set; }

        public int Day1Add { get; set; }
        public int Day2Add { get; set; }
        public int Day3Add { get; set; }
        public int Day4Add { get; set; }
        public int Day5Add { get; set; }
        public int Day6Add { get; set; }
        public int Day7Add { get; set; }
        public int Day8Add { get; set; }
        public int Day9Add { get; set; }
        public int Day10Add { get; set; }
        public int Day11Add { get; set; }
        public int Day12Add { get; set; }
        public int Day13Add { get; set; }
        public int Day14Add { get; set; }
        public int Day15Add { get; set; }
        public int Day16Add { get; set; }
        public int Day17Add { get; set; }
        public int Day18Add { get; set; }
        public int Day19Add { get; set; }
        public int Day20Add { get; set; }
        public int Day21Add { get; set; }
        public int Day22Add { get; set; }
        public int Day23Add { get; set; }
        public int Day24Add { get; set; }
        public int Day25Add { get; set; }
        public int Day26Add { get; set; }
        public int Day27Add { get; set; }
        public int Day28Add { get; set; }
        public int Day29Add { get; set; }
        public int Day30Add { get; set; }
        public int Day31Add { get; set; }

    }

    public class SinMeiViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private List<SinMeiDataModel> sinMeis = new List<SinMeiDataModel>();

        private MasterFinder _mstFinder;
        //private CommonBase.CommonMasters.DbAccess.MasterFinder _commonMstFinder;
        private SanteiFinder _santeiFinder;
        private PtFinder _ptFinder;
        private ReceMasterFinder _receMasterFinder;

        private int _hpId;
        private long _ptId;
        private int _sinDate;
        private List<long> _raiinNos;

        private int _firstDateOfSinYm;
        private int _lastDateOfSinYm;
        private int _lastDateOfFirstWeek;
        private int _firstDateOfLastWeek;
        private int _firstDateOfFirstWeek;
        private int _lastDateOfLastWeek;

        private int _firstSinDate;
        private int _lastSinDate;

        private int _prefCd;

        private List<PtFutanKbnModel> _ptFutanKbns;
        private List<PtHokenPatternModel> _ptHokenPatternModels;
        private List<ReceFutanKbnModel> _receFutanKbnModels;
        private List<Futan.Models.KaikeiInfModel> _kaikeiInfs;
        private List<Receipt.Models.ReceInfModel> _receInfs;

        private List<SystemGenerationConfModel> _systemGenerationConfs;

        private PtInfModel _ptInf;

        List<ReceSinKouiCountModel> filteredSinKouiCounts;
        List<ReceSinRpInfModel> filteredSinRpInfs;
        List<ReceSinKouiModel> filteredSinKouis;
        List<ReceSinKouiDetailModel> filteredSinDtls;

        /// <summary>
        /// 再診の乳幼児に関わる項目のリスト
        /// </summary>
        List<string> saisinNyuls =
            new List<string>
            {
                ItemCdConst.SaisinNyu,
                ItemCdConst.SaisinNyuJikangai,
                ItemCdConst.SaisinNyuJikangaiToku,
                ItemCdConst.SaisinNyuKyujitu,
                ItemCdConst.SaisinNyuSinya,
                ItemCdConst.SaisinSyouniNyuYakan,
                ItemCdConst.SaisinSyouniNyuKyujitu,
                ItemCdConst.SaisinSyouniNyuSinya
            };
        /// <summary>
        /// 再診関連の項目のリスト
        /// </summary>
        List<string> saisinls =
            new List<string>
            {
                ItemCdConst.Saisin,
                ItemCdConst.SaisinDenwa,
                ItemCdConst.SaisinDojitu,
                ItemCdConst.SaisinDenwaDojitu,
                ItemCdConst.SaisinDenwaKeizoku,
                ItemCdConst.SaisinDenwaDojituKeizoku,
                ItemCdConst.SaisinJouhou,
                ItemCdConst.SaisinJouhouDojitu
            };

        /// <summary>
        /// 請求年月 レセプト用
        /// </summary>
        public int SeikyuYm { get; set; } = 0;
        /// <summary>
        /// 診療日が属する月の初日
        /// </summary>
        private int firstDateOfSinYm
        {
            get { return _firstDateOfSinYm; }
        }
        /// <summary>
        /// 診療日が属する月の末日
        /// </summary>
        private int lastDateOfSinYm
        {
            get
            {
                if (_lastDateOfSinYm <= 0)
                {
                    _lastDateOfSinYm = CIUtil.GetLastDateOfMonth(_sinDate);
                }
                return _lastDateOfSinYm;
            }
        }
        /// <summary>
        /// 診療日が属する月の初週の土曜日
        /// </summary>
        private int lastDateOfFirstWeek
        {
            get
            {
                if (_lastDateOfFirstWeek <= 0)
                {
                    _lastDateOfFirstWeek = CIUtil.GetLastDateOfWeek(firstDateOfSinYm);
                }

                return _lastDateOfFirstWeek;
            }
        }
        /// <summary>
        /// 診療日が属する月の末週の日曜日
        /// </summary>
        private int firstDateOfLastWeek
        {
            get
            {
                if (_firstDateOfLastWeek <= 0)
                {
                    _firstDateOfLastWeek = CIUtil.GetFirstDateOfWeek(lastDateOfSinYm);
                }

                return _firstDateOfLastWeek;
            }
        }
        /// <summary>
        /// 診療日が属する月の初週の日曜日
        /// </summary>
        private int firstDateOfFirstWeek
        {
            get
            {
                if (_firstDateOfFirstWeek <= 0)
                {
                    _firstDateOfFirstWeek = CIUtil.GetFirstDateOfWeek(firstDateOfSinYm);
                }
                return _firstDateOfFirstWeek;
            }
        }
        /// <summary>
        /// 診療日が属する月の末週の土曜日
        /// </summary>
        private int lastDateOfLastWeek
        {
            get
            {
                if (_lastDateOfLastWeek <= 0)
                {
                    _lastDateOfLastWeek = CIUtil.GetLastDateOfWeek(lastDateOfSinYm);
                }
                return _lastDateOfLastWeek;
            }
        }

        //private DBContextFactory _dbService;
        //public DBContextFactory DbService
        //{
        //    get => _dbService;
        //    set
        //    {
        //        if (_dbService != null)
        //        {
        //            __tenantDataContext.Dispose();
        //        }
        //        _dbService = value;
        //        _mstFinder = new MasterFinder(value);
        //        _commonMstFinder = new CommonBase.CommonMasters.DbAccess.MasterFinder(value);
        //        _santeiFinder = new SanteiFinder(value);
        //        _ptFinder = new PtFinder(value);
        //        _receMasterFinder = new ReceMasterFinder(value);
        //    }
        //}
        private ITenantProvider _tenantProvider;
        private TenantDataContext _tenantDataContext;
        private ISystemConfigProvider _systemConfigProvider;
        private IEmrLogger _emrLogger;

        #region constructor
        private void InitFinder(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantProvider = tenantProvider;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();

            _mstFinder = new MasterFinder(_tenantDataContext);
            _santeiFinder = new SanteiFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
            _ptFinder = new PtFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
            _receMasterFinder = new ReceMasterFinder(_tenantDataContext, _systemConfigProvider, _emrLogger);
        }

        /// <summary>
        /// 診療明細データ管理クラス
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNos">
        ///     来院番号
        ///     ※mode=3(領収証)の場合は必須、その他のmodeの場合は不要
        /// </param>
        /// <param name="excludeComment">true-コメント除く</param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinDate, List<long> raiinNos,
            ITenantProvider tenantProvider,
            ISystemConfigProvider systemConfigProvider,
            IEmrLogger emrLogger, bool excludeComment = false)
        {
            const string conFncName = nameof(SinMeiViewModel);

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);
            //DbService = EmrConnectionFactory.StartNew();

            try
            {
                List<SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataNoTrack(hpId, ptId, sinDate, raiinNos);
                List<SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataNoTrack(hpId, ptId, sinDate);

                if(excludeComment)
                {
                    // コメント除く
                    sinKouiDetailModels = sinKouiDetailModels.FindAll(p => p.RecId != "CO");
                }

                List<Futan.Models.KaikeiInfModel> kaikeiInfs = _ptFinder.GetKaikeiInf(hpId, ptId, sinDate, raiinNos);
                List<ReceInfModel> receInfs = null;

                if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard }.Contains(mode)))
                {
                    receInfs = _ptFinder.GetReceInf(hpId, ptId, sinDate, 0, 0);
                }
                PtInfModel ptInf = _ptFinder.FindPtInf(hpId, ptId, sinDate);

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinDate, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, kaikeiInfs, receInfs, ptInf);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }
        /// <summary>
        /// 診療明細データ管理クラス（会計カード用）
        /// </summary>
        /// <param name="mode"></param>
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// <param name="includeOutDrg"></param>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinYm"></param>
        /// <param name="hokenId"></param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinYm, int hokenId,
            ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(SinMeiViewModel);

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);
            //DbService = EmrConnectionFactory.StartNew();

            int sinDate = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
            List<long> raiinNos = new List<long>();

            try
            {
                List<SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataNoTrack(hpId, ptId, sinDate);
                List<Futan.Models.KaikeiInfModel> kaikeiInfs = _ptFinder.GetKaikeiInf(hpId, ptId, sinDate, hokenId);

                List<ReceInfModel> receInfs = null;
                if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard }.Contains(mode)))
                {
                    receInfs = _ptFinder.GetReceInf(hpId, ptId, sinDate, 0, 0);
                }
                PtInfModel ptInf = _ptFinder.FindPtInf(hpId, ptId, sinDate);

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinYm * 100 + 1, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, kaikeiInfs, receInfs, ptInf, hokenId);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }
        /// <summary>
        /// 診療明細データ管理クラス（レセプト用）（算定情報を取得するタイプ）
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <param name="receInf">レセプト情報</param>
        /// <param name = "ptInf">患者情報</param>
        /// <param name = "tokki">特記事項</param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinYm, ReceInfModel receInf, PtInfModel ptInf, string tokki,
            ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(SinMeiViewModel);

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);
            //DbService = EmrConnectionFactory.StartNew();

            int sinDate = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
            List<long> raiinNos = new List<long>();

            try
            {
                List<SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataNoTrack(hpId, ptId, sinDate);

                //List<Futan.Models.KaikeiInfModel> kaikeiInfs = _ptFinder.GetKaikeiInf(_hpId, _ptId, _sinDate, _raiinNos);
                List<ReceInfModel> receInfs = new List<ReceInfModel>();
                receInfs.Add(receInf);
                //PtInfModel ptInf = _ptFinder.FindPtInf(_hpId, _ptId, _sinDate);

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinYm * 100 + 1, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, null, receInfs, ptInf, receInf.HokenId, receInf.HokenId2, tokki);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }
        /// <summary>
        /// 診療明細データ管理クラス（レセチェック用）
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="seikyuYm">請求年月</param>
        /// <param name="sinYm">診療年月</param>
        /// <param name="hokenId">保険ID</param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int seikyuYm, int sinYm, int hokenId,
            ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(SinMeiViewModel);

            //DbService = EmrConnectionFactory.StartNew();

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);

            int sinDate = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
            List<long> raiinNos = new List<long>();

            try
            {
                List<SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataNoTrack(hpId, ptId, sinDate);

                List<ReceInfModel> receInfs = null;
                if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard }.Contains(mode)))
                {
                    receInfs = _ptFinder.GetReceInf(hpId, ptId, sinDate, seikyuYm, hokenId);
                }
                PtInfModel ptInf = _ptFinder.FindPtInf(hpId, ptId, sinDate);

                string tokki = "";
                int hokenId2 = 0;
                if(receInfs != null && receInfs.Any())
                {
                    tokki = receInfs.First().Tokki;
                    hokenId2 = receInfs.First().HokenId2;
                }

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinYm * 100 + 1, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, null, receInfs, ptInf, hokenId, hokenId2, tokki);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }

        /// <summary>
        /// 診療明細データ管理クラス（レセプト用）（算定情報を引数で渡すタイプ）
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinYm">診療日</param>
        /// <param name="receInf">レセプト情報</param>
        /// <param name = "ptInf">患者情報</param>
        /// <param name = "tokki">特記事項</param>
        /// <param name="sinRpInfModels"></param>
        /// <param name="sinKouiModels"></param>
        /// <param name="sinKouiDetailModels"></param>
        /// <param name="sinKouiCountModels"></param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinYm, int seikyuYm, 
            ReceInfModel receInf, List<ReceFutanKbnModel> receFutanKbns, PtInfModel ptInf, string tokki, 
            List<SinRpInfModel> sinRpInfModels, List<SinKouiModel> sinKouiModels, List<SinKouiDetailModel> sinKouiDetailModels, List<SinKouiCountModel> sinKouiCountModels,
            ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(SinMeiViewModel);

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);
            //DbService = EmrConnectionFactory.StartNew();

            SeikyuYm = seikyuYm;

            int sinDate = sinYm * 100 + 31;
            List<long> raiinNos = new List<long>();
            _receFutanKbnModels = receFutanKbns;

            try
            {
                //List<Futan.Models.KaikeiInfModel> kaikeiInfs = _ptFinder.GetKaikeiInf(_hpId, _ptId, _sinDate, _raiinNos);
                List<ReceInfModel> receInfs = new List<ReceInfModel>();
                receInfs.Add(receInf);
                //PtInfModel ptInf = _ptFinder.FindPtInf(_hpId, _ptId, _sinDate);

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinYm * 100 + 1, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, null, receInfs, ptInf, receInf.HokenId, receInf.HokenId2, tokki);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }
        
        /// <summary>
        /// 診療明細データ管理クラス（アフターケア用）（算定情報を取得するタイプ）
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNos">診療日の中で対象となる来院番号</param>
        /// <param name="receInf">レセプト情報</param>
        /// <param name="ptInf">患者情報</param>
        /// <param name="tokki">特記事項</param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinDate, List<long> raiinNos, ReceInfModel receInf, PtInfModel ptInf, string tokki,
            ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(SinMeiViewModel);

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);
            //DbService = EmrConnectionFactory.StartNew();

            try
            {
                List<SinRpInfModel> sinRpInfModels = _santeiFinder.FindSinRpInfDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiModel> sinKouiModels = _santeiFinder.FindSinKouiDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiDetailModel> sinKouiDetailModels = _santeiFinder.FindSinKouiDetailDataNoTrack(hpId, ptId, sinDate);
                List<SinKouiCountModel> sinKouiCountModels = _santeiFinder.FindSinKouiCountDataNoTrack(hpId, ptId, sinDate);

                //List<Futan.Models.KaikeiInfModel> kaikeiInfs = _ptFinder.GetKaikeiInf(_hpId, _ptId, _sinDate, _raiinNos);
                List<ReceInfModel> receInfs = new List<ReceInfModel>();
                receInfs.Add(receInf);
                //PtInfModel ptInf = _ptFinder.FindPtInf(_hpId, _ptId, _sinDate);

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinDate, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, null, receInfs, ptInf, receInf.HokenId, receInf.HokenId2, tokki);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }
        /// <summary>
        /// 診療明細データ管理クラス（アフターケア用）（算定情報を引数で渡すタイプ）
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNos">診療日の中で対象となる来院番号</param>
        /// <param name="receInf">レセプト情報</param>
        /// <param name="ptInf">患者情報</param>
        /// <param name="tokki">特記事項</param>
        /// <param name="sinRpInfModels"></param>
        /// <param name="sinKouiModels"></param>
        /// <param name="sinKouiDetailModels"></param>
        /// <param name="sinKouiCountModels"></param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinDate, List<long> raiinNos, 
            ReceInfModel receInf, PtInfModel ptInf, string tokki, 
            List<SinRpInfModel> sinRpInfModels, List<SinKouiModel> sinKouiModels, List<SinKouiDetailModel> sinKouiDetailModels, List<SinKouiCountModel> sinKouiCountModels,
            ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(SinMeiViewModel);

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);
            //DbService = EmrConnectionFactory.StartNew();

            try
            {
                //List<Futan.Models.KaikeiInfModel> kaikeiInfs = _ptFinder.GetKaikeiInf(_hpId, _ptId, _sinDate, _raiinNos);
                List<ReceInfModel> receInfs = new List<ReceInfModel>();
                receInfs.Add(receInf);
                //PtInfModel ptInf = _ptFinder.FindPtInf(_hpId, _ptId, _sinDate);

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinDate, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, null, receInfs, ptInf, receInf.HokenId, receInf.HokenId2, tokki);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }
        /// <summary>
        /// 診療明細データ管理クラス（試算用）
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNos">
        ///     来院番号
        ///     ※mode=3(領収証)の場合は必須、その他のmodeの場合は不要
        /// </param>
        /// <param name="sinRpInfModels">診療Rp情報</param>
        /// <param name="sinKouiModels">診療行為情報</param>
        /// <param name="sinKouiCountModels">診療行為カウント情報</param>
        /// <param name="sinKouiDetailModels">診療詳細情報</param>
        /// <param name="kaikeiInfs">会計情報リスト</param>
        public SinMeiViewModel(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinDate, List<long> raiinNos,
            List<SinRpInfModel> sinRpInfModels, List<SinKouiModel> sinKouiModels, List<SinKouiCountModel> sinKouiCountModels, List<SinKouiDetailModel> sinKouiDetailModels, 
            List<Futan.Models.KaikeiInfModel> kaikeiInfs,
            ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            const string conFncName = nameof(SinMeiViewModel);

            InitFinder(tenantProvider, systemConfigProvider, emrLogger);
            //DbService = EmrConnectionFactory.StartNew();

            try
            {
                //List<Futan.Models.KaikeiInfModel> kaikeiInfs = _ptFinder.GetKaikeiInf(hpId, ptId, sinDate, raiinNos);
                List<ReceInfModel> receInfs = _ptFinder.GetReceInf(hpId, ptId, sinDate, 0, 0);
                PtInfModel ptInf = _ptFinder.FindPtInf(hpId, ptId, sinDate);

                MakeSinMei(mode, includeOutDrg, hpId, ptId, sinDate, raiinNos, sinRpInfModels, sinKouiModels, sinKouiCountModels, sinKouiDetailModels, kaikeiInfs, receInfs, ptInf);
            }
            catch (Exception e)
            {
                _emrLogger.WriteLogError(this, conFncName, e);
                //throw;
            }
        }
        #endregion

        /// <summary>
        /// 診療明細を作成する
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        ///     4: アフターケア
        ///    11: 紙レセプト点数欄
        ///    12: 労災レセプト点数欄
        ///    13: アフターケア点数欄
        ///    21: 会計カード
        /// </param>
        /// <param name="includeOutDrg">true-院外処方を含める</param>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNos">
        ///     来院番号
        ///     ※mode=3(領収証)の場合は必須、その他のmodeの場合は不要
        /// </param>
        /// <param name="sinRpInfModels">診療Rp情報</param>
        /// <param name="sinKouiModels">診療行為情報</param>
        /// <param name="sinKouiCountModels">診療行為カウント情報</param>
        /// <param name="sinKouiDetailModels">診療詳細情報</param>
        /// <param name="kaikeiInfs">会計情報</param>
        /// <param name="receInfs">レセプト情報 レセプト関連のmodeの場合は必須</param>
        /// <param name="ptInf">患者情報</param>
        /// <param name="hokenId">保険ID</param>
        /// <param name="hokenId2">保険ID2</param>
        /// <param name="tokki">特記事項コード</param>
        private void MakeSinMei(
            int mode, bool includeOutDrg, int hpId, long ptId, int sinDate, List<long> raiinNos,
            List<SinRpInfModel> sinRpInfModels, List<SinKouiModel> sinKouiModels, List<SinKouiCountModel> sinKouiCountModels, List<SinKouiDetailModel> sinKouiDetailModels,
            List<Futan.Models.KaikeiInfModel> kaikeiInfs, List<ReceInfModel> receInfs, PtInfModel ptInf,
            int hokenId = 0, int hokenId2 = 0, string tokki = "")
        {
            const string conFncName = nameof(MakeSinMei);
            _emrLogger.WriteLogStart( this, conFncName, string.Format("ptId:{0} sinDate:{1}", ptId, sinDate));

            if (sinRpInfModels.Any())
            {
                _prefCd = _mstFinder.GetPrefCd(hpId, sinDate);

                _hpId = hpId;
                _ptId = ptId;

                _sinDate = sinDate;
                _raiinNos = raiinNos;

                _firstDateOfSinYm = _sinDate / 100 * 100 + 1;
                _lastDateOfSinYm = 0;
                _lastDateOfFirstWeek = 0;
                _firstDateOfLastWeek = 0;

                //_emrLogger.WriteLogMsg( this, conFncName, "pthokenpattern");
                _ptHokenPatternModels = _ptFinder.FindPtHokenPattern(_hpId, _ptId, _sinDate);
                //_kaikeiInfs = _ptFinder.GetKaikeiInf(_hpId, _ptId, _sinDate, _raiinNos);
                //_receInfs = _ptFinder.GetReceInf(_hpId, _ptId, _sinDate);
                //_ptInf = _ptFinder.FindPtInf(_hpId, _ptId, _sinDate);
                _kaikeiInfs = kaikeiInfs;
                _receInfs = receInfs;

                _ptInf = ptInf;

                // 対象を絞り込み、新しいリストを生成、元のリストに影響がないようにする
                //_emrLogger.WriteLogMsg( this, conFncName, "sin filter");
                filteredSinKouiCounts = FilterSinKouiCount(mode, raiinNos, sinKouiCountModels);
                filteredSinRpInfs = FilterSinRpInf(mode, sinRpInfModels);
                filteredSinKouis = FilterSinKoui(mode, includeOutDrg, sinRpInfModels, sinKouiModels, filteredSinKouiCounts, hokenId, hokenId2);
                filteredSinDtls = FilterSinKouiDetail(mode, sinKouiDetailModels);

                List<ReceCmtModel> receCmts = null;
                //List<PtHokenInfModel> ptHokens = null;

                // 保険番号の確定
                if (hokenId <= 0)
                {
                    for (int i = 0; i < sinKouiModels.Count(); i++)
                    {
                        if (sinKouiModels[i].HokenId > 0)
                        {
                            hokenId = sinKouiModels[i].HokenId;
                            break;
                        }
                    }
                }

                if(new int[] { SinMeiMode.Receden, SinMeiMode.PaperRece, SinMeiMode.ReceCheck, SinMeiMode.AfterCare, SinMeiMode.RecedenAfter}.Contains(mode))
                {
                    // レセコメント、保険情報の取得
                    //_emrLogger.WriteLogMsg( this, conFncName, "rececmt hokeninf");
                    receCmts = _ptFinder.FindReceCmt(_hpId, _ptId, _sinDate / 100, hokenId, hokenId2);
                    //ptHokens = _ptFinder.FindPtHokenInf(_hpId, _ptId, hokenId);
                }

                //_emrLogger.WriteLogMsg( this, conFncName, "edit");
                // 算定情報を加工　-------------------------------------------------
                // 実施日列挙ダミーの数量を0に設定しておく
                List<ReceSinKouiDetailModel> jissibis =
                filteredSinDtls.FindAll(p =>
                    p.ItemCd == ItemCdConst.CommentJissiRekkyoZengoDummy);
                foreach (ReceSinKouiDetailModel jissibi in jissibis)
                {
                    jissibi.Suryo = 0;
                }

                // 在がん医総をまとめる
                //_emrLogger.WriteLogMsg( this, conFncName, "edit zaigan");
                if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu }.Contains(mode)))
                {
                    // 領収証以外の場合、在がん医総等、週単位計算項目を調整項目と合算する
                    EditZaiWeek(ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinKouiCounts, ref filteredSinDtls, sinKouiDetailModels);
                }

                //// 注射行為まとめ    ※EditSameRpNoSIに統合
                //if (mode != SinMeiMode.Receden)
                //{
                //    // 電算以外の場合、静脈注射、皮下筋肉内注射行為は手技薬剤特材をひとつのRpにまとめる
                //    EditCyusya(ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinDtls);
                //}

                // 検査まるめ
                //_emrLogger.WriteLogMsg( this, conFncName, "edit kensa");
                if (!(new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter}).Contains(mode))
                {
                    // 検査まるめ項目の項目名を連結する
                    EditKensaMarume(ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinDtls);
                }

                // 検査まるめ分点
                if (new int[] {
                    SinMeiMode.Receden , SinMeiMode.Kaikei, SinMeiMode.AccountingCard, SinMeiMode.Ryosyu, SinMeiMode.ReceCheck
                }.Contains(mode))
                {
                    EditKensaMarumeBunten(ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinDtls, mode);
                }
                else
                {
                    EditKensaMarumeBuntenRece(ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinDtls, mode);
                }

                // 同一Rp内SI行為まとめ
                //_emrLogger.WriteLogMsg( this, conFncName, "edit si");
                //if (mode != SinMeiMode.Receden && mode != SinMeiMode.ReceTensuRousai && mode != SinMeiMode.ReceTensuAfter)
                if (!(new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter }).Contains(mode))
                {
                    // 電算以外の場合、同一Rp番号のSIをひとつのRpにまとめる
                    EditSameRpNoSI(ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinDtls);
                }

                //if (new int[] { SinMeiMode.Receden, SinMeiMode.AccountingCard, SinMeiMode.ReceCheck, SinMeiMode.Kaikei, SinMeiMode.Ryosyu }.Contains(mode))
                if ((new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter }).Contains(mode))
                {
                    // 電算の場合、再診のまるめ
                    //_emrLogger.WriteLogMsg( this, conFncName, "edit saisin");
                    EditSyosaisinMarume(mode, ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinDtls);
                }


                // 同一内容のRpをまとめる
                // （例えば、レセ非表示のコメント有無で分割されただけのRpがあったら、レセプトではまとめて出したい）
                if (_systemConfigProvider.GetSameRpMerge() == 0)
                {
                    //_emrLogger.WriteLogMsg( this, conFncName, "edit si");
                    //if (mode != SinMeiMode.Receden && mode != SinMeiMode.ReceTensuRousai && mode != SinMeiMode.ReceTensuAfter)
                    if ((new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter, SinMeiMode.PaperRece, SinMeiMode.ReceCheck }).Contains(mode))
                    {
                        EditSameRpKouiData(ref filteredSinRpInfs, ref filteredSinKouis, ref filteredSinDtls);
                    }
                }

                // 結合してレセプト情報のもとになるデータを取得
                //_emrLogger.WriteLogMsg( this, conFncName, "join");
                var _join = (
                    from sinRpInf in filteredSinRpInfs
                    join sinKoui in filteredSinKouis on
                        new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.RpNo } equals
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.RpNo }
                    join sinDtl in filteredSinDtls on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.RpNo, sinKoui.SeqNo } equals
                        new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo }
                    select new
                    {
                        sinRpInf,
                        sinKoui,
                        sinDtl,
                        Count = sinKoui.Count,
                        Day1 = sinKoui.Day1,
                        Day2 = sinKoui.Day2,
                        Day3 = sinKoui.Day3,
                        Day4 = sinKoui.Day4,
                        Day5 = sinKoui.Day5,
                        Day6 = sinKoui.Day6,
                        Day7 = sinKoui.Day7,
                        Day8 = sinKoui.Day8,
                        Day9 = sinKoui.Day9,
                        Day10 = sinKoui.Day10,
                        Day11 = sinKoui.Day11,
                        Day12 = sinKoui.Day12,
                        Day13 = sinKoui.Day13,
                        Day14 = sinKoui.Day14,
                        Day15 = sinKoui.Day15,
                        Day16 = sinKoui.Day16,
                        Day17 = sinKoui.Day17,
                        Day18 = sinKoui.Day18,
                        Day19 = sinKoui.Day19,
                        Day20 = sinKoui.Day20,
                        Day21 = sinKoui.Day21,
                        Day22 = sinKoui.Day22,
                        Day23 = sinKoui.Day23,
                        Day24 = sinKoui.Day24,
                        Day25 = sinKoui.Day25,
                        Day26 = sinKoui.Day26,
                        Day27 = sinKoui.Day27,
                        Day28 = sinKoui.Day28,
                        Day29 = sinKoui.Day29,
                        Day30 = sinKoui.Day30,
                        Day31 = sinKoui.Day31,
                        Day1Add = sinKoui.Day1Add,
                        Day2Add = sinKoui.Day2Add,
                        Day3Add = sinKoui.Day3Add,
                        Day4Add = sinKoui.Day4Add,
                        Day5Add = sinKoui.Day5Add,
                        Day6Add = sinKoui.Day6Add,
                        Day7Add = sinKoui.Day7Add,
                        Day8Add = sinKoui.Day8Add,
                        Day9Add = sinKoui.Day9Add,
                        Day10Add = sinKoui.Day10Add,
                        Day11Add = sinKoui.Day11Add,
                        Day12Add = sinKoui.Day12Add,
                        Day13Add = sinKoui.Day13Add,
                        Day14Add = sinKoui.Day14Add,
                        Day15Add = sinKoui.Day15Add,
                        Day16Add = sinKoui.Day16Add,
                        Day17Add = sinKoui.Day17Add,
                        Day18Add = sinKoui.Day18Add,
                        Day19Add = sinKoui.Day19Add,
                        Day20Add = sinKoui.Day20Add,
                        Day21Add = sinKoui.Day21Add,
                        Day22Add = sinKoui.Day22Add,
                        Day23Add = sinKoui.Day23Add,
                        Day24Add = sinKoui.Day24Add,
                        Day25Add = sinKoui.Day25Add,
                        Day26Add = sinKoui.Day26Add,
                        Day27Add = sinKoui.Day27Add,
                        Day28Add = sinKoui.Day28Add,
                        Day29Add = sinKoui.Day29Add,
                        Day30Add = sinKoui.Day30Add,
                        Day31Add = sinKoui.Day31Add
                    }
                    );

                // 領収証、アフターケア、会計カードモードの場合、行為カウント情報も結合する
                if (new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AfterCare, SinMeiMode.ReceTensuAfter, SinMeiMode.AccountingCard, SinMeiMode.RecedenAfter }.Contains(mode))
                {
                    var groupSinKouiCount = (
                        from sinKouiCount in filteredSinKouiCounts
                        group sinKouiCount by
                            new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } into sinKouiGroups
                        //from sinKouiGroup in sinKouiGroups.DefaultIfEmpty()
                        select new
                        {
                            sinKouiGroups.Key.HpId,
                            sinKouiGroups.Key.PtId,
                            sinKouiGroups.Key.SinYm,
                            sinKouiGroups.Key.RpNo,
                            sinKouiGroups.Key.SeqNo,
                            Count = sinKouiGroups.Sum(p => p.Count),
                            AdjCount = sinKouiGroups.Sum(p => p.AdjCount)
                        }


                        );
                    var groupSinKouiDayCount = (
                        from sinKouiCount in filteredSinKouiCounts
                        group sinKouiCount by
                            new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.SinDay, sinKouiCount.RpNo, sinKouiCount.SeqNo } into sinKouiGroups
                        //from sinKouiGroup in sinKouiGroups.DefaultIfEmpty()
                        select new
                        {
                            sinKouiGroups.Key.HpId,
                            sinKouiGroups.Key.PtId,
                            sinKouiGroups.Key.SinYm,
                            sinKouiGroups.Key.SinDay,
                            sinKouiGroups.Key.RpNo,
                            sinKouiGroups.Key.SeqNo,
                            Count = sinKouiGroups.Sum(p => p.Count),
                            AdjCount = sinKouiGroups.Sum(p=>p.AdjCount)
                        }


                        );
                    if (mode == SinMeiMode.AccountingCard)
                    {
                        _join = (
                        from j in _join
                        join sinKouiCount in groupSinKouiCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo } equals
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                        join sinKouiDayCount1 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 1 } equals
                        new { sinKouiDayCount1.HpId, sinKouiDayCount1.PtId, sinKouiDayCount1.RpNo, sinKouiDayCount1.SeqNo, sinKouiDayCount1.SinDay } into Day1Joins
                        from Day1Join in Day1Joins.DefaultIfEmpty()
                        join sinKouiDayCount2 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 2 } equals
                        new { sinKouiDayCount2.HpId, sinKouiDayCount2.PtId, sinKouiDayCount2.RpNo, sinKouiDayCount2.SeqNo, sinKouiDayCount2.SinDay } into Day2Joins
                        from Day2Join in Day2Joins.DefaultIfEmpty()
                        join sinKouiDayCount3 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 3 } equals
                        new { sinKouiDayCount3.HpId, sinKouiDayCount3.PtId, sinKouiDayCount3.RpNo, sinKouiDayCount3.SeqNo, sinKouiDayCount3.SinDay } into Day3Joins
                        from Day3Join in Day3Joins.DefaultIfEmpty()
                        join sinKouiDayCount4 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 4 } equals
                        new { sinKouiDayCount4.HpId, sinKouiDayCount4.PtId, sinKouiDayCount4.RpNo, sinKouiDayCount4.SeqNo, sinKouiDayCount4.SinDay } into Day4Joins
                        from Day4Join in Day4Joins.DefaultIfEmpty()
                        join sinKouiDayCount5 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 5 } equals
                        new { sinKouiDayCount5.HpId, sinKouiDayCount5.PtId, sinKouiDayCount5.RpNo, sinKouiDayCount5.SeqNo, sinKouiDayCount5.SinDay } into Day5Joins
                        from Day5Join in Day5Joins.DefaultIfEmpty()
                        join sinKouiDayCount6 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 6 } equals
                        new { sinKouiDayCount6.HpId, sinKouiDayCount6.PtId, sinKouiDayCount6.RpNo, sinKouiDayCount6.SeqNo, sinKouiDayCount6.SinDay } into Day6Joins
                        from Day6Join in Day6Joins.DefaultIfEmpty()
                        join sinKouiDayCount7 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 7 } equals
                        new { sinKouiDayCount7.HpId, sinKouiDayCount7.PtId, sinKouiDayCount7.RpNo, sinKouiDayCount7.SeqNo, sinKouiDayCount7.SinDay } into Day7Joins
                        from Day7Join in Day7Joins.DefaultIfEmpty()
                        join sinKouiDayCount8 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 8 } equals
                        new { sinKouiDayCount8.HpId, sinKouiDayCount8.PtId, sinKouiDayCount8.RpNo, sinKouiDayCount8.SeqNo, sinKouiDayCount8.SinDay } into Day8Joins
                        from Day8Join in Day8Joins.DefaultIfEmpty()
                        join sinKouiDayCount9 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 9 } equals
                        new { sinKouiDayCount9.HpId, sinKouiDayCount9.PtId, sinKouiDayCount9.RpNo, sinKouiDayCount9.SeqNo, sinKouiDayCount9.SinDay } into Day9Joins
                        from Day9Join in Day9Joins.DefaultIfEmpty()
                        join sinKouiDayCount10 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 10 } equals
                        new { sinKouiDayCount10.HpId, sinKouiDayCount10.PtId, sinKouiDayCount10.RpNo, sinKouiDayCount10.SeqNo, sinKouiDayCount10.SinDay } into Day10Joins
                        from Day10Join in Day10Joins.DefaultIfEmpty()
                        join sinKouiDayCount11 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 11 } equals
                        new { sinKouiDayCount11.HpId, sinKouiDayCount11.PtId, sinKouiDayCount11.RpNo, sinKouiDayCount11.SeqNo, sinKouiDayCount11.SinDay } into Day11Joins
                        from Day11Join in Day11Joins.DefaultIfEmpty()
                        join sinKouiDayCount12 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 12 } equals
                        new { sinKouiDayCount12.HpId, sinKouiDayCount12.PtId, sinKouiDayCount12.RpNo, sinKouiDayCount12.SeqNo, sinKouiDayCount12.SinDay } into Day12Joins
                        from Day12Join in Day12Joins.DefaultIfEmpty()
                        join sinKouiDayCount13 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 13 } equals
                        new { sinKouiDayCount13.HpId, sinKouiDayCount13.PtId, sinKouiDayCount13.RpNo, sinKouiDayCount13.SeqNo, sinKouiDayCount13.SinDay } into Day13Joins
                        from Day13Join in Day13Joins.DefaultIfEmpty()
                        join sinKouiDayCount14 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 14 } equals
                        new { sinKouiDayCount14.HpId, sinKouiDayCount14.PtId, sinKouiDayCount14.RpNo, sinKouiDayCount14.SeqNo, sinKouiDayCount14.SinDay } into Day14Joins
                        from Day14Join in Day14Joins.DefaultIfEmpty()
                        join sinKouiDayCount15 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 15 } equals
                        new { sinKouiDayCount15.HpId, sinKouiDayCount15.PtId, sinKouiDayCount15.RpNo, sinKouiDayCount15.SeqNo, sinKouiDayCount15.SinDay } into Day15Joins
                        from Day15Join in Day15Joins.DefaultIfEmpty()
                        join sinKouiDayCount16 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 16 } equals
                        new { sinKouiDayCount16.HpId, sinKouiDayCount16.PtId, sinKouiDayCount16.RpNo, sinKouiDayCount16.SeqNo, sinKouiDayCount16.SinDay } into Day16Joins
                        from Day16Join in Day16Joins.DefaultIfEmpty()
                        join sinKouiDayCount17 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 17 } equals
                        new { sinKouiDayCount17.HpId, sinKouiDayCount17.PtId, sinKouiDayCount17.RpNo, sinKouiDayCount17.SeqNo, sinKouiDayCount17.SinDay } into Day17Joins
                        from Day17Join in Day17Joins.DefaultIfEmpty()
                        join sinKouiDayCount18 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 18 } equals
                        new { sinKouiDayCount18.HpId, sinKouiDayCount18.PtId, sinKouiDayCount18.RpNo, sinKouiDayCount18.SeqNo, sinKouiDayCount18.SinDay } into Day18Joins
                        from Day18Join in Day18Joins.DefaultIfEmpty()
                        join sinKouiDayCount19 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 19 } equals
                        new { sinKouiDayCount19.HpId, sinKouiDayCount19.PtId, sinKouiDayCount19.RpNo, sinKouiDayCount19.SeqNo, sinKouiDayCount19.SinDay } into Day19Joins
                        from Day19Join in Day19Joins.DefaultIfEmpty()
                        join sinKouiDayCount20 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 20 } equals
                        new { sinKouiDayCount20.HpId, sinKouiDayCount20.PtId, sinKouiDayCount20.RpNo, sinKouiDayCount20.SeqNo, sinKouiDayCount20.SinDay } into Day20Joins
                        from Day20Join in Day20Joins.DefaultIfEmpty()
                        join sinKouiDayCount21 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 21 } equals
                        new { sinKouiDayCount21.HpId, sinKouiDayCount21.PtId, sinKouiDayCount21.RpNo, sinKouiDayCount21.SeqNo, sinKouiDayCount21.SinDay } into Day21Joins
                        from Day21Join in Day21Joins.DefaultIfEmpty()
                        join sinKouiDayCount22 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 22 } equals
                        new { sinKouiDayCount22.HpId, sinKouiDayCount22.PtId, sinKouiDayCount22.RpNo, sinKouiDayCount22.SeqNo, sinKouiDayCount22.SinDay } into Day22Joins
                        from Day22Join in Day22Joins.DefaultIfEmpty()
                        join sinKouiDayCount23 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 23 } equals
                        new { sinKouiDayCount23.HpId, sinKouiDayCount23.PtId, sinKouiDayCount23.RpNo, sinKouiDayCount23.SeqNo, sinKouiDayCount23.SinDay } into Day23Joins
                        from Day23Join in Day23Joins.DefaultIfEmpty()
                        join sinKouiDayCount24 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 24 } equals
                        new { sinKouiDayCount24.HpId, sinKouiDayCount24.PtId, sinKouiDayCount24.RpNo, sinKouiDayCount24.SeqNo, sinKouiDayCount24.SinDay } into Day24Joins
                        from Day24Join in Day24Joins.DefaultIfEmpty()
                        join sinKouiDayCount25 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 25 } equals
                        new { sinKouiDayCount25.HpId, sinKouiDayCount25.PtId, sinKouiDayCount25.RpNo, sinKouiDayCount25.SeqNo, sinKouiDayCount25.SinDay } into Day25Joins
                        from Day25Join in Day25Joins.DefaultIfEmpty()
                        join sinKouiDayCount26 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 26 } equals
                        new { sinKouiDayCount26.HpId, sinKouiDayCount26.PtId, sinKouiDayCount26.RpNo, sinKouiDayCount26.SeqNo, sinKouiDayCount26.SinDay } into Day26Joins
                        from Day26Join in Day26Joins.DefaultIfEmpty()
                        join sinKouiDayCount27 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 27 } equals
                        new { sinKouiDayCount27.HpId, sinKouiDayCount27.PtId, sinKouiDayCount27.RpNo, sinKouiDayCount27.SeqNo, sinKouiDayCount27.SinDay } into Day27Joins
                        from Day27Join in Day27Joins.DefaultIfEmpty()
                        join sinKouiDayCount28 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 28 } equals
                        new { sinKouiDayCount28.HpId, sinKouiDayCount28.PtId, sinKouiDayCount28.RpNo, sinKouiDayCount28.SeqNo, sinKouiDayCount28.SinDay } into Day28Joins
                        from Day28Join in Day28Joins.DefaultIfEmpty()
                        join sinKouiDayCount29 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 29 } equals
                        new { sinKouiDayCount29.HpId, sinKouiDayCount29.PtId, sinKouiDayCount29.RpNo, sinKouiDayCount29.SeqNo, sinKouiDayCount29.SinDay } into Day29Joins
                        from Day29Join in Day29Joins.DefaultIfEmpty()
                        join sinKouiDayCount30 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 30 } equals
                        new { sinKouiDayCount30.HpId, sinKouiDayCount30.PtId, sinKouiDayCount30.RpNo, sinKouiDayCount30.SeqNo, sinKouiDayCount30.SinDay } into Day30Joins
                        from Day30Join in Day30Joins.DefaultIfEmpty()
                        join sinKouiDayCount31 in groupSinKouiDayCount on
                        new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo, SinDay = 31 } equals
                        new { sinKouiDayCount31.HpId, sinKouiDayCount31.PtId, sinKouiDayCount31.RpNo, sinKouiDayCount31.SeqNo, sinKouiDayCount31.SinDay } into Day31Joins
                        from Day31Join in Day31Joins.DefaultIfEmpty()
                        select new
                        {
                            j.sinRpInf,
                            j.sinKoui,
                            j.sinDtl,
                            Count = sinKouiCount.AdjCount,
                            Day1 = Day1Join != null ? Day1Join.Count : 0, Day2 = Day2Join != null ? Day2Join.Count : 0, Day3 = Day3Join != null ? Day3Join.Count : 0,
                            Day4 = Day4Join != null ? Day4Join.Count : 0, Day5 = Day5Join != null ? Day5Join.Count : 0, Day6 = Day6Join != null ? Day6Join.Count : 0,
                            Day7 = Day7Join != null ? Day7Join.Count : 0, Day8 = Day8Join != null ? Day8Join.Count : 0, Day9 = Day9Join != null ? Day9Join.Count : 0,
                            Day10 = Day10Join != null ? Day10Join.Count : 0, Day11 = Day11Join != null ? Day11Join.Count : 0, Day12 = Day12Join != null ? Day12Join.Count : 0,
                            Day13 = Day13Join != null ? Day13Join.Count : 0, Day14 = Day14Join != null ? Day14Join.Count : 0, Day15 = Day15Join != null ? Day15Join.Count : 0,
                            Day16 = Day16Join != null ? Day16Join.Count : 0, Day17 = Day17Join != null ? Day17Join.Count : 0, Day18 = Day18Join != null ? Day18Join.Count : 0,
                            Day19 = Day19Join != null ? Day19Join.Count : 0, Day20 = Day20Join != null ? Day20Join.Count : 0, Day21 = Day21Join != null ? Day21Join.Count : 0,
                            Day22 = Day22Join != null ? Day22Join.Count : 0, Day23 = Day23Join != null ? Day23Join.Count : 0, Day24 = Day24Join != null ? Day24Join.Count : 0,
                            Day25 = Day25Join != null ? Day25Join.Count : 0, Day26 = Day26Join != null ? Day26Join.Count : 0, Day27 = Day27Join != null ? Day27Join.Count : 0,
                            Day28 = Day28Join != null ? Day28Join.Count : 0, Day29 = Day29Join != null ? Day29Join.Count : 0, Day30 = Day30Join != null ? Day30Join.Count : 0,
                            Day31 = Day31Join != null ? Day31Join.Count : 0,
                            Day1Add = 0, Day2Add = 0, Day3Add = 0, Day4Add = 0, Day5Add = 0, Day6Add = 0, Day7Add = 0, Day8Add = 0, Day9Add = 0, Day10Add = 0,
                            Day11Add = 0, Day12Add = 0, Day13Add = 0, Day14Add = 0, Day15Add = 0, Day16Add = 0, Day17Add = 0, Day18Add = 0, Day19Add = 0, Day20Add = 0,
                            Day21Add = 0, Day22Add = 0, Day23Add = 0, Day24Add = 0, Day25Add = 0, Day26Add = 0, Day27Add = 0, Day28Add = 0, Day29Add = 0, Day30Add = 0,
                            Day31Add = 0
                        }
                    );
                    }
                    else
                    {
                        _join = (
                            from j in _join
                            join sinKouiCount in groupSinKouiCount on
                            new { j.sinKoui.HpId, j.sinKoui.PtId, j.sinKoui.RpNo, j.sinKoui.SeqNo } equals
                            new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                            select new
                            {
                                j.sinRpInf,
                                j.sinKoui,
                                j.sinDtl,
                                sinKouiCount.Count,
                                Day1 = 0, Day2 = 0, Day3 = 0, Day4 = 0, Day5 = 0, Day6 = 0, Day7 = 0, Day8 = 0, Day9 = 0, Day10 = 0,
                                Day11 = 0, Day12 = 0, Day13 = 0, Day14 = 0, Day15 = 0, Day16 = 0, Day17 = 0, Day18 = 0, Day19 = 0, Day20 = 0,
                                Day21 = 0, Day22 = 0, Day23 = 0, Day24 = 0, Day25 = 0, Day26 = 0, Day27 = 0, Day28 = 0, Day29 = 0, Day30 = 0,
                                Day31 = 0,
                                Day1Add = 0, Day2Add = 0, Day3Add = 0, Day4Add = 0, Day5Add = 0, Day6Add = 0, Day7Add = 0, Day8Add = 0, Day9Add = 0, Day10Add = 0,
                                Day11Add = 0, Day12Add = 0, Day13Add = 0, Day14Add = 0, Day15Add = 0, Day16Add = 0, Day17Add = 0, Day18Add = 0, Day19Add = 0, Day20Add = 0,
                                Day21Add = 0, Day22Add = 0, Day23Add = 0, Day24Add = 0, Day25Add = 0, Day26Add = 0, Day27Add = 0, Day28Add = 0, Day29Add = 0, Day30Add = 0,
                                Day31Add = 0
                            }
                        );
                    }
                }

                //List<(ReceSinRpInfModel sinRp, ReceSinKouiModel sinKoui, ReceSinKouiDetailModel sinDtl, int Count)> sinData =
                //    new List<(ReceSinRpInfModel, ReceSinKouiModel, ReceSinKouiDetailModel, int)>();
                List<SinMeiDataSet> sinData = new List<SinMeiDataSet>();

                var entities = _join.AsEnumerable().ToList();

                entities?.ForEach(
                    data =>
                    {
                        sinData.Add(new SinMeiDataSet(
                            data.sinRpInf,
                            data.sinKoui,
                            data.sinDtl,
                            data.Count,
                            data.Day1,
                            data.Day2,
                            data.Day3,
                            data.Day4,
                            data.Day5,
                            data.Day6,
                            data.Day7,
                            data.Day8,
                            data.Day9,
                            data.Day10,
                            data.Day11,
                            data.Day12,
                            data.Day13,
                            data.Day14,
                            data.Day15,
                            data.Day16,
                            data.Day17,
                            data.Day18,
                            data.Day19,
                            data.Day20,
                            data.Day21,
                            data.Day22,
                            data.Day23,
                            data.Day24,
                            data.Day25,
                            data.Day26,
                            data.Day27,
                            data.Day28,
                            data.Day29,
                            data.Day30,
                            data.Day31,
                            data.Day1Add,
                            data.Day2Add,
                            data.Day3Add,
                            data.Day4Add,
                            data.Day5Add,
                            data.Day6Add,
                            data.Day7Add,
                            data.Day8Add,
                            data.Day9Add,
                            data.Day10Add,
                            data.Day11Add,
                            data.Day12Add,
                            data.Day13Add,
                            data.Day14Add,
                            data.Day15Add,
                            data.Day16Add,
                            data.Day17Add,
                            data.Day18Add,
                            data.Day19Add,
                            data.Day20Add,
                            data.Day21Add,
                            data.Day22Add,
                            data.Day23Add,
                            data.Day24Add,
                            data.Day25Add,
                            data.Day26Add,
                            data.Day27Add,
                            data.Day28Add,
                            data.Day29Add,
                            data.Day30Add,
                            data.Day31Add
                            )
                        );
                    }
                );

                // ソート
                //_emrLogger.WriteLogMsg( this, conFncName, "sort");
                if (new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter }.Contains(mode))
                {
                    // レセプト電算の場合、コメントが先頭にくるように調整
                    //sinData =
                    //    sinData
                    //    .OrderBy(p => p.sinRp.SinId)
                    //    .ThenBy(p => p.sinRp.CdNo)
                    //    .ThenBy(p => p.sinRp.FirstDay)
                    //    .ThenBy(p => p.sinRp.RpNo)
                    //    //.ThenBy(p => p.sinDtl.RecIdNo)
                    //    .ThenBy(p => p.sinKoui.SeqNo)
                    //    .ThenBy(p => p.sinDtl.RecIdNo)
                    //    .ThenBy(p => p.sinDtl.RowNo)
                    //    .ToList();
                    //sinData =
                    //    sinData
                    //    .OrderBy(p => p.SinRp.SinId)
                    //    .ThenBy(p => p.SinRp.CdNo)
                    //    .ThenBy(p => p.SinRp.FirstDay)
                    //    .ThenBy(p => p.SinRp.RpNo)                        
                    //    .ThenBy(p => p.SinKoui.RecIdNo)
                    //    .ThenBy(p => p.SinKoui.SeqNo)   // test
                    //    .ThenBy(p => p.SinDtl.RecIdNo)
                    //    .ThenBy(p => p.SinKoui.SeqNo)
                    //    .ThenBy(p => p.SinDtl.RowNo)
                    //    .ToList();
                    //sinData =
                    //    sinData
                    //    .OrderBy(p => p.SinRp.SinId)
                    //    .ThenBy(p => p.SinRp.CdNo)
                    //    .ThenBy(p => p.SinRp.FirstDay)
                    //    .ThenBy(p => p.SinRp.RpNo)
                    //    .ThenBy(p => p.SinDtl.RecId == "CO" ? 0 : p.SinKoui.RecIdNo)
                    //    .ThenBy(p => p.SinDtl.RecId == "CO" ? 0 : p.SinKoui.SeqNo)   // test
                    //    .ThenBy(p => p.SinDtl.RecIdNo)
                    //    .ThenBy(p => p.SinKoui.SeqNo)
                    //    .ThenBy(p => p.SinDtl.RowNo)
                    //    .ToList();

                    // 手術と麻酔が同一Rpにあったときにうまくいかない
                    // 両方の項目のCO、実施日などが先頭にまとまって来てしまい、どれがどっちのコメントかわからない
                    // 点数記録までがひと塊で、その中で先頭か末尾にCOがあればよいみたい
                    // 診療区分の記録単位ではないようなので、この並びで記録してみる
                    sinData =
                        sinData
                        .OrderBy(p => p.SinRp.SinId)
                        .ThenBy(p => p.SinRp.CdNo)
                        //.ThenBy(p => p.SinRp.SortKouiData)
                        .ThenBy(p => p.SinRp.FirstDay)
                        .ThenBy(p => p.SinRp.RpNo)
                        .ThenBy(p => p.SinKoui.RecIdNo)
                        .ThenBy(p => p.SinKoui.SeqNo)
                        //.ThenBy(p => p.SinDtl.RecId == "CO" ? 0 : p.SinKoui.RecIdNo)
                        //.ThenBy(p => p.SinDtl.RecId == "CO" ? 0 : p.SinKoui.SeqNo)   // test
                        .ThenBy(p => p.SinDtl.RecIdNo)
                        //.ThenBy(p => p.SinKoui.SeqNo)
                        .ThenBy(p => p.SinDtl.RowNo)
                        .ToList();
                }
                else
                {
                    sinData =
                        sinData
                        .OrderBy(p => p.SinRp.SinId)
                        .ThenBy(p => p.SinRp.CdNo)
                        //.ThenBy(p => p.SinRp.SortKouiData)
                        .ThenBy(p => p.SinRp.FirstDay)
                        .ThenBy(p => p.SinRp.RpNo)
                        .ThenBy(p => p.SinKoui.SeqNo)
                        .ThenBy(p => p.SinDtl.RowNo)
                        .ToList();
                }

                // レセプトデータ作成

                int zeiRate = 0;
                int keigenRate = 0;
                _systemGenerationConfs = _mstFinder.FindSystemGenerationConf(_hpId, sinDate, 3001);
                if(_systemGenerationConfs.Any(p => p.GrpEdaNo == 0))
                {
                    zeiRate = _systemGenerationConfs.Find(p => p.GrpEdaNo == 0).Val;
                }

                if (_systemGenerationConfs.Any(p => p.GrpEdaNo == 1))
                {
                    keigenRate = _systemGenerationConfs.Find(p => p.GrpEdaNo == 1).Val;
                }
                else
                {
                    // 軽減税率の設定がない場合は、通常税率と同じ扱い
                    keigenRate = zeiRate;
                }

                int prevRpNo = 0;
                int prevSeqNo = 0;
                int preSinId = 0;

                int preSinRpNo = 0;

                int rpNo = 0;
                int seqNo = 0;
                int rowNo = 0;

                // レセコメント（01ヘッダー）
                #region レセコメントヘッダー
                //_emrLogger.WriteLogMsg( this, conFncName, "resecmt start", TTraceLevel.tlvNormal);
                int cmtHokenId = sinData.Any() ? sinData[0].SinKoui.HokenPid : hokenId;

                if (new int[] { SinMeiMode.Receden, SinMeiMode.PaperRece, SinMeiMode.ReceCheck, SinMeiMode.AfterCare, SinMeiMode.RecedenAfter }.Contains(mode))
                {
                    List<SinMeiDataModel> appendSinMeis = new List<SinMeiDataModel>();

                    // 低所得
                    if(mode == SinMeiMode.PaperRece)
                    {
                        if (_receInfs != null && _receInfs.Any())
                        {
                            if (_receInfs.First().KogakuOverKbn > 0)
                            {
                                string cmt = "";
                                if (_receInfs.First().KogakuKbn == 4)
                                {
                                    if (_prefCd == 26)
                                    {
                                        cmt = "      区分Ⅱ";
                                    }
                                    else
                                    {
                                        cmt = "低所得Ⅱ";
                                    }
                                }
                                else if(_receInfs.First().KogakuKbn == 5)
                                {
                                    if (_prefCd == 26)
                                    {
                                        cmt = "      区分Ⅰ";
                                    }
                                    else
                                    {
                                        cmt = "低所得Ⅰ";
                                    }
                                }

                                if (string.IsNullOrEmpty(cmt) == false)
                                {
                                    if (_prefCd == 25)
                                    {

                                    }
                                    else
                                    {
                                        SinMeiDataModel sinMei = MakeReceCmtRecord(ItemCdConst.CommentNinpu, cmt, "", 1, cmtHokenId);
                                        appendSinMeis.Add(sinMei);
                                    }
                                }
                            }
                        }
                    }

                    // 自動発生分の確認
                    if (new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter}.Contains(mode))
                    {
                        #region 不詳、災１、災２
                        string cmt = "";
                        //if (ptHokens != null && ptHokens.Any())
                        //{
                        //    if (ptHokens.First().HokensyaNo == "99999999")
                        //    {
                        //        cmt += "不詳";
                        //    }
                        //}
                        if (_receInfs != null && _receInfs.Any() && _receInfs.First().HokensyaNo == "99999999")
                        {
                            cmt += "不詳";
                        }

                        if (tokki != null)
                        {
                            int idx = 0;
                            while (idx + 1 < tokki.Length)
                            {
                                if (tokki.Substring(idx, 2) == "96")
                                {
                                    cmt = AppendText(cmt, "災１");
                                }
                                else if (tokki.Substring(idx, 2) == "97")
                                {
                                    cmt = AppendText(cmt, "災２");
                                }
                                idx += 2;
                            }
                        }

                        if (cmt != "")
                        {

                            SinMeiDataModel sinMei = MakeReceCmtRecord(ItemCdConst.CommentFree, cmt, cmt, 1, cmtHokenId);

                            appendSinMeis.Add(sinMei);
                        }
                        #endregion
                    }

                    if (new int[] { SinMeiMode.Receden , SinMeiMode.PaperRece , SinMeiMode.ReceCheck , SinMeiMode.AfterCare, SinMeiMode.RecedenAfter }.Contains(mode))
                    {
                        // 妊婦加算（RECE_INF.PT_STATUSをチェックし、"001"のコードがあれば、妊婦。なお、RECE_INF.PT_STATUSは3桁コードを結合したもの）
                        #region 妊婦
                        if (_receInfs != null && _receInfs.Any() && _receInfs.First().PtStatus != null)
                        {
                            //if (filteredSinDtls.Any(p=> ItemCdConst.ninpuKasanls.Contains(p.ItemCd)))
                            int idx = 0;
                            bool findNinpuStatus = false;
                            while (idx + 2 < _receInfs.First().PtStatus.Length)
                            {
                                if (_receInfs.First().PtStatus.Substring(idx, 3) == "001")
                                {
                                    findNinpuStatus = true;
                                    break;
                                }
                                idx += 3;
                            }

                            if (findNinpuStatus)
                            {
                                SinMeiDataModel sinMei = MakeReceCmtRecord(ItemCdConst.CommentNinpu, "妊婦", "", 1, cmtHokenId);

                                appendSinMeis.Add(sinMei);
                            }
                        }
                        #endregion

                        // 月中まで乳幼児
                        #region 月中まで乳幼児
                        if (_sinDate / 100 >= 201810)
                        {
                            // 2018/10以降

                            if (receCmts.Any(p => p.CmtKbn == 1 && p.ItemCd == ItemCdConst.CommentTukiTocyuNyu) == false)
                            {
                                // 手オーダーなし

                                int _ageYear = 0;
                                int _ageMonth = 0;
                                int _ageDay = 0;

                                CIUtil.SDateToDecodeAge(_ptInf.Birthday, CIUtil.GetLastDateOfMonth(_sinDate), ref _ageYear, ref _ageMonth, ref _ageDay);
                                if (_ageYear >= 6)
                                {
                                    if (_sinDate / 10000 - _ptInf.Birthday / 10000 == 6 &&
                                       _sinDate / 100 % 100 == _ptInf.Birthday / 100 % 100)
                                    {
                                        // 今月が6歳の誕生月
                                        if (_santeiFinder.CheckSanteiTerm(_hpId, _ptId, _sinDate / 100 * 100 + 1, _sinDate / 100 * 100 + (_ptInf.Birthday % 100 - 1), 0, 0, saisinNyuls, 0))
                                        {
                                            // 誕生日までに再診乳幼児関連項目の算定がある

                                            if (_santeiFinder.CheckSanteiTerm(_hpId, _ptId, _sinDate / 100 * 100 + (_ptInf.Birthday % 100), _sinDate / 100 * 100 + 31, 0, 0, saisinls, 0))
                                            {
                                                // 誕生日移行に再診関連項目の算定がある
                                                SinMeiDataModel sinMei = MakeReceCmtRecord(ItemCdConst.CommentTukiTocyuNyu, "月の途中まで乳幼児", "", 1, cmtHokenId);

                                                appendSinMeis.Add(sinMei);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    //都道府県別処理                    
                    if (_prefCd == PrefCode.Nara)
                    {
                        #region 奈良
                        //奈良県(29)で社保の場合
                        int naraFukusiReceCmtStartDate = CIUtil.StrToIntDef(_systemConfigProvider.GetNaraFukusiReceCmtStartDate(), 0);

                        if (_receInfs.First().HokenKbn == 1 &&
                            _systemConfigProvider.GetNaraFukusiReceCmt() == 1 &&
                            naraFukusiReceCmtStartDate >0 &&
                            naraFukusiReceCmtStartDate <= (sinDate / 100))
                        {
                            AppendNaraFukusiReceCmt(new List<string> { "71", "81", "91" }, ItemCdConst.CommentFree, "奈良県福祉医療", cmtHokenId, ref appendSinMeis);
                            AppendNaraFukusiReceCmt(new List<string> { "80" }, ItemCdConst.CommentFree, "奈良県精神医療", cmtHokenId, ref appendSinMeis);
                        }
                        #endregion
                    }
                    else if (_prefCd == PrefCode.Wakayama)
                    {
                        #region 和歌山
                        // 和歌山(30)

                        if (new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter}.Contains(mode))
                        {
                            AppendWakayamaFukusiReceCmt(new List<int> { 1, 3 }, ItemCdConst.CommentFree, "低１", cmtHokenId, ref appendSinMeis);
                            AppendWakayamaFukusiReceCmt(new List<int> { 2, 4 }, ItemCdConst.CommentFree, "低２", cmtHokenId, ref appendSinMeis);
                        }
                        #endregion
                    }

                    #region レセコメントヘッダー
                    List<ReceCmtModel> headers = receCmts.FindAll(p =>
                        p.CmtKbn == 1)
                        .OrderBy(p => p.CmtKbn)
                        .ThenBy(p => p.CmtSbt)
                        .ThenBy(p => p.SeqNo)
                        .ToList();
                    if (headers.Any())
                    {
                        for (int i = 0; i < headers.Count(); i++)
                        {
                            SinMeiDataModel sinMei = GetReceCmtRecord(headers[i], 1, cmtHokenId);

                            if (i >= headers.Count())
                            {
                                sinMei.LastRowKbn = 1;
                            }

                            appendSinMeis.Add(sinMei);
                        }
                    }
                    #endregion

                    // コメントを追加していく
                    if (appendSinMeis.Any())
                    {
                        rpNo++;
                        seqNo = 1;
                        rowNo = 0;
                        foreach (SinMeiDataModel appendSinMei in appendSinMeis)
                        {
                            rowNo++;
                            appendSinMei.RpNo = rpNo;
                            appendSinMei.SeqNo = seqNo;
                            appendSinMei.RowNo = rowNo;

                            sinMeis.Add(appendSinMei);
                        }
                    }
                }
                #endregion

                // （精）コメントを出力したかどうか true-出力済み
                bool kouseiGen = false;
                // （減）コメントを出力したかどうか true-出力済み
                bool tazaiGen = false;

                bool enFirst = false;
                bool tenFirst = false;

                for (int i = 0; i < sinData.Count; i++)
                {
                    if (new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter}.Contains(mode))
                    {
                        // レセ電の場合、（精）と（減）は先頭の１つだけ出力し、あとは出力しない
                        if (sinData[i].SinDtl.ItemCd == ItemCdConst.TouyakuYakuGenKousei)
                        {
                            // （精）
                            if (kouseiGen == true)
                            {
                                continue;
                            }
                            else
                            {
                                kouseiGen = true;
                            }
                        }
                        else if (sinData[i].SinDtl.ItemCd == ItemCdConst.TouyakuYakuGenNaifuku)
                        {
                            // （減）
                            if (tazaiGen == true)
                            {
                                continue;
                            }
                            else
                            {
                                tazaiGen = true;
                            }
                        }                        
                    }
                    if (new int[] { SinMeiMode.Ryosyu, SinMeiMode.Kaikei}.Contains(mode))
                    {
                        if (new string[] {
                                ItemCdConst.CommentJissiRekkyoDummy,
                                ItemCdConst.CommentJissiRekkyoZengoDummy,
                                ItemCdConst.CommentJissiRekkyoItemNameDummy,
                                ItemCdConst.CommentJissiNissuDummy
                            }.Contains(sinData[i].SinDtl.ItemCd))
                        {
                            // 実施日列挙
                            // 実施日数

                            // 領収証、精算画面には出さない

                            // 自身が最終行だった場合の処理
                            SetLastKbnExclusion(i, sinData);
                            continue;
                        }
                    }

                    if (
                            new string[] {
                                ItemCdConst.CommentJissiRekkyoDummy,
                                ItemCdConst.CommentJissiRekkyoZengoDummy,
                                ItemCdConst.CommentJissiRekkyoItemNameDummy
                            }.Contains(sinData[i].SinDtl.ItemCd))
                    {
                        // コメント変換 ITEM_CD, ITEM_NAME, COMMENT_DATA
                        List<CmtRecordData> cmtRecords = new List<CmtRecordData>();
                        bool commentExist = false;

                        cmtRecords =
                            ConvertTenkaiComment(sinData[i], sinKouiCountModels, sinKouiDetailModels, ref commentExist);

                        if (cmtRecords.Any())
                        {
                            foreach (CmtRecordData cmtRecord in cmtRecords)
                            {
                                SinMeiDataModel addSinmei = new SinMeiDataModel(_systemConfigProvider, _emrLogger);

                                TenMstModel tenMst = _mstFinder.FindTenMstByItemCd(_hpId, firstDateOfSinYm, cmtRecord.ItemCd).FirstOrDefault();
                                string retCommentData = CIUtil.ToWide(cmtRecord.CmtData);

                                List<int> cmtCol = new List<int>();
                                List<int> cmtLen = new List<int>();

                                if (tenMst.ItemCd.Substring(0, 3) == "840")
                                {
                                    for (int j = 1; j <= 4; j++)
                                    {
                                        cmtCol.Add(tenMst.CmtCol(j));
                                        cmtLen.Add(tenMst.CmtColKeta(j));
                                    }
                                }

                                string itemName = _mstFinder.GetCommentStr(firstDateOfSinYm, tenMst.ItemCd, cmtCol, cmtLen, tenMst.Name, tenMst.Name, ref retCommentData, false);
                                                                
                                addSinmei = MakeReceCmtRecord(cmtRecord.ItemCd, itemName, cmtRecord.CmtData, sinData[i].SinRp.SinId, sinData[i].SinKoui.HokenPid);

                                #region RpNo, SeqNo, RowNo
                                if (sinData[i].SinRp.RpNo != prevRpNo)
                                {
                                    // RpNoが変わったら、rpNoをカウントアップ、seqNoとrowNoを初期化
                                    rpNo++;
                                    seqNo = 1;
                                    rowNo = 1;

                                    prevRpNo = sinData[i].SinRp.RpNo;
                                    prevSeqNo = sinData[i].SinKoui.SeqNo;
                                }
                                else if (sinData[i].SinKoui.SeqNo != prevSeqNo)
                                {
                                    // SeqNoが変わったら、seqNoをカウントアップ、rowNoを初期化
                                    seqNo++;
                                    rowNo = 1;

                                    prevSeqNo = sinData[i].SinKoui.SeqNo;
                                }
                                else
                                {
                                    // rowNoをカウントアップ
                                    rowNo++;
                                }

                                preSinId = sinData[i].SinRp.SinId;
                                preSinRpNo = sinData[i].SinRp.RpNo;
                                #endregion


                                // PT_ID
                                addSinmei.PtId = _ptId;
                                // REC_ID
                                addSinmei.RecId = sinData[i].SinDtl.RecId;
                                addSinmei.SinRpNo = sinData[i].SinKoui.RpNo;
                                addSinmei.SinSeqNo = sinData[i].SinKoui.SeqNo;

                                setFutanKbn(ref addSinmei, _getHokenPidforSetFutankbn(sinData[i]));
                                
                                addSinmei.EnTenKbn = sinData[i].SinKoui.EntenKbn;
                                addSinmei.Count = sinData[i].Count;
                                SetCommentInf(ref addSinmei, sinData[i]);

                                addSinmei.RpNo = rpNo;
                                addSinmei.SeqNo = seqNo;
                                addSinmei.RowNo = rowNo;

                                sinMeis.Add(addSinmei);
                            }

                            // LAST_ROW_KBN
                            if (i >= sinData.Count - 1 ||
                                sinData[i].SinRp.RpNo != sinData[i + 1].SinRp.RpNo ||
                                sinData[i].SinKoui.SeqNo != sinData[i + 1].SinKoui.SeqNo)
                            {
                                // 最終行の場合、フラグを立てる
                                SinMeiDataModel updSinMei = sinMeis.Last();

                                updSinMei.LastRowKbn = 1;
                                SetTenKai(ref updSinMei, sinData[i]);
                                SetDays(ref updSinMei, sinData[i]);
                            }
                            continue;
                        }
                        else if(commentExist)
                        {
                            // 自身が最終行だった場合の処理
                            SetLastKbnExclusion(i, sinData);

                            // コメント項目が既に存在していた場合はスキップ
                            continue;
                        }
                    }
                    SinMeiDataModel sinMei = new SinMeiDataModel(_systemConfigProvider, _emrLogger);

                    // KIZAMI_ID, TEN_ID(この処理内で使用するので先に取得
                    if (sinData[i].SinDtl.TenMst != null)
                    {
                        sinMei.KizamiId = sinData[i].SinDtl.KizamiId;
                        sinMei.TenId = sinData[i].SinDtl.TenId;
                        // ついでに薬剤区分をセットしておく
                        sinMei.DrugKbn = sinData[i].SinDtl.DrugKbn;
                    }

                    // PT_ID
                    sinMei.PtId = _ptId;

                    // REC_ID
                    sinMei.RecId = sinData[i].SinDtl.RecId;

                    // LAST_ROW_KBN
                    if (i >= sinData.Count - 1 ||
                        sinData[i].SinRp.RpNo != sinData[i + 1].SinRp.RpNo ||
                        sinData[i].SinKoui.SeqNo != sinData[i + 1].SinKoui.SeqNo)
                    {
                        // 最終行の場合、フラグを立てる
                        sinMei.LastRowKbn = 1;
                    }

                    // RpNo, SeqNo, RowNo
                    if (sinData[i].SinRp.RpNo != prevRpNo)
                    {
                        // RpNoが変わったら、rpNoをカウントアップ、seqNoとrowNoを初期化
                        rpNo++;
                        seqNo = 1;
                        rowNo = 1;

                        prevRpNo = sinData[i].SinRp.RpNo;
                        prevSeqNo = sinData[i].SinKoui.SeqNo;
                    }
                    else if (sinData[i].SinKoui.SeqNo != prevSeqNo)
                    {
                        // SeqNoが変わったら、seqNoをカウントアップ、rowNoを初期化
                        seqNo++;
                        rowNo = 1;

                        prevSeqNo = sinData[i].SinKoui.SeqNo;
                    }
                    else
                    {
                        // rowNoをカウントアップ
                        rowNo++;
                    }

                    // SinRpNo, SinSeqNo
                    sinMei.SinRpNo = sinData[i].SinKoui.RpNo;
                    sinMei.SinSeqNo = sinData[i].SinKoui.SeqNo;

                    if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard, SinMeiMode.ReceCheck }.Contains(mode)) &&
                           new int[] { 11, 12, 13, 14 }.Contains(sinData[i].SinRp.SinId) &&
                           sinData[i].SinRp.SinId == preSinId)
                    {
                        // 初再診、医学管理、在宅は、行為が変わるまで初期化しない
                    }
                    else if (sinData[i].SinRp.RpNo != preSinRpNo)
                    {
                        // sinRp.RpNoが変わったらフラグ初期化
                        tenFirst = true;
                        enFirst = true;
                    }

                    // SIN_ID
                    if (rowNo == 1)
                    {
                        if(sinData[i].SinKoui.EntenKbn == 0 && tenFirst)
                        {
                            sinMei.SinId = sinData[i].SinRp.SinId;
                            tenFirst = false;
                        }
                        else if(sinData[i].SinKoui.EntenKbn == 1 && enFirst)
                        {
                            sinMei.SinId = sinData[i].SinRp.SinId;
                            enFirst = false;
                        }
                        else if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard, SinMeiMode.ReceCheck }.Contains(mode)) &&
                           new int[] { 11, 12, 13, 14 }.Contains(sinData[i].SinRp.SinId) &&
                           sinData[i].SinRp.SinId == preSinId)
                        {
                            // 初再診、医学管理、在宅は、同一行為内の先頭のみ診療区分を記録する
                        }
                        else if (sinData[i].SinRp.RpNo == preSinRpNo)
                        {
                            // 同一sinRp.RpNo内の先頭のみ診療区分を記録する
                        }
                        else
                        {
                            sinMei.SinId = sinData[i].SinRp.SinId;
                        }

                        if(!(new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter}.Contains(mode)))
                        {
                            // レセ電以外の時、手術も麻酔も50にする
                            if(sinMei.SinId > 50 && sinMei.SinId < 60)
                            {
                                sinMei.SinId = 50;
                            }
                        }
                    }
                    else
                    {
                        sinMei.SinId = 0;
                    }

                    if (sinMei.SinId > 0)
                    {
                        sinMei.SinIdOrg = sinMei.SinId;
                    }
                    else
                    {
                        sinMei.SinIdOrg = sinData[i].SinRp.SinId;

                        if (!(new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter }.Contains(mode)))
                        {
                            // レセ電以外の時、手術も麻酔も50にする
                            if (sinMei.SinIdOrg > 50 && sinMei.SinIdOrg < 60)
                            {
                                sinMei.SinIdOrg = 50;
                            }
                        }
                    }

                    preSinId = sinData[i].SinRp.SinId;
                    preSinRpNo = sinData[i].SinRp.RpNo;

                    setFutanKbn(ref sinMei, _getHokenPidforSetFutankbn(sinData[i]));
                    #region 負担区分をセット
                    //// FUTAN_KBN
                    //sinMei.FutanKbn = "";    // 仮

                    //// FUTAN_S
                    //sinMei.FutanS = 0;

                    //// FUTAN_K1
                    //sinMei.FutanK1 = 0;

                    //// FUTAN_K2
                    //sinMei.FutanK2 = 0;

                    //// FUTAN_K3
                    //sinMei.FutanK3 = 0;

                    //// FUTAN_K4
                    //sinMei.FutanK4 = 0;

                    //// PT_FUTAN_KBN
                    //PtFutanKbnModel ptFutanKbn =
                    //    GetHokenKohiFutan(mode, sinData[i].SinKoui.HokenPid);
                    //if (ptFutanKbn != null)
                    //{
                    //    //sinMei.FutanKbn = ptFutanKbn.FutanKbnCd;
                    //    sinMei.FutanS = ptFutanKbn.FutanS;
                    //    sinMei.FutanK1 = ptFutanKbn.FutanK1;
                    //    sinMei.FutanK2 = ptFutanKbn.FutanK2;
                    //    sinMei.FutanK3 = ptFutanKbn.FutanK3;
                    //    sinMei.FutanK4 = ptFutanKbn.FutanK4;
                    //}

                    ////if (mode == SinMeiMode.Receden)
                    ////{
                    //    sinMei.FutanKbn = GetFutanKbn(sinData[i].SinKoui.HokenPid, mode);
                    ////}
                    ////else
                    ////{
                    ////    sinMei.FutanKbn = "";
                    ////}
                    #endregion

                    //sinMei.ItemCd = sinData[i].sinDtl.ItemCd;

                    // コメント変換 ITEM_CD, ITEM_NAME, COMMENT_DATA
                    (sinMei.ItemCd, sinMei.ItemName, sinMei.CommentData) =
                        ConvertComment(sinData[i], sinKouiCountModels, sinKouiDetailModels, mode);

                    // ODR_ITEM_CD
                    sinMei.OdrItemCd = sinData[i].SinDtl.OdrItemCd;

                    if (sinMei.ItemCd == ItemCdConst.CommentFree)
                    {
                        // フリーコメントの場合、REC_IDを変更
                        sinMei.RecId = "CO";
                    }

                    // 麻毒のコメントを付ける
                    if (new int[] { 21, 22, 23 }.Contains(sinData[i].SinRp.SinId) && sinData[i].SinDtl.TenMst != null)
                    {
                        // 投薬
                        switch (sinData[i].SinDtl.TenMst.MadokuKbn)
                        {
                            case 1:
                                sinMei.ItemName = "（麻）" + sinMei.ItemName;
                                break;
                            case 2:
                                sinMei.ItemName = "（毒）" + sinMei.ItemName;
                                break;
                            case 3:
                                sinMei.ItemName = "（覚）" + sinMei.ItemName;
                                break;
                            case 5:
                                sinMei.ItemName = "（向）" + sinMei.ItemName;
                                break;
                        }
                    }
                    else if (sinData[i].SinRp.SinId >= ReceSinId.ChusyaMin && sinData[i].SinRp.SinId <= ReceSinId.ChusyaMax && sinData[i].SinDtl.TenMst != null)
                    {
                        // 注射
                        if (sinData[i].SinDtl.TenMst.MadokuKbn == 1)
                        {
                            sinMei.ItemName = "（麻）" + sinMei.ItemName;
                        }
                    }

                    // 分
                    if (sinData[i].SinDtl.FmtKbn == FmtKbnConst.Minute)
                    {
                        sinMei.ItemName += "（" + CIUtil.ToWide(CIUtil.MinuteToShowHour((int)sinData[i].SinDtl.Suryo2)) + "）";
                    }

                    // 数量
                    if (sinData[i].SinDtl.FmtKbn == FmtKbnConst.GazoSindanSatuei)
                    {
                        // 画像
                        sinMei.Suryo = sinData[i].SinDtl.Suryo2;
                    }
                    else
                    {
                        sinMei.Suryo = sinData[i].SinDtl.Suryo;
                    }

                    // 回数
                    sinMei.Count = sinData[i].Count;

                    if (mode == SinMeiMode.ReceTensuAfter)
                    { 
                        // アフターケアの点数欄データ取得時は、SIN_KOUIのCOUNTとTENCOL_COUNTとTOTAL_TENを算定回数情報のデータで更新しておく
                        if (sinData[i].SinKoui != null)
                        {
                            sinData[i].SinKoui.Count = sinData[i].Count;
                            if(sinData[i].SinKoui.TenColCount > 0)
                            {
                                sinData[i].SinKoui.TenColCount = sinData[i].Count;
                            }
                            sinData[i].SinKoui.TotalTen = sinData[i].SinKoui.Ten * sinData[i].Count;
                        }
                    }

                    // 円点区分
                    sinMei.EnTenKbn = sinData[i].SinKoui.EntenKbn;

                    // 最終行の場合
                    // TEN, KINGAKU, TOTAL_TEN
                    if (sinMei.LastRowKbn == 1)
                    {
                        SetTenKai(ref sinMei, sinData[i]);
                        //double TotalTen = sinData[i].SinKoui.TotalTen;
                        //double Ten = sinData[i].SinKoui.Ten;

                        //if (new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AfterCare, SinMeiMode.AccountingCard }.Contains(mode))
                        //{
                        //    // 領収証、アフターケアの場合は、算定回数情報の回数を元に、合計点数を取得する
                        //    TotalTen = sinData[i].SinKoui.Ten * sinData[i].Count;
                        //}

                        //// 円点区分
                        ////sinMei.EnTenKbn = sinData[i].SinKoui.EntenKbn;

                        //if (sinData[i].SinKoui.EntenKbn == 0)
                        //{
                        //    // 点
                        //    sinMei.TotalTen = TotalTen;
                        //    sinMei.Ten = Ten;
                        //}
                        //else if (sinData[i].SinKoui.EntenKbn == 1)
                        //{
                        //    // 円
                        //    sinMei.TotalKingaku = TotalTen;
                        //    sinMei.Kingaku = Ten;
                        //}
                        //else
                        //{
                        //    // 労災用金額集計
                        //    if (sinData[i].SinKoui.Kingaku > 0)
                        //    {
                        //        sinMei.TotalKingaku = sinData[i].SinKoui.Kingaku + Ten * 10;
                        //        sinMei.Ten = Ten;
                        //        sinMei.Kingaku = sinData[i].SinKoui.Kingaku;
                        //    }
                        //}

                        ////sinMei.CdKbn = sinData[i].sinKoui.CdKbn;
                        ////sinMei.JihiSbt = sinData[i].sinKoui.JihiSbt;
                    }
                    sinMei.CdKbn = sinData[i].SinKoui.CdKbn;
                    sinMei.JihiSbt = sinData[i].SinKoui.JihiSbt;
                    sinMei.KazeiKbn = sinData[i].SinKoui.KazeiKbn;

                    // 税率
                    sinMei.TaxRate = 0;
                    if (sinData[i].SinKoui.SanteiKbn == SanteiKbnConst.Jihi)
                    {
                        switch(sinMei.KazeiKbn)
                        {
                            case 0: // 非課税
                                break;
                            case 1: // 外税（通常税率）
                                sinMei.TaxRate = zeiRate;
                                break;
                            case 2: // 外税（軽減税率）
                                sinMei.TaxRate = keigenRate;
                                break;
                            case 3: // 内税（通常税率）
                                sinMei.TaxRate = zeiRate;
                                break;
                            case 4: // 内税（軽減税率）
                                sinMei.TaxRate = keigenRate;
                                break;
                        }
                    }

                    // UNIT_CD(TO専用)
                    if (new int[] { 1, 2, 4 }.Contains(sinMei.TenId) && sinData[i].SinDtl.UnitCd == 0)
                    {
                        //if (sinData[i].sinDtl.UnitCd == 0)
                        //{
                        UnitMstModel unitMst = _receMasterFinder.FindUnitMst(_sinDate, sinData[i].SinDtl.UnitName);
                        if (unitMst.UnitMst != null)
                        {
                            sinMei.UnitCd = unitMst.UnitCd;
                        }
                        else
                        {
                            // 見つからない場合
                            sinMei.UnitCd = 7;
                        }
                        //}
                        //else
                        //{
                        //    sinMei.UnitCd = sinData[i].sinDtl.UnitCd;
                        //}
                    }
                    else
                    {
                        sinMei.UnitCd = 0;
                    }

                    // UNIT_NAME
                    sinMei.UnitName = sinData[i].SinDtl.UnitName;

                    // PRICE(TO専用)
                    sinMei.Price = 0;
                                        
                    if (sinMei.RecId == "TO")
                    {
                        double tenId = sinMei.TenId;
                        if (sinData[i].SinDtl.Z_TenId > 0 && string.IsNullOrEmpty(sinMei.OdrItemCd) == false && sinMei.OdrItemCd.StartsWith("Z") == true)
                        {
                            // Z特材の場合は、算定用項目コードから取得した値をセット
                            tenId = sinData[i].SinDtl.Z_TenId;
                        }

                        if ((tenId == 2 || (tenId != 5 && sinData[i].SinDtl.SansoKbn > 0)))
                        {
                            sinMei.Price = sinData[i].SinDtl.TenMst?.Ten ?? 0;
                        }
                    }                    

                    // TOKUZAI_NAME(TO専用)
                    sinMei.TokuzaiName = "";

                    // PRODUCT_NAME(TO専用)
                    if (sinData[i].SinDtl.FmtKbn == 20)
                    {
                        sinMei.ProductName = sinData[i].SinDtl.ItemName;
                    }

                    //COMMENT
                    SetCommentInf(ref sinMei, sinData[i]);
                    #region comment
                    //sinMei.CommentCd1 = sinData[i].SinDtl.CmtCd1;
                    //sinMei.CommentData1 = sinData[i].SinDtl.CmtOpt1;
                    //sinMei.Comment1 = sinData[i].SinDtl.Cmt1;
                    //sinMei.CommentCd2 = sinData[i].SinDtl.CmtCd2;
                    //sinMei.CommentData2 = sinData[i].SinDtl.CmtOpt2;
                    //sinMei.Comment2 = sinData[i].SinDtl.Cmt2;
                    //sinMei.CommentCd3 = sinData[i].SinDtl.CmtCd3;
                    //sinMei.CommentData3 = sinData[i].SinDtl.CmtOpt3;
                    //sinMei.Comment3 = sinData[i].SinDtl.Cmt3;
                    #endregion

                    // DAY
                    SetDays(ref sinMei, sinData[i]);
                    #region day
                    //if (sinMei.LastRowKbn == 1 || mode == SinMeiMode.Receden)
                    //{
                    //    if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard }.Contains(mode)))
                    //    {
                    //        sinMei.Day1 = sinData[i].SinKoui.Day1;
                    //        sinMei.Day2 = sinData[i].SinKoui.Day2;
                    //        sinMei.Day3 = sinData[i].SinKoui.Day3;
                    //        sinMei.Day4 = sinData[i].SinKoui.Day4;
                    //        sinMei.Day5 = sinData[i].SinKoui.Day5;
                    //        sinMei.Day6 = sinData[i].SinKoui.Day6;
                    //        sinMei.Day7 = sinData[i].SinKoui.Day7;
                    //        sinMei.Day8 = sinData[i].SinKoui.Day8;
                    //        sinMei.Day9 = sinData[i].SinKoui.Day9;
                    //        sinMei.Day10 = sinData[i].SinKoui.Day10;
                    //        sinMei.Day11 = sinData[i].SinKoui.Day11;
                    //        sinMei.Day12 = sinData[i].SinKoui.Day12;
                    //        sinMei.Day13 = sinData[i].SinKoui.Day13;
                    //        sinMei.Day14 = sinData[i].SinKoui.Day14;
                    //        sinMei.Day15 = sinData[i].SinKoui.Day15;
                    //        sinMei.Day16 = sinData[i].SinKoui.Day16;
                    //        sinMei.Day17 = sinData[i].SinKoui.Day17;
                    //        sinMei.Day18 = sinData[i].SinKoui.Day18;
                    //        sinMei.Day19 = sinData[i].SinKoui.Day19;
                    //        sinMei.Day20 = sinData[i].SinKoui.Day20;
                    //        sinMei.Day21 = sinData[i].SinKoui.Day21;
                    //        sinMei.Day22 = sinData[i].SinKoui.Day22;
                    //        sinMei.Day23 = sinData[i].SinKoui.Day23;
                    //        sinMei.Day24 = sinData[i].SinKoui.Day24;
                    //        sinMei.Day25 = sinData[i].SinKoui.Day25;
                    //        sinMei.Day26 = sinData[i].SinKoui.Day26;
                    //        sinMei.Day27 = sinData[i].SinKoui.Day27;
                    //        sinMei.Day28 = sinData[i].SinKoui.Day28;
                    //        sinMei.Day29 = sinData[i].SinKoui.Day29;
                    //        sinMei.Day30 = sinData[i].SinKoui.Day30;
                    //        sinMei.Day31 = sinData[i].SinKoui.Day31;

                    //        sinMei.Day1Add = sinData[i].SinKoui.Day1Add;
                    //        sinMei.Day2Add = sinData[i].SinKoui.Day2Add;
                    //        sinMei.Day3Add = sinData[i].SinKoui.Day3Add;
                    //        sinMei.Day4Add = sinData[i].SinKoui.Day4Add;
                    //        sinMei.Day5Add = sinData[i].SinKoui.Day5Add;
                    //        sinMei.Day6Add = sinData[i].SinKoui.Day6Add;
                    //        sinMei.Day7Add = sinData[i].SinKoui.Day7Add;
                    //        sinMei.Day8Add = sinData[i].SinKoui.Day8Add;
                    //        sinMei.Day9Add = sinData[i].SinKoui.Day9Add;
                    //        sinMei.Day10Add = sinData[i].SinKoui.Day10Add;
                    //        sinMei.Day11Add = sinData[i].SinKoui.Day11Add;
                    //        sinMei.Day12Add = sinData[i].SinKoui.Day12Add;
                    //        sinMei.Day13Add = sinData[i].SinKoui.Day13Add;
                    //        sinMei.Day14Add = sinData[i].SinKoui.Day14Add;
                    //        sinMei.Day15Add = sinData[i].SinKoui.Day15Add;
                    //        sinMei.Day16Add = sinData[i].SinKoui.Day16Add;
                    //        sinMei.Day17Add = sinData[i].SinKoui.Day17Add;
                    //        sinMei.Day18Add = sinData[i].SinKoui.Day18Add;
                    //        sinMei.Day19Add = sinData[i].SinKoui.Day19Add;
                    //        sinMei.Day20Add = sinData[i].SinKoui.Day20Add;
                    //        sinMei.Day21Add = sinData[i].SinKoui.Day21Add;
                    //        sinMei.Day22Add = sinData[i].SinKoui.Day22Add;
                    //        sinMei.Day23Add = sinData[i].SinKoui.Day23Add;
                    //        sinMei.Day24Add = sinData[i].SinKoui.Day24Add;
                    //        sinMei.Day25Add = sinData[i].SinKoui.Day25Add;
                    //        sinMei.Day26Add = sinData[i].SinKoui.Day26Add;
                    //        sinMei.Day27Add = sinData[i].SinKoui.Day27Add;
                    //        sinMei.Day28Add = sinData[i].SinKoui.Day28Add;
                    //        sinMei.Day29Add = sinData[i].SinKoui.Day29Add;
                    //        sinMei.Day30Add = sinData[i].SinKoui.Day30Add;
                    //        sinMei.Day31Add = sinData[i].SinKoui.Day31Add;
                    //    }
                    //    else
                    //    {
                    //        sinMei.Day1 = sinData[i].Day1;
                    //        sinMei.Day2 = sinData[i].Day2;
                    //        sinMei.Day3 = sinData[i].Day3;
                    //        sinMei.Day4 = sinData[i].Day4;
                    //        sinMei.Day5 = sinData[i].Day5;
                    //        sinMei.Day6 = sinData[i].Day6;
                    //        sinMei.Day7 = sinData[i].Day7;
                    //        sinMei.Day8 = sinData[i].Day8;
                    //        sinMei.Day9 = sinData[i].Day9;
                    //        sinMei.Day10 = sinData[i].Day10;
                    //        sinMei.Day11 = sinData[i].Day11;
                    //        sinMei.Day12 = sinData[i].Day12;
                    //        sinMei.Day13 = sinData[i].Day13;
                    //        sinMei.Day14 = sinData[i].Day14;
                    //        sinMei.Day15 = sinData[i].Day15;
                    //        sinMei.Day16 = sinData[i].Day16;
                    //        sinMei.Day17 = sinData[i].Day17;
                    //        sinMei.Day18 = sinData[i].Day18;
                    //        sinMei.Day19 = sinData[i].Day19;
                    //        sinMei.Day20 = sinData[i].Day20;
                    //        sinMei.Day21 = sinData[i].Day21;
                    //        sinMei.Day22 = sinData[i].Day22;
                    //        sinMei.Day23 = sinData[i].Day23;
                    //        sinMei.Day24 = sinData[i].Day24;
                    //        sinMei.Day25 = sinData[i].Day25;
                    //        sinMei.Day26 = sinData[i].Day26;
                    //        sinMei.Day27 = sinData[i].Day27;
                    //        sinMei.Day28 = sinData[i].Day28;
                    //        sinMei.Day29 = sinData[i].Day29;
                    //        sinMei.Day30 = sinData[i].Day30;
                    //        sinMei.Day31 = sinData[i].Day31;

                    //        sinMei.Day1Add = sinData[i].Day1Add;
                    //        sinMei.Day2Add = sinData[i].Day2Add;
                    //        sinMei.Day3Add = sinData[i].Day3Add;
                    //        sinMei.Day4Add = sinData[i].Day4Add;
                    //        sinMei.Day5Add = sinData[i].Day5Add;
                    //        sinMei.Day6Add = sinData[i].Day6Add;
                    //        sinMei.Day7Add = sinData[i].Day7Add;
                    //        sinMei.Day8Add = sinData[i].Day8Add;
                    //        sinMei.Day9Add = sinData[i].Day9Add;
                    //        sinMei.Day10Add = sinData[i].Day10Add;
                    //        sinMei.Day11Add = sinData[i].Day11Add;
                    //        sinMei.Day12Add = sinData[i].Day12Add;
                    //        sinMei.Day13Add = sinData[i].Day13Add;
                    //        sinMei.Day14Add = sinData[i].Day14Add;
                    //        sinMei.Day15Add = sinData[i].Day15Add;
                    //        sinMei.Day16Add = sinData[i].Day16Add;
                    //        sinMei.Day17Add = sinData[i].Day17Add;
                    //        sinMei.Day18Add = sinData[i].Day18Add;
                    //        sinMei.Day19Add = sinData[i].Day19Add;
                    //        sinMei.Day20Add = sinData[i].Day20Add;
                    //        sinMei.Day21Add = sinData[i].Day21Add;
                    //        sinMei.Day22Add = sinData[i].Day22Add;
                    //        sinMei.Day23Add = sinData[i].Day23Add;
                    //        sinMei.Day24Add = sinData[i].Day24Add;
                    //        sinMei.Day25Add = sinData[i].Day25Add;
                    //        sinMei.Day26Add = sinData[i].Day26Add;
                    //        sinMei.Day27Add = sinData[i].Day27Add;
                    //        sinMei.Day28Add = sinData[i].Day28Add;
                    //        sinMei.Day29Add = sinData[i].Day29Add;
                    //        sinMei.Day30Add = sinData[i].Day30Add;
                    //        sinMei.Day31Add = sinData[i].Day31Add;
                    //    }
                    //}
                    #endregion

                    // HOKEN_PID
                    sinMei.HokenPid = sinData[i].SinKoui.HokenPid;

                    // RP_NO
                    sinMei.RpNo = rpNo;
                    // SEQ_NO
                    sinMei.SeqNo = seqNo;
                    // ROW_NO
                    sinMei.RowNo = rowNo;

                    // SANTEI_KBN
                    sinMei.SanteiKbn = sinData[i].SinRp.SanteiKbn;
                    // INOUT_KBN
                    sinMei.InOutKbn = sinData[i].SinKoui.InoutKbn;

                    // SYUKEI_SAKI
                    sinMei.SyukeiSaki = sinData[i].SinKoui.SyukeiSaki;

                    sinMeis.Add(sinMei);
                }

                // レセコメント（フッター）
                #region レセコメントフッター
                //_emrLogger.WriteLogMsg( this, conFncName, "rececmt footer");
                if (new int[] { SinMeiMode.Receden, SinMeiMode.PaperRece, SinMeiMode.ReceCheck, SinMeiMode.AfterCare, SinMeiMode.RecedenAfter }.Contains(mode))
                {
                    List<ReceCmtModel> footers = receCmts.FindAll(p =>
                        p.CmtKbn == 2)
                        .OrderBy(p => p.CmtKbn)
                        .ThenBy(p => p.CmtSbt)
                        .ThenBy(p => p.SeqNo)
                        .ToList();
                    if (footers.Any())
                    {
                        rpNo++;
                        seqNo = 1;
                        rowNo = 1;

                        for (int i = 0; i < footers.Count(); i++)
                        {
                            SinMeiDataModel sinMei = GetReceCmtRecord(footers[i], 99, cmtHokenId);

                            if (i >= footers.Count())
                            {
                                sinMei.LastRowKbn = 1;
                            }

                            sinMei.RpNo = rpNo;
                            sinMei.SeqNo = seqNo;

                            sinMei.RowNo = rowNo;
                            rowNo++;

                            sinMeis.Add(sinMei);
                        }
                    }
                }
                #endregion

                _firstSinDate = 0;
                List<string> syosinls = new List<string>
                    {
                        ItemCdConst.Syosin,
                        ItemCdConst.Syosin2,
                        ItemCdConst.SyosinCorona,
                        ItemCdConst.SyosinRousai,
                        ItemCdConst.Syosin2Rousai,
                        ItemCdConst.SyosinJouhou,
                        ItemCdConst.Syosin2Jouhou
                    };

                var syosinKouis = sinData.FindAll(p => syosinls.Contains(p.SinDtl.ItemCd)).Select(p=>p.SinKoui);
                foreach (var syosinKoui in syosinKouis)
                {
                    if (sinKouiCountModels.Any(p => p.RpNo == syosinKoui.RpNo && p.SeqNo == syosinKoui.SeqNo))
                    {
                        //int minDate = sinKouiCountModels.Where(p => p.RpNo == syosinKoui.RpNo && p.SeqNo == syosinKoui.SeqNo).Min(p => p.SinYm * 100 + p.SinDay);
                        int minDate = sinKouiCountModels.Where(p => p.RpNo == syosinKoui.RpNo && p.SeqNo == syosinKoui.SeqNo).Min(p => p.SinDate);
                        if (_firstSinDate == 0 || minDate < _firstSinDate)
                        {
                            _firstSinDate = minDate;
                        }
                    }
                }
                if (_firstSinDate == 0)
                {
                    _lastSinDate = sinKouiCountModels.Min(p => p.SinDate);
                }

                _lastSinDate = 0;
                var sinKouis = sinData.GroupBy(p => new { RpNo = p.SinKoui.RpNo, SeqNo = p.SinKoui.SeqNo }).ToList(); ;
                foreach (var sinKoui in sinKouis)
                {
                    if (sinKouiCountModels.Any(p => p.RpNo == sinKoui.Key.RpNo && p.SeqNo == sinKoui.Key.SeqNo))
                    {
                        //int maxDate = sinKouiCountModels.Where(p => p.RpNo == sinKoui.Key.RpNo && p.SeqNo == sinKoui.Key.SeqNo).Min(p => p.SinYm * 100 + p.SinDay);
                        int maxDate = sinKouiCountModels.Where(p => p.RpNo == sinKoui.Key.RpNo && p.SeqNo == sinKoui.Key.SeqNo).Max(p => p.SinDate);
                        if (maxDate > _lastSinDate)
                        {
                            _lastSinDate = maxDate;
                        }
                    }
                }
                if (_lastSinDate == 0)
                {
                    _lastSinDate = sinKouiCountModels.Max(p => p.SinDate);
                }

                _emrLogger.WriteLogEnd( this, conFncName, "");

                // SetFutanKbnの引数用のhokenPidを取得する
                int _getHokenPidforSetFutankbn(SinMeiDataSet targetSinData)
                {
                    int ret = targetSinData.SinKoui.HokenPid;

                    //改正TODO　情報通信機器を用いた場合コメント必要か？
                    if (new List<string>
                         { ItemCdConst.CommentSyosinJikanNai }
                    .Contains(targetSinData.SinDtl.ItemCd))
                    {
                        if (sinData.Any(p => p.SinRp.SinKouiKbn == ReceKouiKbn.Syosin && p.SinRp.RpNo != targetSinData.SinRp.RpNo))
                        {
                            ret =
                                sinData.First(p => p.SinRp.SinKouiKbn == ReceKouiKbn.Syosin && p.SinRp.RpNo != targetSinData.SinRp.RpNo).SinKoui.HokenPid;
                        }
                    }
                    else if (targetSinData.SinDtl.ItemCd == ItemCdConst.CommentSaisinDojitu)
                    {
                        if (sinData.Any(p => p.SinDtl.ItemCd == ItemCdConst.SaisinDojitu && p.SinRp.RpNo != targetSinData.SinRp.RpNo))
                        {
                            ret =
                                sinData.First(p => p.SinDtl.ItemCd == ItemCdConst.SaisinDojitu && p.SinRp.RpNo != targetSinData.SinRp.RpNo).SinKoui.HokenPid;
                        }
                    }
                    else if (targetSinData.SinDtl.ItemCd == ItemCdConst.CommentDenwaSaisin )
                    {
                        if (sinData.Any(p => new List<string> { ItemCdConst.SaisinDenwa, ItemCdConst.SaisinDenwa2 }.Contains(p.SinDtl.ItemCd)
                            && p.SinRp.RpNo != targetSinData.SinRp.RpNo))
                        {
                            ret =
                                sinData.First(p => new List<string> { ItemCdConst.SaisinDenwa, ItemCdConst.SaisinDenwa2 }.Contains(p.SinDtl.ItemCd) 
                                && p.SinRp.RpNo != targetSinData.SinRp.RpNo).SinKoui.HokenPid;
                        }
                    }
                    else if (targetSinData.SinDtl.ItemCd == ItemCdConst.CommentDenwaSaisinDojitu)
                    {
                        if (sinData.Any(p => p.SinDtl.ItemCd == ItemCdConst.SaisinDenwaDojitu && p.SinRp.RpNo != targetSinData.SinRp.RpNo))
                        {
                            ret =
                                sinData.First(p => p.SinDtl.ItemCd == ItemCdConst.SaisinDenwaDojitu && p.SinRp.RpNo != targetSinData.SinRp.RpNo).SinKoui.HokenPid;
                        }
                    }
                    
                    return ret;
                }
            }

            #region Local Method
            SinMeiDataModel GetReceCmtRecord(ReceCmtModel argReceCmtModel, int sinId, int hokenPid)
            {
                return MakeReceCmtRecord(argReceCmtModel.ItemCd, argReceCmtModel.Cmt, argReceCmtModel.CmtData, sinId, hokenPid);
            }

            // レセコメントレコード生成
            SinMeiDataModel MakeReceCmtRecord(string itemCd, string cmt, string cmtData, int sinId, int hokenPid)
            {
                SinMeiDataModel retSinMei = new SinMeiDataModel(_systemConfigProvider, _emrLogger);

                // PT_ID
                retSinMei.PtId = _ptId;
                // REC_ID
                retSinMei.RecId = "CO";
                // SIN_ID
                retSinMei.SinId = sinId;

                // FUTAN_KBN
                retSinMei.FutanKbn = "";    // 仮

                // FUTAN_S
                retSinMei.FutanS = 0;

                // FUTAN_K1
                retSinMei.FutanK1 = 0;

                // FUTAN_K2
                retSinMei.FutanK2 = 0;

                // FUTAN_K3
                retSinMei.FutanK3 = 0;

                // FUTAN_K4
                retSinMei.FutanK4 = 0;

                // PT_FUTAN_KBN
                PtFutanKbnModel ptFutanKbn =
                    GetHokenKohiFutan(mode, hokenPid);
                if (ptFutanKbn != null)
                {
                    retSinMei.FutanKbn = ptFutanKbn.FutanKbnCd;
                    retSinMei.FutanS = ptFutanKbn.FutanS;
                    retSinMei.FutanK1 = ptFutanKbn.FutanK1;
                    retSinMei.FutanK2 = ptFutanKbn.FutanK2;
                    retSinMei.FutanK3 = ptFutanKbn.FutanK3;
                    retSinMei.FutanK4 = ptFutanKbn.FutanK4;
                }

                //if (mode == SinMeiMode.Receden)
                //{
                    retSinMei.FutanKbn = GetFutanKbn(hokenPid, mode);
                //}
                //else
                //{
                //    retSinMei.FutanKbn = "";
                //}

                // ITEM_CD
                if (itemCd == "")
                {
                    retSinMei.ItemCd = ItemCdConst.CommentFree;
                }
                else
                {
                    retSinMei.ItemCd = itemCd;
                }

                if (itemCd == "" || itemCd == ItemCdConst.CommentFree)
                {
                    // ITEM_NAME
                    retSinMei.ItemName = cmt;

                    // COMMENT_DATA
                    retSinMei.CommentData = cmt;
                }
                else
                {
                    // ITEM_NAME
                    retSinMei.ItemName = cmt;

                    // COMMENT_DATA
                    if(itemCd.StartsWith("850"))
                    {
                        if (cmtData.Length < 7)
                        {
                            cmtData = cmtData.PadRight(7, '０');
                        }
                    }
                    else if(itemCd.StartsWith("852"))
                    {
                        if(cmtData.Length < 5)
                        {
                            cmtData = cmtData.PadLeft(5, '０');
                        }
                    }
                    retSinMei.CommentData = cmtData;
                }

                return retSinMei;
            }

            // 奈良県社保＋福祉のレセコメント追加
            void AppendNaraFukusiReceCmt(List<string> checkHoubetu, string itemCd, string cmt, int hokenPid, ref List<SinMeiDataModel> appendSinMeis)
            {
                if(_ptHokenPatternModels.Any(p=>
                        (p.HokenId == _receInfs.First().HokenId || p.HokenId == _receInfs.First().HokenId2) &&
                     (checkHoubetu.Contains(p.Kohi1Houbetu) ||
                     checkHoubetu.Contains(p.Kohi2Houbetu) ||
                     checkHoubetu.Contains(p.Kohi3Houbetu) ||
                     checkHoubetu.Contains(p.Kohi4Houbetu))))
                {
                    SinMeiDataModel sinMei = MakeReceCmtRecord(itemCd, cmt, "", 1, hokenPid);

                    appendSinMeis.Add(sinMei);
                }
            }

            // 和歌山県福祉のレセコメント
            void AppendWakayamaFukusiReceCmt(List<int> edaNos, string itemCd, string cmt, int hokenPid, ref List<SinMeiDataModel> appendSinMeis)
            {
                if (_ptHokenPatternModels.Any(p =>
                        (p.HokenId == _receInfs.First().HokenId || p.HokenId == _receInfs.First().HokenId2) &&
                         ((p.Kohi1HokenNo == 141 && edaNos.Contains(p.Kohi1HokenEdaNo)) ||
                          (p.Kohi2HokenNo == 141 && edaNos.Contains(p.Kohi2HokenEdaNo)) ||
                          (p.Kohi3HokenNo == 141 && edaNos.Contains(p.Kohi3HokenEdaNo)) ||
                          (p.Kohi4HokenNo == 141 && edaNos.Contains(p.Kohi4HokenEdaNo)))
                        ))
                {
                    SinMeiDataModel sinMei = MakeReceCmtRecord(itemCd, cmt, "", 1, hokenPid);

                    appendSinMeis.Add(sinMei);
                }
            }

            // 負担区分をセット
            void setFutanKbn(ref SinMeiDataModel sinMei, int hokenPid)
            {
                // FUTAN_KBN
                sinMei.FutanKbn = "";    // 仮

                // FUTAN_S
                sinMei.FutanS = 0;

                // FUTAN_K1
                sinMei.FutanK1 = 0;

                // FUTAN_K2
                sinMei.FutanK2 = 0;

                // FUTAN_K3
                sinMei.FutanK3 = 0;

                // FUTAN_K4
                sinMei.FutanK4 = 0;

                // PT_FUTAN_KBN
                PtFutanKbnModel ptFutanKbn =
                    GetHokenKohiFutan(mode, hokenPid);
                if (ptFutanKbn != null)
                {
                    //sinMei.FutanKbn = ptFutanKbn.FutanKbnCd;
                    sinMei.FutanS = ptFutanKbn.FutanS;
                    sinMei.FutanK1 = ptFutanKbn.FutanK1;
                    sinMei.FutanK2 = ptFutanKbn.FutanK2;
                    sinMei.FutanK3 = ptFutanKbn.FutanK3;
                    sinMei.FutanK4 = ptFutanKbn.FutanK4;
                }

                //if (mode == SinMeiMode.Receden)
                //{
                sinMei.FutanKbn = GetFutanKbn(hokenPid, mode);
                //}
                //else
                //{
                //    sinMei.FutanKbn = "";
                //}

                //sinMei.ItemCd = sinData[i].sinDtl.ItemCd;
            }

            // 点数回数をセット
            void SetTenKai(ref SinMeiDataModel sinMei, SinMeiDataSet sinData)
            {
                double TotalTen = sinData.SinKoui.TotalTen;
                double Ten = sinData.SinKoui.Ten;

                if (new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AfterCare, SinMeiMode.AccountingCard, SinMeiMode.RecedenAfter }.Contains(mode))
                {
                    // 領収証、アフターケアの場合は、算定回数情報の回数を元に、合計点数を取得する
                    TotalTen = sinData.SinKoui.Ten * sinData.Count;
                }

                // 円点区分
                //sinMei.EnTenKbn = sinData[i].SinKoui.EntenKbn;

                if (sinData.SinKoui.EntenKbn == 0)
                {
                    // 点
                    sinMei.TotalTen = TotalTen;
                    sinMei.Ten = Ten;
                }
                else if (sinData.SinKoui.EntenKbn == 1)
                {
                    // 円
                    sinMei.TotalKingaku = TotalTen;
                    sinMei.Kingaku = Ten;
                }
                else
                {
                    // 労災用金額集計
                    if (sinData.SinKoui.Kingaku > 0)
                    {
                        sinMei.TotalKingaku = sinData.SinKoui.Kingaku + Ten * 10;
                        sinMei.Ten = Ten;
                        sinMei.Kingaku = sinData.SinKoui.Kingaku;
                    }
                }

                // 点数が0でも、点数回数を表示するかどうか
                sinMei.DspZeroTenKai = sinData.SinKoui.DspZeroTenkai;

                if(sinMei.RecId == "CO" && _systemConfigProvider.GetReceiptCommentTenCount() == 1)
                {
                    sinMei.DspZeroTenKai = true;
                }
                //sinMei.CdKbn = sinData[i].sinKoui.CdKbn;
                //sinMei.JihiSbt = sinData[i].sinKoui.JihiSbt;
            }

            // コメントの条件等をセット
            void SetCommentInf(ref SinMeiDataModel sinMei, SinMeiDataSet sinData)
            {
                sinMei.CommentCd1 = sinData.SinDtl.CmtCd1;
                sinMei.CommentData1 = sinData.SinDtl.CmtOpt1;
                sinMei.Comment1 = sinData.SinDtl.Cmt1;
                sinMei.CommentCd2 = sinData.SinDtl.CmtCd2;
                sinMei.CommentData2 = sinData.SinDtl.CmtOpt2;
                sinMei.Comment2 = sinData.SinDtl.Cmt2;
                sinMei.CommentCd3 = sinData.SinDtl.CmtCd3;
                sinMei.CommentData3 = sinData.SinDtl.CmtOpt3;
                sinMei.Comment3 = sinData.SinDtl.Cmt3;
            }
            // 日付情報をセット
            void SetDays(ref SinMeiDataModel sinMei, SinMeiDataSet sinData)
            {
                if (sinMei.LastRowKbn == 1 || new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter}.Contains(mode))
                {
                    if (!(new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard }.Contains(mode)))
                    {
                        sinMei.Day1 = sinData.SinKoui.Day1;
                        sinMei.Day2 = sinData.SinKoui.Day2;
                        sinMei.Day3 = sinData.SinKoui.Day3;
                        sinMei.Day4 = sinData.SinKoui.Day4;
                        sinMei.Day5 = sinData.SinKoui.Day5;
                        sinMei.Day6 = sinData.SinKoui.Day6;
                        sinMei.Day7 = sinData.SinKoui.Day7;
                        sinMei.Day8 = sinData.SinKoui.Day8;
                        sinMei.Day9 = sinData.SinKoui.Day9;
                        sinMei.Day10 = sinData.SinKoui.Day10;
                        sinMei.Day11 = sinData.SinKoui.Day11;
                        sinMei.Day12 = sinData.SinKoui.Day12;
                        sinMei.Day13 = sinData.SinKoui.Day13;
                        sinMei.Day14 = sinData.SinKoui.Day14;
                        sinMei.Day15 = sinData.SinKoui.Day15;
                        sinMei.Day16 = sinData.SinKoui.Day16;
                        sinMei.Day17 = sinData.SinKoui.Day17;
                        sinMei.Day18 = sinData.SinKoui.Day18;
                        sinMei.Day19 = sinData.SinKoui.Day19;
                        sinMei.Day20 = sinData.SinKoui.Day20;
                        sinMei.Day21 = sinData.SinKoui.Day21;
                        sinMei.Day22 = sinData.SinKoui.Day22;
                        sinMei.Day23 = sinData.SinKoui.Day23;
                        sinMei.Day24 = sinData.SinKoui.Day24;
                        sinMei.Day25 = sinData.SinKoui.Day25;
                        sinMei.Day26 = sinData.SinKoui.Day26;
                        sinMei.Day27 = sinData.SinKoui.Day27;
                        sinMei.Day28 = sinData.SinKoui.Day28;
                        sinMei.Day29 = sinData.SinKoui.Day29;
                        sinMei.Day30 = sinData.SinKoui.Day30;
                        sinMei.Day31 = sinData.SinKoui.Day31;

                        sinMei.Day1Add = sinData.SinKoui.Day1Add;
                        sinMei.Day2Add = sinData.SinKoui.Day2Add;
                        sinMei.Day3Add = sinData.SinKoui.Day3Add;
                        sinMei.Day4Add = sinData.SinKoui.Day4Add;
                        sinMei.Day5Add = sinData.SinKoui.Day5Add;
                        sinMei.Day6Add = sinData.SinKoui.Day6Add;
                        sinMei.Day7Add = sinData.SinKoui.Day7Add;
                        sinMei.Day8Add = sinData.SinKoui.Day8Add;
                        sinMei.Day9Add = sinData.SinKoui.Day9Add;
                        sinMei.Day10Add = sinData.SinKoui.Day10Add;
                        sinMei.Day11Add = sinData.SinKoui.Day11Add;
                        sinMei.Day12Add = sinData.SinKoui.Day12Add;
                        sinMei.Day13Add = sinData.SinKoui.Day13Add;
                        sinMei.Day14Add = sinData.SinKoui.Day14Add;
                        sinMei.Day15Add = sinData.SinKoui.Day15Add;
                        sinMei.Day16Add = sinData.SinKoui.Day16Add;
                        sinMei.Day17Add = sinData.SinKoui.Day17Add;
                        sinMei.Day18Add = sinData.SinKoui.Day18Add;
                        sinMei.Day19Add = sinData.SinKoui.Day19Add;
                        sinMei.Day20Add = sinData.SinKoui.Day20Add;
                        sinMei.Day21Add = sinData.SinKoui.Day21Add;
                        sinMei.Day22Add = sinData.SinKoui.Day22Add;
                        sinMei.Day23Add = sinData.SinKoui.Day23Add;
                        sinMei.Day24Add = sinData.SinKoui.Day24Add;
                        sinMei.Day25Add = sinData.SinKoui.Day25Add;
                        sinMei.Day26Add = sinData.SinKoui.Day26Add;
                        sinMei.Day27Add = sinData.SinKoui.Day27Add;
                        sinMei.Day28Add = sinData.SinKoui.Day28Add;
                        sinMei.Day29Add = sinData.SinKoui.Day29Add;
                        sinMei.Day30Add = sinData.SinKoui.Day30Add;
                        sinMei.Day31Add = sinData.SinKoui.Day31Add;
                    }
                    else
                    {
                        sinMei.Day1 = sinData.Day1;
                        sinMei.Day2 = sinData.Day2;
                        sinMei.Day3 = sinData.Day3;
                        sinMei.Day4 = sinData.Day4;
                        sinMei.Day5 = sinData.Day5;
                        sinMei.Day6 = sinData.Day6;
                        sinMei.Day7 = sinData.Day7;
                        sinMei.Day8 = sinData.Day8;
                        sinMei.Day9 = sinData.Day9;
                        sinMei.Day10 = sinData.Day10;
                        sinMei.Day11 = sinData.Day11;
                        sinMei.Day12 = sinData.Day12;
                        sinMei.Day13 = sinData.Day13;
                        sinMei.Day14 = sinData.Day14;
                        sinMei.Day15 = sinData.Day15;
                        sinMei.Day16 = sinData.Day16;
                        sinMei.Day17 = sinData.Day17;
                        sinMei.Day18 = sinData.Day18;
                        sinMei.Day19 = sinData.Day19;
                        sinMei.Day20 = sinData.Day20;
                        sinMei.Day21 = sinData.Day21;
                        sinMei.Day22 = sinData.Day22;
                        sinMei.Day23 = sinData.Day23;
                        sinMei.Day24 = sinData.Day24;
                        sinMei.Day25 = sinData.Day25;
                        sinMei.Day26 = sinData.Day26;
                        sinMei.Day27 = sinData.Day27;
                        sinMei.Day28 = sinData.Day28;
                        sinMei.Day29 = sinData.Day29;
                        sinMei.Day30 = sinData.Day30;
                        sinMei.Day31 = sinData.Day31;

                        sinMei.Day1Add = sinData.Day1Add;
                        sinMei.Day2Add = sinData.Day2Add;
                        sinMei.Day3Add = sinData.Day3Add;
                        sinMei.Day4Add = sinData.Day4Add;
                        sinMei.Day5Add = sinData.Day5Add;
                        sinMei.Day6Add = sinData.Day6Add;
                        sinMei.Day7Add = sinData.Day7Add;
                        sinMei.Day8Add = sinData.Day8Add;
                        sinMei.Day9Add = sinData.Day9Add;
                        sinMei.Day10Add = sinData.Day10Add;
                        sinMei.Day11Add = sinData.Day11Add;
                        sinMei.Day12Add = sinData.Day12Add;
                        sinMei.Day13Add = sinData.Day13Add;
                        sinMei.Day14Add = sinData.Day14Add;
                        sinMei.Day15Add = sinData.Day15Add;
                        sinMei.Day16Add = sinData.Day16Add;
                        sinMei.Day17Add = sinData.Day17Add;
                        sinMei.Day18Add = sinData.Day18Add;
                        sinMei.Day19Add = sinData.Day19Add;
                        sinMei.Day20Add = sinData.Day20Add;
                        sinMei.Day21Add = sinData.Day21Add;
                        sinMei.Day22Add = sinData.Day22Add;
                        sinMei.Day23Add = sinData.Day23Add;
                        sinMei.Day24Add = sinData.Day24Add;
                        sinMei.Day25Add = sinData.Day25Add;
                        sinMei.Day26Add = sinData.Day26Add;
                        sinMei.Day27Add = sinData.Day27Add;
                        sinMei.Day28Add = sinData.Day28Add;
                        sinMei.Day29Add = sinData.Day29Add;
                        sinMei.Day30Add = sinData.Day30Add;
                        sinMei.Day31Add = sinData.Day31Add;
                    }
                }
            }
            // 自身がRp最終行なのに除外となる場合に、最後に追加した項目
            // 最終行のフラグおよび点数回数をセットする
            void SetLastKbnExclusion(int sinDataIndex, List<SinMeiDataSet> AsinData)
            {
                if (sinDataIndex >= AsinData.Count - 1 ||
                    AsinData[sinDataIndex].SinRp.RpNo != AsinData[sinDataIndex + 1].SinRp.RpNo ||
                    AsinData[sinDataIndex].SinKoui.SeqNo != AsinData[sinDataIndex + 1].SinKoui.SeqNo)
                {
                    // 最終行の場合、フラグを立てる
                    SinMeiDataModel updSinMei = sinMeis.Last();

                    if (updSinMei.SinRpNo == AsinData[sinDataIndex].SinRp.RpNo &&
                       updSinMei.SinSeqNo == AsinData[sinDataIndex].SinKoui.SeqNo)
                    {
                        updSinMei.LastRowKbn = 1;
                        SetTenKai(ref updSinMei, AsinData[sinDataIndex]);
                        SetDays(ref updSinMei, AsinData[sinDataIndex]);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// sourceにaddTextを追加する
        /// sourceにすでに文字列が入っている場合は、カンマを付加する
        /// </summary>
        /// <param name="source">元文字列</param>
        /// <param name="addText">追加文字列</param>
        /// <returns></returns>
        private string AppendText(string source, string addText)
        {
            string ret = source;
            if (ret != "")
            {
                ret += ",";
            }
            ret += addText;

            return ret;
        }

        private PtFutanKbnModel GetHokenKohiFutan(int mode, int hokenPid)
        {
            if (_ptFutanKbns == null)
            {
                _ptFutanKbns = new List<PtFutanKbnModel>();
            }
            PtFutanKbnModel ptFutanKbn = _ptFutanKbns?.FirstOrDefault(p => p.HokenPid == hokenPid) ?? null;

            if (ptFutanKbn == null)
            {
                int futanS = 0;
                List<int> futanK = new List<int> { 0, 0, 0, 0 };

                PtHokenPatternModel ptHokenPattern = _ptHokenPatternModels.FindAll(p => p.HokenPid == hokenPid).FirstOrDefault();
                if (ptHokenPattern != null)
                {
                    if (new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard }.Contains(mode))
                    {
                        // 領収証モード
                        Futan.Models.KaikeiInfModel kaikeiInf =
                            _kaikeiInfs.FindAll(p => p.HokenId == ptHokenPattern.HokenId).FirstOrDefault();

                        if (kaikeiInf != null)
                        {
                            if (kaikeiInf.HokenSbtCd == 0)
                            {
                                // 健保以外のケース
                                futanS = 1;
                            }
                            else
                            {
                                // 主保険の負担をチェック
                                if (kaikeiInf.HokenSbtCd < 500)
                                {
                                    futanS = 1;
                                }

                                // 公費の負担をチェック
                                int kohiIndex = 0;
                                List<int> kohiIds = new List<int>
                                {
                                    ptHokenPattern.Kohi1Id,
                                    ptHokenPattern.Kohi2Id,
                                    ptHokenPattern.Kohi3Id,
                                    ptHokenPattern.Kohi4Id
                                };

                                for (int i = 0; i <= 3; i++)
                                {
                                    if (kohiIds[i] > 0)
                                    {
                                        kohiIndex = CheckKohiId(kohiIds[i], kaikeiInf);

                                        if (kohiIndex >= 0 && kohiIndex <= 3)
                                        {
                                            futanK[kohiIndex] = 1;

                                            int hokenSbtKbn = ptHokenPattern.GetHokenSbtKbn(kohiIds[i]);
                                            if (!(new int[] { 5, 6 }.Contains(hokenSbtKbn)))
                                            {
                                                futanK[kohiIndex] = 2;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        // 領収証以外
                        Receipt.Models.ReceInfModel receInf =
                            _receInfs.FindAll(p => (p.HokenId == ptHokenPattern.HokenId || p.HokenId2 == ptHokenPattern.HokenId)).FirstOrDefault();

                        if (receInf != null)
                        {
                            int hokenSbtCd = ptHokenPattern.HokenSbtCd;

                            if (hokenSbtCd == 0)
                            {
                                // 健保以外のケース
                                futanS = 1;
                            }
                            else
                            {
                                // 主保険の負担をチェック
                                if (hokenSbtCd < 500)
                                {
                                    futanS = 1;
                                }

                                // 公費の負担をチェック
                                int kohiIndex = 0;
                                List<(int id, int hokenSbt)> kohiIds = new List<(int, int)>
                                    {
                                        (ptHokenPattern.Kohi1Id, ptHokenPattern.Kohi1HokenSbtKbn),
                                        (ptHokenPattern.Kohi2Id, ptHokenPattern.Kohi2HokenSbtKbn),
                                        (ptHokenPattern.Kohi3Id, ptHokenPattern.Kohi3HokenSbtKbn),
                                        (ptHokenPattern.Kohi4Id, ptHokenPattern.Kohi4HokenSbtKbn)
                                    };

                                for (int i = 0; i <= 3; i++)
                                {
                                    if (kohiIds[i].id > 0)
                                    {
                                        kohiIndex = CheckKohiIdRece(kohiIds[i].id, receInf);

                                        if (kohiIndex >= 0 && kohiIndex <= 3)
                                        {
                                            if (kohiIds[i].hokenSbt == HokenSbtKbn.Bunten)
                                            {
                                                futanK[kohiIndex] = 2;
                                            }
                                            else
                                            {
                                                futanK[kohiIndex] = 1;
                                            }
                                        }
                                    }
                                }

                                if (futanS == 0 && futanK.Any(p => p == 1) == false)
                                {
                                    // 全滅の場合
                                    if (receInf.HokenSbtCd / 100 == 5)
                                    {
                                        // とりあえず、公費１
                                        futanK[0] = 1;
                                    }
                                    else
                                    {
                                        futanS = 1;
                                    }
                                }
                            }
                        }
                    }
                }
                _ptFutanKbns.Add(new PtFutanKbnModel(hokenPid, futanS, futanK[0], futanK[1], futanK[2], futanK[3]));
                ptFutanKbn = _ptFutanKbns.Last();
            }

            return ptFutanKbn;

            #region Local Method
            // 指定の公費IDが第何公費かを判定する（会計情報用）
            int CheckKohiId(int kohiId, Futan.Models.KaikeiInfModel argKaikeiInf)
            {
                int retKohiIndex = -1;
                if (kohiId > 0)
                {
                    if (argKaikeiInf.Kohi1Id == kohiId)
                    {
                        retKohiIndex = 0;
                    }
                    else if (argKaikeiInf.Kohi2Id == kohiId)
                    {
                        retKohiIndex = 1;
                    }
                    else if (argKaikeiInf.Kohi3Id == kohiId)
                    {
                        retKohiIndex = 2;
                    }
                    else if (argKaikeiInf.Kohi4Id == kohiId)
                    {
                        retKohiIndex = 3;
                    }
                }
                return retKohiIndex;
            }

            // 指定の公費IDが第何公費かを判定する（レセ情報用）
            int CheckKohiIdRece(int kohiId, ReceInfModel argReceInf)
            {
                int retKohiIndex = -1;

                int cntIndex = -1;

                if (kohiId > 0)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        if (argReceInf.KohiReceKisai(i) == 1 && argReceInf.KohiId(i) > 0)
                        {
                            // レセに記載する公費
                            cntIndex++;

                            if (argReceInf.KohiId(i) == kohiId)
                            {
                                // 一致した場合、インデックスを返す
                                retKohiIndex = cntIndex;
                                break;
                            }
                        }
                    }
                }
                return retKohiIndex;
            }
            #endregion
        }

        /// <summary>
        /// 負担区分を取得する
        /// </summary>
        /// <param name="hokenPid"></param>
        /// <returns></returns>
        private string GetFutanKbn(int hokenPid, int mode)
        {
            string ret = "";
            if (_receFutanKbnModels == null)
            {
                _receFutanKbnModels = _ptFinder.FindReceFutanKbn(_hpId, _ptId, _sinDate / 100, SeikyuYm);
            }

            if (_receFutanKbnModels != null && _receFutanKbnModels.Any(p => p.HokenPid == hokenPid))
            {
                ret = _receFutanKbnModels?.FirstOrDefault(p => p.HokenPid == hokenPid).FutanKbnCd ?? "";
            }
            else if(mode == SinMeiMode.Receden)
            {
                _emrLogger.WriteLogMsg( this, nameof(GetFutanKbn), string.Format("負担区分が取得できません。ptId:{0}, sinYM:{1}, hokenPid:{2}", _ptId, _sinDate / 100, hokenPid));
            }
            return ret;
        }

        /// <summary>
        /// 指定の公費（第１、第２・・・）に紐づくPidを取得する
        /// </summary>
        /// <param name="Index">公費のインデックス</param>
        /// <returns></returns>
        public List<int> GetKohiPids(int Index)
        {
            List<int> ret = new List<int>();

            List<string> kohiFutanKbnList = null;

            switch (Index)
            {
                case 1:
                    kohiFutanKbnList =
                        new List<string>
                        {
                            "5", "2", "7", "H", "I", "4", "M", "N", "R", "S", "T", "V", "W", "X", "Z", "9"
                        };
                    break;
                case 2:
                    kohiFutanKbnList =
                        new List<string>
                        {
                            "6", "3", "7", "J", "K", "4", "O", "P", "R", "S", "U", "V", "W", "Y", "Z", "9"
                        };
                    break;
                case 3:
                    kohiFutanKbnList =
                        new List<string>
                        {
                            "B", "E", "H", "J", "L", "M", "O", "Q", "R", "T", "U", "V", "X", "Y", "Z", "9"
                        };
                    break;
                case 4:
                    kohiFutanKbnList =
                        new List<string>
                        {
                            "C", "G", "I", "K", "L", "N", "P", "Q", "S", "T", "U", "W", "X", "Y", "Z", "9"
                        };
                    break;
                default:
                    kohiFutanKbnList = new List<string>();
                    break;
            }

            if (_receFutanKbnModels == null)
            {
                _receFutanKbnModels = _ptFinder.FindReceFutanKbn(_hpId, _ptId, _sinDate / 100, SeikyuYm);
            }

            if (_receFutanKbnModels != null)
            {
                ret = _receFutanKbnModels.FindAll(p => kohiFutanKbnList.Contains(p.FutanKbnCd)).GroupBy(p => p.HokenPid).Select(p => p.Key).ToList();
            }

            return ret;
        }

        /// <summary>
        /// 指定の負担区分に関わる公費の番号を返す
        /// </summary>
        /// <param name="futanKbn"></param>
        /// <returns></returns>
        public List<int> FutanKbnToKohiIndex(string futanKbn)
        {
            List<int> ret = new List<int>();
            (string pattern, string futanKbn) findFutanPattern = FutanKbnConst.futanPatternls.FirstOrDefault(p => p.futanKbnCd == futanKbn);

            if (string.IsNullOrEmpty(findFutanPattern.pattern) == false && string.IsNullOrEmpty(findFutanPattern.futanKbn) == false)
            {
                for (int i = 0; i < findFutanPattern.pattern.Length; i++)
                {
                    if (findFutanPattern.pattern.Substring(i, 1) == "1")
                    {
                        ret.Add(i);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 診療Rp情報を抽出
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        /// </param>
        /// <param name="sinRpInfModels"></param>
        /// <returns></returns>
        private List<ReceSinRpInfModel> FilterSinRpInf(int mode, List<SinRpInfModel> sinRpInfModels)
        {
            List<ReceSinRpInfModel> ret = new List<ReceSinRpInfModel>();

            if(new int[] { SinMeiMode.Receden, SinMeiMode.ReceCheck, SinMeiMode.ReceTensu, SinMeiMode.ReceTensuRousai, SinMeiMode.ReceTensuAfter, SinMeiMode.RecedenAfter }.Contains(mode))
            {
                // レセプト電算
                sinRpInfModels.FindAll(p =>
                    p.IsDeleted == DeleteStatus.None &&
                    p.SanteiKbn == SanteiKbnConst.Santei &&
                    p.SinId < 90
                    ).ForEach(p => ret.Add(new ReceSinRpInfModel(p)));
                    
            }
            else if (new int[] { SinMeiMode.PaperRece , SinMeiMode.AfterCare }.Contains(mode))
            {
                // 紙レセプト
                sinRpInfModels.FindAll(p =>
                    p.IsDeleted == DeleteStatus.None &&
                    p.SanteiKbn == SanteiKbnConst.Santei &&
                    p.SinId < 90
                    ).ForEach(p => ret.Add(new ReceSinRpInfModel(p)));

            }
            else if (new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AccountingCard}.Contains(mode))
            {
                // 領収証・会計カード
                sinRpInfModels.FindAll(p =>
                    p.SanteiKbn != SanteiKbnConst.SanteiGai &&
                    p.IsDeleted == DeleteStatus.None
                    ).ForEach(p => ret.Add(new ReceSinRpInfModel(p)));
            }

            ret = ret.OrderBy(p => p.RpNo).ToList();

            return ret;
        }

        //private List<SinRpInfModel> FilterSinRpInf(int mode, List<SinRpInfModel> sinRpInfModels)
        //{
        //    List<SinRpInfModel> ret = new List<SinRpInfModel>();

        //    if (mode == SinMeiMode.Receden || mode == SinMeiMode.ReceCheck || mode == SinMeiMode.ReceTensu || mode == SinMeiMode.ReceTensuRousai)
        //    {
        //        // レセプト電算
        //        ret =
        //            new List<SinRpInfModel>(
        //                sinRpInfModels.FindAll(p =>
        //                    p.IsDeleted == DeleteStatus.None &&
        //                    p.SanteiKbn == SanteiKbnConst.Santei &&
        //                    p.SinId < 90
        //                    )
        //            );
        //    }
        //    else if (mode == SinMeiMode.PaperRece)
        //    {
        //        // 紙レセプト
        //        ret =
        //            new List<SinRpInfModel>(
        //                sinRpInfModels.FindAll(p =>
        //                    p.IsDeleted == DeleteStatus.None &&
        //                    p.SanteiKbn == SanteiKbnConst.Santei &&
        //                    p.SinId < 90
        //                    )
        //            );
        //    }
        //    else if (mode == SinMeiMode.Ryosyu)
        //    {
        //        // 領収証
        //        ret =
        //            new List<SinRpInfModel>(
        //                sinRpInfModels.FindAll(p =>
        //                    p.SanteiKbn != SanteiKbnConst.SanteiGai &&
        //                    p.IsDeleted == DeleteStatus.None
        //                    )
        //            );
        //    }

        //    ret = ret.OrderBy(p => p.RpNo).ToList();

        //    return ret;
        //}

        /// <summary>
        /// 診療行為情報を抽出
        /// 在がん医総調整行為は含める
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        /// </param>
        /// <param name="includeOutDrg"></param>
        /// <param name="sinKouiModels"></param>
        /// <returns></returns>
        private List<ReceSinKouiModel> FilterSinKoui(int mode, bool includeOutDrg, List<SinRpInfModel> sinRpInfModels, List<SinKouiModel> sinKouiModels, List<ReceSinKouiCountModel> sinKouiCountModels, int hokenId, int hokenId2)
        {
            List<ReceSinKouiModel> ret = new List<ReceSinKouiModel>();

            if (new int[] { SinMeiMode.Receden , SinMeiMode.ReceTensu, SinMeiMode.ReceTensuRousai, SinMeiMode.RecedenAfter }.Contains(mode))
            {
                // レセプト電算
                sinKouiModels.FindAll(p =>
                        (
                            (p.IsNodspRece != 1 && p.IsNodspRece != 2) ||
                            p.SyukeiSaki == ReceSyukeisaki.ZaiCyosei
                        ) &&
                        (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                        (p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) || p.HokenId == hokenId2) &&
                        p.IsDeleted == DeleteStatus.None
                    ).ForEach(p => ret.Add(new ReceSinKouiModel(p, null)));
                    

            }
            if (new int[] {  SinMeiMode.ReceTensuAfter }.Contains(mode))
            {
                // アフターケア（点数欄用）
                var groupSinKouiCount = (
                    from sinKouiCount in sinKouiCountModels
                    group sinKouiCount by
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } into sinKouiGroups
                                    //from sinKouiGroup in sinKouiGroups.DefaultIfEmpty()
                                    select new
                    {
                        sinKouiGroups.Key.HpId,
                        sinKouiGroups.Key.PtId,
                        sinKouiGroups.Key.SinYm,
                        sinKouiGroups.Key.RpNo,
                        sinKouiGroups.Key.SeqNo,
                        Count = sinKouiGroups.Sum(p => p.Count)
                    }
                );

                var sinKouiModel = (

                    from sinKoui in sinKouiModels
                    join sinRpInf in sinRpInfModels on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo } equals
                        new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo } 
                    join sinKouiCount in groupSinKouiCount on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo } equals
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } 
                    where
                    (
                        (
                            (sinKoui.IsNodspRece != 1 && sinKoui.IsNodspRece != 2) ||
                            sinKoui.SyukeiSaki == ReceSyukeisaki.ZaiCyosei
                        ) &&
                        (sinKoui.InoutKbn == 0 || sinKoui.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                        (sinKoui.HokenId == (hokenId == 0 ? sinKoui.HokenId : hokenId) || sinKoui.HokenId == hokenId2) &&
                        sinKoui.IsDeleted == DeleteStatus.None
                    )
                    select new
                    {
                        sinKoui,
                        sinRpInf,
                        Count = sinKouiCount?.Count ?? 0
                    }
                    ).ToList();

                sinKouiModel?.ForEach(
                    p => ret.Add(new ReceSinKouiModel(p.sinKoui, p.sinRpInf, p.Count))
                    );

                //sinKouiModels.FindAll(p =>
                //        (
                //            (p.IsNodspRece != 1 && p.IsNodspRece != 2) ||
                //            p.SyukeiSaki == ReceSyukeisaki.ZaiCyosei
                //        ) &&
                //        (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                //        (p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) || p.HokenId == hokenId2) &&
                //        p.IsDeleted == DeleteStatus.None
                //    ).ForEach(p => ret.Add(new ReceSinKouiModel(p, null)));


            }
            else if (new int[] { SinMeiMode.PaperRece, SinMeiMode.AfterCare }.Contains(mode))
            {
                // 紙レセプト
                sinKouiModels.FindAll(p =>
                    (
                        (p.IsNodspRece != 1 && p.IsNodspPaperRece != 1) ||
                        p.SyukeiSaki == ReceSyukeisaki.ZaiCyosei
                    ) &&
                    (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                    (p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) || p.HokenId == hokenId2) &&
                    p.IsDeleted == DeleteStatus.None
                ).ForEach(p => ret.Add(new ReceSinKouiModel(p, null)));

            }
            else if (mode == SinMeiMode.ReceCheck)
            {
                // レセチェック
                sinKouiModels.FindAll(p =>
                    (
                        (p.IsNodspRece != 1) ||
                        p.SyukeiSaki == ReceSyukeisaki.ZaiCyosei ||
                        p.DetailData.Contains($"\"{ItemCdConst.NoMeisai}\"")
                    ) &&
                    (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                    (p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) || p.HokenId == hokenId2) &&
                    p.IsDeleted == DeleteStatus.None
                ).ForEach(p => ret.Add(new ReceSinKouiModel(p, null)));
            }
            else if (mode == SinMeiMode.AccountingCard)
            {
                // 会計カード
                sinKouiModels.FindAll(p =>
                    (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                    (p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) || p.HokenId == hokenId2) &&
                    p.IsDeleted == DeleteStatus.None
                ).ForEach(p => ret.Add(new ReceSinKouiModel(p, null)));
            }
            else if (new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu }.Contains(mode))
            {
                // 領収証
                var groupSinKouiCount = (
                    from sinKouiCount in sinKouiCountModels
                    group sinKouiCount by
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } into sinKouiGroups
                        //from sinKouiGroup in sinKouiGroups.DefaultIfEmpty()
                        select new
                    {
                        sinKouiGroups.Key.HpId,
                        sinKouiGroups.Key.PtId,
                        sinKouiGroups.Key.SinYm,
                        sinKouiGroups.Key.RpNo,
                        sinKouiGroups.Key.SeqNo,
                        Count = sinKouiGroups.Sum(p => p.Count)
                    }


                    );

                var sinKouiModel = (

                    from sinKoui in sinKouiModels
                    join sinRpInf in sinRpInfModels on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo } equals
                        new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo } 
                    join sinKouiCount in groupSinKouiCount on
                        new {sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo} equals
                        new {sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo} 
                    where
                    (
                        (sinKoui.InoutKbn == 0 || sinKoui.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                        (sinKoui.HokenId == (hokenId == 0 ? sinKoui.HokenId : hokenId) || sinKoui.HokenId == hokenId2) &&
                        sinKoui.IsDeleted == DeleteStatus.None
                    )
                    select new
                    {
                        sinKoui,
                        sinRpInf,
                        Count = sinKouiCount?.Count ?? 0
                    }
                    ).ToList();

                sinKouiModel?.ForEach(
                    p => ret.Add(new ReceSinKouiModel(p.sinKoui, p.sinRpInf, p.Count))
                    );

                //sinKouiModels.FindAll(p =>
                //(p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
                //p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) 
                //).ForEach(p => ret.Add(new ReceSinKouiModel(p)));
            }

            ret = ret.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ToList();

            return ret;
        }
        //private List<SinKouiModel> FilterSinKoui(int mode, bool includeOutDrg, List<SinKouiModel> sinKouiModels, int hokenId, int hokenId2)
        //{
        //    List<SinKouiModel> ret = new List<SinKouiModel>();

        //    if (mode == SinMeiMode.Receden || mode == SinMeiMode.ReceCheck || mode == SinMeiMode.ReceTensu || mode == SinMeiMode.ReceTensuRousai)
        //    {
        //        // レセプト電算
        //        ret =
        //            new List<SinKouiModel>(
        //                sinKouiModels.FindAll(p =>
        //                        (
        //                            (p.IsNodspRece != 1 && p.IsNodspRece != 2) ||
        //                            p.SyukeiSaki == ReceSyukeisaki.ZaiCyosei
        //                        ) &&
        //                        (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
        //                        (p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) || p.HokenId == hokenId2) &&
        //                        p.IsDeleted == DeleteStatus.None
        //                    )
        //            );

        //    }
        //    else if (mode == SinMeiMode.PaperRece)
        //    {
        //        // 紙レセプト
        //        ret =
        //            new List<SinKouiModel>(
        //                sinKouiModels.FindAll(p =>
        //                    (
        //                        (p.IsNodspRece != 1 && p.IsNodspPaperRece != 1) ||
        //                        p.SyukeiSaki == ReceSyukeisaki.ZaiCyosei
        //                    ) &&
        //                    (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
        //                    p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) &&
        //                    p.IsDeleted == DeleteStatus.None
        //                )
        //            );
        //    }
        //    else if (mode == SinMeiMode.ReceCheck)
        //    {
        //        // レセチェック
        //        ret =
        //            new List<SinKouiModel>(
        //                sinKouiModels.FindAll(p =>
        //                    (
        //                        (p.IsNodspRece != 1) ||
        //                        p.SyukeiSaki == ReceSyukeisaki.ZaiCyosei
        //                    ) &&
        //                    (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
        //                    p.HokenId == (hokenId == 0 ? p.HokenId : hokenId) &&
        //                    p.IsDeleted == DeleteStatus.None
        //                )
        //            );
        //    }
        //    else if (mode == SinMeiMode.Ryosyu)
        //    {
        //        // 領収証
        //        ret =
        //            new List<SinKouiModel>(
        //                sinKouiModels.FindAll(p =>
        //                (p.InoutKbn == 0 || p.InoutKbn == (includeOutDrg == true ? 1 : 0)) &&
        //                p.HokenId == (hokenId == 0 ? p.HokenId : hokenId)
        //                )
        //            );
        //    }

        //    ret = ret.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ToList();

        //    return ret;
        //}

        /// <summary>
        /// 診療行為詳細情報をフィルタ
        /// 在がん医総調整項目は含める
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        /// </param>
        /// <param name="sinKouiDetailModels"></param>
        /// <returns></returns>
        private List<ReceSinKouiDetailModel> FilterSinKouiDetail(int mode, List<SinKouiDetailModel> sinKouiDetailModels)
        {
            List<ReceSinKouiDetailModel> ret = new List<ReceSinKouiDetailModel>();

            if (new int[] { SinMeiMode.Receden, SinMeiMode.ReceTensu , SinMeiMode.ReceTensuRousai , SinMeiMode.ReceTensuAfter, SinMeiMode.RecedenAfter }.Contains(mode))
            {
                // レセプト電算

                sinKouiDetailModels.FindAll(p =>
                        (
                            //(p.IsNodspRece != 1 && p.IsNodspRece != 2) ||
                            p.IsNodspRece != 1 ||
                            p.FmtKbn == FmtKbnConst.ZaiCyosei
                        ) &&
                        (!(new int[] { SinMeiMode.Receden, SinMeiMode.RecedenAfter}.Contains(mode)) || p.IsNodspRece != 2) &&
                        p.IsDeleted == DeleteStatus.None
                    ).ForEach(p=>ret.Add(new ReceSinKouiDetailModel(p)));


            }
            else if (new int[] { SinMeiMode.PaperRece , SinMeiMode.AfterCare }.Contains(mode))
            {
                // 紙レセプト

                sinKouiDetailModels.FindAll(p =>
                        (
                            (p.IsNodspRece != 1 && p.IsNodspPaperRece != 1) ||
                            p.FmtKbn == FmtKbnConst.ZaiCyosei
                        ) &&
                        p.IsDeleted == DeleteStatus.None
                    ).ForEach(p => ret.Add(new ReceSinKouiDetailModel(p)));

            }
            else if (mode == SinMeiMode.ReceCheck)
            {
                // レセチェック
                sinKouiDetailModels.FindAll(p =>
                        (
                            (p.IsNodspRece != 1) ||
                            p.FmtKbn == FmtKbnConst.ZaiCyosei ||
                            p.ItemCd == ItemCdConst.NoMeisai
                        ) &&
                        p.IsDeleted == DeleteStatus.None
                    ).ForEach(p => ret.Add(new ReceSinKouiDetailModel(p)));

            }
            else if (new int[] { SinMeiMode.Kaikei, SinMeiMode.AccountingCard }.Contains(mode))
            {
                // 領収証・会計カード
                sinKouiDetailModels.FindAll(p =>
                        (
                            p.IsNodspRyosyu != 1
                        ) &&
                        p.IsDeleted == DeleteStatus.None
                    ).ForEach(p => ret.Add(new ReceSinKouiDetailModel(p)));
            }
            else if (new int[] { SinMeiMode.Ryosyu }.Contains(mode))
            {
                // 領収証・会計カード
                sinKouiDetailModels.FindAll(p =>
                        (
                            p.IsNodspRyosyu == 0
                        ) &&
                        p.IsDeleted == DeleteStatus.None
                    ).ForEach(p => ret.Add(new ReceSinKouiDetailModel(p)));
            }
            ret = ret.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ThenBy(p => p.RowNo).ToList();

            return ret;
        }
        //private List<SinKouiDetailModel> FilterSinKouiDetail(int mode, List<SinKouiDetailModel> sinKouiDetailModels)
        //{
        //    List<SinKouiDetailModel> ret = new List<SinKouiDetailModel>();

        //    if (mode == SinMeiMode.Receden || mode == SinMeiMode.ReceCheck || mode == SinMeiMode.ReceTensu || mode == SinMeiMode.ReceTensuRousai)
        //    {
        //        // レセプト電算
        //        ret =
        //            new List<SinKouiDetailModel>(
        //                sinKouiDetailModels.FindAll(p =>
        //                        (
        //                            //(p.IsNodspRece != 1 && p.IsNodspRece != 2) ||
        //                            p.IsNodspRece != 1 ||
        //                            p.FmtKbn == FmtKbnConst.ZaiCyosei
        //                        ) &&
        //                        p.IsDeleted == DeleteStatus.None
        //                    )
        //            );

        //    }
        //    else if (mode == SinMeiMode.PaperRece)
        //    {
        //        // 紙レセプト
        //        ret =
        //            new List<SinKouiDetailModel>(
        //                sinKouiDetailModels.FindAll(p =>
        //                        (
        //                            (p.IsNodspRece != 1 && p.IsNodspPaperRece != 1) ||
        //                            p.FmtKbn == FmtKbnConst.ZaiCyosei
        //                        ) &&
        //                        p.IsDeleted == DeleteStatus.None
        //                    )
        //            );
        //    }
        //    else if (mode == SinMeiMode.ReceCheck)
        //    {
        //        // レセチェック
        //        ret =
        //            new List<SinKouiDetailModel>(
        //                sinKouiDetailModels.FindAll(p =>
        //                        (
        //                            (p.IsNodspRece != 1) ||
        //                            p.FmtKbn == FmtKbnConst.ZaiCyosei
        //                        ) &&
        //                        p.IsDeleted == DeleteStatus.None
        //                    )
        //            );
        //    }
        //    else if (mode == SinMeiMode.Ryosyu)
        //    {
        //        // 領収証
        //        ret =
        //            new List<SinKouiDetailModel>(
        //                sinKouiDetailModels.FindAll(p =>
        //                        (
        //                            p.IsNodspRyosyu != 1
        //                        ) &&
        //                        p.IsDeleted == DeleteStatus.None
        //                    )
        //            );
        //    }

        //    ret = ret.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ThenBy(p => p.RowNo).ToList();

        //    return ret;
        //}

        /// <summary>
        /// 診療行為カウント情報をフィルタ
        /// 領収証モードのときは来院番号を条件に加える
        /// </summary>
        /// <param name="mode">
        ///     0: レセプト電算
        ///     1: 紙レセプト
        ///     2: レセチェック
        ///     3: 領収証
        /// </param>
        /// <param name="raiinNo"></param>
        /// <param name="sinKouiCountModels"></param>
        /// <returns></returns>
        private List<ReceSinKouiCountModel> FilterSinKouiCount(int mode, List<long> raiinNos, List<SinKouiCountModel> sinKouiCountModels)
        {
            List<ReceSinKouiCountModel> ret = new List<ReceSinKouiCountModel>();

            if (new int[] { SinMeiMode.Kaikei, SinMeiMode.Ryosyu, SinMeiMode.AfterCare , SinMeiMode.ReceTensuAfter, SinMeiMode.RecedenAfter }.Contains(mode))
            {
                // 領収証モードの場合
                sinKouiCountModels.FindAll(p =>
                    raiinNos.Contains(p.RaiinNo) &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add)
                    ).ForEach(p => ret.Add(new ReceSinKouiCountModel(p)));
                  
            }
            else
            {
                // 領収証モード以外
                sinKouiCountModels.FindAll(p =>
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.None)
                    ).ForEach(p => ret.Add(new ReceSinKouiCountModel(p)));

            }

            ret = ret.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ThenBy(p => p.SinDay).ToList();

            return ret;
        }
        //private List<SinKouiCountModel> FilterSinKouiCount(int mode, List<long> raiinNos, List<SinKouiCountModel> sinKouiCountModels)
        //{
        //    List<SinKouiCountModel> ret = new List<SinKouiCountModel>();

        //    if (mode != SinMeiMode.Ryosyu)
        //    {
        //        // 領収証モード以外
        //        ret =
        //            new List<SinKouiCountModel>(
        //                sinKouiCountModels.FindAll(p =>
        //                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.None)
        //                    )
        //            );
        //    }
        //    else
        //    {
        //        // 領収証モードの場合
        //        ret =
        //            new List<SinKouiCountModel>(
        //                sinKouiCountModels.FindAll(p =>
        //                    raiinNos.Contains(p.RaiinNo) &&
        //                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add)
        //                    )
        //            );
        //    }

        //    ret = ret.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ThenBy(p => p.SinDay).ToList();

        //    return ret;
        //}

        /// <summary>
        /// 在がん医総の調整処理
        /// 調整項目を本体に合算する
        /// </summary>
        /// <param name="filteredSinRpInfs"></param>
        /// <param name="filteredSinKouis"></param>
        /// <param name="filteredSinDtls"></param>
        private void EditZaiWeek(ref List<ReceSinRpInfModel> filteredSinRpInfs, ref List<ReceSinKouiModel> filteredSinKouis, ref List<ReceSinKouiCountModel> filteredSinKouiCounts, ref List<ReceSinKouiDetailModel> filteredSinDtls, List<SinKouiDetailModel> sinDtls)
        {
            const int conNotYet = 0;
            const int conExist = 1;
            const int conDone = 2;

            // 在宅調整項目を抽出
            List<int> hokenPids = filteredSinKouis.GroupBy(p => p.HokenPid).Select(p=>p.Key).ToList();

            if (hokenPids != null && hokenPids.Any())
            {
                foreach (int hokenPid in hokenPids)
                {
                    //List<ReceSinKouiDetailModel> zaiCyoseiSinDtls = filteredSinDtls.FindAll(p => p.FmtKbn == FmtKbnConst.ZaiCyosei);
                    List<ReceSinKouiDetailModel> zaiCyoseiSinDtls = new List<ReceSinKouiDetailModel>();

                    foreach (ReceSinKouiDetailModel dtl in filteredSinDtls.FindAll(p => p.FmtKbn == FmtKbnConst.ZaiCyosei))
                    {
                        if (filteredSinKouis.Any(p => p.HpId == dtl.HpId && p.PtId == dtl.PtId && p.SinYm == dtl.SinYm && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.HokenPid == hokenPid))
                        {
                            zaiCyoseiSinDtls.Add(dtl);
                        }
                    }

                    if (zaiCyoseiSinDtls.Any())
                    {
                        // 在宅週単位計算項目を合体する
                        List<string> zaiItems = _mstFinder.GetZaiWeekCalc(_hpId, _sinDate);

                        // 調整項目が存在する場合
                        for (int i = 0; i < zaiItems.Count(); i++)
                        {
                            List<ReceSinKouiDetailModel> tgtCyoseiSinDtls =
                                zaiCyoseiSinDtls.FindAll(p => p.ItemCd == zaiItems[i]);

                            //bool firstWeek = false;
                            //bool lastWeek = false;

                            if (tgtCyoseiSinDtls.Any())
                            {
                                // 調整項目以外の項目取得
                                //List<ReceSinKouiDetailModel> zaiSinDtls = filteredSinDtls.FindAll(p => p.ItemCd == zaiItems[i] && p.FmtKbn != FmtKbnConst.ZaiCyosei);
                                List<ReceSinKouiDetailModel> zaiSinDtls = new List<ReceSinKouiDetailModel>();
                                foreach (ReceSinKouiDetailModel dtl in filteredSinDtls.FindAll(p => p.ItemCd == zaiItems[i] && p.FmtKbn != FmtKbnConst.ZaiCyosei))
                                {
                                    if (filteredSinKouis.Any(p => p.HpId == dtl.HpId && p.PtId == dtl.PtId && p.SinYm == dtl.SinYm && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.HokenPid == hokenPid))
                                    {
                                        zaiSinDtls.Add(dtl);
                                    }
                                }

                                if (zaiSinDtls.Any())
                                {
                                    HashSet<(int rpNo, int seqNo)> zaiSinKouiList = new HashSet<(int rpNo, int seqNo)>();

                                    // 対象行為のRpNo, SeqNoをリスト化
                                    foreach (ReceSinKouiDetailModel zaiSinDtl in zaiSinDtls)
                                    {
                                        zaiSinKouiList.Add((zaiSinDtl.RpNo, zaiSinDtl.SeqNo));
                                    }

                                    // 生き残る行為を取得しておく
                                    ReceSinKouiModel updZaiSinKoui = null;

                                    foreach ((int rpNo, int seqNo) in zaiSinKouiList)
                                    {
                                        updZaiSinKoui =
                                            filteredSinKouis.Find(p =>
                                            p.RpNo == rpNo &&
                                            p.SeqNo == seqNo);

                                        if (updZaiSinKoui != null)
                                        {
                                            break;
                                        }
                                    }

                                    if (updZaiSinKoui == null) return;

                                    // さらに調整項目のRpを追加
                                    foreach (ReceSinKouiDetailModel zaiSinDtl in tgtCyoseiSinDtls)
                                    {
                                        zaiSinKouiList.Add((zaiSinDtl.RpNo, zaiSinDtl.SeqNo));

                                        //// 実施日に前月末週、翌月初週を含めるか判断
                                        //if (firstWeek == false && zaiSinDtl.ItemName.EndsWith("(週４日以上 第１週分)"))
                                        //{
                                        //    firstWeek = true;
                                        //}
                                        //else if (lastWeek == false && zaiSinDtl.ItemName.EndsWith("(週４日以上 最終週分)"))
                                        //{
                                        //    lastWeek = true;
                                        //}

                                        //if (firstWeek == false && CIUtil.GetWeek(firstDateOfSinYm) != 0)
                                        //{
                                        //    if (filteredSinKouiCounts.Any(p => p.SinYm * 100 + p.SinDay <= lastDateOfFirstWeek))
                                        //    {
                                        //        firstWeek = true;
                                        //    }
                                        //}

                                        //if (lastWeek == false && CIUtil.GetWeek(lastDateOfSinYm) != 6)
                                        //{
                                        //    if (filteredSinKouiCounts.Any(p => p.SinYm * 100 + p.SinDay >= firstDateOfLastWeek))
                                        //    {
                                        //        lastWeek = true;
                                        //    }
                                        //}
                                    }

                                    foreach ((int rpNo, int seqNo) key in zaiSinKouiList)
                                    {
                                        if (key.rpNo != updZaiSinKoui.RpNo || key.seqNo != updZaiSinKoui.SeqNo)
                                        {
                                            // 行為を取得、カウント等をupdに反映
                                            List<ReceSinKouiModel> tgtSinKouis = filteredSinKouis.FindAll(p => p.RpNo == key.rpNo && p.SeqNo == key.seqNo && p.HokenPid == hokenPid);

                                            foreach (ReceSinKouiModel tgtSinKoui in tgtSinKouis)
                                            {
                                                updZaiSinKoui.Count += tgtSinKoui.Count;
                                                updZaiSinKoui.Day1 += tgtSinKoui.Day1;
                                                updZaiSinKoui.Day2 += tgtSinKoui.Day2;
                                                updZaiSinKoui.Day3 += tgtSinKoui.Day3;
                                                updZaiSinKoui.Day4 += tgtSinKoui.Day4;
                                                updZaiSinKoui.Day5 += tgtSinKoui.Day5;
                                                updZaiSinKoui.Day6 += tgtSinKoui.Day6;
                                                updZaiSinKoui.Day7 += tgtSinKoui.Day7;
                                                updZaiSinKoui.Day8 += tgtSinKoui.Day8;
                                                updZaiSinKoui.Day9 += tgtSinKoui.Day9;
                                                updZaiSinKoui.Day10 += tgtSinKoui.Day10;
                                                updZaiSinKoui.Day11 += tgtSinKoui.Day11;
                                                updZaiSinKoui.Day12 += tgtSinKoui.Day12;
                                                updZaiSinKoui.Day13 += tgtSinKoui.Day13;
                                                updZaiSinKoui.Day14 += tgtSinKoui.Day14;
                                                updZaiSinKoui.Day15 += tgtSinKoui.Day15;
                                                updZaiSinKoui.Day16 += tgtSinKoui.Day16;
                                                updZaiSinKoui.Day17 += tgtSinKoui.Day17;
                                                updZaiSinKoui.Day18 += tgtSinKoui.Day18;
                                                updZaiSinKoui.Day19 += tgtSinKoui.Day19;
                                                updZaiSinKoui.Day20 += tgtSinKoui.Day20;
                                                updZaiSinKoui.Day21 += tgtSinKoui.Day21;
                                                updZaiSinKoui.Day22 += tgtSinKoui.Day22;
                                                updZaiSinKoui.Day23 += tgtSinKoui.Day23;
                                                updZaiSinKoui.Day24 += tgtSinKoui.Day24;
                                                updZaiSinKoui.Day25 += tgtSinKoui.Day25;
                                                updZaiSinKoui.Day26 += tgtSinKoui.Day26;
                                                updZaiSinKoui.Day27 += tgtSinKoui.Day27;
                                                updZaiSinKoui.Day28 += tgtSinKoui.Day28;
                                                updZaiSinKoui.Day29 += tgtSinKoui.Day29;
                                                updZaiSinKoui.Day30 += tgtSinKoui.Day30;
                                                updZaiSinKoui.Day31 += tgtSinKoui.Day31;

                                                updZaiSinKoui.Day1Add += tgtSinKoui.Day1;
                                                updZaiSinKoui.Day2Add += tgtSinKoui.Day2;
                                                updZaiSinKoui.Day3Add += tgtSinKoui.Day3;
                                                updZaiSinKoui.Day4Add += tgtSinKoui.Day4;
                                                updZaiSinKoui.Day5Add += tgtSinKoui.Day5;
                                                updZaiSinKoui.Day6Add += tgtSinKoui.Day6;
                                                updZaiSinKoui.Day7Add += tgtSinKoui.Day7;
                                                updZaiSinKoui.Day8Add += tgtSinKoui.Day8;
                                                updZaiSinKoui.Day9Add += tgtSinKoui.Day9;
                                                updZaiSinKoui.Day10Add += tgtSinKoui.Day10;
                                                updZaiSinKoui.Day11Add += tgtSinKoui.Day11;
                                                updZaiSinKoui.Day12Add += tgtSinKoui.Day12;
                                                updZaiSinKoui.Day13Add += tgtSinKoui.Day13;
                                                updZaiSinKoui.Day14Add += tgtSinKoui.Day14;
                                                updZaiSinKoui.Day15Add += tgtSinKoui.Day15;
                                                updZaiSinKoui.Day16Add += tgtSinKoui.Day16;
                                                updZaiSinKoui.Day17Add += tgtSinKoui.Day17;
                                                updZaiSinKoui.Day18Add += tgtSinKoui.Day18;
                                                updZaiSinKoui.Day19Add += tgtSinKoui.Day19;
                                                updZaiSinKoui.Day20Add += tgtSinKoui.Day20;
                                                updZaiSinKoui.Day21Add += tgtSinKoui.Day21;
                                                updZaiSinKoui.Day22Add += tgtSinKoui.Day22;
                                                updZaiSinKoui.Day23Add += tgtSinKoui.Day23;
                                                updZaiSinKoui.Day24Add += tgtSinKoui.Day24;
                                                updZaiSinKoui.Day25Add += tgtSinKoui.Day25;
                                                updZaiSinKoui.Day26Add += tgtSinKoui.Day26;
                                                updZaiSinKoui.Day27Add += tgtSinKoui.Day27;
                                                updZaiSinKoui.Day28Add += tgtSinKoui.Day28;
                                                updZaiSinKoui.Day29Add += tgtSinKoui.Day29;
                                                updZaiSinKoui.Day30Add += tgtSinKoui.Day30;
                                                updZaiSinKoui.Day31Add += tgtSinKoui.Day31;
                                            }

                                            // 詳細を取得、rpNoとseqNoを更新
                                            List<ReceSinKouiDetailModel> tgtSinDtls = filteredSinDtls.FindAll(p => p.RpNo == key.rpNo && p.SeqNo == key.seqNo);

                                            foreach (ReceSinKouiDetailModel tgtSinDtl in tgtSinDtls)
                                            {
                                                tgtSinDtl.RpNo = updZaiSinKoui.RpNo;
                                                tgtSinDtl.SeqNo = updZaiSinKoui.SeqNo;
                                            }
                                        }
                                    }

                                    foreach ((int rpNo, int seqNo) key in zaiSinKouiList)
                                    {
                                        if (key.rpNo != updZaiSinKoui.RpNo || key.seqNo != updZaiSinKoui.SeqNo)
                                        {
                                            // 行為を削除
                                            filteredSinKouis.RemoveAll(p => p.RpNo == key.rpNo && p.SeqNo == key.seqNo);
                                        }
                                    }

                                    // 更新対象行為カウント取得
                                    List<ReceSinKouiCountModel> updZaiSinCounts =
                                        filteredSinKouiCounts.FindAll(p =>
                                            p.RpNo == updZaiSinKoui.RpNo &&
                                            p.SeqNo == updZaiSinKoui.SeqNo);

                                    foreach ((int rpNo, int seqNo) key in zaiSinKouiList)
                                    {
                                        if (key.rpNo != updZaiSinKoui.RpNo || key.seqNo != updZaiSinKoui.SeqNo)
                                        {
                                            // 行為カウントを取得、カウントをupdに反映
                                            List<ReceSinKouiCountModel> tgtSinKouiCounts = filteredSinKouiCounts.FindAll(p => p.RpNo == key.rpNo && p.SeqNo == key.seqNo);

                                            foreach (ReceSinKouiCountModel tgtSinKouiCount in tgtSinKouiCounts)
                                            {
                                                ReceSinKouiCountModel updZaiSinCount =
                                                    updZaiSinCounts.Find(p => p.RaiinNo == tgtSinKouiCount.RaiinNo);
                                                if (updZaiSinCount != null)
                                                {
                                                    updZaiSinCount.AdjCount += tgtSinKouiCount.Count;
                                                }
                                            }
                                        }
                                    }

                                    // マージした行為カウントを削除
                                    foreach ((int rpNo, int seqNo) key in zaiSinKouiList)
                                    {
                                        if (key.rpNo != updZaiSinKoui.RpNo || key.seqNo != updZaiSinKoui.SeqNo)
                                        {
                                            // 行為を削除
                                            filteredSinKouiCounts.RemoveAll(p => p.RpNo == key.rpNo && p.SeqNo == key.seqNo);
                                        }
                                        // 調整項目削除
                                        filteredSinDtls.RemoveAll(p =>p.RpNo == key.rpNo && p.SeqNo == key.seqNo && p.ItemCd == zaiItems[i] && p.FmtKbn == FmtKbnConst.ZaiCyosei);
                                    }


                                    //// 調整項目削除
                                    //filteredSinDtls.RemoveAll(p => p.ItemCd == zaiItems[i] && p.FmtKbn == FmtKbnConst.ZaiCyosei);

                                    // コメント項目調整                            
                                    //if (firstWeek || lastWeek)
                                    //{
                                    //    ReceSinKouiDetailModel updSinDtl =
                                    //        filteredSinDtls.Find(p =>
                                    //            p.RpNo == updZaiSinKoui.RpNo &&
                                    //            p.SeqNo == updZaiSinKoui.SeqNo &&
                                    //            p.ItemCd == ItemCdConst.CommentJissiRekkyoDummy);

                                    //    if (updSinDtl != null)
                                    //    {
                                    //        if (firstWeek && lastWeek)
                                    //        {
                                    //            updSinDtl.Suryo = 3;
                                    //        }
                                    //        else if (firstWeek)
                                    //        {
                                    //            updSinDtl.Suryo = 1;
                                    //        }
                                    //        else if (lastWeek)
                                    //        {
                                    //            updSinDtl.Suryo = 2;
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                        }
                    }

                    // 実施日に前月末週、翌月初週を含めるか判断
                    List<ReceSinKouiDetailModel> zaiCmts =
                        filteredSinDtls.FindAll(p =>
                            p.ItemCd == ItemCdConst.CommentJissiRekkyoZengoDummy);

                    int first = conNotYet;
                    int last = conNotYet;

                    foreach (ReceSinKouiDetailModel zaiCmt in zaiCmts)
                    {
                        if (filteredSinDtls.Any(p => p.ItemCd == zaiCmt.CmtOpt))
                        {
                            //bool first = false;
                            //bool last = false;

                            // 前月末週、翌月初週の分を算定するが、当月初週、当月末週に対象項目がなかった場合に対処
                            if (first == conNotYet &&
                                sinDtls.Any(p => p.ItemCd == zaiCmt.CmtOpt && p.FmtKbn == FmtKbnConst.ZaiCyosei && p.ItemName.EndsWith("(週４日以上 第１週分)")))
                            {
                                first = conExist;
                            }

                            if (last == conNotYet &&
                                sinDtls.Any(p => p.ItemCd == zaiCmt.CmtOpt && p.FmtKbn == FmtKbnConst.ZaiCyosei && p.ItemName.EndsWith("(週４日以上 最終週分)")))
                            {
                                last = conExist;
                            }

                            // 前月末週、翌月初週の記載が必要か判断
                            //foreach (ReceSinKouiDetailModel sinDtl in filteredSinDtls)
                            //{
                            //    if (first == false &&
                            //        filteredSinKouiCounts.Any(p =>
                            //            p.RpNo == sinDtl.RpNo && p.SeqNo == sinDtl.SeqNo && p.SinYm * 100 + p.SinDay <= lastDateOfFirstWeek))
                            //    {
                            //        first = true;
                            //    }
                            //    if (last == false &&
                            //        filteredSinKouiCounts.Any(p =>
                            //            p.RpNo == sinDtl.RpNo && p.SeqNo == sinDtl.SeqNo && p.SinYm * 100 + p.SinDay >= firstDateOfLastWeek))
                            //    {
                            //        last = true;
                            //    }

                            //    if (first && last)
                            //    {
                            //        break;
                            //    }
                            //}

                            if (first == conNotYet &&
                                filteredSinKouiCounts.Any(p =>
                                    p.RpNo == zaiCmt.RpNo && p.SeqNo == zaiCmt.SeqNo && p.SinYm * 100 + p.SinDay <= lastDateOfFirstWeek))
                            {
                                first = conExist;
                            }
                            if (last == conNotYet &&
                                filteredSinKouiCounts.Any(p =>
                                    p.RpNo == zaiCmt.RpNo && p.SeqNo == zaiCmt.SeqNo && p.SinYm * 100 + p.SinDay >= firstDateOfLastWeek))
                            {
                                last = conExist;
                            }

                            //if (first && last)
                            //{
                            //    break;
                            //}
                            
                            if (first == conExist && last == conExist)
                            {
                                zaiCmt.Suryo = 3;
                                first = conDone;
                                last = conDone;
                            }
                            else if (first == conExist)
                            {
                                zaiCmt.Suryo = 1;
                                first = conDone;
                            }
                            else if (last == conExist)
                            {
                                zaiCmt.Suryo = 2;
                                last = conDone;
                            }
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// 静脈注射、皮下筋肉内注射の調整処理
        ///// 手技、薬剤、特材のRpを1つにまとめる
        ///// </summary>
        ///// <param name="filteredSinRpInfs"></param>
        ///// <param name="filteredSinKouis"></param>
        ///// <param name="filteredSinDtls"></param>
        //private void EditCyusya(ref List<SinRpInfModel> filteredSinRpInfs, ref List<SinKouiModel> filteredSinKouis, ref List<SinKouiDetailModel> filteredSinDtls)
        //{
        //    // 電算以外の場合、静脈注射、皮下筋肉内注射行為は手技薬剤特材をひとつのRpにまとめる

        //    if (filteredSinRpInfs.Any(p => p.SinId == 31 || p.SinId == 32))
        //    {
        //        //静脈注射、皮下筋肉内注射のRpを抽出
        //        List<SinRpInfModel> cyusyaRps = filteredSinRpInfs.FindAll(p => p.SinId == 31 || p.SinId == 32);

        //        foreach (SinRpInfModel sinRp in cyusyaRps)
        //        {
        //            // 行為点数の合計
        //            double totalKouiTen = 0;
        //            bool bFirst = true;

        //            // 行為を抽出
        //            List<SinKouiModel> cyusyaKouis = filteredSinKouis.FindAll(p => p.RpNo == sinRp.RpNo);

        //            // 後で削除する行為のキーを保存する変数
        //            List<(int rpNo, int seqNo)> delKouis = new List<(int, int)>();

        //            foreach (SinKouiModel sinKoui in cyusyaKouis)
        //            {
        //                totalKouiTen += sinKoui.TotalTen;
        //                if (bFirst)
        //                {
        //                    bFirst = false;
        //                }
        //                else
        //                {
        //                    delKouis.Add((sinKoui.RpNo, sinKoui.SeqNo));
        //                }
        //            }

        //            foreach ((int rpno, int seqno) in delKouis)
        //            {
        //                filteredSinKouis.RemoveAll(p => p.RpNo == rpno && p.SeqNo == seqno);
        //            }

        //            cyusyaKouis.First().Ten = totalKouiTen;

        //            List<SinKouiDetailModel> cyusyaDtls = filteredSinDtls.FindAll(p => p.RpNo == sinRp.RpNo);
        //            foreach (SinKouiDetailModel sinDtl in cyusyaDtls)
        //            {
        //                sinDtl.SeqNo = cyusyaKouis.First().SeqNo;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 検査まるめ項目の調整処理
        /// </summary>
        /// <param name="filteredSinRpInfs"></param>
        /// <param name="filteredSinKouis"></param>
        /// <param name="filteredSinDtls"></param>
        private void EditKensaMarume(ref List<ReceSinRpInfModel> filteredSinRpInfs, ref List<ReceSinKouiModel> filteredSinKouis, ref List<ReceSinKouiDetailModel> filteredSinDtls)
        {
            if (filteredSinRpInfs.Any(p => p.SinId == 60))
            {
                List<ReceSinRpInfModel> kensaRps = filteredSinRpInfs.FindAll(p => p.SinId == 60);

                foreach (ReceSinRpInfModel sinRp in kensaRps)
                {

                    List<ReceSinKouiModel> kensaKouis = filteredSinKouis.FindAll(p => p.RpNo == sinRp.RpNo);

                    foreach (ReceSinKouiModel sinKoui in kensaKouis)
                    {
                        string itemName = "";
                        bool bFirst = true;

                        List<ReceSinKouiDetailModel> kensaDtls = filteredSinDtls.FindAll(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.FmtKbn == 1);
                        if (kensaDtls.Any())
                        {
                            // 後で削除するためのリスト
                            List<(int rpNo, int seqNo, int rowNo)> delrows = new List<(int, int, int)>();

                            foreach (ReceSinKouiDetailModel sinDtl in kensaDtls)
                            {
                                if (itemName != "") { itemName += ","; }

                                itemName += sinDtl.ItemName;
                                if (bFirst)
                                {
                                    bFirst = false;
                                }
                                else
                                {
                                    delrows.Add((sinDtl.RpNo, sinDtl.SeqNo, sinDtl.RowNo));
                                }
                            }

                            foreach ((int rpno, int seqno, int rowno) in delrows)
                            {
                                filteredSinDtls.RemoveAll(p => p.RpNo == rpno && p.SeqNo == seqno && p.RowNo == rowno);
                            }
                            kensaDtls.First().ItemName = itemName;
                        }
                    }

                }
            }
        }
        /// <summary>
        /// 検査まるめ項目で分点されたものを１つにまとめる
        /// Koui自体はまるめず、点数だけまるめるイメージ
        /// </summary>
        /// <param name="filteredSinRpInfs"></param>
        /// <param name="filteredSinKouis"></param>
        /// <param name="filteredSinDtls"></param>
        /// <param name="mode"></param>
        private void EditKensaMarumeBunten(ref List<ReceSinRpInfModel> filteredSinRpInfs, ref List<ReceSinKouiModel> filteredSinKouis, ref List<ReceSinKouiDetailModel> filteredSinDtls, int mode)
        {
            if (filteredSinRpInfs.Any(p => p.SinId == 60))
            {
                List<ReceSinRpInfModel> kensaRps = filteredSinRpInfs.FindAll(p => p.SinId == 60);

                foreach (ReceSinRpInfModel sinRp in kensaRps)
                {
                    List<ReceSinKouiModel> kensaKouis = filteredSinKouis.FindAll(p => p.RpNo == sinRp.RpNo && p.RecId == "SI");

                    if (kensaKouis.Count() > 1 && kensaKouis.GroupBy(p => p.HokenPid).Count() > 1)
                    {
                        // "SI"行為が複数ある場合、１つにまとめる
                        // ただし、集計先が複数になる場合はまとめない
                        double totalKouiTen = 0;
                        bool bFirst = true;
                        int pid = 0;
                        int commentMergeRpNo = 0;
                        int commentMergeSeqNo = 0;

                        List<(int rpNo, int seqNo)> moveKouis = new List<(int, int)>();

                        foreach (ReceSinKouiModel sinKoui in kensaKouis)
                        {
                            if (mode == SinMeiMode.Receden)
                            {
                                // レセ電の場合、コメントは上に集める
                                if (sinKoui.SeqNo == kensaKouis.First().SeqNo)
                                {
                                    commentMergeRpNo = sinKoui.RpNo;
                                    commentMergeSeqNo = sinKoui.SeqNo;
                                }
                                else if (filteredSinDtls.Any(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.RecId == "CO"))
                                {
                                    moveKouis.Add((sinKoui.RpNo, sinKoui.SeqNo));
                                }
                            }
                            else
                            {
                                if (sinKoui.SeqNo == kensaKouis.Last().SeqNo)
                                {
                                    commentMergeRpNo = sinKoui.RpNo;
                                    commentMergeSeqNo = sinKoui.SeqNo;
                                }
                                else if (filteredSinDtls.Any(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.RecId == "CO"))
                                {
                                    moveKouis.Add((sinKoui.RpNo, sinKoui.SeqNo));
                                }
                            }

                            totalKouiTen += sinKoui.Ten;
                            sinKoui.Ten = 0;
                        }

                        kensaKouis.Last().Ten = totalKouiTen;

                        // コメント移動
                        if (mode == SinMeiMode.Receden)
                        {
                            // レセ電の場合は上に集める
                            foreach ((int rpno, int seqno) in moveKouis)
                            {
                                foreach (ReceSinKouiDetailModel sinDtl in filteredSinDtls.FindAll(p => p.RpNo == rpno && p.SeqNo == seqno && p.RecId == "CO"))
                                {
                                    sinDtl.SeqNo = commentMergeSeqNo;
                                    sinDtl.RowNo = sinDtl.SeqNo * 1000 + sinDtl.RowNo;
                                }
                            }
                        }
                        else
                        {
                            // 先に集計先のコメントを下にする
                            foreach (ReceSinKouiDetailModel sinDtl in filteredSinDtls.FindAll(p => p.RpNo == commentMergeRpNo && p.SeqNo == commentMergeSeqNo && p.RecId == "CO"))
                            {
                                sinDtl.SeqNo = commentMergeSeqNo;
                                sinDtl.RowNo = sinDtl.SeqNo * 10000 + sinDtl.RowNo;
                            }

                            int adj = 0;
                            foreach ((int rpno, int seqno) in moveKouis)
                            {
                                adj++;
                                foreach (ReceSinKouiDetailModel sinDtl in filteredSinDtls.FindAll(p => p.RpNo == rpno && p.SeqNo == seqno && p.RecId == "CO"))
                                {
                                    sinDtl.SeqNo = commentMergeSeqNo;
                                    sinDtl.RowNo = sinDtl.SeqNo * adj * 1000 + sinDtl.RowNo;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 検査まるめ項目で分点されたものを１つにまとめる
        /// Koui自体はまるめず、点数だけまるめるイメージ
        /// </summary>
        /// <param name="filteredSinRpInfs"></param>
        /// <param name="filteredSinKouis"></param>
        /// <param name="filteredSinDtls"></param>
        /// <param name="mode"></param>
        private void EditKensaMarumeBuntenRece(ref List<ReceSinRpInfModel> filteredSinRpInfs, ref List<ReceSinKouiModel> filteredSinKouis, ref List<ReceSinKouiDetailModel> filteredSinDtls, int mode)
        {
            if (filteredSinRpInfs.Any(p => p.SinId == 60))
            {
                List<ReceSinRpInfModel> kensaRps = filteredSinRpInfs.FindAll(p => p.SinId == 60);

                foreach (ReceSinRpInfModel sinRp in kensaRps)
                {
                    List<ReceSinKouiModel> kensaKouis = filteredSinKouis.FindAll(p => p.RpNo == sinRp.RpNo && p.RecId == "SI");

                    if (kensaKouis.Count() > 1 && kensaKouis.GroupBy(p => p.HokenPid).Count() > 1)
                    {
                        foreach (ReceSinKouiModel sinKoui in kensaKouis)
                        {
                            sinKoui.DspZeroTenkai = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 同一RpNo内のSeqNo単位の行為をまとめる調整処理
        /// ・同一RpNo内手技(SI)の行為
        /// ・処置行為の手技と酸素
        /// ・静脈注射、皮下筋肉内注射の手技薬剤特材
        /// </summary>
        /// <param name="filteredSinRpInfs"></param>
        /// <param name="filteredSinKouis"></param>
        /// <param name="filteredSinDtls"></param>
        private void EditSameRpNoSI(ref List<ReceSinRpInfModel> filteredSinRpInfs, ref List<ReceSinKouiModel> filteredSinKouis, ref List<ReceSinKouiDetailModel> filteredSinDtls)
        {
            // 同一RpNo, SeqNo内のDtlに"SI"レコードが存在する場合、行為のRecIdを"SI"に設定する
            foreach(ReceSinKouiModel sinKoui in filteredSinKouis)
            {
                if (sinKoui.RecId != "SI")
                {
                    //if (filteredSinDtls.Any(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.RecId == "SI"))
                    //{
                    //    sinKoui.RecId = "SI";
                    //}
                    //else 
                    if (sinKoui.CdKbn == "J" && filteredSinDtls.Any(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.TenMst != null && new int[] { 2, 3, 4, 5 }.Contains(p.TenMst.SansoKbn)))
                    {
                        // 処置酸素は手技にまとめる
                        sinKoui.RecId = "SI";
                    }
                    else if (filteredSinRpInfs.Any(p => p.RpNo == sinKoui.RpNo && (p.SinId == 31 || p.SinId == 32)))
                    {
                        // 静脈注射、皮下筋肉内注射行為は手技薬剤特材をひとつのRpにまとめる
                        sinKoui.RecId = "SI";
                    }
                    else if (sinKoui.CdKbn == "E" && filteredSinDtls.Any(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.TenMst != null && (p.TenMst.SinKouiKbn == 77 || p.TenMst.ItemCd == ItemCdConst.GazoDensibaitaiHozon)))
                    {
                        // 画像フィルム
                        sinKoui.RecId = "SI";
                    }
                    //else if (filteredSinDtls.Any(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.RecId == "IY"))
                    //{
                    //    sinKoui.RecId = "IY";
                    //}
                    //else
                    //{
                    //    sinKoui.RecId = "TO";
                    //}
                }
            }


            foreach(ReceSinRpInfModel sinRp in filteredSinRpInfs)
            {
                List<ReceSinKouiModel> sinKouis = filteredSinKouis.FindAll(p => p.RpNo == sinRp.RpNo && p.RecId == "SI");

                if (sinKouis.Any())
                {
                    sinKouis = sinKouis.FindAll(p => p.HokenPid == sinKouis.First().HokenPid);
                    //if (sinKouis.Count() > 1)
                    if (sinKouis.Count() > 1 && sinKouis.GroupBy(p => p.SyukeiSaki).Count() == 1)
                    {
                        // "SI"行為が複数ある場合、１つにまとめる
                        // ただし、集計先が複数になる場合はまとめない
                        double totalKouiTen = 0;
                        bool bFirst = true;
                        int pid = 0;

                        // 後で削除する行為のキーを保存する変数
                        List<(int rpNo, int seqNo)> delKouis = new List<(int, int)>();

                        foreach (ReceSinKouiModel sinKoui in sinKouis)
                        {
                            totalKouiTen += sinKoui.Ten;
                            if (bFirst)
                            {
                                bFirst = false;
                                pid = sinKoui.HokenPid;
                            }
                            else if (sinKoui.HokenPid == pid)
                            {
                                delKouis.Add((sinKoui.RpNo, sinKoui.SeqNo));
                            }
                        }

                        foreach ((int rpno, int seqno) in delKouis)
                        {
                            filteredSinKouis.RemoveAll(p => p.RpNo == rpno && p.SeqNo == seqno);

                            foreach (ReceSinKouiDetailModel sinDtl in filteredSinDtls.FindAll(p => p.RpNo == rpno && p.SeqNo == seqno))
                            {
                                //sinDtl.SeqNo = sinKouis.First().SeqNo;
                                sinDtl.RowNo = sinDtl.SeqNo * 1000 + sinDtl.RowNo;
                                sinDtl.SeqNo = sinKouis.First().SeqNo;
                            }
                        }

                        sinKouis.First().Ten = totalKouiTen;
                    }
                }
            }
        }

        /// <summary>
        /// 初再診のまるめ処理
        /// </summary>
        /// <param name="filteredSinRpInfs"></param>
        /// <param name="filteredSinKouis"></param>
        /// <param name="filteredSinDtls"></param>
        private void EditSyosaisinMarume(int mode, ref List<ReceSinRpInfModel> filteredSinRpInfs, ref List<ReceSinKouiModel> filteredSinKouis, ref List<ReceSinKouiDetailModel> filteredSinDtls)
        {
            //if (filteredSinKouis.Any(p => p.SyukeiSaki == ReceSyukeisaki.EnSaisin))
            List<string> targetSyukeisaki =
                new List<string>
                {
                    ReceSyukeisaki.Syosin, ReceSyukeisaki.SyosinJikanNai, ReceSyukeisaki.SyosinJikanGai, ReceSyukeisaki.SyosinKyujitu, ReceSyukeisaki.SyosinSinya, ReceSyukeisaki.SyosinYasou, ReceSyukeisaki.EnSyosin,
                    ReceSyukeisaki.Saisin, ReceSyukeisaki.EnSaisin
                };

            if (filteredSinKouis.Any(p => targetSyukeisaki.Contains(p.SyukeiSaki)))
            {
                List<int> rpNos = 
                    filteredSinKouis.FindAll(p=>
                        //(p.SyukeiSaki == ReceSyukeisaki.EnSaisin || p.SyukeiSaki == ReceSyukeisaki.EnSyosin))
                        targetSyukeisaki.Contains(p.SyukeiSaki))
                    .GroupBy(p => p.RpNo)
                    .Select(p=>p.Key)
                    .ToList();

                foreach(int rpNo in rpNos)
                {
                    List<ReceSinKouiModel> sinKouis = filteredSinKouis.FindAll(p => p.RpNo == rpNo);

                    if (sinKouis.Count() > 1)
                    {
                        // "SI"行為が複数ある場合、１つにまとめる
                        double totalKouiTen = 0;
                        double totalKingaku = 0;
                        bool bFirst = true;
                        int pid = 0;

                        // 後で削除する行為のキーを保存する変数
                        List<(int rpNo, int seqNo)> delKouis = new List<(int, int)>();

                        foreach (ReceSinKouiModel sinKoui in sinKouis)
                        {
                            if (bFirst)
                            {
                                bFirst = false;
                                pid = sinKoui.HokenPid;
                            }
                            else
                            {
                                if (sinKoui.HokenPid == pid)
                                {
                                    delKouis.Add((sinKoui.RpNo, sinKoui.SeqNo));
                                }
                            }

                            if (sinKoui.HokenPid == pid)
                            {
                                if (sinKoui.EntenKbn == 0 || (new int[]{ SinMeiMode.Kaikei, SinMeiMode.Ryosyu }.Contains(mode)))
                                {
                                    totalKouiTen += sinKoui.Ten;
                                }
                                else
                                {
                                    totalKingaku += sinKoui.Ten;
                                }
                            }

                        }

                        foreach ((int rpno, int seqno) in delKouis)
                        {
                            filteredSinKouis.RemoveAll(p => p.RpNo == rpno && p.SeqNo == seqno);

                            foreach (ReceSinKouiDetailModel sinDtl in filteredSinDtls.FindAll(p => p.RpNo == rpno && p.SeqNo == seqno))
                            {
                                //sinDtl.SeqNo = sinKouis.First().SeqNo;
                                sinDtl.RowNo = sinDtl.SeqNo * 1000 + sinDtl.RowNo;
                                sinDtl.SeqNo = sinKouis.First().SeqNo;
                            }
                        }

                        sinKouis.First().Ten = totalKouiTen;
                        sinKouis.First().Kingaku = totalKingaku;

                        if(totalKouiTen>0 && totalKingaku>0)
                        {
                            // 円点混在
                            sinKouis.First().EntenKbn = 2;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 同一Rpをまとめる調整処理
        /// </summary>
        /// <param name="filteredSinRpInfs"></param>
        /// <param name="filteredSinKouis"></param>
        /// <param name="filteredSinDtls"></param>
        private void EditSameRpKouiData(ref List<ReceSinRpInfModel> filteredSinRpInfs, ref List<ReceSinKouiModel> filteredSinKouis, ref List<ReceSinKouiDetailModel> filteredSinDtls)
        {
            // SIN_KOUI.DETAIL_DATAを設定する
            foreach (ReceSinKouiModel sinKoui in filteredSinKouis)
            {
                List<ReceSinKouiDetailModel> sinDtls =
                    filteredSinDtls.FindAll(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo)
                    .OrderBy(p => p.ItemCd)
                    .ThenBy(p => p.ItemName)
                    .ThenBy(p => p.Suryo)
                    .ThenBy(p => p.Suryo2)
                    .ThenBy(p => p.FmtKbn)
                    .ThenBy(p => p.UnitCd)
                    .ThenBy(p => p.UnitName)
                    .ThenBy(p => p.Ten)
                    .ThenBy(p => p.Zei)
                    .ThenBy(p => p.IsNodspRece)
                    .ThenBy(p => p.IsNodspPaperRece)
                    .ThenBy(p => p.IsNodspRyosyu)
                    .ThenBy(p => p.CmtOpt)
                    .ThenBy(p => p.CmtCd1)
                    .ThenBy(p => p.CmtOpt1)
                    .ThenBy(p => p.CmtCd2)
                    .ThenBy(p => p.CmtOpt2)
                    .ThenBy(p => p.CmtCd3)
                    .ThenBy(p => p.CmtOpt3)
                    .ThenBy(p => p.RecId)
                    .ToList();

                string kouidata = "";
                foreach (ReceSinKouiDetailModel sinDtl in sinDtls)
                {
                    // 詳細の内容を文字列化する
                    string tmpStr = MakeDetailData(sinDtl);

                    if (tmpStr != "")
                    {
                        kouidata = AddStr(kouidata, tmpStr);
                    }
                }

                sinKoui.DetailData = kouidata;
            }

            // SIN_RP_INF.KOUI_DATAを設定する（点滴除く）
            foreach (ReceSinRpInfModel sinRpInf in filteredSinRpInfs.FindAll(p=>!(p.SinId == 33 && p.SinKouiKbn == 330)))
            {
                string ret = "";
                string tmpStr = "";

                List<ReceSinKouiModel> receSinKouis =
                    filteredSinKouis.FindAll(p => p.RpNo == sinRpInf.RpNo)
                        .OrderBy(p => p.SyukeiSaki)
                        .ThenBy(p => p.IsNodspRece)
                        .ThenBy(p => p.IsNodspPaperRece)
                        .ThenBy(p => p.InoutKbn)
                        .ThenBy(p => p.HokenPid)
                        .ThenBy(p => p.HokenId)
                        .ThenBy(p => p.JihiSbt)
                        .ThenBy(p => p.KazeiKbn)
                        .ThenBy(p => p.RecId)
                        .ThenBy(p => p.EntenKbn)
                        .ThenBy(p => p.DetailData)
                        .ThenBy(p => p.SeqNo)
                        .ToList();

                ret = MakeData("hk", sinRpInf.HokenKbn);
                ret = AddStr(ret, MakeData("st", sinRpInf.SanteiKbn));
                ret = AddStr(ret, MakeData("sid", sinRpInf.SinId));
                ret = AddStr(ret, MakeData("cdn", sinRpInf.CdNo));

                foreach (ReceSinKouiModel sinKoui in receSinKouis)
                {
                    // 詳細の内容を文字列化する
                    tmpStr = MakeKouiData(sinKoui);

                    if (tmpStr != "")
                    {
                        ret = AddStr(ret, tmpStr);
                    }
                }

                sinRpInf.KouiData = ret;
            }

            // 同一行為データを持つRpをまとめていく（点滴除く）
            List<int> delRpNos = new List<int>();

            foreach (ReceSinRpInfModel sinRp in filteredSinRpInfs.FindAll(p => !(p.SinId == 33 && p.SinKouiKbn == 330)))
            {
                if (!delRpNos.Contains(sinRp.RpNo) && string.IsNullOrEmpty(sinRp.KouiData) == false)
                {
                    // 削除予定Rpではない場合

                    // このRpと同じ行為データをもつRpを探す
                    List<ReceSinRpInfModel> sameSinRps = 
                        filteredSinRpInfs.FindAll(p => p.RpNo != sinRp.RpNo && p.KouiData == sinRp.KouiData);

                    if (sameSinRps.Any())
                    {
                        // みつかったRpに対してまとめ処理
                        foreach (ReceSinRpInfModel sameSinRp in sameSinRps)
                        {
                            // 元Rpと同一行為データのRpのSIN_KOUIを取得
                            List<ReceSinKouiModel> sameSinKouis = filteredSinKouis.FindAll(p => p.RpNo == sameSinRp.RpNo);
                            // 元RpのSIN_KOUIを取得
                            List<ReceSinKouiModel> updSinKouis = filteredSinKouis.FindAll(p => p.RpNo == sinRp.RpNo);

                            if (sameSinKouis.Count() == updSinKouis.Count())
                            {
                                // 行為の数が同じ場合だけまとめる
                                // まず、お互い、詳細データ-SEQ_NO順にソートしておく　
                                // 行為データが同じで行為レコード数も同じであれば、このキーでソートすると必ず対応するRp順に並ぶはず
                                // なので、回数など、元行為に＋していけば合体できるはず
                                sameSinKouis = sameSinKouis.OrderBy(p => p.DetailData).ThenBy(p => p.SeqNo).ToList();
                                updSinKouis = updSinKouis.OrderBy(p => p.DetailData).ThenBy(p => p.SeqNo).ToList();

                                for (int i = 0; i < updSinKouis.Count(); i++)
                                {
                                    // 各パラメータを合体する
                                    updSinKouis[i].TotalTen += sameSinKouis[i].TotalTen;
                                    updSinKouis[i].Count += sameSinKouis[i].Count;
                                    updSinKouis[i].TenColCount += sameSinKouis[i].TenColCount;
                                    updSinKouis[i].TenCount = string.Format(FormatConst.TenCount, updSinKouis[i].Ten, updSinKouis[i].Count);

                                    updSinKouis[i].Day1 += sameSinKouis[i].Day1;
                                    updSinKouis[i].Day2 += sameSinKouis[i].Day2;
                                    updSinKouis[i].Day3 += sameSinKouis[i].Day3;
                                    updSinKouis[i].Day4 += sameSinKouis[i].Day4;
                                    updSinKouis[i].Day5 += sameSinKouis[i].Day5;
                                    updSinKouis[i].Day6 += sameSinKouis[i].Day6;
                                    updSinKouis[i].Day7 += sameSinKouis[i].Day7;
                                    updSinKouis[i].Day8 += sameSinKouis[i].Day8;
                                    updSinKouis[i].Day9 += sameSinKouis[i].Day9;
                                    updSinKouis[i].Day10 += sameSinKouis[i].Day10;
                                    updSinKouis[i].Day11 += sameSinKouis[i].Day11;
                                    updSinKouis[i].Day12 += sameSinKouis[i].Day12;
                                    updSinKouis[i].Day13 += sameSinKouis[i].Day13;
                                    updSinKouis[i].Day14 += sameSinKouis[i].Day14;
                                    updSinKouis[i].Day15 += sameSinKouis[i].Day15;
                                    updSinKouis[i].Day16 += sameSinKouis[i].Day16;
                                    updSinKouis[i].Day17 += sameSinKouis[i].Day17;
                                    updSinKouis[i].Day18 += sameSinKouis[i].Day18;
                                    updSinKouis[i].Day19 += sameSinKouis[i].Day19;
                                    updSinKouis[i].Day20 += sameSinKouis[i].Day20;
                                    updSinKouis[i].Day21 += sameSinKouis[i].Day21;
                                    updSinKouis[i].Day22 += sameSinKouis[i].Day22;
                                    updSinKouis[i].Day23 += sameSinKouis[i].Day23;
                                    updSinKouis[i].Day24 += sameSinKouis[i].Day24;
                                    updSinKouis[i].Day25 += sameSinKouis[i].Day25;
                                    updSinKouis[i].Day26 += sameSinKouis[i].Day26;
                                    updSinKouis[i].Day27 += sameSinKouis[i].Day27;
                                    updSinKouis[i].Day28 += sameSinKouis[i].Day28;
                                    updSinKouis[i].Day29 += sameSinKouis[i].Day29;
                                    updSinKouis[i].Day30 += sameSinKouis[i].Day30;
                                    updSinKouis[i].Day31 += sameSinKouis[i].Day31;

                                    updSinKouis[i].MergeKeyNos.Add((sameSinKouis[i].RpNo, sameSinKouis[i].SeqNo));
                                }

                                // 初回算定微調整
                                if (sinRp.FirstDay > sameSinRp.FirstDay)
                                {
                                    sinRp.FirstDay = sameSinRp.FirstDay;
                                }

                                // 削除予定Rpに追加
                                delRpNos.Add(sameSinRp.RpNo);
                            }
                        }
                    }
                }
            }

            // 吸収されたRpを削除
            foreach (int rpno in delRpNos)
            {
                filteredSinDtls.RemoveAll(p => p.RpNo == rpno);
                filteredSinKouis.RemoveAll(p => p.RpNo == rpno);
                filteredSinRpInfs.RemoveAll(p => p.RpNo == rpno);
            }
        }

        /// <summary>
        /// コメント項目を変換する
        /// </summary>
        /// <param name="sinData"></param>
        /// <param name="sinKouiCountModels"></param>
        /// <param name="sinKouiDetailModels"></param>
        /// <returns></returns>
        private (string, string, string) ConvertComment(
            SinMeiDataSet sinData,
            List<SinKouiCountModel> sinKouiCountModels,
            List<SinKouiDetailModel> sinKouiDetailModels, int mode)
        {
            string retItemCd = "";
            string retItemName = "";
            string retCommentData = "";

            if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiRekkyoDummy)
            {
                // 実施日列挙
                (string jissiDaysComment, string prevJissiDaysComment, string nextJissiDaysComment, HashSet<int>days) =
                    GetJissiDaysComment(sinKouiCountModels, sinKouiDetailModels, sinData);

                if (jissiDaysComment != "")
                {
                    jissiDaysComment = "（" + jissiDaysComment + "日）";
                }

                retItemCd = ItemCdConst.CommentFree;
                retItemName = jissiDaysComment;
                retCommentData = retItemName;
            }
            else if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiRekkyoZengoDummy)
            {
                // 実施日列挙（前月末・翌月頭含む）
                (string jissiDaysComment, string prevJissiDaysComment, string nextJissiDaysComment, HashSet<int> days) =
                    GetJissiDaysComment(sinKouiCountModels, sinKouiDetailModels, sinData);

                if (jissiDaysComment != "")
                {
                    jissiDaysComment = "（" + jissiDaysComment + "日）";
                }

                if (prevJissiDaysComment != "")
                {
                    jissiDaysComment += "（" + prevJissiDaysComment + "）";
                }
                if (nextJissiDaysComment != "")
                {
                    jissiDaysComment += "（" + nextJissiDaysComment + "）";
                }

                retItemCd = ItemCdConst.CommentFree;
                retItemName = jissiDaysComment;
                retCommentData = retItemName;
            }
            else if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiRekkyoItemNameDummy)
            {
                // 実施日列挙 項目名付き
                (string jissiDaysComment, string prevJissiDaysComment, string nextJissiDaysComment, HashSet<int> days) =
                    GetJissiDaysComment(sinKouiCountModels, sinKouiDetailModels, sinData);

                if (jissiDaysComment != "")
                {
                    List<TenMstModel> tenMst = _mstFinder.FindTenMstByItemCd(_hpId, _sinDate, sinData.SinDtl.CmtOpt);
                    string itemName = "";
                    if (tenMst.Any())
                    {
                        itemName = tenMst.First().Name + "：";
                    }

                    jissiDaysComment = "（" + itemName + jissiDaysComment + "日）";

                    retItemCd = ItemCdConst.CommentFree;
                    retItemName = jissiDaysComment;
                    retCommentData = retItemName;
                }

            }
            else if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiNissuDummy)
            {
                // 実施日数
                //retItemCd = ItemCdConst.CommentFree;
                //retItemName = "実施日数　" + CIUtil.ToWide(sinData.sinKoui.Count.ToString()) + "日";
                //retCommentData = retItemName;
                int jissiDaysCount =
                    GetJissiDaysCount(sinKouiCountModels, sinData);

                retItemCd = ItemCdConst.CommentJissiCnt;
                TenMstModel tenMst = _mstFinder.FindTenMstByItemCd(_hpId, firstDateOfSinYm, retItemCd).FirstOrDefault();
                retCommentData = CIUtil.ToWide(jissiDaysCount.ToString());

                List<int> cmtCol = new List<int>();
                List<int> cmtLen = new List<int>();

                if (tenMst.ItemCd.Substring(0, 3) == "840")
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        cmtCol.Add(tenMst.CmtCol(i));
                        cmtLen.Add(tenMst.CmtColKeta(i));
                    }
                }

                retItemName = _mstFinder.GetCommentStr(firstDateOfSinYm, tenMst.ItemCd, cmtCol, cmtLen, tenMst.Name, tenMst.Name, ref retCommentData, false);

                //retItemName = tenMst.GetCommentStr(ref retCommentData);

            }
            else if (new List<string>
                { ItemCdConst.CommentSyosinJikanNai,
                  ItemCdConst.CommentSaisinDojitu,
                  ItemCdConst.CommentDenwaSaisin,
                  ItemCdConst.CommentDenwaSaisinDojitu }
                .Contains(sinData.SinDtl.ItemCd))
            {
                retItemCd = ItemCdConst.CommentFree;
                retItemName = sinData.SinDtl.ItemName + CIUtil.ToWide(sinData.SinKoui.Count.ToString()) + sinData.SinDtl.UnitName;
                sinData.SinDtl.UnitName = "";
                retCommentData = retItemName;
            }
            else if (sinData.SinDtl.TenMst != null && sinData.SinDtl.TenMst.BuiKbn > 0)
            {
                // 部位
                retItemCd = ItemCdConst.CommentFree;
                retItemName = sinData.SinDtl.ItemName;
                retCommentData = retItemName;
            }
            else if(sinData.SinDtl.ItemCd.StartsWith("W"))
            {
                // 特処コメント
                retItemCd = ItemCdConst.CommentFree;
                retItemName = sinData.SinDtl.ItemName;
                retCommentData = retItemName;
            }
            else if(sinData.SinDtl.TenMst != null && sinData.SinDtl.TenMst.SanteiItemCd == ItemCdConst.NoSantei)
            {
                // 算定診療行為コード未設定
                retItemCd = ItemCdConst.CommentFree;
                retItemName = sinData.SinDtl.ItemName;
                if (mode == SinMeiMode.Ryosyu && string.IsNullOrEmpty(sinData.SinDtl.RyosyuName) == false)
                {
                    retItemName = sinData.SinDtl.RyosyuName;
                }
                retCommentData = retItemName;
            }
            else
            {
                retItemCd = sinData.SinDtl.ItemCd;
                retItemName = sinData.SinDtl.ItemName;
                if(mode == SinMeiMode.Ryosyu && string.IsNullOrEmpty(sinData.SinDtl.RyosyuName) == false)
                {
                    retItemName = sinData.SinDtl.RyosyuName;
                }
                retCommentData = sinData.SinDtl.CmtOpt;
                if(sinData.SinDtl.ItemCd.StartsWith("850"))
                {
                    if(retCommentData.Length < 7)
                    {
                        retCommentData = retCommentData.PadRight(7, '０');
                    }
                }
                else if (sinData.SinDtl.ItemCd.StartsWith("852"))
                {
                    if (retCommentData.Length < 5)
                    {
                        retCommentData = retCommentData.PadLeft(5, '０');
                    }
                }
            }

            return (retItemCd, retItemName, retCommentData);
        }

        private List<RecedenCmtSelectModel> _recedenCmtSelects = null;
        /// <summary>
        /// レセ電コメント関連テーブル
        /// </summary>
        private List<RecedenCmtSelectModel> RecedenCmtSelects
        {
            get
            {
                if (_recedenCmtSelects == null)
                {
                    _recedenCmtSelects = _mstFinder.FindRecedenCmtSelect(_hpId, _sinDate);
                }
                return _recedenCmtSelects;
            }
        }

        /// <summary>
        /// コメント項目を変換する
        /// </summary>
        /// <param name="sinData"></param>
        /// <param name="sinKouiCountModels"></param>
        /// <param name="sinKouiDetailModels"></param>
        /// <param name="commentExist">コメント項目が既に存在していた場合はtrueを返す</param>
        /// <returns>コメント関連テーブルに紐づいたコメント項目に変換したものを返す</returns>
        private List<CmtRecordData> ConvertTenkaiComment(
            SinMeiDataSet sinData,
            List<SinKouiCountModel> sinKouiCountModels,
            List<SinKouiDetailModel> sinKouiDetailModels, ref bool commentExist)
        {
            List<CmtRecordData> results = new List<CmtRecordData>();
            commentExist = false;

            if (RecedenCmtSelects.Any(p => p.ItemCd == sinData.SinDtl.CmtOpt))
            {

                if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiRekkyoDummy && 
                    RecedenCmtSelects.Any(p => p.ItemCd == sinData.SinDtl.CmtOpt && p.CmtSbt == CmtSbtConst.JissiBi))
                {
                    // 実施日列挙
                    (string jissiDaysComment, string prevJissiDaysComment, string nextJissiDaysComment, HashSet<int> days) =
                        GetJissiDaysComment(sinKouiCountModels, sinKouiDetailModels, sinData);

                    string retCommentData = "";
                    RecedenCmtSelectModel recedenCmtSelect =
                        RecedenCmtSelects.Find(p => p.ItemCd == sinData.SinDtl.CmtOpt && p.CmtSbt == CmtSbtConst.JissiBi);

                    if (recedenCmtSelect == null)
                    {
                        if (jissiDaysComment != "")
                        {
                            jissiDaysComment = "（" + jissiDaysComment + "日）";
                        }

                        results.Add(
                            new CmtRecordData(
                                ItemCdConst.CommentFree,
                                jissiDaysComment,
                                jissiDaysComment));
                    }
                    else
                    {
                        //if (filteredSinDtls.Any(p => p.ItemCd == recedenCmtSelect.CommentCd))
                        //{
                        //    commentExist = true;
                        //}
                        //else
                        //{
                        if (days.Any())
                        {
                            foreach (int day in days)
                            {
                                if (day >= 100)
                                {
                                    retCommentData = CIUtil.ToWide(CIUtil.SDateToWDate(day).ToString());
                                }
                                else
                                {
                                    retCommentData = CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate / 100 * 100 + day).ToString());
                                }

                                if (filteredSinDtls.Any(p => p.ItemCd == recedenCmtSelect.CommentCd && p.CmtOpt == retCommentData))
                                {
                                    commentExist = true;
                                }
                                else
                                {

                                    results.Add(
                                    new CmtRecordData(
                                        recedenCmtSelect.CommentCd,
                                        _mstFinder.GetCommentStr(
                                            firstDateOfSinYm, recedenCmtSelect.CommentCd,
                                            recedenCmtSelect.CmtCols,
                                            recedenCmtSelect.CmtColKetas,
                                            recedenCmtSelect.Name,
                                            recedenCmtSelect.Name,
                                            ref retCommentData, false),
                                        retCommentData));
                                }
                            }
                        }
                    }
                    //}
                }
                else if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiRekkyoZengoDummy)
                {
                    // 実施日列挙（前月末・翌月頭含む）
                    (string jissiDaysComment, string prevJissiDaysComment, string nextJissiDaysComment, HashSet<int> days) =
                        GetJissiDaysComment(sinKouiCountModels, sinKouiDetailModels, sinData);

                    string retCommentData = "";
                    RecedenCmtSelectModel recedenCmtSelect =
                        RecedenCmtSelects.Find(p => p.ItemCd == sinData.SinDtl.CmtOpt && p.CmtSbt == CmtSbtConst.JissiBi);

                    if (recedenCmtSelect == null)
                    {
                        if (jissiDaysComment != "")
                        {
                            jissiDaysComment = "（" + jissiDaysComment + "日）";
                        }

                        if (prevJissiDaysComment != "")
                        {
                            jissiDaysComment += "（" + prevJissiDaysComment + "）";
                        }
                        if (nextJissiDaysComment != "")
                        {
                            jissiDaysComment += "（" + nextJissiDaysComment + "）";
                        }

                        results.Add(
                            new CmtRecordData(
                                ItemCdConst.CommentFree,
                                jissiDaysComment,
                                jissiDaysComment));
                    }
                    else

                    {
                        //if (filteredSinDtls.Any(p => p.ItemCd == recedenCmtSelect.CommentCd))
                        //{
                        //    commentExist = true;
                        //}
                        //else
                        //{
                        if (days.Any())
                        {
                            foreach (int day in days)
                            {
                                if (day >= 100)
                                {
                                    retCommentData = CIUtil.ToWide(CIUtil.SDateToWDate(day).ToString());
                                }
                                else
                                {
                                    retCommentData = CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate / 100 * 100 + day).ToString());
                                }

                                if (filteredSinDtls.Any(p => p.ItemCd == recedenCmtSelect.CommentCd && p.CmtOpt == retCommentData))
                                {
                                    commentExist = true;
                                }
                                else
                                {
                                    results.Add(
                                        new CmtRecordData(
                                            recedenCmtSelect.CommentCd,
                                            _mstFinder.GetCommentStr(
                                                firstDateOfSinYm, recedenCmtSelect.CommentCd,
                                                recedenCmtSelect.CmtCols,
                                                recedenCmtSelect.CmtColKetas,
                                                recedenCmtSelect.Name,
                                                recedenCmtSelect.Name,
                                                ref retCommentData, false),
                                            retCommentData));
                                }
                            }
                        }
                    }
                    //}
                }
                else if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiRekkyoItemNameDummy)
                {
                    // 実施日列挙 項目名付き
                    (string jissiDaysComment, string prevJissiDaysComment, string nextJissiDaysComment, HashSet<int> days) =
                        GetJissiDaysComment(sinKouiCountModels, sinKouiDetailModels, sinData);

                    string retCommentData = "";
                    RecedenCmtSelectModel recedenCmtSelect =
                        RecedenCmtSelects.Find(p => p.ItemCd == sinData.SinDtl.CmtOpt && p.CmtSbt == CmtSbtConst.JissiBi);

                    if (recedenCmtSelect == null)
                    {
                        if (jissiDaysComment != "")
                        {
                            List<TenMstModel> tenMst = _mstFinder.FindTenMstByItemCd(_hpId, _sinDate, sinData.SinDtl.CmtOpt);
                            string itemName = "";
                            if (tenMst.Any())
                            {
                                itemName = tenMst.First().Name + "：";
                            }

                            jissiDaysComment = "（" + itemName + jissiDaysComment + "日）";

                            results.Add(
                                new CmtRecordData(
                                    ItemCdConst.CommentFree,
                                    jissiDaysComment,
                                    jissiDaysComment));
                        }
                    }
                    else
                    {
                        //if (filteredSinDtls.Any(p => p.ItemCd == recedenCmtSelect.CommentCd))
                        //{
                        //    commentExist = true;
                        //}
                        //else
                        //{
                        if (days.Any())
                        {
                            foreach (int day in days)
                            {
                                if (day >= 100)
                                {
                                    retCommentData = CIUtil.ToWide(CIUtil.SDateToWDate(day).ToString());
                                }
                                else
                                {
                                    retCommentData = CIUtil.ToWide(CIUtil.SDateToWDate(_sinDate / 100 * 100 + day).ToString());
                                }

                                if (filteredSinDtls.Any(p => p.ItemCd == recedenCmtSelect.CommentCd && p.CmtOpt == retCommentData))
                                {
                                    commentExist = true;
                                }
                                else
                                {
                                    results.Add(
                                    new CmtRecordData(
                                        recedenCmtSelect.CommentCd,
                                        _mstFinder.GetCommentStr(
                                            firstDateOfSinYm, recedenCmtSelect.CommentCd,
                                            recedenCmtSelect.CmtCols,
                                            recedenCmtSelect.CmtColKetas,
                                            recedenCmtSelect.Name,
                                            recedenCmtSelect.Name,
                                            ref retCommentData, false),
                                        retCommentData));
                                }
                            }
                        }
                    }
                    //}
                }
            }

            return results;
        }
        /// <summary>
        /// 実施日コメント取得
        /// </summary>
        /// <param name="sinKouiCountModels">診療行為カウント</param>
        /// <param name="sinKouiDetailModels">診療行為詳細</param>
        /// <param name="sinData">診療データ</param>
        /// <returns></returns>
        private (string jissiDaysComment, string prevJissiDaysComment, string nextJissiDaysComment, HashSet<int> days)
            GetJissiDaysComment(List<SinKouiCountModel> sinKouiCountModels, List<SinKouiDetailModel> sinKouiDetailModels, SinMeiDataSet sinData)
        {
            string jissiDaysComment = "";
            string prevJissiDaysComment = "";
            string nextJissiDaysComment = "";

            // 実施日取り消し項目
            List<ReceSinKouiDetailModel> cancelItems = new List<ReceSinKouiDetailModel>();

            sinKouiDetailModels.FindAll(p =>
                p.ItemCd == ItemCdConst.HoumonCommentCancel)
            .ForEach(p => cancelItems.Add(new ReceSinKouiDetailModel(p)));

            HashSet<int> uqCancelDays = new HashSet<int>();

            foreach (ReceSinKouiDetailModel cancelItem in cancelItems)
            {
                List<int> cancelDays = sinKouiCountModels.FindAll(p =>
                    p.RpNo == cancelItem.RpNo &&
                    p.SeqNo == cancelItem.SeqNo &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add))
                    .GroupBy(p => p.SinDay)
                    .Select(p => p.Key).ToList();
                if (cancelDays.Any())
                {
                    foreach (int cancelDay in cancelDays)
                    {
                        uqCancelDays.Add(cancelDay);
                    }
                }
            }

            //List<SinKouiDetailModel> jissiItems =
            //            sinKouiDetailModels.FindAll(p =>
            //                p.ItemCd == sinData.sinDtl.CmtOpt && p.IsNodspRece != 1 && p.IsNodspPaperRece != 1);

            HashSet<int> uqJissiDays = new HashSet<int>();

            //foreach (SinKouiDetailModel jissiItem in jissiItems)
            //{
            //    List<int> jissiDays = sinKouiCountModels.FindAll(p =>
            //        p.RpNo == jissiItem.RpNo &&
            //        p.SeqNo == jissiItem.SeqNo &&
            //        (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.None))
            //        .GroupBy(p => p.SinDay)
            //        .Select(p => p.Key)
            //        .OrderBy(p => p)
            //        .ToList();
            //    if (jissiDays.Any())
            //    {
            //        foreach (int jissiDay in jissiDays)
            //        {
            //            uqJissiDays.Add(jissiDay);
            //        }
            //    }
            //}

            List<int> jissiDays = sinKouiCountModels.FindAll(p =>
                p.RpNo == sinData.SinKoui.RpNo &&
                p.SeqNo == sinData.SinKoui.SeqNo &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add))
                .GroupBy(p => p.SinDay)
                .Select(p => p.Key)
                .OrderBy(p => p)
                .ToList();
            if (jissiDays.Any())
            {
                foreach (int jissiDay in jissiDays)
                {
                    if (uqCancelDays.Any(p => p == jissiDay) == false)
                    {
                        uqJissiDays.Add(jissiDay);
                    }
                }
            }

            // マージされたRpの分
            foreach((int rpno, int seqno) in sinData.SinKoui.MergeKeyNos)
            {
                jissiDays = sinKouiCountModels.FindAll(p =>
                    p.RpNo == rpno &&
                    p.SeqNo == seqno &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add))
                    .GroupBy(p => p.SinDay)
                    .Select(p => p.Key)
                    .OrderBy(p => p)
                    .ToList();
                if (jissiDays.Any())
                {
                    foreach (int jissiDay in jissiDays)
                    {
                        if (uqCancelDays.Any(p => p == jissiDay) == false)
                        {
                            uqJissiDays.Add(jissiDay);
                        }
                    }
                }
            }

            if (uqJissiDays.Any())
            {
                foreach (int uqJissiDay in uqJissiDays)
                {
                    if (uqCancelDays.Any(p => p == uqJissiDay) == false)
                    {
                        if (jissiDaysComment != "") { jissiDaysComment += ","; }
                        jissiDaysComment += CIUtil.ToWide(uqJissiDay.ToString());
                    }
                }

            }

            if (sinData.SinDtl.ItemCd == ItemCdConst.CommentJissiRekkyoZengoDummy && sinData.SinDtl.Suryo > 0)
            {
                // 前月翌月含む場合

                // 在がん医総訪問日コメント取消しを算定している日を取得
                List<SanteiDaysModel> cancelDays = _santeiFinder.GetSanteiDays(
                    _hpId, _ptId, firstDateOfFirstWeek, lastDateOfLastWeek, 0, _sinDate, ItemCdConst.HoumonCommentCancel, sinData.SinRp.HokenKbn);

                // 数量=1: 前月末週も取得、数量=2: 翌月初週も取得、数量=3: 前月末週と翌月初週両方取得
                if ((firstDateOfFirstWeek / 100 != _sinDate / 100) && (sinData.SinDtl.Suryo == 1 || sinData.SinDtl.Suryo == 3))
                {
                    // 前月末週の実施日も取得する
                    List<SanteiDaysModel> santeiDays = _santeiFinder.GetSanteiDays(
                        _hpId, _ptId, firstDateOfFirstWeek, CIUtil.GetLastDateOfMonth(firstDateOfFirstWeek), 0, _sinDate, sinData.SinDtl.CmtOpt, sinData.SinRp.HokenKbn, sinData.SinKoui.HokenId);

                    if (santeiDays.Any())
                    {
                        prevJissiDaysComment = MakeDaysComment(santeiDays, cancelDays);
                    }
                }

                if ((lastDateOfLastWeek / 100 != _sinDate / 100) && (sinData.SinDtl.Suryo == 2 || sinData.SinDtl.Suryo == 3))
                {
                    // 翌月末週の実施日も取得する
                    List<SanteiDaysModel> santeiDays = _santeiFinder.GetSanteiDays(
                        _hpId, _ptId, lastDateOfLastWeek / 100 * 100 + 1, lastDateOfLastWeek, 0, _sinDate, sinData.SinDtl.CmtOpt, sinData.SinRp.HokenKbn, sinData.SinKoui.HokenId);

                    if (santeiDays.Any())
                    {
                        nextJissiDaysComment = MakeDaysComment(santeiDays, cancelDays);
                    }
                }
            }


            return (jissiDaysComment, prevJissiDaysComment, nextJissiDaysComment, uqJissiDays);

            #region Local Method

            // 数値yyyymmddを、文字列mm/ddに変換
            string GetMMDD(int baseDate)
            {
                return string.Format("{0}/{1}", baseDate % 10000 / 100, baseDate % 100);
            }

            // 実施日の羅列文字列を生成
            string MakeDaysComment(List<SanteiDaysModel> Days, List<SanteiDaysModel> excludeDays)
            {
                string retDays = "";

                foreach (SanteiDaysModel Day in Days)
                {
                    if (excludeDays.Any(p => p.SinDate == Day.SinDate) == false)
                    {
                        if (retDays != "") { retDays += ","; }
                        retDays +=
                            CIUtil.ToWide(GetMMDD(Day.SinDate));
                        uqJissiDays.Add(Day.SinDate);

                    }
                }
                return retDays;
            }
            #endregion
        }

        /// <summary>
        /// 実施日数取得
        /// </summary>
        /// <param name="sinKouiCountModels">診療行為カウント</param>
        /// <param name="sinKouiDetailModels">診療行為詳細</param>
        /// <param name="sinData">診療データ</param>
        /// <returns></returns>
        private int
            GetJissiDaysCount(List<SinKouiCountModel> sinKouiCountModels, SinMeiDataSet sinData)
        {
            HashSet<int> uqJissiDays = new HashSet<int>();

            List<int> jissiDays = sinKouiCountModels.FindAll(p =>
                p.RpNo == sinData.SinKoui.RpNo &&
                p.SeqNo == sinData.SinKoui.SeqNo &&
                //(p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.None))
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add))
                .GroupBy(p => p.SinDay)
                .Select(p => p.Key)
                .OrderBy(p => p)
                .ToList();
            if (jissiDays.Any())
            {
                foreach (int jissiDay in jissiDays)
                {
                    uqJissiDays.Add(jissiDay);
                }
            }

            // マージされたRpの分
            foreach ((int rpno, int seqno) in sinData.SinKoui.MergeKeyNos)
            {
                jissiDays = sinKouiCountModels.FindAll(p =>
                    p.RpNo == rpno &&
                    p.SeqNo == seqno &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add))
                    .GroupBy(p => p.SinDay)
                    .Select(p => p.Key)
                    .OrderBy(p => p)
                    .ToList();
                if (jissiDays.Any())
                {
                    foreach (int jissiDay in jissiDays)
                    {
                        uqJissiDays.Add(jissiDay);
                    }
                }
            }
            return uqJissiDays.Count();
        }
    
        public List<SinMeiDataModel> SinMei
        {
            get { return sinMeis; }
        }

        /// <summary>
        /// データをファイルに書き出す（テスト用）
        /// </summary>
        /// <param name="Path"></param>
        public void SaveToFile(string Path)
        {
            string ret = "";
            foreach (SinMeiDataModel sinMei in sinMeis)
            {
                ret += sinMei.RecId + "\t";
                ret += GetStringZeroIgnore(sinMei.SinId) + "\t";
                ret += sinMei.FutanKbn.ToString() + "\t";
                ret += sinMei.ItemCd + "\t";
                ret += sinMei.ItemName + "\t";
                ret += sinMei.CommentData + "\t";
                ret += sinMei.Suryo.ToString() + "\t";
                ret += sinMei.TotalTen.ToString() + "\t";
                ret += sinMei.TotalKingaku.ToString() + "\t";
                ret += sinMei.Count.ToString() + "\t";
                ret += sinMei.TenKai + "\t";
                ret += sinMei.UnitCd.ToString() + "\t";
                ret += sinMei.UnitName + "\t";
                ret += sinMei.Price.ToString() + "\t";
                ret += sinMei.TokuzaiName + "\t";
                ret += sinMei.ProductName + "\t";
                ret += sinMei.CommentCd1 + "\t";
                ret += sinMei.CommentData1 + "\t";
                ret += sinMei.Comment1 + "\t";
                ret += sinMei.CommentCd2 + "\t";
                ret += sinMei.CommentData2 + "\t";
                ret += sinMei.Comment2 + "\t";
                ret += sinMei.CommentCd3 + "\t";
                ret += sinMei.CommentData3 + "\t";
                ret += sinMei.Comment3 + "\t";
                ret += sinMei.Day1.ToString() + "\t";
                ret += sinMei.Day2.ToString() + "\t";
                ret += sinMei.Day3.ToString() + "\t";
                ret += sinMei.Day4.ToString() + "\t";
                ret += sinMei.Day5.ToString() + "\t";
                ret += sinMei.Day6.ToString() + "\t";
                ret += sinMei.Day7.ToString() + "\t";
                ret += sinMei.Day8.ToString() + "\t";
                ret += sinMei.Day9.ToString() + "\t";
                ret += sinMei.Day10.ToString() + "\t";
                ret += sinMei.Day11.ToString() + "\t";
                ret += sinMei.Day12.ToString() + "\t";
                ret += sinMei.Day13.ToString() + "\t";
                ret += sinMei.Day14.ToString() + "\t";
                ret += sinMei.Day15.ToString() + "\t";
                ret += sinMei.Day16.ToString() + "\t";
                ret += sinMei.Day17.ToString() + "\t";
                ret += sinMei.Day18.ToString() + "\t";
                ret += sinMei.Day19.ToString() + "\t";
                ret += sinMei.Day20.ToString() + "\t";
                ret += sinMei.Day21.ToString() + "\t";
                ret += sinMei.Day22.ToString() + "\t";
                ret += sinMei.Day23.ToString() + "\t";
                ret += sinMei.Day24.ToString() + "\t";
                ret += sinMei.Day25.ToString() + "\t";
                ret += sinMei.Day26.ToString() + "\t";
                ret += sinMei.Day27.ToString() + "\t";
                ret += sinMei.Day28.ToString() + "\t";
                ret += sinMei.Day29.ToString() + "\t";
                ret += sinMei.Day30.ToString() + "\t";
                ret += sinMei.Day31.ToString() + "\t";
                ret += sinMei.HokenPid.ToString() + "\t";
                ret += sinMei.RpNo.ToString() + "\t";
                ret += sinMei.SeqNo.ToString() + "\t";
                ret += sinMei.RowNo.ToString() + "\t";
                ret += sinMei.CdKbn + "\t";
                ret += sinMei.JihiSbt.ToString() + "\t";
                ret += sinMei.FutanS.ToString() + "\t";
                ret += sinMei.FutanK1.ToString() + "\t";
                ret += sinMei.FutanK2.ToString() + "\t";
                ret += sinMei.FutanK3.ToString() + "\t";
                ret += sinMei.FutanK4.ToString() + "\t";

                ret += sinMei.LastRowKbn.ToString() + "\t";
                ret += sinMei.SanteiKbn.ToString() + "\t";
                ret += sinMei.InOutKbn.ToString() + "\t";
                ret += "\r\n";
            }

            File.WriteAllText(Path, ret);

            #region Local Method
            string GetStringZeroIgnore(int val)
            {
                string s = "";
                if(val != 0)
                {
                    s = val.ToString();
                }
                return s;
            }
            #endregion
        }

        /// <summary>
        /// 最初の診療日
        /// </summary>
        public int FirstSyosinDate
        {
            get
            {
                return _firstSinDate;
            }
        }
        /// <summary>
        /// 最後の診療日
        /// </summary>
        public int LastSinDate
        {
            get
            {
                return _lastSinDate;
            }
        }
        /// <summary>
        /// 診療行為データモデル
        /// </summary>
        public List<ReceSinKouiModel> SinKoui
        {
            get => filteredSinKouis;
        }
        /// <summary>
        /// 診療行為回数データモデル
        /// </summary>
        public List<ReceSinKouiCountModel> SinKouiCount
        {
            get => filteredSinKouiCounts;
        }


        /// <summary>
        /// 診療行為詳細データを取得する
        /// （診療行為詳細情報の内容を文字列化したもの）
        /// </summary>
        /// <param name="sinDtl">診療行為詳細情報</param>
        /// <returns>診療行為詳細情報の内容を文字列化したもの</returns>
        private string MakeDetailData(ReceSinKouiDetailModel sinDtl)
        {
            string ret = "";

            ret = AddStr(ret, MakeData("cd", sinDtl.ItemCd));
            ret = AddStr(ret, MakeData("nm", sinDtl.ItemName));
            ret = AddStr(ret, MakeData("su", sinDtl.Suryo));
            ret = AddStr(ret, MakeData("su2", sinDtl.Suryo2));
            ret = AddStr(ret, MakeData("fmt", sinDtl.FmtKbn));
            ret = AddStr(ret, MakeData("ucd", sinDtl.UnitCd));
            ret = AddStr(ret, MakeData("ten", sinDtl.Ten));
            ret = AddStr(ret, MakeData("zei", sinDtl.Zei));
            ret = AddStr(ret, MakeData("ndr", sinDtl.IsNodspRece));
            ret = AddStr(ret, MakeData("ndp", sinDtl.IsNodspPaperRece));
            ret = AddStr(ret, MakeData("nds", sinDtl.IsNodspRyosyu));
            ret = AddStr(ret, MakeData("opt", sinDtl.CmtOpt));
            ret = AddStr(ret, MakeData("ccd1", sinDtl.CmtCd1));
            ret = AddStr(ret, MakeData("opt1", sinDtl.CmtOpt1));
            ret = AddStr(ret, MakeData("ccd2", sinDtl.CmtCd2));
            ret = AddStr(ret, MakeData("opt2", sinDtl.CmtOpt2));
            ret = AddStr(ret, MakeData("ccd3", sinDtl.CmtCd3));
            ret = AddStr(ret, MakeData("opt3", sinDtl.CmtOpt3));
            ret = AddStr(ret, MakeData("rid", sinDtl.RecId));

            if (ret != "")
            {
                ret = "{" + ret + "}";
            }

            return ret;
        }
        private string MakeKouiData(ReceSinKouiModel sinKoui)
        {
            string ret = "";

            ret = AddStr(ret, MakeData("sk", sinKoui.SyukeiSaki));
            ret = AddStr(ret, MakeData("ndrc", sinKoui.IsNodspRece));
            ret = AddStr(ret, MakeData("ndp", sinKoui.IsNodspPaperRece));
            ret = AddStr(ret, MakeData("io", sinKoui.InoutKbn));
            if (sinKoui.DetailData.Contains(ItemCdConst.CommentSaisinDojitu) ||
               sinKoui.DetailData.Contains(ItemCdConst.CommentDenwaSaisin) ||
               sinKoui.DetailData.Contains(ItemCdConst.CommentDenwaSaisinDojitu) ||
               sinKoui.DetailData.Contains(ItemCdConst.CommentSyosinJikanNai))
            {
                // 初再診のコメントの場合は１つにまとめたいのでpid,hidは不問にする
            }
            else
            {
                ret = AddStr(ret, MakeData("pid", sinKoui.HokenPid));
                ret = AddStr(ret, MakeData("hid", sinKoui.HokenId));
            }
            ret = AddStr(ret, MakeData("jihi", sinKoui.JihiSbt));
            ret = AddStr(ret, MakeData("kazei", sinKoui.KazeiKbn));
            ret = AddStr(ret, MakeData("rid", sinKoui.RecId));
            //ret = AddStr(ret, MakeData("et", sinKoui.EntenKbn));
            ret = AddStr(ret, String.Format("\"{0}\":{1}", "et", sinKoui.EntenKbn));
            ret = AddStr(ret, MakeData("rp", sinKoui.DetailData, "[", "]"));

            if (ret != "")
            {
                ret = "{" + ret + "}";
            }

            return ret;
        }

        private string AddStr(string s1, string s2)
        {
            if (s1 != "" && s2 != "")
            {
                s1 += ",";
            }
            return s1 + s2;
        }
        /// <summary>
        /// 詳細データ、行為データのキーバリューを生成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="delimiter1"></param>
        /// <param name="delimiter2"></param>
        /// <returns></returns>
        private string MakeData(string id, string data, string delimiter1 = "\"", string delimiter2 = "\"")
        {
            string ret = "";

            if (data != null && data != "")
            {
                ret = String.Format("\"{0}\":" + delimiter1 + "{1}" + delimiter2, id, data);
            }

            return ret;
        }

        /// <summary>
        /// 詳細データ、行為データのキーバリューを生成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="delimiter1"></param>
        /// <param name="delimiter2"></param>
        /// <returns></returns>
        private string MakeData(string id, double data, string delimiter1 = "", string delimiter2 = "")
        {
            string ret = "";

            if (data != 0)
            {
                ret = String.Format("\"{0}\":" + delimiter1 + "{1}" + delimiter2, id, data);
            }

            return ret;
        }
    }
}
