using UseCase.Core.Sync.Core;

namespace UseCase.Lock.GetLockInf
{
    public class GetLockInfInputData : IInputData<GetLockInfOutputData>
    {
        public GetLockInfInputData(int hpId) 
        {
            HpId = hpId;
        }

        public int HpId {  get; private set; }
    }
}
