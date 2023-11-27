using Helper.Common;
using Reporting.Karte3.DB;
using Reporting.Karte3.Mapper;
using Reporting.Karte3.Model;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using static Reporting.Karte3.Enum.CoKarte3Column;

namespace Reporting.Karte3.Service;

public class Karte3CoReportService : IKarte3CoReportService
{
    #region Private properties
    private readonly ICoKarte3Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private List<CoKarte3PrintDataModel> printOutData;
    #endregion

    public Karte3CoReportService(ICoKarte3Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        coModel = null;
        printOutData = new();
        _singleFieldData = new();
        _listTextData = new();
        _extralData = new();
    }

    private int hpId;
    private long ptId;
    private int startSinYm;
    private int endSinYm;
    private bool includeHoken;
    private bool includeJihi;
    private int dataColCount;
    private int dataRowCount;
    private DateTime printoutDateTime;
    private bool hasNextPage;
    private int currentPage;
    private CoKarte3Model? coModel;

    public CommonReportingRequestModel GetKarte3PrintData(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi)
    {
        try
        {
            this.hpId = hpId;
            this.ptId = ptId;
            this.startSinYm = startSinYm;
            this.endSinYm = endSinYm;
            this.includeHoken = includeHoken;
            this.includeJihi = includeJihi;

            var coModels = GetData();
            if (coModels.Any())
            {
                currentPage = 1;
                coModel = coModels.First();
                hasNextPage = true;
                printoutDateTime = CIUtil.GetJapanDateTimeNow();
                GetRowCount();
                MakePrintDataList();

                while (hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            _extralData.Add("totalPage", (currentPage - 1).ToString());
            _extralData.Add("dataColCount", dataColCount.ToString());
            _extralData.Add("dataRowCount", dataRowCount.ToString());
            return new Karte3Mapper(_singleFieldData, _listTextData, _extralData).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    #region Private function
    private void MakePrintDataList()
    {
        List<CoKarte3DailyDataModel> dailyDatas = new();

        int sinDate = 0;

        CoKarte3DailyDataModel addDailyData = null;

        foreach (CoSinKouiModel sinKoui in coModel!.SinKouis.FindAll(p => p.TotalTen != 0))
        {
            if (sinDate != sinKoui.SinDate)
            {
                sinDate = sinKoui.SinDate;

                if (addDailyData != null && addDailyData.MaxCount > 0)
                {
                    dailyDatas.Add(addDailyData);
                }
                addDailyData = new CoKarte3DailyDataModel();
                addDailyData.Date = sinKoui.SinDate;
            }

            if (sinKoui.SanteiKbn == 2 || new List<string> { "JS", "SZ" }.Contains(sinKoui.CdKbn) || sinKoui.SinId == 96)
            {
                // 自費
                addDailyData[Karte3Column.HokenGai].Add(sinKoui.TotalTen);
                addDailyData.GetEnTenList(Karte3Column.HokenGai).Add(sinKoui.EntenKbn);
            }
            else if (sinKoui.CdKbn == "A")
            {
                // 初再診
                addDailyData[Karte3Column.Sinsatu].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Sinsatu).Add(sinKoui.EntenKbn);
            }
            else if (sinKoui.CdKbn == "C")
            {
                // 在宅
                addDailyData[Karte3Column.Zaitaku].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Zaitaku).Add(sinKoui.EntenKbn);
            }
            else if (sinKoui.CdKbn == "B")
            {
                // 医学管理
                addDailyData[Karte3Column.IgakuKanri].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.IgakuKanri).Add(sinKoui.EntenKbn);
            }
            else if (sinKoui.CdKbn == "F")
            {
                // 投薬
                addDailyData[Karte3Column.Toyaku].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Toyaku).Add(sinKoui.EntenKbn);
            }
            else if (sinKoui.CdKbn == "G")
            {
                // 注射
                addDailyData[Karte3Column.Chusya].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Chusya).Add(sinKoui.EntenKbn);
            }
            else if (sinKoui.CdKbn == "J")
            {
                // 処置
                addDailyData[Karte3Column.Shoti].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Shoti).Add(sinKoui.EntenKbn);
            }
            else if (new List<string> { "K", "L" }.Contains(sinKoui.CdKbn))
            {
                // 手術・麻酔
                addDailyData[Karte3Column.Ope].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Ope).Add(sinKoui.EntenKbn);
            }
            else if (new List<string> { "D", "N" }.Contains(sinKoui.CdKbn))
            {
                // 検査・病理
                addDailyData[Karte3Column.Kensa].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Kensa).Add(sinKoui.EntenKbn);
            }
            else if (new List<string> { "E" }.Contains(sinKoui.CdKbn))
            {
                // 画像
                addDailyData[Karte3Column.Gazo].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Gazo).Add(sinKoui.EntenKbn);
            }
            else if (new List<string> { "H", "I", "M", "R" }.Contains(sinKoui.CdKbn))
            {
                // リハビリ・精神・放射・労災
                addDailyData[Karte3Column.Sonota].Add(sinKoui.TotalTenDsp);
                addDailyData.GetEnTenList(Karte3Column.Sonota).Add(sinKoui.EntenKbn);
            }
        }

        if (addDailyData != null && addDailyData.MaxCount > 0)
        {
            dailyDatas.Add(addDailyData);
        }

        printOutData = new List<CoKarte3PrintDataModel>();

        // 各列の合計を記録するリスト
        List<double> gokeiList = new List<double>();

        for (int i = 0; i < System.Enum.GetNames(typeof(Karte3Column)).Length; i++)
        {
            gokeiList.Add(0);
        }

        // 合計の合計
        int gokeiTensu = 0;
        int gokeiFutan = 0;

        int sinYm = 0;

        CoKarte3PrintDataModel addData;

        foreach (CoKarte3DailyDataModel dailyData in dailyDatas)
        {
            if (sinYm == 0)
            {
                sinYm = dailyData.Date / 100;
            }
            else if (sinYm != dailyData.Date / 100)
            {
                sinYm = dailyData.Date / 100;

                // 月合計を求める
                addData = new CoKarte3PrintDataModel();
                addData.DataType = 1;
                addData.Date = "合計";

                addData.GokeiTensu = gokeiTensu;
                addData.GokeiFutan = gokeiFutan;

                foreach (Karte3Column Col in System.Enum.GetValues(typeof(Karte3Column)))
                {
                    addData[Col] = gokeiList[(int)Col];

                    gokeiList[(int)Col] = 0;//初期化
                }

                // 初期化
                gokeiTensu = 0;
                gokeiFutan = 0;

                printOutData.Add(addData);

            }

            sinDate = 0;

            for (int i = 0; i < dailyData.MaxCount; i++)
            {
                addData = new CoKarte3PrintDataModel();

                if (i == 0)
                {
                    // 日の先頭

                    addData.DataType = 0;

                    if (sinDate != dailyData.Date || (printOutData.Count % dataRowCount == 0))
                    {
                        // 日付が変わった場合、日付をセットする
                        addData.Date = $"{(dailyData.Date % 10000 / 100)}/{dailyData.Date % 100}";
                        sinDate = dailyData.Date;
                    }
                    addData.GokeiTensu = coModel.GetTotalTensu(dailyData.Date);
                    addData.GokeiFutan = coModel.GetTotalPtFutan(dailyData.Date);

                    gokeiTensu += addData.GokeiTensu;
                    gokeiFutan += addData.GokeiFutan;
                }
                else if (printOutData.Count % dataRowCount == 0)
                {
                    // ページ先頭行の場合、日付をセットする
                    addData.Date = $"{(dailyData.Date % 10000 / 100)}/{dailyData.Date % 100}";
                }

                foreach (Karte3Column Col in System.Enum.GetValues(typeof(Karte3Column)))
                {
                    if (i < dailyData.GetList(Col).Count)
                    {
                        addData[Col] = dailyData.GetList(Col)[i];
                        gokeiList[(int)Col] += addData[Col];
                    }
                }

                printOutData.Add(addData);
            }
        }

        // 最後の１回
        // 月合計を求める
        addData = new();
        addData.DataType = 1;
        addData.Date = "合計";

        addData.GokeiTensu = gokeiTensu;
        addData.GokeiFutan = gokeiFutan;

        foreach (Karte3Column Col in System.Enum.GetValues(typeof(Karte3Column)))
        {
            addData[Col] = gokeiList[(int)Col];

            gokeiList[(int)Col] = 0;//初期化
        }
        printOutData.Add(addData);
    }

    private void UpdateDrawForm()
    {
        #region SubMethod
        List<ListTextObject> listDataPerPage = new();
        // ヘッダー
        void UpdateFormHeader()
        {
            #region sub method
            string GetWarekiYm(int ym)
            {
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(ym * 100 + 1);
                string ret = $"{wareki.Gengo} {wareki.Year}年{wareki.Month}月";
                return ret;
            }
            #endregion

            // 発行日時
            SetFieldData("dfPrintDateTime", printoutDateTime.ToString("yyyy/MM/dd HH:mm"));

            // 患者番号
            SetFieldData("dfPtNum", coModel!.PtNum.ToString());

            // 患者氏名
            SetFieldData("dfPtName", coModel.PtName);

            // 性別
            SetFieldData("dfSex", coModel.PtSex);

            // 生年月日
            SetFieldData("dfBirthDay", CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd);

            // 保険の種類
            SetFieldData("dfHokenSbt", coModel.HokenSyu);

            // 集計期間
            SetFieldData("dfSyukeiStartDate", GetWarekiYm(startSinYm));
            SetFieldData("dfSyukeiEndDate", GetWarekiYm(endSinYm));
        }

        // 本体
        void UpdateFormBody()
        {
            int dataIndex = (currentPage - 1) * dataRowCount;

            if (printOutData == null || printOutData.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            for (short i = 0; i < dataRowCount; i++)
            {
                listDataPerPage.Add(new("lsData", 0, i, printOutData[dataIndex].Date));
                listDataPerPage.Add(new("lsData", 1, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Sinsatu], "#,0")));
                listDataPerPage.Add(new("lsData", 2, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Zaitaku], "#,0")));
                listDataPerPage.Add(new("lsData", 3, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.IgakuKanri], "#,0")));
                listDataPerPage.Add(new("lsData", 4, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Toyaku], "#,0")));
                listDataPerPage.Add(new("lsData", 5, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Chusya], "#,0")));
                listDataPerPage.Add(new("lsData", 6, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Shoti] + printOutData[dataIndex][Karte3Column.Ope], "#,0")));
                listDataPerPage.Add(new("lsData", 7, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Kensa], "#,0")));
                listDataPerPage.Add(new("lsData", 8, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Gazo], "#,0")));
                listDataPerPage.Add(new("lsData", 9, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Sonota], "#,0")));

                listDataPerPage.Add(new("lsData", 10, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex].GokeiTensu, "#,0")));
                listDataPerPage.Add(new("lsData", 11, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex].GokeiFutan, "#,0")));

                listDataPerPage.Add(new("lsData", 12, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.HokenGai], "#,0")));

                #region セル装飾
                string rowKey = i + "_" + currentPage;
                if (printOutData[dataIndex].DataType == 1)
                {
                    _extralData.Add("GroupDraw1_" + rowKey, "true");
                }
                else if (!(string.IsNullOrEmpty(printOutData[dataIndex].Date)))
                {
                    if (i != 0)
                    {
                        _extralData.Add("GroupDraw2_" + rowKey, "true");
                    }
                }
                else
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
            _listTextData.Add(currentPage, listDataPerPage);
            _extralData.Add("TotalItemPerPage_" + currentPage, (dataIndex - 1).ToString());
        }

        #endregion
        UpdateFormHeader();
        UpdateFormBody();
    }

    private List<CoKarte3Model> GetData()
    {
        // 患者情報
        CoPtInfModel ptInf = _finder.FindPtInf(hpId, ptId);

        // 診療行為
        List<CoSinKouiModel> sinKouis = _finder.FindSinKoui(hpId, ptId, startSinYm, endSinYm, includeHoken, includeJihi);

        List<int> hokenIds = sinKouis.GroupBy(p => p.HokenId).Select(p => p.Key).ToList();

        List<CoKarte3Model> retDatas = new();

        List<int> targetHokenIds = new();
        foreach (int hokenId in hokenIds)
        {
            // 患者保険
            CoPtHokenInfModel ptHoken = _finder.FindPtHoken(hpId, ptId, hokenId, endSinYm * 100 + 31);

            if (!includeJihi && ptHoken.HokenSbtKbn == 8)
            {
                // 自費を除く設定の場合、自費保険は無視
            }
            else if (!includeHoken && ptHoken.HokenSbtKbn != 8)
            {
                // 保険を除く設定の場合、自費保険以外は無視
            }
            else
            {
                targetHokenIds.Add(hokenId);
            }
        }

        List<CoKaikeiInfModel> kaikeiInfs = _finder.FindKaikeiInf(hpId, ptId, startSinYm, endSinYm, targetHokenIds);
        HashSet<string> houbetuNos = new();

        List<CoSinKouiModel> filteredSinKouis =
            sinKouis.FindAll(p => targetHokenIds.Contains(p.HokenId))
            .OrderBy(p => p.SinDate)
            .ThenBy(p => p.HokenId)
            .ThenBy(p => p.SanteiKbn)
            .ThenBy(p => p.CdKbn)
            .ThenBy(p => p.CdNo)
            .ThenBy(p => p.RpNo)
            .ThenBy(p => p.SeqNo)
            .ToList();

        retDatas.Add(new CoKarte3Model(ptInf, null, filteredSinKouis, kaikeiInfs, houbetuNos));

        return retDatas;
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
    #endregion
}
