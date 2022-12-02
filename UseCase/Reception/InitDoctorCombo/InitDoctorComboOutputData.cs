using UseCase.Core.Sync.Core;

namespace UseCase.Reception.InitDoctorCombo;

public class InitDoctorComboOutputData : IOutputData
{
    public InitDoctorComboOutputData(InitDoctorComboStatus status, long tantoId)
    {
        Status = status;
        TantoId = tantoId;
    }

    public InitDoctorComboStatus Status { get; private set; }

    public long TantoId { get; private set; }
}
