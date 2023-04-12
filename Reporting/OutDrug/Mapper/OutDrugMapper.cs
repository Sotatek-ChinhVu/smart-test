using Reporting.Mappers.Common;

namespace Reporting.OutDrug.Mapper;

public class OutDrugMapper : CommonReportingRequest
{
    private readonly List<CoSijisenPrintDataModel> _printOutData = new();
    private readonly CoSijisenModel _coModel;
    private readonly List<CoRaiinKbnMstModel> _raiinKbnMstList;
    private readonly DateTime _printoutDateTime;
    private readonly int _formType;
    private readonly ISystemConfig _systemConfig;

    private const int _dataCharCount = 68;
    private const int _suryoCharCount = 9;
    private const int _unitCharCount = 8;
    private const int _dataRowCount = 40;

    public SijisenMapper(int formType, CoSijisenModel coSijisen, List<CoRaiinKbnMstModel> raiinKbnMstList, ISystemConfig systemConfig)
    {
        _coSijisen = coSijisen;
        _raiinKbnMstList = raiinKbnMstList;
        _printoutDateTime = DateTime.Now;
        _formType = formType;
        _systemConfig = systemConfig;
    }

    #region override function

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return new Dictionary<string, bool>();
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new Dictionary<string, bool>();
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        Dictionary<string, string> data = new();

        return data;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        List<Dictionary<string, CellModel>> result = new ();

        foreach (var item in _printOutData)
        {
            Dictionary<string, CellModel> data = new Dictionary<string, CellModel>();

            data.Add("lsOdrKbn", new CellModel(item.Sikyu));

            result.Add(data);
        }

        return result;
    }

    public override int GetReportType()
    {
        if (_formType == (int)CoSijisenFormType.Sijisen)
        {
            return (int)CoReportType.Sijisen;
        }
        else
        {
            return (int)CoReportType.JyusinHyo;
        }
    }

    public override string GetRowCountFieldName()
    {
        return "lsData";
    }

    #endregion
}
