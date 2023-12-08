using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P23WelfareSeikyuCoReportService : IP23WelfareSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 23;
        private List<string> kohiHoubetus = new List<string> { "81", "82", "83", "85" };

        private struct countData
        {
            public int Count;
            public int Tensu;
            public int KohiTensu;
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
        private List<CoP23WelfareReceInfModel> receInfs = new();
        private CoHpInfModel hpInf = new();

        private List<string> cityNames = new();
        private string currentCityName = "";
        private List<CoHokensyaMstModel> hokensyaNames = new();
        private List<CoKohiHoubetuMstModel> kohiHoubetuMsts = new();
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p23WelfareSeikyu.rse";

        #region Constructor and Init
        public P23WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
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
        private bool hasNextPage;
        private int currentPage;
        #endregion

        public CommonReportingRequestModel GetP23WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();
                currentPage = 1;
                hasNextPage = true;

                if (getData)
                {
                    foreach (string currentCity in cityNames)
                    {
                        currentCityName = currentCity;
                        while (getData && hasNextPage)
                        {
                            UpdateDrawForm();
                            currentPage++;
                        }
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
            const int maxRow = 25;
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
                //市町村
                SetFieldData("hokensyaName", currentCityName);
                //合計金額
                var curReceInfs = receInfs.Where(r => r.CityName == currentCityName);
                SetFieldData("totalFutan", curReceInfs.Sum(r => r.KohiFutan).ToString());
                //請求総件数
                SetFieldData("totalCount", curReceInfs.Count().ToString());
                //ページ数
                int totalPage = (int)Math.Ceiling((double)curReceInfs.Count() / maxRow);
                SetFieldData("currentPage", currentPage.ToString());
                SetFieldData("totalPage", totalPage.ToString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

                var curReceInfs = receInfs.Where(r => r.CityName == currentCityName).OrderBy(r => r.WelfareJyukyusyaNo).ToList();
                int ptIndex = (currentPage - 1) * maxRow;

                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = curReceInfs[ptIndex];

                    //受給者証番号
                    listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, curReceInf.WelfareJyukyusyaNo ?? string.Empty));
                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                    totalData.Count++;
                    //総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.Tensu.ToString()));
                    totalData.Tensu += curReceInf.Tensu;
                    //結精点数
                    listDataPerPage.Add(new("kohiTensu", 0, rowNo, curReceInf.KohiReceTensu("10").ToString()));
                    totalData.KohiTensu += curReceInf.KohiReceTensu("10");
                    //請求割合
                    switch (curReceInf.HokenRate)
                    {
                        case 20: listDataPerPage.Add(new("hokenRate", 0, rowNo, "○")); break;
                        case 10: listDataPerPage.Add(new("hokenRate", 1, rowNo, "○")); break;
                    }
                    //市町村負担額
                    listDataPerPage.Add(new("kohiFutan", 0, rowNo, curReceInf.KohiFutan.ToString()));
                    totalData.KohiFutan += curReceInf.KohiFutan;

                    #region 備考
                    List<string> bikos = new List<string>();
                    //特例退職者
                    if (curReceInf.HokenKbn == HokenKbn.Syaho && new string[] { "63", "72", "73", "75" }.Contains(curReceInf.Houbetu))
                    {
                        bikos.Add("特退等");
                    }
                    //国保組合
                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == curReceInf.HokensyaNo)?.Name ?? "";
                    if (curReceInf.IsKokuhoKumiai)
                    {
                        bikos.Add(hokensyaName);
                    }
                    //国保特例
                    else if (curReceInf.HokenKbn == HokenKbn.Kokho && hokensyaName != "" &&
                        (!hokensyaName.Contains(currentCityName) || (currentCityName == "名古屋市" && hokensyaName == "北名古屋市")))
                    {
                        bikos.Add(string.Format("特例({0})", hokensyaName));
                    }
                    //月遅れ
                    if (curReceInf.SinYm != seikyuYm)
                    {
                        CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.SinYm * 100 + 1);
                        bikos.Add(string.Format("{0}{1}年{2}月", wrkYmd.Gengo, wrkYmd.Year, wrkYmd.Month));
                    }
                    //公費併用
                    if (curReceInf.ReceSbt.Substring(2, 1).AsInteger() >= 2)
                    {
                        int kohiIndex = 0;

                        for (int i = 1; i <= 4; i++)
                        {
                            if (curReceInf.KohiReceKisai(i) == 1)
                            {
                                kohiIndex = i;
                                break;
                            }
                        }

                        if (kohiIndex >= 1)
                        {
                            string kohiHoubetu = curReceInf.KohiHoubetu(kohiIndex);
                            string kohiName = SokatuUtil.GetKohiName(kohiHoubetuMsts, myPrefNo, kohiHoubetu) ?? kohiHoubetu;

                            if (curReceInf.KohiReceTensu(kohiIndex) != curReceInf.Tensu)
                            {
                                //異点数の場合
                                bikos.Add(string.Format("{0}({1}点)", kohiName, curReceInf.KohiReceTensu(kohiIndex)));
                            }
                            else if (curReceInf.HokenRate > curReceInf.PtRate)
                            {
                                //負担率指定の場合
                                bikos.Add(kohiName);
                            }
                        }
                    }
                    //マル長
                    if (curReceInf.TokkiContains("02"))
                    {
                        bikos.Add("長");
                    }
                    if (curReceInf.TokkiContains("16"))
                    {
                        bikos.Add("長２");
                    }
                    //高額療養費
                    if (curReceInf.KogakuOverKbn != KogakuOverStatus.None)
                    {
                        if (curReceInf.IsElder)
                        {
                            if (curReceInf.SinYm >= KaiseiDate.m201808)
                            {
                                switch (curReceInf.KogakuKbn)
                                {
                                    case 0: bikos.Add("29区エ"); break;
                                    case 4:
                                    case 5: bikos.Add("30区オ"); break;
                                    case 26: bikos.Add("26区ア"); break;
                                    case 27: bikos.Add("27区イ"); break;
                                    case 28: bikos.Add("28区ウ"); break;
                                }
                            }
                            else
                            {
                                switch (curReceInf.KogakuKbn)
                                {
                                    case 0: bikos.Add("18一般"); break;
                                    case 3: bikos.Add("17上位"); break;
                                    case 4:
                                    case 5: bikos.Add("19低所"); break;
                                }
                            }
                        }
                        else
                        {
                            switch (curReceInf.KogakuKbn)
                            {
                                case 26: bikos.Add("26区ア"); break;
                                case 27: bikos.Add("27区イ"); break;
                                case 28: bikos.Add("28区ウ"); break;
                                case 29: bikos.Add("29区エ"); break;
                                case 20: bikos.Add("30区オ"); break;
                            }
                        }
                    }
                    listDataPerPage.Add(new("biko", 0, rowNo, string.Join(" ", bikos)));
                    #endregion

                    ptIndex++;
                    if (ptIndex >= curReceInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //件数
                listDataPerPage.Add(new("ptName", 0, maxRow, totalData.Count.ToString()));
                //総点数
                listDataPerPage.Add(new("tensu", 0, maxRow, totalData.Tensu.ToString()));
                //結精点数
                listDataPerPage.Add(new("kohiTensu", 0, maxRow, totalData.KohiTensu.ToString()));
                //市町村負担額
                listDataPerPage.Add(new("kohiFutan", 0, maxRow, totalData.KohiFutan.ToString()));
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, 0);
            //愛知県用のモデルクラスにコピー
            receInfs = wrkReces.Select(x => new CoP23WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHoubetus)).ToList();
            //負担者番号の一覧を取得
            cityNames = receInfs.GroupBy(r => r.CityName).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            //保険者番号リストを取得
            var hokensyaNos = receInfs.Where(r => r.IsKokuhoKumiai).GroupBy(r => r.HokensyaNo).Select(r => r.Key).ToList();
            //保険者名を取得
            hokensyaNames = _welfareFinder.GetHokensyaName(hpId, hokensyaNos);
            //公費法別番号リストを取得
            kohiHoubetuMsts = _welfareFinder.GetKohiHoubetuMst(hpId, seikyuYm);


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
