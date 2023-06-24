using Domain.Models.SetMst;
using UseCase.Core.Sync.Core;
using UseCase.SetMst.GetList;

namespace UseCase.SetMst.SaveSetMst;

public class SaveSetMstOutputData : IOutputData
{
    public SaveSetMstOutputData(List<GetSetMstListOutputItem> setMstList, SaveSetMstStatus status)
    {
        SetMstList = setMstList;
        Status = status;
    }

    public List<GetSetMstListOutputItem> SetMstList { get; private set; }

    public SaveSetMstStatus Status { get; private set; }
}
