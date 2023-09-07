namespace EmrCloudApi.Responses.Online;

public class InsertOnlineConfirmHistoryResponse
{
    public InsertOnlineConfirmHistoryResponse(List<long> idList)
    {
        IdList = idList;
    }

    public List<long> IdList { get; private set; }
}
