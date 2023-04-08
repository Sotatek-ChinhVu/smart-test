namespace Reporting.Mappers.Common
{

    public class CommonReportingRequest : ICommonReportingRequest
    {
        public CommonReportingRequestModel GetData()
        {
            CommonReportingRequestModel result = new CommonReportingRequestModel()
            {
                ReportType = GetReportType(),
                SingleFieldData = GetSingleFieldData(),
                TableFieldData = GetTableFieldData(),
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
            return (int)CoReportType.Common;
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

        public virtual Dictionary<string, bool> GetWrapFieldData()
        {
            throw new NotImplementedException();
        }
    }
}
