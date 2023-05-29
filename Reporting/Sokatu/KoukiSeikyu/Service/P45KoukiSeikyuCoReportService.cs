using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public class P45KoukiSeikyuCoReportService : IP45KoukiSeikyuCoReportService
{
    #region Constant
    private const int MyPrefNo = 43;
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
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private const string _formFileNameP1 = "p45KoukiSeikyuP1.rse";
    private const string _formFileNameP2 = "p45KoukiSeikyuP2.rse";

    #region Constructor and Init
    public P45KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldDataM = new();
        _singleFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
    }
    #endregion

    #region Init properties
    private int _hpId;
    private int _seikyuYm;
    private SeikyuType _seikyuType;
    private bool _hasNextPage;
    private int _currentPage;
    private List<string> printHokensyaNos;
    #endregion

    public CommonReportingRequestModel GetP45KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
                _currentPage ++;
            }
        }
        var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
        _extralData.Add("totalPage", pageIndex.ToString());
        return new P45KoukiSeikyuMapper(_singleFieldDataM, _listTextData, _extralData, _formFileNameP1, _formFileNameP2, _singleFieldData, _visibleFieldData).GetData();
    }
    #region Private function

    private bool UpdateDrawForm()
    {
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            Dictionary<string, string> fieldDataPerPage = new();
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //保険者
            fieldDataPerPage.Add("hokensyaNo", _currentHokensyaNo.ToString());
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            _singleFieldDataM.Add(pageIndex, fieldDataPerPage);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("hpTel", hpInf.Tel);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());

            if (_currentPage >= 2) return 1;

            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo.ToString());
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());

            return 1;
        }
        #endregion

        #region BodyP1
        int UpdateFormBodyP1()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == _currentHokensyaNo);

            #region Body
            const int maxRow = 2;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces = null;
                switch (rowNo)
                {
                    //国保
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
            #endregion

            #region 公費負担医療
            const int maxKohiRow = 4;
            int kohiIndex = 0;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
            if (kohiHoubetus.Count == 0)
            {
                _listTextData.Add(pageIndex, listDataPerPage);
                _hasNextPage = false;
                return 1;
            }

            //集計
            for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                //法別番号
                listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]));

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                //日数
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiIndex]));
                listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));

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

        #region BodyP2
        int UpdateFormBodyP2()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == _currentHokensyaNo);

            const int maxKohiRow = 7;
            int kohiIndex = (_currentPage - 2) * maxKohiRow + 4;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
            if (kohiHoubetus.Count <= 4)
            {
                _hasNextPage = false;
                return 1;
            }

            //集計
            for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                //法別番号
                listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]));

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                //日数
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiIndex]));
                listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));

                kohiIndex++;
                if (kohiIndex >= kohiHoubetus.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }
            _listTextData.Add(pageIndex, listDataPerPage);

            return 1;
        }
        #endregion

        #endregion

        switch (_currentPage)
        {
            case 1:
                if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                {
                    return false;
                }
                break;
            default:
                if (UpdateFormHeader() < 0 || UpdateFormBodyP2() < 0)
                {
                    return false;
                }
                break;
        }
        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(_hpId, _seikyuYm);
        receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号の指定がある場合は絞り込み
        var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
            receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        //保険者番号リストを取得（県内→県外）
        hokensyaNos = wrkReceInfs.Where(r => r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        hokensyaNos.AddRange(
            wrkReceInfs.Where(r => !r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList()
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
    #endregion
}
