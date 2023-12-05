using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P34KokhoSokatuCoReportService : IP34KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 34;
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
    private List<CoReceInfModel> curReceInfs;
    private CoHpInfModel hpInf;
    private List<CoKaMstModel> kaMsts;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p34KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P34KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _listTextData = new();
        _extralData = new();
        _visibleFieldData = new();
        hpInf = new();
        receInfs = new();
        kaMsts = new();
        curReceInfs = new();
    }
    #endregion


    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion


    #region Private function}

    public CommonReportingRequestModel GetP34KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if (getData)
            {
                for (int prefCnt = 0; prefCnt <= 1; prefCnt++)
                {
                    curReceInfs = receInfs.Where(r => prefCnt == 0 ? r.IsPrefIn : !r.IsPrefIn).ToList();
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

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KokhoSokatuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
        }
        finally
        {
            _kokhoFinder.ReleaseResource();
        }
    }

    private bool UpdateDrawForm()
    {
        hasNextPage = true;

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
            SetFieldData("postCd", hpInf.PostCd.Replace("-", ""));
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
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

            const int maxRow = 46;
            int hokIndex = (currentPage - 1) * maxRow;

            //保険者番号リストを取得
            var hokensyaNos = curReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            //集計
            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                var wrkReces = curReceInfs.Where(r => r.HokensyaNo == hokensyaNos[hokIndex]).ToList();

                //保険者番号
                listDataPerPage.Add(new(string.Format("hokensyaNo{0}", (short)Math.Floor((double)rowNo / 25)), 0, (short)(rowNo % 25), hokensyaNos[hokIndex]));

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new(string.Format("count{0}", (short)Math.Floor((double)rowNo / 25)), 0, (short)(rowNo % 25), wrkData.Count.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                listDataPerPage.Add(new(string.Format("tensu{0}", (short)Math.Floor((double)rowNo / 25)), 0, (short)(rowNo % 25), wrkData.Tensu.ToString()));

                hokIndex++;
                if (hokIndex >= hokensyaNos.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }

            //1枚目のみ記載する
            if (currentPage == 1)
            {
                for (short rowNo = 0; rowNo <= 1; rowNo++)
                {
                    //合計
                    List<CoReceInfModel> wrkReces = new();
                    switch (rowNo)
                    {
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsKoukiAll).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("totalCount", 0, rowNo, wrkData.Count.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    listDataPerPage.Add(new("totalTensu", 0, rowNo, wrkData.Tensu.ToString()));
                }
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
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.NoSum);

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
