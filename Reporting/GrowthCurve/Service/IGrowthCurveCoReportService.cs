using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.GrowthCurve.Service
{
    public interface IGrowthCurveCoReportService
    {
        CommonReportingRequestModel GetGrowthCurvePrintData(int hpId, GrowthCurveConfig growthCurveConfig);
    }
}
