using Domain.Models.HistoryOrder;
using UseCase.Accounting.GetHistoryOrder;

namespace Interactor.Accounting
{
    public class GetAccountingHistoryOrderInteractor : IGetAccountingHistoryOrderInputPort
    {
        private readonly IHistoryOrderRepository _historyOrderRepository;

        public GetAccountingHistoryOrderInteractor(IHistoryOrderRepository historyOrderRepository)
        {
            _historyOrderRepository = historyOrderRepository;
        }

        public GetAccountingHistoryOrderOutputData Handle(GetAccountingHistoryOrderInputData inputData)
        {
            try
            {
                var historyList = _historyOrderRepository.GetListByRaiin(
                    inputData.HpId,
                    inputData.UserId,
                    inputData.PtId,
                    inputData.SinDate,
                    0,
                    inputData.DeleteConditon,
                    inputData.RaiinNo
                    );
                return new GetAccountingHistoryOrderOutputData(historyList, GetAccountingHistoryOrderStatus.Successed);
            }
            finally
            {
                _historyOrderRepository.ReleaseResource();
            }
        }
    }
}