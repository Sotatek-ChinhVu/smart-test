using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetPaymentMethodResponse
    {
        public GetPaymentMethodResponse(List<PaymentMethodMstModel> paymentMethodMstModels)
        {
            PaymentMethodMstModels = paymentMethodMstModels;
        }

        public List<PaymentMethodMstModel> PaymentMethodMstModels { get; private set; }
    }
}
