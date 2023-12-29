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
    public class P29WelfareSeikyuCoReportService : IP29WelfareSeikyuCoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "71", "80", "81", "91" };
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
        private List<CoWelfareReceInfModel> curReceInfs;
        private CoHpInfModel hpInf;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p29WelfareSeikyu.rse";

        #region Constructor and Init
        public P29WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP29WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();

                if (getData)
                {
                    for (int hokenKbn = 2; hokenKbn >= 1; hokenKbn--)
                    {
                        curReceInfs = receInfs.Where(r => r.HokenKbn == hokenKbn).ToList();
                        if (curReceInfs.Count() == 0) continue;
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
            const int maxRow = 12;
            bool _hasNextPage = true;
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

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
                //保険区分
                listDataPerPage.Add(new("hokenKbn", (short)(curReceInfs[0].HokenKbn == HokenKbn.Kokho ? 0 : 1), 0, "○"));
                //ページ数
                int totalPage = (int)Math.Ceiling((double)curReceInfs.Count / maxRow);
                SetFieldData("currentPage", currentPage.ToString());
                SetFieldData("totalPage", totalPage.ToString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                int ptIndex = (currentPage - 1) * maxRow;

                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = curReceInfs[ptIndex];

                    //負担者番号
                    listDataPerPage.Add(new("futansyaNo0", 0, rowNo, curReceInf.FutansyaNo(kohiHoubetus).Substring(0, 2)));
                    listDataPerPage.Add(new("futansyaNo1", 0, rowNo, curReceInf.FutansyaNo(kohiHoubetus).Substring(4, 4)));
                    //受給者番号
                    listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, curReceInf.JyukyusyaNo(kohiHoubetus)));
                    //保険者番号
                    listDataPerPage.Add(new("hokensyaNo", 0, rowNo, string.Format("{0, 8}", curReceInf.HokensyaNo)));
                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                    //生年月日
                    listDataPerPage.Add(new("birthday", 0, rowNo, CIUtil.SDateToWDate(curReceInf.BirthDay).ToString()));
                    //入外区分
                    listDataPerPage.Add(new("gairai", 0, rowNo, "○"));
                    //割合
                    listDataPerPage.Add(new("hokenRate", 0, rowNo, (curReceInf.HokenRate / 10).ToString()));
                    //実日数
                    listDataPerPage.Add(new("nissu", 0, rowNo, curReceInf.HokenNissu.ToString()));
                    //合計点数
                    totalData.Tensu += curReceInf.Tensu;
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.Tensu.ToString()));
                    //一部自己負担
                    totalData.Futan += curReceInf.PtFutan;
                    listDataPerPage.Add(new("futan", 0, rowNo, curReceInf.PtFutan.ToString()));
                    //長
                    listDataPerPage.Add(new("choki", 0, rowNo, curReceInf.IsChoki ? "○" : ""));
                    //診療年月
                    listDataPerPage.Add(new("sinYm", 0, rowNo, curReceInf.SeikyuKbn >= 1 ? CIUtil.SDateToWDate(curReceInf.SinYm * 100 + 1).ToString().Substring(0, 5) : ""));
                    //備考
                    List<string> bikos = new List<string>();
                    bikos.Add(curReceInf.PrefNo(1) == 0 && curReceInf.Kohi1ReceKisai == 1 ? curReceInf.Kohi1Houbetu : "");
                    bikos.Add(curReceInf.PrefNo(2) == 0 && curReceInf.Kohi2ReceKisai == 1 ? curReceInf.Kohi2Houbetu : "");
                    bikos.Add(curReceInf.PrefNo(3) == 0 && curReceInf.Kohi3ReceKisai == 1 ? curReceInf.Kohi3Houbetu : "");
                    bikos.Add(curReceInf.PrefNo(4) == 0 && curReceInf.Kohi4ReceKisai == 1 ? curReceInf.Kohi4Houbetu : "");
                    bikos.RemoveAll(s => s == "");
                    listDataPerPage.Add(new("biko", 0, rowNo, string.Join(" ", bikos)));

                    ptIndex++;
                    if (ptIndex >= curReceInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }
                _listTextData.Add(pageIndex, listDataPerPage);
                //合計
                SetFieldData("totalTensu", totalData.Tensu.ToString());
                SetFieldData("totalFutan", totalData.Futan.ToString());

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
            receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.IchibuFutan, 0, true);

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
