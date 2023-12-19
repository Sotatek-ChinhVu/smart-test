using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.Syaho.DB;
using Reporting.Sokatu.Syaho.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.Syaho.Service;

public class SyahoCoReportService : ISyahoCoReportService
{
    private readonly ICoSyahoFinder _finder;

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, bool> _visibleFieldList;
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<string, string> _fileNamePageMap;
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    private int currentPage;
    private bool hasNextPage;
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;

    private int totalCount;

    public SyahoCoReportService(ICoSyahoFinder finder)
    {
        _finder = finder;
        _singleFieldData = new();
        _listTextData = new();
        _visibleFieldList = new();
        _extralData = new();
        _setFieldData = new();
        _fileNamePageMap = new();
        receInfs = new();
        hpInf = new();
    }

    public CommonReportingRequestModel GetSyahoPrintData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            currentPage = 1;
            hasNextPage = true;
            var getData = GetData();

            if (getData)
            {
                while (hasNextPage && getData)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            _extralData.Add("totalPage", (currentPage - 1).ToString());
            _fileNamePageMap.Add("1", "p99SyahoSokatuP1.rse");
            _fileNamePageMap.Add("2", "p99SyahoSokatuP2.rse");

            return new SyahoMapper(_singleFieldData, _visibleFieldList, _setFieldData, _listTextData, _extralData, _fileNamePageMap).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    private void UpdateDrawForm()
    {
        hasNextPage = currentPage == 1;
        #region P1Header
        void UpdateFormHeaderP1()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
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
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());

            //印
            if (!_visibleFieldList.ContainsKey("inkan"))
            {
                _visibleFieldList.Add("inkan", seikyuYm < KaiseiDate.m202210);
            }
            if (!_visibleFieldList.ContainsKey("inkanMaru"))
            {
                _visibleFieldList.Add("inkanMaru", seikyuYm < KaiseiDate.m202210);
            }
        }
        #endregion

        #region P1Body
        void UpdateFormBodyP1()
        {
            List<ListTextObject> listDataPerPage = new();
            countData totalData = new();
            countData subData = new();
            const int maxRow = 50;

            for (short rowNo = 0; rowNo <= maxRow; rowNo++)
            {
                List<CoReceInfModel> wrkReces;
                switch (rowNo)
                {
                    //70歳以上一般・低所得
                    case 0: wrkReces = receInfs.Where(r => r.IsNrElderIppan && r.IsHeiyo).ToList(); break;
                    case 1: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "01").ToList(); break;
                    case 2: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "02" && new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList(); break;
                    case 3: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "02" && !new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList(); break;
                    case 4: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "03").ToList(); break;
                    case 5: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "04").ToList(); break;
                    case 6: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn == 2).ToList(); break;
                    case 7: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn != 2).ToList(); break;
                    case 8: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "06").ToList(); break;
                    case 9: wrkReces = receInfs.Where(r => r.IsNrElderIppan && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList(); break;
                    //70歳以上7割
                    case 11: wrkReces = receInfs.Where(r => r.IsNrElderUpper && r.IsHeiyo).ToList(); break;
                    case 12: wrkReces = receInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "01").ToList(); break;
                    case 13: wrkReces = receInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "02" && new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList(); break;
                    case 14: wrkReces = receInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "02" && !new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList(); break;
                    case 15: wrkReces = receInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn == 2).ToList(); break;
                    case 16: wrkReces = receInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn != 2).ToList(); break;
                    case 17: wrkReces = receInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "06").ToList(); break;
                    case 18: wrkReces = receInfs.Where(r => r.IsNrElderUpper && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList(); break;
                    //本人
                    case 20: wrkReces = receInfs.Where(r => r.IsNrMine && r.IsHeiyo).ToList(); break;
                    case 21: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "01").ToList(); break;
                    case 22: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "02" && new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList(); break;
                    case 23: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "02" && !new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList(); break;
                    case 24: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "03").ToList(); break;
                    case 25: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "04").ToList(); break;
                    case 26: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn == 2).ToList(); break;
                    case 27: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn != 2).ToList(); break;
                    case 28: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "06").ToList(); break;
                    case 29: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "07").ToList(); break;
                    case 30: wrkReces = receInfs.Where(r => r.IsNrMine && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList(); break;
                    //家族
                    case 32: wrkReces = receInfs.Where(r => r.IsNrFamily && r.IsHeiyo).ToList(); break;
                    case 33: wrkReces = receInfs.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "01").ToList(); break;
                    case 34: wrkReces = receInfs.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "02").ToList(); break;
                    case 35: wrkReces = receInfs.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "03").ToList(); break;
                    case 36: wrkReces = receInfs.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "04").ToList(); break;
                    case 37: wrkReces = receInfs.Where(r => r.IsNrFamily && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu)).ToList(); break;
                    case 38: wrkReces = receInfs.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "06").ToList(); break;
                    case 39: wrkReces = receInfs.Where(r => r.IsNrFamily && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList(); break;
                    //6歳未満
                    case 41: wrkReces = receInfs.Where(r => r.IsNrPreSchool && r.IsHeiyo).ToList(); break;
                    case 42: wrkReces = receInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "01").ToList(); break;
                    case 43: wrkReces = receInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "02").ToList(); break;
                    case 44: wrkReces = receInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "03").ToList(); break;
                    case 45: wrkReces = receInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "04").ToList(); break;
                    case 46: wrkReces = receInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu)).ToList(); break;
                    case 47: wrkReces = receInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "06").ToList(); break;
                    case 48: wrkReces = receInfs.Where(r => r.IsNrPreSchool && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList(); break;

                    default:
                        //小計
                        listDataPerPage.Add(new("count", 0, rowNo, subData.Count.ToString()));
                        listDataPerPage.Add(new("nissu", 0, rowNo, subData.Nissu.ToString()));
                        listDataPerPage.Add(new("tensu", 0, rowNo, subData.Tensu.ToString()));
                        listDataPerPage.Add(new("futan", 0, rowNo, subData.Futan.ToString()));
                        subData.Clear();
                        continue;
                }

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                //実日数
                wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                listDataPerPage.Add(new("nissu", 0, rowNo, wrkData.Nissu.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                listDataPerPage.Add(new("futan", 0, rowNo, wrkData.Futan.ToString()));

                //公費の併用欄は除外する
                if (!new int[] { 0, 11, 20, 32, 41 }.Contains(rowNo))
                {
                    //小計
                    subData.AddValue(wrkData);
                }
                //合計
                totalData.AddValue(wrkData);
            }

            //①合計
            listDataPerPage.Add(new("count", 0, maxRow, totalData.Count.ToString()));
            totalCount = totalData.Count;
            _listTextData.Add(currentPage, listDataPerPage);
        }
        #endregion

        #region P2Header
        void UpdateFormHeaderP2()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
        }
        #endregion

        #region P2Body
        void UpdateFormBodyP2()
        {
            List<ListTextObject> listDataPerPage = new();
            int totalCount2 = 0;

            #region 公費と医保の併用
            //固定枠
            List<string> kohiHoubetus = new List<string> { "12", "10" };
            //その他
            var wrkHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsNrAll && r.IsHeiyo).ToList(), kohiHoubetus);
            kohiHoubetus = kohiHoubetus.Union(wrkHoubetus).ToList();

            //集計
            for (short rowNo = 0; rowNo <= kohiHoubetus.Count - 1; rowNo++)
            {
                //最大11個
                if (rowNo >= 11) break;

                List<CoReceInfModel> wrkReces = receInfs.Where(r => r.IsNrAll && r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                if (rowNo >= 2)
                {
                    //名称
                    listDataPerPage.Add(new("kohiIhoName", 0, rowNo, kohiHoubetus[rowNo]));
                }

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Sum(r => r.KohiReceCount(kohiHoubetus[rowNo]));
                listDataPerPage.Add(new("kohiIhoCnt", 0, rowNo, wrkData.Count.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo]));
                listDataPerPage.Add(new("kohiIhoTensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo]));
                listDataPerPage.Add(new("kohiIhoFutan", 0, rowNo, wrkData.Futan.ToString()));

                //合計
                totalCount2 += wrkData.Count;
            }
            #endregion

            #region 公費と公費の併用
            kohiHoubetus.Clear();

            //固定枠
            kohiHoubetus = new List<string> { "1012" };
            //その他
            wrkHoubetus = getHoubetuPair(receInfs.Where(r => r.IsKohiOnly && r.IsHeiyo).ToList());
            kohiHoubetus = kohiHoubetus.Union(wrkHoubetus).ToList();

            //集計
            for (short pariNo = 0; pariNo <= kohiHoubetus.Count - 1; pariNo++)
            {
                //最大3組合せ
                if (pariNo > 2) break;

                bool sameHoubetus = false;
                List<CoReceInfModel>? wrkReces = null;
                if (kohiHoubetus[pariNo].Length == 2)
                {
                    sameHoubetus = true;
                    wrkReces = receInfs.Where(r =>
                        r.IsKohiOnly &&
                        r.IsHeiyo &&
                        r.IsKohi(kohiHoubetus[pariNo]) &&
                        r.IsKohiHoubetuOnly(kohiHoubetus[pariNo])
                    ).ToList();
                }
                else
                {
                    wrkReces = receInfs.Where(r =>
                        r.IsKohiOnly &&
                        r.IsHeiyo &&
                        r.IsKohi(kohiHoubetus[pariNo].Substring(0, 2)) &&
                        r.IsKohi(kohiHoubetus[pariNo].Substring(2, 2))
                    ).ToList();
                }

                for (int i = 0; i <= 1; i++)
                {
                    string wrkHoubetu = "";
                    if (sameHoubetus)
                    {
                        wrkHoubetu = kohiHoubetus[pariNo];
                    }
                    else
                    {
                        wrkHoubetu = kohiHoubetus[pariNo].Substring(i * 2, 2);
                    }

                    if (pariNo == 0)
                    {
                        //固定枠は優先順位と逆順なので調整
                        wrkHoubetu = i == 0 ? "12" : "10";
                    }
                    short rowNo = (short)(pariNo * 2 + i);

                    if (pariNo >= 1)
                    {
                        //名称
                        listDataPerPage.Add(new("kohiKohiName", 0, rowNo, wrkHoubetu));
                    }

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("kohiKohiCnt", 0, rowNo, wrkData.Count.ToString()));
                    if (sameHoubetus)
                    {
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensuPair(wrkHoubetu, i + 1));
                        listDataPerPage.Add(new("kohiKohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                        //一部負担金
                        wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutanPair(wrkHoubetu, i + 1));
                        listDataPerPage.Add(new("kohiKohiFutan", 0, rowNo, wrkData.Futan.ToString()));
                    }
                    else
                    {
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(wrkHoubetu));
                        listDataPerPage.Add(new("kohiKohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                        //一部負担金
                        wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(wrkHoubetu));
                        listDataPerPage.Add(new("kohiKohiFutan", 0, rowNo, wrkData.Futan.ToString()));
                    }

                    //合計
                    totalCount2 += wrkData.Count;
                }
            }
            #endregion

            #region 公費単独
            //固定枠
            kohiHoubetus = new List<string> { "12", "11", "20" };
            //その他
            wrkHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsKohiOnly && !r.IsHeiyo).ToList(), kohiHoubetus);
            kohiHoubetus = kohiHoubetus.Union(wrkHoubetus).ToList();

            //集計
            for (short rowNo = 0; rowNo <= kohiHoubetus.Count - 1; rowNo++)
            {
                //最大6個
                if (rowNo > 5) break;

                List<CoReceInfModel> wrkReces = receInfs.Where(r => r.IsKohiOnly && !r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                if (rowNo >= 3)
                {
                    //名称
                    listDataPerPage.Add(new("kohiName", 0, rowNo, kohiHoubetus[rowNo]));
                }

                countData wrkData = new countData();
                //件数
                wrkData.Count = wrkReces.Count;
                listDataPerPage.Add(new("kohiCnt", 0, rowNo, wrkData.Count.ToString()));
                //点数
                wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo]));
                listDataPerPage.Add(new("kohiTensu", 0, rowNo, wrkData.Tensu.ToString()));
                //一部負担金
                wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo]));
                listDataPerPage.Add(new("kohiFutan", 0, rowNo, wrkData.Futan.ToString()));

                //合計
                totalCount2 += wrkData.Count;
            }
            #endregion

            //合計①+②
            totalCount += totalCount2;

            Dictionary<string, string> fieldDataPerPage = new()
            {
                //②合計
                { "totalCnt2", totalCount2.ToString() },
                { "totalCnt", totalCount.ToString() }
            };

            _listTextData.Add(currentPage, listDataPerPage);
            _setFieldData.Add(currentPage, fieldDataPerPage);
        }
        #endregion

        //公費の組合せを取得する
        List<string> getHoubetuPair(List<CoReceInfModel> coReceInfs)
        {
            List<string> retNums = new List<string>();

            foreach (var coReceInf in coReceInfs)
            {
                List<string> houbetus = new List<string>();
                if (coReceInf.Kohi1ReceKisai) houbetus.Add(coReceInf.Kohi1Houbetu ?? string.Empty);
                if (coReceInf.Kohi2ReceKisai) houbetus.Add(coReceInf.Kohi2Houbetu ?? string.Empty);
                if (coReceInf.Kohi3ReceKisai) houbetus.Add(coReceInf.Kohi3Houbetu ?? string.Empty);
                if (coReceInf.Kohi4ReceKisai) houbetus.Add(coReceInf.Kohi4Houbetu ?? string.Empty);

                var wrkPair = string.Join("", houbetus.Distinct());
                if (wrkPair != "1012" && wrkPair != "1210" && !retNums.Contains(wrkPair))
                {
                    retNums.Add(wrkPair);
                }
            }

            //法別番号順にソート
            retNums.Sort();

            return retNums;
        }

        if (currentPage == 1)
        {
            UpdateFormHeaderP1();
            UpdateFormBodyP1();
        }
        else
        {
            UpdateFormHeaderP2();
            UpdateFormBodyP2();
        }
    }

    private bool GetData()
    {
        receInfs = _finder.GetReceInf(hpId, seikyuYm, seikyuType);
        hpInf = _finder.GetHpInf(hpId, seikyuYm);

        return (receInfs?.Count ?? 0) > 0;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
        {
            dictionary.Add(field, new CellModel(value));
        }
    }
}
