using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P23NagoyaSeikyuCoReportService : IP23NagoyaSeikyuCoReportService
    {

        #region Constant
        private List<string> kohiHoubetus = new List<string> { "89" };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoWelfareReceInfModel> receInfs = new();
        private CoHpInfModel hpInf = new();

        private List<CoHokensyaMstModel> hokensyaNames = new();
        private List<CoKohiHoubetuMstModel> kohiHoubetuMsts = new();
        #endregion

        #region Constructor and Init
        public P23NagoyaSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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
        private bool hasNextPage;
        private int currentPage;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p23NagoyaSeikyu.rse";

        public CommonReportingRequestModel GetP23NagoyaSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
            const int maxRow = 25;
            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
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
                //合計金額
                SetFieldData("totalFutan", receInfs.Sum(r => r.KohiFutan(kohiHoubetus)).ToString());
                //請求総件数
                SetFieldData("totalCount", receInfs.Count().ToString());
                //ページ数
                int totalPage = (int)Math.Ceiling((double)receInfs.Count() / maxRow);
                SetFieldData("currentPage", currentPage.ToString());
                SetFieldData("totalPage", totalPage.ToString());


                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                int ptIndex = (currentPage - 1) * maxRow;

                countData subData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = receInfs[ptIndex];

                    subData.Count++;

                    //受給者証番号
                    listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, curReceInf.TokusyuNo(kohiHoubetus)));
                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                    //一部負担金
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, curReceInf.KohiFutan(kohiHoubetus).ToString()));
                    subData.Futan += curReceInf.KohiFutan(kohiHoubetus);
                    //総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.Tensu.ToString()));
                    subData.Tensu += curReceInf.Tensu;
                    //加入保険
                    listDataPerPage.Add(new("hokenKbn", (short)(curReceInf.HokenKbn == HokenKbn.Syaho ? 1 : 0), rowNo, "○"));

                    //備考
                    List<string> bikos = new List<string>();
                    //月遅れ
                    if (curReceInf.SinYm != seikyuYm)
                    {
                        CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.SinYm * 100 + 1);
                        bikos.Add(string.Format("{0}{1}年{2}月", wrkYmd.Gengo, wrkYmd.Year, wrkYmd.Month));
                    }
                    //一定所得以上
                    if (curReceInf.ReceSbt.Substring(3, 1) == "0")
                    {
                        bikos.Add("3割");
                    }
                    listDataPerPage.Add(new("biko", 0, rowNo, string.Join(" ", bikos)));

                    ptIndex++;
                    if (ptIndex >= receInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //小計
                SetFieldData("subCount", subData.Count.ToString());
                SetFieldData("subTensu", subData.Tensu.ToString());
                SetFieldData("subFutan", subData.Futan.ToString());
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
            receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, 0);

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
}
