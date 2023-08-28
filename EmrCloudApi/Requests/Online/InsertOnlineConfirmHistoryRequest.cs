namespace EmrCloudApi.Requests.Online;

public class InsertOnlineConfirmHistoryRequest
{
    public List<InsertOnlineConfirmHistoryRequestItem> OnlineConfirmList { get; set; } = new();
}
