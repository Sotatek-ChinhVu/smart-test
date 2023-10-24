using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaIraiLog;

public class GetKensaIraiLogOutputData : IOutputData
{
    public GetKensaIraiLogOutputData(List<KensaIraiLogModel> kensaIraiLogList, GetKensaIraiLogStatus status)
    {
        KensaIraiLogList = kensaIraiLogList;
        Status = status;
    }

    public List<KensaIraiLogModel> KensaIraiLogList { get; private set; }

    public GetKensaIraiLogStatus Status { get; private set; }
}
