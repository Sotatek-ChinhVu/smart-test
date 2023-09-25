using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta9000.Models;

namespace Reporting.Statistics.Sta9000.Service;

public interface ISta9000CoReportService
{
    CommonReportingRequestModel GetSta9000ReportingData(int hpId, int reportType, int sortOrder, int sortOrder2, int sortOrder3,
            CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
            CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
            CoSta9000KensaConf? kensaConf);
    (string, CoPrintExitCode, List<string>) OutPutFile(int hpId, List<string> outputColumns, bool isPutColName, CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
            CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
            CoSta9000KensaConf? kensaConf, List<long> ptIds, int sortOrder, int sortOrder2, int sortOrder3);

}
