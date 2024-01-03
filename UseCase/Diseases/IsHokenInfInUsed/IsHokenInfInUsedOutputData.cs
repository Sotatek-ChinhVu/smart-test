using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.IsHokenInfInUsed;

public class IsHokenInfInUsedOutputData : IOutputData
{
    public IsHokenInfInUsedOutputData(bool result, IsHokenInfInUsedStatus status)
    {
        Result = result;
        Status = status;
    }

    public bool Result { get; private set; }

    public IsHokenInfInUsedStatus Status { get; private set; }
}
