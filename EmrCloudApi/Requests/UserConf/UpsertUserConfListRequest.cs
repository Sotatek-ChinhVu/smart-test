namespace EmrCloudApi.Requests.UserConf;

public class UpsertUserConfListRequest
{
    public List<UserConfListItem> userConfs { get; set; } = new();
}
