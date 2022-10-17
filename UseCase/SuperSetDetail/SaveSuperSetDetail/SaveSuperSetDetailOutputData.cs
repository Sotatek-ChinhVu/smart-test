using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailOutputData: IOutputData
{
    public SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus status)
    {
        Code = 0;
        Status = status;
    }
    public SaveSuperSetDetailOutputData(int code, SaveSuperSetDetailStatus status)
    {
        Code = code;
        Status = status;
    }
    public int Code { get; private set; }
    public SaveSuperSetDetailStatus Status { get; private set; }
}
