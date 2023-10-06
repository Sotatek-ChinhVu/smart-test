using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.GetLockInf
{
    public class GetLockInfOutputData : IOutputData
    {
        public GetLockInfOutputData(List<LockInfModel> lockInfs, GetLockInfStatus status)
        {
            LockInfs = lockInfs;
            Status = status;
        }

        public List<LockInfModel> LockInfs { get; private set; }

        public GetLockInfStatus Status { get; private set; }
    }
}
