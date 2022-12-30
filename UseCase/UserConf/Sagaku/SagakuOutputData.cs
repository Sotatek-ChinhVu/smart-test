using UseCase.Core.Sync.Core;

namespace UseCase.User.Sagaku;

public class SagakuOutputData : IOutputData
{
    public SagakuOutputData(SagakuStatus status, int value)
    {
        Status = status;
        Value = value;
    }

    public SagakuStatus Status { get; private set; }
    public int Value { get; private set; }
}
