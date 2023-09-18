using Reporting.Mappers.Common;

namespace Reporting.Statistics.Sta1001.Mapper
{
    public class Sta1001Mapper : CommonReportingRequest
    {
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, string> _extralData;
        private readonly string _formFileName;
        private readonly Dictionary<string, bool> _visibleFieldData;

        public Sta1001Mapper(Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileName, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData)
        {
            _setFieldData = setFieldData;
            _listTextData = listTextData;
            _extralData = extralData;
            _formFileName = formFileName;
            _singleFieldData = singleFieldData;
            _visibleFieldData = visibleFieldData;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.Sta1001;
        }

        public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
        {
            return _setFieldData;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return _visibleFieldData;
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>();
            fileName.Add("1", "sta1001a.rse");
            return fileName;
        }

        public override Dictionary<int, List<ListTextObject>> GetListTextData()
        {
            return _listTextData;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return _singleFieldData;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return new();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override string GetRowCountFieldName()
        {
            return string.Empty;
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return _extralData;
        }
    }
}
