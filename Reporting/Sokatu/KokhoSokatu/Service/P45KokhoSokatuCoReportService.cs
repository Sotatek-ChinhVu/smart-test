using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P45KokhoSokatuCoReportService : IP45KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 45;
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
    private List<CoKaMstModel> kaMsts;
    private int prefInOut;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p45KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P45KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
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
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP45KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
                prefInOut = prefCnt;

                curReceInfs = receInfs.Where(r => prefCnt == 0 ? r.IsPrefIn : !r.IsPrefIn).ToList();
                if (curReceInfs.Count() == 0) continue;

                while (getData && hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
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
            for (int i = 1; i <= 2; i++)
            {
                //医療機関コード
                SetFieldData(string.Format("hpCode{0}", i), hpInf.HpCd);
                //診療科
                SetFieldData(string.Format("kaName{0}", i), kaMsts[0].KaName);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData(string.Format("seikyuGengo{0}", i), wrkYmd.Gengo);
                SetFieldData(string.Format("seikyuYear{0}", i), wrkYmd.Year.ToString());
                SetFieldData(string.Format("seikyuMonth{0}", i), wrkYmd.Month.ToString());
                //県内・県外
                if (prefInOut == 0)
                {
                    SetFieldData(string.Format("prefIn{0}", i), "〇");
                }
                else
                {
                    SetFieldData(string.Format("prefOut{0}", i), "〇");
                }
            }

            //医療機関情報
            SetFieldData("postCd", hpInf.PostCdDsp);
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            SetFieldData("hpTel", hpInf.Tel);
            //提出年月日
            CIUtil.WarekiYmd wrkYmd2 = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportMonth", wrkYmd2.Month.ToString());
            SetFieldData("reportDay", wrkYmd2.Day.ToString());

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            //高額レセプト件数再掲欄
            int kgKokuhoCount = curReceInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.Tensu >= 80000).ToList().Count;
            int kgKoukiCount = curReceInfs.Where(r => r.IsKoukiAll && r.Tensu >= 80000).ToList().Count;
            int kgTotalCount = curReceInfs.Where(r => r.Tensu >= 80000).ToList().Count;
            if (kgTotalCount >= 1)
            {
                SetFieldData("kgKokuhoCount", kgKokuhoCount.ToString());
                SetFieldData("kgKoukiCount", kgKoukiCount.ToString());
                SetFieldData("kgTotalCount", kgTotalCount.ToString());
            }

            const int maxRow = 5;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = null;
                switch (rowNo)
                {
                    case 0: wrkReces = curReceInfs.Where(r => r.IsNrAll).ToList(); break;
                    case 1: wrkReces = curReceInfs.Where(r => r.IsRetAll).ToList(); break;
                    case 2: wrkReces = curReceInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                    case 3: wrkReces = curReceInfs.Where(r => r.IsKoukiAll).ToList(); break;
                    case 4: wrkReces = curReceInfs.ToList(); break;
                }
                if (wrkReces == null) continue;

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));

                if (rowNo == 2 || rowNo == 3)
                {
                    //（国保分・後期分）件数・点数
                    listDataPerPage.Add(new("totalCount", 0, (short)(rowNo - 2), wrkData.Count.ToString()));
                    listDataPerPage.Add(new("totalTensu", 0, (short)(rowNo - 2), wrkData.Tensu.ToString()));
                }
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
        kaMsts = _kokhoFinder.GetKaMst(hpId);
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
