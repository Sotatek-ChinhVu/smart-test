﻿using Reporting.Mappers.Common;

namespace Reporting.Sokatu.KoukiSeikyu.Mapper;

public class P45KoukiSeikyuMapper : CommonReportingRequest
{
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private readonly string _formFileNameP1;
    private readonly string _formFileNameP2;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;

    public P45KoukiSeikyuMapper(Dictionary<int, ReportConfigModel> reportConfigPerPage, Dictionary<int, Dictionary<string, string>> singleFieldDataM, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileNameP1, string formFileNameP2, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData)
    {
        _reportConfigPerPage = reportConfigPerPage;
        _setFieldData = singleFieldDataM;
        _listTextData = listTextData;
        _extralData = extralData;
        _formFileNameP1 = formFileNameP1;
        _formFileNameP2 = formFileNameP2;
        _singleFieldData = singleFieldData;
        _visibleFieldData = visibleFieldData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.KoukiSeikyu;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return new();
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return _extralData;
    }

    public override string GetJobName()
    {
        return string.Empty;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return _singleFieldData;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
    {
        return _setFieldData;
    }
    public override Dictionary<string, string> GetFileNamePageMap()
    {
        var fileName = new Dictionary<string, string>
        {
            { "1", _formFileNameP1 }, { "2", _formFileNameP2 }, { "3", _formFileNameP1 },{ "4", _formFileNameP1 }
        };
        return fileName;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return _visibleFieldData;
    }

    public override Dictionary<int, ReportConfigModel> GetReportConfigModelPerPage()
    {
        return _reportConfigPerPage;
    }
}
