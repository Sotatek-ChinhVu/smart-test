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
        private readonly string _formFileName;

        public KokhoSokatuMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, string> extralData, Dictionary<string, string> fileNamePageMap, string rowCountFieldName, int reportType)
        {
            _singleFieldData = singleFieldData;
            _tableFieldData = tableFieldData;
            _extralData = extralData;
            _fileNamePageMap = fileNamePageMap;
            _rowCountFieldName = rowCountFieldName;
            _reportType = reportType;
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
