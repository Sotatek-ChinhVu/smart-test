using Reporting.Mappers.Common;

namespace Reporting.Yakutai.Mapper
{
    public class YakutaiMapper : CommonReportingRequest
    {
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _fileNamePageMap;
        private readonly string _rowCountFieldName;
        private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, string> _extralData;

        public YakutaiMapper(Dictionary<string, string> singleFieldData, Dictionary<string, string> fileNamePageMap,  string rowCountFieldName, Dictionary<int, Dictionary<string, string>> singleFieldDataM, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData)
        {
            _singleFieldData = singleFieldData;
            _fileNamePageMap = fileNamePageMap;
            _rowCountFieldName = rowCountFieldName;
            _singleFieldDataM = singleFieldDataM;
            _listTextData = listTextData;
            _extralData = extralData;
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return _extralData;
        }

        public override Dictionary<int, List<ListTextObject>> GetListTextData()
        {
            return _listTextData;
        }

        public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
        {
            return _singleFieldDataM;
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
            return "薬袋ラベル";
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            return _fileNamePageMap;
        }
    }
}
