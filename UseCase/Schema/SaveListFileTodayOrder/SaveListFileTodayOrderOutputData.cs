using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveListFileTodayOrder;

public class SaveListFileTodayOrderOutputData : IOutputData
{
    public SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus status)
    {
        Status = status;
        ListKarteFile = new();
    }

    public SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus status, List<string> listKarteFile)
    {
        Status = status;
        ListKarteFile = listKarteFile;
    }

    public SaveListFileTodayOrderStatus Status { get; private set; }

    public List<string> ListKarteFile { get; private set; }
}
