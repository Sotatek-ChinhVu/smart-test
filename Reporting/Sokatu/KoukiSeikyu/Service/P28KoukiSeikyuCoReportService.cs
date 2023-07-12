using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public class P28KoukiSeikyuCoReportService : IP28KoukiSeikyuCoReportService
{
    #region Constructor and Init
    private const int myPrefNo = 28;
    private int _hpId;
    private int _seikyuYm;
    private SeikyuType _seikyuType;
    private bool _hasNextPage;
    private int _currentPage;
    private string _currentHokensyaNo;
    private int _dataRowCount;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    private List<string> printHokensyaNos;
    private const string _formFileName = "p28KoukiSeikyu.rse";

    /// <summary>
    /// OutPut Data
    /// </summary>
    private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    /// <summary>
    /// Finder
    /// </summary>

    private readonly ICoKoukiSeikyuFinder _finder;

    public P28KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder finder)
    {
        _finder = finder;
        _singleFieldData = new();
        _singleFieldDataM = new();
        _listTextData = new();
        _extralData = new();
        _visibleFieldData = new();
    }

    #endregion
    public CommonReportingRequestModel GetP28KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
        return new KoukiSeikyuMapper(_singleFieldDataM, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
    }

    private void UpdateDrawForm()
    {
        #region SubMethod

        #region Header
        void UpdateFormHeader()
        {
            Dictionary<string, string> fieldDataPerPage = new();
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo.AsString());
            SetFieldData("seikyuYear", wrkYmd.Year.AsString());
            SetFieldData("seikyuMonth", wrkYmd.Month.AsString());
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo.AsString());
            SetFieldData("reportYear", wrkYmd.Year.AsString());
            SetFieldData("reportMonth", wrkYmd.Month.AsString());
            SetFieldData("reportDay", wrkYmd.Day.AsString());

            //保険者
            //SetFieldDataM("hokensyaNo".AsInteger(), _currentHokensyaNo);
            fieldDataPerPage.Add("hokensyaNo", _currentHokensyaNo.ToString());
            //印
            SetVisibleFieldData("kbnIppan", _seikyuYm >= KaiseiDate.m202210);
            SetVisibleFieldData("kbnRate9", _seikyuYm < KaiseiDate.m202210);
            
            SetVisibleFieldData("inkan", _seikyuYm < KaiseiDate.m202210);
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            _singleFieldDataM.Add(pageIndex, fieldDataPerPage);
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            Dictionary<string, CellModel> data = new();
            int dataIndex = (_currentPage - 1) * _dataRowCount;

            var curReceInfs = receInfs.Where(r => r.HokensyaNo == _currentHokensyaNo);

            #region Body
            const int maxRow = 2;

            if (_currentPage == 1)
            {
                //1枚目のみ記載する
                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel>? wrkReces = null;
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
            }

            #endregion

            #region 公費負担医療
            const int maxKohiRow = 12;
            int kohiIndex = (_currentPage - 1) * maxKohiRow;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
            if (kohiHoubetus.Count == 0)
            {
                _hasNextPage = false;
            }

            //集計
            for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty)).ToList();

                short curRowNo = rowNo;

                string fieldBiko = "";
                if (rowNo >= 2)
                {
                    //3つ以上ある場合は備考欄に記載する
                    fieldBiko = "Biko";
                    for (int i = 1; i <= 5; i++)
                    {
                        AddListData(ref data, string.Format("kohiTitleBiko{0}", i), true);
                    }
                    curRowNo -= 2;
                }

                //法別番号
                listDataPerPage.Add(new("kohiHoubetu" + fieldBiko, 0, curRowNo, kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("kohiCount" + fieldBiko, 0, curRowNo, wrkData.Count.ToString()));
                //日数
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                listDataPerPage.Add(new("kohiNissu" + fieldBiko, 0, curRowNo, wrkData.Nissu.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                listDataPerPage.Add(new("kohiTensu" + fieldBiko, 0, curRowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                listDataPerPage.Add(new("kohiFutan" + fieldBiko, 0, curRowNo, wrkData.Futan.ToString()));

                kohiIndex++;
                if (kohiIndex >= kohiHoubetus.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            _listTextData.Add(pageIndex, listDataPerPage);

            #endregion
        }
        #endregion

        #endregion

        UpdateFormHeader();
        UpdateFormBody();
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, bool value)
    {
        if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
        {
            dictionary.Add(field, new CellModel(value));
        }
    }

    private bool GetData()
    {
        hpInf = _finder.GetHpInf(_hpId, _seikyuYm);
        receInfs = _finder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.NoSum);
        var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
        receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        hokensyaNames = _finder.GetHokensyaName(_hpId, hokensyaNos);

        return (receInfs?.Count ?? 0) > 0;
    }
    
    private void SetVisibleFieldData(string field, bool value)
    {
        if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
        {
            _visibleFieldData.Add(field, value);
        }
    }
}
