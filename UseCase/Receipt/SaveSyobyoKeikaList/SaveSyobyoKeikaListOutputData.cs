using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyobyoKeika;

public class SaveSyobyoKeikaListOutputData : IOutputData
{
    public SaveSyobyoKeikaListOutputData(SaveSyobyoKeikaListStatus status, List<SyobyoKeikaItem> syobyoKeikaInvalidList)
    {
        Status = status;
        SyobyoKeikaInvalidList = syobyoKeikaInvalidList;
    }

    public SaveSyobyoKeikaListStatus Status { get;private set; }

    public List<SyobyoKeikaItem> SyobyoKeikaInvalidList { get; private set; }
}
