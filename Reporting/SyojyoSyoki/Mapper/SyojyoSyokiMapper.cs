using Reporting.Mappers.Common;

namespace Reporting.SyojyoSyoki.Mapper
{
    public class SyojyoSyokiMapper : CommonReportingRequest
    {
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly List<Dictionary<string, CellModel>> _tableFieldData;
        private readonly string _rowCountFieldName;
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, string> _extralData;

        public SyojyoSyokiMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, string rowCountFieldName, Dictionary<int, Dictionary<string, string>> setFieldData,
            Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData)
        {
            _singleFieldData = singleFieldData;
            _tableFieldData = tableFieldData;
            _rowCountFieldName = rowCountFieldName;
            _setFieldData = setFieldData;
            _listTextData = listTextData;
            _extralData = extralData;
        }

        public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
        {
            return _setFieldData;
        }

        public override Dictionary<int, List<ListTextObject>> GetListTextData()
        {
            return _listTextData;
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
