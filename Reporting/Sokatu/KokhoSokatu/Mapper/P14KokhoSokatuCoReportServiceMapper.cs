using Reporting.Mappers.Common;

namespace Reporting.Sokatu.KokhoSokatu.Mapper
{
    public class P14KokhoSokatuCoReportServiceMapper : CommonReportingRequest
    {
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<string, string> _fileName;
        private readonly Dictionary<string, bool> _visibleFieldData;

        public P14KokhoSokatuCoReportServiceMapper(Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, Dictionary<string, string> fileName, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData)
        {
            _setFieldData = setFieldData;
            _listTextData = listTextData;
            _extralData = extralData;
            _fileName = fileName;
            _singleFieldData = singleFieldData;
            _visibleFieldData = visibleFieldData;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.KokhoSokatu;
        }

        public override string GetRowCountFieldName()
        {
            return string.Empty;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return new();
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return _extralData;
        }

        public override string GetJobName()
        {
            return string.Empty;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return _singleFieldData;
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new();
        }

        public override Dictionary<int, List<ListTextObject>> GetListTextData()
        {
            return _listTextData;
        }

        public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
        {
            return _setFieldData;
        }
        public override Dictionary<string, string> GetFileNamePageMap()
        {
            return _fileName;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return _visibleFieldData;
        }
    }
}
