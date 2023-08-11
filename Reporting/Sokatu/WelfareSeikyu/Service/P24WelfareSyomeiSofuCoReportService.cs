using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P24WelfareSyomeiSofuCoReportService : IP24WelfareSyomeiSofuCoReportService
    {
        #region Constant
        private List<int> kohiHokenNos = new List<int> { 101, 102, 103, 105, 106, 107, 203, 206 };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP24WelfareReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        private List<string> cityCodes;
        private string currentCityCode;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p24WelfareSyomeiSofu.rse";

        #region Constructor and Init
        public P24WelfareSyomeiSofuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP24WelfareSyomeiSofuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if (getData)
            {
                foreach (var cityCode in cityCodes)
                {
                    currentCityCode = cityCode;
                    currentPage = 1;
                    hasNextPage = true;

                    while (getData && hasNextPage)
                    {
                        UpdateDrawForm();
                        currentPage++;
                    }
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
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                //医療機関情報
                SetFieldData("postCd", hpInf.PostCdDsp);
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("hpTel", hpInf.Tel);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                //提出年月日
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                SetFieldData("reportGengo", wrkYmd.Gengo);
                SetFieldData("reportYear", wrkYmd.Year.ToString());
                SetFieldData("reportMonth", wrkYmd.Month.ToString());
                SetFieldData("reportDay", wrkYmd.Day.ToString());
                //市町村名
                string cityName = receInfs.Where(r => r.CityCode == currentCityCode).First().CityName;
                SetFieldData("cityName", cityName);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                var curReceInfs = receInfs.Where(r => r.CityCode == currentCityCode);
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
                //市町コード
                fieldDataPerPage.Add("cityCode", curReceInfs.First().CityCode);
                //送付月
                DateTime wrkDate = DateTime.ParseExact((seikyuYm * 100 + 1).ToString(), "yyyyMMdd", null).AddMonths(1);
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(int.Parse(wrkDate.ToString("yyyyMMdd")));
                fieldDataPerPage.Add("seikyuGengo", wrkYmd.Gengo);
                fieldDataPerPage.Add("seikyuYear", wrkYmd.Year.ToString());
                fieldDataPerPage.Add("seikyuMonth", wrkYmd.Month.ToString());

                if (!_setFieldData.ContainsKey(pageIndex))
                {
                    _setFieldData.Add(pageIndex, fieldDataPerPage);
                }
                //件数
                const int maxRow = 6;
                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    int count = 0;
                    switch (rowNo)
                    {
                        case 0: count = curReceInfs.Count(); break;
                        default: count = curReceInfs.Where(r => r.KohiSbt == rowNo).Count(); break;
                    }
                    listDataPerPage.Add(new("count", 0, rowNo, count.ToString()));
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.None, 0);

            //三重県用のモデルクラスにコピー
            receInfs = wrkReces
                .Select(x => new CoP24WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokenNos))
                .OrderBy(r => r.CityCode)
                .ThenBy(r => r.KohiSbt)
                .ToList();

            //市町コードのリストを取得
            cityCodes = receInfs.GroupBy(r => r.CityCode).OrderBy(r => r.Key).Select(r => r.Key).ToList();

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
