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
    public class P35WelfareSokatuCoReportService : IP35WelfareSokatuCoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "81" };
        private List<string> kohiHoubetus10 = new List<string> { "10" };
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
        private List<string> futansyaNos;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p35WelfareSokatu.rse";

        #region Constructor and Init
        public P35WelfareSokatuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP35WelfareSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();
            VisibleAtPrint("Frame", true);
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
            const int maxRow = 5;
            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //一般後期区分
                SetFieldData("kbn", "1");
                //県番号
                SetFieldData("prefNo", "35");
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                //医療機関情報
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                int kohiIndex = (currentPage - 1) * maxRow;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    string futansyaNo = futansyaNos[kohiIndex];
                    var wrkReces = receInfs.Where(r => r.FutansyaNo(kohiHoubetus) == futansyaNo).ToList();
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
                    //負担者番号
                    fieldDataPerPage.Add(string.Format("futansyaNo{0}", rowNo), futansyaNo);
                    //件数
                    fieldDataPerPage.Add(string.Format("count{0}", rowNo), wrkReces.Count.ToString().PadLeft(4, ' '));
                    //請求点数
                    fieldDataPerPage.Add(string.Format("tensu{0}", rowNo), wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus)).ToString().PadLeft(8, ' '));
                    //結核点数
                    int wrkTensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus10));
                    if (wrkTensu > 0)
                    {
                        fieldDataPerPage.Add(string.Format("kekkakuTensu{0}", rowNo), wrkTensu.ToString().PadLeft(7, ' '));
                    }
                    //請求額
                    fieldDataPerPage.Add(string.Format("seikyu{0}", rowNo), wrkReces.Sum(r => r.KohiFutan(kohiHoubetus)).ToString().PadLeft(8, ' '));
                    //一部負担金
                    fieldDataPerPage.Add(string.Format("futan{0}", rowNo), wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus)).ToString().PadLeft(7, ' '));

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }

                    kohiIndex++;
                    if (kohiIndex >= futansyaNos.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

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
            receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Syaho);
            //負担者番号の一覧を取得
            futansyaNos = receInfs.GroupBy(r => r.FutansyaNo(kohiHoubetus)).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void VisibleAtPrint(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleAtPrint.ContainsKey(field))
            {
                _visibleAtPrint.Add(field, value);
            }
        }
        #endregion
    }
}
