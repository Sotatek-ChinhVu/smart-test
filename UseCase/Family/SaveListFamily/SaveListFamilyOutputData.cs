using UseCase.Core.Sync.Core;

namespace UseCase.Family.SaveListFamily;

public class SaveListFamilyOutputData : IOutputData
{
    public SaveListFamilyOutputData(SaveListFamilyStatus status)
    {
        Status = status;
    }

    public SaveListFamilyStatus Status { get; private set; }
}
