using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.ImportKensaIrai;

public class ImportKensaIraiOutputData : IOutputData
{
    public ImportKensaIraiOutputData(List<KensaInfMessageModel> kensaInfMessageList, ImportKensaIraiStatus status)
    {
        KensaInfMessageList = kensaInfMessageList;
        Status = status;
    }

    public ImportKensaIraiOutputData(ImportKensaIraiStatus status)
    {
        KensaInfMessageList = new();
        Status = status;
    }

    public List<KensaInfMessageModel> KensaInfMessageList { get; private set; }

    public ImportKensaIraiStatus Status { get; private set; }
}
