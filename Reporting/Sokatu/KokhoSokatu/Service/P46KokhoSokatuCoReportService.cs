using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.Mapper;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P46KokhoSokatuCoReportService : IP46KokhoSokatuCoReportService
    {
        #region Constant
        private const int myPrefNo = 46;
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private ICoKokhoSokatuFinder _kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoReceInfModel> receInfs;
        private List<CoReceInfModel> curReceInfs;
        private CoHpInfModel hpInf;
        private List<CoKaMstModel> kaMsts;
        private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
        private string prefInOut;
        private const string _formFileNameP1 = "p46KokhoSokatuP1.rse";
        private const string _formFileNameP2 = "p46KokhoSokatuP2.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        #endregion

        #region Constructor and Init
        public P46KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            hpInf = new();
            receInfs = new();
            kaMsts = new();
            curReceInfs = new();
            kohiHoubetuMsts = new();
            prefInOut = "";
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
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

                if (currentPage >= 2) return 1;

                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
                //県内・県外を印字
                SetVisibleFieldData("prefIn", prefInOut == "In");
                SetVisibleFieldData("prefOut", prefInOut == "Out");
                //医療機関情報
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                SetFieldData("hpTel", hpInf.Tel);
                //診療科
                SetFieldData("kaName", kaMsts[0].KaName);

                return 1;
            }
            #endregion

            #region BodyP1
            int UpdateFormBodyP1()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                const int maxRow = 9;
                List<CoReceInfModel> wrkReces = new();

                #region CountPages
                int maxKohi = 0;
                int adjPage = 0;
                int totalSeikyu = 0;
                for (short seikyuKbn = 0; seikyuKbn < 3; seikyuKbn++)
                {
                    switch (seikyuKbn)
                    {
                        //後期
                        case 0:
                            wrkReces = curReceInfs.Where(r => r.IsKoukiAll).ToList();
                            maxKohi = 4;
                            adjPage = 1;  //1ページ目に公費印刷欄があるため調整
                            break;
                        //国保・退職
                        case 1:
                            wrkReces = curReceInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList();
                            maxKohi = 7;
                            adjPage = 0;
                            break;
                        //合計
                        case 2: wrkReces = curReceInfs.ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    var hokenshaList = wrkReces.GroupBy(r => r.HokensyaNo).Select(r => r.Key).ToList();

                    //請求保険者数
                    listDataPerPage.Add(new($"seikyuCount{seikyuKbn}", 0, 0, hokenshaList.Count().ToString()));
                    //請求書枚数
                    int wrkCount = 0;
                    if (seikyuKbn != 2)
                    {
                        foreach (string eachHokensyaNo in hokenshaList)
                        {
                            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(wrkReces.Where(r => r.HokensyaNo == eachHokensyaNo && r.IsHeiyo).ToList(), new());
                            //2ページ目以降の枚数計算
                            int addPage = Math.Max((int)Math.Ceiling((double)kohiHoubetus.Count() / maxKohi) - adjPage, 0);
                            wrkCount += 1 + addPage;
                        }
                        totalSeikyu += wrkCount;
                    }
                    listDataPerPage.Add(new($"seikyuCount{seikyuKbn}", 1, 0, (seikyuKbn == 2 ? totalSeikyu : wrkCount).ToString()));
                    //明細書総件数
                    listDataPerPage.Add(new($"seikyuCount{seikyuKbn}", 2, 0, wrkReces.Count.ToString()));
                }
                #endregion

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
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
                        //後期高齢
                        case 7: wrkReces = curReceInfs.Where(r => r.IsKoukiAll).ToList(); break;
                        //合計
                        case 8: wrkReces = curReceInfs.ToList(); break;
                    }
                    if (wrkReces == null) 
                    {
                        continue;
                    }

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
                    //一部負担金(国保と退職のみ)
                    if (rowNo >= 0 && rowNo <= 6)
                    {
                        wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                        listDataPerPage.Add(new("futan", 0, rowNo, wrkData.Futan.ToString()));
                    }
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
                #region 公費負担医療
                const int maxKohiRow = 10;
                int kohiIndex = (currentPage - 2) * maxKohiRow;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo && !r.IsKoukiAll).ToList(), new());
                if (kohiHoubetus.Count == 0 || kohiHoubetus.Count <= kohiIndex)
                {
                    _listTextData.Add(pageIndex, listDataPerPage);
                    _hasNextPage = false;
                    return 1;
                }

                //集計 
                for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex]) && !r.IsKoukiAll).ToList();
                    if (wrkReces != null)
                    {
                        //法別番号
                        listDataPerPage.Add(new("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]));
                        //公費名称
                        listDataPerPage.Add(new("kohiName", 0, rowNo, SokatuUtil.GetKohiName(kohiHoubetuMsts, myPrefNo, kohiHoubetus[kohiIndex])));

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                        //一部負担金
                        wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiIndex]));
                        listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));
                    }
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
            kaMsts = _kokhoFinder.GetKaMst(hpId);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
            //公費法別番号リストを取得
            kohiHoubetuMsts = _kokhoFinder.GetKohiHoubetuMst(hpId, seikyuYm);

            return (receInfs?.Count ?? 0) > 0;
        }
        #endregion

        public CommonReportingRequestModel GetP46KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
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
                    for (int prefCnt = 0; prefCnt <= 1; prefCnt++)
                    {
                        prefInOut = prefCnt == 0 ? "In" : "Out";

                        curReceInfs = receInfs.Where(r => prefCnt == 0 ? r.IsPrefIn && !r.IsKumiai : !r.IsPrefIn || r.IsKumiai).ToList();
                        if (!curReceInfs.Any()) continue;
                        currentPage = 1;
                        hasNextPage = true;

                        while (getData && hasNextPage)
                        {
                            UpdateDrawForm();
                            if (currentPage != 1)
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
