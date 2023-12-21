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
    public class P35WelfareSeikyuCoReportService : IP35WelfareSeikyuCoReportService
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
        private List<CoP35WelfareReceInfModel> receInfs = new();
        private CoHpInfModel hpInf = new();
        private List<(string code, string name)> cityNames = new();
        private string currentFutansyaNo = "";
        private string currentCityName = "";
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p35WelfareSeikyu.rse";

        #region Constructor and Init
        public P35WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP35WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();

                if (getData)
                {
                    foreach ((string currentCode, string currentCity) in cityNames)
                    {
                        currentFutansyaNo = currentCode;
                        currentCityName = currentCity;
                        VisibleAtPrint("Frame", true);
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
            finally
            {
                _welfareFinder.ReleaseResource();
            }
        }

        #region Private function
        private bool UpdateDrawForm()
        {
            const int maxRow = 3;
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
                //提出年月日
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                SetFieldData("reportYear", wrkYmd.Year.ToString());
                SetFieldData("reportMonth", wrkYmd.Month.ToString());
                SetFieldData("reportDay", wrkYmd.Day.ToString());
                //負担者番号
                SetFieldData("futansyaNo", CIUtil.Copy(currentFutansyaNo, 5, 4));
                //市町村
                SetFieldData("hokensyaName", currentCityName);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

                var curReceInfs = receInfs.Where(r => r.WelfareFutansyaNo == currentFutansyaNo).OrderBy(r => r.WelfareJyukyusyaNo).ThenBy(r => r.PtNum).ToList();

                int ptIndex = (currentPage - 1) * maxRow;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = curReceInfs[ptIndex];
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
                    //診療年月
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(curReceInf.SinYm * 100 + 1);
                    fieldDataPerPage.Add(string.Format("SinYm{0}", rowNo), $"{wareki.Year.ToString().PadLeft(2, '0')}{wareki.Month.ToString().PadLeft(2, '0')}");
                    //入外
                    fieldDataPerPage.Add(string.Format("nyugai{0}", rowNo), curReceInf.GetNyugai().ToString());
                    //受給者証番号
                    fieldDataPerPage.Add(string.Format("kigou{0}", rowNo), CIUtil.Copy(curReceInf.WelfareJyukyusyaNo?.PadLeft(8, ' ') ?? string.Empty, 1, 3));
                    fieldDataPerPage.Add(string.Format("bangou{0}", rowNo), CIUtil.Copy(curReceInf.WelfareJyukyusyaNo?.PadLeft(8, ' ') ?? string.Empty, 4, 5));
                    //性別
                    fieldDataPerPage.Add(string.Format("sex{0}", rowNo), curReceInf.Sex.ToString());
                    //氏名
                    fieldDataPerPage.Add(string.Format("ptName{0}", rowNo), curReceInf.PtName);
                    //生年月日
                    fieldDataPerPage.Add(string.Format("Birthday{0}", rowNo), CIUtil.SDateToShowWDate3(curReceInf.Birthday).GYmd);
                    //保険者番号
                    fieldDataPerPage.Add(string.Format("hokensyaNo{0}", rowNo), curReceInf.HokensyaNo);
                    //特殊（マル長）
                    if (curReceInf.TokkiContains("02"))
                    {
                        fieldDataPerPage.Add(string.Format("tokushu{0}", rowNo), "5");
                    }
                    if (curReceInf.TokkiContains("16"))
                    {
                        fieldDataPerPage.Add(string.Format("tokushu{0}", rowNo), "2");
                    }
                    //日数
                    fieldDataPerPage.Add(string.Format("nissu{0}", rowNo), curReceInf.HokenNissu.ToString().PadLeft(2, ' '));
                    //請求点数
                    fieldDataPerPage.Add(string.Format("tensu{0}", rowNo), curReceInf.KohiReceTensu(kohiHoubetus).ToString().PadLeft(7, ' '));
                    //結核点数
                    int wrkTensu = curReceInf.KohiReceTensu(kohiHoubetus10);
                    if (wrkTensu > 0)
                    {
                        fieldDataPerPage.Add(string.Format("kekkakuTensu{0}", rowNo), wrkTensu.ToString().PadLeft(6, ' '));
                    }
                    //請求額
                    fieldDataPerPage.Add(string.Format("seikyu{0}", rowNo), curReceInf.KohiFutan.ToString().PadLeft(7, ' '));
                    //一部負担金
                    fieldDataPerPage.Add(string.Format("futan{0}", rowNo), curReceInf.KohiReceFutan(kohiHoubetus).ToString().PadLeft(6, ' '));

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }

                    ptIndex++;
                    if (ptIndex >= curReceInfs.Count)
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Syaho);
            //山口県用のモデルクラスにコピー
            receInfs = wrkReces.Select(x => new CoP35WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHoubetus)).ToList();
            //負担者番号の一覧を取得
            cityNames =
                receInfs.GroupBy(r => (r.WelfareFutansyaNo, r.CityName))
                .OrderBy(r => r.Key.WelfareFutansyaNo)
                .Select(r => (r.Key.WelfareFutansyaNo, r.Key.CityName)).ToList();

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
