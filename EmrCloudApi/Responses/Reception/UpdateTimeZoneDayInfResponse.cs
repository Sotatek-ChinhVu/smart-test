namespace EmrCloudApi.Responses.Reception;

public class UpdateTimeZoneDayInfResponse
{
    public UpdateTimeZoneDayInfResponse(bool status)
    {
        IsSuccess = status;
    }

    public bool IsSuccess { get; private set; }
}
