using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P13KokhoSokatuCoReportService : IP13KokhoSokatuCoReportService
{
    #region Constant
    private const int MyPrefNo = 13;
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

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p13KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P13KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    int diskKind;
    int diskCnt;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP13KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int diskKind, int diskCnt)
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

    #region Private function
    private bool UpdateDrawForm()
    {
        bool _hasNextPage = false;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関情報
            SetFieldData("postCd1", CIUtil.Copy(hpInf.PostCd, 1, 3));
            SetFieldData("postCd2", hpInf.PostCd.Length == 7 ? CIUtil.Copy(hpInf.PostCd, 4, 4) : CIUtil.Copy(hpInf.PostCd, 5, 4));
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            SetFieldData("hpTel", string.Format("({0})", hpInf.Tel));
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //印
            SetVisibleFieldData("inkan", seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("inkanMaru", seikyuYm < KaiseiDate.m202210);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            const int maxRow = 8;
            countData totalData = new countData();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = null;
                switch (rowNo)
                {
                    //国保
                    case 0: wrkReces = receInfs.Where(r => r.IsNrAll && r.IsPrefIn).ToList(); break;
                    case 1: wrkReces = receInfs.Where(r => r.IsRetAll && r.IsPrefIn).ToList(); break;
                    case 3: wrkReces = receInfs.Where(r => r.IsNrAll && !r.IsPrefIn).ToList(); break;
                    case 4: wrkReces = receInfs.Where(r => r.IsRetAll && !r.IsPrefIn).ToList(); break;
                    //後期
                    case 6: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsPrefIn).ToList(); break;
                    case 7: wrkReces = receInfs.Where(r => r.IsKoukiAll && !r.IsPrefIn).ToList(); break;
                    default:
                        //計
                        listDataPerPage.Add(new("count", 0, rowNo, totalData.Count.ToString()));
                        listDataPerPage.Add(new("tensu", 0, rowNo, totalData.Tensu.ToString()));
                        listDataPerPage.Add(new("kohiCount", 0, rowNo, totalData.KohiCount.ToString()));
                        totalData.Clear();
                        break;
                }
                if (wrkReces == null) continue;

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
                //公費併用件数
                wrkData.KohiCount = wrkReces.Where(r => r.IsHeiyo).ToList().Count;
                listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.KohiCount.ToString()));

                //計
                totalData.AddValue(wrkData);
            }
            _listTextData.Add(pageIndex, listDataPerPage);

            return 1;
        }
        #endregion

        #endregion

        if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
        {
            hasNextPage = _hasNextPage;
            return false;
        }

        hasNextPage = _hasNextPage;
        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);

        return (receInfs?.Count ?? 0) > 0;
    }

    private void SetVisibleFieldData(string field, bool value)
    {
        if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
        {
            _visibleFieldData.Add(field, value);
        }
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
