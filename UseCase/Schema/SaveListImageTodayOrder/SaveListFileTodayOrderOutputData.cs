using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveListImageTodayOrder;

public class SaveListFileTodayOrderOutputData : IOutputData
{
    public SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus status, List<long> listFileIds)
    {
        Status = status;
        ListFileIds = listFileIds;
    }

    public SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus status)
    {
        Status = status;
        ListFileIds = new();
    }

    public SaveListFileTodayOrderStatus Status { get; private set; }

    public List<long> ListFileIds { get; private set; }
}
