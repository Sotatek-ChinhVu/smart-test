using Domain.Models.SetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstOutputData : IOutputData
{
    public CopyPasteSetMstStatus Status { get; private set; }

    public List<SetMstModel> SetMstModels { get; private set; }

    public CopyPasteSetMstOutputData(CopyPasteSetMstStatus status)
    {
        Status = status;
        SetMstModels = new();
    }

    public CopyPasteSetMstOutputData(List<SetMstModel> setMstModels, CopyPasteSetMstStatus status)
    {
        Status = status;
        SetMstModels = setMstModels;
    }
}
