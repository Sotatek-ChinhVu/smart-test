namespace EmrCloudApi.Requests.SystemConf;

public class GetSystemConfListRequest
{
    public List<GetSystemConfListRequestItem> GrpItemList { get; set; } = new();
}
