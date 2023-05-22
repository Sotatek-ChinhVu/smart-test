using Reporting.Mappers.Common;

namespace Reporting.Yakutai.Mapper
{
    public class YakutaiMapper : CommonReportingRequest
    {
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _fileNamePageMap;
        private readonly List<Dictionary<string, CellModel>> _tableFieldData;
        private readonly string _rowCountFieldName;
        public YakutaiMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, string> fileNamePageMap, string rowCountFieldName)
        {
            _singleFieldData = singleFieldData;
            _tableFieldData = tableFieldData;
            _fileNamePageMap = fileNamePageMap;
            _rowCountFieldName = rowCountFieldName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.Yakutai;
        }

        public override string GetRowCountFieldName()
        {
            return _rowCountFieldName;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return _singleFieldData;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return _tableFieldData;
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return new();
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return new();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new();
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            return _fileNamePageMap;
        }
    }
}
