using Domain.Models.SetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.SaveSetMst;

public class SaveSetMstOutputData : IOutputData
{
    public SaveSetMstOutputData(SetMstModel? setMstModel, SaveSetMstStatus status)
    {
        this.setMstModel = setMstModel;
        Status = status;
    }

    public SetMstModel? setMstModel { get; private set; }
    public SaveSetMstStatus Status { get; private set; }
}
