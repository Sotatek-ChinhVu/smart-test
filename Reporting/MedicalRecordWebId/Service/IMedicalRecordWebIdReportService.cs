using Reporting.Mappers.Common;

namespace Reporting.MedicalRecordWebId.Service;

public interface IMedicalRecordWebIdReportService
{
    CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate);
}
