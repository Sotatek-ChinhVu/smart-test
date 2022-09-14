using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.SaveSupperSetDetail;

public class SaveSupperSetDetailInputData : IInputData<SaveSupperSetDetailOutputData>
{
    public SaveSupperSetDetailInputData(int setCd, int userId, SaveSupperSetDetailInputItem saveSupperSetDetailInput)
    {
        SetCd = setCd;
        UserId = userId;
        SaveSupperSetDetailInput = saveSupperSetDetailInput;
    }

    public int SetCd { get; private set; }

    public int UserId { get; private set; }

    public SaveSupperSetDetailInputItem SaveSupperSetDetailInput { get; set; }
}
