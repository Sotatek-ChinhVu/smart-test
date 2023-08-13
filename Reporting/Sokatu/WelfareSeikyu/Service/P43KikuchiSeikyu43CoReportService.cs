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
    public class P43KikuchiSeikyu43CoReportService : IP43KikuchiSeikyu43CoReportService
    {
        #region Constant
        private const int myPrefNo = 43;
        private List<int> kohiHokenNos = new List<int> { 143 };
        private List<int> kohiHokenEdaNos = new List<int> { 0, 4 };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP43WelfareReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p43KikuchiSeikyu43.rse";

        #region Constructor and Init
        public P43KikuchiSeikyu43CoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP43KikuchiSeikyu43ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
            bool _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
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
                //医療機関情報
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                const int maxRow = 5;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoP43WelfareReceInfModel> wrkReces = null;

                    switch (rowNo % 5)
                    {
                        case 0: wrkReces = receInfs.Where(r => r.HokenKbn == HokenKbn.Kokho && r.HokenRate == 20).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.HokenKbn == HokenKbn.Kokho && r.HokenRate == 30).ToList(); break;
                        case 2: wrkReces = receInfs.Where(r => r.HokenKbn == HokenKbn.Syaho && r.HokenRate == 20).ToList(); break;
                        case 3: wrkReces = receInfs.Where(r => r.HokenKbn == HokenKbn.Syaho && r.HokenRate == 30).ToList(); break;
                        case 4: wrkReces = receInfs; break;
                    }
                    if (wrkReces == null || wrkReces.Count == 0) continue;

                    //件数
                    listDataPerPage.Add(new("count", 0, rowNo, wrkReces.Count.ToString()));
                    //総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, wrkReces.Sum(r => r.Tensu).ToString()));
                    //一部負担額 ※１の位四捨五入
                    listDataPerPage.Add(new("futan", 0, rowNo, wrkReces.Sum(r => CIUtil.RoundInt(r.HokenIchibuFutan, 1)).ToString()));
                    //請求金額 = 一部負担額 × ３分の２　※小数点以下切り捨て
                    int seikyu = (int)wrkReces.Sum(r => CIUtil.RoundDown(CIUtil.RoundInt(r.HokenIchibuFutan, 1) * 2 / 3, 1));
                    listDataPerPage.Add(new("seikyu", 0, rowNo, seikyu.ToString()));
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.KohiFutan, -1);
            //熊本県用のモデルにコピー
            var wrkReces2 = wrkReces.Select(x => new CoP43WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokenNos));
            //菊池市ひとり親医療費の対象に絞る
            receInfs = wrkReces2.Where(x => kohiHokenEdaNos.Contains(x.KohiHokenEdaNo(kohiHokenNos)) && x.TokusyuNo(kohiHokenNos)?.Length == 8).ToList();
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
