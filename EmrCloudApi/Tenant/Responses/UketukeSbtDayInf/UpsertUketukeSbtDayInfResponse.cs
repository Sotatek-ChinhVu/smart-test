namespace EmrCloudApi.Tenant.Responses.UketukeSbtDayInf;

public class UpsertUketukeSbtDayInfResponse
{
    public UpsertUketukeSbtDayInfResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
