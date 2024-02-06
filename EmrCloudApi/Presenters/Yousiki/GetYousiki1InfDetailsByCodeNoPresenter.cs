using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Responses.Yousiki.Dto;
using UseCase.Yousiki.GetYousiki1InfDetailsByCodeNo;

namespace EmrCloudApi.Presenters.Yousiki
{
    public class GetYousiki1InfDetailsByCodeNoPresenter
    {
        public Response<GetYousiki1InfDetailsResponse> Result { get; private set; } = new();

        public void Complete(GetYousiki1InfDetailsByCodeNoOutputData output)
        {
            Result.Data = new GetYousiki1InfDetailsResponse(output.Yousiki1InfDetailList.Select(item => new Yousiki1InfDetailDto(item)).ToList());
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetYousiki1InfDetailsByCodeNoStatus status) => status switch
        {
            GetYousiki1InfDetailsByCodeNoStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };

    }
}
