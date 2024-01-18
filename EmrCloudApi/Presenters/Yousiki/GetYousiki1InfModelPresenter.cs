using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Responses.Yousiki.Dto;
using UseCase.Yousiki.GetYousiki1InfModel;

namespace EmrCloudApi.Presenters.Yousiki
{
    public class GetYousiki1InfModelPresenter : IGetYousiki1InfModelOutputPort
    {

        public Response<GetYousiki1InfModelResponse> Result { get; private set; } = new();
        public void Complete(GetYousiki1InfModelOutputData outputData)
        {
            Result.Data = new GetYousiki1InfModelResponse(outputData.Yousiki1InfModels.Select(item => new Yousiki1InfDto(item)).ToList());
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetYousiki1InfModelStatus status) => status switch
        {
            GetYousiki1InfModelStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
