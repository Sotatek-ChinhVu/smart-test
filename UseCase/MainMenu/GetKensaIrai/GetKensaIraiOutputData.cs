using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaIrai;

public class GetKensaIraiOutputData : IOutputData
{
    public GetKensaIraiOutputData(GetKensaIraiStatus status, List<KensaIraiModel> kensaIraiList)
    {
        Status = status;
        KensaIraiList = kensaIraiList;
    }

    public GetKensaIraiStatus Status { get; private set; }

    public List<KensaIraiModel> KensaIraiList { get; private set; }
}
