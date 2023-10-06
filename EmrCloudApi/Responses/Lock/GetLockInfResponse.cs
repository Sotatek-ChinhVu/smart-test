using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock
{
    public class GetLockInfResponse
    {
        public GetLockInfResponse(List<LockInfModel> lockInfs)
        {
            LockInfs = lockInfs;
        }

        public List<LockInfModel> LockInfs {  get; private set; }
    }
}