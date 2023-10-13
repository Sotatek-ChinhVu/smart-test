using UseCase.Lock.Unlock;

namespace EmrCloudApi.Requests.Lock
{
    public class UnlockRequest
    {
        public List<LockInfInputItem> LockInfModels { get; set; } = new();

        public int ManagerKbn {  get; set; }
    }

}
