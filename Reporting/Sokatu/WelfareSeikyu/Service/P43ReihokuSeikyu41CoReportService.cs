using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;
using static Reporting.Sokatu.WelfareSeikyu.Models.CoP43WelfareReceInfModel2;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P43ReihokuSeikyu41CoReportService : IP43ReihokuSeikyu41CoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "41" };
        private List<CityKohi> kohiHokens = new List<CityKohi> {
            new CityKohi() { HokenNo = 141, HokenEdaNo = 6 }
        };

        private struct countData
        {
            public int KohiNissu;
            public int KohiTensu;
            public int IchibuFutan;
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
        private List<CoP43WelfareReceInfModel2> receInfs;
        private CoHpInfModel hpInf;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p43ReihokuSeikyu41.rse";
        private readonly IReadRseReportFileService _readRseReportFileService;

        #region Constructor and Init
        public P43ReihokuSeikyu41CoReportService(ICoWelfareSeikyuFinder welfareFinder, IReadRseReportFileService readRseReportFileService)
        {
            _welfareFinder = welfareFinder;
            _readRseReportFileService = readRseReportFileService;
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
        public int maxRow;
        #endregion

        public CommonReportingRequestModel GetP43ReihokuSeikyu41ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();
                currentPage = 1;
                hasNextPage = true;
                GetRowCount("p43ReihokuSeikyu41.rse");

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
                SetFieldData("hpTel", hpInf.Tel);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo1", wrkYmd.Gengo);
                SetFieldData("seikyuYear1", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth1", wrkYmd.Month.ToString());
                SetFieldData("seikyuGengo2", wrkYmd.Gengo);
                SetFieldData("seikyuYear2", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth2", wrkYmd.Month.ToString());
                //提出年月日
                wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                SetFieldData("reportGengo", wrkYmd.Gengo);
                SetFieldData("reportYear", wrkYmd.Year.ToString());
                SetFieldData("reportMonth", wrkYmd.Month.ToString());
                SetFieldData("reportDay", wrkYmd.Day.ToString());
                //ページ数
                SetFieldData("page", currentPage.ToString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                int ptIndex = (currentPage - 1) * maxRow;

                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    if(receInfs.Count <= ptIndex)
                    {
                        hasNextPage = false;
                        break;
                    }

                    var curReceInf = receInfs[ptIndex];

                    //受給者番号
                    listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, curReceInf.JyukyusyaNo));

                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));

                    //実日数
                    listDataPerPage.Add(new("nissu", 0, rowNo, curReceInf.KohiNissu.ToString()));
                    totalData.KohiNissu += curReceInf.KohiNissu;
                    //総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.KohiTensu.ToString()));
                    totalData.KohiTensu += curReceInf.KohiTensu;
                    //公費負担額
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, "0"));
                    //一部負担額
                    listDataPerPage.Add(new("futan", 0, rowNo, curReceInf.IchibuFutan.ToString()));
                    totalData.IchibuFutan += curReceInf.IchibuFutan;

                    ptIndex++;
                    if (ptIndex >= receInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //小計
                short wrkRow = (short)maxRow;
                listDataPerPage.Add(new("nissu", 0, wrkRow, totalData.KohiNissu.ToString()));
                listDataPerPage.Add(new("tensu", 0, wrkRow, totalData.KohiTensu.ToString()));
                listDataPerPage.Add(new("kohiFutan", 0, wrkRow, "0"));
                listDataPerPage.Add(new("futan", 0, wrkRow, totalData.IchibuFutan.ToString()));

                if (!_hasNextPage)
                {
                    //合計
                    wrkRow = (short)(maxRow + 1);
                    if (receInfs.Sum(r => r.KohiNissu) == 0)
                    {
                        listDataPerPage.Add(new("nissu", 0, wrkRow, ""));
                    }
                    else
                    {
                        listDataPerPage.Add(new("nissu", 0, wrkRow, receInfs.Sum(r => r.KohiNissu).ToString()));
                    }

                    if (receInfs.Sum(r => r.KohiTensu) == 0)
                    {
                        listDataPerPage.Add(new("tensu", 0, wrkRow, ""));
                    }
                    else
                    {
                        listDataPerPage.Add(new("tensu", 0, wrkRow, receInfs.Sum(r => r.KohiTensu).ToString()));
                    }

                    if (receInfs.Sum(r => r.IchibuFutan) == 0)
                    {
                        listDataPerPage.Add(new("futan", 0, wrkRow, ""));
                    }
                    else
                    {
                        listDataPerPage.Add(new("futan", 0, wrkRow, receInfs.Sum(r => r.IchibuFutan).ToString()));
                    }

                    listDataPerPage.Add(new("kohiFutan", 0, wrkRow, "0"));
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, -1);
            //熊本県用のモデルにコピー
            receInfs = wrkReces.Select(x => new CoP43WelfareReceInfModel2(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokens)).ToList();
            //苓北町 子育て支援医療費の対象に絞る
            receInfs = receInfs.Where(x => x.IsWelfare).OrderBy(x => x.JyukyusyaNo.PadLeft(7, '0')).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void GetRowCount(string formFileName)
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("jyukyusyaNo", (int)CalculateTypeEnum.GetListRowCount));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.WelfareSeikyu, formFileName, fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == "jyukyusyaNo" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
        }
        #endregion
    }
}
