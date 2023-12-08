using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P08KokhoSokatuCoReportService : IP08KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 8;
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
    private const string _formFileName = "p08KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P08KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        hpInf = new();
        receInfs = new();
    }
    #endregion

    public CommonReportingRequestModel GetP08KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
        hasNextPage = false;
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
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

            const int maxRow = 7;

            //福祉
            List<string> prefInHoubetus = new List<string> { "60", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "94", "95" };
            //全国公費
            var prefAllHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsHeiyo).ToList(), prefInHoubetus);

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = new();
                switch (rowNo)
                {
                    case 0: wrkReces = receInfs.Where(r => r.IsNrAll).ToList(); break;
                    case 1: wrkReces = receInfs.Where(r => r.IsRetAll).ToList(); break;
                    case 2: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo).ToList(); break;
                    case 3: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo).ToList(); break;
                    case 4: wrkReces = receInfs.Where(r => r.IsKoukiAll).ToList(); break;
                    case 5: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo).ToList(); break;
                    case 6: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo).ToList(); break;
                }
                if (wrkReces == null) continue;

                countData wrkData = new countData();
                if (rowNo == 2)
                {
                    //福祉                        
                    foreach (var houbetu in prefInHoubetus)
                    {
                        wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                        wrkData.Count += wrkReces.Count;
                        wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                    }
                }
                else if (rowNo == 3)
                {
                    //全国公費
                    foreach (var houbetu in prefAllHoubetus)
                    {
                        wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                        wrkData.Count += wrkReces.Count;
                        wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                    }
                }
                else if (rowNo == 5)
                {
                    //福祉
                    foreach (var houbetu in prefInHoubetus)
                    {
                        wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                        wrkData.Count += wrkReces.Count;
                        wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                    }
                }
                else if (rowNo == 6)
                {
                    //全国公費
                    foreach (var houbetu in prefAllHoubetus)
                    {
                        wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                        wrkData.Count += wrkReces.Count;
                        wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                    }
                }
                else
                {
                    wrkData.Count = wrkReces.Count;
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                }

                //件数
                listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                //点数                   
                listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
            }
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
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
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
