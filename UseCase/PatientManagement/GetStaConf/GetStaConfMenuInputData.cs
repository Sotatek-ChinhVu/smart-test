using UseCase.Core.Sync.Core;

namespace UseCase.PatientManagement.GetStaConf
{
    public class GetStaConfMenuInputData : IInputData<GetStaConfMenuOutputData>
    {
        public GetStaConfMenuInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
