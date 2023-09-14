namespace EmrCloudApi.Responses.HokenMst
{
    public class GetHokenMstResponse
    {
        public GetHokenMstResponse(Dictionary<string, string> hokenInfModels, Dictionary<string, string> kohiModelWithFutansyanos)
        {
            HokenInfModels = hokenInfModels;
            KohiModelWithFutansyanos = kohiModelWithFutansyanos;
        }

        public Dictionary<string, string> HokenInfModels { get; private set; }

        public Dictionary<string, string> KohiModelWithFutansyanos { get; private set; }
    }
}
