namespace EmrCloudApi.Responses.Online;

public class InsertOnlineConfirmHistoryResponse
{
    public InsertOnlineConfirmHistoryResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
