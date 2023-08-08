using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public class P20WelfareSokatuCoReportService : IP20WelfareSokatuCoReportService
{
    #region Constant
    private List<int> kohiHokenNos = new List<int> { 199 };

    private struct cityMst
    {
        public string cityNo;
        public string name;
    }
    private List<cityMst> cityMsts = new List<cityMst>()
        {
            new cityMst() { cityNo = "001", name = "長野市" },
            new cityMst() { cityNo = "002", name = "松本市" },
            new cityMst() { cityNo = "003", name = "上田市" },
            new cityMst() { cityNo = "004", name = "岡谷市" },
            new cityMst() { cityNo = "005", name = "飯田市" },
            new cityMst() { cityNo = "006", name = "諏訪市" },
            new cityMst() { cityNo = "007", name = "須坂市" },
            new cityMst() { cityNo = "008", name = "小諸市" },
            new cityMst() { cityNo = "009", name = "伊那市" },
            new cityMst() { cityNo = "010", name = "駒ヶ根市" },
            new cityMst() { cityNo = "011", name = "中野市" },
            new cityMst() { cityNo = "012", name = "大町市" },
            new cityMst() { cityNo = "013", name = "飯山市" },
            new cityMst() { cityNo = "014", name = "茅野市" },
            new cityMst() { cityNo = "015", name = "塩尻市" },
            new cityMst() { cityNo = "016", name = "千曲市" },
            new cityMst() { cityNo = "017", name = "佐久市" },
            new cityMst() { cityNo = "019", name = "佐久穂町" },
            new cityMst() { cityNo = "020", name = "小海町" },
            new cityMst() { cityNo = "021", name = "川上村" },
            new cityMst() { cityNo = "022", name = "南牧村" },
            new cityMst() { cityNo = "023", name = "南相木村" },
            new cityMst() { cityNo = "024", name = "北相木村" },
            new cityMst() { cityNo = "026", name = "軽井沢町" },
            new cityMst() { cityNo = "028", name = "御代田町" },
            new cityMst() { cityNo = "029", name = "立科町" },
            new cityMst() { cityNo = "033", name = "長和町" },
            new cityMst() { cityNo = "034", name = "東御市" },
            new cityMst() { cityNo = "039", name = "青木村" },
            new cityMst() { cityNo = "040", name = "坂城町" },
            new cityMst() { cityNo = "042", name = "下諏訪町" },
            new cityMst() { cityNo = "043", name = "富士見町" },
            new cityMst() { cityNo = "044", name = "原村" },
            new cityMst() { cityNo = "046", name = "辰野町" },
            new cityMst() { cityNo = "047", name = "箕輪町" },
            new cityMst() { cityNo = "048", name = "飯島町" },
            new cityMst() { cityNo = "049", name = "南箕輪町" },
            new cityMst() { cityNo = "050", name = "中川村" },
            new cityMst() { cityNo = "052", name = "宮田村" },
            new cityMst() { cityNo = "053", name = "木曽町" },
            new cityMst() { cityNo = "054", name = "上松町" },
            new cityMst() { cityNo = "055", name = "南木曽町" },
            new cityMst() { cityNo = "057", name = "木祖村" },
            new cityMst() { cityNo = "061", name = "王滝村" },
            new cityMst() { cityNo = "062", name = "大桑村" },
            new cityMst() { cityNo = "068", name = "筑北村" },
            new cityMst() { cityNo = "069", name = "麻績村" },
            new cityMst() { cityNo = "071", name = "生坂村" },
            new cityMst() { cityNo = "072", name = "波田町" },
            new cityMst() { cityNo = "073", name = "山形村" },
            new cityMst() { cityNo = "074", name = "朝日村" },
            new cityMst() { cityNo = "076", name = "安曇野市" },
            new cityMst() { cityNo = "082", name = "池田町" },
            new cityMst() { cityNo = "083", name = "松川村" },
            new cityMst() { cityNo = "086", name = "白馬村" },
            new cityMst() { cityNo = "087", name = "小谷村" },
            new cityMst() { cityNo = "089", name = "松川町" },
            new cityMst() { cityNo = "090", name = "高森町" },
            new cityMst() { cityNo = "091", name = "阿南町" },
            new cityMst() { cityNo = "093", name = "清内路村" },
            new cityMst() { cityNo = "094", name = "阿智村" },
            new cityMst() { cityNo = "096", name = "平谷村" },
            new cityMst() { cityNo = "097", name = "根羽村" },
            new cityMst() { cityNo = "098", name = "下條村" },
            new cityMst() { cityNo = "099", name = "売木村" },
            new cityMst() { cityNo = "100", name = "天龍村" },
            new cityMst() { cityNo = "101", name = "泰阜村" },
            new cityMst() { cityNo = "102", name = "喬木村" },
            new cityMst() { cityNo = "103", name = "豊丘村" },
            new cityMst() { cityNo = "104", name = "大鹿村" },
            new cityMst() { cityNo = "109", name = "小布施町" },
            new cityMst() { cityNo = "111", name = "高山村" },
            new cityMst() { cityNo = "112", name = "山ノ内町" },
            new cityMst() { cityNo = "113", name = "木島平村" },
            new cityMst() { cityNo = "114", name = "野沢温泉村" },
            new cityMst() { cityNo = "115", name = "信州新町" },
            new cityMst() { cityNo = "117", name = "信濃町" },
            new cityMst() { cityNo = "118", name = "飯綱町" },
            new cityMst() { cityNo = "122", name = "小川村" },
            new cityMst() { cityNo = "123", name = "中条村" },
            new cityMst() { cityNo = "125", name = "栄村" }
        };
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

    private List<string> cityNos;
    #endregion

    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    private const string _formFileName = "p20WelfareSokatu.rse";

    #region Constructor and Init
    public P20WelfareSokatuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
    {
        _welfareFinder = welfareFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        _visibleAtPrint = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private int currentPage;
    private bool hasNextPage;
    #endregion

    public CommonReportingRequestModel GetP20WelfareSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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

    #region Private function
    private bool UpdateDrawForm()
    {
        bool _hasNextPage = true;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //都道府県番号+点数表+医療機関コード
            SetFieldData("hpCode", string.Format("{0}{1}{2}", "20", "1", hpInf.ReceHpCd));
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("hpTel", hpInf.Tel);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            const int maxRow = 40;
            const int maxLineCount = 20;
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            if (currentPage == 1)
            {
                SetFieldData("totalCount", receInfs.Count.ToString());
            }

            int cityIndex = (currentPage - 1) * maxRow;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if (cityNos.Count <= cityIndex)
                {
                    _hasNextPage = false;
                    break;
                }
                string cityNo = cityNos[cityIndex];
                string cityName = cityMsts.Find(c => c.cityNo == cityNo).name;
                int count = receInfs.Where(r => r.TokusyuNo(kohiHokenNos).Substring(0, 3) == cityNo).Count();

                listDataPerPage.Add(new(string.Format("cityNo{0}", (short)Math.Floor((double)rowNo / maxLineCount)), 0, (short)(rowNo % maxLineCount), cityNo.ToString()));
                listDataPerPage.Add(new(string.Format("cityName{0}", (short)Math.Floor((double)rowNo / maxLineCount)), 0, (short)(rowNo % maxLineCount), cityName.ToString()));
                listDataPerPage.Add(new(string.Format("count{0}", (short)Math.Floor((double)rowNo / maxLineCount)), 0, (short)(rowNo % maxLineCount), count.ToString()));

                cityIndex++;
                if (cityIndex >= cityNos.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }
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
        hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
        receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.IchibuFutan, 0);
        //市町村番号の一覧を取得
        cityNos = receInfs
            .Where(r => r.TokusyuNo(kohiHokenNos) != "")
            .GroupBy(r => r.TokusyuNo(kohiHokenNos).Substring(0, 3))
            .OrderBy(r => r.Key)
            .Select(r => r.Key)
            .ToList();

        return (receInfs?.Count ?? 0) == 0 && cityNos.Count == 0;
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
