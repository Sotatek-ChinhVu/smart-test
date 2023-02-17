using Domain.Models.HistoryOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderOutputData : IOutputData
    {
        public GetAccountingHistoryOrderOutputData(int total, List<HistoryOrderModel> historyOrderModels, GetAccountingHistoryOrderStatus status)
        {
            Total = total;
            HistoryOrderModels = historyOrderModels;
            Status = status;
        }

        public int Total { get; private set; }

        public List<HistoryOrderModel> HistoryOrderModels { get; private set; }

        public GetAccountingHistoryOrderStatus Status { get; private set; }
    }
}
