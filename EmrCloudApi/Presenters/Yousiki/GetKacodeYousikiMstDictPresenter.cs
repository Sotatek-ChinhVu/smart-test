using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using UseCase.Yousiki.GetKacodeYousikiMstDict;

namespace EmrCloudApi.Presenters.Yousiki
{
    public class GetKacodeYousikiMstDictPresenter : IGetKacodeYousikiMstDictOutputPort
    {
        public Response<GetKacodeYousikiMstDictResponse> Result { get; private set; } = new();
        public void Complete(GetKacodeYousikiMstDictOutputData outputData)
        {
            Result.Data = new GetKacodeYousikiMstDictResponse(outputData.KacodeYousikiMstDict);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetKacodeYousikiMstDictStatus status) => status switch
        {
            GetKacodeYousikiMstDictStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
