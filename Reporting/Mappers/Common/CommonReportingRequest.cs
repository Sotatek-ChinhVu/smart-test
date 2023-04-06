namespace Reporting.Mappers.Common
{
    public class CommonReportingRequest : ICommonReportingRequest
    {
        public CommonReportingRequestModel GetData()
        {
            CommonReportingRequestModel result = new CommonReportingRequestModel()
            {
                FormNameList = GetFormNameList(),
                SingleFieldData = GetSingleFieldData(),
                TableFieldData = GetTableFieldData(),
                VisibleFieldData = GetVisibleFieldData(),
                WrapFieldData = GetWrapFieldData()
            };
            return result;
        }

        public virtual List<string> GetFormNameList()
        {
            throw new NotImplementedException();
        }

        public virtual Dictionary<string, string> GetSingleFieldData()
        {
            throw new NotImplementedException();
        }

        public virtual List<Dictionary<string, string>> GetTableFieldData()
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
