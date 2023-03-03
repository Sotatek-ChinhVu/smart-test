using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetSinMeiResponse
    {
        public GetSinMeiResponse(List<SinMeiModel> sinMeiModels)
        {
            SinMeiModels = sinMeiModels;
        }

        public List<SinMeiModel> SinMeiModels { get; private set; }
    }
}
