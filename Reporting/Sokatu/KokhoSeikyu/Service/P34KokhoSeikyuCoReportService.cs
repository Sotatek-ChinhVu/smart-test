using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P34KokhoSeikyuCoReportService : IP34KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 34;

        private List<string> fixedHoubetuP2 = new List<string> { "19", "41", "90", "91", "92", "93" };
        private List<string> fixedHoubetuP3 = new List<string> { "10", "11" };
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

        private const string _formFileNameP1 = "p34KokhoSeikyuP1.rse";
        private const string _formFileNameP2 = "p34KokhoSeikyuP2.rse";
        private const string _formFileNameP3 = "p34KokhoSeikyuP3.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        #endregion

        #region Constructor and Init
        public P34KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
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
        private bool hasNextPage;
        private int currentPage;
        #endregion

        #region Private function
        private bool UpdateDrawForm()
        {
            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                Dictionary<string, string> fieldDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                //医療機関コード
                SetFieldData("hpCode", hpInf.ReceHpCd);

                if (currentPage >= 2) return 1;

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
                SetFieldData("reportGengo", wrkYmd.Gengo.ToString());
                SetFieldData("reportYear", wrkYmd.Year.ToString());
                SetFieldData("reportMonth", wrkYmd.Month.ToString());
                SetFieldData("reportDay", wrkYmd.Day.ToString());
                //保険者
                fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo.ToString());
                _setFieldData.Add(pageIndex, fieldDataPerPage);
                SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == currentHokensyaNo)?.Name ?? "");
                //印
                SetVisibleFieldData("inkan", seikyuYm < KaiseiDate.m202210);

                return 1;
            }
            #endregion

            #region BodyP1
            int UpdateFormBodyP1()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                const int maxRow = 9;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        //国保
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                        case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        //退職
                        case 4: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 5: wrkReces = curReceInfs.Where(r => r.IsRetElderIppan).ToList(); break;
                        case 6: wrkReces = curReceInfs.Where(r => r.IsRetElderUpper).ToList(); break;
                        case 7: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 8: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
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
                _listTextData.Add(pageIndex, listDataPerPage);

                return 1;
            }
            #endregion

            #region BodyP2
            int UpdateFormBodyP2()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo).ToList();

                //公費負担医療（固定枠）
                for (short rowNo = 0; rowNo < fixedHoubetuP2.Count; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetuP2[rowNo])).ToList();

                    countData wrkData = new countData();

                    wrkData.Count = wrkReces.Count;
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(fixedHoubetuP2[rowNo]));
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(fixedHoubetuP2[rowNo]));
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(fixedHoubetuP2[rowNo]));

                    listDataPerPage.Add(new("fixedCount", 0, rowNo, wrkData.Count.ToString()));
                    if (fixedHoubetuP2[rowNo] == "19")
                    {
                        listDataPerPage.Add(new("fixedNissu", 0, rowNo, wrkData.Nissu.ToString()));
                        listDataPerPage.Add(new("fixedTensu", 0, rowNo, wrkData.Tensu.ToString()));
                    }
                    listDataPerPage.Add(new("fixedFutan", 0, rowNo, wrkData.Futan.ToString()));
                }

                #region 公費負担医療（フリー枠）
                const int maxKohiRow = 3;
                //固定枠の法別番号リスト
                List<string> excludeHoubetu = fixedHoubetuP2.Union(fixedHoubetuP3).ToList();

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), excludeHoubetu);
                if (kohiHoubetus.Count == 0)
                {
                    _listTextData.Add(pageIndex, listDataPerPage);
                    _hasNextPage = false;
                    return 1;
                }

                //集計
                for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                {
                    if (rowNo >= kohiHoubetus.Count)
                    {
                        _listTextData.Add(pageIndex, listDataPerPage);
                        _hasNextPage = false;
                        return 1;
                    }

                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                    //法別番号
                    listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[rowNo]));

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[rowNo]));
                    listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo]));
                    listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo]));
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));
                }

                //続紙に記載するデータの存在確認
                int fixedCountP3 = 0;
                for (short i = 0; i < fixedHoubetuP3.Count; i++)
                {
                    fixedCountP3 += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetuP3[i])).ToList().Count;
                }

                if (maxKohiRow >= kohiHoubetus.Count && fixedCountP3 == 0)
                {
                    _hasNextPage = false;
                }
                #endregion
                _listTextData.Add(pageIndex, listDataPerPage);

                return 1;
            }
            #endregion

            #region BodyP3
            int UpdateFormBodyP3()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo).ToList();

                //公費負担医療（固定枠）
                for (short rowNo = 0; rowNo < fixedHoubetuP3.Count; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(fixedHoubetuP3[rowNo])).ToList();

                    countData wrkData = new countData();

                    wrkData.Count = wrkReces.Count;
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(fixedHoubetuP3[rowNo]));
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(fixedHoubetuP3[rowNo]));
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(fixedHoubetuP3[rowNo]));

                    listDataPerPage.Add(new("fixedCount", 0, rowNo, wrkData.Count.ToString()));
                    listDataPerPage.Add(new("fixedNissu", 0, rowNo, wrkData.Nissu.ToString()));
                    listDataPerPage.Add(new("fixedTensu", 0, rowNo, wrkData.Tensu.ToString()));
                    if (fixedHoubetuP3[rowNo] == "11")
                    {
                        listDataPerPage.Add(new("fixedFutan", 0, rowNo, wrkData.Futan.ToString()));
                    }
                }

                #region 公費負担医療（フリー枠）
                const int maxKohiRow = 6;
                const int p2FreeCount = 3;
                //固定枠の法別番号リスト
                List<string> excludeHoubetu = fixedHoubetuP2.Union(fixedHoubetuP3).ToList();

                int kohiIndex = (currentPage - 3) * maxKohiRow + p2FreeCount;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), excludeHoubetu);
                if (kohiHoubetus.Count == 0 || kohiHoubetus.Count <= kohiIndex)
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
                #endregion
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
                case 2:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP2() < 0)
                    {
                        hasNextPage = _hasNextPage;
                        return false;
                    }
                    break;
                default:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP3() < 0)
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
        #endregion

        public CommonReportingRequestModel GetP34KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
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
                    currentHokensyaNo = currentNo;
                    currentPage = 1;
                    hasNextPage = true;

                    while (getData && hasNextPage)
                    {
                        UpdateDrawForm();
                        if (currentPage == 2)
                        {
                            fileName.Add(indexPage.ToString(), _formFileNameP2);
                        }
                        else if (currentPage == 3)
                        {
                            fileName.Add(indexPage.ToString(), _formFileNameP3);
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

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void SetVisibleFieldData(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
            {
                _visibleFieldData.Add(field, value);
            }
        }
    }
}
