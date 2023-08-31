using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SaveKensaIrai;

public class SaveKensaIraiOutputData : IOutputData
{
    public SaveKensaIraiOutputData(string message, SaveKensaIraiStatus status)
    {
        Message = message;
        Status = status;
    }

    public string Message { get; private set; }

    public SaveKensaIraiStatus Status { get; private set; }
}
