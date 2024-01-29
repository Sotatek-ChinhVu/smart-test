using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;
using UseCase.Yousiki.GetHistoryYousiki;

namespace UseCase.Yousiki.GetYousiki1InfModel
{
    public class GetYousiki1InfModelOutputData : IOutputData
    {
        public GetYousiki1InfModelOutputData(List<Yousiki1InfModel> yousiki1InfModels, Dictionary<string, string> kacodeYousikiMstDict, GetYousiki1InfModelStatus status)
        {
            Yousiki1InfModels = yousiki1InfModels;
            KacodeYousikiMstDict = kacodeYousikiMstDict;
            Status = status;
        }

        public List<Yousiki1InfModel> Yousiki1InfModels { get; private set; }

        public Dictionary<string, string> KacodeYousikiMstDict { get; private set; }

        public GetYousiki1InfModelStatus Status { get; private set; }
    }
}
