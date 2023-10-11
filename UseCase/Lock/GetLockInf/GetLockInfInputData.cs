using UseCase.Core.Sync.Core;

namespace UseCase.Lock.GetLockInf
{
    public class GetLockInfInputData : IInputData<GetLockInfOutputData>
    {
        public GetLockInfInputData(int hpId, int userId, byte managerKbn) 
        {
            HpId = hpId;
            UserId = userId;
            ManagerKbn = managerKbn;
        }

        public int HpId {  get; private set; }

        public int UserId { get; private set; }

        public byte ManagerKbn {  get; private set; }
    }
}
