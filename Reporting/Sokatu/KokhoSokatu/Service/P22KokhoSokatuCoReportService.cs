using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P22KokhoSokatuCoReportService : IP22KokhoSokatuCoReportService
{
    #region Constant
    private const int MyPrefNo = 22;
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
    private CoHpInfModel hpInf;
    private List<CoKaMstModel> kaMsts;
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> curReceInfs;
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p22KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P22KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
    }
    #endregion

    public CommonReportingRequestModel GetP22KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
                for (int prefCnt = 0; prefCnt <= 1; prefCnt++)
                {
                    curReceInfs = receInfs.Where(r => prefCnt == 0 ? r.IsPrefIn : !r.IsPrefIn).ToList();
                    if (curReceInfs.Count() == 0) continue;
                    while (getData && hasNextPage)
                    {
                        UpdateDrawForm(prefCnt);
                        currentPage++;
                    }
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
    private bool UpdateDrawForm(int prefCnt)
    {
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region Body
            //国保
            const int maxKokhoCol = 6;
            int kokhoIndex = (currentPage - 1) * maxKokhoCol;

            var kokhoNos = curReceInfs.Where(
                r => r.IsNrAll || r.IsRetAll
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            bool kokhoNextPage = true;

            countData[] kokhoSubTotals = new countData[7];

            for (short colNo = 0; colNo < maxKokhoCol; colNo++)
            {
                if (kokhoIndex < kokhoNos.Count)
                {
                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == kokhoNos[kokhoIndex])?.Name ?? "";
                    listDataPerPage.Add(new(
                        "kokhoName", colNo, 0,
                        hokensyaName == "" ? kokhoNos[kokhoIndex] : hokensyaName
                    ));

                    const int maxRow = 7;
                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
                        switch (rowNo)
                        {
                            //国保
                            case 0: wrkReces = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsNrElderIppan).ToList(); break;
                            case 1: wrkReces = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsNrElderUpper).ToList(); break;
                            case 2: wrkReces = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && (r.IsNrMine || r.IsNrFamily)).ToList(); break;
                            case 3: wrkReces = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsNrPreSchool).ToList(); break;
                            //退職
                            case 4: wrkReces = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsRetMine).ToList(); break;
                            case 5: wrkReces = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsRetFamily).ToList(); break;
                            case 6: wrkReces = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsRetPreSchool).ToList(); break;
                        }
                        if (wrkReces == null) continue;

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new(string.Format("kokhoCount{0}", colNo), 0, rowNo, wrkData.Count.ToString()));
                        kokhoSubTotals[rowNo].Count += wrkData.Count;

                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        listDataPerPage.Add(new(string.Format("kokhoTensu{0}", colNo), 0, rowNo, wrkData.Tensu.ToString()));
                        kokhoSubTotals[rowNo].Tensu += wrkData.Tensu;
                    }
                }

                kokhoIndex++;
                if (kokhoIndex >= kokhoNos.Count)
                {
                    kokhoNextPage = false;
                    break;
                }
            }

            for (short rowNo = 0; rowNo <= 6; rowNo++)
            {
                listDataPerPage.Add(new("kokhoCount6", 0, rowNo, kokhoSubTotals[rowNo].Count.ToString()));
                listDataPerPage.Add(new("kokhoTensu6", 0, rowNo, kokhoSubTotals[rowNo].Tensu.ToString()));
            }

            //後期
            const int maxKoukiCol = 6;
            int koukiIndex = (currentPage - 1) * maxKoukiCol;

            var koukiNos = curReceInfs.Where(
                r => r.IsKoukiAll
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            if (kokhoNos.Count == 0 && koukiNos.Count == 0)
            {
                hasNextPage = false;
                return 1;
            }

            bool koukiNextPage = true;

            countData[] koukiSubTotals = new countData[7];
            for (short colNo = 0; colNo < maxKoukiCol; colNo++)
            {
                const int maxRow = 2;

                if (koukiIndex < koukiNos.Count)
                {
                    int prefNo = koukiNos[koukiIndex].Substring(koukiNos[koukiIndex].Length - 6, 2).AsInteger();
                    listDataPerPage.Add(new("koukiName", colNo, 0, PrefCode.PrefName(prefNo)));

                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
                        switch (rowNo)
                        {
                            //国保
                            case 0: wrkReces = curReceInfs.Where(r => r.HokensyaNo == koukiNos[koukiIndex] && r.IsKoukiIppan).ToList(); break;
                            case 1: wrkReces = curReceInfs.Where(r => r.HokensyaNo == koukiNos[koukiIndex] && r.IsKoukiUpper).ToList(); break;
                        }
                        if (wrkReces == null) continue;

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new(string.Format("koukiCount{0}", colNo), 0, rowNo, wrkData.Count.ToString()));
                        koukiSubTotals[rowNo].Count += wrkData.Count;
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        listDataPerPage.Add(new(string.Format("koukiTensu{0}", colNo), 0, rowNo, wrkData.Tensu.ToString()));

                        koukiSubTotals[rowNo].Tensu += wrkData.Tensu;
                    }
                }

                koukiIndex++;
                if (koukiIndex >= koukiNos.Count)
                {
                    koukiNextPage = false;
                    break;
                }
            }

            for (short rowNo = 0; rowNo <= 6; rowNo++)
            {
                listDataPerPage.Add(new("koukiCount6", 0, rowNo, koukiSubTotals[rowNo].Count.ToString()));
                listDataPerPage.Add(new("koukiTensu6", 0, rowNo, koukiSubTotals[rowNo].Tensu.ToString()));
            }

            if (!kokhoNextPage && !koukiNextPage)
            {
                pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                fieldDataPerPage.Add("kokhoTotalCount", curReceInfs.Count(r => r.IsNrAll || r.IsRetAll).ToString());
                fieldDataPerPage.Add("kokhoTotalTensu", curReceInfs.Where(r => r.IsNrAll).Sum(r => r.Tensu).ToString());
                fieldDataPerPage.Add("koukiTotalCount", curReceInfs.Count(r => r.IsKoukiAll).ToString());
                fieldDataPerPage.Add("koukiTotalTensu", curReceInfs.Where(r => r.IsKoukiAll).Sum(r => r.Tensu).ToString());

                if (!_setFieldData.ContainsKey(pageIndex))
                {
                    _setFieldData.Add(pageIndex, fieldDataPerPage);
                }
                hasNextPage = false;
            }
            #endregion
            _listTextData.Add(pageIndex, listDataPerPage);

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
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号リストを取得
        hokensyaNos = receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        //保険者名を取得
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