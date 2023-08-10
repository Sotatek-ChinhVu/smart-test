using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public class P14KokhoSeikyuCoReportService : IP14KokhoSeikyuCoReportService
{
    #region Constant
    private const int myPrefNo = 14;
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
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p14KokhoSeikyu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P14KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinde)
    {
        _kokhoFinder = kokhoFinde;
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

    public CommonReportingRequestModel GetP14KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
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

    #region Private function
    private bool UpdateDrawForm()
    {
        bool _hasNextPage = true;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            Dictionary<string, string> fieldDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
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
            _setFieldData.Add(pageIndex, fieldDataPerPage);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

            const int maxRow = 12;
            //公費（固定枠）
            List<string> fixedHoubetus = new List<string>
                    {
                        "10", "11", "15", "16", "17", "19", "20", "21", "22", "23", "24", "28", "29",
                        "38", "41", "51", "52", "53", "66", "79", "80", "81", "85", "88", "89"
                    };

            if (currentPage == 1)
            {
                #region Body
                //1枚目のみ記載する
                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        //国保
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                        case 1: break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 3: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 4: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        //退職
                        case 5: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 6: break;
                        case 7: wrkReces = curReceInfs.Where(r => r.IsRetElderUpper).ToList(); break;
                        case 8: wrkReces = curReceInfs.Where(r => r.IsRetElderIppan).ToList(); break;
                        case 9: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 10: break;
                        case 11: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
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

                #region 特記事項
                List<string> tokkiCds = new List<string> {
                    "01", "02", "03", "08", "13", "15", "16", "20"
                    };

                //集計
                for (short rowNo = 0; rowNo < tokkiCds.Count; rowNo++)
                {
                    int tokkiCount = curReceInfs.Where(r => r.TokkiContains("01")).ToList().Count;
                    listDataPerPage.Add(new("tokkiCount", 0, rowNo, tokkiCount.ToString()));
                }
                #endregion

                #region 公費（固定枠）
                const int maxLineCount = 8;

                for (short rowNo = 0; rowNo < fixedHoubetus.Count; rowNo++)
                {
                    int fixedCount = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetus[rowNo])).ToList().Count;
                    listDataPerPage.Add(new(string.Format("fixedCount{0}", (short)Math.Floor((double)rowNo / maxLineCount)), 0, (short)(rowNo % maxLineCount), fixedCount.ToString()));
                }
                #endregion
            }

            #region 公費負担医療（フリー枠）
            const int maxKohiRow = 7;
            int kohiIndex = (currentPage - 1) * maxKohiRow;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), fixedHoubetus);
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
                if (kohiHoubetus.Count < kohiIndex)
                {
                    break;
                }
                else
                {
                    listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]));
                }

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));

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
            hasNextPage = _hasNextPage;
            return false;
        }

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
