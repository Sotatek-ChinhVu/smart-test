using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.PaymentMethod
{
    public interface IGetPaymentMethodInputPort : IInputPort<GetPaymentMethodInputData, GetPaymentMethodOutputData>
    {
    }
}
