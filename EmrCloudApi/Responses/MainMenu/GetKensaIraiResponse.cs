using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Responses.MainMenu;

public class GetKensaIraiResponse
{
    public GetKensaIraiResponse(List<KensaIraiDto> kensaIraiList)
    {
        KensaIraiList = kensaIraiList;
    }

    public List<KensaIraiDto> KensaIraiList { get; private set; }
}
