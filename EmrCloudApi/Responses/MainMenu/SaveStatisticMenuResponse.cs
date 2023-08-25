namespace EmrCloudApi.Responses.MainMenu;

public class SaveStatisticMenuResponse
{
    public SaveStatisticMenuResponse(int menuIdTemp, bool success)
    {
        Success = success;
        MenuIdTemp = menuIdTemp;
    }

    public int MenuIdTemp { get; private set; }

    public bool Success { get; private set; }
}
