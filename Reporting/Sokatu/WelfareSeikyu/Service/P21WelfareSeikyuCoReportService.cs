using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public class P21WelfareSeikyuCoReportService : IP21WelfareSeikyuCoReportService
{
    #region Constant
    private List<string> kohiHoubetus = new List<string> { "43", "44", "45", "46" };

    private struct countData
    {
        public int Tensu7;
        public int Tensu8;
        public int Tensu9;
        public int Count7;
        public int Count8;
        public int Count9;
        public int KohiTensu;
        public int PtFutan;
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
    private List<CoWelfareReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion

    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    private string _formFileName = "p21WelfareSeikyu.rse";

    #region Constructor and Init
    public P21WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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

    public CommonReportingRequestModel GetP21WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        this.hpId = hpId;
        this.seikyuYm = seikyuYm;
        this.seikyuType = seikyuType;
        var getData = GetData();
        currentPage = 1;
        hasNextPage = true;

        if (seikyuYm >= 202303)
        {
            _formFileName = "p21WelfareSeikyu_2303.rse";
        }

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
        const int maxRow = 10;
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

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            int ptIndex = (currentPage - 1) * maxRow;

            countData totalData = new countData();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if(receInfs.Count <= ptIndex)
                {
                    _hasNextPage = false;
                    break;
                }
                var curReceInf = receInfs[ptIndex];

                //受給者番号
                string jyukyusyaNo = string.Format("{0, -14}", curReceInf.TokusyuNo(kohiHoubetus).Replace("-", ""));
                listDataPerPage.Add(new("jyukyusyaNo0", 0, rowNo, jyukyusyaNo.Substring(0, 3)));
                listDataPerPage.Add(new("jyukyusyaNo1", 0, rowNo, jyukyusyaNo.Substring(3, 11)));
                //氏名
                listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                //入院外来
                listDataPerPage.Add(new("gairai", 0, rowNo, "○"));
                //社保・国保・退職・後期
                short hokenKbn = 0;  //社保
                if (curReceInf.HokenKbn == HokenKbn.Kokho)
                {
                    switch (curReceInf.ReceSbt.Substring(1, 1))
                    {
                        case "3": hokenKbn = 3; break;  //退職
                        case "4": hokenKbn = 2; break;  //後期
                        default: hokenKbn = 1; break;   //国保
                    }
                }
                listDataPerPage.Add(new("hokenKbn", hokenKbn, rowNo, "○"));

                //長期（レセに特記事項の記載がない場合も記載する）
                string choki = curReceInf.IsChokiHoken ? "1" :
                    curReceInf.IsKohiHoubetu("52") ? "2" :
                    curReceInf.IsKohiHoubetu("51") ? "3" :
                    curReceInf.IsKohiHoubetu("15") ? "4" :
                    curReceInf.IsKohiHoubetu("16") ? "4" :
                    curReceInf.IsKohiHoubetu("21") ? "4" :
                    curReceInf.IsKohiHoubetu("54") ? "5" : "";
                listDataPerPage.Add(new("choki", 0, rowNo, choki));
                //総点数
                int hokenKyufu = (100 - curReceInf.HokenRate) / 10;
                listDataPerPage.Add(new(string.Format("tensu{0}", hokenKyufu), 0, rowNo, curReceInf.Tensu.ToString()));

                //前期高齢
                string zenki = "";
                if (curReceInf.IsElderZenki)
                {
                    zenki = curReceInf.IsSiteiKohi || curReceInf.IsElderJyoi ? "1" : "2";
                }
                listDataPerPage.Add(new("zenkiRate", 0, rowNo, zenki));

                switch (hokenKyufu)
                {
                    case 7:
                        totalData.Count7++;
                        totalData.Tensu7 += curReceInf.Tensu;
                        break;
                    case 8:
                        totalData.Count8++;
                        totalData.Tensu8 += curReceInf.Tensu;
                        break;
                    case 9:
                        totalData.Count9++;
                        totalData.Tensu9 += curReceInf.Tensu;
                        break;
                }

                //公費対象点数（異点数の場合は記載）
                int kohiIndex = 0;
                if (curReceInf.ReceSbt.Substring(2, 1).AsInteger() >= 2)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        if (curReceInf.KohiReceKisai(i) == 1)
                        {
                            if (curReceInf.KohiReceTensu(i) != curReceInf.Tensu)
                            {
                                kohiIndex = i;
                            }
                            break;
                        }
                    }
                }
                if (kohiIndex >= 1)
                {
                    listDataPerPage.Add(new("kohiTensu", 0, rowNo, curReceInf.KohiReceTensu(kohiIndex).ToString()));
                    totalData.KohiTensu += curReceInf.KohiReceTensu(kohiIndex);
                }

                //患者負担額
                int ptFutan = 0;
                for (int i = 1; i <= 4; i++)
                {
                    if (curReceInf.KohiReceKisai(i) == 1)
                    {
                        //公費併用の受給者については、原則、公費負担医療に係る患者負担額を記載する
                        ptFutan = curReceInf.KohiReceFutan(i);
                        break;
                    }
                }
                ptFutan = ptFutan > 0 ? ptFutan : curReceInf.HokenReceFutan;
                //マル長の場合はレセ記載に関係なく記載が必要
                ptFutan = ptFutan > 0 ? ptFutan :
                    curReceInf.IsChokiHoken ? curReceInf.IchibuFutan + curReceInf.Kohi1Futan + curReceInf.Kohi2Futan + curReceInf.Kohi3Futan + curReceInf.Kohi4Futan : 0;
                listDataPerPage.Add(new("ptFutan", 0, rowNo, ptFutan.ToString()));
                totalData.PtFutan += ptFutan;

                //備考
                List<string> bikos = new List<string>();
                if (curReceInf.SinYm != seikyuYm)
                {
                    bikos.Add(string.Format("{0}年{1}月分", curReceInf.SinYm / 100, curReceInf.SinYm % 100));
                }
                if (curReceInf.IsChokiHoken && kohiIndex >= 1)
                {
                    bikos.Add(curReceInf.FutansyaNo(kohiIndex));
                }
                for (int i = 26; i <= 30; i++)
                {
                    var wrkBiko = curReceInf.TokkiContainsName(i.ToString());
                    if (!string.IsNullOrEmpty(wrkBiko))
                    {
                        bikos.Add(wrkBiko);
                    }
                }
                listDataPerPage.Add(new("biko", 0, rowNo, string.Join("\r\n", bikos)));

                ptIndex++;
                if (ptIndex >= receInfs.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            //合計
            SetFieldData("totalCount7", totalData.Count7.ToString());
            SetFieldData("totalCount8", totalData.Count8.ToString());
            SetFieldData("totalCount9", totalData.Count9.ToString());
            SetFieldData("totalTensu7", totalData.Tensu7.ToString());
            SetFieldData("totalTensu8", totalData.Tensu8.ToString());
            SetFieldData("totalTensu9", totalData.Tensu9.ToString());
            SetFieldData("totalKohiTensu", totalData.KohiTensu.ToString());
            SetFieldData("totalPtFutan", totalData.PtFutan.ToString());
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
        receInfs = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, 0);

        return (receInfs?.Count ?? 0) == 0;
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
