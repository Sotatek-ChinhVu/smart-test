using Domain.Models.Accounting;
using Domain.Models.MstItem;
using UseCase.Accounting.PaymentMethod;

namespace Interactor.Accounting
{
    public class GetPaymentMethodInteractor : IGetPaymentMethodInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetPaymentMethodInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }
        public GetPaymentMethodOutputData Handle(GetPaymentMethodInputData inputData)
        {
            try
            {
                //Get Payment Method
                var paymentList = _accountingRepository.GetListPaymentMethodMst(inputData.HpId);
                if (!paymentList.Any())
                {
                    return new GetPaymentMethodOutputData(new List<PaymentMethodMstModel>(), GetPaymentMethodStatus.NoData);
                }
                return new GetPaymentMethodOutputData(paymentList, GetPaymentMethodStatus.Successed);

            }
            catch (Exception)
            {
                return new GetPaymentMethodOutputData(new List<PaymentMethodMstModel>(), GetPaymentMethodStatus.Failed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }

        }
    }
}
