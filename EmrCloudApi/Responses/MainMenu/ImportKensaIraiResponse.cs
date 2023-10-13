using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Responses.MainMenu;

public class ImportKensaIraiResponse
{
    public ImportKensaIraiResponse(List<KensaInfMessageDto> kensaInfMessageList)
    {
        KensaInfMessageList = kensaInfMessageList;
    }

    public List<KensaInfMessageDto> KensaInfMessageList { get; private set; }
}
