﻿using Reporting.Mappers.Common;
using Reporting.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public interface IP17KokhoSeikyuCoReportService
    {
        CommonReportingRequestModel GetP17KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos);
    }
}
