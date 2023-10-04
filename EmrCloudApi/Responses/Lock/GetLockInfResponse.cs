using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock
{
    public class GetLockInfResponse
    {
        public GetLockInfResponse(List<LockInfModel> lockInfs, Dictionary<int, Dictionary<int, string>> value)
        {
            LockInfs = lockInfs;
            Value = value;
        }

        public List<LockInfModel> LockInfs {  get; private set; }

        public Dictionary<int, Dictionary<int, string>> Value { get; private set; }
    }
}