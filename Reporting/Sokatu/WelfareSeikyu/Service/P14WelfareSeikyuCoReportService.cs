using Helper.Common;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P14WelfareSeikyuCoReportService : IP14WelfareSeikyuCoReportService
    {
        #region Constant
        private List<string> kohiHoubetus;
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP14WelfareReceInfModel> receInfs;
        private CoHpInfModel hpInf;

        private List<string> futansyaNos;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;

        #region Constructor and Init
        public P14WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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
        private int welfareType;
        private int currentPage;
        private bool hasNextPage;
        #endregion

        public CommonReportingRequestModel GetP14WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;

            switch (welfareType)
            {
                //障害者医療費助成事業請求書
                case 0: kohiHoubetus = new List<string> { "80" }; break;
                //小児医療費助成事業請求書
                case 1: kohiHoubetus = new List<string> { "81" }; break;
                //ひとり親家庭等医療費助成事業請求書
                case 2: kohiHoubetus = new List<string> { "85" }; break;
                //小児ぜん息患者医療費支給事業請求書
                case 3: kohiHoubetus = new List<string> { "88" }; break;
                //成人ぜん息患者医療費助成事業請求書
                case 4: kohiHoubetus = new List<string> { "89" }; break;
            }

            var getData = GetData();

            string _formFileName = "";
            switch (welfareType)
            {
                case 0: _formFileName = "p14WelfareSeikyu80.rse"; break;
                case 1: _formFileName = "p14WelfareSeikyu81.rse"; break;
                case 2: _formFileName = "p14WelfareSeikyu85.rse"; break;
                case 3: _formFileName = "p14WelfareSeikyu88.rse"; break;
                case 4: _formFileName = "p14WelfareSeikyu89.rse"; break;
            }

            currentPage = 1;
            hasNextPage = true;

            if (getData)
            {
                while (getData && hasNextPage)
                {
                    if (_formFileName == "") continue;
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
            const int maxRow = 8;
            bool _hasNextPage = true;

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
                SetFieldData("hpTel", hpInf.Tel);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
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

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

                int kohiIndex = (currentPage - 1) * maxRow;

                int totalCount = 0;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    if (futansyaNos.Count <= kohiIndex)
                    {
                        _hasNextPage = false;
                        break;
                    }
                    string futansyaNo = futansyaNos[kohiIndex];
                    var wrkReces = receInfs.Where(r => r.FutansyaNo(kohiHoubetus) == futansyaNo).ToList();

                    //負担者番号
                    listDataPerPage.Add(new("futansyaNo", 0, rowNo, futansyaNo.Substring(4, 4)));
                    //療養の給付・件数
                    listDataPerPage.Add(new("count", 0, rowNo, wrkReces.Count.ToString()));
                    //療養の給付・点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus)).ToString()));
                    //医療費助成事業一部負担金・件数（償還を除く）
                    var kohiReces = wrkReces.Where(r => r.KohiHokenNo(kohiHoubetus) != "1800");
                    listDataPerPage.Add(new("kohiCount", 0, rowNo, kohiReces.Where(r => r.KohiReceFutan(kohiHoubetus) > 0).Count().ToString()));
                    //医療費助成事業一部負担金・金額
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, kohiReces.Sum(r => r.KohiReceFutan(kohiHoubetus)).ToString()));
                    if (welfareType != 1)
                    {
                        //一部負担金・件数（高額療養費のレセ給付対象額）
                        listDataPerPage.Add(new("receKyufuCount", 0, rowNo, wrkReces.Where(r => r.KohiReceKyufu(kohiHoubetus) > 0).Count().ToString()));
                        //一部負担金・金額
                        listDataPerPage.Add(new("receKyufu", 0, rowNo, wrkReces.Sum(r => r.KohiReceKyufu(kohiHoubetus)).ToString()));
                    }

                    kohiIndex++;
                    if (kohiIndex >= futansyaNos.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //最終ページに合計と再掲
                if (!_hasNextPage)
                {
                    //療養の給付・件数
                    listDataPerPage.Add(new("count", 0, maxRow, receInfs.Count.ToString()));
                    //療養の給付・点数
                    listDataPerPage.Add(new("tensu", 0, maxRow, receInfs.Sum(r => r.KohiReceTensu(kohiHoubetus)).ToString()));
                    //医療費助成事業一部負担金・件数（償還を除く）
                    var kohiReces = receInfs.Where(r => r.KohiHokenNo(kohiHoubetus) != "1800");
                    listDataPerPage.Add(new("kohiCount", 0, maxRow, kohiReces.Where(r => r.KohiReceFutan(kohiHoubetus) > 0).Count().ToString()));
                    //医療費助成事業一部負担金・金額
                    listDataPerPage.Add(new("kohiFutan", 0, maxRow, kohiReces.Sum(r => r.KohiReceFutan(kohiHoubetus)).ToString()));
                    if (welfareType != 1)
                    {
                        //一部負担金・件数（高額療養費のレセ給付対象額）
                        listDataPerPage.Add(new("receKyufuCount", 0, maxRow, receInfs.Where(r => r.KohiReceKyufu(kohiHoubetus) > 0).Count().ToString()));
                        //一部負担金・金額
                        listDataPerPage.Add(new("receKyufu", 0, maxRow, receInfs.Sum(r => r.KohiReceKyufu(kohiHoubetus)).ToString()));
                    }

                    //再掲
                    int maxSaikei = 6;
                    for (int rowNo = 0; rowNo < maxSaikei; rowNo++)
                    {
                        int count = 0;
                        switch (rowNo)
                        {
                            case 0: count = receInfs.Where(r => r.IsNrMine).Count(); break;
                            case 1: count = receInfs.Where(r => r.IsNrFamily).Count(); break;
                            case 2: count = receInfs.Where(r => r.IsNrPreSchool).Count(); break;
                            case 3: count = receInfs.Where(r => r.IsChoki).Count(); break;
                            case 4: count = receInfs.Where(r => r.IsNrElderUpper).Count(); break;
                            case 5: count = receInfs.Where(r => r.IsNrElderIppan).Count(); break;
                        }

                        string fieldName = string.Format("totalCount{0}", rowNo);
                        SetFieldData(fieldName, count.ToString());
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Syaho);
            //神奈川県用のモデルクラスにコピー
            receInfs = wrkReces.Select(x => new CoP14WelfareReceInfModel(x.ReceInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4)).ToList();
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
        #endregion
    }
}
