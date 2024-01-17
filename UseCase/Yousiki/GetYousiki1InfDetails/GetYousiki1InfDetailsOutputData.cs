using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfDetails;

public class GetYousiki1InfDetailsOutputData : IOutputData
{
    public GetYousiki1InfDetailsOutputData(List<Yousiki1InfDetailModel> yousiki1InfDetailList, GetYousiki1InfDetailsStatus status)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        Status = status;
    }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public GetYousiki1InfDetailsStatus Status { get; private set; }
}
