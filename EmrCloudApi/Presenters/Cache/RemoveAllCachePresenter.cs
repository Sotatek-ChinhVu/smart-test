using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Cache;
using UseCase.Cache.RemoveAllCache;

namespace EmrCloudApi.Presenters.Cache
{
    public class RemoveAllCachePresenter : IRemoveAllCacheOutputPort
    {
        public Response<RemoveAllCacheResponse> Result { get; private set; } = new();
        public void Complete(RemoveAllCacheOutputData outputData)
        {
            Result.Data = new RemoveAllCacheResponse(outputData.Status == RemoveAllCacheStaus.Successed);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(RemoveAllCacheStaus status) => status switch
        {
            RemoveAllCacheStaus.Successed => ResponseMessage.Success,
            RemoveAllCacheStaus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
