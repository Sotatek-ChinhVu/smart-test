using Reporting.Mappers.Common;
using Reporting.OrderLabel.Model;

namespace Reporting.Mappers;

public class OrderLabelMapper : CommonReportingRequest
{
    private List<CoOrderLabelPrintDataModel> _printOutData;

    public OrderLabelMapper(List<CoOrderLabelPrintDataModel> printOutData)
    {
        _printOutData = printOutData;
    }

    public override List<string> GetFormNameList()
    {
        return new List<string>() { "fmOrderLabel.rse" };
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        Dictionary<string, string> data = new();

        return data;
    }

    public override List<Dictionary<string, string>> GetTableFieldData()
    {
        if (_printOutData == null)
        {
            return new();
        }

        List<Dictionary<string, string>> result = new();
        foreach (var item in _printOutData)
        {
            Dictionary<string, string> data = new();
            data.Add("lsOdrKbn", item.InOut);
            data.Add("lsRpNo", item.RpNo);
            data.Add("lsOrder", item.Data);
            data.Add("lsOrderWide", item.DataWide);
            data.Add("lsComment", item.Comment);
            data.Add("lsSuryo", item.Suuryo);
            data.Add("lsTani", item.Tani);
            result.Add(data);
        }
        return result;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        if (_printOutData == null ||
            !_printOutData.Any())
        {
            return new Dictionary<string, bool>();
        }
        Dictionary<string, bool> data = new();
        return data;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new Dictionary<string, bool>() {
                   { "lsOrder", true },
                   { "lsOrderWide", true },
                   { "lsComment", true },
                   { "lsSuryo", true }
        };
    }
}
