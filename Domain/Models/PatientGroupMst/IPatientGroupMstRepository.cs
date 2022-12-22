using Domain.Common;

namespace Domain.Models.PatientGroupMst
{
    public interface IPatientGroupMstRepository : IRepositoryBase
    {
        List<PatientGroupMstModel> GetAll();

        bool SaveListPatientGroup(int hpId, int userId, List<PatientGroupMstModel> patientGroupMstModels);
    }
}
