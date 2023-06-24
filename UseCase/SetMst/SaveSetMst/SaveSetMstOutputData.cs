using Domain.Models.SetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.SaveSetMst;

public class SaveSetMstOutputData : IOutputData
{
    public SaveSetMstOutputData(List<SetMstModel> setMstList, SaveSetMstStatus status)
    {
        SetMstList = setMstList;
        Status = status;
    }

    public List<SetMstModel> SetMstList { get; private set; }
    public SaveSetMstStatus Status { get; private set; }
}
