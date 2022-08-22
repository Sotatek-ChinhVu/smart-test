using Domain.Models.RaiinFilterMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinFilterMst.SaveList;

public class SaveRaiinFilterMstListOutputData : IOutputData
{
    public SaveRaiinFilterMstListOutputData(SaveRaiinFilterMstListStatus status)
    {
        Status = status;
    }

    public SaveRaiinFilterMstListStatus Status { get; private set; }
}
