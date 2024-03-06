namespace EmrCloudApi.Responses.ReceSeikyu;

public class GetRecedenHenJiyuuListResponse
{
    public GetRecedenHenJiyuuListResponse(List<RecedenHenJiyuuDto> recedenHenJiyuuList)
    {
        RecedenHenJiyuuList = recedenHenJiyuuList;
    }

    public List<RecedenHenJiyuuDto> RecedenHenJiyuuList { get;private set; }
}
