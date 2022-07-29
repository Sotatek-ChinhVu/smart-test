using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateStaticCell;

public class UpdateReceptionStaticCellOutputData : IOutputData
{
    public UpdateReceptionStaticCellOutputData(string message)
    {
        Message = message;
    }

    public UpdateReceptionStaticCellOutputData(bool success)
    {
        Success = success;
        Status = 1;
    }

    public int Status { get; private set; } = 0;
    public string Message { get; private set; } = string.Empty;
    public bool Success { get; private set; }
}
