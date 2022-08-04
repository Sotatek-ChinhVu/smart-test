namespace EmrCloudApi.Tenant.Responses.UketukeSbt;

public class UpsertUketukeSbtDayInfResponse
{
    public UpsertUketukeSbtDayInfResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
