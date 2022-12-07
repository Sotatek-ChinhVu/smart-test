using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveListFileTodayOrder;

public class SaveListFileTodayOrderOutputData : IOutputData
{
    public SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus status)
    {
        Status = status;
        SeqNo = 0;
    }

    public SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus status, long seqNo)
    {
        Status = status;
        SeqNo = seqNo;
    }

    public SaveListFileTodayOrderStatus Status { get; private set; }

    public long SeqNo { get; private set; }
}
