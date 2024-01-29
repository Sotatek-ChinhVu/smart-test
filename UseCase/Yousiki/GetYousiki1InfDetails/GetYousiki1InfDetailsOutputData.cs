using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfDetails;

public class GetYousiki1InfDetailsOutputData : IOutputData
{
    public GetYousiki1InfDetailsOutputData(Yousiki1InfModel yousiki1Inf, Dictionary<string, string> kacodeYousikiMstDict, GetYousiki1InfDetailsStatus status)
    {
        Yousiki1Inf = yousiki1Inf;
        Status = status;
        KacodeYousikiMstDict = kacodeYousikiMstDict;
    }

    public Yousiki1InfModel Yousiki1Inf { get; private set; }

    public Dictionary<string, string> KacodeYousikiMstDict { get; private set; }

    public GetYousiki1InfDetailsStatus Status { get; private set; }
}
