using Reporting.Mappers.Common;

namespace Reporting.DrugNoteSeal.Mapper;

public class DrugNoteSealMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly string _rowCountFileName;

    public DrugNoteSealMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, string rowCountFileName)
    {
        _singleFieldData = singleFieldData;
        _rowCountFileName = rowCountFileName;
        _tableFieldData = tableFieldData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.DrugNoteSeal;
    }

    public override string GetRowCountFieldName()
    {
        return _rowCountFileName;
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return new();
    }

    public override string GetJobName()
    {
        return "お薬手帳シール";
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return _singleFieldData;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return _tableFieldData;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return new();
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return new();
    }

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        return new();
    }
}
