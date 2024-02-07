using EmrCloudApi.Responses.Yousiki.Dto;

namespace EmrCloudApi.Responses.Yousiki;

public class GetVisitingInfsResponse
{
    public GetVisitingInfsResponse(Dictionary<int, string> allGrpDictionary, List<VisitingInfDto> visitingInfList)
    {
        AllGrpDictionary = allGrpDictionary;
        VisitingInfList = visitingInfList;
    }

    public Dictionary<int, string> AllGrpDictionary { get; private set; }

    public List<VisitingInfDto> VisitingInfList { get; private set; }
}
