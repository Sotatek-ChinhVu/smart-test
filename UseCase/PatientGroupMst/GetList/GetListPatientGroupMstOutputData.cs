using Domain.Models.PatientGroupMst;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientGroupMst.GetList
{
    public class GetListPatientGroupMstOutputData : IOutputData
    {
        public List<PatientGroupMstModel> PatientGroupMstModels { get; private set; }

        public GetListPatientGroupMstOutputData(List<PatientGroupMstModel> patientGroupMstModels)
        {
            PatientGroupMstModels = patientGroupMstModels;
        }
    }
}
