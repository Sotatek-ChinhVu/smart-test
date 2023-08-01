using Domain.Models.SystemConf;
using Helper.Common;
using Reporting.Calculate.Constants;
using Reporting.Calculate.Receipt.Constants;
using Reporting.Calculate.Receipt.Models;
using Reporting.Mappers.Common;
using Reporting.Receipt.Constants;
using Reporting.Receipt.DB;
using Reporting.Receipt.Models;
using static Helper.Common.CIUtil;

namespace Reporting.Receipt.Mapper
{
    public class ReceiptPreviewMapper : CommonReportingRequest
    {
        Dictionary<string, string> _fileName = new Dictionary<string, string>();
        Dictionary<string, string> _singleData = new Dictionary<string, string>();
        List<Dictionary<string, CellModel>> _cellData = new List<Dictionary<string, CellModel>>();

        public ReceiptPreviewMapper(List<Dictionary<string, CellModel>> cellData, Dictionary<string, string> singleData, Dictionary<string, string> fileName)
        {
            _cellData = cellData;
            _singleData = singleData;
            _fileName = fileName;
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
            return _cellData;
        }

        public override string GetJobName()
        {
            return "レセプト";
        }
    }
}
