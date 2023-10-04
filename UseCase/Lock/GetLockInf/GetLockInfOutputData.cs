using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.GetLockInf
{
    public class GetLockInfOutputData : IOutputData
    {
        public GetLockInfOutputData(List<LockInfModel> lockInfs, GetLockInfStatus status, Dictionary<int, Dictionary<int, string>> value)
        {
            LockInfs = lockInfs;
            Status = status;
            Value = value;
        }

        public List<LockInfModel> LockInfs { get; private set; }

        public GetLockInfStatus Status { get; private set; }

        public Dictionary<int, Dictionary<int, string>> Value {  get; private set; }
    }
}
