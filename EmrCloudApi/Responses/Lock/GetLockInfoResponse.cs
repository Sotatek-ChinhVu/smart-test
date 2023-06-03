using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock
{
    public class GetLockInfoResponse
    {
        public GetLockInfoResponse(List<LockModel> lockInfs)
        {
            LockInfs = lockInfs;
        }

        public List<LockModel> LockInfs { get; private set; }
    }
}
