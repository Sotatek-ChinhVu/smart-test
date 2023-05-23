using Reporting.Mappers.Common;

namespace Reporting.DrugNoteSeal.Service;

public interface IDrugNoteSealCoReportService
{
    CommonReportingRequestModel GetDrugNoteSealPrintData(int hpId, long ptId, int sinDate, long raiinNo);
}
