using Domain.Models.SinKoui;

namespace EmrCloudApi.Responses.SinKoui
{
    public class GetSinKouiResponse
    {
        public GetSinKouiResponse(List<KaikeiInfModel> kaikeiInfModels)
        {
            KaikeiInfModels = kaikeiInfModels;
        }

        public List<KaikeiInfModel> KaikeiInfModels { get; private set; }
    }
}
