namespace EmrCloudApi.Responses.MainMenu;

public class SaveStaCsvMstResponse
{
    public SaveStaCsvMstResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
