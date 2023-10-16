using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Unlock;

namespace EmrCloudApi.Presenters.Lock
{
    public class UnlockPresenter : IUnlockOutputPort
    {
        public Response<UnlockResponse> Result { get; private set; } = default!;

        public void Complete(UnlockOutputData outputData)
        {
            Result = new Response<UnlockResponse>()
            {
                Data = new UnlockResponse(outputData.Status == UnlockStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(UnlockStatus status) => status switch
        {
            UnlockStatus.Success => ResponseMessage.Success,
            UnlockStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
