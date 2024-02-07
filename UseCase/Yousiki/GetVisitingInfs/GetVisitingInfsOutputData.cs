using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetVisitingInfs;

public class GetVisitingInfsOutputData : IOutputData
{
    public GetVisitingInfsOutputData(Dictionary<int, string> allGrpDictionary, List<VisitingInfModel> visitingInfList, GetVisitingInfsStatus status)
    {
        AllGrpDictionary = allGrpDictionary;
        VisitingInfList = visitingInfList;
        Status = status;
    }

    public Dictionary<int, string> AllGrpDictionary { get; private set; }

    public List<VisitingInfModel> VisitingInfList { get; private set; }

    public GetVisitingInfsStatus Status { get; private set; }
}
