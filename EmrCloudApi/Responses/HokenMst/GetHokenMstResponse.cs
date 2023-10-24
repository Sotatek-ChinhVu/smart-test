using UseCase.HokenMst.GetHokenMst;

namespace EmrCloudApi.Responses.HokenMst
{
    public class GetHokenMstResponse
    {
        public GetHokenMstResponse(Dictionary<string, string> hokenInfModels, Dictionary<string, string> kohiModelWithFutansyanos, List<HokenMstItem> kohiModels)
        {
            HokenInfModels = hokenInfModels;
            KohiModelWithFutansyanos = kohiModelWithFutansyanos;
            KohiModels = kohiModels;
        }

        public Dictionary<string, string> HokenInfModels { get; private set; }

        public Dictionary<string, string> KohiModelWithFutansyanos { get; private set; }

        public List<HokenMstItem> KohiModels { get; private set; }
    }
}
