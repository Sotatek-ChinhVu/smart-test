using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfModelWithCommonInf;

public class GetYousiki1InfModelWithCommonInfOutputData : IOutputData
{
    public GetYousiki1InfModelWithCommonInfOutputData(List<Yousiki1InfModel> yousiki1InfList, GetYousiki1InfModelWithCommonInfStatus status)
    {
        Yousiki1InfList = yousiki1InfList;
        Status = status;
    }

    public List<Yousiki1InfModel> Yousiki1InfList { get; private set; }

    public GetYousiki1InfModelWithCommonInfStatus Status { get; private set; }
}
