using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Responses.MainMenu;

public class GetKensaIraiLogResponse
{
    public GetKensaIraiLogResponse(List<KensaIraiLogDto> kensaIraiLogList)
    {
        KensaIraiLogList = kensaIraiLogList;
    }

    public List<KensaIraiLogDto> KensaIraiLogList { get; private set; }
}
