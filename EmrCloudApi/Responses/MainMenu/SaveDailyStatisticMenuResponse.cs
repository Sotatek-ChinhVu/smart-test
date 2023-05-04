namespace EmrCloudApi.Responses.MainMenu;

public class SaveDailyStatisticMenuResponse
{
    public SaveDailyStatisticMenuResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
