using Domain.Models.HistoryOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderOutputData : IOutputData
    {
        public GetAccountingHistoryOrderOutputData(List<HistoryOrderModel> historyOrderModels, GetAccountingHistoryOrderStatus status)
        {
            HistoryOrderModels = historyOrderModels;
            Status = status;
        }

        public List<HistoryOrderModel> HistoryOrderModels { get; private set; }

        public GetAccountingHistoryOrderStatus Status { get; private set; }
    }
}
