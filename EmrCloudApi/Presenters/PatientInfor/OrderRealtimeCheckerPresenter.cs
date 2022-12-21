using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.CommonChecker;

namespace EmrCloudApi.Presenters.PatientInfor
{
    public class OrderRealtimeCheckerPresenter : IGetOrderCheckerOutputPort
    {
        public Response<OrderRealtimeCheckerResponse> Result { get; private set; } = new Response<OrderRealtimeCheckerResponse>();

        public void Complete(GetOrderCheckerOutputData outputData)
        {
            Result.Data = new OrderRealtimeCheckerResponse(outputData.OrdInfoModels, outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetOrderCheckerStatus status) => status switch
        {
            GetOrderCheckerStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
