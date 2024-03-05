using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetHistoryYousiki
{
    public class GetHistoryYousikiOutputData : IOutputData
    {
        public GetHistoryYousikiOutputData(List<Yousiki1InfModel> yousiki1InfModels, Dictionary<string, string> kacodeYousikiMstDict, GetHistoryYousikiStatus status)
        {
            Yousiki1InfModels = yousiki1InfModels;
            KacodeYousikiMstDict = kacodeYousikiMstDict;
            Status = status;
        }

        public Dictionary<string, string> KacodeYousikiMstDict { get; private set; }

        public List<Yousiki1InfModel> Yousiki1InfModels { get; private set; }

        public GetHistoryYousikiStatus Status { get; private set; }
    }
}
