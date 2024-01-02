using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public class P26KokhoSeikyuOutCoReportService : IP26KokhoSeikyuOutCoReportService
{
    #region Constant
    private const int myPrefNo = 26;
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
    private const string _formFileName = "p26KokhoSeikyuOut.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P26KokhoSeikyuOutCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
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

    public CommonReportingRequestModel GetP26KokhoSeikyuOutReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
        bool _hasNextPage = false;
        Dictionary<string, string> fieldDataPerPage = new();
        var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
        List<ListTextObject> listDataPerPage = new();

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
            fieldDataPerPage.Add("hokensyaNo", string.Format("{0, 8}", currentHokensyaNo));
            _setFieldData.Add(pageIndex, fieldDataPerPage);
            //印
            SetVisibleFieldData("inkan", seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("inkanMaru", seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("kbnRate9", seikyuYm < KaiseiDate.m202210);
            SetVisibleFieldData("kbnIppan", seikyuYm >= KaiseiDate.m202210);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

            #region Body
            countData totalData = new countData();
            const int maxRow = 14;

            if (currentPage == 1)
            {
                //1枚目のみ記載する
                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        //国保
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                        case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        //退職
                        case 4: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 5: break;
                        case 6: break;
                        case 7: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 8: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                        case 9: break;
                        case 10: break;
                        //後期
                        case 11: wrkReces = curReceInfs.Where(r => r.IsKoukiIppan).ToList(); break;
                        case 12: wrkReces = curReceInfs.Where(r => r.IsKoukiUpper).ToList(); break;
                        //合計
                        case 13:
                            listDataPerPage.Add(new("count", 0, rowNo, totalData.Count.ToString()));
                            listDataPerPage.Add(new("nissu", 0, rowNo, totalData.Nissu.ToString()));
                            listDataPerPage.Add(new("tensu", 0, rowNo, totalData.Tensu.ToString()));
                            listDataPerPage.Add(new("futan", 0, rowNo, totalData.Futan.ToString()));
                            break;
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

                    //合計集計
                    totalData.AddValue(wrkData);
                }
            }
            #endregion

            #region 公費負担医療
            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
            if (kohiHoubetus.Count == 0)
            {
                _listTextData.Add(pageIndex, listDataPerPage);
                _hasNextPage = false;
                return 1;
            }

            //集計
            totalData.Clear();

            for (short rowNo = 0; rowNo < kohiHoubetus.Count; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                //法別番号
                if (rowNo < 10)
                {
                    listDataPerPage.Add(new("kohiHoubetu", (short)(rowNo % 5), (short)Math.Floor((double)rowNo / 5), kohiHoubetus[rowNo]));
                }
                countData wrkData = new countData();

                wrkData.Count = wrkReces.Count;
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[rowNo]));
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo]));
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo]));
                //合計
                totalData.AddValue(wrkData);
            }
            Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
            //件数
            fieldDataPerPage.Add("kohiCount", totalData.Count.ToString());
            //日数
            fieldDataPerPage.Add("kohiNissu", totalData.Nissu.ToString());
            //点数
            fieldDataPerPage.Add("kohiTensu", totalData.Tensu.ToString());
            //一部負担金
            fieldDataPerPage.Add("kohiFutan", totalData.Futan.ToString());
            
            if (!_setFieldData.ContainsKey(pageIndex))
            {
                _setFieldData.Add(pageIndex, fieldDataPerPage);
            }
            #endregion
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
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefOut, myPrefNo, HokensyaNoKbn.NoSum);
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
