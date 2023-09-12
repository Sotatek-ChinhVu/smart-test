using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Responses.MainMenu;

public class GetKensaCenterMstListResponse
{
    public GetKensaCenterMstListResponse(List<KensaCenterMstDto> kensaCenterMstList)
    {
        KensaCenterMstList = kensaCenterMstList;
    }

    public List<KensaCenterMstDto> KensaCenterMstList { get;private set; }
}
