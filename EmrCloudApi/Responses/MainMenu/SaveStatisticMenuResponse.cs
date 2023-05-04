namespace EmrCloudApi.Responses.MainMenu;

public class SaveStatisticMenuResponse
{
    public SaveStatisticMenuResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
