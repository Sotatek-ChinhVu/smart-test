using Helper.Common;
using Infrastructure.Interfaces;
using Reporting.AccountingCardList.DB;
using Reporting.AccountingCardList.Mapper;
using Reporting.AccountingCardList.Model;
using CalculateService.Interface;
using CalculateService.Receipt.Constants;
using CalculateService.Receipt.Models;
using CalculateService.Receipt.ViewModels;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;

namespace Reporting.AccountingCardList.Service;

public class AccountingCardListCoReportService : IAccountingCardListCoReportService
{
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly ICoAccountingCardListFinder _finder;
    private readonly IEmrLogger _emrLogger;
    private readonly ITenantProvider _tenantProvider;
    private readonly ISystemConfigProvider _systemConfigProvider;

    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private List<CoAccountingCardListModel> coModels;
    int dataRowCount;
    int byomeiCharCount;
    int dataCharCount;
    private DateTime printoutDateTime;
    int printPage;
    int currentPage;

    private List<TargetItem> targets;
    private int hpId;
    private bool includeOutDrug;
    private string kaName;
    private string tantoName;
    private string uketukeSbt;
    private string hoken;
    private int sinYm;
    private bool hasNextPage;
    private List<CoAccountingCardListPrintDataModel> printOutData;

    public AccountingCardListCoReportService(IReadRseReportFileService readRseReportFileService, ICoAccountingCardListFinder finder, ISystemConfigProvider systemConfigProvider, ITenantProvider tenantProvider, IEmrLogger emrLogger)
    {
        _readRseReportFileService = readRseReportFileService;
        _finder = finder;
        _systemConfigProvider = systemConfigProvider;
        _tenantProvider = tenantProvider;
        _emrLogger = emrLogger;
        _setFieldData = new();
        _listTextData = new();
        _extralData = new();
        targets = new();
        coModels = new();
        kaName = string.Empty;
        tantoName = string.Empty;
        uketukeSbt = string.Empty;
        hoken = string.Empty;
        printOutData = new();
    }

    public CommonReportingRequestModel GetAccountingCardListData(int hpId, List<TargetItem> targets, bool includeOutDrug, string kaName, string tantoName, string uketukeSbt, string hoken)
    {
        try
        {
            this.hpId = hpId;
            this.targets = targets;
            this.includeOutDrug = includeOutDrug;
            this.kaName = kaName;
            this.tantoName = tantoName;
            this.uketukeSbt = uketukeSbt;
            this.hoken = hoken;
            var targetSinYms = this.targets.GroupBy(p => p.SinYm).Select(p => p.Key).OrderBy(p => p).ToList();

            printPage = 1;
            coModels = GetData();
            if (coModels.Any())
            {
                GetRowCount();
                foreach (int sinYmItem in targetSinYms)
                {
                    //グリッドのパラメータ取得
                    printoutDateTime = CIUtil.GetJapanDateTimeNow();

                    // 診療明細データを作成する
                    MakePrintDataList(sinYmItem);

                    // 診療年月をメンバ変数へ渡しておく
                    sinYm = sinYmItem;

                    currentPage = 1;
                    hasNextPage = true;

                    while (hasNextPage)
                    {
                        UpdateDrawForm();
                        currentPage++;
                        printPage++;
                    }
                }
            }

            _extralData.Add("totalPage", (printPage - 1).ToString());
            _extralData.Add("dataRowCount", dataRowCount.ToString());
            return new CoAccountingCardListMapper(_setFieldData, _listTextData, _extralData).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
            _tenantProvider.DisposeDataContext();
        }
    }

    private void MakePrintDataList(int sinYm)
    {
        #region sub function
        // 病名リストに追加
        void AddByomeiList(string sinId, string addByomei, string addStartDate, string addByomeiTenki, CoAccountingCardListModel data)
        {
            bool first = true;
            string wkline = addByomei;

            if (wkline != string.Empty)
            {
                while (wkline != string.Empty)
                {
                    CoAccountingCardListPrintDataModel addData = new CoAccountingCardListPrintDataModel(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu);

                    if (first)
                    {
                        addData.SinId = sinId;
                        first = false;
                    }

                    string tmp = wkline;
                    if (CIUtil.LenB(tmp) > byomeiCharCount)
                    {
                        tmp = CIUtil.CiCopyStrWidth(tmp, 1, byomeiCharCount);
                    }

                    addData.Byomei = tmp;
                    printOutData.Add(addData);

                    wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                }

                printOutData.Last().StartDate = addStartDate;
                printOutData.Last().Tenki = addByomeiTenki;
            }
        }

        //摘要欄に追加
        void AddDataList(string sinId, string addStr, string inout, string suryo, string tensu, string x, string count, CoAccountingCardListModel data)
        {
            bool firstLine = true;
            string wkline = addStr;

            if (addStr != string.Empty)
            {
                while (wkline != string.Empty)
                {
                    string tmp = wkline;
                    if (CIUtil.LenB(tmp) > dataCharCount)
                    {
                        tmp = CIUtil.CiCopyStrWidth(wkline, 1, dataCharCount);
                    }

                    CoAccountingCardListPrintDataModel addData = new CoAccountingCardListPrintDataModel(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu);

                    if (firstLine)
                    {
                        addData.SinId = sinId;
                        addData.InOut = inout;
                        firstLine = false;
                    }

                    addData.Data = tmp;
                    printOutData.Add(addData);

                    wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));
                }

                printOutData.Last().Suryo = suryo;
                printOutData.Last().Tensu = tensu;
                printOutData.Last().X = x;
                printOutData.Last().Count = count;

            }
        }
        #endregion
        printOutData = new List<CoAccountingCardListPrintDataModel>();

        foreach (CoAccountingCardListModel data in coModels.FindAll(p => p.SinYm == sinYm))
        {
            bool first = true;

            data.PtByomeis?.ForEach(byomei =>
            {
                string sinid = string.Empty;
                if (first)
                {
                    sinid = "病名";
                    first = false;
                }

                AddByomeiList(sinid, byomei.Byomei, CIUtil.SDateToShowSDate(byomei.StartDate), byomei.Tenki, data);

            }
            );

            if (data.SinMeiVM != null)
            {
                int preSinId = 0;
                int syokei = 0;
                int gokei = 0;

                foreach (SinMeiDataModel sinMei in data.SinMeiVM.SinMei)
                {
                    string sinid = string.Empty;
                    string inout = string.Empty;
                    string suryo = string.Empty;
                    string tensu = string.Empty;
                    string x = string.Empty;
                    string count = string.Empty;

                    if (preSinId == 0)
                    {
                        preSinId = sinMei.SinId;
                        sinid = sinMei.SinId.ToString();
                    }
                    else if (sinMei.SinId > 0 && sinMei.SinId != preSinId)
                    {
                        CoAccountingCardListPrintDataModel addData = new CoAccountingCardListPrintDataModel(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu);

                        addData.Tensu = syokei.ToString();
                        addData.X = "点";

                        printOutData.Add(addData);

                        syokei = 0;

                        sinid = sinMei.SinId.ToString();
                        preSinId = sinMei.SinId;

                    }

                    inout = string.Empty;
                    if (sinMei.InOutKbn == 1)
                    {
                        inout = "★外";
                    }

                    if (sinMei.LastRowKbn == 1)
                    {
                        if (!string.IsNullOrEmpty(sinMei.UnitName))
                        {
                            suryo = sinMei.Suryo.ToString() + sinMei.UnitName;
                        }

                        if (sinMei.EnTenKbn == 0)
                        {
                            tensu = sinMei.Ten.ToString();
                            syokei += (int)sinMei.TotalTen;
                            gokei += (int)sinMei.TotalTen;
                        }
                        else
                        {
                            tensu = @"\" + sinMei.Kingaku.ToString();
                            syokei += (int)(sinMei.TotalKingaku / 10);
                            gokei += (int)(sinMei.TotalKingaku / 10);
                        }

                        x = "x";
                        count = sinMei.Count.ToString();
                    }
                    AddDataList(sinid, sinMei.ItemName, inout, suryo, tensu, x, count, data);

                }

                // 最後の小計
                printOutData.Add(new CoAccountingCardListPrintDataModel(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu)
                {
                    Tensu = syokei.ToString(),
                    X = "点"
                });

                // 最後に合計
                printOutData.Add(new CoAccountingCardListPrintDataModel(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu)
                {
                    SinId = "合計",
                    Tensu = gokei.ToString(),
                    X = "点"
                }
                );

                printOutData.AddRange(AppendBlankRows(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu));
            }
        }
    }

    List<CoAccountingCardListPrintDataModel> AppendBlankRows(long ptNum, string ptName, int birthday, int age, int nissu)
    {
        List<CoAccountingCardListPrintDataModel> addPrintOutData = new();

        // 追加する行数を決定する
        int appendRowCount = GetRemainingLineCount();
        if (appendRowCount % dataRowCount != 0)
        {
            for (int j = 0; j < appendRowCount; j++)
            {
                // 空行で埋める
                addPrintOutData.Add(new CoAccountingCardListPrintDataModel(ptNum, ptName, birthday, age, nissu));
                addPrintOutData.Last().IsBlank = true;
            }
        }

        return addPrintOutData;
    }

    /// <summary>
    /// このページに印字可能な残り行数
    /// 1ページの最大行数 - (既に追加した行数 % 1ページの最大行数)
    /// </summary>
    /// <returns></returns>
    int GetRemainingLineCount()
    {
        return dataRowCount - GetPrintedLineCount();
    }

    /// <summary>
    /// このページの印字済み行数
    /// 既に追加した行数 % 1ページの最大行数
    /// </summary>
    /// <returns></returns>
    int GetPrintedLineCount()
    {
        return printOutData.Count % dataRowCount;
    }

    private void UpdateDrawForm()
    {
        Dictionary<string, string> setFieldDataPerPage = new();
        List<ListTextObject> listDataPerPage = new();

        #region SubMethod
        void UpdateFormHeader()
        {
            string GetStr(string str)
            {
                return string.IsNullOrEmpty(str) ? "全て" : str;
            }

            //HEAD印字
            //日付
            setFieldDataPerPage.Add("dfSinYm", $"{sinYm / 100}年{sinYm % 100:D2}月");
            setFieldDataPerPage.Add("dfKaName", GetStr(kaName));
            setFieldDataPerPage.Add("dfTantoName", GetStr(tantoName));
            setFieldDataPerPage.Add("dfHokenKbn", GetStr(hoken));
            setFieldDataPerPage.Add("dfUketukeSbt", GetStr(uketukeSbt));
            setFieldDataPerPage.Add("dfPrintDateTime", printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
            setFieldDataPerPage.Add("dfPrintDateTimeW", CIUtil.SDateToShowWDate3(CIUtil.StrToIntDef(printoutDateTime.ToString("yyyyMMdd"), 0)).Ymd + printoutDateTime.ToString(" HH:mm"));
        }

        void UpdateFormBody()
        {
            int dataIndex = (currentPage - 1) * dataRowCount * 3;

            for (short listNo = 1; listNo <= 3; listNo++)
            {
                if (!hasNextPage) break;
                for (short i = 0; i < dataRowCount; i++)
                {
                    CoAccountingCardListPrintDataModel printData = printOutData[dataIndex];

                    if (i == 0)
                    {
                        setFieldDataPerPage.Add($"dfPtNum{listNo}", printData.PtNum);
                        setFieldDataPerPage.Add($"dfPtName{listNo}", printData.PtName);
                        setFieldDataPerPage.Add($"dfBirthday{listNo}", printData.Birthday);
                        setFieldDataPerPage.Add($"dfNissu{listNo}", printData.Nissu);
                    }

                    listDataPerPage.Add(new($"lsSinId{listNo}", 0, i, printData.SinId));
                    listDataPerPage.Add(new($"lsByomei{listNo}", 0, i, printData.Byomei));
                    listDataPerPage.Add(new($"lsStartDate{listNo}", 0, i, printData.StartDate));
                    listDataPerPage.Add(new($"lsTenki{listNo}", 0, i, printData.Tenki));

                    listDataPerPage.Add(new($"lsInOut{listNo}", 0, i, printData.InOut));
                    listDataPerPage.Add(new($"lsData{listNo}", 0, i, printData.Data));
                    listDataPerPage.Add(new($"lsSuryo{listNo}", 0, i, printData.Suryo));
                    listDataPerPage.Add(new($"lsTen{listNo}", 0, i, printData.Tensu));
                    listDataPerPage.Add(new($"lsX{listNo}", 0, i, printData.X));
                    listDataPerPage.Add(new($"lsCount{listNo}", 0, i, printData.Count));

                    #region セル装飾
                    // 行の四方位置を取得する
                    string rowKey = listNo + "_" + i + "_" + printPage;
                    if (!string.IsNullOrEmpty(printData.SinId))
                    {
                        // 上に線を引く（ただし、先頭行の場合は引かない）
                        if (i != 0)
                        {
                            _extralData.Add("GroupDraw1_" + rowKey, "true");
                        }
                        if (printData.SinId == "合計" && i != dataRowCount - 1)
                        {
                            _extralData.Add("GroupDraw2_" + rowKey, "true");
                        }
                    }
                    else if (!(string.IsNullOrEmpty(printData.X)) && printData.X == "点" && i != 0)
                    {
                        _extralData.Add("GroupDraw3_" + rowKey, "true");
                    }
                    #endregion

                    dataIndex++;
                    if (dataIndex >= printOutData.Count)
                    {
                        hasNextPage = false;
                        break;
                    }
                }
            }
        }
        #endregion

        UpdateFormHeader();
        UpdateFormBody();
        _listTextData.Add(printPage, listDataPerPage);
        _setFieldData.Add(printPage, setFieldDataPerPage);
    }

    private List<CoAccountingCardListModel> GetData()
    {
        List<CoAccountingCardListModel> results = new();

        foreach (var item in targets)
        {
            // 会計情報
            List<CoKaikeiInfModel> kaikeiInfModels = _finder.FindKaikeiInf(hpId, item.PtId, item.SinYm, item.HokenId);

            // 患者情報 
            CoPtInfModel ptInfModel = _finder.FindPtInf(hpId, item.PtId, item.SinYm * 100 + 31);

            // 診療情報
            SinMeiViewModel sinMeiViewModel = new SinMeiViewModel(SinMeiMode.AccountingCard, includeOutDrug, hpId, item.PtId, item.SinYm, item.HokenId, _tenantProvider, _systemConfigProvider, _emrLogger);

            // 病名
            List<CoPtByomeiModel> ptByomeiModels = _finder.FindPtByomei(hpId, item.PtId, item.SinYm * 100 + 1, item.SinYm * 100 + 31, item.HokenId);
            results.Add(new CoAccountingCardListModel(item.SinYm, ptInfModel, kaikeiInfModels, sinMeiViewModel, ptByomeiModels));
        }

        return results;
    }

    private void GetRowCount()
    {
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsSinId1", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsData1", (int)CalculateTypeEnum.GetFormatLength),
            new ObjectCalculate("lsByomei1", (int)CalculateTypeEnum.GetFormatLength),
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.AccountingCardList, string.Empty, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        dataRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsSinId1" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
        dataCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsData1" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result ?? 0;
        byomeiCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsByomei1" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result ?? 0;
    }
}
