using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListReceCmt;

public class SaveReceCmtListOutputData : IOutputData
{
    public SaveReceCmtListOutputData(List<ReceCmtItem> receCmtInvalidList, SaveReceCmtListStatus status)
    {
        Status = status;
        ReceCmtInvalidList = receCmtInvalidList;
    }

    public SaveReceCmtListStatus Status { get; private set; }

    public List<ReceCmtItem> ReceCmtInvalidList { get; private set; }
}
