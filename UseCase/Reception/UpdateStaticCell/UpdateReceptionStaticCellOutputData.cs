using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateStaticCell;

public class UpdateReceptionStaticCellOutputData : IOutputData
{
    public UpdateReceptionStaticCellOutputData(UpdateReceptionStaticCellStatus status)
    {
        Status = status;
    }

    public UpdateReceptionStaticCellStatus Status { get; private set; }
}
