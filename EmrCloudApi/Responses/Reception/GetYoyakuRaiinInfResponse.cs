namespace EmrCloudApi.Responses.Reception;

public class GetYoyakuRaiinInfResponse
{
    public GetYoyakuRaiinInfResponse(RaiinInfDto raiinInf)
    {
        RaiinInf = raiinInf;
    }

    public RaiinInfDto RaiinInf { get; private set; }
}
