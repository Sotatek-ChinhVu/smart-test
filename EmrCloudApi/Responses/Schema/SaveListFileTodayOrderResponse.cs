namespace EmrCloudApi.Responses.Schema;

public class SaveListFileTodayOrderResponse
{
    public SaveListFileTodayOrderResponse(List<long> listFileIds)
    {
        ListFileIds = listFileIds;
    }

    public List<long> ListFileIds { get; private set; }
}
