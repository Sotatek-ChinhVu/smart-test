using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public class P13KoukiSeikyuCoReportService : IP13KoukiSeikyuCoReportService
{
    #region Constant
    private const int myPrefNo = 13;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKoukiSeikyuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private string _currentHokensyaNo;
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> receInfs;
    private List<CoReceInfModel> tokuyohiReceInfs;
    private CoHpInfModel hpInf;
    private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
    #endregion

    #region Init properties
    private int _hpId;
    private int _seikyuYm;
    private SeikyuType _seikyuType;
    private List<string> printHokensyaNos;
    private bool _hasNextPage;
    private int _currentPage;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private const string _formFileName = "p13KoukiSeikyu.rse";
    private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;

    #region Constructor and Init
    public P13KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _listTextData = new();
        _extralData = new();
        _visibleFieldData = new();
    }
    #endregion

    public CommonReportingRequestModel GetP13KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        _hpId = hpId;
        _seikyuYm = seikyuYm;
        _seikyuType = seikyuType;
        var getData = GetData();

        foreach (string currentNo in hokensyaNos)
        {
            _currentPage = 1;
            _currentHokensyaNo = currentNo;
            _hasNextPage = true;
            while (getData && _hasNextPage)
            {
                UpdateDrawForm();
                _currentPage++;
            }
        }
        var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
        _extralData.Add("totalPage", pageIndex.ToString());
        return new KoukiSeikyuMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
    }

    #region Private function
    private bool UpdateDrawForm()
    {
        _hasNextPage = false;
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            Dictionary<string, string> fieldDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            SetFieldData("hpTel", hpInf.Tel);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo.ToString());
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //保険者
            fieldDataPerPage.Add("hokensyaNo1", _currentHokensyaNo.Substring(2, 2));
            fieldDataPerPage.Add("hokensyaNo2", _currentHokensyaNo.Substring(7, 1));
            _setFieldData.Add(pageIndex, fieldDataPerPage);
            //印
            SetVisibleFieldData("inkan", _seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("inkanMaru", _seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("kbnRate9", _seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("kbnIppan", _seikyuYm >= KaiseiDate.m202210);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == _currentHokensyaNo);

            const int maxRow = 2;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = null;
                switch (rowNo)
                {
                    case 0: wrkReces = curReceInfs.Where(r => r.IsKoukiIppan).ToList(); break;
                    case 1: wrkReces = curReceInfs.Where(r => r.IsKoukiUpper).ToList(); break;
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
        hpInf = _kokhoFinder.GetHpInf(_hpId, _seikyuYm);
        //県外分のみ出力する
        receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.Kouki, PrefKbn.PrefOut, myPrefNo, HokensyaNoKbn.NoSum);
        //保険者番号の指定がある場合は絞り込み
        var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
            receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        //保険者番号リストを取得
        hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

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
