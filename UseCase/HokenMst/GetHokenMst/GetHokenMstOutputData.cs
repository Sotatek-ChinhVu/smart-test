using UseCase.Core.Sync.Core;

namespace UseCase.HokenMst.GetHokenMst
{
    public class GetHokenMstOutputData : IOutputData
    {
        public GetHokenMstOutputData(Dictionary<string, string> hokenInfModels, Dictionary<string, string> kohiModelWithFutansyanos, Dictionary<string, string> kohiModels, GetHokenMstStatus status)
        {
            HokenInfModels = hokenInfModels;
            KohiModelWithFutansyanos = kohiModelWithFutansyanos;
            KohiModels = kohiModels;
            Status = status;
        }

        public Dictionary<string, string> HokenInfModels { get; private set; }

        public Dictionary<string, string> KohiModelWithFutansyanos { get; private set; }

        public Dictionary<string, string> KohiModels { get; private set; }

        public GetHokenMstStatus Status { get; private set; }
    }
}
