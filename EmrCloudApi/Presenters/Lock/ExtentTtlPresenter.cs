using EmrCloudApi.Responses;
using UseCase.Lock.ExtendTtl;

namespace EmrCloudApi.Presenters.Lock
{
    public class ExtentTtlPresenter
    {
        public Response Result { get; private set; } = new();

        public void Complete(ExtendTtlLockOutputData outputData)
        {
            Result.Message = outputData.Status == ExtendTtlLockStatus.Successed ? "Successed" : "Failed";
            Result.Status = (int)outputData.Status;
        }
    }
}
