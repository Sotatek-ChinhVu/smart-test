using Helper.Common;
using Helper.Constants;
using Reporting.AccountingCardList.DB;
using Reporting.AccountingCardList.Model;
using Reporting.Calculate.Receipt.Constants;
using Reporting.Calculate.Receipt.ViewModels;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;

namespace Reporting.AccountingCardList.Service;

public class AccountingCardListCoReportService : IAccountingCardListCoReportService
{
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly ICoAccountingCardListFinder _finder;

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private CoAccountingCardListModel coModel;
    private List<CoAccountingCardListModel> coModels;
    private List<int> targetSinYms;
    int dataRowCount;
    int byomeiCharCount;
    int dataCharCount;
    private DateTime printoutDateTime;
    int PrintPage;

    private List<(long PtId, int SinYm, int HokenId)> targets;
    private int hpId;
    private bool includeOutDrug;
    private long fromPtNum;
    private long toPtNum;
    private string kaName;
    private string tantoName;
    private string uketukeSbt;
    private string hoken;
    private int sinYm;
    private List<CoAccountingCardListPrintDataModel> printOutData;

    public AccountingCardListCoReportService(IReadRseReportFileService readRseReportFileService, ICoAccountingCardListFinder finder)
    {
        _readRseReportFileService = readRseReportFileService;
        _finder = finder;
        _singleFieldData = new();
        _extralData = new();
        _tableFieldData = new();
        coModels = new();
        coModel = new();
        targetSinYms = new();
    }

    private void MakePrintDataList(int sinYm)
    {
        #region sub function
        // 病名リストに追加
        void _addByomeiList(string sinId, string addByomei, string addStartDate, string addByomeiTenki, CoAccountingCardListModel data)
        {
            bool first = true;
            string wkline = addByomei;

            if (wkline != "")
            {
                while (wkline != "")
                {
                    CoAccountingCardListPrintDataModel addData = new CoAccountingCardListPrintDataModel(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu);

                    if (first)
                    {
                        addData.SinId = sinId;
                        first = false;
                    }

                    string tmp = wkline;
                    if (CIUtil.LenB(tmp) > _byomeiCharCount)
                    {
                        tmp = CIUtil.CiCopyStrWidth(tmp, 1, _byomeiCharCount);
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
        void _addDataList(string sinId, string addStr, string inout, string suryo, string tensu, string x, string count, CoAccountingCardListModel data)
        {
            bool firstLine = true;
            string wkline = addStr;

            if (addStr != "")
            {
                while (wkline != "")
                {
                    string tmp = wkline;
                    if (CIUtil.LenB(tmp) > _dataCharCount)
                    {
                        tmp = CIUtil.CiCopyStrWidth(wkline, 1, _dataCharCount);
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
        int i = 1;
        printOutData = new List<CoAccountingCardListPrintDataModel>();

        foreach (CoAccountingCardListModel data in coModels.FindAll(p => p.SinYm == sinYm))
        {
            bool first = true;

            data.PtByomeis?.ForEach(byomei =>
            {
                string sinid = "";
                if (first)
                {
                    sinid = "病名";
                    first = false;
                }

                _addByomeiList(sinid, byomei.Byomei, CIUtil.SDateToShowSDate(byomei.StartDate), byomei.Tenki, data);

            }
            );

            if (data.SinMeiVM != null)
            {
                int preSinId = 0;
                int syokei = 0;
                int gokei = 0;

                foreach (SinMeiDataModel sinMei in data.SinMeiVM.SinMei)
                {
                    string sinid = "";
                    string inout = "";
                    string suryo = "";
                    string tensu = "";
                    string x = "";
                    string count = "";

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

                    inout = "";
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
                        count = sinMei.Count().ToString();
                    }
                    _addDataList(sinid, sinMei.ItemName, inout, suryo, tensu, x, count, data);

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

                printOutData.AddRange(_appendBlankRows(data.PtNum, data.Name, data.Birthday, data.Age, data.Nissu));
            }
        }
    }

    List<CoAccountingCardListPrintDataModel> _appendBlankRows(long ptNum, string ptName, int birthday, int age, int nissu)
    {
        List<CoAccountingCardListPrintDataModel> addPrintOutData = new List<CoAccountingCardListPrintDataModel>();

        // 追加する行数を決定する
        int appendRowCount = _getRemainingLineCount();
        if (appendRowCount % _dataRowCount != 0)
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
    int _getRemainingLineCount()
    {
        return _dataRowCount - _getPrintedLineCount();
    }
    /// <summary>
    /// このページの印字済み行数
    /// 既に追加した行数 % 1ページの最大行数
    /// </summary>
    /// <returns></returns>
    int _getPrintedLineCount()
    {
        return printOutData.Count() % _dataRowCount;
    }

    private bool UpdateDrawForm(out bool hasNextPage)
    {
        bool _hasNextPage = true;
        #region SubMethod

        int UpdateFormHeader()
        {
            string _getStr(string str)
            {
                return string.IsNullOrEmpty(str) ? "全て" : str;
            }

            //HEAD印字
            //日付
            CoRep.SetFieldData("dfSinYm", $"{sinYm / 100}年{sinYm % 100:D2}月");
            CoRep.SetFieldData("dfKaName", _getStr(kaName));
            CoRep.SetFieldData("dfTantoName", _getStr(tantoName));
            CoRep.SetFieldData("dfHokenKbn", _getStr(hoken));
            CoRep.SetFieldData("dfUketukeSbt", _getStr(uketukeSbt));
            CoRep.SetFieldData("dfPrintDateTime", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
            CoRep.SetFieldData("dfPrintDateTimeW", CIUtil.SDateToShowWDate3(CIUtil.StrToIntDef(_printoutDateTime.ToString("yyyyMMdd"), 0)).Ymd + _printoutDateTime.ToString(" HH:mm"));
            CoRep.SetFieldData("dfPage", $"{PrintPage}");

            return 1;
        }

        int UpdateFormBody()
        {
            int dataIndex = (CurrentPage - 1) * _dataRowCount * 3;

            for (int listNo = 1; listNo <= 3; listNo++)
            {
                if (!_hasNextPage) break;
                for (short i = 0; i < _dataRowCount; i++)
                {
                    CoAccountingCardListPrintDataModel printData = printOutData[dataIndex];

                    if (i == 0)
                    {
                        CoRep.SetFieldData($"dfPtNum{listNo}", printData.PtNum);
                        CoRep.SetFieldData($"dfPtName{listNo}", printData.PtName);
                        CoRep.SetFieldData($"dfBirthday{listNo}", printData.Birthday);
                        CoRep.SetFieldData($"dfNissu{listNo}", printData.Nissu);
                    }

                    CoRep.ListText($"lsSinId{listNo}", 0, i, printData.SinId);
                    CoRep.ListText($"lsByomei{listNo}", 0, i, printData.Byomei);
                    CoRep.ListText($"lsStartDate{listNo}", 0, i, printData.StartDate);
                    CoRep.ListText($"lsTenki{listNo}", 0, i, printData.Tenki);

                    CoRep.ListText($"lsInOut{listNo}", 0, i, printData.InOut);
                    CoRep.ListText($"lsData{listNo}", 0, i, printData.Data);
                    CoRep.ListText($"lsSuryo{listNo}", 0, i, printData.Suryo);
                    CoRep.ListText($"lsTen{listNo}", 0, i, printData.Tensu);
                    CoRep.ListText($"lsX{listNo}", 0, i, printData.X);
                    CoRep.ListText($"lsCount{listNo}", 0, i, printData.Count);

                    #region セル装飾

                    // 行の四方位置を取得する


                    if (string.IsNullOrEmpty(printData.SinId) == false)
                    {
                        (long startX, long startY, long endX, long endY) = CoRep.GetListRowBounds($"lsLine{listNo}", i);

                        // 上に線を引く（ただし、先頭行の場合は引かない）
                        if (i != 0)
                        {
                            CoRep.DrawLine(startX, startY, endX, startY, 30);
                        }

                        if (printData.SinId == "合計")
                        {
                            if (i != _dataRowCount - 1)
                            {
                                CoRep.DrawLine(startX, endY, endX, endY, 30);
                            }
                        }
                    }
                    else if (!(string.IsNullOrEmpty(printData.X)) && printData.X == "点")
                    {
                        (long startX, long startY, long endX, long endY) = CoRep.GetListCellBounds($"lsLine{listNo}", 1, i);
                        if (i != 0)
                        {
                            CoRep.DrawLine(startX, startY, endX, startY, (long)Hos.CnDraw.Constants.ConLineStyle.Dot);
                        }
                    }
                    #endregion

                    dataIndex++;
                    if (dataIndex >= printOutData.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }
            }
            return dataIndex;
        }

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
            Log.WriteLogError(ModuleName, this, nameof(UpdateDrawForm), e);
            hasNextPage = _hasNextPage;
            return false;
        }

        hasNextPage = _hasNextPage;
        return true;
    }

    private List<CoAccountingCardListModel> GetData()
    {
        List<CoAccountingCardListModel> results = new();

        foreach ((long ptId, int sinYm, int hokenId) in targets)
        {
            // 会計情報
            List<CoKaikeiInfModel> kaikeiInfModels = _finder.FindKaikeiInf(hpId, ptId, sinYm, hokenId);

            // 患者情報 
            CoPtInfModel ptInfModel = _finder.FindPtInf(hpId, ptId, sinYm * 100 + 31);

            // 診療情報
            SinMeiViewModel sinMeiViewModel = new SinMeiViewModel(SinMeiMode.AccountingCard, includeOutDrug, hpId, ptId, sinYm, hokenId);

            // 病名
            List<CoPtByomeiModel> ptByomeiModels = _finder.FindPtByomei(hpId, ptId, sinYm * 100 + 1, sinYm * 100 + 31, hokenId);
            results.Add(new CoAccountingCardListModel(sinYm, ptInfModel, kaikeiInfModels, sinMeiViewModel, ptByomeiModels));
        }

        return results;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void GetRowCount()
    {
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsData", (int)CalculateTypeEnum.GetListColCount),
            new ObjectCalculate("lsData", (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Karte3, string.Empty, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        dataColCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsData" && item.typeInt == (int)CalculateTypeEnum.GetListColCount)?.result ?? 0;
        dataRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsData" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
    }
}
