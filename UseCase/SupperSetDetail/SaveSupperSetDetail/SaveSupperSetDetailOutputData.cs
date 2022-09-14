using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.SaveSupperSetDetail;

public class SaveSupperSetDetailOutputData: IOutputData
{
    public SaveSupperSetDetailOutputData(SaveSupperSetDetailStatus status)
    {
        Code = 0;
        Status = status;
    }
    public SaveSupperSetDetailOutputData(int code, SaveSupperSetDetailStatus status)
    {
        Code = code;
        Status = status;
    }
    public int Code { get; private set; }
    public SaveSupperSetDetailStatus Status { get; private set; }
}
