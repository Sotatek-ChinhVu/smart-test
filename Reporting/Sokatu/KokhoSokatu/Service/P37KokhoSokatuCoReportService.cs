using Entity.Tenant;
using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P37KokhoSokatuCoReportService : IP37KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 37;

    private List<string> fixedHoubetu = new List<string> { "10", "21" };
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
    private List<CoReceInfModel> curReceInfs;
    private CoHpInfModel hpInf;
    private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
    private string prefInOut;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName1 = "p37KokhoSokatuP1.rse";
    private const string _formFileName2 = "p37KokhoSokatuP2.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P37KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        hpInf = new();
        receInfs = new();
        curReceInfs = new();
        kohiHoubetuMsts = new();
        prefInOut = "";
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP37KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();
            int indexPage = 1;
            var fileName = new Dictionary<string, string>();

            if (getData)
            {
                for (int prefCnt = 0; prefCnt <= 1; prefCnt++)
                {
                    if (prefCnt == 0)
                    {
                        prefInOut = "（県内）";
                    }
                    else
                    {
                        prefInOut = "（県外）";
                    }

                    curReceInfs = receInfs.Where(r => prefCnt == 0 ? r.IsPrefIn : !r.IsPrefIn).ToList();
                    if (curReceInfs.Count() == 0) continue;

                    hasNextPage = true;
                    this.currentPage = 1;
                    while (getData && hasNextPage)
                    {
                        UpdateDrawForm();
                        if (currentPage == 2)
                        {
                            fileName.Add(indexPage.ToString(), _formFileName2);
                        }
                        else
                        {
                            fileName.Add(indexPage.ToString(), _formFileName1);
                        }
                        currentPage++;
                        indexPage++;
                    }
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new P14KokhoSokatuCoReportServiceMapper(_setFieldData, _listTextData, _extralData, fileName, _singleFieldData, _visibleFieldData).GetData();
        }
        finally
        {
            _kokhoFinder.ReleaseResource();
        }
    }

    #region Private function
    private bool UpdateDrawForm()
    {
        bool _hasNextPage = true;
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //県内・県外を印字
            SetFieldData("prefInOut", prefInOut.ToString());

            if (currentPage >= 2) return 1;

            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);

            return 1;
        }
        #endregion

        #region BodyP1
        int UpdateFormBodyP1()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            const int maxRow = 7;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = new();
                switch (rowNo)
                {
                    //国保
                    case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                    case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                    case 2: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                    case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                    //退職
                    case 4: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                    case 5: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                    case 6: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                }
                if (wrkReces.Count == 0) continue;

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                listDataPerPage.Add(new("futan", 0, rowNo, wrkData.Futan.ToString()));
            }
            _listTextData.Add(pageIndex, listDataPerPage);
            return 1;
        }
        #endregion

        #region BodyP2
        int UpdateFormBodyP2()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            //1枚目のみ記載する
            if (currentPage == 2)
            {
                #region 公費負担医療（固定枠）
                for (short rowNo = 0; rowNo < fixedHoubetu.Count; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetu[rowNo])).ToList();

                    countData wrkData = new countData();

                    wrkData.Count = wrkReces.Count;
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(fixedHoubetu[rowNo]));
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(fixedHoubetu[rowNo]));

                    listDataPerPage.Add(new("fixedCount", 0, rowNo, wrkData.Count.ToString()));
                    listDataPerPage.Add(new("fixedTensu", 0, rowNo, wrkData.Tensu.ToString()));
                    listDataPerPage.Add(new("fixedFutan", 0, rowNo, wrkData.Futan.ToString()));
                }
                #endregion
            }

            #region 公費負担医療（フリー枠）
            const int maxKohiRow = 8;
            //固定枠の法別番号は除く
            List<string> excludeHoubetu = fixedHoubetu.ToList();

            int kohiIndex = (currentPage - 2) * maxKohiRow;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), excludeHoubetu);
            if (kohiHoubetus.Count == 0 || kohiHoubetus.Count <= kohiIndex)
            {
                //_listTextData.Add(pageIndex, listDataPerPage);
                _hasNextPage = false;
                return 1;
            }

            //集計 
            for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();
                if (wrkReces != null)
                {
                    //法別番号
                    listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]));
                    //公費名称
                    listDataPerPage.Add(new("kohiName", 0, rowNo, SokatuUtil.GetKohiName(kohiHoubetuMsts, myPrefNo, kohiHoubetus[kohiIndex])));

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                    listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiIndex]));
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));
                }
                kohiIndex++;
                if (kohiIndex >= kohiHoubetus.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }
            _listTextData.Add(pageIndex, listDataPerPage);
            #endregion

            return 1;
        }
        #endregion

        #endregion

        switch (currentPage)
        {
            case 1:
                if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
                break;
            default:
                if (UpdateFormHeader() < 0 || UpdateFormBodyP2() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
                break;
        }
        hasNextPage = _hasNextPage;
        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
        //公費法別番号リストを取得
        kohiHoubetuMsts = _kokhoFinder.GetKohiHoubetuMst(hpId, seikyuYm);

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
