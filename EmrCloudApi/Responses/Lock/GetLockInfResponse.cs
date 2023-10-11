using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock
{
    public class GetLockInfResponse
    {
        public GetLockInfResponse(List<LockInfModel> lockInfs, Dictionary<int, Dictionary<int, string>> userLocks)
        {
            LockInfs = lockInfs;
            UserLocks = userLocks;
        }

        public List<LockInfModel> LockInfs {  get; private set; }

        public Dictionary<int, Dictionary<int, string>> UserLocks { get; private set; }
    }
}