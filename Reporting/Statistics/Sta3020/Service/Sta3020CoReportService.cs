using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3020.DB;
using Reporting.Statistics.Sta3020.Mapper;
using Reporting.Statistics.Sta3020.Models;

namespace Reporting.Statistics.Sta3020.Service
{
    public class Sta3020CoReportService : ISta3020CoReportService
    {
        #region Constant
        private int maxRow = 40;

        private List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SetKbn", "セット区分"),
            new PutColumn("SetKbnName", "セット区分名称"),
            new PutColumn("Level1", "階層１"),
            new PutColumn("Level2", "階層２"),
            new PutColumn("Level3", "階層３"),
            new PutColumn("Level4", "階層４"),
            new PutColumn("Level5", "階層５"),
            new PutColumn("SetCd", "セットコード"),
            new PutColumn("IsTitle", "タイトル"),
            new PutColumn("ItemCd", "診療行為コード"),
            new PutColumn("SetName", "診療行為名称"),
            new PutColumn("SelectType", "選択方式"),
            new PutColumn("SelectTypeName", "選択方式名称"),
            new PutColumn("Suryo", "数量"),
            new PutColumn("UnitName", "単位"),
            new PutColumn("KensaItemCd", "検査項目コード"),
            new PutColumn("CenterItemCd", "外注検査項目コード"),
            new PutColumn("EndDateFmt", "有効期限"),
            new PutColumn("Expired", "期限切れ"),
            new PutColumn("RenNo", "連番")
        };
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoSta3020Finder _sta3020Finder;
        private readonly IReadRseReportFileService _readRseReportFileService;
        private int hpId;
        private int _currentPage;
        private bool _hasNextPage;
        private string _rowCountFieldName = string.Empty;
        private CoHpInfModel _hpInf;

        private List<CoListSetModel> listSets;
        private List<CoSta3020PrintData> printDatas;


        private List<string> _objectRseList;
        private CoSta3020PrintConf _printConf;
        private CoFileType? coFileType;
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> _visibleFieldData = new Dictionary<string, bool>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        // private BackgroundWorker _backgroundWorker = null;
        #endregion

        public Sta3020CoReportService(ICoSta3020Finder sta3020Finder, IReadRseReportFileService readRseReportFileService)
        {
            _sta3020Finder = sta3020Finder;
            _readRseReportFileService = readRseReportFileService;
        }

        public CommonReportingRequestModel GetSta3020ReportingData(CoSta3020PrintConf printConf, int hpId)
        {
            try
            {
                this.hpId = hpId;
                _printConf = printConf;
                // get data to print
                GetFieldNameList();
                GetRowCount();

                if (GetData())
                {
                    _hasNextPage = true;

                    _currentPage = 1;

                    //印刷
                    while (_hasNextPage)
                    {
                        UpdateDrawForm();
                        _currentPage++;
                    }
                }

                return new Sta3020Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
            }
            finally
            {
                _sta3020Finder.ReleaseResource();
            }
        }

        private bool GetData()
        {
            void MakePrintData()
            {
                //改ページ条件
                bool pbSetKbn = new int[] { _printConf.PageBreak1 }.Contains(1);

                //ソート順
                listSets = listSets.OrderBy(x => x.SetKbn)
                    .ThenBy(x => x.Level1)
                    .ThenBy(x => x.Level2)
                    .ThenBy(x => x.Level3)
                    .ThenBy(x => x.Level4)
                    .ThenBy(x => x.Level5)
                    .ThenBy(x => x.SetCd)
                    .ToList();

                printDatas = new List<CoSta3020PrintData>();

                CoListSetModel preListSet = new CoListSetModel();
                int rowCnt = 0;

                foreach (var listSet in listSets)
                {
                    CoSta3020PrintData printData = new CoSta3020PrintData();
                    CoSta3020PrintData prePrintData = printDatas.Count >= 1 ? printDatas.Last() : new CoSta3020PrintData();

                    //改ページ
                    if ((pbSetKbn && (preListSet?.SetKbn ?? -1) != listSet.SetKbn && rowCnt > 0) || rowCnt >= maxRow)
                    {
                        //空行追加
                        for (int i = rowCnt; i < maxRow; i++)
                        {
                            printDatas.Add(new CoSta3020PrintData(RowType.Brank));
                        }

                        rowCnt = 0;
                    }

                    #region 上の行と同じ値は省略
                    string setKbnName = "";
                    string level1Fmt = "";
                    string level2Fmt = "";
                    string level3Fmt = "";
                    string level4Fmt = "";
                    string level5Fmt = "";

                    if (rowCnt == 0)
                    {
                        //１行目
                        setKbnName = listSet.SetKbnName;
                        level1Fmt = listSet.Level1 == 0 ? "-" : listSet.Level1.ToString();
                        level2Fmt = listSet.Level2 == 0 ? "-" : listSet.Level2.ToString();
                        level3Fmt = listSet.Level3 == 0 ? "-" : listSet.Level3.ToString();
                        level4Fmt = listSet.Level4 == 0 ? "-" : listSet.Level4.ToString();
                        level5Fmt = listSet.Level5 == 0 ? "-" : listSet.Level5.ToString();
                    }
                    else if ((preListSet?.SetKbn ?? -1) != listSet.SetKbn)
                    {
                        // セット区分でブレイク
                        setKbnName = listSet.SetKbnName;
                        level1Fmt = listSet.Level1 == 0 ? "-" : listSet.Level1.ToString();
                        level2Fmt = listSet.Level2 == 0 ? "-" : listSet.Level2.ToString();
                        level3Fmt = listSet.Level3 == 0 ? "-" : listSet.Level3.ToString();
                        level4Fmt = listSet.Level4 == 0 ? "-" : listSet.Level4.ToString();
                        level5Fmt = listSet.Level5 == 0 ? "-" : listSet.Level5.ToString();
                    }
                    else if ((preListSet?.Level1 ?? -1) != listSet.Level1)
                    {
                        // レベル１でブレイク
                        level1Fmt = listSet.Level1 == 0 ? "-" : listSet.Level1.ToString();
                        level2Fmt = listSet.Level2 == 0 ? "-" : listSet.Level2.ToString();
                        level3Fmt = listSet.Level3 == 0 ? "-" : listSet.Level3.ToString();
                        level4Fmt = listSet.Level4 == 0 ? "-" : listSet.Level4.ToString();
                        level5Fmt = listSet.Level5 == 0 ? "-" : listSet.Level5.ToString();
                    }
                    else if ((preListSet?.Level2 ?? -1) != listSet.Level2)
                    {
                        // レベル２でブレイク
                        level2Fmt = listSet.Level2 == 0 ? "-" : listSet.Level2.ToString();
                        level3Fmt = listSet.Level3 == 0 ? "-" : listSet.Level3.ToString();
                        level4Fmt = listSet.Level4 == 0 ? "-" : listSet.Level4.ToString();
                        level5Fmt = listSet.Level5 == 0 ? "-" : listSet.Level5.ToString();
                    }
                    else if ((preListSet?.Level3 ?? -1) != listSet.Level3)
                    {
                        // レベル３でブレイク
                        level3Fmt = listSet.Level3 == 0 ? "-" : listSet.Level3.ToString();
                        level4Fmt = listSet.Level4 == 0 ? "-" : listSet.Level4.ToString();
                        level5Fmt = listSet.Level5 == 0 ? "-" : listSet.Level5.ToString();
                    }
                    else if ((preListSet?.Level4 ?? -1) != listSet.Level4)
                    {
                        // レベル４でブレイク
                        level4Fmt = listSet.Level4 == 0 ? "-" : listSet.Level4.ToString();
                        level5Fmt = listSet.Level5 == 0 ? "-" : listSet.Level5.ToString();
                    }
                    else if ((preListSet?.Level5 ?? -1) != listSet.Level5)
                    {
                        // レベル５でブレイク
                        level5Fmt = listSet.Level5 == 0 ? "-" : listSet.Level5.ToString();
                    }
                    #endregion

                    printData.SetKbn = listSet.SetKbn;
                    printData.SetKbnName = coFileType == CoFileType.Csv ? listSet.SetKbnName : setKbnName;
                    printData.Level1 = coFileType == CoFileType.Csv ? listSet.Level1.ToString() : level1Fmt;
                    printData.Level2 = coFileType == CoFileType.Csv ? listSet.Level2.ToString() : level2Fmt;
                    printData.Level3 = coFileType == CoFileType.Csv ? listSet.Level3.ToString() : level3Fmt;
                    printData.Level4 = coFileType == CoFileType.Csv ? listSet.Level4.ToString() : level4Fmt;
                    printData.Level5 = coFileType == CoFileType.Csv ? listSet.Level5.ToString() : level5Fmt;
                    printData.SetCd = listSet.SetCd;
                    printData.ItemCd = listSet.ItemCd;
                    printData.SetName = listSet.SetName;
                    printData.IsTitle = listSet.IsTitle == 1 ? "◇" : "";
                    printData.SelectType = listSet.SelectType;
                    if (listSet.Suryo != 0)
                    {
                        printData.Suryo = listSet.Suryo.ToString("#,0.###");
                    }
                    printData.UnitName = listSet.UnitName;
                    printData.KensaItemCd = listSet.KensaItemCd;
                    printData.CenterItemCd = listSet.CenterItemCd;
                    printData.EndDate = listSet.EndDate;
                    printData.Expired = listSet.EndDate < _printConf.StdDate ? "*" : "";
                    printData.RenNo = printDatas.Where(x => x.RowType == RowType.Data).Count() + 1;

                    printDatas.Add(printData);
                    rowCnt++;

                    preListSet = listSet;
                }

            }

            _hpInf = _sta3020Finder.GetHpInf(hpId, CIUtil.DateTimeToInt(DateTime.Today));

            listSets = _sta3020Finder.GetListSet(hpId, _printConf);
            if ((listSets?.Count ?? 0) == 0) return false;

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }

        private bool UpdateDrawForm()
        {
            _hasNextPage = true;

            #region SubMethod
            #region Header
            int UpdateFormHeader()
            {
                //タイトル
                SetFieldData("Title", _printConf.ReportName);

                //医療機関名
                _extralData.Add("HeaderR_0_0_" + _currentPage, _hpInf.HpName);

                //作成日時
                _extralData.Add("HeaderR_0_1_" + _currentPage, CIUtil.SDateToShowSWDate(
                    CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
                ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");

                //ページ数
                int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
                _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);

                //基準日
                SetFieldData("StandardDate", string.Format("基準日: {0}　",
                        _printConf.StdDate > 0 ? CIUtil.SDateToShowSWDate(_printConf.StdDate, 0, 1) : ""));

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                int ptIndex = (_currentPage - 1) * maxRow;
                int lineCount = 0;
                int sptMaxRow = 5;

                //存在しているフィールドに絞り込み
                var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var printData = printDatas[ptIndex];
                    string baseListName = "";

                    Dictionary<string, CellModel> data = new();
                    //明細データ出力
                    foreach (var colName in existsCols)
                    {
                        var value = typeof(CoSta3020PrintData).GetProperty(colName).GetValue(printData);
                        AddListData(ref data, colName, value == null ? "" : value.ToString());

                        if (baseListName == "" && _objectRseList.Contains(colName))
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
                        string rowNoKey = rowNo + "_" + _currentPage;
                        _extralData.Add("baseListName_" + rowNoKey, baseListName);
                        _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                    }

                    _tableFieldData.Add(data);

                    ptIndex++;
                    if (ptIndex >= printDatas.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                return ptIndex;
            }
            #endregion
            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        #region get data java

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

        private void GetFieldNameList()
        {
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3020, "sta3020a.rse", new());
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _objectRseList = javaOutputData.objectNames;
        }

        private void GetRowCount()
        {
            _rowCountFieldName = putColumns.Find(p => _objectRseList.Contains(p.ColName)).ColName;
            List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3020, "sta3020a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
        }
        #endregion

        public CommonExcelReportingModel ExportCsv(CoSta3020PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
        {
            _printConf = printConf;
            this.coFileType = coFileType;
            this.hpId = hpId;
            string fileName = menuName + "_" + monthFrom + "_" + monthTo;
            List<string> retDatas = new List<string>();

            if (!GetData()) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

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

            string RecordData(CoSta3020PrintData csvData)
            {
                List<string> colDatas = new List<string>();

                foreach (var column in putColumns)
                {
                    var value = typeof(CoSta3020PrintData).GetProperty(column.ColName).GetValue(csvData);
                    colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
                }

                return string.Join(",", colDatas);
            }

            return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
        }
    }
}
