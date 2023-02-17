using Domain.Models.HistoryOrder;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingHistoryOrderResponse
    {
        public GetAccountingHistoryOrderResponse(int total, List<HistoryOrderDtoModel> historyOrderDtoModels)
        {
            Total = total;
            HistoryOrderDtoModels = historyOrderDtoModels;
        }

        public int Total { get; private set; }
        public List<HistoryOrderDtoModel> HistoryOrderDtoModels { get; private set; }
    }
}
