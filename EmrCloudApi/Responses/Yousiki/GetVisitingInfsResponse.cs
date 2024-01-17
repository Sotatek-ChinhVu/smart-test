using EmrCloudApi.Responses.Yousiki.Dto;

namespace EmrCloudApi.Responses.Yousiki;

public class GetVisitingInfsResponse
{
    public GetVisitingInfsResponse(List<VisitingInfDto> visitingInfList)
    {
        VisitingInfList = visitingInfList;
    }

    public List<VisitingInfDto> VisitingInfList { get; private set; }
}
