using Reporting.PatientManagement.Models;

namespace Reporting.PatientManagement.DB;

public interface IPatientManagementFinder
{
    PatientManagementModel GetPatientManagement(int hpId, int menuId);

    List<GroupInfoSearchPatientModel> GetListGroupInfo(int hpId);
}
