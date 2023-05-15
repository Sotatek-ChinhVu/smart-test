﻿using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3060.Models;

namespace Reporting.Statistics.Sta3060.Service;

public interface ISta3060CoReportService
{
    CommonReportingRequestModel GetSta3060ReportingData(CoSta3060PrintConf printConf, int hpId, CoFileType outputFileType);
}
