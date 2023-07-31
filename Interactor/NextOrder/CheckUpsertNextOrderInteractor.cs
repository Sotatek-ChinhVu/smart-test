using Domain.Models.NextOrder;
using UseCase.NextOrder.Check;

namespace Interactor.NextOrder
{
    public class CheckUpsertNextOrderInteractor : ICheckUpsertNextOrderInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;

        public CheckUpsertNextOrderInteractor(INextOrderRepository nextOrderRepository)
        {
            _nextOrderRepository = nextOrderRepository;
        }

        public CheckUpsertNextOrderOutputData Handle(CheckUpsertNextOrderInputData inputData)
        {
            try
            {
                var existed = _nextOrderRepository.CheckUpsertNextOrder(inputData.HpId, inputData.PtId, inputData.RsvDate);

                if (existed)
                {
                    return new CheckUpsertNextOrderOutputData(CheckUpsertNextOrderStatus.InValid);
                }

                return new CheckUpsertNextOrderOutputData(CheckUpsertNextOrderStatus.Valid);
            }
            finally
            {
                _nextOrderRepository.ReleaseResource();
            }
        }
    }
}
