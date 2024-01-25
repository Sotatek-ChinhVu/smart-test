using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfDetails;

public class GetYousiki1InfDetailsOutputData : IOutputData
{
    public GetYousiki1InfDetailsOutputData(Yousiki1InfModel yousiki1Inf, GetYousiki1InfDetailsStatus status)
    {
        Yousiki1Inf = yousiki1Inf;
        Status = status;
    }

    public Yousiki1InfModel Yousiki1Inf { get; private set; }

    public GetYousiki1InfDetailsStatus Status { get; private set; }
}
