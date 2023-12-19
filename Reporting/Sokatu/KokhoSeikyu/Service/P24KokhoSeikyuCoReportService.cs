using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public class P24KokhoSeikyuCoReportService : IP24KokhoSeikyuCoReportService
{
    #region Constant
    private const int myPrefNo = 24;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSeikyuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private short printKokhoKbn;
    private string currentHokensyaNo;
    private List<string> hokensyaNos;
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p24KokhoSeikyu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P24KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
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
        curReceInfs = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private List<string> printHokensyaNos;
    private List<CoReceInfModel> curReceInfs;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP24KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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

                    //国保一般と退職は別に請求書を作成する
                    for (short kokhoKbn = 0; kokhoKbn <= 1; kokhoKbn++)
                    {
                        printKokhoKbn = kokhoKbn;

                        curReceInfs = receInfs.Where(r => (kokhoKbn == 0 ? r.IsNrAll : r.IsRetAll) && r.HokensyaNo == currentHokensyaNo).ToList();
                        if (curReceInfs.Count() == 0) continue;
                        hasNextPage = true;
                        currentPage = 1;

                        while (getData && hasNextPage)
                        {
                            UpdateDrawForm();
                            currentPage++;
                        }
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
        List<ListTextObject> listDataPerPage = new();
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
            //国保・退職の種別
            listDataPerPage.Add(new("kokhoKbn", printKokhoKbn, 0, "○"));
            //印
            SetVisibleFieldData("inkan", seikyuYm < KaiseiDate.m202210);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            const int maxRow = 6;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = new();
                if (printKokhoKbn == 0)
                {
                    //国保一般
                    switch (rowNo)
                    {
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                        case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        case 4: break;
                        case 5: wrkReces = curReceInfs.ToList(); break;
                    }
                }
                else
                {
                    //退職
                    switch (rowNo)
                    {
                        case 0: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 1: break;
                        case 2: break;
                        case 3: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 4: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                        case 5: wrkReces = curReceInfs.ToList(); break;
                    }
                }
                if (wrkReces.Count == 0) continue;

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

            //原爆
            List<string> kohi19Houbetus = new List<string> { "18", "19" };
            //その他公費
            var elseHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsHeiyo).ToList(), kohi19Houbetus);

            //公費併用件数
            int kohiCount = 0;
            foreach (var houbetu in elseHoubetus)
            {
                kohiCount += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
            }
            fieldDataPerPage.Add("kohiCount", kohiCount.ToString());

            //原爆件数
            int kohi19Count = 0;
            foreach (var houbetu in kohi19Houbetus)
            {
                kohi19Count += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
            }
            fieldDataPerPage.Add("kohi19Count", kohi19Count.ToString());

            //在医総
            int zaiCount = curReceInfs.Where(r => r.IsZaiiso == 1).Count();
            fieldDataPerPage.Add("zaiCount", zaiCount.ToString());
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
        var wrkReceInfs = printHokensyaNos.Count == 0 ? receInfs.ToList() :
            receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        //保険者番号リストを取得（県内→県外）
        hokensyaNos = receInfs.Where(r => r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        hokensyaNos.AddRange(
            receInfs.Where(r => !r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList()
        );

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
