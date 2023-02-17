using Domain.Models.HistoryOrder;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingHistoryOrderResponse
    {
        public GetAccountingHistoryOrderResponse(int total, List<HistoryOrderModel> historyOrderModels)
        {
            Total = total;
            HistoryOrderModels = historyOrderModels;
        }

        public int Total { get; private set; }

        public List<HistoryOrderModel> HistoryOrderModels { get; private set; }
    }
}
