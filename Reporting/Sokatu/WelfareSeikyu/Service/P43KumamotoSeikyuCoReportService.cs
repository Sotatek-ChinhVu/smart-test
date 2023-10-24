using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P43KumamotoSeikyuCoReportService : IP43KumamotoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 43;
        private List<string> kohiHoubetus;

        private struct countData
        {
            public int RecordCount;
            public int Tensu;
        }
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

        #region Constructor and Init
        public P43KumamotoSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

        public CommonReportingRequestModel GetP43KumamotoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            this.welfareType = welfareType;

            switch (welfareType)
            {
                //子ども医療費請求書
                case 0: kohiHoubetus = new List<string> { "41" }; break;
                //障がい者医療費請求書
                case 1: kohiHoubetus = new List<string> { "42" }; break;
                //ひとり親家庭等医療費請求書
                case 2: kohiHoubetus = new List<string> { "43" }; break;
            }

            var getData = GetData();
            string formFileName = "";

            switch (welfareType)
            {
                case 0: formFileName = "p43KumamotoSeikyu41.rse"; break;
                case 1: formFileName = "p43KumamotoSeikyu42.rse"; break;
                case 2: formFileName = "p43KumamotoSeikyu43.rse"; break;
            }

            if (formFileName == "") return new();
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
            return new WelfareSeikyuMapper(_setFieldData, _listTextData, _extralData, formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }

        #region Private function
        private bool UpdateDrawForm()
        {
            const int maxRow = 10;
            bool _hasNextPage = true;

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
                //診療年月
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
                int totalPage = (int)Math.Ceiling((double)receInfs.Count() / maxRow);
                if (totalPage >= 2)
                {
                    //1枚に収まる場合は記載不要
                    SetFieldData("currentPage", currentPage.ToString());
                    SetFieldData("totalPage", totalPage.ToString());
                }
                //総合計（１枚目に記載）
                if (currentPage == 1)
                {
                    //請求総件数
                    SetFieldData("totalCount", receInfs.Count().ToString());
                    //合計点数
                    SetFieldData("totalTensu", receInfs.Sum(r => r.KohiReceTensu(kohiHoubetus)).ToString());
                }

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = receInfs.OrderBy(r => r.WelfareTokusyuNo).ThenBy(r => r.PtNum).ToList();
                int ptIndex = (currentPage - 1) * maxRow;

                countData subTotalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = curReceInfs[ptIndex];

                    subTotalData.RecordCount++;

                    //受給者証番号
                    listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, string.Format("{0}{1}", (curReceInf.WelfareTokusyuNo ?? "").PadLeft(2, '　').Substring(0, 2), curReceInf.WelfareJyukyusyaNo)));
                    //保険者番号
                    listDataPerPage.Add(new("hokensyaNo", 0, rowNo, curReceInf.HokensyaNo.PadLeft(8, '0')));
                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                    //入外区分
                    listDataPerPage.Add(new("gairai", 0, rowNo, "○"));
                    //総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.KohiReceTensu(kohiHoubetus).ToString()));
                    subTotalData.Tensu += curReceInf.KohiReceTensu(kohiHoubetus);
                    //以前診療分
                    if (curReceInf.SinYm != seikyuYm)
                    {
                        CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.SinYm * 100 + 1);
                        listDataPerPage.Add(new("sinymYear", 0, rowNo, wrkYmd.Year.ToString()));
                        listDataPerPage.Add(new("sinymMonth", 0, rowNo, wrkYmd.Month.ToString()));
                    }

                    ptIndex++;
                    if (ptIndex >= curReceInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //小計
                listDataPerPage.Add(new("tensu", 0, 10, subTotalData.Tensu.ToString()));
                _listTextData.Add(pageIndex, listDataPerPage);

                return 1;
            }
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, 0);
            //熊本県用のモデルクラスにコピー
            receInfs = wrkReces.Select(x => new CoP43WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHoubetus)).ToList();
            //
            int dateFrom = CIUtil.DateTimeToInt(CIUtil.IntToDate(seikyuYm * 100 + 1).AddMonths(-11)) / 100;
            if (welfareType != 2)
            {
                receInfs = receInfs.Where(x => !(string.IsNullOrEmpty(x.WelfareTokusyuNo)) && x.SinYm >= dateFrom).ToList();
            }
            else
            {
                receInfs = receInfs.Where(x => x.WelfareTokusyuNo?.Length != 8 && x.SinYm >= dateFrom).ToList();
            }

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
