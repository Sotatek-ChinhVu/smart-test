using Helper.Common;
using Helper.Constants;
using Microsoft.EntityFrameworkCore.Internal;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public class P29KoukiSeikyuCoReportService : IP29KoukiSeikyuCoReportService
{
    #region Constant
    private const int MyPrefNo = 29;
    private int _hpId;
    private int _seikyuYm;
    private SeikyuType _seikyuType;
    private bool _hasNextPage;
    private int _currentPage;
    private string _currentHokensyaNo;
    private int _dataRowCount;
    #endregion

    #region
    private ICoKoukiSeikyuFinder _kokhofinder;
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<string> printHokensyaNos;
    private bool printZaiiso;
    private List<CoReceInfModel> receInfs;
    private List<CoReceInfModel> curReceInfs;
    private CoHpInfModel hpInf;
    #endregion

    #region
    private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM = new Dictionary<int, Dictionary<string, string>>();
    private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private string _formFileName = "p29KoukiSeikyu.rse";
    #endregion

    public P29KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder finder, IReadRseReportFileService readRseReportFileService)
    {
        _kokhofinder = finder;
        _readRseReportFileService = readRseReportFileService;
        _singleFieldDataM = new();
        _singleFieldData = new();
        _listTextData = new();
        _extralData = new();
        _visibleFieldData = new();
    }
    public CommonReportingRequestModel GetP29KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        _hpId = hpId;
        _seikyuYm = seikyuYm;
        _seikyuType = seikyuType;
        var getData = GetData();
        if (_seikyuYm >= 202210)
        {
            _formFileName = "p29KoukiSeikyu_2210.rse";
        }
        for (int zaiFlg = 0; zaiFlg <= 1; zaiFlg++)
        {
            printZaiiso = zaiFlg == 1;
            foreach (string currentNo in hokensyaNos)
            {
                _currentHokensyaNo = currentNo;
                curReceInfs = receInfs.Where(r => r.IsZaiiso == zaiFlg && r.HokensyaNo == _currentHokensyaNo).ToList();
                if (curReceInfs.Count() == 0) continue;

                _hasNextPage = true;
                _currentPage = 1;
                while (getData && _hasNextPage)
                {
                    UpdateDrawForm();
                    _currentPage++;
                }
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
            SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == _currentHokensyaNo)?.Name ?? "");
            fieldDataPerPage.Add("hokensyaNo", _currentHokensyaNo.ToString());
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            _singleFieldDataM.Add(pageIndex, fieldDataPerPage);
            //在医総及び在医総管
            if (printZaiiso)
            {
                fieldDataPerPage.Add("zaiisoWord", "在");
                fieldDataPerPage.Add("zaiisoCircle", "〇");
            }
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            int dataIndex = (_currentPage - 1) * _dataRowCount;

            #region Body
            const int maxRow = 2;

            if (_currentPage == 1)
            {
                //1枚目のみ記載する
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

                //「長」欄
                int chokiCnt = curReceInfs.Where(r => r.IsChoki).Count();
                SetFieldData("chokiCnt", chokiCnt.ToString());
            }
            #endregion

            #region 公費負担医療
            const int maxKohiRow = 5;
            int kohiIndex = (_currentPage - 1) * maxKohiRow;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
            if (kohiHoubetus.Count == 0)
            {
                _hasNextPage = false;
            }

            //集計
            for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                //法別番号
                listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                //日数
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));

                kohiIndex++;
                if (kohiIndex >= kohiHoubetus.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            _listTextData.Add(pageIndex, listDataPerPage);
        }
        #endregion

        #endregion
        #endregion
        UpdateFormHeader();
        UpdateFormBody();
        
    }
    private bool GetData()
    {
        hpInf = _kokhofinder.GetHpInf(_hpId, _seikyuYm);
        receInfs = _kokhofinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.NoSum);
        //保険者番号の指定がある場合は絞り込み
        var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
            receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        //保険者番号リストを取得
        hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        //保険者名を取得
        hokensyaNames = _kokhofinder.GetHokensyaName(_hpId, hokensyaNos);

        return (receInfs?.Count ?? 0) > 0;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }
}