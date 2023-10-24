using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.DeleteKensaInf;

public class DeleteKensaInfOutputData : IOutputData
{
    public DeleteKensaInfOutputData(DeleteKensaInfStatus status)
    {
        Status = status;
    }

    public DeleteKensaInfStatus Status { get; private set; }
}
