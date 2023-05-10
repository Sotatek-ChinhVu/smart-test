using Reporting.Mappers.Common;

namespace Reporting.PatientManagement.Service;

public interface IPatientManagementService
{
    CommonReportingRequestModel PrintData(int hpId, int menuId);
}
