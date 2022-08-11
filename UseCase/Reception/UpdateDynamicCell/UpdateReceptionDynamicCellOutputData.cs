using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateDynamicCell;

public class UpdateReceptionDynamicCellOutputData : IOutputData
{
    public UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus status)
    {
        Status = status;
    }

    public UpdateReceptionDynamicCellStatus Status { get; private set; }
}
