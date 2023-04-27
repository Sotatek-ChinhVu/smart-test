using Reporting.Mappers.Common;

namespace Reporting.Statistics.Sta1001.Mapper
{
    public class Sta1001Mapper : CommonReportingRequest
    {
        Dictionary<string, string> _extralData = new Dictionary<string, string>();
        Dictionary<string, string> SingleData = new Dictionary<string, string>();
        List<Dictionary<string, CellModel>> CellData = new List<Dictionary<string, CellModel>>();
        private string _rowCountFieldName;

        public Sta1001Mapper(Dictionary<string, string> extralData, Dictionary<string, string> singleData, List<Dictionary<string, CellModel>> cellData, string rowCountFieldName)
        {
            _extralData = extralData;
            SingleData = singleData;
            CellData = cellData;
            _rowCountFieldName = rowCountFieldName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.Sta1001;
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>();
            fileName.Add("1", "sta1001a.rse");
            return fileName;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return SingleData;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return CellData;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override string GetRowCountFieldName()
        {
            return _rowCountFieldName;
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return _extralData;
        }
    }
}
