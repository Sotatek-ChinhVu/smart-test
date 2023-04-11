﻿using Reporting.Mappers.Common;

namespace Reporting.Sijisen.Service;

public interface ISijisenReportService
{
    CommonReportingRequestModel GetSijisenReportingData(int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr);
}
