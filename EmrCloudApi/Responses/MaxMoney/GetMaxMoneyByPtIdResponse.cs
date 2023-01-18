using Domain.Models.MaxMoney;

namespace EmrCloudApi.Responses.MaxMoney
{
    public class GetMaxMoneyByPtIdResponse
    {
        public GetMaxMoneyByPtIdResponse(IEnumerable<MaxMoneyModel> data) => Data = data;

        public IEnumerable<MaxMoneyModel> Data { get; private set; }
    }
}
