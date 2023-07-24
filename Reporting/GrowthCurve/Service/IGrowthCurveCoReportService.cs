using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;

namespace Reporting.GrowthCurve.Service;

public interface IGrowthCurveCoReportService
{
    CommonReportingRequestModel GetGrowthCurveA4PrintData(int hpId, GrowthCurveConfig growthCurveConfig);
}
