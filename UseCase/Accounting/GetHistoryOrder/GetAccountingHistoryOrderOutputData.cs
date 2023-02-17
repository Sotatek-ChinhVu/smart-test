using Domain.Models.HistoryOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderOutputData : IOutputData
    {
        public GetAccountingHistoryOrderOutputData(int total, List<HistoryOrderDto> historyOrderDtos, GetAccountingHistoryOrderStatus status)
        {
            Total = total;
            HistoryOrderDtos = historyOrderDtos;
            Status = status;
        }

        public int Total { get; private set; }

        public List<HistoryOrderDto> HistoryOrderDtos { get; private set; }

        public GetAccountingHistoryOrderStatus Status { get; private set; }
    }
}
