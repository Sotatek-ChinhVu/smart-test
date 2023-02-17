using Domain.Models.HistoryOrder;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingHistoryOrderResponse
    {
        public GetAccountingHistoryOrderResponse(int total, List<HistoryOrderDto> historyOrderDtos)
        {
            Total = total;
            HistoryOrderDtos = historyOrderDtos;
        }
        public int Total { get; private set; }
        public List<HistoryOrderDto> HistoryOrderDtos { get; private set; }
    }
}