using UseCase.Core.Sync.Core;

namespace UseCase.Document.CheckExistFileName;

public class CheckExistFileNameOutputData : IOutputData
{
    public CheckExistFileNameOutputData(bool result, CheckExistFileNameStatus status)
    {
        Result = result;
        Status = status;
    }

    public CheckExistFileNameOutputData(CheckExistFileNameStatus status)
    {
        Result = false;
        Status = status;
    }

    public bool Result { get; private set; }

    public CheckExistFileNameStatus Status { get; private set; }
}
