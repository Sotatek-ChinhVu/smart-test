using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P45KokhoSeikyuCoReportService : IP45KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 45;
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
        private const string _formFileNameP1 = "p45KokhoSeikyuP1.rse";
        private const string _formFileNameP2 = "p45KokhoSeikyuP2.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        #endregion

        #region Constructor and Init
        public P45KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
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
        private int kohiTotalCnt;
        private int kohiIndex;
        private int kohiHoubetusIndex;
        private int hokenRate;
        private int wrkRateCnt;
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
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                //保険者
                Dictionary<string, string> fieldDataPerPage = new();
                fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo);
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _setFieldData.Add(pageIndex, fieldDataPerPage);
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
                //県内・県外
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);
                List<CoReceInfModel> wrkReces = null;
                wrkReces = curReceInfs.Where(r => r.IsPrefIn).ToList();
                if (wrkReces.Count == 0)
                {
                    SetFieldData("prefInOut", "県外分");
                }
                else
                {
                    SetFieldData("prefInOut", "県内分");
                }

                if (currentPage >= 2) return 1;

                //保険者名
                SetFieldData("hokensyaName", hokensyaNames.Find(h => h.HokensyaNo == currentHokensyaNo)?.Name ?? "");
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

            #region BodyP1
            int UpdateFormBodyP1()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                #region Body
                const int maxRow = 7;

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
                        case 5: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 6: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
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
                #endregion

                #region 公費負担医療
                const int maxKohiRow = 2;
                kohiIndex = 0;
                kohiHoubetusIndex = 0;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count == 0)
                {
                    _listTextData.Add(pageIndex, listDataPerPage);
                    _hasNextPage = false;
                    return 1;
                }

                //公費法別・割合毎に集計した件数
                kohiTotalCnt = 0;
                for (short i = 0; i < kohiHoubetus.Count; i++)
                {
                    kohiTotalCnt += curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[i])).GroupBy(r => new { r.HokenRate }).Count();
                }

                //集計
                wrkRateCnt = 0;
                short rowNo2 = 0;
                while (rowNo2 < maxKohiRow)
                {
                    for (int rateCnt = 0; rateCnt <= 3; rateCnt++)
                    {
                        hokenRate = 30;
                        switch (rateCnt)
                        {
                            case 1: hokenRate = 20; break;
                            case 2: hokenRate = 10; break;
                            case 3: hokenRate = 0; break;
                        }
                        wrkRateCnt = rateCnt;
                        if (rowNo2 == maxKohiRow) break;

                        //公費法別・割合毎に集計
                        var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiHoubetusIndex]) && r.HokenRate == hokenRate).ToList();
                        if (wrkReces.Count() == 0) continue;

                        //法別番号
                        listDataPerPage.Add(new("kohiHoubetu", 0, rowNo2, kohiHoubetus[kohiHoubetusIndex]));
                        //割合
                        listDataPerPage.Add(new("kohiRate", 0, rowNo2, (wrkReces[0].HokenRate / 10).ToString()));

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new("kohiCount", 0, rowNo2, wrkData.Count.ToString()));
                        //日数
                        wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiHoubetusIndex]));
                        listDataPerPage.Add(new("kohiNissu", 0, rowNo2, wrkData.Nissu.ToString()));
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiHoubetusIndex]));
                        listDataPerPage.Add(new("kohiTensu", 0, rowNo2, wrkData.Tensu.ToString()));
                        //一部負担金
                        wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiHoubetusIndex]));
                        listDataPerPage.Add(new("kohiFutan", 0, rowNo2, wrkData.Futan.ToString()));

                        rowNo2++;
                        kohiIndex++;
                        if (kohiIndex >= kohiTotalCnt)
                        {
                            _hasNextPage = false;
                            break;
                        }
                    }

                    if (hokenRate == 0) kohiHoubetusIndex++;

                    if (kohiIndex >= kohiTotalCnt)
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

            #region BodyP2
            int UpdateFormBodyP2()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                const int maxKohiRow = 7;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                if (kohiIndex >= kohiTotalCnt)
                {
                    _listTextData.Add(pageIndex, listDataPerPage);
                    _hasNextPage = false;
                    return 1;
                }

                //集計
                int startRateCnt = 0;
                startRateCnt = wrkRateCnt;
                short rowNo = 0;
                while (rowNo < maxKohiRow)
                {
                    for (int rateCnt = startRateCnt; rateCnt <= 3; rateCnt++)
                    {
                        startRateCnt = 0;
                        hokenRate = 30;
                        switch (rateCnt)
                        {
                            case 1: hokenRate = 20; break;
                            case 2: hokenRate = 10; break;
                            case 3: hokenRate = 0; break;
                        }
                        wrkRateCnt = rateCnt;
                        if (rowNo == maxKohiRow) break;

                        //公費法別・割合毎に集計
                        var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiHoubetusIndex]) && r.HokenRate == hokenRate).ToList();
                        if (wrkReces.Count() == 0) continue;

                        //法別番号
                        listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiHoubetusIndex]));
                        //割合
                        listDataPerPage.Add(new("kohiRate", 0, rowNo, (wrkReces[0].HokenRate / 10).ToString()));

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                        //日数
                        wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiHoubetusIndex]));
                        listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiHoubetusIndex]));
                        listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                        //一部負担金
                        wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiHoubetusIndex]));
                        listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));

                        rowNo++;
                        kohiIndex++;
                        if (kohiIndex >= kohiTotalCnt)
                        {
                            _hasNextPage = false;
                            break;
                        }
                    }

                    if (hokenRate == 0) kohiHoubetusIndex++;

                    if (kohiIndex >= kohiTotalCnt)
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
        public CommonReportingRequestModel GetP45KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
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
    }
}
