using Reporting.Mappers.Common;
using Reporting.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP43KikuchiMeisai43CoReportService
    {
        CommonReportingRequestModel GetP43KikuchiMeisai43ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
