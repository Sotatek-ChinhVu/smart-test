using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;

namespace Reporting.GrowthCurve.Service;

public interface IGrowthCurveA5CoReportService
{
    CommonReportingRequestModel GetGrowthCurveA5PrintData(int hpId, GrowthCurveConfig growthCurveConfig);
}
