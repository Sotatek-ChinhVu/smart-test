namespace EmrCloudApi.Responses.Family;

public class GetRaiinInfListResponse
{
    public GetRaiinInfListResponse(List<RaiinInfDto> raiinInfList)
    {
        RaiinInfList = raiinInfList;
    }

    public List<RaiinInfDto> RaiinInfList { get; private set; }
}
