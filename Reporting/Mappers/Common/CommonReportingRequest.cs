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
            ReportConfigModel = new ReportConfigModel()
            {
                VisibleFieldList = GetVisibleFieldData(),
                WrapFieldList = GetWrapFieldData(),
                RowCountFieldName = GetRowCountFieldName(),
            }
        };
        return result;
    }

    public virtual int GetReportType()
    {
        throw new NotImplementedException();
    }

    public virtual string GetRowCountFieldName()
    {
        throw new NotImplementedException();
    }

    public virtual Dictionary<string, string> GetSingleFieldData()
    {
        throw new NotImplementedException();
    }

    public virtual List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        throw new NotImplementedException();
    }

    public virtual Dictionary<string, bool> GetVisibleFieldData()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public virtual Dictionary<string, string> GetExtralData()
    {
        return new();
    }

    public virtual string GetJobName()
    {
        return string.Empty;
    }

    public virtual List<ListTextObject> GetListTextData()
    {
        return new();
    }
}
