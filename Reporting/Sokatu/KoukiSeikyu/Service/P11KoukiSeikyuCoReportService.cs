﻿using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public class P11KoukiSeikyuCoReportService : IP11KoukiSeikyuCoReportService
{
    #region Constant
    private const int MyPrefNo = 11;
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
    private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private const string _formFileName = "p11KoukiSeikyu.rse";

    #region Constructor and Init
    public P11KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _singleFieldDataM = new();
        _listTextData = new();
        _extralData = new();
        _visibleFieldData = new();
    }
    #endregion
    public CommonReportingRequestModel GetP11KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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

    #region Private function
    private bool UpdateDrawForm()
    {
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
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //保険者（都道府県番号のみ）
            fieldDataPerPage.Add("hokensyaNo", _currentHokensyaNo.Substring(2, 2));
            _singleFieldDataM.Add(pageIndex, fieldDataPerPage);
            //広域連合名
            SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == _currentHokensyaNo)?.Name ?? "");

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == _currentHokensyaNo);

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
            }
            #endregion

            //特別療養費
            if (_currentPage == 1)
            {
                var wrkReces = tokuyohiReceInfs.Where(r => r.HokensyaNo == _currentHokensyaNo).ToList();

                if (wrkReces.Count >= 1)
                {
                    SetFieldData("tokuyohiCount", wrkReces.Count.ToString());
                    SetFieldData("tokuyohiNissu", wrkReces.Sum(r => r.HokenNissu).ToString());
                    SetFieldData("tokuyohiTensu", wrkReces.Sum(r => r.Tensu).ToString());
                }
            }

            #region 公費負担医療
            const int maxKohiRow = 2;
            int kohiIndex = (_currentPage - 1) * maxKohiRow;

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
        receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.NoSum);

        //特別療養費
        tokuyohiReceInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.TokuyohiKouki, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.NoSum);

        //保険者番号の指定がある場合は絞り込み
        var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
            receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        wrkReceInfs.AddRange(tokuyohiReceInfs);
        //保険者番号リストを取得
        hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        //公費法別番号リストを取得
        kohiHoubetuMsts = _kokhoFinder.GetKohiHoubetuMst(_hpId, _seikyuYm);
        //保険者名を取得
        hokensyaNames = _kokhoFinder.GetHokensyaName(_hpId, hokensyaNos);

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
