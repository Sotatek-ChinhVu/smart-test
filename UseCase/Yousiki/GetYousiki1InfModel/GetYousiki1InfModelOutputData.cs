using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfModel;

public class GetYousiki1InfModelOutputData : IOutputData
{
    public GetYousiki1InfModelOutputData(List<Yousiki1InfModel> yousiki1InfModels, GetYousiki1InfModelStatus status)
    {
        Yousiki1InfModels = yousiki1InfModels;
        Status = status;
    }

    public List<Yousiki1InfModel> Yousiki1InfModels { get; private set; }

    public GetYousiki1InfModelStatus Status { get; private set; }
}
