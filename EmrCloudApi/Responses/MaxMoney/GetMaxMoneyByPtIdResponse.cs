using Domain.Models.MaxMoney;

namespace EmrCloudApi.Responses.MaxMoney
{
    public class GetMaxMoneyByPtIdResponse
    {
        public GetMaxMoneyByPtIdResponse(IEnumerable<LimitListModel> data) => Data = data;

        public IEnumerable<LimitListModel> Data { get; private set; }
    }
}
