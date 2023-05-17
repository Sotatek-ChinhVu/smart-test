using Amazon.Runtime.Internal.Transform;
using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Memo.Mapper;

namespace Reporting.Memo.Service;

public class MemoMsgCoReportService : IMemoMsgCoReportService
{
    private const int Max_Length = 90;
    private const int Max_Line = 66;

    private readonly Dictionary<string, string> _extralData;

    private int currentPage;
    private bool hasNextPage;
    private List<string> printOutData;
    private string reportName, title;
    private int _dataRowCount;

    public MemoMsgCoReportService()
    {
        _extralData = new();
        reportName = string.Empty;
        title = string.Empty;
        printOutData = new();
    }

    public CommonReportingRequestModel GetMemoMsgReportingData(string reportName, string title, List<string> listMessage)
    {
        this.reportName = reportName;
        this.title = title;
        printOutData = new List<string>();

        foreach (var item in listMessage)
        {
            var strAdd = item;
            if (string.IsNullOrEmpty(strAdd))
            {
                printOutData.Add(strAdd);
            }
            else
            {
                while (!string.IsNullOrEmpty(strAdd))
                {
                    var strBuf = CIUtil.CiCopyStrWidth(strAdd, 1, Max_Length, 0);
                    printOutData.Add(strBuf);

                    strAdd = CIUtil.Copy(strAdd, strBuf.Length, strAdd.Length - strBuf.Length);
                }
            }
        }
        hasNextPage = true;
        currentPage = 1;

        //印刷
        while (hasNextPage)
        {
            UpdateDrawForm();
            currentPage++;
        }
        _extralData.Add("totalPage", (currentPage - 1).ToString());

        return new MemoMsgMapper(reportName, _extralData).GetData();
    }

    private void UpdateDrawForm()
    {
        int linePrint = 1;
        #region SubMethod

        void PrintText(string textPrint)
        {
            var strAdd = textPrint;
            if (string.IsNullOrEmpty(strAdd))
            {
                SetFieldData("Line" + linePrint, strAdd);
                linePrint++;
            }
            else
            {
                while (!string.IsNullOrEmpty(strAdd))
                {
                    var strBuf = CIUtil.CiCopyStrWidth(strAdd, 1, Max_Length, 0);
                    SetFieldData("Line" + linePrint, strBuf);
                    linePrint++;

                    strAdd = strAdd.Substring(strBuf.Length);
                }
            }
        }

        // 本体
        void UpdateFormBody()
        {
            int dataIndex = _dataRowCount;
            if (printOutData == null || printOutData.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            if (currentPage == 1)
            {
                if (!string.IsNullOrEmpty(reportName))
                {
                    PrintText(reportName);

                    string str = "";
                    PrintText(str.PadLeft(Max_Length, '-'));
                }

                if (!string.IsNullOrEmpty(title))
                {
                    string[] lines = title.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    var listTitle = lines.ToList();
                    foreach (var item in listTitle)
                    {
                        PrintText(item);
                    }
                }
            }

            for (int i = dataIndex; i < printOutData.Count; i++)
            {
                PrintText(printOutData[i]);

                dataIndex++;
                _dataRowCount = dataIndex;

                if (dataIndex >= printOutData.Count)
                {
                    hasNextPage = false;
                    break;
                }

                if (linePrint == Max_Line)
                {
                    return;
                }
            }
        }

        #endregion

        UpdateFormBody();
    }

    private void SetFieldData(string field, string value)
    {
        field = field + "_" + currentPage;
        if (!string.IsNullOrEmpty(field) && !_extralData.ContainsKey(field))
        {
            _extralData.Add(field, value);
        }
    }
}
