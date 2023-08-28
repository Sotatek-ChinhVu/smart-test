using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3001.DB;
using Reporting.Statistics.Sta3001.Mapper;
using Reporting.Statistics.Sta3001.Models;

namespace Reporting.Statistics.Sta3001.Service
{
    public class Sta3001CoReportService : ISta3001CoReportService
    {
        #region Constant

        private List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("ItemCd", "コード"),
            new PutColumn("Name", "名称"),
            new PutColumn("KanaName1", "カナ１"),
            new PutColumn("KanaName2", "カナ２"),
            new PutColumn("KanaName3", "カナ３"),
            new PutColumn("KanaName4", "カナ４"),
            new PutColumn("KanaName5", "カナ５"),
            new PutColumn("KanaName6", "カナ６"),
            new PutColumn("KanaName7", "カナ７"),
            new PutColumn("Suryo", "既定数量"),
            new PutColumn("OdrUnitName", "基本単位"),
            new PutColumn("Price", "金額"),
            new PutColumn("ReceUnitName", "レセ単位"),
            new PutColumn("ReceUnitNameFmt", "レセ単位Ｆ"),
            new PutColumn("StartDate", "開始日"),
            new PutColumn("StartDateFmt", "開始日Ｆ"),
            new PutColumn("EndDate", "終了日"),
            new PutColumn("EndDateFmt", "終了日Ｆ"),
            new PutColumn("DrugKbn", "薬剤区分"),
            new PutColumn("DrugKbnSname", "薬剤区分略称"),
            new PutColumn("DrugKbnName", "薬剤区分名称"),
            new PutColumn("MadokuKbn", "麻毒区分"),
            new PutColumn("MadokuKbnSname", "麻毒区分略称"),
            new PutColumn("MadokuKbnName", "麻毒区分名称"),
            new PutColumn("KouseisinKbn", "向精神薬区分"),
            new PutColumn("KouseisinKbnSname", "向精神薬区分略称"),
            new PutColumn("KouseisinKbnName", "向精神薬区分名称"),
            new PutColumn("KohatuKbn", "後発区分"),
            new PutColumn("KohatuKbnName", "後発名称"),
            new PutColumn("IpnName", "一般名"),
            new PutColumn("ReceName", "レセ名称"),
            new PutColumn("YjCd", "YJコード")
        };

        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoSta3001Finder _sta3001Finder;

        private CoHpInfModel hpInf;

        private List<string> headerL;
        private List<CoSta3001PrintData> printDatas;
        private List<CoAdpDrugsModel> adpDrugs;
        private CoSta3001PrintConf _printConf;
        private CoFileType? coFileType;
        //private BackgroundWorker _backgroundWorker = null;

        private int HpId;
        private int maxRow = 30;
        private int _currentPage;
        private bool _hasNextPage;
        private string _rowCountFieldName = string.Empty;
        private List<string> _objectRseList;
        private readonly IReadRseReportFileService _readRseReportFileService;
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        #endregion

        public Sta3001CoReportService(ICoSta3001Finder sta3001Finder, IReadRseReportFileService readRseReportFileService)
        {
            _sta3001Finder = sta3001Finder;
            _readRseReportFileService = readRseReportFileService;
        }

        public CommonReportingRequestModel GetSta3001ReportingData(CoSta3001PrintConf printConf, int hpId)
        {
            HpId = hpId;
            _printConf = printConf;
            // get data to print
            GetFieldNameList();
            GetColRowCount();

            _hasNextPage = true;

            _currentPage = 1;
            var getData = GetData();
            if (getData)
            {
                while (_hasNextPage && getData)
                {
                    UpdateDrawForm();
                    _currentPage++;
                }
            }
            //印刷

            return new Sta3001Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
        }
        #region Get Data
        private bool GetData()
        {
            void MakePrintData()
            {
                printDatas = new List<CoSta3001PrintData>();
                headerL = new List<string>();

                int rowCount = 0;
                int preDrugKbn = 0;
                bool pgBreak = false;
                bool isFirstItem = true;
                string curDrgKbnName = "";

                //改ページ条件
                bool pbDrugKbn = new int[] { _printConf.PageBreak1 }.Contains(1);

                #region ソート順
                adpDrugs = adpDrugs
                    .OrderBy(s => pbDrugKbn ? s.DrugKbn : 0)
                    .ThenBy(s =>
                       _printConf.SortOpt1 == 1 ? "0" :
                       _printConf.SortOrder1 == 1 ? s.DrugKbn.ToString() :
                       _printConf.SortOrder1 == 2 ? s.Name :
                       _printConf.SortOrder1 == 3 ? s.ItemCd : "0")
                    .ThenByDescending(s =>
                       _printConf.SortOpt1 == 0 ? "0" :
                       _printConf.SortOrder1 == 1 ? s.DrugKbn.ToString() :
                       _printConf.SortOrder1 == 2 ? s.Name :
                       _printConf.SortOrder1 == 3 ? s.ItemCd : "0")
                    .ThenBy(s =>
                       _printConf.SortOpt2 == 1 ? "0" :
                       _printConf.SortOrder2 == 1 ? s.DrugKbn.ToString() :
                       _printConf.SortOrder2 == 2 ? s.Name :
                       _printConf.SortOrder2 == 3 ? s.ItemCd : "0")
                    .ThenByDescending(s =>
                       _printConf.SortOpt2 == 0 ? "0" :
                       _printConf.SortOrder2 == 1 ? s.DrugKbn.ToString() :
                       _printConf.SortOrder2 == 2 ? s.Name :
                       _printConf.SortOrder2 == 3 ? s.ItemCd : "0")
                    .ThenBy(s =>
                       _printConf.SortOpt3 == 1 ? "0" :
                       _printConf.SortOrder3 == 1 ? s.DrugKbn.ToString() :
                       _printConf.SortOrder3 == 2 ? s.Name :
                       _printConf.SortOrder3 == 3 ? s.ItemCd : "0")
                    .ThenByDescending(s =>
                       _printConf.SortOpt3 == 0 ? "0" :
                       _printConf.SortOrder3 == 1 ? s.DrugKbn.ToString() :
                       _printConf.SortOrder3 == 2 ? s.Name :
                       _printConf.SortOrder3 == 3 ? s.ItemCd : "0")
                    .ToList();
                #endregion

                #region SubMethod
                void AddAllCols(CoAdpDrugsModel tgtDrgInf)
                {
                    printDatas.Add(
                        new CoSta3001PrintData(RowType.Data)
                        {
                            ItemCd = tgtDrgInf.ItemCd,
                            KanaName1 = tgtDrgInf.KanaName1,
                            KanaName2 = tgtDrgInf.KanaName2,
                            KanaName3 = tgtDrgInf.KanaName3,
                            KanaName4 = tgtDrgInf.KanaName4,
                            KanaName5 = tgtDrgInf.KanaName5,
                            KanaName6 = tgtDrgInf.KanaName6,
                            KanaName7 = tgtDrgInf.KanaName7,
                            Suryo = tgtDrgInf.DefaultVal == 0 ? string.Empty : tgtDrgInf.DefaultVal.ToString("#,0.###"),
                            Price = tgtDrgInf.Price.ToString("#,0.###"),
                            StartDate = tgtDrgInf.StartDate,
                            EndDate = tgtDrgInf.EndDate,
                            DrugKbn = tgtDrgInf.DrugKbn,
                            MadokuKbn = tgtDrgInf.MadokuKbn,
                            KohatuKbn = tgtDrgInf.KohatuKbn,
                            Name = tgtDrgInf.Name,
                            OdrUnitName = tgtDrgInf.OdrUnitName,
                            ReceUnitName = tgtDrgInf.ReceUnitName,
                            KouseisinKbn = tgtDrgInf.KouseisinKbn,
                            YjCd = tgtDrgInf.YjCd,
                            IpnName = tgtDrgInf.IpnName,
                            ReceName = tgtDrgInf.ReceName
                        }
                    );
                    rowCount++;
                }

                void AddLine1(CoAdpDrugsModel tgtDrgInf, out string drgKbnName)
                {
                    printDatas.Add(
                        new CoSta3001PrintData(RowType.Data)
                        {
                            ItemCd = tgtDrgInf.ItemCd,
                            KanaName1 = tgtDrgInf.KanaName1,
                            KanaName2 = tgtDrgInf.KanaName2,
                            KanaName3 = tgtDrgInf.KanaName3,
                            KanaName4 = tgtDrgInf.KanaName4,
                            KanaName5 = tgtDrgInf.KanaName5,
                            KanaName6 = tgtDrgInf.KanaName6,
                            KanaName7 = tgtDrgInf.KanaName7,
                            Suryo = tgtDrgInf.DefaultVal == 0 ? string.Empty : tgtDrgInf.DefaultVal.ToString("#,0.###"),
                            Price = tgtDrgInf.Price.ToString("#,0.###"),
                            StartDate = tgtDrgInf.StartDate,
                            DrugKbn = tgtDrgInf.DrugKbn,
                            MadokuKbn = tgtDrgInf.MadokuKbn,
                            KohatuKbn = tgtDrgInf.KohatuKbn
                        }
                    );
                    rowCount++;
                    drgKbnName = printDatas.Last().DrugKbnName;
                }

                void AddLine2(CoAdpDrugsModel tgtDrgInf)
                {
                    printDatas.Add(
                        new CoSta3001PrintData(RowType.Data)
                        {
                            Name = tgtDrgInf.Name,
                            OdrUnitName = tgtDrgInf.OdrUnitName,
                            ReceUnitName = tgtDrgInf.ReceUnitName,
                            EndDate = tgtDrgInf.EndDate,
                            KouseisinKbn = tgtDrgInf.KouseisinKbn,
                            YjCd = tgtDrgInf.YjCd
                        }
                    );
                    rowCount++;
                }

                void AddLine3(CoAdpDrugsModel tgtDrgInf)
                {
                    if (_printConf.IpnNameOpt > 0 || _printConf.ReceNameOpt > 0)
                    {
                        printDatas.Add(
                            new CoSta3001PrintData(RowType.Data)
                            {
                                IpnName = _printConf.IpnNameOpt > 0 ? tgtDrgInf.IpnName : "",
                                ReceName = _printConf.ReceNameOpt > 0 ? tgtDrgInf.ReceName : ""
                            }
                        );
                        rowCount++;

                    }
                }
                #endregion

                foreach (var adpDrug in adpDrugs)
                {
                    //明細
                    if (coFileType == CoFileType.Csv)
                    {
                        //CSV
                        AddAllCols(adpDrug);
                    }
                    else
                    {

                        //改ページ条件
                        pgBreak = false;
                        if (pbDrugKbn && adpDrug.DrugKbn != preDrugKbn)
                        {
                            pgBreak = rowCount > 0;
                            preDrugKbn = adpDrug.DrugKbn;
                        }

                        //改ページ
                        if (rowCount == maxRow || pgBreak)
                        {
                            for (int i = rowCount; i < maxRow; i++)
                            {
                                //空行を追加
                                printDatas.Add(new CoSta3001PrintData(RowType.Brank));
                            }
                            rowCount = 0;
                            isFirstItem = true;
                        }

                        AddLine1(adpDrug, out curDrgKbnName);
                        AddLine2(adpDrug);
                        AddLine3(adpDrug);

                        //ヘッダー
                        if (isFirstItem && pbDrugKbn)
                        {
                            headerL.Add(curDrgKbnName);
                        }

                        isFirstItem = false;
                    }
                }

            }

            hpInf = _sta3001Finder.GetHpInf(HpId, CIUtil.DateTimeToInt(DateTime.Today));

            adpDrugs = _sta3001Finder.GetAdpDrugs(HpId, _printConf);
            if ((adpDrugs?.Count ?? 0) == 0) return false;

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }
        #endregion

        #region Update Draw Form
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
                _extralData.Add("HeaderR_0_0_" + _currentPage, hpInf.HpName);

                //作成日時
                _extralData.Add("HeaderR_0_1_" + _currentPage, CIUtil.SDateToShowSWDate(
                    CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
                ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");

                //ページ数
                int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
                _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);

                //改ページ条件
                _extralData.Add("HeaderL_0_2_" + _currentPage, headerL.Count >= _currentPage ? headerL[_currentPage - 1] : "");

                //期間
                string range = "";
                if (_printConf.StartDateFrom > 0 || _printConf.StartDateTo > 0)
                {
                    range = string.Format("開始日: {0} ～ {1}　",
                        _printConf.StartDateFrom > 0 ? CIUtil.SDateToShowSWDate(_printConf.StartDateFrom, 0, 1) : "",
                        _printConf.StartDateTo > 0 ? CIUtil.SDateToShowSWDate(_printConf.StartDateTo, 0, 1) : "");
                }
                if (_printConf.EndDateFrom > 0 || _printConf.EndDateTo > 0)
                {
                    range = range + string.Format("終了日: {0} ～ {1}",
                        _printConf.EndDateFrom > 0 ? CIUtil.SDateToShowSWDate(_printConf.EndDateFrom, 0, 1) : "",
                        _printConf.EndDateTo > 0 ? CIUtil.SDateToShowSWDate(_printConf.EndDateTo, 0, 1) : "");
                }

                SetFieldData("Range", range);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                int ptIndex = (_currentPage - 1) * maxRow;
                int lineCount = 0;

                //１項目の行数に合わせて区切り線を調整
                int sptMaxRow = 5;
                sptMaxRow = (_printConf.IpnNameOpt > 0 || _printConf.ReceNameOpt > 0) ? sptMaxRow * 3 : sptMaxRow * 2;


                //存在しているフィールドに絞り込み
                var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    Dictionary<string, CellModel> data = new();

                    var printData = printDatas[ptIndex];
                    string baseListName = "";

                    //明細データ出力
                    foreach (var colName in existsCols)
                    {
                        var value = typeof(CoSta3001PrintData).GetProperty(colName).GetValue(printData);
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
        #endregion

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
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3001, "sta3001a.rse", new());
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _objectRseList = javaOutputData.objectNames;
        }

        private void GetColRowCount()
        {
            _rowCountFieldName = putColumns.Find(p => _objectRseList.Contains(p.ColName)).ColName;
            List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3001, "sta3001a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
        }

        public CommonExcelReportingModel ExportCsv(CoSta3001PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
        {
            _printConf = printConf;
            HpId = hpId;
            this.coFileType = coFileType;
            string fileName = menuName + "_" + monthFrom + "_" + monthTo;
            List<string> retDatas = new List<string>();
            if (!GetData()) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

            var csvDatas = printDatas.Where(p => p.RowType == RowType.Data).ToList();
            if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

            //出力フィールド
            List<string> wrkTitles = putColumns.Select(p => p.JpName).ToList();
            List<string> wrkColumns = putColumns.Select(p => p.CsvColName).ToList();

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

            string RecordData(CoSta3001PrintData csvData)
            {
                List<string> colDatas = new List<string>();

                foreach (var column in putColumns)
                {
                    var value = typeof(CoSta3001PrintData).GetProperty(column.CsvColName).GetValue(csvData);
                    colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
                }

                return string.Join(",", colDatas);
            }

            return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
        }
        #endregion
    }
}
