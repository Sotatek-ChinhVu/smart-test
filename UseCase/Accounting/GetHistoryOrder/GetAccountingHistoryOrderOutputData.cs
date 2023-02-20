using Domain.Models.HistoryOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderOutputData : IOutputData
    {
        public GetAccountingHistoryOrderOutputData( List<HistoryOrderDto> historyOrderDtos, GetAccountingHistoryOrderStatus status)
        {
            HistoryOrderDtos = historyOrderDtos;
            Status = status;
        }

        public List<HistoryOrderDto> HistoryOrderDtos { get; private set; }

        public GetAccountingHistoryOrderStatus Status { get; private set; }
    }
}
