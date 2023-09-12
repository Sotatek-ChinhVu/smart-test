using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.CreateDataKensaIraiRenkei;

public class CreateDataKensaIraiRenkeiOutputData : IOutputData
{
    public CreateDataKensaIraiRenkeiOutputData(CreateDataKensaIraiRenkeiStatus status)
    {
        Status = status;
    }

    public CreateDataKensaIraiRenkeiStatus Status { get; private set; }
}
