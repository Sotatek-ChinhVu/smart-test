using Helper.Common;
using Helper.Constants;
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
    public class P43AmakusaSeikyu42CoReportService : IP43AmakusaSeikyu42CoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "42" };
        private List<CityKohi> KohiHokens = new List<CityKohi> {
            new CityKohi() { HokenNo = 142, HokenEdaNo = 4 }
        };

        private struct countData
        {
            public int KohiNissu;
            public int KohiTensu;
            public int IchibuFutan;
            public int KohiPtFutan;
            public int KohiFutan;
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
        private string _formFileName = "p43AmakusaSeikyu42.rse";
        private readonly IReadRseReportFileService _readRseReportFileService;

        #region Constructor and Init
        public P43AmakusaSeikyu42CoReportService(ICoWelfareSeikyuFinder welfareFinder, IReadRseReportFileService readRseReportFileService)
        {
            _welfareFinder = welfareFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
            _readRseReportFileService = readRseReportFileService;
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private int currentPage;
        private bool hasNextPage;
        private int maxRow;
        #endregion

        public CommonReportingRequestModel GetP43AmakusaSeikyu42ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();
            currentPage = 1;
            hasNextPage = true;
            GetRowCount("p43AmakusaSeikyu42.rse");

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
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
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
                int ptIndex = (currentPage - 1) * maxRow;

                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = receInfs[ptIndex];

                    //受給者番号
                    listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, curReceInf.JyukyusyaNo));
                    switch (curReceInf.HokenKbn)
                    {
                        case HokenKbn.Syaho:
                            listDataPerPage.Add(new("hokenKbn", 1, rowNo, "〇"));
                            break;
                        case HokenKbn.Kokho:
                            listDataPerPage.Add(new("hokenKbn", 0, rowNo, "〇"));
                            break;
                    }

                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));

                    //実日数
                    listDataPerPage.Add(new("nissu", 0, rowNo, curReceInf.KohiNissu.ToString()));
                    totalData.KohiNissu += curReceInf.KohiNissu;
                    //総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.KohiTensu.ToString()));
                    totalData.KohiTensu += curReceInf.KohiTensu;
                    //一部負担額
                    listDataPerPage.Add(new("futan", 0, rowNo, curReceInf.IchibuFutan.ToString()));
                    totalData.IchibuFutan += curReceInf.IchibuFutan;
                    //自己負担額
                    listDataPerPage.Add(new("ptFutan", 0, rowNo, curReceInf.KohiPtFutan.ToString()));
                    totalData.KohiPtFutan += curReceInf.KohiPtFutan;
                    //重度医療助成額
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, (curReceInf.IchibuFutan - curReceInf.KohiPtFutan).ToString()));
                    totalData.KohiFutan += curReceInf.IchibuFutan - curReceInf.KohiPtFutan;

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
                listDataPerPage.Add(new("futan", 0, wrkRow, totalData.IchibuFutan.ToString()));
                listDataPerPage.Add(new("ptFutan", 0, wrkRow, totalData.KohiPtFutan.ToString()));
                listDataPerPage.Add(new("kohiFutan", 0, wrkRow, totalData.KohiFutan.ToString()));

                if (!_hasNextPage)
                {
                    //合計
                    wrkRow = (short)(maxRow + 1);
                    listDataPerPage.Add(new("nissu", 0, wrkRow, (receInfs.Sum(r => r.KohiNissu)).ToString()));
                    listDataPerPage.Add(new("tensu", 0, wrkRow, (receInfs.Sum(r => r.KohiTensu)).ToString()));
                    listDataPerPage.Add(new("futan", 0, wrkRow, (receInfs.Sum(r => r.IchibuFutan)).ToString()));
                    listDataPerPage.Add(new("ptFutan", 0, wrkRow, (receInfs.Sum(r => r.KohiPtFutan)).ToString()));
                    listDataPerPage.Add(new("kohiFutan", 0, wrkRow, (receInfs.Sum(r => r.IchibuFutan - r.KohiPtFutan)).ToString()));
                }
                _listTextData.Add(pageIndex, listDataPerPage);
                return 1;
            }
            #endregion

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
            }
            catch (Exception e)
            {
                hasNextPage = _hasNextPage = false;
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
            receInfs = wrkReces.Select(x => new CoP43WelfareReceInfModel2(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, KohiHokens)).ToList();
            //天草市重度心身障害者の対象に絞る
            receInfs = receInfs.Where(x => x.IsWelfare).OrderBy(x => x.JyukyusyaNo.PadLeft(7, '0')).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }

        private void GetRowCount(string formFileName)
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("jyukyusyaNo", (int)CalculateTypeEnum.GetListRowCount));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.WelfareSeikyu, formFileName, fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == "jyukyusyaNo" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
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
