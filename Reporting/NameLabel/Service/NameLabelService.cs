using Reporting.Mappers.Common;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Mapper;
using Reporting.NameLabel.Models;

namespace Reporting.NameLabel.Service;

public class NameLabelService : INameLabelService
{
    private readonly ICoNameLabelFinder _coNameLabelFinder;

    public NameLabelService(ICoNameLabelFinder coNameLabelFinder)
    {
        _coNameLabelFinder = coNameLabelFinder;
    }

    public CommonReportingRequestModel GetNameLabelReportingData(int hpId, long ptId, string kanjiName, int sinDate)
    {
        try
        {
            CoNameLabelModel coModel = GetData(hpId, ptId, kanjiName, sinDate);
            return new NameLabelMapper(coModel).GetData();
        }
        finally
        {
            _coNameLabelFinder.ReleaseResource();
        }
    }

    private CoNameLabelModel GetData(int hpId, long ptId, string kanjiName, int sinDate)
    {
        // 患者情報
        var ptInf = _coNameLabelFinder.FindPtInf(hpId, ptId);

        return new CoNameLabelModel(ptInf, kanjiName, sinDate);
    }
}
