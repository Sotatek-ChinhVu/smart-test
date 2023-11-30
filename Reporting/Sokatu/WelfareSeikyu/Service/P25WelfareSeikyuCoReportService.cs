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
    public class P25WelfareSeikyuCoReportService : IP25WelfareSeikyuCoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "70", "71", "75", "76", "82", "83", "84", "85", "86" };
        private const int myPrefNo = 25;
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
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p25WelfareSeikyu.rse";

        #region Constructor and Init
        public P25WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP25WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
            const int maxRow = 6;
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
                //ページ数
                SetFieldData("currentPage", currentPage.ToString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                int ptIndex = (currentPage - 1) * maxRow;

                //公費分合計
                List<int> totalKohiTensus = new List<int> { 0, 0 };
                List<int> totalKohiFutans = new List<int> { 0, 0 };
                //合計
                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = receInfs[ptIndex];

                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                    //性別
                    listDataPerPage.Add(new("sex", 0, rowNo, curReceInf.Sex.ToString()));
                    //生年月日
                    listDataPerPage.Add(new("birthday", 0, rowNo, CIUtil.SDateToWDate(curReceInf.BirthDay).ToString()));
                    //保険者番号
                    listDataPerPage.Add(new("hokensyaNo", 0, rowNo, curReceInf.HokensyaNo));
                    //記号・番号
                    listDataPerPage.Add(new("kigo", 0, rowNo, curReceInf.Kigo));
                    listDataPerPage.Add(new("bango", 0, rowNo, curReceInf.Bango));

                    //公費１、公費２のIndexを取得
                    int[] kohiIndexes = new int[2] { 1, 0 };
                    for (int i = 1; i <= 4; i++)
                    {
                        if (curReceInf.PrefNo(i) == myPrefNo || curReceInf.KohiReceKisai(i) == 1)
                        {
                            kohiIndexes[0] = i;
                            break;
                        }
                    }
                    for (int i = kohiIndexes[0] + 1; i <= 4; i++)
                    {
                        if (curReceInf.PrefNo(i) == myPrefNo || curReceInf.KohiReceKisai(i) == 1)
                        {
                            kohiIndexes[1] = i;
                            break;
                        }
                    }

                    for (int i = 1; i <= 2; i++)
                    {
                        int kohiIndex = kohiIndexes[i - 1];

                        //公費番号
                        listDataPerPage.Add(new(string.Format("futansyaNo{0}", i), 0, rowNo, curReceInf.FutansyaNo(kohiIndex)));
                        //受給者番号
                        listDataPerPage.Add(new(string.Format("jyukyusyaNo{0}", i), 0, rowNo, curReceInf.JyukyusyaNo(kohiIndex)));
                        //公費分点数
                        totalKohiTensus[i - 1] += curReceInf.KohiReceTensu(kohiIndex);
                        listDataPerPage.Add(new(string.Format("kohiTensu{0}", i), 0, rowNo, curReceInf.KohiReceTensu(kohiIndex).ToString()));
                        //公費対象患者負担額
                        totalKohiFutans[i - 1] += curReceInf.KohiReceFutan(kohiIndex);
                        listDataPerPage.Add(new(string.Format("kohiFutan{0}", i), 0, rowNo, curReceInf.KohiReceFutan(kohiIndex).ToString()));
                    }

                    //診療年月
                    string sinYm = CIUtil.SDateToWDate(curReceInf.SinYm * 100 + 1).ToString().Substring(0, 5);
                    listDataPerPage.Add(new("sinYear", 0, rowNo, sinYm.Substring(1, 2)));
                    listDataPerPage.Add(new("sinMonth", 0, rowNo, sinYm.Substring(3, 2)));

                    //給付割合
                    listDataPerPage.Add(new("hokenRate", 0, rowNo, (100 - curReceInf.HokenRate).ToString()));
                    //本家
                    listDataPerPage.Add(new("honkeKbn", 0, rowNo, curReceInf.ReceSbt.Substring(3, 1)));
                    //日数
                    listDataPerPage.Add(new("nissu", 0, rowNo, curReceInf.HokenNissu.ToString()));
                    //合計点数
                    totalData.Tensu += curReceInf.Tensu;
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.Tensu.ToString()));
                    //一部負担金
                    totalData.Futan += curReceInf.HokenReceFutan;
                    listDataPerPage.Add(new("futan", 0, rowNo, curReceInf.HokenReceFutan.ToString()));
                    //長
                    listDataPerPage.Add(new("choki", 0, rowNo, curReceInf.TokkiContains("02") ? "02" : curReceInf.TokkiContains("16") ? "16" : ""));

                    //件数
                    totalData.Count++;

                    ptIndex++;
                    if (ptIndex >= receInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //合計
                short totalRow = maxRow;

                pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                fieldDataPerPage.Add("totalCount", totalData.Count.ToString());

                if (!_setFieldData.ContainsKey(pageIndex))
                {
                    _setFieldData.Add(pageIndex, fieldDataPerPage);
                }

                listDataPerPage.Add(new("tensu", 0, totalRow, totalData.Tensu.ToString()));
                listDataPerPage.Add(new("futan", 0, totalRow, totalData.Futan.ToString()));
                for (int i = 1; i <= 2; i++)
                {
                    //公費分点数
                    listDataPerPage.Add(new(string.Format("kohiTensu{0}", i), 0, totalRow, totalKohiTensus[i - 1].ToString()));
                    //公費対象患者負担額
                    listDataPerPage.Add(new(string.Format("kohiFutan{0}", i), 0, totalRow, totalKohiFutans[i - 1].ToString()));
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
            receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Syaho);

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
