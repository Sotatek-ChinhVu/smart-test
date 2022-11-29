namespace EmrCloudApi.Responses.VisitingList;

public class SaveVisitingListSettingsResponse
{
    public SaveVisitingListSettingsResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
