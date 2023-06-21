using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Get;

namespace EmrCloudApi.Presenters.Lock
{
    public class CheckLockVisitingPresenter : ICheckLockVisitingOutputPort
    {
        public Response<CheckLockVisitingResponse> Result { get; private set; } = new();

        public void Complete(CheckLockVisitingOutputData outputData)
        {
            Result.Data = new CheckLockVisitingResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            CheckLockVisitingStatus.None => ResponseMessage.None,
            CheckLockVisitingStatus.Locked => ResponseMessage.Locked,
            _ => string.Empty
        };
    }
}
