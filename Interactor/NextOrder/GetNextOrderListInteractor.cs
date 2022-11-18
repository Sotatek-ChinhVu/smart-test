using Domain.Models.NextOrder;
using UseCase.NextOrder.GetList;

namespace Interactor.NextOrder
{
    public class GetNextOrderListInteractor : IGetNextOrderListInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;

        public GetNextOrderListInteractor(INextOrderRepository nextOrderRepository)
        {
            _nextOrderRepository = nextOrderRepository;
        }

        public GetNextOrderListOutputData Handle(GetNextOrderListInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetNextOrderListOutputData(new(), GetNextOrderListStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetNextOrderListOutputData(new(), GetNextOrderListStatus.InvalidPtId);

                }
                if (inputData.RsvkrtKbn != 0 && inputData.RsvkrtKbn != 1)
                {
                    return new GetNextOrderListOutputData(new(), GetNextOrderListStatus.InvalidRsvkrtKbn);
                }

                var nextOrders = _nextOrderRepository.GetList(inputData.HpId, inputData.PtId, inputData.RsvkrtKbn, inputData.IsDeleted);

                if (nextOrders.Count == 0)
                {
                    return new GetNextOrderListOutputData(new(), GetNextOrderListStatus.NoData);
                }

                return new GetNextOrderListOutputData(nextOrders, GetNextOrderListStatus.Successed);
            }
            catch
            {
                return new GetNextOrderListOutputData(new(), GetNextOrderListStatus.Failed);
            }
        }
    }
}
