using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P23KokhoSokatuCoReportService : IP23KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 23;
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
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoKaMstModel> kaMsts;
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
    private const string _formFileName = "p23KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P23KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
    }
    #endregion

    public CommonReportingRequestModel GetP23KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            for (int i = 0; i <= 1; i++)
            {
                SetFieldData(string.Format("seikyuGengo{0}", i), wrkYmd.Gengo);
                SetFieldData(string.Format("seikyuYear{0}", i), wrkYmd.Year.ToString());
                SetFieldData(string.Format("seikyuMonth{0}", i), wrkYmd.Month.ToString());
            }
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

            #region 合計
            const int maxRow = 4;

            //福祉
            List<string> prefInHoubetus = new List<string> { "81", "82", "83", "85", "89" };
            //全国公費
            var prefAllHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsHeiyo).ToList(), prefInHoubetus);

            if (currentPage == 1)
            {
                //1枚目のみ記載する
                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        case 0: wrkReces = receInfs.Where(r => r.IsNrAll).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsRetAll).ToList(); break;
                        case 2: wrkReces = receInfs.Where(r => r.IsKoukiAll).ToList(); break;
                        case 3: wrkReces = receInfs.ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("totalCount", 0, rowNo, wrkData.Count.ToString()));
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                    listDataPerPage.Add(new("totalNissu", 0, rowNo, wrkData.Nissu.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    listDataPerPage.Add(new("totalTensu", 0, rowNo, wrkData.Tensu.ToString()));
                }

                //「① 公費」欄                    
                int totalKohiCount = 0;
                foreach (var houbetu in prefAllHoubetus)
                {
                    totalKohiCount += receInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
                }

                fieldDataPerPage.Add("totalKohiCount", totalKohiCount.ToString());

                //「② 福祉」欄
                int totalWelfareCount = 0;
                foreach (var houbetu in prefInHoubetus)
                {
                    totalWelfareCount += receInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
                }
                SetFieldData("totalWelfareCount", totalWelfareCount.ToString());
            }
            #endregion

            #region 保険者単位の集計
            const int maxHokensyaCol = 3;
            const int maxHokensyaRow = 8;
            int hokensyaIndex = (currentPage - 1) * maxHokensyaRow;

            int subKohiCount = 0;
            int subWelfareCount = 0;

            //保険者単位で集計
            for (short rowNo = 0; rowNo < maxHokensyaRow; rowNo++)
            {
                var currentNo = hokensyaNos[hokensyaIndex];
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentNo);
                //保険者名
                if (currentNo.Length == 8)
                {
                    int prefNo = currentNo.Substring(currentNo.Length - 6, 2).AsInteger();
                    SetFieldData("hokensyaName", string.Format("{0}広域連合", PrefCode.PrefName(prefNo)));
                }
                else
                {
                    var hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == currentNo)?.Name ?? "";
                    listDataPerPage.Add(new("hokensyaName", 0, rowNo, hokensyaName == "" ? currentNo : hokensyaName));
                }

                for (short colNo = 0; colNo < maxHokensyaCol; colNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (colNo)
                    {
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrAll).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsRetAll).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsKoukiAll).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new(string.Format("count{0}", colNo), 0, rowNo, wrkData.Count.ToString()));
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                    listDataPerPage.Add(new(string.Format("nissu{0}", colNo), 0, rowNo, wrkData.Nissu.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    listDataPerPage.Add(new(string.Format("tensu{0}", colNo), 0, rowNo, wrkData.Tensu.ToString()));
                }

                //公費（小計）
                foreach (var houbetu in prefAllHoubetus)
                {
                    subKohiCount += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
                }
                //福祉（小計）
                foreach (var houbetu in prefInHoubetus)
                {
                    subWelfareCount += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList().Count;
                }

                hokensyaIndex++;
                if (hokensyaIndex >= hokensyaNos.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
            pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            fieldDataPerPage.Add("kohiCount", subKohiCount.ToString());
            fieldDataPerPage.Add("welfareCount", subWelfareCount.ToString());

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
            return false;
        }
        return true;
    }

    private bool GetData()
    {
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        kaMsts = _kokhoFinder.GetKaMst(hpId);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号リストを取得（県内→県外）
        hokensyaNos = receInfs.Where(r => r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        hokensyaNos.AddRange(
            receInfs.Where(r => !r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList()
        );

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
    #endregion
}
