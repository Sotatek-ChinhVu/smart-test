using UseCase.Core.Sync.Core;

namespace UseCase.Reception.InitDoctorCombo;

public class InitDoctorComboInputData : IInputData<InitDoctorComboOutputData>
{
    public InitDoctorComboInputData(int userId, int tantoId, long ptId, int hpId, int sinDate)
    {
        UserId = userId;
        TantoId = tantoId;
        PtId = ptId;
        HpId = hpId;
        SinDate = sinDate;
    }

    public int UserId { get; private set; }

    public int TantoId { get; private set; }

    public long PtId { get; private set; }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }
}
