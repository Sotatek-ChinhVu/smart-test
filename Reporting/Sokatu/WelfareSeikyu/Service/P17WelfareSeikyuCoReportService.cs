using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public class P17WelfareSeikyuCoReportService : IP17WelfareSeikyuCoReportService
{
    #region Constant
    private const int myPrefNo = 17;

    private class CityKohi
    {
        /// <summary>
        /// 市町コード
        /// </summary>
        public string cityNo = string.Empty;

        /// <summary>
        /// 公費の保険番号
        /// </summary>
        public int kohiHokenNo;

        /// <summary>
        /// 種別
        ///     1:乳幼児, 2:児童, 3:ひとり親
        /// </summary>
        public int syubetu;
    }

    private readonly List<CityKohi> CityKohis = new List<CityKohi>()
        {
            new CityKohi(){ cityNo = "172120", kohiHokenNo = 291, syubetu = 3}
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
    private List<int> kohiHokenNos;
    #endregion

    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    private const string _formFileName = "p17WelfareSeikyu.rse";

    #region Constructor and Init
    public P17WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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
        kohiHokenNos = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private int currentPage;
    private bool hasNextPage;
    #endregion

    public CommonReportingRequestModel GetP17WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
        const int maxRow = 12;
        bool _hasNextPage = true;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", string.Format("{0}{1}{2}", myPrefNo, 1, hpInf.HpCd));
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
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //市町コード
            if (receInfs.Count == 0)
            {
                return 1;
            }
            int kohiHokenNo = receInfs[0].KohiHokenNo(kohiHokenNos);
            string cityNo = CityKohis.Find(c => c.kohiHokenNo == kohiHokenNo)?.cityNo ?? string.Empty;
            SetFieldData("cityNo", cityNo.ToString());
            //ページ数
            int totalPage = (int)Math.Ceiling((double)receInfs.Count / maxRow);
            if (totalPage >= 2)
            {
                SetFieldData("currentPage", currentPage.ToString());
                SetFieldData("totalPage", totalPage.ToString());
            }

            return 1;
        }
        #endregion

        #region Body

        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            int ptIndex = (currentPage - 1) * maxRow;

            countData totalData = new countData();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if (receInfs.Count <= ptIndex)
                {
                    _hasNextPage = false;
                    break;
                }
                var curReceInf = receInfs[ptIndex];
                //特殊番号
                if (curReceInf.TokusyuNo(kohiHokenNos) != null)
                {
                    listDataPerPage.Add(new("tokusyuNo", 0, rowNo, curReceInf.TokusyuNo(kohiHokenNos)));
                }
                //保険者番号
                listDataPerPage.Add(new("hokensyaNo", 0, rowNo, curReceInf.HokensyaNo));
                //種別
                int kohiHokenNo = curReceInf.KohiHokenNo(kohiHokenNos);
                int sbt = CityKohis.Find(c => c.kohiHokenNo == kohiHokenNo)?.syubetu ?? 0;
                listDataPerPage.Add(new("syubetu", 0, rowNo, sbt.ToString()));
                //カナ氏名
                listDataPerPage.Add(new("kanaName", 0, rowNo, CIUtil.ToWide(curReceInf.KanaName)));
                //生年月日(g.ee.MM.dd)
                string WDate = CIUtil.SDateToWDate(curReceInf.BirthDay).ToString();
                listDataPerPage.Add(new("birthday", 0, rowNo, string.Format("{0}.{1}.{2}.{3}", WDate.Substring(0, 1), WDate.Substring(1, 2), WDate.Substring(3, 2), WDate.Substring(5, 2))));
                //入外区分
                listDataPerPage.Add(new("gairai", 0, rowNo, "2"));
                //割合
                listDataPerPage.Add(new("hokenRate", 0, rowNo, (curReceInf.HokenRate / 10).ToString()));
                //日数
                //入院日数のことなので印字しない
                //合計点数
                totalData.Tensu += curReceInf.Tensu;
                listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.Tensu.ToString()));
                //一部自己負担
                totalData.Futan += curReceInf.PtFutan;
                listDataPerPage.Add(new("futan", 0, rowNo, curReceInf.PtFutan.ToString()));
                //診療年月(g.ee.MM)
                WDate = CIUtil.SDateToWDate(curReceInf.SinYm * 100 + 1).ToString();
                listDataPerPage.Add(new("sinYm", 0, rowNo, string.Format("{0}.{1}.{2}", WDate.Substring(0, 1), WDate.Substring(1, 2), WDate.Substring(3, 2))));

                ptIndex++;
                if (ptIndex >= receInfs.Count)
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
        kohiHokenNos = CityKohis.Select(c => c.kohiHokenNo).ToList();
        receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.None, 0);

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
