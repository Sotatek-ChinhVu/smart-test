using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveRenkei;

public class SaveRenkeiOutputData : IOutputData
{
    public SaveRenkeiOutputData(SaveRenkeiStatus status)
    {
        Status = status;
    }

    public SaveRenkeiStatus Status { get; private set; }
}
