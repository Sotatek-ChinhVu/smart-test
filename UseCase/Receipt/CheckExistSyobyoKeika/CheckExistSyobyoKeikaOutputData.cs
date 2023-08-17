using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CheckExistSyobyoKeika;

public class CheckExistSyobyoKeikaOutputData : IOutputData
{
    public CheckExistSyobyoKeikaOutputData(CheckExistSyobyoKeikaStatus status)
    {
        Status = status;
    }

    public CheckExistSyobyoKeikaStatus Status { get; private set; }
}
