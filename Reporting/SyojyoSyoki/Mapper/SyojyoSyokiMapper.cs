using Reporting.Mappers.Common;

namespace Reporting.SyojyoSyoki.Mapper
{
    public class SyojyoSyokiMapper : CommonReportingRequest
    {
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly List<Dictionary<string, CellModel>> _tableFieldData;
        private readonly string _rowCountFieldName;

        public SyojyoSyokiMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, string rowCountFieldName)
        {
            _singleFieldData = singleFieldData;
            _tableFieldData = tableFieldData;
            _rowCountFieldName = rowCountFieldName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.SyojyoSyoki;
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

        public override string GetJobName()
        {
            return "症状詳記";
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>();
            fileName.Add("1", "fmSyojyoSyoki.rse");
            return fileName;
        }
    }
}
