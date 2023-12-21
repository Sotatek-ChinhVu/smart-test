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
    public class P27IzumisanoSeikyuCoReportService : IP27IzumisanoSeikyuCoReportService
    {
        #region Constant
        private List<int> kohiHokenNos = new List<int> { 280 };
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
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p27IzumisanoSeikyu.rse";

        #region Constructor and Init
        public P27IzumisanoSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP27IzumisanoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
            const int maxRow = 10;
            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
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
                //ページ数
                int totalPage = (int)Math.Ceiling((double)receInfs.Count / maxRow);
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

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    //合計
                    if (currentPage == 1)
                    {
                        //件数
                        SetFieldData("totalTensu", receInfs.Count.ToString());
                        //診療総点数
                        SetFieldData("totalTensu", receInfs.Sum(r => r.Tensu).ToString());
                        //一部自己負担
                        SetFieldData("totalFutan0", receInfs.Sum(r => r.KohiReceFutan(kohiHokenNos)).ToString());
                        SetFieldData("totalFutan1", receInfs.Sum(r => r.KohiReceFutan(kohiHokenNos)).ToString());
                        //公費負担額
                        SetFieldData("totalKohiFutan", receInfs.Sum(r => r.KohiFutan(kohiHokenNos)).ToString());
                    }

                    var curReceInf = receInfs[ptIndex];

                    //医療証記号・番号
                    listDataPerPage.Add(new("kohiNo", 0, rowNo, curReceInf.TokusyuNo(kohiHokenNos)));
                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                    //入院外来
                    listDataPerPage.Add(new("gairai", 0, rowNo, "○"));
                    //診療日数
                    listDataPerPage.Add(new("nissu", 0, rowNo, curReceInf.HokenNissu.ToString()));
                    //診療総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.Tensu.ToString()));
                    //一部自己負担
                    listDataPerPage.Add(new("futan", 0, rowNo, curReceInf.KohiReceFutan(kohiHokenNos).ToString()));
                    //公費負担額
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, curReceInf.KohiFutan(kohiHokenNos).ToString()));
                    //国保・社保の別
                    int hokenKbn = curReceInf.HokenKbn == HokenKbn.Kokho ? 0 : 1;
                    listDataPerPage.Add(new("hokenKbn", (short)hokenKbn, rowNo, "○"));

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
            receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.KohiFutan, 0);

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
