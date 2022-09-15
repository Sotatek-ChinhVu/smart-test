using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputData : IInputData<SaveSuperSetDetailOutputData>
{
    public SaveSuperSetDetailInputData(int setCd, int userId, SaveSuperSetDetailInputItem saveSupperSetDetailInput)
    {
        SetCd = setCd;
        UserId = userId;
        SaveSupperSetDetailInput = saveSupperSetDetailInput;
    }

    public int SetCd { get; private set; }

    public int UserId { get; private set; }

    public SaveSuperSetDetailInputItem SaveSupperSetDetailInput { get; private set; }
}
