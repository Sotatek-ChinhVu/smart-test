using Reporting.Mappers.Common;
using Reporting.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP24WelfareSyomeiCoReportService
    {
        CommonReportingRequestModel GetP24WelfareSyomeiReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<long> printPtIds);
    }
}
