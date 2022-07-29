using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateDynamicCell;

public class UpdateReceptionDynamicCellOutputData : IOutputData
{
    public UpdateReceptionDynamicCellOutputData(string message)
    {
        Message = message;
    }

    public UpdateReceptionDynamicCellOutputData(bool success)
    {
        Success = success;
        Status = 1;
    }

    public int Status { get; private set; } = 0;
    public string Message { get; private set; } = string.Empty;
    public bool Success { get; private set; }
}
