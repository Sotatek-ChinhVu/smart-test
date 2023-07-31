﻿using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service
{
    public class P35KoukiSeikyuCoReportService : IP35KoukiSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 35;
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private ICoKoukiSeikyuFinder _kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private bool hasNextPage;
        private int currentPage;
        private string currentHokensyaNo;
        private List<string> hokensyaNos;
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
        #endregion

        /// <summary>
        /// OutPut Data
        /// </summary>
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private const string _formFileName = "p35KoukiSeikyu.rse";
        private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;
        private readonly Dictionary<string, bool> _visibleAtPrint;

        #region Constructor and Init
        public P35KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _setFieldData = new();
            _singleFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
            _reportConfigPerPage = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private List<string> printHokensyaNos;
        #endregion

        public CommonReportingRequestModel GetP35KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if(getData)
            {
                foreach (string currentNo in hokensyaNos)
                {
                    currentHokensyaNo = currentNo;
                    currentPage = 1;
                    hasNextPage = true;
                    while (getData && hasNextPage)
                    {
                        UpdateDrawForm();
                        currentPage++;
                    }

                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KoukiSeikyuMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }

        #region Private function

        private bool UpdateDrawForm()
        {

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                Dictionary<string, string> fieldDataPerPage = new();
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
                    CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd"))
                );
                SetFieldData("reportGengo", wrkYmd.Gengo);
                SetFieldData("reportYear", wrkYmd.Year.ToString());
                SetFieldData("reportMonth", wrkYmd.Month.ToString());
                SetFieldData("reportDay", wrkYmd.Day.ToString());
                //保険者（下6桁頭2桁）
                //SetFieldData("hokensyaNo", _currentHokensyaNo.Substring(_currentHokensyaNo.Length - 6, 2));
                fieldDataPerPage.Add("hokensyaNo", currentHokensyaNo.Substring(currentHokensyaNo.Length - 6, 2));
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _setFieldData.Add(pageIndex, fieldDataPerPage);
                int prefNo = currentHokensyaNo.Substring(currentHokensyaNo.Length - 6, 2).AsInteger();
                SetFieldData("hokensyaPref", PrefCode.PrefName(prefNo));

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                #region Body
                const int maxRow = 2;

                if (currentPage == 1)
                {
                    //1枚目のみ記載する
                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
                        switch (rowNo)
                        {
                            //国保
                            case 0: wrkReces = curReceInfs.Where(r => r.IsKoukiIppan).ToList(); break;
                            case 1: wrkReces = curReceInfs.Where(r => r.IsKoukiUpper).ToList(); break;
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
                }

                #endregion

                #region 公費負担医療
                const int maxKohiRow = 5;
                int kohiIndex = (currentPage - 1) * maxKohiRow;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count == 0)
                {
                    _listTextData.Add(pageIndex, listDataPerPage);
                    hasNextPage = false;
                    return 1;
                }

                //集計
                for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty)).ToList();

                    //公費名称
                    listDataPerPage.Add(new("kohiName", 0, rowNo, SokatuUtil.GetKohiName(kohiHoubetuMsts, myPrefNo, kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty)));

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("kohiCount", 0, rowNo, wrkData.Count.ToString()));
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                    listDataPerPage.Add(new("kohiNissu", 0, rowNo, wrkData.Nissu.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                    listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus.Any() ? kohiHoubetus[kohiIndex] : string.Empty));
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));

                    kohiIndex++;
                    if (kohiIndex >= kohiHoubetus.Count)
                    {
                        hasNextPage = false;
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
                return false;
            }
            return true;
        }



        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
            //保険者番号リストを取得（県外→県内）
            hokensyaNos = wrkReceInfs.Where(r => !r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
            hokensyaNos.AddRange(
                wrkReceInfs.Where(r => r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList()
            );
            //公費法別番号リストを取得
            kohiHoubetuMsts = _kokhoFinder.GetKohiHoubetuMst(hpId, seikyuYm);

            return (receInfs?.Count ?? 0) > 0;
        }
        #endregion
    }
}
