using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Responses.MainMenu;

public class GetKensaInfResponse
{
    public GetKensaInfResponse(List<KensaInfDto> kensaInfList)
    {
        KensaInfList = kensaInfList;
    }

    public List<KensaInfDto> KensaInfList { get; private set; }
}
