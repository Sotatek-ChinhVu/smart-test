using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.DeleteYousikiInf;

public class DeleteYousikiInfOutputData : IOutputData
{
    public DeleteYousikiInfOutputData(DeleteYousikiInfStatus status)
    {
        Status = status;
    }

    public DeleteYousikiInfStatus Status { get; private set; }
}
