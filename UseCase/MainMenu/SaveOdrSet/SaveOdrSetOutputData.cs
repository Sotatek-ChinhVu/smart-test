using UseCase.Core.Sync.Core;
using UseCase.SetMst.GetList;

namespace UseCase.MainMenu.SaveOdrSet;

public class SaveOdrSetOutputData : IOutputData
{
    public SaveOdrSetOutputData(SaveOdrSetStatus status)
    {
        Status = status;
        SetMstModels = new();
    }

    public SaveOdrSetOutputData(SaveOdrSetStatus status, List<GetSetMstListOutputItem> setMstModels)
    {
        Status = status;
        SetMstModels = setMstModels;
    }

    public SaveOdrSetStatus Status { get; private set; }

    public List<GetSetMstListOutputItem> SetMstModels { get; private set; }
}
