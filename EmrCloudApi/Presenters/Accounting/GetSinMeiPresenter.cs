using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.GetSinMei;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetSinMeiPresenter : IGetSinMeiOutputPort
    {
        public Response<GetSinMeiResponse> Result { get; private set; } = new();
        public void Complete(GetSinMeiOutputData outputData)
        {
            Result.Data = new GetSinMeiResponse(outputData.SinMeiModels, outputData.SinHoModels, outputData.SinGaiModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            GetSinMeiStatus.Successed => ResponseMessage.Success,
            GetSinMeiStatus.Failed => ResponseMessage.Failed,
            GetSinMeiStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
