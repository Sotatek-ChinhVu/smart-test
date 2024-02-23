using UseCase.Core.Sync.Core;

namespace UseCase.PatientGroupMst.GetList
{
    public class GetListPatientGroupMstInputData : IInputData<GetListPatientGroupMstOutputData>
    {
        public GetListPatientGroupMstInputData(int hpId)
        {
            HpId = hpId;
        }
        public int HpId { get; private set; }
    }
}
