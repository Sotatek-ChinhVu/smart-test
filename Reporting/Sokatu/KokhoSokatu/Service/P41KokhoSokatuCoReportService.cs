using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P41KokhoSokatuCoReportService : IP41KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 41;

    private List<string> KohiHoubetus = new List<string> { "80", "81", "90" };
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSokatuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoReceInfModel> receInfs;
    private List<PrintUnit> printUnits;
    private List<CoHokensyaMstModel> hokensyaNames;
    private CoHpInfModel hpInf;
    private List<CoKaMstModel> kaMsts;

    struct PrintUnit
    {
        public bool IsKoukiAll;
        public bool IsPrefIn;
        public bool IsKumiai;
        public string HokensyaNo;
        public int HokenRate;
    }
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p41KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P41KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        hpInf = new();
        receInfs = new();
        hokensyaNames = new();
        kaMsts = new();
        printUnits = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP41KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            currentPage = 1;
            var getData = GetData();
            hasNextPage = true;

            if (getData)
            {
                while (getData && hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KokhoSokatuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
        }
        finally
        {
            _kokhoFinder.ReleaseResource();
        }
    }

    #region Private function
    private bool UpdateDrawForm()
    {
        hasNextPage = true;
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //医療機関情報
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //診療科
            SetFieldData("kaName", kaMsts[0].KaName ?? string.Empty);

            return 1;
        }
        #endregion

        #region Body
        int GetHokenWariByHokenRate(int rate)
        {
            if (rate > 0)
            {
                return (100 - rate) / 10;
            }
            else
            {
                return 7;
            }
        }

        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region 合計
            if (currentPage == 1)
            {
                //1枚目のみ記載する
                for (short i = 0; i <= 1; i++)
                {
                    List<CoReceInfModel> wrkReces = new();
                    //国保＋退職の合計と後期の合計
                    switch (i)
                    {
                        case 0: wrkReces = receInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsKoukiAll).ToList(); break;
                    }
                    if (wrkReces.Count == 0) continue;

                    countData wrkData = new countData();
                    //件数
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                    wrkData.Count = wrkReces.Count;
                    fieldDataPerPage.Add(string.Format("totalCount{0}", i), wrkData.Count.ToString());
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                    fieldDataPerPage.Add(string.Format("totalNissu{0}", i), wrkData.Nissu.ToString());
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    fieldDataPerPage.Add(string.Format("totalTensu{0}", i), wrkData.Tensu.ToString());

                    //1件当り点数
                    if (wrkData.Count > 0)
                    {
                        int avgTensu = CIUtil.RoundInt((double)wrkData.Tensu / wrkData.Count, 0);
                        fieldDataPerPage.Add(string.Format("avgTensu{0}", i), avgTensu.ToString());
                    }

                    //県外総件数
                    wrkData.Count = wrkReces.Where(r => r.IsPrefIn == false).Count();
                    fieldDataPerPage.Add(string.Format("kengaiTotalCount{0}", i), wrkData.Count.ToString());
                    //県内総件数
                    wrkData.Count = wrkReces.Where(r => r.IsPrefIn == true).Count();
                    fieldDataPerPage.Add(string.Format("kennaiTotalCount{0}", i), wrkData.Count.ToString());

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }
                }

            }
            #endregion

            #region 集計
            const int maxRow = 10;

            int hokIndex = (currentPage - 1) * maxRow;
            hasNextPage = true;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if (hokIndex < printUnits.Count)
                {
                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == printUnits[hokIndex].HokensyaNo)?.Name ?? "";
                    if (printUnits[hokIndex].HokenRate > 0)
                    {
                        //国保組合は割合ごとに行を分けて記載する
                        int wari = GetHokenWariByHokenRate(printUnits[hokIndex].HokenRate);
                        hokensyaName += String.Format("({0}割)", wari);
                    }
                    listDataPerPage.Add(new("hokensyaName", 0, rowNo, hokensyaName));

                    for (short colNo = 0; colNo <= 2; colNo++)
                    {
                        List<CoReceInfModel> wrkReces = new();
                        wrkReces = receInfs.Where(r => r.HokensyaNo == printUnits[hokIndex].HokensyaNo && (printUnits[hokIndex].HokenRate == -1 || r.HokenRate == printUnits[hokIndex].HokenRate)).ToList();

                        //国保・退職・後期それぞれの列に印字する
                        switch (colNo)
                        {
                            case 0: wrkReces = wrkReces.Where(r => r.IsNrAll).ToList(); break;
                            case 1: wrkReces = wrkReces.Where(r => r.IsRetAll).ToList(); break;
                            case 2: wrkReces = wrkReces.Where(r => r.IsKoukiAll).ToList(); break;
                        }
                        if (wrkReces.Count == 0) continue;

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new(string.Format("count{0}", colNo), 0, rowNo, wrkData.Count.ToString()));
                        //日数
                        wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                        listDataPerPage.Add(new(string.Format("nissu{0}", colNo), 0, rowNo, wrkData.Nissu.ToString()));
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        listDataPerPage.Add(new(string.Format("tensu{0}", colNo), 0, rowNo, wrkData.Tensu.ToString()));
                    }
                }

                hokIndex++;
                if (hokIndex >= printUnits.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
            _listTextData.Add(pageIndex, listDataPerPage);
            #endregion

            return 1;
        }
        #endregion

        #endregion

        if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
        {
            return false;
        }
        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        kaMsts = _kokhoFinder.GetKaMst(hpId);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);

        //国保組合以外の保険者番号リストを取得
        printUnits = receInfs.Where(r => !r.IsKumiai || r.IsKoukiAll)
            .GroupBy(r => new { r.IsPrefIn, r.HokensyaNo, r.IsKoukiAll })
            .Select(r => new PrintUnit { IsKumiai = false, IsPrefIn = r.Key.IsPrefIn, HokensyaNo = r.Key.HokensyaNo, HokenRate = -1, IsKoukiAll = r.Key.IsKoukiAll })
            .ToList();

        //国保組合はさらに給付割合ごとに分けて記載する
        printUnits.AddRange(
            receInfs.Where(r => r.IsKumiai && !r.IsKoukiAll)
            .GroupBy(r => new { r.IsPrefIn, r.HokensyaNo, r.HokenRate, r.IsKoukiAll })
            .Select(r => new PrintUnit { IsKumiai = true, IsPrefIn = r.Key.IsPrefIn, HokensyaNo = r.Key.HokensyaNo, HokenRate = r.Key.HokenRate, IsKoukiAll = r.Key.IsKoukiAll })
            .ToList()
        );
        //ソート順（国保→後期 > 県外→県内）
        printUnits = printUnits
            .OrderBy(r => r.IsKoukiAll)
            .ThenBy(r => r.IsPrefIn)
            .ThenBy(r => r.HokensyaNo)
            .ThenBy(r => r.HokenRate)
            .ToList();

        //保険者名を取得
        List<string> hokensyaNos = printUnits.Select(r => r.HokensyaNo).ToList();
        hokensyaNames = _kokhoFinder.GetHokensyaName(hpId, hokensyaNos);

        return (receInfs?.Count ?? 0) > 0;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }
    #endregion
}
