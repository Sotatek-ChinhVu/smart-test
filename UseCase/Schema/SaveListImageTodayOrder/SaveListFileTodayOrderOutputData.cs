using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveListImageTodayOrder;

public class SaveListFileTodayOrderOutputData : IOutputData
{
    public SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus status)
    {
        Status = status;
    }

    public SaveListFileTodayOrderStatus Status { get; private set; }
}
