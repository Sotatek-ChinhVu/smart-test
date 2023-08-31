using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3040.DB;
using Reporting.Statistics.Sta3040.Mapper;
using Reporting.Statistics.Sta3040.Models;
using System.Globalization;

namespace Reporting.Statistics.Sta3040.Service;

public class Sta3040CoReportService : ISta3040CoReportService
{

    #region Constant
    private int maxRow = 40;

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SinYm", "診療年月"),
            new PutColumn("ItemCd", "診療行為コード"),
            new PutColumn("ReceName", "請求名称"),
            new PutColumn("Suryo", "数量"),
            new PutColumn("ReceUnitName", "レセ単位名称"),
            new PutColumn("Price", "薬価"),
            new PutColumn("Kbn", "区分"),
            new PutColumn("UnitName", "単位"),
            new PutColumn("ExistCnvVal", "換算有無"),
            new PutColumn("SuryoKaisu", "数量回数"),
            new PutColumn("TermVal", "単位換算値"),
            new PutColumn("CnvVal", "換算係数")
        };

    private readonly List<PutColumn> totalColumns = new List<PutColumn>
        {
            new PutColumn("TotalKbn", "集計区分"),
            new PutColumn("TotalCaption", "集計名称"),
            new PutColumn("TotalVal", "集計値")
        };
    #endregion

    #region Private properties
    private CoHpInfModel hpInf;

    private List<CoUsedDrugInf> usedDrugInfs;
    private List<CoSta3040PrintData> printDatas;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3040Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3040PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;

    public Sta3040CoReportService(ICoSta3040Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        hpInf = new();
        _singleFieldData = new();
        _extralData = new();
        _tableFieldData = new();
        objectRseList = new();
        printConf = new();
        printDatas = new();
        rowCountFieldName = string.Empty;
    }

    public CommonReportingRequestModel GetSta3040ReportingData(CoSta3040PrintConf printConf, int hpId, CoFileType outputFileType)
    {
        this.printConf = printConf;
        this.outputFileType = outputFileType;
        string formFileName = printConf.FormFileName;

        // get data to print
        GetFieldNameList(formFileName);
        GetRowCount(formFileName);

        if (GetData(hpId))
        {
            hasNextPage = true;
            currentPage = 1;

            //印刷
            while (hasNextPage)
            {
                UpdateDrawForm();
                currentPage++;
            }
        }

        return new Sta3040Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
    }

    private void UpdateDrawForm()
    {
        #region Header
        void UpdateFormHeader()
        {
            //タイトル
            SetFieldData("Title", printConf.ReportName);

            //医療機関名
            _extralData.Add("HeaderR_0_0_" + currentPage, hpInf.HpName);

            //作成日時
            _extralData.Add("HeaderR_0_1_" + currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");

            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);

            //期間
            SetFieldData("Range", string.Format("期間: {0}～{1}　",
                    printConf.FromYm > 0 ? CIUtil.SMonthToShowSWMonth(printConf.FromYm) : string.Empty,
                    printConf.ToYm > 0 ? CIUtil.SMonthToShowSWMonth(printConf.ToYm) : string.Empty));
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            if (printDatas == null || printDatas.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            int ptIndex = (currentPage - 1) * maxRow;
            int lineCount = 0;
            int sptMaxRow = 5;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();
            var totalexistsCols = totalColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();
            foreach (var totalexistsCol in totalexistsCols)
            {
                existsCols.Add(totalexistsCol);
            }

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[ptIndex];
                string baseListName = string.Empty;

                //明細と合計のデータ出力
                foreach (var colName in existsCols)
                {
                    if (colName != "SinYm" || rowNo == 0) //診療年月は1行目だけ印字する
                    {
                        var value = typeof(CoSta3040PrintData).GetProperty(colName).GetValue(printData);
                        AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);
                    }

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                //5項目毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank && printData.RowType != RowType.Total ? lineCount + 1 : lineCount;

                if (lineCount == sptMaxRow && rowNo != maxRow - 1)
                {
                    lineCount = 0;

                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    string rowNoKey = rowNo + "_" + currentPage;
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }

                _tableFieldData.Add(data);
                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
        }
        #endregion
        UpdateFormHeader();
        UpdateFormBody();
    }

    private struct CountData
    {
        public double Total1;
        public double Total2;
        public double TotalWhiteStar;
        public double Total3;
        public double TotalBlackStar;
        public double TotalEmpty;
        public double TotalBatsu;

        public void AddValue(CoSta3040PrintData printData)
        {
            switch (printData.Kbn)
            {
                case "1": Total1 += double.Parse(printData.Suryo ?? "0", NumberStyles.Any); break;
                case "2": Total2 += double.Parse(printData.Suryo ?? "0", NumberStyles.Any); break;
                case "☆": TotalWhiteStar += double.Parse(printData.Suryo ?? "0", NumberStyles.Any); break;
                case "3": Total3 += double.Parse(printData.Suryo ?? "0", NumberStyles.Any); break;
                case "★": TotalBlackStar += double.Parse(printData.Suryo ?? "0", NumberStyles.Any); break;
                case "×": TotalBatsu += double.Parse(printData.Suryo ?? "0", NumberStyles.Any); break;
                default: TotalEmpty += double.Parse(printData.Suryo ?? "0", NumberStyles.Any); break;
            }
        }

        public void Clear()
        {
            Total1 = 0;
            Total2 = 0;
            TotalWhiteStar = 0;
            Total3 = 0;
            TotalBlackStar = 0;
            TotalEmpty = 0;
            TotalBatsu = 0;
        }
    }

    private CountData total = new();
    private CountData subTotal = new();
    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            printDatas = new List<CoSta3040PrintData>();

            int rowCount = 0;
            int pgCount = 1;
            string preSinYm = string.Empty;
            bool pgBreak = false;

            total.Clear();
            subTotal.Clear();

            #region ソート順
            usedDrugInfs = usedDrugInfs?
                .OrderBy(s => s.SinYm)
                .ThenBy(s =>
                   printConf.SortOpt1 == 1 ? "0" :
                   printConf.SortOrder1 == 1 ? s.ReceName :
                   printConf.SortOrder1 == 2 ? s.ItemCd : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt1 == 0 ? "0" :
                   printConf.SortOrder1 == 1 ? s.ReceName :
                   printConf.SortOrder1 == 2 ? s.ItemCd : "0")
                .ThenBy(s =>
                   printConf.SortOpt2 == 1 ? "0" :
                   printConf.SortOrder2 == 1 ? s.ReceName :
                   printConf.SortOrder2 == 2 ? s.ItemCd : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt2 == 0 ? "0" :
                   printConf.SortOrder2 == 1 ? s.ReceName :
                   printConf.SortOrder2 == 2 ? s.ItemCd : "0")
                .ToList() ?? new();
            #endregion

            #region SubMethod
            void AddMeisaiRecord(CoUsedDrugInf tgtData)
            {
                CoSta3040PrintData printData = new CoSta3040PrintData
                {
                    SinYm = outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv ? tgtData.SinYm.ToString() : tgtData.FmtSinYm,
                    ItemCd = tgtData.ItemCd,
                    ReceName = tgtData.ReceName,
                    Suryo = tgtData.Suryo.ToString("#,0.000"),
                    ReceUnitName = tgtData.ReceUnitName,
                    Price = tgtData.Price.ToString("#,0.000"),
                    Kbn = tgtData.Kbn,
                    UnitName = tgtData.UnitName,
                    ExistCnvVal = tgtData.ExistCnvVal,
                    SuryoKaisu = tgtData.SuryoKaisu.ToString("#,0.000"),
                    TermVal = tgtData.TermVal.ToString("#,0.000"),
                    CnvVal = tgtData.CnvVal.ToString("#,0.000")
                };
                //明細行追加
                printDatas.Add(printData);

                //集計
                subTotal.AddValue(printData);
                total.AddValue(printData);
            }

            void AddTotalRecord(string totalKbn, ref CountData totalData, string fmtSinYm = "")
            {
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        SinYm = fmtSinYm,
                        TotalKbn = totalKbn,
                        TotalCaption = "1   ：後発医薬品がない先発医薬品",
                        TotalVal = totalData.Total1.ToString("#,0.000")
                    }
                );
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "2   ：後発医薬品がある先発医薬品（先発医薬品と後発医薬品で剤形や規格が同一でない場合等を含む）",
                        TotalVal = totalData.Total2.ToString("#,0.000")
                    }
                );
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "☆  ：後発医薬品がある先発医薬品で、後発医薬品と同額又は薬価が低いもの",
                        TotalVal = totalData.TotalWhiteStar.ToString("#,0.000")
                    }
                );
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "3   ：後発医薬品",
                        TotalVal = totalData.Total3.ToString("#,0.000")
                    }
                );
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "★  ：後発医薬品及び先発医薬品と同額又は薬価が高いもの",
                        TotalVal = totalData.TotalBlackStar.ToString("#,0.000")
                    }
                );
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "空欄：「各先発医薬品の後発医薬品の有無に関する情報」が空欄のもの",
                        TotalVal = totalData.TotalEmpty.ToString("#,0.000")
                    }
                );
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "×  ：一致する医薬品の情報がないもの",
                        TotalVal = totalData.TotalBatsu.ToString("#,0.000")
                    }
                );

                //空行を追加
                printDatas.Add(new CoSta3040PrintData(RowType.Brank));

                double kikakuTani1 = totalData.Total1 + totalData.Total2 + totalData.TotalWhiteStar
                        + totalData.Total3 + totalData.TotalBlackStar + totalData.TotalEmpty + totalData.TotalBatsu;
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "①  ：全医薬品の規格単位数量",
                        TotalVal = kikakuTani1.ToString("#,0.000")
                    }
                );
                double kikakuTani2 = totalData.Total2 + totalData.TotalWhiteStar + totalData.Total3 + totalData.TotalBlackStar;
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "②  ：後発医薬品あり先発医薬品及び後発医薬品の規格単位数量",
                        TotalVal = kikakuTani2.ToString("#,0.000")
                    }
                );
                double kikakuTani3 = totalData.Total3 + totalData.TotalBlackStar;
                printDatas.Add(
                    new CoSta3040PrintData(RowType.Total)
                    {
                        TotalCaption = "③  ：後発医薬品の規格単位数量",
                        TotalVal = kikakuTani3.ToString("#,0.000")
                    }
                );
                printDatas.Add(
                   new CoSta3040PrintData(RowType.Total)
                   {
                       TotalCaption = "④  ：カットオフ値の割合（②／①）",
                       TotalVal = (kikakuTani1 == 0 ? 0 : (kikakuTani2 / kikakuTani1 * 100)).ToString("#,###,###,##0.00％")
                   }
                );
                printDatas.Add(
                   new CoSta3040PrintData(RowType.Total)
                   {
                       TotalCaption = "⑤  ：後発医薬品の割合（③／②）",
                       TotalVal = (kikakuTani2 == 0 ? 0 : (kikakuTani3 / kikakuTani2 * 100)).ToString("#,###,###,##0.00％")
                   }
                );

                totalData.Clear();
            }
            #endregion

            foreach (var usedDrugInf in usedDrugInfs)
            {

                //改ページ条件
                pgBreak = false;
                if (usedDrugInf.FmtSinYm != preSinYm)
                {
                    pgBreak = rowCount > 0;
                }

                //改ページ
                if (rowCount == maxRow || pgBreak)
                {
                    for (int i = printDatas.Count; i < maxRow * pgCount; i++)
                    {
                        //空行を追加
                        printDatas.Add(new CoSta3040PrintData(RowType.Brank));
                    }
                    pgCount++;

                    if (pgBreak)
                    {
                        //診療年月ごとに小計を出力
                        AddTotalRecord("小計", ref subTotal, preSinYm);
                        for (int i = printDatas.Count; i < maxRow * pgCount; i++)
                        {
                            //空行を追加
                            printDatas.Add(new CoSta3040PrintData(RowType.Brank));
                        }
                        pgCount++;
                    }

                    rowCount = 0;
                }

                AddMeisaiRecord(usedDrugInf);
                rowCount++;
                preSinYm = usedDrugInf.FmtSinYm;

            }

            //最終ページ

            for (int i = printDatas.Count; i < maxRow * pgCount; i++)
            {
                //空行を追加
                printDatas.Add(new CoSta3040PrintData(RowType.Brank));
            }
            pgCount++;

            AddTotalRecord("小計", ref subTotal, preSinYm);

            //空行を追加
            printDatas.Add(new CoSta3040PrintData(RowType.Brank));

            AddTotalRecord("合計", ref total);
        }

        hpInf = _finder.GetHpInf(hpId, CIUtil.DateTimeToInt(DateTime.Today));

        usedDrugInfs = _finder.GetUsedDrugInfs(hpId, printConf);
        if ((usedDrugInfs?.Count ?? 0) == 0) return false;

        //印刷用データの作成
        MakePrintData();

        return printDatas.Count > 0;
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

    private void GetFieldNameList(string fileName)
    {
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3040, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        objectRseList = javaOutputData.objectNames;
    }

    private void GetRowCount(string fileName)
    {
        rowCountFieldName = putColumns.Find(p => objectRseList.Contains(p.ColName)).ColName;
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3040, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3040PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        this.coFileType = coFileType;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        List<string> retDatas = new List<string>();
        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        var csvDatas = printDatas.Where(p => p.RowType == RowType.Data).ToList();
        if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        //出力フィールド
        List<string> wrkTitles = putColumns.Select(p => p.JpName).ToList();
        List<string> wrkColumns = putColumns.Select(p => p.ColName).ToList();

        //タイトル行
        retDatas.Add("\"" + string.Join("\",\"", wrkTitles) + "\"");
        if (isPutColName)
        {
            retDatas.Add("\"" + string.Join("\",\"", wrkColumns) + "\"");
        }

        //データ
        int totalRow = csvDatas.Count;
        int rowOutputed = 0;
        foreach (var csvData in csvDatas)
        {
            retDatas.Add(RecordData(csvData));
            rowOutputed++;
        }

        string RecordData(CoSta3040PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putColumns)
            {

                var value = typeof(CoSta3040PrintData).GetProperty(column.ColName).GetValue(csvData);
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
