using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.PaymentMethod
{
    public class GetPaymentMethodInputData : IInputData<GetPaymentMethodOutputData>
    {
        public GetPaymentMethodInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
