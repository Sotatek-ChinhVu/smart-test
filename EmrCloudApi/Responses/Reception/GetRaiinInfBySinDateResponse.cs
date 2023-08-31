namespace EmrCloudApi.Responses.Reception;

public class GetRaiinInfBySinDateResponse
{
    public GetRaiinInfBySinDateResponse(RaiinInfDto raiinInf)
    {
        RaiinInf = raiinInf;
    }

    public RaiinInfDto RaiinInf { get; private set; }
}
