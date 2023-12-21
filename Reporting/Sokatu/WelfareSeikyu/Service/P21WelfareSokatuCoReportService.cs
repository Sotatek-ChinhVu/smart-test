using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public class P21WelfareSokatuCoReportService : IP21WelfareSokatuCoReportService
{
    #region Constant
    private List<string> kohiHoubetus = new List<string> { "43", "44", "45", "46" };
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoWelfareSeikyuFinder _welfareFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoWelfareReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion

    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    private const string _formFileName = "p21WelfareSokatu.rse";

    #region Constructor and Init
    public P21WelfareSokatuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
    {
        _welfareFinder = welfareFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        _visibleAtPrint = new();
        hpInf = new();
        receInfs = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private int currentPage;
    private bool hasNextPage;
    #endregion

    public CommonReportingRequestModel GetP21WelfareSokatuCoReportService(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            currentPage = 1;
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
            return new WelfareSeikyuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }
        finally
        {
            _welfareFinder.ReleaseResource();
        }
    }

    #region Private function
    private bool UpdateDrawForm()
    {
        bool _hasNextPage = false;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //控えループ
            for (int i = 0; i <= 1; i++)
            {
                //医療機関コード
                SetFieldData(string.Format("hpCode{0}", i), hpInf.ReceHpCd);
                //医療機関情報
                SetFieldData(string.Format("address1{0}", i), hpInf.Address1);
                SetFieldData(string.Format("address2{0}", i), hpInf.Address2);
                SetFieldData(string.Format("hpName{0}", i), hpInf.ReceHpName);
                SetFieldData(string.Format("hpTel{0}", i), hpInf.Tel);
                SetFieldData(string.Format("kaisetuName{0}", i), hpInf.KaisetuName);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData(string.Format("seikyuGengo{0}", i), wrkYmd.Gengo);
                SetFieldData(string.Format("seikyuYear{0}", i), wrkYmd.Year.ToString());
                SetFieldData(string.Format("seikyuMonth{0}", i), wrkYmd.Month.ToString());
            }

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            const int maxRow = 10;

            //請求書枚数
            int totalPage = (int)Math.Ceiling((double)receInfs.Count / maxRow);
            SetFieldData("totalPage0", totalPage.ToString());
            SetFieldData("totalPage1", totalPage.ToString());
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
        hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, 0);

        return (receInfs?.Count ?? 0) == 0;
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
