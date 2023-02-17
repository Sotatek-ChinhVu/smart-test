using Domain.Models.HistoryOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderOutputData : IOutputData
    {
        public GetAccountingHistoryOrderOutputData(int total, List<HistoryOrderDtoModel> historyOrderDtoModels, GetAccountingHistoryOrderStatus status)
        {
            Total = total;
            HistoryOrderDtoModels = historyOrderDtoModels;
            Status = status;
        }

        public int Total { get; private set; }
        public List<HistoryOrderDtoModel> HistoryOrderDtoModels { get; private set; }
        public GetAccountingHistoryOrderStatus Status { get; private set; }
    }
}
