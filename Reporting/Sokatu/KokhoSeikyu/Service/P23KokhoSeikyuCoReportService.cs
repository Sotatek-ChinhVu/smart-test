using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public class P23KokhoSeikyuCoReportService : IP23KokhoSeikyuCoReportService
{
    #region Constant
    private const int myPrefNo = 23;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSeikyuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private string currentHokensyaNo;
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion
    
    /// <summary>
    /// OutPut Data
    /// </summary>
    private string _formFileName = "p23KokhoSeikyu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P23KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        hpInf = new();
        hokensyaNos = new();
        receInfs = new();
        currentHokensyaNo = "";
        printHokensyaNos = new();
        hokensyaNames = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private List<string> printHokensyaNos;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP23KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if (seikyuYm >= 202210)
            {
                _formFileName = "p23KokhoSeikyu_2210.rse";
            }

            if (getData)
            {
                foreach (string currentNo in hokensyaNos)
                {
                    currentHokensyaNo = currentNo;
                    hasNextPage = true;
                    currentPage = 1;

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
        finally
        {
            _kokhoFinder.ReleaseResource();
        }
    }

    #region Private function
    private bool UpdateDrawForm()
    {
        bool _hasNextPage = false;
        Dictionary<string, string> fieldDataPerPage = new();
        var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

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
            SetFieldData("hpTel", hpInf.Tel);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //保険者
            fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo);
            //_singleFieldDataM.Add(pageIndex, fieldDataPerPage);
            SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == currentHokensyaNo)?.Name ?? "");

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

            const int maxRow = 8;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = new();
                switch (rowNo)
                {
                    //国保
                    case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                    case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                    case 2: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && r.HokenRate != 30).ToList(); break;
                    case 3: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && r.HokenRate == 30).ToList(); break;
                    case 4: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                    //退職
                    case 5: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                    case 6: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                    case 7: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                }
                if (wrkReces == null) continue;

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                //日数
                wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                listDataPerPage.Add(new("nissu", 0, rowNo, wrkData.Nissu.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                listDataPerPage.Add(new("futan", 0, rowNo, wrkData.Futan.ToString()));
            }

            //福祉
            List<string> prefInHoubetus = new List<string> { "81", "82", "83", "85", "89" };
            //全国公費
            var prefAllHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsHeiyo).ToList(), prefInHoubetus);

            //「① 公費」欄                    
            int kohiCount = 0;
            foreach (var houbetu in prefAllHoubetus)
            {
                kohiCount += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
            }
            fieldDataPerPage.Add("kohiCount", kohiCount.ToString());

            //「② 福祉」欄
            int welfareCount = 0;
            foreach (var houbetu in prefInHoubetus)
            {
                welfareCount += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
            }
            fieldDataPerPage.Add("welfareCount", welfareCount.ToString());

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
        _setFieldData.Add(pageIndex, fieldDataPerPage);

        hasNextPage = _hasNextPage;
        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号の指定がある場合は絞り込み
        var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
            receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        //保険者番号リストを取得
        hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
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
