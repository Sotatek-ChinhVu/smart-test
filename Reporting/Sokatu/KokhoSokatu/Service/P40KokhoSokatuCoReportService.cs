using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P40KokhoSokatuCoReportService : IP40KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 40;

    private List<string> KohiHoubetus = new List<string> { "80", "81", "90" };
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
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    private List<CoKaMstModel> kaMsts;
    private List<CoP40WelfareReceInfModel> welfareInfs;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p40KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P40KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder, ICoWelfareSeikyuFinder welfareFinder)
    {
        _kokhoFinder = kokhoFinder;
        _welfareFinder = welfareFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        hpInf = new();
        receInfs = new();
        kaMsts = new();
        welfareInfs = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion
    public CommonReportingRequestModel GetP40KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関情報
            SetFieldData("postCd", hpInf.PostCdDsp);
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpTel", hpInf.Tel);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //診療科
            SetFieldData("kaName", kaMsts[0].KaName);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region 合計
            const int maxRow = 8;

            if (currentPage == 1)
            {
                //1枚目のみ記載する
                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = new();
                    switch (rowNo)
                    {
                        case 0: wrkReces = receInfs.Where(r => r.IsNrAll).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsRetAll).ToList(); break;
                        case 2: wrkReces = receInfs.Where(r => r.IsKoukiAll).ToList(); break;
                        case 3: wrkReces = receInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                        case 4: wrkReces = receInfs.ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
                    fieldDataPerPage.Add(string.Format("totalCount{0}", rowNo), wrkData.Count.ToString());
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    fieldDataPerPage.Add(string.Format("totalTensu{0}", rowNo), wrkData.Tensu.ToString());

                    //1件当り点数
                    if (rowNo == 4 && wrkData.Count > 0)
                    {
                        int avgTensu = CIUtil.RoundInt((double)wrkData.Tensu / wrkData.Count, 0);
                        fieldDataPerPage.Add("avgTensu", avgTensu.ToString());
                    }

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }
                }

                //医保 障害者・乳幼児・母子医療合計
                int welCount = 0;
                foreach (var houbetu in KohiHoubetus)
                {
                    welCount += welfareInfs.Where(w => w.IsKohiHoubetu(houbetu)).Count();
                }
                SetFieldData("welfareCount", welCount.ToString());

                int welTensu = 0;
                foreach (var houbetu in KohiHoubetus)
                {
                    welTensu += welfareInfs.Where(w => w.IsKohiHoubetu(houbetu)).Sum(w => w.KohiTensu(houbetu));
                }
                SetFieldData("welfareTensu", welTensu.ToString());
            }
            #endregion

            //国保
            int kokhoIndex = (currentPage - 1) * maxRow;

            var kokhoNos = receInfs.Where(
                r => r.IsNrAll || r.IsRetAll
            ).GroupBy(r => new { r.HokensyaNo, r.IsPrefIn }).OrderBy(r => !r.Key.IsPrefIn).ThenBy(r => r.Key.HokensyaNo).Select(r => r.Key.HokensyaNo).ToList();

            bool kokhoNextPage = true;
            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if (kokhoIndex < kokhoNos.Count)
                {
                    listDataPerPage.Add(new("hokensyaNo", 0, rowNo, kokhoNos[kokhoIndex]));

                    for (short colNo = 0; colNo <= 1; colNo++)
                    {
                        List<CoReceInfModel> wrkReces = new();
                        switch (colNo)
                        {
                            case 0: wrkReces = receInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsNrAll).ToList(); break;
                            case 1: wrkReces = receInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex] && r.IsRetAll).ToList(); break;
                        }
                        if (wrkReces == null) continue;

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new(string.Format("kokCount{0}", colNo), 0, rowNo, wrkData.Count.ToString()));
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        listDataPerPage.Add(new(string.Format("kokTensu{0}", colNo), 0, rowNo, wrkData.Tensu.ToString()));
                    }
                }

                kokhoIndex++;
                if (kokhoIndex >= kokhoNos.Count)
                {
                    kokhoNextPage = false;
                    break;
                }
            }

            //後期
            int koukiIndex = (currentPage - 1) * maxRow;

            var koukiNos = receInfs.Where(
                r => r.IsKoukiAll
            ).GroupBy(r => new { r.HokensyaNo, r.IsPrefIn }).OrderBy(r => !r.Key.IsPrefIn).ThenBy(r => r.Key.HokensyaNo).Select(r => r.Key.HokensyaNo).ToList();

            bool koukiNextPage = true;
            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if (koukiIndex < koukiNos.Count)
                {
                    int prefNo = koukiNos[koukiIndex].Substring(koukiNos[koukiIndex].Length - 6, 2).AsInteger();
                    listDataPerPage.Add(new("koukiNo", 0, rowNo, prefNo.ToString()));

                    var wrkReces = receInfs.Where(r => r.HokensyaNo == koukiNos[koukiIndex] && r.IsKoukiAll).ToList();

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("koukiCount", 0, rowNo, wrkData.Count.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    listDataPerPage.Add(new("koukiTensu", 0, rowNo, wrkData.Tensu.ToString()));
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
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumPrefIn);

        //すべて
        SeikyuType seikyuAll = new SeikyuType(
            isNormal: true, isPaper: true, isDelay: true, isHenrei: true, isOnline: true
        );
        //福祉
        var wrkWelfareInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuAll, KohiHoubetus, FutanCheck.KohiFutan10en, HokenKbn.Syaho);
        //福岡県用のモデルクラスにコピー
        welfareInfs = wrkWelfareInfs.Select(x => new CoP40WelfareReceInfModel(x.ReceInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, KohiHoubetus)).ToList();

        return (receInfs?.Count ?? 0) > 0 || (welfareInfs?.Count ?? 0) > 0;
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
