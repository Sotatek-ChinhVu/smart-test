using Domain.Models.Ka;

namespace EmrCloudApi.Responses.Yousiki
{
    public class GetKacodeYousikiMstDictResponse
    {
        public GetKacodeYousikiMstDictResponse(Dictionary<string, string> kacodeYousikiMstDict, List<KaMstModel> kaMstModels)
        {
            KacodeYousikiMstDict = kacodeYousikiMstDict;
            KaMstModels = kaMstModels;
        }

        public Dictionary<string, string> KacodeYousikiMstDict { get; private set; }

        public List<KaMstModel> KaMstModels { get; private set; }
    }
}
