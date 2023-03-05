using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetMeiHoGaiResponse
    {
        public GetMeiHoGaiResponse(List<SinMeiModel> sinMeiModels, List<SinHoModel> sinHoModels, List<SinGaiModel> sinGaiModels)
        {
            SinMeiModels = sinMeiModels;
            SinHoModels = sinHoModels;
            SinGaiModels = sinGaiModels;
        }

        public List<SinMeiModel> SinMeiModels { get; private set; }
        public List<SinHoModel> SinHoModels { get; private set; }
        public List<SinGaiModel> SinGaiModels { get; private set; }
    }
}
