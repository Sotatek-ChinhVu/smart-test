﻿using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P41KokhoSeikyuCoReportService : IP41KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 41;
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private ICoKokhoSeikyuFinder _kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private PrintUnit currentPrintUnit;
        private List<PrintUnit> printUnits;
        private List<CoHokensyaMstModel> hokensyaNames;
        private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        private const string _formFileNameP1 = "p41KokhoSeikyuP1.rse";
        private const string _formFileNameP2 = "p41KokhoSeikyuP2.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;

        struct PrintUnit
        {
            public bool IsPrefIn;
            public bool IsKumiai;
            public string HokensyaNo;
            public int HokenRate;
        }
        #endregion

        #region Constructor and Init
        public P41KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
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
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);

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
                string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == currentPrintUnit.HokensyaNo)?.Name ?? "";
                SetFieldData("hokensyaName", hokensyaName);
                Dictionary<string, string> fieldDataPerPage = new();
                fieldDataPerPage.Add("hokensyaNo", currentPrintUnit.HokensyaNo.PadLeft(6, '0'));
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _setFieldData.Add(pageIndex, fieldDataPerPage);

                //国保組合は給付割合に印をつける
                if (currentPrintUnit.IsKumiai)
                {
                    SetFieldData("kyufu7", currentPrintUnit.HokenRate != 30 ? "×" : string.Empty);
                    SetFieldData("kyufu8", currentPrintUnit.HokenRate == 20 ? "○" : string.Empty);
                    SetFieldData("kyufu9", currentPrintUnit.HokenRate == 10 ? "○" : string.Empty);
                    SetFieldData("kyufu10", currentPrintUnit.HokenRate == 100 ? "○" : string.Empty);
                }

                return 1;
            }
            #endregion

            #region BodyP1
            int UpdateFormBodyP1()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = currentPrintUnit.IsKumiai ?
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo && r.HokenRate == currentPrintUnit.HokenRate) :
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo);

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

                _hasNextPage = curReceInfs.Where(r => r.IsHeiyo).Count() >= 1;
                _listTextData.Add(pageIndex, listDataPerPage);

                return 1;
            }
            #endregion

            #region BodyP2
            int UpdateFormBodyP2()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = currentPrintUnit.IsKumiai ?
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo && r.HokenRate == currentPrintUnit.HokenRate) :
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo);

                const int maxKohiRow = 6;
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
                    //制度略称
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    fieldDataPerPage.Add(string.Format("kohiName{0}", rowNo), SokatuUtil.GetKohiName(kohiHoubetuMsts, myPrefNo, kohiHoubetus[kohiIndex]));

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }

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
                    wrkData.Futan = wrkReces.Sum(r => r.SagaKohiReceFutan(kohiHoubetus[kohiIndex]));
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));
                    //患者負担額
                    wrkData.PtFutan = wrkReces.Sum(r => r.SagaKohiIchibuFutan(kohiHoubetus[kohiIndex]));
                    listDataPerPage.Add(new("kohiPtFutan", 0, rowNo, wrkData.PtFutan.ToString()));

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
            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();

            //保険者番号リストを取得
            printUnits = wrkReceInfs.Where(r => !r.IsKumiai)
                .GroupBy(r => new { r.IsPrefIn, r.HokensyaNo })
                .Select(r => new PrintUnit { IsKumiai = false, IsPrefIn = r.Key.IsPrefIn, HokensyaNo = r.Key.HokensyaNo, HokenRate = -1 })
                .ToList();

            //国保組合は給付割合ごとに用紙を変えて印刷する
            printUnits.AddRange(
                wrkReceInfs.Where(r => r.IsKumiai)
                .GroupBy(r => new { r.IsPrefIn, r.HokensyaNo, r.HokenRate })
                .Select(r => new PrintUnit { IsKumiai = true, IsPrefIn = r.Key.IsPrefIn, HokensyaNo = r.Key.HokensyaNo, HokenRate = r.Key.HokenRate })
                .ToList()
            );

            //県外→県内
            printUnits = printUnits.OrderBy(p => p.IsPrefIn).ThenBy(p => p.HokensyaNo).ThenByDescending(p => p.HokenRate).ToList();

            //保険者名リスト取得
            var hokensyaNos = printUnits.Select(p => p.HokensyaNo).ToList();
            hokensyaNames = _kokhoFinder.GetHokensyaName(hpId, hokensyaNos);

            //公費法別番号リストを取得
            kohiHoubetuMsts = _kokhoFinder.GetKohiHoubetuMst(hpId, seikyuYm);

            return (wrkReceInfs?.Count ?? 0) > 0;
        }
        #endregion

        public CommonReportingRequestModel GetP40KokhoSeikyuKumiaiReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();
            int indexPage = 1;
            var fileName = new Dictionary<string, string>();

            if (getData)
            {
                foreach (PrintUnit currentUnit in printUnits)
                {
                    currentPrintUnit = currentUnit;
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