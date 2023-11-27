using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public class P34KoukiSeikyuCoReportService : IP34KoukiSeikyuCoReportService
{
    #region Constant
    private const int myPrefNo = 34;

    private List<string> fixedHoubetuP1 = new List<string> { "19", "91", "92", "93" };
    private List<string> fixedHoubetuP3 = new List<string> { "10", "11" };
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKoukiSeikyuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private string currentHokensyaNo;
    private List<string> hokensyaNos;
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private List<string> printHokensyaNos;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private string _formFileName = "p34KoukiSeikyu.rse";
    private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;
    private readonly Dictionary<string, bool> _visibleAtPrint;

    public P34KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _setFieldData = new();
        _singleFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        _visibleAtPrint = new();
        _reportConfigPerPage = new();
    }

    public CommonReportingRequestModel GetP34KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if (seikyuYm >= 202210)
            {
                _formFileName = "p34KoukiSeikyu_2210.rse";
            }

            if (getData)
            {
                foreach (string currentNo in hokensyaNos)
                {
                    currentPage = 1;
                    currentHokensyaNo = currentNo;
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

            return new KoukiSeikyuMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }
        finally
        {
            _kokhoFinder.ReleaseResource();
        }
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
            SetFieldData("hpCode", hpInf.ReceHpCd);

            if (currentPage >= 2) return 1;

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
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //保険者
            int prefNo = currentHokensyaNo.Substring(currentHokensyaNo.Length - 6, 2).AsInteger();
            SetFieldData("hokensyaPref", PrefCode.PrefName(prefNo));
            fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo.ToString());
            _setFieldData.Add(pageIndex, fieldDataPerPage);
            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBodyP1()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);
            var count = curReceInfs.Count();

            #region Body
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
            #endregion

            //公費負担医療（固定枠）
            for (short rowNo = 0; rowNo < fixedHoubetuP1.Count; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetuP1.Any() ? fixedHoubetuP1[rowNo] : string.Empty)).ToList();

                countData wrkData = new countData();

                wrkData.Count = wrkReces.Count;
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(fixedHoubetuP1.Any() ? fixedHoubetuP1[rowNo] : string.Empty));
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(fixedHoubetuP1.Any() ? fixedHoubetuP1[rowNo] : string.Empty));
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(fixedHoubetuP1.Any() ? fixedHoubetuP1[rowNo] : string.Empty));

                listDataPerPage.Add(new("fixedCount", 0, rowNo, wrkData.Count.ToString()));
                if (fixedHoubetuP1[rowNo] == "19")
                {
                    listDataPerPage.Add(new("fixedNissu", 0, rowNo, wrkData.Nissu.ToString()));
                    listDataPerPage.Add(new("fixedTensu", 0, rowNo, wrkData.Tensu.ToString()));
                }
                if (fixedHoubetuP1[rowNo] != "93")
                {
                    listDataPerPage.Add(new("fixedFutan", 0, rowNo, wrkData.Futan.ToString()));
                }
            }

            #region 公費負担医療（フリー枠）
            const int maxKohiRow = 3;
            //固定枠の法別番号リスト
            List<string> excludeHoubetu = fixedHoubetuP1.Union(fixedHoubetuP3).ToList();

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), excludeHoubetu);
            if (kohiHoubetus.Count == 0)
            {
                _listTextData.Add(pageIndex, listDataPerPage);
                hasNextPage = false;
                return 1;
            }

            //集計
            for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
            {
                if (kohiHoubetus.Count <= rowNo)
                {
                    hasNextPage = true;
                    break;
                }

                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus.Any() ? kohiHoubetus[rowNo] : string.Empty)).ToList();

                //法別番号
                listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus.Any() ? kohiHoubetus[rowNo] : string.Empty));

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                //日数
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus.Any() ? kohiHoubetus[rowNo] : string.Empty));
                listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus.Any() ? kohiHoubetus[rowNo] : string.Empty));
                listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus.Any() ? kohiHoubetus[rowNo] : string.Empty));
                listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));
            }

            //続紙に記載するデータの存在確認
            int fixedCountP3 = 0;
            for (short i = 0; i < fixedHoubetuP3.Count; i++)
            {
                fixedCountP3 += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetuP3.Any() ? fixedHoubetuP3[i] : string.Empty)).ToList().Count;
            }

            if (maxKohiRow >= kohiHoubetus.Count && fixedCountP3 == 0)
            {
                _listTextData.Add(pageIndex, listDataPerPage);
                hasNextPage = false;
            }
            _listTextData.Add(pageIndex, listDataPerPage);
            #endregion
            return 1;
        }

        #region BodyP3
        int UpdateFormBodyP3()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo).ToList();

            //公費負担医療（固定枠）
            for (short rowNo = 0; rowNo < fixedHoubetuP3.Count; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetuP3.Any() ? fixedHoubetuP3[rowNo] : string.Empty)).ToList();

                countData wrkData = new countData();

                wrkData.Count = wrkReces.Count;
                wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(fixedHoubetuP3.Any() ? fixedHoubetuP3[rowNo] : string.Empty));
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(fixedHoubetuP3.Any() ? fixedHoubetuP3[rowNo] : string.Empty));
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(fixedHoubetuP3.Any() ? fixedHoubetuP3[rowNo] : string.Empty));

                listDataPerPage.Add(new("fixedCount", 0, rowNo, wrkData.Count.ToString()));
                listDataPerPage.Add(new("fixedNissu", 0, rowNo, wrkData.Nissu.ToString()));
                listDataPerPage.Add(new("fixedTensu", 0, rowNo, wrkData.Tensu.ToString()));
                if (fixedHoubetuP3[rowNo] == "11")
                {
                    listDataPerPage.Add(new("fixedFutan", 0, rowNo, wrkData.Futan.ToString()));
                }
            }

            #region 公費負担医療（フリー枠）
            const int maxKohiRow = 6;
            const int p2FreeCount = 3;
            //固定枠の法別番号リスト
            List<string> excludeHoubetu = fixedHoubetuP1.Union(fixedHoubetuP3).ToList();

            int kohiIndex = (currentPage - 2) * maxKohiRow + p2FreeCount;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), excludeHoubetu);
            if (kohiHoubetus.Count == 0 || kohiHoubetus.Count <= kohiIndex)
            {
                _listTextData.Add(pageIndex, listDataPerPage);
                hasNextPage = false;
            }

            //集計
            for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty)).ToList();

                //法別番号
                listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus.Any() ? kohiHoubetus[kohiIndex].ToString() : string.Empty));

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
                    hasNextPage = false;
                    break;
                }
            }
            #endregion
            _listTextData.Add(pageIndex, listDataPerPage);
            return 1;
        }
        #endregion

        #endregion

        #endregion

        switch (currentPage)
        {
            case 1:
                if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                {
                    return false;
                }
                break;
            default:
                if (UpdateFormHeader() < 0 || UpdateFormBodyP3() < 0)
                {
                    return false;
                }
                break;
        }

        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.NoSum);
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
