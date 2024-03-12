namespace Reporting.Mappers.Common;

public class CommonReportingRequest : ICommonReportingRequest
{
    public CommonReportingRequestModel GetData()
    {
        CommonReportingRequestModel result = new CommonReportingRequestModel()
        {
            ReportType = GetReportType(),
            JobName = GetJobName(),
            FileNamePageMap = GetFileNamePageMap(),
            SingleFieldList = GetSingleFieldData(),
            TableFieldData = GetTableFieldData(),
            SystemConfigList = GetSystemConfigList(),
            ExtralData = GetExtralData(),
            ListTextData = GetListTextData(),
            SetFieldData = GetSetFieldData(),
            DrawTextData = GetDrawTextData(),
            DrawBoxData = GetDrawBoxData(),
            DrawCircleData = GetDrawCircleData(),
            DrawLineData = GetDrawLineData(),
            ReportConfigPerPage = GetReportConfigModelPerPage(),
            ReportConfigModel = new ReportConfigModel()
            {
                VisibleFieldList = GetVisibleFieldData(),
                WrapFieldList = GetWrapFieldData(),
                RowCountFieldName = GetRowCountFieldName(),
                VisibleAtPrint = GetVisibleAtPrint(),
            }
        };
        return result;
    }

    public virtual int GetReportType()
    {
        return new();
    }

    public virtual string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public virtual Dictionary<string, string> GetSingleFieldData()
    {
        return new();
    }

    public virtual List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return new();
    }

    public virtual Dictionary<string, bool> GetVisibleFieldData()
    {
        return new();
    }

    public virtual Dictionary<string, bool> GetVisibleAtPrint()
    {
        return new();
    }

    public virtual Dictionary<string, string> GetSystemConfigList()
    {
        return new();
    }

    public virtual Dictionary<string, string> GetFileNamePageMap()
    {
        return new();
    }

    public virtual Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }

    public virtual Dictionary<string, string> GetExtralData()
    {
        return new();
    }

    public virtual string GetJobName()
    {
        return string.Empty;
    }

    public virtual Dictionary<int, Dictionary<int, List<ListDrawTextObject>>> GetDrawTextData()
    {
        return new();
    }

    public virtual Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return new();
    }

    public virtual Dictionary<int, Dictionary<string, string>> GetSetFieldData()
    {
        return new();
    }

    public virtual Dictionary<int, ReportConfigModel> GetReportConfigModelPerPage()
    {
        return new();
    }

    public virtual Dictionary<int, Dictionary<int, List<ListDrawLineObject>>> GetDrawLineData()
    {
        return new();
    }

    public virtual Dictionary<int, Dictionary<int, List<ListDrawBoxObject>>> GetDrawBoxData()
    {
        return new();
    }

    public virtual Dictionary<int, Dictionary<int, List<ListDrawCircleObject>>> GetDrawCircleData()
    {
        return new();
    }
}
