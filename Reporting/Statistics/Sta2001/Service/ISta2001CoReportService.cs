﻿using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2001.Models;

namespace Reporting.Statistics.Sta2001.Service;

public interface ISta2001CoReportService
{
    CommonReportingRequestModel GetSta2001ReportingData(CoSta2001PrintConf printConf, int hpId);
}
