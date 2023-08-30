using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1010.DB;
using Reporting.Statistics.Sta1010.Mapper;
using Reporting.Statistics.Sta1010.Models;
using System.Globalization;

namespace Reporting.Statistics.Sta1010.Service;

public class Sta1010CoReportService : ISta1010CoReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta1010Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private CoSta1010PrintConf _printConf;
    private List<CoSta1010PrintData> _printDatas;
    private List<string> _headerL1;
    private List<CoSyunoInfModel>? _syunoInfs;
    private CoHpInfModel _hpInf;
    private CoFileType? coFileType;
    private List<PutColumn> putCurColumns = new List<PutColumn>();

    private List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行"),
            new PutColumn("TotalCount", "合計件数"),
        };

    public Sta1010CoReportService(ICoSta1010Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _singleFieldData = new();
        _tableFieldData = new();
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        _printDatas = new();
        _headerL1 = new();
        _syunoInfs = new();
        _hpInf = new();
        _extralData = new();
        _objectRseList = new();
        _printConf = new();
    }


    #region Constant
    private int maxRow = 43;

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("KaId", "診療科ID", false),
            new PutColumn("KaSname", "診療科略称", false),
            new PutColumn("TantoId", "担当医ID", false),
            new PutColumn("TantoSname", "担当医略称", false),
            new PutColumn("PtNum", "患者番号", false),
            new PutColumn("PtKanaName", "カナ氏名", false),
            new PutColumn("PtName", "氏名", false),
            new PutColumn("SexCd", "性別コード", false),
            new PutColumn("Sex", "性別", false),
            new PutColumn("BirthDayFmt", "生年月日", false, "BirthDay"),
            new PutColumn("Age", "年齢", false),
            new PutColumn("HomePost", "郵便番号", false),
            new PutColumn("HomeAddress", "住所", false),
            new PutColumn("Tel1", "電話番号１", false),
            new PutColumn("Tel2", "電話番号２", false),
            new PutColumn("RenrakuTel", "緊急連絡先電話番号", false),
            new PutColumn("SinDateFmt", "診察日", false, "SinDate"),
            new PutColumn("RaiinNo", "来院番号", false),
            new PutColumn("HokenSbt", "保険種別", false),
            new PutColumn("Syosaisin", "初再診", false),
            new PutColumn("SeikyuGaku", "請求額"),
            new PutColumn("OldSeikyuGaku", "請求額(旧)"),
            new PutColumn("NewSeikyuGaku", "請求額(新)"),
            new PutColumn("AdjustFutan", "調整額"),
            new PutColumn("OldAdjustFutan", "調整額(旧)"),
            new PutColumn("NewAdjustFutan", "調整額(新)"),
            new PutColumn("TotalSeikyuGaku", "合計請求額"),
            new PutColumn("NyukinGaku", "入金額"),
            new PutColumn("MisyuGaku", "未収額"),
            new PutColumn("NyukinCmt", "コメント", false),
            new PutColumn("LastVisitDateFmt", "最終来院日", false, "LastVisitDate"),
            new PutColumn("MisyuKbn", "未収区分", false)
        };
    #endregion

    #region Printer method
    private struct CountData
    {
        public int Count;
        public int SeikyuGaku;
        public int OldSeikyuGaku;
        public int NewSeikyuGaku;
        public int AdjustFutan;
        public int OldAdjustFutan;
        public int NewAdjustFutan;
        public int NyukinGaku;
        public int MisyuGaku;
        public int TotalSeikyuGaku;

        public void AddValue(CoSta1010PrintData printData)
        {
            Count++;
            SeikyuGaku += int.Parse(printData.SeikyuGaku ?? "0", NumberStyles.Any);
            OldSeikyuGaku += int.Parse(printData.OldSeikyuGaku ?? "0", NumberStyles.Any);
            NewSeikyuGaku += int.Parse(printData.NewSeikyuGaku ?? "0", NumberStyles.Any);
            AdjustFutan += int.Parse(printData.AdjustFutan ?? "0", NumberStyles.Any);
            OldAdjustFutan += int.Parse(printData.OldAdjustFutan ?? "0", NumberStyles.Any);
            NewAdjustFutan += int.Parse(printData.NewAdjustFutan ?? "0", NumberStyles.Any);
            NyukinGaku += int.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any);
            MisyuGaku += int.Parse(printData.MisyuGaku ?? "0", NumberStyles.Any);
            TotalSeikyuGaku += int.Parse(printData.TotalSeikyuGaku ?? "0", NumberStyles.Any);
        }

        public void Clear()
        {
            Count = 0;
            SeikyuGaku = 0;
            OldSeikyuGaku = 0;
            NewSeikyuGaku = 0;
            AdjustFutan = 0;
            OldAdjustFutan = 0;
            NewAdjustFutan = 0;
            NyukinGaku = 0;
            MisyuGaku = 0;
            TotalSeikyuGaku = 0;
        }
    }

    private CountData total = new();
    private CountData subTotal = new();
    private CountData ptTotal = new();

    #endregion

    private int _currentPage;
    private string _rowCountFieldName = string.Empty;
    private List<string> _objectRseList;
    private bool _hasNextPage;

    public CommonReportingRequestModel GetSta1010ReportingData(CoSta1010PrintConf printConf, int hpId)
    {
        _printConf = printConf;
        string formFileName = _printConf.FormFileName;

        // get data to print
        GetFieldNameList(formFileName);
        GetRowCount(formFileName);
        if (GetData(hpId))
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

        return new Sta1010Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName, formFileName).GetData();
    }

    private void UpdateDrawForm()
    {
        if (_printDatas.Count == 0)
        {
            _hasNextPage = false;
            return;
        }
        #region Header
        void UpdateFormHeader()
        {
            //タイトル
            SetFieldData("Title", _printConf.ReportName);
            //医療機関名
            //医療機関名
            _extralData.Add("HeaderR_0_0_" + _currentPage, _hpInf.HpName);
            //作成日時
            _extralData.Add("HeaderR_0_1_" + _currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");
            //ページ数
            int totalPage = (int)Math.Ceiling((double)_printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);
            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + _currentPage, _headerL1.Count >= _currentPage ? _headerL1[_currentPage - 1] : string.Empty);

            //期間
            SetFieldData("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(_printConf.StartSinDate, 0, 1),
                    CIUtil.SDateToShowSWDate(_printConf.EndSinDate, 0, 1)
                )
            );
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            int ptIndex = (_currentPage - 1) * maxRow;
            int lineCount = 0;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = _printDatas[ptIndex];
                string baseListName = string.Empty;

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta1010PrintData).GetProperty(colName).GetValue(printData);

                    string valueInput = value?.ToString() ?? string.Empty;

                    AddListData(ref data, colName, valueInput);

                    if (baseListName == string.Empty && _objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                //合計行キャプションと件数
                AddListData(ref data, "TotalCaption", printData.TotalCaption);
                AddListData(ref data, "TotalCount", printData.TotalCount);

                //5行毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank ? lineCount + 1 : lineCount;
                string rowNoKey = rowNo + "_" + _currentPage;
                _extralData.Add("lineCount_" + rowNoKey, lineCount.ToString());

                if (lineCount == 5)
                {
                    lineCount = 0;
                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }

                _tableFieldData.Add(data);
                ptIndex++;
                if (ptIndex >= _printDatas.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }
        }
        #endregion
        UpdateFormHeader();
        UpdateFormBody();
    }
     
    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            _printDatas = new List<CoSta1010PrintData>();
            _headerL1 = new List<string>();
            int pageCount = 1;

            //改ページ条件
            bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(1);
            bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(2);

            #region ソート順
            _syunoInfs =
                _syunoInfs?
                    .OrderBy(s => pbKaId ? s.KaId : 0)
                    .ThenBy(s => pbTantoId ? s.TantoId : 0)
                    .ThenBy(s =>
                        _printConf.SortOpt1 == 1 ? "0" :
                        _printConf.SortOrder1 == 1 ? s.PtKanaName :
                        _printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder1 == 3 ? s.MisyuGaku.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder1 == 4 ? s.LastVisitDate.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder1 == 5 ? s.SinDate.ToString().PadLeft(10, '0') : "0")
                    .ThenByDescending(s =>
                        _printConf.SortOpt1 == 0 ? "0" :
                        _printConf.SortOrder1 == 1 ? s.PtKanaName :
                        _printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder1 == 3 ? s.MisyuGaku.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder1 == 4 ? s.LastVisitDate.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder1 == 5 ? s.SinDate.ToString().PadLeft(10, '0') : "0")
                    .ThenBy(s =>
                        _printConf.SortOpt2 == 1 ? "0" :
                        _printConf.SortOrder2 == 1 ? s.PtKanaName :
                        _printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder2 == 3 ? s.MisyuGaku.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder2 == 4 ? s.LastVisitDate.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder2 == 5 ? s.SinDate.ToString().PadLeft(10, '0') : "0")
                    .ThenByDescending(s =>
                        _printConf.SortOpt2 == 0 ? "0" :
                        _printConf.SortOrder2 == 1 ? s.PtKanaName :
                        _printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder2 == 3 ? s.MisyuGaku.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder2 == 4 ? s.LastVisitDate.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder2 == 5 ? s.SinDate.ToString().PadLeft(10, '0') : "0")
                    .ThenBy(s =>
                        _printConf.SortOpt3 == 1 ? "0" :
                        _printConf.SortOrder3 == 1 ? s.PtKanaName :
                        _printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder3 == 3 ? s.MisyuGaku.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder3 == 4 ? s.LastVisitDate.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder3 == 5 ? s.SinDate.ToString().PadLeft(10, '0') : "0")
                    .ThenByDescending(s =>
                        _printConf.SortOpt3 == 0 ? "0" :
                        _printConf.SortOrder3 == 1 ? s.PtKanaName :
                        _printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder3 == 3 ? s.MisyuGaku.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder3 == 4 ? s.LastVisitDate.ToString().PadLeft(10, '0') :
                        _printConf.SortOrder3 == 5 ? s.SinDate.ToString().PadLeft(10, '0') : "0")
                    .ThenBy(s => s.PtNum)
                    .ToList();
            #endregion

            foreach (var syunoInf in _syunoInfs ?? new())
            {
                CoSta1010PrintData printData = new();
                CoSta1010PrintData prePrintData = _printDatas.Count >= 1 ? _printDatas.Last() : new();

                //改ページ条件
                bool pageBreak =
                    (pbKaId && syunoInf.KaId != prePrintData.KaId && prePrintData.KaId > 0) ||
                    (pbTantoId && syunoInf.TantoId != prePrintData.TantoId && prePrintData.TantoId > 0);

                if (prePrintData.PtNumKey != syunoInf.PtNum)
                {
                    if (ptTotal.Count >= 2)
                    {
                        AddTotalRecord("◆患者計", ref ptTotal);
                        //空行を追加
                        _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
                    }
                    ptTotal.Clear();
                }

                if (pageBreak)
                {
                    //空行を追加
                    _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
                    _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
                    //小計
                    AddTotalRecord("◆小計", ref subTotal);
                    //改ページ
                    while (_printDatas.Count % maxRow != 0)
                    {
                        //空行を追加
                        _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
                    }
                    pageCount++;

                    //ヘッダー情報
                    while ((int)Math.Ceiling((double)(_printDatas.Count) / maxRow) > _headerL1.Count && _headerL1.Count >= 1)
                    {
                        _headerL1.Add(_headerL1.Last());
                    }
                }

                if (syunoInf.PtNum != prePrintData.PtNumKey || coFileType == CoFileType.Csv)
                {
                    printData.PtNum = syunoInf.PtNum;
                    printData.PtKanaName = syunoInf.PtKanaName;
                    printData.PtName = syunoInf.PtName;
                    printData.SexCd = syunoInf.SexCd;
                    printData.Sex = syunoInf.Sex;
                    printData.BirthDay = syunoInf.BirthDay;
                    printData.Age = syunoInf.Age.ToString();
                    printData.HomePost = syunoInf.HomePost;
                    printData.HomeAddress = syunoInf.HomeAddress;
                    printData.Tel1 = syunoInf.Tel1;
                    printData.Tel2 = syunoInf.Tel2;
                    printData.RenrakuTel = syunoInf.RenrakuTel;
                    printData.LastVisitDate = syunoInf.LastVisitDate;
                }

                printData.PtNumKey = syunoInf.PtNum;
                printData.SexCd = syunoInf.SexCd;
                printData.Sex = syunoInf.Sex;
                printData.BirthDay = syunoInf.BirthDay;
                printData.Age = syunoInf.Age.ToString();
                printData.HomePost = syunoInf.HomePost;
                printData.HomeAddress = syunoInf.HomeAddress;
                printData.SinDate = syunoInf.SinDate;
                printData.KaId = syunoInf.KaId;
                printData.KaSname = syunoInf.KaSname;
                printData.TantoId = syunoInf.TantoId;
                printData.TantoSname = syunoInf.TantoSname;
                printData.RaiinNo = syunoInf.RaiinNo;
                printData.HokenSbt = syunoInf.HokenSbt;
                printData.Syosaisin = syunoInf.Syosaisin;
                printData.SeikyuGaku = syunoInf.SeikyuGaku.ToString("#,0");
                printData.OldSeikyuGaku = syunoInf.OldSeikyuGaku.ToString("#,0");
                printData.NewSeikyuGaku = syunoInf.NewSeikyuGaku.ToString("#,0");
                printData.AdjustFutan = syunoInf.AdjustFutan.ToString("#,0");
                printData.OldAdjustFutan = syunoInf.OldAdjustFutan.ToString("#,0");
                printData.NewAdjustFutan = syunoInf.NewAdjustFutan.ToString("#,0");
                printData.TotalSeikyuGaku = syunoInf.TotalSeikyuGaku.ToString("#,0");
                printData.NyukinGaku = syunoInf.NyukinGaku.ToString("#,0");
                printData.MisyuGaku = syunoInf.MisyuGaku.ToString("#,0");
                printData.NyukinCmt = syunoInf.NyukinCmt;
                printData.MisyuKbn = syunoInf.MisyuKbn;

                //合計
                total.AddValue(printData);
                subTotal.AddValue(printData);
                ptTotal.AddValue(printData);

                //行追加
                _printDatas.Add(printData);

                //ヘッダー情報
                if ((int)Math.Ceiling((double)(_printDatas.Count) / maxRow) > _headerL1.Count)
                {
                    //改ページ条件
                    List<string> wrkHeaders = new List<string>();
                    if (pbKaId) wrkHeaders.Add(printData.KaSname);
                    if (pbTantoId) wrkHeaders.Add(printData.TantoSname);

                    if (wrkHeaders.Count >= 1) _headerL1.Add(string.Join("／", wrkHeaders));
                }
            }

            if (ptTotal.Count >= 2)
            {
                AddTotalRecord("◆患者計", ref ptTotal);
                //空行を追加
                _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
            }
            if (pageCount >= 2)
            {
                //空行を追加
                _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
                _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
                //小計
                AddTotalRecord("◆小計", ref subTotal);
            }

            //ヘッダー情報
            while ((int)Math.Ceiling((double)(_printDatas.Count) / maxRow) > _headerL1.Count && _headerL1.Count >= 1)
            {
                _headerL1.Add(_headerL1.Last());
            }

            //空行を追加
            _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
            _printDatas.Add(new CoSta1010PrintData(RowType.Brank));
            //合計
            AddTotalRecord("◆合計", ref total);
        }

        //合計レコードの追加
        void AddTotalRecord(string title, ref CountData totalData)
        {
            _printDatas.Add(
                new CoSta1010PrintData(RowType.Total)
                {
                    TotalCaption = title,
                    TotalCount = totalData.Count.ToString("#,0件"),
                    SeikyuGaku = totalData.SeikyuGaku.ToString("#,0"),
                    OldSeikyuGaku = totalData.OldSeikyuGaku.ToString("#,0"),
                    NewSeikyuGaku = totalData.NewSeikyuGaku.ToString("#,0"),
                    AdjustFutan = totalData.AdjustFutan.ToString("#,0"),
                    OldAdjustFutan = totalData.OldAdjustFutan.ToString("#,0"),
                    NewAdjustFutan = totalData.NewAdjustFutan.ToString("#,0"),
                    NyukinGaku = totalData.NyukinGaku.ToString("#,0"),
                    MisyuGaku = totalData.MisyuGaku.ToString("#,0"),
                    TotalSeikyuGaku = totalData.TotalSeikyuGaku.ToString("#,0"),
                }
            );
            totalData.Clear();
        }

        _hpInf = _finder.GetHpInf(hpId, _printConf.StartSinDate);

        _syunoInfs = _finder.GetSyunoInfs(hpId, _printConf);
        if ((_syunoInfs?.Count ?? 0) == 0)
        {
            return false;
        }

        //印刷用データの作成
        MakePrintData();

        return _printDatas.Count > 0;
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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta1010, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        _objectRseList = javaOutputData.objectNames;
    }

    private void GetRowCount(string fileName)
    {
        _rowCountFieldName = putColumns.Find(p => _objectRseList.Contains(p.ColName)).ColName;
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta1010, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta1010PrintConf printConf, int dateFrom, int dateTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        _printConf = printConf;
        this.coFileType = coFileType;
        string fileName = menuName + "_" + dateFrom + "_" + dateTo;
        List<string> retDatas = new List<string>();
        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        if (isPutTotalRow)
        {
            putCurColumns.AddRange(csvTotalColumns);
        }

        putCurColumns.AddRange(putColumns);
        var csvDatas = _printDatas.Where(p => p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)).ToList();

        if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        //出力フィールド
        List<string> wrkTitles = putCurColumns.Select(p => p.JpName).ToList();
        List<string> wrkColumns = putCurColumns.Select(p => p.CsvColName).ToList();

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

        string RecordData(CoSta1010PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putCurColumns)
            {
                var value = typeof(CoSta1010PrintData).GetProperty(column.CsvColName).GetValue(csvData);
                if (csvData.RowType == RowType.Total && !column.IsTotal)
                {
                    value = string.Empty;
                }
                else if (value is RowType)
                {
                    value = (int)value;
                }
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
