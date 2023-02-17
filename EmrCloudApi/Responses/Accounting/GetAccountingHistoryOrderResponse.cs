using Domain.Models.HistoryOrder;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingHistoryOrderResponse
    {
        public GetAccountingHistoryOrderResponse(List<HistoryOrderDto> historyOrderDtos)
        {
            HistoryOrderDtos = historyOrderDtos;
        }

        public List<HistoryOrderDto> HistoryOrderDtos { get; private set; }
    }
}