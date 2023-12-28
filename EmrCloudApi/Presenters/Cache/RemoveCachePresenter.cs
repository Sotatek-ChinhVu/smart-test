using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Cache;
using UseCase.Cache;

namespace EmrCloudApi.Presenters.Cache
{
    public class RemoveCachePresenter : IRemoveCacheOutputPort
    {
        public Response<RemoveCacheResponse> Result { get; private set; } = new();
        public void Complete(RemoveCacheOutputData outputData)
        {
            Result.Data = new RemoveCacheResponse(outputData.Status == RemoveCacheStaus.Successed);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(RemoveCacheStaus status) => status switch
        {
            RemoveCacheStaus.Successed => ResponseMessage.Success,
            RemoveCacheStaus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
