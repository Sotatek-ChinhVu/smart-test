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
    public class P40WelfareSeikyuCoReportService : IP40WelfareSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 40;
        private List<string> kohiHoubetus = new List<string> { "81", "80", "90" };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP40WelfareReceInfModel> receInfs;
        private CoHpInfModel hpInf;

        private List<string> futansyaNos;
        private string currentFutansyaNo;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p40WelfareSeikyu.rse";

        #region Constructor and Init
        public P40WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP40WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();

                if (getData)
                {
                    foreach (string currentNo in futansyaNos)
                    {
                        currentFutansyaNo = currentNo;
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
            bool _hasNextPage = false;

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
                //負担者番号
                string futansyaNo = CIUtil.Copy(currentFutansyaNo, 3, 3) + CIUtil.CalcChkDgtM10W2(currentFutansyaNo);
                SetFieldData("futansyaNo", futansyaNo);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                const int maxRow = 9;
                var curReceInfs = receInfs.Where(r => r.FutansyaNo() == currentFutansyaNo);

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoP40WelfareReceInfModel> wrkReces = null;

                    string kohiHoubetu = kohiHoubetus[rowNo / 3];

                    switch (rowNo % 3)
                    {
                        case 0: wrkReces = curReceInfs.Where(r => r.IsKohiHoubetu(kohiHoubetu) && r.HokenRate == 30).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsKohiHoubetu(kohiHoubetu) && r.HokenRate == 20).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsKohiHoubetu(kohiHoubetu) && r.HokenRate != 30 && r.HokenRate != 20).ToList(); break;
                    }
                    if (wrkReces == null || wrkReces.Count == 0) continue;

                    //件数
                    listDataPerPage.Add(new("count", 0, rowNo, wrkReces.Count.ToString()));
                    //日数
                    listDataPerPage.Add(new("nissu", 0, rowNo, wrkReces.Sum(r => r.KohiNissu(kohiHoubetu)).ToString()));
                    //点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, wrkReces.Sum(r => r.KohiTensu(kohiHoubetu)).ToString()));
                    //医療費給付外の額
                    listDataPerPage.Add(new("kyufugai", 0, rowNo, wrkReces.Sum(r => r.KohiKyufugai(kohiHoubetu)).ToString()));
                    //一部負担金
                    listDataPerPage.Add(new("futan", 0, rowNo, wrkReces.Sum(r => r.KohiIchibuFutan(kohiHoubetu)).ToString()));

                    //法定外給付
                    if (new int[] { 2, 5, 8 }.Contains(rowNo))
                    {
                        pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                        Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                        fieldDataPerPage.Add(
                            string.Format("rate{0}", kohiHoubetus[rowNo / 3]),
                            wrkReces.Max(r => r.HokenRate).ToString()
                        );

                        if (!_setFieldData.ContainsKey(pageIndex))
                        {
                            _setFieldData.Add(pageIndex, fieldDataPerPage);
                        }
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan10en, HokenKbn.Syaho);
            //福岡県用のモデルクラスにコピー
            receInfs = wrkReces.Select(x => new CoP40WelfareReceInfModel(x.ReceInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHoubetus)).ToList();
            //負担者番号の一覧を取得
            try
            {
                futansyaNos = receInfs.GroupBy(r => r.FutansyaNo()).OrderBy(r => r.Key).Select(r => r.Key).ToList();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
