using Domain.Models.HistoryOrder;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingHistoryOrderResponse
    {
        public GetAccountingHistoryOrderResponse(List<HistoryOrderModel> historyOrderModels)
        {
            HistoryOrderModels = historyOrderModels;
        }

        public List<HistoryOrderModel> HistoryOrderModels { get; private set; }
    }
}