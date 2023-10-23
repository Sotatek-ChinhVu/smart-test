using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Responses.MainMenu;

public class CreateDataKensaIraiRenkeiResponse
{
    public CreateDataKensaIraiRenkeiResponse(bool successed, List<KensaIraiDto> kensaIraiList)
    {
        Successed = successed;
        KensaIraiList = kensaIraiList;
    }

    public bool Successed { get; private set; }

    public List<KensaIraiDto> KensaIraiList { get; private set; }
}
