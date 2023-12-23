using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Statistics.Sta2011.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P25KokhoSokatuCoReportService : IP25KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 25;
    //県内保険者(固定枠)
    private List<string> prefInHokensyaNo = new List<string> {
            "253013", "250019", "250027", "250035", "250043", "250050", "250068", "250076", "250092", "250100",
            "250118", "250126", "250134", "250522", "250647", "250654", "250712", "250738", "250746", "250753"
        };
    //県外保険者(固定枠)
    private List<string> prefOutHokensyaNo = new List<string> {
            "095013", "133033", "133231", "133264", "133280", "133298", "233064", "263129", "273102"
        };
    //福祉
    private List<string> kohiHoubetus = new List<string> {
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "70", "71", "75", "76", "82", "83", "84", "85", "86"
        };
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSokatuFinder _kokhoFinder;
    private ICoWelfareSeikyuFinder _welfareFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> receInfs;
    private List<CoWelfareReceInfModel> welfareInfs;
    private CoHpInfModel hpInf;
    #endregion

    #region Constructor and Init
    public P25KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder, ICoWelfareSeikyuFinder welfareFinder)
    {
        _kokhoFinder = kokhoFinder;
        _welfareFinder = welfareFinder;
        _setFieldData = new();
        _singleFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        hpInf = new();
        receInfs = new();
        hokensyaNames = new();
        hokensyaNos = new();
        welfareInfs = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    int diskKind;
    int diskCnt;
    private bool hasNextPage;
    private int currentPage;

    // <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p25KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    #endregion

    public CommonReportingRequestModel GetP25KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int diskKind, int diskCnt)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            this.diskKind = diskKind;
            this.diskCnt = diskCnt;
            var getData = GetData();
            hasNextPage = true;
            currentPage = 1;

            if (getData)
            {
                while (getData && hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
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
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
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
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //レセプト記載
            if (new int[] { 0, 1, 2 }.Contains(diskKind))
            {
                SetFieldData("receMedia", "〇");
            }
            else
            {
                SetFieldData("receOnline", "〇");
            }

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region Body
            if (currentPage == 1)
            {
                //1枚目のみ記載する

                //県内保険者
                for (short rowNo = 0; rowNo < prefInHokensyaNo.Count; rowNo++)
                {
                    int receCount = receInfs.Where(r => r.HokensyaNo == prefInHokensyaNo[rowNo]).Count();
                    listDataPerPage.Add(new(string.Format("prefInCount{0}", (short)Math.Floor((double)rowNo / 17)), 0, (short)(rowNo % 17), receCount.ToString()));
                }
                //県外保険者(国保)
                for (short rowNo = 0; rowNo < prefOutHokensyaNo.Count; rowNo++)
                {
                    int receCount = receInfs.Where(r => r.HokensyaNo == prefOutHokensyaNo[rowNo]).Count();
                    listDataPerPage.Add(new("prefOutFixedCount", 0, rowNo, receCount.ToString()));
                }
                //合計
                for (short rowNo = 0; rowNo < 4; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = new();
                    switch (rowNo)
                    {
                        case 0: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsPrefIn).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsPrefIn).ToList(); break;
                        case 2: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && !r.IsPrefIn).ToList(); break;
                        case 3: wrkReces = receInfs.Where(r => r.IsKoukiAll && !r.IsPrefIn).ToList(); break;
                    }
                    if (wrkReces.Count == 0) continue;

                    listDataPerPage.Add(new("totalCount", 0, rowNo, wrkReces.Count.ToString()));
                }
                //磁気媒体種類・枚数
                if (new int[] { 0, 1, 2 }.Contains(diskKind))
                {
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                    fieldDataPerPage.Add(string.Format("diskKind{0}", diskKind), "〇");

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }

                    SetFieldData("diskCnt", diskCnt.ToString());
                }
                //福祉医療費請求書                    
                SetFieldData("welfarePaperCnt", Math.Ceiling((double)welfareInfs.Count / 6).ToString());
            }
            #endregion

            #region 県外保険者
            //県外保険者(国保・固定枠除く)
            const int maxKokhoRow = 4;
            int kokhoIndex = (currentPage - 1) * maxKokhoRow;

            var kokhoNos = receInfs.Where(
                r => (r.IsNrAll || r.IsRetAll) && !r.IsPrefIn && !prefOutHokensyaNo.Contains(r.HokensyaNo)
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            bool kokhoNextPage = true;
            for (short rowNo = 0; rowNo < maxKokhoRow; rowNo++)
            {
                if (kokhoIndex < kokhoNos.Count)
                {
                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == kokhoNos[kokhoIndex])?.Name ?? "";
                    listDataPerPage.Add(new("prefOutName", 0, rowNo, hokensyaName == "" ? kokhoNos[kokhoIndex] : hokensyaName));
                    listDataPerPage.Add(new("prefOutNo", 0, rowNo, kokhoNos[kokhoIndex]));

                    int receCount = receInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex]).Count();
                    listDataPerPage.Add(new("prefOutCount", 0, rowNo, receCount.ToString()));
                }

                kokhoIndex++;
                if (kokhoIndex >= kokhoNos.Count)
                {
                    kokhoNextPage = false;
                    break;
                }
            }

            //県外保険者(後期)
            const int maxKoukiRow = 5;
            int koukiIndex = (currentPage - 1) * maxKoukiRow;

            var koukiNos = receInfs.Where(
                r => r.IsKoukiAll && !r.IsPrefIn
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            if (kokhoNos.Count == 0 && koukiNos.Count == 0)
            {
                hasNextPage = false;
                return 1;
            }

            bool koukiNextPage = true;
            for (short rowNo = 0; rowNo < maxKoukiRow; rowNo++)
            {
                if (koukiIndex < koukiNos.Count)
                {
                    int prefNo = koukiNos[koukiIndex].Substring(koukiNos[koukiIndex].Length - 6, 2).AsInteger();
                    listDataPerPage.Add(new("koukiName", 0, rowNo, PrefCode.PrefName(prefNo)));
                    listDataPerPage.Add(new("koukiNo", 0, rowNo, koukiNos[koukiIndex].ToString()));

                    int receCount = receInfs.Where(r => r.HokensyaNo == koukiNos[koukiIndex]).Count();
                    listDataPerPage.Add(new("koukiCount", 0, rowNo, receCount.ToString()));
                }

                koukiIndex++;
                if (koukiIndex >= koukiNos.Count)
                {
                    koukiNextPage = false;
                    break;
                }
            }

            if (!kokhoNextPage && !koukiNextPage)
            {
                hasNextPage = false;
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
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号リストを取得
        hokensyaNos = receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        //保険者名を取得
        hokensyaNames = _kokhoFinder.GetHokensyaName(hpId, hokensyaNos);

        //福祉           
        SeikyuType welSeikyutype = new SeikyuType(
            isNormal: true, isPaper: true, isDelay: true, isHenrei: true, isOnline: false
        );
        welfareInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, welSeikyutype, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Syaho);

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
