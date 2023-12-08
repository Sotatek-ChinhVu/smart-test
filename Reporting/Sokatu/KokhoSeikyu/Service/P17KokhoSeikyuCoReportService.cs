using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P17KokhoSeikyuCoReportService : IP17KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 17;

        private List<string> fixedHoubetuP1 = new List<string> { "10", "20", "21", "85" };
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
        private CoHpInfModel hpInf;
        private List<CoKaMstModel> kaMsts;
        #endregion

        private const string _formFileNameP1 = "p17KokhoSeikyuP1.rse";
        private const string _formFileNameP2 = "p17KokhoSeikyuP2.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;

        #region Constructor and Init
        public P17KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _setFieldData = new();
            _singleFieldData = new();
            _extralData = new();
            _visibleFieldData = new();
            _listTextData = new();
            hpInf = new();
            hokensyaNos = new();
            receInfs = new();
            currentHokensyaNo = "";
            printHokensyaNos = new();
            hokensyaNames = new();
            kaMsts = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private List<string> printHokensyaNos;
        private bool hasNextPage;
        private int currentPage;
        #endregion

        public CommonReportingRequestModel GetP17KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
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
                            if (currentPage >= 2)
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

                var pageIndex = _setFieldData.Select(item => item.Key).Distinct().Count();
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
            bool _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                var pageIndex = _setFieldData.Select(item => item.Key).Distinct().Count() + 1;
                Dictionary<string, string> fieldDataPerPage = new();
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());

                if (currentPage >= 2)
                {
                    _setFieldData.Add(pageIndex, fieldDataPerPage);
                    return 1;
                }

                //医療機関情報
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                //提出年月日
                wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                SetFieldData("reportGengo", wrkYmd.Gengo);
                SetFieldData("reportYear", wrkYmd.Year.ToString());
                SetFieldData("reportMonth", wrkYmd.Month.ToString());
                SetFieldData("reportDay", wrkYmd.Day.ToString());
                //保険者
                fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo);
                _setFieldData.Add(pageIndex, fieldDataPerPage);
                //診療科
                SetFieldData("kaName", kaMsts[0]?.KaName ?? string.Empty);
                return 1;
            }
            #endregion

            #region BodyP1
            int UpdateFormBodyP1()
            {
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                List<ListTextObject> listDataPerPage = new();

                if (pageIndex % 2 != 0)
                {
                    var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                    const int maxRow = 11;
                    List<CoReceInfModel> wrkReces;

                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        wrkReces = new();
                        switch (rowNo)
                        {
                            //国保
                            case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                            case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                            case 2: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && (r.HokenRate == 10)).ToList(); break;
                            case 3: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && (r.HokenRate == 20)).ToList(); break;
                            case 4: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && (r.HokenRate == 30)).ToList(); break;
                            case 5: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                            //退職
                            case 6: wrkReces = curReceInfs.Where(r => r.IsRetElderIppan).ToList(); break;
                            case 7: wrkReces = curReceInfs.Where(r => r.IsRetElderUpper).ToList(); break;
                            case 8: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                            case 9: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                            case 10: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                        }
                        if (wrkReces == null) continue;

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

                    //公費負担医療
                    for (short rowNo = 0; rowNo < fixedHoubetuP1.Count + 1; rowNo++)
                    {
                        countData wrkData = new countData(); ;
                        if (rowNo < fixedHoubetuP1.Count)
                        {
                            //固定枠
                            wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetuP1[rowNo])).ToList();
                            wrkData.Count = wrkReces.Count;
                            wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(fixedHoubetuP1[rowNo]));
                            wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(fixedHoubetuP1[rowNo]));
                            wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(fixedHoubetuP1[rowNo]));
                        }
                        else
                        {
                            //その他
                            var prefAllHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsHeiyo).ToList(), fixedHoubetuP1);
                            foreach (var prefAllHoubetu in prefAllHoubetus)
                            {
                                wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(prefAllHoubetu)).ToList();
                                wrkData.Count += wrkReces.Count;
                                wrkData.Nissu += wrkReces.Sum(r => r.KohiReceNissu(prefAllHoubetu));
                                wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(prefAllHoubetu));
                                wrkData.Futan += wrkReces.Sum(r => r.KohiReceFutan(prefAllHoubetu));
                            }
                        }

                        listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                        listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                        listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                        listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));
                    }
                    _hasNextPage = !curReceInfs.FirstOrDefault()?.IsPrefIn ?? false;
                }

                _listTextData.Add(pageIndex, listDataPerPage);
                //県外保険者は2ページ目も印刷する

                return 1;
            }
            #endregion

            #endregion

            if (currentPage == 1)
            {
                if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
            }
            else
            {
                if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
            kaMsts = _kokhoFinder.GetKaMst(hpId);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.NoSum);
            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
            //保険者番号リストを取得（県内→県外）
            hokensyaNos = wrkReceInfs.Where(r => r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
            hokensyaNos.AddRange(
                wrkReceInfs.Where(r => !r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList()
            );
            //保険者名を取得
            hokensyaNames = _kokhoFinder.GetHokensyaName(hpId, hokensyaNos);

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
