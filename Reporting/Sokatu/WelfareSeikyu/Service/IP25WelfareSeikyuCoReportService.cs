﻿using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP25WelfareSeikyuCoReportService
    {
        CommonReportingRequestModel GetP25WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}