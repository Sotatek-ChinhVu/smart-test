namespace EmrCloudApi.Responses.PtGroupMst;

public class GetGroupNameMstResponse
{
    public GetGroupNameMstResponse(List<GroupNameDtoResponse> data)
    {
        Data = data;
    }

    public List<GroupNameDtoResponse> Data { get; private set; }
}
