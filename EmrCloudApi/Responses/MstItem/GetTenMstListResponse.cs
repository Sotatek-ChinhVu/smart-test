namespace EmrCloudApi.Responses.MstItem;

public class GetTenMstListResponse
{
    public GetTenMstListResponse(List<TenItemDto> tenMstList)
    {
        TenMstList = tenMstList;
    }

    public List<TenItemDto> TenMstList { get; private set; }
}
