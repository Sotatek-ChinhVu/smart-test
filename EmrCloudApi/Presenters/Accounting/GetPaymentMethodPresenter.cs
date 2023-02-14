using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.PaymentMethod;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetPaymentMethodPresenter : IGetPaymentMethodOutputPort
    {
        public Response<GetPaymentMethodResponse> Result { get; private set; } = new();

        public void Complete(GetPaymentMethodOutputData outputData)
        {
            Result.Data = new GetPaymentMethodResponse(outputData.PaymentMethodMstModels);
            Result.Message = GetMessage(outputData.PaymentMethodMstModels);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetPaymentMethodStatus.Successed => ResponseMessage.Success,
            GetPaymentMethodStatus.Failed => ResponseMessage.Failed,
            GetPaymentMethodStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
