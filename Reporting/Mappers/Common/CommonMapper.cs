namespace Reporting.Mappers.Common
{
    public class CommonMapper : CommonReportingRequest
    {
        private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM = new Dictionary<int, Dictionary<string, string>>();
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<int, List<ListTextObject>> _listTextData = new Dictionary<int, List<ListTextObject>>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> _visibleFieldData = new Dictionary<string, bool>();
        private readonly string _formFileName;
        private readonly int _reportType;

        public CommonMapper(Dictionary<int, Dictionary<string, string>> singleFieldDataM, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileName, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData, int reportType)
        {
            _singleFieldDataM = singleFieldDataM;
            _listTextData = listTextData;
            _extralData = extralData;
            _formFileName = formFileName;
            _singleFieldData = singleFieldData;
            _visibleFieldData = visibleFieldData;
            _reportType = reportType;
        }

        public override int GetReportType()
        {
            return _reportType;
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
            return _singleFieldDataM;
        }
        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>
        {
            { "1", _formFileName }
        };
            return fileName;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return _visibleFieldData;
        }
    }
}
