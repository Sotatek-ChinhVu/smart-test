using Reporting.Mappers.Common;
using Reporting.OutDrug.Model;

namespace Reporting.OutDrug.Service;

public interface IOutDrugCoReportService
{
    CommonReportingRequestModel GetOutDrugReportingData(int hpId, List<ReceptionDtoReq> receptionDtos);
}
