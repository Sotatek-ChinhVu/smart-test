using UseCase.Lock.Unlock;

namespace EmrCloudApi.Requests.Lock
{
    public class UnlockRequest
    {
        public List<LockInfInputItem> LockInfModels { get; set; } = new();

        /*public List<LockPtInfInputItem> PatientInfoModels { get; set; } = new();

        public List<LockCalcStatusInputItem> CalcStatusModels { get; set; } = new();

        public List<LockDocInfInputItem> DocInfModels { get; set; } = new();*/
    }

}
