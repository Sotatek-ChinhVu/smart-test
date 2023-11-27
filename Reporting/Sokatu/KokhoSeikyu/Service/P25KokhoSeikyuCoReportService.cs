using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P25KokhoSeikyuCoReportService : IP25KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 25;
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoKokhoSeikyuFinder _kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private bool printZaiiso;
        private string currentHokensyaNo;
        private List<string> hokensyaNos;
        private List<CoHokensyaMstModel> hokensyaNames;
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        #endregion

        /// <summary>
        /// OutPut Data
        /// </summary>
        private const string _formFileName = "p25KokhoSeikyu.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;

        #region Constructor and Init
        public P25KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
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
        private List<CoReceInfModel> curReceInfs;
        private int hokenRate;
        private bool hasNextPage;
        private int currentPage;
        #endregion

        public CommonReportingRequestModel GetP25KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();


                for (int zaiFlg = 0; zaiFlg <= 1; zaiFlg++)
                {
                    printZaiiso = zaiFlg == 1;

                    //保険者単位で出力
                    foreach (string currentNo in hokensyaNos)
                    {
                        currentHokensyaNo = currentNo;

                        //国保一般被保険者分については、給付割合毎に作成する
                        for (int rateCnt = 0; rateCnt <= 3; rateCnt++)
                        {
                            curReceInfs = receInfs.Where(r => r.IsZaiiso == zaiFlg && r.HokensyaNo == currentHokensyaNo).ToList();

                            hokenRate = 30;
                            switch (rateCnt)
                            {
                                case 1: hokenRate = 20; break;
                                case 2: hokenRate = 10; break;
                                case 3: hokenRate = 0; break;
                            }

                            switch (rateCnt)
                            {
                                case 0:
                                    curReceInfs = curReceInfs.Where(r => ((r.IsNrMine || r.IsNrFamily) && r.HokenRate == hokenRate) || !r.IsNrMine || !r.IsNrFamily).ToList();
                                    break;
                                default:
                                    //法定外給付
                                    curReceInfs = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && r.HokenRate == hokenRate).ToList();
                                    break;
                            }
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
                }

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
                _extralData.Add("totalPage", pageIndex.ToString());
                return new KokhoSokatuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
            }
            finally
            {
                _kokhoFinder.ReleaseResource();
            }
        }

        #region Private function
        private bool UpdateDrawForm()
        {
            Dictionary<string, string> fieldDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            List<ListTextObject> listDataPerPage = new();

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
                //保険者
                fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo);
                _setFieldData.Add(pageIndex, fieldDataPerPage);
                //給付割合
                SetFieldData(string.Format("kyufuRate{0}", (100 - hokenRate) / 10), "〇");
                //在医総及び在医総管
                if (printZaiiso)
                {
                    SetFieldData("zaiisoCircle", "〇");
                }

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                int maxRow = seikyuYm >= KaiseiDate.m202210 ? 7 : 9;

                //公費負担医療（福祉）
                var prefInHoubetus = SokatuUtil.GetPrefInHoubetu(curReceInfs, myPrefNo);

                if (currentPage == 1)
                {
                    //1枚目のみ記載する
                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
                        switch (rowNo)
                        {
                            //一般
                            case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                            case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                            case 2: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                            case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                            //退職
                            case 4: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                            case 5:
                                wrkReces =
                                        seikyuYm >= KaiseiDate.m202210 ?
                                            curReceInfs.Where(r => r.IsRetFamily).ToList() :
                                            curReceInfs.Where(r => r.IsRetElderIppan).ToList();
                                break;
                            case 6:
                                wrkReces =
                                        seikyuYm >= KaiseiDate.m202210 ?
                                            curReceInfs.Where(r => r.IsRetPreSchool).ToList() :
                                            curReceInfs.Where(r => r.IsRetElderUpper).ToList();
                                break;
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

                    //「長」欄
                    int chokiCnt = curReceInfs.Where(r => r.IsChoki).Count();
                    SetFieldData("chokiCnt", chokiCnt.ToString());

                    //公費負担医療（福祉）
                    countData welfareData = new countData();
                    foreach (var houbetu in prefInHoubetus)
                    {
                        var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(houbetu)).ToList();

                        countData wrkData = new countData();

                        wrkData.Count = wrkReces.Count;
                        wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(houbetu));
                        wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                        wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(houbetu));
                        //合計
                        welfareData.AddValue(wrkData);
                    }
                    SetFieldData("welfareCount", welfareData.Count.ToString());
                    SetFieldData("welfareNissu", welfareData.Nissu.ToString());
                    SetFieldData("welfareTensu", welfareData.Tensu.ToString());
                    SetFieldData("welfareFutan", welfareData.Futan.ToString());
                }

                #region 公費負担医療
                int maxKohiRow = seikyuYm >= KaiseiDate.m202210 ? 5 : 2;
                int kohiIndex = (currentPage - 1) * maxKohiRow;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), prefInHoubetus);
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
                #endregion
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
            hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.NoSum);
            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
            //保険者番号リストを取得
            hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
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