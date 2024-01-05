using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3010.DB;
using Reporting.Statistics.Sta3010.Mapper;
using Reporting.Statistics.Sta3010.Models;

namespace Reporting.Statistics.Sta3010.Service;

public class Sta3010CoReportService : ISta3010CoReportService
{
    #region Constant
    private int maxRow = 40;

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SetKbn", "セット区分コード"),
            new PutColumn("SetKbnEdaNo", "セット区分枝番"),
            new PutColumn("SetKbnName", "セット区分名称"),
            new PutColumn("Level1", "階層１"),
            new PutColumn("Level2", "階層２"),
            new PutColumn("Level3", "階層３"),
            new PutColumn("SetCd", "セットコード"),
            new PutColumn("SetName", "セット名称"),
            new PutColumn("WeightKbnFmt", "体重別セット"),
            new PutColumn("Rp", "RP"),
            new PutColumn("RpNo", "RP番号"),
            new PutColumn("RpEdaNo", "RP番号枝番"),
            new PutColumn("OdrKouiKbnName", "行為区分"),
            new PutColumn("InOutKbnName", "院内院外"),
            new PutColumn("SikyuKbnName", "至急区分"),
            new PutColumn("SyohoSbtName", "処方種別"),
            new PutColumn("SanteiKbnName", "算定区分"),
            new PutColumn("TosekiKbnName", "透析区分"),
            new PutColumn("RowNo", "行番号"),
            new PutColumn("ItemCd", "診療行為コード"),
            new PutColumn("ItemName", "診療行為名称"),
            new PutColumn("Suryo", "数量"),
            new PutColumn("UnitName", "単位"),
            new PutColumn("SyohoKbnName", "処方せん記載区分"),
            new PutColumn("SyohoLimitKbnName", "処方せん記載制限区分"),
            new PutColumn("KensaItemCd", "検査項目コード"),
            new PutColumn("CenterItemCd", "外注検査項目コード"),
            new PutColumn("EndDateFmt", "有効期限"),
            new PutColumn("Expired", "期限切れ"),
            new PutColumn("RenNo", "連番")
        };
    #endregion

    #region Private properties

    private CoHpInfModel hpInf;

    private List<CoOdrSetModel> odrSets;
    private List<CoSta3010PrintData> printDatas;
    #endregion
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3010Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3010PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;

    public Sta3010CoReportService(ICoSta3010Finder finder, IReadRseReportFileService readRseReportFileService)
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

            //基準日
            SetFieldData("StandardDate", string.Format("基準日: {0}　",
                    printConf.StdDate > 0 ? CIUtil.SDateToShowSWDate(printConf.StdDate, 0, 1) : string.Empty));
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

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[ptIndex];
                string baseListName = string.Empty;

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta3010PrintData).GetProperty(colName).GetValue(printData);
                    AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                //5項目毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank ? lineCount + 1 : lineCount;

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

        UpdateFormHeader(); UpdateFormBody();
    }

    public CommonReportingRequestModel GetSta3010ReportingData(CoSta3010PrintConf printConf, int hpId, CoFileType outputFileType)
    {
        try
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

            return new Sta3010Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            //改ページ条件
            bool pbSetKbn = new int[] { printConf.PageBreak1 }.Contains(1);

            //ソート順
            odrSets = odrSets?.OrderBy(x => x.SetKbn)
                .ThenBy(x => x.SetKbnEdaNo)
                .ThenBy(x => x.Level1)
                .ThenBy(x => x.Level2)
                .ThenBy(x => x.Level3)
                .ThenBy(x => x.GroupKoui)
                .ThenBy(x => x.SortNo)
                .ThenBy(x => x.RpNo)
                .ThenBy(x => x.RpEdaNo)
                .ThenBy(x => x.RowNo)
                .ToList() ?? new();


            printDatas = new List<CoSta3010PrintData>();

            CoOdrSetModel preOdrSet = new CoOdrSetModel();
            int rowCnt = 0;

            foreach (var odrSet in odrSets)
            {
                CoSta3010PrintData printData = new CoSta3010PrintData();

                //改ページ
                if ((pbSetKbn && (preOdrSet?.SetKbn ?? -1) != odrSet.SetKbn && rowCnt > 0) || rowCnt >= maxRow)
                {
                    //空行追加
                    for (int i = rowCnt; i < maxRow; i++)
                    {
                        printDatas.Add(new CoSta3010PrintData(RowType.Brank));
                    }

                    rowCnt = 0;
                }

                #region 上の行と同じ値は省略
                string setKbnNameFmt = string.Empty;
                string setKbnEdaNoFmt = string.Empty;
                string level1Fmt = string.Empty;
                string level2Fmt = string.Empty;
                string level3Fmt = string.Empty;
                string setNameFmt = string.Empty;
                string RpFmt = string.Empty;

                if (rowCnt == 0)
                {
                    //１行目
                    setKbnNameFmt = odrSet.SetKbnName;
                    setKbnEdaNoFmt = odrSet.SetKbnEdaNoPlus1.ToString();
                    level1Fmt = odrSet.Level1 == 0 ? "-" : odrSet.Level1.ToString();
                    level2Fmt = odrSet.Level2 == 0 ? "-" : odrSet.Level2.ToString();
                    level3Fmt = odrSet.Level3 == 0 ? "-" : odrSet.Level3.ToString();
                    setNameFmt = odrSet.SetName;
                    if ((preOdrSet?.RpNo ?? -1) != odrSet.RpNo)
                    {
                        // ＲＰでブレイク
                        RpFmt = "*";
                    }
                }
                else if ((preOdrSet?.SetKbn ?? -1) != odrSet.SetKbn)
                {
                    // セット区分でブレイク
                    setKbnNameFmt = odrSet.SetKbnName;
                    setKbnEdaNoFmt = odrSet.SetKbnEdaNoPlus1.ToString();
                    level1Fmt = odrSet.Level1 == 0 ? "-" : odrSet.Level1.ToString();
                    level2Fmt = odrSet.Level2 == 0 ? "-" : odrSet.Level2.ToString();
                    level3Fmt = odrSet.Level3 == 0 ? "-" : odrSet.Level3.ToString();
                    setNameFmt = odrSet.SetName;
                    RpFmt = "*";
                }
                else if ((preOdrSet?.SetKbnEdaNo ?? -1) != odrSet.SetKbnEdaNo)
                {
                    // セット区分枝番でブレイク
                    setKbnEdaNoFmt = odrSet.SetKbnEdaNoPlus1.ToString();
                    level1Fmt = odrSet.Level1 == 0 ? "-" : odrSet.Level1.ToString();
                    level2Fmt = odrSet.Level2 == 0 ? "-" : odrSet.Level2.ToString();
                    level3Fmt = odrSet.Level3 == 0 ? "-" : odrSet.Level3.ToString();
                    setNameFmt = odrSet.SetName;
                    RpFmt = "*";
                }
                else if ((preOdrSet?.Level1 ?? -1) != odrSet.Level1)
                {
                    // レベル１でブレイク
                    level1Fmt = odrSet.Level1 == 0 ? "-" : odrSet.Level1.ToString();
                    level2Fmt = odrSet.Level2 == 0 ? "-" : odrSet.Level2.ToString();
                    level3Fmt = odrSet.Level3 == 0 ? "-" : odrSet.Level3.ToString();
                    setNameFmt = odrSet.SetName;
                    RpFmt = "*";
                }
                else if ((preOdrSet?.Level2 ?? -1) != odrSet.Level2)
                {
                    // レベル２でブレイク
                    level2Fmt = odrSet.Level2 == 0 ? "-" : odrSet.Level2.ToString();
                    level3Fmt = odrSet.Level3 == 0 ? "-" : odrSet.Level3.ToString();
                    setNameFmt = odrSet.SetName;
                    RpFmt = "*";
                }
                else if ((preOdrSet?.Level3 ?? -1) != odrSet.Level3)
                {
                    // レベル３でブレイク
                    level3Fmt = odrSet.Level3 == 0 ? "-" : odrSet.Level3.ToString();
                    setNameFmt = odrSet.SetName;
                    RpFmt = "*";
                }
                else if ((preOdrSet?.SetCd ?? -1) != odrSet.SetCd)
                {
                    // セットコードでブレイク
                    setNameFmt = odrSet.SetName;
                    RpFmt = "*";
                }
                else if ((preOdrSet?.RpNo ?? -1) != odrSet.RpNo)
                {
                    // ＲＰでブレイク
                    RpFmt = "*";
                }
                #endregion

                printData.SetKbn = odrSet.SetKbn;
                printData.SetKbnEdaNo = (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv) ? odrSet.SetKbnEdaNoPlus1.ToString() : setKbnEdaNoFmt;
                printData.SetKbnName = (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv) ? odrSet.SetKbnName.ToString() : setKbnNameFmt;
                printData.Level1 = (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv) ? odrSet.Level1.ToString() : level1Fmt;
                printData.Level2 = (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv) ? odrSet.Level2.ToString() : level2Fmt;
                printData.Level3 = (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv) ? odrSet.Level3.ToString() : level3Fmt;
                printData.SetCd = odrSet.SetCd;
                printData.SetName = (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv) ? odrSet.SetName : setNameFmt;
                printData.WeightKbn = odrSet.WeightKbn;
                printData.RenNo = printDatas.Count + 1;
                if (odrSet.ItemName != string.Empty)
                {
                    printData.Rp = RpFmt;
                }
                printData.RpNo = odrSet.RpNo > 0 ? odrSet.RpNo.ToString() : string.Empty;
                printData.RpEdaNo = odrSet.RpEdaNo > 0 ? odrSet.RpEdaNo.ToString() : string.Empty;
                printData.OdrKouiKbn = odrSet.OdrKouiKbn;
                if (odrSet.IsShowInOut)
                {
                    printData.InOutKbn = odrSet.InoutKbn;
                }
                if (odrSet.IsKensa)
                {
                    printData.SikyuKbn = odrSet.SikyuKbn;
                    printData.TosekiKbn = odrSet.TosekiKbn;
                }
                if (odrSet.IsDrug)
                {
                    printData.SyohoSbt = odrSet.SyohoSbt;
                }
                printData.SanteiKbn = odrSet.SanteiKbn;
                printData.RowNo = odrSet.RowNo > 0 ? odrSet.RowNo.ToString() : string.Empty;
                printData.ItemCd = odrSet.ItemCd;
                printData.ItemName = odrSet.ItemName ?? string.Empty;
                if (odrSet.Suryo != 0)
                {
                    printData.Suryo = odrSet.Suryo.ToString("#,0.###");
                }
                printData.UnitName = odrSet.UnitName;
                printData.DrugKbn = odrSet.DrugKbn;
                printData.SyohoKbn = odrSet.SyohoKbn;
                printData.SyohoLimitKbn = odrSet.SyohoLimitKbn;
                printData.KensaItemCd = odrSet.KensaItemCd;
                printData.CenterItemCd = odrSet.CenterItemCd;
                printData.EndDate = odrSet.EndDate;
                printData.Expired = odrSet.EndDate < printConf.StdDate ? "*" : string.Empty;

                printDatas.Add(printData);
                rowCnt++;

                preOdrSet = odrSet;
            }
        }

        hpInf = _finder.GetHpInf(hpId, CIUtil.DateTimeToInt(DateTime.Today));

        odrSets = _finder.GetOdrSet(hpId, printConf);
        if ((odrSets?.Count ?? 0) == 0) return false;

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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3010, fileName, new());
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

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3010, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3010PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        this.coFileType = coFileType;
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

        string RecordData(CoSta3010PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putColumns)
            {
                var value = typeof(CoSta3010PrintData).GetProperty(column.ColName).GetValue(csvData);
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
