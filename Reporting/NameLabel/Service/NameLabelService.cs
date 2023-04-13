using Infrastructure.Interfaces;
using Reporting.Mappers.Common;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Mapper;
using Reporting.NameLabel.Models;

namespace Reporting.NameLabel.Service;

public class NameLabelService : INameLabelService
{
    private readonly ITenantProvider _tenantProvider;

    public NameLabelService(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    public CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
    {
        CoNameLabelModel coModel = GetData(ptId, kanjiName, sinDate);
        return new NameLabelMapper(coModel).GetData();
    }

    private CoNameLabelModel GetData(long ptId, string kanjiName, int sinDate)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoNameLabelFinder(noTrackingDataContext);

            // 患者情報
            var ptInf = finder.FindPtInf(ptId);

            return new CoNameLabelModel(ptInf, kanjiName, sinDate);
        }
    }
}
