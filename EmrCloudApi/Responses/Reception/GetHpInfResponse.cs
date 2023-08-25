namespace EmrCloudApi.Responses.Reception;

public class GetHpInfResponse
{
    public GetHpInfResponse(HpInfDto hpInf)
    {
        HpInf = hpInf;
    }

    public HpInfDto HpInf { get; private set; }
}
