using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetVisitingInfs;

public class GetVisitingInfsOutputData : IOutputData
{
    public GetVisitingInfsOutputData(List<VisitingInfModel> visitingInfList, GetVisitingInfsStatus status)
    {
        VisitingInfList = visitingInfList;
        Status = status;
    }

    public List<VisitingInfModel> VisitingInfList { get; private set; }

    public GetVisitingInfsStatus Status { get; private set; }
}
