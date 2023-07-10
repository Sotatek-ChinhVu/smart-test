using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Get;

namespace EmrCloudApi.Presenters.Lock
{
    public class GetLockInfoPresenter : IGetLockInfoOutputPort
    {
        public Response<GetLockInfoResponse> Result { get; private set; } = new();

        public void Complete(GetLockInfoOutputData outputData)
        {
            Result.Data = new GetLockInfoResponse(outputData.LockInfs);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetLockInfoStatus.Successed => ResponseMessage.Success,
            GetLockInfoStatus.Failed => ResponseMessage.Failed,
            GetLockInfoStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
