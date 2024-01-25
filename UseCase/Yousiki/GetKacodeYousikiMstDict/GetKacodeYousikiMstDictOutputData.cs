using Domain.Models.Ka;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetKacodeYousikiMstDict
{
    public class GetKacodeYousikiMstDictOutputData : IOutputData
    {
        public GetKacodeYousikiMstDictOutputData(Dictionary<string, string> kacodeYousikiMstDict, List<KaMstModel> kaMstModels, GetKacodeYousikiMstDictStatus status)
        {
            KacodeYousikiMstDict = kacodeYousikiMstDict;
            KaMstModels = kaMstModels;
            Status = status;
        }

        public Dictionary<string, string> KacodeYousikiMstDict { get; private set; }

        public List<KaMstModel> KaMstModels { get; private set; }

        public GetKacodeYousikiMstDictStatus Status { get; private set; }
    }
}
