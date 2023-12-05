using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.CommonMasters.Constants;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P20KokhoSokatuCoReportService : IP20KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 20;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSokatuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    private List<CoKaMstModel> kaMsts;
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> curReceInfs;
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p20KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P20KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        hpInf = new();
        receInfs = new();
        hokensyaNames = new();
        kaMsts = new();
        hokensyaNos = new();
        curReceInfs = new();
    }
    #endregion

    public CommonReportingRequestModel GetP20KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            currentPage = 1;
            var getData = GetData();
            hasNextPage = true;

            if (getData)
            {
                for (int prefCnt = 0; prefCnt <= 1; prefCnt++)
                {
                    curReceInfs = receInfs.Where(r => prefCnt == 0 ? r.IsPrefIn : !r.IsPrefIn).ToList();
                    if (curReceInfs.Count() == 0) continue;
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
        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
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
            //診療科
            SetFieldData("kaName", kaMsts[0].KaName);
            //印
            SetVisibleFieldData("inkan", seikyuYm < KaiseiDate.m202210);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region 合計
            if (currentPage == 1)
            {
                //国保合計
                int totalCount = curReceInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList().Count;
                SetFieldData("kokhoTotalCount", totalCount.ToString());
                //後期合計
                totalCount = curReceInfs.Where(r => r.IsKoukiAll).ToList().Count;
                SetFieldData("koukiTotalCount", totalCount.ToString());
            }
            #endregion

            #region Body
            //国保
            const int maxKokhoRow = 32;
            int kokhoIndex = (currentPage - 1) * maxKokhoRow;

            var kokhoNos = curReceInfs.Where(
                r => r.IsNrAll || r.IsRetAll
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            bool kokhoNextPage = true;
            int kokhoSubTotal = 0;
            for (short rowNo = 0; rowNo < maxKokhoRow; rowNo++)
            {
                if (kokhoIndex < kokhoNos.Count)
                {
                    const int maxLineCount = 17;

                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == kokhoNos[kokhoIndex])?.Name ?? "";
                    listDataPerPage.Add(new(
                        string.Format("kokhoName{0}", (short)Math.Floor((double)rowNo / maxLineCount)), 0, (short)(rowNo % maxLineCount),
                        hokensyaName == "" ? kokhoNos[kokhoIndex] : hokensyaName
                    ));

                    int receCount = curReceInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex]).Count();
                    listDataPerPage.Add(new(string.Format("kokhoCount{0}", (short)Math.Floor((double)rowNo / maxLineCount)), 0, (short)(rowNo % maxLineCount), receCount.ToString()));
                    kokhoSubTotal += receCount;
                }

                kokhoIndex++;
                if (kokhoIndex >= kokhoNos.Count)
                {
                    kokhoNextPage = false;
                    break;
                }
            }
            SetFieldData("kokhoSubTotalCount", kokhoSubTotal.ToString());

            //後期
            const int maxKoukiRow = 8;
            int koukiIndex = (currentPage - 1) * maxKoukiRow;

            var koukiNos = curReceInfs.Where(
                r => r.IsKoukiAll
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            if (kokhoNos.Count == 0 && koukiNos.Count == 0)
            {
                hasNextPage = false;
                return 1;
            }

            bool koukiNextPage = true;
            int koukiSubTotal = 0;
            for (short rowNo = 0; rowNo < maxKoukiRow; rowNo++)
            {
                if (koukiIndex < koukiNos.Count)
                {
                    int prefNo = koukiNos[koukiIndex].Substring(koukiNos[koukiIndex].Length - 6, 2).AsInteger();
                    listDataPerPage.Add(new("koukiName", 0, rowNo, PrefCode.PrefName(prefNo)));

                    int receCount = receInfs.Where(r => r.HokensyaNo == koukiNos[koukiIndex]).Count();
                    listDataPerPage.Add(new("koukiCount", 0, rowNo, receCount.ToString()));
                    koukiSubTotal += receCount;
                }

                koukiIndex++;
                if (koukiIndex >= koukiNos.Count)
                {
                    koukiNextPage = false;
                    break;
                }
            }
            SetFieldData("koukiSubTotalCount", koukiSubTotal.ToString());

            if (!kokhoNextPage && !koukiNextPage)
            {
                hasNextPage = false;
            }
            #endregion
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
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        kaMsts = _kokhoFinder.GetKaMst(hpId);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号リストを取得
        hokensyaNos = receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
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
