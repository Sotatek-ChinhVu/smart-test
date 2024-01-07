using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public class P27KokhoSeikyuInCoReportService : IP27KokhoSeikyuInCoReportService
{
    #region Constant
    private const int myPrefNo = 27;
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
    private const string _formFileName = "p27KokhoSeikyuIn.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P27KokhoSeikyuInCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
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
    private List<string> printHokensyaNos;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP27KokhoSeikyuInReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if (getData)
            {
                foreach (string currentNo in hokensyaNos)
                {
                    currentHokensyaNo = currentNo;
                    currentPage = 1;
                    hasNextPage = true;

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
        List<ListTextObject> listDataPerPage = new();
        Dictionary<string, string> fieldDataPerPage = new();
        var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

        bool _hasNextPage = false;

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
            SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == currentHokensyaNo)?.Name ?? "");
            fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo);
            _setFieldData.Add(pageIndex, fieldDataPerPage);
            //印
            SetVisibleFieldData("inkan", seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("inkanMaru", seikyuYm < KaiseiDate.m202210);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

            countData totalData = new countData();
            const int maxRow = 12;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = null;
                switch (rowNo)
                {
                    //退職
                    case 0: wrkReces = curReceInfs.Where(r => r.IsRetAll && r.IsHeiyo).ToList(); break;
                    case 1: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool && !r.IsHeiyo).ToList(); break;
                    case 2: wrkReces = curReceInfs.Where(r => (r.IsRetMine || r.IsRetFamily) && !r.IsHeiyo && r.HokenRate != 30).ToList(); break;
                    case 3: wrkReces = curReceInfs.Where(r => (r.IsRetMine || r.IsRetFamily) && !r.IsHeiyo && r.HokenRate == 30).ToList(); break;
                    //国保
                    case 4: wrkReces = curReceInfs.Where(r => r.IsNrAll && r.IsHeiyo).ToList(); break;
                    case 5: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo).ToList(); break;
                    case 6: break;  //在医総欄は未記載
                    case 7: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo).ToList(); break;
                    case 8: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo).ToList(); break;
                    case 9: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && !r.IsHeiyo && r.HokenRate != 30).ToList(); break;
                    case 10: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && !r.IsHeiyo && r.HokenRate == 30).ToList(); break;
                    case 11:
                        //合計
                        listDataPerPage.Add(new("count", 0, rowNo, totalData.Count.ToString()));
                        listDataPerPage.Add(new("tensu", 0, rowNo, totalData.Tensu.ToString()));
                        listDataPerPage.Add(new("futan", 0, rowNo, totalData.Futan.ToString()));
                        break;
                }
                if (wrkReces == null) continue;

                if (new int[] { 2, 9 }.Contains(rowNo) && wrkReces.Count >= 1)
                {
                    int wrkRate = (100 - wrkReces[0].HokenRate) / 10;
                    SetFieldData(string.Format("kyufu{0}", rowNo), wrkRate.ToString());
                }

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

                //合計集計
                totalData.AddValue(wrkData);
            }

            //「免」欄
            int menjyoCnt = curReceInfs.Where(r => new int[] { GenmenKbn.Gengaku, GenmenKbn.Menjyo, GenmenKbn.Yuyo }.Contains(r.GenmenKbn)).Count();
            SetFieldData("menjyoCnt", menjyoCnt.ToString());

            //「他」欄
            int prefOutKohiCnt = curReceInfs.Where(r => r.IsPrefOutKohi).Count();
            SetFieldData("prefOutKohiCnt", prefOutKohiCnt.ToString());
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
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefIn, myPrefNo, HokensyaNoKbn.SumAll);
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

    private void SetVisibleFieldData(string field, bool value)
    {
        if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
        {
            _visibleFieldData.Add(field, value);
        }
    }
    #endregion
}
