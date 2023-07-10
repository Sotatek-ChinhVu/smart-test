using Reporting.Mappers.Common;

namespace Reporting.Kensalrai.Mapper
{
    public class KensalraiMapper : CommonReportingRequest
    {
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly List<Dictionary<string, CellModel>> _tableFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly string _rowCountFieldName;

        public KensalraiMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, string> extralData, string rowCountFieldName)
        {
            _singleFieldData = singleFieldData;
            _tableFieldData = tableFieldData;
            _extralData = extralData;
            _rowCountFieldName = rowCountFieldName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.KensaIrai;
        }

        public override string GetRowCountFieldName()
        {
            return _rowCountFieldName;
        }

        public override string GetJobName()
        {
            return "検査依頼書";
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
            var fileName = new Dictionary<string, string>();
            fileName.Add("1", "fmKensaIraiList.rse");
            return fileName;
        }
    }
}
