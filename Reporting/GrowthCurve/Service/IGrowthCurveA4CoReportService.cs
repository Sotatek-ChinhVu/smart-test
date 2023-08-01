using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;

namespace Reporting.GrowthCurve.Service;

public interface IGrowthCurveA4CoReportService
{
    CommonReportingRequestModel GetGrowthCurveA4PrintData(int hpId, GrowthCurveConfig growthCurveConfig);
}
