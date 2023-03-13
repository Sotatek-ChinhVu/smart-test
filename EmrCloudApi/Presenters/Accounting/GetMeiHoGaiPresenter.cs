using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.GetSinMei;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetMeiHoGaiPresenter : IGetMeiHoGaiOutputPort
    {
        public Response<GetMeiHoGaiResponse> Result { get; private set; } = new();
        public void Complete(GetMeiHoGaiOutputData outputData)
        {
            Result.Data = new GetMeiHoGaiResponse(outputData.SinMeiModels, outputData.SinHoModels, outputData.SinGaiModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            GetMeiHoGaiStatus.Successed => ResponseMessage.Success,
            GetMeiHoGaiStatus.Failed => ResponseMessage.Failed,
            GetMeiHoGaiStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
