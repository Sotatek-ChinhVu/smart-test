using UseCase.Core.Sync.Core;

namespace UseCase.Document.SaveDocInf;

public class SaveDocInfOutputData : IOutputData
{
    public SaveDocInfOutputData(SaveDocInfStatus status)
    {
        Status = status;
    }

    public SaveDocInfStatus Status { get; private set; }
}
