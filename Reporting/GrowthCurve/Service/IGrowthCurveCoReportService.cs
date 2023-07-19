using Entity.Tenant;
using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;

namespace Reporting.GrowthCurve.Service
{
    public interface IGrowthCurveCoReportService
    {
        CommonReportingRequestModel GetGrowthCurvePrintData(int hpId, GrowthCurveConfig growthCurveConfig);
    }
}
