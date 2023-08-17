using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P26WelfareSeikyuCoReportService : IP26WelfareSeikyuCoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "43", "44", "45" };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoWelfareReceInfModel> receInfs;
        private CoHpInfModel hpInf;

        private int totalTensu;
        private int totalKohiFutan;
        private int ptIndex;
        private int rowIndex;
        private int rowCount;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p26WelfareSeikyu.rse";

        #region Constructor and Init
        public P26WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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
        private int currentPage;
        private bool hasNextPage;
        #endregion

        public CommonReportingRequestModel GetP26WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();
            hasNextPage = true;
            this.currentPage = 1;
            totalTensu = 0;
            totalKohiFutan = 0;
            ptIndex = 0;
            rowIndex = 1;
            rowCount = 0;

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
            const int maxRow = 20;
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
                //ページ数
                SetFieldData("currentPage", currentPage.ToString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = receInfs[ptIndex];

                    //全国公費併用で異点数の場合は２段書き
                    for (int ptRowNo = rowIndex; ptRowNo <= 2; ptRowNo++)
                    {
                        //３併で異点数の場合は２段表示
                        int kohiIndex = 0;
                        bool ptTwoRow = false;
                        bool ptTwoRowSkip = false;
                        if (curReceInf.ReceSbt.Substring(2, 1).AsInteger() >= 2)
                        {
                            for (int i = 1; i <= 4; i++)
                            {
                                if (curReceInf.KohiReceKisai(i) == 1)
                                {
                                    kohiIndex = i;
                                    if (curReceInf.KohiReceTensu(i) != curReceInf.Tensu)
                                    {
                                        ptTwoRow = true;
                                        if (curReceInf.KohiIchibuSotogaku(i) == 0 ||
                                            curReceInf.KohiIchibuSotogaku(i) == curReceInf.KohiIchibuFutan(i))  //福祉負担なし
                                        {
                                            ptTwoRowSkip = true;
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        if (!ptTwoRow && ptRowNo == 2)
                        {
                            break;
                        }
                        rowCount++;

                        //氏名
                        listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                        //生年月日
                        CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.BirthDay);
                        listDataPerPage.Add(new("birthGengo", 0, rowNo, wrkYmd.Gengo));
                        listDataPerPage.Add(new("birthYear", 0, rowNo, wrkYmd.Year.ToString()));
                        listDataPerPage.Add(new("birthMonth", 0, rowNo, wrkYmd.Month.ToString()));
                        listDataPerPage.Add(new("birthDay", 0, rowNo, wrkYmd.Day.ToString()));
                        //保険種別
                        switch (curReceInf.Houbetu)
                        {
                            case "01":
                                listDataPerPage.Add(new("hokenSbt0", 0, rowNo, "○"));
                                break;
                            case "02":
                                listDataPerPage.Add(new("hokenSbt0", 1, rowNo, "○"));
                                break;
                            case "03":
                            case "04":
                                listDataPerPage.Add(new("hokenSbt0", 2, rowNo, "○"));
                                break;
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                                listDataPerPage.Add(new("hokenSbt1", 0, rowNo, "○"));
                                break;
                            case "06":
                            case "63":
                            case "72":
                            case "73":
                            case "74":
                            case "75":
                                listDataPerPage.Add(new("hokenSbt1", 1, rowNo, "○"));
                                break;
                            case "07":
                                listDataPerPage.Add(new("hokenSbt1", 2, rowNo, "○"));
                                break;
                        }
                        //本家
                        listDataPerPage.Add(new("honkeKbn", 0, rowNo, curReceInf.ReceSbt.Substring(3, 1)));
                        //負担者番号
                        listDataPerPage.Add(new("futansyaNo0", 0, rowNo, curReceInf.FutansyaNo(kohiHoubetus).Substring(0, 2)));
                        listDataPerPage.Add(new("futansyaNo1", 0, rowNo, curReceInf.FutansyaNo(kohiHoubetus).Substring(4, 4)));
                        //受給者番号
                        listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, string.Format("{0, 7}", curReceInf.JyukyusyaNo(kohiHoubetus))));
                        //診療年月
                        string sinYm = CIUtil.SDateToWDate(curReceInf.SinYm * 100 + 1).ToString().Substring(1, 4);
                        listDataPerPage.Add(new("sinYm", 0, rowNo, sinYm));

                        if (ptTwoRow)
                        {
                            //２段表示

                            if (ptRowNo == 1)
                            {
                                //上段（公費併用分）

                                if (ptTwoRowSkip)
                                {
                                    rowCount--;
                                    rowIndex++;
                                    continue;
                                }

                                //請求点数
                                listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.KohiReceTensu(kohiIndex).ToString()));
                                totalTensu += curReceInf.KohiReceTensu(kohiIndex);
                                //公費分患者負担額
                                listDataPerPage.Add(new("kohiFutan", 0, rowNo, curReceInf.KohiIchibuSotogaku(kohiIndex).ToString()));
                                totalKohiFutan += curReceInf.KohiIchibuSotogaku(kohiIndex);
                                //自己負担
                                listDataPerPage.Add(new("futan", 0, rowNo, string.Format("{0:#,0}", curReceInf.KohiIchibuFutan(kohiIndex))));
                            }
                            else
                            {
                                //下段（保険分）

                                //請求点数
                                listDataPerPage.Add(new("tensu", 0, rowNo, (curReceInf.KohiReceTensu(kohiHoubetus) - curReceInf.KohiReceTensu(kohiIndex)).ToString()));
                                totalTensu += (curReceInf.Tensu - curReceInf.KohiReceTensu(kohiIndex));
                                //公費分患者負担額
                                if (seikyuYm < KaiseiDate.m202204)  //変更日不明のためタイマー設定
                                {
                                    if (ptTwoRowSkip)
                                    {
                                        //３併で異点数だが公１に係る医療費が全額助成の場合は、上段の記載を省略するので２併の場合と同様に扱う
                                        if (curReceInf.IsSiteiKohi ||
                                            curReceInf.IsChoki ||
                                            (curReceInf.KogakuOverKbn != KogakuOverStatus.None && curReceInf.IsTokurei))
                                        {
                                            //前期１割（指定公費）の場合は請求点数の１割の金額を記載
                                            //マル長もしくは限度額特例月で高額療養費の現物給付が発生している場合も記載
                                            listDataPerPage.Add(new("kohiFutan", 0, rowNo, curReceInf.HokenIchibuFutan.ToString()));
                                            totalKohiFutan += curReceInf.HokenIchibuFutan;
                                        }
                                    }
                                    else
                                    {
                                        if (curReceInf.IsSiteiKohi ||
                                            curReceInf.IsChoki ||
                                            (curReceInf.KogakuOverKbn != KogakuOverStatus.None && curReceInf.IsTokurei))
                                        {
                                            //前期１割（指定公費）の場合は請求点数の１割の金額を記載
                                            //マル長もしくは限度額特例月で高額療養費の現物給付が発生している場合も記載
                                            int wrkFutan = curReceInf.IchibuFutan;
                                            for (int i = kohiIndex + 1; i <= 4; i++)
                                            {
                                                wrkFutan += curReceInf.KohiFutan(i);
                                            }
                                            wrkFutan -= curReceInf.KohiIchibuSotogaku(kohiIndex);
                                            listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkFutan.ToString()));
                                            totalKohiFutan += wrkFutan;
                                        }
                                    }
                                }
                                //自己負担
                                listDataPerPage.Add(new("futan", 0, rowNo, string.Format("{0:#,0}", curReceInf.KohiIchibuSotogaku(kohiHoubetus) - curReceInf.KohiIchibuFutan(kohiIndex)).ToString()));
                            }
                        }
                        else
                        {
                            //１段表示

                            //請求点数
                            listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.KohiReceTensu(kohiHoubetus).ToString()));
                            totalTensu += curReceInf.Tensu;
                            //公費分患者負担額
                            if (curReceInf.ReceSbt.Substring(2, 1) == "1")
                            {
                                //２併
                                if (seikyuYm < KaiseiDate.m202204)  //変更日不明のためタイマー設定
                                {
                                    if (curReceInf.IsSiteiKohi ||
                                        curReceInf.IsChoki ||
                                        (curReceInf.KogakuOverKbn != KogakuOverStatus.None && curReceInf.IsTokurei))
                                    {
                                        //前期１割（指定公費）の場合は請求点数の１割の金額を記載
                                        //マル長もしくは限度額特例月で高額療養費の現物給付が発生している場合も記載
                                        listDataPerPage.Add(new("kohiFutan", 0, rowNo, curReceInf.HokenIchibuFutan.ToString()));
                                        totalKohiFutan += curReceInf.HokenIchibuFutan;
                                    }
                                }
                            }
                            else
                            {
                                //３併（同点数）の場合、公１負担を除いた金額
                                int wrkFutan = curReceInf.IchibuFutan;
                                for (int i = kohiIndex + 1; i <= 4; i++)
                                {
                                    wrkFutan += curReceInf.KohiFutan(i);
                                }
                                listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkFutan.ToString()));
                                totalKohiFutan += wrkFutan;
                            }
                            //自己負担
                            listDataPerPage.Add(new("futan", 0, rowNo, string.Format("{0:#,0}", curReceInf.KohiIchibuSotogaku(kohiHoubetus)).ToString()));
                        }

                        //公長表示の確認欄
                        if ((ptTwoRow && ptRowNo == 1) || !ptTwoRow)
                        {
                            string wrkVal = curReceInf.KohiHoubetu(kohiIndex);
                            if (wrkVal != "")
                            {
                                listDataPerPage.Add(new("biko", 0, (short)(rowNo * 2), wrkVal));
                                listDataPerPage.Add(new("bikoMaru", 0, (short)(rowNo * 2), "○"));
                            }
                        }

                        if ((ptTwoRow && ptRowNo == 2) || !ptTwoRow)
                        {
                            if (curReceInf.TokkiContains("02"))
                            {
                                listDataPerPage.Add(new("biko", 0, (short)(rowNo * 2 + 1), "長"));
                                listDataPerPage.Add(new("bikoMaru", 0, (short)(rowNo * 2 + 1), "○"));
                            }
                            else if (curReceInf.TokkiContains("16"))
                            {
                                listDataPerPage.Add(new("biko", 0, (short)(rowNo * 2 + 1), "長2"));
                                listDataPerPage.Add(new("bikoMaru", 0, (short)(rowNo * 2 + 1), "○"));
                            }
                            else if (curReceInf.TokkiContains("01"))
                            {
                                listDataPerPage.Add(new("biko", 0, (short)(rowNo * 2 + 1), "公"));
                                listDataPerPage.Add(new("bikoMaru", 0, (short)(rowNo * 2 + 1), "○"));
                            }
                        }

                        if (rowIndex == 1)
                        {
                            rowIndex++;
                            if (ptTwoRow)
                            {
                                rowNo++;
                                if (rowNo >= maxRow)
                                {
                                    return 1;
                                }
                            }
                        }
                    }

                    ptIndex++;
                    rowIndex = 1;
                    if (ptIndex >= receInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //合計
                if (!_hasNextPage)
                {
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                    fieldDataPerPage.Add("totalCount", rowCount.ToString());
                    fieldDataPerPage.Add("totalTensu", totalTensu.ToString());
                    fieldDataPerPage.Add("totalKohiFutan", totalKohiFutan.ToString());
                    fieldDataPerPage.Add("totalFutan", receInfs.Sum(r => r.IchibuFutan).ToString());

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }
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
            receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Syaho);

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
