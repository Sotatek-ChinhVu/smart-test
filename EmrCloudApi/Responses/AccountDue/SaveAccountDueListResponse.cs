namespace EmrCloudApi.Responses.AccountDue;

public class SaveAccountDueListResponse
{
    public SaveAccountDueListResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
