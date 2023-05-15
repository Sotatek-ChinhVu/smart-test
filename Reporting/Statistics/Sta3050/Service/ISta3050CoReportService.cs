﻿using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3050.Models;

namespace Reporting.Statistics.Sta3050.Service;

public interface ISta3050CoReportService
{
    CommonReportingRequestModel GetSta3050ReportingData(CoSta3050PrintConf printConf, int hpId, CoFileType outputFileType);
}
