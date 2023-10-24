using Reporting.Mappers.Common;

namespace Reporting.Receipt.Mapper
{
    public class ReceiptPreviewMapper : CommonReportingRequest
    {
        Dictionary<string, string> _fileName = new Dictionary<string, string>();
        Dictionary<string, string> _extralData = new Dictionary<string, string>();
        Dictionary<int, List<ListTextObject>> _listTextData = new Dictionary<int, List<ListTextObject>>();
        Dictionary<int, Dictionary<string, string>> _setFieldData = new Dictionary<int, Dictionary<string, string>>();

        public ReceiptPreviewMapper(Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, Dictionary<string, string> fileName)
        {
            _setFieldData = setFieldData;
            _listTextData = listTextData;
            _extralData = extralData;
            _fileName = fileName;
        }

        public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
        {
            return _setFieldData;
        }

        public override Dictionary<int, List<ListTextObject>> GetListTextData()
        {
            return _listTextData;
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
            return new();
        }

        public override string GetRowCountFieldName()
        {
            return string.Empty;
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
            return "レセプトプレビュー";
        }
    }
}
