namespace EmrCloudApi.Tenant.Responses.Reception;

public class InsertReceptionResponse
{
    public InsertReceptionResponse(long raiinNo)
    {
        RaiinNo = raiinNo;
    }

    public long RaiinNo { get; private set; }
}
