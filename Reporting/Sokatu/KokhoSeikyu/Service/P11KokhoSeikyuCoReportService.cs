using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P11KokhoSeikyuCoReportService : IP11KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 11;
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoKokhoSeikyuFinder _kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private string currentHokensyaNo;
        private List<string> hokensyaNos;
        private List<CoHokensyaMstModel> hokensyaNames;
        private List<CoReceInfModel> receInfs;
        private List<CoReceInfModel> tokuyohiReceInfs;
        private CoHpInfModel hpInf;
        #endregion

        /// <summary>
        /// OutPut Data
        /// </summary>
        private const string _formFileNameP1 = "p11KokhoSeikyuP1.rse";
        private const string _formFileNameP2 = "p11KokhoSeikyuP2.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;

        #region Constructor and Init
        public P11KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private List<string> printHokensyaNos;
        private int currentPage;
        private bool hasNextPage;

        #endregion

        public CommonReportingRequestModel GetP11KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();
                int indexPage = 1;
                var fileName = new Dictionary<string, string>();

                if (getData)
                {
                    foreach (string currentNo in hokensyaNos)
                    {
                        currentPage = 1;
                        currentHokensyaNo = currentNo;
                        hasNextPage = true;
                        while (getData && hasNextPage)
                        {
                            UpdateDrawForm();
                            if (currentPage == 2)
                            {
                                fileName.Add(indexPage.ToString(), _formFileNameP2);
                            }
                            else
                            {
                                fileName.Add(indexPage.ToString(), _formFileNameP1);
                            }
                            currentPage++;
                            indexPage++;
                        }
                    }
                }

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
                _extralData.Add("totalPage", pageIndex.ToString());
                return new P08KokhoSeikyuMapper(_setFieldData, _listTextData, _extralData, fileName, _singleFieldData, _visibleFieldData).GetData();
            }
            finally
            {
                _kokhoFinder.ReleaseResource();
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
                //保険者番号
                Dictionary<string, string> fielDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
                fielDataPerPage.Add("hokensyaNo", currentHokensyaNo);
                _setFieldData.Add(pageIndex, fielDataPerPage);

                if (currentPage >= 2) return 1;

                //保険者名
                SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == currentHokensyaNo)?.Name ?? "");

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

            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region BodyP1
            int UpdateFormBodyP1()
            {
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                const int maxRow = 8;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        //国保
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && r.HokenRate != 30).ToList(); break;
                        case 3: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && r.HokenRate == 30).ToList(); break;
                        case 4: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        //退職
                        case 5: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 6: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 7: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    if (new int[] { 2 }.Contains(rowNo) && wrkReces.Count >= 1)
                    {
                        int wrkRate = (100 - wrkReces[0].HokenRate) / 10;
                        SetFieldData("kyufu", wrkRate.ToString());
                    }

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                    listDataPerPage.Add(new("nissu", 0, rowNo, wrkData.Nissu.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                    listDataPerPage.Add(new("futan", 0, rowNo, wrkData.Futan.ToString()));
                }
                _listTextData.Add(pageIndex, listDataPerPage);
                return 1;
            }
            #endregion

            #region BodyP2
            int UpdateFormBodyP2()
            {
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                //特別療養費
                if (currentPage == 2)
                {
                    var wrkReces = tokuyohiReceInfs.Where(r => r.HokensyaNo == currentHokensyaNo).ToList();

                    if (wrkReces.Count >= 1)
                    {
                        SetFieldData("tokuyohiCount", wrkReces.Count.ToString());
                        SetFieldData("tokuyohiNissu", wrkReces.Sum(r => r.HokenNissu).ToString());
                        SetFieldData("tokuyohiTensu", wrkReces.Sum(r => r.Tensu).ToString());
                    }
                }

                const int maxKohiRow = 3;
                int kohiIndex = (currentPage - 2) * maxKohiRow;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count == 0)
                {
                    _listTextData.Add(pageIndex, listDataPerPage);
                    _hasNextPage = false;
                    return 1;
                }

                //集計
                for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                    //法別番号
                    listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]));

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                    listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                    listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiIndex]));
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));

                    kohiIndex++;
                    if (kohiIndex >= kohiHoubetus.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }
                _listTextData.Add(pageIndex, listDataPerPage);

                return 1;
            }
            #endregion

            #endregion

            switch (currentPage)
            {
                case 1:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                    {
                        hasNextPage = _hasNextPage;
                        return false;
                    }
                    break;
                default:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP2() < 0)
                    {
                        hasNextPage = _hasNextPage;
                        return false;
                    }
                    break;
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);

            //特別療養費
            tokuyohiReceInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.TokuyohiKokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);

            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
            wrkReceInfs.AddRange(tokuyohiReceInfs);

            //保険者番号リストを取得
            hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
            //保険者名を取得
            hokensyaNames = _kokhoFinder.GetHokensyaName(hpId, hokensyaNos);

            return (receInfs?.Count ?? 0) > 0 || (tokuyohiReceInfs?.Count ?? 0) > 0;
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
