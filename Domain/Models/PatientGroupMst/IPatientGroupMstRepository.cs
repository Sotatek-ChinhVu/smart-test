namespace Domain.Models.PatientGroupMst
{
    public interface IPatientGroupMstRepository
    {
        List<PatientGroupMstModel> GetAll();

        bool SaveListPatientGroup(int hpId, int userId, List<PatientGroupMstModel> patientGroupMstModels);
    }
}
