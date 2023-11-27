using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.SaveSinrekiFilterMstList;

public class SaveSinrekiFilterMstListOutputData : IOutputData
{
    public SaveSinrekiFilterMstListOutputData(SaveSinrekiFilterMstListStatus status)
    {
        Status = status;
    }

    public SaveSinrekiFilterMstListStatus Status { get; private set; }
}
