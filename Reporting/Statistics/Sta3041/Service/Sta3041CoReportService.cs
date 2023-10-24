using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3041.DB;
using Reporting.Statistics.Sta3041.Mapper;
using Reporting.Statistics.Sta3041.Models;

namespace Reporting.Statistics.Sta3041.Service;

public class Sta3041CoReportService : ISta3041CoReportService
{

    #region Constant
    private int maxRow = 40;
    private const int conKouFuan = 1;
    private const int conSuimin = 2;
    private const int conKouUtu = 3;
    private const int conKouSeisin = 4;

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SinYm", "診療年月"),
            new PutColumn("PtNum", "患者番号"),
            new PutColumn("KanaName", "カナ氏名"),
            new PutColumn("PtName", "氏名"),
            new PutColumn("SinDate", "診療日", false, "SinDateI"),
            new PutColumn("KouseisinKbnCd", "向精神薬区分コード"),
            new PutColumn("KouseisinKbn", "向精神薬区分"),
            new PutColumn("DrugCount", "種類数"),
            new PutColumn("YakkaCd", "薬価基準コード"),
            new PutColumn("YakkaCd7", "薬価基準コード7"),
            new PutColumn("ItemCd", "診療行為コード"),
            new PutColumn("DrugName", "医薬品名称"),
            new PutColumn("Tazai", "多剤投与"),
            new PutColumn("TazaiFuan", "多剤_抗不安薬"),
            new PutColumn("TazaiSuimin", "多剤_睡眠薬"),
            new PutColumn("TazaiUtu", "多剤_抗うつ薬"),
            new PutColumn("TazaiSeisin", "多剤_抗精神病薬"),
            new PutColumn("TazaiUtuSeisin", "多剤_抗うつ抗精神病"),
            new PutColumn("TazaiFuanSuimin", "多剤_抗不安睡眠"),
            new PutColumn("MeisaiKbn", "明細区分"),
            new PutColumn("MeisaiKbnName", "計見出し"),
            new PutColumn("PtCnt", "投与患者数"),
            new PutColumn("TazaiPtCnt", "多剤投与患者数"),
            new PutColumn("TazaiFuanPtCnt", "多剤_抗不安薬投与患者数"),
            new PutColumn("TazaiSuiminPtCnt", "多剤_睡眠薬投与患者数"),
            new PutColumn("TazaiUtuPtCnt", "多剤_抗うつ薬投与患者数"),
            new PutColumn("TazaiSeisinPtCnt", "多剤_抗精神病薬投与患者数"),
            new PutColumn("TazaiUtuSeisinPtCnt", "多剤_抗うつ抗精神病投与患者数"),
            new PutColumn("TazaiFuanSuiminPtCnt", "多剤_抗不安睡眠投与患者数"),
            new PutColumn("UtuPtCnt", "抗うつ薬投与患者数"),
            new PutColumn("SeisinPtCnt", "抗精神病薬投与患者数"),
            new PutColumn("UtuSeisinPtCnt", "抗うつ抗精神病投与患者数")
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

    private List<string> headerL;
    private List<CoKouseisinInf> kouseisinInfs;
    private List<CoSta3041PrintData> printDatas;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3041Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3041PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;

    public Sta3041CoReportService(ICoSta3041Finder finder, IReadRseReportFileService readRseReportFileService)
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

    public CommonReportingRequestModel GetSta3041ReportingData(CoSta3041PrintConf printConf, int hpId, CoFileType outputFileType)
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

        return new Sta3041Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
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
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");

            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + currentPage, headerL.Count >= currentPage ? headerL[currentPage - 1] : string.Empty);

            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);

            //期間
            SetFieldData("Range", string.Format("期間: {0}～{1}　",
                    printConf.FromYm > 0 ? CIUtil.SMonthToShowSWMonth(printConf.FromYm, 0, 0, 1) : string.Empty,
                    printConf.ToYm > 0 ? CIUtil.SMonthToShowSWMonth(printConf.ToYm, 0, 0, 1) : string.Empty));
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

                    var value = typeof(CoSta3041PrintData).GetProperty(colName).GetValue(printData);
                    AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

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

    private struct countData
    {
        public struct countKey
        {
            public long PtNum;
            public int SinYm;
            public int SinDate;
        }

        public List<countKey> Pts;
        public List<countKey> KouFuanPts;
        public List<countKey> SuiminPts;
        public List<countKey> KouUtuPts;
        public List<countKey> KouSeisinPts;
        public List<countKey> KouUtuKouSeisinPts;
        public List<countKey> TazaiPts;
        public List<countKey> TazaiKouFuanPts;
        public List<countKey> TazaiSuiminPts;
        public List<countKey> TazaiKouUtuPts;
        public List<countKey> TazaiKouSeisinPts;
        public List<countKey> TazaiKouFuanSuiminPts;
        public List<countKey> TazaiKouUtuKouSeisinPts;

        //初期化
        public void Init()
        {
            if (Pts == null) Pts = new List<countKey>();
            if (KouFuanPts == null) KouFuanPts = new List<countKey>();
            if (SuiminPts == null) SuiminPts = new List<countKey>();
            if (KouUtuPts == null) KouUtuPts = new List<countKey>();
            if (KouSeisinPts == null) KouSeisinPts = new List<countKey>();
            if (KouUtuKouSeisinPts == null) KouUtuKouSeisinPts = new List<countKey>();
            if (TazaiPts == null) TazaiPts = new List<countKey>();
            if (TazaiKouFuanPts == null) TazaiKouFuanPts = new List<countKey>();
            if (TazaiSuiminPts == null) TazaiSuiminPts = new List<countKey>();
            if (TazaiKouUtuPts == null) TazaiKouUtuPts = new List<countKey>();
            if (TazaiKouSeisinPts == null) TazaiKouSeisinPts = new List<countKey>();
            if (TazaiKouFuanSuiminPts == null) TazaiKouFuanSuiminPts = new List<countKey>();
            if (TazaiKouUtuKouSeisinPts == null) TazaiKouUtuKouSeisinPts = new List<countKey>();
        }


        public void AddValue(int sinDate, long ptNum, ref int[] drugCounts)
        {

            countKey tgtKey = new countKey { PtNum = ptNum, SinYm = sinDate / 100, SinDate = sinDate };

            Pts.Add(tgtKey);

            if (drugCounts[conKouFuan - 1] >= 1)
            {
                KouFuanPts.Add(tgtKey);
                if (drugCounts[conKouFuan - 1] >= 3)
                {
                    TazaiPts.Add(tgtKey);
                    TazaiKouFuanPts.Add(tgtKey);
                }
            }
            if (drugCounts[conSuimin - 1] >= 1)
            {
                SuiminPts.Add(tgtKey);
                if (drugCounts[conSuimin - 1] >= 3)
                {
                    TazaiPts.Add(tgtKey);
                    TazaiSuiminPts.Add(tgtKey);
                }
            }
            if (drugCounts[conKouUtu - 1] >= 1)
            {
                KouUtuPts.Add(tgtKey);
                KouUtuKouSeisinPts.Add(tgtKey);
                if (drugCounts[conKouUtu - 1] >= 3)
                {
                    TazaiPts.Add(tgtKey);
                    TazaiKouUtuPts.Add(tgtKey);
                    TazaiKouUtuKouSeisinPts.Add(tgtKey);
                }
            }
            if (drugCounts[conKouSeisin - 1] >= 1)
            {
                KouSeisinPts.Add(tgtKey);
                KouUtuKouSeisinPts.Add(tgtKey);
                if (drugCounts[conKouSeisin - 1] >= 3)
                {
                    TazaiPts.Add(tgtKey);
                    TazaiKouSeisinPts.Add(tgtKey);
                    TazaiKouUtuKouSeisinPts.Add(tgtKey);
                }
            }
            if (sinDate >= 20180401 && drugCounts[conKouFuan - 1] + drugCounts[conSuimin - 1] >= 4)
            {
                //平成30年4月以降は、4種類以上の抗不安薬及び睡眠薬の投与を多剤投与に含める
                TazaiPts.Add(tgtKey);
                TazaiKouFuanSuiminPts.Add(tgtKey);
            }

            //種類数を初期化
            for (int i = 0; i < drugCounts.Length; i++)
            {
                drugCounts[i] = 0;
            }
        }

    }

    private countData total = new countData();

    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            headerL = new List<string>();
            printDatas = new List<CoSta3041PrintData>();
            CoKouseisinInf? preKouseisinInf = null;
            const int subTotalRowsCnt = 3;
            const int totalRowsCnt = 14;

            int[] drugCounts = new int[4] { 0, 0, 0, 0 }; //向精神薬区分ごとの種類数
            int rowCount = 0;
            int pgCount = 1;
            int prePgCount;
            bool pgBreak = false;
            total.Init();

            #region ソート順
            kouseisinInfs = kouseisinInfs?
                .OrderBy(s => s.SinYm)
                .ThenBy(s => s.PtNum)
                .ThenBy(s => s.SinDate)
                .ThenBy(s => s.KouseisinKbnCd)
                .ThenBy(s => s.YakkaCd)
                .ToList() ?? new();
            #endregion

            #region SubMethod
            int GetDrugCount(CoKouseisinInf tgtData, CoKouseisinInf preData)
            {
                //csvの場合、向精神薬区分ごとの1行目にだけ種類数は記録し、残りは0にする
                int ret = 0;
                if (outputFileType != CoFileType.Csv || coFileType != CoFileType.Csv)
                {
                    ret = tgtData.DrugCount;
                }

                if (preData == null || tgtData.PtNum != preData?.PtNum || tgtData.SinDate != preData?.SinDate || tgtData.KouseisinKbnCd != preData?.KouseisinKbnCd)
                {
                    ret = tgtData.DrugCount;

                    for (int i = 0; i < drugCounts.Length; i++)
                    {
                        if (tgtData.KouseisinKbnCd == i + 1)
                        {
                            //抗精神病薬区分ごとに種類数をカウント
                            drugCounts[i] = tgtData.DrugCount;
                        }
                    }
                }

                return ret;
            }

            void AddMeisaiRecord(CoKouseisinInf tgtData, CoKouseisinInf preData, bool is1stRow)
            {
                CoSta3041PrintData printData = new CoSta3041PrintData();

                //フォームファイルで出力するとき、前の行と重複する値を省略する
                if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv || is1stRow || tgtData.PtNum != preData.PtNum)
                {
                    printData.PtNum = tgtData.PtNum.ToString();
                    printData.KanaName = tgtData.KanaName;
                    printData.PtName = tgtData.PtName;
                    printData.SinDate = CIUtil.SDateToShowSDate(tgtData.SinDate);
                    printData.KouseisinKbn = tgtData.KouseisinKbn;
                    printData.DrugCount = GetDrugCount(tgtData, preData).ToString();
                }
                else if (tgtData.SinDate != preData.SinDate)
                {
                    printData.SinDate = CIUtil.SDateToShowSDate(tgtData.SinDate);
                    printData.KouseisinKbn = tgtData.KouseisinKbn;
                    printData.DrugCount = GetDrugCount(tgtData, preData).ToString();
                }
                else if (tgtData.KouseisinKbnCd != preData.KouseisinKbnCd)
                {
                    printData.KouseisinKbn = tgtData.KouseisinKbn;
                    printData.DrugCount = GetDrugCount(tgtData, preData).ToString();
                }

                printData.SinYm = outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv ? tgtData.SinYm.ToString() : CIUtil.SMonthToShowSMonth(tgtData.SinYm);
                printData.KouseisinKbnCd = tgtData.KouseisinKbnCd.ToString();
                printData.YakkaCd = tgtData.YakkaCd;
                printData.YakkaCd7 = tgtData.YakkaCd7;
                printData.ItemCd = tgtData.ItemCd;
                printData.DrugName = tgtData.DrugName;
                printData.MeisaiKbn = 0;

                printData.SinDateI = tgtData.SinDate;
                printData.PtNumL = tgtData.PtNum;

                //明細行追加
                printDatas.Add(printData);

            }

            void AddSubTotalRecord(countData totalData, int sinYm = 0)
            {
                if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                {
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            SinYm = sinYm.ToString(),
                            MeisaiKbn = 1,
                            PtCnt = totalData.Pts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiPtCnt = totalData.TazaiPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiFuanPtCnt = totalData.TazaiKouFuanPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiSuiminPtCnt = totalData.TazaiSuiminPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiUtuPtCnt = totalData.TazaiKouUtuPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiSeisinPtCnt = totalData.TazaiKouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiUtuSeisinPtCnt = totalData.TazaiKouUtuKouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiFuanSuiminPtCnt = totalData.TazaiKouFuanSuiminPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            UtuPtCnt = totalData.KouUtuPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            SeisinPtCnt = totalData.KouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            UtuSeisinPtCnt = totalData.KouUtuKouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                        }
                    );
                }
                else
                {
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalKbn = "◆小計",
                            TotalCaption = string.Format("投与患者数　　： {0,5}（抗不安薬： {1,5} 睡眠薬: {2,5} 抗うつ薬: {3,5} 抗精神病薬: {4,5} 抗うつ薬又は抗精神病薬: {5,5}）",
                               totalData.Pts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouFuanPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.SuiminPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouUtuPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouUtuKouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("多剤投与患者数： {0,5}（抗不安薬： {1,5} 睡眠薬: {2,5} 抗うつ薬: {3,5} 抗精神病薬: {4,5} 抗うつ薬又は抗精神病薬: {5,5} 抗不安薬及び睡眠薬: {6,5}）",
                               totalData.TazaiPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouFuanPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiSuiminPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouUtuPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouUtuKouSeisinPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouFuanSuiminPts.Where(x => x.SinYm == sinYm).Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );
                }

            }

            void AddTotalRecord(countData totalData)
            {

                if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                {
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            MeisaiKbn = 2,
                            PtCnt = totalData.Pts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiPtCnt = totalData.TazaiPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiFuanPtCnt = totalData.TazaiKouFuanPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiSuiminPtCnt = totalData.TazaiSuiminPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiUtuPtCnt = totalData.TazaiKouUtuPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiSeisinPtCnt = totalData.TazaiKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiUtuSeisinPtCnt = totalData.TazaiKouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            TazaiFuanSuiminPtCnt = totalData.TazaiKouFuanSuiminPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            UtuPtCnt = totalData.KouUtuPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            SeisinPtCnt = totalData.KouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                            UtuSeisinPtCnt = totalData.KouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                        }
                    );
                }
                else
                {
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalKbn = "◆合計",
                            TotalCaption = string.Format("投与患者数　　： {0,5}（抗不安薬： {1,5} 睡眠薬: {2,5} 抗うつ薬: {3,5} 抗精神病薬: {4,5} 抗うつ薬又は抗精神病薬: {5,5}）",
                               totalData.Pts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouFuanPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.SuiminPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouUtuPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("多剤投与患者数： {0,5}（抗不安薬： {1,5} 睡眠薬: {2,5} 抗うつ薬: {3,5} 抗精神病薬: {4,5} 抗うつ薬又は抗精神病薬: {5,5} 抗不安薬及び睡眠薬: {6,5}）",
                               totalData.TazaiPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouFuanPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiSuiminPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouUtuPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouFuanSuiminPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );

                    //空行を追加
                    printDatas.Add(new CoSta3041PrintData(RowType.Brank));
                    printDatas.Add(new CoSta3041PrintData(RowType.Brank));

                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("①投与患者数： {0,5}",
                               totalData.Pts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("　　②抗うつ薬又は抗精神病薬： {0,5} （③抗うつ薬: {1,5} ④抗精神病薬: {2,5}）",
                               totalData.KouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouUtuPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.KouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );

                    //空行を追加
                    printDatas.Add(new CoSta3041PrintData(RowType.Brank));

                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("⑤向精神薬多剤投与患者数： {0,5}",
                               totalData.TazaiPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("　　⑥抗不安薬：           {0,5}",
                               totalData.TazaiKouFuanPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("　　⑦睡眠薬：             {0,5}",
                               totalData.TazaiSuiminPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );
                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("　　⑧抗うつ薬又は抗精神病薬： {0,5} （⑨抗うつ薬: {1,5} ➉抗精神病薬: {2,5}）",
                               totalData.TazaiKouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouUtuPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0"),
                               totalData.TazaiKouSeisinPts.Select(x => x.PtNum).Distinct().Count().ToString("#,0")
                               )
                        }
                    );

                    //空行を追加
                    printDatas.Add(new CoSta3041PrintData(RowType.Brank));

                    double KouUtuKouSeisinCnt = totalData.KouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count();
                    double TazaiKouUtuKouSeisinCnt = totalData.TazaiKouUtuKouSeisinPts.Select(x => x.PtNum).Distinct().Count();
                    double rate = 0;
                    if (KouUtuKouSeisinCnt > 0)
                    {
                        rate = TazaiKouUtuKouSeisinCnt / KouUtuKouSeisinCnt;
                    }

                    printDatas.Add(
                        new CoSta3041PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("⑧／②　＝　{0}", rate.ToString("#0.00%"))
                        }
                    );
                }


            }

            void AddBrankRecord(int rowCnt, ref int pgCnt, int addedRowCnt = 0, bool isBreak = false)
            {
                if (isBreak || (addedRowCnt > 0 && maxRow * pgCnt - rowCnt < addedRowCnt))
                {
                    //改ページ
                    for (int i = printDatas.Count; i < maxRow * pgCount; i++)
                    {
                        printDatas.Add(new CoSta3041PrintData(RowType.Brank));
                    }
                    pgCnt++;
                }
                else
                {
                    //1行空白を追加
                    printDatas.Add(new CoSta3041PrintData(RowType.Brank));
                }
            }

            void SetPtCountFlg(countData totalData, int sinYm, long ptNum)
            {
                int lastIdx = printDatas.Count - 1;
                if (lastIdx >= 0)
                {
                    printDatas[lastIdx].PtCnt = totalData.Pts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.Pts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].TazaiPtCnt = totalData.TazaiPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.TazaiPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].TazaiFuanPtCnt = totalData.TazaiKouFuanPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.TazaiKouFuanPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].TazaiSuiminPtCnt = totalData.TazaiSuiminPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.TazaiSuiminPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].TazaiUtuPtCnt = totalData.TazaiKouUtuPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.TazaiKouUtuPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].TazaiSeisinPtCnt = totalData.TazaiKouSeisinPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.TazaiKouSeisinPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].TazaiUtuSeisinPtCnt = totalData.TazaiKouUtuKouSeisinPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.TazaiKouUtuKouSeisinPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].TazaiFuanSuiminPtCnt = totalData.TazaiKouFuanSuiminPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.TazaiKouFuanSuiminPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].UtuPtCnt = totalData.KouUtuPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.KouUtuPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].SeisinPtCnt = totalData.KouSeisinPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.KouSeisinPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                    printDatas[lastIdx].UtuSeisinPtCnt = totalData.KouUtuKouSeisinPts.Exists(x => x.SinYm == sinYm && x.PtNum == ptNum)
                       ? totalData.KouUtuKouSeisinPts.Exists(x => x.SinYm < sinYm && x.PtNum == ptNum) ? "-" : "1"
                       : "0";
                }

            }

            void SetTazaiFlg()
            {
                foreach (var printData in printDatas)
                {
                    if (printData.RowType == RowType.Data)
                    {
                        switch (CIUtil.StrToIntDef(printData.KouseisinKbnCd, -1))
                        {
                            case conKouFuan:
                                printData.TazaiFuan = total.TazaiKouFuanPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                printData.TazaiFuanSuimin = total.TazaiKouFuanSuiminPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                break;
                            case conSuimin:
                                printData.TazaiSuimin = total.TazaiSuiminPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                printData.TazaiFuanSuimin = total.TazaiKouFuanSuiminPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                break;
                            case conKouUtu:
                                printData.TazaiUtu = total.TazaiKouUtuPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                printData.TazaiUtuSeisin = total.TazaiKouUtuKouSeisinPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                break;
                            case conKouSeisin:
                                printData.TazaiSeisin = total.TazaiKouSeisinPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                printData.TazaiUtuSeisin = total.TazaiKouUtuKouSeisinPts.Exists(x => x.PtNum == printData.PtNumL && x.SinDate == printData.SinDateI) ? "*" : string.Empty;
                                break;
                            default:
                                break;
                        }
                        printData.Tazai = printData.TazaiFuan == "*" || printData.TazaiSuimin == "*" ||
                                          printData.TazaiUtu == "*" || printData.TazaiSeisin == "*" || printData.TazaiUtuSeisin == "*" ||
                                          printData.TazaiFuanSuimin == "*"
                                          ? "*" : string.Empty;
                    }
                }
            }
            #endregion

            foreach (var kouseisinInf in kouseisinInfs)
            {
                //診療日ごとに投与患者数をカウント
                if (preKouseisinInf != null && (kouseisinInf.PtNum != preKouseisinInf.PtNum || kouseisinInf.SinDate != preKouseisinInf.SinDate))
                {
                    total.AddValue(preKouseisinInf.SinDate, preKouseisinInf.PtNum, ref drugCounts);
                }

                //患者ごとにカウントフラグをセット
                if (preKouseisinInf != null && (kouseisinInf.PtNum != preKouseisinInf.PtNum))
                {
                    SetPtCountFlg(total, preKouseisinInf.SinYm, preKouseisinInf.PtNum);
                }

                //改ページ条件
                pgBreak = false;
                if (preKouseisinInf != null)
                {
                    pgBreak = kouseisinInf.SinYm != preKouseisinInf.SinYm;
                }

                //改ページ
                if (rowCount == maxRow || pgBreak)
                {
                    if (pgBreak)
                    {
                        //診療年月ごとに小計を出力
                        prePgCount = pgCount;
                        AddBrankRecord(printDatas.Count, ref pgCount, subTotalRowsCnt);
                        if (pgCount > prePgCount)
                        {
                            headerL.Add(CIUtil.SMonthToShowSWMonth(preKouseisinInf?.SinYm ?? 0, 0, 0, 1));
                        }
                        AddSubTotalRecord(total, preKouseisinInf?.SinYm ?? 0);
                    }
                    AddBrankRecord(printDatas.Count, ref pgCount, 0, true);
                    rowCount = 0;
                }

                //各ページに診療年月を印字
                if (rowCount == 0)
                {
                    headerL.Add(CIUtil.SMonthToShowSWMonth(kouseisinInf.SinYm, 0, 0, 1));
                }

                //明細
                AddMeisaiRecord(kouseisinInf, preKouseisinInf, rowCount == 0);
                rowCount++;

                preKouseisinInf = kouseisinInf;
            }

            //最後の患者をカウント
            total.AddValue(preKouseisinInf.SinDate, preKouseisinInf.PtNum, ref drugCounts);
            SetPtCountFlg(total, preKouseisinInf.SinYm, preKouseisinInf.PtNum);

            //小計
            prePgCount = pgCount;
            AddBrankRecord(printDatas.Count, ref pgCount, subTotalRowsCnt);
            if (pgCount > prePgCount)
            {
                headerL.Add(CIUtil.SMonthToShowSWMonth(preKouseisinInf.SinYm, 0, 0, 1));
            }
            AddSubTotalRecord(total, preKouseisinInf.SinYm);

            //合計
            AddBrankRecord(printDatas.Count, ref pgCount, totalRowsCnt);
            AddTotalRecord(total);

            //多剤投与フラグをセット
            SetTazaiFlg();
        }

        hpInf = _finder.GetHpInf(hpId, CIUtil.DateTimeToInt(DateTime.Today));

        kouseisinInfs = _finder.GetKouseisinInfs(hpId, printConf);
        if ((kouseisinInfs?.Count ?? 0) == 0) return false;

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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3041, fileName, new());
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

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3041, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3041PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        this.coFileType = coFileType;
        string fileName = printConf.ReportName + "_" + monthFrom + "_" + monthTo;
        List<string> retDatas = new List<string>();
        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        var csvDatas = printDatas.Where(p => p.RowType != RowType.Brank).ToList();
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

        string RecordData(CoSta3041PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putColumns)
            {

                var value = typeof(CoSta3041PrintData).GetProperty(column.CsvColName).GetValue(csvData);
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
