﻿using Reporting.Mappers.Common;

namespace Reporting.Statistics.Sta3080.Mapper;

public class Sta3080Mapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly string _rowCountFieldName;
    private readonly string _formFileName;

    public Sta3080Mapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, string> extralData, Dictionary<string,bool> visibleFieldData, string rowCountFieldName, string formFileName)
    {
        _singleFieldData = singleFieldData;
        _tableFieldData = tableFieldData;
        _extralData = extralData;
        _rowCountFieldName = rowCountFieldName;
        _formFileName = formFileName;
        _visibleFieldData = visibleFieldData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.Sta3080;
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
    public override Dictionary<string, string> GetFileNamePageMap()
    {
        var fileName = new Dictionary<string, string>
        {
            { "1", _formFileName }
        };
        return fileName;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return _visibleFieldData;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }
}
