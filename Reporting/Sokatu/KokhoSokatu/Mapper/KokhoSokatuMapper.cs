using Reporting.Mappers.Common;

namespace Reporting.Sokatu.KokhoSokatu.Mapper
{
    public class KokhoSokatuMapper : CommonReportingRequest
    {
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly List<Dictionary<string, CellModel>> _tableFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<string, string> _fileNamePageMap;
        private readonly int _reportType;
        private readonly string _rowCountFieldName;

        public KokhoSokatuMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, string> extralData, string rowCountFieldName)
        {
            _singleFieldData = singleFieldData;
            _tableFieldData = tableFieldData;
            _extralData = extralData;
            _rowCountFieldName = rowCountFieldName;
        }

        public override int GetReportType()
        {
            return _reportType;
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
            return _extralData;
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
