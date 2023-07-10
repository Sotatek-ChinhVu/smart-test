using UseCase.Core.Sync.Core;
using UseCase.SetMst.GetList;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstOutputData : IOutputData
{
    public CopyPasteSetMstStatus Status { get; private set; }

    public List<GetSetMstListOutputItem>? SetMstModels { get; private set; }

    public CopyPasteSetMstOutputData(CopyPasteSetMstStatus status)
    {
        Status = status;
        SetMstModels = new();
    }

    public CopyPasteSetMstOutputData(List<GetSetMstListOutputItem>? setMstModels, CopyPasteSetMstStatus status)
    {
        Status = status;
        SetMstModels = setMstModels;
    }
}
