using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.ReceTarget.DB;
using Reporting.ReceTarget.Mapper;
using Reporting.ReceTarget.Model;

namespace Reporting.ReceTarget.Service;

public class ReceTargetCoReportService : IReceTargetCoReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly Dictionary<string, string> _extralData;
    private readonly ICoReceTargetFinder _finder;
    private const string formFileName = "fmReceiptTarget.rse";


    public ReceTargetCoReportService(ICoReceTargetFinder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        _singleFieldData = new();
        _listTextData = new();
        _extralData = new();
        printOutData = new();
        coModel = new();
    }

    private List<CoReceTargetPrintDataModel> printOutData;

    private int dataRowCount;
    private int dataColCount;
    private DateTime printoutDateTime;
    private int currentPage;
    private bool hasNextPage;
    private CoReceTargetModel coModel;

    public CommonReportingRequestModel GetReceTargetPrintData(int hpId, int seikyuYm)
    {
        try
        {
            currentPage = 1;
            hasNextPage = true;
            printoutDateTime = CIUtil.GetJapanDateTimeNow();
            GetRowCount();
            coModel = GetData(hpId, seikyuYm);
            if (coModel != null)
            {
                MakePrintDataList();
                while (hasNextPage)
                {
                    UpdateDrawForm(seikyuYm);
                    currentPage++;
                }
            }
            _extralData.Add("totalPage", (currentPage - 1).ToString());
            return new ReceTargetMapper(_singleFieldData, _listTextData, _extralData, formFileName).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    private void MakePrintDataList()
    {
        printOutData = new();

        string receSbt = string.Empty;
        int syokei = 0;
        int gokei = 0;
        int col = 0;
        int hokenKbn = -1;

        List<CoReceInfModel> receInfModels = coModel.ReceInfModels.OrderBy(p => p.HokenKbn).ThenBy(p => p.ReceSbt).ToList();

        foreach (CoReceInfModel receInfModel in receInfModels)
        {
            if (hokenKbn == -1)
            {
                receSbt = receInfModel.ReceSbt;
                hokenKbn = receInfModel.HokenKbn;
            }
            if (hokenKbn != receInfModel.HokenKbn || receSbt != receInfModel.ReceSbt)
            {
                printOutData.Add(new CoReceTargetPrintDataModel(dataColCount));
                printOutData.Last().Comment = $"{GetReceSbt(hokenKbn, receSbt)}　小計: {syokei}件";

                printOutData.Add(new CoReceTargetPrintDataModel(dataColCount));
                printOutData.Last().Comment = new string('-', 200);

                receSbt = receInfModel.ReceSbt;
                hokenKbn = receInfModel.HokenKbn;

                syokei = 0;
                col = 0;
            }

            if (col == 0)
            {
                printOutData.Add(new CoReceTargetPrintDataModel(dataColCount));
            }

            printOutData.Last().LineDatas[col] = $"{receInfModel.PtNum.ToString().PadLeft(10)} {receInfModel.PtName}";
            col++;

            if (col >= dataColCount)
            {
                col = 0;
            }

            syokei++;
            gokei++;
        }

        if (syokei > 0)
        {
            printOutData.Add(new CoReceTargetPrintDataModel(dataColCount));
            printOutData.Last().Comment = $"{GetReceSbt(hokenKbn, receSbt)}　小計: {syokei}件";

            printOutData.Add(new CoReceTargetPrintDataModel(dataColCount));
            printOutData.Last().Comment = new string('-', 200);
        }

        printOutData.Add(new CoReceTargetPrintDataModel(dataColCount));
        printOutData.Last().Comment = $"合計　{gokei}件";

    }

    private string GetReceSbt(int hokenKbn, string receSbt)
    {
        string ret = string.Empty;

        if (hokenKbn == 999)
        {
            ret = "自費";
        }
        else if (hokenKbn == 1)
        {

            if (CIUtil.Copy(receSbt, 2, 1) == "1")
            {
                ret = "社保";
                switch (CIUtil.Copy(receSbt, 3, 1))
                {
                    case "1": ret = ret + "・単独"; break;
                    case "2": ret = ret + "・２併"; break;
                    case "3": ret = ret + "・３併"; break;
                    case "4": ret = ret + "・４併"; break;
                    case "5": ret = ret + "・５併"; break;
                }

                switch (CIUtil.Copy(receSbt, 4, 1))
                {
                    case "2": ret = ret + "（本人）"; break;
                    case "4": ret = ret + "（６歳未就学）"; break;
                    case "6": ret = ret + "（家族）"; break;
                    case "8": ret = ret + "（高齢者一般・低所）"; break;
                    case "0": ret = ret + "（高齢者７割）"; break;

                }
            }
            else if (CIUtil.Copy(receSbt, 2, 1) == "2")
            {
                ret = "公費";


                switch (CIUtil.Copy(receSbt, 3, 1))
                {
                    case "1": ret = ret + "・単独"; break;
                    case "2": ret = ret + "・２併"; break;
                    case "3": ret = ret + "・３併"; break;
                    case "4": ret = ret + "・４併"; break;
                    case "5": ret = ret + "・５併"; break;
                }
            }
        }
        else if (hokenKbn == 2)
        {
            switch (CIUtil.Copy(receSbt, 2, 1))
            {
                case "1": ret = ret + "国保"; break;
                case "3": ret = ret + "後期"; break;
                case "4": ret = ret + "退職"; break;
            }
            switch (CIUtil.Copy(receSbt, 3, 1))
            {
                case "1": ret = ret + "・単独"; break;
                case "2": ret = ret + "・２併"; break;
                case "3": ret = ret + "・３併"; break;
                case "4": ret = ret + "・４併"; break;
                case "5": ret = ret + "・５併"; break;
            }

            switch (CIUtil.Copy(receSbt, 4, 1))
            {
                case "2": ret = ret + "（本人）"; break;
                case "4": ret = ret + "（６歳未就学）"; break;
                case "6": ret = ret + "（家族）"; break;
                case "8": ret = ret + "（高齢者一般・低所）"; break;
                case "0": ret = ret + "（高齢者７割）"; break;
            }

        }
        else if (new int[] { 11, 12, 13 }.Contains(hokenKbn))
        {
            ret = "労災";
        }
        else if (hokenKbn == 14)
        {
            ret = "自賠";
        }
        return ret;
    }

    private void UpdateDrawForm(int seikyuYm)
    {
        #region SubMethod
        // ヘッダー
        void UpdateFormHeader()
        {
            // 請求月
            SetFieldData("dfSeikyuYm", $"{seikyuYm / 100}年{seikyuYm % 100}月");

            // 発行日時
            int date = CIUtil.DateTimeToInt(printoutDateTime);
            SetFieldData("dfPrintDateTime", CIUtil.SDateToShowSDate(date) + "　" + printoutDateTime.ToString("HH:mm"));
        }


        // 本体
        void UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            int dataIndex = (currentPage - 1) * dataRowCount;

            if (printOutData == null || printOutData.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            for (short i = 0; i < dataRowCount; i++)
            {
                for (short j = 0; j < dataColCount; j++)
                {
                    listDataPerPage.Add(new("lsReceTarget", j, i, printOutData[dataIndex].LineDatas[j]));
                }
                listDataPerPage.Add(new("lsComment", 0, i, printOutData[dataIndex].Comment));

                dataIndex++;
                if (dataIndex >= printOutData.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
            _listTextData.Add(currentPage, listDataPerPage);
        }
        #endregion

        UpdateFormHeader();
        UpdateFormBody();
    }

    private CoReceTargetModel GetData(int hpId, int seikyuYm)
    {
        return _finder.FindReceInf(hpId, seikyuYm);
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
            new ObjectCalculate("lsReceTarget", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsReceTarget", (int)CalculateTypeEnum.GetListColCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.ReceTarget, formFileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);

        var responses = javaOutputData.responses;
        dataRowCount = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListRowCount && item.listName == "lsReceTarget")!.result;
        dataColCount = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListColCount && item.listName == "lsReceTarget")!.result;
    }
}
