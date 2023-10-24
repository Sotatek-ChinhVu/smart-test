using Reporting.Mappers.Common;
using Reporting.PatientManagement.Models;

namespace Reporting.PatientManagement.Service;

public interface IPatientManagementService
{
    CommonReportingRequestModel PrintData(int hpId, PatientManagementModel patientManagementModel);
}
