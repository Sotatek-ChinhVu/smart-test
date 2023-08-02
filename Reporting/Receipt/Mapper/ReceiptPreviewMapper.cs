using Reporting.Mappers.Common;

namespace Reporting.Receipt.Mapper
{
    public class ReceiptPreviewMapper : CommonReportingRequest
    {
        Dictionary<string, string> _fileName = new Dictionary<string, string>();
        Dictionary<string, string> _singleData = new Dictionary<string, string>();
        Dictionary<string, string> _extralData = new Dictionary<string, string>();

        public ReceiptPreviewMapper(Dictionary<string, string> extralData, Dictionary<string, string> singleData, Dictionary<string, string> fileName)
        {
            _extralData = extralData;
            _singleData = singleData;
            _fileName = fileName;
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return _extralData;
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            return _fileName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.Receipt;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return _singleData;
        }

        public override string GetRowCountFieldName()
        {
            return "lsTekiyo";
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return new();
        }

        public override string GetJobName()
        {
            return "レセプト";
        }
    }
}
