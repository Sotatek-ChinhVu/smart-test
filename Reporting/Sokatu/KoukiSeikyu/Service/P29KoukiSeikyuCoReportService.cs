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
    private const int myPrefNo = 29;
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    private string currentHokensyaNo;
    private int dataRowCount;
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
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private string _formFileName = "p29KoukiSeikyu.rse";
    private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    #endregion

    public P29KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder finder)
    {
        _kokhofinder = finder;
        _setFieldData = new();
        _singleFieldData = new();
        _listTextData = new();
        _extralData = new();
        _visibleFieldData = new();
        _visibleAtPrint = new();
        _reportConfigPerPage = new();
    }
    public CommonReportingRequestModel GetP29KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if (seikyuYm >= 202210)
            {
                _formFileName = "p29KoukiSeikyu_2210";
            }

            if (getData)
            {
                for (int zaiFlg = 0; zaiFlg <= 1; zaiFlg++)
                {
                    printZaiiso = zaiFlg == 1;
                    foreach (string currentNo in hokensyaNos)
                    {
                        currentHokensyaNo = currentNo;
                        curReceInfs = receInfs.Where(r => r.IsZaiiso == zaiFlg && r.HokensyaNo == currentHokensyaNo).ToList();
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
            return new KoukiSeikyuMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }
        finally
        {
            _kokhofinder.ReleaseResource();
        }
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
            SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == currentHokensyaNo)?.Name ?? "");
            fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo.ToString());
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            _setFieldData.Add(pageIndex, fieldDataPerPage);
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
            int dataIndex = (currentPage - 1) * dataRowCount;

            #region Body
            const int maxRow = 2;

            if (currentPage == 1)
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
            int kohiIndex = (currentPage - 1) * maxKohiRow;

            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
            if (kohiHoubetus.Count == 0)
            {
                hasNextPage = false;
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
                    hasNextPage = false;
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
        hpInf = _kokhofinder.GetHpInf(hpId, seikyuYm);
        receInfs = _kokhofinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.NoSum);
        //保険者番号の指定がある場合は絞り込み
        var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
        receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
        //保険者番号リストを取得
        hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        //保険者名を取得
        hokensyaNames = _kokhofinder.GetHokensyaName(hpId, hokensyaNos);

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