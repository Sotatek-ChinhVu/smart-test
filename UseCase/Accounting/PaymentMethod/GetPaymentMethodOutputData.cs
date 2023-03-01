using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.PaymentMethod
{
    public class GetPaymentMethodOutputData : IOutputData
    {
        public GetPaymentMethodOutputData(List<PaymentMethodMstModel> paymentMethodMstModels, GetPaymentMethodStatus status)
        {
            PaymentMethodMstModels = paymentMethodMstModels;
            Status = status;
        }

        public List<PaymentMethodMstModel> PaymentMethodMstModels { get; private set; }
        public GetPaymentMethodStatus Status { get; private set; }
    }
}
