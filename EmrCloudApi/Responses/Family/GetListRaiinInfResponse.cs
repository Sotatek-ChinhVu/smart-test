namespace EmrCloudApi.Responses.Family;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<RaiinInfDto> listRaiinInf)
    {
        ListRaiinInf = listRaiinInf;
    }

    public List<RaiinInfDto> ListRaiinInf { get; private set; }
}
