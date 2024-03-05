using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Responses.Yousiki.Dto;
using UseCase.Yousiki.GetHistoryYousiki;

namespace EmrCloudApi.Presenters.Yousiki
{
    public class GetHistoryYousikiPresenter : IGetHistoryYousikiOutputPort
    {
        public Response<GetHistoryYousikiResponse> Result { get; private set; } = new();
        public void Complete(GetHistoryYousikiOutputData outputData)
        {
            Result.Data = new GetHistoryYousikiResponse(outputData.Yousiki1InfModels.Select(item => new Yousiki1InfDto(item)).ToList());
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetHistoryYousikiStatus status) => status switch
        {
            GetHistoryYousikiStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
